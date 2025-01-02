using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;
using System.Globalization;
using System.Collections.Generic;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;



public partial class Reports_S3G_Rpt_WDV : ApplyThemeForProject
{

    string strPageName = "Lease Consolidation and WDV Report";

    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    int intCompanyId;
    int intUserId;
    int intProgramId;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            intProgramId = 218;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");
            //txtEndDateSearch.Attributes.Add("readonly", "readonly");
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvWDV.ErrorMessage = "Due to Data Problem, Unable to Load Page";
            cvWDV.IsValid = false;
        }

    }

    #region [LOAD LOB]
    private void FunPriLoadLob()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Program_ID", intProgramId.ToString());
            ddlLOB.BindDataTable("S3G_RPT_GETLOBDETAILS", Procparam, false, new string[] { "LOB_ID",  "LOB_NAME" });
            Procparam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion [LOAD LOB]

    #region [LOAD LOCATIONS]
    private void FunPriLoadLocation()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Program_ID", intProgramId.ToString());
            Procparam.Add("@LOb_ID", ddlLOB.SelectedValue);
            ddlLocation1.BindDataTable("S3G_RPT_GET_BranchDetails", Procparam, false, new string[] { "Location_Id", "Location" });
            ddlLocation1.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            Procparam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    private void FunPriLoadLocation2()
    {
        try
        {
            if (ddlLocation1.SelectedIndex > 0)
                ddlLocation2.Enabled = true;
            else
                ddlLocation2.Enabled = false;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Program_ID", intProgramId.ToString());
            Procparam.Add("@LOb_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Location_Id", ddlLocation1.SelectedValue);
            ddlLocation2.BindDataTable(SPNames.getlocation2, Procparam, false, new string[] { "Location_Id", "Location_Name" });
            ddlLocation2.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
            Procparam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriLoadDenoMination()
    {
        try
        {

            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            ddlDenomination.BindDataTable("S3G_RPT_GETDENOMINATION", Procparam, false, new string[] { "DENOMINATION_ID", "DENOMINATION_NAME" });
            //ddlDenomination.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("--Select--", "0")));
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion [LOAD LOCATIONS]

    public void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (!IsPostBack)
            {
                FunPriLoadLob();
                FunPriLoadLocation();
                FunPriLoadDenoMination();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unable to Load Report Page");
        }
    }

    private void FunPriLoadGrid()
    {
        try
        {
            DataTable dt;
           int finyear;
            divdetails.Style.Add("display", "block");
            pnlDetails.Visible = true;
            int financialyearmonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            DateTime year = Utility.StringToDate(txtEndDateSearch.Text);
            string endyear = year.Year.ToString();
            DateTime month = Utility.StringToDate(txtEndDateSearch.Text);
            string endmonth = month.Month.ToString();
            if (Convert.ToInt32(endmonth) <= financialyearmonth)
            {
                finyear = Convert.ToInt32(endyear) - 1;
            }
            else
            {
                finyear = Convert.ToInt32(endyear);
            }
            DateTime finmonth=Convert.ToDateTime(financialyearmonth+"/"+1+"/"+finyear);
            DateTime firstenddate = Convert.ToDateTime(Convert.ToInt32(endmonth) + "/" + 1  +"/" + Convert.ToInt32(endyear));
            string cutoffmonth = year.ToString("0000") + month.ToString("00");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyId.ToString());
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@lob_id", ddlLOB.SelectedValue);
            Procparam.Add("@Program_ID", intProgramId.ToString());
            Procparam.Add("@location_id1", ddlLocation1.SelectedValue);
            Procparam.Add("@location_id2", ddlLocation2.SelectedValue);
            Procparam.Add("@FROM_DATE", Utility.StringToDate(txtStartDateSearch.Text.Trim()).ToString());
            Procparam.Add("@TO_DATE", Utility.StringToDate(txtEndDateSearch.Text.Trim()).ToString());
            Procparam.Add("@FYSTARTDATE", finmonth.ToString());
            Procparam.Add("@FIRSTENDDATE", firstenddate.ToString());
            Procparam.Add("@ENDMONTH", cutoffmonth.ToString());
            Procparam.Add("@DENOMINATION", ddlDenomination.SelectedValue);
            dt = Utility.GetDefaultData("S3G_RPT_WDV", Procparam);
            grvdetails.DataSource=dt;
            grvdetails.DataBind();
            ViewState["DT_Grid"] = dt;
            if (grvdetails.Rows.Count > 1)
            {
                Label lblAssetvaluef = (Label)(grvdetails).FooterRow.FindControl("lblAssetvaluef");
                lblAssetvaluef.Text = Convert.ToDecimal(dt.Compute("sum(ASSET_VALUE)", "ASSET_VALUE>=0")).ToString(Funsetsuffix());
                Label lblDEPRECIATION_AMOUNTf = (Label)(grvdetails).FooterRow.FindControl("lblDEPRECIATION_AMOUNTf");
                lblDEPRECIATION_AMOUNTf.Text = Convert.ToDecimal(dt.Compute("sum(DEPRECIATION_AMOUNT)", "DEPRECIATION_AMOUNT>=0")).ToString(Funsetsuffix());
                Label lblPrincipalamountf = (Label)(grvdetails).FooterRow.FindControl("lblPrincipalamountf");
                lblPrincipalamountf.Text = Convert.ToDecimal(dt.Compute("sum(PRINCIPAL)", "PRINCIPAL>=0")).ToString(Funsetsuffix());
                Label lblResidualAmountf = (Label)(grvdetails).FooterRow.FindControl("lblResidualAmountf");
                lblResidualAmountf.Text = Convert.ToDecimal(dt.Compute("sum(RVAMT)", "RVAMT>=0")).ToString(Funsetsuffix());
                Label lblGrossf = (Label)(grvdetails).FooterRow.FindControl("lblGrossf");
                lblGrossf.Text = Convert.ToDecimal(dt.Compute("sum(GROSS)", "GROSS>=0")).ToString(Funsetsuffix());
                Label lblExpensesf = (Label)(grvdetails).FooterRow.FindControl("lblExpensesf");
                lblExpensesf.Text = Convert.ToDecimal(dt.Compute("sum(EXPENSES)", "EXPENSES>=0")).ToString(Funsetsuffix());
                Label lblNetIncomef = (Label)(grvdetails).FooterRow.FindControl("lblNetIncomef");
                lblNetIncomef.Text = Convert.ToDecimal(dt.Compute("sum(NETINCOME)", "NETINCOME>=0")).ToString(Funsetsuffix());
                Label lblPVGROSSf = (Label)(grvdetails).FooterRow.FindControl("lblPVGROSSf");
                lblPVGROSSf.Text = Convert.ToDecimal(dt.Compute("sum(PVGROSS)", "PVGROSS>=0")).ToString(Funsetsuffix());
                BtnPrint.Visible = true;
            }
           
            else
                BtnPrint.Visible = false;
            
        }
    

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
       
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {

        FunPriLoadGrid();
    }

    protected void ddlLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunPriLoadLocation2();
    }

    private DataTable FunPriHDR_DT()
    {
        DataTable dt;
        try
        {
            
            dt = new DataTable();
            DataRow drEmptyRow;
            dt.Columns.Add("Company_id");
            dt.Columns.Add("Company_Name");
            dt.Columns.Add("Company_Address");
            dt.Columns.Add("LOB");
            dt.Columns.Add("Location");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("Heading");
            dt.Columns.Add("Denomination");
            dt.Columns.Add("Date");


            drEmptyRow = dt.NewRow();
            drEmptyRow["Company_Id"] = ObjUserInfo.ProCompanyIdRW.ToString();
            drEmptyRow["Company_Name"] = ObjUserInfo.ProCompanyNameRW.ToString();
            drEmptyRow["Company_Address"] = "Lease Consolidation and WDV Report For the Period from " + txtStartDateSearch.Text.Trim() + " T0 " + txtEndDateSearch.Text.Trim();
            drEmptyRow["LOB"] = ddlLOB.SelectedItem.Text.Trim();
            drEmptyRow["Denomination"] = "All Amounts are in " + ObjS3GSession.ProCurrencyNameRW.ToString();
            drEmptyRow["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            dt.Rows.Add(drEmptyRow);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;

    }

    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet(); ;


            ds.Tables.Add(FunPriHDR_DT());
            ds.Tables[0].TableName = "DT_Header";
            ds.Tables.Add((DataTable)ViewState["DT_Grid"]);
            ds.Tables[1].TableName = "DT_Grid";
            Session["DT_Grid"] = ds;
            //PreviewPDF_Click(true);
            string strScipt = "window.open('../Reports/S3G_RPT_WDV_ReportPage.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this,this.GetType(), "Lease Consolidation and WDV Report", strScipt, true);
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void btnclear_Click(object sender, EventArgs e)
    {



        ddlLocation1.SelectedIndex = 0;
        if(ddlLocation2.SelectedIndex>0)
        ddlLocation2.SelectedIndex = 0;
        txtStartDateSearch.Text="";
        txtEndDateSearch.Text="";
        pnlDetails.Visible=false;
        BtnPrint.Visible = false;
        ddlDenomination.SelectedValue = "1";
        
    }



}
