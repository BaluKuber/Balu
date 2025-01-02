
#region -------------> NameSpaces
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Web.Security;
#endregion

/// ---------><summary>
/// Created By      : Rajendran
/// Purpose         : S3G Dash Board  | Work flow implementation
/// Created Date    : 2/2/2011
/// Last Modified   :    
/// Software Pattern  : Singletone Web Service Implementation
/// </summary>

public partial class HomePage : ApplyThemeForProject
{

    S3GAdminServicesReference.S3GAdminServicesClient ObjS3GAdminClient = new S3GAdminServicesReference.S3GAdminServicesClient();
    static string lDateFormate;

    #region " WORKFLOW EVENTS "
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo objS3GUser = new UserInfo();

        if (!IsPostBack)
        {
            AssisgnApplicationSessionValues();

            FunpriWorkflowBind('P');
        }
        CETo.Format = CEFrom.Format = (DateFormate != null) ? DateFormate : lDateFormate;

        if (objS3GUser.ProUserTypeRW.ToString().ToUpper() == "UTPA")
        {
            tblContainer.Visible = false;
        }
    }
    protected void ddlHome_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/HomePage.aspx");
    }
    protected void gvToDoList_SelectedIndexChanged(object sender, EventArgs e)
    {

        AssignSelectedWorkFlowRecord('G');
    }
    protected void gvListWFDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label WFStatus = (Label)gvListWFDocuments.SelectedRow.FindControl("lblStatus");
        if (WFStatus.Text.Length > 0 && WFStatus.Text.ToLower() == "open")
            AssignSelectedWorkFlowRecord('A');
        else
            Utility.FunShowAlertMsg(this, "Use left hand side menu to navigate to the corresponding screen");
    }
    protected void gvDocuments_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {


    }
    protected void grvTransLander_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void grvTransLander_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void grvTransLander_SelectedIndexChanged(object sender, EventArgs e)
    {
        //int selectedDocumentId = int.Parse(grvTransLander.SelectedDataKey.Value.ToString());
        //bindDocumentsList(selectedDocumentId);
    }
    protected void lbtnAll_Click(object sender, EventArgs e)
    {
        FunpriWorkflowBind('A');
    }
    protected void lbtnGW_Click(object sender, EventArgs e)
    {
        FunpriWorkflowBind('G');
    }

    protected void gvListWFDocuments_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string eventArg = "Select$" + e.Row.RowIndex.ToString();
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvListWFDocuments, eventArg));
            e.Row.Attributes.Add("onmouseover", "this.style.cursor='pointer';");
            //e.Row.Attributes.Add("onhover",e.Row.Style.cu );
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            //CheckBox chkSelected = (CheckBox)(e.Row.FindControl("chkStatus"));
            //chkSelected.Checked = (ViewState["ShowAll"] != null) ? (Boolean.Parse(ViewState["ShowAll"].ToString())) : false;
            //if (chkSelected.Checked)
            //    chkSelected.Text = "Show Pending";

        }
    }
    protected void chkStatus_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = true;
        FunpriWorkflowBind('A');
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = true;
        FunpriWorkflowBind('A');
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = true;
        FunpriWorkflowBind('A');
    }
    #endregion

    #region " HOMEPAGE INTERFACE METHODS"
    private static void AssisgnApplicationSessionValues()
    {
        UserInfo UserInfo = new UserInfo();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", UserInfo.ProCompanyIdRW.ToString());
        DataTable dtSessionInfo = Utility.GetDefaultData(SPNames.S3G_Get_GobalCompanyDetails, Procparam);

        if (dtSessionInfo.Rows.Count > 0)
        {
            string strDateformat = dtSessionInfo.Rows[0]["DISPLAY_DATE_FORMAT"].ToString();
            lDateFormate = strDateformat;
            string strCurrencyName = dtSessionInfo.Rows[0]["CURRENCY_NAME"].ToString();
            string strCurrencyCode = dtSessionInfo.Rows[0]["CURRENCY_CODE"].ToString();
            int intCurrencyId = Convert.ToInt32(dtSessionInfo.Rows[0]["CURRENCY_ID"].ToString());
            int intPINdigits = Convert.ToInt32(dtSessionInfo.Rows[0]["Pincode_Digits"].ToString());
            string strPINType = dtSessionInfo.Rows[0]["Pincode_Field_Type"].ToString();
            int intGpsPrefix = Convert.ToInt32(dtSessionInfo.Rows[0]["Currency_Max_Digit"].ToString());
            int intGpsSuffix = Convert.ToInt32(dtSessionInfo.Rows[0]["Currency_Max_Dec_Digit"].ToString());
            string strCompanyCntry = dtSessionInfo.Rows[0]["Country"].ToString();
            S3GSession ObjS3GSession = new S3GSession(strDateformat, strCurrencyName, strCurrencyCode, intCurrencyId, intPINdigits, strPINType, intGpsPrefix, intGpsSuffix, strCompanyCntry);
            //CultureInfo.CurrentCulture.D = strDateformat;
        }
    }
    private void FunpriWorkflowBind(Char ListType)
    {
        byte[] byte_ToDoList;
        if (ListType == 'A')
            //byte_ToDoList=ObjS3GAdminClient.FunPubGetWorkflowDocuments(int.Parse(CompanyId), int.Parse(UserId));
            byte_ToDoList = ObjS3GAdminClient.FunPubGetWorkflowToDoList(int.Parse(CompanyId), int.Parse(UserId), true, FromDate, ToDate);
        else
            byte_ToDoList = ObjS3GAdminClient.FunPubGetWorkflowToDoList(int.Parse(CompanyId), int.Parse(UserId), false, FromDate, ToDate);

        DataSet dsToDoList = (DataSet)S3GBusEntity.ClsPubSerialize.DeSerialize(byte_ToDoList, S3GBusEntity.SerializationMode.Binary, typeof(DataSet));

        gvListWFDocuments.DataSource = dsToDoList.Tables[0];
        gvListWFDocuments.DataBind();

        // HAS TO BE REMOVED FROM QUERY.... KR
        //ViewState["URL"]=dsToDoList.Tables[1];

    }
    void AssignSelectedWorkFlowRecord(char ListType)
    {
        //SessionValues WFSessionItems = new SessionValues();
        WorkFlowSession WFSessionItems = new WorkFlowSession();

        GridView GVDocuments;
        GVDocuments = gvListWFDocuments;

        WFSessionItems.WorkFlowStatusID = int.Parse(GVDocuments.SelectedDataKey.Values[0].ToString());
        WFSessionItems.WorkFlowProgramId = int.Parse(GVDocuments.SelectedDataKey.Values[1].ToString());
        WFSessionItems.WorkFlowDocumentNo = GVDocuments.SelectedDataKey.Values[2].ToString();

        WFSessionItems.LOBId = int.Parse(GVDocuments.SelectedDataKey.Values[3].ToString());
        WFSessionItems.BranchID = int.Parse(GVDocuments.SelectedDataKey.Values[4].ToString());
        WFSessionItems.ProductId = int.Parse(GVDocuments.SelectedDataKey.Values[5].ToString());
        WFSessionItems.LastDocumentNo = GVDocuments.SelectedDataKey.Values[6].ToString();
        WFSessionItems.WorkFlowSequence = GVDocuments.SelectedDataKey.Values[7].ToString();

        WFSessionItems.PANUM = GVDocuments.SelectedDataKey.Values[8].ToString();
        WFSessionItems.SANUM = GVDocuments.SelectedDataKey.Values[9].ToString();
        if (!string.IsNullOrEmpty(GVDocuments.SelectedDataKey.Values[10].ToString()))
            WFSessionItems.Document_Type = int.Parse(GVDocuments.SelectedDataKey.Values[10].ToString());
        else
            WFSessionItems.Document_Type = 0;//Instead of null we use 0
        LoadWorkFlowDocument(WFSessionItems);
    }
    DataTable LoadWorkFlowScreensList(string WFSequenceID)
    {
        //S3G_GEN_GetWorkFlowScreens
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        Procparam.Add("@WFSequence", WFSequenceID);
        DataTable WorkFlowScreens = Utility.GetDefaultData(SPNames.S3G_WORKFLOW_GetWorkFlowScreens, Procparam);
        return WorkFlowScreens;
    }
    void LoadWorkFlowDocument(WorkFlowSession sessionItems)
    {
        try
        {
            WorkFlowSession WFValues = new WorkFlowSession(); // sessionItems.SelecteDocument, sessionItems.SelectedProgramId, sessionItems.SelectedDocumentNo, sessionItems.BranchID, sessionItems.LOBId, sessionItems.ProductId, sessionItems.ReferenceDocNo);
            //WFValues.PANUM = "2011-2012/PRIME/591";
            //WFValues.SANUM = "2011-2012/PRIME/591DUMMY";
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(WFValues.WorkFlowDocumentNo, false, 0);

            WFValues.WorkFlowScreens = LoadWorkFlowScreensList(WFValues.WorkFlowSequence);
//reterive the Progrram code from the  available data table kept under session
            string ProgramCode = string.Empty;
            DataTable dtWFScreens = new DataTable();
            if (WFValues.WorkFlowScreens != null)
                dtWFScreens = WFValues.WorkFlowScreens;

            if (dtWFScreens.Rows.Count > 0)
            {
                DataRow[] dtRow = dtWFScreens.Select("Workflow_Prg_Id = " + WFValues.WorkFlowProgramId);
                if (dtRow.Length > 0)
                    ProgramCode = dtRow[0]["Program_Ref_No"].ToString();
            }


            //Incase we didn't receive the program Code then fetch it from database.
            if (string.IsNullOrEmpty(ProgramCode))
            {
                Dictionary<string, string> Procparam = new Dictionary<string, string>();
                Procparam.Add("@WFProgramId", WFValues.WorkFlowProgramId.ToString());
                // GET Program ID from WF PROGRAMID
                ProgramCode = Utility.GetTableScalarValue("S3G_GEN_GetProgramCodeFromWFProgram", Procparam);
                //S3GBusEntity.S3GLogger.LogMessage("Test message for Approval", "home page Load");
            }
            NavigateToWFURL(WFValues, Ticket, ProgramCode);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    private void NavigateToWFURL(WorkFlowSession WFValues, FormsAuthenticationTicket Ticket, string ProgramCode)
    {
        DataRow[] NavigationURL = WFValues.WorkFlowScreens.Select("Program_Ref_No=" + ProgramCode);

        if (NavigationURL.Length > 0) // URL EXISTS
        {
            switch (NavigationURL[0]["Module_Code"].ToString().ToLower())
            {
                case "o":
                    Response.Redirect("~/Origination/" + NavigationURL[0]["WFUrl"].ToString() + FormsAuthentication.Encrypt(Ticket) + "&qsMode=W");
                    break;
                case "l":
                    Response.Redirect("~/LoanAdmin/" + NavigationURL[0]["WFUrl"].ToString() + FormsAuthentication.Encrypt(Ticket) + "&qsMode=W");
                    break;
                case "c":
                    Response.Redirect("~/Collection/" + NavigationURL[0]["WFUrl"].ToString() + FormsAuthentication.Encrypt(Ticket) + "&qsMode=W");
                    break;
                default:
                    break;
            }

        }
    }
    #endregion

    #region "PROPERTIES"
    public DateTime? FromDate
    {
        get
        {
            return ((txtFromDate.Text.Trim().Length > 0) ? Utility.StringToDate(txtFromDate.Text) : DateTime.MaxValue);
        }
    }
    public DateTime? ToDate
    {
        get
        {
            return ((txtToDate.Text.Trim().Length > 0) ? Utility.StringToDate(txtToDate.Text) : DateTime.MaxValue);
        }
    }

    #endregion




    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        //CheckBox chkSelected = (CheckBox)(gvListWFDocuments.HeaderRow.FindControl("chkStatus"));
        if (chkShowAll.Checked)
        {
            ViewState["ShowAll"] = true;
            FunpriWorkflowBind('A');
        }
        else
        {
            ViewState["ShowAll"] = false;
            FunpriWorkflowBind('G');
        }
    }
}
