using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Data;
using System.IO;

public partial class Reports_S3G_Rpt_LeadAnalysis :ApplyThemeForProject
{

    Dictionary<string, string> ObjDictionary = null;
    static string strPageName = "Lead Analysis Report";
    int intCompanyID, intUserID = 0;
    UserInfo ObjUserInfo = new UserInfo();

    protected void Page_Load(object sender, EventArgs e)
    {
           try
        {
            FunPriPageLoad();
        }
 catch (Exception ex)
 {
     CVVendorDetails.ErrorMessage = ex.Message;
     CVVendorDetails.IsValid = false;
 }
    }
    protected void FunProLoadLeadDetails()
    {


        ObjDictionary.Clear();
        ObjDictionary.Add("@Is_Active", "1");
        ObjDictionary.Add("@User_Id", intUserID.ToString());
        ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
        ObjDictionary.Add("@program_ID", "241");
        ddlLob.BindDataTable(SPNames.LOBMaster, ObjDictionary, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        ObjDictionary.Clear();
        ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
        ObjDictionary.Add("@LookupType_Code", "107");
        ddlsourcetype.BindDataTable("S3G_LOANAD_GetLookupTypeDescription", ObjDictionary, new string[] { "Lookup_Code", "Lookup_Description" });

       

    }

    private void FunPriPageLoad()
    {
        S3GSession ObjS3GSession = null;

        if (ObjDictionary != null)
            ObjDictionary.Clear();
        else
            ObjDictionary = new Dictionary<string, string>();
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            //Date Format
            ObjS3GSession = new S3GSession();
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            if (!IsPostBack)
            {
                FunProLoadLeadDetails();
                FunPriLoadLocation();
                PopulateType();
            }
           
        }
        catch (Exception ex)
        {
            ObjS3GSession = null;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjS3GSession = null;
        }
    }

    private void PopulateType()
    {
        try
        {
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataSet dsLookUp = Utility.GetDataset("S3G_CLN_GetFollowUp_LookUp", ObjDictionary);

            ddlleadstatus.DataSource = dsLookUp.Tables[5];
            ddlleadstatus.DataValueField = "Lookup_Code";
            ddlleadstatus.DataTextField = "Lookup_Description";
            ddlleadstatus.DataBind();
            ddlleadstatus.Items.Insert(0, new ListItem("--Select--", "0"));
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
            if (ObjDictionary != null)
                ObjDictionary.Clear();
            else
                ObjDictionary = new Dictionary<string, string>();

            ObjDictionary.Add("@Company_ID", Convert.ToString(intCompanyID));
            ObjDictionary.Add("@user_id", Convert.ToString(intUserID));

            ddllocation.BindDataTable("S3G_RPT_GET_Location", ObjDictionary, false, new string[] { "Location_code", "Location" });
            ddllocation.Items.Insert(0, "--Select--");
   
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            pnllead.Visible = true;
            DataTable dtrow = new DataTable();
            DataRow dr;
            DataColumn dc;
            dtrow.Columns.Add("Rows");
            string strrow="";
            ObjDictionary.Clear();
            
            ObjDictionary.Add("@User_Id", intUserID.ToString());
            ObjDictionary.Add("@Company_ID", intCompanyID.ToString());
            ObjDictionary.Add("@rtype_id", ddlRows.SelectedValue);
            ObjDictionary.Add("@ctype_id", ddlcolumn.SelectedValue);
            ObjDictionary.Add("@type", ddlType.SelectedValue);
            ObjDictionary.Add("@start_Date", Utility.StringToDate(txtStartDate.Text).ToString());
            ObjDictionary.Add("@end_Date", Utility.StringToDate(txtEndDate.Text).ToString());
            if(ddlleadstatus.SelectedIndex>0)
                ObjDictionary.Add("@LEAD_TYPE", ddlleadstatus.SelectedValue);
            DataSet ds = Utility.GetDataset("S3G_RPT_LEAD_ANALYSIS", ObjDictionary);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strrow = ds.Tables[0].Rows[i]["row_name"].ToString();

                    dr = dtrow.NewRow();
                    dr["Rows"] = strrow;
                    dtrow.Rows.Add(dr);
                }
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    strrow = ds.Tables[1].Rows[i]["cols_name"].ToString();

                    dtrow.Columns.Add(strrow);
                    
                }
            }
            if (dtrow.Rows.Count > 0)
            {
                for(int i=0; i<dtrow.Rows.Count;i++)
                {
                    foreach(DataColumn dcol in dtrow.Columns)
                    {
                        if (dcol.ColumnName.ToUpper() != "ROWS")
                        {
                            string strfilter = "rows='" + dtrow.Rows[i]["Rows"].ToString() + "' and columns='" + dcol.ColumnName + "'";
                            DataRow[] drr = ds.Tables[2].Select(strfilter);

                            if (drr.Length > 0)
                            {
                                string strvalu = drr[0]["value"].ToString();
                                dtrow.Rows[i][dcol.ColumnName] = strvalu;
                                dtrow.AcceptChanges();
                            }
                        }
                    }
                }
            }
            if (dtrow.Rows.Count > 0)
            {
                btnexcel.Visible = true;
                gvlead.DataSource = dtrow;
                gvlead.DataBind();
            }
            else
            {
                gvlead.DataSource = "No records Found";
                gvlead.DataBind();
            }
        
        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Due to Data Problem, Unable to Load Vendor Details Grid.";
            CVVendorDetails.IsValid = false;
        }


    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {

            ddllocation.SelectedIndex = 0;
            ddlLob.SelectedIndex = 0;
            ddlleadstatus.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlsourcetype.SelectedIndex = 0;
            ddlRows.SelectedIndex = 0;
            ddlcolumn.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            gvlead.DataSource = null;
            gvlead.DataBind();
            pnllead.Visible = false;

        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Due to Data Problem, Unable to Load Vendor Details Grid.";
            CVVendorDetails.IsValid = false;
        }


    }

    protected void gvlead_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }

    protected void FunExcelExport(object sender, EventArgs e)
    {
        //Changed by Thangam M on 08/Feb/2012 to solve nullable out param probs
        //FADALDBType FA_DBType;
        //FA_DBType = FunPubGetDatabaseType();
        //if (FA_DBType == FADALDBType.SQL)
        //{
        //    strProcName = "S3G_RPT_PROBABLEDELINQUENCY";
        //}
        //else
        //{
        //    strProcName = "S3G_LOANAD_GetIncomeRecognitionSpool";
        //}

        // Changes end

        //strProcName = "S3G_LOANAD_GetIncomeRecognition";


        string attachment = "attachment; filename=Lead Analysis.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.xls";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        
        gvlead.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }


}
