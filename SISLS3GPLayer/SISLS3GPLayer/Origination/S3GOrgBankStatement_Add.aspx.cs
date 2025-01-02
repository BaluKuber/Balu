#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Orgination 
/// Screen Name			: 
/// Created By			: 
/// Created Date		: 
/// Purpose	            : To Add/Edit the Customer
/// 
/// Modified By         : Thangam M
/// Modified On         : 17-Sep-2010
/// Reason              : Bug Fixation 
///
#endregion


using System;
using System.Collections.Generic;
using System.Data;
using S3GBusEntity;
using System.ServiceModel;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Text;
using System.Web.Security;
using System.IO;
using System.Configuration;

public partial class Origination_S3GOrgBankStatement_Add : ApplyThemeForProject
{
    int intErrCode = 0;
    int intErroBank = 0;
    int intBankCaptureID = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    Dictionary<string, string> dictLOBMaster;
    Dictionary<string, string> dictParam = null; 
    UserInfo ObjUserInfo = new UserInfo();
    string strRedirectPage = "window.location.href='../Origination/S3GOrgBankStatement_View.aspx';";

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3GOrgBankStatement_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgBankStatement_Add.aspx';";

    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objBankStatement_LOVClient;
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsDataTable objS3G_ORG_BankLOVDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsDataTable();
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable objS3G_ORG_BankStatementTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable();
    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLDataTable objS3G_ORG_ListXMLTable = new S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLDataTable();
    DataTable dtListValues = new DataTable();
    StringBuilder strbListOfValues = new StringBuilder();

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;

    int inthdUserid;

    protected void Page_Load(object sender, EventArgs e)
    {
        objBankStatement_LOVClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;

            if (Request.QueryString["qsBankViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsBankViewId"));
                intBankCaptureID = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString.Get("qsMode");
            }

            FunPubSetIndex(1);
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            DataTable dtBankID = new DataTable();
            
