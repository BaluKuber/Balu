/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : Origination 
/// Screen Name               : File Upload of PO/PI/VI
/// Created By                : Vinodha.M
/// Created Date              : 30-Aug-2014
/// Purpose                   : To Upload PO/PI/VI Documents And Proceed To Approval
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    : 

/// <Program Summary>

using System;
using System.Data;
using S3GBusEntity;
using System.Web.UI;
using System.ServiceModel;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity.Origination;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Data.Common;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Services.Description;
using System.Drawing;
using System.Text;
using Ionic.Zip;

public partial class Origination_S3G_ORG_UploadPoPiVi : ApplyThemeForProject
{
    string FileNameFormat = String.Empty;
    string SheetName = String.Empty;
    string filepath = String.Empty;
    string Extension = String.Empty;
    int intErrCode = 0, Flag = 0;
    static string strPageName = "File Import";
    string strAlert = "alert('__ALERT__');";
    string strKey = "Insert";
    public const Int32 ConstDelete = 5;
    public const Int32 ConstAction = 6;
    public string strDateFormat;
    public static Origination_S3G_ORG_UploadPoPiVi obj_Page;

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bDelelete = false;
    #endregion

    #region Paging Config

    int intUserID = 0;
    int intCompanyID = 0;
    string strUserName = "";
    UserInfo ObjUserInfo = null;
    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
    public int ProPageSizeRW
    {
        get;
        set;
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGridDetails();
    }
    #endregion

    //User Authorization    
    FileImportServiceReference.FileImportClient objFileImportClient = new FileImportServiceReference.FileImportClient();
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable objS3G_ORG_BankStatementTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable();
    S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadDataTable ObjS3G_ORG_FileUploadDataTable = new S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadDataTable();
    S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveDataTable ObjS3G_ORG_FileSaveDataTable = new S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveDataTable();

    Microsoft.Office.Interop.Excel.Application excel;
    Microsoft.Office.Interop.Excel.Workbook excelworkBook;
    Microsoft.Office.Interop.Excel.Worksheet excelSheet;
    Microsoft.Office.Interop.Excel.Range excelCellrange;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
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
            obj_Page = this;

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

            #region  User Authorization
            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bDelelete = ObjUserInfo.ProDeleteRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            strUserName = ObjUserInfo.ProUserNameRW;
            #endregion
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            ceFromDate.Format = ceToDate.Format = strDateFormat;
            CalendarExtender1.Format = strDateFormat;
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',false,  false);");
            CalendarExtender2.Format = strDateFormat;
            txtenddate.Attributes.Add("onblur", "fnDoDate(this,'" + txtenddate.ClientID + "','" + strDateFormat + "',false,  false);");

            
            if (!IsPostBack)
            {
                if (!bCreate)
                {
                    flUpload.Enabled = false;
                    btnUpload.Enabled = false;
                }
                lblFromDate.Visible = txtFromDate.Visible = lblToDate.Visible = txtToDate.Visible =
                lblInvoiceType.Visible = ddlInvoiceType.Visible = lblInvoiceNo.Visible = ddlInvoiceNo.Visible =
                imgFromDate.Visible = imgToDate.Visible = btnExport.Visible = false;
                FunPriGetActivityNameDetails();
                FunPriBindGridDetails();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    private void FunPriGetActivityNameDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@User_ID", intUserID.ToString());

            DataSet dsInitFileImportDetails = Utility.GetDataset("S3G_Get_ActivityName_List", dictParam);
            ddlActivityName.BindDataTable(dsInitFileImportDetails.Tables[0], new string[] { "Program_ID", "Program_Name" });
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void ddlActivityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCustomerName.Clear();
            ddlFunderName.Clear();

            lblFunder.Visible = false;
            ddlFunderName.Visible = false;
            //gvTrancheName.Visible = false;
            btnReceipt.Visible = false;

            if (ddlActivityName.SelectedValue != "519" && ddlActivityName.SelectedValue != "294" && ddlActivityName.SelectedValue != "22" && ddlActivityName.SelectedValue != "32" 
                && ddlActivityName.SelectedValue != "533" && ddlActivityName.SelectedValue != "542") //22-Asset Master,32-Vendor Master
            {
                lblCustomerName.Visible = true;
                ddlCustomerName.Visible = true;
                if (ddlActivityName.SelectedValue == "110")
                {
                    lblFunder.Visible = true;
                    ddlFunderName.Visible = true;
                    //gvTrancheName.Visible = true;
                    btnReceipt.Visible = true;


                }

            }
            else
            {
                lblCustomerName.Visible = false;
                ddlCustomerName.Visible = false;
            }
            lnkDownload.Enabled = false;

            if (Convert.ToInt32(ddlActivityName.SelectedValue.ToString()) == 298)
            {
                lblFromDate.Visible = txtFromDate.Visible = lblToDate.Visible = txtToDate.Visible =
                lblInvoiceType.Visible = ddlInvoiceType.Visible = lblInvoiceNo.Visible = ddlInvoiceNo.Visible =
                imgFromDate.Visible = imgToDate.Visible = btnExport.Visible = true;
            }
            else
            {
                lblFromDate.Visible = txtFromDate.Visible = lblToDate.Visible = txtToDate.Visible =
                lblInvoiceType.Visible = ddlInvoiceType.Visible = lblInvoiceNo.Visible = ddlInvoiceNo.Visible =
                imgFromDate.Visible = imgToDate.Visible = btnExport.Visible = false;
            }
            if (Convert.ToInt32(ddlActivityName.SelectedValue.ToString()) > 0)
            {
                GetTableColumnDetails(ddlActivityName.SelectedValue);
                if (ddlActivityName.SelectedValue == "535")
                {
                    lblCustomerName.Visible = false;
                    ddlCustomerName.Visible = false;
                }
                else
                {
                    lnkDownload.Enabled = true;
                }
            }
            FunPriBindGridDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }


    //Save Tranche Name from Popup Model Window

