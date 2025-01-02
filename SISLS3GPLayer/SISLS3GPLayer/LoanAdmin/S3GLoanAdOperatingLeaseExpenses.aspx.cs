
//Module Name      :   Loan Admin
//Screen Name      :   S3GLoanAdOperatingLeaseExpenses.aspx
//Created By       :   Irsathameen K
//Created Date     :   31-Aug-2010
//Modified By      :   Irsathameen
//Modified Date    :   1-Feb-2011
//Purpose          :   Bug fixation in OLE (Round 1 )
//Modified By      :   Shibu
//Modified Date    :   01-Oct-2013
//Purpose          :   Performance Tuning DDL Controls to Auto Suggest 

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

#endregion

public partial class LoanAdmin_S3GLoanAdOperatingLeaseExpenses : ApplyThemeForProject
{
    #region Declaration

    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjOperatingLeaseExpensesClient;
    LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsDataTable ObjS3G_LOANAD_OperatingLeaseExpensesDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    string strOLEID;
    int intErrCode = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    int intRowId = 0;
    string strOLENo, s;
    static string strPageName = "Operating Lease Expenses";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    DataTable dtApprovals = new DataTable();
    Dictionary<string, string> dictParam = null;
    string strXMLOLEDet = null;
    string strRedirectPage = "../LoanAdmin/S3GLoanAdTransLander.aspx?Code=OLE";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strProcName = string.Empty;
    string strDateFormat = string.Empty;
    public static LoanAdmin_S3GLoanAdOperatingLeaseExpenses obj_Page;
  

    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdOperatingLeaseExpenses.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdTransLander.aspx?Code=OLE';";
    DataTable dtOLE = new DataTable();

    #endregion

    #region Page_Loading

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        { FunPriLoadPage(); }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load Operating Lease Expenses";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Gridview_Events

