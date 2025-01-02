#region NAMESPACES

using AjaxControlToolkit;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

#endregion

public partial class LoanAdmin_S3G_Loanad_InterimBilling : ApplyThemeForProject
{
    # region INITILIZATION AND DELARATION OF VARIABLES

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    UserInfo ObjUserInfo = null;

    int intErrCode = 0;
    int intErrCount = 0;
    int intWBTID = 0;
    int intUserId = 0;
    int intCompanyID = 0;

    string strDateFormat = string.Empty;
    string strMode = string.Empty;
    string PA_SA_Ref_ID = string.Empty;
    string Invoice_ID = string.Empty;
    int strNote_id;
    Dictionary<string, string> dictparam;

    DataTable dtInterim;
    DataTable dtData;
    DataSet dsInterim;
    public static LoanAdmin_S3G_Loanad_InterimBilling obj_Page;
    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjInterimClient; 
    LoanAdminAccMgtServices.S3g_LAD_InterimDataTable objInterimeDatatable;
    LoanAdminAccMgtServices.S3g_LAD_InterimRow objInterimRow;
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode ObjSerMode = SerializationMode.Binary;

    string strRedirectPage = "~/LoanAdmin/S3G_Loanad_InterimBilling_View.aspx?Code=INTB";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3G_Loanad_InterimBilling.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3G_Loanad_InterimBilling_View.aspx?Code=INTB';";
    //ReportDocument rpd = new ReportDocument();

    
    #endregion
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

