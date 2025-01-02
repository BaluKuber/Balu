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
using S3GBusEntity;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.IO;
using System.IO.Compression;
using System.Data.SqlClient;

public partial class Reports_S3G_Rpt_Projected_Rental :ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;
    bool Is_Active;
    int Active;
    int intProgramId = 310;
    public string strDateFormat;
    string Flag = string.Empty;
    //decimal decAltFC;
    Dictionary<string, string> Procparam;
    string strPageName = "Projected Rental Report";
    DataTable dtTable = new DataTable();
    public static Reports_S3G_Rpt_Projected_Rental obj_Page;
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
        FunPriLoadReport();
    }

    #endregion

    #region Page Load

    /// <summary>
    /// This event is handled for load the page
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            obj_Page = this;
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Projected Rental Report.";
            CVRepaymentSchedule.IsValid = false;
            throw ex;
        }
    }

    #endregion

    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intUserId = ObjUserInfo.ProUserIdRW;

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

            CalendarExtender1.Format = strDateFormat;
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");
            CalendarExtender2.Format = strDateFormat;
            txtenddate.Attributes.Add("onblur", "fnDoDate(this,'" + txtenddate.ClientID + "','" + strDateFormat + "',false,  false);");


            if (!IsPostBack)
            {
                ucCustomPaging.Visible = false;
                ListItem liSelect = new ListItem("--All--", "0");
                
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Projected Rental Report page");
        }
    }

  

    /// <summary>
    /// This Method is called after Clicking the Ok button.
    /// To Load the Repayment Details in Grid.
    /// </summary>
    /// <param name="PANum"></param>
    /// <param name="SANum"></param>
    private void FunPriLoadReport()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());

            if (txtenddate.Text != "")
                Procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", "-1");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText == "")
                Procparam.Add("@Customer_Id", "0");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue.ToString());

            if (ddlFunderName.SelectedValue != "0" && ddlFunderName.SelectedText != "")
                Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue.ToString());
            else
                Procparam.Add("@Funder_Id", ddlFunderName.SelectedValue.ToString());

            if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                Procparam.Add("@Location_Id", ddlState.SelectedValue.ToString());

            if (ddlTranche.SelectedValue != "0" && ddlTranche.SelectedText != "")
                Procparam.Add("@Tranche_Id", ddlTranche.SelectedValue.ToString());

            Procparam.Add("@Rental_Group", ddlRentalGroup.SelectedValue.ToString());

            if (ddlType.SelectedValue != "0")
                Procparam.Add("@Type", ddlType.SelectedValue.ToString());

            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyId;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            bool bIsNewRow = false;

            if (ddlType.SelectedValue == "0")
                grvPR.BindGridView("S3G_RPT_GetProjectedRentalReport", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            else
                grvPR.BindGridView("S3G_RPT_GetProjectedRentalReport_Unsold", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            if (bIsNewRow)
            {
                grvPR.Rows[0].Visible = btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            grvPR.Visible = ucCustomPaging.Visible = pnlPR.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    #region Page Events


    #region Button ( Ok / Clear / Print)

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtenddate.Text != "")
            {
                if (Utility.StringToDate(txtStartDate.Text) > Utility.StringToDate(txtenddate.Text)) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to Start Date");
                    return;
                }
            }

            FunPriLoadReport();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtStartDate.Text = String.Empty;
            ddlLesseeName.Clear();
            ddlFunderName.Clear();
            ddlTranche.Clear();
            ddlState.Clear();
            ddlType.SelectedValue = "0";
            txtenddate.Text = string.Empty;
            grvPR.DataSource = null;
            grvPR.DataBind();
            grvPR.Visible = ucCustomPaging.Visible = pnlPR.Visible = btnExport.Visible = false;
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string strConn = System.Configuration.ConfigurationSettings.AppSettings["DBPath"].ToString();
            SqlConnection objSqlConnection = new SqlConnection(strConn);

            DataSet ds = new DataSet();
            objSqlConnection.Open();

            SqlCommand cmd = new SqlCommand();

            if (ddlType.SelectedValue == "0")
                cmd = new SqlCommand("S3G_RPT_GetProjectedRentalReport", objSqlConnection);
            else
                cmd = new SqlCommand("S3G_RPT_GetProjectedRentalReport_Unsold", objSqlConnection);

            if (ddlType.SelectedValue != "0")
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);

            cmd.CommandTimeout = 8000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company_Id", intCompanyId.ToString());
            cmd.Parameters.AddWithValue("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());

            if (txtenddate.Text != "")
                cmd.Parameters.AddWithValue("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != "")
                cmd.Parameters.AddWithValue("@Customer_Id", "-1");  
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText == "")
                cmd.Parameters.AddWithValue("@Customer_Id", "0");  
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != "")
                cmd.Parameters.AddWithValue("@Customer_Id", ddlLesseeName.SelectedValue.ToString());

            if (ddlFunderName.SelectedValue != "0" && ddlFunderName.SelectedText != "")
                cmd.Parameters.AddWithValue("@Funder_Id", ddlFunderName.SelectedValue.ToString());
            else
                cmd.Parameters.AddWithValue("@Funder_Id", ddlFunderName.SelectedValue.ToString());

            if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                cmd.Parameters.AddWithValue("@Location_Id", ddlState.SelectedValue.ToString());

            if (ddlTranche.SelectedValue != "0" && ddlTranche.SelectedText != "")
                cmd.Parameters.AddWithValue("@Tranche_Id", ddlTranche.SelectedValue.ToString());

            cmd.Parameters.AddWithValue("@Rental_Group", ddlRentalGroup.SelectedValue.ToString());

            cmd.Parameters.AddWithValue("@IsExport", "1");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            objSqlConnection.Close();

            /*
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
            Procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            Procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            if (ddlLesseeName.SelectedValue == "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", "-1");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText == "")
                Procparam.Add("@Customer_Id", "0");
            else if (ddlLesseeName.SelectedValue != "0" && ddlLesseeName.SelectedText != "")
                Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue.ToString());

            //if (ddlLesseeName.SelectedValue != "0")
            //    Procparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);

            Procparam.Add("@IsExport", "1");
            */
            dtTable = new DataTable();
            dtTable = ds.Tables[0];

            //dtTable = Utility.GetDefaultData("S3G_RPT_GetProjectedRentalReport", Procparam);

            GridView Grv = new GridView();
            Grv.ForeColor = System.Drawing.Color.DarkBlue;
            Grv.Font.Name = "calibri";
            Grv.Font.Size = 10;

            Grv.DataSource = dtTable;
            Grv.DataBind();
            
            //Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            //Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                //string attachment = "attachment; filename=Rental Projection Report.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.xlsx";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();

                if (txtenddate.Text != "")
                    row["Column1"] = "Rental Projection Report between " + txtStartDate.Text + " to " + txtenddate.Text;        // Space added in header by Senthilkumar P 15-Apr-2015
                else
                    row["Column1"] = "Rental Projection Report From " + txtStartDate.Text;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlLesseeName.SelectedValue == "0")
                    row["Column1"] = "Lessee Name : --All--";
                else
                    row["Column1"] = "Lessee Name : " + ddlLesseeName.SelectedText;

                if (ddlFunderName.SelectedValue == "0")
                    row["Column1"] = row["Column1"].ToString() + "             " + "Funder Name : --All--";
                else
                    row["Column1"] = row["Column1"].ToString() + "             " + "Funder Name : " + ddlFunderName.SelectedText;

                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 9;
                grv1.Rows[1].Cells[0].ColumnSpan = 9;
                grv1.Rows[2].Cells[0].ColumnSpan = 9;
                grv1.Rows[3].Cells[0].ColumnSpan = 9;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;

                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;

                //grv1.RenderControl(htw);
                //Grv.RenderControl(htw);
                //Response.Write(sw.ToString());
                //Response.End();

                string path = Server.MapPath("~/RentalProjectionReport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "RentalProjectionReport.xls"))
                {
                    File.Delete(path + "RentalProjectionReport.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "RentalProjectionReport.xls");
                        grv1.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/RentalProjectionRpt/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "RentalProjectionReport.zip";

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
                    if (File.Exists(path + "RentalProjectionReport.xls"))
                    {
                        File.Delete(path + "RentalProjectionReport.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "RentalProjectionReport.zip");
                    Response.ContentType = "application/x-zip-compressed";
                    Response.WriteFile(filePath);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

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



    protected void grvPR_RowDataBound(object sender, GridViewRowEventArgs e)
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
        //if (ProgramCodeToCompare == strOperatingLeaseExpenses)
        //{ e.Row.Cells[1].Visible = false; }
        //else
        //{ e.Row.Cells[1].Visible = true; }


    }
 
    #endregion

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

    [System.Web.Services.WebMethod]
    public static string[] GetFunderList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Prefix_Text", prefixText);
        Procparam.Add("@Option", "2");

        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_RPT_GetLmtTrckLKP", Procparam));

        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();

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

    [System.Web.Services.WebMethod]
    public static string[] GetTrancheNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", dictparam));
        return suggetions.ToArray();
    }

    #endregion

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        btnExport.Visible = false;
    }
    protected void txtenddate_TextChanged(object sender, EventArgs e)
    {
        btnExport.Visible = false;
    }
}