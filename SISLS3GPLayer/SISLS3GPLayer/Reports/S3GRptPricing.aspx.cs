#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Pricing Report
/// Created By          :   Sangeetha R
/// Created Date        :   4-Oct-2012
/// Purpose             :   To Get data based on the Pricing details.
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion

#region Namespaces
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
using System.Collections.Generic;
using S3GBusEntity.Reports;
using System.ServiceModel;
using System.IO;
using System.Text;
using System.Globalization;
using S3GBusEntity;
#endregion

public partial class Reports_S3GRptPricing : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    bool Is_Active;
    int LocationId1;
    int Active;
    public string strDateFormat;
    Dictionary<string, string> dictParam = null;
    static string strPageName = "Pricing Report";
    DataTable dt;
    DataTable dtTable = new DataTable();
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            cvPricing.ErrorMessage = "Due to Data problem, Unable to Load the Pricing Report Page";
            cvPricing.IsValid = false;
        }
    }
    #endregion

    #region Page Methods
    private void FunPriLoadPage()
    {
        try
        {
            CalendarExtender1.Format = ObjS3GSession.ProDateFormatRW;
            CalendarExtender2.Format = ObjS3GSession.ProDateFormatRW;

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;

            intUserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDate.Attributes.Add("Readonly", "true");
            //txtEndDate.Attributes.Add("Readonly", "true");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */

            if (!IsPostBack)
            {
                FunPriLoadLOB();
                FunPriLoadLocation1();
                ddlLoc2.Enabled = false;
                FunPriLoadLocation();
                FunPriLoadDenomination();
            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Due to Data problem, Unable to Load the Pricing Report Page");
        }
    }

    private void FunPriLoadLOB()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "217");
            ddlLOB.BindDataTable("S3G_RPT_GETLOBDETAILS", dictParam, new string[] { "LOB_ID", "LOB_NAME" });
            dictParam = null;
            //if (ddlLoc.Items.Count == 2)
            //    ddlLoc.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadLocation1()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "217");
            dictParam.Add("@Program_Code", "PRI");
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            ddlLoc1.BindDataTable("S3G_RPT_GET_BranchDetails", dictParam, new string[] { "LOCATION_ID", "LOCATION" });
            dictParam = null;
            //if (ddlLoc.Items.Count == 2)
            //    ddlLoc.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadLocation()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "217");
            dictParam.Add("@Program_Code", "PRI");
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            ddlLoc2.BindDataTable("S3G_RPT_GET_BranchDetails", dictParam, new string[] { "LOCATION_ID", "LOCATION" });
            dictParam = null;
            //if (ddlLoc.Items.Count == 2)
            //    ddlLoc.SelectedIndex = 1;
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
            if (ddlLoc1.SelectedIndex > 0)
            {
                LocationId1 = Convert.ToInt32(ddlLoc1.SelectedValue);
            }
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "217");
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            dictParam.Add("@Location_Id", ddlLoc1.SelectedValue);
            ddlLoc2.BindDataTable("S3G_RPT_LEVELVALUE", dictParam, new string[] { "LOCATION_ID", "LOCATION_CODE", "LOCATION_NAME", "CATEGORY_CODE", "CATEGORY_ID", "LOCATION" });
            dictParam = null;
            //if (ddlLoc.Items.Count == 2)
            //    ddlLoc.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadDenomination()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            ddlDenomination.BindDataTable("S3G_RPT_GETDENOMINATION", dictParam, new string[] { "DENOMINATION_ID", "DENOMINATION_NAME" });
            dictParam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
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

    private void FunPriBindGrid()
    {
        try
        {
            DataSet ds = new DataSet();
            divPricing.Style.Add("display", "block");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "217");
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            dictParam.Add("@Location_Id1", ddlLoc1.SelectedValue);
            if (ddlLoc1.SelectedValue != "0")
                dictParam.Add("@Location_Id2", ddlLoc2.SelectedValue);
            dictParam.Add("@Startdate", Utility.StringToDate(txtStartDate.Text).ToString());
            dictParam.Add("@Enddate", Utility.StringToDate(txtEndDate.Text).ToString());
            if (ddlDenomination.SelectedValue != "0")
                dictParam.Add("@Denomination", ddlDenomination.SelectedValue);
            //ds = Utility.GetDefaultData("RP_GET_INWARD_DET", dictParam);
            dtTable = Utility.GetDefaultData("S3G_RPT_PRICINGDETAILS", dictParam);
            
            Session["Pricing"] = dtTable;
            pnlPricingDet.Visible = true;
            if (dtTable.Rows.Count > 0)
            //if(ds.Tables[0].Rows.Count > 0)
            {
                grvPricingDet.DataSource = dtTable;
                grvPricingDet.DataBind();

                Label lblTotFacAmt = (grvPricingDet).FooterRow.FindControl("lblTotFacAmt") as Label;
                lblTotFacAmt.Text = Convert.ToDecimal(dtTable.Compute("SUM(FACILITY_AMOUNT)", "")).ToString(Funsetsuffix());
                Label lblTotRVAmt = (grvPricingDet).FooterRow.FindControl("lblTotRVAmt") as Label;
                lblTotRVAmt.Text = Convert.ToDecimal(dtTable.Compute("SUM(RV_AMOUNT)", "")).ToString(Funsetsuffix());
                Label lblTotMrgnAmt = (grvPricingDet).FooterRow.FindControl("lblTotMrgnAmt") as Label;
                lblTotMrgnAmt.Text = Convert.ToDecimal(dtTable.Compute("SUM(MARGIN_AMOUNT)", "")).ToString(Funsetsuffix());
                Label lblTotCompIRR = (grvPricingDet).FooterRow.FindControl("lblTotCompIRR") as Label;
                lblTotCompIRR.Text = Convert.ToDecimal(dtTable.Compute("AVG(COMPANY_IRR)", "")).ToString(Funsetsuffix());

                BtnPrint.Visible = true;
                ds.Tables.Add(FunPriHDR_DT());
                ds.Tables[0].TableName = "DT_Header";

                ds.Tables.Add(dtTable);
                ds.Tables[1].TableName = "DT_Grid";
                Session["Report_Data"] = ds;
            }
            else
            {
                grvPricingDet.EmptyDataText = "No Data Between the Selected Date Range";
                grvPricingDet.DataBind();
                BtnPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable FunPriHDR_DT()
    {
        try
        {
            dt = new DataTable();
            DataRow drEmptyRow;
            dt.Columns.Add("LOB_ID");
            dt.Columns.Add("LOB_NAME");
            dt.Columns.Add("LOCATION_ID");
            dt.Columns.Add("LOCATION");
            dt.Columns.Add("LOCATION_NAME");
            dt.Columns.Add("DENOMINATION_NAME");
            dt.Columns.Add("FROM_DATE");
            dt.Columns.Add("TO_DATE");
            dt.Columns.Add("COMPANY_ID");
            dt.Columns.Add("COMPANY_NAME");
            dt.Columns.Add("COMPANY_ADDRESS");
            dt.Columns.Add("COL1");
            dt.Columns.Add("COL2");

            drEmptyRow = dt.NewRow(); //ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();
            drEmptyRow["LOB_ID"] = ddlLOB.SelectedValue;
            drEmptyRow["LOB_NAME"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            drEmptyRow["LOCATION_ID"] = ddlLoc1.SelectedValue;
            if (ddlLoc2.SelectedValue == "0")
            {
                drEmptyRow["LOCATION"] = ddlLoc1.SelectedItem.Text;
            }
            else
            {
                drEmptyRow["LOCATION"] = ddlLoc2.SelectedItem.Text;
            }
            drEmptyRow["LOCATION_NAME"] = ddlLoc2.SelectedItem.Text;
            drEmptyRow["DENOMINATION_NAME"] = ddlDenomination.SelectedItem.Text;
            drEmptyRow["FROM_DATE"] = txtStartDate.Text.Trim();
            drEmptyRow["TO_DATE"] = txtEndDate.Text.Trim();
            drEmptyRow["COMPANY_ID"] = ObjUserInfo.ProCompanyIdRW.ToString();
            drEmptyRow["COMPANY_NAME"] = ObjUserInfo.ProCompanyNameRW.ToString();
            drEmptyRow["COL1"] = "Pricing Report for the period from " + txtStartDate.Text.Trim() + " to " + txtEndDate.Text.Trim();
            drEmptyRow["COL2"] = "All Amounts are in " + ObjS3GSession.ProCurrencyNameRW.ToString();
            dt.Rows.Add(drEmptyRow);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    #endregion

    #region Page Events

    #region DropdownLists and Textbox Events

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadLocation1();
        FunPriLoadLocation();
    }

    protected void ddlLoc1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLoc1.SelectedValue == "0")
        {
            FunPriLoadLocation();
            ddlLoc2.Enabled = false;
        }
        else
        {
            ddlLoc2.Enabled = true;
            FunPriLoadLocation2();
        }
    }
    #endregion

    #region Button Events

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
            string lob = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();
            //Session["LOB"]=
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            lblAmounts.Visible = true;
            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvPricing.ErrorMessage = "Unable to get Pricing details";
            cvPricing.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLOB();
            FunPriLoadLocation1();
            ddlLoc2.Enabled = false;
            FunPriLoadLocation();
            FunPriLoadDenomination();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            pnlPricingDet.Visible = false;
            lblAmounts.Text = "";
            BtnPrint.Visible = false;
        }
        catch (Exception ex)
        {
            cvPricing.ErrorMessage = "Due to Data Problem, Unable to Clear the Report";
            cvPricing.IsValid = false;
        }
    }

    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strScipt = "window.open('../Reports/S3GRptPricingReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pricing", strScipt, true);
        }
        catch (Exception ex)
        {
            cvPricing.ErrorMessage = "Due to Data Problem, Unable to Print the Report";
            cvPricing.IsValid = false;
        }
    }
    #endregion

    #endregion
}