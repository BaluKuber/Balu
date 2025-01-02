using ReportAccountsMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_S3GRptPOReport : ApplyThemeForProject
{
    #region Variable Declaration    

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intErrCount = 0;
    int intProgramId = 302;
    string strPageName = "PO Report";
    public string strDateFormat;
    ReportAccountsMgtServicesClient objSerClient;
    public static Reports_S3GRptPOReport obj_Page;
    PagingValues ObjPaging = new PagingValues();
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
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
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid(0);
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        FunPriLoadPage();        
    }

    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            #region Application Standard Date Format

            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            txtPODateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtPODateFrom.ClientID + "','" + strDateFormat + "',true,  false);");
            txtPODateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtPODateTo.ClientID + "','" + strDateFormat + "',true,  false);");

            #endregion

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            #region Paging Config

            ProPageNumRW = 1;
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

            if (!IsPostBack)
            {
                ddlLOB.Focus();
                FunPriLoadLob();
                ucCustomPaging.Visible = false;
                pnlPOReportDetails.Style.Add("display", "none");               
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = LOB;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count > 0)
            {
                if (ddlLOB.Items.Count == 2)
                    ddlLOB.SelectedIndex = 1;
                ddlLOB.Items.RemoveAt(0);
            }
        }

        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    private void FunPriBindGrid(int IS_EXPORT)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if ((ddlLesseeName.SelectedValue != "0")&&(ddlLesseeName.SelectedText!=""))
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            else
                Procparam.Add("@Cust_ID", Convert.ToString("0"));

            if ((ddlVendorName.SelectedValue != "0")&&(ddlVendorName.SelectedText!=""))
                Procparam.Add("@Entity_ID", Convert.ToString(ddlVendorName.SelectedValue));
            else
                Procparam.Add("@Entity_ID", Convert.ToString("0"));

            if ((ddlAssetCategory.SelectedValue != "0")&&(ddlAssetCategory.SelectedText != ""))
                Procparam.Add("@AC_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            else
                Procparam.Add("@AC_ID", Convert.ToString("0"));

            if (!String.IsNullOrEmpty(txtPODateFrom.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtPODateFrom.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtPODateTo.Text))
                Procparam.Add("@EndDate",Convert.ToDateTime(Utility.StringToDate(txtPODateTo.Text)).ToString("yyyy/MM/dd"));

            Procparam.Add("@PO_Status", ddlPOStatus.SelectedValue);

            Procparam.Add("@IS_EXPORT", IS_EXPORT.ToString());

            if ((ddlCreatedBy.SelectedValue != "0")&&(ddlCreatedBy.SelectedText!=""))
                Procparam.Add("@Created_By", Convert.ToString(ddlCreatedBy.SelectedValue));
            else
                Procparam.Add("@Created_By", Convert.ToString("0"));
            //Paging Properties set

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            //Paging Properties end

            //Paging Config            

            //This is to show grid header
            bool bIsNewRow = false;
            pnlPOReportDetails.Style.Add("display", "block");             
            grvPOReportDetails.BindGridView("S3G_RPT_Get_POReport_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPOReportDetails.Rows[0].Visible = false;
            }
            else
                btnExport.Visible = true;

            pnlPOReportDetails.Visible=ucCustomPaging.Visible=grvPOReportDetails.Visible= true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<AccountMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            //lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            //objTaxGuideClient.Close();
        }
    }


    private void CheckDate()
    {
        if (txtPODateFrom.Text != String.Empty && txtPODateTo.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtPODateFrom.Text, txtPODateTo.Text);
            if (intErrCount == -1)
            {
                txtPODateTo.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "PO Date To Should Be Greater Than PO Date From");
                txtPODateTo.Focus();
                return;
            }
        }
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnGo_Click(Object sender, EventArgs e)
    {
        FunPriBindGrid(0);        
    }

    protected void btnClear_Click(Object sender, EventArgs e)
    {
        ddlLesseeName.Clear();
        ddlVendorName.Clear();
        ddlAssetCategory.Clear();
        ddlCreatedBy.Clear();
        txtPODateFrom.Text = txtPODateTo.Text = String.Empty;
        grvPOReportDetails.Visible =ucCustomPaging.Visible=btnExport.Visible= false;
        pnlPOReportDetails.Style.Add("display", "none");               
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {
        try
        {
            //FunPriBindGrid(1);
            //FunProExport(grvPOReportDetails, "PO Report");
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());

            if ((ddlLesseeName.SelectedValue != "0") && (ddlLesseeName.SelectedText != ""))
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            else
                Procparam.Add("@Cust_ID", Convert.ToString("0"));

            if ((ddlVendorName.SelectedValue != "0") && (ddlVendorName.SelectedText != ""))
                Procparam.Add("@Entity_ID", Convert.ToString(ddlVendorName.SelectedValue));
            else
                Procparam.Add("@Entity_ID", Convert.ToString("0"));

            if ((ddlAssetCategory.SelectedValue != "0") && (ddlAssetCategory.SelectedText != ""))
                Procparam.Add("@AC_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            else
                Procparam.Add("@AC_ID", Convert.ToString("0"));

            if (!String.IsNullOrEmpty(txtPODateFrom.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtPODateFrom.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtPODateTo.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtPODateTo.Text)).ToString("yyyy/MM/dd"));

            Procparam.Add("@PO_Status", ddlPOStatus.SelectedValue);

            Procparam.Add("@IS_EXPORT", "1");

            if ((ddlCreatedBy.SelectedValue != "0") && (ddlCreatedBy.SelectedText != ""))
                Procparam.Add("@Created_By", Convert.ToString(ddlCreatedBy.SelectedValue));
            else
                Procparam.Add("@Created_By", Convert.ToString("0"));

            DataTable dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_Get_POReport_Paging", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 18px; font-weight: bold;");
            Grv.Attributes.Add("Style", "font-family: calibri; font-size: 18px;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;
            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=Purchase Order.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
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
        }
    }

    protected void FunProExport(GridView Grv, string FileName)
    {
        try
        {
            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=" + FileName + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
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
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        
    }

    #endregion    

    #region TEXT CHANGED EVENTS

    protected void txtPODateFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtPODateTo.Text = String.Empty;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }


    protected void txtPODateTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckDate();
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region FETCH METHODS FOR AUTO SUGGEST CONTROLS

    //Lessee Name 
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

    //Vendor Name 
    [System.Web.Services.WebMethod]
    public static string[] GetVendorNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetVendorName", dictparam));
        return suggetions.ToArray();
    }

    //User Name 
    [System.Web.Services.WebMethod]
    public static string[] GetUserNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_Sysad_GetUserName", dictparam));
        return suggetions.ToArray();
    }

    //Vendor Name 
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

    #endregion
}