    protected void Edit(object sender, EventArgs e)
    {
        using (GridViewRow row = (GridViewRow)((CheckBox)sender).Parent.Parent)
        {


        }
    }





    protected void GetTableColumnDetails(string activitytype)
    {
        ViewState["TableName"] = String.Empty;
        ViewState["ColumnList"] = String.Empty;
        ViewState["TableName1"] = String.Empty;
        ViewState["ColumnList1"] = String.Empty;

        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();

            if(activitytype == "298")
                    dictParam.Add("@ActivityType_ID", "299");
            else
                dictParam.Add("@ActivityType_ID", activitytype.ToString());

            DataSet dsTableDetails = Utility.GetDataset("S3G_ORG_GetTableColumnNamesList", dictParam);
            ViewState["TableName"] = dsTableDetails.Tables[0].Rows[0]["TableName"].ToString();
            ViewState["ColumnList"] = dsTableDetails.Tables[0];
            Session["DocumentPath"] = dsTableDetails.Tables[1].Rows[0][0].ToString();

            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - Start */
            //if (ddlActivityName.SelectedValue == "131") // Asset Insurance
            //{
            //    ViewState["TableName"] = dsTableDetails.Tables[0].Rows[0]["TableName"].ToString();
            //    ViewState["ColumnList"] = dsTableDetails.Tables[0];
            //}
            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - End */
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            vsUserMgmt.Visible = true;
            if (Session["DocumentPath"] != null)
            {
                if (Session["DocumentPath"].ToString() != "NA")
                {
                    if (flUpload.HasFile)
                    {
                        Extension = Path.GetExtension(flUpload.FileName);
                        if (ddlActivityName.SelectedValue != "535" && (Extension == ".xls" || Extension == ".xlsx" || Extension == ".csv")) // Except Of Json FileUpload
                        {
                            if (ddlActivityName.SelectedValue != "0")
                            {
                                if (ddlActivityName.SelectedValue != "519" && ddlActivityName.SelectedValue != "542" && ddlActivityName.SelectedValue != "294" && ddlActivityName.SelectedValue != "22" && ddlActivityName.SelectedValue != "32" && ddlActivityName.SelectedValue != "533")
                                {
                                    if (ddlCustomerName.SelectedValue == "0")
                                    {
                                        Utility.FunShowAlertMsg(this.Page, "Select The Lessee Name");
                                        return;
                                    }
                                }
                            }
                            string[] filename = flUpload.FileName.Split('.');
                            FileNameFormat = filename[0] + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + filename[1];

                            filepath = Session["DocumentPath"].ToString();
                            if (!Directory.Exists(filepath))
                                Directory.CreateDirectory(filepath);

                            filepath = Session["DocumentPath"].ToString() + "\\" + FileNameFormat;
                            flUpload.SaveAs(filepath);
                            Session["DocumentPath"] = null;

                            FunPriFileUpload(FileNameFormat, filepath, Extension);
                            if (ddlActivityName.SelectedValue != "0")
                                ddlActivityName.ClearSelection();
                            if (ddlCustomerName.SelectedValue != "0")
                                ddlCustomerName.Clear();
                        }
                        else if (ddlActivityName.SelectedValue == "535")
                        {
                            string[] filename = flUpload.FileName.Split('.');
                            FileNameFormat = filename[0] + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + filename[1];

                            filepath = Session["DocumentPath"].ToString();
                            if (!Directory.Exists(filepath))
                                Directory.CreateDirectory(filepath);

                            filepath = Session["DocumentPath"].ToString() + "\\" + FileNameFormat;
                            flUpload.SaveAs(filepath);
                            Session["DocumentPathFinal"] = null;
                            UnzipFolder(filepath);
                            if (Flag == 1)
                            {
                                FunPriBindGridDetails();
                                if (intErrCode == 0)
                                {
                                    strAlert = strAlert.Replace("__ALERT__", "File uploaded successfully");
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                                }
                            }
                            else
                            {
                                strAlert = strAlert.Replace("__ALERT__", "File upload Failed");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                            }
                           
                            if (ddlActivityName.SelectedValue != "0")
                                ddlActivityName.ClearSelection();

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid File Format...');", true);
                        }
                    }
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Document Path is not Configured.Please Configure and Upload");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    private void FunPriFileUpload(string FileNameFormat, string filepath, string Extension)
    {
        try
        {
            ImportExcelData_To_DBTable(filepath, Extension, "Yes", "S1");

            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - Start */
            //if (ddlActivityName.SelectedValue == "131") // Only Asset Insurance
            //    ImportExcelData_To_DBTable(filepath, Extension, "Yes", "S2");
            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - End */

            if (Flag == 1)
            {
                FunPriBindGridDetails();
                if (intErrCode == 0)
                {
                    strAlert = strAlert.Replace("__ALERT__", "File uploaded successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                }
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "File upload Failed");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    protected int ImportExcelData_To_DBTable(string FilePath, string Extension, string isHDR, string Ins_Sheet)
    {
        try
        {
            string errormsg = String.Empty;
            string Sheet_Name = String.Empty;
            string DestTblName = String.Empty;

            DataTable dtXLData = new DataTable();
            DataTable dtValidateXLData = new DataTable();
            DataTable ObjDtColumnList = new DataTable();

            ObjDtColumnList = (DataTable)ViewState["ColumnList"];
            DestTblName = ViewState["TableName"].ToString();
            String sFileName = "";
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"]
                             .ConnectionString;
                    conStr = String.Format(conStr, FilePath, isHDR);
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"]
                              .ConnectionString;
                    conStr = String.Format(conStr, FilePath, isHDR);
                    break;
                //opc043 start
                case ".csv": //Excel 07
                    sFileName = System.IO.Path.GetDirectoryName(FilePath) + "\\" + "E_Inv_" + ObjUserInfo.ProUserIdRW +"_"+ DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".csv";
                    File.Copy(FilePath, sFileName);
                    conStr = ConfigurationManager.ConnectionStrings["Excel08ConString"]
                              .ConnectionString;
                    conStr = String.Format(conStr, System.IO.Path.GetDirectoryName(sFileName), isHDR);
                    break;
                    //opc043 end
            }

            //if (Extension.ToLower() != ".csv")
            //{
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();

            cmdExcel.Connection = connExcel;
            //opc043 start
            if (Extension.ToLower() == ".csv")
            {
                SheetName = System.IO.Path.GetFileName(sFileName);
            }
            //opc043 end
            else
            {
                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                SheetName = GetSheetName(dtExcelSchema);
                connExcel.Close();
            }

            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - Start */

            //if (ddlActivityName.SelectedValue == "131" && Ins_Sheet == "S1")
            //{
            //    SheetName = "1Insurance_Header$";
            //}
            //else if (ddlActivityName.SelectedValue == "131" && Ins_Sheet == "S2")
            //{
            //    Flag = 0;
            //    ObjDtColumnList = (DataTable)ViewState["ColumnList1"];
            //    DestTblName = ViewState["TableName1"].ToString();
            //    SheetName = "2Insurance_Details$";
            //}
            ///* Call ID - 4264 - OPC_CR_055 - 28-Oct-2016 - Thalai - End */
            
                //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "] WHERE [" + ObjDtColumnList.Rows[0][1].ToString() + "] IS NOT NULL";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtXLData);

            cmdExcel.CommandText = "SELECT * From [" + SheetName + "] WHERE [" + ObjDtColumnList.Rows[0][1].ToString() + "] IS NULL";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtValidateXLData);

            connExcel.Close();
            //opc043 start
            if (Extension.ToLower() == ".csv")
            {
                File.Delete(sFileName);
            }
            //cmdExcel.CommandText = "SELECT * From [" + sFileName + "]";
            //OleDbDataReader rdr = cmdExcel.ExecuteReader();
            //DataTable dt1 = new DataTable();
            //dt1.Load(rdr);
            //}
            //else
            //{
            //    FunPriLoadCSV(FilePath);
            //    dtXLData = (DataTable)Session["Terror_CSV"];
            //}
            //opc043 end
            if (dtValidateXLData.Rows.Count >= 0 && dtXLData.Rows.Count == 0)
            {
                lblErr.Text = "Please Correct The Validations";
                errormsg = ObjDtColumnList.Rows[0][1].ToString() + "  is Blank ;";
                lblErrorMessage.Text = errormsg;
                dtValidateXLData.Dispose();
                Flag = 2;
            }

            //dtXLData.AsEnumerable().ToList()
            // .ForEach(row =>
            // {
            //     var cellList = row.ItemArray.ToList();
            //     row.ItemArray = cellList.Select(x => x.ToString().Trim()).ToArray();
            // });

            if (Flag == 0)
                Flag = ValidateExcelColumnHeaderDetails(dtXLData, ObjDtColumnList, FilePath, DestTblName);

        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            if (objException.Message.Contains("No value given for one or more required parameters."))
            {
                lblErr.Text = "Please Correct The Validations";
                lblErrorMessage.Text = "Uploaded File Contains Empty Rows and Columns";
            }
            else
            {
                lblErr.Text = "Please Correct The Validations";
                //Check error log to get exact error message 
                lblErrorMessage.Text = "Uploaded File is in Invalid Format";
            }
        }
        return Flag;
    }

    private void FunPriLoadCSV(string strFileName)
    {
        try
        {
            string strFilePath = string.Empty;
            //strFilePath = ClsPubConfigReader.FunPubReadConfig("FA_TERRORIST_FILE_PATH");
            //string CSVFilePathName = strFilePath + "\\" + txtFileName.Text + ".csv";
            string CSVFilePathName = strFileName;
            string[] Lines = (File.ReadAllLines(CSVFilePathName, Encoding.UTF8));
            if (Lines == null)
            {
                Utility.FunShowAlertMsg(this, "Invalid File");
                return;
            }
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);
            DataTable dt = new DataTable();
            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
            {
                dt.Columns.Add(Fields[i].Replace(".", "#"), typeof(string));

                //dt.Columns.Add(Fields[i].Replace(" ", " "), typeof(string));

            }

            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)//Lines.GetLength(0)
            {
                Fields = Lines[i].Split(new char[] { ',' });
                Row = dt.NewRow();
                for (int f = 0; f < Cols; f++)
                    Row[f] = Fields[f];
                dt.Rows.Add(Row);
            }
            Session["Terror_CSV"] = dt;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //CVUsermanagement.ErrorMessage = ex.ToString();
            //CVUsermanagement.IsValid = false;
        }
    }

    //method added to get the valid sheet name
    protected string GetSheetName(DataTable dtExcelSchema)
    {
        foreach (DataRow row in dtExcelSchema.Rows)
        {
            if (!(row["TABLE_NAME"].ToString().Contains("FilterDatabase")
                || row["TABLE_NAME"].ToString().EndsWith("_")))
            {
                SheetName = row["TABLE_NAME"].ToString();
                return SheetName;
            }
        }
        return SheetName;
    }

    protected int ValidateExcelColumnHeaderDetails(DataTable XLData, DataTable DBXLcolumnHeaderNames, string FilePath, string DestTblName)
    {
        StringBuilder StrIncorrectColumn = new StringBuilder();
        StringBuilder StrColumnLength = new StringBuilder();
        if (XLData.Columns.Count >= DBXLcolumnHeaderNames.Select("Is_ExcelColumn = 1").Length)
        {

            foreach (DataRow ObjDR in DBXLcolumnHeaderNames.Select("Is_ExcelColumn = 1"))
            {
                string StrExcelColumn = "";
                StrExcelColumn = ObjDR["Excel_ColumnName"].ToString().Trim().ToUpper();
                if (!XLData.Columns.Contains(StrExcelColumn))
                {
                    if (StrIncorrectColumn.Length == 0)
                        StrIncorrectColumn.Append("Unmatched Column(s) : ");

                    StrIncorrectColumn.Append(ObjDR["Excel_ColumnName"].ToString() + "; <BR/>");
                }
                else
                {
                    int maxlength = 0;
                    int intColNo = XLData.Columns.IndexOf(StrExcelColumn);
                    XLData.Rows.OfType<DataRow>().ToList()
                        .ForEach(ss =>
                        {
                            maxlength = Convert.ToString(ss.ItemArray[intColNo]).Length > maxlength ?
                                Convert.ToString(ss.ItemArray[intColNo]).Length : maxlength;
                        });

                    if (!String.IsNullOrEmpty(ObjDR["Column_Length"].ToString()) && Convert.ToInt32(ObjDR["Column_Length"].ToString()) < maxlength)
                    {
                        if (StrColumnLength.Length == 0)
                            StrColumnLength.Append("Column Length Exceed : ");

                        StrColumnLength.Append(ObjDR["Excel_ColumnName"].ToString() +
                            "; Valid Length - " + ObjDR["Column_Length"].ToString() +
                            "; Available Length - " + maxlength + " <BR/>");
                    }
                }
            }
            if (StrIncorrectColumn.Length == 0 && StrColumnLength.Length == 0)
            {
                try
                {
                    SerializationMode SerMode = SerializationMode.Binary;
                    Int32 Upload_ID = 0;
                    S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadRow ObjFileUploadRow;
                    ObjFileUploadRow = ObjS3G_ORG_FileUploadDataTable.NewS3G_ORG_FileUploadRow();
                    ObjFileUploadRow.Program_ID = Convert.ToInt32(ddlActivityName.SelectedValue);
                    ObjFileUploadRow.File_Name = FileNameFormat;
                    ObjFileUploadRow.Customer_ID = Convert.ToInt32(ddlCustomerName.SelectedValue);
                    ObjFileUploadRow.Upload_path = filepath;
                    ObjFileUploadRow.Upload_by = Convert.ToString(intUserID);
                    ObjS3G_ORG_FileUploadDataTable.AddS3G_ORG_FileUploadRow(ObjFileUploadRow);

                    if (DestTblName != "S3G_Tmp_Asset_Insurance_Det")
                    {
                        intErrCode = objFileImportClient.FunPubCreateFileUpload(out Upload_ID,SerMode,
                            ClsPubSerialize.Serialize(ObjS3G_ORG_FileUploadDataTable, SerMode));
                        ViewState["AI_Upload_ID"] = Upload_ID.ToString();
                    }

                    hfuploadid.Value = Upload_ID.ToString();

                    /* Call ID - 4264 - OPC_CR_055 - 1-Nov-2016 - Chandru - Start */
                    if (Upload_ID == 0)
                        hfuploadid.Value = ViewState["AI_Upload_ID"].ToString();
                    /* Call ID - 4264 - OPC_CR_055 - 1-Nov-2016 - Chandru - End */

                    BindDataForCommonColumns(XLData);
                    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                    string constr = (string)AppReader.GetValue("DBPath", typeof(string));
                    SqlBulkCopy sbcd = new SqlBulkCopy(constr, SqlBulkCopyOptions.Default);

                    sbcd.DestinationTableName = DestTblName;

                    foreach (DataRow row in DBXLcolumnHeaderNames.Rows)
                    {
                        sbcd.ColumnMappings.Add(row["Excel_ColumnName"].ToString(), row["Db_ColumnName"].ToString());
                    }

                    sbcd.BatchSize = 1000;
                    sbcd.WriteToServer(XLData);
                    Flag = 1;
                }
                catch (Exception objException)
                {
                    ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);

                    }
                    //Dictionary<string, string> objParameters = new Dictionary<string, string>();
                    //objParameters.Add("@Upload_ID", hfuploadid.Value.ToString());
                    //objParameters.Add("@Program_ID", ddlActivityName.SelectedValue.ToString());
                    //DataTable Isdelete = Utility.GetDefaultData("S3G_ORG_DeleteUploadedFile", objParameters);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Template Invalid ...');", true);
                }
                lblErrorMessage.Text = "";
                lblErr.Text = "";
                XLData.Clear();
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                lblErr.Text = "File contains invalid column / data.";
                lblErrorMessage.Text = StrIncorrectColumn.ToString() + "<BR/>" + StrColumnLength.ToString();
            }
        }
        else
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Invalid file For Selected Template...');", true);
        }
        return Flag;
    }

