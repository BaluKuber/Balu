
//Module Name      :   Origination
//Screen Name      :   S3GORGPDDC_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   01-JUN-2010
//Purpose          :   To insert and update PDDC code details

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

public partial class S3GLoanAdPDDCMaster_Add : ApplyThemeForProject
{
    #region Intialization

    PRDDCMgtServicesReference.PRDDCMgtServicesClient ObjPDDCMasterClient;
    PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_LOANAD_PDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;
    static int intRowCount = 0;

    int intPDDCID = 0;
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
    string strXMLPDDCDocumentsDet = "<Root><Details Doc_Cat_ID='0' /></Root>";
    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbPDDCDocumentsDet = new StringBuilder();
    string strRedirectPage = "../LoanAdmin/S3GLoanAdPDDCMaster_View.aspx";
    string strKey = "Insert";
    string strPgmName = "S3G_ORG_GetWorkflowProgramMaster";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdPDDCMaster_Add.aspx';";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdPDDCMaster_View.aspx';";
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
                    intPDDCID = Convert.ToInt32(fromTicket.Name);
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
            {//Performance Issue Dropdown control Load only Creare Mode Added By Shibu
                if (strMode == "")
                {
                    FunPriBindLOBProduct();
                    FunPriBindProgram();


                    ddlConstitution.Items.Add(new ListItem("--Select--", "0"));
                }
                DummyRow();
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if ((intPDDCID > 0) && (strMode == "M"))
                {
                    FunGetPRDDCDetails();
                    modifyPDDCDetails();
                    FunPriDisableControls(1);
                    txtScanPath.Focus();
                }
                else if ((intPDDCID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunGetPRDDCDetails();
                    modifyPDDCDetails();
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                    lnkCopyProfile.Enabled = false;
                    divPDDC.Visible = false;
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
        ObjPDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            if (txtScanPath.Text.Trim() != "")
            {
                //string[] strScan = txtScanPath.Text.Split('/');

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
            foreach (GridViewRow grvData in grvPDDCDocuments.Rows)
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
                Utility.FunShowAlertMsg(this.Page, "Add atleast one row for save");
                return;
            }
            if (chkMan == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one mandatory Post Dis. document for save");
                return;
            }
            if (txtScanPath.Text.Trim() == "" && chk)
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the Scan Location Path");
                return;
            }

            ObjS3G_LOANAD_PDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCRow;
            ObjPDDCRow = ObjS3G_LOANAD_PDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPDDCRow.Company_ID = intCompanyID;
            ObjPDDCRow.PRDDC_ID = intPDDCID;
            ObjPDDCRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            ObjPDDCRow.Product_ID = Convert.ToInt32(ddlProductCode.SelectedValue);
            ObjPDDCRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
            ObjPDDCRow.DocPath = txtScanPath.Text.Trim();//.Replace("/", "\\").Trim();
            ObjPDDCRow.Created_By = intUserID;
            FunPriGeneratePDDCDocDet();
            ObjPDDCRow.XMLPRDDCDocumentsDet = strXMLPDDCDocumentsDet;

            ObjS3G_LOANAD_PDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPDDCRow);

            intErrCode = ObjPDDCMasterClient.FunPubCreatePDDCDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_PDDCMasterDataTable, SerMode));

            if (intErrCode == 0)
            {
                if (intPDDCID > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "PDDC details updated successfully", strRedirectPage);
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                }
                else
                {
                    strAlert = "PDDC details added successfully";
                    strAlert += @"\n\nWould you like to add one more PDDC?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "PDDC details already exist");
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "PDDC details cannot be updated.Transaction exists");
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
            ObjPDDCMasterClient.Close();
        }
    }


    /// <summary>
    /// This is used to redirect page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage, false);
    }

    protected void grvPDDC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grvPDDC = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSel");
            chkBoxSelect.Attributes["onclick"] = string.Format
                                      (
                                         "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                         grvPDDC.ClientID
                                     );
        }
    }

    /// <summary>
    /// checkbox validation for PRDDCal Documents
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    protected void grvPDDCDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
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
                LinkButton lnkRemovePRDDC = (LinkButton)e.Row.FindControl("lnkRemovePDDC");

                ddlPgmID.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                ddlPgmID.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");
                BindDataTable(ddlPgmID, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" });

                if (lblOptMan.Text == "True") chkOptMan.Checked = true;
                if (lblImageCopy.Text == "True") chkImageCopy.Checked = true;
                ddlPgmID.SelectedValue = lblPgmID.Text;

                //if (strMode == "M")
                //{
                    //if (intRowCount > grvPDDCDocuments.Rows.Count)
                    //{
                        lnkRemovePRDDC.Enabled = false;
                    //}
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtPDDCDesc = (TextBox)e.Row.FindControl("txtPDDCDesc");
                txtPDDCDesc.Attributes.Add("onkeypress", "javascript:if(this.value.toUpperCase()!='--SELECT--' && this.value!='')fnCheckSpecialCharsStartWithAlphabets(true,this.value);");
                txtPDDCDesc.Attributes.Add("onblur", "javascript:if(this.value.toUpperCase()!='--SELECT--' && this.value!='' )fnCheckSingleAlphabets(this.value,this,'Description'); else this.value='--Select--'");
                txtPDDCDesc.Attributes.Add("onfocus", "if(this.value.toUpperCase()=='--SELECT--')this.value=''");

                DropDownList ddlPDDCType = (DropDownList)e.Row.FindControl("ddlPDDCType");
                DropDownList ddlPDDCDesc = (DropDownList)e.Row.FindControl("ddlPDDCDesc");
                DropDownList ddlFooterPgmID = (DropDownList)e.Row.FindControl("ddlFooterPgmID");

                Procparam = new Dictionary<string, string>();
                DataSet dsPRDDCDesc = Utility.GetTableValues(SPNames.S3G_LoanAd_GetPRDDC_DocCateg, Procparam);

                ddlPDDCType.DataSource = dsPRDDCDesc.Tables[1];
                ddlPDDCType.DataTextField = "PDDC_Doc_Type";
                ddlPDDCType.DataValueField = "PDDC_Doc_Type";
                ddlPDDCType.DataBind();
                ddlPDDCType.Items.Insert(0, new ListItem("--Select--", "0"));

                ddlPDDCType.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                ddlPDDCType.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");
                BindDataTable(ddlFooterPgmID, new string[] { "Program_ID", "Program_Ref_No", "Program_Name" });
                ddlFooterPgmID.Attributes.Add("onmouseover", "showDDTooltip(this,event,'" + divTooltip.ClientID + "');");
                ddlFooterPgmID.Attributes.Add("onmouseout", "hideDDTooltip('" + divTooltip.ClientID + "');");
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void grvPDDC_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView grvPDDC = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSel");
            chkBoxSelect.Attributes["onclick"] = string.Format
                                      (
                                         "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                                         grvPDDC.ClientID
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
            ddlLOB.ToolTip = ddlLOB.SelectedItem.Text.Trim();
            if (ddlProductCode.Items.Count > 0)
            {
                ddlProductCode.SelectedIndex = 0;
                ddlProductCode.ToolTip = ddlProductCode.SelectedItem.Text.Trim();
            }
            if (ddlConstitution.Items.Count > 0)
            {
                ddlConstitution.SelectedIndex = 0;
                ddlConstitution.ToolTip = ddlConstitution.SelectedItem.Text.Trim();
            }

            txtScanPath.Text = "";
            hdnPDDC.Value = "0";
            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();
            //DummyRow();
            //grvPDDCDocuments.Columns[grvPDDCDocuments.Columns.Count - 1].Visible = true;            
            grvPDDCDocuments.ClearGrid();
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
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "183");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void ddlLOB_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlLOB.ToolTip = ddlLOB.SelectedItem.Text.Trim();
        FunPriBindProduct();
        //if (ddlLOB.SelectedValue == "0") 
        ClearPDDCGrid();
    }

    private void ClearPDDCGrid()
    {
        if (divPDDC.Visible)
        {
            divPDDC.Visible = false;
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
            if (ddlLOB.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                Procparam.Add("@Is_Active", "1");

                ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });

                //Constitution
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                Procparam.Add("@Is_Active", "1");

                ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
                // Added by Palani Kumar.A on 30/05/2014 for UAT Bug
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                Procparam.Add("@Program_Id", "183");
                if (PageMode == PageModes.Create)
                {
                    Procparam.Add("@Is_Active", "1");
                }

                DataTable dtPath = new DataTable();
                dtPath = Utility.GetDefaultData("S3G_ORG_GetDocPathforLOB", Procparam);

                if (dtPath.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dtPath.Rows[0]["Path"].ToString()))                   
                        txtScanPath.Text = Convert.ToString(dtPath.Rows[0]["Path"]);                   
                    else
                    {
                        Utility.FunShowAlertMsg(this.Page, "Document Path not defined..");
                        return;
                    }
                    
                }
                txtScanPath.Attributes.Add("Readonly", "Readonly");
                ddlConstitution.AddItemToolTip();
                // End
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
        divPDDC.Style.Add("display", "block");
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@PDDC_ID", intPDDCID.ToString());
            Procparam.Add("@User_ID", intUserID.ToString());

            dsPRDDC = Utility.GetDataset("S3G_LOANAD_GetPDDCDocumentsDetails", Procparam);

            if ((intPDDCID > 0) && (dsPRDDC.Tables[0].Rows.Count > 0) && hdnPDDC.Value == "0")
            {
                ddlLOB.Items.Add(new ListItem(dsPRDDC.Tables[0].Rows[0]["LOB_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["LOB_ID"].ToString()));
                ddlLOB.ToolTip = dsPRDDC.Tables[0].Rows[0]["LOB_Code"].ToString();

                // FunPriBindProduct();
                if (dsPRDDC.Tables[0].Rows[0]["Product_Code"].ToString() != "")
                {
                    ddlProductCode.Items.Add(new ListItem(dsPRDDC.Tables[0].Rows[0]["Product_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["LOB_ID"].ToString()));
                    ddlProductCode.ToolTip = dsPRDDC.Tables[0].Rows[0]["Product_Code"].ToString();
                }
                else
                {
                    ddlProductCode.Items.Insert(0, new ListItem("--Select--", "0"));
                }

                // ddlProductCode.SelectedValue = dsPRDDC.Tables[0].Rows[0]["Product_ID"].ToString();

                //ddlConstitution.SelectedValue = dsPRDDC.Tables[0].Rows[0]["Constitution_ID"].ToString();
                //ddlConstitution.ToolTip = ddlConstitution.SelectedItem.Text.Trim();
                ddlConstitution.Items.Add(new ListItem(dsPRDDC.Tables[0].Rows[0]["Constitution_Code"].ToString(), dsPRDDC.Tables[0].Rows[0]["Constitution_ID"].ToString()));
                ddlConstitution.ToolTip = dsPRDDC.Tables[0].Rows[0]["Constitution_Code"].ToString();

                txtScanPath.Text = dsPRDDC.Tables[0].Rows[0]["Document_Path"].ToString();
            }

            if ((dsPRDDC.Tables[1].Rows.Count > 0) && (hdnPDDC.Value == "0"))
            {
                grvPDDC.DataSource = dsPRDDC.Tables[1];
                grvPDDC.DataBind();
                trCopyProfileMessage.Visible = false;
            }
            else if (dsPRDDC.Tables[1].Rows.Count == 0)
            {
                trCopyProfileMessage.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
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
                intPDDCID = Convert.ToInt32(((Label)gvRow.FindControl("lblPDDCID")).Text);
                ddlLOB.Attributes.Add("LOB_ID", ((Label)gvRow.FindControl("lblLOBID")).Text);
                ddlProductCode.Attributes.Add("Product_ID", ((Label)gvRow.FindControl("lblProductID")).Text);
                hdnPDDC.Value = intPDDCID.ToString();
                ViewState["CopyProfile"] = "COPY";
            }

            if (chk.Checked)
            {
                modifyPDDCDetails();
            }

            if (chk.Checked)
            {
                foreach (GridViewRow grv in grvPDDCDocuments.Rows)
                {
                    LinkButton lnkRemovePDDC = (LinkButton)grv.FindControl("lnkRemovePDDC");
                    if (chk.Checked) lnkRemovePDDC.Visible = false; //lnkRemovePDDC.Enabled = false;
                }
                grvPDDCDocuments.FooterRow.Visible = false;
                grvPDDCDocuments.Columns[grvPDDCDocuments.Columns.Count - 1].Visible = false;
            }
            else
            {
                grvPDDCDocuments.Columns[grvPDDCDocuments.Columns.Count - 1].Visible = true;
                if (hdnPDDC.Value == ((Label)gvRow.FindControl("lblPDDCID")).Text)
                {
                    DataTable dt = (DataTable)ViewState["GRIDROW"];
                    dt.Rows.Clear();
                    DummyRow();
                    foreach (GridViewRow grv in grvPDDC.Rows)
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

    private void FunPriGeneratePDDCDocDet()
    {
        try
        {
            string strCatID = string.Empty;
            string strOptMan = string.Empty;
            string strImageCopy = string.Empty;
            string strRemarks = string.Empty;

            //CheckBox chkPDDCDoc = null;
            DropDownList ddlPgmID = null;

            strbPDDCDocumentsDet.Append("<Root>");

            foreach (GridViewRow grvPDDCDoc in grvPDDCDocuments.Rows)
            {
                //chkPDDCDoc = ((CheckBox)grvPDDCDoc.FindControl("chkSel"));
                //if (chkPDDCDoc.Checked)
                if (grvPDDCDoc.Visible)
                {
                    ddlPgmID = ((DropDownList)grvPDDCDoc.FindControl("ddlPgmID"));
                    strCatID = ((Label)grvPDDCDoc.FindControl("lblDocCatID")).Text.Trim();
                    strOptMan = ((CheckBox)grvPDDCDoc.FindControl("chkOptMan")).Checked == true ? "1" : "0";
                    //if (strOptMan == "1")
                    strImageCopy = ((CheckBox)grvPDDCDoc.FindControl("chkImageCopy")).Checked == true ? "1" : "0";
                    //else
                    //    strImageCopy = "0";
                    strRemarks = ((TextBox)grvPDDCDoc.FindControl("txtRemarks")).Text.Replace("'", "\"").Replace(">", "").Replace("<", "").Replace("&", "");

                    strbPDDCDocumentsDet.Append(" <Details Doc_Cat_ID='" + strCatID + "' Doc_Cat_OptMan='" + strOptMan + "'");
                    strbPDDCDocumentsDet.Append(" Doc_Cat_ImageCopy='" + strImageCopy + "' Program_ID='" + ddlPgmID.SelectedValue + "' Doc_Cat_Remarks='" + strRemarks + "' /> ");
                }
            }
            strbPDDCDocumentsDet.Append("</Root>");
            strXMLPDDCDocumentsDet = strbPDDCDocumentsDet.ToString();
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
                        //grvPDDCDocuments.FooterRow.Visible = false;
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
                    //grvPDDCDocuments.FooterRow.Visible = false;
                    lnkCopyProfile.Visible = false;
                    btnGo.Visible = false;
                    ddlLOB.Enabled = false;
                    ddlProductCode.Enabled = false;
                    ddlConstitution.Enabled = false;

                    btnClear.Enabled = false;
                    txtScanPath.ReadOnly = true;
                    break;

                case -1:// Query Mode

                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    //lnkAddOtherDoc.Visible = false;
                    grvPDDCDocuments.FooterRow.Visible = false;
                    grvPDDCDocuments.Columns[grvPDDCDocuments.Columns.Count - 1].Visible = false;
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
                    fnGridPDDC_CtrlDisable();
                    break;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    private void fnGridPDDC_CtrlDisable()
    {
        try
        {
            foreach (GridViewRow grv in grvPDDCDocuments.Rows)
            {
                TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                //CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                LinkButton lnkRemovePDDC = (LinkButton)grv.FindControl("lnkRemovePDDC");
                lnkRemovePDDC.Enabled = chkOptMan.Enabled = chkImageCopy.Enabled = false;
                txtRemarks.ReadOnly = true;

                if (bClearList)
                {
                    if (ddlPgmID.SelectedIndex != -1)
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
        divPDDC.Visible = false;
        grvPDDCDocuments.Columns[grvPDDCDocuments.Columns.Count - 1].Visible = true;
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

        DataTable dtPDDC = new DataTable();
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Product_ID", ddlProductCode.SelectedValue.ToString());
            Procparam.Add("@Constitution_ID", ddlConstitution.SelectedValue.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());

            dtPDDC = Utility.GetDefaultData("S3G_LOANAD_GetPDDCCombinationDetails", Procparam);

            if (dtPDDC.Rows.Count > 0)
            {
                if (dtPDDC.Rows[0]["CNT"].ToString() != "0")//) '0' means selected combination not exist
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert18", "alert('Selected combination already exist');", true);
                    return;
                }
                else
                {
                    //Changed By Thangam M on 09/Jan/2012 to fix Malolan Observation
                    if (grvPDDC.Rows.Count > 0)
                    {
                        lnkCopyProfile.Enabled = true;
                    }

                    divPDDC.Visible = true;
                    btnSave.Visible = true;
                    btnClear.Visible = true;

                    ViewState["CopyProfile"] = "NEW";

                    DataTable dt = (DataTable)ViewState["GRIDROW"];
                    dt.Rows.Clear();
                    DummyRow();
                    foreach (GridViewRow grv in grvPDDC.Rows)
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

    protected void ddlConstitution_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlConstitution.SelectedValue == "0") divPDDC.Visible = false;
        ClearPDDCGrid();
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

                DataColumn dcPDDCType = new DataColumn("PDDCType");
                dtGrid.Columns.Add(dcPDDCType);

                DataColumn dcPDDCDesc = new DataColumn("PDDCDesc");
                dtGrid.Columns.Add(dcPDDCDesc);

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

            dr["PDDCType"] = "";
            dr["PDDCDesc"] = "";
            dr["Doc_Cat_OptMan"] = false;
            dr["Doc_Cat_ID"] = "0";
            dr["Doc_Cat_IDAssigned"] = "0";
            dr["Doc_Cat_ImageCopy"] = false;
            dr["Program_ID"] = "0";
            dr["Remarks"] = "";

            dtGrid.Rows.Add(dr);
            grvPDDCDocuments.DataSource = dtGrid;
            grvPDDCDocuments.DataBind();
            ViewState["GRIDROW"] = dtGrid;
            grvPDDCDocuments.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void txtPDDCDesc_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (System.Web.HttpContext.Current.Session["dsPDDCDesc"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["dsPDDCDesc"];
                if (dt.Rows.Count > 0)
                {
                    string filterExpression = "PDDC_Doc_Description = '" + txtPDDCDesc.Text + "'";
                    DataRow[] dtSuggestions = dt.Select(filterExpression);
                    if (dtSuggestions.Length > 0)
                    {
                        ViewState["DocID"] = dtSuggestions[0]["PDDC_Doc_Cat_ID"].ToString();
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
    public static string[] getPDDCDescription(String prefixText, int count)
    {
        List<String> suggetions = null;
        DataTable dtDesc = new DataTable();
        try
        {
            if (System.Web.HttpContext.Current.Session["dsPDDCDesc"] != null)
            {
                dtDesc = (DataTable)System.Web.HttpContext.Current.Session["dsPDDCDesc"];
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
            string filterExpression = "PDDC_Doc_Description like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);
            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["PDDC_Doc_Description"].ToString();
                suggestions.Add(suggestion);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return suggestions;
    }

    protected void ddlPDDCType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["DocID"] = "";
            DropDownList ddlPDDCType = (DropDownList)sender;
            DropDownList ddlPDDCDesc = (DropDownList)ddlPDDCType.Parent.Parent.FindControl("ddlPDDCDesc");
            TextBox txtPDDCDesc = (TextBox)ddlPDDCType.Parent.Parent.FindControl("txtPDDCDesc");

            txtPDDCDesc.Text = "--Select--";
            if (ddlPDDCType.SelectedValue != "0")
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@PDDC_Doc_Type", ddlPDDCType.SelectedValue);
                DataSet dsPDDCDesc = Utility.GetTableValues("S3G_LOANAD_GetPDDC_DocumentCategory", Procparam);

                System.Web.HttpContext.Current.Session["dsPDDCDesc"] = dsPDDCDesc.Tables[2];
                txtPDDCDesc.Enabled = false;
                if (ddlPDDCType.SelectedValue.ToUpper().Trim() == "OTHERS")
                {
                    txtPDDCDesc.Enabled = true;
                    ddlPDDCDesc.Focus();
                }
                else
                {
                    //Changed By Thangam M on 30/Nov/2011
                    DataRow[] dr = dsPDDCDesc.Tables[0].Select("PDDC_Doc_Type + ' - ' + PDDC_Doc_Description = '" + ddlPDDCType.SelectedValue + "' ");
                    txtPDDCDesc.Text = dr[0]["PDDC_Doc_Description"].ToString();
                    ViewState["DocID"] = dr[0]["PDDC_Doc_Cat_ID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    protected void lnkRemovePDDC_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();

            foreach (GridViewRow grv in grvPDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                    CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                    CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                    DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                    TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    Label lblPDDCType = (Label)grv.FindControl("lblPDDCType");
                    Label lblPDDCDesc = (Label)grv.FindControl("lblPDDCDesc");
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");

                    DataRow dr = dt.NewRow();
                    dr["PDDCType"] = lblPDDCType.Text.Trim();
                    dr["PDDCDesc"] = lblPDDCDesc.Text.Trim();
                    dr["Doc_Cat_OptMan"] = true;//chkOptMan.Checked ? true : false;
                    dr["Doc_Cat_ID"] = lblDocCatID.Text.Trim();
                    dr["Doc_Cat_IDAssigned"] = "1";
                    dr["Doc_Cat_ImageCopy"] = chkImageCopy.Checked ? true : false;
                    dr["Program_ID"] = ddlPgmID.SelectedValue;
                    dr["Remarks"] = txtRemarks.Text.Trim(); ;

                    dt.Rows.Add(dr);
                }
            }

            LinkButton lnkRemovePDDC = (LinkButton)sender;
            GridViewRow gvRow = (GridViewRow)lnkRemovePDDC.Parent.Parent;

            dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.RemoveAt(gvRow.RowIndex);
            grvPDDCDocuments.DataSource = dt;
            grvPDDCDocuments.DataBind();

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
        ObjPDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            Button lnkAAdd = (Button)sender;
            DropDownList ddlPDDCType = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlPDDCType");
            DropDownList ddlPDDCDesc = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlPDDCDesc");
            TextBox txtPDDCDesc = (TextBox)lnkAAdd.Parent.Parent.FindControl("txtPDDCDesc");
            CheckBox chkFootOptMan = (CheckBox)lnkAAdd.Parent.Parent.FindControl("chkFootOptMan");
            CheckBox chkScan = (CheckBox)lnkAAdd.Parent.Parent.FindControl("chkScan");
            DropDownList ddlFooterPgmID = (DropDownList)lnkAAdd.Parent.Parent.FindControl("ddlFooterPgmID");
            TextBox txtFooterRemarks = (TextBox)lnkAAdd.Parent.Parent.FindControl("txtFooterRemarks");

            if (ddlPDDCType.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this.Page, "Select the PDDC Type");
                ddlPDDCType.Focus();
                return;
            }
            if (txtPDDCDesc.Text.Trim() == "" || txtPDDCDesc.Text.Trim().ToUpper() == "--SELECT--")
            {
                Utility.FunShowAlertMsg(this.Page, "Enter the PDDC other document description.");
                txtPDDCDesc.Focus();
                return;
            }
            //if (ddlFooterPgmID.SelectedValue == "0")
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Select the Last Program ID");
            //    ddlPDDCType.Focus();
            //    return;
            //}
            if (ddlPDDCType.SelectedItem.Text.Trim().ToUpper() == "OTHERS" && ViewState["DocID"].ToString() == "")
            {
                PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
                PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
                //  ObjPDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

                try
                {
                    ObjPRDDCRow = ObjS3G_ORG_PDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();
                    ObjPRDDCRow.DocCategory = "";
                    ObjPRDDCRow.DocType = "PDDC";
                    ObjPRDDCRow.DocDesc = txtPDDCDesc.Text.Trim();
                    ObjPRDDCRow.Created_By = intUserID;

                    ObjS3G_ORG_PDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

                    intErrCode = ObjPDDCMasterClient.FunPubCreateOtherPostDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PDDCMasterDataTable, SerMode));

                    if (intErrCode != -1)
                    {
                        Utility.FunShowAlertMsg(this.Page, "PRDDC other document details added successfully");
                        ViewState["DocID"] = intErrCode.ToString();
                        if (Request.QueryString["qsPDDCId"] != null)
                            intPDDCID = Convert.ToInt32(Request.QueryString["qsPDDCId"]);
                    }
                    else if (intErrCode == -1)
                    {
                        Utility.FunShowAlertMsg(this.Page, "PDDC other document details already exist in data base.");
                    }
                    //else if (intErrCode == 2)
                    //{
                    //    Utility.FunShowAlertMsg(this.Page, "Document sequence number is not defined to create PDDC Code");
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
                    ObjPDDCMasterClient.Close();
                }
            }

            foreach (GridViewRow grv in grvPDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");
                    Label lblPDDCDesc = (Label)grv.FindControl("lblPDDCDesc");
                    if (ddlPDDCType.SelectedItem.Text.Trim().ToUpper() != "OTHERS" && lblDocCatID.Text.Trim() == ViewState["DocID"].ToString())
                    {
                        Utility.FunShowAlertMsg(this.Page, "Selected PDDC Type already exists in the grid.");
                        return;
                    }
                    if (ddlPDDCType.SelectedItem.Text.Trim().ToUpper() == "OTHERS")
                    {
                        if (txtPDDCDesc.Text.Trim().ToLower() == lblPDDCDesc.Text.Trim().ToLower())
                        {
                            Utility.FunShowAlertMsg(this.Page, "Selected PDDC Type & Description already exists in the grid.");
                            return;
                        }
                    }
                }
            }


            DataTable dt = (DataTable)ViewState["GRIDROW"];
            dt.Rows.Clear();

            foreach (GridViewRow grv in grvPDDCDocuments.Rows)
            {
                if (grv.Visible)
                {
                    CheckBox chkSel = (CheckBox)grv.FindControl("chkSel");
                    CheckBox chkOptMan = (CheckBox)grv.FindControl("chkOptMan");
                    CheckBox chkImageCopy = (CheckBox)grv.FindControl("chkImageCopy");
                    DropDownList ddlPgmID = (DropDownList)grv.FindControl("ddlPgmID");
                    TextBox txtRemarks = (TextBox)grv.FindControl("txtRemarks");
                    Label lblPDDCType = (Label)grv.FindControl("lblPDDCType");
                    Label lblPDDCDesc = (Label)grv.FindControl("lblPDDCDesc");
                    Label lblDocCatID = (Label)grv.FindControl("lblDocCatID");

                    DataRow dr = dt.NewRow();
                    dr["PDDCType"] = lblPDDCType.Text.Trim();
                    dr["PDDCDesc"] = lblPDDCDesc.Text.Trim();
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

                dr["PDDCType"] = ddlPDDCType.SelectedValue.Trim();
                //dr["PDDCDesc"] =  ddlPDDCType.SelectedValue.Trim().ToUpper()=="OTHERS"? ddlPDDCDesc.SelectedItem.Text.Trim():txtPDDCDesc.Text.Trim(); 
                dr["PDDCDesc"] = txtPDDCDesc.Text.Trim();
                dr["Doc_Cat_OptMan"] = chkFootOptMan.Checked ? true : false;
                dr["Doc_Cat_ID"] = ViewState["DocID"].ToString();
                dr["Doc_Cat_IDAssigned"] = "0";
                dr["Doc_Cat_ImageCopy"] = chkScan.Checked ? true : false;
                dr["Program_ID"] = ddlFooterPgmID.SelectedValue;
                dr["Remarks"] = txtFooterRemarks.Text.Trim();

                dt.Rows.Add(dr);
                grvPDDCDocuments.DataSource = dt;
                grvPDDCDocuments.DataBind();
                //grvPDDCDocuments.Rows[0].Visible = false;

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

    private void modifyPDDCDetails()
    {
        DataSet dsPDDC = new DataSet();
        divPDDC.Style.Add("display", "block");
        divPDDC.Visible = true;
        ObjPDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        try
        {
            ObjS3G_LOANAD_PDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
            PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCRow;
            ObjPDDCRow = ObjS3G_LOANAD_PDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();

            ObjPDDCRow.Company_ID = intCompanyID;
            ObjPDDCRow.PRDDC_ID = intPDDCID;
            ObjPDDCRow.Created_By = intUserID;

            ObjS3G_LOANAD_PDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPDDCRow);

            byte[] bytePDDCDetails = ObjPDDCMasterClient.FunPubQueryPDDCDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOANAD_PDDCMasterDataTable, SerMode));
            dsPDDC = (DataSet)ClsPubSerialize.DeSerialize(bytePDDCDetails, SerializationMode.Binary, typeof(DataSet));

            if (dsPDDC.Tables[2] != null && dsPDDC.Tables[2].Rows.Count > 0)
            {
                intRowCount = dsPDDC.Tables[2].Rows.Count;
                grvPDDCDocuments.DataSource = dsPDDC.Tables[2];
                grvPDDCDocuments.DataBind();
            }
            if (dsPDDC.Tables[0] != null && dsPDDC.Tables[0].Rows.Count > 0)
                txtScanPath.Text = dsPDDC.Tables[0].Rows[0]["Document_Path"].ToString();
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
            dsPDDC.Dispose();
            dsPDDC = null;
            ObjPDDCMasterClient.Close();
        }
    }

    protected void ddlProductCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearPDDCGrid();
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



