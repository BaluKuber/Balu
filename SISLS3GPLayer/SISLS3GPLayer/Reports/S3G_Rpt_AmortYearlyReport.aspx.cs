using System;
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

public partial class Reports_S3G_Rpt_AmortYearlyReport : ApplyThemeForProject
{
    public static Reports_S3G_Rpt_AmortYearlyReport obj_Page;
    int intUserId;
    int intCompanyId;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    string strDateFormat = string.Empty;
    DataSet dsAmort = new DataSet();
    public string cutoff_month;
    public string cutoff_month1;
    public static string strTableName = string.Empty;
    public static string strID_Column = string.Empty;
    public static string strID_Column_Value = string.Empty;
    public static string strColumn_Name_Display = string.Empty;
    public static string strColumn_Name_Sort = string.Empty;
    public static int first = 1;
    public static int last = 100;
    Dictionary<string, string> Procparam;
    
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
        try
        {
            ProPageNumRW = intPageNum;
            ProPageSizeRW = intPageSize;
            FunMonthSearch1();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    //S3G_RPT_GETAMORTYEARLY
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        try
        {
            ProPageNumRW = 1;
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
            {
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            }
            else
            {
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
            }
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            CalendarExtenderPostingDateFrom.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtenderPostingDateTo.Format = strDateFormat;                       // assigning the first textbox with the start date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDate.Attributes.Add("readonly", "readonly");
            //txtEndDate.Attributes.Add("readonly", "readonly");
            txtPostingDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateFrom.ClientID + "','" + strDateFormat + "',false,  false);");
            txtPostingDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtPostingDateTo.ClientID + "','" + strDateFormat + "',false,  false);");
            //txtPostingDateFrom.Attributes.Add("readonly", "readonly");
            //txtPostingDateTo.Attributes.Add("readonly", "readonly");
            PageAssignValue Obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = Obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;
            if (!IsPostBack)
            {
                FunPriLoadStatus();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriLoadStatus()
    {
        Procparam = new Dictionary<string, string>();
        try
        {
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@Program_ID", "325");
            ddlStatus.BindMemoDataTable("S3G_RPT_GetAccountStatus", Procparam, new string[] { "ID", "DESCR" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void txtPostingDateFrom_TextChanged(object sender, EventArgs e)
    {
        if ((!(string.IsNullOrEmpty(txtPostingDateFrom.Text))) &&
           (!(string.IsNullOrEmpty(txtPostingDateTo.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtPostingDateFrom.Text) > Utility.StringToDate(txtPostingDateTo.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "From Date should be lesser than or equal to the To Date");
                txtPostingDateTo.Text = "";
                return;
            }
        }
    }

    protected void txtPostingDateTo_TextChanged(object sender, EventArgs e)
    {
        if ((!(string.IsNullOrEmpty(txtPostingDateFrom.Text))) &&
           (!(string.IsNullOrEmpty(txtPostingDateTo.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtPostingDateFrom.Text) > Utility.StringToDate(txtPostingDateTo.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to the From Date");
                txtPostingDateTo.Text = "";
                return;
            }
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    // Export Region -- Start --//
    #region Export
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FunMonthSearch();
        try
        {
            StringBuilder shtml = new StringBuilder();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    private void FunMonthSearch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());

            if (ddlCustName.SelectedValue == "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CUSTOMER_ID", "-1");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText == "")
                Procparam.Add("@CUSTOMER_ID", "0");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CUSTOMER_ID", ddlCustName.SelectedValue.ToString());


            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@STATUS_ID", ddlStatus.SelectedValue);

            if (ddlFunder.SelectedValue == "0" && ddlFunder.SelectedText != "")
                Procparam.Add("@FUNDER_ID", "-1");
            else if (ddlFunder.SelectedValue != "0" && ddlFunder.SelectedText == "")
                Procparam.Add("@FUNDER_ID", "0");
            else if (ddlFunder.SelectedValue != "0" && ddlFunder.SelectedText != "")
                Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue.ToString());


            if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@TRANCHE_ID", "-1");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText == "")
                Procparam.Add("@TRANCHE_ID", "0");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue.ToString());

            //if (ddlCustName.SelectedValue != "0")
            //    Procparam.Add("@CUSTOMER_ID", ddlCustName.SelectedValue);
            //if (ddlStatus.SelectedValue != "0")
            //    Procparam.Add("@STATUS_ID", ddlStatus.SelectedValue);
            //if (ddlFunder.SelectedValue != "0")
            //    Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue);
            //if (ddlTrancheNo.SelectedValue != "0")
            //    Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue);

            Procparam.Add("@TRANCHE_WISE", ddlTranchegrp.SelectedValue);
            Procparam.Add("@STARTDATE", Utility.StringToDate(txtPostingDateFrom.Text).ToString());
            Procparam.Add("@ENDDATE", Utility.StringToDate(txtPostingDateTo.Text).ToString());

            //Procparam.Add("@COMPANY_ID", "1");
            //if (ddlCustName.SelectedValue != "0")
            //    Procparam.Add("@CUSTOMER_ID", "37");
            //if (ddlStatus.SelectedValue != "0")
            //    Procparam.Add("@STATUS_ID", "3");
            //if (ddlFunder.SelectedValue != "0")
            //    Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue);
            //if (ddlTrancheNo.SelectedValue != "0")
            //    Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue);
            //Procparam.Add("@TRANCHE_WISE", "2");
            //Procparam.Add("@STARTDATE", "4/1/2014 12:00:00 AM");
            //Procparam.Add("@ENDDATE", "3/31/2015 12:00:00 AM");
            //Procparam.Add("@FROMMONTH", "201404");
            //Procparam.Add("@TOMONTH", "201503");

            dsAmort = Utility.GetDataset("S3G_RPT_GETAMORTYEARLY", Procparam);
            StringBuilder shtml = new StringBuilder();
            DataSet dset = new DataSet();
            if (dsAmort.Tables[2].Rows.Count > 0)
            {
                FunExcelExport(dsAmort);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records found");
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunExcelExport(DataSet dst)
    {
        try
        {
            StringBuilder shtml = new StringBuilder();
            shtml.Append(FunForExportExcel(dst));
            tdExportDtl.InnerHtml = shtml.ToString();
            string attachment = "attachment; filename=Amortisation_Yearly.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.xls";
            Response.Write(shtml.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            //    return null;
        }
    }

    /// <summary>
    /// To Get  Exception Report Excel 
    /// </summary>
    protected string FunForExportExcel(DataSet ds)
    {

        StringBuilder shtml = new StringBuilder();
        shtml.Append("<html><head>");
        shtml.Append("<style type='text/css'>");
        shtml.Append(".PageHeading{background-image:url('../../images/title_headerBG.jpg');font-family:calibri,Verdana;font-weight:bold;font-size:15px;color:Navy;width:99.5%; padding-left:3px;border-bottom:0px solid #788783;border-top:0px solid #788783;margin-bottom:2px;filter:glow(color=InactiveCaptionText,strength=50);}");
        shtml.Append(".stylePageHeading{background-image:url('../../images/title_headerBG.jpg');font-family:calibri,Verdana;font-weight:bold;font-size:13px;color:Navy;width:99.5%; padding-left:3px;border-bottom:0px solid #788783;border-top:0px solid #788783;margin-bottom:2px;filter:glow(color=InactiveCaptionText,strength=50);}");
        shtml.Append(".styleGridHeader{color:Navy;text-decoration:none;background-color:aliceblue;font-weight:bold;}");
        shtml.Append(".styleInfoLabel{font-family:calibri,Verdana;font-weight:normal;font-size:13px;color:Navy;text-decoration:none;background-color:White;}");
        shtml.Append("</style>");
        shtml.Append("</head>");
        shtml.Append("<Body>");
        shtml.Append("<table width='100%'>");

        shtml.Append("<tr>");
        shtml.Append("<td align='center' class='PageHeading' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + " scope='col'>");
        shtml.Append(ObjUserInfo.ProCompanyNameRW.ToString());
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td align='center' class='PageHeading' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + " scope='col'>");
        shtml.Append("Amortisation Report - Yearly");
        shtml.Append("</td>");
        shtml.Append("</tr>");


        shtml.Append("<tr>");
        shtml.Append("<td align='centre' class='PageHeading' colspan=2 scope='col'>");
        shtml.Append("From Date: " + txtPostingDateFrom.Text);
        shtml.Append("</td>");
        shtml.Append("<td class='PageHeading'>");
        shtml.Append("To Date: " + txtPostingDateTo.Text);
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td align='centre' class='PageHeading' colspan=2 scope='col'>");
        shtml.Append("Lessee Name: " + ddlCustName.SelectedText);
        shtml.Append("</td>");
        shtml.Append("<td class='PageHeading'>");
        shtml.Append("Status: " + ddlStatus.SelectedItem.ToString());
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td align='centre' class='PageHeading'  colspan=2 scope='col'>");
        shtml.Append("Funder Name: " + ddlFunder.SelectedText);
        shtml.Append("</td>");
        shtml.Append("<td class='PageHeading'>");
        shtml.Append("Tranche Number: " + ddlTrancheNo.SelectedText);
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td align='centre' class='PageHeading'  colspan=2 scope='col'>");
        shtml.Append("Tranche Wise grouping :" + ddlTranchegrp.SelectedItem.ToString());
        shtml.Append("</td>");
        shtml.Append("</tr>");

    
        shtml.Append("<tr>");
        shtml.Append("<td  class='stylePageHeading' align='left' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + " scope='col' >");
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td style='width:100%;' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + ">");
        shtml.Append(FunReturnTable(ds));
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("</table>");

        return shtml.ToString();

    }

    private string FunReturnTable(DataSet dsDetails)
    {
        StringBuilder shtml = new StringBuilder();
        shtml.Append(" <table class='styleGridView' cellspacing='0' cellpadding='1' rules='all' border='1'style='color: #003D9E; font-family: calibri;font-size: 13px; font-weight: normal; width: 100%; border-collapse: collapse;'>");
        //Group Header 
        shtml.Append("<tr>");
        for (int i = 0; i <= 11; i++)
        {
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");
        }

        for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
        {
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px'; colspan='6'>");
            shtml.Append(dsDetails.Tables[0].Rows[i][0].ToString() + " </td>");

        }

        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");
        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");

        shtml.Append("</tr>");

        //Sub Group Header 
        shtml.Append("<tr>");
        for (int i = 0; i <= 11; i++)
        {
            if (i == 10)
                shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Asset Cost </td>");
            else if (i == 11)
                shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>End of Term RV </td>");
            else
                shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");
        }

        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px'; colspan='2'></td>");

        for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
        {
            for (int k = 0; k < dsDetails.Tables[1].Rows.Count; k++)
            {
                shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px'; colspan='2'>");
                shtml.Append(dsDetails.Tables[1].Rows[k][0].ToString() + " </td>");
            }

        }

        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");
        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';></td>");

        shtml.Append("</tr>");

        shtml.Append("<tr>");
        for (int i = 0; i < dsDetails.Tables[2].Columns.Count; i++)
        {
            string strHeader = string.Empty;

            strHeader = dsDetails.Tables[2].Columns[i].Caption.ToString().Contains("!") == true ? dsDetails.Tables[2].Columns[i].Caption.ToString().Substring(dsDetails.Tables[2].Columns[i].Caption.ToString().IndexOf('!'), dsDetails.Tables[2].Columns[i].Caption.ToString().Length - dsDetails.Tables[2].Columns[i].Caption.ToString().IndexOf('!')).Replace("!", "") : dsDetails.Tables[2].Columns[i].Caption.ToString();

            if (i == 12 || i == 13)
            { }
            else
            {
                shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='Center' scope='col' border-width:'1px';>");
                shtml.Append(strHeader.Replace("_RNS_DINT", "Lease - Interest").Replace("_RNS_CPR", "Lease - Principal").Replace("_RS_DINT", "Lease - Interest").Replace("_RS_CPR", "Lease - Principal").Replace("_XBRW_DINT", "Borrowing - Interest").Replace("_XBRW_CPR", "Borrowing - Principal") + " </td>");
            }
        }


        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='Center' scope='col' border-width:'1px';>LFA Balance</td>");
        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='Center' scope='col' border-width:'1px';>Funder Balance</td>");

        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Lease - Principal </td>");
        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Lease - Interest</td>");
        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Lease - Principal </td>");
        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Lease - Interest</td>");
        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Borrowing - Principal </td>");
        //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='center' scope='col' border-width:'1px';>Borrowing - Interest</td>");

        shtml.Append("</tr>");
        for (int i = 0; i < dsDetails.Tables[2].Rows.Count; i++)
        {
            //decimal RNSLease_Principal = 0, RSLease_Principal = 0, BWLease_Principal = 0, RNSLease_Interest = 0, RSLease_Interest = 0, BWLease_Interest = 0,LFA=0,RV=0;
            shtml.Append("<tr>");
            for (int j = 0; j < dsDetails.Tables[2].Columns.Count; j++)
            {
                /*
                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_RNS_DINT"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        RNSLease_Interest = RNSLease_Interest + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }
                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_RNS_CPR"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        RNSLease_Principal = RNSLease_Principal + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }

                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_RS_CPR"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        RSLease_Principal = RSLease_Principal + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }
                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_RS_DINT"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        RSLease_Interest = RSLease_Interest + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }
                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_XBRW_CPR"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        BWLease_Principal = BWLease_Principal + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }
                if (dsDetails.Tables[2].Columns[j].ToString().Contains("_XBRW_DINT"))
                {
                    if (dsDetails.Tables[2].Rows[i][j].ToString() != "")
                        BWLease_Interest = BWLease_Interest + Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString());
                }
                */
                if (j == 12 || j == 13)
                {

                }
                else
                {
                    if (dsDetails.Tables[2].Rows[i][j].GetType() == Type.GetType("System.Int32") || dsDetails.Tables[2].Rows[i][j].GetType() == Type.GetType("System.Decimal"))
                    {
                        shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleInfoLabel' align='right' scope='col' >");
                        shtml.Append(Decimal.Round(Convert.ToDecimal(dsDetails.Tables[2].Rows[i][j].ToString()), 3) + " </td>");
                    }
                    else
                    {
                        shtml.Append("<td style='border: 1px solid #EFF4FF;mso-number-format:\"" + @"\@" + "\"" + "' class='styleInfoLabel' align='left' scope='col' >");
                        shtml.Append(dsDetails.Tables[2].Rows[i][j].ToString() + " </td>");
                    }
                }

            }

            for (int j = 12; j < 14; j++)
            {

                if (dsDetails.Tables[2].Rows[i][j].GetType() == Type.GetType("System.Int32") || dsDetails.Tables[2].Rows[i][j].GetType() == Type.GetType("System.Decimal"))
                {
                    shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleInfoLabel' align='right' scope='col' >");
                    shtml.Append(dsDetails.Tables[2].Rows[i][j].ToString() + " </td>");
                }

            }
            //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + LFA.ToString() + "</td>");
            //shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + RV.ToString() + "</td>");
          /*
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + RNSLease_Principal.ToString() + "</td>");
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + RNSLease_Interest.ToString() + "</td>");
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + RSLease_Principal.ToString() + " </td>");
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + RSLease_Interest.ToString() + "</td>");
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + BWLease_Principal.ToString() + " </td>");
            shtml.Append("<td style='border: 1px solid #EFF4FF;' class='styleGridHeader' align='right' scope='col' border-width:'1px';>" + BWLease_Interest.ToString() + "</td>");
            */
            shtml.Append("</tr>");
        }
        shtml.Append("</table>");

        return shtml.ToString();
    }
    #endregion
    // Export Region -- End --//

    // Search Region -- Start --//
    #region Search
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FunMonthSearch1();
        try
        {
            StringBuilder shtml = new StringBuilder();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunMonthSearch1()
    {
        try
        {
            //MMM-yyyy
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());


            if (ddlCustName.SelectedValue == "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CUSTOMER_ID", "-1");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText == "")
                Procparam.Add("@CUSTOMER_ID", "0");
            else if (ddlCustName.SelectedValue != "0" && ddlCustName.SelectedText != "")
                Procparam.Add("@CUSTOMER_ID", ddlCustName.SelectedValue.ToString());


            if (ddlStatus.SelectedValue != "0")
                Procparam.Add("@STATUS_ID", ddlStatus.SelectedValue);

            if (ddlFunder.SelectedValue == "0" && ddlFunder.SelectedText != "")
                Procparam.Add("@FUNDER_ID", "-1");
            else if (ddlFunder.SelectedValue != "0" && ddlFunder.SelectedText == "")
                Procparam.Add("@FUNDER_ID", "0");
            else if (ddlFunder.SelectedValue != "0" && ddlFunder.SelectedText != "")
                Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue.ToString());


            if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@TRANCHE_ID", "-1");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText == "")
                Procparam.Add("@TRANCHE_ID", "0");
            else if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != "")
                Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue.ToString());


            //if (ddlCustName.SelectedValue != "0")
            //    Procparam.Add("@CUSTOMER_ID", ddlCustName.SelectedValue);
            //if (ddlStatus.SelectedValue != "0")
            //    Procparam.Add("@STATUS_ID", ddlStatus.SelectedValue);
            //if (ddlFunder.SelectedValue != "0")
            //    Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue);
            //if (ddlTrancheNo.SelectedValue != "0")
            //    Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue);

            Procparam.Add("@TRANCHE_WISE", ddlTranchegrp.SelectedValue);
            Procparam.Add("@STARTDATE", Utility.StringToDate(txtPostingDateFrom.Text).ToString());
            Procparam.Add("@ENDDATE", Utility.StringToDate(txtPostingDateTo.Text).ToString());

            //Procparam.Add("@CP", ProPageNumRW.ToString());
            //Procparam.Add("@PS", ProPageSizeRW.ToString());
            //Procparam.Add("@TR", intTotalRecords.ToString());
            
            //Procparam.Add("@COMPANY_ID", "1");
            //if (ddlCustName.SelectedValue != "0")
            //    Procparam.Add("@CUSTOMER_ID", "37");
            //if (ddlStatus.SelectedValue != "0")
            //    Procparam.Add("@STATUS_ID", "3");
            //if (ddlFunder.SelectedValue != "0")
            //    Procparam.Add("@FUNDER_ID", ddlFunder.SelectedValue);
            //if (ddlTrancheNo.SelectedValue != "0")
            //    Procparam.Add("@TRANCHE_ID", ddlTrancheNo.SelectedValue);
            //Procparam.Add("@TRANCHE_WISE", "2");
            //Procparam.Add("@STARTDATE", "4/1/2014 12:00:00 AM");
            //Procparam.Add("@ENDDATE", "3/31/2015 12:00:00 AM");
            //Procparam.Add("@FROMMONTH", "201404");
            //Procparam.Add("@TOMONTH", "201503");

            dsAmort = Utility.GetDataset("S3G_RPT_GETAMORTYEARLY", Procparam);

            StringBuilder shtml = new StringBuilder();
            DataSet dset = new DataSet();
            if (dsAmort.Tables[2].Rows.Count > 0)
            {
                
                //pagetable.Visible = true;
                FunExcelExport1(dsAmort);
                tdExportDtl.Visible = true;
                //ucCustomPaging.Navigation(dsAmort.Tables[2].Rows.Count, ProPageNumRW, ProPageSizeRW);
                //ucCustomPaging.setPageSize(ProPageSizeRW);
                //ucCustomPaging.Visible = true;
                lblTotalRecords.Text = dsAmort.Tables[2].Rows.Count.ToString();
                ViewState["dsAmort"] = dsAmort;
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records found");
                tdExportDtl.Visible = false;
                //ucCustomPaging.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunExcelExport1(DataSet dst)
    {
        try
        {
            StringBuilder shtml = new StringBuilder();
            shtml.Append(FunForExportExcel1(dst));
            tdExportDtl.InnerHtml = shtml.ToString();
            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// To Get  Exception Report Excel 
    /// </summary>
    protected string FunForExportExcel1(DataSet ds)
    {

        StringBuilder shtml = new StringBuilder();
        shtml.Append("<html><head>");
        shtml.Append("<style type='text/css'>");
        shtml.Append(".PageHeading{background-image:url('../../images/title_headerBG.jpg');font-family:calibri,Verdana;font-weight:bold;font-size:15px;color:White;width:99%; padding-left:3px;border-bottom:0px solid #788783;border-top:0px solid #788783;margin-bottom:2px;filter:glow(color=InactiveCaptionText,strength=50);}");
        //shtml.Append(".stylePageHeading{background-image:url('../../images/title_headerBG.jpg');font-family:calibri,Verdana;font-weight:bold;font-size:13px;color:Navy;width:99.5%; padding-left:3px;border-bottom:0px solid #788783;border-top:0px solid #788783;margin-bottom:2px;filter:glow(color=InactiveCaptionText,strength=50);}");
        shtml.Append(".styleGridHeader{color:Navy;text-decoration:none;background-color:aliceblue;font-weight:bold;}");
        shtml.Append(".styleInfoLabel{font-family:calibri,Verdana;font-weight:normal;font-size:13px;color:Navy;text-decoration:none;background-color:White;}");
        shtml.Append("</style>");
        shtml.Append("</head>");
        shtml.Append("<Body>");
        shtml.Append("<table width='99%'>");
        shtml.Append("<tr>");
        shtml.Append("<td align='left' class='styleGridHeader' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + " scope='col'>");
        shtml.Append("Amortisation Report - Yearly");
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td  class='styleGridHeader' align='left' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + " scope='col' >");
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("<tr>");
        shtml.Append("<td style='width:99%;' colspan=" + Convert.ToSingle(ds.Tables[2].Columns.Count) + ">");
        shtml.Append(FunReturnTable1(ds));
        shtml.Append("</td>");
        shtml.Append("</tr>");

        shtml.Append("</table>");

        return shtml.ToString();

    }
    //#003D9E
    private string FunReturnTable1(DataSet dsDetails)
    {
        StringBuilder shtml = new StringBuilder();
        shtml.Append("<div style='overflow-x: scroll; width: 1300px;'>");
        shtml.Append(" <table class='styleGridView' cellspacing='0' cellpadding='1' rules='all' border='1' style='color: #003D9E; font-family: calibri;font-size: 13px; font-weight: normal; width: 100%; border-collapse: collapse;'>");
        //Group Header 

        shtml.Append("<tr  class='styleGridHeader' align='Center' scope='col' border-width:'1px';>");
        shtml.Append("  <td rowspan=3>  S.No </td> <td rowspan=3> Funder Name </td> <td rowspan=3>  Lessee Name </td>  <td rowspan=3>   End use Customer	 </td> ");
        shtml.Append("  <td rowspan=3>  Status </td>  <td rowspan=3> Schedule Sate  </td> <td rowspan=3> Billing State  </td> <td rowspan=3> Schedule No.  </td> <td rowspan=3> Tranche No.  </td>  <td rowspan=3> Note No. </td> ");
        shtml.Append("  <td rowspan=2> Asset Cost </td>  <td rowspan=2> End of Term  </td>  ");

        for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
            shtml.Append("<td  colspan='6'>" + dsDetails.Tables[0].Rows[i][0].ToString() + " </td>");

        shtml.Append("  <td rowspan=2 colspan=2> </td>  ");

        shtml.Append("</tr>");

        shtml.Append("<tr   class='styleGridHeader' align='Center' scope='col' border-width:'1px';>");

        for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
            for (int k = 0; k < dsDetails.Tables[1].Rows.Count; k++)
                shtml.Append("<td colspan='2'> " + dsDetails.Tables[1].Rows[k][0].ToString() + " </td>");

        shtml.Append("</tr>");

        shtml.Append(" <tr  class='styleGridHeader'>  <td align='Center'> LFA  </td> <td align='Center'> RV  </td> ");

        for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
        {
            shtml.Append("<td>  Lease - Principal  </td> <td>  Lease - Interest  </td> ");
            shtml.Append("<td>  Lease - Principal  </td> <td>  Lease - Interest  </td> ");
            shtml.Append(" <td>  Borrowing - Principal </td>  <td>  Borrowing - Interest </td> ");
        }

        shtml.Append(" <td align='Center'> LFA Balance </td> <td align='Center'> Funder Balance </td> ");
        shtml.Append("</tr>");
        
        for (int i = 0; i < dsDetails.Tables[2].Rows.Count; i++)
        {
            shtml.Append("<tr  class='styleInfoLabel' align='right' scope='col'>");
            for (int j = 0; j < dsDetails.Tables[2].Columns.Count; j++)
            {
                if (j == 12 || j == 13)
                {

                }
                else
                {
                    if (j <= 8)
                        shtml.Append("<td  align='left'>" + dsDetails.Tables[2].Rows[i][j].ToString() + " </td>");
                    else
                        shtml.Append("<td>" + dsDetails.Tables[2].Rows[i][j].ToString() + " </td>");
                }
            }

            for (int j = 0; j < dsDetails.Tables[2].Columns.Count; j++)
            {
                if (j == 12 || j == 13)
                {
                    shtml.Append("<td>" + dsDetails.Tables[2].Rows[i][j].ToString() + " </td>");
                }
            }

            shtml.Append("</tr>");
        }
         
        shtml.Append("</table>");
        shtml.Append("</div>");
        return shtml.ToString();
    }

    protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        int ig = 0;
        first = 1;
        last = 100;
        lblCurrentPage.Text = "1";
        //for (; ig < dsAmort.Tables[2].Rows.Count; ig++)
        //{
        //    HiddenField hfDeDupeRecords = (HiddenField)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("hfDeDupeRecords");
        //    CheckBox chkActiveDeDupe = (CheckBox)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("chkActiveDeDupe");
        //    if (chkActiveDeDupe.Checked == true)
        //    {
        //        strColumn_Name_Sort = hfDeDupeRecords.Value.ToString();

        //        if (strColumn_Name_Sort == "")
        //        {
        //            Utility.FunShowAlertMsg(this, "Select Any one of the Parameter to De Dupe");
        //            return;
        //        }
        //        else
        //        {
        //            string condition = "BETWEEN " + first + " AND " + last;
        //            txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        //            FunPageingNavig(strColumn_Name_Sort, condition);
        //            btnFirst.Enabled = false;
        //            btnPrevious.Enabled = false;
        //        }
        //    }
        //}
    }

    protected void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        int ig = 0;
        first = first - 100;
        last = last - 100;
        lblCurrentPage.Text = (Convert.ToInt32(lblCurrentPage.Text) - 1).ToString();
        //for (; ig < dsAmort.Tables[2].Rows.Count; ig++)
        //{
        //    HiddenField hfDeDupeRecords = (HiddenField)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("hfDeDupeRecords");
        //    CheckBox chkActiveDeDupe = (CheckBox)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("chkActiveDeDupe");
        //    if (chkActiveDeDupe.Checked == true)
        //    {
        //        strColumn_Name_Sort = hfDeDupeRecords.Value.ToString();

        //        if (strColumn_Name_Sort == "")
        //        {
        //            Utility.FunShowAlertMsg(this, "Select Any one of the Parameter to De Dupe");
        //            return;
        //        }
        //        else
        //        {
        //            string condition = "BETWEEN " + first + " AND " + last;
        //            txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        //            FunPageingNavig(strColumn_Name_Sort, condition);
        //            if (first == 1)
        //            {
        //                btnPrevious.Enabled = false;
        //                btnFirst.Enabled = false;
        //            }
        //        }
        //    }
        //}
    }

    protected void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        int temp = Convert.ToInt32(lblTotalRecords.Text);
        temp = Convert.ToInt32(Math.Round(temp / 100d, 0) * 100);
        int ig = 0;
        first = first + 100;
        last = last + 100;
        if (last >= temp)
        {
            first = temp - 100;
            last = temp;
        }

        lblCurrentPage.Text = (Convert.ToInt32(lblCurrentPage.Text) + 1).ToString();
        //for (; ig < dsAmort.Tables[2].Rows.Count; ig++)
        //{
        //    HiddenField hfDeDupeRecords = (HiddenField)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("hfDeDupeRecords");
        //    CheckBox chkActiveDeDupe = (CheckBox)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("chkActiveDeDupe");
        //    if (chkActiveDeDupe.Checked == true)
        //    {
        //        strColumn_Name_Sort = hfDeDupeRecords.Value.ToString();

        //        if (strColumn_Name_Sort == "")
        //        {
        //            Utility.FunShowAlertMsg(this, "Select Any one of the Parameter to De Dupe");
        //            return;
        //        }
        //        else
        //        {
        //            string condition = "BETWEEN " + first + " AND " + last;
        //            txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        //            FunPageingNavig(strColumn_Name_Sort, condition);
        //        }
        //    }
        //}
    }

    protected void btnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        int temp = Convert.ToInt32(lblTotalRecords.Text);
        temp = Convert.ToInt32(Math.Round(temp / 100d, 0) * 100);
        int ig = 0;
        first = temp - 100;
        last = temp;
        lblCurrentPage.Text = lblTotalPages.Text;
        //for (; ig < dsAmort.Tables[2].Rows.Count; ig++)
        //{
        //    //HiddenField hfDeDupeRecords = (HiddenField)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("hfDeDupeRecords");
        //    //CheckBox chkActiveDeDupe = (CheckBox)dsAmort.Tables[2].Rows[ig].Cells[0].FindControl("chkActiveDeDupe");
        //    if (chkActiveDeDupe.Checked == true)
        //    {
        //        strColumn_Name_Sort = hfDeDupeRecords.Value.ToString();

        //        if (strColumn_Name_Sort == "")
        //        {
        //            Utility.FunShowAlertMsg(this, "Select Any one of the Parameter to De Dupe");
        //            return;
        //        }
        //        else
        //        {
        //            string condition = "BETWEEN " + first + " AND " + last;
        //            txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        //            FunPageingNavig(strColumn_Name_Sort, condition);
        //        }
        //    }
        //}
    }

    protected void FunPageingNavig(string strColumn_Name_Sort, string condition)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        strColumn_Name_Sort = strColumn_Name_Sort.TrimStart(',');
        //pagetable.Visible = true;
        lblTotalRecords.Text = dsAmort.Tables[2].Rows.Count.ToString();
        txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        if (((Math.Round(dsAmort.Tables[2].Rows.Count / 100d, 0) * 100) / 100) > 1)
        {
            lblTotalPages.Text = ((Math.Round(dsAmort.Tables[2].Rows.Count / 100d, 0) * 100) / 100).ToString();
        }
        else
        {
            lblTotalPages.Text = "1";
        }
        if (dsAmort.Tables[2].Rows.Count >= 100)
        {
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = true;
            btnLast.Enabled = true;

        }
        else
        {
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
    }

    public void FunPageing(string strColumn_Name_Sort, string condition, int ig)
    {
        dsAmort = (DataSet)ViewState["dsAmort"];
        strColumn_Name_Sort = strColumn_Name_Sort.TrimStart(',');
        //strColumn_Name_Display = strColumn_Name_Display.TrimStart(',');
        lblTotalRecords.Text = dsAmort.Tables[2].Rows.Count.ToString();
        txtGotoPage.Text = first.ToString() + " - " + last.ToString();
        if (((Math.Round(dsAmort.Tables[2].Rows.Count / 100d, 0) * 100) / 100) > 1)
        {
            lblTotalPages.Text = ((Math.Round(dsAmort.Tables[2].Rows.Count / 100d, 0) * 100) / 100).ToString();
        }
        else
        {
            lblTotalPages.Text = "1";
        }
        if (dsAmort.Tables[2].Rows.Count >= 100)
        {
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = true;
            btnLast.Enabled = true;

        }
        else
        {
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
    }
    #endregion
    // Search Region -- End --//

    protected void btnclear_Click(object sender, EventArgs e)
    {
        try
        {
            txtPostingDateFrom.Text = "";
            txtPostingDateTo.Text = "";
            ddlCustName.SelectedText = "--Select--";
            ddlCustName.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
            ddlFunder.SelectedText = "--Select--";
            ddlFunder.SelectedValue = "0";
            ddlTrancheNo.SelectedText = "--Select--";
            ddlTrancheNo.SelectedValue = "0";
            ddlTranchegrp.SelectedValue = "0";
            tdExportDtl.Visible = false;
            pagetable.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetCustomerName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomers", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetFunderName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetFunderName", Procparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetTrancheDetails", Procparam));
        return suggetions.ToArray();
    }
}