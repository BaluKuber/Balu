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

public partial class Reports_S3GRptForm17Report : ApplyThemeForProject
{
    #region Variable Declaration

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intErrCount = 0;
    int intProgramId = 308;
    string strPageName = "Form 17 Report";
    public string strDateFormat;
    ReportAccountsMgtServicesClient objSerClient;
    public static Reports_S3GRptForm17Report obj_Page;
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
        FunPriBindGrid();
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
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");

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
                pnlForm17ReportDetails.Style.Add("display", "none");
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

    private void FunPriBindGrid()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));

            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Fund_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (!String.IsNullOrEmpty(txtStartDate.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));

            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@STATUS", Convert.ToString(ddlStatus.SelectedValue));
            else
                Procparam.Add("@STATUS", Convert.ToString("0"));
            Procparam.Add("@IsExport", "0");

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
            pnlForm17ReportDetails.Style.Add("display", "block");
            grvForm17ReportDetails.Visible = true;
            grvForm17ReportDetails.BindGridView("S3G_RPT_GetForm17DetailsReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvForm17ReportDetails.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            pnlForm17ReportDetails.Visible = ucCustomPaging.Visible = true;
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
        if (txtStartDate.Text != String.Empty && txtEndDate.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtStartDate.Text, txtEndDate.Text);
            if (intErrCount == -1)
            {
                txtEndDate.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "Closure Date To Should Be Greater Than Closure Date From");
                txtEndDate.Focus();
                return;
            }
        }
    }

    private Int32 FunPriTypeCast(string val)
    {
        try                                                         // casting - to use proper align       
        {
            Int32 tempint = Convert.ToInt32(Convert.ToDecimal(val));                   // Try int     
            return 1;
        }
        catch (Exception ex)
        {
            return 3;        // try String
        }
    }

    #endregion

    #region TEXT CHANGED EVENTS

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtEndDate.Text = String.Empty;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }


    protected void txtEndDate_TextChanged(object sender, EventArgs e)
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

    #region BUTTON EVENTS

    protected void btnGo_Click(Object sender, EventArgs e)
    {
        FunPriBindGrid();
    }

    protected void btnClear_Click(Object sender, EventArgs e)
    {
        ddlLesseeName.Clear();
        ddlFunderName.Clear();
        ddlStatus.ClearSelection();
        txtStartDate.Text = txtEndDate.Text = String.Empty;
        grvForm17ReportDetails.Visible = ucCustomPaging.Visible = btnExport.Visible = false;
        pnlForm17ReportDetails.Style.Add("display", "none");
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));

            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Fund_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (!String.IsNullOrEmpty(txtStartDate.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));
            Procparam.Add("@IsExport", "1");

            DataTable dtTable = new DataTable();
            dtTable = Utility.GetDefaultData("S3G_RPT_GetForm17DetailsReport", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                string attachment = "attachment; filename=CHG 4 Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "CHG-4 Report for the period  of " + txtStartDate.Text + " to " + txtEndDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlLesseeName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : --All--";
                else
                    row["Column1"] = "Lessee Name : " + ddlLesseeName.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlFunderName.SelectedValue == "0")
                    row["Column1"] = "Funder Name : --All--";
                else
                    row["Column1"] = "Funder Name : " + ddlFunderName.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlStatus.SelectedValue == "0")
                    row["Column1"] = "Status : --All--";
                else
                    row["Column1"] = "Status : " + ddlStatus.SelectedItem.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 13;
                grv1.Rows[1].Cells[0].ColumnSpan = 13;
                grv1.Rows[2].Cells[0].ColumnSpan = 13;
                grv1.Rows[3].Cells[0].ColumnSpan = 13;
                grv1.Rows[4].Cells[0].ColumnSpan = 13;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;

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

    #region GRID EVENTS

    protected void grvForm17ReportDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
        {
            for (int i_cellVal = 1; i_cellVal < e.Row.Cells.Count; i_cellVal++)
            {
                try
                {
                    // if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text) && e.Row.Cells[i_cellVal].Text.Contains("/"))
                    if (!string.IsNullOrEmpty(e.Row.Cells[i_cellVal].Text))
                    {
                        Int32 type = 0;       // 1 = int, 2 = datetime, 3 = string

                        type = FunPriTypeCast(e.Row.Cells[i_cellVal].Text);

                        // cell alignment
                        switch (type)
                        {
                            case 1:  // int - right to left
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Right;
                                break;
                            case 3:  // string - do nothing - left align(default)
                                e.Row.Cells[i_cellVal].HorizontalAlign = HorizontalAlign.Left;
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //continue;
                }
            }
        }
    }

    #endregion

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

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

    //Funder Name 
    [System.Web.Services.WebMethod]
    public static string[] GetFunderNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetFunderName", dictparam));
        return suggetions.ToArray();
    }


    //RS NO 
    [System.Web.Services.WebMethod]
    public static string[] GetRSNoDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetRSNO", dictparam));
        return suggetions.ToArray();
    }

    #endregion
}