    protected void GRVOLE_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                txtAmount.SetDecimalPrefixSuffix(10, 2, true, false, "Amount");
                TextBox txtDesc = (TextBox)e.Row.FindControl("txtDesc");
                txtDesc.Text = txtDesc.Text.Trim();
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Bind the values in Gridview";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void GRVOLE_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["OLEDetails"];
            dtDelete.Rows.RemoveAt(e.RowIndex);
            GRVOLE.DataSource = dtDelete;
            GRVOLE.DataBind();
            ViewState["OLEDetails"] = dtDelete;
            if (dtDelete.Rows.Count == 0)
            { GetGridviewvalues(); }
            FunPriBindGLAcc();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Delete the Rows from Gridview";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void GRVOLE_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            DropDownList ddlGLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlGLAcc");
            DropDownList ddlSLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlSLAcc");
            TextBox txtDesc = (TextBox)GRVOLE.FooterRow.FindControl("txtDesc");
            TextBox txtAmount = (TextBox)GRVOLE.FooterRow.FindControl("txtAmount");
            TextBox txtBalCreGL = (TextBox)GRVOLE.FooterRow.FindControl("txtBalCreGL");
            //TextBox txtBalCreSL = (TextBox)GRVOLE.FooterRow.FindControl("txtBalCreSL");
            DropDownList ddlBalCreSL = (DropDownList)GRVOLE.FooterRow.FindControl("ddlBalCreSL");

            Button btnAdd = (Button)GRVOLE.FooterRow.FindControl("btnAdd");
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                dtOLE = (DataTable)ViewState["OLEDetails"];
                if (ddlGLAcc.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select a GL account');", true);
                    ddlGLAcc.Focus();
                    return;
                }
                if (ddlSLAcc.Items.Count > 1)
                {
                    if (ddlSLAcc.SelectedValue == "0")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select a SL account');", true);
                        ddlSLAcc.Focus();
                        return;
                    }
                }
                if (txtDesc.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the description');", true);
                    txtDesc.Focus();
                    return;
                }
                if (txtAmount.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the amount');", true);
                    txtAmount.Focus();
                    return;
                }
                if (btnAdd.Text == "Add")
                {
                    if (dtOLE.Rows[0]["GLACC"].ToString() == "")
                    { dtOLE.Rows[0].Delete(); }
                    dr = dtOLE.NewRow();
                    //dr["GLAcc"] = ddlGLAcc.SelectedValue;
                    dr["GLAcc"] = ddlGLAcc.SelectedItem.Text;
                    dr["SLAcc"] = ddlSLAcc.SelectedValue == "0" ? string.Empty : ddlSLAcc.SelectedValue;
                    dr["Desc"] = txtDesc.Text;
                    dr["Amount"] = txtAmount.Text;
                    dr["BalCreGL"] = txtBalCreGL.Text;
                    //dr["BalCreSL"] = txtBalCreSL.Text;
                    if (ddlBalCreSL.SelectedValue != "0")
                    {
                        dr["BalCreSL"] = ddlBalCreSL.SelectedItem.Text;
                    }
                    dtOLE.Rows.Add(dr);
                }
                if (btnAdd.Text == "Update")
                {
                    intRowId = Convert.ToInt32(hdnRowID.Value);
                    //dtOLE.Rows[intRowId]["GLAcc"] = ddlGLAcc.SelectedValue;
                    dtOLE.Rows[intRowId]["GLAcc"] = ddlGLAcc.SelectedItem.Text; 
                    dtOLE.Rows[intRowId]["SLAcc"] = ddlSLAcc.SelectedValue;
                    dtOLE.Rows[intRowId]["Desc"] = txtDesc.Text;
                    dtOLE.Rows[intRowId]["Amount"] = txtAmount.Text;
                    dtOLE.Rows[intRowId]["BalCreGL"] = txtBalCreGL.Text;
                    //dtOLE.Rows[intRowId]["BalCreSL"] = txtBalCreSL.Text;
                    dtOLE.Rows[intRowId]["BalCreSL"] = ddlBalCreSL.SelectedItem.Text;
                    btnAdd.Text = "Add";
                }
                GRVOLE.DataSource = dtOLE;
                GRVOLE.DataBind();
                ViewState["OLEDetails"] = dtOLE;
                FunPriBindGLAcc();
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Add the Rows in Gridview";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region Save_Clear_Cancel_Go

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            strOLENo = "";
            dtOLE = (DataTable)ViewState["OLEDetails"];
            if (dtOLE.Rows[0]["GLACC"].ToString() == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one A/c Level Information");
                return;
            }
            //if (Utility.StringToDate(txtOLEDate.Text) < Utility.StringToDate(txtBillRefDate.Text))
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Bill reference date should be less than or equal to OLE date");
            //    return;
            //}
            if (Utility.StringToDate(hidAssetAcquisitionDate.Value) > Utility.StringToDate(txtOLEDate.Text))
            {
                Utility.FunShowAlertMsg(this.Page, "OLE date should not be greater than the lease asset acquisition date");
                return;
            }
            ObjS3G_LOANAD_OperatingLeaseExpensesDataTable = new LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsDataTable();
            LoanAdminAccMgtServices.S3G_LOANAD_OperatingLeaseExpensesDetailsRow ObjOLERow;
            ObjOLERow = ObjS3G_LOANAD_OperatingLeaseExpensesDataTable.NewS3G_LOANAD_OperatingLeaseExpensesDetailsRow();
            ObjOLERow.Company_ID = intCompanyID;
            ObjOLERow.Operation_Lease_Expenses_Date = Utility.StringToDate(txtOLEDate.Text);
            //ObjOLERow.SJV_No=Convert.ToString(txtSystemJVNo.Text);
            if (txtsystemjvdate.Text != string.Empty)
            { ObjOLERow.SJV_Date = Utility.StringToDate(txtsystemjvdate.Text.Trim()); }
            ObjOLERow.LOB_ID = Convert.ToInt32(hidLOBID.Value.Trim());
            ObjOLERow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjOLERow.Lease_Asset_No = Convert.ToString(ddlLeaseAssetNo.SelectedValue.Split('-')[0].ToString().Trim());
            ObjOLERow.Bill_No = Convert.ToString(txtBillrefno.Text.Trim());
            ObjOLERow.Bill_Date = Utility.StringToDate(txtBillRefDate.Text.Trim());
            ObjOLERow.Debit_Type = Convert.ToInt32(ddlDebit.SelectedValue);
            ObjOLERow.Debit_Type_Code = Convert.ToInt32("20");
            ObjOLERow.Expenditure_Type = Convert.ToInt32(ddlExpenditureType.SelectedValue);
            ObjOLERow.Expenditure_Type_Code = Convert.ToInt32("21");
            if (hidcusID.Value != string.Empty)
            { ObjOLERow.Customer_ID = Convert.ToInt32(hidcusID.Value); }
            else
            { ObjOLERow.Customer_ID = 0; }
            ObjOLERow.Created_By = intUserID;
            FunPriGenerateLeaseExpensesXMLDet();
            ObjOLERow.XMLOLEDetails = strXMLOLEDet;
            ObjS3G_LOANAD_OperatingLeaseExpensesDataTable.AddS3G_LOANAD_OperatingLeaseExpensesDetailsRow(ObjOLERow);
            ObjOperatingLeaseExpensesClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            string strErrMsg = string.Empty;
            intErrCode = ObjOperatingLeaseExpensesClient.FunPubCreateOperatingLeaseExpensesDetails(out strOLENo, out strErrMsg, SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_OperatingLeaseExpensesDataTable, SerMode));
            if (intErrCode == 0)  //Saving Successfully
            {
                txtOLENo.Text = strOLENo;
                strAlert = "Operating Lease Expenses details added successfully - " + strOLENo;
                strAlert += @"\n\nWould you like to add one more Lease Expenses?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                strRedirectPageView = "";
                //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                btnSave.Enabled = false;
                //END
                //lblsysJVno.Visible = lblsystemjvdate.Visible = txtSystemJVNo.Visible = txtsystemjvdate.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                lblErrorMessage.Text = string.Empty;
                return;
            }
            else if (intErrCode == -1)  // DOC # Not Defined
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoNotDefined);
                return;
            }
            else if (intErrCode == -2)  //DOC # Exceed
            {
                Utility.FunShowAlertMsg(this.Page, Resources.LocalizationResources.DocNoExceeds);
                return;
            }
            else if (intErrCode == 3)  // Check whether Expenses entered in open month
            {
                Utility.FunShowAlertMsg(this.Page, "OLE date should be in an open month");
                return;
            }
            else if (intErrCode == 11)  // Check whether Asset Value ISNULL
            {
                Utility.FunShowAlertMsg(this.Page, "Generate an invoice against that asset before booking expense");
                return;
            }
            else if (intErrCode == 10)  // Check whether Expenses amount exceed than Asset Value
            {
                Utility.FunShowAlertMsg(this.Page, "Expenses amount should not exceed than asset value ");
                return;
            }

            else
            {
                Utility.FunShowValidationMsg(this.Page, "OLE", intErrCode, strErrMsg, false);
                return;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            lblErrorMessage.Text = "Unable Save the  Operating Lease Expenses Details";
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            if (ObjOperatingLeaseExpensesClient != null)
                ObjOperatingLeaseExpensesClient.Close();
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
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlBranch.Clear();
            //if (ddlBranch.Items.Count > 0)
            //{ ddlBranch.SelectedIndex = 0; }
            //if (ddlLeaseAssetNo.Items.Count > 0)
            //{ ddlLeaseAssetNo.Items.Clear(); }
            ddlLeaseAssetNo.Clear();
            FunpriClear();
            ddlExpenditureType.Enabled = false;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Clear the controls";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            GetGridviewvalues();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Gridview`s Row";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region DDLEvents

    protected void ddlLeaseAssetNo_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlLeaseAssetNo.SelectedValue !="0")
            {
                FunpriClear();
                //FunPriGetAccountDetails();
            }
            else
                FunpriClear();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Prime A/c ,SubA/c and Customer Details";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void ddlGLAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlGLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlGLAcc");
            DropDownList ddlSLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlSLAcc");
            //Get SL Account
            Dictionary<string, string> dictGL = new Dictionary<string, string>();
            dictGL.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictGL.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
            dictGL.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictGL.Add("@GL_Code", Convert.ToString(ddlGLAcc.SelectedValue));
            ddlSLAcc.BindDataTable("S3G_LOANAD_GetGLSLACC", dictGL, new string[] { "SLAcc", "SLAcc" });

            // If Debit type = To Account then SL Account should be Customer code
            if (ddlDebit.SelectedIndex == 1)
            {
                ddlSLAcc.Items.Clear();
                ddlSLAcc.Items.Add(hidcuscode.Value);
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
                ddlSLAcc.Items.Insert(0, liSelect);
            }

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the SL Account";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlSLAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlSLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlSLAcc");
            if (ddlSLAcc.Items.Count > 0)
            {
                //TextBox txtBalCreSL = (TextBox)GRVOLE.FooterRow.FindControl("txtBalCreSL");
                //txtBalCreSL.Text = ddlSLAcc.SelectedItem.Text;
                //txtBalCreSL.ToolTip = ddlSLAcc.SelectedItem.Text;
                ddlSLAcc.ToolTip = ddlSLAcc.SelectedItem.Text;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the SL Account";
            cvCustomerMaster.IsValid = false;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunpriClear();
            GetAssetNo();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Asset Number";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void ddlDebit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDebit.SelectedValue == "1")
                FunPriGetAccountDetails();
            else
            {
                DataSet DS = new DataSet();
                dictParam = new Dictionary<string, string>();
                dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
                //dictParam.Add("@Asset_ID", Convert.ToString(ddlLeaseAssetNo.SelectedValue));
                dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
                dictParam.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
                dictParam.Add("@User_ID", Convert.ToString(intUserID));
                if (ddlLeaseAssetNo.SelectedValue != "0")
                { dictParam.Add("@Lease_Asset_No", Convert.ToString(ddlLeaseAssetNo.SelectedText.Split('-')[0].ToString().Trim())); }
                else
                {
                    Utility.FunShowAlertMsg(this.Page, "Select lease asset number");
                    return;
                }
                DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetAssetDesc, dictParam);


                // To get Asset Acquisition Date
                if (DS.Tables[1].Rows.Count >= 1)
                {
                    hidAssetAcquisitionDate.Value = Convert.ToString(DS.Tables[1].Rows[0]["Acquisition_Date"]);
                }
                ViewState["EmptyA/c"] = 0;
            }
            if (ddlDebit.SelectedIndex != 2)
            {
                lblExpenditureType.Enabled = ddlExpenditureType.Enabled = false;
                if (ddlExpenditureType.Items.Count > 0)
                { ddlExpenditureType.SelectedIndex = 0; }
                RFVExpenditureType.Visible = false;
                txtsubAccno.Visible = txtprimeAcc.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = true;
                FunPriGetAccountDetails();
                if (Convert.ToInt32(ViewState["EmptyA/c"]) != 1)
                    pnlCommAddress.Visible = true;
                GRVOLE.DataSource = null;
                GRVOLE.DataBind();
                pnlGridview.Visible = false;
            }
            else if (ddlDebit.SelectedIndex == 2)
            {
                lblExpenditureType.Enabled = ddlExpenditureType.Enabled = true;
                if (ddlExpenditureType.Items.Count > 0)
                { ddlExpenditureType.SelectedIndex = 0; }
                RFVExpenditureType.Visible = true;
                txtsubAccno.Visible = txtprimeAcc.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = false;
                clearcusdetails();
                txtprimeAcc.Text = txtsubAccno.Text = string.Empty;
                pnlCommAddress.Visible = false;
                GRVOLE.DataSource = null;
                GRVOLE.DataBind();
                pnlGridview.Visible = false;
            }
            btnGo.Enabled = true;

            //   ddlExpenditureType_SelectedIndexChanged(this, EventArgs.Empty);
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load Debit values";
            cvCustomerMaster.IsValid = false;
        }
    }
    protected void ddlExpenditureType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Enabled == true)
            {
                //GetGridviewvalues(); 
                GRVOLE.DataSource = null;
                GRVOLE.DataBind();
                pnlGridview.Visible = false;
                btnGo.Enabled = true;
                btnSave.Enabled = false;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            cvCustomerMaster.ErrorMessage = "Unable to Load the Expenditure Type";
            cvCustomerMaster.IsValid = false;
        }
    }

    #endregion

    #region User_Defined_Functions

    protected void getGLSLCODE()
    {
        try
        {
            Dictionary<string, string> dictGL = new Dictionary<string, string>();
            dictGL.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictGL.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
            dictGL.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictGL.Add("@Debit_Type", Convert.ToString(ddlDebit.SelectedValue));
            dictGL.Add("@Expenditure_Type", Convert.ToString(ddlExpenditureType.SelectedValue));

            DataRow dr;
            dtOLE.Columns.Add("GLACC");
            dtOLE.Columns.Add("SLACC");
            dtOLE.Columns.Add("Desc");
            dtOLE.Columns.Add("Amount");
            dtOLE.Columns.Add("BalCreGL");
            dtOLE.Columns.Add("BalCreSL");
            dr = dtOLE.NewRow();
            dtOLE.Rows.Add(dr);

            if ((strOLEID == "") || (strOLEID == null))
            {
                ViewState["OLEDetails"] = dtOLE;
                GRVOLE.DataSource = dtOLE;
                GRVOLE.DataBind();
                GRVOLE.Rows[0].Visible = false;
            }
            else
            {
                DataTable dt = Utility.GetDefaultData("S3G_LOANAD_GetGLSLACC", dictGL);
                if (dt.Rows.Count >= 1)
                {
                    GRVOLE.DataSource = dt;
                    GRVOLE.DataBind();
                }
                ViewState["OLEDetails"] = dt;
            }
            FunPriBindGLAcc();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriBindGLAcc()
    {
        try
        {
            Dictionary<string, string> dictGL = new Dictionary<string, string>();
            dictGL.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictGL.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
            dictGL.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictGL.Add("@Debit_Type", Convert.ToString(ddlDebit.SelectedValue));
            dictGL.Add("@Expenditure_Type", Convert.ToString(ddlExpenditureType.SelectedValue));
            DropDownList ddlGLAcc = (DropDownList)GRVOLE.FooterRow.FindControl("ddlGLAcc");
            //Changed by Thangam M on 16/Apr/2012
            ddlGLAcc.BindDataTable("S3G_LOANAD_GetGLSLACC", dictGL, new string[] { "GLAcc", "GLAcc" });
            //End here
            DataTable dt = Utility.GetDefaultData("S3G_LOANAD_GetGLSLACC", dictGL);
            if (dt.Rows.Count > 0)
            {
                TextBox txtBalCreGL = (TextBox)GRVOLE.FooterRow.FindControl("txtBalCreGL");
                txtBalCreGL.Text = Convert.ToString(dt.Rows[0]["BalCreGL"]);
                txtBalCreGL.ToolTip = Convert.ToString(dt.Rows[0]["BalCreGL"]);
            }
            dt.Clear();
            dt.Dispose();
            dictGL.Clear();
            FunproloadEntityCodes();

        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    protected void FunproloadEntityCodes()
    {
        if (GRVOLE.FooterRow != null)
        {
            Dictionary<string, string> dictGL = new Dictionary<string, string>();
            dictGL.Add("@Company_ID", Convert.ToString(intCompanyID));

            DropDownList ddlBalCreSL = (DropDownList)GRVOLE.FooterRow.FindControl("ddlBalCreSL");
            ddlBalCreSL.BindDataTable("S3G_LOANAD_GetEntityCodesOLExpenses", dictGL, new string[] { "Entity_Code", "Entity_Code", "Entity_Name" });
            //Added by Thangam.M on 16/Apr/2012
            ddlBalCreSL.AddItemToolTip();
            //End here
        }
    }
    protected void clearcusdetails()
    {
        try
        {
            S3GCustomerAddress1.ClearCustomerDetails();
            hidcusID.Value = hidcuscode.Value = string.Empty;
            ////// hidcusID.Value=txtPincode.Text = txtState.Text = txtAddress1.Text = txtAddress2.Text = txtTelephone.Text = txtCity.Text = txtCountry.Text = txtCustName.Text = txtEmailid.Text = txtMobile.Text = string.Empty;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void GetGridviewvalues()
    {
        try
        {
            if ((int)ViewState["EmptyA/c"] == 1 && Convert.ToInt32(ddlDebit.SelectedValue) == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Expenses can not book against the selected account change the debit type");
                btnSave.Enabled = false;
                return;
            }
            else
            {
                btnGo.Enabled = false;
                pnlGridview.Visible = btnSave.Enabled = pnlGridview.Visible = GRVOLE.Visible = true;
                getGLSLCODE();
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGenerateLeaseExpensesXMLDet()
    {
        try
        { strXMLOLEDet = GRVOLE.FunPubFormXml(); }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGetOperatingLeaseExpensesDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@OLE_ID", Convert.ToString(strOLEID));
            DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetOperatingLeaseExpensesDetails, dictParam);

            // Table 0[OLE Details]
            if (DS.Tables[0].Rows.Count >= 1)
            {
                ddlLOB.Items.Add(new ListItem(DS.Tables[0].Rows[0]["LOB_Name"].ToString(),DS.Tables[0].Rows[0]["LOB_ID"].ToString()));
                txtLOBName.Text = DS.Tables[0].Rows[0]["LOB_Name"].ToString();
                hidLOBID.Value = DS.Tables[0].Rows[0]["LOB_ID"].ToString();
                txtOLENo.Text = Convert.ToString(DS.Tables[0].Rows[0]["Operation_Lease_Expenses_No"]);
                DateTime Date = Convert.ToDateTime(DS.Tables[0].Rows[0]["Operation_Lease_Expenses_Date"]);
                txtOLEDate.Text = Date.ToString(strDateFormat);
                txtSystemJVNo.Text = Convert.ToString(DS.Tables[0].Rows[0]["SJV_No"]);
                if (Convert.ToString(DS.Tables[0].Rows[0]["JV_Date"]) != string.Empty)
                {
                    DateTime Date1 = Convert.ToDateTime(DS.Tables[0].Rows[0]["JV_Date"]);
                    txtsystemjvdate.Text = Date1.ToString(strDateFormat);
                }
                ddlBranch.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Location_ID"]);
                ddlBranch.SelectedText = Convert.ToString(DS.Tables[0].Rows[0]["Location_Name"]);
                ddlBranch.ToolTip = Convert.ToString(DS.Tables[0].Rows[0]["Location_Name"]);
                //Added by Thangam M on 16-Jul-2012 for UAT bug fixing
                //ddlLeaseAssetNo.Items.Add(Convert.ToString(DS.Tables[0].Rows[0]["Lease_Asset_No"]) + " - " + Convert.ToString(DS.Tables[0].Rows[0]["Asset_Description"]));
                ddlLeaseAssetNo.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Lease_Asset_No"]);
                ddlLeaseAssetNo.SelectedText = Convert.ToString(DS.Tables[0].Rows[0]["Lease_Asset_No"]) + " - " + Convert.ToString(DS.Tables[0].Rows[0]["Asset_Description"]);
                //ddlLeaseAssetNo.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Lease_Asset_No"]) ;
                //End here
                txtAssetDesc.Text = Convert.ToString(DS.Tables[0].Rows[0]["Asset_Description"]);
                txtAssetDesc.ToolTip = Convert.ToString(DS.Tables[0].Rows[0]["Asset_Description"]);
                txtBillrefno.Text = Convert.ToString(DS.Tables[0].Rows[0]["Bill_No"]);
                DateTime Date2 = Convert.ToDateTime(DS.Tables[0].Rows[0]["Bill_Date"]);
                txtBillRefDate.Text = Date2.ToString(strDateFormat);
                ddlDebit.Items.Add(new ListItem(Convert.ToString(DS.Tables[0].Rows[0]["Debit_Type_Desc"]),Convert.ToString(DS.Tables[0].Rows[0]["Debit_Type"])));
                ddlDebit.ToolTip = Convert.ToString(DS.Tables[0].Rows[0]["Debit_Type_Desc"]);
                if (ddlDebit.SelectedIndex == 1)
                { 
                    lblExpenditureType.Enabled = ddlExpenditureType.Enabled = false;
                }
                else if (ddlDebit.SelectedIndex == 2)
                {
                    pnlCommAddress.Visible = false;
                    lblprimeaccno.Visible = txtsubAccno.Visible = lblsubAccno.Visible = txtprimeAcc.Visible = false;
                }
                if( Convert.ToString(DS.Tables[0].Rows[0]["Expenditure_Type"])!=""&& Convert.ToString(DS.Tables[0].Rows[0]["Expenditure_Type"])!="0")
                {
                    ddlExpenditureType.Items.Add(new ListItem(DS.Tables[0].Rows[0]["Expenditure_Type_Desc"].ToString(),DS.Tables[0].Rows[0]["Expenditure_Type"].ToString()));
                    ddlExpenditureType.ToolTip = Convert.ToString(DS.Tables[0].Rows[0]["Expenditure_Type_Desc"]);
                    //ddlExpenditureType.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Expenditure_Type"]);
                    //ddlExpenditureType.SelectedValue = Convert.ToString(DS.Tables[0].Rows[0]["Expenditure_Type"]);
                }
                else
                {
                    ddlExpenditureType.Items.Add(new ListItem("--Select--","0"));
                }
             
                //txtSystemJVNo.Text=Convert.ToString(DS.Tables[0].Rows[0]["Journal_No"]);
                //if (DS.Tables[0].Columns.Count > 13)
                //{
                if (Convert.ToString(DS.Tables[0].Rows[0]["PANum"])!="")
                    txtprimeAcc.Text = Convert.ToString(DS.Tables[0].Rows[0]["PANum"]);
                  
                 s = Convert.ToString(DS.Tables[0].Rows[0]["SANum"]);
                
                    if (s.Contains("DUMMY"))
                    { txtsubAccno.Text = ""; }
                    else
                    { txtsubAccno.Text = Convert.ToString(DS.Tables[0].Rows[0]["SANum"]); }
               
                if(Convert.ToString(DS.Tables[0].Rows[0]["Customer_ID"])!="")
                    S3GCustomerAddress1.SetCustomerDetails(DS.Tables[0].Rows[0], true);
                //}
            }

            // Table 1 [Asset Details to be stored in Gridvview]
            if (DS.Tables[1].Rows.Count >= 1)
            {
                GRVOLE.DataSource = DS.Tables[1];
                GRVOLE.DataBind();
            }
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
                    txtBillRefDate.Attributes.Add("readonly", "readonly");
                    txtOLEDate.Attributes.Add("readonly", "readonly");
                    txtLOBName.Attributes.Add("readonly", "readonly");
                    btnSave.Enabled = false;
                    ddlBranch.Focus();
                    ddlExpenditureType.Enabled = pnlGridview.Visible = lblsysJVno.Visible = lblsystemjvdate.Visible = txtSystemJVNo.Visible = txtsystemjvdate.Visible = false;
                    break;
                case -1:// Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //ddlBranch.ClearDropDownList();
                    //ddlLeaseAssetNo.ClearDropDownList();
                   ddlDebit.ClearDropDownList();
                    ddlExpenditureType.ClearDropDownList();
                    txtsystemjvdate.ReadOnly = txtSystemJVNo.ReadOnly = txtOLENo.ReadOnly = txtOLEDate.ReadOnly = txtAssetDesc.ReadOnly = txtBillRefDate.ReadOnly = txtBillrefno.ReadOnly = true;
                    btnGo.Enabled = CECBillRefDate.Enabled = btnClear.Enabled = btnSave.Enabled = CECOLEDATE.Enabled = false;
                    pnlGridview.Visible = GRVOLE.Visible = true;
                    GRVOLE.FooterRow.Visible = imgBillRefdate.Visible = imgOLEDate.Visible = GRVOLE.Columns[7].Visible = false;
                    txtLOBName.Attributes.Add("readonly", "readonly");
                    txtLOBName.ReadOnly = true;
                    ddlBranch.ReadOnly = true;
                    break;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }

    }

    private string FunPubGetPageTitles(string p)
    {
        throw new NotImplementedException();
    }
    private void FunPriBindBranchLOB()
    {
        try
        {
            // LOB
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (strMode != "Q")
                dictParam.Add("@Is_Active", "1");
            dictParam.Add("@User_ID", Convert.ToString(intUserID));
            dictParam.Add("@Program_Id", "68");

            ddlLOB.BindDataTable(SPNames.S3G_LOANAD_GetOperatingLeaseExpensesLOB, dictParam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
           
            if (ddlLOB.Items.Count > 1)
            {
                txtLOBName.Text = ddlLOB.Items[1].Text;
                hidLOBID.Value = ddlLOB.Items[1].Value;
            }
            if (ddlLOB.Items.Count == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "The LOB OL-Operating Lease is inactive ");
                return;
            }
            //Branch
            //dictParam = new Dictionary<string, string>();
            //dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //if (strMode != "Q")
            //    dictParam.Add("@Is_Active", "1");
            //dictParam.Add("@User_ID", Convert.ToString(intUserID));
            //dictParam.Add("@Lob_Id", Convert.ToString(hidLOBID.Value));
            //dictParam.Add("@Program_ID", "68");

            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, dictParam, new string[] { "Location_ID", "Location_Code", "Location_Name" });

            // Debit Type
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LookupType_Code", "20");
            ddlDebit.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });

            // Expenditure Status
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictParam.Add("@LookupType_Code", "21");
            ddlExpenditureType.BindDataTable(SPNames.S3G_LOANAD_GetLookUpValues, dictParam, new string[] { "Lookup_Code", "Lookup_Description" });
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    protected void GetAssetNo()
    {
        //try
        //{
        //    //Lease Asset No
        //    dictParam = new Dictionary<string, string>();
        //    dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
        //    dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
        //    dictParam.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
        //    dictParam.Add("@User_ID", Convert.ToString(intUserID));
        //    ddlLeaseAssetNo.BindDataTable(SPNames.S3G_LOANAD_GetLeaseAssetNo, dictParam, new string[] { "Lease_Asset_No", "Lease_Asset_No" });
        //}
        //catch (Exception objException)
        //{
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        //    throw objException;
        //}
        ddlLeaseAssetNo.Clear();
    }
    [System.Web.Services.WebMethod]
    public static string[] GetDocNo(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Add("@Company_ID", Convert.ToString(obj_Page.intCompanyID));
        Procparam.Add("@Location_ID", Convert.ToString(obj_Page.ddlBranch.SelectedValue));
        Procparam.Add("@LOB_ID", Convert.ToString(obj_Page.hidLOBID.Value));
        Procparam.Add("@User_ID", obj_Page.UserId.ToString());
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LOANAD_GetLeaseAssetNo_AGT", Procparam));

        return suggetions.ToArray();
    }
    private void FunpriClear()
    {
        try
        {
            if (ddlExpenditureType.Items.Count > 0)
            { ddlExpenditureType.SelectedIndex = 0; }
            if (ddlDebit.Items.Count > 0)
            { ddlDebit.SelectedIndex = 0; }
            txtsubAccno.Text = txtprimeAcc.Text = lblErrorMessage.Text = txtBillrefno.Text = txtBillRefDate.Text = txtOLEDate.Text = txtAssetDesc.Text = txtOLENo.Text = txtsystemjvdate.Text = txtSystemJVNo.Text = string.Empty;
            RFVExpenditureType.Visible = btnGo.Enabled = lblExpenditureType.Enabled = txtprimeAcc.Visible = txtsubAccno.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = true;
            btnSave.Enabled = pnlGridview.Visible = GRVOLE.Visible = false;
            ViewState["EmptyA/c"] = ViewState["OLEDetails"] = null;
            clearcusdetails();
            S3GCustomerAddress1.Visible = pnlCommAddress.Visible = true;
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGetAccountDetails()
    {
        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //dictParam.Add("@Asset_ID", Convert.ToString(ddlLeaseAssetNo.SelectedValue));
            dictParam.Add("@Location_ID", Convert.ToString(ddlBranch.SelectedValue));
            dictParam.Add("@LOB_ID", Convert.ToString(hidLOBID.Value));
            dictParam.Add("@User_ID", Convert.ToString(intUserID));
            if (ddlLeaseAssetNo.SelectedValue != "0")
            { dictParam.Add("@Lease_Asset_No", Convert.ToString(ddlLeaseAssetNo.SelectedValue.Split('-')[0].ToString().Trim())); }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Select lease asset number");
                return;
            }
            DS = Utility.GetDataset(SPNames.S3G_LOANAD_GetAssetDesc, dictParam);

            if (DS.Tables.Count == 2)
            {
                if (DS.Tables[0].Rows.Count >= 1)
                {
                    txtAssetDesc.Text = Convert.ToString(DS.Tables[0].Rows[0][0]);
                    txtAssetDesc.ToolTip = Convert.ToString(DS.Tables[0].Rows[0][0]);
                }
                // To get Asset Acquisition Date
                if (DS.Tables[1].Rows.Count >= 1)
                {
                    hidAssetAcquisitionDate.Value = Convert.ToString(DS.Tables[1].Rows[0]["Acquisition_Date"]);
                }
                Utility.FunShowAlertMsg(this.Page, "Selected Lease Asset Number is not mapped to an Account");
                txtprimeAcc.Visible = txtsubAccno.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = false;
                txtprimeAcc.Text = txtsubAccno.Text = string.Empty;
                ViewState["EmptyA/c"] = 1;
                btnSave.Enabled = pnlGridview.Visible = GRVOLE.Visible = pnlCommAddress.Visible = S3GCustomerAddress1.Visible = false;
                return;
            }

            else if (DS.Tables.Count == 4)
            {

                if (DS.Tables[0].Rows.Count >= 1)
                {
                    txtAssetDesc.Text = Convert.ToString(DS.Tables[0].Rows[0][0]);
                    txtAssetDesc.ToolTip = Convert.ToString(DS.Tables[0].Rows[0][0]);
                }
                // To get Asset Acquisition Date
                if (DS.Tables[1].Rows.Count >= 1)
                {
                    hidAssetAcquisitionDate.Value = Convert.ToString(DS.Tables[1].Rows[0]["Acquisition_Date"]);
                }
                if (DS.Tables[2].Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Selected Lease Asset Number is not mapped to an Account");
                    txtprimeAcc.Text = txtsubAccno.Text = string.Empty;
                    txtprimeAcc.Visible = txtsubAccno.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = false;
                    ViewState["EmptyA/c"] = 1;
                    pnlCommAddress.Visible = false;
                    return;
                }
                if (DS.Tables[2].Rows.Count >= 1)
                {
                    txtprimeAcc.Visible = txtsubAccno.Visible = lblprimeaccno.Visible = lblsubAccno.Visible = true;
                    txtprimeAcc.Text = Convert.ToString(DS.Tables[2].Rows[0]["PANUM"]);
                    s = Convert.ToString(DS.Tables[2].Rows[0]["SANUM"]);
                    if (s.Contains("DUMMY"))
                    { txtsubAccno.Text = ""; }
                    else
                    { txtsubAccno.Text = Convert.ToString(DS.Tables[2].Rows[0]["SANUM"]); }
                }
                if (DS.Tables[3].Rows.Count >= 1)
                {
                    pnlCommAddress.Visible = S3GCustomerAddress1.Visible = true;
                    hidcusID.Value = Convert.ToString(DS.Tables[3].Rows[0]["Customer_ID"]);
                    hidcuscode.Value = Convert.ToString(DS.Tables[3].Rows[0]["Customer_Code"]);
                    S3GCustomerAddress1.SetCustomerDetails(DS.Tables[3].Rows[0], true);

                }
                ViewState["EmptyA/c"] = 0;
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
            //Get the Company and user ID
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();
            obj_Page = this;
            //Set the Date Format
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CECOLEDATE.Format = strDateFormat;
            CECBillRefDate.Format = strDateFormat;
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                { strOLEID = Convert.ToString(fromTicket.Name); }
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
           
            //Added by Thangam.M on 16/Apr/2012
            if (GRVOLE.FooterRow != null)
            {
                DropDownList ddlBalCreSL = (DropDownList)GRVOLE.FooterRow.FindControl("ddlBalCreSL");
                ddlBalCreSL.AddItemToolTip();
            }
            //End here

            if (!IsPostBack)
            {
               
                //User Authorization            
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if ((strOLEID != "") && (strMode == "Q")) // Query // Modify
                {
                    FunPriGetOperatingLeaseExpensesDetails();
                    FunPriDisableControls(-1);
                }
                else
                {  // Create Mode
                    FunPriDisableControls(0);
                    FunPriBindBranchLOB();
                } 
            }
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
        Procparam.Add("@Program_Id", "068");
        Procparam.Add("@Lob_Id", obj_Page.hidLOBID.Value);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }


    #endregion

}
