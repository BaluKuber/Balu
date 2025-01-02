#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Purchase Order Print
/// Created By			: Chandrasekar K
/// Created Date		: 12-Jan-2014
/// <Program Summary>
#endregion

#region Name Spaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.Collections;

#endregion

#region Delivery Instruction / LPO
public partial class Origination_S3G_ORG_Tally_Integration : ApplyThemeForProject
{
    #region Intialization
    Dictionary<string, string> dictParam = null;
    string strDateFormat;
    int intErrCode;
    int intPO_Hdr_ID;
    int intUserId;
    int intCompanyId;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    int intRSSelecRows = 0;
    string strKey = "Insert";
    int intCompanyID, intUserID = 0;
    static string strPageName = "DeliveryInstruction / LPO";
    DataTable dtPO = new DataTable();
    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    Dictionary<string, string> Procparam_PO_PDF= new Dictionary<string, string>();
    UserInfo ObjUserInfo = new UserInfo();
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3G_ORG_Tally_Integration.aspx?qsMode=C';";
    string strExceptionEmail = "";
    string filePath_zip = "";
    SerializationMode SerMode = SerializationMode.Binary;
    ApplicationMgtServicesReference.ApplicationMgtServicesClient ObjTallyProcessSave;
    //ReportDocument rpd = new ReportDocument();

