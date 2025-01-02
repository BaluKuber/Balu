using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.IO;
using System.Data;
using ReportAccountsMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Reports;

public partial class Reports_S3GRPTStatewiseSalesVAT : ApplyThemeForProject
{
    #region Variable Declaration

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intErrCount = 0;
    int intProgramId = 325;
    string strPageName = "StatewiseSales-VAT Report";
    public string strDateFormat;
    ReportAccountsMgtServicesClient objSerClient;
    public static Reports_S3GRPTStatewiseSalesVAT obj_Page;
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
        FunPriBindGrid();
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
           // CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            //txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
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
                //ddlLOB.Focus();
                //FunPriLoadLob();
                ucCustomPaging.Visible = false;
                pnlVATReportDetails.Style.Add("display", "none");
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }



    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    protected void grvVATReportDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;

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


    private void FunPriBindGrid()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlAssetCategory.SelectedValue == "0" && ddlAssetCategory.SelectedText != "")
                Procparam.Add("@AssetCategory_ID", "-1");
            else if (ddlAssetCategory.SelectedValue != "0" && ddlAssetCategory.SelectedText == "")
                Procparam.Add("@AssetCategory_ID", "0");
            else if (ddlAssetCategory.SelectedValue != "0" && ddlAssetCategory.SelectedText != "")
                Procparam.Add("@AssetCategory_ID", ddlAssetCategory.SelectedValue.ToString());

            if (ddlState.SelectedValue == "0" && ddlState.SelectedText != "")
                Procparam.Add("@State_ID", "-1");
            else if (ddlState.SelectedValue != "0" && ddlState.SelectedText == "")
                Procparam.Add("@State_ID", "0");
            else if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                Procparam.Add("@State_ID", ddlState.SelectedValue.ToString());

            if (ddlAssettype.SelectedValue == "0" && ddlAssettype.SelectedText != "")
                Procparam.Add("@AssetType_ID", "-1");
            else if (ddlAssettype.SelectedValue != "0" && ddlAssettype.SelectedText == "")
                Procparam.Add("@AssetType_ID", "0");
            else if (ddlAssettype.SelectedValue != "0" && ddlAssettype.SelectedText != "")
                Procparam.Add("@AssetType_ID", ddlAssettype.SelectedValue.ToString());


            //if (ddlAssetCategory.SelectedValue != "0")
            //    Procparam.Add("@AssetCategory_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            //else
            //    Procparam.Add("@AssetCategory_ID", Convert.ToString("0"));

            // if (ddlState.SelectedValue != "0")
            //    Procparam.Add("@State_ID", Convert.ToString(ddlState.SelectedValue));
            //else
            //    Procparam.Add("@State_ID", Convert.ToString("0"));

            // if (ddlAssettype.SelectedValue != "0")
            //     Procparam.Add("@AssetType_ID", Convert.ToString(ddlAssettype.SelectedValue));
            // else
            //     Procparam.Add("@AssetType_ID", Convert.ToString("0"));

            //if (!String.IsNullOrEmpty(txtStartDate.Text))
            //    Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));

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
            pnlVATReportDetails.Style.Add("display", "block");
            grvVATReportDetails.Visible = true;
            grvVATReportDetails.BindGridView("S3G_RPT_Get_StateWiseSalesRate", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);


            //for (int i = 0; i < grvVATReportDetails.Rows.Count; i++)
            //{
            //    grvVATReportDetails.Rows[i].HorizontalAlign = HorizontalAlign.Right;
            //}

            
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvVATReportDetails.Rows[0].Visible = false;
                btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            pnlVATReportDetails.Visible = ucCustomPaging.Visible = true;
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
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
        
            //if (ddlAssetCategory.SelectedValue != "0")
            //    Procparam.Add("@AssetCategory_ID", Convert.ToString(ddlAssetCategory.SelectedValue));
            //else
            //    Procparam.Add("@AssetCategory_ID", Convert.ToString("0"));

            // if (ddlState.SelectedValue != "0")
            //    Procparam.Add("@State_ID", Convert.ToString(ddlState.SelectedValue));
            //else
            //    Procparam.Add("@State_ID", Convert.ToString("0"));

            // if (ddlAssettype.SelectedValue != "0")
            //     Procparam.Add("@AssetType_ID", Convert.ToString(ddlAssettype.SelectedValue));
            // else
            //     Procparam.Add("@AssetType_ID", Convert.ToString("0"));

                if (ddlAssetCategory.SelectedValue == "0" && ddlAssetCategory.SelectedText != "")
                    Procparam.Add("@AssetCategory_ID", "-1");
                else if (ddlAssetCategory.SelectedValue != "0" && ddlAssetCategory.SelectedText == "")
                    Procparam.Add("@AssetCategory_ID", "0");
                else if (ddlAssetCategory.SelectedValue != "0" && ddlAssetCategory.SelectedText != "")
                    Procparam.Add("@AssetCategory_ID", ddlAssetCategory.SelectedValue.ToString());

                if (ddlState.SelectedValue == "0" && ddlState.SelectedText != "")
                    Procparam.Add("@State_ID", "-1");
                else if (ddlState.SelectedValue != "0" && ddlState.SelectedText == "")
                    Procparam.Add("@State_ID", "0");
                else if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                    Procparam.Add("@State_ID", ddlState.SelectedValue.ToString());

                if (ddlAssettype.SelectedValue == "0" && ddlAssettype.SelectedText != "")
                    Procparam.Add("@AssetType_ID", "-1");
                else if (ddlAssettype.SelectedValue != "0" && ddlAssettype.SelectedText == "")
                    Procparam.Add("@AssetType_ID", "0");
                else if (ddlAssettype.SelectedValue != "0" && ddlAssettype.SelectedText != "")
                    Procparam.Add("@AssetType_ID", ddlAssettype.SelectedValue.ToString());



            //if (!String.IsNullOrEmpty(txtStartDate.Text))
            //    Procparam.Add("@StartDate", Convert.ToDateTime(Utility.StringToDate(txtStartDate.Text)).ToString("yyyy/MM/dd"));

            if (!String.IsNullOrEmpty(txtEndDate.Text))
                Procparam.Add("@EndDate", Convert.ToDateTime(Utility.StringToDate(txtEndDate.Text)).ToString("yyyy/MM/dd"));

            //Paging Properties set

            int intTotalRecords = 0;
            
            //This is to show grid header
          DataTable dt = new DataTable();
              dt = Utility.GetDefaultData("[S3G_RPT_Get_StateWiseSalesRate]", Procparam);
              Grv.DataSource = dt;
              Grv.DataBind();
            if (Grv.Rows.Count > 0)
            {
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                string attachment = "attachment; filename=" + FileName + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.xlsx";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "State Wise Sales-VAT Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlState.SelectedValue == "0")
                    row["Column1"] = "State : All";
                else
                    row["Column1"] = "State : " + ddlState.SelectedText;

                if (ddlAssetCategory.SelectedValue == "0")
                    row["Column2"] = "Asset Category : All";
                else
                    row["Column2"] = "Asset Category : " + ddlAssetCategory.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlAssettype.SelectedValue == "0")
                    row["Column1"] = "Asset Type : All";
                else
                    row["Column1"] = "Asset Type : " + ddlAssettype.SelectedText;

                if (txtEndDate.Text == "")
                    row["Column2"] = "Upto Date : ";
                else
                    row["Column2"] = "Upto Date : " + txtEndDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;
                //grv1.Rows[2].Cells[0].ColumnSpan = 4;
                //grv1.Rows[3].Cells[0].ColumnSpan = 4;
                //grv1.Rows[4].Cells[0].ColumnSpan = 4;
                //grv1.Rows[5].Cells[0].ColumnSpan = 4;
                //grv1.Rows[6].Cells[0].ColumnSpan = 4;


                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                //for (int i = 0; i < grv1.Rows.Count; i++)
                //{
                //    grv1.Columns[5][i].HorizontalAlign = HorizontalAlign.Right;
                //}
                //grv1.Rows[5].HorizontalAlign = HorizontalAlign.Left;
             
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);

                ddlState.SelectedValue = "0";
                ddlAssetCategory.SelectedValue = "0";
                ddlAssettype.SelectedValue = "0";

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
        //if (txtStartDate.Text != String.Empty && txtEndDate.Text != String.Empty)
        //{
        //    intErrCount = Utility.CompareDates(txtStartDate.Text, txtEndDate.Text);
        //    if (intErrCount == -1)
        //    {
        //        txtEndDate.Text = String.Empty;
        //        Utility.FunShowAlertMsg(this, "End Date Should Be Greater Than Start Date");
        //        txtEndDate.Focus();
        //        return;
        //    }
        //}
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnGo_Click(Object sender, EventArgs e)
    {
        FunPriBindGrid();
    }

    protected void btnClear_Click(Object sender, EventArgs e)
    {
        ddlAssetCategory.Clear();
        ddlState.Clear();
        ddlAssettype.Clear();
       // txtStartDate.Text = txtEndDate.Text = String.Empty;
        txtEndDate.Text = String.Empty;
        grvVATReportDetails.Visible = ucCustomPaging.Visible = btnExport.Visible = false;
        pnlVATReportDetails.Style.Add("display", "none");
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {
        FunProExport(grvVATReportDetails, "State Wise Sales-VAT Report");
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

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    #region FETCH METHODS FOR AUTO SUGGEST CONTROLS


    //AssetCategory
    [System.Web.Services.WebMethod]
    public static string[] GetAssetCategoryDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        //DataSet dsInitTaxTypeDetails;
        int taxtype = 0;
        //DataSet dsInitLoadDetails;
        List<String> suggetions = new List<String>();
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        Procparam.Add("@Option", "1");
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));
        
        return suggetions.ToArray();
          
    }
    /// <summary>
    /// GetAssettypeList
    /// </summary>
    /// <param name="prefixText">search text</param>
    /// <param name="count">no of matches to display</param>
    /// <returns>string[] of matching names</returns>
    [System.Web.Services.WebMethod]
    public static string[] GetAssettypeList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyId.ToString()));
        Procparam.Add("@Option", "2");
        Procparam.Add("@PrefixText", prefixText);
     
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GETTAXGUIDE_TRANSVALUES", Procparam));
        return suggetions.ToArray();
    }

    //StateDetails
    [System.Web.Services.WebMethod]
    public static string[] GetStateDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetStateName", dictparam));
        return suggetions.ToArray();
    }

    #endregion
}