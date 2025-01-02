
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;
using COLLATERAL = S3GBusEntity.Collateral;
using COLLATERALSERVICES = CollateralMgtServicesReference;
using System.IO;
using System.Globalization;
using System.Web.Services;
using System.ServiceModel;
using System.Text;

public partial class System_Admin_S3GSysAdminAuditTrial : ApplyThemeForProject
{
    #region [Common Variable declaration]
    DataTable dtAuditTrial;
    int intCompanyId, intUserId, intUserLevelId = 0;
    static int intCustomer = 0;

    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();

    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    string strProcName;

    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    string strAuditTrial = "AuditTrial";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strXMLAuditTrial;
    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }

    string strRedirectPage = "~/System Admin/S3GSysAdminAuditTrial.aspx";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminAuditTrial.aspx';";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KCP';";

    SerializationMode SerMode = SerializationMode.Binary;

    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    AuditTrial.S3G_SYSAD_AuditTrialDataTable objS3G_Sysad_AuditTrialDataTable = null;
    AuditTrial.S3G_SYSAD_AuditTrialRow objS3G_Sysad_AuditTrialDataRow;

    //Code end
    #endregion
    #region "Page Load"
    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = new S3GSession();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intUserLevelId = ObjUserInfo.ProUserLevelIdRW;

        #region Grid Paging Config
        ProPageNumRW = 1;                                                           // to set the default page number
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

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        if (!IsPostBack)
        {
            FunPriGetAuditTrialData();
            FunPriBindGridPaging();
        }
    }
    #endregion
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;              // To set the page Number
        ProPageSizeRW = intPageSize;            // To set the page size    
        FunPriBindGridPaging();
    }
    private void FunPriBindGridPaging()
    {
        int intTotalRecords = 0;
        FunPriBindDtAuditTableFromGridView();
        dtAuditTrial = (DataTable)ViewState[strAuditTrial];
        intTotalRecords = dtAuditTrial.Rows.Count;
        grvAuditTrial.DataSource = FunPriGetDataTablewithPaging(dtAuditTrial);
        grvAuditTrial.DataBind();
        //This is to hide first row if grid is empty
        if (((Label)grvAuditTrial.Rows[0].FindControl("lblSerialNo")).Text == "0")
        {
            grvAuditTrial.Rows[0].Visible = false;
        }
        ucCustomPaging.Visible = true;
        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
    }
    private DataTable FunPriGetDataTablewithPaging(DataTable dtTable)
    {
        int fromValue;
        int toValue;
        if (dtTable.Rows.Count == 1 && dtTable.Rows[0]["SLNO"].ToString() == "0")
        {
            return dtTable;
        }
        fromValue = (ProPageNumRW - 1) * ProPageSizeRW + 1;
        toValue = ProPageNumRW * ProPageSizeRW;
        dtTable.DefaultView.RowFilter = " SLNO >= " + fromValue + " AND SLNO <= " + toValue + "";
        return dtTable;
    }
    #region "Private Functions"
    private void FunPriInitialRow()
    {
        try
        {
            dtAuditTrial = new DataTable();
            DataRow dr;
            dtAuditTrial.Columns.Add("SLNO", typeof(System.Int32));
            dtAuditTrial.Columns.Add("ProgramID", typeof(System.Int32));
            dtAuditTrial.Columns.Add("ProgramCode", typeof(System.String));
            dtAuditTrial.Columns.Add("ProgramName", typeof(System.String));
            dtAuditTrial.Columns.Add("EditArchive", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("EditLog", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("DeleteArchive", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("DeleteLog", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("QueryArchive", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("QueryLog", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("IsActive", typeof(System.Boolean));
            dtAuditTrial.Columns.Add("Created_By", typeof(System.Int32));
            dtAuditTrial.Columns.Add("User_Level_ID", typeof(System.Int32));

            if (dtAuditTrial.Rows.Count == 0)
            {
                dr = dtAuditTrial.NewRow();
                dr["SLNO"] = "0";
                dr["ProgramCode"] = string.Empty;
                dr["ProgramName"] = string.Empty;
                dr["EditArchive"] = 0;
                dr["EditLog"] = 0;
                dr["DeleteArchive"] = 0;
                dr["DeleteLog"] = 0;
                dr["QueryArchive"] = 0;
                dr["QueryLog"] = 0;
                dr["IsActive"] = 0;
                dr["Created_By"] = 0;
                dr["User_Level_ID"] = 0;
                dtAuditTrial.Rows.Add(dr);
            }
            ViewState[strAuditTrial] = dtAuditTrial;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private void FunPriGetAuditTrialData()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", intCompanyId.ToString());


            dtAuditTrial = Utility.GetDefaultData("S3G_SYSAD_GetAuditTrialDetails", Procparam);
            if (dtAuditTrial.Rows.Count == 0)
                FunPriInitialRow();
            ViewState[strAuditTrial] = dtAuditTrial;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }

    }
    private void FunPriSaveAuditTrialMaster()
    {
        try
        {
            objS3G_Sysad_AuditTrialDataTable = new AuditTrial.S3G_SYSAD_AuditTrialDataTable();
            objS3G_Sysad_AuditTrialDataRow = objS3G_Sysad_AuditTrialDataTable.NewS3G_SYSAD_AuditTrialRow();
            objS3G_Sysad_AuditTrialDataRow.Company_ID = intCompanyId;
            objS3G_Sysad_AuditTrialDataRow.Created_By = intUserId;
            objS3G_Sysad_AuditTrialDataRow.Modified_By = intUserId;

            FunPriBindDtAuditTableFromGridView();

            if (dtAuditTrial.Rows.Count == 1 && dtAuditTrial.Rows[0]["ProgramName"].ToString() == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Please select atleast one record");
                return;
            }
            strXMLAuditTrial = Utility.FunPubFormXml(dtAuditTrial);
            objS3G_Sysad_AuditTrialDataRow.XmlAuditTrial = strXMLAuditTrial;

            objS3G_Sysad_AuditTrialDataTable.AddS3G_SYSAD_AuditTrialRow(objS3G_Sysad_AuditTrialDataRow);
            objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
            intErrCode = objS3GAdminServicesClient.FunPubCreateAuditTrial(SerMode, ClsPubSerialize.Serialize(objS3G_Sysad_AuditTrialDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                Utility.FunShowAlertMsg(this, "Audit Trial updated successfully");
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
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the  Audit Trial details");
                return;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            Utility.FunShowAlertMsg(this.Page, "Unable to save the Audit Trial details");
            cvAuditTrial.IsValid = false;
        }
        finally
        {
            if (objS3GAdminServicesClient != null)
                objS3GAdminServicesClient.Close();
        }
    }
    private void FunPriBindDtAuditTableFromGridView()
    {
        int intloop = 0;
        try
        {
            dtAuditTrial = (DataTable)ViewState[strAuditTrial];
            if (dtAuditTrial.Rows.Count > 0)
            {
                for (intloop = 0; intloop < grvAuditTrial.Rows.Count; intloop++)
                {
                    int innerLoop = Convert.ToInt32(((Label)grvAuditTrial.Rows[intloop].FindControl("lblSerialNo")).Text);
                    if (innerLoop != 0)
                    {
                        dtAuditTrial.Rows[innerLoop - 1]["EditArchive"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkEditArchive")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["EditLog"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkEditLog")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["DeleteArchive"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkDeleteArchive")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["DeleteLog"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkDeleteLog")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["QueryArchive"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkQueryArchive")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["QueryLog"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkQueryLog")).Checked;
                        dtAuditTrial.Rows[innerLoop - 1]["IsActive"] = ((CheckBox)grvAuditTrial.Rows[intloop].FindControl("chkIsActive")).Checked;
                    }
                }
            }
            dtAuditTrial.AcceptChanges();
            ViewState[strAuditTrial] = dtAuditTrial;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
    }
    private void FunGridviewRowcommand(GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                dtAuditTrial = (DataTable)ViewState[strAuditTrial];
                DropDownList ddlProgramCode = (DropDownList)grvAuditTrial.FooterRow.FindControl("ddlProgramCode");
                CheckBox chkEditArchive = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkEditArchive");
                CheckBox chkEditLog = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkEditLog");
                CheckBox chkDeleteArchive = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkDeleteArchive");
                CheckBox chkDeleteLog = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkDeleteLog");
                CheckBox chkQueryArchive = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkQueryArchive");
                CheckBox chkQueryLog = (CheckBox)grvAuditTrial.FooterRow.FindControl("chkQueryLog");

                if (dtAuditTrial.Rows.Count > 0)
                {
                    if (dtAuditTrial.Rows[0]["ProgramID"].ToString() == "" || dtAuditTrial.Rows[0]["ProgramName"].ToString() == "")
                    {
                        dtAuditTrial.Rows[0].Delete();
                        dtAuditTrial.AcceptChanges();
                    }
                }
                if (ddlProgramCode.SelectedValue == "0")
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Program Description");
                    return;
                }
                dr = dtAuditTrial.NewRow();
                dr["SLNO"] = dtAuditTrial.Rows.Count + 1;
                dr["ProgramID"] = ddlProgramCode.SelectedValue;
                dr["ProgramName"] = ddlProgramCode.SelectedItem.Text.Trim();
                dr["EditArchive"] = chkEditArchive.Checked ? 1 : 0;
                dr["EditLog"] = chkEditLog.Checked ? 1 : 0;
                dr["DeleteArchive"] = chkDeleteArchive.Checked ? 1 : 0;
                dr["DeleteLog"] = chkDeleteLog.Checked ? 1 : 0;
                dr["QueryArchive"] = chkQueryArchive.Checked ? 1 : 0;
                dr["QueryLog"] = chkQueryLog.Checked ? 1 : 0;
                dr["IsActive"] = 1;
                dr["Created_By"] = intUserId.ToString();
                dr["User_Level_ID"] = intUserLevelId.ToString();
                dtAuditTrial.Rows.Add(dr);

                ViewState[strAuditTrial] = dtAuditTrial;
                if (dtAuditTrial.Rows.Count % ProPageSizeRW == 0)
                    ProPageNumRW = (dtAuditTrial.Rows.Count / ProPageSizeRW);
                else
                    ProPageNumRW = (dtAuditTrial.Rows.Count / ProPageSizeRW) + 1;
                FunPriBindGridPaging();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    private static void AddMergedCells(GridViewRow objgridviewrow, TableCell objtablecell, int colspan, string celltext)
    {
        objtablecell = new TableCell();
        objtablecell.Text = celltext;
        objtablecell.ColumnSpan = colspan;

        objgridviewrow.Cells.Add(objtablecell);
    }
    #endregion
    #region "Button Events"
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveAuditTrialMaster();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
    #endregion
    #region "GridView Events"
    protected void grvAuditTrial_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FunGridviewRowcommand(e);
    }
    protected void grvAuditTrial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int intLoop = 0;
            DataTable dtDefaultGetProgramCodes = new DataTable();
            StringBuilder strprogramID = new StringBuilder();
            string strExpr;
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlProgramCode = e.Row.FindControl("ddlProgramCode") as DropDownList;
                FunPriBindDtAuditTableFromGridView();
                dtDefaultGetProgramCodes = Utility.GetDefaultData("S3G_SYSAD_GetProgramCode", Procparam);

                for (intLoop = 0; intLoop < dtAuditTrial.Rows.Count; intLoop++)
                {
                    if (dtAuditTrial.Rows[intLoop]["ProgramID"].ToString() != "")
                    {
                        strprogramID.Append("'");
                        strprogramID.Append(dtAuditTrial.Rows[intLoop]["ProgramID"].ToString());
                        strprogramID.Append("'");
                        strprogramID.Append(",");
                    }
                }

                if (strprogramID.ToString() != "")
                {
                    strExpr = " Program_ID NOT IN  (" + strprogramID.ToString().Substring(0, strprogramID.Length - 1) + ")";
                    dtDefaultGetProgramCodes.DefaultView.RowFilter = strExpr;
                }
                ddlProgramCode.DataValueField = "Program_ID";
                ddlProgramCode.DataTextField = "Program_Name";
                ddlProgramCode.DataSource = dtDefaultGetProgramCodes;
                ddlProgramCode.DataBind();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlProgramCode.Items.Insert(0, liSelect);
                Button btnAdd = (Button)e.Row.FindControl("btnAdd");
                if (ObjUserInfo.IsUserLevelUpdate(intUserId, intUserLevelId))
                {
                    btnAdd.Enabled = true;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                }
                else
                {
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)                // If data Row then check the data type and set the style - Alignment.
            {

                #region User Authorization
                Label lblUserID = (Label)e.Row.FindControl("lblUserID");
                Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");

                CheckBox chkEditArchive = (CheckBox)e.Row.FindControl("chkEditArchive");
                CheckBox chkEditLog = (CheckBox)e.Row.FindControl("chkEditLog");
                CheckBox chkDeleteArchive = (CheckBox)e.Row.FindControl("chkDeleteArchive");
                CheckBox chkDeleteLog = (CheckBox)e.Row.FindControl("chkDeleteLog");
                CheckBox chkQueryArchive = (CheckBox)e.Row.FindControl("chkQueryArchive");
                CheckBox chkQueryLog = (CheckBox)e.Row.FindControl("chkQueryLog");
                CheckBox chkIsActive = (CheckBox)e.Row.FindControl("chkIsActive");

                if (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text)))
                {
                    chkEditArchive.Enabled = true;
                    chkEditLog.Enabled = true;
                    chkDeleteArchive.Enabled = true;
                    chkDeleteLog.Enabled = true;
                    chkQueryArchive.Enabled = true;
                    chkQueryLog.Enabled = true;
                    chkIsActive.Enabled = true;
                   
                }
                else
                {
                    chkEditArchive.Enabled = false;
                    chkEditLog.Enabled = false;
                    chkDeleteArchive.Enabled = false;
                    chkDeleteLog.Enabled = false;
                    chkQueryArchive.Enabled = false;
                    chkQueryLog.Enabled = false;
                    chkIsActive.Enabled = false;
                }
                #endregion
            }
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }
    protected void grvAuditTrial_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            //Creating a gridview object            
            GridView objGridView = (GridView)sender;

            //Creating a gridview row object
            GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //Creating a table cell object
            TableCell objtablecell = new TableCell();

            AddMergedCells(objgridviewrow, objtablecell, 2, string.Empty);

            AddMergedCells(objgridviewrow, objtablecell, 2, " EDIT ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " DELETE ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " QUERY ");

            AddMergedCells(objgridviewrow, objtablecell, 1, string.Empty);

            objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);
        }


    }
    #endregion
    public bool GVBoolFormat(string val)
    {
        if (val != "")
            return Convert.ToBoolean(val);
        else
            return false;
    }

}