            if (intBankCaptureID > 0)
            {
                ViewState["LOV_ID"] = 1;
            }
            else
            {
                ViewState["LOV_ID"] = 0;

            }
            if (!IsPostBack)
            {
                if (PageMode == PageModes.Create)
                {
                    FunLoadGridInsert();
                    FunLoadLOBMaster();

                    byte[] byteObjCaptureData = objBankStatement_LOVClient.FunPubGetBankStatemnt_ID();
                    dtBankID = (DataTable)ClsPubSerialize.DeSerialize(byteObjCaptureData, SerializationMode.Binary, typeof(DataTable));

                    if (dtBankID.Rows.Count > 0)
                    {
                        FunLOBProductLoadGrid(intCompanyID, intUserID);
                    }
                }
                if ((intBankCaptureID > 0) && (strMode == "M"))
                {
                    FunProLoadBankDetails();
                    //FunLoadBankLOBMaster(intBankCaptureID);
                    //FunctionLoadListValues(intBankCaptureID);
                    FunPriDisableControls(1);

                }
                else if ((intBankCaptureID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunProLoadBankDetails();
                    //FunLoadBankLOBMaster(intBankCaptureID);
                    //FunctionLoadListValues(intBankCaptureID);
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }

                ddlLOB.Focus();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
        finally
        {
            objBankStatement_LOVClient.Close();
        }
    }

    protected void FunProLoadBankDetails()
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            DataSet Dst = new DataSet();

            dictParam.Add("@Bank_Stmt_Data_Capture_ID", intBankCaptureID.ToString());
            Dst = Utility.GetDataset("S3G_Get_Bank_Stmt_LOVDetails", dictParam);

            if (Dst != null)
            {
                ListItem lst;

                lst = new ListItem(Dst.Tables[0].Rows[0]["LOB_Name"].ToString(), Dst.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);

                ddlLOB.SelectedValue = Dst.Tables[0].Rows[0]["LOB_ID"].ToString();
                //FunLoadProductCode();

                if (!string.IsNullOrEmpty(Dst.Tables[0].Rows[0]["Product_ID"].ToString()))
                {
                    lst = new ListItem(Dst.Tables[0].Rows[0]["Product_Name"].ToString(), Dst.Tables[0].Rows[0]["Product_ID"].ToString());
                }
                else
                {
                    lst = new ListItem("--Select--", "0");
                }
                ddlProductCode.Items.Add(lst);
                ddlProductCode.SelectedValue = Dst.Tables[0].Rows[0]["Product_ID"].ToString();
                //FunLoadConstitutionCode();

                lst = new ListItem(Dst.Tables[0].Rows[0]["Constitution_Name"].ToString(), Dst.Tables[0].Rows[0]["Constitution_ID"].ToString());
                ddlConstitution.Items.Add(lst);
                ddlConstitution.SelectedValue = Dst.Tables[0].Rows[0]["Constitution_ID"].ToString();
                txtScanPath.Text = Dst.Tables[0].Rows[0]["FlatFile_Path"].ToString();
                if((Dst.Tables[0].Rows[0]["Scan_Download"].ToString() == "True")||(Dst.Tables[0].Rows[0]["Scan_Download"].ToString() == "1"))
                {
                    RBDownloadBankStatement.Checked = true;
                }
                else
                {
                    RBScanBankStatement.Checked = true;
                }

                if (Dst.Tables[1].Rows.Count > 0)
                {
                    ViewState["ListValues"] = Dst.Tables[1];
                    grvListValues.DataSource = Dst.Tables[1];
                    grvListValues.DataBind();
                }
                else
                {
                    FunLoadGridInsert();
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void FunLoadGridInsert()
    {

        try
        {
            DataRow drEmptyRow;
            dtListValues = new DataTable();
            dtListValues.Columns.Add("Bank_Stmt_LOV_ID");
            dtListValues.Columns.Add("Field_Name");
            dtListValues.Columns.Add("From_Position");
            dtListValues.Columns.Add("To_Position");
            dtListValues.Columns.Add("FromCopy");
            drEmptyRow = dtListValues.NewRow();
            dtListValues.Rows.Add(drEmptyRow);
            grvListValues.DataSource = dtListValues;
            grvListValues.DataBind();
            dtListValues.Rows.RemoveAt(0);
            ViewState["ListValues"] = dtListValues;
            
            grvListValues.Rows[0].Visible = false;

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            dtListValues.Dispose();
            dtListValues = null;
        }
    }

    protected void grvListValues_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow drRow;
            if (e.CommandName == "AddNew")
            {
                DataTable dtGrid = new DataTable();
                dtGrid = (DataTable)ViewState["ListValues"];


                if (dtGrid.Rows.Count > 0)
                {

                    for (int i = 0; i <= dtGrid.Rows.Count - 1; i++)
                    {
                        TextBox Field_Name = (TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd");
                        TextBox txtToAdd1 = (TextBox)grvListValues.FooterRow.FindControl("txtToAdd");
                        if (dtGrid.Rows[i]["Field_Name"].ToString() == Field_Name.Text.Trim())
                        {
                            Utility.FunShowAlertMsg(this, "Selected List of Values Already Exist");
                            Field_Name.Text = string.Empty;
                            txtToAdd1.Text = string.Empty;

                            return;
                        }
                    }
                }

                if (dtGrid.Rows.Count > 0)
                {
                    for (int i = 0; i <= grvListValues.Rows.Count - 1; i++)
                    {
                        TextBox From_Position = (TextBox)grvListValues.Rows[i].FindControl("txtFrom");
                        TextBox To_Position = (TextBox)grvListValues.Rows[i].FindControl("txtTo");

                        int x = Convert.ToInt32(From_Position.Text);
                        int y = Convert.ToInt32(To_Position.Text);

                        if (y <= x)
                        {
                            Utility.FunShowAlertMsg(this, "To value need to be greater than From value");
                            To_Position.Focus();
                            return;
                        }
                        if (i != 0)
                        {
                            TextBox Pre_To_Position = (TextBox)grvListValues.Rows[i - 1].FindControl("txtTo");

                            int x1 = Convert.ToInt32(Pre_To_Position.Text);
                            if (x1 >= x)
                            {
                                Utility.FunShowAlertMsg(this, "  The values should not overlap");
                                From_Position.Focus();
                                return;
                            }
                        }
                    }
                }

                for (int i = 0; i <= ((DataTable)ViewState["ListValues"]).Rows.Count - 1; i++)
                {
                    TextBox From_Position = (TextBox)grvListValues.Rows[i].FindControl("txtFrom");
                    TextBox To_Position = (TextBox)grvListValues.Rows[i].FindControl("txtTo");

                    dtGrid.Rows[i]["From_Position"] = From_Position.Text;
                    dtGrid.Rows[i]["To_Position"] = To_Position.Text;
                }

                ViewState["ListValues"] = dtGrid;

                TextBox txtSerialAdd = (TextBox)grvListValues.FooterRow.FindControl("txtSerialAdd");
                TextBox txtListValuesAdd = (TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd");
                TextBox txtFromAdd = (TextBox)grvListValues.FooterRow.FindControl("txtFromAdd");
                TextBox txtToAdd = (TextBox)grvListValues.FooterRow.FindControl("txtToAdd");
                txtListValuesAdd.Focus();
                if (txtListValuesAdd.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter List of values to the list');", true);
                    txtListValuesAdd.Focus();
                    return;

                }

                else if (txtFromAdd.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter From value');", true);
                    txtFromAdd.Focus();
                    return;

                }

                else if (txtToAdd.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter To value');", true);
                    txtToAdd.Focus();
                    return;
                }

                else if (Convert.ToInt32(txtFromAdd.Text) > Convert.ToInt32(txtToAdd.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To value need to be greater than From value');", true);
                    txtToAdd.Text = string.Empty;
                    txtToAdd.Focus();
                    return;
                }

                else if (txtFromAdd.Text.CompareTo(txtToAdd.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From and To value need to be different');", true);
                    txtFromAdd.Text = txtToAdd.Text = string.Empty;
                    txtFromAdd.Focus();
                    return;
                }
                else
                {
                    int intTo = 0;
                    TextBox txtTo = (TextBox)grvListValues.Rows[grvListValues.Rows.Count - 1].FindControl("txtTo");
                    if (!string.IsNullOrEmpty(txtTo.Text.Trim()))
                    {
                        intTo = Convert.ToInt32(txtTo.Text.Trim());// +1;
                        if (intTo >= Convert.ToInt32(txtFromAdd.Text.Trim()))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Values cannot overlap');", true);
                            txtListValuesAdd.Focus();
                            return;
                        }
                    }
                }

                dtListValues = (DataTable)ViewState["ListValues"];
                //if (dtListValues.Rows[0]["Field_Name"].ToString() == string.Empty && intBankCaptureID == 0)
                //{
                //    dtListValues.Rows[0].Delete();
                //}
                foreach (DataRow dRow in dtListValues.Rows)
                {
                    if (txtFromAdd.Text == dRow["From_Position"].ToString())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From values need to be distinct');", true);
                        txtFromAdd.Text = string.Empty;
                        txtFromAdd.Focus();
                        return;
                    }
                }
                drRow = dtListValues.NewRow();

                //txtSerialAdd.Text = "0";
                drRow["Bank_Stmt_LOV_ID"] = "0";
                //Convert.ToInt32(txtSerialAdd.Text.Trim());
                drRow["Field_Name"] = txtListValuesAdd.Text.Trim();
                drRow["From_Position"] = txtFromAdd.Text.Trim();
                drRow["To_Position"] = txtToAdd.Text.Trim();
                drRow["FromCopy"] = "0";
                dtListValues.Rows.Add(drRow);
                grvListValues.DataSource = dtListValues;
                grvListValues.DataBind();
                ViewState["ListValues"] = dtListValues;
                TextBox txtFromVale = (TextBox)grvListValues.FooterRow.FindControl("txtFromAdd");
                txtFromVale.Text = Convert.ToString(Convert.ToInt32(txtToAdd.Text.Trim()) + Convert.ToInt32("1"));
                if (intBankCaptureID > 0)
                {
                    ViewState["LOV_ID"] = 1;
                }
                else
                {
                    ViewState["LOV_ID"] = 0;
                }
                //RequiredFieldValidator rqvListValue = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvListValue");
                //RequiredFieldValidator rqvFrom = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvFrom");
                //RequiredFieldValidator rqvTo = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvTo");
                //rqvListValue.Enabled = rqvFrom.Enabled = rqvTo.Enabled = false;

                TextBox txtListValues = (TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd");
                txtListValues.Focus();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }

    protected void grvListValues_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtTable = new DataTable();
            dtTable = (DataTable)ViewState["ListValues"];

            if (dtTable.Rows.Count > 0)
            {
                dtTable.Rows.RemoveAt(e.RowIndex);

                if (dtTable.Rows.Count <= 0)
                {
                    FunLoadGridInsert();
                }
                else
                {
                    grvListValues.DataSource = dtTable;
                    grvListValues.DataBind();
                    ViewState["ListValues"] = dtTable;
                }

                if (dtTable.Rows.Count > 0 && dtTable.Rows[grvListValues.Rows.Count - 1]["Field_Name"].ToString() != "")
                {
                    TextBox txtToAdd = (TextBox)grvListValues.Rows[grvListValues.Rows.Count - 1].FindControl("txtTo");
                    TextBox txtFromVale = (TextBox)grvListValues.FooterRow.FindControl("txtFromAdd");
                    txtFromVale.Text = Convert.ToString(Convert.ToInt32(txtToAdd.Text.Trim()) + Convert.ToInt32("1"));
                }

                RequiredFieldValidator rqvListValue = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvListValue");
                RequiredFieldValidator rqvFrom = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvFrom");
                RequiredFieldValidator rqvTo = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvTo");
                rqvListValue.Enabled = rqvFrom.Enabled = rqvTo.Enabled = false;

                TextBox txtListValues = (TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd");
                txtListValues.Focus();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }

    #region "Drow down list load"

    private void FunLoadLOBMaster()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            dictLOBMaster = new Dictionary<string, string>();
            dictLOBMaster.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            dictLOBMaster.Add("@User_Id", Convert.ToString(ObjUserInfo.ProUserIdRW));
            if (PageMode == PageModes.Create)
            {
                dictLOBMaster.Add("@Is_Active", "1");
            }
            dictLOBMaster.Add("@Program_ID", "39");
            ddlLOB.BindDataTable(SPNames.LOBMaster, dictLOBMaster, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    private void FunLoadProductCode()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            dictLOBMaster = new Dictionary<string, string>();
            dictLOBMaster.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            if (PageMode == PageModes.Create)
            {
                dictLOBMaster.Add("@Is_Active", "1");
            }
            dictLOBMaster.Add("@LOB_ID", ddlLOB.SelectedValue);
            
            //Commented by saranya on 15-Feb-2012 to remove the Product Code in LOV
            //ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, dictLOBMaster, new string[] { "Product_ID", "Product_Code", "Product_Name" });
            ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, dictLOBMaster, new string[] { "Product_ID", "Product_Name" });
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    private void FunLoadConstitutionCode()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "true");
            }
            //Commented by saranya on 15-Feb-2012 to remove the Constitution_ID in LOV
           // ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
            ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "Constitution_Name" });
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }


    private void FunPriBindDocumentPath()
    {
        try
        {
            txtScanPath.Text = "";
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyID.ToString());
            dictParam.Add("@Program_Id", "39");
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            dictParam.Add("@Is_Active", "1");
            DataTable dtPath = new DataTable();
            dtPath = Utility.GetDefaultData("S3G_ORG_GetDocPathforLOB", dictParam);
            if (dtPath != null)
            {
                if (dtPath.Rows.Count > 0)
                {
                    txtScanPath.Text = dtPath.Rows[0]["Path"].ToString();
                }
            }


        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    private void FunLoadBankLOBMaster(int intBank_ID)
    {
        try
        {
            string strBankID = Convert.ToString(intBank_ID);
            dictLOBMaster = new Dictionary<string, string>();
            dictLOBMaster.Add("@Bank_ID", strBankID);

            DataTable dtLOBMaster = new DataTable();
            dtLOBMaster = Utility.GetDefaultData("S3G_Get_Bank_LOBList", dictLOBMaster);
            //ddlConstitution.Visible = ddlLOB.Visible = ddlProductCode.Visible = false;
            //       txtConstitution.Visible = true;

            //txtLOB.Text = dtLOBMaster.Rows[0]["LOB_Code"].ToString() + "-" + dtLOBMaster.Rows[0]["LOB_Name"].ToString();
            //txtProduct.Text = dtLOBMaster.Rows[0]["Product_Code"].ToString() + "-" + dtLOBMaster.Rows[0]["Product_Name"].ToString();
            //txtConstitution.Text = dtLOBMaster.Rows[0]["Constitution_Code"].ToString() + "-" + dtLOBMaster.Rows[0]["Constitution_Name"].ToString();

            hdnID.Value = dtLOBMaster.Rows[0]["Created_By"].ToString();

            //txtConstitution.ReadOnly = true;
            //txtLOB.ReadOnly = txtProduct.ReadOnly = true;
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../Origination/S3GOrgBankStatement_View.aspx");
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = ddlProductCode.SelectedIndex = ddlConstitution.SelectedIndex = 0;
            
            txtScanPath.Text = string.Empty;
            lnkCopyProfile.Enabled = false;
            FunLoadGridInsert();
            //added by saranya on 16-Feb-2012 
            ddlProductCode.ClearDropDownList();
            ddlConstitution.ClearDropDownList();
            //end
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }



    #region "Save List Values and Bank Data"

    protected void btnSave_Click(object sender, EventArgs e)
    {
        objBankStatement_LOVClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            //ValidationSummary1.Enabled = false;
            //rfvLOB1.Enabled = false;
            //rfvConstitution.Enabled = false;
            if (Page.IsValid)// && ViewState["FilePath"] != null)
            {
                if (((DataTable)ViewState["ListValues"]).Rows.Count == 0)
                {
                    Utility.FunShowAlertMsg(this, "Add atleast one Bank statement");
                    return;
                }
                DataTable dtGrid = new DataTable();
                dtGrid = (DataTable)ViewState["ListValues"];
                //TextBox Field_Name = (TextBox)grvListValues.Rows[i].FindControl("txtListValues");
                //if (dtGrid.Rows.Count > 0)
                //{
                //    for (int i = 0; i <= dtGrid.Rows.Count - 1; i++)
                //    {

                //        if (dtGrid.Rows[i]["Field_Name"].ToString() == Field_Name.Text.Trim())
                //        {
                //            Utility.FunShowAlertMsg("Choose another List Value");
                //            return;
                //        }
                //    }
                //}
                if (((DataTable)ViewState["ListValues"]).Rows.Count > 0)
                {
                    for (int i = 0; i <= grvListValues.Rows.Count - 1; i++)
                    {
                        TextBox txtListValues = (TextBox)grvListValues.Rows[i].FindControl("txtListValues");
                        TextBox From_Position = (TextBox)grvListValues.Rows[i].FindControl("txtFrom");
                        TextBox To_Position = (TextBox)grvListValues.Rows[i].FindControl("txtTo");

                        if (txtListValues.Text.Trim() == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter List of value");
                            txtListValues.Focus();
                            return;
                        }

                        if (From_Position.Text.Trim() == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter From value");
                            From_Position.Focus();
                            return;
                        }
                        if (To_Position.Text.Trim() == "")
                        {
                            Utility.FunShowAlertMsg(this, "Enter To value");
                            To_Position.Focus();
                            return;
                        }

                        int x = Convert.ToInt32(From_Position.Text);
                        int y = Convert.ToInt32(To_Position.Text);

                        if (y <= x)
                        {
                            Utility.FunShowAlertMsg(this, "To value need to be greater than from value");
                            To_Position.Focus();
                            return;
                        }
                        if (i != 0)
                        {
                            TextBox Pre_To_Position = (TextBox)grvListValues.Rows[i - 1].FindControl("txtTo");

                            int x1 = Convert.ToInt32(Pre_To_Position.Text);
                            if (x1 >= x)
                            {
                                Utility.FunShowAlertMsg(this, "  The values should not overlap");
                                From_Position.Focus();
                                return;
                            }
                        }
                    }
                }
                ObjUserInfo = new UserInfo();

                int intBankCapture_ID = 0;

                SerializationMode SerMode = SerializationMode.Binary;

                //objBankStatement_LOVClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
                //byte[] byteObjCaptureData = objBankStatement_LOVClient.FunPubGetBankStatemnt_ID();
                //dtBankID = (DataTable)ClsPubSerialize.DeSerialize(byteObjCaptureData, SerializationMode.Binary, typeof(DataTable));

                //if (dtBankID.Rows.Count > 0)
                //{
                //    intBankCapture_ID = Convert.ToInt32(dtBankID.Rows[0]["Bank_Stmt_Data_Capture_ID"]) + 1;
                //}
                //else
                //{
                //    intBankCapture_ID = 1;
                //}

                try
                {


                    if (txtScanPath.Text.Trim() == "")
                    {
                        //Commented by Saranya
                        //Utility.FunShowAlertMsg(this.Page, "Enter the " + lblScanPath.Text);
                        Utility.FunShowAlertMsg(this.Page, lblScanPath.Text+ " Not Defined in Document Path Setup");
                        //end
                        return;
                    }
                    //else
                    //{
                    //    Utility.FunShowAlertMsg(this.Page, "Enter the Download Location Path");
                    //    return;
                    //}
                    if (txtScanPath.Text.Trim() != "" && !Directory.Exists(txtScanPath.Text.Trim()))
                    {
                        string Alert = "";
                        if (RBDownloadBankStatement.Checked)
                        {
                            Alert = "Invalid Download Location Path";
                        }
                        else
                        {
                            Alert = "Invalid Scan Location Path";
                        }
                        Utility.FunShowAlertMsg(this.Page, Alert);
                        return;
                    }


                    if (intBankCaptureID > 0)
                        FunGenerateXMLListOfValues(intBankCaptureID);
                    else
                        FunGenerateXMLListOfValues(intBankCapture_ID);



                    S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureRow objBankStatementRow;
                    objBankStatementRow = objS3G_ORG_BankStatementTable.NewS3G_ORG_Bank_Statement_Data_CaptureRow();
                    objBankStatementRow.Bank_Stmt_Data_Capture_ID = intBankCaptureID;
                    objBankStatementRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                    objBankStatementRow.Product_ID = Convert.ToInt32(ddlProductCode.SelectedValue);
                    objBankStatementRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
                    objBankStatementRow.FlatFile_Path = txtScanPath.Text.Trim();
                    objBankStatementRow.XMLListValues = strbListOfValues.ToString();
                    objBankStatementRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
                    objBankStatementRow.FlatFile_Path = txtScanPath.Text.Trim();
                    if (RBDownloadBankStatement.Checked)
                    {
                        objBankStatementRow.Scan_Download = true;
                    }
                    else
                    {
                        objBankStatementRow.Scan_Download = false;
                    }


                    objBankStatementRow.Created_By = ObjUserInfo.ProUserIdRW;
                    objBankStatementRow.Created_On = DateTime.Now;
                    objBankStatementRow.Modified_By = ObjUserInfo.ProUserIdRW;
                    objBankStatementRow.Modified_On = DateTime.Now;
                    objBankStatementRow.Modified_By = 0;
                    objBankStatementRow.Modified_On = DateTime.Now;
                    objS3G_ORG_BankStatementTable.AddS3G_ORG_Bank_Statement_Data_CaptureRow(objBankStatementRow);


                    intErrCode = objBankStatement_LOVClient.FunPubCreateBankStatemntData(SerMode, ClsPubSerialize.Serialize(objS3G_ORG_BankStatementTable, SerMode), 0);
                    //if (intErroBank == 0)
                    //    intErrCode = objBankStatement_LOVClient.FunPubCreateBankStatemntListValues(SerMode, ClsPubSerialize.Serialize(objS3G_ORG_ListXMLTable, SerMode));

                    if (intErrCode == 0)
                    {

                        strAlert = "Bank Statement Data Capture added successfully";
                        if (strMode != "M")
                        {
                            //Added by Thangam M on 18/Oct/2012 to avoid double save click
                            btnSave.Enabled = false;
                            //End here

                            strAlert += @"\n\nWould you like to add one more Bank Statement Data Capture  ?";
                            strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            strRedirectPageView = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                            lblErrorMessage.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                        }
                        else
                        {
                            if (intErrCode == 0)
                            {
                                if (intBankCaptureID > 0)
                                {
                                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                                    btnSave.Enabled = false;
                                    //End here

                                    Utility.FunShowAlertMsg(this.Page, "Bank Statement Data Capture updated successfully", "../Origination/S3GOrgBankStatement_View.aspx");
                                }

                            }
                            else
                            {
                                strAlert = strAlert.Replace("__ALERT__", "Error adding List values and Bank Statement Data Capture ");
                            }
                        }
                    }
                    else
                    {
                        strAlert = strAlert.Replace("__ALERT__", "List Values for the  Bank Statement Data Capture already exists");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
                    }

   

                    //FunLOBProductLoadGrid(intCompanyID, intUserID);
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = ex.Message;
                }
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
        finally
        {
            objBankStatement_LOVClient.Close();
        }
    }
    #endregion

    protected void ddlLOB_SelectIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string strLOBIDValue = ddlLOB.SelectedValue;
            if (intBankCaptureID == 0)
            {
                if (strLOBIDValue != "0")
                {
                    ddlProductCode.Enabled = ddlConstitution.Enabled = true;
                    FunLoadProductCode();
                    FunLoadConstitutionCode();
                    //added by saranya on 16-Feb-2012 to bind Document path from Document Path Setup
                    FunPriBindDocumentPath();
                    //End
                }
                else
                {
                    ddlConstitution.Items.Clear();
                    ddlProductCode.Items.Clear();
                    ddlProductCode.Enabled = ddlConstitution.Enabled = false;
                    txtScanPath.Text = string.Empty;
                }
            }

            ddlLOB.Focus();
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }
    private void DummyRow()
    {
        try
        {
            DataRow dr;
            DataTable dtGrid;

            if (ViewState["GRIDROW"] == null)
            {
                dtGrid = new DataTable();

                DataColumn dcSerialAdd = new DataColumn("Bank_Stmt_LOV_ID");
                dtGrid.Columns.Add(dcSerialAdd);

                DataColumn dcListValuesAdd = new DataColumn("Field_Name");
                dtGrid.Columns.Add(dcListValuesAdd);

                DataColumn dcFromAdd = new DataColumn("From_Position");
                dtGrid.Columns.Add(dcFromAdd);

                DataColumn dcToAdd = new DataColumn("To_Position");
                dtGrid.Columns.Add(dcToAdd);
            }
            else
            {
                dtGrid = (DataTable)ViewState["GRIDROW"];
            }
            dr = dtGrid.NewRow();

            dr["Bank_Stmt_LOV_ID"] = "";
            dr["Field_Name"] = "";
            dr["From_Position"] = "";
            dr["To_Position"] = "";

            dtGrid.Rows.Add(dr);
            grvListValues.DataSource = dtGrid;
            grvListValues.DataBind();
            ViewState["GRIDROW"] = dtGrid;
            grvListValues.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }
       
   #region "Loading the LOB, Product and Constitution for the respective Bank list values"

    public void FunLOBProductLoadGrid(int intCompanyID, int intUserID)
    {
        objBankStatement_LOVClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        DataTable dtLOBProductTable = new DataTable();
        try
        {
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteObjCaptureData = objBankStatement_LOVClient.FunPubQueryBankStatemntCaptureData(intCompanyID, intUserID);
            dtLOBProductTable = (DataTable)ClsPubSerialize.DeSerialize(byteObjCaptureData, SerializationMode.Binary, typeof(DataTable));

            grvLOBProduct.DataSource = dtLOBProductTable;
            grvLOBProduct.DataBind();

            if (dtLOBProductTable.Rows.Count == 0)
            {
                trCopyProfileMessage.Visible = true;
            }
            else
            {
                trCopyProfileMessage.Visible = false;
            }

        }

        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            dtLOBProductTable.Dispose();
            dtLOBProductTable = null;
            objBankStatement_LOVClient.Close();
        }

    }

    #endregion

    #region "Copying existent list values for new set of LOB, Product and Constitution"

    protected void grvLOBProduct_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkStatementCaptured = (CheckBox)e.Row.FindControl("chkStatementCaptured");
                if (chkStatementCaptured != null)
                    chkStatementCaptured.Checked = false;
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }
    protected void grvListOfValues_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtListValues = (TextBox)e.Row.FindControl("txtListValues");
                TextBox txtFrom = (TextBox)e.Row.FindControl("txtFrom");
                TextBox txtTo = (TextBox)e.Row.FindControl("txtTo");

                if (ViewState["ListValues"] != null)
                {
                    if (e.Row.RowIndex != ((DataTable)ViewState["ListValues"]).Rows.Count - 1)
                    {
                        if (!(PageMode == PageModes.Modify && ((DataTable)ViewState["ListValues"]).Rows.Count == 1))
                        {
                            LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                            btnRemove.Enabled = false;
                            btnRemove.OnClientClick = "";
                        }
                    }
                    else
                    {
                        if (PageMode == PageModes.Modify && ((DataTable)ViewState["ListValues"]).Rows.Count == 1)
                        {
                            LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                            btnRemove.Enabled = false;
                            btnRemove.OnClientClick = "";
                        }
                    }
                }

                Label lblFromCopy = (Label)e.Row.FindControl("lblFromCopy");
                if (lblFromCopy.Text == "1")
                {
                    LinkButton btnRemove = (LinkButton)e.Row.FindControl("btnRemove");
                    btnRemove.Enabled = false;
                    btnRemove.OnClientClick = "";
                }

                if (strMode == "Q")
                {
                    txtListValues.ReadOnly = true;
                    txtFrom.ReadOnly = true;
                    txtTo.ReadOnly = true;

                }
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }


    /// <summary>
    /// To select the LOB,Product,Constitution for which the List needs to be copied
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>


    protected void chkStatementCaptured_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtRoleCode = null;
            int intBank_ID = 0;
            CheckBox chkLOBProduct = null;
            foreach (GridViewRow grvLOBRow in grvLOBProduct.Rows)
            {
                chkLOBProduct = ((CheckBox)grvLOBRow.FindControl("chkStatementCaptured"));
                if (chkLOBProduct.Checked)
                {
                    intBank_ID = Convert.ToInt32(((Label)grvLOBRow.FindControl("lblBankID")).Text);
                }
                else
                    chkLOBProduct.Enabled = false;
            }
            FunLOBProductLoadGrid(intCompanyID, intUserID);
            if (intBank_ID == 0)
            {
                FunLoadGridInsert();
                trLOBProduct.Style.Add("visibility", "hidden");
            }
            else
            {
                FunctionLoadListValues(intBank_ID);

            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }       
    }

    #endregion

   

    public void FunctionLoadListValues(int intBank_ID)
    {
        objBankStatement_LOVClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        DataTable dtTable = new DataTable();
        try
        {
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteObjCaptureData = objBankStatement_LOVClient.FunPubQueryBankStatemntCopyListValues(intBank_ID);
            dtTable = (DataTable)ClsPubSerialize.DeSerialize(byteObjCaptureData, SerializationMode.Binary, typeof(DataTable));

            if (!dtTable.Columns.Contains("FromCopy"))
            {
                dtTable.Columns.Add("FromCopy");
            }
            if (dtTable.Rows.Count > 0)
            {
                txtScanPath.Text = dtTable.Rows[0]["FlatFile_Path"].ToString();
                for (int i = 0; i <= dtTable.Rows.Count - 1; i++)
                {
                    dtTable.Rows[i]["FromCopy"] = "1";
                }
                grvListValues.DataSource = dtTable;
                grvListValues.DataBind();
                ViewState["ListValues"] = dtTable;
                grvListValues.Columns[0].Visible = false;
            }
            else
                FunLoadGridInsert();

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objBankStatement_LOVClient.Close();
        }

    }


    /// <summary>
    /// Storing the Values of the grid as XML documnet for bulk insert in the Data base
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    #region "XML Document Geneartion"

    private void FunGenerateXMLListOfValues(int intBankCapture_ID)
    {
        try
        {
            string strBankID = Convert.ToString(intBankCapture_ID);
            string strLOVID = string.Empty;
            string strFieldName = string.Empty;
            string strFrom = string.Empty;
            string strTo = string.Empty;
            string strLOVIDA = string.Empty;
            string strFieldNameA = string.Empty;
            string strFromA = string.Empty;
            string strToA = string.Empty;

            DataTable dtTable = new DataTable();
            dtTable = (DataTable)ViewState["ListValues"];

            strbListOfValues.Append("<Root>");

            foreach (GridViewRow grvListRow in grvListValues.Rows)
            {
                if (((TextBox)grvListRow.FindControl("txtListValues")).Text != string.Empty)
                {
                    strLOVID = ((Label)grvListRow.FindControl("txtSerial")).Text;
                    strFieldName = ((TextBox)grvListRow.FindControl("txtListValues")).Text;
                    strFrom = ((TextBox)grvListRow.FindControl("txtFrom")).Text;
                    strTo = ((TextBox)grvListRow.FindControl("txtTo")).Text;

                    strbListOfValues.Append("<Details Bank_Stmt_Data_Capture_ID='" + strBankID + "' Bank_Stmt_LOV_ID='" + strLOVID + " ' Field_Name='"
                            + strFieldName + "' From_Position='" + strFrom + "' To_Position='" + strTo + "' /> ");
                }
            }

            //if (((TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd")).Text != string.Empty)
            //{
            //    strLOVID = ((TextBox)grvListValues.FooterRow.FindControl("txtSerialAdd")).Text;
            //    strLOVID = "0";
            //    strFieldName = ((TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd")).Text;
            //    strFrom = ((TextBox)grvListValues.FooterRow.FindControl("txtFromAdd")).Text;
            //    strTo = ((TextBox)grvListValues.FooterRow.FindControl("txtToAdd")).Text;

            //    strbListOfValues.Append("<Details Bank_Stmt_Data_Capture_ID='" + strBankID + "' Bank_Stmt_LOV_ID='" + strLOVID + " ' Field_Name='"
            //      + strFieldName + "' From_Position='" + strFrom + "' To_Position='" + strTo + "' /> ");
            //}

            strbListOfValues.Append("</Root>");

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion


    protected void RBDownloadBankStatement_CheckedChanged(object sender, EventArgs e)
    {
        //if (RBDownloadBankStatement.Checked)
        //{
        //    lblScanPath.Text = "Download Location Path";
        //}
        //else
        //{
        //    lblScanPath.Text = "Scan Location Path";
        //}
    }
    protected void RBScanBankStatement_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtScanPath.Style.Add("visibility", "visble");
            //trFile.Style.Add("visibility", "hidden");
            //fileStatement.Style.Add("visibility", "hidden");
            ViewState["Download"] = 0;
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.Text = ae.Message;
            throw ae;
        }
    }

    #region "File Upload"

    public void FunFileLoad()
    {
        try
        {
            if (RBDownloadBankStatement.Checked == true)
            {
                //AjaxControlToolkit.AsyncFileUpload fileStatement = (AjaxControlToolkit.AsyncFileUpload)UpdatePanel1.FindControl("fileStatement");
                //if (fileStatement.HasFile)
                //{
                //    try
                //    {
                //        System.Text.RegularExpressions.Regex strFileValidationExpression = new System.Text.RegularExpressions.Regex(@"\.txt$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                //        string strFileName = fileStatement.FileName;
                //        string strApplicationPath = Server.MapPath(".");
                //        string strFilePath = strApplicationPath.Replace("\\Origination", "") + "\\Data\\Bank Statement\\" + strFileName;

                //        if (!(Directory.Exists(strApplicationPath.Replace("\\Origination", "") + "\\Data\\Bank Statement")))
                //        {
                //            Directory.CreateDirectory(strApplicationPath.Replace("\\Origination", "") + "\\Data\\Bank Statement");
                //        }

                //        if (!(strFileValidationExpression.IsMatch(strFilePath)))
                //        {
                //            lblErrorMessage.Text = "Select only .txt file";
                //        }
                //        else if (System.IO.File.Exists(strFilePath))
                //        {
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Already the same file name(" + fileStatement.FileName + ") exists in the target path');", true);
                //            return;
                //        }
                //        else
                //        {

                //            fileStatement.PostedFile.SaveAs(strFilePath);
                //            ViewState["FilePath"] = strFilePath;
                //            ViewState["Download"] = 1;
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Select", "alert('Select a file to upload');", true);
                return;
            }
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }



    //protected void custFile_ServerValidate(object sender, ServerValidateEventArgs args)
    //{

    //    if (intBankCaptureID == 0)
    //    {
    //        if (txtScanPath.Text == string.Empty && fileStatement.HasFile == false)
    //        {
    //            args.IsValid = false;
    //            custFile.ErrorMessage = "Please select a source Path";
    //        }
    //        else
    //        {
    //            args.IsValid = true;
    //        }
    //    }
    //}

    #endregion

    #region ValueDisable
    /// <summary>
    ///Access Rights
    /// </summary
    private void FunPriDisableControls(int intModeID)
    {
        //if (!bQuery)
        //{
        //    Response.Redirect(strRedirectPage,false);
        //}

        //if ((!bCreate) || (!bModify))
        //{
        //    btnSave.Enabled = false;
        //}
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
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    lnkCopyProfile.Enabled = false;
                    break;

                //txtConstitution.Enabled = true; // txtLOB.Enabled = txtProduct.Enabled 


                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }

                    lnkCopyProfile.Visible = false;
                    btnGo.Visible = false;
                    ddlLOB.Enabled = false;
                    ddlProductCode.Enabled = false;
                    ddlConstitution.Enabled = false;
                    divBankStatement.Visible = true;
                    btnClear.Enabled = false;

                    
                    RequiredFieldValidator rqvListValue = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvListValue");
                    RequiredFieldValidator rqvFrom = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvFrom");
                    RequiredFieldValidator rqvTo = (RequiredFieldValidator)grvListValues.FooterRow.FindControl("rqvTo");
                    rqvListValue.Enabled = rqvFrom.Enabled = rqvTo.Enabled = false;
                    RBDownloadBankStatement.Enabled = RBScanBankStatement.Enabled =true;

                    //txtConstitution.Enabled = false; // txtLOB.Enabled = txtProduct.Enabled = 

                    break;

                case -1:// Query Mode
                    //FunGetProductDet();

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    grvListValues.FooterRow.Visible = false;
                    grvListValues.Columns[4].Visible = false;
                    RBScanBankStatement.Enabled = false;
                    RBDownloadBankStatement.Enabled = false;
                    txtScanPath.ReadOnly =true ;
                    btnSave.Enabled = btnClear.Enabled = false;
                    lnkCopyProfile.Visible = false;
                    divBankStatement.Visible = true;
                    ddlProductCode.Enabled = ddlConstitution.Enabled = true;
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        ddlProductCode.ClearDropDownList();
                        ddlConstitution.ClearDropDownList();
                    }
                    btnGo.Visible = false;
                   
                    break;
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }
    //grvListValues.Columns[4].Visible =  lnkCopyProfile.Visible =
    //trFile.Visible = trOptions.Visible = grvListValues.FooterRow.Visible = false;

