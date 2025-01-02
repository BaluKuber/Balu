/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : Loan Admin 
/// Screen Name               : Bill Generation
/// Created By                : Prabhu.K
/// Created Date              : 23-Dec-2010
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>


#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using S3GBusEntity.Collection;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ServiceProcess;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Collections;
#endregion

public partial class LoanAdmin_S3GClnBilling : ApplyThemeForProject
{
    #region Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> objProcedureParameter = null;
    Dictionary<string, string> dictParam = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    public static LoanAdmin_S3GClnBilling obj_Page = null;
    public string strDateFormat = string.Empty;
    static string strPageName = "Bill Generation";
    int intBillingId;
    private string strDocPath = "";
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=CLNBILL";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdBilling.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=CLNBILL';";
    StringBuilder strExceptionEmail = new StringBuilder();
    StringBuilder strFileSizeException = new StringBuilder();
    StringBuilder strPDFCountException = new StringBuilder();
    DataTable dtaccounts;
    DataSet dsaccounts;
    DataTable dttranche;
    DataTable dttarget;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    #endregion

    #region Events

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sadasd = txtStartDate.Text;
            //txtScheduleDate.Attributes.Add("readonly", "true");
            obj_Page = this;
            FunPrilLoadPage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Billing Details";
            cvBilling.IsValid = false;
        }
    }






    #endregion

    #region Button Events

    #region Common Control

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Unable to Clear the data";
            cvBilling.IsValid = false;
        }
    }
    protected void btnBillPDF_Click(object sender, EventArgs e)
    {
        try
        {
            FunGetBillPDF();

            //if (ViewState["DocPath"] != null)
            //{
            //    string strMessage = "Generated Bills are available in Server (" + ViewState["DocPath"].ToString() + " Directory)";
            //    Utility.FunShowAlertMsg(this, strMessage);
            //    return;
            //}
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Unable to Clear the data";
            cvBilling.IsValid = false;
        }
    }


    // Code added by Santhosh.S on 10.07.2013 to export Gridview data to Excel Sheet
    protected void btnXLPorting_Click(object sender, EventArgs e)
    {
        try
        {
            FunProExport(grvAccounts, "Accounts");
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            //Utility.FunShowAlertMsg(this, "Service is not running. Start the service and schedule");
            //return;


            bool chkServiceStatus = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ServiceStatus"));

            if (chkServiceStatus)
            {
                ServiceController sc = new ServiceController("SISLS3GWSBilling");
                if ((sc.Status.Equals(ServiceControllerStatus.Stopped)) ||
                    (sc.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    // Start the service if the current status is stopped.    sc.Start();    
                    Utility.FunShowAlertMsg(this, "Service is not running. Contact S3G Admin to start the service");
                    return;
                }
            }
            if (rbtnSchedule.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(txtScheduleDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
                    return;
                }
                if (string.IsNullOrEmpty(txtScheduleTime.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
                    return;
                }


                if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
                    return;
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
                        if (span.Hours == 0 && span.Minutes < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
                            return;
                        }
                    }
                }
            }
            FunPriSaveRecord();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }

    }

    /* Added by Suresh - Start  */
    protected void btnRentalCancel_Click(object sender, EventArgs e)
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {

            if (grvRental.Rows.Count > 0)
            {
                CheckBox chkSelectAll = grvRental.HeaderRow.FindControl("chkSelectAllRS") as CheckBox;
                int intSelectedRentalCount = 0;
                if (!chkSelectAll.Checked)
                {
                    foreach (GridViewRow grv in grvRental.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                intSelectedRentalCount += 1;
                            }
                        }
                    }
                    if (intSelectedRentalCount == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
                        return;
                    }
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
                return;
            }

            BillingEntity objBillingEntity = new BillingEntity();
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.intUserId = intUserID;
            objBillingEntity.strXmlAccDetails = FunPriFormXml(grvRental, true, "chkSelectAllRS", "chkSelectRS");
            objBillingEntity.intBillingId = intBillingId;
            objBillingEntity.intInvoiceType = 1;
            
            string strInvoiceNo = "1";
            intErrorCode = 0;

            intErrorCode = objServiceClient.FunPubCancelInvoice(out strInvoiceNo, objBillingEntity);
            //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            if (intErrorCode == 0)
            {

                foreach (GridViewRow grv in grvRental.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            int i = 0;

                            Label lblRS_Number = grv.FindControl("lblRS_Number") as Label;

                            string strnewfile = ViewState["strnewFile"].ToString();
                            string strtranche = lblRS_Number.Text;
                            string strTempfile = "";

                            strnewfile += "/" + "Rental";
                            strTempfile = strnewfile = strnewfile.Replace("\\", "/");
                            strtranche = strtranche + ".pdf";

                            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche);

                            if (str.Length > 0)
                            {
                                i = 0;
                                foreach (string strRS in str)
                                {
                                    FileInfo fl = new FileInfo(strRS);

                                    if (fl.Exists == true)
                                    {
                                        if (!fl.Name.StartsWith("C_"))
                                        {
                                            strtranche = fl.Name;
                                            strnewfile = strRS.Replace("\\", "/");
                                        }
                                        i = i + 1;
                                    }
                                }
                                strTempfile += "/" + "C_" + i.ToString() + strtranche + "";
                                System.IO.File.Copy(strnewfile, strTempfile, true);
                                File.Delete(strnewfile);
                            }
                        }
                    }
                }
                Utility.FunShowAlertMsg(this, "Invoices cancelled successfully");
                FetchInvoiceAccountData();
            }
            else if (intErrorCode == 1)
            {
                Utility.FunShowAlertMsg(this, "Rental Invoices Already Cancelled for " + strInvoiceNo + "");
                return;
            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
        finally
        {
            objServiceClient.Close();
        }
    }
    /* Added by Suresh - End */

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClosePage();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            GetPDFFiles("Rental");
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }

    /*added by vinodha m - event to print AMF details*/
    protected void btnPrintAMF_Click(object sender, EventArgs e)
    {
        try
        {
            GetPDFFiles("AMF");
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }
    protected void bntDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string fileNameZip = "";
            string fileName = Server.MapPath(".") + "\\PDF Files";
            int nCnt = 0;
            dtaccounts = (DataTable)ViewState["dtaccounts"];
            if (dtaccounts != null)
            {
                DataTable dtprint;
                string panum;
                dtprint = new DataTable();
                dtprint.Columns.Add("tranche_name");
                if (dtaccounts.Rows.Count > 0)
                {
                    foreach (GridViewRow gvrow in grvRental.Rows)
                    {
                        CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                        Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                        if (chkSelectAccount.Checked)
                        {
                            nCnt += 1;
                            DataRow dr = dtprint.NewRow();
                            panum = lblRS_Number.Text.ToString();
                            dr["tranche_name"] = panum;
                            dtprint.Rows.Add(dr);
                            dtprint.AcceptChanges();
                        }
                    }
                    if (nCnt == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atlease one account to print");
                        return;
                    }
                    ViewState["dtprint"] = dtprint;
                }
                DataTable dtN = (DataTable)ViewState["dtprint"];
                string strnewfile = ViewState["strnewFile"].ToString();
                strnewfile += "\\" + "Rental";

                List<String> list = new List<String>();

                foreach (DataRow drow in dtN.Rows)
                {
                    if (ddlLocation.SelectedValue != "0")
                    {
                        string[] str = Directory.GetFiles(strnewfile, "*_" + drow[0].ToString() + "*");
                        for (int i = 0; i <= str.Length - 1; i++)
                        {
                            list.Add(str[i].ToString());
                        }
                    }
                    else
                    {
                        string[] str = Directory.GetFiles(strnewfile, drow[0].ToString() + "*");
                        for (int i = 0; i <= str.Length - 1; i++)
                        {
                            list.Add(str[i].ToString());
                        }
                    }
                }
                string[] GetAllFiles = list.ToArray();

                if (!Directory.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                }

                fileNameZip = fileName + "\\" + "Temp" + ".zip";

                FileInfo fi = new FileInfo(fileNameZip);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                //using (StreamWriter sw = fi.CreateText())
                //{
                //    sw.WriteLine(strHTMLText.ToString());
                //}

                //FunPubCreateZip(GetAllFiles, fileNameZip);

                //using (ZipFile zip = new ZipFile())
                //{
                //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                //    zip.AddDirectoryByName("Files");
                //    foreach (string strFileName in list)
                //    {
                //        zip.AddFile(strFileName, "Files");
                //    }
                //    Response.Clear();
                //    Response.BufferOutput = false;
                //    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                //    Response.ContentType = "application/zip";
                //    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                //    zip.Save(Response.OutputStream);
                //    Response.End();
                //}
            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }

    //private void FunPubCreateZip(string[] strDocPath, string fileNameZip)
    //{
    //    if (!File.Exists(strDocPath.ToString()))
    //    {
    //        ZipFile.CreateFromDirectory(strDocPath[0].ToString(), fileNameZip);
    //    }
    //}


    protected void btnAMFCancel_Click(object sender, EventArgs e)
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            if (grvRental.Rows.Count > 0)
            {
                CheckBox chkSelectAll = grvRental.HeaderRow.FindControl("chkSelectAllRS") as CheckBox;
                int intSelectedRentalCount = 0;
                if (!chkSelectAll.Checked)
                {
                    foreach (GridViewRow grv in grvRental.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;
                            if (chkSelect.Checked)
                            {
                                intSelectedRentalCount += 1;
                            }
                        }
                    }
                    if (intSelectedRentalCount == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
                        return;
                    }
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select atleast one record for Invoice Cancellation");
                return;
            }

            BillingEntity objBillingEntity = new BillingEntity();
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.intUserId = intUserID;
            objBillingEntity.strXmlAccDetails = FunPriFormXml(grvRental, true, "chkSelectAllRS", "chkSelectRS");
            objBillingEntity.intBillingId = intBillingId;
            objBillingEntity.intInvoiceType = 2;
            
            string strInvoiceNo = "";
            intErrorCode = objServiceClient.FunPubCancelInvoice(out strInvoiceNo, objBillingEntity);
            //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            if (intErrorCode == 0)
            {
                foreach (GridViewRow grv in grvRental.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow)
                    {
                        // CheckBox chkSelect = grv.FindControl("chkSelectAllRS") as CheckBox;
                        CheckBox chkSelect = grv.FindControl("chkSelectRS") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            int i = 0;

                            Label lblRS_Number = grv.FindControl("lblRS_Number") as Label;

                            string strnewfile = ViewState["strnewFile"].ToString();
                            string strtranche = lblRS_Number.Text;
                            string strTempfile = "";

                            strnewfile += "/" + "AMF";
                            strTempfile = strnewfile = strnewfile.Replace("\\", "/");
                            strtranche = strtranche + ".pdf";

                            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche);

                            if (str.Length > 0)
                            {
                                i = 0;
                                foreach (string strRS in str)
                                {
                                    FileInfo fl = new FileInfo(strRS);

                                    if (fl.Exists == true)
                                    {
                                        if (!fl.Name.StartsWith("C_"))
                                        {
                                            strtranche = fl.Name;
                                            strnewfile = strRS.Replace("\\", "/");
                                        }
                                        i = i + 1;
                                    }
                                }
                                strTempfile += "/" + "C_" + i.ToString() + strtranche + "";
                                System.IO.File.Copy(strnewfile, strTempfile, true);
                                File.Delete(strnewfile);
                            }
                        }
                    }
                }

                Utility.FunShowAlertMsg(this, "Invoices cancelled successfully");
                FetchInvoiceAccountData();
            }
            else if (intErrorCode == 1)
            {

                Utility.FunShowAlertMsg(this, "AMF Invoices Already Cancelled for " + strInvoiceNo + "");
                return;

            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
        finally
        {
            objServiceClient.Close();
        }
    }

    protected void btnRerun_Click(object sender, EventArgs e)
    {
        try
        {
            dtaccounts = (DataTable)ViewState["dtaccounts"];
            DataTable dtrerun;
            string panum;
            dtrerun = new DataTable();
            dtrerun.Columns.Add("Rs_number");
            if (dtaccounts.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grvRental.Rows)
                {
                    CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                    Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");

                    if (chkSelectAccount.Checked)
                    {
                        DataRow dr = dtrerun.NewRow();
                        panum = lblRS_Number.Text.ToString();

                        dr["Rs_number"] = panum;
                        dtrerun.Rows.Add(dr);
                        dtrerun.AcceptChanges();
                    }
                }
                ViewState["dtrerun"] = dtrerun;
            }
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@BillingId", intBillingId.ToString());
            objProcedureParameter.Add("@accxml", dtrerun.FunPubFormXml());

            string strXmlCustomerDetails = FunPriFormXml(gvCustomer, true, "chkSelectAllBranch", "chkSelectBranch");
            objProcedureParameter.Add("@XmlCustomerDetails", strXmlCustomerDetails);

            objProcedureParameter.Add("@User_Id", intUserID.ToString());
            DataTable dterror = Utility.GetDefaultData("s3g_loanad_RerunBilling", objProcedureParameter);
            if (dterror.Rows[0]["ErrorCode"].ToString() == "14")
            {
                Utility.FunShowAlertMsg(this, "OPC Bank Account Should be Selected In GPS");
                return;
            }
            if (dterror.Rows[0]["errorcode"].ToString() == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Pdf generated in Document path");
                return;
            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }

    protected void grvRental_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    CheckBox chkAll = (CheckBox)e.Row.FindControl("chkSelectAllRS");
        //    chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvRental.ClientID + "',this,'chkSelectRS');");
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectRS");
            chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvRental.ClientID + "','chkSelectAllRS','chkSelectRS');");
            
        }
    }

    protected void grvRental_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow Objrow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

        Label lblDigi_Flag = (Label)Objrow.Cells[0].FindControl("lblDigi_Flag");
        int Digi_Flag = 0;

        if (lblDigi_Flag.Text == "1")
            Digi_Flag = 1;

        switch (e.CommandName.ToLower())
        {

            case "print":

                if (ddlRerun.SelectedValue == "3")
                {
                    GetPDFCancelledTrnacheFiles(e.CommandArgument.ToString(), Digi_Flag);
                }
                else
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int rowIndex = gvr.RowIndex;
                    //int index = int.Parse(e.CommandArgument.ToString());
                    Label lblInvoiceno = (Label)grvRental.Rows[rowIndex].FindControl("lblInvoiceno");
                    Label lblInvoiceNoamf = (Label)grvRental.Rows[rowIndex].FindControl("lblInvoiceNoamf");
                    Label lblStatewiseBilling = (Label)grvRental.Rows[rowIndex].FindControl("lblStatewiseBilling");
                    if (intBillingId >= 174 && lblStatewiseBilling.Text == "1")
                        GetPDFTrnacheFiles(lblInvoiceno.Text.Replace("/", "-").ToString(), Digi_Flag);
                    else
                        GetPDFTrnacheFiles(e.CommandArgument.ToString(), Digi_Flag);


                }
                break;
            case "print amf":
                if (ddlRerun.SelectedValue == "3")
                {
                    GetPDFAMFCancelledTrnacheFiles(e.CommandArgument.ToString(), Digi_Flag);

                }
                else
                {
                    GridViewRow gvr = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                    int rowIndex = gvr.RowIndex;
                    //int index = int.Parse(e.CommandArgument.ToString());
                    Label lblInvoiceno = (Label)grvRental.Rows[rowIndex].FindControl("lblInvoiceno");
                    Label lblInvoiceNoamf = (Label)grvRental.Rows[rowIndex].FindControl("lblInvoiceNoamf");
                    Label lblStatewiseBilling = (Label)grvRental.Rows[rowIndex].FindControl("lblStatewiseBilling");
                    if (intBillingId >= 174 && lblStatewiseBilling.Text == "1")
                        GetPDFAMFTrnacheFiles(lblInvoiceNoamf.Text.Replace("/", "-").ToString(), Digi_Flag);
                    else
                        GetPDFAMFTrnacheFiles(e.CommandArgument.ToString(), Digi_Flag);

                }


                break;

        }

    }
    //protected void FunOpenPDF(string strReceiptProcessID)
    //{
    //   // string strHTML = FunGetHTMLForRECP(strReceiptProcessID);

    //    // System.Guid  objGuid = new System.Guid();
    //    string objGuid = System.Guid.NewGuid().ToString();

    //    //string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + objGuid.ToString() + "Receipt.pdf");
    //    string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + strReceiptProcessID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf");
    //    string strnewFile1 = strReceiptProcessID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";
    //    //string strFileName = "/Collection/PDF Files/Receipt.pdf";
    //    try
    //    {
    //        if (File.Exists(strnewFile) == true)
    //        {
    //            File.Delete(strnewFile);
    //        }

    //        Document doc = new Document();

    //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));

    //        doc.AddCreator("Sundaram Infotech Solutions");
    //        doc.AddTitle("Receipt");
    //        doc.Open();
    //        List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(strHTML), null);
    //        for (int k = 0; k < htmlarraylist.Count; k++)
    //        {
    //            doc.Add((IElement)htmlarraylist[k]);
    //        }
    //        doc.AddAuthor("S3G Team");
    //        doc.Close();

    //        string strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewFile1 + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt, true);

    //        //System.Diagnostics.Process.Start(strnewFile);
    //        //Process Prc = new Process();
    //        //Prc.StartInfo.Verb = "open";
    //        //Prc.StartInfo.CreateNoWindow = false;
    //        //Prc.StartInfo.FileName = strnewFile;
    //        //Prc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
    //        //Prc.Start();
    //        //Prc.CloseMainWindow();
    //        //Prc.Close();




    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Receipt");
    //        System.Diagnostics.Process.Start(strnewFile);
    //    }


    //    //Prc.Kill();

    //    //string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt, true);
    //}


    //public void CombineMultiplePDFs(string[] fileNames, string outFile)
    //{
    //    // step 1: creation of a document-object
    //    Document document = new Document();

    //    // step 2: we create a writer that listens to the document
    //    PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
    //    if (writer == null)
    //    {
    //        return;
    //    }

    //    // step 3: we open the document
    //    document.Open();

    //    foreach (string fileName in fileNames)
    //    {
    //        // we create a reader for a certain document
    //        PdfReader reader = new PdfReader(fileName);
    //        reader.ConsolidateNamedDestinations();

    //        // step 4: we add content
    //        for (int i = 1; i <= reader.NumberOfPages; i++)
    //        {
    //            PdfImportedPage page = writer.GetImportedPage(reader, i);
    //            writer.AddPage(page);
    //        }

    //        PRAcroForm form = reader.AcroForm;
    //        if (form != null)
    //        {
    //            writer.CopyAcroForm(reader);
    //        }

    //        reader.Close();
    //    }

    //    // step 5: we close the document and writer
    //    writer.Close();
    //    document.Close();
    //    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(outFile, false, 0);

    //    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + outFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

    //}


    private void GetPDFTrnacheFiles(string strtranche, int nDigi_Flag)
    {
        try
        {

            string strnewfile = ViewState["strnewFile"].ToString();
            strnewfile += "/" + "Rental";
            strnewfile = strnewfile.Replace("\\", "/");

            // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);

            //strnewfile = Server.MapPath(".") + strnewfile;
            //FileInfo[] files = Sourcedir.GetFiles();
            // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");

            string outfile = string.Empty;

            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");

            if ((ddlLocation.SelectedValue != "0")&&(ddlLocation.SelectedText!=""))
            {
                if (intBillingId <= 174)
                    str = Directory.GetFiles(strnewfile, "*" + strtranche + ".pdf");
                else
                    //opc205 start
                    //Invoice no concatinated , so * is required in suffix
                    //str = Directory.GetFiles(strnewfile, "*" + strtranche + ".pdf");
                    str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");
                    //opc205 end
                    //if (intBillingId == 80)
                    //{
                    //    Utility.FunShowAlertMsg(this.Page, strtranche);
                    //    return;
                    //}
            }
            else
            {
                str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");
            }

            if (Convert.ToInt32(str.Length.ToString()) > 0)
            {
                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";

                List<string> strnewPdf = new List<string>();

                foreach (string strRS in str)
                {
                    FileInfo f1 = new FileInfo(strRS);
                    if (!f1.Name.StartsWith("C_"))
                    {
                        strnewPdf.Add(strRS);
                    }
                }
                if (strnewPdf.Count > 0)
                {
                    CombineMultiplePDF(strnewPdf.ToArray(), strnewfile, nDigi_Flag, "RIGHT");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Please upload IRN details to generate the invoice");
                    return;
                }
            }

            else
            {
                strtranche = "/_" + strtranche + ".pdf";
                strnewfile = strnewfile + strtranche;
                //string[] str1 = Directory.GetFiles(strnewfile, strtranche + "*");
                //if (Convert.ToInt32(str1.Length.ToString()) > 0)
                //{
                //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(str1, false, 0);
                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                FileInfo fl = new FileInfo(strnewfile);
                if (fl.Exists == true)
                {
                    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewfile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                }


                //}
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Please upload IRN details to generate the invoice.");
                    return;
                }
            }






        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }

    }

    private void GetPDFCancelledTrnacheFiles(string strtranche, int nDigi_Flag)
    {
        try
        {

            string strnewfile = ViewState["strnewFile"].ToString();
            strnewfile += "/" + "Rental";
            strnewfile = strnewfile.Replace("\\", "/");

            // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);

            //strnewfile = Server.MapPath(".") + strnewfile;
            //FileInfo[] files = Sourcedir.GetFiles();
            // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");
            string outfile = string.Empty;




            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");

            //string[] str = Directory.GetFiles(strnewfile, "*" + strtranche+".pdf");

            //string[] str = Directory.GetFiles(strnewfile, cancelledTranch + "*");

            if (Convert.ToInt32(str.Length.ToString()) > 0)
            {

                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                //CombineMultiplePDFs(str, strnewfile + "/File.pdf");
                //string[] strCancelled;
                List<string> strCancelled = new List<string>();

                foreach (string strRS in str)
                {
                    FileInfo f1 = new FileInfo(strRS);
                    if (f1.Name.StartsWith("C_"))
                    {
                        strCancelled.Add(strRS);

                    }

                }

                if (strCancelled.Count > 0)
                {
                    CombineMultiplePDF(strCancelled.ToArray(), strnewfile, nDigi_Flag, "RIGHT");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }



            }

            else
            {
                strtranche = "/_" + strtranche + ".pdf";
                strnewfile = strnewfile + strtranche;
                //string[] str1 = Directory.GetFiles(strnewfile, strtranche + "*");
                //if (Convert.ToInt32(str1.Length.ToString()) > 0)
                //{
                //FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(str1, false, 0);
                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";
                FileInfo fl = new FileInfo(strnewfile);
                if (fl.Exists == true)
                {
                    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewfile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                }


                //}
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
            }






        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }

    }


    private void GetPDFAMFTrnacheFiles(string strtranche, int nDigi_Flag)
    {
        try
        {

            string strnewfile = ViewState["strnewFile"].ToString();
            strnewfile += "/" + "AMF";
            // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);
            strnewfile = strnewfile.Replace("\\", "/");

            //FileInfo[] files = Sourcedir.GetFiles();
            // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");
            string outfile = string.Empty;
            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");
            if (Convert.ToInt32(str.Length.ToString()) > 0)
            {

                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";

                List<string> strnewAmfPdf = new List<string>();
                foreach (string strRS in str)
                {
                    FileInfo f1 = new FileInfo(strRS);
                    if (!f1.Name.StartsWith("C_"))
                    {
                        strnewAmfPdf.Add(strRS);

                    }
                }
                if (strnewAmfPdf.Count > 0)
                {
                    CombineMultiplePDF(strnewAmfPdf.ToArray(), strnewfile, nDigi_Flag, "RIGHT");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
            }

            else
            {
                strtranche = "/_" + strtranche + ".pdf";
                strnewfile = strnewfile + strtranche;
                FileInfo fl = new FileInfo(strnewfile);
                if (fl.Exists == true)
                {
                    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewfile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                }


                //}
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
            }






        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }

    }

    private void GetPDFAMFCancelledTrnacheFiles(string strtranche, int nDigi_Flag)
    {
        try
        {

            string strnewfile = ViewState["strnewFile"].ToString();
            strnewfile += "/" + "AMF";
            // DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);
            strnewfile = strnewfile.Replace("\\", "/");

            //FileInfo[] files = Sourcedir.GetFiles();
            // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");
            string outfile = string.Empty;

            string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + "*");
            //string[] str = Directory.GetFiles(strnewfile, "*" + strtranche + ".pdf");



            if (Convert.ToInt32(str.Length.ToString()) > 0)
            {
                List<string> strCancelled = new List<string>();
                //strnewfile = "D:/OPC_S3G/Upload/Bill Generation/Bill Generation/BillNo-119/Rental";

                foreach (string strRS in str)
                {
                    FileInfo f1 = new FileInfo(strRS);
                    if (f1.Name.StartsWith("C_"))
                    {
                        strCancelled.Add(strRS);

                    }

                }
                if (strCancelled.Count > 0)
                {
                    CombineMultiplePDF(str, strnewfile, nDigi_Flag, "RIGHT");
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
            }

            else
            {
                strtranche = "/_" + strtranche + ".pdf";
                strnewfile = strnewfile + strtranche;
                FileInfo fl = new FileInfo(strnewfile);
                if (fl.Exists == true)
                {
                    string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strnewfile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
                }


                //}
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                    return;
                }
            }






        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }

    }

    protected void btnPrintall_Click(object sender, EventArgs e)
    {
        try
        {
            GetPDFFilesall();
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = ex.Message;
            cvBilling.IsValid = false;
        }
    }
    //private void GetPDFFiles1()
    //{
    //    try
    //    {
    //        string name;
    //        dtaccounts = (DataTable)ViewState["dtaccounts"];
    //        DataTable dtprint;
    //        string panum;
    //        dtprint = new DataTable();
    //        dtprint.Columns.Add("tranche_name");
    //        if (dtaccounts.Rows.Count > 0)
    //        {                
    //            foreach (GridViewRow gvrow in grvRental.Rows)
    //            {
    //                CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectAccount");
    //                Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");

    //                if (chkSelectAccount.Checked)
    //                {
    //                    DataRow dr = dtprint.NewRow();
    //                    panum = lblRS_Number.Text.ToString();

    //                    dr["tranche_name"] = panum;
    //                    dtprint.Rows.Add(dr);
    //                    dtprint.AcceptChanges();



    //                }
    //            }
    //            ViewState["dtprint"] = dtprint;
    //        }

    //        string strnewfile = ViewState["strnewFile"].ToString();
    //        strnewfile += "\\//" + "Rental";
    //        DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);


    //        FileInfo[] files = Sourcedir.GetFiles();
    //        // FileInfo[] files = Directory.GetFiles(strnewfile, "GANESH0004");



    //        PrinterSettings settings = new PrinterSettings();

    //        foreach (FileInfo pdf in files)
    //        {
    //            if (pdf.Length > 0)
    //            {
    //                int indx = Convert.ToInt32(pdf.Name.IndexOf("_"));
    //                if (indx > 0)
    //                {
    //                    name = pdf.Name.Substring(0, indx);

    //                }
    //                else
    //                {
    //                    name = pdf.Name;
    //                }
    //                DataRow[] dtrow = dtprint.Select("tranche_name like '%" + name + "%'");
    //                if (dtrow.Length > 0)
    //                {
    //                    string strsourceFile = strnewfile + "\\" + pdf.Name;

    //                    Print(strsourceFile, settings.PrinterName);
    //                }


    //            }
    //        }





    //    }
    //    catch (Exception ex)
    //    {
    //        //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
    //        throw ex;
    //    }

    //}


    /*added by vinodha.m to combine multiple pdf into one*/
    private void GetPDFFiles()
    {

        int nCnt = 0;
        dtaccounts = (DataTable)ViewState["dtaccounts"];
        if (dtaccounts != null)
        {
            DataTable dtprint;
            string panum;
            dtprint = new DataTable();
            dtprint.Columns.Add("tranche_name");
            if (dtaccounts.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in grvRental.Rows)
                {
                    CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                    Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                    if (chkSelectAccount.Checked)
                    {
                        nCnt += 1;
                        DataRow dr = dtprint.NewRow();
                        panum = lblRS_Number.Text.ToString();
                        dr["tranche_name"] = panum;
                        dtprint.Rows.Add(dr);
                        dtprint.AcceptChanges();
                    }
                }
                if (nCnt == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atlease one account to print");
                    return;
                }
                ViewState["dtprint"] = dtprint;
            }
        }
    }

    private void GetPDFFiles(string Type)
    {
        try
        {
            string name;
            int nCnt = 0;
            int nDigi_Flag = 0;
            dtaccounts = (DataTable)ViewState["dtaccounts"];
            if (dtaccounts != null)
            {
                DataTable dtprint;
                string panum;
                dtprint = new DataTable();
                dtprint.Columns.Add("tranche_name");
                if (dtaccounts.Rows.Count > 0)
                {
                    foreach (GridViewRow gvrow in grvRental.Rows)
                    {
                        CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                        Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                        Label lblInvoiceno = (Label)gvrow.Cells[0].FindControl("lblInvoiceno");
                        Label lblInvoiceNoamf = (Label)gvrow.Cells[0].FindControl("lblInvoiceNoamf");
                        Label lblStatewiseBilling = (Label)gvrow.Cells[0].FindControl("lblStatewiseBilling");
                        Label lblDigi_Flag = (Label)gvrow.Cells[0].FindControl("lblDigi_Flag");
                        if (chkSelectAccount.Checked)
                        {
                            
                            nCnt += 1;
                            DataRow dr = dtprint.NewRow();
                            if (intBillingId >= 174 && lblStatewiseBilling.Text == "1" && Type == "Rental")
                                panum = lblInvoiceno.Text.Replace("/", "-").ToString();
                            else if (intBillingId >= 174 && lblStatewiseBilling.Text == "1" && Type == "AMF")
                                panum = lblInvoiceNoamf.Text.Replace("/", "-").ToString();
                            else
                                panum = lblRS_Number.Text.ToString();
                            if (lblDigi_Flag.Text == "1")
                                nDigi_Flag = 1;
                            if (dtprint.Rows.Count > 0)
                            {
                                DataRow[] drrow = dtprint.Select("tranche_name = '" + panum + "'");
                                if (drrow.Length.ToString() == "0")
                                {
                                    dr["tranche_name"] = panum;
                                    dtprint.Rows.Add(dr);
                                    dtprint.AcceptChanges();
                                }
                            }
                            else
                            {
                                dr["tranche_name"] = panum;
                                dtprint.Rows.Add(dr);
                                dtprint.AcceptChanges();
                            }
                        }
                    }
                    if (nCnt == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atlease one account to print");
                        return;
                    }
                    ViewState["dtprint"] = dtprint;
                }
                DataTable dtN = (DataTable)ViewState["dtprint"];
                string strnewfile = ViewState["strnewFile"].ToString();
                strnewfile += "\\" + Type;

                List<String> list = new List<String>();

                foreach (DataRow drow in dtN.Rows)
                {
                    //if (ddlLocation.SelectedValue != "0")
                    //{

                    //string[] str = new string[50];
                    //string[] str = Directory.GetFiles(strnewfile, lblInvoiceno.Text.Replace("/", "-").ToString());

                    //if (lblStatewiseBilling.Text == "1" && Type == "Rental")
                    //    str = Directory.GetFiles(strnewfile, lblInvoiceno.Text.Replace("/", "-").ToString());
                    //else if (lblStatewiseBilling.Text == "1" && Type == "AMF")
                    //    str = Directory.GetFiles(strnewfile, lblInvoiceNoamf.Text.Replace("/", "-").ToString());
                    //else
                    //       string[] str = Directory.GetFiles(strnewfile, drow[0].ToString());

                    //    for (int i = 0; i <= str.Length - 1; i++)
                    //    {
                    //        list.Add(str[i].ToString());
                    //    }
                    //}
                    //else
                    //{

                    string[] str = Directory.GetFiles(strnewfile, drow[0].ToString() + "*");

                    if ((ddlLocation.SelectedValue != "0") && (ddlLocation.SelectedText != ""))
                    {
                        if (intBillingId <= 174)
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + ".pdf");
                        else
                            //str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + ".pdf");
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + "*");
                    }
                    else
                    {

                        if (intBillingId <= 174)
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + "*");
                        else
                            str = Directory.GetFiles(strnewfile, drow[0].ToString() + "*");
                    }

                    for (int i = 0; i <= str.Length - 1; i++)
                    {
                        list.Add(str[i].ToString());
                    }
                    
                    //}
                    string[] GetAllFiles = list.ToArray();
                    if (Convert.ToInt32(GetAllFiles.Length.ToString()) > 0)
                    {
                        CombineMultiplePDF(GetAllFiles, strnewfile, nDigi_Flag, "RIGHT");
                    }
                    //else
                    //{
                    //    Utility.FunShowAlertMsg(this, "Please upload IRN details to generate the invoice.");
                    //    return;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }
    }

    public void CombineMultiplePDF(string[] fileNames, string outFilePath, int Digi_Flag, String DigiPosition)
    {

        if (Digi_Flag == 1)
        {
            string InFile = outFilePath + "/Combine.pdf";

            outFilePath = outFilePath + "/" + intUserID.ToString();

            if (!System.IO.Directory.Exists(outFilePath + "/Signed"))
            {
                System.IO.Directory.CreateDirectory(outFilePath + "/Signed");
            }

            string filePath = outFilePath + "/Signed.zip";

            //before creation of compressed folder,deleting it if exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();

            foreach (string fileName in fileNames)
            {

                string SignedFile = outFilePath + "/Signed/" + Path.GetFileName(fileName);
                
                ObjPDFSign.DigiPDFSign(fileName, SignedFile, "RIGHT");
            }

            if (!File.Exists(filePath))
            {
                //creating a zip file from one folder to another folder
                ZipFile.CreateFromDirectory(outFilePath + "/Signed", filePath);

                //Delete The excel file which is created

                string[] str = Directory.GetFiles(outFilePath + "/Signed");
                foreach (string fileName in str)
                {
                    File.Delete(fileName);
                }

                if (Directory.Exists(outFilePath + "/Signed"))
                {
                    Directory.Delete(outFilePath + "/Signed");
                }
            }

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(filePath, false, 0);
            string strScipt1 = "";

            strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + filePath.Replace("\\", "/") + 
                "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ZIP", strScipt1, true);

        }
        else
        {
            // step 1: creation of a document-object
            Document document = new Document();

            string InFile = outFilePath + "/Combine.pdf";
            string SignedFile = outFilePath + "/Signed.pdf";

            // step 2: we create a writer that listens to the document
            PdfCopy writer = new PdfCopy(document, new FileStream(InFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }

            // step 3: we open the document
            document.Open();

            foreach (string fileName in fileNames)
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();

                // step 4: we add content
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

            // step 5: we close the document and writer
            writer.Close();
            document.Close();

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
            if (Digi_Flag == 1)
                ObjPDFSign.DigiPDFSign(InFile, SignedFile, "RIGHT");
            else
                SignedFile = InFile;

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
            string strScipt1 = "";
            if (SignedFile.Contains("/File.pdf"))
            {
                strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + SignedFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            else
            {
                strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + SignedFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
        }
    }






    private void GetPDFFilesall()
    {
        try
        {

            string strnewfile = ViewState["strnewFile"].ToString();
            DirectoryInfo Sourcedir = new DirectoryInfo(strnewfile);


            FileInfo[] files = Sourcedir.GetFiles();



            PrinterSettings settings = new PrinterSettings();

            foreach (FileInfo pdf in files)
            {
                if (pdf.Length > 0)
                {

                    string strsourceFile = strnewfile + "\\" + pdf.Name;

                    Print(strsourceFile, settings.PrinterName);

                }
            }





        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }

    }

    public static bool Print(string file, string printer)
    {
        try
        {
            Process.Start(Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion" +
                    @"\App Paths\AcroRd32.exe").GetValue("").ToString(),
                    string.Format("/h /t \"{0}\" \"{1}\"", file, printer));
            return true;
        }
        catch
        { return false; }
    }

    #endregion

    #region Page Specific

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlFrequency.SelectedItem.Text.ToUpper() == "MONTHLY")
            //{
            //    if (Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtStartDate.Text).Month)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date Month should be equal to Month/Year");
            //        gvBranchWise.DataSource = null;
            //        gvBranchWise.DataBind();
            //        gvControlDataSheet.DataSource = null;
            //        gvControlDataSheet.DataBind();
            //        return;
            //    }
            //    if (Utility.StringToDate(txtMonthYear.Text).Month != Utility.StringToDate(txtEndDate.Text).Month)
            //    {
            //        Utility.FunShowAlertMsg(this, "End Date Month should be equal to Month/Year");
            //        gvBranchWise.DataSource = null;
            //        gvBranchWise.DataBind();
            //        gvControlDataSheet.DataSource = null;
            //        gvControlDataSheet.DataBind();
            //        return;
            //    }
            //}
            //    if (Utility.CompareDates(txtStartDate.Text, txtEndDate.Text) != 1)
            //    {
            //        Utility.FunShowAlertMsg(this, "End Date should be greater than Start Date");
            //        gvBranchWise.DataSource = null;
            //        gvBranchWise.DataBind();
            //        gvControlDataSheet.DataSource = null;
            //        gvControlDataSheet.DataBind();
            //        return;
            //    }

            //if (ddlFrequency.SelectedItem.Text.ToUpper() == "MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 27)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than or equal to 28 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "WEEKLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 7)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 7 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "FORTNIGHTLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 15)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 15 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "BI MONTHLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays != 60)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be 60 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "QUARTERLY")
            //{

            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 90)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 90 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "HALF YEARLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 181)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 181 days");
            //        return;
            //    }
            //}
            //else if (ddlFrequency.SelectedItem.Text.ToUpper() == "ANNUALLY")
            //{
            //    if ((Utility.StringToDate(txtEndDate.Text) - Utility.StringToDate(txtStartDate.Text)).TotalDays < 365)
            //    {
            //        Utility.FunShowAlertMsg(this, "Start Date ,End Date Range should be greater than 365 days");
            //        return;
            //    }
            //}
            if (rbtnSchedule.SelectedValue == "0")
            {
                if (string.IsNullOrEmpty(txtScheduleDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Select a Date for scheduling");
                    return;
                }
                if (string.IsNullOrEmpty(txtScheduleTime.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter the Time for scheduling");
                    return;
                }


                if (Utility.CompareDates(txtScheduleDate.Text, DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat)) == 1)
                {
                    Utility.FunShowAlertMsg(this, "Schedule Date should be greater than or equal to Current Date");
                    return;
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
                        if (span.Hours == 0 && span.Minutes < 4)
                        {
                            Utility.FunShowAlertMsg(this, "Schduled Time should be greater than or equal to 5 minutes for current time");
                            return;
                        }
                    }
                }
            }
            if (txtMonthYear.Text != "")
            {
                txtDataMonthYear.Text = txtMonthYear.Text;
            }
            string strMonthYear = "";
            if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            else
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@MonthYear", strMonthYear);
            DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            if (dtMonthLock.Rows.Count > 0)
            {
                if (dtMonthLock.Rows[0][0].ToString() == "True")
                {
                    Utility.FunShowAlertMsg(this, "Month/Year already Locked");
                    return;
                }
            }
            // txtDataFrequency.Text = ddlFrequency.SelectedItem.Text;
            txtDataLOB.Text = ddlLOB.SelectedItem.Text;
            FunPriLoadBranchDetails();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }

    /* Added by Suresh - Start */
    private void FetchInvoiceAccountData()
    {
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
        objProcedureParameter.Add("@BillingId", intBillingId.ToString());
        if (Convert.ToInt32(ddlcust.SelectedValue) > 0)
            objProcedureParameter.Add("@customer_id", ddlcust.SelectedValue);
        if (Convert.ToInt32(ddlFunder.SelectedValue) > 0 && ddlFunder.SelectedText != "")
            objProcedureParameter.Add("@Funder_id", ddlFunder.SelectedValue);
        if (Convert.ToInt32(ddlTranche.SelectedValue) > 0 && ddlTranche.SelectedText != "")
            objProcedureParameter.Add("@Tranche_id", ddlTranche.SelectedValue);
        objProcedureParameter.Add("@option", ddlRerun.SelectedValue);
        objProcedureParameter.Add("@STARTDATE", Utility.StringToDate(txtStartDate.Text).ToString());
        objProcedureParameter.Add("@ENDDATE", Utility.StringToDate(txtEndDate.Text).ToString());

        if (txtInvoice.Text != "")
            objProcedureParameter.Add("@Invoice_No", hdnInvoice.Value);

        if (txtAMFInvoice.Text != "")
            objProcedureParameter.Add("@AMFInvoice_No", hdnAMFInvoice.Value);

        string strXmlCustomerDetails = FunPriFormXml(gvCustomer, true, "chkSelectAllBranch", "chkSelectBranch");
        objProcedureParameter.Add("@XmlCustomerDetails", strXmlCustomerDetails);

        if (Convert.ToInt32(ddlLocation.SelectedValue) > 0 && ddlLocation.SelectedText != "")
            objProcedureParameter.Add("@Location_Id", ddlLocation.SelectedValue);

        //objProcedureParameter.Add("@ENDDATE", Utility.StringToDate("28/02/2018").ToString());
        dsaccounts = Utility.GetDataset("S3g_loanad_billcustomer_print", objProcedureParameter);

        dtaccounts = dsaccounts.Tables[0];
        ViewState["dtaccounts"] = dtaccounts;
        if (dtaccounts.Rows.Count > 0)
        {
            pnlrs.Visible = true;
            divacc.Style.Add("display", "block");
            grvRental.DataSource = dtaccounts;
            grvRental.DataBind();
            grvRental.Columns[4].Visible = true;
            if (ddlRerun.SelectedValue == "0")
            {
                btnPrint.Enabled = true;
                btnPrintAMF.Enabled = true;
                foreach (GridViewRow grv in grvRental.Rows)
                {
                    ImageButton imgbtnPrint = (ImageButton)grv.FindControl("imgbtnPrint");
                    ImageButton imgbtnPrint1 = (ImageButton)grv.FindControl("imgbtnPrint1");
                    imgbtnPrint.Enabled = true;
                    imgbtnPrint1.Enabled = true;

                    Label lblIs_Rental = (Label)grv.FindControl("lblIs_Rental");
                    Label lblIs_AMF = (Label)grv.FindControl("lblIs_AMF");

                    if (lblIs_Rental.Text == "0")
                    {
                        imgbtnPrint.Visible = false;
                    }
                    if (lblIs_AMF.Text == "0")
                    {
                        imgbtnPrint1.Visible = false;
                    }
                }
                btnRerun.Visible = false;

                grvRental.Columns[2].Visible = true;
                grvRental.Columns[3].Visible = true;

            }
            else if (ddlRerun.SelectedValue == "1")
            {
                btnPrint.Enabled = false;
                btnPrintAMF.Enabled = false;
                foreach (GridViewRow grv in grvRental.Rows)
                {
                    ImageButton imgbtnPrint = (ImageButton)grv.FindControl("imgbtnPrint");
                    ImageButton imgbtnPrint1 = (ImageButton)grv.FindControl("imgbtnPrint1");
                    imgbtnPrint.Enabled = false;
                    imgbtnPrint1.Enabled = false;
                }

                btnRerun.Visible = true;

                if (Request.QueryString["qsMode"] == "C")
                    btnRerun.Visible = false;

                grvRental.Columns[2].Visible = true;
                grvRental.Columns[3].Visible = true;

            }
            else if (ddlRerun.SelectedValue == "2")
            {
                btnRerun.Visible = false;


            }
            else if (ddlRerun.SelectedValue == "3")
            {
                btnRentalCancel.Enabled = false;
                BtnAMFCancel.Enabled = false;
                grvRental.Columns[4].Visible = false;
                btnRerun.Visible = false;
                btnPrint.Enabled = false;
                btnPrintAMF.Enabled = false;

                grvRental.Columns[2].Visible = true;
                grvRental.Columns[3].Visible = true;
            }

            else if (ddlRerun.SelectedValue == "4")
            {
                btnRentalCancel.Enabled = false;
                BtnAMFCancel.Enabled = false;
                
                btnRerun.Visible = false;
                btnPrint.Enabled = false;
                btnPrintAMF.Enabled = false;

                grvRental.Columns[2].Visible = false;
                grvRental.Columns[3].Visible = false;
                //grvRental.Columns[5].Visible = true;
                //grvRental.Columns[6].Visible = true;

            }

        }
        else
        {
            btnPrint.Enabled = false;
            btnPrintAMF.Enabled = false;
            pnlrs.Visible = true;
            btnRerun.Visible = false;
            divacc.Style.Add("display", "block");
            grvRental.EmptyDataText = "No records Found";
            grvRental.DataBind();
        }


    }

    /* Added by Suresh - End */

    protected void btnFetch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlRerun.SelectedValue == "0" || ddlRerun.SelectedValue == "2" || ddlRerun.SelectedValue == "3")
            //{
            //    if (ddlcust.SelectedText == "")
            //    {
            //        Utility.FunShowAlertMsg(this, "Select Lessee");
            //        return;
            //    }
            //}

            //if (ddlRerun.SelectedValue == "1")
            //{
            CheckBox chkSelectCustAll = gvCustomer.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            int intSelectedCustCount = 0;
            if (!chkSelectCustAll.Checked)
            {
                foreach (GridViewRow grBranch in gvCustomer.Rows)
                {
                    if (grBranch.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelect = grBranch.FindControl("chkSelectBranch") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            intSelectedCustCount += 1;
                        }
                    }
                }
                if (intSelectedCustCount == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one Customer for Bill Generation");
                    return;
                }
            }
            //}
            FetchInvoiceAccountData();

            //opc042 start
            if(ddlRerun.SelectedValue=="4")
            {
                btnSendEmail.Enabled = true;
            }
            else
            {
                btnSendEmail.Enabled = false;
            }
            //opc042 end
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvBilling.ErrorMessage = strErrorMessagePrefix + " Due to Data Problem,Unable to Load Location Details";
            cvBilling.IsValid = false;
        }

    }

    protected void rbtnSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtScheduleDate.Text = txtScheduleTime.Text = "";
        if (rbtnSchedule.SelectedValue == "0")
        {
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = calScheduleDate.Enabled = true;
            REVScheduleTime.Enabled = true;
            // REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = true;
        }
        else
        {
            txtScheduleDate.Enabled = txtScheduleTime.Enabled = calScheduleDate.Enabled = false;
            REVScheduleTime.Enabled = false;
            // REVScheduleTime.Enabled = RFVScheduleDate.Enabled = RFVScheduleTime.Enabled = false;
        }

    }

    protected void ddlRerun_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* Added by Suresh - Start */
        //if (Convert.ToString(Request.QueryString["qsMode"]) == "Q")
        //{
        //    if (ddlRerun.SelectedValue == "0")
        //    {
        //        btnInvoiceCancel.Visible = true;
        //    }
        //    else
        //    {
        //        btnInvoiceCancel.Visible = false;
        //    }
        //}
        //else
        //{
        //    btnInvoiceCancel.Visible = false;
        //}
        /* Added by Suresh - Start */


        if (ddlRerun.SelectedValue == "0" || ddlRerun.SelectedValue == "1")
        {
            btnRentalCancel.Enabled = false;
            BtnAMFCancel.Enabled = false;
        }
        //else if(ddlRerun.SelectedValue == "3")
        //{
        //    btnRentalCancel.Enabled = false;
        //    BtnAMFCancel.Enabled = false;

        //    for (int i = 0; i < grvRental.Rows.Count; i++)
        //    {
        //        string t1 = grvRental.Rows[i].Cells[1].Text;
        //        CheckBox chbkHeader = (CheckBox)grvRental.Rows[i].Cells[1].FindControl("chkSelectAllRS");
        //        CheckBox chbkChiled = grvRental.Rows[i].FindControl("chkSelectRS") as CheckBox;
        //        chbkHeader.Visible = false;
        //        chbkChiled.Visible = false;

        //        //CheckBox chkSelectBranch = e.Row.FindControl("chkSelectBranch") as CheckBox;


        //    }



        //}
        else
        {
            btnRentalCancel.Enabled = true;
            BtnAMFCancel.Enabled = true;
        }


        pnlrs.Visible = false;
        divacc.Style.Add("display", "none");
    }

    #endregion

    #endregion

    #region Grid Events

    protected void gvBranchWise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    CheckBox chkSelectAllBranch = e.Row.FindControl("chkSelectAllBranch") as CheckBox;
                    chkSelectAllBranch.Checked = true;
                    chkSelectAllBranch.Enabled = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelectBranch = e.Row.FindControl("chkSelectBranch") as CheckBox;
            TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "Q")
                {
                    chkSelectBranch.Checked = true;
                    chkSelectBranch.Enabled = false;
                    txtRemarks.ReadOnly = true;
                }
            }
            //CheckBox chkSelectAllBranch = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            //if (chkSelectBranch != null)
            //{
            //    chkSelectBranch.Attributes.Add("onclick", "javascript:fnSelectBranch(" + chkSelectBranch.ClientID + "," + chkSelectAllBranch.ClientID + ");");
            //}
        }

    }

    protected void gvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Request.QueryString["qsMode"] != null)
            {
                if (Request.QueryString["qsMode"].ToString() == "C")
                {
                    CheckBox chkSelectAllBranch = e.Row.FindControl("chkSelectAllBranch") as CheckBox;
                    chkSelectAllBranch.Checked = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelectBranch");
            chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvCustomer.ClientID + "','chkSelectAllBranch','chkSelectBranch');");
        }
    }

    #endregion

    #endregion

    #region Methods

    #region Local Methods

    private void FunPrilLoadPage()
    {
        S3GSession ObjS3GSession = new S3GSession();
        try
        {

            //txtStartDate.ReadOnly = true;
            //txtEndDate.ReadOnly = true;
            //imgStartDate.Visible = false;
            //imgEndDate.Visible = false;
            txtMonthYear.Attributes.Add("readonly", "true");
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //txtStartDate.Attributes.Add("readonly", "true");
            //txtEndDate.Attributes.Add("readonly", "true");
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            //calEndDate.Format = strDateFormat;
            //calStartDate.Format = strDateFormat;
            calScheduleDate.Format = strDateFormat;
            txtScheduleDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtScheduleDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',false,  false);");
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                intBillingId = Convert.ToInt32(fromTicket.Name);
            }
            if (txtMonthYear.Text == "")
            {
                // txtBillMonthYear.Text = txtMonthYear.Text =
                txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
            }
            if (!IsPostBack)
            {
                tcBilling.ActiveTabIndex = 0;

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriDisableControls(-1);
                    gvControlDataSheet.Columns[4].Visible = true;
                }
                else
                {
                    FunPriLoadLOV();
                    //tcBilling.Tabs[1].Enabled = tcBilling.Tabs[2].Enabled = false;  //Commented On 07-Sep-2017
                    //btnBillPDF.Enabled = false;
                    FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Billing Related Details");
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlFrequency.SelectedIndex = 0;
        txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtDataLOB.Text = txtMonthYear.Text =
         txtDataFrequency.Text = "";
        pnlBranch.Visible = false;
        gvBranchWise.DataSource = null;
        gvBranchWise.DataBind();
        pnlControlData.Visible = false;
        gvControlDataSheet.DataSource = null;
        gvControlDataSheet.DataBind();
        btnSave.Enabled = false;
        if (txtMonthYear.Text == "")
        {
            //txtBillMonthYear.Text = txtMonthYear.Text =
            txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
        }
        if (ddlLOB.SelectedIndex > 0)
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@LobId", ddlLOB.SelectedValue);
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            DataTable dtDocPath = Utility.GetDefaultData("s3g_LoanAd_GetBillingDocPath", objProcedureParameter);
            if (dtDocPath.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Define the Document Path for Billing in Document Path Setup");
                return;
            }
            else
            {
                ViewState["DocPath"] = strDocPath = dtDocPath.Rows[0]["Document_Path"].ToString();
            }


        }


    }



    private void FunGetBillPDF()
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.dtScheduleDate = DateTime.Now;
            objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();
            objBillingEntity.intFrequency = intBillingId;
            objBillingEntity.intUserId = intUserID;
            intErrorCode = objServiceClient.FunPubGetPDF(objBillingEntity);
            if (intErrorCode == 0)
            {
                if (ViewState["DocPath"] != null)
                {
                    string strMessage = "Generated Bills are available in Server after " + objBillingEntity.strScheduleTime + "(" + ViewState["DocPath"].ToString() + " Directory)";
                    Utility.FunShowAlertMsg(this, strMessage);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Get Bill Details");
        }
        finally
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }


    private void FunPriSaveRecord()
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        //        ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient objServiceClient = new ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {


            CheckBox chkSelectAll = gvBranchWise.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            int intSelectedBranchCount = 0;
            if (!chkSelectAll.Checked)
            {
                foreach (GridViewRow grBranch in gvBranchWise.Rows)
                {
                    if (grBranch.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelect = grBranch.FindControl("chkSelectBranch") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            intSelectedBranchCount += 1;
                        }
                    }
                }
                if (intSelectedBranchCount == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one Location for Bill Generation");
                    return;
                }
            }

            CheckBox chkSelectCustAll = gvCustomer.HeaderRow.FindControl("chkSelectAllBranch") as CheckBox;
            int intSelectedCustCount = 0;
            if (!chkSelectCustAll.Checked)
            {
                foreach (GridViewRow grBranch in gvCustomer.Rows)
                {
                    if (grBranch.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkSelect = grBranch.FindControl("chkSelectBranch") as CheckBox;
                        if (chkSelect.Checked)
                        {
                            intSelectedCustCount += 1;
                        }
                    }
                }
                if (intSelectedCustCount == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select atleast one Customer for Bill Generation");
                    return;
                }
            }

            string strMonthYear = "";
            if (Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month).Length == 1)
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + "0" + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            else
            {
                strMonthYear = Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Year) + Convert.ToString(Convert.ToDateTime(txtMonthYear.Text).Month);
            }
            //s3g_loanad_CheckPrevMonthLock
            string strXmlBranchDetails = FunPriFormXml(gvBranchWise, true, "chkSelectAllBranch", "chkSelectBranch");
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            objProcedureParameter.Add("@XmlBranchDetails", strXmlBranchDetails);
            objProcedureParameter.Add("@MonthYear", strMonthYear);
            DataTable dtMonthLock = Utility.GetDefaultData("s3g_loanad_CheckPrevMonthLock", objProcedureParameter);
            bool blnIsUnLockBranch = false;

            foreach (DataRow drBranchMonth in dtMonthLock.Rows)
            {
                if (drBranchMonth["Month_Lock"].ToString() == "False")
                {
                    blnIsUnLockBranch = true;
                }
            }
            //if (blnIsUnLockBranch || dtMonthLock.Rows.Count == 0)
            //{
            //    Utility.FunShowAlertMsg(this, "Previous Month/Year should be locked for selected Location(s) ");
            //    return;
            //}
            objBillingEntity.intCompanyId = intCompanyID;
            objBillingEntity.intLobId = Convert.ToInt32(ddlLOB.SelectedValue);
            objBillingEntity.intFrequency = 0;
            objBillingEntity.intBranchId = 0;

            objBillingEntity.lngMonthYear = Convert.ToInt64(strMonthYear);
            objBillingEntity.intUserId = intUserID;
            objBillingEntity.dtStartDate = Utility.StringToDate(txtStartDate.Text);
            objBillingEntity.dtEndDate = Utility.StringToDate(txtEndDate.Text);
            objBillingEntity.dtBillingDate = Utility.StringToDate(DateTime.Now.ToString());
            if (rbtnSchedule.SelectedValue == "0")
            {
                objBillingEntity.strScheduleTime = txtScheduleTime.Text;
            }
            else
            {
                txtScheduleDate.Text = DateTime.Parse(DateTime.Now.ToShortDateString(), CultureInfo.CurrentCulture).ToString(strDateFormat);
                objBillingEntity.strScheduleTime = DateTime.Now.AddMinutes(5.0).ToShortTimeString();

            }
            objBillingEntity.dtScheduleDate = Utility.StringToDate(txtScheduleDate.Text);
            objBillingEntity.strXmlBranchDetails = strXmlBranchDetails;
            objBillingEntity.strXmlControlDataDetails = gvControlDataSheet.FunPubFormXml(true);
            //Added On 07-Sep-2016 = > Code Starts
            if (Convert.ToInt32(ddlcust.SelectedValue) > 0 && ddlcust.SelectedText != "")
            {
                objBillingEntity.intCustomer_ID = Convert.ToInt32(ddlcust.SelectedValue);

                dtaccounts = (DataTable)ViewState["dtaccounts"];
                DataTable dtrerun;
                string panum;
                dtrerun = new DataTable();
                dtrerun.Columns.Add("Rs_number");
                if (dtaccounts.Rows.Count > 0)
                {
                    foreach (GridViewRow gvrow in grvRental.Rows)
                    {
                        CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                        Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");

                        if (chkSelectAccount.Checked)
                        {
                            DataRow dr = dtrerun.NewRow();
                            panum = lblRS_Number.Text.ToString();

                            dr["Rs_number"] = panum;
                            dtrerun.Rows.Add(dr);
                            dtrerun.AcceptChanges();
                        }
                    }
                    ViewState["dtrerun"] = dtrerun;
                }
                objBillingEntity.strXmlAccDetails = dtrerun.FunPubFormXml();
            }

            string strXmlCustomerDetails = FunPriFormXml(gvCustomer, true, "chkSelectAllBranch", "chkSelectBranch");
            objBillingEntity.strXmlCustomerDetails = strXmlCustomerDetails;

            //Added On 07-Sep-2016 = > Code Ends
            //objBillingEntity.strXmlCashFlowDetails = FunPriFormCFXml(grvCashFlow, true); //grvCashFlow.FunPubFormXml(true);
            string strJournalMessage = "";
            intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            //intErrorCode = objServiceClient.FunPubCreateBillingInt(out strJournalMessage, objBillingEntity);
            if (intErrorCode == 0)
            {
                strAlert = "Billing Generated successfully";
                strAlert += @"\n\nWould you like to generate one more Bill ?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            else if (intErrorCode == 53)
            {
                Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                return;
            }
            else if (intErrorCode == 13)
            {
                Utility.FunShowAlertMsg(this, "Month is not open in GPS");
                return;
            }
            else if (intErrorCode == 14)
            {
                Utility.FunShowAlertMsg(this, "OPC Bank Account Should be Selected In GPS");
                return;
            }
            else if (intErrorCode == 20)
            {
                Utility.FunShowAlertMsg(this, "CashFlow Master not defined");
                return;
            }
            else
            {
                if (intErrorCode == 50)
                    Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                else
                    Utility.FunShowValidationMsg(this.Page, "CLN_BILL", intErrorCode, strJournalMessage, false);
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Insert Billing Details");
        }
        finally 
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }

    private string FunPriFormXml(GridView grvXml, bool IsNeedUpperCase, string HdrCheckbox, string DtlCheckBox)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            CheckBox chkSelect = grvRow.FindControl(DtlCheckBox) as CheckBox;
            CheckBox chkSelectAll = grvXml.HeaderRow.FindControl(HdrCheckbox) as CheckBox;
            bool blnIsRowSelect = false;
            if ((!chkSelectAll.Checked && chkSelect.Checked) || chkSelectAll.Checked)
            {
                blnIsRowSelect = true;
            }
            intcolcount = 0;
            if (blnIsRowSelect)
            {
                strbXml.Append(" <Details ");
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {

                    if (grvXml.Columns[intcolcount].HeaderText != "")
                    {
                        strColValue = grvRow.Cells[intcolcount].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[intcolcount].Controls.Count > 0)
                            {
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                            }
                            if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                            {
                                if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                                {
                                    strColValue = Utility.StringToDate(strColValue).ToString();
                                }
                            }

                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                        // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                        // Avoid adding that column to XML to insert the null value
                        if (strColValue.Trim() == "&nbsp;")
                        {
                            continue;
                        }
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strColValue = strColValue.Replace("'", "\"");
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                    }

                }
                strbXml.Append(" /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    private string FunPriFormCFXml(GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            CheckBox chkSelect = grvRow.FindControl("chkSelectCF") as CheckBox;
            CheckBox chkSelectAll = grvXml.HeaderRow.FindControl("chkSelectAllCF") as CheckBox;
            bool blnIsRowSelect = false;
            if ((!chkSelectAll.Checked && chkSelect.Checked) || chkSelectAll.Checked)
            {
                blnIsRowSelect = true;
            }
            intcolcount = 0;
            if (blnIsRowSelect)
            {
                strbXml.Append(" <Details ");
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {

                    if (grvXml.Columns[intcolcount].HeaderText != "")
                    {
                        strColValue = grvRow.Cells[intcolcount].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[intcolcount].Controls.Count > 0)
                            {
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                            }
                            if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                            {
                                if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                                {
                                    strColValue = Utility.StringToDate(strColValue).ToString();
                                }
                            }

                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                        // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                        // Avoid adding that column to XML to insert the null value
                        if (strColValue.Trim() == "&nbsp;")
                        {
                            continue;
                        }
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strColValue = strColValue.Replace("'", "\"");
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                    }

                }
                strbXml.Append(" /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    private void FunPriClosePage()
    {
        try
        {
            Response.Redirect(strRedirectPage, false);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Cancel this process");
        }
    }

    private void FunPriClearPage()
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            //ddlFrequency.SelectedIndex = 0;
            txtStartDate.Text = txtEndDate.Text = txtScheduleDate.Text = txtScheduleTime.Text = txtDataLOB.Text = txtMonthYear.Text =
             txtDataFrequency.Text = "";
            pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
            pnlControlData.Visible = false;
            gvControlDataSheet.DataSource = null;
            gvControlDataSheet.DataBind();
            btnSave.Enabled = false;
            if (txtMonthYear.Text == "")
            {
                txtMonthYear.Text =
                txtDataMonthYear.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.Year;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Clear the Data");
        }
    }

    private void FunPriLoadLOV()
    {
        try
        {
            if (objProcedureParameter != null)
                objProcedureParameter.Clear();
            else
                objProcedureParameter = new Dictionary<string, string>();

            objProcedureParameter.Add("@OPTION", "1");
            objProcedureParameter.Add("@COMPANYID", Convert.ToString(intCompanyID));
            objProcedureParameter.Add("@USERID", Convert.ToString(intUserID));
            objProcedureParameter.Add("@ProgramId", "95");
            if (Request.QueryString["qsMode"] == "Q")
            {
                objProcedureParameter.Add("@TYPE", "Q");
            }
            ddlLOB.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.SelectedIndex = 1;
            ddlLOB.ClearDropDownList();
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "2");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            // ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });

            if (Request.QueryString["qsMode"] == "C")
            {
                ddlRerun.Items.RemoveAt(1);
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem("Bill Generation", "1");
                ddlRerun.Items.Insert(1, item);
            }

        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Line of Business / Frequency");
        }
    }


    private void FunPriLoadFrequency()
    {
        try
        {
            objProcedureParameter.Clear();
            objProcedureParameter.Add("@OPTION", "2");
            objProcedureParameter.Add("@TYPE", S3G_Statu_Lookup.ORG_ROI_RULES_FREQUENCY.ToString());
            //ddlFrequency.BindDataTable("S3G_CLN_LOADLOV", objProcedureParameter, new string[] { "FREQUENCY_ID", "FREQUENCY_CODE" });
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data Problem,Unable to Load Line of Business / Frequency");
        }
    }

    private void FunPriLoadBranchDetails()
    {
        try
        {
            DataTable dtBranchwise = new DataTable();
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@USERID", intUserID.ToString());
            objProcedureParameter.Add("@OPTION", "1");
            objProcedureParameter.Add("@COMPANY_ID", intCompanyID.ToString());
            objProcedureParameter.Add("@LOBID", ddlLOB.SelectedValue);
            objProcedureParameter.Add("@STARTDATE", Utility.StringToDate(txtStartDate.Text).ToString());
            objProcedureParameter.Add("@ENDDATE", Utility.StringToDate(txtEndDate.Text).ToString());
            DataSet dsBranchwise = Utility.GetDataset("S3G_CLN_LOADBILLINGBRANCH", objProcedureParameter);
            gvBranchWise.DataSource = dsBranchwise.Tables[0];
            gvBranchWise.DataBind();
            Session["dttarget"] = dsBranchwise.Tables[0];
            FunPriFindTotal();
            pnlBranch.Visible = true;

            pnlCustomer.Visible = true;
            gvCustomer.DataSource = dsBranchwise.Tables[1];
            gvCustomer.DataBind();

            if (gvBranchWise.Rows.Count > 0)
            {
                btnSave.Enabled = true;

            }
            else
            {
                btnSave.Enabled = false;
            }
            pnlControlData.Visible = true;
            //gvControlDataSheet.DataSource = dsBranchwise.Tables[1];
            //gvControlDataSheet.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Location Details");
        }
    }

    private void FunPriFindTotal()
    {
        try
        {
            if (Session["dttarget"] != null)
                dttarget = (System.Data.DataTable)Session["dttarget"];
            if (dttarget.Rows.Count > 0)
            {
                Label lbltotaccountcount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotaccountcount") as Label;
                lbltotaccountcount.Text = Convert.ToDecimal(dttarget.Compute("sum(ACCOUNTCOUNT)", "")).ToString();

                Label lbltotopcamount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotopcamount") as Label;
                lbltotopcamount.Text = Convert.ToDecimal(dttarget.Compute("sum(DEBITAMOUNT1)", "")).ToString();

                Label lbltotFunderamt = (Label)(gvBranchWise).FooterRow.FindControl("lbltotFunderamt") as Label;
                lbltotFunderamt.Text = Convert.ToDecimal(dttarget.Compute("sum(funder_amt1)", "")).ToString();

            }
        }
        catch (Exception ex)
        {
            cvBilling.ErrorMessage = "Due to Data Problem,Unable to Move Invoices.";
            cvBilling.IsValid = false;
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

    #endregion

    #region Common Methods

    #region "User Authorization"

    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    btnPrint.Enabled = false;
                    btnPrintAMF.Enabled = false;
                    //btnXLPorting.Enabled = false;
                    ddlcust.Enabled = false;
                    txtInvoice.Enabled = false;
                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnSave.Enabled = false;
                    btnGonw.Visible = false;
                    btnPrint.Enabled = false;
                    btnPrintAMF.Enabled = false;
                    FunPriLoadBillingDetails();
                    txtStartDate.ReadOnly = txtEndDate.ReadOnly = txtScheduleDate.ReadOnly = true;
                    txtScheduleDate.Attributes.Remove("onblur");
                    txtStartDate.Attributes.Remove("onblur");
                    txtEndDate.Attributes.Remove("onblur");
                    //btnXLPorting.Enabled = false;
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }


                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Toggling the Controls based on Mode");
        }
    }

    private void FunPriLoadBillingDetails()
    {
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
        objProcedureParameter.Add("@BillingId", intBillingId.ToString());
        DataSet dsBilling = Utility.GetDataset("s3g_loanad_GetBillingDetails", objProcedureParameter);
        //FunPriLoadLOV();
        ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString(), dsBilling.Tables[0].Rows[0]["Lob_Id"].ToString()));
        ddlLOB.ToolTip = dsBilling.Tables[0].Rows[0]["Lob_Name"].ToString();
        // ddlLOB.ClearDropDownList();
        //FunProLoadCahsFlows();

        //ddlFrequency.Items.Add(new System.Web.UI.WebControls.ListItem(dsBilling.Tables[0].Rows[0]["FREQUENCY_Name"].ToString(), dsBilling.Tables[0].Rows[0]["Frequency_Type"].ToString()));
        //ddlFrequency.ToolTip = dsBilling.Tables[0].Rows[0]["FREQUENCY_Name"].ToString();
        FunPriLoadFrequency();

        //ddlFrequency.SelectedValue = dsBilling.Tables[0].Rows[0]["Frequency_Type"].ToString();
        //ddlFrequency.ClearDropDownList();

        txtMonthYear.Text = dsBilling.Tables[0].Rows[0]["Month_Year"].ToString();
        calMonthYear.Enabled = false;
        txtDataMonthYear.Text = txtMonthYear.Text;
        txtStartDate.Text = dsBilling.Tables[0].Rows[0]["StartDate"].ToString();
        //calStartDate.Enabled = false;
        txtEndDate.Text = dsBilling.Tables[0].Rows[0]["EndDate"].ToString();
        //calEndDate.Enabled = false;
        btnGonw.Visible = false;
        rbtnSchedule.Enabled = false;
        calScheduleDate.Enabled = false;
        txtScheduleDate.Text = dsBilling.Tables[0].Rows[0]["ScheduleDate"].ToString();
        txtScheduleTime.Attributes.Add("readonly", "true");
        txtScheduleTime.Text = dsBilling.Tables[0].Rows[0]["ScheduleTime"].ToString();
        txtScheduleTime.Text = Convert.ToDateTime(txtScheduleTime.Text).ToShortTimeString();
        txtBillNumber.Text = dsBilling.Tables[0].Rows[0]["FolderNumber"].ToString();
        //Added On 07-Sep-2016 = > Code Starts
        if (!string.IsNullOrEmpty(dsBilling.Tables[0].Rows[0]["Lessee_Name"].ToString()))
        {
            ddlcust.SelectedText = dsBilling.Tables[0].Rows[0]["Lessee_Name"].ToString();
        }
        //Added On 07-Sep-2016 = > Code Ends
        string strFolderNo = dsBilling.Tables[0].Rows[0]["FolderNumber"].ToString();
        string strDocumentPath = dsBilling.Tables[0].Rows[0]["Document_Path"].ToString();
        string strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
        string strbillperiod = dsBilling.Tables[0].Rows[0]["Month_Year"].ToString();
        decimal decGrandTotal = 0;
        //for (int idx = 0; idx < dt.Rows.Count; idx++)
        //{
        //    decGrandTotal = Convert.ToDecimal(dt.Rows[idx]["AMOUNT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["VAT"].ToString()) + Convert.ToDecimal(dt.Rows[idx]["Service_TAX"].ToString());
        //}



        //strnewFile += "\\" + strbillperiod;
        ViewState["strnewFile"] = strnewFile;
        if (dsBilling.Tables[4].Rows.Count > 0)
        {
            ViewState["DocPath"] = strDocPath = dsBilling.Tables[4].Rows[0]["Document_Path"].ToString();
        }
        if (dsBilling.Tables[1].Rows.Count > 0)
        {
            pnlBranch.Visible = true;
            gvBranchWise.DataSource = dsBilling.Tables[1];
            gvBranchWise.DataBind();
            Session["dttarget"] = dsBilling.Tables[1];
            if (Session["dttarget"] != null)
                dttarget = (System.Data.DataTable)Session["dttarget"];
            if (dttarget.Rows.Count > 0)
            {
                Label lbltotaccountcount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotaccountcount") as Label;
                lbltotaccountcount.Text = Convert.ToDecimal(dttarget.Compute("sum(ACCOUNTCOUNT)", "")).ToString();

                Label lbltotopcamount = (Label)(gvBranchWise).FooterRow.FindControl("lbltotopcamount") as Label;
                lbltotopcamount.Text = Convert.ToDecimal(dttarget.Compute("sum(DEBITAMOUNT1)", "")).ToString(Funsetsuffix());

                Label lbltotFunderamt = (Label)(gvBranchWise).FooterRow.FindControl("lbltotFunderamt") as Label;
                lbltotFunderamt.Text = Convert.ToDecimal(dttarget.Compute("sum(funder_amt1)", "")).ToString(Funsetsuffix());

            }
        }
        else
        {
            pnlBranch.Visible = false;
            gvBranchWise.DataSource = null;
            gvBranchWise.DataBind();
        }
        // txtDataFrequency.Text = ddlFrequency.SelectedItem.Text;
        txtDataLOB.Text = ddlLOB.SelectedItem.Text;
        if (dsBilling.Tables[2].Rows.Count > 0)
        {
            pnlControlData.Visible = true;
            gvControlDataSheet.DataSource = dsBilling.Tables[2];
            gvControlDataSheet.DataBind();
        }
        else
        {
            pnlControlData.Visible = false;
            gvControlDataSheet.DataSource = null;
            gvControlDataSheet.DataBind();
        }
        //btnBillPDF.Enabled = true;
        btnClear.Enabled = false;

        if (dsBilling.Tables[5].Rows.Count > 0)
        {
            pnlCustomer.Visible = true;
            gvCustomer.DataSource = dsBilling.Tables[5];
            gvCustomer.DataBind();
        }
    }



    ////Code end
    #endregion

    #endregion
    #endregion

    protected bool FunProCheckLOB()
    {
        if (ddlLOB.SelectedItem.Text.StartsWith("TE") || ddlLOB.SelectedItem.Text.StartsWith("TL") || ddlLOB.SelectedItem.Text.StartsWith("WC"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void gvControlDataSheet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Billing_Control_Id", e.CommandArgument.ToString());
        DataTable dtAccounts = Utility.GetDefaultData("S3G_LOANAD_GetBillingAccounts", objProcedureParameter);

        if (dtAccounts.Rows.Count == 0)
        {
            DataRow dRow = dtAccounts.NewRow();
            dtAccounts.Rows.Add(dRow);

            grvAccounts.DataSource = dtAccounts;
            grvAccounts.DataBind();

            grvAccounts.Rows[0].Visible = false;
            btnXLPorting.Enabled = false;
        }
        else
        {
            //btnXLPorting.Enabled = true;
            grvAccounts.DataSource = dtAccounts;
            grvAccounts.DataBind();
            btnXLPorting.Enabled = true;
        }




    }


    // Code added by Santhosh.S on 10.07.2013 to export Gridview data to Excel Sheet
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
                string attachment = "attachment; filename=" + FileName + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

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
    public static string[] GetFundList(String prefixText, int count)
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

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", obj_Page.intCompanyID.ToString());

        Procparam.Add("@Option", "12");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetAMFInvoiceNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Company_Id", obj_Page.intCompanyID.ToString());

        Procparam.Add("@Option", "15");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRSLst_AGT", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Add("@option", "22");
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID.ToString()));
        Procparam.Add("@PrefixText", Convert.ToString(prefixText));
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GETLASCMNLST", Procparam));

        return suggetions.ToArray();
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        DateTime origDT = Utility.StringToDate(txtStartDate.Text);
        DateTime lastDate = new DateTime(origDT.Year, origDT.Month, 1).AddMonths(1).AddDays(-1);

        if ((!(string.IsNullOrEmpty(txtStartDate.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDate.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtEndDate.Text) > lastDate) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End date should not exceed the last date of Month Year");
                txtEndDate.Text = lastDate.ToString(strDateFormat);
                return;
            }
        }
    }
    protected void txtMonthYear_TextChanged(object sender, EventArgs e)
    {
        string str = "01" + txtMonthYear.Text;
        DateTime dt = Utility.StringToDate(str);
        DateTime dtstartdate = new DateTime(Convert.ToInt32(((TextBox)sender).Text.Substring(((TextBox)sender).Text.IndexOf('-') + 1)), dt.Month, 1);

        txtStartDate.Text = dtstartdate.ToString("dd-MMM-yyyy");

        DateTime dtenddate = dtstartdate.AddMonths(1).AddDays(-1);

        txtEndDate.Text = dtenddate.ToString("dd-MMM-yyyy");
    }

    protected void txtInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnInvoice.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtInvoice.Text = string.Empty;
                hdnInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    protected void txtAMFInvoice_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnAMFInvoice.Value;

            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtAMFInvoice.Text = string.Empty;
                hdnAMFInvoice.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    // opc038 start
    protected void btnEmail_Click(object sender, EventArgs e)
    {

        btnSendEmail.Enabled = false;
        StringBuilder strRSDetails = new StringBuilder();
        StringBuilder strGroupRSDetails = new StringBuilder();
        int intRowCt = 0;
        string strHTML_Email = "";
        string strHTML_Email_Base = "";
        string strPONos = "";
        StringBuilder strBody = new StringBuilder();
        string strErrMsg = "";
        List<string> arrRSDetails = new List<string>();
        List<string> arrGroupRSDet = new List<string>();

        DataSet dsEmail = new DataSet();
        DataSet dsTrancheEmail = new DataSet();
        try
        {
            strRSDetails.Append("<Root>");

            foreach (GridViewRow grvRow in grvRental.Rows)
            {
                if (((CheckBox)grvRow.FindControl("chkSelectRS")).Checked)
                {
                    intRowCt = intRowCt + 1;
                    Label lblRS_Number = (Label)grvRow.FindControl("lblRS_Number");
                    Label lblEmail_Group = (Label)grvRow.FindControl("lblEmail_Group");
                    if (!arrRSDetails.Contains(lblRS_Number.Text))
                    {
                        strRSDetails.Append(
                       " <Details RS_Number = '" + lblRS_Number.Text + "'  Group_Name = '" + lblEmail_Group.Text + "'   />");
                        arrRSDetails.Add(lblRS_Number.Text);
                        //arrEmailGroupDet.Add(lblEmail_Group.Text);
                        if(lblEmail_Group.Text=="")
                        {
                            Utility.FunShowAlertMsg(this.Page, "Email Group should not be empty for Selected Tranche");
                            return;
                        }
                    }
                }
            }
            strRSDetails.Append("</Root>");

            if (intRowCt == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one Record");
                return;
            }

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@XMLRSDetail", strRSDetails.ToString());
            dictParam.Add("@Billing_id", txtBillNumber.Text);
            //Fetch Email Groups based on Tranche using below SP
            dsEmail = Utility.GetDataset("S3G_ORG_Get_RS_Bill_Email_Group_Det", dictParam);

            if (dsEmail.Tables[0].Columns.Contains("Error_Code"))
            {
                if (dsEmail.Tables[0].Rows[0]["Error_Code"].ToString() == "1")
                {
                    Utility.FunShowAlertMsg(this.Page, "Error: Multiple Customer's Tranche selection not allowed ");
                    return;
                }

                if (dsEmail.Tables[0].Rows[0]["Error_Code"].ToString() == "2")
                {
                    Utility.FunShowAlertMsg(this.Page, "Error: Unable to Send Mail for State wise Billing. ");
                    return;
                }

                //if (dsEmail.Tables[0].Rows[0]["Error_Code"].ToString() == "2")
                //{
                //    strPDFCountMatch = strPDFCountMatch + "PDF File Count is not Matching for Tranche " + arrRSDetails[i].ToString();
                //    //Utility.FunShowAlertMsg(this.Page, "Error: PDF File Count is not Matching ");
                //    //return;
                //}

            }
            strHTML_Email = PDFPageSetup.FunPubGetTemplateContent(1, 3, 0, 80, "0");

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogoEmail~");
            List<string> listImagePath = new List<string>();
            listImagePath.Add("cid:CompanyLogoEmail");
            strHTML_Email = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML_Email);
            strHTML_Email_Base = strHTML_Email;
            
            //Send mail based on group , if no of group is 2 then loop will execute 2 times
            for (int i = 0; i < dsEmail.Tables[0].Rows.Count; i++)
            {
                arrGroupRSDet.Clear();
                strGroupRSDetails.Clear();
                strGroupRSDetails.Append("<Root>");
                string strGroupName; string strCustomer_ID;   string strDue_Date;
                string strEmailTo; string strEmailCC; string strStatus;
                ArrayList arrMailAttachement = new ArrayList();
                string filePath_Invoice = "";
                //string strPDFCountMatch = "";
                strErrMsg = "";
                int required_no_of_pdf = 0;
                strGroupName = dsEmail.Tables[0].Rows[i]["Group_Name"].ToString();
                strCustomer_ID = dsEmail.Tables[0].Rows[i]["Customer_Id"].ToString();
                strDue_Date = dsEmail.Tables[0].Rows[i]["Due_Date"].ToString();
                required_no_of_pdf = Convert.ToInt32(dsEmail.Tables[0].Rows[i]["Tot_PDF_Count"].ToString());

                foreach (GridViewRow grvRow in grvRental.Rows)
                {
                    if (((CheckBox)grvRow.FindControl("chkSelectRS")).Checked)
                    {
                        Label lblRS_Number = (Label)grvRow.FindControl("lblRS_Number");
                        Label lblEmail_Group = (Label)grvRow.FindControl("lblEmail_Group");
                        //using (StreamWriter writer = new StreamWriter("D:\\S3G_OPC\\Err_Log.txt", true))
                        //{
                        //    writer.WriteLine(strGroupName);
                        //    writer.WriteLine(lblEmail_Group.Text);
                        //}
                        if (strGroupName == lblEmail_Group.Text)
                        {
                           if (!arrGroupRSDet.Contains(lblRS_Number.Text))
                            {
                                strGroupRSDetails.Append(
                                      " <Details RS_Number = '" + lblRS_Number.Text + "' />");
                                arrGroupRSDet.Add(lblRS_Number.Text);
                               
                            }
                        }

                    }
                }
                strGroupRSDetails.Append("</Root>");

                GetPDFFiles_Email("Rental", arrGroupRSDet);

                if (required_no_of_pdf > (Convert.ToInt32(ViewState["FilePath_PDF_Count"].ToString())))
                {
                    if (strPDFCountException.ToString() == "")
                    {
                        strPDFCountException.Append("PDF File Count Mismatch for E Mail Group " + strGroupName);
                    }
                    else
                    {
                        strPDFCountException = strPDFCountException.Append("," + strGroupName);

                    }
                }
                else
                {

                    filePath_Invoice = ViewState["FilePath_Email_Attach"].ToString();
                    long length = new System.IO.FileInfo(filePath_Invoice).Length;
                    Double lengthinMB = (length / 1024.0) / 1024.0;

                    if (lengthinMB > 5)
                    {
                        if (strFileSizeException.ToString() == "")
                        {
                            strFileSizeException.Append("Mail cannot be sent , File size is > 5 MB for E Mail Group " + strGroupName);
                        }
                        else
                        {
                            strFileSizeException = strFileSizeException.Append("," + strGroupName);

                        }

                    }
                    else
                    {
                        dictParam = new Dictionary<string, string>();
                        dictParam.Add("@Email_Group", strGroupName);
                        dictParam.Add("@Customer_ID", strCustomer_ID);
                        dictParam.Add("@XMLRSDetail", strGroupRSDetails.ToString());
                        dictParam.Add("@Due_Date", strDue_Date);

                        //ViewState["FilePath_Email_Attach"] = filePath;
                        //ViewState["FilePath_PDF_Count"] = fileNames.Length;
                        //dictParam.Add("@PDF_Count", ViewState["FilePath_PDF_Count"].ToString());
                        //dictParam.Add("@Bill_Type", "Tranche");

                        //Fetch E-mail To,CC and Due to funder etc using below SP
                        dsTrancheEmail = Utility.GetDataset("S3G_ORG_Get_RS_Bill_Email_Det", dictParam);

                        arrMailAttachement.Clear();
                        strBody.Clear();
                        Dictionary<string, string> dictMail = new Dictionary<string, string>();

                        strEmailTo = dsTrancheEmail.Tables[0].Rows[0]["Email_To"].ToString();
                        strEmailCC = dsTrancheEmail.Tables[0].Rows[0]["CC"].ToString();

                        if (dictMail.Count == 0)
                        {
                            //strLesseName = dsTrancheEmail.Tables[0].Rows[0]["Customer_Name"].ToString();
                            dictMail.Add("FromMail", dsTrancheEmail.Tables[0].Rows[0]["From_Email"].ToString());
                            dictMail.Add("ToCC", strEmailCC);
                            dictMail.Add("DisplayName", dsTrancheEmail.Tables[0].Rows[0]["Display_Name"].ToString());
                            dictMail.Add("ToMail", strEmailTo);
                            dictMail.Add("Subject", dsTrancheEmail.Tables[0].Rows[0]["Subject_Name"].ToString());

                            dictMail.Add("imgPath", Server.MapPath("../Images/TemplateImages/" + CompanyId + "/Company_Logo_Email_Img.jpg"));
                            //filePath_zip = Server.MapPath(".") + "\\PDF Files\\PO_OPC_" + dsTrancheEmail.Tables[0].Rows[0]["Short_Name"].ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".zip";
                        }

                        arrMailAttachement.Add(filePath_Invoice);

                        strHTML_Email = strHTML_Email_Base;

                        if (dsTrancheEmail.Tables[0].Rows.Count > 0)
                            strHTML_Email = PDFPageSetup.FunPubBindCommonVariables(strHTML_Email, dsTrancheEmail.Tables[0]);

                        if (dsTrancheEmail.Tables[1].Rows.Count > 0)
                        {
                            if (dsTrancheEmail.Tables[1].Rows[0]["Is_Funder"].ToString() == "0")
                            {
                                strHTML_Email = PDFPageSetup.FunPubDeleteTable("~Rental_Table_Funder~", strHTML_Email);
                                strHTML_Email = PDFPageSetup.FunPubBindTable("~Rental_Table~", strHTML_Email, dsTrancheEmail.Tables[1]);
                            }
                            else
                            {
                                strHTML_Email = PDFPageSetup.FunPubDeleteTable("~Rental_Table~", strHTML_Email);
                                strHTML_Email = PDFPageSetup.FunPubBindTable("~Rental_Table_Funder~", strHTML_Email, dsTrancheEmail.Tables[1]);
                            }
                        }
                        
                        //strHTML_Email = strHTML_Email.Replace("~User_Name~", ObjUserInfo.ProUserNameRW);
                        strBody = strBody.Append(strHTML_Email);

                        strErrMsg = Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);

                     
                        if (strErrMsg != "")
                        {

                            if (strExceptionEmail.ToString() == "")
                            {
                                strExceptionEmail.Append("Unable to sent Email to " + dsEmail.Tables[0].Rows[i]["Group_Name"].ToString() + " " + strErrMsg);
                            }
                            else
                            {
                                strExceptionEmail = strExceptionEmail.Append("," + dsEmail.Tables[0].Rows[i]["Group_Name"].ToString() + " " + strErrMsg);

                            }
                            //FunPubCreateBillEmailStatus(strGroupName, strCustomer_ID, strGroupRSDetails.ToString(), txtBillNumber.Text,
                            //     strEmailTo, strEmailCC, "E", strErrMsg);
                            //Utility.FunShowAlertMsg(this.Page, strExceptionEmail.ToString());
                            //return;
                        }
                        else
                        {
                            FunPubCreateBillEmailStatus(strGroupName, strCustomer_ID, strGroupRSDetails.ToString(), Convert.ToInt32(txtBillNumber.Text),
                                 strEmailTo, strEmailCC, "S", "");
                        }
                    }
                } ///if pdf count mismatch 
               
            }
           
            if (dsEmail.Tables[0].Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Tranche Details not found");
                return;
            }
            if (strExceptionEmail.ToString() != "" || strFileSizeException.ToString() != "" || strPDFCountException.ToString() != "")
            {
                //if (strExceptionEmail.ToString() != "" && strFileSizeException.ToString() != "")
                //{
                //    Utility.FunShowAlertMsg(this.Page, strExceptionEmail.ToString() + " . \\n" + strFileSizeException.ToString());
                //    return;
                //}

                Utility.FunShowAlertMsg(this.Page, strPDFCountException.ToString() + " \\n" + strExceptionEmail.ToString() + " \\n" + strFileSizeException.ToString());
                return;

            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Mail Sent Successfully");
                return;
            }
        }
        //Sending Mails for each Vendor Id End
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Send Mail." + objException.Message);
        }

    }

    private void FunPubCreateBillEmailStatus(string strGroupName, string strCustomer_ID,
        string strGroupRSDetails, int intBilling_ID, string strEmailTo, string strEmailCC,string strStatus, string strErrMsg)
    {
        LoanAdminMgtServicesReference.LoanAdminMgtServicesClient objServiceClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();

        //        ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient objServiceClient = new ClnReceivableMgtServicesReference.ClnReceivableMgtServicesClient();
        BillingEntity objBillingEntity = new BillingEntity();
        try
        {

            objBillingEntity.strGroupName = strGroupName;
            objBillingEntity.strCustomer_ID = Convert.ToInt32(strCustomer_ID);
            objBillingEntity.XMLGroupRSDetails = strGroupRSDetails;
            objBillingEntity.intBilling_ID = intBilling_ID;
            objBillingEntity.strEmailTo = strEmailTo;
            objBillingEntity.strEmailCC = strEmailCC;
            objBillingEntity.strStatus = strStatus;
            objBillingEntity.strErrMsg = strErrMsg;
            objBillingEntity.intUserId = intUserID;

            string strJournalMessage = "";
            intErrorCode = objServiceClient.FunPubCreateBillEmailStatus(out strJournalMessage, objBillingEntity);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            objServiceClient.Close();
            objBillingEntity = null;
        }
    }

    private void GetPDFFiles_Email(string Type, List<string> arrGroupRSDet)
    {
        try
        {
            string name;
            int nCnt = 0;
            int nDigi_Flag = 0;
            dtaccounts = (DataTable)ViewState["dtaccounts"];
            if (dtaccounts != null)
            {
                //using (StreamWriter writer = new StreamWriter("D:\\S3G_OPC\\Err_Log.txt",true))
                //{
                //    writer.WriteLine("Check");
                //}

                DataTable dtprint;
                string panum;
                dtprint = new DataTable();
                dtprint.Columns.Add("tranche_name");
                if (dtaccounts.Rows.Count > 0)
                {
                    foreach (GridViewRow gvrow in grvRental.Rows)
                    {
                        CheckBox chkSelectAccount = (CheckBox)gvrow.Cells[0].FindControl("chkSelectRS");
                        Label lblRS_Number = (Label)gvrow.Cells[0].FindControl("lblRS_Number");
                        Label lblInvoiceno = (Label)gvrow.Cells[0].FindControl("lblInvoiceno");
                        Label lblInvoiceNoamf = (Label)gvrow.Cells[0].FindControl("lblInvoiceNoamf");
                        Label lblStatewiseBilling = (Label)gvrow.Cells[0].FindControl("lblStatewiseBilling");
                        Label lblDigi_Flag = (Label)gvrow.Cells[0].FindControl("lblDigi_Flag");
                        //using (StreamWriter writer1 = new StreamWriter("D:\\S3G_OPC\\Err_Log.txt",true))
                        //{
                        //    writer1.WriteLine("Check 1");
                        //}
                        if ((chkSelectAccount.Checked) && (arrGroupRSDet.Contains(lblRS_Number.Text)))
                        {
                            
                            nCnt += 1;
                            DataRow dr = dtprint.NewRow();
                            if (intBillingId >= 174 && lblStatewiseBilling.Text == "1" && Type == "Rental")
                                panum = lblInvoiceno.Text.Replace("/", "-").ToString();
                            else if (intBillingId >= 174 && lblStatewiseBilling.Text == "1" && Type == "AMF")
                                panum = lblInvoiceNoamf.Text.Replace("/", "-").ToString();
                            else
                                panum = lblRS_Number.Text.ToString();
                            if (lblDigi_Flag.Text == "1")
                                nDigi_Flag = 1;
                            if (dtprint.Rows.Count > 0)
                            {
                                DataRow[] drrow = dtprint.Select("tranche_name = '" + panum + "'");
                                if (drrow.Length.ToString() == "0")
                                {
                                    dr["tranche_name"] = panum;
                                    dtprint.Rows.Add(dr);
                                    dtprint.AcceptChanges();
                                }
                            }
                            else
                            {
                                dr["tranche_name"] = panum;
                                dtprint.Rows.Add(dr);
                                dtprint.AcceptChanges();
                            }
                        }
                    }
                    if (nCnt == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atlease one account to print");
                        return;
                    }
                    ViewState["dtprint"] = dtprint;
                }
                DataTable dtN = (DataTable)ViewState["dtprint"];
                string strnewfile = ViewState["strnewFile"].ToString();
                strnewfile += "\\" + Type;

                List<String> list = new List<String>();

                foreach (DataRow drow in dtN.Rows)
                {
                    //if (ddlLocation.SelectedValue != "0")
                    //{

                    //string[] str = new string[50];
                    //string[] str = Directory.GetFiles(strnewfile, lblInvoiceno.Text.Replace("/", "-").ToString());

                    //if (lblStatewiseBilling.Text == "1" && Type == "Rental")
                    //    str = Directory.GetFiles(strnewfile, lblInvoiceno.Text.Replace("/", "-").ToString());
                    //else if (lblStatewiseBilling.Text == "1" && Type == "AMF")
                    //    str = Directory.GetFiles(strnewfile, lblInvoiceNoamf.Text.Replace("/", "-").ToString());
                    //else
                    //       string[] str = Directory.GetFiles(strnewfile, drow[0].ToString());

                    //    for (int i = 0; i <= str.Length - 1; i++)
                    //    {
                    //        list.Add(str[i].ToString());
                    //    }
                    //}
                    //else
                    //{

                    string[] str = Directory.GetFiles(strnewfile, drow[0].ToString() + "*");

                    if (ddlLocation.SelectedValue != "0")
                    {
                        if (intBillingId <= 174)
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + ".pdf");
                        else
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + ".pdf");
                    }
                    else
                    {

                        if (intBillingId <= 174)
                            str = Directory.GetFiles(strnewfile, "*" + drow[0].ToString() + "*");
                        else
                            str = Directory.GetFiles(strnewfile, drow[0].ToString() + "*");
                    }

                    for (int i = 0; i <= str.Length - 1; i++)
                    {
                        list.Add(str[i].ToString());
                    }

                    //}
                    string[] GetAllFiles = list.ToArray();
                    if (Convert.ToInt32(GetAllFiles.Length.ToString()) > 0)
                    {
                        CombineMultiplePDF_Email(GetAllFiles, strnewfile, nDigi_Flag, "RIGHT");
                    }
                    //else
                    //{
                    //    Utility.FunShowAlertMsg(this, "Please upload IRN details to generate the invoice.");
                    //    return;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLog.CustomErrorRoutine(ex, "HANDhELD", "WINSERVICE");
            throw ex;
        }
    }

    public void CombineMultiplePDF_Email(string[] fileNames, string outFilePath, int Digi_Flag, String DigiPosition)
    {

        if (Digi_Flag == 1)
        {
            string InFile = outFilePath + "/Combine.pdf";

            outFilePath = outFilePath + "/" + intUserID.ToString() + "/" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

            if (!System.IO.Directory.Exists(outFilePath + "/Signed"))
            {
                System.IO.Directory.CreateDirectory(outFilePath + "/Signed");
            }

            string filePath = outFilePath + "/Signed.zip";

            //before creation of compressed folder,deleting it if exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();

            foreach (string fileName in fileNames)
            {

                string SignedFile = outFilePath + "/Signed/" + Path.GetFileName(fileName);

                ObjPDFSign.DigiPDFSign(fileName, SignedFile, "RIGHT");
            }

            if (!File.Exists(filePath))
            {
                //creating a zip file from one folder to another folder
                ZipFile.CreateFromDirectory(outFilePath + "/Signed", filePath);

                //Delete The excel file which is created

                string[] str = Directory.GetFiles(outFilePath + "/Signed");
                foreach (string fileName in str)
                {
                    File.Delete(fileName);
                }

                if (Directory.Exists(outFilePath + "/Signed"))
                {
                    Directory.Delete(outFilePath + "/Signed");
                }
            }

            ViewState["FilePath_Email_Attach"] = filePath;
            ViewState["FilePath_PDF_Count"] = fileNames.Length;

            ////FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(filePath, false, 0);
            ////string strScipt1 = "";

            ////strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + filePath.Replace("\\", "/") +
            ////    "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "ZIP", strScipt1, true);

        }
        else
        {
            // step 1: creation of a document-object
            Document document = new Document();

            string InFile = outFilePath + "/Combine.pdf";
            string SignedFile = outFilePath + "/Signed.pdf";

            // step 2: we create a writer that listens to the document
            PdfCopy writer = new PdfCopy(document, new FileStream(InFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }

            // step 3: we open the document
            document.Open();

            foreach (string fileName in fileNames)
            {
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(fileName);
                reader.ConsolidateNamedDestinations();

                // step 4: we add content
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

            // step 5: we close the document and writer
            writer.Close();
            document.Close();

            S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
            if (Digi_Flag == 1)
                ObjPDFSign.DigiPDFSign(InFile, SignedFile, "RIGHT");
            else
                SignedFile = InFile;

            ViewState["FilePath_Email_Attach"] = SignedFile;
            ////FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(SignedFile, false, 0);
            ////string strScipt1 = "";
            ////if (SignedFile.Contains("/File.pdf"))
            ////{
            ////    strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + SignedFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            ////}
            ////else
            ////{
            ////    strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + SignedFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            ////}
            ////ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
        }
    }

    //opc038 end

}