    protected void BindDataForCommonColumns(DataTable XL)
    {
        try
        {
            Int32 count = 0;
            XL.Columns.Add(new DataColumn("Company_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("Upload_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("Status_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("USRID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("Customer_ID", typeof(Int32)));
            XL.Columns.Add(new DataColumn("SNO", typeof(Int32)));
            foreach (DataRow row in XL.Rows)
            {
                row["Company_ID"] = intCompanyID.ToString();
                row["Upload_ID"] = hfuploadid.Value;
                row["Status_ID"] = 1;
                row["USRID"] = intUserID.ToString();
                row["Customer_ID"] = ddlCustomerName.SelectedValue.ToString();
                row["SNO"] = count + 1;
                count = count + 1;
            }
            hfuploadid.Value = String.Empty;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    private void FunPriBindGridDetails()
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        try
        {
            if (ddlActivityName.SelectedValue != "0")
                Procparam.Add("@Program_ID", Convert.ToString(ddlActivityName.SelectedValue));
            else
                Procparam.Add("@Program_ID", Convert.ToString("0"));

            if (ddlCustomerName.SelectedValue != "0")
                Procparam.Add("@Customer_ID", Convert.ToString(ddlCustomerName.SelectedValue));
            else
                Procparam.Add("@Customer_ID", Convert.ToString("0"));

            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            grvUploadedDetails.BindGridView("S3G_ORG_GetUploadedFile_Page", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvUploadedDetails.Rows[0].Visible = false;
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    #region [Grid Event's]

    protected void grvUploadedDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //User Authorization
            Label cb = (Label)e.Row.FindControl("lblStatus_Id");
            Label lblProgram_ID = (Label)e.Row.FindControl("lblProgram_ID");
            if (cb.Text == "1")
            {
                ((LinkButton)e.Row.FindControl("btnSave")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnSave")).Attributes.Remove("href");
                ((LinkButton)e.Row.FindControl("btnException")).Enabled = false;
            }
            else if (cb.Text == "3")
            {
                ((LinkButton)e.Row.FindControl("btnSave")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnSave")).Attributes.Remove("href");
                ((LinkButton)e.Row.FindControl("btnDelete")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnDelete")).Attributes.Remove("href");
            }
            else if (cb.Text == "4")
            {
                ((LinkButton)e.Row.FindControl("btnSave")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnSave")).Attributes.Remove("href");
                ((LinkButton)e.Row.FindControl("btnDelete")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnDelete")).Attributes.Remove("href");
                ((LinkButton)e.Row.FindControl("btnException")).Enabled = false;
            }
            if (lblProgram_ID.Text.Trim() == "535")
            {
                ((LinkButton)e.Row.FindControl("btnDelete")).Enabled = false;
                ((LinkButton)e.Row.FindControl("btnDelete")).Attributes.Remove("href");
                ((LinkButton)e.Row.FindControl("btnException")).Enabled = false;
            }
            //Authorization code end
            if (!bCreate)
            {
                ((LinkButton)e.Row.FindControl("btnSave")).Enabled = false;
            }
            if (!bDelelete)
            {
                ((LinkButton)e.Row.FindControl("btnDelete")).Enabled = false;
            }
        }
    }


    //protected void grvGetTranchName_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (gvTrancheName == null)
    //        {
    //            Utility.FunShowAlertMsg(this, "No Tranche Name is found");
    //            return;
    //        }
    //        Int32 intChkCnt = 0;

    //        for (Int32 i = 0; i < gvTrancheName.Rows.Count; i++)
    //        {
    //            CheckBox cbxSelectRS = (CheckBox)gvTrancheName.Rows[i].FindControl("cbxSelectRS");
    //            if (cbxSelectRS.Checked == true)
    //                intChkCnt++;
    //        }

    //        if (intChkCnt == 0)
    //        {
    //            Utility.FunShowAlertMsg(this, "Select atleast One Tranche Name");
    //            return;
    //        }
    //        BindTranchName();
    //    }
    //    catch (Exception objException)
    //    {
    //        throw objException;
    //    }

    //}












    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            btnReceipt.Visible = false;
            gvTrancheName.DataSource = null;
            gvTrancheName.DataBind();
            ddlFunderName.SelectedText = "";
            ddlFunderName.SelectedValue = "0";


            ddlActivityName.SelectedIndex = ddlInvoiceType.SelectedIndex = 0;
            txtFromDate.Text = txtToDate.Text = "";
            ddlInvoiceNo.Clear();

            ddlCustomerName.Clear();
            lblErr.Text = lblErrorMessage.Text = String.Empty;
            lblCustomerName.Visible = false;
            ddlCustomerName.Visible = false;
            lnkDownload.Enabled = false;
            FunPriBindGridDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriBindGridDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void btnException_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (sender as LinkButton);
            GridViewRow row = (lnkView.NamingContainer as GridViewRow);
            string Upload_ID = (row.FindControl("lblUpload_ID") as Label).Text;
            string Program_ID = (row.FindControl("lblProgram_ID") as Label).Text;
            string Program_Name = (row.FindControl("lblProgram_Name") as Label).Text;
            FunGetUploadedFilesByUpload_ID(Upload_ID, Program_ID, Program_Name);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string strPath = "";
            if (ddlActivityName.SelectedValue != "0" && ddlActivityName.SelectedValue != "110")
            {
                switch (ddlActivityName.SelectedValue)
                {
                    case "519":
                        strPath = Server.MapPath(@"Upload Format\HSN Upload Template.xls");
                        break;
                    case "294":
                        strPath = Server.MapPath(@"Upload Format\RS_Charge_Maintenance_Creation.xls");
                        break;
                    case "542":
                        strPath = Server.MapPath(@"Upload Format\RS_Charge_Maintenance_Releas.xls");
                        break;
                    case "22":
                        strPath = Server.MapPath(@"Upload Format\Asset Upload Format.xls");
                        break;
                    case "23":
                        strPath = Server.MapPath(@"Upload Format\Extension Rental Format.xls");
                        break;
                    case "32":
                        strPath = Server.MapPath(@"Upload Format\Vendor Upload Format.xls");
                        break;
                    case "278":
                        strPath = Server.MapPath(@"Upload Format\Po Upload Format.xls");
                        break;
                    case "279":
                        strPath = Server.MapPath(@"Upload Format\PI Upload Format.xls");
                        break;
                    case "280":
                        strPath = Server.MapPath(@"Upload Format\VI Upload Format.xls");
                        break;
                    case "131":
                        strPath = Server.MapPath(@"Upload Format\Asset Insurance Format.xls");
                        break;
                    case "298":
                        strPath = Server.MapPath(@"Upload Format\Debit Credit Note Format.xls");
                        break;
                    case "72":
                        strPath = Server.MapPath(@"Upload Format\Over Due interest Format.xls");
                        break;
                    case "533":
                        strPath = Server.MapPath(@"Upload Format\EInvoice.xls");
                        break;
                    case "538":
                        strPath = Server.MapPath(@"Upload Format\Master_PO_Upload_Format.xls");
                        break;
                    case "540":
                        strPath = Server.MapPath(@"Upload Format\Additional_Rebate_Upload.xls");
                        break;
                    case "546":
                        strPath = Server.MapPath(@"Upload Format\PO Reference Number Upload Template.xls");
                        break;
                }
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=FileUpload.xls");
                Response.TransmitFile(strPath);
                Response.End();
            }
            // Created By : Anbuvel.T , Date : 13-SEP-2016, Description : Bulk Receipt Upload
            else if (ddlActivityName.SelectedValue == "110")
            {
                if (!(ddlCustomerName.SelectedValue != "0"))
                {
                    strPath = Server.MapPath(@"Upload Format\Receipt Upload Format.xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=FileUpload.xls");
                    Response.TransmitFile(strPath);
                    Response.End();
                }
                Dictionary<string, string> objParameters = new Dictionary<string, string>();
                //if (ddlBranchList.SelectedValue != "0")
                //    objParameters.Add("@Location_ID", ddlBranchList.SelectedValue.ToString());
                if (ddlCustomerName.SelectedValue != "0")
                    objParameters.Add("@CustomerID", ddlCustomerName.SelectedValue.ToString());
                objParameters.Add("@CompanyId", intCompanyID.ToString());

                if (ViewState["TranchName"] != null)
                {

                    objParameters.Add("@XML_TrancheDtls", Utility.FunPubFormXml((DataTable)ViewState["TranchName"], true));

                }
                DataSet dsReceipt = Utility.GetDataset("S3G_CLN_GetPendingInstallments", objParameters);
                ViewState["dsReceipt"] = dsReceipt.Tables[0];
                ExportToExcel(dsReceipt.Tables[0], ddlCustomerName.SelectedValue, ddlCustomerName.SelectedValue, "Receipt and Collection Update");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Choose Activity Name ...');", true);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    //added for CR_OPCSL_052 on july 14,2016 by Swarna S
    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderApprover.Hide();
            FunPriSaveDetails(sender, e);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    //added for CR_OPCSL_052 on july 14,2016 by Swarna S
    protected void btnModalCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderApprover.Hide();
            btnClear_Click(null, null);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //added for CR_OPCSL_052 on july 14,2016 by Swarna S
        LinkButton lnkView = (sender as LinkButton);
        GridViewRow row = (lnkView.NamingContainer as GridViewRow);
        string Upload_ID = (row.FindControl("lblUpload_ID") as Label).Text;
        string Program_ID = (row.FindControl("lblProgram_ID") as Label).Text;
        ViewState["Upload_ID"] = Upload_ID;
        ViewState["Program_ID"] = Program_ID;
        Dictionary<string, string> objParameters = new Dictionary<string, string>();
        objParameters.Add("@Upload_ID", Upload_ID.ToString());
        objParameters.Add("@Program_ID", Program_ID.ToString());
        objParameters.Add("@User_ID", intUserID.ToString());
        objParameters.Add("@Company_ID", intCompanyID.ToString());
        DataSet dsUploadedeException = Utility.GetDataset("S3G_ORG_GetFileUploadException", objParameters);

        if (dsUploadedeException.Tables[0].Rows[0][0].ToString() != "0")
        {
            ModalPopupExtenderApprover.Show();
            return;
        }
        else
            FunPriSaveDetails(sender, e);
    }

    protected void FunPriSaveDetails(object sender, EventArgs e)
    {
        SerializationMode SerMode = SerializationMode.Binary;
        try
        {
            string LSQ_number = "";
            string Error_Msg = "";

            S3GBusEntity.Origination.FileImport.S3G_ORG_FileSaveRow ObjFileSaveRow;
            ObjFileSaveRow = ObjS3G_ORG_FileSaveDataTable.NewS3G_ORG_FileSaveRow();
            ObjFileSaveRow.Upload_ID = Convert.ToInt32(ViewState["Upload_ID"].ToString());
            ObjFileSaveRow.Program_ID = Convert.ToInt32(ViewState["Program_ID"].ToString());
            ObjFileSaveRow.User_ID = intUserID;
            ObjFileSaveRow.Company_ID = intCompanyID;
            ObjS3G_ORG_FileSaveDataTable.AddS3G_ORG_FileSaveRow(ObjFileSaveRow);

            intErrCode = objFileImportClient.FunPubSaveFileUpload(out LSQ_number, out Error_Msg,SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_FileSaveDataTable, SerMode));
            if (intErrCode == 0)
            {
                if (LSQ_number.Trim() == "")
                    strAlert = strAlert.Replace("__ALERT__", "File Saved successfully");
                else if (LSQ_number.Trim() == "-1")
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                else if (LSQ_number.Trim() == "-2")
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                else
                    strAlert = strAlert.Replace("__ALERT__", "File Saved successfully; Load Sequence Number - " + LSQ_number.Trim());
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 5)
            {
                strAlert = strAlert.Replace("__ALERT__", "No Valid Records To Save.Please Correct The Exceptions");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 1)
            {
                strAlert = strAlert.Replace("__ALERT__", "MRA not created for the specific customer");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 2)
            {
                strAlert = strAlert.Replace("__ALERT__", "MRA not approved for the specific customer");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 3)
            {
                strAlert = strAlert.Replace("__ALERT__", "Specific Record Already Saved");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else if (intErrCode == 53)//Error In Posting
            {
                strAlert = strAlert.Replace("__ALERT__", Error_Msg);
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Due to Data problem,Unable to save");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
            FunPriBindGridDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkView = (sender as LinkButton);
            GridViewRow row = (lnkView.NamingContainer as GridViewRow);
            string Upload_ID = (row.FindControl("lblUpload_ID") as Label).Text;
            string Program_ID = (row.FindControl("lblProgram_ID") as Label).Text;
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@Upload_ID", Upload_ID.ToString());
            objParameters.Add("@Program_ID", Program_ID.ToString());
            DataSet dsUploadedFileDetails = Utility.GetDataset("S3G_ORG_DeleteUploadedFile", objParameters);
            FunPriBindGridDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlActivityName.SelectedIndex > 0)
            {
                FunPriBindGridDetails();
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select Activity Name");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomerName.SelectedValue != "0")
            {
                Dictionary<string, string> objParameters = new Dictionary<string, string>();
                objParameters.Add("@Company_Id", intCompanyID.ToString());
                objParameters.Add("@Customer_Id", ddlCustomerName.SelectedValue);
                objParameters.Add("@FromDate", Utility.StringToDate(txtFromDate.Text).ToString());
                objParameters.Add("@ToDate", Utility.StringToDate(txtToDate.Text).ToString());
                if (ddlInvoiceNo.SelectedValue != "0")
                    objParameters.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);
                objParameters.Add("@Invoice_Type", ddlInvoiceType.SelectedValue);

                DataTable dtUploadedFileDetails = Utility.GetDefaultData("S3G_ORG_GetCrNote_Export", objParameters);

                if (dtUploadedFileDetails.Rows.Count > 0)
                {
                    ExportToExcel(dtUploadedFileDetails, strUserName, "298", "CrNote");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Recors Not Found");
                    return;
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select Lessee Name");
                return;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetCustomerNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        //suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", Procparam), false);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", Procparam));
        return suggetions.ToArray();
    }

    public void BindTranchName(Int32 FunderID)
    {
        try
        {

            MPEReceipt.Show();
            pnlReceipt.Visible = true;

            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            procparam.Add("@User_ID", Convert.ToString(intUserID));
            procparam.Add("@FunderID", Convert.ToString(FunderID));
            procparam.Add("@Customer_ID", ddlCustomerName.SelectedValue.ToString());

            if (txtStartDate.Text != "")
                procparam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            if (txtenddate.Text != "")
                procparam.Add("@Enddate", Utility.StringToDate(txtenddate.Text).ToString());

            DataSet ds = Utility.GetDataset("S3G_ORG_FU_GetTranchName", procparam);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = (DataTable)ViewState["TranchName"];

                gvTrancheName.DataSource = ds.Tables[0];
                gvTrancheName.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                gvTrancheName.EmptyDataText = "No Record Found";
                gvTrancheName.DataBind();
            }

            ViewState["TranchName"] = ds.Tables[0];
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }



    [System.Web.Services.WebMethod]
    public static string[] GetFunderName(String prefixText, int count)
 {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Customer_ID", obj_Page.ddlCustomerName.SelectedValue.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetFunderName", Procparam));



        //MPEReceipt.Show();
        //gvTrancheName.Visible = true;
        //pnlReceipt.Visible = true;


        return suggetions.ToArray();
    }


    protected void FunGetUploadedFilesByUpload_ID(string Upload_ID, string Program_ID, string Program_Name)
    {
        try
        {
            Dictionary<string, string> objParameters = new Dictionary<string, string>();
            objParameters.Add("@Upload_ID", Upload_ID.ToString());
            objParameters.Add("@Program_ID", Program_ID.ToString());

            DataSet dsUploadedFileDetails = Utility.GetDataset("S3G_ORG_GetUploadedFileDetails", objParameters);
            ViewState["ExportDataExcel"] = dsUploadedFileDetails.Tables[0];

            ExportToExcel(dsUploadedFileDetails.Tables[0], Upload_ID, Program_ID, Program_Name);
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    public void ExportToExcel(DataTable dtExportData, string Upload_ID, string Program_ID, string Program_Name)
    {
        try
        {
            if (dtExportData.Rows.Count > 0)
            {
                string filename = Program_Name + "_" + Upload_ID + ".xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                GridView dgGrid = new GridView();
                dgGrid.ForeColor = System.Drawing.Color.DarkBlue;
                dgGrid.Font.Name = "calibri";
                dgGrid.Font.Size = 10;
                dgGrid.DataSource = dtExportData;
                dgGrid.DataBind();
                dgGrid.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-weight: bold;");
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.            
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
        finally
        {
            objFileImportClient.Close();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void btnReceipt_Click(object sender, EventArgs e)
    {
        if (ddlCustomerName.SelectedValue == "0" || ddlCustomerName.SelectedText == "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select Lessee Name First')", true);
        }
        else
        {

            MPEReceipt.Show();
            pnlReceipt.Visible = true;
        }

    }
    protected void btnRcptModalClose_Click(object sender, EventArgs e)
    {
        MPEReceipt.Hide();
        pnlReceipt.Visible = false;

        DataTable dt = (DataTable)ViewState["TranchName"];

        foreach (GridViewRow row in gvTrancheName.Rows)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkgdtrnSelect");
            Label LblName = (Label)row.FindControl("lblgdtrnTrancheID");


            //if (chkSelect.Checked == true)
            //{
                DataRow dr = dt.Rows[row.RowIndex];
                dr["Is_Checked"] = (chkSelect.Checked == true) ? 1 : 0;
                dt.AcceptChanges();

            //}
        }

        ViewState["TranchName"] = dt;
    }

    protected void Showtranch_Click(object sender, EventArgs e)
    {

       //int count= Select_Funder_ID(ddlFunderName.SelectedText);

        string test = ddlFunderName.SelectedText;

        if ((ddlFunderName.SelectedValue != null) || ddlFunderName.SelectedText=="")
       {
              if (ddlFunderName.SelectedValue=="" || ddlFunderName.SelectedText=="")
               {
                   ddlFunderName.SelectedValue = "0";
               }

               BindTranchName(Convert.ToInt32(ddlFunderName.SelectedValue));
           
       }
       else
       {
           ddlFunderName.SelectedValue = "0";
           ddlFunderName.SelectedText = "";
           gvTrancheName.DataSource = null;
           gvTrancheName.DataBind();
        
       }
        
    }



    //public int Select_Funder_ID(string text)
    //{

    //    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //    List<String> suggetions = new List<String>();
    //    DataTable dtCommon = new DataTable();
    //    DataSet Ds = new DataSet();

    //    Procparam.Clear();
    //    Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
    //    Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
    //    Procparam.Add("@Customer_ID", obj_Page.ddlCustomerName.SelectedValue.ToString());
    //    Procparam.Add("@PrefixText", text);
    //    suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetFunderID", Procparam), false);
    //    int count=suggetions.Count();
    //    return count;
        
    //}

    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtenddate.Text = String.Empty;
        }
        catch (Exception ex)
        {
            //lblErrorMessage.Text = ex.Message;
        }
    }


    protected void txtDateTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckDate();
        }
        catch (Exception ex)
        {

        }
    }

    private void CheckDate()
    {
        if (txtStartDate.Text != String.Empty && txtenddate.Text != String.Empty)
        {
            int intErrCount = 0;
            intErrCount = Utility.CompareDates(txtStartDate.Text, txtenddate.Text);
            if (intErrCount == -1)
            {
                txtenddate.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date ");
                txtenddate.Focus();
                return;
            }
        }
    }



    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }

    protected void ddlCustomerName_Item_Selected(object Sender, EventArgs e)
    {
        ddlFunderName.SelectedValue = "";
        gvTrancheName.DataSource = null;
        gvTrancheName.DataBind();

        if (ddlActivityName.SelectedValue == "278" || ddlActivityName.SelectedValue == "538")
        {
            Dictionary<string, string> procparam = new Dictionary<string, string>();
            procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            procparam.Add("@User_ID", Convert.ToString(intUserID));
            procparam.Add("@Customer_ID", ddlCustomerName.SelectedValue.ToString());
            DataTable dt = new DataTable();
            dt = Utility.GetDefaultData("S3G_ORG_GetIs_POBlocked", procparam);

            if (dt.Rows[0]["Is_PO_Black"].ToString() == "1")
            {
                Utility.FunShowAlertMsg(this.Page, "PO Blocked, Unable to Proceed!.");
                ddlCustomerName.Clear();
            }
        }
    }
    private void UnzipFolder(string strFilePath)
    {
        string filepath = string.Empty;
        StringBuilder strbXml = new StringBuilder();
        GridView grd = new GridView();
        string strInvoiceNumber = string.Empty;
        try
        {
            string path = strFilePath.Trim(); //Server.MapPath("~/SampleFiles.zip");
            using (ZipFile zip = ZipFile.Read(path))
            {
                filepath = Session["DocumentPath"].ToString();
                filepath = Session["DocumentPath"].ToString() + "\\ExtractFiles\\";
                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(filepath);

                zip.ExtractAll(Server.MapPath("~/Files/"), ExtractExistingFileAction.DoNotOverwrite);
                grvFiles.DataSource = zip;
                grvFiles.DataBind();
                if (grvFiles.Rows.Count > 0)
                {
                    strbXml.Append("<Root>");
                    for (int rowcoun = 0; rowcoun <= grvFiles.Rows.Count - 1; rowcoun++)
                    {
                        strInvoiceNumber = string.Empty;
                        string[] splivalues;
                        string[] SplitFilename;
                        string[] splitInvalues;
                        
                        string strFilename = string.Empty;
                        Label lblZFileName = (Label)grvFiles.Rows[rowcoun].FindControl("lblZFileName");
                        splivalues=lblZFileName.Text.Trim().Split('/');
                        if (splivalues.Count() > 1)
                        {
                            strFilename = splivalues[1].Trim();
                        }
                        else
                        {
                            strFilename = splivalues[0].Trim();
                        }


                        if (strFilename.Trim() != string.Empty)
                        {
                            SplitFilename = strFilename.Trim().Split('.');
                            splitInvalues=SplitFilename[0].Split('_');
                            strInvoiceNumber = splitInvalues[2].Trim();
                        }

                        Label lblZUncompressedSize = (Label)grvFiles.Rows[rowcoun].FindControl("lblZUncompressedSize");

                        strbXml.Append(" <Details File_Name='" + strFilename.Trim() + "' Invoice_Number='" + strInvoiceNumber + "' File_Size='" + lblZUncompressedSize.Text.Trim() + "' /> ");

                        
                    }
                    strbXml.Append("</Root>");
                }
                ViewState["FilePath"] = strbXml.ToString();
                //lbltxt.Text = "Files Extracted Successfully and it contains following files";
            }
            ValidateJSONHeaderDetails(strFilePath);
            FunPriBindGridDetails();
            if (intErrCode == 0)
            {
                strAlert = strAlert.Replace("__ALERT__", "File uploaded successfully");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }
        }
        catch
        {

        }
    }

    protected int ValidateJSONHeaderDetails(string FilePath)
    {
        StringBuilder StrIncorrectColumn = new StringBuilder();
        StringBuilder StrColumnLength = new StringBuilder();
        try
        {
            SerializationMode SerMode = SerializationMode.Binary;
            Int32 Upload_ID = 0;
            S3GBusEntity.Origination.FileImport.S3G_ORG_FileUploadRow ObjFileUploadRow;
            ObjFileUploadRow = ObjS3G_ORG_FileUploadDataTable.NewS3G_ORG_FileUploadRow();
            ObjFileUploadRow.Program_ID = Convert.ToInt32(ddlActivityName.SelectedValue);
            ObjFileUploadRow.File_Name = FileNameFormat;
            ObjFileUploadRow.Customer_ID = Convert.ToInt32(ddlCustomerName.SelectedValue);
            ObjFileUploadRow.Upload_path = filepath;
            ObjFileUploadRow.Upload_by = Convert.ToString(intUserID);
            //if (ViewState["FilePath"] != null)
            //{
            //    ObjFileUploadRow.XML_FileList = Convert.ToString(ViewState["FilePath"]);
            //}
            ObjS3G_ORG_FileUploadDataTable.AddS3G_ORG_FileUploadRow(ObjFileUploadRow);
            intErrCode = objFileImportClient.FunPubCreateFileUpload(out Upload_ID, SerMode,
                    ClsPubSerialize.Serialize(ObjS3G_ORG_FileUploadDataTable, SerMode));
            ViewState["AI_Upload_ID"] = Upload_ID.ToString();
            hfuploadid.Value = Upload_ID.ToString();
            if (Upload_ID == 0)
                hfuploadid.Value = ViewState["AI_Upload_ID"].ToString();
            Flag = 1;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Template Invalid ...');", true);
        }
        lblErrorMessage.Text = "";
        lblErr.Text = "";
        return Flag;
    }
}