    //Modified By :  Thangam M  -- based on TC_109

    //for (int i = 0; i <= grvListValues.Rows.Count - 1; i++)
    //{
    //    TextBox txtListValuesAdd = (TextBox)grvListValues.Rows[i].FindControl("txtListValues");
    //    TextBox txtFromAdd = (TextBox)grvListValues.Rows[i].FindControl("txtFrom");
    //    TextBox txtToAdd = (TextBox)grvListValues.Rows[i].FindControl("txtTo");

    //    txtListValuesAdd.ReadOnly = txtFromAdd.ReadOnly = txtToAdd.ReadOnly = true;
    //}

    #endregion


    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_Id", CompanyId);
            dictParam.Add("@Lob_Id", ddlLOB.SelectedValue);
            dictParam.Add("@Constitution_Id", ddlConstitution.SelectedValue);
            if (ddlProductCode.SelectedIndex != 0)
                dictParam.Add("@Product_Id", ddlProductCode.SelectedValue);
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetBankCombinationDetails", dictParam);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CB"].ToString() != "0")
                {
                    Utility.FunShowAlertMsg(this, "Selected combination already exist");
                    return;
                }
                else
                {
                    lnkCopyProfile.Enabled = true;

                    divBankStatement.Visible = true;
                    btnSave.Visible = true;
                    btnClear.Visible = true;

                    ViewState["CopyProfile"] = "New";

                    FunLoadGridInsert();

                    //DataTable d = (DataTable)ViewState["GRIDROW"];
                    //d.Rows.Clear();
                    foreach (GridViewRow grv in grvListValues.Rows)
                    {
                        CheckBox chkStatementCaptured = (CheckBox)grv.FindControl("chkStatementCaptured");
                        if (chkStatementCaptured != null)
                            chkStatementCaptured.Checked = false;
                    }
                }

                TextBox txtListValues = (TextBox)grvListValues.FooterRow.FindControl("txtListValuesAdd");
                txtListValues.Focus();
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
        finally
        {
            //dtListValues.Dispose();
            dtListValues = null;            
        }
    }

    
}   


       

       

