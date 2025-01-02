using ReportAccountsMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_S3GRptAssetRegisterReport : ApplyThemeForProject
{

    #region Variable Declaration

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intErrCount = 0;
    int intProgramId = 308;
    string strPageName = "Asset Register Report";
    DataSet dsassetregister;
    Dictionary<string, string> Procparam;
    public string strDateFormat;
    ReportAccountsMgtServicesClient objSerClient;
    public static Reports_S3GRptAssetRegisterReport obj_Page;
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
                FunPriLoadStatus();

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyId.ToString());
                Procparam.Add("@Option", "1");
                DataSet ds = Utility.GetDataset("S3G_ORG_ProposalLukup", Procparam);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)//Proposal Type
                    {
                        ddlProposalType.FillDataTable(ds.Tables[0], "Value", "Name", false);
                        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "0");
                        ddlProposalType.Items.Insert(0, liSelect);
                    }
                }
                ucCustomPaging.Visible = false;
                pnlAssetRegisterReportDetails.Style.Add("display", "none");
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

    private void FunPriLoadStatus()
    {
        Dictionary<string, string> dictassetrgstr = new Dictionary<string, string>();
        try
        {
            dictassetrgstr.Add("@Company_ID", intCompanyId.ToString());
            dictassetrgstr.Add("@User_ID", intUserId.ToString());            
            ddlAssetStatus.BindMemoDataTable("S3G_RPT_GetAssetRgstrRptStatus", dictassetrgstr, new string[] { "Value", "Name" });
        }
        catch (Exception ex)
        {
        }
    }

    private void FunPriBindGrid(int iExport)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != String.Empty)
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            else if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText == String.Empty)
                Procparam.Add("@Cust_ID", Convert.ToString("0"));
            else if (ddlLesseeName.SelectedValue != "0" && Convert.ToInt32(ddlLesseeName.SelectedValue) > 0 && ddlLesseeName.SelectedText == String.Empty)
                Procparam.Add("@Cust_ID", Convert.ToString("0"));
            else
                Procparam.Add("@Cust_ID", Convert.ToString("-1"));


            if (ddlRSNo.SelectedValue != "0" && ddlRSNo.SelectedText != String.Empty)
                Procparam.Add("@RS_NO", Convert.ToString(ddlRSNo.SelectedValue));
            else if (ddlRSNo.SelectedValue == "0" && ddlRSNo.SelectedText == String.Empty)
                Procparam.Add("@RS_NO", Convert.ToString("0"));
            else if (ddlRSNo.SelectedValue != "0" && Convert.ToInt32(ddlRSNo.SelectedValue) > 0 && ddlRSNo.SelectedText == String.Empty)
                Procparam.Add("@RS_NO", Convert.ToString("0"));
            else
                Procparam.Add("@RS_NO", Convert.ToString("-1"));

            if (ddlTrancheNo.SelectedValue != "0" && ddlTrancheNo.SelectedText != String.Empty)
                Procparam.Add("@TRANCHE_ID", Convert.ToString(ddlTrancheNo.SelectedValue));
            else if (ddlTrancheNo.SelectedValue == "0" && ddlTrancheNo.SelectedText == String.Empty)
                Procparam.Add("@TRANCHE_ID", Convert.ToString("0"));
            else if (ddlTrancheNo.SelectedValue != "0" && Convert.ToInt32(ddlTrancheNo.SelectedValue) > 0 && ddlTrancheNo.SelectedText == String.Empty)
                Procparam.Add("@TRANCHE_ID", Convert.ToString("0"));
            else
                Procparam.Add("@TRANCHE_ID", Convert.ToString("-1"));

            if (ddlAssetCategory.SelectedValue != "0" && ddlAssetCategory.SelectedText != String.Empty)
                Procparam.Add("@AC_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            else if (ddlAssetCategory.SelectedValue == "0" && ddlAssetCategory.SelectedText == String.Empty)
                Procparam.Add("@AC_ID", Convert.ToString("0"));
            else if (ddlAssetCategory.SelectedValue != "0" && Convert.ToInt32(ddlAssetCategory.SelectedValue) > 0 && ddlAssetCategory.SelectedText == String.Empty)
                Procparam.Add("@AC_ID", Convert.ToString("0"));
            else
                Procparam.Add("@AC_ID", Convert.ToString("-1"));

            if (!String.IsNullOrEmpty(txtStartDate.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));

            if (ddlAssetStatus.SelectedValue != "0")
                Procparam.Add("@ASSET_STATUS", Convert.ToString(ddlAssetStatus.SelectedValue));
            else
                Procparam.Add("@ASSET_STATUS", Convert.ToString("0"));

            if (ddlVendorInvoiceStatus.SelectedValue != "0")
                Procparam.Add("@VEND_INV_STATUS", Convert.ToString(ddlVendorInvoiceStatus.SelectedValue));
            else
                Procparam.Add("@VEND_INV_STATUS", Convert.ToString("0"));

            // Code Added for Call Id : 5262 CR_070

            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (ddlVendor.SelectedValue != "0")
                Procparam.Add("@Vendor_ID", Convert.ToString(ddlVendor.SelectedValue));

            if (ddlCustomersCustomerName.SelectedValue != "0")
                Procparam.Add("@EndUseCustomer_id", Convert.ToString(ddlCustomersCustomerName.SelectedValue));

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", ddlLocation.SelectedValue);
            else if (ddlLocation.SelectedValue == "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            Procparam.Add("@Export", Convert.ToString(iExport));                // Added By Senthilkumar P              15-Apr-2015         Bug From Client 

            if (ddlProposalType.SelectedValue != "0")
                Procparam.Add("@Proposal_Type", Convert.ToString(ddlProposalType.SelectedValue));
            else
                Procparam.Add("@Proposal_Type", Convert.ToString("0"));

            if ((ddlHSNCode.SelectedValue != "0") && (ddlHSNCode.SelectedText != ""))
                Procparam.Add("@HSN_ID", Convert.ToString(ddlHSNCode.SelectedValue));
            else
                Procparam.Add("@HSN_ID", Convert.ToString("0"));

            if ((ddlSACCode.SelectedValue != "0") && (ddlSACCode.SelectedText != ""))
                Procparam.Add("@SAC_ID", Convert.ToString(ddlSACCode.SelectedValue));
            else
                Procparam.Add("@SAC_ID", Convert.ToString("0"));

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
            pnlAssetRegisterReportDetails.Style.Add("display", "block");               
            grvAssetRegisterReportDetails.BindGridView_Report("S3G_RPT_Get_AssetRegister_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);


            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvAssetRegisterReportDetails.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            if (grvAssetRegisterReportDetails.Rows.Count > 0)
                grvAssetRegisterReportDetails.Visible = true;

            pnlAssetRegisterReportDetails.Visible = ucCustomPaging.Visible = true;
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
            Utility.FunShowAlertMsg(this, ex.Message);
            //lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            //objTaxGuideClient.Close();
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

    private void CheckDate()
    {
        if (txtStartDate.Text != String.Empty && txtEndDate.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtStartDate.Text, txtEndDate.Text);
            if (intErrCount == -1)
            {
                txtEndDate.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "Rental Sch Booking Date Range To Should Be Greater Than Rental Sch Booking Date Range From");
                txtEndDate.Focus();
                return;
            }
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
        FunPriBindGrid(0);
    }

    protected void btnClear_Click(Object sender, EventArgs e)
    {
        ddlLesseeName.Clear();
        ddlRSNo.Clear();
        ddlTrancheNo.Clear();
        ddlAssetCategory.Clear();
        ddlFunderName.Clear();
        ddlVendor.Clear();
        ddlCustomersCustomerName.Clear();
        ddlLocation.Clear();
        ddlAssetStatus.ClearSelection();
        ddlProposalType.ClearSelection();
        ddlVendorInvoiceStatus.ClearSelection();
        txtStartDate.Text = txtEndDate.Text = String.Empty;
        //grvAssetRegisterReportDetails.DataSource = null;
        //grvAssetRegisterReportDetails.DataBind();        
        grvAssetRegisterReportDetails.Visible = ucCustomPaging.Visible = btnExport.Visible = false;
        pnlAssetRegisterReportDetails.Style.Add("display", "none");  
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {       
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlLesseeName.SelectedValue != "0")
                Procparam.Add("@Cust_ID", Convert.ToString(ddlLesseeName.SelectedValue));
            else
                Procparam.Add("@Cust_ID", Convert.ToString("0"));

            if (ddlRSNo.SelectedValue != "0")
                Procparam.Add("@RS_NO", Convert.ToString(ddlRSNo.SelectedValue));
            else
                Procparam.Add("@RS_NO", Convert.ToString("0"));

            if (ddlTrancheNo.SelectedValue != "0")
                Procparam.Add("@TRANCHE_ID", Convert.ToString(ddlTrancheNo.SelectedValue));
            else
                Procparam.Add("@TRANCHE_ID", Convert.ToString("0"));

            if (ddlAssetCategory.SelectedValue != "0")
                Procparam.Add("@AC_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            else
                Procparam.Add("@AC_ID", Convert.ToString("0"));

            if (!String.IsNullOrEmpty(txtStartDate.Text))
                Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));

            if (ddlAssetStatus.SelectedValue != "0")
                Procparam.Add("@ASSET_STATUS", Convert.ToString(ddlAssetStatus.SelectedValue));
            else
                Procparam.Add("@ASSET_STATUS", Convert.ToString("0"));

            if (ddlVendorInvoiceStatus.SelectedValue != "0")
                Procparam.Add("@VEND_INV_STATUS", Convert.ToString(ddlVendorInvoiceStatus.SelectedValue));
            else
                Procparam.Add("@VEND_INV_STATUS", Convert.ToString("0"));

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));

            if (ddlFunderName.SelectedValue != "0")
                Procparam.Add("@Funder_ID", Convert.ToString(ddlFunderName.SelectedValue));

            if (ddlVendor.SelectedValue != "0")
                Procparam.Add("@Vendor_ID", Convert.ToString(ddlVendor.SelectedValue));

            if (ddlCustomersCustomerName.SelectedValue != "0")
                Procparam.Add("@EndUseCustomer_id", Convert.ToString(ddlCustomersCustomerName.SelectedValue));

            Procparam.Add("@Export", Convert.ToString("1"));                // Added By Senthilkumar P              15-Apr-2015         Bug From Client 

            if (ddlLocation.SelectedValue != "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", ddlLocation.SelectedValue);
            else if (ddlLocation.SelectedValue == "0" && ddlLocation.SelectedText != String.Empty)
                Procparam.Add("@location_id", "0");

            if (ddlProposalType.SelectedValue != "0")
                Procparam.Add("@Proposal_Type", Convert.ToString(ddlProposalType.SelectedValue));
            else
                Procparam.Add("@Proposal_Type", Convert.ToString("0"));

            if ((ddlHSNCode.SelectedValue != "0") && (ddlHSNCode.SelectedText != ""))
                Procparam.Add("@HSN_ID", Convert.ToString(ddlHSNCode.SelectedValue));
            else
                Procparam.Add("@HSN_ID", Convert.ToString("0"));

            if ((ddlSACCode.SelectedValue != "0") && (ddlSACCode.SelectedText != ""))
                Procparam.Add("@SAC_ID", Convert.ToString(ddlSACCode.SelectedValue));
            else
                Procparam.Add("@SAC_ID", Convert.ToString("0"));

            DataTable dtTable = new DataTable();
            dtTable = Utility.GetDefaultData_Report("S3G_RPT_Get_AssetRegister_Paging", Procparam);

            GridView Grv = new GridView();

            Grv.DataSource = dtTable;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 18px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                //string attachment = "attachment; filename=" + strPageName + ".xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.xlsx";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");
                dtHeader.Columns.Add("Column3");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Asset Register Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Line of Business : " + ddlLOB.SelectedItem.Text;

                if (ddlLesseeName.SelectedValue == "0")
                    row["Column2"] = "Lessee Name   : --All-- ";
                else
                    row["Column2"] = "Lessee Name   : " + ddlLesseeName.SelectedText;

                if (ddlProposalType.SelectedValue == "0")
                    row["Column3"] = "Proposal Type   : --All-- ";
                else
                    row["Column3"] = "Proposal Type   : " + ddlProposalType.SelectedItem.Text;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlRSNo.SelectedValue == "0")
                    row["Column1"] = "Rental Schedule No.    : --All-- ";
                else
                    row["Column1"] = "Rental Schedule No.   : " + ddlRSNo.SelectedText;

                if (ddlTrancheNo.SelectedValue == "0")
                    row["Column2"] = "Tranche No.    : --All-- ";
                else
                    row["Column2"] = "Tranche No.    : " + ddlTrancheNo.SelectedText;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlAssetCategory.SelectedValue == "0")
                    row["Column1"] = "Asset Category     : --All-- ";
                else
                    row["Column1"] = "Asset Category    : " + ddlAssetCategory.SelectedText;

                if (ddlAssetStatus.SelectedValue == "0")
                    row["Column2"] = "Asset Status    : --All-- ";
                else
                    row["Column2"] = "Asset Status     : " + ddlAssetStatus.SelectedItem.Text;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (txtStartDate.Text == string.Empty)
                    row["Column1"] = "Rental Sch Booking Date Range From  : ";
                else
                    row["Column1"] = "Rental Sch Booking Date Range From  : " + txtStartDate.Text;

                if (txtEndDate.Text == string.Empty)
                    row["Column2"] = "Rental Sch Booking Date Range To  : ";
                else
                    row["Column2"] = "Rental Sch Booking Date Range To  : " + txtEndDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlVendorInvoiceStatus.SelectedValue == "0")
                    row["Column1"] = "Vendor Invoice Status  : --All--";
                else
                    row["Column1"] = "Vendor Invoice Status  : " + ddlVendorInvoiceStatus.SelectedItem.Text;

                if (ddlFunderName.SelectedValue == "0")
                    row["Column2"] = "Funder Name    : --All-- ";
                else
                    row["Column2"] = "Funder Name     : " + ddlFunderName.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlVendor.SelectedValue == "0")
                    row["Column1"] = "Vendor Name    : --All-- ";
                else
                    row["Column1"] = "Vendor Name     : " + ddlVendor.SelectedText;

                if (ddlCustomersCustomerName.SelectedValue == "0")
                    row["Column2"] = "Customer's Customer Name     : --All-- ";
                else
                    row["Column2"] = "Customer's Customer Name     : " + ddlCustomersCustomerName.SelectedText;

                dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 12;
                grv1.Rows[1].Cells[0].ColumnSpan = 12;
                grv1.Rows[2].Cells[0].ColumnSpan = 4;
                grv1.Rows[3].Cells[0].ColumnSpan = 4;
                grv1.Rows[4].Cells[0].ColumnSpan = 4;
                grv1.Rows[5].Cells[0].ColumnSpan = 4;
                grv1.Rows[6].Cells[0].ColumnSpan = 4;
                grv1.Rows[7].Cells[0].ColumnSpan = 4;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[5].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[6].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[7].HorizontalAlign = HorizontalAlign.Left;

                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                //grv1.RenderControl(htw);

                //Grv.RenderControl(htw);
                //Response.Write(sw.ToString());
                //Response.End();

                string path = Server.MapPath("~/AssetRegisterReport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "AssetRegisterReport.xls"))
                {
                    File.Delete(path + "AssetRegisterReport.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "AssetRegisterReport.xls");
                        grv1.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/AssetRegisterRpt/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "AssetRegisterReport.zip";

                //before creation of compressed folder,deleting it if exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                //checking the path is available or not
                if (!File.Exists(filePath))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(path, filePath);

                    //Delete The excel file which is created
                    if (File.Exists(path + "AssetRegisterReport.xls"))
                    {
                        File.Delete(path + "AssetRegisterReport.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "AssetRegisterReport.zip");
                    Response.ContentType = "application/x-zip-compressed";
                    Response.WriteFile(filePath);
                    Response.End();
                }
            }
        }
        catch(Exception ex)
        {
            Utility.FunShowAlertMsg(this, ex.Message);
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

    //TRANCHE NAME 
    [System.Web.Services.WebMethod]
    public static string[] GetTrancheDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetTrancheDetails", dictparam));
        return suggetions.ToArray();
    }

    //Asset Category
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

    //Funders
    [System.Web.Services.WebMethod]
    public static string[] GetFunders(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETFunders", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendor(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetVendorName", dictparam));
        return suggetions.ToArray();
    }

    //Customer's Customer Name
    [System.Web.Services.WebMethod]
    public static string[] GetCustomersCustomerName(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETCustomersCustomerName", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        System.Data.DataTable dtCommon = new System.Data.DataTable();
        System.Data.DataSet Ds = new System.Data.DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "318");
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #endregion

    protected void grvAssetRegisterReportDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.ToLower() == "print")
        {
            GetPDFTrnacheFiles(e.CommandArgument.ToString());
        }
    }

    private void GetPDFTrnacheFiles(string strtranche)
    {
        try
        {
            strtranche = strtranche.Replace("\\", "/");

            FileInfo fl = new FileInfo(strtranche);
            if (fl.Exists == true)
            {
                string strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + strtranche + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "The Document has No pages");
                return;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grvAssetRegisterReportDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
        e.Row.Cells[2].Visible = false; 
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

    [System.Web.Services.WebMethod]
    public static string[] GetHSNCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_HSNCode", Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetSACCodeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
       
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_SYSAD_GET_SACCode", Procparam));

        return suggestions.ToArray();
    }

}