    #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
     {
        obj_Page = this;
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        
        #region Paging Config

       

        #endregion

        txtInterimDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInterimDate.ClientID + "','" + strDateFormat + "',null,null);");
        txtDueDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDueDate.ClientID + "','" + strDateFormat + "',null,null);");

        CalExtenderDueDate.Format = CalendarExtender1.Format = strDateFormat;
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
           
            if (strMode == "Q")                //Query Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                FunPriLoadInterimDtls();
            }
            else                                   //Create Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                txtInterimDate.Text = DateTime.Now.ToString(strDateFormat);
            }
            FunPriEnableDisableControls();
            
        }
    }

    #endregion


    protected void btnGo_click(object sender, EventArgs e)
    {
        try
        {
            FunPriloadgrid();
        }
        catch (Exception ex)
        {
            throw ex;

        }

    }

    protected void btnPrintRental_Click(object sender, EventArgs e)
    {
        var outputStream = new MemoryStream();
        try
        {
            dtInterim = (DataTable)ViewState["dtInterim"];
            if (dtInterim.Compute("sum(Interim_rent1)", "").ToString().StartsWith("0") || dtInterim.Compute("sum(Interim_rent1)", "").ToString() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "No records Found");
                return;
            }

            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Interim_ID", Convert.ToString(strNote_id));
            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserId));
            dictparam.Add("@Option", "1");
            dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);
            if (dsInterim.Tables[0].Rows.Count > 0)
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();

                //rpd.Load(Server.MapPath("BillReg_Inv_Cov.rpt"));

                //rpd.SetDataSource(dsInterim.Tables[2]);
                //rpd.Subreports["BillReg_Inv.rpt"].SetDataSource(dsInterim.Tables[0]);
                //rpd.Subreports["BillRegInv_Sub"].SetDataSource(dsInterim.Tables[1]);

                string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

                string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

                if (!(System.IO.Directory.Exists(strFolder)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strFolder);
                }

                //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] ar = new byte[(int)fs.Length];
                fs.Read(ar, 0, (int)fs.Length);
                fs.Close();

                var pdfReader = new PdfReader(strFileName);
                var pdfStamper = new PdfStamper(pdfReader, outputStream);
                var writer = pdfStamper.Writer;
                pdfStamper.Close();
                var content = outputStream.ToArray();
                outputStream.Close();
                Response.AppendHeader("content-disposition", "attachment;filename=" + dsInterim.Tables[0].Rows[0]["tranche_name"].ToString() + ".pdf");
                Response.ContentType = "application/octectstream";
                Response.BinaryWrite(content);
                Response.End();
                outputStream.Close();
                outputStream.Dispose();

                //strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
        finally
        {
            outputStream.Close();
            outputStream.Dispose(); //Upto This
        }
    }

    protected void btnPrintAMF_Click(object sender, EventArgs e)
    {
        var outputStream = new MemoryStream();
        dtInterim = (DataTable)ViewState["dtInterim"];
        decimal decAMF;
        decAMF = Convert.ToDecimal(dtInterim.Compute("sum(Interim_rent_amf1)", "").ToString());

        if (decAMF== 0)
        {
            Utility.FunShowAlertMsg(this.Page, "No records Found");
            return;
        }
            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Interim_ID", Convert.ToString(strNote_id));
            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserId));
            dictparam.Add("@Option", "2");
            dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETPrintInterim", dictparam);
            if (dsInterim.Tables[0].Rows.Count > 0)
            {
                Guid objGuid;
                objGuid = Guid.NewGuid();
                //ReportDocument rpd = new ReportDocument();

                //rpd.Load(Server.MapPath("BillReg_AMF_Cov.rpt"));


                //rpd.SetDataSource(dsInterim.Tables[2]);
                //rpd.Subreports["BillReg_AMF.rpt"].SetDataSource(dsInterim.Tables[0]);
                //rpd.Subreports["BillRegAMF_Sub"].SetDataSource(dsInterim.Tables[1]);


                string strFileName = Server.MapPath(".") + "\\PDF Files\\PO\\" + objGuid.ToString() + ".pdf";

                string strFolder = Server.MapPath(".") + "\\PDF Files\\PO";

                if (!(System.IO.Directory.Exists(strFolder)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strFolder);

                }

                //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);

                System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] ar = new byte[(int)fs.Length];
                fs.Read(ar, 0, (int)fs.Length);
                fs.Close();

                var pdfReader = new PdfReader(strFileName);
                var pdfStamper = new PdfStamper(pdfReader, outputStream);
                var writer = pdfStamper.Writer;
                pdfStamper.Close();
                var content = outputStream.ToArray();
                outputStream.Close();
                Response.AppendHeader("content-disposition", "attachment;filename=" + dsInterim.Tables[0].Rows[0]["tranche_name"].ToString() + ".pdf");
                Response.ContentType = "application/octectstream";
                Response.BinaryWrite(content);
                Response.End();
                outputStream.Close();
                outputStream.Dispose();

                //strScipt = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strFileName.Replace(@"\", "/") + "&qsNeedPrint=yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        

    }

    private void FunPriLoadInterimDtls()
    {
        try
        {
         
            
            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Interim_ID", Convert.ToString(strNote_id));
            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserId));

           dsInterim = Utility.GetDataset("S3G_FUNDMGT_GETInterim", dictparam);
           if (dsInterim.Tables[0].Rows.Count > 0)
           {
               txtDocno.Text = dsInterim.Tables[0].Rows[0]["Interim_No"].ToString();
               txtInterimDate.Text = dsInterim.Tables[0].Rows[0]["Interim_Date"].ToString();
               txtDueDate.Text = dsInterim.Tables[0].Rows[0]["Due_Date"].ToString();
               ddlLesseeName.SelectedValue = dsInterim.Tables[0].Rows[0]["Customer_id"].ToString();
               ddlTranche.SelectedValue = dsInterim.Tables[0].Rows[0]["Tranche_id"].ToString();
               ddlLesseeName.SelectedText = dsInterim.Tables[0].Rows[0]["Customer_name"].ToString();
               ddlTranche.SelectedText = dsInterim.Tables[0].Rows[0]["Tranche_name"].ToString();
           }
           dtInterim = dsInterim.Tables[1];
           if (dsInterim.Tables[1].Rows.Count > 0)
           {
               pnlAssetDet.Visible = true;
               divinterim.Style.Add("display", "block");
               gvInterim.DataSource = dtInterim;
               gvInterim.DataBind();
               ViewState["dtInterim"] = dtInterim;
               
           }
           
          
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
                txtInterimDate.ReadOnly = txtDueDate.ReadOnly = true;
                txtDocno.ReadOnly = true;
                CalendarExtender1.Enabled = false;
                ddlTranche.ReadOnly = true;
                ddlLesseeName.ReadOnly = true;
                ddlRSNO.ReadOnly = true;
                btnGo.Enabled = false;
                btnSave.Enabled = false;
                gvInterim.Columns[11].Visible = false;
                btnClear.Enabled = false;
                btnPrintAMF.Enabled = true;
                btnPrintRental.Enabled = true;
            }
            else
            {
                btnPrintAMF.Enabled = false;
                btnPrintRental.Enabled = false;
            }
           
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriloadgrid()
    {
        try
        {
         
        
            dictparam = new Dictionary<string, string>();
           
            dictparam.Add("@Company_ID", intCompanyID.ToString());
            dictparam.Add("@User_ID", intUserId.ToString());
            if(Convert.ToInt32(ddlLesseeName.SelectedValue)>0)
                dictparam.Add("@customer_id", ddlLesseeName.SelectedValue);
            if (Convert.ToInt32(ddlTranche.SelectedValue) > 0)
                dictparam.Add("@funder_id", ddlTranche.SelectedValue);
            if (Convert.ToInt32(ddlRSNO.SelectedValue) > 0)
                dictparam.Add("@pasa_id", ddlRSNO.SelectedValue);
            dictparam.Add("@Interim_Date", Utility.StringToDate(txtInterimDate.Text.ToString()).ToString());
            dtInterim = Utility.GetDefaultData("S3G_Interim_Interest", dictparam);
            if(dtInterim.Rows.Count>0)
            {
            pnlAssetDet.Visible = true;
            divinterim.Style.Add("display", "block");
            gvInterim.DataSource = dtInterim;
            gvInterim.DataBind();
            ViewState["dtInterim"] = dtInterim;
            btnSave.Enabled = true;
            }
            else
            { 
                pnlAssetDet.Visible = true;
            divinterim.Style.Add("display", "block");
            gvInterim.EmptyDataText = "No Records Found";
            gvInterim.DataBind();
            ViewState["dtInterim"] = dtInterim;
            btnSave.Enabled = false;

            }

          
        }
        catch (Exception ex)
        {

            throw ex;
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

      protected void gvInterim_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //code added by vinodha for UAT Obs
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvInterim.ClientID + "',this,'chkSelected');");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelected");
                chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + gvInterim.ClientID + "','chkAll','chkSelected');");
            }
            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex);
        }
    }

    private void FunpriClear()
    {
        try
        {
            ddlLesseeName.SelectedValue = "0";
            ddlLesseeName.SelectedText = "--Select--";
            ddlTranche.SelectedValue = "0";
            ddlTranche.SelectedText = "--Select--";
            ddlRSNO.SelectedValue = "0";
            ddlRSNO.SelectedText = "--Select--";
            txtDocno.Text = txtDueDate.Text = "";
            pnlAssetDet.Visible = false;

            ViewState["dtInterim"] = null;
          
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objInterimeDatatable = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimDataTable();
            objInterimRow = objInterimeDatatable.NewS3g_LAD_InterimRow();
            dtInterim = (DataTable)ViewState["dtInterim"];
            foreach (GridViewRow gvrow in gvInterim.Rows)
            {
                CheckBox chkSelected = (CheckBox)gvrow.FindControl("chkSelected") as CheckBox;
                Label lblPANum = (Label)gvrow.FindControl("lblPANum") as Label;
                DataRow[] dr = dtInterim.Select("panum='" + lblPANum.Text + "'");
                DataTable dt = dr.CopyToDataTable();
                if (chkSelected.Checked)
                {
                    
                   // dt.Columns.Add("Status");
                    if (dt.Rows.Count > 0)
                    {
                        dr[0]["status"] = "1";
                        dtInterim.AcceptChanges();
                    }
                   
                }
             

               
            }

            //foreach(DataRow drin in  dtInterim.Rows)
            //{
            //    if(drin["status"].ToString()=="0")
            //       drin.Delete();
            //    dtInterim.AcceptChanges();
            //}
            if (dtInterim.Rows.Count <= 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Account Number");
                return;
            }
            ViewState["dtInterim"] = dtInterim;
           
            objInterimRow.Company_ID = intCompanyID.ToString();
            objInterimRow.user_id = intUserId.ToString();
            objInterimRow.customer_id = ddlLesseeName.SelectedValue.ToString();
            objInterimRow.Tranche_id = ddlTranche.SelectedValue.ToString();
            objInterimRow.Interim_Date = Utility.StringToDate(txtInterimDate.Text.ToString());
            objInterimRow.Due_Date = Utility.StringToDate(txtDueDate.Text.ToString());
            objInterimRow.Interim_id = "0";
            dtInterim = (DataTable)ViewState["dtInterim"];
            objInterimRow.Xml_Interim = dtInterim.FunPubFormXml();
    
         

            objInterimeDatatable.AddS3g_LAD_InterimRow(objInterimRow);

            ObjInterimClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            string strNoteCode, strErrorMsg = string.Empty;
            Int32 intInterim = 0;
            string StrInterimNumber = string.Empty;
            int iErrorCode = ObjInterimClient.FunPubCreateOrModifyInterim(out intInterim, out strErrorMsg, out StrInterimNumber,ObjSerMode, ClsPubSerialize.Serialize(objInterimeDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnSave.Enabled = false;
                    
                        strAlert = "Interim Billing " + StrInterimNumber + "Created successfully";
                        strAlert += @"\n\nWould you like to Create one more Interim Billing?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                   
                    
                    break;
                case 5:

                    Utility.FunShowAlertMsg(this.Page, "Document Number Not Defined");
                    return;
                    break;

                case 4:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;

                case 3:
                    Utility.FunShowAlertMsg(this, "CashFlow Master Not Defined");
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
            Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

      [System.Web.Services.WebMethod]
    public static string[] GetLesseeNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", dictparam));
        return suggetions.ToArray();
    }

   

   

    //RS NO Details
    [System.Web.Services.WebMethod]
    public static string[] GetRSNODetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@program_id", "323");
        if (Convert.ToInt32(obj_Page.ddlLesseeName.SelectedValue.ToString()) > 0)
            dictparam.Add("@cust_id", obj_Page.ddlLesseeName.SelectedValue.ToString());
        if (Convert.ToInt32(obj_Page.ddlTranche.SelectedValue.ToString()) > 0)
            dictparam.Add("@tranche_id", obj_Page.ddlTranche.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetRSNO", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        if(Convert.ToInt32(obj_Page.ddlLesseeName.SelectedValue.ToString())>0)
            dictparam.Add("@customer_id", obj_Page.ddlLesseeName.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTrancheAGT", dictparam));
        return suggetions.ToArray();
    }
                   
 
}