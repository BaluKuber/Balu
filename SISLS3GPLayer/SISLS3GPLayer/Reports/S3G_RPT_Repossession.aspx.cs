//Module Name      :   Reports
//Screen Name      :   S3G_RPT_Repossession.aspx
//Created By       :   Palani Kumar A
//Created Date     :   09-05-2014
//Purpose          :   To get Repossession details

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
using System.Text;
using System.Data.Common;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Resources;

public partial class Reports_S3G_RPT_Repossession : ApplyThemeForProject
{
    string strPageName = "RepossessionReport";
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictParam = null;

    public static Reports_S3G_RPT_Repossession obj_Page;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    public int CompanyId;
    public int ProgramId;
    public int UserId;
    public int LobId;
    public int LocationId;
    bool Is_Active;
    string EndDate;
    string LOB_ID;
    string Branch_ID;
    string Region_Id;
    string Product_Id;
    StringBuilder sw = new StringBuilder();
    ReportAccountsMgtServicesClient objSerClient;
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();// This screen       
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.CompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.UserId.ToString());
        Procparam.Add("@Program_Id", "270");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GET_BRANCHLIST_AGT", Procparam));
        return suggetions.ToArray();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    [System.Web.Services.WebMethod]
    public static string[] GetGarageWise(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.CompanyId.ToString());
        Procparam.Add("@OptionValue", "1");
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetGarageWise_AGT", Procparam));
        return suggetions.ToArray();
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
            ProgramId = 270;
            UserId = ObjUserInfo.ProUserIdRW;

            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            /* Changed Date Control start - 30-Nov-2012 */
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            obj_Page = this;

            if (!IsPostBack)
            {
                ClearSession();
                PnlDetailedView.Visible = false;
                btnPrint.Visible = false;
                FunPriLoadLob(CompanyId, UserId, ProgramId);
            }
            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load Repossession page");
        }
    }
    #region Load LOB
    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
        try
        {
            ddlLOB.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "All";
            //if (ddlLOB.Items.Count == 2)
            //{
            //    ddlLOB.SelectedIndex = 1;
            //}
            //else
            //{
            //    ddlLOB.SelectedIndex = 0;
            //}
            if (ddlLOB.SelectedValue != "-1")
                ddlBranch.Enabled = true;
            else
                ddlBranch.Enabled = false;
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
    #endregion
    #region Events
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedValue == "-1")
            {
                Utility.FunShowAlertMsg(this, "Select Line of Business");
                return;
            }
            
            if (ddlRepoType.SelectedValue == "-1")
            {
                Utility.FunShowAlertMsg(this, "Choose Atleast one Repossession Type");
                return;
            }
            else
                FunPriValidateFromEndDate();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    #endregion

    #region Deserialize
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    #endregion

    #region Validate Functions
    private void FunPriValidateFromEndDate()
    {
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
        {
            // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "ToDate should be greater than or equal to the From Date");
                txtEndDateSearch.Text = "";
                return;
            }
        }
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
        }
        if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
            (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtStartDateSearch.Text = txtEndDateSearch.Text;
        }
        if (ddlRepoType.SelectedValue != "-1")
            FunPriLoadDetails();
        else
        {
            Utility.FunShowAlertMsg(this, "Choose Atleast One Repossession Type");
            return;
        }
    }
    #endregion
    public void ClearSession()
    {
        Session["RepoReport"] =
        ViewState["DSRepoReport"] = null;
    }
    #region Load Details Grid
    private void FunPriLoadDetails()
    {
        try
        {
            //divDetails.Style.Add("display", "block");
            PnlDetailedView.Visible = true;
            DataTable dtRepoReport = new DataTable();

            //Call the Procedure and get value from Database
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            if (ddlBranch.SelectedText == "All")
                dictParam.Add("@Location_Id", "0");
            else
                dictParam.Add("@Location_Id", ddlBranch.SelectedValue);

            if (txtStartDateSearch.Text != string.Empty)
                dictParam.Add("@FromDate", Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString());
            if (txtEndDateSearch.Text != string.Empty)
                dictParam.Add("@ToDate", Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString());
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());

            if (ddlGaragecNo.SelectedText != string.Empty)
                dictParam.Add("@GarageNo", ddlGaragecNo.SelectedValue.ToString());

            dictParam.Add("@ProgramID", ProgramId.ToString());
            dictParam.Add("@userid", ObjUserInfo.ProUserIdRW.ToString());

            if (ddlRepoType.SelectedValue != "-1")
                dictParam.Add("@Type", ddlRepoType.SelectedValue);

            dtRepoReport = Utility.GetDefaultData("S3G_RPT_REPOSSESSION_REPORT", dictParam);


            if (dtRepoReport != null && dtRepoReport.Rows.Count == null)
            {
                GrvDetails.EmptyDataText = "No Records were found for the Location for this month";
                GrvDetails.DataBind();
                btnPrint.Visible = false;
                return;
            }
            //if (GrvDetails.Rows.Count != 0)
            //{
            //    GrvDetails.HeaderRow.Style.Add("position", "relative");
            //    GrvDetails.HeaderRow.Style.Add("z-index", "auto");
            //    GrvDetails.HeaderRow.Style.Add("top", "auto");
            //}
            if (dtRepoReport != null && dtRepoReport.Rows.Count > 0)
            {
                Session["RepoReport"] = dtRepoReport;
                GrvDetails.Visible = true;
                GrvDetails.DataSource = ViewState["DSRepoReport"] = dtRepoReport;
                GrvDetails.DataBind();
                if (ddlRepoType.SelectedValue == "0")
                {
                    GrvDetails.Columns[10].Visible = false;
                    GrvDetails.Columns[11].Visible = false;
                }
                if (ddlRepoType.SelectedValue == "1")
                {
                    GrvDetails.Columns[10].Visible = true;
                    GrvDetails.Columns[11].Visible = false;
                }
                if (ddlRepoType.SelectedValue == "4")
                {
                    GrvDetails.Columns[10].Visible = false;
                    GrvDetails.Columns[11].Visible = true;
                }
                btnPrint.Visible = true;
            }
            else
            {
                GrvDetails.Visible = true;
                btnPrint.Visible = false;
                GrvDetails.EmptyDataText = "No Records Found";
                GrvDetails.DataBind();
                return;
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
    #endregion
    protected void FunForExportExcel()
    {
        DataTable dtDetail = (DataTable)ViewState["DSRepoReport"];
        StringBuilder shtml = new StringBuilder();
        shtml.Append("<html><head>");
        shtml.Append("<style type='text/css'>");
        shtml.Append(".stylePageHeading{background-image:url('../../images/title_headerBG.jpg');font-family:calibri,Verdana;font-weight:bold;font-size:13px;color:Navy;width:99.5%; padding-left:3px;border-bottom:0px solid #788783;border-top:0px solid #788783;margin-bottom:2px;filter:glow(color=InactiveCaptionText,strength=50);}");
        shtml.Append(".styleGridHeader{color:Navy;text-decoration:none;background-color:aliceblue;font-weight:bold;}");
        shtml.Append(".styleInfoLabel{font-family:calibri,Verdana;font-weight:normal;font-size:13px;color:Navy;text-decoration:none;background-color:White;}");
        shtml.Append("</style>");
        shtml.Append("</head>");
        shtml.Append("<Body>");
        shtml.Append("<table width='50%'>");
        shtml.Append("<tr>");
        shtml.Append("<td class='stylePageHeading' style='width:50%;' align='center' colspan=" + Convert.ToSingle((dtDetail.Columns.Count)) + " " + " scope='col'>");
        shtml.Append(ObjUserInfo.ProCompanyNameRW);
        shtml.Append("</td>");
        shtml.Append("</tr>");
        shtml.Append("<td align='center'  class='stylePageHeading' colspan=" + Convert.ToSingle((dtDetail.Columns.Count)) + " " + " scope='col'>");
        shtml.Append("&nbsp;&nbsp;&nbsp;" + lblHeading.Text + " - " + ddlRepoType.SelectedItem.Text.ToString() + " for the period from " + txtStartDateSearch.Text + " to " + txtEndDateSearch.Text + "");
        shtml.Append("</td>");
        shtml.Append("</tr>");
        //shtml.Append("<tr>");
        //shtml.Append("<td  class='stylePageHeading' align='left' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col' >");
        //shtml.Append("&nbsp;&nbsp;&nbsp;Line of Business : " + ddlLOB.SelectedItem.Text + "</td>");
        //shtml.Append("</tr>");
        //if (!string.IsNullOrEmpty(ddlBranch.SelectedText.ToString()))
        //{
        //    shtml.Append("<tr>");
        //    shtml.Append("<td  class='stylePageHeading' align='left' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col' >");
        //    shtml.Append("&nbsp;&nbsp;&nbsp;Location : " + ddlBranch.SelectedText + "</td>");
        //    shtml.Append("</tr>");
        //}
        //if (!string.IsNullOrEmpty(ddlGaragecNo.SelectedText.ToString()))
        //{
        //    shtml.Append("<tr>");
        //    shtml.Append("<td  class='stylePageHeading' align='left' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col' >");
        //    shtml.Append("&nbsp;&nbsp;&nbsp;Garage Wise Name : " + ddlGaragecNo.SelectedText.ToString() + "</td>");
        //    shtml.Append("</tr>");
        //}
        //shtml.Append("<tr>");
        //shtml.Append("<td  class='stylePageHeading' align='left' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col' >");

        //if (ddlRepoType.SelectedValue != "-1")
        //    shtml.Append("&nbsp;&nbsp;&nbsp;Type : " + ddlRepoType.SelectedItem.Text.ToString() + "</td>");
        ////else if (chkReleased.Checked == true && chkReSold.Checked == true)
        ////    shtml.Append("&nbsp;&nbsp;&nbsp;Type : " + chkReleased.ToolTip.ToString() + " , " + chkReSold.ToolTip.ToString() + "</td>");
        ////else if (chkReleased.Checked == true)
        ////    shtml.Append("&nbsp;&nbsp;&nbsp;Type : " + chkReleased.ToolTip.ToString() + "</td>");
        ////else
        ////{
        ////    shtml.Append("&nbsp;&nbsp;&nbsp;Type : " + chkReSold.ToolTip.ToString() + "</td>");
        ////}
        //shtml.Append("</tr>");
        shtml.Append("</table>");
        shtml.Append(" <table class='styleGridView' cellspacing='0' cellpadding='1' rules='all' border='0'style='color: #003D9E; font-family: calibri;font-size: 13px; font-weight: normal; width: 100%; border-collapse: collapse;>");
        shtml.Append("<tr>");
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        GrvDetails.RenderControl(htw);
        shtml.Append("<td style='width:100%;' class='stylePageHeading' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col'>");
        shtml.Append(sw.ToString());
        shtml.Append("</td>");
        shtml.Append("</tr>");
        shtml.Append("</table>");
        shtml.Append("<table width='100%'>");
        shtml.Append("<tr>");
        shtml.Append("<td align='left'  class='stylePageHeading' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col'>");
        shtml.Append("</td>");
        shtml.Append("</tr>");
        shtml.Append("<tr>");
        shtml.Append("<td align='left'  class='stylePageHeading' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col'>");
        shtml.Append("&nbsp;&nbsp;&nbsp;Name : " + ObjUserInfo.ProUserNameRW);
        shtml.Append("</td>");
        shtml.Append("</tr>");
        shtml.Append("<tr>");
        shtml.Append("<td align='left'  class='stylePageHeading' colspan=" + Convert.ToSingle(dtDetail.Columns.Count) + " " + " scope='col'>");
        //shtml.Append(System.DateTime.Now);
        shtml.Append("&nbsp;&nbsp;&nbsp;DateTime : " + DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + " " + DateTime.Parse(DateTime.Now.ToString()).ToString("hh:mm tt") + "");
        shtml.Append("</td>");
        shtml.Append("</tr>");
        shtml.Append("</table>");
        shtml.Append("</Body></html>");
        try
        {
            string attachment = "attachment; filename=" + strPageName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.xls";
            Response.Write(shtml);
            Response.End();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    //public static string SetGarageAddress(string Garage_Address1, string Garage_Address2, string Garage_City, string Garage_State, string Garage_Country, string Garage_Pin, string Garage_Telephone, string Garage_Mobile, string Garage_Email_ID, string Garage_Web_Site)
    //{
    //    try
    //    {
    //        string strAddress = "";
    //        //if (Garage_Name.ToString() != "") strAddress += Garage_Name.ToString() + System.Environment.NewLine;
    //        if (Garage_Address1.ToString() != string.Empty) strAddress += Garage_Address1.ToString() + System.Environment.NewLine;
    //        if (Garage_Address2.ToString() != string.Empty) strAddress += Garage_Address1.ToString() + System.Environment.NewLine;
    //        if (Garage_City.ToString() != string.Empty) strAddress += Garage_City.ToString() + System.Environment.NewLine;
    //        if (Garage_State.ToString() != string.Empty) strAddress += Garage_State.ToString() + System.Environment.NewLine;
    //        if (Garage_Country.ToString() != string.Empty) strAddress += Garage_Country.ToString() + System.Environment.NewLine;
    //        if (Garage_Pin.ToString() != string.Empty) strAddress += Garage_Pin.ToString() + System.Environment.NewLine;
    //        if (Garage_Telephone.ToString() != string.Empty) strAddress += Garage_Telephone.ToString() + System.Environment.NewLine;
    //        if (Garage_Mobile.ToString() != string.Empty) strAddress += Garage_Mobile.ToString() + System.Environment.NewLine;
    //        if (Garage_Email_ID.ToString() != string.Empty) strAddress += Garage_Email_ID.ToString() + System.Environment.NewLine;
    //        if (Garage_Web_Site.ToString() != string.Empty) strAddress += Garage_Web_Site.ToString();
    //        return strAddress;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
    //        throw ex;
    //    }
    //}
    protected void btnClear_Click(object sender, EventArgs e)
    {
        FunPriClear();
    }
    protected void FunPriClear()
    {
        FunClear();
        ddlLOB.SelectedValue = "-1";
        if (!string.IsNullOrEmpty(ddlBranch.SelectedText.ToString()))
            ddlBranch.Clear();
        ddlBranch.Enabled = false;
        if (!string.IsNullOrEmpty(ddlGaragecNo.SelectedText))
            ddlGaragecNo.Clear();
        txtStartDateSearch.Text =
        txtEndDateSearch.Text = string.Empty;
        ddlRepoType.SelectedValue = "-1";
        //chkRepossession.Checked = true;
        //chkReleased.Checked = false;
        //chkReSold.Checked = false;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DSRepoReport"] != null && GrvDetails.Rows.Count > 0)
            {
                FunForExportExcel();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClear();
        txtStartDateSearch.Text =
        txtEndDateSearch.Text = string.Empty;
        if (ddlLOB.SelectedValue == "-1")
        {
            if (!string.IsNullOrEmpty(ddlBranch.SelectedText))
                ddlBranch.Clear();
        }
        if (ddlLOB.SelectedValue == "-1")
            ddlBranch.Enabled = false;
        else
            ddlBranch.Enabled = true;
    }
    protected void ddlGaragecNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriclearGrid();
        PnlDetailedView.Visible =
        btnPrint.Visible = false;
    }
    private void FunPriclearGrid()
    {
        ViewState["DSRepoReport"] =
        Session["RepoReport"] =
        GrvDetails.DataSource = null;
        GrvDetails.DataBind();
    }
    protected void ddlRepoType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlRepoType.SelectedValue=="-1")
        //{
        //    FunClear();
        //    //chkReleased.Checked =
        //    //chkReSold.Checked = false;
        //}
        FunClear();
    }
    //protected void chkReleased_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkReleased.Checked == true)
    //        chkRepossession.Checked = false;
    //    FunClear();
    //}
    //protected void chkReSold_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkReSold.Checked == true)
    //        chkRepossession.Checked = false;
    //    FunClear();
    //}
    protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
    {
        FunClear();
    }
    protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
    {
        FunClear();
    }
    protected void FunClear()
    {
        FunPriclearGrid();
        PnlDetailedView.Visible =
        btnPrint.Visible = false;
        ddlGaragecNo.Clear();
    }
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", false);
    //}
    //#region Grid Commands
    //protected void GrvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        //DataTable dtRepoReport = (DataTable)Session["RepoReport"];
    //        //if (e.Row.RowType == DataControlRowType.DataRow)
    //        //{                
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
    //    }
    //}
    //#endregion
    protected void GrvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}