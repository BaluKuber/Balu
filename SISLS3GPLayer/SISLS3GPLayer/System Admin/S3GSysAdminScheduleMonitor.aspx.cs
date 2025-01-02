
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Scheduled Jobs
/// Screen Name         :   Scheduled Jobs 
/// Created By          :   Muni Kavitha    
/// Created Date        :   
/// Purpose             :   To save Scheduled Jobs details
/// Created By          :  Prabhu.K 
/// Created Date        :  20-dec-2011 
/// Purpose             :  To Develop the Scheduled Jobs as per new requirement
/// <Program Summary>
#endregion


#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Web.Security;
using System.IO;
using System.Xml;
using S3GBusEntity.ScheduledJobs;
using System.Globalization;
using System.Configuration;
using Resources;
using System.Collections;
using System.Collections.Generic;
#endregion


public partial class S3GSysAdminScheduleMonitor : ApplyThemeForProject
{
    #region Common Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = null;
    string strRedirectPageView = "S3GSysAdminScheduledJobsView.aspx";
    string strRedirectPageView1 = "window.location.href='../System Admin/S3GSysAdminScheduledJobsView.aspx';";
    string strDateFormat = string.Empty;
    static string strPageName = "Scheduled Jobs";

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
        try
        {
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

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
                Timer1.Enabled = true;
                FunPriBindGrid();
            }
        }
        catch (Exception ex)
        {
            cvScheduledJobs.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad + this.Page.Header.Title;
            cvScheduledJobs.IsValid = false;
        }

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        TextBox txtGotoPage = (TextBox)ucCustomPaging.FindControl("txtGotoPage");
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        AssignValue(Convert.ToInt32(txtGotoPage.Text), Convert.ToInt32(txtPageSize.Text));
        //FunPriBindGrid();
    }

    protected void FunProSetEmptyRow()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Location");
        dt.Columns.Add("Job");
        dt.Columns.Add("StartDate");
        dt.Columns.Add("EndDate");
        dt.Columns.Add("Process");

        DataRow dRow = dt.NewRow();
        dt.Rows.Add(dRow);

        dt.AcceptChanges();

        grvJobs.DataSource = dt;
        grvJobs.DataBind();
        grvJobs.Rows[0].Visible = false;

    }

    protected void grvJobs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image imgStatus = (Image)e.Row.FindControl("imgStatus");
            Label lblProcess = (Label)e.Row.FindControl("lblProcess");
            
            lblProcess.Visible = false;
            imgStatus.Visible = true;

            if (lblProcess.Text == "WIP")
            {
                imgStatus.ImageUrl = "~/Images/processing.gif";                
            }
            else if (lblProcess.Text == "Completed")
            {
                imgStatus.ImageUrl = "~/Images/Task_Complete.jpg";

                //imgStatus.ImageUrl = "~/Images/Task_pending.jpg";
                //imgStatus.Width = 20;

                //imgStatus.ImageUrl = "~/Images/processing.gif";
            }
            else
            {
                imgStatus.ImageUrl = "~/Images/Task_pending.jpg";
                imgStatus.Width = 20;
            }
        }
    }

    protected void FunProGetJobs()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@User_ID", intUserID.ToString());
        if (ddlStatus.SelectedValue != "0")
        {
            Procparam.Add("@Status", ddlStatus.SelectedValue);
        }

        DataTable dt = Utility.GetDefaultData("S3G_SCH_GetMonitor", Procparam);

        if (dt.Rows.Count > 0)
        {
            grvJobs.DataSource = dt;
            grvJobs.DataBind();

            trNoRecord.Visible = false;
        }
        else
        {
            FunProSetEmptyRow();

            trNoRecord.Visible = true;
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriBindGrid();
    }

    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            Procparam = new Dictionary<string, string>();
            if (ddlStatus.SelectedValue != "0")
            {
                Procparam.Add("@Status", ddlStatus.SelectedValue);
            }

            grvJobs.BindGridView("S3G_SCH_GetMonitor", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvJobs.Rows[0].Visible = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (grvJobs.Rows.Count > 0)
            {
                trNoRecord.Visible = false;
            }
            else
            {
                FunProSetEmptyRow();

                trNoRecord.Visible = true;
            }

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }
}
