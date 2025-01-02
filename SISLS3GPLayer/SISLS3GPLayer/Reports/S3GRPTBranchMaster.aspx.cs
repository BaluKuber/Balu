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

public partial class Reports_S3GRPTBranchMaster : ApplyThemeForProject
{
    #region Variable Declaration

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intErrCount = 0;
    int intProgramId = 325;
    string strPageName = "Branch Master Report";
    public string strDateFormat;
    ReportAccountsMgtServicesClient objSerClient;
    public static Reports_S3GRPTBranchMaster obj_Page;
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
                pnlBranchtDetails.Style.Add("display", "none");
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

    private void FunPriBindGrid()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlState.SelectedValue == "0" && ddlState.SelectedText != "")
                Procparam.Add("@State_ID", "-1");
            else if (ddlState.SelectedValue != "0" && ddlState.SelectedText == "")
                Procparam.Add("@State_ID", "0");
            else if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                Procparam.Add("@State_ID", ddlState.SelectedValue.ToString());

            // if (ddlState.SelectedValue != "0")
            //    Procparam.Add("@State_ID", Convert.ToString(ddlState.SelectedValue));
            //else
            //    Procparam.Add("@State_ID", Convert.ToString("0"));
         
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
            pnlBranchtDetails.Style.Add("display", "block");
            grvBranchtDetails.Visible = true;
            grvBranchtDetails.BindGridView("S3G_RPT_Get_BranchMaster", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);
           
            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvBranchtDetails.Rows[0].Visible = false;
                btnExport.Visible = false;
            }
            else
                btnExport.Visible = true;

            pnlBranchtDetails.Visible = ucCustomPaging.Visible = true;
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

    protected void grvBranchtDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[2].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;

       
    }


    protected void FunProExport(GridView Grv, string FileName)
    {
        try
        {
                Dictionary<string, string> Procparam = new Dictionary<string, string>();

                if (ddlState.SelectedValue == "0" && ddlState.SelectedText != "")
                    Procparam.Add("@State_ID", "-1");
                else if (ddlState.SelectedValue != "0" && ddlState.SelectedText == "")
                    Procparam.Add("@State_ID", "0");
                else if (ddlState.SelectedValue != "0" && ddlState.SelectedText != "")
                    Procparam.Add("@State_ID", ddlState.SelectedValue.ToString());

            // if (ddlState.SelectedValue != "0")
            //    Procparam.Add("@State_ID", Convert.ToString(ddlState.SelectedValue));
            //else
            //    Procparam.Add("@State_ID", Convert.ToString("0"));
            // Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
           

            //Paging Properties set

            int intTotalRecords = 0;
            
            //This is to show grid header
          DataTable dt = new DataTable();
          dt = Utility.GetDefaultData("[S3G_RPT_Get_BranchMaster]", Procparam);

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
                row["Column1"] = "Branch Master Report";
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                if (ddlState.SelectedValue == "0")
                    row["Column1"] = "State : All";
                else
                    row["Column1"] = "State : " + ddlState.SelectedText;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;


                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;


                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;
                grv1.RenderControl(htw);
                Grv.RenderControl(htw);

                ddlState.SelectedValue = "0";

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
        ddlState.Clear();
        grvBranchtDetails.Visible = ucCustomPaging.Visible = btnExport.Visible = false;
        pnlBranchtDetails.Style.Add("display", "none");
    }

    protected void btnExport_Click(Object sender, EventArgs e)
    {
        FunProExport(grvBranchtDetails, "Branch Master Report");
    }

    #endregion

    #region TEXT CHANGED EVENTS

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //txtEndDate.Text = String.Empty;
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