#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Document Deficiency Report
/// Created By          :   Sangeetha R
/// Created Date        :   4-Jun-2011
/// Purpose             :   To get the Documents that are waived, Collected and Uncollected
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion

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
using ReportAccountsMgtServicesReference;
using System.Collections.Generic;
using S3GBusEntity.Reports;
using S3GBusEntity;
using System.Globalization;

public partial class Reports_S3GRptDocumentDeficiencyReport : ApplyThemeForProject
{

    Dictionary<string, string> dictParam = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    int CompanyId;
    int UserId;
    bool Is_Active;
    public int ProgramId;
    string Customer_ID;
    string LOB_ID;
    string Branch_ID;
    public int LobId;
    public int LocationId;
    string strKey = "Insert";
    string strPageName = "Document Deficiency Report";
    DataTable dt;
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient;
    DateTime StartDate;
    DateTime EndDate;
    public bool chk = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Application Standard Date Format

        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
        CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
        CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
        /* Changed Date Control start - 30-Nov-2012 */
        //txtStartDateSearch.Attributes.Add("readonly", "readonly");
        //txtEndDateSearch.Attributes.Add("readonly", "readonly");
        Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
        txtFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtFrom.ClientID + "','" + strDateFormat + "',true,  false);");
        txtTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtTo.ClientID + "','" + strDateFormat + "',true,  false);");
        #endregion

        FunPriLoadPage();
    }

    #region Load Page
    public void FunPriLoadPage()
    {
        try
        {
            //this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            //strDateFormat = ObjS3GSession.ProDateFormatRW;
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            UserId = ObjUserInfo.ProUserIdRW;
            ProgramId = 243;
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txt.Attributes.Add("onfocus", "fnLoadCustomer()");
            txt.Attributes.Add("ReadOnly", "ReadOnly");
            if (!IsPostBack)
            {
                //today = DateTime.Now.Date.ToString(strDateFormat);
                //txtStartDateSearch.Text = today.Substring(0, 11);
                //txtEndDateSearch.Text = today.Substring(0, 11);
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadDocType(CompanyId);
                FunPriLoadDocStatus(CompanyId);
                //FunPriLoadRegion(CompanyId, Is_Active, UserId);
                //FunPriLoadBranch(CompanyId, UserId, Region_Id, Is_Active);
                //FunPriLoadBranch(CompanyId, UserId, Is_Active);


                //ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID.ToString();
                //TextBox txt = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                //txt.Attributes.Add("onfocus", "fnLoadCustomer()");
                //pnlsummary.Visible = false;
                //PnlMemorandum.Visible = false;

                //pnlAssetDetails.Visible = false;
                //pnlTransactionDetails.Visible = false;

            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Page");
        }
    }
    #endregion

    #region Load LOB

    public void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = LOB;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "All";
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
            throw new ApplicationException("Unable to load LOB");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    #endregion

    public void FunPriLoadDocType(int CompanyId)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            ddlType.BindDataTable("S3G_RPT_GETDOCUMENTTYPE", dictParam, new string[] { "ID", "NAME" });
            dictParam = null;
        }
        catch(Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Document Type");
        }
    }

    public void FunPriLoadDocStatus(int CompanyId)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            ddlStatus.BindDataTable("S3G_RPT_GETDOCSTATUS", dictParam, new string[] { "ID", "NAME" });
            dictParam = null;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Document Status");
        }
    }

    public void FunPriValidateFromEndDate()
    {
        if ((!(string.IsNullOrEmpty(txtFrom.Text))) &&
           (!(string.IsNullOrEmpty(txtTo.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtFrom.Text) > Utility.StringToDate(txtTo.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                txtTo.Text = "";
                return;
            }
        }
        if ((!(string.IsNullOrEmpty(txtFrom.Text))) &&
           ((string.IsNullOrEmpty(txtTo.Text))))
        {
            txtTo.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtFrom.Text))) &&
            (!(string.IsNullOrEmpty(txtTo.Text))))
        {
            txtFrom.Text = txtTo.Text;

        }
    }

    private void FunPriBindGrid()
    {
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        try
        {
            DataSet ds = new DataSet();
            //divDemand.Style.Add("display", "block");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", ObjUserInfo.ProCompanyIdRW.ToString());
            dictParam.Add("@User_Id", ObjUserInfo.ProUserIdRW.ToString());
            dictParam.Add("@Program_Id", "243");
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            dictParam.Add("@Customer_Id", hdnCustomerId.Value);
            dictParam.Add("@Type_Id", ddlType.SelectedValue);
            dictParam.Add("@Status_Id", ddlStatus.SelectedValue);
            dictParam.Add("@FromDate", Utility.StringToDate(txtFrom.Text).ToString());
            dictParam.Add("@ToDate", Utility.StringToDate(txtTo.Text).ToString());
            //ds = Utility.GetDefaultData("RP_GET_INWARD_DET", dictParam);
            dtTable = Utility.GetDefaultData("S3G_RPT_GETDOCDEFICIENCY", dictParam);

            Session["DocDef"] = dtTable;
            pnlDemand.Visible = true;
            if (dtTable.Rows.Count > 0)
            {
                grvDemand.DataSource = dtTable;
                ViewState["DocPath"] = dtTable.Rows[0]["DOCPATH"].ToString();
                //ViewState["intPDransID"] = dtTable.Rows[0]["DOCTRANID"].ToString();
                grvDemand.DataBind();

                btnPrint.Visible = true;

                ds.Tables.Add(dtTable);
            }
            else
            {
                
                lblError.Text = "No Records Found";
                grvDemand.DataBind();
                grvDemand.Visible = false;
                btnPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    /// <summary>
    /// To Deseriliaze the service Object
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        if ((ddlLOB.SelectedValue == "-1") && (hdnCustomerId.Value == string.Empty))
        {
            Utility.FunShowAlertMsg(this.Page, "Select either LOB or Customer");
            return;
        }
        //btnPrint.Visible = false;
        try
        {
            FunPriValidateFromEndDate();
            FunPriBindGrid();
        }
        catch (Exception ex)
        {
            CVDocDef.ErrorMessage = "Due to Data Problem, Unable to Load Document Deficiency details Grid";
            CVDocDef.IsValid = false;
        }
    }

    protected void hyplnkView_Click(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((ImageButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvDemand", strFieldAtt);
            Label lblPath = (Label)grvDemand.Rows[gRowIndex].FindControl("lblPath");

            if (lblPath.Text.Trim() != ViewState["DocPath"].ToString().Trim())
            {
                string strFileName = lblPath.Text.Replace("\\", "/").Trim();
                string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "File not to be scanned yet");
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblError.Text = ex.Message;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlLOB.SelectedValue = "-1";
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        ucCustomerCodeLov.FunPubClearControlValue();
        string s = hdnCustomerId.Value;
        hdnCustID.Value = null;
        ddlType.SelectedValue = "0";
        ddlStatus.SelectedValue = "0";
        txtFrom.Text = "";
        txtTo.Text = "";
        grvDemand.DataSource = null;
        grvDemand.DataBind();
        pnlDemand.Visible = false;
        btnPrint.Visible = false;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strScipt = "window.open('../Reports/S3GRptDocumentDefReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DocumentDef", strScipt, true);
        }
        catch (Exception ex)
        {
            CVDocDef.ErrorMessage = "Due to Data Problem, Unable to Print the Report";
            CVDocDef.IsValid = false;
        }
    }
}
