
//Module Name      :   Origination
//Screen Name      :   S3GORGPRDDC_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   01-JUN-2010
//Purpose          :   To insert and update PRDDC code details

//Modified By      :    Thangam M
//Modified On      :    16-SEP-2010 

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Security;
using System.Configuration;
using System.Linq;

public partial class S3GORGPRDDCMaster_Add : ApplyThemeForProject
{
    #region Intialization

    PRDDCMgtServicesReference.PRDDCMgtServicesClient ObjPRDDCMasterClient;
    PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    static int intRowCount = 0;

    int intPRDDCID = 0;
    int intUserID = 0;
    int intCompanyID = 0;

    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    Dictionary<string, string> Procparam = null;
    string strXMLPRDDCDocumentsDet = "<Root><Details Doc_Cat_ID='0' /></Root>";
    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbPRDDCDocumentsDet = new StringBuilder();
    string strRedirectPage = "../Origination/S3GORGPRDDCMaster_View.aspx";
    string strKey = "Insert";
    string strPgmName = "S3G_ORG_GetWorkflowProgramMaster";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGPRDDCMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGPRDDCMaster_View.aspx';";
    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            lblErrorMessage.Text = "";

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intPRDDCID = Convert.ToInt32(fromTicket.Name);
                }
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


            if (!IsPostBack)
            {
                //<<Performance>>
                if (PageMode == PageModes.Create)
                {
                    FunPriBindLOBProduct();
                    //FunPriBindProgram();
                }
                DummyRow();
                FunGetPRDDCDetails();
                
                ddlConstitution.Items.Add(new ListItem("--Select--", "0"));
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if ((intPRDDCID > 0) && (strMode == "M"))
                {
                    //<<Performance>>
                    //modifyPRDDCDetails();
                    FunPriDisableControls(1);
                    txtScanPath.Focus();
                }
                else if ((intPRDDCID > 0) && (strMode == "Q")) // Query // Modify
                {
                    //modifyPRDDCDetails();
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                    lnkCopyProfile.Enabled = false;
                    divPRDDC.Visible = false;
                    btnSave.Visible = false;
                    btnClear.Visible = false;
                    ddlLOB.Focus();
                }
                //Code end
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }


    #endregion

    #region Page Events

    /// <summary>
    /// This is used to save PRDDC details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            if (txtScanPath.Text.Trim() != "")
            {
                //string[] strScan = txtScanPath.Text.Split('/');c

                //string[]  strScan = txtScanPath.Text.Replace("/", "\\").Split('\\');              
                string[] strScan = txtScanPath.Text.Trim().Split('\\');
                var matchQuery = from word in strScan
                                 where word == ""
                                 select word;

                // Count the matches.
                int wordCount = matchQuery.Count();

                if (wordCount >= 2 || txtScanPath.Text.Trim().Contains('/'))
                {
                    Utility.FunShowAlertMsg(this.Page, "Invalid scan location path");
                    return;
                }

            }

            if (txtScanPath.Text.Trim() != "" && !Directory.Exists(txtScanPath.Text.Trim()))
            {
                Utility.FunShowAlertMsg(this.Page, "Invalid scan location path");
                return;
            }

            bool rowCnt = false;
            bool chkMan = false;
            bool chk = false;
            foreach (GridViewRow grvData in grvPRDDCDocuments.Rows)
            {
                if (grvData.Visible)
                {
                    rowCnt = true;
                    CheckBox chkImageCopy = (CheckBox)grvData.FindControl("chkImageCopy");
                    CheckBox chkOptMan = (CheckBox)grvData.FindControl("chkOptMan");
                    if (chkOptMan.Checked) chkMan = true;
                    if (chkImageCopy.Checked)
                    {
                        chk = true;
                        //break;
                    }
                }
            }
            if (rowCnt == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one row to save");
                return;
            }
            if (chkMan == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one mandatory PRDDc document to save");
                return;
            }
            if (txtScanPath.Text.Trim() == "" && chk)
            {
                Utility.FunShowAlertMsg(this.Page, "Define the Scan Location Path in Document path setup");
                return;
            }

            ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
            ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPRDDCRow.Company_ID = intCompanyID;
            ObjPRDDCRow.PRDDC_ID = intPRDDCID;
            ObjPRDDCRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjPRDDCRow.Product_ID = Convert.ToInt32(ddlProductCode.SelectedValue);
            ObjPRDDCRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
            ObjPRDDCRow.DocPath = txtScanPath.Text.Trim();//.Replace("/", "\\").Trim();
            ObjPRDDCRow.Created_By = intUserID;
            FunPriGeneratePRDDCDocDet();
            ObjPRDDCRow.XMLPRDDCDocumentsDet = strXMLPRDDCDocumentsDet;

            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

            intErrCode = ObjPRDDCMasterClient.FunPubCreatePRDDCDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here


                if (intPRDDCID > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "PRDDC details updated successfully", strRedirectPage);
                }
                else
                {
                    strAlert = "PRDDC details added successfully";
                    strAlert += @"\n\nWould you like to add one more PRDDC?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "PRDDC details already exist");
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "PRDDC details cannot be updated.Transaction exists");
                return;
            }
            //else if (intErrCode == 2)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create PRDDC Code");
            //}
            lblErrorMessage.Text = string.Empty;

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
            ObjPRDDCMasterClient.Close();
        }
    }


    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    protected void grvPRDDC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grvPRDDC = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSel");
            chkBoxSelect.Attributes["onclick"] = string.Format
                                      (
                                         "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                         grvPRDDC.ClientID
                                     );
        }
    }

    /// <summary>
    /// checkbox validation for PRDDCal Documents
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    protected void grvPRDDCDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDocCatID = (Label)e.Row.FindControl("lblDocCatID");
                Label lblPgmID = (Label)e.Row.FindControl("lblPgmID");
                //CheckBox chkSel = (CheckBox)e.Row.FindControl("chkSel");
                DropDownList ddlPgmID = (DropDownList)e.Row.FindControl("ddlPgmID");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                Label lblOptMan = (Label)e.Row.FindControl("lblOptMan");
                Label lblImageCopy = (Label)e.Row.FindControl("lblImageCopy");
                CheckBox chkOptMan = (CheckBox)e.Row.FindControl("chkOptMan");
                CheckBox chkImageCopy = (CheckBox)e.Row.FindControl("chkImageCopy");
                LinkButton lnkRemovePRDDC = (LinkButton)e.Row.FindControl("lnkRemovePRDDC");

                ddlPgmID.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                ddlPgmID.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");

                //BindDataTable(ddlPgmID, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" }); - kuppu
                BindDataTable(ddlPgmID, new string[] { "Program_ID", "Program_Name" });


                //if (lblOptMan.Text == "True") chkOptMan.Checked = true;
                //if (lblImageCopy.Text == "True") chkImageCopy.Checked = true;

                //  Modified by SENTHILKUMAR P - PSK for Oracle

                if (lblOptMan.Text.Trim() == "True" || lblOptMan.Text.Trim() == "1") chkOptMan.Checked = true;
                if (lblImageCopy.Text.Trim() == "True" || lblImageCopy.Text.Trim() == "1") chkImageCopy.Checked = true;

                //  Modified by SENTHILKUMAR P -- PSK for Oracle

                ddlPgmID.SelectedValue = lblPgmID.Text;

                //Commented by Thangam M on 14/Mar/2012 to fix bug id - 5481
                //if (strMode == "M")
                //{
                //    if (intRowCount > grvPRDDCDocuments.Rows.Count)
                //    {
                //        lnkRemovePRDDC.Enabled = false;
                //    }
                //}
                //End here

                //ddlPgmID.AddItemToolTip();
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtPRDDCDesc = (TextBox)e.Row.FindControl("txtPRDDCDesc");
                txtPRDDCDesc.Attributes.Add("onkeypress", "javascript:if(this.value.toUpperCase()!='--SELECT--' && this.value!='')fnCheckSpecialCharsStartWithAlphabets(true,this.value);");
                txtPRDDCDesc.Attributes.Add("onblur", "javascript:if(this.value.toUpperCase()!='--SELECT--' && this.value!='' )fnCheckSingleAlphabets(this.value,this,'Description'); else this.value='--Select--'");
                txtPRDDCDesc.Attributes.Add("onfocus", "if(this.value.toUpperCase()=='--SELECT--')this.value=''");

                DropDownList ddlPRDDCType = (DropDownList)e.Row.FindControl("ddlPRDDCType");
                DropDownList ddlPRDDCDesc = (DropDownList)e.Row.FindControl("ddlPRDDCDesc");
                DropDownList ddlFooterPgmID = (DropDownList)e.Row.FindControl("ddlFooterPgmID");

                //<<Performance>>
                if (PageMode != PageModes.Query)
                {
                    Procparam = new Dictionary<string, string>();
                    DataSet dsPRDDCDesc = Utility.GetTableValues(SPNames.S3G_ORG_GetPRDDC_DocumentCategory, Procparam);

                    ddlPRDDCType.DataSource = dsPRDDCDesc.Tables[1];
                    ddlPRDDCType.DataTextField = "PRDDC_Doc_Type";
                    ddlPRDDCType.DataValueField = "PRDDC_Doc_Type";
                    ddlPRDDCType.DataBind();
                    ddlPRDDCType.Items.Insert(0, new ListItem("--Select--", "0"));
                    ddlPRDDCType.AddItemToolTip();
                }

                //ddlPRDDCType.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                //ddlPRDDCType.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");
                BindDataTable(ddlFooterPgmID, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" });
                ddlFooterPgmID.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                ddlFooterPgmID.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");
                //ddlFooterPgmID.AddItemToolTip();   

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void grvPRDDC_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView grvPRDDC = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSel");
            chkBoxSelect.Attributes["onclick"] = string.Format
                                      (
                                         "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                         grvPRDDC.ClientID
                                     );
        }
    }


    /// <summary>
    /// This is used to clear data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text.Trim();
            if (ddlProductCode.Items.Count > 0)
            {
                ddlProductCode.SelectedIndex = 0;
                //ddlProductCode.ToolTip = ddlProductCode.SelectedItem.Text.Trim();
            }
            if (ddlConstitution.Items.Count > 0)
            {
                ddlConstitution.SelectedIndex = 0;
                //ddlConstitution.ToolTip = ddlConstitution.SelectedItem.Text.Trim();
            }

            txtScanPath.Text = "";
            hdnPRDDC.Value = "0";
            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();
            //DummyRow();
            //grvPRDDCDocuments.Columns[grvPRDDCDocuments.Columns.Count - 1].Visible = true;            
            grvPRDDCDocuments.ClearGrid();
            btnClear.Visible = btnSave.Visible = false;
            lnkCopyProfile.Enabled = false;
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
    }

    #endregion

    #region Page Methods

    private void FunPriBindProgram()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            ViewState["dtProgram"] = Utility.GetDefaultData(strPgmName, Procparam);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    /// <summary>
    /// to bind LOB and Product details
    /// </summary>
    private void FunPriBindLOBProduct()
    {
        //LOB List
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (PageMode == PageModes.Create)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "25");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.AddItemToolTip();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void ddlLOB_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlLOB.AddItemToolTip();
        ddlLOB.ToolTip = ddlLOB.SelectedItem.Text.Trim();
        FunPriBindProduct();
        ClearPRDDCGrid();
    }

    private void ClearPRDDCGrid()
    {
        if (divPRDDC.Visible)
        {
            divPRDDC.Visible = false;
            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();
            DummyRow();
            btnSave.Visible = false;
            btnClear.Visible = false;
            lnkCopyProfile.Enabled = false;
        }
    }

    private void FunPriBindProduct()
    {
        //Product Code
        try
        {
            ddlConstitution.Items.Clear();
            ddlProductCode.Items.Clear();

            //condition added for bug fixing - Bug id_5469 - Kuppusamy.B
            if (ddlLOB.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                if (PageMode == PageModes.Create)
                {
                    Procparam.Add("@Is_Active", "1");
                }

                //Code changed for bug fixing - Product code not required - Kuppusamy.B - Feb-29-2012
                //ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });
                ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Name" });
                ddlProductCode.AddItemToolTip();

                //Constitution
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                if (PageMode == PageModes.Create)
                {
                    Procparam.Add("@Is_Active", "1");
                }
                DataTable dtConstitutionMaster = Utility.GetDefaultData(SPNames.S3G_Get_ConstitutionMaster, Procparam);

                //code modified get scan path from Doc.Path setup - bug fixing _ Bug_Id - 5469 - Kuppusamy.B - 15/02/2012
                //ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "ConstitutionName" });

                //Code changed for bug fixing - Constitution code not required - Kuppusamy.B - Feb-29-2012
                ddlConstitution.BindDataTable(dtConstitutionMaster, new string[] { "Constitution_ID", "Constitution_Name" });
                if (dtConstitutionMaster.Rows.Count > 0)
                {
                    txtScanPath.Text = Convert.ToString(dtConstitutionMaster.Rows[0]["Document_Path"]);
                }
                txtScanPath.Attributes.Add("Readonly", "Readonly");
                ddlConstitution.AddItemToolTip();
            }
            else
            {
                txtScanPath.Text = "";
                ddlConstitution.Items.Insert(0, new ListItem("--Select--", "--Select--"));
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetPRDDCDetails()
    {
        DataSet dsPRDDC = new DataSet();
        divPRDDC.Style.Add("display", "block");
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
            ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPRDDCRow.Company_ID = intCompanyID;
            ObjPRDDCRow.PRDDC_ID = intPRDDCID;
            ObjPRDDCRow.Created_By = intUserID;

            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

            byte[] bytePRDDCDetails = ObjPRDDCMasterClient.FunPubQueryPRDDCDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));
            dsPRDDC = (DataSet)ClsPubSerialize.DeSerialize(bytePRDDCDetails, SerializationMode.Binary, typeof(DataSet));

            if ((intPRDDCID > 0) && (dsPRDDC.Tables[0].Rows.Count > 0) && hdnPRDDC.Value == "0")
            {
                //<<Performance>>
                ListItem lst;

                lst = new ListItem(dsPRDDC.Tables[0].Rows[0]["LOB_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);
                ddlLOB.SelectedValue = dsPRDDC.Tables[0].Rows[0]["LOB_ID"].ToString();
                //ddlLOB.ToolTip = ddlLOB.SelectedItem.Text.Trim();
                //FunPriBindProduct();
                //if (strMode == "Q")
                //{
                txtScanPath.Text = dsPRDDC.Tables[0].Rows[0]["Document_Path"].ToString();
                //}

                lst = new ListItem(dsPRDDC.Tables[0].Rows[0]["Product_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["Product_ID"].ToString());
                ddlProductCode.Items.Add(lst);

                ddlProductCode.SelectedValue = dsPRDDC.Tables[0].Rows[0]["Product_ID"].ToString();
                //ddlProductCode.ToolTip = ddlProductCode.SelectedItem.Text.Trim();

                lst = new ListItem(dsPRDDC.Tables[0].Rows[0]["Constitution_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["Constitution_ID"].ToString());
                ddlConstitution.Items.Add(lst);

                ddlConstitution.SelectedValue = dsPRDDC.Tables[0].Rows[0]["Constitution_ID"].ToString();
                //ddlConstitution.ToolTip = ddlConstitution.SelectedItem.Text.Trim();
                //txtScanPath.Text = dsPRDDC.Tables[0].Rows[0]["Document_Path"].ToString();
            }

            //<<Performance>>
            if ((dsPRDDC.Tables[1].Rows.Count > 0) && (intPRDDCID == 0))
            {
                grvPRDDC.DataSource = dsPRDDC.Tables[1];
                grvPRDDC.DataBind();
                trCopyProfileMessage.Visible = false;
            }
            else if (dsPRDDC.Tables[1].Rows.Count == 0)
            {
                trCopyProfileMessage.Visible = true;
            }

            //<<Performance>>
            divPRDDC.Style.Add("display", "block");
            divPRDDC.Visible = true;

            if (dsPRDDC.Tables[3] != null)
            {
                ViewState["dtProgram"] = dsPRDDC.Tables[3];
            }

            intRowCount = dsPRDDC.Tables[2].Rows.Count;
            grvPRDDCDocuments.DataSource = dsPRDDC.Tables[2];
            grvPRDDCDocuments.DataBind();
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
            dsPRDDC.Dispose();
            dsPRDDC = null;
            ObjPRDDCMasterClient.Close();
        }
    }

    protected void FunPriGetCopyProfileDetails(object sender, EventArgs e)
    {
        //Modified based on the Test case AH_006
        try
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvRow = null;
            if (chk != null) gvRow = (GridViewRow)chk.Parent.Parent;

            if (chk.Checked && gvRow != null)
            {
                intPRDDCID = Convert.ToInt32(((Label)gvRow.FindControl("lblPRDDCID")).Text);
                ddlLOB.Attributes.Add("LOB_ID", ((Label)gvRow.FindControl("lblLOBID")).Text);
                ddlProductCode.Attributes.Add("Product_ID", ((Label)gvRow.FindControl("lblProductID")).Text);
                hdnPRDDC.Value = intPRDDCID.ToString();
                ViewState["CopyProfile"] = "COPY";
            }

            if (chk.Checked)
            {
                modifyPRDDCDetails();
            }

            if (chk.Checked)
            {
                foreach (GridViewRow grv in grvPRDDCDocuments.Rows)
                {
                    LinkButton lnkRemovePRDDC = (LinkButton)grv.FindControl("lnkRemovePRDDC");
                    if (chk.Checked) lnkRemovePRDDC.Visible = false; //lnkRemovePRDDC.Enabled = false;
                }
                grvPRDDCDocuments.FooterRow.Visible = false;
                grvPRDDCDocuments.Columns[grvPRDDCDocuments.Columns.Count - 1].Visible = false;
            }
            else
            {
                grvPRDDCDocuments.Columns[grvPRDDCDocuments.Columns.Count - 1].Visible = true;
                if (hdnPRDDC.Value == ((Label)gvRow.FindControl("lblPRDDCID")).Text)
                {
                    DataTable dt = (DataTable)ViewState["GRIDROW"];
                    dt.Rows.Clear();
                    DummyRow();
                    foreach (GridViewRow grv in grvPRDDC.Rows)
                    {
                        CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                        chkSel.Checked = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    private void FunPriGeneratePRDDCDocDet()
    {
        try
        {
            string strCatID = string.Empty;
            string strOptMan = string.Empty;
            string strImageCopy = string.Empty;
            string strRemarks = string.Empty;

            //CheckBox chkPRDDCDoc = null;
            DropDownList ddlPgmID = null;

            strbPRDDCDocumentsDet.Append("<Root>");

            foreach (GridViewRow grvPRDDCDoc in grvPRDDCDocuments.Rows)
            {
                //chkPRDDCDoc = ((CheckBox)grvPRDDCDoc.FindControl("chkSel"));
                //if (chkPRDDCDoc.Checked)
                if (grvPRDDCDoc.Visible)
                {
                    ddlPgmID = ((DropDownList)grvPRDDCDoc.FindControl("ddlPgmID"));
                    strCatID = ((Label)grvPRDDCDoc.FindControl("lblDocCatID")).Text.Trim();
                    strOptMan = ((CheckBox)grvPRDDCDoc.FindControl("chkOptMan")).Checked == true ? "1" : "0";
                    //if (strOptMan == "1")
                    strImageCopy = ((CheckBox)grvPRDDCDoc.FindControl("chkImageCopy")).Checked == true ? "1" : "0";
                    //else
                    //    strImageCopy = "0";
                    strRemarks = ((TextBox)grvPRDDCDoc.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");

                    strbPRDDCDocumentsDet.Append(" <Details Doc_Cat_ID='" + strCatID + "' Doc_Cat_OptMan='" + strOptMan + "'");
                    strbPRDDCDocumentsDet.Append(" Doc_Cat_ImageCopy='" + strImageCopy + "' Program_ID='" + ddlPgmID.SelectedValue + "' Doc_Cat_Remarks='" + strRemarks + "' /> ");
                }
            }
            strbPRDDCDocumentsDet.Append("</Root>");
            strXMLPRDDCDocumentsDet = strbPRDDCDocumentsDet.ToString();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    //This is used to implement User Authorization

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
                        // btnSaveDocType.Enabled = false;
                        //grvPRDDCDocuments.FooterRow.Visible = false;
                        btnSave.Enabled = false;
                    }
                    break;

                case 1: // Modify Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                    if (!bModify)
                    {
                        //btnSaveDocType.Enabled = false;

                        btnSave.Enabled = false;
                    }
                    //grvPRDDCDocuments.FooterRow.Visible = false;
                    lnkCopyProfile.Visible = false;
                    btnGo.Visible = false;
                    ddlLOB.Enabled = false;
                    ddlProductCode.Enabled = false;
                    ddlConstitution.Enabled = false;
                    txtScanPath.ReadOnly = true;
                    btnClear.Enabled = false;

                    break;

                case -1:// Query Mode

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //lnkAddOtherDoc.Visible = false;
                    grvPRDDCDocuments.FooterRow.Visible = false;
                    grvPRDDCDocuments.Columns[grvPRDDCDocuments.Columns.Count - 1].Visible = false;
                    lnkCopyProfile.Visible = false;
                    txtScanPath.ReadOnly = true;
                    // panCategoryType.Visible = false;
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        ddlProductCode.ClearDropDownList();
                        ddlConstitution.ClearDropDownList();
                    }
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    btnGo.Visible = false;
                    fnGridPRDDC_CtrlDisable();
                    break;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    private void fnGridPRDDC_CtrlDisable()
    {
        try
        {
            foreach (GridViewRow grv in grvPRDDCDocuments.Rows)
            {
                TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                //CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                LinkButton lnkRemovePRDDC = (LinkButton)grv.FindControl("lnkRemovePRDDC");
                lnkRemovePRDDC.Enabled = chkOptMan.Enabled = chkImageCopy.Enabled = false;
                txtRemarks.ReadOnly = true;

                if (bClearList)
                {
                    ddlPgmID.ClearDropDownList();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }


    //Code end

    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {
        divPRDDC.Visible = false;
        grvPRDDCDocuments.Columns[grvPRDDCDocuments.Columns.Count - 1].Visible = true;
        if (ddlLOB.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert16", "alert('Select the LOB');", true);
            return;
        }
        if (ddlConstitution.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert17", "alert('Select the Constitution');", true);
            return;
        }

        DataTable dtPRDDC = new DataTable();
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRows;
            ObjPRDDCRows = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPRDDCRows.Company_ID = intCompanyID;
            ObjPRDDCRows.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjPRDDCRows.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
            if (Convert.ToInt32(ddlProductCode.SelectedValue) > 0)
            {
                ObjPRDDCRows.Product_ID = Convert.ToInt32(ddlProductCode.SelectedValue);
            }
            else
                ObjPRDDCRows.Product_ID = 0;
            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRows);

            byte[] bytePRDDCDetails = ObjPRDDCMasterClient.FunPubQueryPRDDCMasterCombi(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));
            dtPRDDC = (DataTable)ClsPubSerialize.DeSerialize(bytePRDDCDetails, SerializationMode.Binary, typeof(DataTable));
            if (dtPRDDC.Rows.Count > 0)
            {
                if (dtPRDDC.Rows[0]["CNT"].ToString() != "0")//) '0' means selected combination not exist
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert18", "alert('Selected combination already exist');", true);
                    return;
                }
                else
                {
                    lnkCopyProfile.Enabled = true;

                    divPRDDC.Visible = true;
                    btnSave.Visible = true;
                    btnClear.Visible = true;

                    ViewState["CopyProfile"] = "NEW";

                    DataTable dt = (DataTable)ViewState["GRIDROW"];
                    dt.Rows.Clear();
                    DummyRow();
                    foreach (GridViewRow grv in grvPRDDC.Rows)
                    {
                        CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                        chkSel.Checked = false;
                    }


                }
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
            dtPRDDC.Dispose();
            dtPRDDC = null;
            ObjPRDDCMasterClient.Close();
        }
    }

    protected void ddlConstitution_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearPRDDCGrid();
        ddlConstitution.AddItemToolTip();
        ddlConstitution.ToolTip = ddlConstitution.SelectedItem.Text.Trim();
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

                DataColumn dcPRDDCType = new DataColumn("PRDDCType");
                dtGrid.Columns.Add(dcPRDDCType);

                DataColumn dcPRDDCDesc = new DataColumn("PRDDCDesc");
                dtGrid.Columns.Add(dcPRDDCDesc);

                DataColumn dcDoc_Cat_OptMan = new DataColumn("Doc_Cat_OptMan");
                dcDoc_Cat_OptMan.DataType = System.Type.GetType("System.Boolean");
                dtGrid.Columns.Add(dcDoc_Cat_OptMan);

                DataColumn dcDoc_Cat_ID = new DataColumn("Doc_Cat_ID");
                dtGrid.Columns.Add(dcDoc_Cat_ID);

                DataColumn dcDoc_Cat_IDAssigned = new DataColumn("Doc_Cat_IDAssigned");
                dtGrid.Columns.Add(dcDoc_Cat_IDAssigned);

                DataColumn dcDoc_Cat_ImageCopy = new DataColumn("Doc_Cat_ImageCopy");
                dcDoc_Cat_ImageCopy.DataType = System.Type.GetType("System.Boolean");
                dtGrid.Columns.Add(dcDoc_Cat_ImageCopy);

                DataColumn dcProgram_ID = new DataColumn("Program_ID");
                dtGrid.Columns.Add(dcProgram_ID);

                DataColumn dcRemarks = new DataColumn("Remarks");
                dtGrid.Columns.Add(dcRemarks);
            }
            else
            {
                dtGrid = (DataTable)ViewState["GRIDROW"];
            }
            dr = dtGrid.NewRow();

            dr["PRDDCType"] = "";
            dr["PRDDCDesc"] = "";
            dr["Doc_Cat_OptMan"] = false;
            dr["Doc_Cat_ID"] = "0";
            dr["Doc_Cat_IDAssigned"] = "0";
            dr["Doc_Cat_ImageCopy"] = false;
            dr["Program_ID"] = "0";
            dr["Remarks"] = "";

            dtGrid.Rows.Add(dr);
            grvPRDDCDocuments.DataSource = dtGrid;
            grvPRDDCDocuments.DataBind();
            ViewState["GRIDROW"] = dtGrid;
            grvPRDDCDocuments.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void txtPRDDCDesc_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (System.Web.HttpContext.Current.Session["dsPRDDCDesc"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["dsPRDDCDesc"];
                if (dt.Rows.Count > 0)
                {
                    string filterExpression = "PRDDC_Doc_Description = '" + txtPRDDCDesc.Text + "'";
                    DataRow[] dtSuggestions = dt.Select(filterExpression);
                    if (dtSuggestions.Length > 0)
                    {
                        ViewState["DocID"] = dtSuggestions[0]["PRDDC_Doc_Cat_ID"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] getPRDDCDescription(String prefixText, int count)
    {
        List<String> suggetions = null;
        DataTable dtDesc = new DataTable();
        try
        {
            if (System.Web.HttpContext.Current.Session["dsPRDDCDesc"] != null)
            {
                dtDesc = (DataTable)System.Web.HttpContext.Current.Session["dsPRDDCDesc"];
                suggetions = GetDescription(prefixText, count, dtDesc);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return suggetions.ToArray();
    }

    private static List<String> GetDescription(string key, int count, DataTable dt1)
    {
        List<String> suggestions = new List<string>();
        try
        {
            string filterExpression = "PRDDC_Doc_Description like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);
            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["PRDDC_Doc_Description"].ToString();
                suggestions.Add(suggestion);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return suggestions;
    }

    protected void ddlPRDDCType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["DocID"] = "";
            DropDownList ddlPRDDCType = (DropDownList)sender;
            DropDownList ddlPRDDCDesc = (DropDownList)ddlPRDDCType.Parent.Parent.FindControl("ddlPRDDCDesc");
            TextBox txtPRDDCDesc = (TextBox)ddlPRDDCType.Parent.Parent.FindControl("txtPRDDCDesc");

            txtPRDDCDesc.Text = "--Select--";
            if (ddlPRDDCType.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();
                if (ddlPRDDCType.SelectedValue != "Others")
                    Procparam.Add("@PRDDC_Doc_Type", ddlPRDDCType.SelectedValue.Split('-')[0].ToString());
                else
                    Procparam.Add("@PRDDC_Doc_Type", ddlPRDDCType.SelectedValue);

                DataSet dsPRDDCDesc = Utility.GetTableValues(SPNames.S3G_ORG_GetPRDDC_DocumentCategory, Procparam);

                System.Web.HttpContext.Current.Session["dsPRDDCDesc"] = dsPRDDCDesc.Tables[2];
                txtPRDDCDesc.Enabled = false;
                if (ddlPRDDCType.SelectedValue.ToUpper().Trim() == "OTHERS")
                {
                    txtPRDDCDesc.Enabled = true;
                    ddlPRDDCDesc.Focus();
                }
                else
                {
                    //Changed By Thangam M on 02/Dec/2011

                    DataRow[] dr;
                    if (ddlPRDDCType.SelectedValue != "Others")
                        dr = dsPRDDCDesc.Tables[0].Select("PRDDC_Doc_Type = '" + ddlPRDDCType.SelectedValue.Split('-')[0].ToString() + "' ");
                    else
                        dr = dsPRDDCDesc.Tables[0].Select("PRDDC_Doc_Type + ' - ' + PDDC_Doc_Description = '" + ddlPRDDCType.SelectedValue + "' ");

                    if (dr != null && dr.Count() > 0)
                    {
                        txtPRDDCDesc.Text = dr[0]["PRDDC_Doc_Description"].ToString();
                        ViewState["DocID"] = dr[0]["PRDDC_Doc_Cat_ID"].ToString();
                    }
                }
            }
            ddlPRDDCType.AddItemToolTip();
            ddlPRDDCType.ToolTip = ddlPRDDCType.SelectedItem.Text;
            ddlPRDDCDesc.AddItemToolTip();
            if (ddlPRDDCDesc.Items.Count > 0)
            {
                ddlPRDDCDesc.ToolTip = ddlPRDDCDesc.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void lnkRemovePRDDC_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();

            foreach (GridViewRow grv in grvPRDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                    CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                    CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                    DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                    TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    Label lblPRDDCType = (Label)grv.FindControl("lblPRDDCType");
                    Label lblPRDDCDesc = (Label)grv.FindControl("lblPRDDCDesc");
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");

                    DataRow dr = dt.NewRow();
                    dr["PRDDCType"] = lblPRDDCType.Text.Trim();
                    dr["PRDDCDesc"] = lblPRDDCDesc.Text.Trim();
                    dr["Doc_Cat_OptMan"] = true;//chkOptMan.Checked ? true : false;
                    dr["Doc_Cat_ID"] = lblDocCatID.Text.Trim();
                    dr["Doc_Cat_IDAssigned"] = "1";
                    dr["Doc_Cat_ImageCopy"] = chkImageCopy.Checked ? true : false;
                    dr["Program_ID"] = ddlPgmID.SelectedValue;
                    dr["Remarks"] = txtRemarks.Text.Trim(); ;

                    dt.Rows.Add(dr);
                }
            }

            LinkButton lnkRemovePRDDC = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lnkRemovePRDDC.Parent.Parent;

            dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.RemoveAt(gvRow.RowIndex);
            grvPRDDCDocuments.DataSource = dt;
            grvPRDDCDocuments.DataBind();

            if (dt.Rows.Count == 0)
                DummyRow();

            ViewState["GRIDROW"] = dt;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void lnkAAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Button lnkAAdd = (Button)sender;
            DropDownList ddlPRDDCType = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlPRDDCType");
            DropDownList ddlPRDDCDesc = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlPRDDCDesc");
            TextBox txtPRDDCDesc = (TextBox)lnkAAdd.Parent.Parent.FindControl("txtPRDDCDesc");
            CheckBox chkFootOptMan = (CheckBox)lnkAAdd.Parent.Parent.FindControl("chkFootOptMan");
            CheckBox chkScan = (CheckBox)lnkAAdd.Parent.Parent.FindControl("chkScan");
            DropDownList ddlFooterPgmID = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlFooterPgmID");
            TextBox txtFooterRemarks = (TextBox)lnkAAdd.Parent.Parent.FindControl("txtFooterRemarks");

            if (ddlPRDDCType.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the PRDDC Type");
                ddlPRDDCType.Focus();
                return;
            }
            if (txtPRDDCDesc.Text.Trim() == "" || txtPRDDCDesc.Text.Trim().ToUpper() == "--SELECT--")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the PRDDC other document description.");
                txtPRDDCDesc.Focus();
                return;
            }
            if (ddlFooterPgmID.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Last Program ID");
                ddlPRDDCType.Focus();
                return;
            }
            if (ddlPRDDCType.SelectedItem.Text.Trim().ToUpper() == "OTHERS" && ViewState["DocID"].ToString() == "")
            {
                PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
                PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
                ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

                try
                {
                    ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();
                    ObjPRDDCRow.DocCategory = "";
                    ObjPRDDCRow.DocType = "PRDDC";
                    ObjPRDDCRow.DocDesc = txtPRDDCDesc.Text.Trim();
                    ObjPRDDCRow.Created_By = intUserID;

                    ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

                    intErrCode = ObjPRDDCMasterClient.FunPubCreateOtherDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));

                    if (intErrCode != -1)
                    {
                        Utility.FunShowAlertMsg(this.Page, "PRDDC other document details added successfully");
                        ViewState["DocID"] = intErrCode.ToString();
                        if (Request.QueryString["qsPRDDCId"] != null)
                            intPRDDCID = Convert.ToInt32(Request.QueryString["qsPRDDCId"]);
                    }
                    else if (intErrCode == -1)
                    {
                        Utility.FunShowAlertMsg(this.Page, "PRDDC other document details already exist in data base.");
                    }
                    //else if (intErrCode == 2)
                    //{
                    //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create PRDDC Code");
                    //}
                    lblErrorMessage.Text = string.Empty;

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
                    ObjPRDDCMasterClient.Close();
                }
            }

            foreach (GridViewRow grv in grvPRDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");
                    Label lblPRDDCDesc = (Label)grv.FindControl("lblPRDDCDesc");
                    if (ddlPRDDCType.SelectedItem.Text.Trim().ToUpper() != "OTHERS" && lblDocCatID.Text.Trim() == ViewState["DocID"].ToString())
                    {
                        Utility.FunShowAlertMsg(this.Page, "Selected PRDDC Type already exists in the grid.");
                        return;
                    }
                    if (ddlPRDDCType.SelectedItem.Text.Trim().ToUpper() == "OTHERS")
                    {
                        if (txtPRDDCDesc.Text.Trim().ToLower() == lblPRDDCDesc.Text.Trim().ToLower())
                        {
                            Utility.FunShowAlertMsg(this.Page, "Selected PRDDC Type & Description already exists in the grid.");
                            return;
                        }
                    }
                }
            }


            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();

            foreach (GridViewRow grv in grvPRDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                    CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                    CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                    DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                    TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    Label lblPRDDCType = (Label)grv.FindControl("lblPRDDCType");
                    Label lblPRDDCDesc = (Label)grv.FindControl("lblPRDDCDesc");
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");

                    DataRow dr = dt.NewRow();
                    dr["PRDDCType"] = lblPRDDCType.Text.Trim();
                    dr["PRDDCDesc"] = lblPRDDCDesc.Text.Trim();
                    dr["Doc_Cat_OptMan"] = chkOptMan.Checked ? true : false;
                    dr["Doc_Cat_ID"] = lblDocCatID.Text.Trim();
                    dr["Doc_Cat_IDAssigned"] = "1";
                    dr["Doc_Cat_ImageCopy"] = chkImageCopy.Checked ? true : false;
                    dr["Program_ID"] = ddlPgmID.SelectedValue;
                    dr["Remarks"] = txtRemarks.Text.Trim(); ;

                    dt.Rows.Add(dr);
                }
            }

            if (ViewState["GRIDROW"] != null)
            {
                DataRow dr = dt.NewRow();

                dr["PRDDCType"] = ddlPRDDCType.SelectedValue.Trim();
                //dr["PRDDCDesc"] =  ddlPRDDCType.SelectedValue.Trim().ToUpper()=="OTHERS"? ddlPRDDCDesc.SelectedItem.Text.Trim():txtPRDDCDesc.Text.Trim(); 
                dr["PRDDCDesc"] = txtPRDDCDesc.Text.Trim();
                dr["Doc_Cat_OptMan"] = chkFootOptMan.Checked ? true : false;
                dr["Doc_Cat_ID"] = ViewState["DocID"].ToString();
                dr["Doc_Cat_IDAssigned"] = "0";
                dr["Doc_Cat_ImageCopy"] = chkScan.Checked ? true : false;
                dr["Program_ID"] = ddlFooterPgmID.SelectedValue;
                dr["Remarks"] = txtFooterRemarks.Text.Trim();

                dt.Rows.Add(dr);
                grvPRDDCDocuments.DataSource = dt;
                grvPRDDCDocuments.DataBind();
                //grvPRDDCDocuments.Rows[0].Visible = false;

                ViewState["GRIDROW"] = dt;
            }
            ViewState["DocID"] = "";
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    private void modifyPRDDCDetails()
    {
        DataSet dsPRDDC = new DataSet();
        divPRDDC.Style.Add("display", "block");
        divPRDDC.Visible = true;
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
            ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPRDDCRow.Company_ID = intCompanyID;
            ObjPRDDCRow.PRDDC_ID = intPRDDCID;
            ObjPRDDCRow.Created_By = intUserID;

            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

            byte[] bytePRDDCDetails = ObjPRDDCMasterClient.FunPubQueryPRDDCDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));
            dsPRDDC = (DataSet)ClsPubSerialize.DeSerialize(bytePRDDCDetails, SerializationMode.Binary, typeof(DataSet));

            if (dsPRDDC.Tables[2] != null && dsPRDDC.Tables[2].Rows.Count > 0)
            {
                intRowCount = dsPRDDC.Tables[2].Rows.Count;
                grvPRDDCDocuments.DataSource = dsPRDDC.Tables[2];
                grvPRDDCDocuments.DataBind();
            }
            //commented for bug fixing - Bug_ID - 5469 - Kuppusamy.B - 15/Feb/2012
            //if (dsPRDDC.Tables[0] != null && dsPRDDC.Tables[0].Rows.Count > 0)
            //    txtScanPath.Text = dsPRDDC.Tables[0].Rows[0]["Document_Path"].ToString();
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
            dsPRDDC.Dispose();
            dsPRDDC = null;
            ObjPRDDCMasterClient.Close();
        }
    }

    protected void ddlProductCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearPRDDCGrid();
        ddlProductCode.AddItemToolTip();
        ddlProductCode.ToolTip = ddlProductCode.SelectedItem.Text.Trim();
    }

    private void BindDataTable(DropDownList ddlSourceControl, params string[] strBinidArgs)
    {
        string strDataTextField = "";

        try
        {
            if (ViewState["dtProgram"] != null)
            {
                DataTable ObjDataTable = (DataTable)ViewState["dtProgram"];

                if (ObjDataTable == null)
                    return;

                if (ObjDataTable.Columns["DataText"] == null)
                {
                    if (strBinidArgs.Length > 2)
                    {
                        strDataTextField = strBinidArgs[1].ToString() + "+'  -  '+" + strBinidArgs[2].ToString();
                    }
                    else
                    {
                        strDataTextField = strBinidArgs[1].ToString();
                    }

                    ObjDataTable.Columns.Add("DataText", typeof(string), strDataTextField);
                }

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

                //foreach (ListItem lt in ddlSourceControl.Items)
                //    lt.Attributes.Add("title", lt.Text);
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        finally
        {
            //ObjAdminService.Close();
        }
    }
}