    public static Origination_S3G_ORG_Tally_Integration obj_Page;

    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }
    public int ProPageSizeRW
    {
        get;
        set;
    }

    #endregion

    #region Page Load

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        //if (rpd != null)
        //{
        //    rpd.Close();
        //    rpd.Dispose();
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        ProgramCode = "304";
        obj_Page = this;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);


        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bDelete = ObjUserInfo.ProDeleteRW;
        bQuery = ObjUserInfo.ProViewRW;

        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        ProPageNumRW = 1;                                                           // to set the default page number
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
        CalendarExtenderFromDate.Format = strDateFormat;
        CalendarExtenderToDate.Format = strDateFormat;
        txtPO_From_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_From_Date.ClientID + "','" + strDateFormat + "',false,  false);");
        txtPO_To_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtPO_To_Date.ClientID + "','" + strDateFormat + "',false,  false);");

        if (!IsPostBack)
        {
            RBLPOType.SelectedIndex = 0;
        }

        if (PageMode == PageModes.WorkFlow && !IsPostBack)
        {
            try
            {
                PreparePageForWFLoad();
            }
            catch (Exception ex)
            {

                Utility.FunShowAlertMsg(this, "Invalid data to load, access side menu");
            }
        }
    }

    private void PreparePageForWFLoad()
    {
        if (!IsPostBack)
        {
            WorkFlowSession WFSessionValues = new WorkFlowSession();

            DataRow HeaderValues = GetHeaderDetailsFromDOCNo(WFSessionValues.PANUM, ProgramCode);
        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindPOGrid();   // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    #endregion

    #region Button Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FunPriBindPOGrid();
    }
    
    protected void btnCancelPopup_Click(object sender, EventArgs e)
    {

    }

    protected void btnException_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (sender as LinkButton);
            GridViewRow row = (lnkView.NamingContainer as GridViewRow);
            string Tally_Integ_Hdr_ID = (row.FindControl("lblTally_Integ_Hdr_ID") as Label).Text;
            string strRSNO = (row.FindControl("lblPANum") as Label).Text;
            FunGetTallyExceptionDet_ID(strRSNO,Tally_Integ_Hdr_ID);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        
    }

    protected void FunGetTallyExceptionDet_ID(string RSNo, string Tally_Integ_Hdr_ID)
    {
        try
        {
            Dictionary<string, string> objParameters = new Dictionary<string, string>();

            StringBuilder strbXml = new StringBuilder();
            strbXml.Append("<Root>");
            strbXml.Append(" <Details ");
            strbXml.Append("PANum='" + RSNo + "' ");
            strbXml.Append(" /> ");
            strbXml.Append("</Root>");

            objParameters.Add("@XmlPANumDetails", strbXml.ToString());
            objParameters.Add("@Tally_Integ_Hdr_ID", Tally_Integ_Hdr_ID);

            DataSet dsExceptionDetails = Utility.GetDataset("S3G_Org_Get_Tally_Integration", objParameters);
            ViewState["ExportDataExcel"] = dsExceptionDetails.Tables[0];

            ExportToExcel(dsExceptionDetails.Tables[0], Tally_Integ_Hdr_ID);
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }

    }

    public void ExportToExcel(DataTable dtExportData, string Tally_Integ_Hdr_ID)
    {
        try
        {
            if (dtExportData.Rows.Count > 0)
            {
                string filename = "Tally_Integration" + "_" + Tally_Integ_Hdr_ID + ".xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                GridView dgGrid = new GridView();
                dgGrid.ForeColor = System.Drawing.Color.DarkBlue;
                dgGrid.Font.Name = "calibri";
                dgGrid.Font.Size = 10;
                dgGrid.DataSource = dtExportData;
                dgGrid.DataBind();
                dgGrid.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-weight: bold;");
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.            
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
      
    }

    protected void lnkUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (sender as LinkButton);
            GridViewRow row = (lnkView.NamingContainer as GridViewRow);
            string PO_Header_ID = (row.FindControl("lblPO_dtl_ID") as Label).Text;
            string PO_Type = (row.FindControl("lblPOType") as Label).Text;
            Session["VendorGroup"] = (row.FindControl("lblPO_Vendor_Group") as Label).Text;
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(PO_Header_ID, false, 0);
            string strPage = "S3G_ORG_PurchaseOrder_Add.aspx?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M&Popup=yes&POType="+ PO_Type;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "New", "window.open('" + strPage + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50')", true);
            return;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {

        }
    }

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        ExportToExcel(1);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtPO_From_Date.Text = txtPO_To_Date.Text = String.Empty;
        ddlCustomerName.Clear();
        ddlVendorName.Clear();
        ddlLoadSequenceNo.Clear();
        ddlPONo.Clear();
        pnlPO.Visible = false;
        gvPO.DataSource = null;
        gvPO.DataBind();
        btnExport1.Enabled = false;

    }

    public void ExportToExcel(int intOption)
    {
        try
        {
            Guid objGuid;
            objGuid = Guid.NewGuid();
            DataTable dtPO = new DataTable();

            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            if (!String.IsNullOrEmpty(txtPO_From_Date.Text))
                dictParam.Add("@From_Date", Utility.StringToDate(txtPO_From_Date.Text).ToString());
            if (!String.IsNullOrEmpty(txtPO_To_Date.Text))
                dictParam.Add("@To_Date", Utility.StringToDate(txtPO_To_Date.Text).ToString());
            if ((ddlCustomerName.SelectedValue != "0") && (ddlCustomerName.SelectedText != String.Empty))
                dictParam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
            if (ddlLoadSequenceNo.SelectedValue != "0" && ddlLoadSequenceNo.SelectedText != String.Empty)
                dictParam.Add("@Tranche_Id", ddlLoadSequenceNo.SelectedValue);
            if ((ddlPONo.SelectedValue != "0") && ddlPONo.SelectedText != String.Empty)
                dictParam.Add("@RS_No", ddlPONo.SelectedValue);
            
            dictParam.Add("@Option", "2");

            string strColValue = string.Empty;
            StringBuilder strbXml = new StringBuilder();
            strbXml.Append("<Root>");

            if (gvPO.Rows.Count > 0)
            {
                CheckBox chkSelectAll = gvPO.HeaderRow.FindControl("chkAll") as CheckBox;
                int intSelectedRentalCount = 0;

                if (!chkSelectAll.Checked)
                {
                    foreach (GridViewRow grv in gvPO.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelected") as CheckBox;

                            if (chkSelect.Checked)
                            {
                                strbXml.Append(" <Details ");
                                intSelectedRentalCount += 1;
                                Label lblPANum = grv.FindControl("lblPANum") as Label;
                                strColValue = lblPANum.Text;
                                strbXml.Append("PANum='" + strColValue + "' ");
                                strbXml.Append(" /> ");
                            }
                        }
                    }

                    if (intSelectedRentalCount == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Select atleast one record to Export");
                        return;
                    }
                }
                else
                {
                    foreach (GridViewRow grv in gvPO.Rows)
                    {
                        if (grv.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkSelect = grv.FindControl("chkSelected") as CheckBox;

                            if (chkSelect.Checked)
                            {
                                strbXml.Append(" <Details ");
                                intSelectedRentalCount += 1;
                                Label lblPANum = grv.FindControl("lblPANum") as Label;
                                strColValue = lblPANum.Text;
                                strbXml.Append("PANum='" + strColValue + "' ");
                                strbXml.Append(" /> ");
                            }
                        }
                    }
                }
                strbXml.Append("</Root>");

                dictParam.Add("@XmlPANumDetails", strbXml.ToString());

                dtPO = Utility.GetDefaultData("S3G_Org_Get_Tally_Integration", dictParam);

                string filename = "Tally_Integration_" + DateTime.Now.ToString() + ".xls";

                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dtPO;
                dgGrid.DataBind();
                dgGrid.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();

            }
        }
        catch (System.IO.IOException ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Error: Unable to Export PO");

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                chkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvPO.ClientID + "',this,'chkSelected');");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                if (lblStatus.Text == "Exception" || lblStatus.Text == "Completed")
                {
                    LinkButton LinkButton = (LinkButton)e.Row.FindControl("btnException");
                    LinkButton.Enabled = true;
                }

                if (lblStatus.Text == "Pending")
                {
                    gvPO.Columns[5].Visible = false;
                    gvPO.Columns[6].Visible = false;
                }
                else
                {
                    gvPO.Columns[5].Visible = true;
                    gvPO.Columns[6].Visible = true;
                }

            }
                
        }
        catch (Exception exp)
        {

        }
    }

    private void FunPriBindPOGrid()
    {
        if (txtPO_From_Date.Text == "" && txtPO_To_Date.Text == "" && (ddlCustomerName.SelectedValue == "0"
            || ddlCustomerName.SelectedText == String.Empty) && (ddlVendorName.SelectedValue == "0" || ddlVendorName.SelectedText == String.Empty)
            && (ddlLoadSequenceNo.SelectedValue == "0" || ddlLoadSequenceNo.SelectedText == String.Empty)
            && (ddlPONo.SelectedValue == "0" || ddlPONo.SelectedText == String.Empty)
            )
        {
            Utility.FunShowAlertMsg(this.Page, "Enter any one of input details");
            return;
        }

        dictParam = new Dictionary<string, string>();
        if (!String.IsNullOrEmpty(txtPO_From_Date.Text))
            dictParam.Add("@From_Date", Utility.StringToDate(txtPO_From_Date.Text).ToString());

        if (!String.IsNullOrEmpty(txtPO_To_Date.Text))
            dictParam.Add("@To_Date", Utility.StringToDate(txtPO_To_Date.Text).ToString());

        if (ddlCustomerName.SelectedValue != "0" && ddlCustomerName.SelectedText != String.Empty)
            dictParam.Add("@Customer_Id", ddlCustomerName.SelectedValue);
       
        if (ddlVendorName.SelectedValue != "0" && ddlVendorName.SelectedText != String.Empty)
            dictParam.Add("@Entity_ID", ddlVendorName.SelectedValue);
       
        if (ddlLoadSequenceNo.SelectedValue != "0" && ddlLoadSequenceNo.SelectedText != String.Empty)
            dictParam.Add("@Tranche_Id", ddlLoadSequenceNo.SelectedValue);

        if (ddlPONo.SelectedValue != "0" && ddlPONo.SelectedText != String.Empty)
            dictParam.Add("@RS_No", ddlPONo.SelectedValue);

        dictParam.Add("@Invoice_Status", ddlStatus.SelectedValue);

        dictParam.Add("@Option", "1");

        int intTotalRecords = 0;
        bool bIsNewRow = false;

        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProCompany_ID = intCompanyId;
        gvPO.BindGridView("S3G_Org_Get_Tally_Integration", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
        pnlPO.Visible = true;
        btnExport1.Visible = true;
        if (bIsNewRow == true)
        {
            gvPO.Rows[0].Visible = false;
            btnExport1.Enabled = false;
        }
        else
        {
            btnExport1.Enabled = true;
        }

        if (!bModify)
            gvPO.Columns[8].Visible = false;

        if(ddlStatus.SelectedValue=="1")
        {
            btnSave.Visible = true;
            btnRefresh.Visible = false;
            gvPO.Columns[8].Visible = false;
        }
        else
        {
            btnSave.Visible = false;
            btnRefresh.Visible = true;
            gvPO.Columns[8].Visible = true;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendors(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlVendorName.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetEntity_Master_AGT", Procparam));
        return suggestions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetRSNoDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetRSNO", dictparam));
        return suggetions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetCustomerNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();
        //added by vinodha m for the issue(invalid input,grid showing records)starts
        obj_Page.ddlCustomerName.SelectedValue = String.Empty;
        //added by vinodha m for the issue(invalid input,grid showing records)ends
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", Procparam), false);
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetTrancheDetails", dictparam));
        return suggetions.ToArray();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        try
        {
            FunPriSaveTallyDtls();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
           
        }
    }
        private void FunPriSaveTallyDtls()
    {
        try
        {
            ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrDataTable objTallyDataTable = new ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrDataTable();
            ApplicationMgtServices.S3G_ORG_Tally_Integ_HdrRow objTallyDataRow = objTallyDataTable.NewS3G_ORG_Tally_Integ_HdrRow();
            objTallyDataRow.Tally_Integ_Type =Convert.ToInt32(RBLPOType.SelectedValue);
            objTallyDataRow.XML_RS_Details = FunRSBuilder();
            if(objTallyDataRow.XML_RS_Details== "<ROOT></ROOT>")
            {
                Utility.FunShowAlertMsg(this, "Select atleast one record to post");
                return;
            }
            objTallyDataRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            objTallyDataRow.Created_By = ObjUserInfo.ProUserIdRW;

            objTallyDataTable.AddS3G_ORG_Tally_Integ_HdrRow(objTallyDataRow);

            ObjTallyProcessSave = new ApplicationMgtServicesReference.ApplicationMgtServicesClient();
            SerializationMode SMode = SerializationMode.Binary;

            string strMRANumber = string.Empty;
           
            int intErrCode = ObjTallyProcessSave.FunPubCreateTallyIntegDetails( SMode, ClsPubSerialize.Serialize(objTallyDataTable, SMode));

            switch (intErrCode)
            {
                case 0:
                    if (intErrCode == 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Purchase Invoice Details Scheduled successfully for Tally Insertion");
                        //strAlert = "Purchase Invoice Details Scheduled successfully for Tally Insertion";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        //lblErrorMessage.Text = string.Empty;
                        ddlStatus.SelectedValue = "0";
                        FunPriBindPOGrid();
                    }
                   
                    break;
                
                case 50:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;
            }


        }
        catch (Exception objException)
        {
            throw objException;
        }
        finally
        {
            ObjTallyProcessSave.Close();
        }
    }

    private string FunRSBuilder()
    {
        StringBuilder strRSEmailBuilder = new StringBuilder();
        strRSEmailBuilder.Append("<ROOT>");

        try
        {
            foreach (GridViewRow grvRS in gvPO.Rows)
            {
                CheckBox chkSelected = (CheckBox)grvRS.FindControl("chkSelected");
                Label lblPANum = (Label)grvRS.FindControl("lblPANum");
                
                if (chkSelected.Checked)
                {
                    intRSSelecRows = intRSSelecRows + 1;
                    strRSEmailBuilder.Append(" <DETAILS ");
                    strRSEmailBuilder.Append("RS_Number='" + lblPANum.Text + "'");
                    strRSEmailBuilder.Append(" ");
                    strRSEmailBuilder.Append("/>");
                }
            }
            strRSEmailBuilder.Append("</ROOT>");
            // return strbXml.ToString();
            return strRSEmailBuilder.ToString();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }



    ////protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    ////{
    ////    //gvPO.DataSource = null;
    ////    //gvPO.DataBind();
    ////    //FunPriBindPOGrid();
    ////    pnlPO.Visible = false;
    ////    if (ddlStatus.SelectedValue=="1")
    ////    {
    ////        btnSave.Visible = true;
    ////        btnRefresh.Visible = false;
    ////    }
    ////    else
    ////    {
    ////        btnSave.Visible = false;
    ////        btnRefresh.Visible = true;
    ////    }
    ////}


}

#endregion

