#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Escalation Details
/// Created By			: Kannan RC
/// Created Date		: 15-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-July-2010   
/// Reason              : Add Role Access setup 
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
#endregion

public partial class System_Admin_S3G_SysAdminEscalationMaster_View : ApplyThemeForProject
{
    #region Intialization
    //UserMgtServicesReference.UserMgtServicesClient ObjEscalationClient;
    S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable ObjS3G_EscalationMasterViewDataTable = new UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable();
    int intEscalationId = 0;
    string strRedirectPageAdd = "~/System Admin/S3G_SysAdminEscalationMaster_Add.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageMC;
    DataView dvSearchView;
    private Int32 intCount;
    string strSortColName;
    string qsMode;
    UserInfo ObjUserInfo = null;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    string strRedirectPage = "~/System Admin/S3G_SysAdminEscalationMaster_Add.aspx?qsMode=Escal";
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
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
        FunGetEscalationDetails();
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
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

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
        if (!IsPostBack)
        {

            FunGetEscalationDetails();
            if (!bIsActive)
            {
                gvEscalation.Columns[2].Visible = false;
                gvEscalation.Columns[1].Visible = false;
                btnCreate.Enabled = false;
            }

            if (!bModify)
            {
                gvEscalation.Columns[2].Visible = false;
            }
            if (!bQuery)
            {
                gvEscalation.Columns[1].Visible = false;
            }
            if (!bCreate)
            {
                btnCreate.Enabled = false;
            }



        }
    }

    /// <summary>
    /// Get Escalation details
    /// </summary>
    private void FunGetEscalationDetails()
    {
        try
        {

            UserMgtServices.S3G_SYSAD_Get_EscalationDetailsRow ObjEscalationMasterRow;
            ObjEscalationMasterRow = ObjS3G_EscalationMasterViewDataTable.NewS3G_SYSAD_Get_EscalationDetailsRow();
            ObjEscalationMasterRow.Escalation_ID = intEscalationId;
            ObjEscalationMasterRow.Company_ID = intCompanyID;
            //Paging Properties set


            int intTotalRecords = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //Paging code end

            // Commanded By Thangam M on 11-Aug-2011

            //ObjS3G_EscalationMasterViewDataTable.AddS3G_SYSAD_Get_EscalationDetailsRow(ObjEscalationMasterRow);
            //ObjEscalationClient = new UserMgtServicesReference.UserMgtServicesClient();
            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteEscalaionDetails = ObjEscalationClient.FunPubQueryEscalationMasterPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_EscalationMasterViewDataTable, SerMode), ObjPaging);
            //ObjS3G_EscalationMasterViewDataTable = (UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable)ClsPubSerialize.DeSerialize(byteEscalaionDetails, SerMode, typeof(UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable));



            //DataView dvEscalation = ObjS3G_EscalationMasterViewDataTable.DefaultView;



            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            //if (dvEscalation.Count == 0)
            //{
            //    //dvEscalation
            //    dvEscalation.AddNew();
            //    bIsNewRow = true;
            //}

            //gvEscalation.DataSource = dvEscalation;
            //gvEscalation.DataBind();

            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            gvEscalation.BindGridView("S3G_Get_Escalation_Paging", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvEscalation.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvEscalation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblEscalActive = (Label)e.Row.FindControl("lblEscalationActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("CbxEscalationActive");
            if (lblEscalActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnModify");
            if (lblUserID.Text != "")
            {
                //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker 
                //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text),true)))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";

                    //Authorization code end

                }
            }

        }
    }
    protected void gvEscalation_DataBound(object sender, EventArgs e)
    {
        if (gvEscalation.Rows.Count > 0)
        {
            gvEscalation.UseAccessibleHeader = true;
            gvEscalation.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvEscalation.FooterRow.TableSection = TableRowSection.TableFooter;
        }
    }
    /// <summary>
    /// On Click create new escalation details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/System Admin/S3G_SysAdminEscalationMaster_Add.aspx");
        Response.Redirect(strRedirectPageAdd);
    }
    /// <summary>
    /// show all the escalation records
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunGetEscalationDetails();
        }
        //}
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (gvEscalation.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            strSearchVal3 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            //strSearchVal4 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (gvEscalation.HeaderRow != null)
        {
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch2")).Text = "";
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (gvEscalation.HeaderRow != null)
        {
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            ((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)gvEscalation.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
        }
    }


    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

    public bool CheckBool(string str)
    {
        return ((str == "True") ? true : false);

    }
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                strSearchVal += "(SLM.LOB_CODE+' - '+SLM.LOB_Name) like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Role_Code  + ' - ' + Program_Name like '%" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and (UM.User_CODE+' - '+ UM.[User_Name]) like '%" + strSearchVal3 + "%'";
            }
            //if (strSearchVal4 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal4 + "%'";
            //}
            //if (strSearchVal5 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal5 + "%'";
            //}

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunGetEscalationDetails();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)gvEscalation.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// Gets the Sort Direction of the strColumn in the Grid View Using hidden control
    /// </summary>
    /// <param name="strColumn"> Colunm Name is Passed</param>
    /// <returns>Sort Direction as a String </returns>

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        if (strColumn == "LOB")
            strColumn = "SLM.LOB_CODE+' - '+SLM.LOB_Name";
        if (strColumn == "Users")
            strColumn = "UM.User_CODE+' - '+ UM.[User_Name]";

        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;

        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }

    /// <summary>
    /// Will Perform Sorting On Colunm base upon the link button id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnSortLOB":
                    strSortColName = "LOB";
                    break;
                case "lnkbtnSortRolecode":
                    strSortColName = "Role_Code";
                    break;
                case "lnkbtnUser":
                    strSortColName = "Users";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunGetEscalationDetails();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvEscalation.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvEscalation.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion


    /// <summary>
    /// passig escalation id using modify and query
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvEscalation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {


            case "modify":
                Response.Redirect(strRedirectPageAdd + "?qsEscalId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect(strRedirectPageAdd + "?qsEscalId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            GetDistanceAsync("600106","600083");
        }
        //}
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public async Task GetDistanceAsync(string origin, string destination)
    {
        //string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?key=AIzaSyD76RsJFYvlTD2Xxv1o8TqW9Ql_Lv7ph30&origins=" + origin + "&destinations=" + destination + "&sensor=false";

        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //WebResponse response = request.GetResponse();
        //Stream dataStream = response.GetResponseStream();
        //StreamReader sreader = new StreamReader(dataStream);
        //string responsereader = sreader.ReadToEnd();
        //response.Close();

        //DataSet ds = new DataSet();
        //ds.ReadXml(new XmlTextReader(new StringReader(responsereader)));
        //if (ds.Tables.Count > 0)
        //{
        //    if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
        //    {
        //        String strDuration=ds.Tables["duration"].Rows[0]["text"].ToString();
        //        String strDistance = ds.Tables["distance"].Rows[0]["text"].ToString();
        //    }
        //}

        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://redline-redline-zipcode.p.rapidapi.com/rest/multi-info.json/08057,08055/degrees"),
            Headers =
    {
        { "X-RapidAPI-Key", "237e01294emshada3f18fb2bdf0dp161b5ajsn7ca0f52d331a" },
        { "X-RapidAPI-Host", "redline-redline-zipcode.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

    }




}
