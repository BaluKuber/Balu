
    #region Header
//Module Name      :   Loan Admin
//Screen Name      :   S3GLoanAdNocTermination.aspx
//Created By       :   Irsathameen K
//Created Date     :   06-Sep-2010
//Purpose          :   To insert and update Noc Termination

#endregion

    #region Namespaces

using System;
using System.Web.UI;
using System.Data;
using System.Text;
using S3GBusEntity.LoanAdmin;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;

//This NameSpace for PDF Format
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;

#endregion

public partial class LoanAdmin_S3GLoanAdNocTermination : ApplyThemeForProject
{
    #region Declaration

    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjNOCTerminationClient;
    LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsDataTable ObjS3G_LOANAD_NOCTerminationDetailsDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    string intNOCID, s, TenureStatus;
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int Maturitydays, i;
    DateTime MDate;
    static string strPageName = "NOC Termination";
    bool status;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end
    DataTable dt = null;
    Dictionary<string, string> dictBranch = null;
    Dictionary<string, string> dictLOB = null;
    Dictionary<string, string> dictParam = null;

    public static LoanAdmin_S3GLoanAdNocTermination obj_Page;

    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=NOCT";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdNocTermination.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=NOCT';";

    #endregion

    #region Page_Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {    FunPriLoadPage();     }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load NOC Termination";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region DropDownEvents

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clearControls();
            ddlPAN.Clear();
            ddlSAN.Items.Clear();
            LoadPrimeAccNo();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Branch Name";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void ddlPAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        clearControls();
        ddlSAN.Items.Clear();
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@PANUM", Convert.ToString(ddlPAN.SelectedValue));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@User_ID", intUserID.ToString());
            DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetDetailsforNOCTermination, dictParam);

            if (DS.Tables.Count > 0)
            {
                // Table 0[Sub Account]
                if (DS.Tables[0].Rows.Count >= 1)
                {
                    ddlSAN.FillDataTable(DS.Tables[0], "SANum", "SANum");
                    getSAN();
                }
                //  check whether Sub Account is mandatory or not (if mandatory means change the css(*))
                if (ddlSAN.Items.Count > 1)
                {
                    RFVSLA.Visible = true;
                    lblsubAccno.CssClass = "styleReqFieldLabel";
                    return;
                }
                else
                {
                    RFVSLA.Visible = false;
                    lblsubAccno.CssClass = "styleDisplayLabel";
                }

                // Table 1 [Closure Date]
                if (DS.Tables[1].Rows.Count >= 1 && DS.Tables[1].Rows[0]["Closure_Date"].ToString() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(DS.Tables[1].Rows[0]["Closure_Date"]);
                    txtClosureDate.Text = Date.ToString(strDateFormat);
                }
                // Table 2[NOC Details][1.Account status 2.Customer Information]
                if (DS.Tables[2].Rows.Count >= 1)
                {
                    txtAccountStatuse.Text = DS.Tables[2].Rows[0]["AccStatus"].ToString();
                    hidcuscode.Value = DS.Tables[2].Rows[0]["Customer_ID"].ToString();
                    S3GCustomerAddress1.SetCustomerDetails(DS.Tables[2].Rows[0], true);
                }

                // Table4 [Activation Date]
                if (DS.Tables[4].Rows.Count >= 1 && DS.Tables[4].Rows[0]["AccActDate"].ToString() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(DS.Tables[4].Rows[0]["AccActDate"]);
                    txtAccDate.Text = Date.ToString(strDateFormat);
                }

                // Table 3 [Maturity Date]
                if (DS.Tables[3].Rows.Count >= 1)
                {
                    TenureStatus = Convert.ToString(DS.Tables[3].Rows[0]["MaturityStatus"]);
                    if (DS.Tables[3].Rows[0]["MaturityDate"].ToString() != string.Empty && txtAccDate.Text != string.Empty)
                    {
                        Maturitydays = Convert.ToInt32(DS.Tables[3].Rows[0]["MaturityDate"]);
                        MDate = Utility.StringToDate(txtAccDate.Text);
                        if (string.Compare(TenureStatus, "Days") == 0)
                        { MDate = MDate.AddDays(Maturitydays); }
                        else if (string.Compare(TenureStatus, "Weeks") == 0)
                        { MDate = MDate.AddDays(Maturitydays * 7); }
                        else if (string.Compare(TenureStatus, "Months") == 0)
                        { MDate = MDate.AddMonths(Maturitydays); }
                        DateTime Date = Convert.ToDateTime(MDate);
                        txtMaturityDate.Text = Date.ToString(strDateFormat);
                    }
                }

                // Table 5 [Asset Details ]
                if (DS.Tables.Count == 6)
                {
                    if (DS.Tables[5].Rows.Count >= 1)
                    {
                        PNLAssetDetails.Visible = GRVNOC.Visible = true;
                        GRVNOC.DataSource = DS.Tables[5];
                        GRVNOC.DataBind();
                        ViewState["Gridviewdetails"] = DS.Tables[5];
                    }
                }

            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = objException.Message;
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void ddlSAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clearControls();
            GRVNOC.DataSource = null;
            GRVNOC.DataBind();
            //if (ddlSAN.SelectedItem.Text != "--Select--")
            if (ddlSAN.SelectedValue != "0")
            {
                DataSet DS = new DataSet();
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictParam.Add("@PANUM", Convert.ToString(ddlPAN.SelectedValue));
                dictParam.Add("@SANUM", Convert.ToString(ddlSAN.SelectedValue));
                dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
                dictParam.Add("@User_ID", intUserID.ToString());
                DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetNocDetailsBySAN, dictParam);

                // Table 0 [Closure Date]
                if (DS.Tables[0].Rows.Count >= 1 && DS.Tables[0].Rows[0]["Closure_Date"].ToString() != string.Empty)
                {
                    DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["Closure_Date"]);
                    txtClosureDate.Text = Date.ToString(strDateFormat);
                }

                // Table 1[NOC Details][1.Account status 2.Customer Information]
                if (DS.Tables[1].Rows.Count >= 1)
                {
                    txtAccountStatuse.Text = Convert.ToString(DS.Tables[1].Rows[0]["AccStatus"]);                   
                    hidcuscode.Value = Convert.ToString(DS.Tables[1].Rows[0]["Customer_ID"]);
                    S3GCustomerAddress1.SetCustomerDetails(DS.Tables[1].Rows[0], true);
                }

                // Table 4 [Activation Date]
                if (DS.Tables[4].Rows.Count >= 1)
                {
                    DateTime Date = Convert.ToDateTime(DS.Tables[4].Rows[0]["AccActDate"]);
                    txtAccDate.Text = Date.ToString(strDateFormat);
                }

                // Table 2 [Maturity Date]
                if (DS.Tables[2].Rows.Count >= 1)
                {
                    TenureStatus = Convert.ToString(DS.Tables[2].Rows[0]["MaturityStatus"]);
                    if (Convert.ToString(DS.Tables[2].Rows[0]["MaturityDate"]) != string.Empty && txtAccDate.Text != string.Empty)
                    {
                        Maturitydays = Convert.ToInt32(DS.Tables[2].Rows[0]["MaturityDate"]);
                        MDate = Utility.StringToDate(txtAccDate.Text);
                        if (string.Compare(TenureStatus, "Days") == 0)
                        { MDate = MDate.AddDays(Maturitydays); }
                        else if (string.Compare(TenureStatus, "Weeks") == 0)
                        { MDate = MDate.AddDays(Maturitydays * 7); }
                        else if (string.Compare(TenureStatus, "Months") == 0)
                        { MDate = MDate.AddMonths(Maturitydays); }
                        DateTime Date = Convert.ToDateTime(MDate);
                        txtMaturityDate.Text = Date.ToString(strDateFormat);
                    }
                }

                // Table 4 [Asset Details ]
                if (DS.Tables[3].Rows.Count >= 1)
                {
                    PNLAssetDetails.Visible = GRVNOC.Visible = true;
                    GRVNOC.DataSource = DS.Tables[3];
                    GRVNOC.DataBind();
                    ViewState["Gridviewdetails"] = DS.Tables[3];
                }
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load Sub A/c and Customer Details";
            cvCustomerMaster.IsValid = false;
        }

    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dictBranch = new Dictionary<string, string>();
            dictBranch.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictBranch.Add("@Is_Active", "1");
            dictBranch.Add("@User_ID", Convert.ToString(intUserID));
            dictBranch.Add("@Program_ID", "76");
            dictBranch.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictBranch, new string[] { "Location_ID", "Location" });

            clearControls();
            //if (ddlBranch.Items.Count > 0)
            //    ddlBranch.SelectedIndex = 0;
            //if (ddlPAN.Items.Count > 0)
            ddlBranch.Clear();
                ddlPAN.Clear();
            if (ddlSAN.Items.Count > 0)
                ddlSAN.Items.Clear();
            //LoadPrimeAccNo();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load Prime A/c Number";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Save,cancel,Clear,Email,NocTermination Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveNOCDetails();            
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddlBranch.Items.Count > 0)
            //{ 
            //    //ddlBranch.SelectedIndex = 0; 
            //    ddlBranch.Clear();
            //}
            ddlBranch.Clear();
            if (ddlLOB.Items.Count > 0)
            { ddlLOB.SelectedIndex = 0; }
            //if (ddlPAN.Items.Count > 0)
            { ddlPAN.Clear(); }
            if (ddlSAN.Items.Count > 0)
            { ddlSAN.Items.Clear(); }
            clearControls();
            ddlLOB.Focus();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the Controls";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // wf cancel
            if (PageMode == PageModes.WorkFlow)
                ReturnToWFHome();
            else            
                Response.Redirect(strRedirectPage,false); 
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Redirect the Page";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void btnNocLetter_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriUpdateNOCStatus(1);
            PreviewPDF_Click(sender, e);            
            btnNOCcancel.Enabled = false;
            if (strMode != "Q")
                btnEmail.Enabled = true;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Generate NOC Termination Letter";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void btnNOCcancel_Click(object sender, EventArgs e)
    {
        FunPriUpdateNOCStatus(2);
        btnNocLetter.Enabled = false;

    }
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            string body = GetHTMLText();
            CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
            ObjCom_Mail.ProFromRW = "ganapathy.g@sundaraminfotech.in";
            //ObjCom_Mail.ProTORW = "irsathameen.k@sundaraminfotech.in";
            //if (txtEmailid.Text == string.Empty)

            if (S3GCustomerAddress1.EmailID == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, " Unable to send the Email Due to Email Id is not Given");
                return;
            }
            ObjCom_Mail.ProTORW = "kuppusamy.b@sundaraminfotech.in";//S3GCustomerAddress1.EmailID;
            ObjCom_Mail.ProSubjectRW = "NOC Termination Letter";
            ObjCom_Mail.ProMessageRW = body;
            ObjCommonMail.FunSendMail(ObjCom_Mail);
            Utility.FunShowAlertMsg(this, "Mail sent successfully");
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to send Email";
            cvCustomerMaster.IsValid = false;
            Utility.FunShowAlertMsg(this, "Invalid EMail ID. Mail not sent.");
        }
    }

    #endregion

    #region User_Defined_Functions

    protected void FunPriGetNocTerminationDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@Noc_ID", intNOCID);
            DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetNocTerminationDetails, dictParam);
            btnNocLetter.Enabled = true;

            // Table 0  Lob,Branch,PA,SA Number,Noc Date,no,CusDetails
            if (DS.Tables[0].Rows.Count >= 1)
            {
                //ddlLOB.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["LOB_ID"]);    
                ddlLOB.Items.Add(new System.Web.UI.WebControls.ListItem(DS.Tables[0].Rows[0]["LOBName"].ToString(), DS.Tables[0].Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = DS.Tables[0].Rows[0]["LOBName"].ToString();

                //ddlLOB.Items[0].Text=Convert.ToString(DS.Tables[0].Rows[0]["LOBName"]);
                //ddlLOB.Items[0].Value = Convert.ToString(DS.Tables[0].Rows[0]["LOB_ID"]);
                //ddlLOB.ToolTip = Convert.ToString(DS.Tables[0].Rows[0]["LOBName"]);
                //ddlBranch.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Branch_ID"]);                

                //ddlBranch.Items[0].Text = Convert.ToString(DS.Tables[0].Rows[0]["LocationName"]);
                //ddlBranch.Items[0].Value = Convert.ToString(DS.Tables[0].Rows[0]["Location_ID"]);

                //ddlBranch.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Location_ID"]);
                ddlBranch.SelectedValue = DS.Tables[0].Rows[0]["Location_ID"].ToString();
                ddlBranch.SelectedText = DS.Tables[0].Rows[0]["LocationName"].ToString();
                ddlBranch.ToolTip = DS.Tables[0].Rows[0]["LocationName"].ToString();

                txtNocNo.Text = Convert.ToString(DS.Tables[0].Rows[0]["NOC_Number"]);
                DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["NOC_Date"]);
                txtNocDate.Text = Date.ToString(strDateFormat);
                //ddlPAN.Items.Add(Convert.ToString(DS.Tables[0].Rows[0]["PANum"]));
                ddlPAN.SelectedValue = (Convert.ToString(DS.Tables[0].Rows[0]["PANum"]));
                ddlPAN.SelectedText = (Convert.ToString(DS.Tables[0].Rows[0]["PANum"]));
                //s = Convert.ToString(DS.Tables[0].Rows[0]["SANum"]);
                //if (s.Contains("DUMMY") == true)
                //{ ddlSAN.Items.Clear(); }
                //else
                //{ ddlSAN.Items.Add(Convert.ToString(DS.Tables[0].Rows[0]["SANum"])); }
                if (DS.Tables[0].Rows[0]["SANum"].ToString() != "" && !DS.Tables[0].Rows[0]["SANum"].ToString().Contains("DUMMY"))
                {
                    ddlSAN.Items.Add(new System.Web.UI.WebControls.ListItem(DS.Tables[0].Rows[0]["SANum"].ToString(), DS.Tables[0].Rows[0]["SANum"].ToString()));
                    ddlSAN.ToolTip = DS.Tables[0].Rows[0]["SANum"].ToString();
                }
                else
                {
                    ddlSAN.Items.Add(new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                }
                if (Convert.ToInt32(DS.Tables[0].Rows[0]["NOCStatus"]) < 0)
                {
                    txtNocPrintstatus.Text = "Cancelled";
                    btnNocLetter.Enabled=btnEmail.Enabled= false;
                }
                else if (Convert.ToString(DS.Tables[0].Rows[0]["NOCStatus"]) == "0")
                {
                    if ((intNOCID != "") && (strMode == "M"))
                    {
                        btnNOCcancel.Enabled = true;
                    }
                    txtNocPrintstatus.Text = "Original";
                    btnEmail.Enabled = false;
                }
                else
                {
                    txtNocPrintstatus.Text = "Duplicate";
                    btnEmail.Enabled = true;
                }
                hidcuscode.Value = Convert.ToString(DS.Tables[0].Rows[0]["Customer_ID"]);
                S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
            }
            // Table 1
            if (DS.Tables[1].Rows.Count >= 1)
            {
                txtAccountStatuse.Text = Convert.ToString(DS.Tables[1].Rows[0]["AccStatus"]);
                if (Convert.ToString(DS.Tables[1].Rows[0]["AccActDate"]) != string.Empty)
                {
                    DateTime Date1 = Convert.ToDateTime(DS.Tables[1].Rows[0]["AccActDate"]);
                    txtAccDate.Text = Date1.ToString(strDateFormat);
                }
            }
            // Table 2 [Maturity Date]
            if (DS.Tables[2].Rows.Count >= 1)
            {
                TenureStatus = Convert.ToString(DS.Tables[2].Rows[0]["MaturityStatus"]);
                if (Convert.ToString(DS.Tables[1].Rows[0]["AccActDate"]) != string.Empty)
                {
                    Maturitydays = Convert.ToInt32(DS.Tables[2].Rows[0]["MaturityDate"]);
                    MDate = Utility.StringToDate(txtAccDate.Text);
                    if (string.Compare(TenureStatus, "Days") == 0)
                    { MDate = MDate.AddDays(Maturitydays); }
                    else if (string.Compare(TenureStatus, "Weeks") == 0)
                    { MDate = MDate.AddDays(Maturitydays * 7); }
                    else if (string.Compare(TenureStatus, "Months") == 0)
                    { MDate = MDate.AddMonths(Maturitydays); }
                    DateTime Date = Convert.ToDateTime(MDate);
                    txtMaturityDate.Text = Date.ToString(strDateFormat);
                }
            }

            // Table 3 Asset Details
            if (DS.Tables[3].Rows.Count >= 1)
            {
                GRVNOC.DataSource = DS.Tables[3];
                GRVNOC.DataBind();
                ViewState["Gridviewdetails"] = DS.Tables[3];
            }
            // Table 4 Closure Date
            if (DS.Tables[4].Rows.Count >= 1)
            {
                if (Convert.ToString(DS.Tables[4].Rows[0]["Closure_Date"]) != string.Empty)
                {
                    DateTime Date2 = Convert.ToDateTime(DS.Tables[4].Rows[0]["Closure_Date"]);
                    txtClosureDate.Text = Date2.ToString(strDateFormat);
                }
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void FunPriBindBranchLOB()
    {
        try
        {
            if (strMode != "C")
            {
                //Branch
                dictBranch = new Dictionary<string, string>();
                dictBranch.Add("@Company_ID", Convert.ToString(intCompanyID));
                dictBranch.Add("@Is_Active", "1");
                dictBranch.Add("@User_ID", Convert.ToString(intUserID));
                dictBranch.Add("@Program_ID", "76");
                dictBranch.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictBranch, new string[] { "Location_ID", "Location" });
            }

            //LOB
            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictLOB.Add("@Is_Active", "1");
            dictLOB.Add("@User_ID", Convert.ToString(intUserID));
            dictLOB.Add("@Program_ID", "76");
            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetLOBNOCTermination, dictLOB, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
                ddlLOB_SelectedIndexChanged(this, new EventArgs());                                    
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void LoadPrimeAccNo()
    {
        try
        {
            //Primary A/c Number
            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            //dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            //dictParam.Add("@User_ID", Convert.ToString(intUserID));
            //ddlPAN.BindDataTable(SPNames.S3G_LOANAD_GetPANumforNocTermination, dictParam, new string[] { "PANum", "PANum" });
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANUM(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.ddlLOB.SelectedValue));
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetPANumforNocTermination_AGT", Procparam));

        return suggetions.ToArray();
    }
    protected void PreviewPDF_Click(object sender, EventArgs e)
    {
        try
        {
            string htmlText = GetHTMLText();
            string strnewFile;
            string strFileName;
            if (txtNocNo.Text != string.Empty)
            {
                strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + txtNocNo.Text.Replace("/", "").Replace(" ", "").Replace(":", "").Replace("-", "") + ".pdf");
                strFileName = "/LoanAdmin/PDF Files/" + txtNocNo.Text.Replace("/", "").Replace(" ", "-").Replace(":", "").Replace("-", "") + ".pdf";
            }
            else
            {
                strnewFile = (Server.MapPath(".") + "\\PDF Files\\NOC Termination Letter.pdf");
                strFileName = "/LoanAdmin/PDF Files/" + "NOC Termination Letter.pdf";
            }
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
            doc.AddCreator("Sundaram Infotech Solutions");
            doc.AddTitle("New PDF Document");
            doc.Open();
            //List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
            //for (int k = 0; k < htmlarraylist.Count; k++)
            //{ doc.Add((IElement)htmlarraylist[k]); }
            doc.AddAuthor("S3G Team");
            doc.Close();
            //System.Diagnostics.Process.Start(strnewFile);
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private string GetHTMLText()
    {
        try
        {
            UserInfo ObjUserInfo = new UserInfo();
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<font size=\"2\"  color=\"black\" face=\"Times New Roman\">");
            sbHtml.Append(" <table width=\"100%\">");
            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"Left\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\">" + Convert.ToString(ObjUserInfo.ProCompanyNameRW) + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("<td></td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"Left\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\">" + Convert.ToString(ddlBranch.SelectedText) + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("<td></td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"Center\" colspan=\"2\">");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "<b><U>" + " NOC/TERMINATION LETTER" + "</font> " + "</b></U>");
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td></td>");
            sbHtml.Append("<td  align=\"Right\">");
            sbHtml.Append("<table    align=\"Right\">");
            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"right\">" + " Branch &nbsp; &nbsp; &nbsp;:" + "</td>" + "<td align=\"left\">" + Convert.ToString(ddlBranch.SelectedText) + "</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"right\" >");
            sbHtml.Append(" NOC No  &nbsp; &nbsp;: ");
            sbHtml.Append("</td>");
            sbHtml.Append("<td align=\"left\">" + Convert.ToString(txtNocNo.Text));
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"right\">");
            sbHtml.Append("NOC Date   :");
            sbHtml.Append("</td>");
            sbHtml.Append("<td align=\"left\">" + Convert.ToString(txtNocDate.Text));

            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("</table>");
            sbHtml.Append("</td>");
            sbHtml.Append(" </tr>");

            sbHtml.Append("<tr>");

            sbHtml.Append("<td  align=\"Left\">");
            sbHtml.Append("<table  align=\"left\">");
            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\" >");
            sbHtml.Append("Print Status &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;:");
            sbHtml.Append("</td>");
            sbHtml.Append("<td align=\"left\" >" + Convert.ToString(txtNocPrintstatus.Text));

            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\">");
            sbHtml.Append("Line of Business &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;:");
            sbHtml.Append("</td>");
            //UAT Fix Rounded 3 "NOC_014" by saran on 19-Apr-2012.
            string StrLob = "";
            StrLob = ddlLOB.SelectedItem.Text.Split('-')[1].ToString().Trim();
            if (StrLob == null)
                StrLob = ddlLOB.SelectedItem.Text;

            sbHtml.Append("<td align=\"left\" >" + StrLob);
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\" >");
            sbHtml.Append("Account Reference &nbsp;&nbsp; :");
            sbHtml.Append("</td>");
            if (ddlSAN.Items.Count > 0 && ddlSAN.SelectedValue.ToString()!="0")
                sbHtml.Append("<td align=\"left\" >" + Convert.ToString(ddlPAN.SelectedValue) + " <br>  " + Convert.ToString(ddlSAN.SelectedItem.Text));
            else
                sbHtml.Append("<td align=\"left\" >" + Convert.ToString(ddlPAN.SelectedValue));
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");
            
            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\">");
            sbHtml.Append("Customer  Code &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:");
            sbHtml.Append("</td>");
            sbHtml.Append("<td align=\"left\">" + Convert.ToString(S3GCustomerAddress1.CustomerCode) + "</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\">");
            sbHtml.Append("Customer Name &nbsp; &nbsp;&nbsp; &nbsp; :");
            sbHtml.Append("</td>");
            sbHtml.Append("<td align=\"left\">" + Convert.ToString(S3GCustomerAddress1.CustomerName) + "</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td align=\"Left\">");
            sbHtml.Append("Customer  Address &nbsp;&nbsp;&nbsp;:");
            sbHtml.Append("</td >");
            sbHtml.Append("<td align=\"left\" >" + Convert.ToString(S3GCustomerAddress1.CustomerAddress) + "</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");

            //sbHtml.Append("<td align=\"Right\">" + "");
            //sbHtml.Append("</td>");
            //sbHtml.Append("<td align=\"left\">" + txtCity.Text);

            //sbHtml.Append("</td>");
            //sbHtml.Append("</tr>");

            //sbHtml.Append("<tr>");
            //sbHtml.Append("<td align=\"right\">" + "");
            //sbHtml.Append("</td>");
            //sbHtml.Append("<td align=\"left\">" + txtState.Text + "</td>");
            //sbHtml.Append("</tr>" + "<tr>");
            //sbHtml.Append("<td align=\"right\">" + "" + "</td>");
            //sbHtml.Append("<td align=\"left\">" + txtPincode.Text + "</td>" + "</tr>");

            sbHtml.Append("</table>");
            sbHtml.Append("</td>");
            sbHtml.Append("<td></td>");
            sbHtml.Append(" </tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"Left\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "<b>" + " Sir / Madam" + "</font> " + "</b>");
            sbHtml.Append("</td>");
            sbHtml.Append("<td></td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"center\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "<b><u>Sub: NOC/TERMINATION LETTER has been issued for the following asset</u></b>" + "</font> ");
            sbHtml.Append("</td>");

            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"Left\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "This is to confirm that the following asset has been issued with No Objection Certificate/ Termination Letter. The same can be submitted to RTO and Insurance companies respectively for removal of hypothecation. " + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"left\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "<b> Asset Details</b>" + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");


            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"left\" colspan=\"2\" >");

            sbHtml.Append("<table border=1 cellpadding=0 cellspacing=0 width=\"100%\">");
            sbHtml.Append("<tr >");
            sbHtml.Append("<td align=\"Center\" width=\"10%\" >" + "S.No" + "</td>");
            sbHtml.Append("<td align=\"Center\" width=\"30%\" >" + "Asset Code" + "</td>");
            sbHtml.Append("<td align=\"Center\" width=\"30%\">" + "Asset Description" + "</td>");
            sbHtml.Append("<td align=\"Center\" width=\"30%\">" + "Asset Registration/Serial Number" + "</td>");
            sbHtml.Append("</tr>");

            
            sbHtml.Append(FunPriGetHtmlTable());
            

            sbHtml.Append("</table>");

            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr>");
            sbHtml.Append("<td  align=\"left\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + "Yours Truly," + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");

            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");
            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");
            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");
            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");
            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");
            sbHtml.Append("<tr><td colspan=\"2\" ></td></tr>");


            sbHtml.Append("<td  align=\"left\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + " for " + Convert.ToString(ObjUserInfo.ProCompanyNameRW) + "</font> ");
            sbHtml.Append("</td>");
            sbHtml.Append("</tr>");
            sbHtml.Append("</tr>");
            sbHtml.Append("<td  align=\"left\" colspan=\"2\" >");
            sbHtml.Append("<font size=\"2\"  color=\"Black\" face=\"Times New Roman\" >" + " AUTHORIZED SIGNATORY" + "</font> ");
            sbHtml.Append("</td>");

            sbHtml.Append("</tr>");
            sbHtml.Append("</table>" + "</font>");
            return sbHtml.ToString();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }

    }
    private void FunPriDisableControls(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    btnEmail.Enabled = false;
                    PNLAssetDetails.Visible=lblNocPrintstatus.Visible = txtNocPrintstatus.Visible = RFVSLA.Visible = false;
                    ddlLOB.Focus();
                    break;
                case -1:// Query Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (ddlLOB.Items.Count != 0)
                    { ddlLOB.ClearDropDownList(); }
                    //if (ddlBranch.Items.Count != 0)
                    //{ ddlBranch.ClearDropDownList(); }
                    ddlBranch.ReadOnly = true;
                    txtMaturityDate.ReadOnly = txtNocDate.ReadOnly = txtNocPrintstatus.ReadOnly = txtAccountStatuse.ReadOnly = txtClosureDate.ReadOnly = txtAccDate.ReadOnly = true;
                    btnNocLetter.Enabled=btnEmail.Enabled = btnSave.Enabled = btnClear.Enabled = false;
                    ddlPAN.ReadOnly = true;
                    //if (btnNocLetter.Enabled)
                    //    btnNocLetter.Enabled = true;                   
                    break;
                case 1:// Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (ddlLOB.Items.Count != 0)
                    { ddlLOB.ClearDropDownList(); }
                    //if (ddlBranch.Items.Count != 0)
                    //{ ddlBranch.ClearDropDownList(); }
                    ddlBranch.ReadOnly = true;
                    txtMaturityDate.ReadOnly = txtNocDate.ReadOnly = txtNocPrintstatus.ReadOnly = txtAccountStatuse.ReadOnly = txtClosureDate.ReadOnly = txtAccDate.ReadOnly = true;
                    btnSave.Enabled = btnClear.Enabled = false;
                    ddlPAN.ReadOnly = true;
                    if (btnNocLetter.Enabled)
                        btnNocLetter.Enabled = true;     
                    break;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void clearControls()
    {
        try
        {
            txtMaturityDate.Text = txtAccDate.Text = txtNocNo.Text = txtAccountStatuse.Text = txtClosureDate.Text = txtClosureDate.Text = lblErrorMessage.Text = txtAccountStatuse.Text = String.Empty;
            S3GCustomerAddress1.ClearCustomerDetails();
            // txtMobile.Text  = txtPincode.Text = txtState.Text = txtTelephone.Text = txtAddress1.Text = txtAddress2.Text = txtCustName.Text = txtCity.Text = txtCountry.Text = txtEmailid.Text = = String.Empty;
            GRVNOC.DataSource = null;
            GRVNOC.DataBind();
            PNLAssetDetails.Visible = false;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private string FunPriGetHtmlTable()
    {
        try
        {
            string strHtml = string.Empty;
            dt = (DataTable)ViewState["Gridviewdetails"];
            if (dt != null)
            {

                if (dt.Rows.Count > 0)
                {
                   // strHtml = "<table width=\"100%\" border=0 >";// border=\"1px\" width=\"100%\">";
                    for (int i_row = 0; i_row < dt.Rows.Count; i_row++)
                    {
                        strHtml += " <tr>";
                        for (int i_column = 0; i_column < dt.Columns.Count; i_column++)
                        {
                            strHtml += " <td align=\"Left\"> ";
                            strHtml += "&nbsp;" + dt.Rows[i_row][i_column].ToString();
                            strHtml += " </td> ";
                        }
                        strHtml += " </tr> ";
                    }
                   // strHtml += "</table>";
                }
            }
            return strHtml;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void getSAN()
    {
        try
        {
            for (i = 0; i < ddlSAN.Items.Count; i++)
            {
                s = ddlSAN.Items[i].ToString();
                status = s.Contains("DUMMY");
                if (status == true)
                {
                    if (ddlSAN.Items.Count == 2)
                    {
                        ViewState["dum"] = ddlSAN.Items[i].Text;
                        ddlSAN.Items.Clear();
                        break;
                    }
                    else
                    {
                        ViewState["dum"] = ddlSAN.Items[i].Text;
                        ddlSAN.Items.RemoveAt(i);
                    }
                }
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriUpdateNOCStatus(int flag)
    {
        try
        {
            txtAccountStatuse.Visible = true;
            lblAccountStatus.Visible = true;
            DataTable dtNOCDupCount = null;
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@NOC_No", Convert.ToString(txtNocNo.Text));
            dictParam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            dictParam.Add("@Location_ID ", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@User_ID", Convert.ToString(intUserID));
            dictParam.Add("@flag", Convert.ToString(flag));

            dtNOCDupCount = Utility.GetDefaultData(SPNames.S3G_LOANAD_CountNOCTermination, dictParam);
            if (dtNOCDupCount.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtNOCDupCount.Rows[0]["NOC_Duplicate_Count"]) == 1)
                { txtNocPrintstatus.Text = "Original"; }
                else if (Convert.ToInt32(dtNOCDupCount.Rows[0]["NOC_Duplicate_Count"]) < 0)
                {
                    txtNocPrintstatus.Text = "Cancelled";
                    btnNOCcancel.Enabled = btnNocLetter.Enabled = false;
                    Utility.FunShowAlertMsg(this.Page, "Noc Termination for " + txtNocNo.Text + " has been cancelled");
                    return;
                }
                else
                { txtNocPrintstatus.Text = "Duplicate"; }
                
                //if (Convert.ToInt32(dtNOCDupCount.Rows[0]["NOC_Duplicate_Count"]) < 0)
                //{                   
                //    txtNocPrintstatus.Text = "Canceled";
                //    btnNOCcancel.Enabled = btnNocLetter.Enabled = false;
                //    Utility.FunShowAlertMsg(this.Page, "Noc Termination "+txtNocNo.Text +" has been canceled "  );
                //    return;
                //}
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            //Begin
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            //End
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                { intNOCID = Convert.ToString(fromTicket.Name); }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
                strMode = Request.QueryString["qsMode"];
            }
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            obj_Page = this;

            if (!IsPostBack)
            {
                txtNocDate.Text = DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
              
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                // Query MOde
                if ((intNOCID != "") && (strMode == "Q"))
                {
                    FunPriGetNocTerminationDetails();
                    FunPriDisableControls(-1);
                }
                else if ((intNOCID != "") && (strMode == "M"))
                {
                    FunPriGetNocTerminationDetails();
                    FunPriDisableControls(1);
                }
                //Create Mode
                else
                {
                FunPriBindBranchLOB();
                FunPriDisableControls(0);
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriSaveNOCDetails()
    {
        try{
        string strNOCNo = "";
            ObjS3G_LOANAD_NOCTerminationDetailsDataTable = new LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsDataTable();
            LoanAdminMgtServices.S3G_LOANAD_NOCTerminationDetailsRow ObjNOCTerminationRow;
            ObjNOCTerminationRow = ObjS3G_LOANAD_NOCTerminationDetailsDataTable.NewS3G_LOANAD_NOCTerminationDetailsRow();
            ObjNOCTerminationRow.Company_ID = intCompanyID;
            ObjNOCTerminationRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjNOCTerminationRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjNOCTerminationRow.NOC_Date = Utility.StringToDate(txtNocDate.Text);
            ObjNOCTerminationRow.PANum = Convert.ToString(ddlPAN.SelectedValue);
            if (ddlSAN.Items.Count == 0)
            { ObjNOCTerminationRow.SANum = Convert.ToString(ViewState["dum"]); }
            else if (ddlSAN.SelectedValue == "0")
            { ObjNOCTerminationRow.SANum = Convert.ToString(ViewState["dum"]); }
            else { ObjNOCTerminationRow.SANum = Convert.ToString(ddlSAN.SelectedValue); }
            ObjNOCTerminationRow.Customer_ID = Convert.ToInt32(hidcuscode.Value);//hidden Customer_ID;                
            ObjNOCTerminationRow.Created_By = intUserID;
            ObjS3G_LOANAD_NOCTerminationDetailsDataTable.AddS3G_LOANAD_NOCTerminationDetailsRow(ObjNOCTerminationRow);
            ObjNOCTerminationClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
            intErrCode = ObjNOCTerminationClient.FunPubCreateNOCTerminationDetails(out strNOCNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_NOCTerminationDetailsDataTable, SerMode));
            if (intErrCode == 0)
            {               
                Utility.FunShowAlertMsg(this.Page, "Noc Termination details added successfully -  " + strNOCNo);
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                txtNocNo.Text = strNOCNo;
                btnNocLetter.Enabled = true;
                //btnNOCcancel.Enabled =true;
                lblNocPrintstatus.Visible = txtNocPrintstatus.Visible = true;                
                btnClear.Enabled = btnSave.Enabled = false;
                
                //FunPriClearDropdown();
                return;
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Noc Termination details already exist");
                btnNocLetter.Enabled = true;
                btnEmail.Enabled = true;
                return;
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (intErrCode == -2)
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                return;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            lblErrorMessage.Text = "Unable Save the  NOC Termination Details";
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            if (ObjNOCTerminationClient != null)
                ObjNOCTerminationClient.Close();        
        }
    }
    private void FunPriClearDropdown()
    {
        try
        {
            if (ddlLOB.Items.Count != 0)
            { ddlLOB.ClearDropDownList(); }
            //if (ddlBranch.Items.Count != 0)
            //{ ddlBranch.ClearDropDownList(); }            
            //if (ddlPAN.Items.Count != 0)
            ddlBranch.Clear();
            { ddlPAN.Clear(); }
            if (ddlSAN.Items.Count != 0)
            { ddlSAN.ClearDropDownList(); }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "076");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue.ToString());
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    #endregion

}




