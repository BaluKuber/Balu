using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AjaxControlToolkit;
using System.Data;
using System.Web.Security;
using System.Text;
using S3GBusEntity;
using System.Xml;
using System.IO;


/// <summary>
/// Summary description for ApplyThemeForProject
/// </summary>
public class ApplyThemeForProject : System.Web.UI.Page
{
    public ApplyThemeForProject()
    {
        if (ObjUserInfo == null)
        {
            ObjUserInfo = new UserInfo();

        }
        if (ObjS3GSession == null)
        {
            ObjS3GSession = new S3GSession();
        }

    }

    #region " Properties"

    // Common Properties Used Across the application
    public PageModes PageMode { get; set; }
    public string PageIdValue { get; set; }
    public String UserId { get; set; }
    public string CompanyId { get; set; }
    public string FinancialYear { get; set; }
    public string FinancialMonth { get; set; }
    public string PostBackControlId { get; set; }
    public string DateFormate { get; set; }
    public string RedirectOnCancel { get; set; }

    // Work Flow Properties
    public string ProgramCode { get; set; }
    public string WorkFlowId { get; set; }
    public string WorkFlowSequence { get; set; }
    public int WFProgramCode { get; set; }
    public int CurrentProgramFlowNumber { get; set; }
    public int NextProgramFlowNumber { get; set; }
    public string DocumentControlNo { get; set; }

    public bool isWorkFlowTraveler { get; set; }


    #endregion

    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession;

    public enum PageModes
    {
        Create,
        Modify,
        Query,
        Delete,
        WorkFlow
    }

    #region "Page Values Initialization "
    private void SetPageMode(string queryMode)
    {
        switch (queryMode.ToLower())
        {
            case "c":
                PageMode = PageModes.Create;
                break;
            case "q":
                PageMode = PageModes.Query;
                break;
            case "m":
                PageMode = PageModes.Modify;
                break;
            case "d":
                PageMode = PageModes.Delete;
                break;
            case "w":
                PageMode = PageModes.WorkFlow;
                break;
            default:
                break;
        }
    }
    #endregion

    #region " Page Level Events"

    /// <summary>
    /// Page Pre Init Event to Initialize the Page Level Values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (ObjUserInfo.ProCompanyIdRW == 0 && ObjUserInfo.ProUserNameRW != "S3GUSER")
            HttpContext.Current.Response.Redirect("../SessionExpired.aspx");

