/// Module Name         :   Utility
/// Screen Name         :   Utility.cs
/// Created By          :   Kaliraj K
/// Created Date        :   13-May-2010
/// Purpose             :   To include common methods
/// Last Updated By		:   Suresh P
/// Last Updated Date   :   17-May-2010
/// Reason              :   To include Extension method of fille dropdownlist
using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using S3GBusEntity;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.Collections;
using System.Linq;

//This NameSpace for PDF Format
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.IO;
using S3GAdminServicesReference;
using ClnDebtMgtServicesReference;
using WorkflowMgtServiceReference;
using System.Resources;
using System.Reflection;
using System.Configuration;
using System.Net;
using Languages;
using System.Text.RegularExpressions;

public static partial class Utility
{
    #region Method for PrefixLength
    public static int MinimumPrefixLength(string strPrefixLength)
    {
        int intMinimumPrefixLength = int.Parse(ConfigurationManager.AppSettings[strPrefixLength].ToString());
        return intMinimumPrefixLength;
    }
    #endregion

    #region Alerts Message

    /// <summary>
    /// This function is used to show alert message and page will not redirct after clicking ok button
    /// </summary>
    /// 
    public static void FunShowAlertMsg(Page thisPage, string strAlertMsg)
    {
        StringBuilder strMsg = new StringBuilder();
        strMsg.Append("alert('" + strAlertMsg + "');");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    public static void FunShowAlertMsg(Control thisControl, string strAlertMsg)
    {
        StringBuilder strMsg = new StringBuilder();
        strMsg.Append("alert('" + strAlertMsg + "');");
        ScriptManager.RegisterStartupScript(thisControl, thisControl.GetType(), "Display", strMsg.ToString(), true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisPage"></param>
    /// <param name="strDocType"></param>
    /// <param name="intErrCode"></param>
    public static void FunShowValidationMsg(Page thisPage, string strDocType, int intErrCode)
    {
        StringBuilder strMsg = new StringBuilder();
        string strErrCode = string.Empty;
        if (strDocType == "")
            strErrCode = intErrCode.ToString();
        else
            strErrCode = strDocType + "_" + intErrCode.ToString();
        string strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(strErrCode);
        strMsg.Append("alert('" + strAlertMsg + "');");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisPage"></param>
    /// <param name="strDocType"></param>
    /// <param name="intErrCode"></param>
    /// <param name="strRedirectPage"></param>
    public static void FunShowValidationMsg(Page thisPage, string strDocType, int intErrCode, string strRedirectPage)
    {
        StringBuilder strMsg = new StringBuilder();
        string strErrCode = string.Empty;
        if (strDocType == "")
            strErrCode = intErrCode.ToString();
        else
            strErrCode = strDocType + "_" + intErrCode.ToString();
        string strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(strErrCode);
        strMsg.Append("alert('" + strAlertMsg + "');");
        strMsg.Append("window.location.href='" + strRedirectPage + "';");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    /// <summary>
    /// This function is used to show alert message and page will be redircted after clicking ok button
    /// </summary>
    ///
    public static void FunShowAlertMsg(System.Web.UI.Page thisPage, string strAlertMsg, string strRedirectPage)
    {
        StringBuilder strMsg = new StringBuilder();
        strMsg.Append("alert('" + strAlertMsg + "');");
        strMsg.Append("window.location.href='" + strRedirectPage + "';");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisPage"></param>
    /// <param name="strDocType"></param>
    /// <param name="intErrCode"></param>
    /// <param name="strAlertMsg"></param>
    /// <param name="blnRedirectPage"></param>
    public static void FunShowValidationMsg(Page thisPage, string strDocType, int intErrCode, string strAlertMsg, bool blnRedirectPage)
    {
        StringBuilder strMsg = new StringBuilder();
        string strErrCode = strDocType + "_" + intErrCode.ToString();
        if ((strAlertMsg == "") || (strAlertMsg.Trim() == string.Empty))
        {
            strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(strErrCode);
        }
        if ((intErrCode == 120) || (strDocType == ""))
        {
            strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(intErrCode.ToString());
        }
        strMsg.Append("alert('" + strAlertMsg + "');");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisPage"></param>
    /// <param name="strDocType"></param>
    /// <param name="intErrCode"></param>
    /// <param name="strAlertMsg"></param>
    /// <param name="strRedirectPage"></param>
    public static void FunShowValidationMsg(Page thisPage, string strDocType, int intErrCode, string strAlertMsg, string strRedirectPage)
    {
        StringBuilder strMsg = new StringBuilder();
        string strErrCode = strDocType + "_" + intErrCode.ToString();
        if ((strAlertMsg == "") || (strAlertMsg.Trim() == string.Empty))
        {
            strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(strErrCode);
        }
        if ((intErrCode == 120) || (strDocType == ""))
        {
            strAlertMsg = Resources.ValidationMsgs.ResourceManager.GetString(intErrCode.ToString());
        }
        strMsg.Append("alert('" + strAlertMsg + "');");
        strMsg.Append("window.location.href='" + strRedirectPage + "';");
        ScriptManager.RegisterStartupScript(thisPage, thisPage.GetType(), "Display", strMsg.ToString(), true);
    }

    #endregion

    #region To Fill Gridview
    public static void BindGridView(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam)
    {

        try
        {
            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count > 0)
                {
                    grvSource.DataSource = ObjDataView;
                    grvSource.DataBind();
                }
                else
                {
                    grvSource.DataSource = null;
                    grvSource.DataBind();

                }
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void BindGridView_Report(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam)
    {

        try
        {
            DataTable ObjDataTable = GetDefaultData_Report(strProcName, dictProcParam);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count > 0)
                {
                    grvSource.DataSource = ObjDataView;
                    grvSource.DataBind();
                }
                else
                {
                    grvSource.DataSource = null;
                    grvSource.DataBind();

                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static DataTable BindGridViewR(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam)
    {

        try
        {
            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count > 0)
                {
                    grvSource.DataSource = ObjDataView;
                    grvSource.DataBind();
                }
                else
                {
                    grvSource.DataSource = null;
                    grvSource.DataBind();

                }
            }
            return ObjDataTable;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    #endregion

    #region WorkFlow Methods
    public static int FunPubAssignWorkflowStatus(int intCompanyId, int intBranchId, int intUserId, string strTaskDocumentNo, int intLobId, int intProductId, int intProgramRefNo)
    {
        WorkFlowStatus objWorkflowStatus = new WorkFlowStatus();
        objWorkflowStatus.intCompanyId = intCompanyId;
        objWorkflowStatus.intBranchId = intBranchId;
        objWorkflowStatus.intUserId = intUserId;
        objWorkflowStatus.intLOBId = intLobId;
        objWorkflowStatus.intProductId = intProductId;
        objWorkflowStatus.strTaskDocumentNo = strTaskDocumentNo;
        objWorkflowStatus.intProgramRefNo = intProgramRefNo;
        if (System.Web.HttpContext.Current.Session["WorkflowSequenceId"] != null)
        {
            objWorkflowStatus.strWorkSequenceId = Convert.ToString(System.Web.HttpContext.Current.Session["WorkflowSequenceId"]);
        }
        WorkflowMgtServiceReference.WorkflowMgtServiceClient objWorkflowServiceClient = new WorkflowMgtServiceReference.WorkflowMgtServiceClient();
        return objWorkflowServiceClient.FunPubInsertWorkflowStatus(objWorkflowStatus);
    }

    /// <summary>
    /// This is to return the program master details - which can be reterived using the program code  
    /// </summary>
    /// <param name="strProgramCode"></param>
    /// <returns></returns>
    public static DataTable FunGetProgramDetailsByProgramCode(string strProgramCode)
    {
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Program_Code", strProgramCode);
        return GetDefaultData(SPNames.S3G_LOANAD_GetProgram_Ref_No, dictProcParam);
    }
    #endregion

    #region Sys Journal Methods
    // Created by Kali on 28-Aug-2010
    // System journal entry for all the transactions

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ObjSysJournal"></param>
    /// <returns></returns>
    public static string FunPubSysJournalXMLGenerate(ClsSystemJournal ObjSysJournal)
    {
        StringBuilder strbSysJournal = new StringBuilder();
        strbSysJournal.Append("<Details ");
        strbSysJournal.Append("Occur_No='" + ObjSysJournal.Occurrence_No + "' ");
        strbSysJournal.Append("Txn_Amt='" + ObjSysJournal.Txn_Amount + "' ");
        strbSysJournal.Append("Acc_Flag='" + ObjSysJournal.Accounting_Flag + "' ");
        strbSysJournal.Append("Dim2_Code='" + ObjSysJournal.Global_Dimension2_Code + "' ");
        strbSysJournal.Append("Dim2_No='" + ObjSysJournal.Global_Dimension2_No + "' ");
        strbSysJournal.Append(" /> ");
        return strbSysJournal.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ObjSysJournal"></param>
    public static void FunPubSysJournalEntry(ClsSystemJournal ObjSysJournal)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            ObjAdminService.FunPubSysJournalEntry(ObjSysJournal);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }

    }

    //Journal entry code end

    //Added by Kali on 09-Sep-2010 to identify row value of grid

    #endregion

    #region Other General Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strGridName"></param>
    /// <param name="strClientID"></param>
    /// <returns></returns>
    public static int FunPubGetGridRowID(string strGridName, string strClientID)
    {
        string strVal = strClientID.Substring(strClientID.LastIndexOf(strGridName + "_")).Replace(strGridName + "_ctl", "");
        int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        gRowIndex = gRowIndex - 2;
        return gRowIndex;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strSplitValue"></param>
    /// <returns></returns>
    public static string FunGetValueBySplit(string strSplitValue)
    {
        return (strSplitValue.Substring(0, strSplitValue.LastIndexOf("-")).Trim().ToUpper());
    }

    /// <summary>
    /// Method to convert a string to date based on Global parmeter datformat
    /// </summary>
    /// <param name="strInput"></param>
    /// <returns></returns>
    public static DateTime StringToDate(string strInput)
    {
        DateTime dtValidDatetime;
        //if (strInput != string.Empty || strInput != "")
        //{
        if ((strInput.Contains(":")) && (IsDateTime(strInput)))
        {
            dtValidDatetime = Convert.ToDateTime(strInput);
        }
        else
        {
            S3GSession ObjS3GSession = new S3GSession();
            DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
            if (ObjS3GSession.ProDateFormatRW != null || ObjS3GSession.ProDateFormatRW != string.Empty)
            {
                dtformat.ShortDatePattern = ObjS3GSession.ProDateFormatRW;
            }
            else
            {
                dtformat.ShortDatePattern = "MM/dd/yy";
            }
            DateTime dt = DateTime.Parse(strInput, dtformat);
            dtValidDatetime = Convert.ToDateTime(dt.ToString("yyyy/MM/dd"));

        }
        return dtValidDatetime;
        //}
        //else
        //{
        //    return DateTime.Now;
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtCtrl"></param>
    public static void Clear(this TextBox txtCtrl)
    {
        txtCtrl.Text = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intCompanyid"></param>
    /// <param name="intLOBId"></param>
    /// <returns></returns>
    public static DataTable FunPubGetGlobalIRRDetails(int intCompanyid, int? intLOBId)
    {
        Dictionary<string, string> dictProcParam = new Dictionary<string, string>();
        dictProcParam.Add("@Company_ID", intCompanyid.ToString());
        if (intLOBId.HasValue)
            dictProcParam.Add("@Lob_ID", intLOBId.ToString());

        DataTable dtGlobalIRRDtls = GetDefaultData(SPNames.S3G_ORG_GetGlobalIRRDetails, dictProcParam);

        return dtGlobalIRRDtls;
    }

    /// <summary>
    /// Method will calculate Sum of amount field by grouping any description field both sum field and 
    /// group by filed are got as Inputs
    /// </summary>
    /// <param name="objDataTable">base Datatable from which calculatio to be carried</param>
    /// <param name="strGroupByField">Filed on which group by to be carried out </param>
    /// <param name="strSumField">The field for which sum should be obtained</param>
    /// <returns></returns>
    public static DataTable FunPriCalculateSumAmount(DataTable objDataTable, string strGroupByField, string strSumField)
    {
        DataTable objDataTableCopy = objDataTable.Copy();

        //DataRow[] drobjDataTableCopy = objDataTableCopy.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 35 and CashFlow_Flag_ID <> 91");
        //foreach (DataRow dr in drobjDataTableCopy)
        //    dr.Delete();
        //objDataTableCopy.AcceptChanges();

       DataTable objReturnDataTable = new DataTable();
        objReturnDataTable.Columns.Add(strGroupByField);
        objReturnDataTable.Columns.Add(strSumField);
        var varQuery = from row in objDataTableCopy.AsEnumerable()
                       group row by row.Field<string>(strGroupByField) into grp
                       orderby grp.Key
                       select new { Id = grp.Key, Sum = grp.Sum(r => r.Field<Decimal>(strSumField)) };

        foreach (var varCollection in varQuery)
        {
            DataRow objDataRow = objReturnDataTable.NewRow();
            objDataRow[strGroupByField] = varCollection.Id;
            objDataRow[strSumField] = varCollection.Sum;
            objReturnDataTable.Rows.Add(objDataRow);
        }
        return objReturnDataTable;
    }

 
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strPath"></param>
    /// <param name="strPdfContent"></param>
    public static void FunPubGeneratePDF(string strPath, string strPdfContent, string strDocTitle)
    {
        Document doc = new Document();
        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(strPath, FileMode.Create));
        doc.AddCreator("Sundaram Infotech Solutions");
        doc.AddTitle(strDocTitle);
        doc.Open();
        List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(strPdfContent), null).Cast<IElement>().ToList();
        for (int k = 0; k < htmlarraylist.Count; k++)
        { doc.Add((IElement)htmlarraylist[k]); }
        doc.AddAuthor("SISL-S3G");
        doc.Close();
        System.Diagnostics.Process.Start(strPath);
    }

    /// <summary>
    /// Will convert the source datatable table rows to columns
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="strMainHeader"></param>
    /// <param name="strColName"></param>
    /// <returns></returns>
    public static DataTable FunConvertRowToColumn(DataTable dt, string strMainHeader, string[] strColName)
    {
        if (dt != null && dt.Rows.Count > 0 && strColName != null && strColName.Length > 0)
        {
            DataTable dttemp = new DataTable();

            // to add the headings column
            DataColumn dcType = new DataColumn(strMainHeader);
            dttemp.Columns.Add(dcType);
            for (int i_col = 0; i_col < dt.Columns.Count; i_col++)
            {
                DataRow dr = dttemp.NewRow();
                dr[strMainHeader] = dt.Columns[i_col].Caption;
                dttemp.Rows.Add(dr);
            }

            // to add the columns
            for (int i_col = 0; i_col < strColName.Length; i_col++)
            {
                DataColumn dc = new DataColumn(strColName[i_col]);
                dc.DefaultValue = "";
                dttemp.Columns.Add(dc);
            }

            // cell values
            for (int i_row = 0; i_row < dt.Rows.Count; i_row++)  // source table
            {
                for (int i_col = 0; i_col < dt.Columns.Count; i_col++) // source table
                {
                    dttemp.Rows[i_col][i_row + 1] = dt.Rows[i_row][i_col]; // destn table
                }
            }
            return dttemp;
        }
        return dt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="strMainHeader"></param>
    /// <param name="Columns"></param>
    /// <returns></returns>
    public static DataTable FunConvertRowToColumn(DataTable dt, string strMainHeader, Dictionary<string, string> Columns)
    {
        if (dt != null && dt.Rows.Count > 0 && Columns != null && Columns.Count > 0)
        {
            DataTable dttemp = new DataTable();

            // to add the headings column
            DataColumn dcType = new DataColumn(strMainHeader);
            dttemp.Columns.Add(dcType);
            for (int i_col = 0; i_col < dt.Columns.Count; i_col++)
            {
                DataRow dr = dttemp.NewRow();
                dr[strMainHeader] = dt.Columns[i_col].Caption;
                dttemp.Rows.Add(dr);
            }
            foreach (KeyValuePair<string, string> Pair in Columns)
            {
                DataColumn dc = new DataColumn(Pair.Key, Type.GetType(Pair.Value));
                dc.DefaultValue = DBNull.Value;
                dttemp.Columns.Add(dc);
            }

            //// to add the columns
            //for (int i_col = 0; i_col < Columns.Count; i_col++)
            //{

            //}

            // cell values
            for (int i_row = 0; i_row < dt.Rows.Count; i_row++)  // source table
            {
                for (int i_col = 0; i_col < dt.Columns.Count; i_col++) // source table
                {
                    dttemp.Rows[i_col][i_row + 1] = dt.Rows[i_row][i_col]; // destn table
                }
            }
            return dttemp;
        }
        return dt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ddlSource"></param>
    public static void RemoveDropDownList(this DropDownList ddlSource)
    {
        try
        {
            System.Web.UI.WebControls.ListItem lstItem;

            if (ddlSource.Items.Count > 0)
            {
                lstItem = new System.Web.UI.WebControls.ListItem(ddlSource.SelectedItem.Text, ddlSource.SelectedItem.Value);
                ddlSource.Items.Clear();
                ddlSource.Items.Add(lstItem);
            }
            else
            {
                ddlSource.Items.Clear();
                ddlSource.Items.Add("--Select--");
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ddlSource"></param>
    public static void ClearDropDownList(this DropDownList ddlSource)
    {

        try
        {
            System.Web.UI.WebControls.ListItem lstItem = new System.Web.UI.WebControls.ListItem(ddlSource.SelectedItem.Text, ddlSource.SelectedItem.Value); ;
            ddlSource.Items.Clear();
            ddlSource.Items.Add(lstItem);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmbSource"></param>
    public static void ClearDropDownList(this ComboBox cmbSource)
    {

        try
        {
            System.Web.UI.WebControls.ListItem lstItem = new System.Web.UI.WebControls.ListItem(cmbSource.SelectedItem.Text, cmbSource.SelectedItem.Value); ;
            cmbSource.Items.Clear();
            cmbSource.Items.Add(lstItem);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string GetAmountInWords(this decimal dcmlVal)
    {
        try
        {
            string[] strarr = dcmlVal.ToString().Split('.');
            string strAmt = "";
            if (strarr[0] != null)
            {
                strAmt += FunGetAmountInWords(Convert.ToInt32(strarr[0]));
            }
            //if (strarr.Length > 1)
            //{
            //    if (strarr[1] != null && Convert.ToInt32(strarr[1]) > 0)
            //    {
            //        strAmt += " and " + FunGetAmountInWords(Convert.ToInt32(strarr[1]));
            //    }
            //}
            return strAmt + " only";
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunGetAmountInWords(int number)
    {
        try
        {
            if (number == 0) return "Zero";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units  
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands  
            num[3] = number / 10000000; // crores  
            num[2] = num[2] - 100 * num[3]; // lakhs  
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones  
                t = num[i] / 10;
                h = num[i] / 100; // hundreds  
                t = t - 10 * h; // tens  
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    //if (h > 0 || i == 0) sb.Append("and "); 
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string FunGetApprovalMessage(string strSceenName, DropDownList ddlStatus)
    {
        string strMessage = "";
        if (ddlStatus.SelectedValue == "3")
            strMessage = strSceenName + " approved successfully.";
        else if (ddlStatus.SelectedValue == "4")
            strMessage = strSceenName + " rejected successfully.";
        else
            strMessage = strSceenName + " approval details added successfully.";

        return strMessage;
    }

    ///// <summary>
    ///// To get the Value of the Dropdown list by passing its item name
    ///// </summary>
    ///// <param name="ddl"></param>
    ///// <param name="strItem"></param>
    ///// <returns></returns>
    //public static string FunGetDdlValueByItem(this DropDownList ddl, string strItem)
    //{
    //    if (ddl != null && ddl.Items.Count > 0)
    //    {
    //        for (int i = 0; i < ddl.Items.Count; i++)
    //        {
    //            //if ((string.Compare( strItem, ddl.Items[i].Text)) == 0)
    //            if (ddl.Items[i].Text.Contains(strItem))
    //                return ddl.Items[i].Value;
    //        }
    //        //return ddl.Items.FindByText(strItem).Value;
    //    }
    //    return "1";
    //}


    #endregion

    #region Methods for Validations
    /// <summary>
    /// This Extension Method is used check for a valid password 
    /// </summary>
    /// <param name="strPassword"></param>
    /// <returns></returns>
    public static bool IsValidPassword(this string strPassword)
    {
        if (strPassword.Length < 6 || strPassword.Length > 6)
        {
            return false;
        }

        char charFirst = Convert.ToChar(strPassword.Substring(0, 1).ToLower());
        if (!char.IsLetterOrDigit(charFirst))
            return false;

        string strSp = @"~!@#$%^&*()_+|`-=\/?,.`[]{}:;'";
        string strNo = "0123456789";

        char[] chArray = strPassword.ToCharArray();

        bool blnSpResult = false;
        bool blnNoResult = false;
        bool blnUpResult = false;
        bool blnLoResult = false;

        foreach (char ch in chArray)
        {
            blnSpResult = (blnSpResult == false) ? strSp.IndexOf(ch) > 0 : blnSpResult;
            blnNoResult = (blnNoResult == false) ? strNo.IndexOf(ch) > 0 : blnNoResult;
            blnLoResult = (blnLoResult == false) ? char.IsLower(ch) : blnLoResult;
            blnUpResult = (blnUpResult == false) ? char.IsUpper(ch) : blnUpResult;
        }
        string strResult = blnSpResult.ToString() + blnNoResult.ToString() + blnLoResult.ToString() + blnUpResult.ToString();
        return strResult.Replace("False", "").Replace("True", "T").ToString().Length >= 3;
    }

    #region Methods to Fill Dropdown Combo
    /// <summary>
    /// This Extension method used to Fill DropdownList
    /// </summary>
    /// <param name="ddlSourceControl">Source DropdownList Control</param>
    /// <param name="ObjDataTable">DataTable Name</param>
    /// <param name="strDataValueColumn">DataValueColumn</param>
    /// <param name="strDataTextColumn">DataTextColumn</param>
    public static void FillDataTable(this DropDownList ddlSourceControl, DataTable ObjDataTable, string strDataValueColumn, string strDataTextColumn)
    {
        ddlSourceControl.Items.Clear();
        if (ObjDataTable != null)
        {
            if (ObjDataTable.Rows.Count > 0)
            {
                ddlSourceControl.DataValueField = strDataValueColumn;
                ddlSourceControl.DataTextField = strDataTextColumn;
                ddlSourceControl.DataSource = ObjDataTable;
                ddlSourceControl.DataBind();
            }
        }
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
        ddlSourceControl.Items.Insert(0, liSelect);
    }

    //code added by saran on 26-aug-2014 start
    /// <summary>
    /// This Extension method used to Fill DropdownList
    /// </summary>
    /// <param name="ddlSourceControl">Source DropdownList Control</param>
    /// <param name="ObjDataTable">DataTable Name</param>
    /// <param name="strDataValueColumn">DataValueColumn</param>
    /// <param name="strDataTextColumn">DataTextColumn</param>
    public static void FillDataTable(this DropDownList ddlSourceControl, DataTable ObjDataTable, string strDataValueColumn, string strDataTextColumn,bool IsSelectReq)
    {
        ddlSourceControl.Items.Clear();
        if (ObjDataTable != null)
        {
            if (ObjDataTable.Rows.Count > 0)
            {
                ddlSourceControl.DataValueField = strDataValueColumn;
                ddlSourceControl.DataTextField = strDataTextColumn;
                ddlSourceControl.DataSource = ObjDataTable;
                ddlSourceControl.DataBind();
            }
        }
        if (IsSelectReq)
        {
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
    }

    //code added by saran on 26-aug-2014 end
    /// <summary>
    /// This Extension method used to Fill Ajax DropdownList
    /// </summary>
    /// <param name="ddlSourceControl">Source DropdownList Control</param>
    /// <param name="ObjDataTable">DataTable Name</param>
    /// <param name="strDataValueColumn">DataValueColumn</param>
    /// <param name="strDataTextColumn">DataTextColumn</param>
    public static void FillDataTable(this AjaxControlToolkit.ComboBox ddlSourceControl, DataTable ObjDataTable, string strDataValueColumn, string strDataTextColumn)
    {
        ddlSourceControl.Items.Clear();
        if (ObjDataTable != null)
        {
            if (ObjDataTable.Rows.Count > 0)
            {
                ddlSourceControl.DataValueField = strDataValueColumn;
                ddlSourceControl.DataTextField = strDataTextColumn;
                ddlSourceControl.DataSource = ObjDataTable;
                ddlSourceControl.DataBind();
            }
        }
        System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
        ddlSourceControl.Items.Insert(0, liSelect);
    }

    public static void FillDataTable(this AjaxControlToolkit.ComboBox ddlSourceControl, DataTable ObjDataTable, string strDataValueColumn, string strDataTextColumn, bool NeedSelect)
    {
        ddlSourceControl.Items.Clear();
        if (ObjDataTable != null)
        {
            if (ObjDataTable.Rows.Count > 0)
            {
                ddlSourceControl.DataValueField = strDataValueColumn;
                ddlSourceControl.DataTextField = strDataTextColumn;
                ddlSourceControl.DataSource = ObjDataTable;
                ddlSourceControl.DataBind();
            }
        }
        System.Web.UI.WebControls.ListItem liSelect;
        if (NeedSelect)
        {
            liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
        }
        else
        {
            liSelect = new System.Web.UI.WebControls.ListItem(" ", "0");
        }
        ddlSourceControl.Items.Insert(0, liSelect);
    }


    /// <summary>
    /// This method for filling dropdown By Narayanan
    /// </summary>
    /// <param name="ObjDDL">Pass dropdown object</param>
    /// <param name="ObjDT">Pass Datable</param>
    public static void FillDLL(this DropDownList ObjDDL, DataTable ObjDT)
    {
        try
        {
            ObjDDL.Items.Clear();

            //if (ObjDDL.Items.Count > 0)
            //{
            //    ObjDDL.DataSource = null;
            //    ObjDDL.DataBind();
            //}
            if (ObjDT.Rows.Count > 0 && ObjDT.Columns.Count > 1)
            {
                ObjDDL.DataTextField = ObjDT.Columns[1].ColumnName;
                ObjDDL.DataValueField = ObjDT.Columns[0].ColumnName;
                ObjDDL.DataSource = ObjDT;
                ObjDDL.DataBind();
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ObjDDL.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FillDLL(this DropDownList ObjDDL, DataTable ObjDT, bool IsNeedSelect)
    {
        try
        {
            ObjDDL.Items.Clear();
            //ObjDDL.DataSource = null;
            //ObjDDL.DataBind();

            if (ObjDT.Rows.Count > 0 && ObjDT.Columns.Count > 1)
            {
                ObjDDL.DataSource = ObjDT;
                ObjDDL.DataTextField = ObjDT.Columns[1].ColumnName;
                ObjDDL.DataValueField = ObjDT.Columns[0].ColumnName;
                ObjDDL.DataBind();
            }
            if (IsNeedSelect)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ObjDDL.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void FillDLL(AjaxControlToolkit.ComboBox ObjDDL, bool IsNeedSelect, DataTable ObjDT)
    {
        try
        {
            //ObjDDL.Items.Clear();
            ObjDDL.DataSource = null;
            ObjDDL.DataBind();
            if (ObjDT.Rows.Count > 0 && ObjDT.Columns.Count > 1)
            {
                ObjDDL.DataSource = ObjDT;
                ObjDDL.DataTextField = ObjDT.Columns[1].ColumnName;
                ObjDDL.DataValueField = ObjDT.Columns[0].ColumnName;
                ObjDDL.DataBind();
            }
            if (IsNeedSelect)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ObjDDL.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static void BindGlobalDataTable(this DropDownList ddlSourceControl, string strProcName, Dictionary<string, string> dictProcParam, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {

            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (ObjDataTable == null)
                return;
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    /// <summary>
    /// Extension method to bind a dropdown list
    /// also adds select by default at 0th index
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="strBinidArgs"></param>
    public static void BindDataTable(this DropDownList ddlSourceControl, string strProcName, Dictionary<string, string> dictProcParam, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {

            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (ObjDataTable == null)
                return;
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();

            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    public static void BindDataTable(this DropDownList ddlSourceControl, DataTable dtDropDownValues)
    {
        string strDataTextField;

        try
        {
            ddlSourceControl.Items.Clear();
            if (dtDropDownValues != null)
            {
                if (dtDropDownValues.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = dtDropDownValues.Columns[0].ColumnName;
                    ddlSourceControl.DataTextField = dtDropDownValues.Columns[1].ColumnName;
                    ddlSourceControl.DataSource = dtDropDownValues;
                    ddlSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }
    }
    /// <summary>
    ///Extension method to bind a dropdown list
    /// also adds the select at 0th index if the blnIsSelectReq is true
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="blnIsSelectReq"></param>
    /// <param name="strBinidArgs"></param>
    public static void BindDataTable(this DropDownList ddlSourceControl, string strProcName, Dictionary<string, string> dictProcParam, bool blnIsSelectReq, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {

            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            if (blnIsSelectReq)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlSourceControl.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    /// <summary>
    /// Extension method to bind a dropdown list
    /// also adds given text by default at 0th index
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="blnIsSelectReq"></param>
    /// <param name="strDefaultSelect">Text to be dispalyed in 0th Index</param>
    /// <param name="strBinidArgs"></param>
    public static void BindDataTable(this DropDownList ddlSourceControl, string strProcName, Dictionary<string, string> dictProcParam, bool blnIsSelectReq, string strDefaultSelect, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {

            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            if (blnIsSelectReq)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--" + strDefaultSelect + "--", "0");
                ddlSourceControl.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    /// <summary>
    /// Extension method to bind a Ajax combo list
    /// also adds select by default at 0th index
    /// </summary>
    /// <param name="cmbSourceControl"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="strBinidArgs"></param>
    public static void BindDataTable(this ComboBox cmbSourceControl, string strProcName, Dictionary<string, string> dictProcParam, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {
            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);

            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString().Trim();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField.Trim());

            cmbSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    cmbSourceControl.DataValueField = strBinidArgs[0].ToString().Trim();
                    cmbSourceControl.DataTextField = "DataText";
                    cmbSourceControl.DataSource = ObjDataTable;
                    cmbSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(" ", "0");
            cmbSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    public static void BindDataTable(this DropDownList ddlSourceControl, DataTable ObjDataTable, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString().Trim();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField.Trim());

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString().Trim();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    /// <summary>
    /// Extension method to bind a Ajax combo list
    /// also adds required text sent as strSelectValue at 0th index
    /// if blnIsSelectReq is true
    /// </summary>
    /// <param name="cmbSourceControl"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="blnIsSelectReq"></param>
    /// <param name="strSelectValue"></param>
    /// <param name="strBinidArgs"></param>
    public static void BindDataTable(this ComboBox cmbSourceControl, string strProcName, Dictionary<string, string> dictProcParam, bool blnIsSelectReq, string strSelectValue, params string[] strBinidArgs)
    {
        string strDataTextField;
        try
        {
            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (strBinidArgs.Length > 2)
            {
                if (strBinidArgs[1].ToString().Trim().ToUpper() == "LOCATION_CODE")
                    strDataTextField = strBinidArgs[2].ToString().Trim();
                else
                    strDataTextField = strBinidArgs[1].ToString().Trim() + "+'  -  '+" + strBinidArgs[2].ToString().Trim();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString().Trim();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField.Trim());

            cmbSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    cmbSourceControl.DataValueField = strBinidArgs[0].ToString().Trim();
                    cmbSourceControl.DataTextField = "DataText";
                    cmbSourceControl.DataSource = ObjDataTable;
                    cmbSourceControl.DataBind();
                }
            }
            if (blnIsSelectReq)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem(strSelectValue, "0");
                cmbSourceControl.Items.Insert(0, liSelect);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    public static void BindMemoDataTable(this DropDownList ddlSourceControl, string strProcName, Dictionary<string, string> dictProcParam, params string[] strBinidArgs)
    {
        string strDataTextField;

        try
        {

            DataTable ObjDataTable = GetDefaultData(strProcName, dictProcParam);
            if (ObjDataTable == null)
                return;
            if (strBinidArgs.Length > 2)
            {
                strDataTextField = strBinidArgs[1].ToString() + "+'  -  '+" + strBinidArgs[2].ToString();
            }
            else
            {
                strDataTextField = strBinidArgs[1].ToString();
            }

            ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);

            ddlSourceControl.Items.Clear();
            if (ObjDataTable != null)
            {
                if (ObjDataTable.Rows.Count > 0)
                {
                    ddlSourceControl.DataValueField = strBinidArgs[0].ToString();
                    ddlSourceControl.DataTextField = "DataText";
                    ddlSourceControl.DataSource = ObjDataTable;
                    ddlSourceControl.DataBind();
                }
            }
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--All--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            //ObjAdminService.Close();
        }

    }

    /// <summary>
    /// Created by   : Tamilselvan.S
    /// Craeted Date : 04/08/2011
    /// Purpose      : For adding Dropdown list tooptip for all values.
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void AddItemToolTip(this DropDownList ddlSourceControl)
    {
        foreach (System.Web.UI.WebControls.ListItem li in ddlSourceControl.Items)
        {
            li.Attributes.Add("title", li.Text.ToString());
        }
    }


    /// <summary>
    /// Created by   : Tamilselvan.S
    /// Craeted Date : 23/09/2011
    /// Purpose      : For adding Dropdown list tooptip for all values.
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void AddItemToolTipValue(this DropDownList ddlSourceControl)
    {
        foreach (System.Web.UI.WebControls.ListItem li in ddlSourceControl.Items)
        {
            li.Attributes.Add("title", li.Value.ToString());
        }
    }

    #endregion

    /// <summary>
    /// To Check the string has special charcters
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="blnSpaceAllowed"></param>
    /// <param name="blnFirstLetterIsNumeric"></param>
    /// <returns></returns>
    public static bool HasSpecialCharacter(this string strValue, bool blnSpaceAllowed, bool blnFirstLetterIsNumeric)
    {
        bool blnSpResult = false;

        char[] chArray = strValue.ToCharArray();

        if (!blnFirstLetterIsNumeric)
        {
            string strNumber = "0123456789";
            char charFirst = Convert.ToChar(strValue.Substring(0, 1).ToLower());
            if (strNumber.IndexOf(charFirst) > 0)
            {
                return true;
            }
        }

        string strSpecialChar = @"~!@#$%^&*()_+|`-=\/?,.`[]{}:;'";

        if (!blnSpaceAllowed)
        {
            strSpecialChar += " ";
        }

        foreach (char ch in chArray)
        {
            if (strSpecialChar.IndexOf(ch) > 0)
            {
                blnSpResult = true;
                break;
            }
        }
        return blnSpResult;
    }

    /// <summary>
    /// To compare two dates
    /// gives 1 if startdate is less than enddate
    /// gives -1 if start date is greater that enddate
    /// gives 0 when both date are same
    /// else gives 2
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public static int CompareDates(string startDate, string endDate)
    {
        try
        {

            DateTime strDT = new DateTime();
            DateTime endDT = new DateTime();
            if (startDate != "")
            {
                strDT = StringToDate(startDate);
            }
            if (endDate != "")
            {
                endDT = StringToDate(endDate);
            }
            if (strDT == endDT)
            {
                return 0;
            }
            if (strDT < endDT)
            {
                return 1;
            }
            else if (strDT > endDT)
            {
                return -1;
            }
            else
            {
                return 2;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }


    //Created by Kali to compare Last Date
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strDateTime"></param>
    /// <returns></returns>
    public static bool FunPriCompareLastDate(string strDateTime)
    {
        DateTime dtDate = Utility.StringToDate(strDateTime);
        DateTime dtLast = new DateTime(dtDate.Year, dtDate.Month, 1).AddMonths(1).AddDays(-1);
        if (DateTime.Compare(dtDate, dtLast) == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strDateTime"></param>
    /// <returns></returns>
    public static bool IsDateTime(string strDateTime)
    {
        try
        {

            DateTime dtDatetime = Convert.ToDateTime(strDateTime);
            if (dtDatetime != null)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strVal"></param>
    /// <returns></returns>
    public static bool CheckZeroValidate(this string strVal)
    {
        try
        {
            int val = Convert.ToInt32(strVal);
            if (val != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grvGrid"></param>
    public static void ClearGrid(this GridView grvGrid)
    {
        try
        {
            if (grvGrid != null) // Check the Null value Before Dispose
            {
                grvGrid.Dispose();
                grvGrid.DataSource = null;
                grvGrid.DataBind();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    #endregion

    #region Methods to Handle Session

    /// <summary>
    /// Method to store a data in session
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public static void store(string name, object value)
    {
        if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
        {
            return;

        }
        else
        {
            System.Web.HttpContext.Current.Session[name] = value;

        }
    }
    /// <summary>
    /// method to get a value from session
    /// </summary>
    /// <param name="name"></param>
    /// <param name="defaultvalue"></param>
    /// <returns></returns>
    public static object Load(string name, object defaultvalue)
    {
        if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Session == null)
        {
            return null;

        }
        else
        {
            return System.Web.HttpContext.Current.Session[name];

        }
    }
    /// <summary>
    /// Method to kill sessions
    /// </summary>
    public static void FunPubKillSession()
    {
        System.Web.HttpContext.Current.Session.Abandon();
    }
    #endregion

    #region Methods For Paging User Control

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grvPaging"></param>
    /// <param name="arrSearchVal"></param>
    public static void FunPriSetSearchValue(this GridView grvPaging, ArrayList arrSearchVal)
    {
        if (grvPaging.HeaderRow != null)
        {
            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                TextBox txtHead = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch" + (iCount + 1).ToString());
                if (txtHead != null)
                    txtHead.Text = arrSearchVal[iCount].ToString().Replace("'", "");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grvPaging"></param>
    /// <param name="arrSearchVal"></param>
    public static void FunPriClearSearchValue(this GridView grvPaging, ArrayList arrSearchVal)
    {
        if (grvPaging.HeaderRow != null)
        {
            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                TextBox txtHead = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch" + (iCount + 1).ToString());
                if (txtHead != null)
                    txtHead.Text = "";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grvPaging"></param>
    /// <param name="arrSearchVal"></param>
    /// <param name="intNoofSearch"></param>
    /// <returns></returns>
    public static ArrayList FunPriGetSearchValue(this GridView grvPaging, ArrayList arrSearchVal, int intNoofSearch)
    {
        arrSearchVal = new ArrayList(intNoofSearch);
        if (grvPaging.HeaderRow != null)
        {
            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                TextBox txtHead = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch" + (iCount + 1).ToString());
                if (txtHead != null)
                    arrSearchVal.Add(txtHead.Text.Trim().Replace("'", "''"));
            }
        }
        else
        {
            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                arrSearchVal.Add("");
            }
        }
        return arrSearchVal;
    }

    /// <summary>
    /// Method will return a datatable by passing procedure name and 
    /// parameters as dictionary
    /// </summary>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <returns></returns>
    public static DataTable GetGridData(string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging)
    {
        intTotalRecords = 0;
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjDataTable = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteRoleDetails = ObjAdminService.FunPubFillGridPaging(out intTotalRecords, strProcName, dictProcParam, ObjPaging);
            ObjDataTable = (DataTable)ClsPubSerialize.DeSerialize(byteRoleDetails, SerializationMode.Binary, typeof(DataTable));

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataTable;

    }

    public static DataTable GetGridData_Report(string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging)
    {
        intTotalRecords = 0;
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjDataTable = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteRoleDetails = ObjAdminService.FunPubFillGridPaging(out intTotalRecords, strProcName, dictProcParam, ObjPaging);
            ObjDataTable = (DataTable)ClsPubSerialize.DeSerialize(byteRoleDetails, SerializationMode.Binary, typeof(DataTable));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataTable;

    }

    public static DataTable GetGridData(string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, out int intErrorCode, PagingValues ObjPaging)
    {
        intTotalRecords = 0;
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjDataTable = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteRoleDetails = ObjAdminService.FunPubFillGridPagingWithErrorCode(out intTotalRecords, out intErrorCode, strProcName, dictProcParam, ObjPaging);
            ObjDataTable = (DataTable)ClsPubSerialize.DeSerialize(byteRoleDetails, SerializationMode.Binary, typeof(DataTable));

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataTable;
    }

    public static void BindRepeater(this Repeater grvSource, string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging, out bool bIsNewRow)
    {
        intTotalRecords = 0;
        bIsNewRow = false;
        try
        {
            DataTable ObjDataTable = GetGridData(strProcName, dictProcParam, out intTotalRecords, ObjPaging);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count == 0)
                {
                    ObjDataView.AddNew();

                    if (ObjDataTable.Columns.Contains("is_active"))
                        ObjDataView[0]["is_active"] = false;

                    if (ObjDataTable.Columns.Contains("Created_By"))
                        ObjDataView[0]["Created_By"] = 0;

                    if (ObjDataTable.Columns.Contains("USRID"))
                        ObjDataView[0]["USRID"] = 0;

                    if (ObjDataTable.Columns.Contains("User_Level_ID"))
                        ObjDataView[0]["User_Level_ID"] = 0;

                    if (ObjDataTable.Columns.Contains("ID"))
                        ObjDataView[0]["ID"] = 0;

                    if (ObjDataTable.Columns.Contains("Is_Operational"))
                        ObjDataView[0]["Is_Operational"] = false;

                    bIsNewRow = true;
                }
                grvSource.DataSource = ObjDataView;
                grvSource.DataBind();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }


    public static void BindGridView(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging, out bool bIsNewRow)
    {
        intTotalRecords = 0;
        bIsNewRow = false;
        try
        {
            DataTable ObjDataTable = GetGridData(strProcName, dictProcParam, out intTotalRecords, ObjPaging);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count == 0)
                {
                    ObjDataView.AddNew();

                    if (ObjDataTable.Columns.Contains("is_active"))
                        ObjDataView[0]["is_active"] = false;

                    if (ObjDataTable.Columns.Contains("Created_By"))
                        ObjDataView[0]["Created_By"] = 0;

                    if (ObjDataTable.Columns.Contains("USRID"))
                        ObjDataView[0]["USRID"] = 0;

                    if (ObjDataTable.Columns.Contains("User_Level_ID"))
                        ObjDataView[0]["User_Level_ID"] = 0;

                    if (ObjDataTable.Columns.Contains("ID"))
                        ObjDataView[0]["ID"] = 0;

                    if (ObjDataTable.Columns.Contains("Is_Operational"))
                        ObjDataView[0]["Is_Operational"] = false;

                    bIsNewRow = true;
                }
                grvSource.DataSource = ObjDataView;
                grvSource.DataBind();
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }

    public static void BindGridView_Report(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging, out bool bIsNewRow)
    {
        intTotalRecords = 0;
        bIsNewRow = false;
        try
        {
            DataTable ObjDataTable = GetGridData_Report(strProcName, dictProcParam, out intTotalRecords, ObjPaging);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count == 0)
                {
                    ObjDataView.AddNew();

                    if (ObjDataTable.Columns.Contains("is_active"))
                        ObjDataView[0]["is_active"] = false;

                    if (ObjDataTable.Columns.Contains("Created_By"))
                        ObjDataView[0]["Created_By"] = 0;

                    if (ObjDataTable.Columns.Contains("USRID"))
                        ObjDataView[0]["USRID"] = 0;

                    if (ObjDataTable.Columns.Contains("User_Level_ID"))
                        ObjDataView[0]["User_Level_ID"] = 0;

                    if (ObjDataTable.Columns.Contains("ID"))
                        ObjDataView[0]["ID"] = 0;

                    if (ObjDataTable.Columns.Contains("Is_Operational"))
                        ObjDataView[0]["Is_Operational"] = false;

                    bIsNewRow = true;
                }
                grvSource.DataSource = ObjDataView;
                grvSource.DataBind();
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }


    /// <summary>
    /// Created by Tamilselvan.S
    /// created Date 25/01/2011
    /// Purpose List of view control list grid view bind
    /// </summary>
    /// <param name="grvSource"></param>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <param name="intTotalRecords"></param>
    /// <param name="ObjPaging"></param>
    /// <param name="bIsNewRow"></param>
    /// <param name="arrSortColumn"></param>
    public static void BindGridView(this GridView grvSource, string strProcName, Dictionary<string, string> dictProcParam, out int intTotalRecords, PagingValues ObjPaging, out bool bIsNewRow, out string[] arrSortColumn)
    {
        intTotalRecords = 0;
        bIsNewRow = false;
        string[] arrSortColumns = new string[] { };
        try
        {
            DataTable ObjDataTable = GetGridData(strProcName, dictProcParam, out intTotalRecords, ObjPaging);

            //grvSource.Columns.Clear();

            if (ObjDataTable != null)
            {
                DataView ObjDataView = ObjDataTable.DefaultView;

                if (ObjDataView.Count == 0)
                {
                    ObjDataView.AddNew();

                    if (ObjDataTable.Columns.Contains("is_active"))
                        ObjDataView[0]["is_active"] = false;

                    if (ObjDataTable.Columns.Contains("Created_By"))
                        ObjDataView[0]["Created_By"] = 0;

                    if (ObjDataTable.Columns.Contains("User_Level_ID"))
                        ObjDataView[0]["User_Level_ID"] = 0;

                    if (ObjDataTable.Columns.Contains("ID"))
                        ObjDataView[0]["ID"] = 0;

                    bIsNewRow = true;
                }
                grvSource.DataSource = ObjDataView;
                grvSource.DataBind();
                arrSortColumns = new string[] { ObjDataView.Table.Columns[1].ToString(), ObjDataView.Table.Columns[2].ToString() };
            }
            arrSortColumn = arrSortColumns;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }
    #endregion

    #region Methods to from XML from Gridview

    /// <summary>
    /// This method will form XML for Gridview its is a extended method available for 
    /// all gridview.
    /// Enum for type of grid will be passed.Currently only grid with 
    /// all column as  Template Column or Bound column will be supported.
    /// </summary>
    /// <param name="grvPaging"></param>
    /// <param name="enumGridTyp"></param>
    /// <returns></returns>
    public static string FunPubFormXml(this GridView grvXml, enumGridType enumGridTyp)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");
        foreach (GridViewRow grvrow in grvXml.Rows)
        {
            intcolcount = 0;
            strbXml.Append(" <Details ");
            if (enumGridTyp == enumGridType.TemplateGrid)
            {
                foreach (TemplateField dtCols in grvXml.Columns)
                {
                    strColValue = "";
                    if (intcolcount < grvXml.Columns.Count)
                    {
                        if (grvrow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                        {
                            strColValue = ((TextBox)grvrow.Cells[intcolcount].Controls[1]).Text;
                        }
                        if (grvrow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                        {
                            strColValue = ((DropDownList)grvrow.Cells[intcolcount].Controls[1]).SelectedValue;
                        }
                        if (grvrow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                        {
                            strColValue = ((CheckBox)grvrow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                        }
                        if (grvrow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                        {
                            strColValue = ((Label)grvrow.Cells[intcolcount].Controls[1]).Text;
                        }
                        if (dtCols.HeaderText.ToString() != "" || dtCols.HeaderText.ToString() != string.Empty)
                        {
                            if (strColValue != "" || strColValue != string.Empty)
                            {
                                strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                                strColValue = strColValue.Replace("'", "\"");
                                strbXml.Append(dtCols.HeaderText.Replace(" ", "") + "='" + strColValue + "' ");
                            }
                        }
                    }
                    intcolcount++;
                }
            }
            if (enumGridTyp == enumGridType.BoundGrid)
            {
                foreach (BoundField dtCols in grvXml.Columns)
                {
                    strColValue = grvrow.Cells[intcolcount].Text;
                    if (dtCols.HeaderText.ToString() != "" || dtCols.HeaderText.ToString() != string.Empty)
                    {
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strbXml.Append(dtCols.HeaderText.Replace(" ", "") + "='" + strColValue + "' ");
                    }
                    intcolcount++;
                    strColValue = "";
                }
            }
            strbXml.Append(" /> ");
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    /// <summary>
    /// This method will form XML for Gridview its is a extended method available for 
    /// all gridview.
    /// </summary>
    /// <param name="grvXml"></param>
    /// <returns></returns>
    public static string FunPubFormXml(this GridView grvXml)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            intcolcount = 0;
            strbXml.Append(" <Details ");
            for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
            {
                if (grvXml.Columns[intcolcount].HeaderText != "")
                {
                    strColValue = grvRow.Cells[intcolcount].Text.Trim();
                    if (strColValue == "")
                    {
                        if (grvRow.Cells[intcolcount].Controls.Count > 0)
                        {
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                            {
                                strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                            {
                                strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                            {
                                strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                            {
                                strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "AjaxControlToolkit.AsyncFileUpload")
                            {
                                strColValue = ((AjaxControlToolkit.AsyncFileUpload)grvRow.Cells[intcolcount].Controls[1]).PostedFile.FileName;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                    }
                    if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                    {
                        if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                        {
                            strColValue = Utility.StringToDate(strColValue).ToString();
                        }
                    }
                    if (strColValue.Trim() == "")
                    {
                        strColValue = string.Empty;
                    }
                    strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                    strColValue = strColValue.Replace("'", "\"");
                    strbXml.Append(grvXml.Columns[intcolcount].HeaderText.Replace(" ", "") + "='" + strColValue.Trim() + "' ");
                }

            }
            strbXml.Append(" /> ");
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    /// <summary>
    /// TO Form XML WIth Column as Uppercase
    /// </summary>
    /// <param name="grvXml"></param>
    /// <param name="IsNeedUpperCase">If its true then Alll colunm name will be in upper case</param>
    /// <returns></returns>
    public static string FunPubFormXml(this GridView grvXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {
            intcolcount = 0;
            strbXml.Append(" <Details ");
            for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
            {
                if (grvXml.Columns[intcolcount].HeaderText != "")
                {
                    strColValue = grvRow.Cells[intcolcount].Text;
                    if (strColValue == "")
                    {
                        if (grvRow.Cells[intcolcount].Controls.Count > 0)
                        {
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                            {
                                strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                            {
                                strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                            {
                                strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                            }
                            if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                            {
                                strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }

                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                    }
                    if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                    {
                        if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                        {
                            strColValue = Utility.StringToDate(strColValue).ToString();
                        }
                    }
                    if (strColValue.Trim() == "")
                    {
                        strColValue = string.Empty;
                    }
                    // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                    // Avoid adding that column to XML to insert the null value
                    if (strColValue.Trim() == "&nbsp;")
                    {
                        continue;
                    }
                    strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                    strColValue = strColValue.Replace("'", "\"").Trim();
                    if (IsNeedUpperCase)
                    {
                        strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                    }
                    else
                    {
                        strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                    }
                }

            }
            strbXml.Append(" /> ");
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    public static string FunPubFormXml(this GridView grvXml, bool IsNeedUpperCase, bool blnOnlyEnabledRows)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");

        foreach (GridViewRow grvRow in grvXml.Rows)
        {

            if ((grvRow.Enabled && blnOnlyEnabledRows) || (!blnOnlyEnabledRows))
            {
                intcolcount = 0;
                strbXml.Append(" <Details ");
                for (intcolcount = 0; intcolcount < grvRow.Cells.Count; intcolcount++)
                {
                    if (grvXml.Columns[intcolcount].HeaderText != "")
                    {
                        strColValue = grvRow.Cells[intcolcount].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[intcolcount].Controls.Count > 0)
                            {
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[intcolcount].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[intcolcount].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[intcolcount].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[intcolcount].Controls[1]).Text;
                                }
                            }
                            if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                            {
                                if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                                {
                                    strColValue = Utility.StringToDate(strColValue).ToString();
                                }
                            }

                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        if (grvXml.Columns[intcolcount].HeaderText.Contains("Date"))
                        {
                            if (strColValue.Trim() != "" && strColValue.Trim() != string.Empty)
                            {
                                strColValue = Utility.StringToDate(strColValue).ToString();
                            }
                        }
                        if (strColValue.Trim() == "")
                        {
                            strColValue = string.Empty;
                        }
                        // If Numeric BoundColumn has empty (SPACE &nbsp; value ) at the same time that field is a nullable column in DB 
                        // Avoid adding that column to XML to insert the null value
                        if (strColValue.Trim() == "&nbsp;")
                        {
                            continue;
                        }
                        strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                        strColValue = strColValue.Replace("'", "\"");
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(grvXml.Columns[intcolcount].HeaderText.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                    }
                }

                strbXml.Append(" /> ");
            }
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    /// <summary>
    /// This method will form XML for DataTable its is a extended method available for 
    /// all Datatable.
    /// </summary>
    /// <param name="DtXml"></param>
    /// <returns></returns>
    public static string FunPubFormXml(this DataTable DtXml)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");
        foreach (DataRow grvRow in DtXml.Rows)
        {
            intcolcount = 0;
            strbXml.Append(" <Details ");
            foreach (DataColumn dtCols in DtXml.Columns)
            {
                strColValue = grvRow.ItemArray[intcolcount].ToString();
                strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                strColValue = strColValue.Replace("'", "\"");

                if (!string.IsNullOrEmpty(strColValue))
                {
                    if (grvRow.ItemArray[intcolcount].ToString() != "" || dtCols.ColumnName != string.Empty)
                    {

                        dtCols.ColumnName.Replace(" ", "_");

                        if (dtCols.ColumnName.ToUpper().Contains("DATE"))
                            strbXml.Append(dtCols.ColumnName.Replace(" ", "_").Replace(".", "") + "='" + StringToDate(strColValue).ToString() + "' ");
                        else
                            strbXml.Append(dtCols.ColumnName.Replace(" ", "_").Replace(".", "") + "='" + strColValue + "' ");

                    }
                }
                intcolcount++;
            }
            strColValue = "";
            strbXml.Append(" /> ");
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DtXml"></param>
    /// <param name="IsNeedUpperCase"></param>
    /// <returns></returns>
    public static string FunPubFormXml(this DataTable DtXml, bool IsNeedUpperCase)
    {
        int intcolcount = 0;
        string strColValue = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        strbXml.Append("<Root>");
        foreach (DataRow grvRow in DtXml.Rows)
        {
            intcolcount = 0;
            strbXml.Append(" <Details ");
            foreach (DataColumn dtCols in DtXml.Columns)
            {
                strColValue = grvRow.ItemArray[intcolcount].ToString();
                strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
                strColValue = strColValue.Replace("'", "\"");
                if (!string.IsNullOrEmpty(strColValue))
                {
                    if (grvRow.ItemArray[intcolcount].ToString() != "" || dtCols.ColumnName != string.Empty)
                    {
                        if (IsNeedUpperCase)
                        {
                            if (dtCols.ColumnName.ToUpper().Contains("DATE"))
                                strbXml.Append(dtCols.ColumnName.ToUpper() + "='" + StringToDate(strColValue).ToString() + "' ");

                            else
                                strbXml.Append(dtCols.ColumnName.ToUpper() + "='" + strColValue + "' ");
                        }
                        else
                        {
                            if (dtCols.ColumnName.ToUpper().Contains("DATE"))

                                strbXml.Append(dtCols.ColumnName.ToLower() + "='" + StringToDate(strColValue).ToString() + "' ");

                            else
                                strbXml.Append(dtCols.ColumnName.ToLower() + "='" + strColValue + "' ");
                        }

                    }
                }
                intcolcount++;
            }
            strColValue = "";
            strbXml.Append(" /> ");
        }
        strbXml.Append("</Root>");
        return strbXml.ToString();
    }

    #endregion

    #region Method to export Grid To Excel,Word,TextFile
    public static void FunPubExportGrid(this GridView Grv, string strFileName, enumFileType FileType)
    {
        try
        {
            string attachment = "";
            StringBuilder str = new StringBuilder();
            if (FileType == enumFileType.Excel)
            {
                attachment = "attachment; filename=" + strFileName + ".xls";
                //for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                //{
                //    Grv.Rows[i].Cells[0].Attributes.Add("class", "text");

                //}
            }
            if (FileType == enumFileType.Word)
                attachment = "attachment; filename=" + strFileName + ".doc";
            if (FileType == enumFileType.FlatFile)
            {

                if (Grv.Caption != string.Empty)
                {
                    str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.Append(System.Environment.NewLine);
                    str.Append("|" + Grv.Caption + "|");
                    str.Append(System.Environment.NewLine);
                }
                str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                str.Append(System.Environment.NewLine);
                for (int i = 0; i <= Grv.HeaderRow.Cells.Count - 1; i++)
                {
                    string strHeaderText = Grv.HeaderRow.Cells[i].Text;
                    if (strHeaderText == "")
                    {
                        strHeaderText = Grv.Columns[i].HeaderText;

                    }
                    //str.Append(strHeaderText);
                    if (strHeaderText.Contains("Amount"))
                        str.AppendFormat("{0,-10}", strHeaderText);
                    else
                        str.AppendFormat("{0,-20}", strHeaderText);
                    //str.Append("\t");
                }
                str.Append(System.Environment.NewLine);
                str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                str.Append(System.Environment.NewLine);
                for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                {
                    GridViewRow grvRow = Grv.Rows[i];
                    string strColValue;
                    for (int j = 0; j <= grvRow.Cells.Count - 1; j++)
                    {
                        strColValue = grvRow.Cells[j].Text;
                        if (strColValue == "")
                        {
                            if (grvRow.Cells[j].Controls.Count > 0)
                            {
                                if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                                }
                                if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                                }
                                if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                {
                                    strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                                }
                                if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                {
                                    strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                                }
                            }
                            if (strColValue.Trim() == "")
                            {
                                strColValue = string.Empty;
                            }
                        }
                        //str.Append(strColValue);
                        decimal decColValue; int intColValue;
                        if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                        {
                            str.AppendFormat("{0,8}", strColValue);
                        }
                        else
                        {
                            str.AppendFormat("{0,-20}", strColValue);
                        }
                        //str.Append("\t");
                    }
                    str.Append(System.Environment.NewLine);
                }

                attachment = "attachment; filename=" + strFileName + ".txt";
            }
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (FileType == enumFileType.Excel)
                HttpContext.Current.Response.ContentType = "application/vnd.xls";
            if (FileType == enumFileType.Word)
                HttpContext.Current.Response.ContentType = "application/vnd.doc";
            if (FileType == enumFileType.FlatFile)
                HttpContext.Current.Response.ContentType = "application/vnd.text";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //string strStyle = @"<style>.text { mso-number-format:\@; } </style>";

            if (FileType == enumFileType.FlatFile)
            {
                if (Grv.Rows.Count > 0)
                {

                    HttpContext.Current.Response.Write(str.ToString());
                }
            }

            else
            {
                if (Grv.Rows.Count > 0)
                {
                    Grv.RenderControl(htw);
                    // HttpContext.Current.Response.Write(strStyle);
                    HttpContext.Current.Response.Write(sw.ToString());

                }
            }
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export the Grid");
        }
    }

    /// <summary>
    /// Only Demand spoolig report
    /// </summary>
    /// <param name="Grv"></param>
    /// <param name="strFileName"></param>
    /// <param name="FileType"></param>

    /*
    public static void FunPubExportGrid_DemandSpooling(this GridView Grv, string strFileName, enumFileType FileType)
    {
        try
        {
            string attachment = "";
            StringBuilder str = new StringBuilder();
            if (FileType == enumFileType.Excel)
            {
                //attachment = "attachment; filename=" + strFileName + ".xls";

                #region For Excel
                str.Append("<table border='1' width='100%'>");

                if (Grv.Caption != string.Empty)
                {
                    //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    //str.Append(System.Environment.NewLine);
                    str.Append("<tr><td align='center' style=' font-weight:bold;' colspan='" + (Grv.HeaderRow.Cells.Count - 1).ToString() + "' >");
                    str.Append(Grv.Caption);
                    str.Append("</td></tr>");
                    //str.Append(System.Environment.NewLine);
                }
                //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                //str.Append(System.Environment.NewLine);

                string strPreLine = "";
                for (int DtSpooli = 0; DtSpooli < Grv.Rows.Count; DtSpooli++)
                {
                    string strLine = "";
                    strLine = Grv.Rows[DtSpooli].Cells[0].Text;

                    if (strLine != strPreLine)
                    {
                        if (strLine != "")
                        {
                            str.Append("<tr><td align='center' colspan='" + (Grv.HeaderRow.Cells.Count - 1).ToString() + "' style=' font-weight:bold;'>");
                            str.Append("Line Of Business - " + strLine);
                            str.Append("</td></tr>");
                        }
                        str.Append(System.Environment.NewLine);
                        str.Append("<tr>");
                        for (int i = 0; i <= Grv.HeaderRow.Cells.Count - 1; i++)
                        {
                            string strHeaderText = Grv.HeaderRow.Cells[i].Text;
                            if (strHeaderText == "")
                            {
                                strHeaderText = Grv.Columns[i].HeaderText;
                            }

                            //str.Append(strHeaderText);
                            if (strHeaderText.ToUpper() != "LINE OF BUSINESS")
                            {
                                str.Append("<td align='center' style=' font-weight:bold;' >");
                                if (strHeaderText.Contains("Amount"))
                                    str.AppendFormat("{0,-10}", strHeaderText);
                                else
                                    str.AppendFormat("{0,-20}", strHeaderText);
                                str.Append("</td>");
                            }
                            //str.Append("\t");
                        }
                        str.Append("</tr>");
                        //str.Append(System.Environment.NewLine);
                        //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        //str.Append(System.Environment.NewLine);
                        for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                        {
                            str.Append("<tr>");
                            GridViewRow grvRow = Grv.Rows[i];
                            string strColValue;
                            for (int j = 0; j <= grvRow.Cells.Count - 1; j++)
                            {
                                strColValue = grvRow.Cells[j].Text;

                                if (strColValue == "")
                                {
                                    if (grvRow.Cells[j].Controls.Count > 0)
                                    {
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                        {
                                            strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                        {
                                            strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                        {
                                            strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                        {
                                            strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                    }
                                    if (strColValue.Trim() == "")
                                    {
                                        strColValue = string.Empty;
                                    }
                                }
                                //str.Append(strColValue);
                                if (j != 0 && Grv.HeaderRow.Cells[j].Text.ToUpper() != "LINE OF BUSINESS")
                                {
                                    str.Append("<td>");
                                    decimal decColValue; int intColValue;
                                    if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                                    {
                                        str.AppendFormat("{0,8}", strColValue);
                                    }
                                    else
                                    {
                                        str.AppendFormat("{0,-20}", strColValue.Trim().Replace("&nbsp;", ""));
                                    }
                                    str.Append("</td>");
                                }

                                //str.Append("\t");
                            }
                            str.Append("</tr>");
                            //str.Append(System.Environment.NewLine);
                        }
                        strPreLine = strLine;
                        str.Append(System.Environment.NewLine);
                    }
                }
                str.Append("</table>");
                attachment = "attachment; filename=" + strFileName + ".xls";

                #endregion
            }
            if (FileType == enumFileType.Word)
                attachment = "attachment; filename=" + strFileName + ".doc";
            if (FileType == enumFileType.FlatFile)
            {
                if (Grv.Caption != string.Empty)
                {
                    str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.Append(System.Environment.NewLine);
                    str.Append("|" + Grv.Caption + "|");
                    str.Append(System.Environment.NewLine);
                }
                str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                str.Append(System.Environment.NewLine);

                string strPreLine = "";
                for (int DtSpooli = 0; DtSpooli < Grv.Rows.Count; DtSpooli++)
                {
                    string strLine = "";
                    strLine = Grv.Rows[DtSpooli].Cells[0].Text;

                    if (strLine != strPreLine)
                    {
                        if (strLine != "")
                        {
                            str.Append(System.Environment.NewLine);
                            str.Append("Line Of Business - " + strLine);
                            str.Append(System.Environment.NewLine);
                        }
                        str.Append(System.Environment.NewLine);

                        for (int i = 0; i <= Grv.HeaderRow.Cells.Count - 1; i++)
                        {
                            string strHeaderText = Grv.HeaderRow.Cells[i].Text;
                            if (strHeaderText == "")
                            {
                                strHeaderText = Grv.Columns[i].HeaderText;

                            }
                            //str.Append(strHeaderText);
                            if (strHeaderText.ToUpper() != "LINE OF BUSINESS")
                            {
                                if (strHeaderText.Contains("Amount"))
                                    str.AppendFormat("{0,-10}", strHeaderText);
                                else
                                    str.AppendFormat("{0,-20}", strHeaderText);
                            }
                            //str.Append("\t");
                        }
                        str.Append(System.Environment.NewLine);
                        str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        str.Append(System.Environment.NewLine);
                        for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                        {
                            GridViewRow grvRow = Grv.Rows[i];
                            string strColValue;
                            for (int j = 0; j <= grvRow.Cells.Count - 1; j++)
                            {
                                strColValue = grvRow.Cells[j].Text;

                                if (strColValue == "")
                                {
                                    if (grvRow.Cells[j].Controls.Count > 0)
                                    {
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                        {
                                            strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                        {
                                            strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                        {
                                            strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                        {
                                            strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                    }
                                    if (strColValue.Trim() == "")
                                    {
                                        strColValue = string.Empty;
                                    }
                                }
                                //str.Append(strColValue);
                                if (j != 0 && Grv.HeaderRow.Cells[j].Text.ToUpper() != "LINE OF BUSINESS")
                                {
                                    decimal decColValue; int intColValue;
                                    if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                                    {
                                        str.AppendFormat("{0,8}", strColValue);
                                    }
                                    else
                                    {
                                        str.AppendFormat("{0,-20}", strColValue.Trim().Replace("&nbsp;", ""));
                                    }
                                }

                                //str.Append("\t");
                            }
                            str.Append(System.Environment.NewLine);
                        }
                        strPreLine = strLine;
                        str.Append(System.Environment.NewLine);
                    }
                }
                attachment = "attachment; filename=" + strFileName + ".txt";
            }
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (FileType == enumFileType.Excel)
                HttpContext.Current.Response.ContentType = "application/vnd.xls";
            if (FileType == enumFileType.Word)
                HttpContext.Current.Response.ContentType = "application/vnd.doc";
            if (FileType == enumFileType.FlatFile)
                HttpContext.Current.Response.ContentType = "application/vnd.text";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //string strStyle = @"<style>.text { mso-number-format:\@; } </style>";

            if (FileType == enumFileType.FlatFile || FileType == enumFileType.Excel)
            {
                if (Grv.Rows.Count > 0)
                {

                    HttpContext.Current.Response.Write(str.ToString());
                }
            }

            else
            {
                if (Grv.Rows.Count > 0)
                {
                    Grv.RenderControl(htw);
                    // HttpContext.Current.Response.Write(strStyle);
                    HttpContext.Current.Response.Write(sw.ToString());

                }
            }
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export the Grid");
        }
    }
    */

    //Added by Kali on 15-Jan-2013
    //Code end to avoid same account repeating in all LOB's
    public static void FunPubExportGrid_DemandSpooling(this GridView Grv, DataTable DTGrv, string strFileName, enumFileType FileType)
    {
        try
        {
            string attachment = "";
            StringBuilder str = new StringBuilder();
            if (FileType == enumFileType.Excel)
            {
                //attachment = "attachment; filename=" + strFileName + ".xls";

                #region For Excel
                str.Append("<table border='1' width='100%'>");

                if (Grv.Caption != string.Empty)
                {
                    //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    //str.Append(System.Environment.NewLine);
                    str.Append("<tr><td align='center' style=' font-weight:bold;' colspan='" + (Grv.HeaderRow.Cells.Count - 1).ToString() + "' >");
                    str.Append(Grv.Caption);
                    str.Append("</td></tr>");
                    //str.Append(System.Environment.NewLine);
                }
                //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                //str.Append(System.Environment.NewLine);

                string strPreLine = "";
                for (int DtSpooli = 0; DtSpooli < Grv.Rows.Count; DtSpooli++)
                {
                    string strLine = "";
                    strLine = Grv.Rows[DtSpooli].Cells[0].Text;

                    if (strLine != strPreLine)
                    {
                        if (strLine != "")
                        {
                            str.Append("<tr><td align='center' colspan='" + (Grv.HeaderRow.Cells.Count - 1).ToString() + "' style=' font-weight:bold;'>");
                            str.Append("Line Of Business - " + strLine);
                            str.Append("</td></tr>");
                        }
                        str.Append(System.Environment.NewLine);
                        str.Append("<tr>");
                        for (int i = 0; i <= Grv.HeaderRow.Cells.Count - 1; i++)
                        {
                            string strHeaderText = Grv.HeaderRow.Cells[i].Text;
                            if (strHeaderText == "")
                            {
                                strHeaderText = Grv.Columns[i].HeaderText;
                            }

                            //str.Append(strHeaderText);
                            if (strHeaderText.ToUpper() != "LINE OF BUSINESS")
                            {
                                str.Append("<td align='center' style=' font-weight:bold;' >");
                                if (strHeaderText.Contains("Amount"))
                                    str.AppendFormat("{0,-10}", strHeaderText);
                                else
                                    str.AppendFormat("{0,-20}", strHeaderText);
                                str.Append("</td>&nbsp;&nbsp;&nbsp;");
                            }
                            //str.Append("\t");
                        }
                        str.Append("</tr>");
                        //str.Append(System.Environment.NewLine);
                        //str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        //str.Append(System.Environment.NewLine);
                        for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                        {
                            str.Append("<tr>");
                            GridViewRow grvRow = Grv.Rows[i];
                            string strColValue;
                            for (int j = 0; j <= grvRow.Cells.Count - 1; j++)
                            {
                                strColValue = grvRow.Cells[j].Text;

                                if (strColValue == "")
                                {
                                    if (grvRow.Cells[j].Controls.Count > 0)
                                    {
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                        {
                                            strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                        {
                                            strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                        {
                                            strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                        {
                                            strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                    }
                                    if (strColValue.Trim() == "")
                                    {
                                        strColValue = string.Empty;
                                    }
                                }
                                //str.Append(strColValue);
                                if (j != 0 && Grv.HeaderRow.Cells[j].Text.ToUpper() != "LINE OF BUSINESS")
                                {
                                    str.Append("<td>");
                                    decimal decColValue; int intColValue;
                                    if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                                    {
                                        str.AppendFormat("{0,8}", strColValue);
                                    }
                                    else
                                    {
                                        str.AppendFormat("{0,-20}", strColValue.Trim().Replace("&nbsp;", ""));
                                    }
                                    str.Append("</td>&nbsp;&nbsp;&nbsp;");
                                }

                                //str.Append("\t");
                            }
                            str.Append("</tr>");
                            //str.Append(System.Environment.NewLine);
                        }
                        strPreLine = strLine;
                        str.Append(System.Environment.NewLine);
                    }
                }
                str.Append("</table>");
                //attachment = "attachment; filename=" + strFileName + ".xls";

                #endregion
            }
            if (FileType == enumFileType.Word)
                attachment = "attachment; filename=" + strFileName + ".doc";

            if (FileType == enumFileType.Excel)
                attachment = "attachment; filename=" + strFileName + ".xls";
            if (FileType == enumFileType.FlatFile)
            {
                if (Grv.Caption != string.Empty)
                {
                    str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.Append(System.Environment.NewLine);
                    str.Append("|" + Grv.Caption + "|");
                    str.Append(System.Environment.NewLine);
                }
                str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                str.Append(System.Environment.NewLine);

                string strPreLine = "";
                DataTable DTLOBList = new DataTable();
                DTLOBList = DTGrv.DefaultView.ToTable("LOB", true, "LINE OF BUSINESS");

                for (int DRLOB = 0; DRLOB < DTLOBList.Rows.Count; DRLOB++)
                {
                    string strLine = "";
                    strLine = DTLOBList.Rows[DRLOB][0].ToString();

                    if (strLine != strPreLine)
                    {
                        if (strLine != "")
                        {
                            str.Append(System.Environment.NewLine);
                            str.Append("Line Of Business - " + strLine);
                            str.Append(System.Environment.NewLine);
                        }
                        str.Append(System.Environment.NewLine);



                        for (int i = 0; i <= DTGrv.Columns.Count - 1; i++)
                        {
                            string strHeaderText = DTGrv.Columns[i].Caption.ToString();
                            if (strHeaderText == "")
                            {
                                strHeaderText = DTGrv.Columns[i].Caption.ToString();

                            }
                            //str.Append(strHeaderText);
                            if (strHeaderText.ToUpper() != "LINE OF BUSINESS")
                            {
                                if (strHeaderText.Contains("Amount"))
                                {
                                    str.AppendFormat("{0,-12}", strHeaderText);
                                    str.Append("");
                                    str.Append("");
                                    str.Append("");
                                }
                                else
                                    str.AppendFormat("{0,-20}", strHeaderText);
                            }
                            //str.Append("\t");
                        }

                    }
                    str.Append(System.Environment.NewLine);
                    str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.Append(System.Environment.NewLine);

                    //DTGrv.DefaultView.ToTable("LOB", true, "LINE OF BUSINESS");
                    DataRow[] DRDET = DTGrv.Select("[LINE OF BUSINESS]='" + strLine.ToString() + "'");

                    for (int i = 0; i < DRDET.Length; i++)
                    {
                        string strColValue;
                        for (int j = 0; j < DTGrv.Columns.Count; j++)
                        {
                            strColValue = DRDET[i][j].ToString();
                            //if (strColValue == "")
                            ////{
                            ////    if (grvRow.Cells[j].Controls.Count > 0)
                            ////    {
                            ////        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                            ////        {
                            ////            strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                            ////        }
                            ////        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                            ////        {
                            ////            strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                            ////        }
                            ////        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                            ////        {
                            ////            strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                            ////        }
                            ////        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                            ////        {
                            ////            strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                            ////        }
                            ////    }
                            //    //if (strColValue.Trim() == "")
                            //    //{
                            //    //    strColValue = string.Empty;
                            //    //}
                            //}
                            //str.Append(strColValue);
                            if (j != 0 && DTGrv.Columns[j].Caption.ToUpper() != "LINE OF BUSINESS")
                            {
                                decimal decColValue; int intColValue;
                                if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                                {
                                    str.AppendFormat("{0,10}", strColValue);
                                    str.Append("");
                                    str.Append("");
                                    str.Append("");
                                    str.Append("");
                                }
                                else
                                {
                                    str.AppendFormat("{0,-20}", strColValue.Trim().Replace("&nbsp;", ""));
                                }
                            }
                        }
                        str.Append(System.Environment.NewLine);
                        strPreLine = strLine;
                        str.Append(System.Environment.NewLine);
                    }
                    attachment = "attachment; filename=" + strFileName + ".txt";
                }                
            }
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (FileType == enumFileType.Excel)
                HttpContext.Current.Response.ContentType = "application/vnd.xls";
            if (FileType == enumFileType.Word)
                HttpContext.Current.Response.ContentType = "application/vnd.doc";
            if (FileType == enumFileType.FlatFile)
                HttpContext.Current.Response.ContentType = "application/vnd.text";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //string strStyle = @"<style>.text { mso-number-format:\@; } </style>";

            if (FileType == enumFileType.FlatFile || FileType == enumFileType.Excel)
            {
                if (Grv.Rows.Count > 0)
                {

                    HttpContext.Current.Response.Write(str.ToString());
                }
            }

            else
            {
                if (Grv.Rows.Count > 0)
                {
                    Grv.RenderControl(htw);
                    // HttpContext.Current.Response.Write(strStyle);
                    HttpContext.Current.Response.Write(sw.ToString());

                }
            }
            HttpContext.Current.Response.End();
        }


        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Export the Grid");
        }

    }
    //Code end to avoid same account repeating in all LOB's
    

    /// <summary>
    /// To save a file Only supports Flat File as of now
    /// </summary>
    /// <param name="Grv"></param>
    /// <param name="strFileName"></param>
    /// <param name="FileType"></param>
    /// <param name="blnSaveFile"></param>
    /// <param name="strPath"></param>
    public static void FunPubExportGrid(this GridView Grv, string strFileName, enumFileType FileType, bool blnSaveFile, string strPath)
    {
        try
        {
            string attachment = "";
            StringBuilder str = new StringBuilder();
            if (FileType == enumFileType.Excel)
            {
                strFileName = strFileName + ".xls";
            }
            if (FileType == enumFileType.Word)
                strFileName = strFileName + ".doc";
            if (FileType == enumFileType.FlatFile)
            {

                if (Grv.Caption != string.Empty)
                {
                    str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    str.Append(System.Environment.NewLine);
                    str.Append("|" + Grv.Caption + "|");
                    str.Append(System.Environment.NewLine);
                }
                str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                str.Append(System.Environment.NewLine);

                string strPreLine = "";
                for (int DtSpooli = 0; DtSpooli < Grv.Rows.Count; DtSpooli++)
                {
                    string strLine = "";
                    strLine = Grv.Rows[DtSpooli].Cells[0].Text.ToString();

                    if (strLine != strPreLine)
                    {
                        if (strLine != "")
                        {
                            str.Append(System.Environment.NewLine);
                            str.Append("Line Of Business - " + strLine);
                            str.Append(System.Environment.NewLine);
                        }

                        str.Append(System.Environment.NewLine);

                        for (int i = 0; i <= Grv.HeaderRow.Cells.Count - 1; i++)
                        {
                            string strHeaderText = Grv.HeaderRow.Cells[i].Text;
                            if (strHeaderText == "")
                            {
                                strHeaderText = Grv.Columns[i].HeaderText;
                            }
                            //str.Append(strHeaderText);
                            if (strHeaderText.ToUpper() != "LINE OF BUSINESS")
                            {
                                str.AppendFormat("{0,-20}", strHeaderText);
                            }
                        }
                        str.Append(System.Environment.NewLine);
                        str.Append("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        str.Append(System.Environment.NewLine);
                        for (int i = 0; i <= Grv.Rows.Count - 1; i++)
                        {
                            GridViewRow grvRow = Grv.Rows[i];
                            string strColValue;
                            for (int j = 0; j <= grvRow.Cells.Count - 1; j++)
                            {
                                strColValue = grvRow.Cells[j].Text;
                                if (strColValue == "")
                                {
                                    if (grvRow.Cells[j].Controls.Count > 0)
                                    {
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                        {
                                            strColValue = ((TextBox)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                        {
                                            strColValue = ((DropDownList)grvRow.Cells[j].Controls[1]).SelectedValue;
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                                        {
                                            strColValue = ((CheckBox)grvRow.Cells[j].Controls[1]).Checked == true ? "1" : "0";
                                        }
                                        if (grvRow.Cells[j].Controls[1].GetType().ToString() == "System.Web.UI.WebControls.Label")
                                        {
                                            strColValue = ((Label)grvRow.Cells[j].Controls[1]).Text;
                                        }
                                    }
                                    if (strColValue.Trim() == "")
                                    {
                                        strColValue = string.Empty;
                                    }
                                }
                                //str.Append(strColValue);
                                // str.Append("\t");
                                decimal decColValue; int intColValue;
                                if (j != 0 && Grv.HeaderRow.Cells[j].Text.ToUpper() != "LINE OF BUSINESS")
                                {
                                    if (decimal.TryParse(strColValue, out decColValue) || int.TryParse(strColValue, out intColValue))
                                    {
                                        str.AppendFormat("{0,10}", strColValue);
                                    }
                                    else
                                    {
                                        str.AppendFormat("{0,-20}", strColValue.Trim().Replace("&nbsp;", ""));
                                    }
                                }
                            }
                            str.Append(System.Environment.NewLine);
                        }
                    }
                    strPreLine = strLine;
                    str.Append(System.Environment.NewLine);
                }
            }


            if (blnSaveFile)
            {
                if (strPath != "")
                {
                    using (StreamWriter strWriter = new StreamWriter(strPath + @"\" + strFileName))
                    {
                        strWriter.Write(str.ToString());
                        strWriter.AutoFlush = true;
                        strWriter.Flush();
                        strWriter.Close();
                        strWriter.Dispose();

                    }
                    //StreamWriter strWriter = new StreamWriter(strPath + @"\" + strFileName);

                }
            }
            //else
            //{
            //    HttpContext.Current.Response.End();
            //}

        }
        catch (Exception ex)
        {
            if (!blnSaveFile)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw new ApplicationException("Unable to Export the Grid");
            }
        }
    }
    #endregion

    #region To Excute a Sp and Return A datatable
    /// <summary>
    /// Method will return a datatable by passing procedure name and 
    /// parameters as dictionary
    /// </summary>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <returns></returns>
    public static DataTable GetDefaultData(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjDataTable = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteRoleDetails = ObjAdminService.FunPubFillDropdown(strProcName, dictProcParam);
            if (byteRoleDetails != null)
                ObjDataTable = (DataTable)ClsPubSerialize.DeSerialize(byteRoleDetails, SerializationMode.Binary, typeof(DataTable));

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
             throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataTable;
    }

    public static DataTable GetDefaultData_Report(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjDataTable = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteRoleDetails = ObjAdminService.FunPubFillDropdown_Report(strProcName, dictProcParam);
            if (byteRoleDetails != null)
                ObjDataTable = (DataTable)ClsPubSerialize.DeSerialize(byteRoleDetails, SerializationMode.Binary, typeof(DataTable));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataTable;
    }


    public static DataTable GetDDLRows(Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataTable ObjTables = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteDataset = ObjAdminService.FunPubExecuteSQL(dictProcParam);
            ObjTables = (DataTable)ClsPubSerialize.DeSerialize(byteDataset, SerializationMode.Binary, typeof(DataSet));
            return ObjTables;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
            if (ObjTables != null) // Check the Null value Before Dispose
            {
                ObjTables.Dispose();
                ObjTables = null;
            }
        }
        return null;
    }

    #endregion

    #region To Excecute an Sp and Return a dataset
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <returns></returns>
    public static DataSet GetTableValues(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataSet ObjTables = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteDataset = ObjAdminService.FunPubFillDataset(strProcName, dictProcParam);
            ObjTables = (DataSet)ClsPubSerialize.DeSerialize(byteDataset, SerializationMode.Binary, typeof(DataSet));
            return ObjTables;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
            if (ObjTables != null) // Check the Null value Before Dispose
            {
                ObjTables.Dispose();
                ObjTables = null;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strProcName"></param>
    /// <param name="dictProcParam"></param>
    /// <returns></returns>
    public static DataSet GetDataset(string strProcName, Dictionary<string, string> dictProcParam)
    {
        S3GAdminServicesReference.S3GAdminServicesClient ObjAdminService = null;
        DataSet ObjDataset = null;
        try
        {
            ObjAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
            byte[] byteDataset = ObjAdminService.FunPubFillDataset(strProcName, dictProcParam);
            ObjDataset = (DataSet)ClsPubSerialize.DeSerialize(byteDataset, SerializationMode.Binary, typeof(DataSet));

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjAdminService.Close();
        }
        return ObjDataset;
    }
    #endregion

    #region Methods to Fill Financial year Dropdown
    public static void FillYears(this DropDownList ddlSourceControl, int intPreviousYears, int intNextYears)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            System.Web.UI.WebControls.ListItem liSelect = null;
            int intCurrentYear = DateTime.Now.Year;
            int intCurrentMonth = DateTime.Now.Month;
            if (intCurrentMonth < 4)
            {
                intCurrentYear = intCurrentYear - 1;
            }
            int itemYear = 0;
            for (int intCount = intPreviousYears; intCount > 0; intCount--)
            {
                itemYear = intCurrentYear - intCount;
                liSelect = new System.Web.UI.WebControls.ListItem(itemYear.ToString(), itemYear.ToString());
                ddlSourceControl.Items.Add(liSelect);
            }
            liSelect = new System.Web.UI.WebControls.ListItem(intCurrentYear.ToString(), intCurrentYear.ToString());
            ddlSourceControl.Items.Add(liSelect);
            for (int intCount = 1; intCount <= intNextYears; intCount++)
            {
                itemYear = intCurrentYear + intCount;
                liSelect = new System.Web.UI.WebControls.ListItem(itemYear.ToString(), itemYear.ToString());
                ddlSourceControl.Items.Add(liSelect);
            }



        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    /// <summary>
    /// FillFinancialYears - to load countable financial years
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void FillFinancialYears(this DropDownList ddlSourceControl, int intPreviousYears, int intNextYears, bool blNeedSelect, bool blNeedCurYear)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            if (blNeedSelect)
            {
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlSourceControl.Items.Insert(0, liSelect);
            }

            int itemYear = 0;
            for (int intCount = intPreviousYears; intCount > 0; intCount--)
            {
                itemYear = intCurrentYear - intCount;
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((itemYear) + "-" + (itemYear + 1)), ((itemYear) + "-" + (itemYear + 1)));
                ddlSourceControl.Items.Add(liPSelect);
            }

            if (blNeedCurYear)
            {
                if (DateTime.Now.Month > 3)
                {
                    System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                    ddlSourceControl.Items.Add(liPSelect);
                }
                else
                {

                    System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                    ddlSourceControl.Items.Add(liPSelect);
                }
            }

            for (int intCount = 1; intCount <= intNextYears; intCount++)
            {
                itemYear = intCurrentYear + intCount;
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((itemYear) + "-" + (itemYear + 1)), ((itemYear) + "-" + (itemYear + 1)));
                ddlSourceControl.Items.Add(liPSelect);
            }

        }
        catch (Exception exp)
        {
            throw exp;
        }

    }

    /// <summary>
    /// FillFinancialYears - to load countable financial years
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void FillFinancialYears(this DataTable dtReturnTable, int intPreviousYears, int intNextYears)
    {
        try
        {
            dtReturnTable.Columns.Add("ID");
            dtReturnTable.Columns.Add("YearValue");
            int intCurrentYear = DateTime.Now.Year;

            DataRow dRow;

            int itemYear = 0;
            for (int intCount = intPreviousYears; intCount > 0; intCount--)
            {
                itemYear = intCurrentYear - intCount;
                dRow = dtReturnTable.NewRow();
                dRow["ID"] = (dtReturnTable.Rows.Count + 1).ToString();
                dRow["YearValue"] = (itemYear).ToString() + "-" + (itemYear + 1).ToString();

                dtReturnTable.Rows.Add(dRow);
            }

            if (DateTime.Now.Month > 3)
            {
                dRow = dtReturnTable.NewRow();
                dRow["ID"] = (dtReturnTable.Rows.Count + 1).ToString();
                dRow["YearValue"] = (intCurrentYear).ToString() + "-" + (intCurrentYear + 1).ToString();

                dtReturnTable.Rows.Add(dRow);
            }
            else
            {
                dRow = dtReturnTable.NewRow();
                dRow["ID"] = (dtReturnTable.Rows.Count + 1).ToString();
                dRow["YearValue"] = (intCurrentYear - 1).ToString() + "-" + (intCurrentYear).ToString();

                dtReturnTable.Rows.Add(dRow);
            }

            for (int intCount = 1; intCount <= intNextYears; intCount++)
            {
                itemYear = intCurrentYear + intCount;

                dRow = dtReturnTable.NewRow();
                dRow["ID"] = (dtReturnTable.Rows.Count + 1).ToString();
                dRow["YearValue"] = (itemYear).ToString() + "-" + (itemYear + 1).ToString();

                dtReturnTable.Rows.Add(dRow);
            }

        }
        catch (Exception exp)
        {
            throw exp;
        }

    }
    /// <summary>
    /// Extension method for ddls to load financial year
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void FillFinancialYears(this DropDownList ddlSourceControl)
    {
        try
        {
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");

            ddlSourceControl.Items.Insert(0, liSelect);
            if (DateTime.Now.Month > 3)
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(1, liPSelect);

                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                ddlSourceControl.Items.Insert(2, liCSelect);

                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear + 1) + "-" + (intCurrentYear + 2)), ((intCurrentYear + 1) + "-" + (intCurrentYear + 2)));
                ddlSourceControl.Items.Insert(3, liNSelect);
            }
            else
            {

                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear - 1)), ((intCurrentYear - 2) + "-" + (intCurrentYear - 1)));
                ddlSourceControl.Items.Insert(1, liPSelect);

                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(2, liCSelect);

                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                ddlSourceControl.Items.Insert(3, liNSelect);

            }

        }
        catch (Exception exp)
        {
            throw exp;
        }

    }
    /// <summary>
    /// extension method ro load financial month based on financial year and 
    /// start month defined in web config app setting as "StartMonth"
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strFinancialYear"></param>
    public static void FillFinancialMonth(this DropDownList ddlSourceControl, string strFinancialYear)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            string[] Years = strFinancialYear.Split('-');
            int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            string strActualYear = Years[0].ToString();

            for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intActualMonth = 1;
                    strActualYear = Years[1].ToString();
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
                ddlSourceControl.Items.Insert(intMonthCnt, liPSelect);
                intActualMonth = intActualMonth + 1;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public static string Translate_Language(string htmlText, string Language)
    {
        System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();

        string strUserID = (string)AppReader.GetValue("USERID", typeof(string));
        string strPassword = (string)AppReader.GetValue("PASSWORD", typeof(string));
        if (strUserID != "")
            WebRequest.DefaultWebProxy.Credentials = new NetworkCredential(strUserID, strPassword);
        #region "Multi Lingual Implementation"
        Language Web_Language = new Language();
        if (htmlText != "")
        {
            Dictionary<string, string> Words = new Dictionary<string, string>();
            string s = htmlText;
            Regex r = new Regex(@">(.*?)<");
            s = Regex.Replace(s, "~.*?~", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //var pattern = @"[^0-9]+";

            //s = Regex.Replace(s, pattern, "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //var pattern = @"\d+";
            //s = Regex.Replace(s, pattern, "");
            MatchCollection mc = r.Matches(s);
            for (int i = 0; i < mc.Count; i++)
            {
                string text = mc[i].Groups[1].Value.Trim();
                //var pattern = @"[^0-9]+";
                //text = Regex.Replace(text, pattern, "");


                //if (!mc[i].Groups[1].Value.Contains("&") && mc[i].Groups[1].Value != "" && mc[i].Groups[1].Value != " ")
                text = text.Replace("&nbsp;", " ").Trim();
                if (!text.Contains("&") && text != "" && text != " ")
                {
                    string[] arrtext = text.Split(':');
                    foreach (string word in arrtext)
                    {
                        bool isNumeric = IsNumeric(word.Trim());
                        if (!Words.ContainsKey(word.Trim()) && word.Trim() != "" && isNumeric == false)
                        {
                            Words.Add(word.Trim(), " ");
                            //if (UserID != string.Empty)

                            htmlText = htmlText.Replace(word.Trim(), LanguageTranslator.LanguageTranslator.Translate(Language, word.Trim()));
                        }
                    }
                }
            }
        }

        return htmlText;
        //Language Change Code Ends
        #endregion

    }

    public static bool IsNumeric(object Expression)
    {
        double retNum;

        bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    public static void Fill2FinancialMonth(this DropDownList ddlSourceControl, string strFinancialYear)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            string[] Years = new string[3];
            Years[0] = strFinancialYear.Split('-')[0].ToString();
            Years[1] = strFinancialYear.Split('-')[1].ToString();
            Years[2] = (Convert.ToInt32(Years[0]) - 1).ToString();

            int intActualMonth = 4;
            Array.Sort(Years, StringComparer.InvariantCulture);
            int intYearcount = 0;
            string strActualYear = Years[intYearcount].ToString();

            for (int intMonthCnt = 1; intMonthCnt <= 24; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intYearcount += 1;
                    intActualMonth = 1;
                    strActualYear = Years[intYearcount].ToString();
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
                ddlSourceControl.Items.Insert(intMonthCnt, liPSelect);
                intActualMonth = intActualMonth + 1;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }



    //Added by M.saran on 26-August-2011.
    /// <summary>
    /// extension method ro load financial month based on financial year and 
    /// start month defined in web config app setting as "StartMonth"
    /// Includes Frequency Based Financial month.
    /// To load in dropdownList

    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strFinancialYear"></param>
    public static void FillFinancialMonth(this DropDownList ddlSourceControl, string strFinancialYear, int Frequency_Type)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            string[] Years = strFinancialYear.Split('-');
            int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            string strActualYear = Years[0].ToString();
            int intMonthFCnt = 0;
            for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intActualMonth = 1;
                    strActualYear = Years[1].ToString();
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem();
                switch (Frequency_Type)
                {
                    case 1:
                        intMonthFCnt++;
                        liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        break;
                    case 2:
                        if (intMonthCnt == 3 || intMonthCnt == 6 || intMonthCnt == 9 || intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        }
                        break;
                    case 3:
                        if (intMonthCnt == 6 || intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        }
                        break;
                    case 4:
                        if (intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strFinancialYear;
                        }
                        break;
                    default:
                        intMonthFCnt++;
                        liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        break;

                }
                if (liPSelect.Text != string.Empty)
                {
                    ddlSourceControl.Items.Insert(intMonthFCnt, liPSelect);
                }
                intActualMonth = intActualMonth + 1;
            }





        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }
    //Added by M.saran on 26-August-2011.
    /// <summary>
    /// extension method ro load financial month based on financial year and 
    /// start month defined in web config app setting as "StartMonth"
    /// Includes Frequency Based Financial month. 
    /// To load in Combo box
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    /// <param name="strFinancialYear"></param>
    public static void FillFinancialMonth(this ComboBox ddlSourceControl, string strFinancialYear, int Frequency_Type)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            string[] Years = strFinancialYear.Split('-');
            int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            string strActualYear = Years[0].ToString();
            int intMonthFCnt = 0;
            for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intActualMonth = 1;
                    strActualYear = Years[1].ToString();
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem();
                switch (Frequency_Type)
                {
                    case 1:
                        intMonthFCnt++;
                        liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        break;
                    case 2:
                        if (intMonthCnt == 3 || intMonthCnt == 6 || intMonthCnt == 9 || intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        }
                        break;
                    case 3:
                        if (intMonthCnt == 6 || intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        }
                        break;
                    case 4:
                        if (intMonthCnt == 12)
                        {
                            intMonthFCnt++;
                            liPSelect.Value = liPSelect.Text = strFinancialYear;
                        }
                        break;
                    default:
                        intMonthFCnt++;
                        liPSelect.Value = liPSelect.Text = strActualYear + intActualMonth.ToString("00");
                        break;

                }
                if (liPSelect.Text != string.Empty)
                {
                    ddlSourceControl.Items.Insert(intMonthFCnt, liPSelect);
                }
                intActualMonth = intActualMonth + 1;
            }





        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    public static void FillFinancialMonth(this ComboBox ddlSourceControl, string strFinancialYear)
    {
        try
        {
            ddlSourceControl.Items.Clear();
            int intCurrentYear = DateTime.Now.Year;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            ddlSourceControl.Items.Insert(0, liSelect);
            string[] Years = strFinancialYear.Split('-');
            int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
            string strActualYear = Years[0].ToString();
            for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
            {
                if (intActualMonth >= 13)
                {
                    intActualMonth = 1;
                    strActualYear = Years[1].ToString();
                }
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
                ddlSourceControl.Items.Insert(intMonthCnt, liPSelect);
                intActualMonth = intActualMonth + 1;
            }





        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    #endregion

    #region set suffix for label
    /// <summary>
    /// This methode used in Set Suffix for Label
    /// </summary>
    public static string SetSuffix()
    {
        int suffix = 0;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    public static string SetSuffix(int ControlSuffix)
    {
        int suffix = 0;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        if (suffix > ControlSuffix)
        {
            suffix = ControlSuffix;
        }
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    #endregion

    #region Method for Customer Address User Control

    /// <summary>
    /// 
    /// </summary>
    /// <param name="drCust"></param>
    /// <returns></returns>
    public static string SetCustomerAddress(DataRow drCust)
    {
        string strAddress = "";
        if (drCust["Comm_Address1"].ToString() != "") strAddress += drCust["Comm_Address1"].ToString() + System.Environment.NewLine;
        if (drCust["Comm_Address2"].ToString() != "") strAddress += drCust["Comm_Address2"].ToString() + System.Environment.NewLine;
        if (drCust["Comm_City"].ToString() != "") strAddress += drCust["Comm_City"].ToString() + System.Environment.NewLine;
        if (drCust["Comm_State"].ToString() != "") strAddress += drCust["Comm_State"].ToString() + System.Environment.NewLine;
        if (drCust["Comm_Country"].ToString() != "") strAddress += drCust["Comm_Country"].ToString() + System.Environment.NewLine;
        if (drCust["Comm_Pincode"].ToString() != "") strAddress += drCust["Comm_Pincode"].ToString();
        return strAddress;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="drCust"></param>
    /// <returns></returns>
    public static string SetVendorAddress(DataRow drCust)
    {
        string strAddress = "";
        if (drCust["Address"].ToString() != "") strAddress += drCust["Address"].ToString() + System.Environment.NewLine;
        if (drCust["Address2"].ToString() != "") strAddress += drCust["Address2"].ToString() + System.Environment.NewLine;
        if (drCust["City"].ToString() != "") strAddress += drCust["City"].ToString() + System.Environment.NewLine;
        if (drCust["State"].ToString() != "") strAddress += drCust["State"].ToString() + System.Environment.NewLine;
        if (drCust["Country"].ToString() != "") strAddress += drCust["Country"].ToString() + System.Environment.NewLine;
        if (drCust["Pincode"].ToString() != "") strAddress += drCust["Pincode"].ToString();
        return strAddress;
    }


    /// <summary>
    /// This methode used in SetCustomer Permanent Address in Textarea 
    /// </summary>
    #region SetCustomerPerAddress

    public static string SetCustomerPerAddress(DataRow drCust)
    {
        string strAddress = "";
        try
        {
            if (drCust["Perm_Address1"].ToString() != "") strAddress += drCust["Perm_Address1"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Address2"].ToString() != "") strAddress += drCust["Perm_Address2"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_City"].ToString() != "") strAddress += drCust["Perm_City"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_State"].ToString() != "") strAddress += drCust["Perm_State"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_Country"].ToString() != "") strAddress += drCust["Perm_Country"].ToString() + System.Environment.NewLine;
            if (drCust["Perm_PINCode"].ToString() != "") strAddress += drCust["Perm_PINCode"].ToString();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        return strAddress;
    }
    #endregion

    #endregion

    #region Global Decimal Value and Prefix Sufix Set

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="IsZeroCheckReq"></param>
    public static void CheckGPSLength(this TextBox txtBox, bool IsZeroCheckReq)
    {

        try
        {
            int intMaxLength;
            intMaxLength = txtBox.MaxLength;
            S3GSession S3GSession = new S3GSession();

            int intGPSLength = S3GSession.ProGpsPrefixRW;

            if (intGPSLength < intMaxLength)
            {
                txtBox.MaxLength = intGPSLength;
            }
            else
            {
                txtBox.MaxLength = intMaxLength == 0 ? intGPSLength : intMaxLength;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "ChkIsZero(this)");
            }

            txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");

            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="strFieldName"></param>
    public static void CheckGPSLength(this TextBox txtBox, bool IsZeroCheckReq, string strFieldName)
    {

        try
        {
            int intMaxLength;
            intMaxLength = txtBox.MaxLength;
            S3GSession S3GSession = new S3GSession();

            int intGPSLength = S3GSession.ProGpsPrefixRW;

            if (intGPSLength < intMaxLength)
            {
                txtBox.MaxLength = intGPSLength;
            }
            else
            {
                txtBox.MaxLength = intMaxLength == 0 ? intGPSLength : intMaxLength;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "ChkIsZero(this,'" + strFieldName + "')");
            }

            txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");

            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="IsNegativeRequired"></param>
    public static void CheckGPSLength(this TextBox txtBox, bool IsZeroCheckReq, bool IsNegativeRequired)
    {

        try
        {
            int intMaxLength;
            intMaxLength = txtBox.MaxLength;
            S3GSession S3GSession = new S3GSession();

            int intGPSLength = S3GSession.ProGpsPrefixRW;

            if (intGPSLength < intMaxLength)
            {
                txtBox.MaxLength = intGPSLength;
            }
            else
            {
                txtBox.MaxLength = intMaxLength == 0 ? intGPSLength : intMaxLength;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "ChkIsZero(this)");
            }
            if (IsNegativeRequired)
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,true,this)");
            }
            else
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(false,false,this)");
            }

            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    public static void SetDecimalPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {

            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','undefined',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','undefined',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// prefix and suffix value will not refer GPS

    public static void SetUserDecimalPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {            
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", " return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="strFieldName"></param>
    public static void SetDecimalPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {

            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", " return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="strFieldName"></param>
    public static void SetPercentagePrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", " return funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }


    /// <summary>
    /// Created by Tamilselvan.S 
    /// Created on 14/02/2011
    /// For Allowed only number with minus, plus and decimal
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="strFieldName"></param>
    public static void SetDecimalWithPlusMinPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimialWithPlusMin(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimialWithPlusMin(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }
    /// <summary>
    /// To show alternative colors in the grid based on the selected coloum
    /// </summary>
    /// <param name="grdView"></param>
    /// <param name="strPANColName"></param>
    public static void GetAlternativeColorToGrid(GridView grdView, string strPANColName)
    {
        if (grdView.Rows.Count > 0)
        {
            Label PANum, strPANum;
            string strColor = "A";
            strPANum = (Label)grdView.Rows[0].FindControl(strPANColName);
            //grdView.Rows[0].BackColor = System.Drawing.Color.AliceBlue;
            grdView.Rows[0].CssClass = "gridRowBackColor_First";
            for (int i = 1; i < grdView.Rows.Count; i++)
            {
                PANum = (Label)grdView.Rows[i].FindControl(strPANColName);
                if (PANum.Text == strPANum.Text)
                {
                    //if (grdView.Rows[i - 1].BackColor == System.Drawing.Color.AliceBlue)
                    if (strColor == "A")
                    {
                        //grdView.Rows[i].BackColor = System.Drawing.Color.AliceBlue;
                        grdView.Rows[i].CssClass = "gridRowBackColor_First";
                    }
                    else
                    {
                        //grdView.Rows[i].BackColor = System.Drawing.Color.DeepPink;
                        grdView.Rows[i].CssClass = "gridRowBackColor_Second";
                        //strColor = "PostColor";
                    }
                }
                else
                {
                    //if (grdView.Rows[i - 1].BackColor == System.Drawing.Color.DeepPink)
                    if (strColor == "B")
                    {
                        if (strPANum.Text == ((Label)grdView.Rows[i - 1].FindControl(strPANColName)).Text)
                        {
                            //grdView.Rows[i].BackColor = System.Drawing.Color.AliceBlue;
                            grdView.Rows[i].CssClass = "gridRowBackColor_First";
                            strColor = "A";
                            strPANum = PANum;
                        }
                        else
                        {
                            //grdView.Rows[i].BackColor = System.Drawing.Color.DeepPink;
                            grdView.Rows[i].CssClass = "gridRowBackColor_Second";
                            //strColor = "PostColor";
                            strPANum = PANum;
                        }
                    }
                    else
                    {
                        //grdView.Rows[i].BackColor = System.Drawing.Color.DeepPink;
                        grdView.Rows[i].CssClass = "gridRowBackColor_Second";
                        strColor = "B";
                        strPANum = PANum;
                    }
                }
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="IsNegativeRequired"></param>
    public static void SetDecimalPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, bool IsNegativeRequired)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {

            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }
            txtBox.MaxLength = intPrefix + intSuffix + 1;
            if (IsNegativeRequired)
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,true,this);");
            }
            else
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this);");
            }

            txtBox.Attributes.Add("onkeyup", "funCutDecimal(this,'" + intSuffix + "')");
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','undefined',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','undefined',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="strSourcePath"></param>
    /// <param name="strDestPath"></param>
    public static void saveFilePath(Page page, string strSourcePath, string strDestPath)
    {
        try
        {
            string path = strDestPath;
            strDestPath = strDestPath + Path.GetFileName(strSourcePath);

            if (strSourcePath == null || strDestPath == null)
            {
                //MessageBox.Show("source or destination path is missing");
                Utility.FunShowAlertMsg(page, "Source or Destination path is missing");
                return;
            }
            else
            {
                if (File.Exists(strSourcePath) == true)
                {
                    if (Directory.Exists(strSourcePath) == true)
                    {
                        if (File.Exists(strDestPath) == true)
                        {
                            File.Delete(strDestPath);
                            File.Copy(strSourcePath, strDestPath);
                            //MessageBox.Show("file replaced");
                        }
                        else
                        {

                            File.Copy(strSourcePath, strDestPath);
                            Utility.FunShowAlertMsg(page, "file saved");
                        }
                    }
                    else
                    {
                        File.Copy(strSourcePath, strDestPath);
                        if (Directory.Exists(path) == true)
                        {
                            //MessageBox.Show("file created and data copied");
                            Utility.FunShowAlertMsg(page, "file created and data copied");
                        }
                        else
                        {
                            //MessageBox.Show("Invalid path for destination");
                            //return "Invalid path for destination";
                            Utility.FunShowAlertMsg(page, "Invalid path for destination");
                            return;
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("source file not exists");                
                    Utility.FunShowAlertMsg(page, "source file not exists");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(page, ex.Message);
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="IsNegativeRequired"></param>
    /// <param name="strFieldName"></param>
    public static void SetDecimalPrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, bool IsNegativeRequired, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {

            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            S3GSession S3GSession = new S3GSession();
            intGPSPrefix = S3GSession.ProGpsPrefixRW;
            intGPSSuffix = S3GSession.ProGpsSuffixRW;

            if (intPrefix > intGPSPrefix)
            {
                intPrefix = intGPSPrefix;
            }
            if (intSuffix > intGPSSuffix)
            {
                intSuffix = intGPSSuffix;
            }
            txtBox.MaxLength = intPrefix + intSuffix + 1;
            if (IsNegativeRequired)
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,true,this);");
            }
            else
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this);");
            }
            txtBox.Attributes.Add("onkeyup", "funCutDecimal(this,'" + intSuffix + "')");
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="txtBox"></param>
    /// <param name="intPrefix"></param>
    /// <param name="intSuffix"></param>
    /// <param name="IsZeroCheckReq"></param>
    /// <param name="IsNegativeRequired"></param>
    /// <param name="strFieldName"></param>
    public static void SetPercentagePrefixSuffix(this TextBox txtBox, int intPrefix, int intSuffix, bool IsZeroCheckReq, bool IsNegativeRequired, string strFieldName)
    {
        DataTable dtTable = new DataTable();
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            txtBox.MaxLength = intPrefix + intSuffix + 1;
            if (IsNegativeRequired)
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,true,this);");
            }
            else
            {
                txtBox.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this);");
            }
            txtBox.Attributes.Add("onkeyup", "funCutDecimal(this,'" + intSuffix + "')");
            if (IsZeroCheckReq)
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',true);");
            }
            else
            {
                txtBox.Attributes.Add("onblur", "funChkDecimial(this,'" + intPrefix.ToString() + "','" + intSuffix.ToString() + "','" + strFieldName + "',false);");
            }
            txtBox.Style.Add("text-align", "right");
            txtBox.Attributes.Add("onpaste", "return false;");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    #endregion


    #region Common Method fo Mail
    public static string FunPubSentMail(Dictionary<string, string> dictMail, ArrayList arrMailAttachement, StringBuilder strBody)
    {
        string strErrMsg = "";
        if (dictMail["ToMail"].Trim() != "")
        {
            CommonMailServiceReference.CommonMailClient ObjCommonMail = new CommonMailServiceReference.CommonMailClient();
            try
            {
                ClsPubCOM_Mail ObjCom_Mail = new ClsPubCOM_Mail();
                ObjCom_Mail.ProFromRW = dictMail["FromMail"];
                ObjCom_Mail.ProDisplayName = dictMail["DisplayName"];
                ObjCom_Mail.ProTORW = dictMail["ToMail"];
                ObjCom_Mail.ProSubjectRW = dictMail["Subject"];
                ObjCom_Mail.ProMessageRW = strBody.ToString();// dictMail["Body"];
                if (dictMail.ContainsKey("ToCC") && dictMail["ToCC"].ToString() != "") 
                    ObjCom_Mail.ProCCRW = dictMail["ToCC"];
                if (dictMail.ContainsKey("ToBCC") && dictMail["ToBCC"].ToString() != "") 
                    ObjCom_Mail.ProBCCRW = dictMail["ToBCC"];
                if (arrMailAttachement != null && arrMailAttachement.Count > 0)
                    ObjCom_Mail.ProFileAttachementRW = arrMailAttachement;
                //opc038 start
                //opc056 start
                if(dictMail.ContainsKey("imgPath"))
                    ObjCom_Mail.ProImgPathRW = dictMail["imgPath"];
                //end
                ObjCommonMail.FunSendMail(ObjCom_Mail);
                
            }
            catch (System.ServiceModel.FaultException<CommonMailServiceReference.ClsPubFaultException> ex)
            {
                //if (ObjCommonMail != null)
                //    ObjCommonMail.Close();
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                strErrMsg = ex.Message;
                //throw ex;
            }
            catch (Exception ex)
            {
                //if (ObjCommonMail != null)
                //    ObjCommonMail.Close();
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                strErrMsg = ex.Message;
                //throw ex;
            }
            finally
            {
                if (ObjCommonMail != null)
                    ObjCommonMail.Close();
            }
        }

        return strErrMsg;
    }
    #endregion

    public static string FunPubSetCommaSeperator(string DecValue, string strCurrencyCode)
    {
        string Strvalue = "";
        string Strrevvalue = "";
        if (!string.IsNullOrEmpty(DecValue))
        {
            switch (strCurrencyCode.ToUpper().Trim())
            {
                case "INR"://Indian Currency
                    Strvalue = string.Format(System.Globalization.CultureInfo.GetCultureInfo("hi-IN").NumberFormat,
                    "{0:c}", Convert.ToDecimal(DecValue)).Split(' ')[1].ToString();
                    break;
                default://Us,Uk,Ero etc Currency
                    Strvalue = string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-us").NumberFormat,
                    "{0:c}", Convert.ToDecimal(DecValue)).Substring(1).Trim();
                    break;
            }

        }

        return Strvalue;

    }

    public static string FunPriCheckRowConcurrency(int intuserID, string strProgramCode, string strPROGREFNUM, string strSessionID)
    {
        Dictionary<string, string> Procparam = null;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@USER_ID", intuserID.ToString());
        Procparam.Add("@PROGRAM_CODE", strProgramCode);
        Procparam.Add("@PROG_REF_NUM", strPROGREFNUM);
        Procparam.Add("@SESSION_ID", strSessionID);
        string strUserRowLocked = Utility.GetTableScalarValue("S3G_SYSAD_Check_Locking", Procparam);
        return strUserRowLocked;
    }
    public static string FunPriRemoveLockedRow(int intuserID1, string strProgramCode1, string strPROGREFNUM1)
    {
        Dictionary<string, string> ProcparamRemoveLockRow = null;
        ProcparamRemoveLockRow = new Dictionary<string, string>();
        ProcparamRemoveLockRow.Add("@USER_ID", intuserID1.ToString());
        ProcparamRemoveLockRow.Add("@PROGRAM_CODE", strProgramCode1);
        ProcparamRemoveLockRow.Add("@PROG_REF_NUM", strPROGREFNUM1);
        string strRemoveRowLock = Utility.GetTableScalarValue("S3G_SYSAD_Remove_Locking", ProcparamRemoveLockRow);
        return strRemoveRowLock;
    }

    //Added by Chandru on 20/04/2012
    #region GetSuggestions
    /// <summary>
    /// GetSuggestions
    /// </summary>
    /// <param name="key">Country Names to search</param>
    /// <returns>Country Names Similar to key</returns>
    //public static List<String> GetSuggestions(string key, int count, DataTable dt1)
    public static List<String> GetSuggestions(DataTable dt1)
    {
        List<String> suggestions = new List<string>();
        try
        {
            DataRow[] dtSuggestions = dt1.Select();

            if (dt1.Rows.Count == 0)
            {
                string suggestion1 = AutoCompleteExtender.CreateAutoCompleteItem("Records not found!", "-1");
                suggestions.Add(suggestion1);
                return suggestions;
            }
            else
            {
                foreach (DataRow dr in dtSuggestions)
                {
                    string suggestion = AutoCompleteExtender.CreateAutoCompleteItem(dr["NAME"].ToString(), dr["ID"].ToString());
                    suggestions.Add(suggestion);
                }
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            return suggestions;
        }
        return suggestions;
    }

    public static List<String> GetSuggestions(DataTable dt1, bool isNeedEmptyRow)
    {
        List<String> suggestions = new List<string>();
        try
        {
            DataRow[] dtSuggestions = dt1.Select();

            if (dt1.Rows.Count == 0)
            {
                if (isNeedEmptyRow)
                {
                    string suggestion1 = AutoCompleteExtender.CreateAutoCompleteItem("Records not found!", "-1");
                    suggestions.Add(suggestion1);
                }
                return suggestions;
            }
            else
            {
                foreach (DataRow dr in dtSuggestions)
                {
                    string suggestion = AutoCompleteExtender.CreateAutoCompleteItem(dr["NAME"].ToString(), dr["ID"].ToString());
                    suggestions.Add(suggestion);
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            return suggestions;
        }
        return suggestions;
    }
    #endregion

}
public enum enumGridType
{
    TemplateGrid, BoundGrid
}

// Code added by Kali on 15-Sep-2010 for Sys Journal Enum Declaration

public enum enumTxnType
{
    Debit = 0,
    Credit = 1

}
public enum enumFileType
{
    Excel,
    Word,
    FlatFile

}
public enum enumJVStatus
{
    Active = 1,
    Cancelled = 2

}
public enum enumDim2
{
    AssetCode = 1,
    ProcessingFees = 2
}
//Code end


