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
using System.Collections.Generic;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;

public partial class Reports_S3GRptFactoringBusinessReport : ApplyThemeForProject
{
    Dictionary<string, string> dictParam = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    public int CompanyId;
    public int ProgramId;
    public int UserId;
    public int LobId;
    public int LocationId;
    decimal TotalApprovedAmount;
    decimal TotalPaidAmount;
    decimal TotalRemainingAmount;
    decimal TotalAccountYettoBeCreatedAmount;
    decimal Totalageing0days;
    decimal Totalageing30days;
    decimal Totalageing60days;
    bool Is_Active;
    string EndDate;
    string LOB_ID;
    string Branch_ID;
    string Region_Id;
    string Product_Id;

    string strPageName = "FactoringBusinessReport";
    DataTable dt;
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient;

    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();// This screen

    }

    private void FunPriLoadPage()
    {
        try
        {

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            ProgramId = 239;
            UserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date            
            CalendarExtender2.Format = strDateFormat;
            //todaydate = DateTime.Now.ToString();
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //Session["Date"] = DateTime.Now.Date.ToString();
            if (!IsPostBack)
            {
                ClearSession();

                btnPrint.Visible = false;

                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                FunPubLoadDenomination();
            }

            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load DisbursementReport page");
        }
    }


    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
        try
        {
            ddlLOB.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB_FT(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            ddlLOB.Items[0].Text = "All";
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
            {
                ddlLOB.SelectedIndex = 0;
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


    private void FunPriLoadLocation(int CompanyId, int UserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlLocation.DataSource = Branch;
            ddlLocation.DataTextField = "Description";
            ddlLocation.DataValueField = "ID";
            ddlLocation.DataBind();
            ddlLocation.Items[0].Text = "All";

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


    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlDenomination.DataSource = Denomination;
            ddlDenomination.DataTextField = "Description";
            ddlDenomination.DataValueField = "ID";
            ddlDenomination.DataBind();
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


    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }


    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Abstract"] = null;
        Session["Details"] = null;
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        divMaturity.Visible = true;
        btnPrint.Visible = true;
        FunPriLoadDetails();

    }


    private void FunPriLoadDetails()
    {
        try
        {
            DataSet ds = new DataSet();
            //divDemand.Style.Add("display", "block");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            if (ddlLocation.SelectedIndex != 0)
            {
                dictParam.Add("@Location_Id", ddlLocation.SelectedValue);
            }
            dictParam.Add("@Startdate", Utility.StringToDate(txtStartDateSearch.Text).ToString());
            dictParam.Add("@Enddate", Utility.StringToDate(txtEndDateSearch.Text).ToString());
            dictParam.Add("@Denomination", ddlDenomination.SelectedValue);
            //ds = Utility.GetDefaultData("RP_GET_INWARD_DET", dictParam);
            dtTable = Utility.GetDefaultData("S3G_GET_BUSINESS_DTL", dictParam);

            Session["DocDef"] = dtTable;
            pnlDemand.Visible = true;
            if (dtTable.Rows.Count > 0)
            {
                grvBusiness.DataSource = dtTable;
                grvBusiness.DataBind();

                if (dtTable.Rows[0]["MLASLA"].ToString() == "True")
                {
                    grvBusiness.Columns[4].Visible = true;
                }
                else
                {
                    grvBusiness.Columns[4].Visible = false;
                }
                btnPrint.Visible = true;

                ds.Tables.Add(dtTable);
            }
            else
            {

                lblError.Text = "No Records Found";
                grvBusiness.DataBind();
                grvBusiness.Visible = false;
                btnPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        divMaturity.Visible = false;
        ddlLOB.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        txtStartDateSearch.Text = "";
        txtEndDateSearch.Text = "";
        //ddlDenomination.SelectedIndex = 0;
        btnPrint.Visible = false;

    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GFactoringBusinessReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Factoring Business", strScipt, true);

    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
