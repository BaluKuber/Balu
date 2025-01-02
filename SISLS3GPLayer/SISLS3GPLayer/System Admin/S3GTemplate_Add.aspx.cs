
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         : System Admin  
/// Screen Name         : Template Creation
/// Created By          : M.Saran
/// Created Date        : 28-Jul-2012 
/// <Program Summary>
#endregion

#region Namespaces
using FreeTextBoxControls;
using iTextSharp.text;
using Languages;
using S3GBusEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

public partial class System_Admin_S3GTemplate : ApplyThemeForProject
{

    #region Variable Declaration Region
    int intErrorCode = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    int intTemplateId = -1;
    string strDateFormat = string.Empty;
    string strTemplateNo = string.Empty;
    string strTempRefNo = string.Empty;
    const string strvalidationmsgname = "TMPL";
    public static System_Admin_S3GTemplate obj_Page;

    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictLOBMaster = null;

    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode SerMode = SerializationMode.Binary;

    DataTable dtExclusionDetails = new DataTable();

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    string strRedirectPageAdd = "window.location.href='../System Admin/S3GTemplate_Add.aspx';";
    string strRedirectPageView = "window.location.href='../System Admin/S3GTemplate_View.aspx';";
    string strRedirectPage = "S3GTemplate_View.aspx";
    static string strPageName = "Template Master";