        if (Request.QueryString["qsMode"] != null)
        {
            //_pageMode = Request.QueryString["qsMode"];             
            SetPageMode(Request.QueryString["qsMode"].ToString());
        }

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket.Name != null)
                PageIdValue = fromTicket.Name;
        }
        if (Request.QueryString["qsWorkFlow"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsWorkFlow"));
            if (fromTicket.Name != null)
                isWorkFlowTraveler = true;
            WorkFlowId = fromTicket.Name;

            SetPageMode("w");
        }

        // CPT Setup Master Page 
        if (Request.QueryString["Popup"] != null)
            Page.MasterPageFile = "~/Common/MasterPage.master";
        else if (Request.QueryString["IsFromAccount"] != null)
            Page.MasterPageFile = "~/Common/MasterPage.master";
        else
            Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";


        // Setup Page Theme
        Page.Theme = ObjUserInfo.ProUserThemeRW;

        // Initialize from User Information
        CompanyId = ObjUserInfo.ProCompanyIdRW.ToString();
        UserId = ObjUserInfo.ProUserIdRW.ToString();
        // Initialize Date Formate
        DateFormate = ObjS3GSession.ProDateFormatRW; //"dd/MM/yyyy";

        DropDownList d = (DropDownList)Page.Master.FindControl("ddlTheme");
        if (d != null)
            d.SelectedValue = ObjUserInfo.ProUserThemeRW;

        if (!String.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
        {
            PostBackControlId = Request.Form["__EVENTTARGET"];
        }

        #region To Set FinanicalYear & Month as Property
        int intCurrentYear = DateTime.Now.Year;
        int intCurrentMonth = DateTime.Now.Month;
        int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
        if (intCurrentMonth > (intActualMonth - 1))
        {
            FinancialYear = (intCurrentYear).ToString() + "-" + (intCurrentYear + 1).ToString();

        }
        else
        {
            FinancialYear = (intCurrentYear - 1).ToString() + "-" + (intCurrentYear).ToString();

        }
        FinancialMonth = intCurrentYear.ToString() + intCurrentMonth.ToString("00");
        #endregion

        //KeepSessionAlive();
    }
    #endregion

    #region "Clear Page"
    // to enable\disable the control & its child controls        
    public void DisableControl(Control control, bool isDisable)
    {
        foreach (Control c in control.Controls) try
            {
                if (c is WebControl)
                    ((WebControl)c).Enabled = !isDisable;
                if (c.HasControls())
                    DisableControl(c, isDisable);
            }
            catch (Exception) { }
    }
    public virtual void ClearControlValues(Control control)
    {
        foreach (Control formControl in control.Controls) try
            {
                if (formControl is WebControl)
                {

                    switch (((System.Reflection.MemberInfo)(formControl.GetType())).Name.ToLower())
                    {
                        case "textbox":
                            ((TextBox)formControl).Text = "";
                            break;
                        case "dropdownlist":
                            if (((DropDownList)formControl).Enabled)
                                ((DropDownList)formControl).SelectedIndex = -1;
                            break;
                        case "checkbox":
                            if (((CheckBox)formControl).Checked)
                                ((CheckBox)formControl).Checked = false;
                            break;
                        default:
                            break;
                    }
                }
                if (formControl.HasControls())
                    ClearControlValues(formControl);

            }
            catch (Exception) { }
    }
    public virtual void ClearControlValues(Control control, bool isClearDropDownRequired)
    {
        foreach (Control formControl in control.Controls) try
            {
                if (formControl is WebControl)
                {

                    switch (((System.Reflection.MemberInfo)(formControl.GetType())).Name.ToLower())
                    {
                        case "textbox":
                            ((TextBox)formControl).Text = "";
                            break;
                        case "dropdownlist":
                            if (((DropDownList)formControl).Enabled)
                                if (isClearDropDownRequired && ((DropDownList)formControl).AccessKey != "N") ((DropDownList)formControl).Items.Clear();
                            break;
                        case "checkbox":
                            if (((CheckBox)formControl).Checked)
                                ((CheckBox)formControl).Checked = false;
                            break;
                        default:
                            break;
                    }
                }
                if (formControl.HasControls())
                    ClearControlValues(formControl);

            }
            catch (Exception) { }
    }
    public virtual void SetControlValuesReadOnly(Control control)
    {
        foreach (Control formControl in control.Controls) try
            {
                if (formControl is WebControl)
                {

                    switch (((System.Reflection.MemberInfo)(formControl.GetType())).Name.ToLower())
                    {
                        case "textbox":
                            ((TextBox)formControl).Attributes.Add("ContentEditable", "False");
                            break;
                        case "dropdownlist":
                            if (((DropDownList)formControl).Enabled)
                                ((DropDownList)formControl).Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
                //if (formControl.HasControls())
                //    ClearControlValues(formControl);

            }
            catch (Exception) { }
    }
    public virtual void ClearGridValues(Control control)
    {
        foreach (Control formControl in control.Controls) try
            {
                if (formControl is WebControl)
                {

                    switch (((System.Reflection.MemberInfo)(formControl.GetType())).Name)
                    {
                        case "GridView":
                            if (((GridView)formControl).Enabled)
                            {
                                ((GridView)formControl).DataSource = null;
                                ((GridView)formControl).DataBind();
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (formControl.HasControls())
                    ClearGridValues(formControl);

            }
            catch (Exception) { }
    }
    #endregion


    //string _pageMode=string.Empty;
    int _idValue = 0;
    //public string PageMode { 
    //    get {return _pageMode;} set{_pageMode=value;} }
    public int IdValue { get { return _idValue; } set { _idValue = value; } }

    public enum enumPageTitle
    {
        PageTitle,
        Create,
        Modify,
        View,
        Details,
        Schedule,
        Approve
    }

    /// <summary>
    /// Get the Title of the module
    /// </summary>
    /// <param name="ePageTitle"></param>
    /// <returns></returns>
    public string FunPubGetPageTitles(enumPageTitle ePageTitle)
    {
        SiteMapPath s1 = (SiteMapPath)this.Master.FindControl("SiteMapPath1");
        string strValue = s1.Provider.CurrentNode.Description;

        /*if (ePageTitle == enumPageTitle.PageTitle)
        {
            strValue = "S3G - " + strValue;
        }
        else */

        if (ePageTitle == enumPageTitle.Create)
        {
            strValue = strValue + " - Create";
        }
        else if (ePageTitle == enumPageTitle.Modify)
        {
            strValue = strValue + " - Modify";
        }
        else if (ePageTitle == enumPageTitle.View)
        {
            strValue = strValue + " - View";
        }
        else if (ePageTitle == enumPageTitle.Details)
        {
            strValue = strValue + " - Details";
        }

        return strValue;
    }

    public void FunPubGetPageUrl(string strPageUrl, out int intModuleID, out int intProgramID)
    {
        string strPageName;
        string strModuleName;

        UserInfo ObjUserInfo = new UserInfo();

        DataSet ds_Menu = new DataSet();
        DataTable dtMenu;
        DataView dvSearch;

        ds_Menu.ReadXml(Server.MapPath(@"~\Data\UserManagement\" + ObjUserInfo.ProUserIdRW.ToString() + ".xml"));

        //  Get PageName
        strPageName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);

        //  Get the Remaining Page URL Except PageName
        strPageUrl = strPageUrl.Replace(strPageName, "");
        strPageUrl = strPageUrl.TrimEnd('/');

        //  Get ModuleName
        strModuleName = strPageUrl.Substring(strPageUrl.LastIndexOf("/") + 1);


        dtMenu = ds_Menu.Tables["Module_Header"];
        dvSearch = new DataView(dtMenu);
        dvSearch.RowFilter = "Module_Name LIKE '" + strModuleName + "'";
        intModuleID = Convert.ToInt32(dvSearch[0]["Module_ID"].ToString());

        strPageName = strPageName.Replace("_Add", "_View");
        dtMenu = ds_Menu.Tables[strModuleName];
        dvSearch = new System.Data.DataView(dtMenu);
        dvSearch.RowFilter = "PAGE_URL LIKE '" + strPageName + "'";
        //intProgramID = Convert.ToInt32(dvSearch[0]["Program_ID"].ToString());
        intProgramID = 34;
    }


    public int GetUniqueIdFromDOCNo(string PageIdValue, string ProgramCode)
    {
        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@DocSeqNo", PageIdValue);
        Procparam.Add("@ProgramCode", ProgramCode);
        int returnValue = 0;
        if (int.TryParse(Utility.GetTableScalarValue("S3G_GEN_GetUniqueIdFromDOCNo", Procparam), out returnValue)) ;
        if (returnValue == 0)
        {
            Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_WF_NoRecords.ToString());
        }
        return returnValue;
    }
    public DataRow GetHeaderDetailsFromDOCNo(string PageIdValue, string ProgramCode)
    {
        DataTable HeaderDetails = new DataTable();
        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@DocumentSequenceNo", PageIdValue);
        Procparam.Add("@Program_Code", ProgramCode);
        HeaderDetails = Utility.GetDefaultData("S3G_WORKFLOW_GETHEADERVALUES", Procparam);
        if (HeaderDetails.Rows.Count > 0)
            return HeaderDetails.Rows[0];
        else
        {
            Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_WF_NoRecords.ToString());
            return null;
        }
    }
    public DataTable GetHeaderDetailsFromPANUMandSANUM(string PANum, string ProgramCode, string SANum)
    {
        DataTable HeaderDetails = new DataTable();
        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", CompanyId);
        Procparam.Add("@PANum", PANum);
        if (SANum != null && SANum.Length > 0)
            Procparam.Add("@SANum", SANum);
        HeaderDetails = Utility.GetDefaultData("S3G_WORKFLOW_PAYMENTREQUEST", Procparam);
        if (HeaderDetails.Rows.Count > 0)
            return HeaderDetails;
        else
        {
            Utility.FunShowAlertMsg(this, Resources.ValidationMsgs.S3G_WF_NoRecords.ToString());
            return null;
        }
    }

    public void ShowWFAlertMessage(string DocumentSequnce, string ProgramCode,string WorkFlowMsg)
    {
        StringBuilder AlertMessage = new StringBuilder();
        string DocumentName;
        string strRedirectPageView = "window.location.href='" + Utility.NavigateToWFURL(out DocumentName, Utility.S3GModules.Orgination, ProgramCode, DocumentSequnce) + "';";

        if (DocumentSequnce.Length > 0)
        {
            AlertMessage.Append(DocumentName + " (" + DocumentSequnce + ")  saved successfully" + @"\n\n" + WorkFlowMsg + "");
            //AlertMessage += @"\n\n" + WorkFlowMsg + "";
        }
        else
        {
            AlertMessage.Append(DocumentName + "  saved successfully");
            //AlertMessage += @"\n\n" + WorkFlowMsg + "";
        }
        Utility.FunShowAlertMsg(this, AlertMessage.ToString());

        string strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";

        //AlertMessage.Append("if(confirm('" + ss + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShWFMsg", strRedirectPageAdd, true);
    }

    public void ReturnToWFHome()
    {
        string strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShWFMsg", strRedirectPageAdd, true);
    }

    public int CheckForWorkFlowConfiguration(string pProgramRefCode, int pLineofBusiness, int pProductCode, out int WFProgramID, out DataTable dtWorkFlow)
    {
        Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
        if (CompanyId == null)
            CompanyId = ObjUserInfo.ProCompanyIdRW.ToString();
        ObjDictParam.Add("@Company_ID", CompanyId);
        ObjDictParam.Add("@LOB_ID", pLineofBusiness.ToString());
        ObjDictParam.Add("@Product_ID", pProductCode.ToString());
        ObjDictParam.Add("@Program_ID", pProgramRefCode);

        dtWorkFlow = Utility.GetDefaultData(S3GBusEntity.SPNames.S3G_WORKFLOW_IsWorkFlowConfigured, ObjDictParam);
        DataRow drWorkflow;

        DataRow[] dtrRows = dtWorkFlow.Select("Program_Ref_No=" + pProgramRefCode);
        drWorkflow = dtrRows[0];

        int intErrorCode = Convert.ToInt32(drWorkflow["Workflow_id"]);

        WFProgramID = -1;
        if (intErrorCode > 0)
            WFProgramID = int.Parse(drWorkflow["Workflow_Prg_Id"].ToString());

        return intErrorCode;

    }

    public bool CheckForForcePullOperation(int? DocumentId, string DocumentSeqNo, string PrgCode, string PANum, string SANum, string Module, string CompanyId, int? LOBId, int? ProductId, string LOBName, string ProductName, out DataTable dtWorkFlow)
    {
        Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
        if (CompanyId == null)
            CompanyId = ObjUserInfo.ProCompanyIdRW.ToString();
        ObjDictParam.Add("@DocumentSeqNo", DocumentSeqNo);
        ObjDictParam.Add("@ProgramCode", PrgCode);
        if (PANum != null)
            ObjDictParam.Add("@PANum", PANum);
        if (SANum != null)
            ObjDictParam.Add("@SANum", SANum);
        ObjDictParam.Add("@Module", Module);
        ObjDictParam.Add("@CompanyId", CompanyId);
        if (LOBId != null)
            ObjDictParam.Add("@LOBId", LOBId.ToString());
        if (ProductId != null)
            ObjDictParam.Add("@ProductId", ProductId.ToString());
        if (LOBName != null)
            ObjDictParam.Add("@LOBName", LOBName);
        if (ProductName != null)
            ObjDictParam.Add("@ProductName", ProductName);

        dtWorkFlow = Utility.GetDefaultData("S3G_WORKFLOW_ISFORCEPULL", ObjDictParam);
        DataRow drWorkflow;

        if (dtWorkFlow !=null && dtWorkFlow.Rows.Count > 0)
        {
            //DataRow[] dtrRows = dtWorkFlow.Select("Program_Ref_No=" + pProgramRefCode);
            //drWorkflow = dtrRows[0];        
            //int intErrorCode = Convert.ToInt32(drWorkflow["Workflow_id"]);
            return true;
        }
        return false;

    }

    public bool CheckForForcePullOperation(string PrgCode, string PANum, string SANum, string Module, string CompanyId, out DataTable dtWorkFlow)
    {
        Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
        if (CompanyId == null)
            CompanyId = ObjUserInfo.ProCompanyIdRW.ToString();
        ObjDictParam.Add("@ProgramCode", PrgCode);
        ObjDictParam.Add("@PANum", PANum);
        ObjDictParam.Add("@SANum", SANum);
        ObjDictParam.Add("@Module", Module);
        ObjDictParam.Add("@CompanyId", CompanyId);

        dtWorkFlow = Utility.GetDefaultData("S3G_WORKFLOW_ISFORCEPULL", ObjDictParam);

        if (dtWorkFlow != null && dtWorkFlow.Rows.Count > 0)
        {
            //DataRow[] dtrRows = dtWorkFlow.Select("Program_Ref_No=" + pProgramRefCode);
            //drWorkflow = dtrRows[0];        
            //int intErrorCode = Convert.ToInt32(drWorkflow["Workflow_id"]);
            return true;
        }
        return false;

    }



    public int UpdateWorkFlowTasks(string CompanyId, string UserId, int LOBId, int BranchId, string TaskDocumentNo, int WorkflowPrgId, string WorkFlowStatusId,int Product_Id, int Document_Type)
    {
        try
        {
            TaskStatus Status = TaskStatus.FREE;
            Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
            ObjDictParam.Clear();

            ObjDictParam.Add("@CompanyId", CompanyId);
            ObjDictParam.Add("@UserId", UserId);
            ObjDictParam.Add("@LOBId", LOBId.ToString());
            ObjDictParam.Add("@Location_ID", BranchId.ToString());
            ObjDictParam.Add("@TaskDocumentNo", TaskDocumentNo);
            ObjDictParam.Add("@WorkflowPrgId", WorkflowPrgId.ToString());
            ObjDictParam.Add("@TaskStatus", Status.ToString());
            ObjDictParam.Add("@ProductId", Product_Id.ToString());
            ObjDictParam.Add("@Document_Type", Document_Type.ToString());
            if (WorkFlowStatusId.Length > 0)
                ObjDictParam.Add("@WorkFlowStatusId", WorkFlowStatusId);
            //ObjDictParam.Add("@ErrorCode", 0);

            //S3g_GEN_InsertWorkFlowStatusTodo
            DataTable dtWorkflow = Utility.GetDefaultData(S3GBusEntity.SPNames.S3G_WORKFLOW_WorkFlowStatusTodo, ObjDictParam);
            return 0;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public int UpdateWorkFlowTasks(string CompanyId, string UserId, int LOBId, int BranchId, string TaskDocumentNo, int WorkflowPrgId, string WorkFlowStatusId, String RefValue1, String RefValue2,int Product_Id)
    {
        try
        {
            TaskStatus Status = TaskStatus.FREE;
            Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
            ObjDictParam.Clear();

            ObjDictParam.Add("@CompanyId", CompanyId);
            ObjDictParam.Add("@UserId", UserId);
            ObjDictParam.Add("@LOBId", LOBId.ToString());
            ObjDictParam.Add("@Location_ID", BranchId.ToString());
            ObjDictParam.Add("@TaskDocumentNo", TaskDocumentNo);
            ObjDictParam.Add("@WorkflowPrgId", WorkflowPrgId.ToString());
            ObjDictParam.Add("@TaskStatus", Status.ToString());
            ObjDictParam.Add("@ProductId", Product_Id.ToString());
            if (RefValue1.Length > 0)
                ObjDictParam.Add("@RefValue1", RefValue1);
            if (RefValue2.Length > 0)
                ObjDictParam.Add("@RefValue2", RefValue2);
            if (WorkFlowStatusId.Length > 0)
                ObjDictParam.Add("@WorkFlowStatusId", WorkFlowStatusId);
            //ObjDictParam.Add("@ErrorCode", 0);

            //S3g_GEN_InsertWorkFlowStatusTodo
            DataTable dtWorkflow = Utility.GetDefaultData(S3GBusEntity.SPNames.S3G_WORKFLOW_WorkFlowStatusTodo, ObjDictParam);
            return 0;
        }
        catch (Exception ex)
        {
            throw;


        }
    }
    public int UpdateWorkFlowTasks(string CompanyId, string UserId, string TaskDocumentNo, int WorkflowPrgId, string WorkFlowStatusId)
    {
        try
        {
            TaskStatus Status = TaskStatus.FREE;
            Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
            ObjDictParam.Clear();

            ObjDictParam.Add("@CompanyId", CompanyId);
            ObjDictParam.Add("@UserId", UserId);
            ObjDictParam.Add("@TaskDocumentNo", TaskDocumentNo);
            ObjDictParam.Add("@WorkflowPrgId", WorkflowPrgId.ToString());
            ObjDictParam.Add("@TaskStatus", Status.ToString());
            //if (RefValue1.Length > 0)
            //    ObjDictParam.Add("@RefValue1", RefValue1);
            //if (RefValue2.Length > 0)
            //    ObjDictParam.Add("@RefValue2", RefValue2);
            if (WorkFlowStatusId.Length > 0)
                ObjDictParam.Add("@WorkFlowStatusId", WorkFlowStatusId);
            //ObjDictParam.Add("@ErrorCode", 0);

            //S3g_GEN_InsertWorkFlowStatusTodo
            DataTable dtWorkflow = Utility.GetDefaultData("S3G_WORKFLOW_UpdateStatus", ObjDictParam);
            return 0;
        }
        catch (Exception ex)
        {
            throw;


        }
    }
    enum TaskStatus
    {
        FREE,
        INITIATED,
        CANCELED,
        CONSUMED
    }
    public int InsertWorkFlowTasks(string CompanyId, string UserId, int LOBId, int BranchId, string TaskDocumentNo, int WorkflowPrgId,int ProductID,int Document_Type)
    {
        try
        {
            string TaskStatus = "FREE";
            Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
            ObjDictParam.Clear();

            ObjDictParam.Add("@CompanyId", CompanyId);
            ObjDictParam.Add("@UserId", UserId);
            ObjDictParam.Add("@LOBId", LOBId.ToString());
            ObjDictParam.Add("@Location_ID", BranchId.ToString());
            ObjDictParam.Add("@TaskDocumentNo", TaskDocumentNo);
            ObjDictParam.Add("@WorkflowPrgId", WorkflowPrgId.ToString());
            ObjDictParam.Add("@TaskStatus", TaskStatus);
            ObjDictParam.Add("@ProductId", ProductID.ToString());
            ObjDictParam.Add("Document_Type", Document_Type.ToString());
            //ObjDictParam.Add("@WorkFlowStatusId", TaskStatus);            

            DataTable dtWorkflow = Utility.GetDefaultData(S3GBusEntity.SPNames.S3G_WORKFLOW_WorkFlowStatusTodo, ObjDictParam);
            return 0;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public enum WorkFlowType
    {
        Initiater,
        Consumer,
        NotConfigured,
        Finisher
    }
    /*
    public int FunPubGetWorkflowStatus(string strPageUrl, int intLineofBusiness, int intProductCode, int intTransaction_Doc_No, out int intOutputErrorCode)
    {
        Dictionary<string, string> ObjDictParam = new Dictionary<string, string>();
        //UserInfo ObjUserInfo = new UserInfo();

        int intModuleId, intProgramId, intErrorCode = -100;
        FunPubGetPageUrl(strPageUrl, out intModuleId, out intProgramId);

        ObjDictParam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        ObjDictParam.Add("@LOB_ID", intLineofBusiness.ToString());
        ObjDictParam.Add("@Product_ID", intProductCode.ToString());
        ObjDictParam.Add("@Module_ID", intModuleId.ToString());
        ObjDictParam.Add("@Program_ID", intProgramId.ToString());
        ObjDictParam.Add("@Transaction_Doc_No", intTransaction_Doc_No.ToString());

        DataTable dtWorkflow = Utility.GetDefaultData("S3G_GET_WorkFlowIsSupported", ObjDictParam);
        DataRow drWorkflow = dtWorkflow.Rows[0];
        intErrorCode = Convert.ToInt32(drWorkflow["IsWorkFlowSupported"]);
        //if (Convert.ToInt32(drWorkflow["IsWorkFlowSupported"]) == -2)
        //{
        //    blnReturnValue = false;
        //    lblErrorMessage.Text = "Previous program of the Workflow is not completed";
        //}
        intOutputErrorCode = intErrorCode;
        return intOutputErrorCode;

    }*/

    private void KeepSessionAlive()
    {
        //        int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000) - 30000;
        //        string str_Script = @" <script type='text/javascript'>
        //        //Number of Reconnects
        //        var count=0;
        //        //Maximum reconnects setting
        //        var max = 5;
        //        function Reconnect(){
        //
        //        count++;
        //        if (count < max)
        //        {
        //        window.status = 'Link to Server Refreshed ' + count.toString()+' time(s)' ;
        //        var img = new Image(1,1);
        //        img.src = 'Reconnect.aspx';
        //        }
        //        }
        //        window.setInterval('Reconnect()'," + int_MilliSecondsTimeOut.ToString() + @"); //Set to length required
        //        </script>
        //        ";
        //        this.Page.RegisterClientScriptBlock("Reconnect", str_Script);


        // METHOD to WARN THE USER
        StringBuilder ScriptToWarn = new StringBuilder();
        ScriptToWarn.Append("<script type='text/javascript' language='javascript'>");
        ScriptToWarn.Append("Timeout = 30000;"); // Milli-Seconds ( warning in 50th second for 1 minute timeout session)
        ScriptToWarn.Append(@"setTimeout(""AlertUser();"", Timeout);");
        ScriptToWarn.Append("function AlertUser()");
        ScriptToWarn.Append("{");
        ScriptToWarn.Append(@"var ans = confirm(""Timeout in 10 Seconds. Reset?"");");
        ScriptToWarn.Append("if (ans)");
        ScriptToWarn.Append("{");
        ScriptToWarn.Append("window.location.reload();");
        ScriptToWarn.Append(@"setTimeout(""AlertUser();"", Timeout);");
        ScriptToWarn.Append("}");

        //ScriptToWarn.Append(" openph('../SessionReconnect.aspx');");
        //ScriptToWarn.Append("}");
        //ScriptToWarn.Append("function openph(url)");
        //    ScriptToWarn.Append("{");
        //    ScriptToWarn.Append("var url1 = url;");
        //    ScriptToWarn.Append(@"window.open(url1, """",""top=0,left=0,menubar=no,toolbar=no,location=no,resizable=no,height=200,width=450,status=no,scrollbars=no,maximize=null,resizable=0,titlebar=no;"");");
        //ScriptToWarn.Append("}");

        ScriptToWarn.Append("</script>");
        this.Page.RegisterClientScriptBlock("Reconnect", ScriptToWarn.ToString());
    }

    public void FunPubSetIndex(int intAccordIndex)
    {
        AjaxControlToolkit.Accordion ajxacc = new AjaxControlToolkit.Accordion();
        ajxacc = (AjaxControlToolkit.Accordion)Master.FindControl("Accordion1");
        if (ajxacc != null)
        ajxacc.SelectedIndex = intAccordIndex;
    }

    #region Code addition
    /* Code changes Summary
         * Added by : S.Srivatsan
         * Added on : 03/02/2012
         * Purpose  : To get the Database type in the Presentation layer, logical decisions could be 
         *            taken based on the Databases when necessary.
         */
    #endregion
    private static XmlDocument _xmlDoc;
    public static XmlDocument xmlDoc
    {
        get
        {
            FunPubGetConnectionString();
            return _xmlDoc;
        }
    }
    public static void FunPubGetConnectionString()
    {
        System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();

        string strFileName = (string)AppReader.GetValue("INIFILEPATH", typeof(string));

        if (File.Exists(strFileName))
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(File.ReadAllText(strFileName).Trim());
        }
        else
        {
            throw new FileNotFoundException("Configuration file not found");
        }
    }
    public S3GDALDBType FunPubGetDatabaseType()
    {
        try
        {
            S3GDALDBType S3G_DBType;
            S3G_DBType = S3GDALDBType.SQL;
            XmlDocument conxmlDoc = xmlDoc;
            string ConnectionString = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connectionString"].Value;
            string strDataProvider = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["providerName"].Value;
            string strConType = conxmlDoc.ChildNodes[0].SelectSingleNode("connectionStrings").ChildNodes[0].Attributes["connnectionType"].Value;
            if (strConType.Trim().ToUpper() == "ORACLE")
            {
                S3G_DBType = S3GDALDBType.ORACLE;
            }
            return S3G_DBType;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }
    public enum S3GDALDBType
    {
        ORACLE = 0,
        SQL = 1,
    }
}
