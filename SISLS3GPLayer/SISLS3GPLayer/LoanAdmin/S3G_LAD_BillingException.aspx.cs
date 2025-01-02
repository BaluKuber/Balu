using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
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

public partial class LoanAdmin_S3G_LAD_BillingException : ApplyThemeForProject
{
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> objProcedureParameter = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    public static LoanAdmin_S3G_LAD_BillingException obj_Page = null;
    public string strDateFormat = string.Empty;
    static string strPageName = "Billing Exception";
    int intBillingId;
    private string strDocPath = "";
    DataTable dsaccounts = new DataTable();
   // string strRedirectPage = "~/LoanAdmin/S3GLoanAdTransLander.aspx?Code=CLNBILL";
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        FunPrilLoadPage();

    }

    private void FunPrilLoadPage()
    {
        S3GSession ObjS3GSession = new S3GSession();
        try
        {


            txtMonthYear.Attributes.Add("readonly", "true");
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //txtStartDate.Attributes.Add("readonly", "true");
            //txtEndDate.Attributes.Add("readonly", "true");
           
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
           
         
            if (!IsPostBack)
            {
               

                
            }
        }
        catch (Exception ex)
        {
            S3GBusEntity.ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Billing Related Details");
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    protected void btnFetch_Click(object sender, EventArgs e)
    {
        try
        {
            objProcedureParameter = new Dictionary<string, string>();
            objProcedureParameter.Add("@CompanyId", intCompanyID.ToString());
            if ((Convert.ToInt32(ddlBranchList.SelectedValue.ToString()) > 0) && (ddlBranchList.SelectedText.ToString()!="") )
            objProcedureParameter.Add("@location_id", ddlBranchList.SelectedValue);
            objProcedureParameter.Add("@month", Convert.ToDateTime(txtMonthYear.Text.ToString()).ToString("yyyyMM"));

            dsaccounts = Utility.GetDefaultData("S3g_loanad_bill_Exception", objProcedureParameter);


            ViewState["dtaccounts"] = dsaccounts;
            pnlexp.Visible = true;
            if (dsaccounts.Rows.Count > 0)
            {
                gexp.DataSource = dsaccounts;
                gexp.DataBind();

                btnExport1.Enabled = true;
            }
            else
            {
                gexp.EmptyDataText = "No Records Found";
                gexp.DataBind();
                btnExport1.Enabled = false;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
           
        }

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
  

         
          

            if (gexp.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Exception.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gexp.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

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
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
      
        Procparam.Add("@Program_Id", "331");
        
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


}