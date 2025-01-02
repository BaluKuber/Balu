using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Web.Security;
using System.Globalization;
//Created By :Sathish R  
//Created On:28-May-2014
//Purpose:TO View Schedule Jobs Details
public partial class Reports_ReportScheduler : ApplyThemeForProject
{
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    S3GBusEntity.PagingValues ObjPaging = new PagingValues();
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
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    string strDateFormat = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"] = ObjUserInfo.ProCompanyIdRW.ToString();
            System.Web.HttpContext.Current.Session["AutoSuggestUserID"] = ObjUserInfo.ProUserIdRW.ToString();

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
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            txtFromDate_CalendarExtender.Format = strDateFormat;
            txtToDate_CalendarExtender.Format = strDateFormat;
          //  txtFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDate.ClientID + "','" + strDateFormat + "',true,false);");
        //    txtToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtToDate.ClientID + "','" + strDateFormat + "',true,false);");
            if (!IsPostBack)
            {
                ucCustomPaging.Visible = false;
                FunPubLoadReportName();

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally { }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetUserName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@PrefixText", prefixText.Trim());
        Procparam.Add("@Type", "1");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_SCHEDULEUSNAME", Procparam));
        return suggetions.ToArray();
    }
    [System.Web.Services.WebMethod]
    public static string[] GetProgramName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", System.Web.HttpContext.Current.Session["AutoSuggestCompanyID"].ToString());
        Procparam.Add("@User_ID", System.Web.HttpContext.Current.Session["AutoSuggestUserID"].ToString());
        Procparam.Add("@PrefixText", prefixText.Trim());
        Procparam.Add("@Type", "2");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GET_PRGNAMEDUSER", Procparam));
        return suggetions.ToArray();
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally { }
    }
    private void FunPriBindGrid()
    {
        try
        {
            ucCustomPaging.Visible = true;
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                if (Utility.StringToDate(txtFromDate.Text) > Utility.StringToDate(txtToDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "To date cannot be less than From date");
                    txtToDate.Focus();
                    txtToDate.Text = string.Empty;
                    gvScheduleDetails.DataSource = null;
                    gvScheduleDetails.DataBind();
                    ucCustomPaging.Visible = false;
                    return;
                }
                else if (Utility.StringToDate(txtFromDate.Text) > Utility.StringToDate(txtToDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "From date cannot be less than To date");
                    txtToDate.Focus();
                    txtToDate.Text = string.Empty;
                    return;
                }
            }
            if (!(txtUserName.Text.Trim() != string.Empty))
            {
                hdnLocationID.Value = string.Empty;
            }
            ucCustomPaging.Visible = true;
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            if(hdnLocationID.Value!=string.Empty)
                Procparam.Add("@USERID", hdnLocationID.Value.ToString());
            Procparam.Add("@PROGRAMID", "520");
            if (txtFromDate.Text != string.Empty)
                Procparam.Add("@DATEFROM", Utility.StringToDate(txtFromDate.Text.Trim()).ToString());
            else
                Procparam.Add("@DATEFROM", null);
            Procparam.Add("@ReportId", ddlprogramName.SelectedValue.ToString());            
            if (txtToDate.Text != string.Empty)
                Procparam.Add("@DATETO", Utility.StringToDate(txtToDate.Text.Trim()).ToString());
            else
                Procparam.Add("@DATETO", null);
            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = ObjUserInfo.ProCompanyIdRW;
            ObjPaging.ProUser_ID = ObjUserInfo.ProUserIdRW;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = "";
            ObjPaging.ProOrderBy = "";
            bool bIsNewRow = false;
            gvScheduleDetails.BindGridView("S3G_GET_RP_SCHEDULERDET", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            if (intTotalRecords > 0)
            {
                Panel1.Visible = true;
                gvScheduleDetails.Visible = true;
                ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                ucCustomPaging.setPageSize(ProPageSizeRW);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "no records found");
                Panel1.Visible = false;
                gvScheduleDetails.Visible = false;
                ucCustomPaging.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void GrdUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GrdUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblstatusId = (Label)e.Row.FindControl("statusId");
                Label lblReport_Id = (Label)e.Row.FindControl("lblReport_Id");
                LinkButton Lnkstatus = (LinkButton)e.Row.FindControl("ImghyplnkView");
                if (lblstatusId.Text == "1")
                {
                    Lnkstatus.Enabled = true;
                    //// Modified By : Anbuvel.T,Date : 08-May-2017,Description : RM3I New Process Flow Changes.
                    if (lblReport_Id.Text.Trim() == "6")// Report Id - RM3I Adjustment Entry -- DownLoad Link Disabled Option Done
                    {
                        Lnkstatus.Enabled = false;
                    }
                    else
                    {
                        Lnkstatus.Enabled = true;
                    }
                }
                else
                    Lnkstatus.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        try
        {

            string strhdnValue = hdnLocationID.Value;
            if (strhdnValue == "-1" || strhdnValue == string.Empty)
            {
                txtUserName.Text = string.Empty;
                hdnLocationID.Value = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally { }
    }
    protected void txtProgramName_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ImghyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            if (gvScheduleDetails.Rows.Count > 0)
            {

                int intRowIndex = Utility.FunPubGetGridRowID("gvScheduleDetails", ((LinkButton)sender).ClientID.ToString());
                Label lbldownloadpath = (Label)gvScheduleDetails.Rows[intRowIndex].FindControl("lbldownload");
                LinkButton lnkReqId = (LinkButton)gvScheduleDetails.Rows[intRowIndex].FindControl("ReqIdhyplnkView");
                Label lblReport = (Label)gvScheduleDetails.Rows[intRowIndex].FindControl("lblReport_Name");

                string strFileName = string.Empty;
                strFileName = lbldownloadpath.Text.ToString();
                //strFileName = strFileName + "\\" + lnkReqId.Text.Trim() + ".zip";
                strFileName = strFileName + "\\" + lblReport.Text.ToString() + "_" + lnkReqId.Text.Trim() + ".xls";
                if (System.IO.File.Exists(strFileName))
                {
                    System.IO.FileStream fs = new System.IO.FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    byte[] bytBytes = new byte[(int)fs.Length];
                    fs.Read(bytBytes, 0, (int)fs.Length);
                    fs.Close();

                    Response.AddHeader("Content-disposition", "attachment; filename=" + lblReport.Text.ToString() + ".xls");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(bytBytes);
                    Response.Output.Flush();
                    Response.End();
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Path or File Not Found...");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void LnkReqIdView_Click(object sender, EventArgs e)
    {
        try
        {
            int intRowIndex = Utility.FunPubGetGridRowID("gvScheduleDetails", ((LinkButton)sender).ClientID.ToString());
            LinkButton LnkTicket = (LinkButton)gvScheduleDetails.Rows[intRowIndex].FindControl("ReqIdhyplnkView");
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(LnkTicket.Text.ToString(), false, 0);
            string strScipt = "window.open('" + "S3GReportSchedule.aspx?QsMode=C&IsfromReport=YES&qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "', 'newwindow','toolbar=no,location=no,menubar=no,width=750,height=500,resizable=yes,scrollbars=yes,top=50,left=250');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "newwindow", strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void btn_Schedule_Onclick(object sender, EventArgs e)
    {
        try
        {
            string strScipt = "window.open('" + "S3GReportSchedule.aspx?QsMode=Q&IsfromReport=YES" + "', 'newwindow','toolbar=no,location=no,menubar=no,width=750,height=500,resizable=yes,scrollbars=yes,top=50,left=250');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "newwindow", strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    public void FunPubLoadReportName()
    {
        try
        {
            Dictionary<string, string> strProParm = new Dictionary<string, string>();
            strProParm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            strProParm.Add("@LoadOption", "3");
            ddlprogramName.BindDataTable("S3G_RP_LOADMUL_LST", strProParm, new string[] { "ID", "NAME" });
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }
    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
        try
        {
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            ddlprogramName.SelectedValue = "0";
            txtUserName.Text = string.Empty;
            gvScheduleDetails.DataSource = null;
            gvScheduleDetails.DataBind();
            gvScheduleDetails.Visible = false;
            ucCustomPaging.Visible = false;
            hdnLocationID.Value = string.Empty;
            Panel1.Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
        }

    }
    void Page_PreRender(object obj, EventArgs e)
    {
        ViewState["update"] = Session["update"];
    }


}