    DocMgtServicesReference.DocMgtServicesClient ObjDocServicesClient;
    DocMgtServices.S3G_SYSAD_TemplateDtlsDataTable ObjS3G_SYSAD_TemplateDtlsDataTable;

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
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Due to Data Problem, Unable to Load the Template Master.";
            CVTemplate.IsValid = false;
        }
    }

    #endregion

    #region "Events"

    #region "DropDown Events"

    protected void ddlTemplateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindVariables();
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Due to Data Problem, Unable to Load the variables.";
            CVTemplate.IsValid = false;
        }
    }

    protected void ddlModeofMail_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriSetModeofMail();

        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Due to Data Problem, Unable to get Doc path.";
            CVTemplate.IsValid = false;
        }
    }

    //protected void ddlFooterCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {


    //    }
    //    catch (Exception ex)
    //    {
    //        CVTemplate.ErrorMessage = "Due to Data Problem, Unable to get Doc path.";
    //        CVTemplate.IsValid = false;
    //    }
    //}
    //protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunProLoadBranch();
    //    }
    //    catch (Exception ex)
    //    {
    //        CVTemplate.ErrorMessage = "Due to Data Problem, Unable to get Doc path.";
    //        CVTemplate.IsValid = false;
    //    }
    //}


    #endregion

    #region "Button Events"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSave();
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to save Template.";
            CVTemplate.IsValid = false;
        }

    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadCustomerDtls();
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to Load Details.";
            CVTemplate.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage, false);
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to save Template.";
            CVTemplate.IsValid = false;
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClear();
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to save Template.";
            CVTemplate.IsValid = false;
        }

    }

    //protected void btnGenerate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlTemplateType.SelectedValue == "5")//Demand Dunning
    //        {

    //            FunPriDunninMail();
    //        }
    //        else if (ddlTemplateType.SelectedValue == "7")//Payment Today's Disbursement
    //        {
    //            FunPriPaymentMail();
    //        }
    //        else if (ddlTemplateType.SelectedValue == "8")//Account Agreement
    //        {
    //            FunPriAgreementMail();
    //        }
    //        else if (ddlTemplateType.SelectedValue == "1")//pricing
    //        {
    //            FunPriPricingDetails(intCompanyID, ddlLOB.SelectedValue, FTBTemplate.Text);
    //        }
    //        else
    //        {
    //            //FunPriSendMail(FTBTemplate.Text);
    //            FunPriGeneratePDF(FTBTemplate.Text, ddlTemplateType.SelectedItem.Text);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        CVTemplate.ErrorMessage = "Unable to Generate Template.";
    //        CVTemplate.IsValid = false;
    //    }

    //}
    #endregion

    #region "GridLines Events"

    protected void GrvExclusion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                FunInsertExclusionDetails();
            }
        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to add details.";
            CVTemplate.IsValid = false;
        }
    }

    protected void GrvExclusion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            FunPriRemoveExclusionDetails(e.RowIndex);

        }
        catch (Exception ex)
        {
            CVTemplate.ErrorMessage = "Unable to Delete details.";
            CVTemplate.IsValid = false;
        }
    }

    #endregion

    #endregion

    #region "Methods"

    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket formTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                strMode = Request.QueryString.Get("qsMode");
                if (formTicket != null)
                {
                    strTemplateNo = formTicket.Name;
                }
            }

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;

            if (!IsPostBack)
            {
                FunProLoadLOB();
                // FunProLoadBranch();
                FunProLoadControls();
                FunProLoadVariables();
                if (strMode == "M")
                {

                    FunPriDisableControls(1);//Modify
                }
                else if (strMode == "Q")
                    FunPriDisableControls(-1);//Query 
                else
                    FunPriDisableControls(0);//Create

                FunPriLoadExclusionCategory();
            }
            FunSetCodeLOV();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    //public void FunPubSentMail(string strBody)
    //{
    //    try
    //    {
    //        Dictionary<string, string> dictMail = new Dictionary<string, string>();
    //        dictMail.Add("FromMail", "saran.m@sundaraminfotech.in");
    //        dictMail.Add("ToMail", "saran.m@sundaraminfotech.in");
    //        dictMail.Add("Subject", "Eg:Dunning Mail ");
    //        ArrayList arrMailAttachement = new ArrayList();
    //        StringBuilder strBody = new StringBuilder();
    //        strBody.Append(FunPriGenerateHTML());
    //        Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}


    //private void FunPriAgreementMail()
    //{

    //    try
    //    {
    //        //Getting Database value
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", intCompanyID.ToString());
    //        Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
    //        //Procparam.Add("@Location_Id", ddlLocation.SelectedValue);
    //        DataSet DS = new DataSet();
    //        DataTable dtHeader = new DataTable();
    //        DataTable dtDetails = new DataTable();
    //        DataTable dtSubDetails = new DataTable();
    //        string strNewHTML = string.Empty;
    //        DS = Utility.GetDataset("S3G_DUN_AgreementDtls", Procparam);

    //        if (DS != null)
    //        {
    //            if (DS.Tables[0].Rows.Count > 0)
    //                dtHeader = DS.Tables[0].Copy();
    //            if (DS.Tables[1].Rows.Count > 0)
    //                dtDetails = DS.Tables[1].Copy();
    //        }
    //        if (dtHeader.Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in dtHeader.Rows)
    //            {
    //                strNewHTML = FTBTemplate.Text;

    //                foreach (DataColumn dcol in dtHeader.Columns)
    //                {
    //                    string strColname = string.Empty;
    //                    strColname = "~" + dcol.ColumnName + "~";
    //                    if (strNewHTML.Contains(strColname))
    //                        strNewHTML = strNewHTML.Replace(strColname, dr[dcol].ToString());
    //                }

    //                DataRow[] drCustDetails = dtDetails.Select("Account_No = '" + dr["Account_No"].ToString() + "' and Sub_Account_No='" + dr["Sub_Account_No"].ToString() + "'");
    //                if (drCustDetails != null)
    //                {
    //                    if (drCustDetails.Length > 0)
    //                    {
    //                        dtSubDetails = drCustDetails.CopyToDataTable();
    //                    }
    //                }

    //                //string[] stringSeparators = new string[] { "<TD>~" };

    //                //string[] strColumn = strNewHTML.Split(stringSeparators, StringSplitOptions.None);

    //                int intstartindex = 0;
    //                int intEndindex = 0;

    //                int inttbodysize = 0;
    //                if (strNewHTML.Contains("<TBODY>"))
    //                    intstartindex = strNewHTML.IndexOf("<TBODY>");
    //                if (strNewHTML.Contains("</TBODY>"))
    //                {
    //                    intEndindex = strNewHTML.IndexOf("</TBODY>");
    //                    inttbodysize = 8;
    //                }


    //                string strCutString = strNewHTML.Substring(intstartindex, intEndindex - intstartindex + inttbodysize);
    //                string strCutStringTD = string.Empty;
    //                string[] stringSeparators1 = new string[] { "<TR>" };

    //                string[] strCutSplit = strCutString.Split(stringSeparators1, StringSplitOptions.None);

    //                if (strCutSplit.Length > 2)
    //                {
    //                    int intEndindx = strCutSplit[2].IndexOf("</TR>");
    //                    strCutStringTD = "<TR>" + strCutSplit[2].Substring(0, intEndindx) + "</TR>";
    //                }


    //                if (dtSubDetails.Rows.Count > 0)
    //                {
    //                    int i = 1;
    //                    int j = 1;
    //                    string strSubHTml = string.Empty;
    //                    foreach (DataRow drsub in dtSubDetails.Rows)
    //                    {
    //                        strSubHTml += strCutStringTD.Replace("~", i + "~");
    //                        ++i;
    //                    }



    //                    foreach (DataRow drsub1 in dtSubDetails.Rows)
    //                    {
    //                        foreach (DataColumn dcolsub1 in dtSubDetails.Columns)
    //                        {
    //                            string strColnamesub = string.Empty;
    //                            strColnamesub = j.ToString() + "~" + dcolsub1.ColumnName + j.ToString() + "~";
    //                            if (strSubHTml.Contains(strColnamesub))
    //                            {
    //                                strSubHTml = strSubHTml.Replace(strColnamesub, drsub1[dcolsub1].ToString());
    //                            }
    //                        }
    //                        j++;
    //                    }
    //                    if ((!string.IsNullOrEmpty(strCutStringTD)) && (!string.IsNullOrEmpty(strSubHTml)))
    //                        strNewHTML = strNewHTML.Replace(strCutStringTD, strSubHTml);
    //                }

    //                //Sending Mail
    //                //FunPriSendMail(strNewHTML);
    //                FunPriGeneratePDF(strNewHTML, dr["Account_No"].ToString().Replace("/", ""));
    //            }
    //        }



    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    //private void FunPriPaymentMail()
    //{

    //    try
    //    {
    //        //Getting Database value
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", intCompanyID.ToString());
    //        Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
    //        //Procparam.Add("@Location_Id", ddlLocation.SelectedValue);
    //        DataSet DS = new DataSet();
    //        DataTable dtDetails = new DataTable();
    //        DataTable dtSubDetails = new DataTable();
    //        string strNewHTML = string.Empty;
    //        dtDetails = Utility.GetDefaultData("S3G_DUN_PaymentDtls", Procparam);

    //        strNewHTML = FTBTemplate.Text;
    //        int intstartindex = 0;
    //        int intEndindex = 0;

    //        int inttbodysize = 0;
    //        if (strNewHTML.Contains("<TBODY>"))
    //            intstartindex = strNewHTML.IndexOf("<TBODY>");
    //        if (strNewHTML.Contains("</TBODY>"))
    //        {
    //            intEndindex = strNewHTML.IndexOf("</TBODY>");
    //            inttbodysize = 8;
    //        }
    //        string strCutString = strNewHTML.Substring(intstartindex, intEndindex - intstartindex + inttbodysize);
    //        string strCutStringTD = string.Empty;
    //        string[] stringSeparators1 = new string[] { "<TR>" };

    //        string[] strCutSplit = strCutString.Split(stringSeparators1, StringSplitOptions.None);

    //        if (strCutSplit.Length > 2)
    //        {
    //            int intEndindx = strCutSplit[2].IndexOf("</TR>");
    //            strCutStringTD = "<TR>" + strCutSplit[2].Substring(0, intEndindx) + "</TR>";
    //        }

    //        if (dtDetails.Rows.Count > 0)
    //        {
    //            int i = 1;
    //            int j = 1;
    //            string strSubHTml = string.Empty;
    //            foreach (DataRow drsub in dtDetails.Rows)
    //            {
    //                strSubHTml += strCutStringTD.Replace("~", i + "~");
    //                ++i;
    //            }



    //            foreach (DataRow drsub1 in dtDetails.Rows)
    //            {
    //                foreach (DataColumn dcolsub1 in dtDetails.Columns)
    //                {
    //                    string strColnamesub = string.Empty;
    //                    strColnamesub = j.ToString() + "~" + dcolsub1.ColumnName + j.ToString() + "~";
    //                    if (strSubHTml.Contains(strColnamesub))
    //                    {
    //                        strSubHTml = strSubHTml.Replace(strColnamesub, drsub1[dcolsub1].ToString());
    //                    }
    //                }
    //                j++;
    //            }
    //            if ((!string.IsNullOrEmpty(strCutStringTD)) && (!string.IsNullOrEmpty(strSubHTml)))
    //                strNewHTML = strNewHTML.Replace(strCutStringTD, strSubHTml);

    //            //Any Header part data will be replaced by first row value

    //            foreach (DataColumn dcol in dtDetails.Columns)
    //            {
    //                string strColname = string.Empty;
    //                strColname = "~" + dcol.ColumnName + "~";
    //                if (strNewHTML.Contains(strColname))
    //                    strNewHTML = strNewHTML.Replace(strColname, dtDetails.Rows[0][dcol].ToString());
    //            }



    //        }
    //        //Sending Mail
    //        //FunPriSendMail(strNewHTML);
    //        FunPriGeneratePDF(strNewHTML, "Disbursement" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", ""));


    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    private string FunPriTempPriDetails(string str, DataTable dtDetails)
    {
        try
        {
            int j = 1;
            string[] q = Regex.Split(str, "</TR>");
            string strHeader = q[0] + "</TR>";
            string strDetails = q[1] + "</TR>";
            string strDontChange = q[1] + "</TR>";

            string Output = string.Empty;
            if (dtDetails.Rows.Count == 0)
            {
                Output = "<TR>";
                foreach (DataColumn dcolsub1 in dtDetails.Columns)
                {

                    string strColnamesub = string.Empty;
                    strColnamesub = "~" + dcolsub1.ColumnName + "~";
                    if (strColnamesub == strColnamesub.ToUpper())
                    {

                        strDetails = strDetails.ToUpper();
                    }
                    if (strDetails.Contains(strColnamesub))
                    {
                        strDetails = strDetails.Replace(strColnamesub, "NIL");
                        Output = strDetails;
                    }

                }
                Output += "</TR>";
                Output = strHeader + Output + "</TBODY></TABLE>";
                return Output;
            }

            foreach (DataRow drsub1 in dtDetails.Rows)
            {
                strDetails = strDontChange;
                foreach (DataColumn dcolsub1 in dtDetails.Columns)
                {
                    string strColnamesub = string.Empty;
                    strColnamesub = "~" + dcolsub1.ColumnName + "~";
                    if (strColnamesub == strColnamesub.ToUpper())
                    {

                        strDetails = strDetails.ToUpper();
                    }
                    if (strDetails.Contains(strColnamesub))
                    {
                        if (strColnamesub == ("~" + "SlNo" + "~"))
                        {
                            strDetails = strDetails.Replace(strColnamesub, j.ToString());
                            j++;
                        }
                        strDetails = strDetails.Replace(strColnamesub, drsub1[dcolsub1].ToString());
                    }

                }
                Output += strDetails;
            }
            Output = strHeader + Output + "</TBODY></TABLE>";
            return Output;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private string FunPriHeadPriDetails(string str, DataTable dtHeader, DataRow dr)
    {
        try
        {
            foreach (DataColumn dcol in dtHeader.Columns)
            {

                string ColName1 = string.Empty;
                ColName1 = "~" + dcol.ColumnName + "~";
                if (ColName1 == ColName1.ToUpper())
                {
                    str = str.ToUpper();
                }
                if (str.Contains(ColName1))
                    str = str.Replace(ColName1, dr[dcol].ToString());
            }


            return str;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriRepayDetails()
    {

    }

    private string FunPriCheckDatatable(string strTable, DataSet ds)
    {
        string Table = string.Empty;

        DataTable dt;


        for (int i = 0; i < ds.Tables.Count; i++)
        {
            dt = ds.Tables[i].Copy();
            foreach (DataColumn dcol in dt.Columns)
            {
                string ColName1 = string.Empty;
                ColName1 = "~" + dcol.ColumnName + "~";
                if (ColName1 == ColName1.ToUpper())
                {
                    strTable = strTable.ToUpper();
                }
                if (ColName1 != "~SlNo~")
                {
                    if (strTable.Contains(ColName1.ToUpper()))
                    {
                        return dt.TableName;
                    }

                }

            }
        }



        return Table;
    }

    //private void FunPriPricingDetails(int CompanyID, string LOB, string Template)
    //{
    //    try
    //    {
    //        Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", CompanyID.ToString());
    //        if (LOB != "")
    //            Procparam.Add("@Lob_Id", LOB);


    //        DataSet DS = new DataSet();

    //        DataTable dtHeader = new DataTable("Header");
    //        DataTable dtHeadDetails = new DataTable("Details");
    //        DataTable dtHeadSubDetails = new DataTable("Subdetails");


    //        DataTable dtDetails = new DataTable();



    //        string strHtml = string.Empty;
    //        string strHtml1 = string.Empty;
    //        string strHtml2 = string.Empty;
    //        DataSet dsTabs = new DataSet();
    //        DS = Utility.GetDataset("S3G_Sys_GetTmplPricingDetails", Procparam);
    //        if (DS != null)
    //        {
    //            if (DS.Tables[0].Rows.Count > 0)
    //                dtHeader = DS.Tables[0];//.Copy();
    //            //if (DS.Tables[1].Rows.Count > 0)
    //            dtHeadDetails = DS.Tables[1].Copy();
    //            if (DS.Tables[2].Rows.Count > 0)
    //                dtHeadSubDetails = DS.Tables[2];//.Copy();

    //            //dsTabs.Tables.Add(dtHeadDetails);
    //            //dsTabs.Tables.Add(dtHeadSubDetails);

    //        }

    //        if (dtHeader.Rows.Count == 0)
    //            return;

    //        strHtml = Template;
    //        string[] a = Regex.Split(strHtml, "<TBODY>");
    //        foreach (DataRow dr in dtHeader.Rows)
    //        {
    //            if (!strHtml.Contains("<TBODY>"))
    //            {
    //                strHtml = FunPriHeadPriDetails(strHtml, dtHeader, dr);
    //                FunPriGeneratePDF(strHtml, dr["Pricing_ID"].ToString());
    //                return;
    //            }

    //            string strFinal = string.Empty;
    //            for (int i = 0; i < a.Length; i++)
    //            {
    //                string str = a[i];
    //                string strWithoutTable;
    //                string strTable;

    //                if (str.Contains("<TR>"))
    //                {
    //                    string[] q = Regex.Split(str, "</TABLE>");
    //                    strTable = "<TBODY>" + q[0] + "</TABLE>";
    //                    strWithoutTable = q[1];
    //                    DataRow[] dtr = null;

    //                    string strWhichTable = FunPriCheckDatatable(strTable, DS);

    //                    if (strWhichTable == "Table2")
    //                    {

    //                        dtDetails = dtHeadSubDetails.Clone();
    //                        dtr = dtHeadSubDetails.Select("Pricing_ID=" + dr["Pricing_ID"]);

    //                    }
    //                    else if (strWhichTable == "Table1")
    //                    {
    //                        dtDetails = dtHeadDetails.Clone();
    //                        // dtDetails = dtHeadDetails.Copy();
    //                        dtr = dtHeadDetails.Select("Pricing_ID=" + dr["Pricing_ID"]);
    //                    }


    //                    if (dtr.Length > 0)
    //                    {
    //                        dtDetails = dtr.CopyToDataTable();
    //                    }
    //                    strTable = FunPriTempPriDetails(strTable, dtDetails);
    //                    strFinal += strTable + strWithoutTable;

    //                }
    //                else
    //                {

    //                    strFinal += FunPriHeadPriDetails(str, dtHeader, dr);
    //                }
    //            }

    //            FunPriGeneratePDF(strFinal, dr["Pricing_ID"].ToString());
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }



    //    //if (dtHeader.Rows.Count > 0)
    //    //{

    //    //    foreach (DataRow dr in dtHeader.Rows)
    //    //    {

    //    //        strHtml1 = FTBTemplate.Text;
    //    //        string[] a = Regex.Split(strHtml1, "</TBODY>");
    //    //        strHtml1 = a[0].ToString();

    //    //        if (!(a.Length > 1))
    //    //            if (!(a[0].Contains("<TABLE>")))
    //    //            {

    //    //                //if (!(a[1].Contains("</TABLE>")))
    //    //                //{
    //    //                foreach (DataColumn dcol in dtHeader.Columns)
    //    //                {
    //    //                    string ColName1 = string.Empty;
    //    //                    ColName1 = "~" + dcol.ColumnName + "~";
    //    //                    if (strHtml1.Contains(ColName1))
    //    //                        strHtml1 = strHtml1.Replace(ColName1, dr[dcol].ToString());
    //    //                }
    //    //                FunPriGeneratePDF(strHtml1, dr["Pricing_ID"].ToString());
    //    //                return;


    //    //            }
    //    //        foreach (DataColumn dcol in dtHeader.Columns)
    //    //        {
    //    //            string ColName1 = string.Empty;
    //    //            ColName1 = "~" + dcol.ColumnName + "~";
    //    //            if (strHtml1.Contains(ColName1))
    //    //                strHtml1 = strHtml1.Replace(ColName1, dr[dcol].ToString());
    //    //        }
    //    //        DataRow[] drCustDetails = dtHeadDetails.Select("Customer_ID = " + dr["Customer_ID"].ToString());
    //    //        if (drCustDetails != null)
    //    //        {
    //    //            if (drCustDetails.Length > 0)
    //    //            {
    //    //                dtDetails = drCustDetails.CopyToDataTable();
    //    //            }
    //    //        }

    //    //        //string[] stringSeparators = new string[] { "<TD>~" };

    //    //        //string[] strColumn = strNewHTML.Split(stringSeparators, StringSplitOptions.None);
    //    //        strHtml1 += "</TBODY>";
    //    //        int intstartindex = 0;
    //    //        int intEndindex = 0;

    //    //        int inttbodysize = 0;
    //    //        if (strHtml1.Contains("<TBODY>"))
    //    //            intstartindex = strHtml1.IndexOf("<TBODY>");
    //    //        if (strHtml1.Contains("</TBODY>"))
    //    //        {
    //    //            intEndindex = strHtml1.IndexOf("</TBODY>");
    //    //            inttbodysize = 8;
    //    //        }


    //    //        string strCutString = strHtml1.Substring(intstartindex, intEndindex - intstartindex + inttbodysize);
    //    //        string strCutStringTD = string.Empty;
    //    //        string[] stringSeparators1 = new string[] { "<TR>" };

    //    //        string[] strCutSplit = strCutString.Split(stringSeparators1, StringSplitOptions.None);

    //    //        if (strCutSplit.Length > 2)
    //    //        {
    //    //            int intEndindx = strCutSplit[2].IndexOf("</TR>");
    //    //            strCutStringTD = "<TR>" + strCutSplit[2].Substring(0, intEndindx) + "</TR>";
    //    //        }

    //    //        if (dtDetails.Rows.Count == 0)
    //    //        {
    //    //            int j = 0;
    //    //            string strColnamesub = string.Empty;
    //    //            string strSubHTml = "<TR>";
    //    //            foreach (DataColumn dcolsub1 in dtHeadDetails.Columns)
    //    //            {
    //    //                if (j != 0 && j != 1) {
    //    //                    strSubHTml += "<TD>Nil</TD>";
    //    //                }
    //    //                j++;
    //    //            }
    //    //            strSubHTml += "</TR>";
    //    //            if ((!string.IsNullOrEmpty(strCutStringTD)) && (!string.IsNullOrEmpty(strSubHTml)))
    //    //                strHtml1 = strHtml1.Replace(strCutStringTD, strSubHTml);
    //    //        }

    //    //        if (dtDetails.Rows.Count > 0)
    //    //        {
    //    //            int i = 1;
    //    //            int j = 1;
    //    //            string strSubHTml = string.Empty;
    //    //            foreach (DataRow drsub in dtDetails.Rows)
    //    //            {
    //    //                strSubHTml += strCutStringTD.Replace("~", i + "~");
    //    //                ++i;
    //    //            }



    //    //            foreach (DataRow drsub1 in dtDetails.Rows)
    //    //            {

    //    //                foreach (DataColumn dcolsub1 in dtDetails.Columns)
    //    //                {
    //    //                    string strColnamesub = string.Empty;
    //    //                    strColnamesub = j.ToString() + "~" + dcolsub1.ColumnName + j.ToString() + "~";
    //    //                    if (strSubHTml.Contains(strColnamesub))
    //    //                    {
    //    //                        if (strColnamesub == (j.ToString() + "~" + "SlNo" + j.ToString() + "~"))
    //    //                        {


    //    //                                strSubHTml = strSubHTml.Replace(strColnamesub, j.ToString());

    //    //                        }

    //    //                        strSubHTml = strSubHTml.Replace(strColnamesub, drsub1[dcolsub1].ToString());
    //    //                    }
    //    //                }
    //    //                j++;

    //    //            }
    //    //            if ((!string.IsNullOrEmpty(strCutStringTD)) && (!string.IsNullOrEmpty(strSubHTml)))
    //    //                strHtml1 = strHtml1.Replace(strCutStringTD, strSubHTml);
    //    //        }
    //    //        if (a[1].Contains("<TBODY>"))
    //    //        {
    //    //            strHtml2 = a[1].ToString();
    //    //            DataRow[] drCust = dtHeadSubDetails.Select("Pricing_ID =" + dr["Pricing_ID"].ToString());

    //    //            if (drCust != null)
    //    //            {
    //    //                if (drCust.Length > 0)
    //    //                {
    //    //                    dtSubDetails = drCust.CopyToDataTable();
    //    //                }
    //    //            }
    //    //            strHtml2 += "</TBODY>";


    //    //            //strHtml1 = a[1].ToString();
    //    //            //string q = strHtml1.Substring(intstartindex + intEndindex );
    //    //            int intstartindex1 = 0;
    //    //            int intEndindex1 = 0;

    //    //            int inttbodysize1 = 0;
    //    //            if (strHtml2.Contains("<TBODY>"))
    //    //                intstartindex1 = strHtml2.IndexOf("<TBODY>");
    //    //            if (strHtml2.Contains("</TBODY>"))
    //    //            {
    //    //                intEndindex1 = strHtml2.IndexOf("</TBODY>");
    //    //                inttbodysize = 8;
    //    //            }


    //    //            string strCutString1 = strHtml2.Substring(intstartindex1, intEndindex1 - intstartindex1 + inttbodysize);
    //    //            string strCutStringTD1 = string.Empty;
    //    //            string[] stringSeparators2 = new string[] { "<TR>" };

    //    //            string[] strCutSplit1 = strCutString1.Split(stringSeparators2, StringSplitOptions.None);

    //    //            if (strCutSplit1.Length > 2)
    //    //            {
    //    //                int intEndindx = strCutSplit1[2].IndexOf("</TR>");
    //    //                strCutStringTD1 = "<TR>" + strCutSplit1[2].Substring(0, intEndindx) + "</TR>";
    //    //            }
    //    //            if (dtSubDetails.Rows.Count > 0)
    //    //            {
    //    //                int i = 1;
    //    //                int j = 1;
    //    //                string strSubHTml = string.Empty;
    //    //                foreach (DataRow drsub in dtSubDetails.Rows)
    //    //                {
    //    //                    strSubHTml += strCutStringTD1.Replace("~", i + "~");
    //    //                    ++i;
    //    //                }



    //    //                foreach (DataRow drsub1 in dtSubDetails.Rows)
    //    //                {
    //    //                    foreach (DataColumn dcolsub1 in dtSubDetails.Columns)
    //    //                    {
    //    //                        string strColnamesub = string.Empty;
    //    //                        strColnamesub = j.ToString() + "~" + dcolsub1.ColumnName + j.ToString() + "~";
    //    //                        if (strSubHTml.Contains(strColnamesub))
    //    //                        {
    //    //                            strSubHTml = strSubHTml.Replace(strColnamesub, drsub1[dcolsub1].ToString());
    //    //                        }
    //    //                    }
    //    //                    j++;
    //    //                }

    //    //                strHtml1 += strHtml2;



    //    //                if ((!string.IsNullOrEmpty(strCutStringTD1)) && (!string.IsNullOrEmpty(strSubHTml)))
    //    //                    strHtml1 = strHtml1.Replace(strCutStringTD1, strSubHTml);
    //    //                strHtml1 += "</TABLE>";
    //    //            }
    //    //        }
    //    //        else
    //    //        {


    //    //            strHtml1 += "</TABLE>";
    //    //        }


    //    //        FunPriGeneratePDF(strHtml1, dr["Pricing_ID"].ToString());
    //    //    }

    //    //}

    //}

    //private void FunPriDunninMail()
    //{

    //    try
    //    {
    //        //Getting Database value
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_Id", intCompanyID.ToString());
    //        if (ddlLOB.SelectedIndex > 0)
    //            Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
    //        DataSet DS = new DataSet();
    //        DataTable dtHeader = new DataTable();
    //        DataTable dtDetails = new DataTable();
    //        DataTable dtSubDetails = new DataTable();
    //        string strNewHTML = string.Empty;


    //        DS = Utility.GetDataset("S3G_DUN_DemandDtls", Procparam);
    //        if (DS != null)
    //        {
    //            if (DS.Tables[0].Rows.Count > 0)
    //                dtHeader = DS.Tables[0].Copy();
    //            if (DS.Tables[1].Rows.Count > 0)
    //                dtDetails = DS.Tables[1].Copy();
    //        }
    //        if (dtHeader.Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in dtHeader.Rows)
    //            {
    //                strNewHTML = FTBTemplate.Text;

    //                foreach (DataColumn dcol in dtHeader.Columns)
    //                {
    //                    string strColname = string.Empty;
    //                    strColname = "~" + dcol.ColumnName + "~";
    //                    if (strNewHTML.Contains(strColname))
    //                        strNewHTML = strNewHTML.Replace(strColname, dr[dcol].ToString());
    //                }

    //                DataRow[] drCustDetails = dtDetails.Select("Customer_ID = " + dr["Customer_ID"].ToString());
    //                if (drCustDetails != null)
    //                {
    //                    if (drCustDetails.Length > 0)
    //                    {
    //                        dtSubDetails = drCustDetails.CopyToDataTable();
    //                    }
    //                }

    //                //string[] stringSeparators = new string[] { "<TD>~" };

    //                //string[] strColumn = strNewHTML.Split(stringSeparators, StringSplitOptions.None);

    //                int intstartindex = 0;
    //                int intEndindex = 0;

    //                int inttbodysize = 0;
    //                if (strNewHTML.Contains("<TBODY>"))
    //                    intstartindex = strNewHTML.IndexOf("<TBODY>");
    //                if (strNewHTML.Contains("</TBODY>"))
    //                {
    //                    intEndindex = strNewHTML.IndexOf("</TBODY>");
    //                    inttbodysize = 8;
    //                }


    //                string strCutString = strNewHTML.Substring(intstartindex, intEndindex - intstartindex + inttbodysize);
    //                string strCutStringTD = string.Empty;
    //                string[] stringSeparators1 = new string[] { "<TR>" };

    //                string[] strCutSplit = strCutString.Split(stringSeparators1, StringSplitOptions.None);

    //                if (strCutSplit.Length > 2)
    //                {
    //                    int intEndindx = strCutSplit[2].IndexOf("</TR>");
    //                    strCutStringTD = "<TR>" + strCutSplit[2].Substring(0, intEndindx) + "</TR>";
    //                }


    //                if (dtSubDetails.Rows.Count > 0)
    //                {
    //                    int i = 1;
    //                    int j = 1;
    //                    string strSubHTml = string.Empty;
    //                    foreach (DataRow drsub in dtSubDetails.Rows)
    //                    {
    //                        strSubHTml += strCutStringTD.Replace("~", i + "~");
    //                        ++i;
    //                    }



    //                    foreach (DataRow drsub1 in dtSubDetails.Rows)
    //                    {
    //                        foreach (DataColumn dcolsub1 in dtSubDetails.Columns)
    //                        {
    //                            string strColnamesub = string.Empty;
    //                            strColnamesub = j.ToString() + "~" + dcolsub1.ColumnName + j.ToString() + "~";
    //                            if (strSubHTml.Contains(strColnamesub))
    //                            {
    //                                strSubHTml = strSubHTml.Replace(strColnamesub, drsub1[dcolsub1].ToString());
    //                            }
    //                        }
    //                        j++;
    //                    }
    //                    if ((!string.IsNullOrEmpty(strCutStringTD)) && (!string.IsNullOrEmpty(strSubHTml)))
    //                        strNewHTML = strNewHTML.Replace(strCutStringTD, strSubHTml);
    //                }



    //                //Sending Mail
    //                //FunPriSendMail(strNewHTML);
    //                FunPriGeneratePDF(strNewHTML, dr["Customer_ID"].ToString());
    //            }
    //        }

    //        //Utility.FunShowAlertMsg(this, "Mail Sent Successfully");

    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }

    //}

    //private void FunPriGeneratePDF(string strNewHTML, string FileName)
    //{
    //    try
    //    {
    //        if (strNewHTML.Contains("&NBSP;"))
    //        {
    //            strNewHTML = strNewHTML.Replace("&NBSP;", "<BR>");
    //        }
    //        if (strNewHTML.Contains("&nbsp;"))
    //        {
    //            strNewHTML = strNewHTML.Replace("&nbsp;", "<BR>");

    //        }
    //        String htmlText = strNewHTML.Replace("</P>", "</P></BR>");
    //        htmlText = htmlText.Replace("<HR>", "<HR width=\"100\">");

    //        string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + FileName + ".pdf");
    //        string strFileName = "/System Admin/PDF Files/" + FileName + ".pdf";

    //        if (!string.IsNullOrEmpty(txtDocumentPath.Text))
    //        {
    //            strnewFile = txtDocumentPath.Text + "\\" + FileName + ".pdf";
    //            strFileName = txtDocumentPath.Text.Replace("\\", "//") + "//" + FileName + ".pdf";
    //        }

    //        Document doc = new Document();
    //        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strnewFile, FileMode.Create));
    //        doc.AddCreator("Sundaram Infotech Solutions Limited");
    //        doc.AddTitle("Dunning Letter_" + FileName);
    //        doc.Open();
    //        List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);
    //        for (int k = 0; k < htmlarraylist.Count; k++)
    //        { doc.Add((IElement)htmlarraylist[k]); }
    //        doc.AddAuthor("S3G Team");
    //        doc.Close();
    //        //System.Diagnostics.Process.Start(strnewFile);
    //        string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
    //    }
    //    catch (Exception e)
    //    {
    //        throw e;

    //    }
    //}

    private static void FunPriSendMail(string strNewHTML)
    {
        Dictionary<string, string> dictMail = new Dictionary<string, string>();
        dictMail.Add("FromMail", "saran.m@sundaraminfotech.in");
        dictMail.Add("ToMail", "bashyam.k@sundaraminfotech.in,saran.m@sundaraminfotech.in");
        //dictMail.Add("ToMail", "saran.m@sundaraminfotech.in");
        dictMail.Add("Subject", "Reg:Dunning Mail ");
        ArrayList arrMailAttachement = new ArrayList();
        StringBuilder strBody = new StringBuilder();
        strBody.Append(strNewHTML);
        Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
    }

    private void FunInsertExclusionDetails()
    {
        try
        {
            dtExclusionDetails = new DataTable();
            if (ViewState["dtExclusionDetails"] != null)
            {
                dtExclusionDetails = (DataTable)ViewState["dtExclusionDetails"];
            }

            //deleting dummy Row
            if (dtExclusionDetails.Rows.Count == 1)
            {
                if (dtExclusionDetails.Rows[0]["CategoryId"].ToString() == string.Empty)
                    dtExclusionDetails.Rows[0].Delete();
            }


            DropDownList ddlFooterCategory = (DropDownList)GrvExclusion.FooterRow.FindControl("ddlFooterCategory");
            Label lblFooterId = (Label)GrvExclusion.FooterRow.FindControl("lblFooterId");
            UserControls_LOBMasterView ucCustomerLov = GrvExclusion.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
            TextBox txtCode = (TextBox)ucCustomerLov.FindControl("txtName");
            TextBox txtDescription = (TextBox)GrvExclusion.FooterRow.FindControl("txtDescription");
            string sError = "";
            DataRow drExclusionDtls;
            drExclusionDtls = dtExclusionDetails.NewRow();
            if (txtCode.Text == "")
            {
                sError = "Select Code.";
            }
            if (txtDescription.Text == "")
            {
                sError = sError + "  Select Description.";
            }
            if (sError == "")
            {
                drExclusionDtls["CategoryId"] = ddlFooterCategory.SelectedValue;
                drExclusionDtls["Category"] = ddlFooterCategory.SelectedItem.Text;
                drExclusionDtls["Id"] = lblFooterId.Text;
                drExclusionDtls["Code"] = txtCode.Text;
                drExclusionDtls["Description"] = txtDescription.Text;

                dtExclusionDetails.Rows.Add(drExclusionDtls);

                ViewState["dtExclusionDetails"] = dtExclusionDetails;
                FunFillgrid(GrvExclusion, dtExclusionDetails);
                FunPriLoadExclusionCategory();
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, sError);
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }


    }

    private void FunPriRemoveExclusionDetails(int intRowIndex)
    {
        try
        {
            dtExclusionDetails = (DataTable)ViewState["dtExclusionDetails"];
            dtExclusionDetails.Rows.RemoveAt(intRowIndex);
            if (dtExclusionDetails.Rows.Count == 0)
            {
                FunPriBindEmpty();
            }
            else
            {
                FunFillgrid(GrvExclusion, dtExclusionDetails);
            }
            FunPriLoadExclusionCategory();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }


    }

    private void FunSetCodeLOV()
    {
        try
        {
            if (GrvExclusion.FooterRow != null)
            {
                UserControls_LOBMasterView ucCustomerLov = GrvExclusion.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
                DropDownList ddlFooterCategory = GrvExclusion.FooterRow.FindControl("ddlFooterCategory") as DropDownList;
                ucCustomerLov.strLOV_Code = "TCMD";
                if (ddlFooterCategory.SelectedIndex > 0)
                {
                    switch (ddlFooterCategory.SelectedValue)
                    {
                        case "1"://Location
                            ucCustomerLov.strLOV_Code = "TLOC";
                            break;
                        case "2"://Customer
                            ucCustomerLov.strLOV_Code = "TCMD";
                            break;
                        case "3"://Account
                            ucCustomerLov.strLOV_Code = "TACC";
                            break;
                        case "4"://Asset
                            ucCustomerLov.strLOV_Code = "TAST";
                            break;
                        case "5"://Employee
                            ucCustomerLov.strLOV_Code = "TUSM";
                            break;
                        case "6"://Product
                            ucCustomerLov.strLOV_Code = "TPRO";
                            break;
                        default://Customer
                            ucCustomerLov.strLOV_Code = "TCMD";
                            break;
                    }

                }
                ucCustomerLov.strControlID = ucCustomerLov.ClientID;
                TextBox txt1 = (TextBox)ucCustomerLov.FindControl("txtName");
                txt1.Attributes.Add("onfocus", "fnLoadCustomerg()");
                //end
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadCustomerDtls()
    {
        try
        {
            UserControls_LOBMasterView ucCustomerLov = GrvExclusion.FooterRow.FindControl("ucCustomerLov") as UserControls_LOBMasterView;
            TextBox txtName = (TextBox)ucCustomerLov.FindControl("txtName");
            TextBox txtDescription = (TextBox)GrvExclusion.FooterRow.FindControl("txtDescription");
            Label lblFooterId = (Label)GrvExclusion.FooterRow.FindControl("lblFooterId");

            HiddenField hdnCustomerId = (HiddenField)ucCustomerLov.FindControl("hdnID");
            if (hdnCustomerId != null)
            {
                if (hdnCustomerId.Value != string.Empty)
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", intCompanyID.ToString());
                    Procparam.Add("@LOV_Code", ucCustomerLov.strLOV_Code);
                    Procparam.Add("@LOV_Id", hdnCustomerId.Value);
                    DataTable dt = Utility.GetDefaultData("S3G_SYSAD_GetLOVDtls", Procparam);
                    if (dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["Code"].ToString()))
                            txtName.Text = dt.Rows[0]["Code"].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[0]["Name"].ToString()))
                            txtDescription.Text = dt.Rows[0]["Name"].ToString();
                        lblFooterId.Text = hdnCustomerId.Value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProLoadLOB()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            Procparam.Add("@Program_ID", "215");
            Procparam.Add("@Is_Active", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            Procparam = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //protected void FunProLoadBranch()
    //{
    //    try
    //    {
    //        Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyID.ToString());
    //        Procparam.Add("@User_ID", intUserID.ToString());
    //        if (ddlLOB.SelectedIndex > 0)
    //            Procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
    //        Procparam.Add("@Program_ID", "215");
    //        if (strMode == "C")
    //            Procparam.Add("@Is_Active", "1");
    //        ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "ALL", new string[] { "Location_ID", "Location_Code", "Location_Name" });
    //        Procparam = null;
    //    }
    //    catch (Exception ex)
    //    {
    //        ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}

    protected void FunProLoadControls()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@LookupType_Code", "103");//Template Type
            ddlTemplateType.BindDataTable(SPNames.S3G_LOANAD_GetLookupTypeDescription, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            Procparam.Remove("@LookupType_Code");
            Procparam.Add("@LookupType_Code", "104");//Mode of Mail
            ddlModeofMail.BindDataTable(SPNames.S3G_LOANAD_GetLookupTypeDescription, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            FunPriLoadExclusionCategory();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadExclusionCategory()
    {
        try
        {
            if (GrvExclusion.FooterRow != null)
            {
                DropDownList ddlFooterCategory = (DropDownList)GrvExclusion.FooterRow.FindControl("ddlFooterCategory");
                if (ddlFooterCategory != null)
                {
                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", intCompanyID.ToString());
                    Procparam.Add("@LookupType_Code", "105");//Exclusion Category
                    ddlFooterCategory.BindDataTable(SPNames.S3G_LOANAD_GetLookupTypeDescription, Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
                }
            }
            Procparam = null;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunPriBindVariables()
    {
        try
        {
            DataTable dt = new DataTable();
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@Template_Type_Id", ddlTemplateType.SelectedValue);
            dt = Utility.GetDefaultData("S3G_SYSAD_Get_Var_Fields", Procparam);
            FreeTextBoxControls.ToolbarDropDownList tddl = (FreeTextBoxControls.ToolbarDropDownList)(FTBTemplate.Toolbars[1].Items[FTBTemplate.Toolbars[1].Items.Count - 1]);
            if (tddl.Items.Count > 0)
                tddl.Items.Clear();
            ToolbarListItem item;
            foreach (DataRow dr in dt.Rows)
            {
                item = new ToolbarListItem(Convert.ToString(dr["FieldId"]), Convert.ToString(dr["FieldValue"]));
                tddl.Items.Add(item);
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void FunProLoadVariables()
    {
        try
        {
            DataTable dt = new DataTable();
            Toolbar tb1 = new Toolbar();

            FreeTextBoxControls.ToolbarButton ib = new ToolbarButton();
            ib.ButtonImage = "TextWrap";
            ib.Title = "Text Wrap";
            ib.ScriptBlock = "javascript:FTBDesignWrap();";
            FTBTemplate.Toolbars[1].Items.Add(ib);

            FreeTextBoxControls.ToolbarDropDownList ih = new ToolbarDropDownList();
            ToolbarListItem item;
            foreach (DataRow dr in dt.Rows)
            {
                item = new ToolbarListItem(Convert.ToString(dr["FieldId"]), Convert.ToString(dr["FieldValue"]));
                ih.Items.Add(item);
            }
            ih.Title = "Variables";
            ih.ScriptBlock = "this.ftb.InsertHtml(this.list.options[this.list.options.selectedIndex].value);";
            FTBTemplate.Toolbars[1].Items.Add(ih);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    //protected void btnPdf_Click(object sender, EventArgs e)
    //{

    //    Dictionary<string, string> dictparam1 = new Dictionary<string, string>();
    //    dictparam1.Add("@Company_Id", intCompanyID.ToString());
    //    if (ddlLOB.SelectedIndex > 0)
    //        dictparam1.Add("@Lob_Id", ddlLOB.SelectedValue);
    //    DataSet DS = new DataSet();
    //    DataTable dtHeader = new DataTable();
    //    DataTable dtDetails = new DataTable();
    //    DataTable dtCusDet=new DataTable();
    //    string strHtml1=string.Empty;

    //    DataSet ds = new DataSet();
    //    ds = Utility.GetDataset("S3G_DUN_DemandDtls", dictparam1);
    //    if (ds != null)
    //    {
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            dtHeader = ds.Tables[0].Copy();

    //        }
    //        if (ds.Tables[1].Rows.Count > 0)
    //        {
    //            dtDetails = ds.Tables[1].Copy();
    //        }

    //        if (dtHeader.Rows.Count > 0)
    //        {
    //            foreach (DataRow dr in dtHeader.Rows)
    //            {
    //                strHtml1 = FTBTemplate.Text;

    //                foreach (DataColumn dcol in dtHeader.Columns)
    //                {
    //                    string strColmname = dcol.ColumnName;
    //                    if (strHtml1.Contains(strColmname))
    //                        strHtml1 = strHtml1.Replace(strColmname, dr[dcol].ToString());

    //                }

    //                DataRow[] drCusDet = dtDetails.Select("Customer_Id =" + dr["Customer_Id"].ToString());
    //                if(drCusDet != null)
    //                { 
    //                    if(drCusDet.Length>0)
    //                    {  
    //                        dtCusDet=drCusDet.CopyToDataTable();
    //                    }






    //                    }

    //        }

    //    }

    //}

    //To disable controls based on Create/Modify/Query
    /// <summary>
    ///This method is used to Disable the controls based on Create/Modify/Query Mode. 
    ///Here argument is used as intModeID to differentiate the Modes.
    /// </summary>
    /// <param name="intModeID"></param>
    private void FunPriDisableControls(int intModeID)
    {
        try
        {

            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    FunPriBindEmpty();
                    chkIsActive.Enabled = false;
                    chkIsActive.Checked = true;
                    //btnGenerate.Visible = false;
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                    if (!string.IsNullOrEmpty(strTemplateNo))
                        FunPriLoadTemplateModification(strTemplateNo);
                    //ddlLocation.Clear();
                    ddlTemplateType.ClearDropDownList();
                    ddlModeofMail.ClearDropDownList();
                    chkIsActive.Enabled = true;
                    btnClear.Enabled = false;
                    //btnGenerate.Visible = true;
                    break;


                case -1:// Query Mode


                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    if (!string.IsNullOrEmpty(strTemplateNo))
                        FunPriLoadTemplateModification(strTemplateNo);
                    //ddlLocation.Clear();
                    ddlTemplateType.ClearDropDownList();
                    ddlModeofMail.ClearDropDownList();
                    chkIsActive.Enabled = false;
                    //FTBTemplate.ReadOnly = true;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    if (GrvExclusion != null)
                    {
                        GrvExclusion.Columns[GrvExclusion.Columns.Count - 1].Visible = false;
                        if (GrvExclusion.FooterRow != null)
                        {
                            GrvExclusion.FooterRow.Visible = false;
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriBindEmpty()
    {
        try
        {
            DataRow drEmptyRow;
            dtExclusionDetails = new DataTable();
            dtExclusionDetails.Columns.Add("CategoryId");
            dtExclusionDetails.Columns.Add("Category");
            dtExclusionDetails.Columns.Add("Id");
            dtExclusionDetails.Columns.Add("Code");
            dtExclusionDetails.Columns.Add("Description");
            drEmptyRow = dtExclusionDetails.NewRow();
            dtExclusionDetails.Rows.Add(drEmptyRow);
            ViewState["dtExclusionDetails"] = dtExclusionDetails;
            FunFillgrid(GrvExclusion, dtExclusionDetails);
            GrvExclusion.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This method is used to load/Bind the grid for the given datatable.
    /// </summary>
    /// <param name="grv"></param>
    /// <param name="dtEntityBankdetails"></param>
    private void FunFillgrid(GridView grv, DataTable dtbl)
    {
        try
        {
            grv.DataSource = dtbl;
            grv.DataBind();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriSetModeofMail()
    {
        try
        {
            RFVtxtDocumentPath.Enabled = false;
            if (ddlModeofMail.SelectedValue == "1")
            {
                RFVtxtDocumentPath.Enabled = true;
                FunPriGetDocPath();
            }
            else
            {
                txtDocumentPath.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriGetDocPath()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@Program_Id", "215");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Is_Active", "1");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetDocPathforLOB", Procparam);
            txtDocumentPath.Text = string.Empty;
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["Path"].ToString()))
                    txtDocumentPath.Text = dt.Rows[0]["Path"].ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadTemplateModification(string strTemplateNo)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@Tmp_Doc_Id", strTemplateNo);
            DataSet ds = Utility.GetDataset("S3G_SYSAD_GET_TMPL_DTLS", Procparam);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (!string.IsNullOrEmpty(dt.Rows[0]["LOB_Id"].ToString()))
                    ddlLOB.SelectedValue = dt.Rows[0]["LOB_Id"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Location_Id"].ToString()))
                    ddlLocation.SelectedValue = dt.Rows[0]["Location_Id"].ToString();
                ddlLocation.SelectedText = dt.Rows[0]["Location_Name"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Tmp_Doc_No"].ToString()))
                    txtTemplateRefNo.Text = dt.Rows[0]["Tmp_Doc_No"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Template_Type_Code"].ToString()))
                    ddlTemplateType.SelectedValue = dt.Rows[0]["Template_Type_Code"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Mode_of_Mail_Type_Code"].ToString()))
                    ddlModeofMail.SelectedValue = dt.Rows[0]["Mode_of_Mail_Type_Code"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Doc_Path"].ToString()))
                    txtDocumentPath.Text = dt.Rows[0]["Doc_Path"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Is_Active"].ToString()))
                    chkIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Active"].ToString());
                if (!string.IsNullOrEmpty(dt.Rows[0]["Template_Content"].ToString()))
                {
                    FTBTemplate.Text = dt.Rows[0]["Template_Content"].ToString();
                    ViewState["TemplateText"] = FTBTemplate.Text;
                    ViewState["TranslatedText"] = dt.Rows[0]["Translated_Content"].ToString();
                }
                txtDescription.Text = dt.Rows[0]["Template_Desc"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Template_language"].ToString()))
                    ddlLanguage.SelectedValue = dt.Rows[0]["Template_language"].ToString();
                FunPriBindVariables();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                FunFillgrid(GrvExclusion, ds.Tables[1]);
                ViewState["dtExclusionDetails"] = ds.Tables[1];
            }
            else
            {
                FunPriBindEmpty();
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriClear()
    {
        try
        {
            ddlTemplateType.SelectedIndex = ddlModeofMail.SelectedIndex = -1;
            txtDocumentPath.Text = string.Empty;
            FTBTemplate.Text = string.Empty;
            ddlLocation.Clear();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriSave()
    {
        try
        {
            ObjDocServicesClient = new DocMgtServicesReference.DocMgtServicesClient();

            ObjS3G_SYSAD_TemplateDtlsDataTable = new DocMgtServices.S3G_SYSAD_TemplateDtlsDataTable();
            DocMgtServices.S3G_SYSAD_TemplateDtlsRow ObjTemplateDtlsRow;
            ObjTemplateDtlsRow = ObjS3G_SYSAD_TemplateDtlsDataTable.NewS3G_SYSAD_TemplateDtlsRow();

            ObjTemplateDtlsRow.Company_ID = intCompanyID;
            if (strMode == "M")
            {
                ObjTemplateDtlsRow.Tmp_Doc_Id = Convert.ToInt32(strTemplateNo);
                ObjTemplateDtlsRow.Tmp_Doc_No = txtTemplateRefNo.Text;
            }
            else
            {
                ObjTemplateDtlsRow.Tmp_Doc_Id = -1;
                ObjTemplateDtlsRow.Tmp_Doc_No = "";
            }
            ObjTemplateDtlsRow.LOB_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjTemplateDtlsRow.Location_Id = Convert.ToInt32(ddlLocation.SelectedValue);
            ObjTemplateDtlsRow.Template_Type_Code = Convert.ToInt32(ddlTemplateType.SelectedValue);
            ObjTemplateDtlsRow.Mode_of_Mail_Type_Code = Convert.ToInt32(ddlModeofMail.SelectedValue);
            ObjTemplateDtlsRow.Doc_Path = txtDocumentPath.Text;
            ObjTemplateDtlsRow.Template_language = ddlLanguage.SelectedValue;
            // ObjTemplateDtlsRow.Template_Content = ddlLanguage.SelectedValue == "en" ?Convert.ToString(FTBTemplate.Text) : (string)ViewState["TemplateText"];
            //ObjTemplateDtlsRow.Translated_Content = (string)ViewState["TranslatedText"] != null ? (string)ViewState["TranslatedText"] : " ";
            ObjTemplateDtlsRow.Template_Content = Convert.ToString(FTBTemplate.Text);
            ObjTemplateDtlsRow.Translated_Content = " ";
            ObjTemplateDtlsRow.Template_Desc = txtDescription.Text;
            ObjTemplateDtlsRow.Is_Active = chkIsActive.Checked;
            ObjTemplateDtlsRow.Created_By = intUserID;
            if (ViewState["dtExclusionDetails"] != null)
            {
                dtExclusionDetails = (DataTable)ViewState["dtExclusionDetails"];
            }
            if (dtExclusionDetails.Rows.Count > 0)
            {
                ObjTemplateDtlsRow.XMLTemplate_Excl = dtExclusionDetails.FunPubFormXml();
            }
            else
                ObjTemplateDtlsRow.XMLTemplate_Excl = "<Root></Root>";
            ObjS3G_SYSAD_TemplateDtlsDataTable.AddS3G_SYSAD_TemplateDtlsRow(ObjTemplateDtlsRow);
            intErrorCode = ObjDocServicesClient.FunPubCreateTemplate(out strTempRefNo, SerMode, ClsPubSerialize.Serialize(ObjS3G_SYSAD_TemplateDtlsDataTable, SerMode));

            if (intErrorCode == 0 && strTemplateNo == string.Empty)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Delinquency parameters" +  DSNO  + " defined sucessfully');" + strRedirectPageAdd, true);
                strAlert = "Template \"" + strTempRefNo + "\" added successfully";
                strAlert += @"\n\nWould you like to add one more Template?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "} else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);

            }
            else if (intErrorCode == 0 && strTemplateNo != string.Empty)
            {
                //TMPL_2
                Utility.FunShowValidationMsg(this.Page, strvalidationmsgname, 2, strRedirectPage);
                return;

            }
            else if (intErrorCode < 0 || intErrorCode == 50)
            {
                Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                return;
            }
            else
            {
                Utility.FunShowValidationMsg(this.Page, strvalidationmsgname, intErrorCode);
                return;
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "215");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    #endregion

    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FTBTemplate.Text=Utility.Translate_Language(FTBTemplate.Text, ddlLanguage.SelectedValue);
        //if (ddlLanguage.SelectedValue != "en")
        //{
        //    ViewState["TemplateText"] = FTBTemplate.Text;
        //}
        #region "Multi Lingual Implementation"
        //Included by Sathish.G to Change the Web Page Language 
        Language Web_Language = new Language();
        if (ddlLanguage.SelectedValue != "en")
        {
            ViewState["TemplateText"] = FTBTemplate.Text;
            string text = FTBTemplate.Text;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyID.ToString());
            Procparam.Add("@Tmp_Doc_Id", strTemplateNo);
            DataTable dt = new DataTable();
            if (strTemplateNo != "")
            {
                dt = Utility.GetDefaultData("S3G_SYSAD_GET_TRANSLATED_TMPL_DTLS", Procparam);
            }
            else
            {
                DataColumn column = new DataColumn();
                dt.Columns.Add(column);
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "Translated_Content";
                DataRow dr = dt.NewRow();
                dr[0] = "Translated_Content";
                dt.Rows.Add(dr);

                dt.Rows[0]["Translated_Content"] = "";
            }
            if (Convert.ToString(dt.Rows[0]["Translated_Content"]) == "")
            {
                Dictionary<string, string> Words = new Dictionary<string, string>();
                string s = FTBTemplate.Text;
                Regex r = new Regex(@">(.*?)<");
                s = Regex.Replace(s, "~.*?~", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                MatchCollection mc = r.Matches(s);
                for (int i = 0; i < mc.Count; i++)
                {
                    if (!mc[i].Groups[1].Value.Contains("&") && mc[i].Groups[1].Value != "" && mc[i].Groups[1].Value != " ")
                    {
                        if (!Words.ContainsKey(mc[i].Groups[1].Value))
                        {
                            Words.Add(mc[i].Groups[1].Value, "sathish");
                            text = text.Replace(mc[i].Groups[1].Value.Trim(), LanguageTranslator.LanguageTranslator.Translate(ddlLanguage.SelectedValue, mc[i].Groups[1].Value));
                        }
                    }
                }
            }
            else
            {
                text = dt.Rows[0]["Translated_Content"].ToString();
            }

            FTBTemplate.Text = text;
            ViewState["TranslatedText"] = FTBTemplate.Text;

        }
        else
        {
            //FunPriLoadTemplateModification(strTemplateNo);
            ViewState["TranslatedText"] = FTBTemplate.Text;
            FTBTemplate.Text = (string)ViewState["TemplateText"];
        }

        //Language Change Code Ends
        #endregion
    }

    protected void lnkViewTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriGenerateHTML(FTBTemplate.Text, "Viewtemplate", "1", ddlLanguage.SelectedValue);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
            //throw ex;
            strAlert = "No Internet Connectivity.";
            strAlert += @"\nWould you like to Open Network Settings?";
            strAlert = "if(confirm('" + strAlert + "')){ PageMethods.OkClick() }else {}";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
        }
    }

    [System.Web.Services.WebMethod]
    public static void OkClick()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("ncpa.cpl");
        startInfo.UseShellExecute = true;
        Process.Start(startInfo);
    }
   
    private void FunPriGenerateHTML(string strNewHTML, string FileName, string strNeedShow, string Template_Language)
    {

        String htmlText = strNewHTML;
        if (Template_Language != "en")
        {
            htmlText = Utility.Translate_Language(htmlText, Template_Language);
        }

        string str = "/SISLS3GPLayer";
        string newstr = "..";
        htmlText = htmlText.Replace(str, newstr);
        Session["HTMLValue"] = htmlText;
        string strnewFile = (Server.MapPath(".") + "\\PDF Files\\" + FileName + ".htm");
        string strFileName = "/System Admin/PDF Files/" + FileName + ".htm";
        if (strNeedShow == "1")
        {
            string strScipt = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=1050,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (FUpload.HasFile)
        {

            string FileName = System.IO.Path.GetFileName(FUpload.PostedFile.FileName);

            string FilePath = "images/" + FileName;

            FUpload.SaveAs(Server.MapPath(FilePath));

            FTBTemplate.Text += string.Format("<img src = '{0}' alt = '{1}' />", FilePath, FileName);

        }

    }
}