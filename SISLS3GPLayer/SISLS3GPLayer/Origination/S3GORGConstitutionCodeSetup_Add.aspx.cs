
//Module Name      :   Origination
//Screen Name      :   S3GORGConstitutionCode_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   01-JUN-2010
//Purpose          :   To insert and update constitution code details

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
using System.Web.Security;
using System.Configuration;
using System.Web.Services;
using System.Collections;

public partial class S3GORGConstitutionCodeSetup_Add : ApplyThemeForProject
{
    #region Intialization

    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient ObjConstitutionCodeMasterClient;
    OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable ObjS3G_ORG_ConstitutionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;

    int intConstitutionID = 0;
    bool  IsEnableLOB = true;
    bool IsLOBCalled = false;
    int intUserID = 0;
    int intCompanyID = 0;


    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bClearList = false;
    bool bMakerChecker = false;
    //Code end

    string strXMLLOBDet = "<Root><Details LOB_ID='0' /></Root>";
    string strXMLConsDocumentsDet = "<Root><Details LOB_ID='0' /></Root>";
    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbConsDocumentsDet = new StringBuilder();
    string strRedirectPage = "../Origination/S3GORGConstitutionCodeSetup_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGConstitutionCodeSetup_Add.aspx?qsMode=C';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGConstitutionCodeSetup_View.aspx';";
    Dictionary<string, string> dictParam = null;
    #endregion

    #region PageLoad

  

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);        
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        //txtOthers.Attributes.CssStyle.Add("text-transform", "uppercase");
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end


        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (fromTicket != null)
            {
                intConstitutionID = Convert.ToInt32(fromTicket.Name);
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            }
            strMode = Request.QueryString["qsMode"];
        }

        // KR PageMode.ToString() != ""
        if (ViewState["PageMode"]==null )
        {
            ViewState["PageMode"] = PageMode.ToString();
        }

        if (!IsPostBack)
        {
            FunGetConstitutionCodeDetails();

          
            //LoadDocumentFlags();

            //User Authorization

            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if ((intConstitutionID > 0) && (strMode == "M"))
            {
                //LoadDocumentFlags();
                hdnMode.Value = "M";
                FunPriDisableControls(1);

            }
            else if ((intConstitutionID > 0) && (strMode == "Q")) // Query // Modify
            {
                FunPriDisableControls(-1);

            }
            else
            {
                FunPriDisableControls(0);
            }

            //Code end

        }
        txtConstitutionName.Focus();

        //DropDownList ddlDocCatGird = (DropDownList)grvConsDocuments.FooterRow.FindControl("ddlDocCatGird");
        //ddlDocCatGird.Attributes.Add("onmouseover", "return showTip(this,event);");
        //ddlDocCatGird.Attributes.Add("onmouseout", "return hideText();");
       // BindTooltip(ddlDocCatGird);
    }

    //<<Performance>>
    [System.Web.Services.WebMethod]
    public static string[] GetDocumentFlags(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_GetConstitutionDocumentCategory_AGT", Procparam));

        return suggetions.ToArray();
    }

    private void LoadDocumentFlags()
    {
        if (grvConsDocuments.FooterRow != null)
        {
            DropDownList ddlDocCatGird = (DropDownList)grvConsDocuments.FooterRow.FindControl("ddlDocCatGird");
            dictParam = new Dictionary<string, string>();
            /*dictParam.Add("@TextField", " distinct Document_Flag");
            dictParam.Add("@ValueField", " Document_Flag");
            dictParam.Add("@TableName", "S3G_ORG_ConstitutionDocumentCategory");
            //dictParam.Add("@FilterCondition", "Order By Document_Flag");
            DataTable dtDocuments = new DataTable();
            dtDocuments = Utility.GetDDLRows(dictParam);
            if (dtDocuments != null && dtDocuments.Rows.Count >
                0)
                ddlDocCatGird.BindDataTable(dtDocuments);
            ddlDocCatGird.Focus();*/
            //modified by Saran 29-April-2011 for the dropdown(ddlDocCatGird) was appended by Document Desription

            ddlDocCatGird.BindDataTable("S3G_ORG_GetConstitutionDocumentCategory", dictParam, new string[] { ConstitutionDocumentCategory_ID,"Document_Description" });
        }
    }


    #endregion

    public static void BindTooltip(DropDownList lc)
    {
        for (int i = 0; i < lc.Items.Count; i++)
        {
            lc.Items[i].Attributes.Add("title", lc.Items[i].Text);
        }
    }


    #region Page Events

    /// <summary>
    /// This is used to save ConstitutionCode details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (!Page.IsValid)
            return;

        int result = FunPriGenerateLOBDocDet();
        if (result == 1)
        {
            if (grvConsDocuments.Rows.Count == 1 && ViewState["PageMode"].ToString() == PageModes.Create.ToString())
                Utility.FunShowAlertMsg(this.Page, "Add atleast one document to proceed.");
            else            
                Utility.FunShowAlertMsg(this.Page, "Atleast one mandatory document should be selected ");
            return;
        }
        //else if (result == 2)
        //{
        //    Utility.FunShowAlertMsg(this.Page, "Remarks cannot be empty");
        //    return;
        //}
        else if (result == 3)
        {
            Utility.FunShowAlertMsg(this.Page, "Atleast one Line of Business should be selected");
            return;
        }


        //else if (grvConsDocuments.Rows.Count == 0 && ViewState["PageMode"].ToString() == PageModes.Modify.ToString())
        //{
        //    Utility.FunShowAlertMsg(this.Page, "Add atleast one document to proceed.");
        //    return;
        //}

        ObjConstitutionCodeMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable ObjS3G_ORG_ConstitutionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable();
            OrgMasterMgtServices.S3G_ORG_ConstitutionMasterRow ObjConstitutionCodeRow;
            ObjConstitutionCodeRow = ObjS3G_ORG_ConstitutionMasterDataTable.NewS3G_ORG_ConstitutionMasterRow();


            ObjConstitutionCodeRow.Company_ID = intCompanyID;
            ObjConstitutionCodeRow.Constitution_ID = intConstitutionID;
            ObjConstitutionCodeRow.ConstitutionCode = 0;
            ObjConstitutionCodeRow.ConstitutionName = txtConstitutionName.Text.Trim();
            ObjConstitutionCodeRow.LOB_ID = 0;
            ObjConstitutionCodeRow.Created_By = intUserID;
           
            ObjConstitutionCodeRow.XMLLOBDet = strXMLLOBDet;
            ObjConstitutionCodeRow.XMLConsDocumentsDet = strXMLConsDocumentsDet;


            ObjS3G_ORG_ConstitutionMasterDataTable.AddS3G_ORG_ConstitutionMasterRow(ObjConstitutionCodeRow);
            //SerializationMode SerMode = SerializationMode.Binary;

            string ConstitutionCode = "-1";

            intErrCode = ObjConstitutionCodeMasterClient.FunPubCreateConstitutionCode(out ConstitutionCode, SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_ConstitutionMasterDataTable, SerMode));

            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intConstitutionID > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Constitution Code setup updated successfully", strRedirectPage);
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(ConstitutionCode))
                        txtConstitutionCode.Text = Convert.ToString(ConstitutionCode); 
                    strAlert = "Constitution Code setup added successfully";
                    strAlert += @"\n\nWould you like to add one more Constitution Code setup?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                    ViewState["PageMode"] = PageModes.Create.ToString();
                    return;
                } 
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Constitution Name already exists");
                return;
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Constitution Code cannot be created.There are 99 Constitution Codes Already in the system.");
                return;
            }          
            lblErrorMessage.Text = string.Empty;

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
            ObjConstitutionCodeMasterClient.Close();
        }
    }

    /// <summary>
    /// This is used to save Constitution other document details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSaveDocType_Click(object sender, EventArgs e)
    {
        //InsertNewDocumentCategory();
    }

    private void InsertNewDocumentCategory(string codeName,string codeFlag, string codeDesc)
    {
        PRDDCMgtServicesReference.PRDDCMgtServicesClient ObjPRDDCMasterClient;
        PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
        PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();
        try
        {

            ObjPRDDCRow.DocType = "Consti";
            ObjPRDDCRow.DocCategory = codeName;
            ObjPRDDCRow.DocDesc = codeDesc;
            ObjPRDDCRow.Created_By = intUserID;

            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);

            ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();

            intErrCode = ObjPRDDCMasterClient.FunPubCreateOtherDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));

            if (intErrCode >=0)
            {
                //Utility.FunShowAlertMsg(this.Page, "Constitution other document added successfully");
                if (Request.QueryString["qsConstitutionId"] != null)
                    intConstitutionID = Convert.ToInt32(Request.QueryString["qsConstitutionId"]);
                ViewState["DocID"] = intErrCode;
                //FunGetConstitutionCodeDetails();
            }
            else if (intErrCode == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Constitution other document already exists");
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
        }
        finally
        {
            ObjPRDDCMasterClient.Close();
        }
    }

    protected void grvConstitution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkSel = (CheckBox)e.Row.FindControl("chkSel");
            Label lblConstitutionID = (Label)e.Row.FindControl("lblConstitutionID");
            if (lblConstitutionID.Text == hdnConstitution.Value)
            {
                chkSel.Checked = true;
            }

            // For adding Tooltip in gridview
            // Modified by Senthilkumar P on 12/Mar/2012
            e.Row.Cells[1].ToolTip = e.Row.Cells[1].Text;
            e.Row.Cells[2].ToolTip = e.Row.Cells[2].Text;
            // Modified by Senthilkumar P on 12/Mar/2012
 
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





    /// <summary>
    /// checkbox validation for LOB
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvLOB_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAssigned = (Label)e.Row.FindControl("lblAssigned");
            CheckBox chkSelectLOB = (CheckBox)e.Row.FindControl("chkSelectLOB");
            chkSelectLOB.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvLOB.ClientID + "','chkSelectAllLOB','chkSelectLOB');");
            if (lblAssigned.Text == "1")
            {
                if (IsLOBCalled == false)
                {
                    if ((intConstitutionID > 0) && (strMode == "M"))
                    {
                        IsLOBCalled = true;
                        Dictionary<string, string> Procparam = new Dictionary<string, string>();
                        Procparam.Add("@Constitution_ID", intConstitutionID.ToString());
                        DataTable dt_IsExists = Utility.GetDefaultData("S3G_ORG_InsertConstitutionTransactionIsExits", Procparam);
                        if (dt_IsExists != null && dt_IsExists.Rows.Count > 0)
                            IsEnableLOB = false;
                    }
                }
                 
                //if (!(IsEnableLOB))
                //    chkSelectLOB.Enabled = false;
               
            }
            if (strMode == "Q")
            {
                chkSelectLOB.Enabled = false;
            }
            chkSelectLOB.Checked = true;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAllRole");

            //CheckBox chkSelectAllLOB = (CheckBox)e.Row.FindControl("chkSelectAllLob");
            //chkSelectAllLOB.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvLOB.ClientID + "',this,'chkSelectLOB');");
            ////chkAll.Checked = true;
            //if (ViewState["SelectAll"] != null)
            //{
            //    bool SelectAll= bool.Parse(ViewState["SelectAll"].ToString());
            //    chkSelectAllLOB.Checked = SelectAll;
            //}
            //if (strMode == "Q")
            //{
            //    chkSelectAllLOB.Enabled = false;
            //}
        }

    }
    /// <summary>
    /// checkbox validation for Constitutional Documents
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvConsDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDocCatID = (Label)e.Row.FindControl("lblDocCatIDAssigned");
            CheckBox chkSel = (CheckBox)e.Row.FindControl("chkSel");

            
            CheckBox chkOptMan = (CheckBox)e.Row.FindControl("chkOptMan");
            CheckBox chkImageCopy = (CheckBox)e.Row.FindControl("chkImageCopy");

            // chkSel.Attributes.Add("", "javascript:fnCheck();");
            //chkSel.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvConsDocuments.ClientID + "','chkAllDoc','chkSel');"); //javascript:fnEnableCheckes('" + chkSel.ClientID + "','" + chkOptMan.ClientID + "','" + chkImageCopy.ClientID + "');");
            //chkOptMan.Enabled = false;
            //chkImageCopy.Enabled = false;
            TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
            //if ((intConstitutionID > 0) && (strMode == "M"))
            //{
            //    chkSel.Enabled = false;
            //}

            txtRemarks.ReadOnly = true;
            if (Convert.ToInt32(lblDocCatID.Text) > 0)
            {
                txtRemarks.Text = System.Web.HttpUtility.HtmlDecode(txtRemarks.Text);
                if (chkSel != null)
                {
                    chkSel.Checked = true;
                    chkSel.Enabled = true; //ENABLED CHANGED
                }
                Label lblOptMan = (Label)e.Row.FindControl("lblOptMan");
                Label lblImageCopy = (Label)e.Row.FindControl("lblImageCopy");
                txtRemarks.ReadOnly = false;
                

                chkOptMan.Enabled = true; //ENABLED CHANGED
                chkImageCopy.Enabled = true; // ENABLED CHANGED


                if ((lblOptMan.Text == "True") || (lblOptMan.Text == "1"))    //  Added for Oracle Conversion , PSK -- 4/Nov/2011
                {
                    chkOptMan.Checked = true;
                    // chkOptMan.Enabled = true; //ENABLED CHANGED
                }

                if ((lblImageCopy.Text == "True") || (lblImageCopy.Text == "1"))   //  Added for Oracle Conversion, PSK -- 4/Nov/2011
                {
                    chkImageCopy.Checked = true;
                    //chkImageCopy.Enabled = true; // ENABLED CHANGED
                }
               

            }
            else
            {
                txtRemarks.Text = "";
            }

            if (strMode == "Q")
            {              
                chkOptMan.Enabled = false;
                chkImageCopy.Enabled = false;
                txtRemarks.ReadOnly = true;
            }


        }
   
    }

    protected void FunPriGetCopyProfileDetails(object sender, EventArgs e)
    {
         string strChkId = ((CheckBox)sender).ClientID;
        CheckBox chkCurSelect = (CheckBox)sender;
        CheckBox chkConstitution = null;
        if (chkCurSelect.Checked == true)
        {
            foreach (GridViewRow grvData in grvConstitution.Rows)
            {
                chkConstitution = ((CheckBox)grvData.FindControl("chkSel"));
                if ((strChkId == chkConstitution.ClientID))
                {
                    intConstitutionID = Convert.ToInt32(((Label)grvData.FindControl("lblConstitutionID")).Text);
                    hdnConstitution.Value = intConstitutionID.ToString();
                }
            }
            chkCurSelect.Enabled = true;
            ViewState["PageMode"] = PageModes.Delete.ToString();            
            //FunPriBindLOBProduct();
            GetLOBGridSelectedValues();
            FunGetConstitutionCodeDetails();
            ClearLOBGrid();
            LoadSelectedLOBsAgain();           
              
        }
        else
        {
            GridViewRow gvRow = null;
            if (chkCurSelect != null) gvRow = (GridViewRow)chkCurSelect.Parent.Parent;
            if (hdnConstitution.Value == ((Label)gvRow.FindControl("lblConstitutionID")).Text)
            {
                DataTable dtConstitutions = ViewState["Documents"] as DataTable;
                dtConstitutions.Rows.Clear();
                ViewState["Documents"] = dtConstitutions;
                ViewState["PageMode"] = PageModes.Create.ToString();     
                AddNewRowTable("", "", false, false, "");                  
                grvConsDocuments.FooterRow.Visible  = grvConsDocuments.Columns[8].Visible = true;
                ClearLOBGrid();
               
            }
        }
       
    }

    void GetLOBGridSelectedValues()
    {
        // clear the Selected LOB's
        CheckBox chkLOB;
        Label LblLOB;
        ArrayList SelectedLOBs = new ArrayList();
        foreach (GridViewRow grvData in grvLOB.Rows)
        {
            chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
            LblLOB = ((Label)grvData.FindControl("lblLOBID"));
            if(chkLOB.Checked)
                SelectedLOBs.Add(int.Parse(LblLOB.Text.Trim()));
            chkLOB.Checked = false;
        }
        //chkLOB = (CheckBox)grvLOB.HeaderRow.FindControl("chkSelectAllLOB");
        //chkLOB.Checked = false;
        ViewState["SelectedLOBs"] = SelectedLOBs;
    }

    void ClearLOBGrid()
    {
        // clear the Selected LOB's
        CheckBox chkLOB;
        foreach (GridViewRow grvData in grvLOB.Rows)
        {
            chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
            chkLOB.Checked = false;
        }
        //chkLOB = (CheckBox)grvLOB.HeaderRow.FindControl("chkSelectAllLOB");
        //chkLOB.Checked = false;
    }
    void LoadSelectedLOBsAgain()
    {
        ArrayList SelectedLOB ;
        DataTable AllLOB=new DataTable();
        if (ViewState["SelectedLOBs"] != null)
        {
            SelectedLOB = (ArrayList)ViewState["SelectedLOBs"];
            AllLOB =(DataTable) grvLOB.DataSource;

            foreach (DataRow LOB in AllLOB.Rows)
            {
                LOB["Assigned"] = false;
            }
            foreach (DataRow LOB in AllLOB.Rows)
            {
                foreach (int sLOB in SelectedLOB)
                {
                    if (int.Parse(LOB["LOB_ID"].ToString()) == sLOB)
                    {                        
                        LOB["Assigned"] = true;
                    }                   
                }                
                
            }
        }        
        grvLOB.DataSource = AllLOB;
        grvLOB.DataBind();
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
            txtConstitutionName.Text = "";
            ViewState["PageMode"] = PageModes.Create.ToString();
            //txtOthers.Text = "";
            //ddlDocCat.SelectedIndex = 0;
            hdnConstitution.Value = "0";
            FunGetConstitutionCodeDetails();

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

    #region Page Methods

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetConstitutionCodeDetails()
    {
        DataSet dsConstitution = new DataSet();
        ObjConstitutionCodeMasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_ORG_ConstitutionMasterDataTable = new OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable();
            OrgMasterMgtServices.S3G_ORG_ConstitutionMasterRow ObjConstitutionCodeRow;
            SerializationMode SerMode = SerializationMode.Binary;
            ObjConstitutionCodeRow = ObjS3G_ORG_ConstitutionMasterDataTable.NewS3G_ORG_ConstitutionMasterRow();
            //ObjConstitutionCodeRow.LOB_ID = intLOBID;
            ObjConstitutionCodeRow.Company_ID = intCompanyID;
            ObjConstitutionCodeRow.Constitution_ID = intConstitutionID;
            ObjConstitutionCodeRow.Created_By = intUserID;
            if(PageMode==PageModes.Query)
                ObjConstitutionCodeRow.IsQueryMode = true;
            if (PageMode == PageModes.Modify)
                ObjConstitutionCodeRow.IsQueryMode = false;

            ObjS3G_ORG_ConstitutionMasterDataTable.AddS3G_ORG_ConstitutionMasterRow(ObjConstitutionCodeRow);

            byte[] byteConstitutionCodeDetails = ObjConstitutionCodeMasterClient.FunPubQueryConstitutionDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_ConstitutionMasterDataTable, SerMode));
            dsConstitution = (DataSet)ClsPubSerialize.DeSerialize(byteConstitutionCodeDetails, SerializationMode.Binary, typeof(DataSet));
            try
            {
                if (dsConstitution.Tables[0] != null)
                {
                   /* EnumerableRowCollection<DataRow> resultCount= from LOB in dsConstitution.Tables[0].AsEnumerable()
                                      where LOB.Field<Nullable<int>>("Assigned") == 1
                                      select LOB;

                    if (resultCount.AsDataView().Count == dsConstitution.Tables[0].Rows.Count)
                        ViewState["SelectAll"] = true;
                    else
                        ViewState["SelectAll"] = false;
                    */

                    //Commanded by Senthilkumar P [PSK] -- 04/Nov -- For Oracle Conversion...
                    DataView dv = new DataView(dsConstitution.Tables[0], "Assigned=1", "", DataViewRowState.CurrentRows);

                    if (dv.Count == dsConstitution.Tables[0].Rows.Count)
                        ViewState["SelectAll"] = true;
                    else
                        ViewState["SelectAll"] = false;

                    grvLOB.DataSource = dsConstitution.Tables[0];
                    grvLOB.DataBind();
                }

            }
            catch (Exception es)
            {                
                throw;
            }
            if ((dsConstitution.Tables[1].Rows.Count > 0) && hdnConstitution.Value == "0")
            {
                txtConstitutionCode.Text = dsConstitution.Tables[1].Rows[0]["Constitution_Code"].ToString();
                txtConstitutionName.Text = dsConstitution.Tables[1].Rows[0]["Constitution_Name"].ToString();
            }

            if (ViewState["PageMode"].ToString() == PageModes.Create.ToString())
            {
                 if (!dsConstitution.Tables[2].Columns.Contains("Sno"))
                        dsConstitution.Tables[2].Columns.Add("Sno", typeof(System.Int16));
                    ViewState["Documents"] = dsConstitution.Tables[2].Clone();
                    AddNewRowTable("", "", false, false, "");
                    grvConsDocuments.Columns[8].Visible = true;
            }
            else if (ViewState["PageMode"].ToString() == PageModes.Modify.ToString())
            {
                ViewState["Documents"] = dsConstitution.Tables[2].Copy();
                BindGrid(dsConstitution.Tables[2]);                
                
                
            }
            else if (ViewState["PageMode"].ToString() == PageModes.Delete.ToString() || ViewState["PageMode"].ToString() == PageModes.Query.ToString())
            {
                BindGrid(dsConstitution.Tables[2]);
                if(grvConsDocuments.FooterRow!=null)
                grvConsDocuments.FooterRow.Visible = grvConsDocuments.Columns[8].Visible = false;
                //ViewState["Documents"] = dsConstitution.Tables[2].Copy();
            }


            if ((dsConstitution.Tables[3].Rows.Count > 0) && (hdnConstitution.Value == "0"))
            {
                grvConstitution.DataSource = dsConstitution.Tables[3];
                grvConstitution.DataBind();
                trCopyProfileMessage.Visible = false;
            }
            else if (dsConstitution.Tables[3].Rows.Count == 0) 
            {
                trCopyProfileMessage.Visible = true;
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
            dsConstitution.Dispose();
            dsConstitution = null;
           // ObjConstitutionCodeMasterClient.Close();
        }
    }
    void AddNewRowTable(string codeName, string codeDesc, bool codeMandatory, bool codeImageCopy, string codeRemarks)
    {

        DataTable dtConstitutions = ViewState["Documents"] as DataTable;
        string strCatID=string.Empty;
        DataRow[] dtrFilter;
        // Retain the Existing Row Values from Grid
        foreach (GridViewRow grvConsDoc in grvConsDocuments.Rows)
        {
            strCatID = ((Label)grvConsDoc.FindControl("lblDocCatID")).Text;
            dtrFilter = dtConstitutions.Select("Doc_Cat_ID=" + strCatID);
            if (dtrFilter.Length > 0)
            {
                dtrFilter[0].BeginEdit();
                dtrFilter[0]["Doc_Cat_OptMan"] = ((CheckBox)grvConsDoc.FindControl("chkOptMan")).Checked == true ? true : false;
                dtrFilter[0]["Doc_Cat_ImageCopy"] = ((CheckBox)grvConsDoc.FindControl("chkImageCopy")).Checked == true ? true : false;
                dtrFilter[0]["Remarks"] = ((TextBox)grvConsDoc.FindControl("txtRemarks")).Text.Replace("'", "~");
                dtrFilter[0].EndEdit();
            }            

        }
      
        DataRow drNew = dtConstitutions.NewRow();


        string[] str = codeName.Split('-');
        string val = str[0] + ((str.Length >= 2) ? '-' + str[1] : "");
            //codeName.Split('-').GetValue(0) + "-" + codeName.Split('-').GetValue(1);

        if (!codeName.ToLower().Contains("others") &&
            dtConstitutions.Select("Doc_Cat_Flag='" + val
            // Condition Added for Code and Description Validation...
            // By Senthilkumar P on 12/mar/2012
                + "' and Doc_Cat_Desc='" + codeDesc + "'").Length > 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Document Type already exists");
            return;
        }
        else if (codeName.ToLower().Contains("others") && codeDesc == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Document Description cannot be empty");
            return;
        }
        else if (codeName.ToLower().Contains("others") && dtConstitutions.Select("Doc_Cat_Desc='" + codeDesc + "'").Length > 0)
        {
            Utility.FunShowAlertMsg(this.Page, "Document Description already exists");
            return;
        }
        
        if (codeName == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Document Type cannot be empty");
            return;
        }
        //if (codeRemarks.Trim().Length == 0 && codeName!="")
        //{
        //    Utility.FunShowAlertMsg(this.Page, "Remarks cannot be empty");
        //    return;
        //}
        // insert the newly added OTHERS  row to Documents master
        if (ViewState["DocID"] == null && codeName.ToLower().Contains("others"))
        {
            if (codeDesc.Trim().Length > 0)
                InsertNewDocumentCategory(codeName.Split('-').GetValue(0).ToString(), codeName, codeDesc);
            // Code can Split and pass to DB because Doc_Cat_Name has 3 Char Max Length.
            // Modified by Senthilkumar on 12/mar/2012
        }
        drNew.BeginEdit();
        drNew["Sno"] = dtConstitutions.Rows.Count+1;
        drNew["Doc_Cat_Name"] = codeName;
        drNew["Doc_Cat_ID"] = ((ViewState["DocID"] != null) ? ViewState["DocID"].ToString() : int.MaxValue.ToString());
        drNew["Doc_Cat_IDAssigned"] = ((ViewState["DocID"] != null) ? ViewState["DocID"].ToString() : int.MaxValue.ToString()); 
        drNew["Doc_Cat_Desc"] = codeDesc.ToUpper();
        drNew["Doc_Cat_Flag"] = val;//((ViewState["DocumentFlag"] != null) ? ViewState["DocumentFlag"].ToString() : "");
        drNew["Doc_Cat_OptMan"] = codeMandatory;
        drNew["Doc_Cat_ImageCopy"] = codeImageCopy;
        drNew["Remarks"] = codeRemarks;
        drNew.EndEdit();
        dtConstitutions.Rows.Add(drNew);
        BindGrid(dtConstitutions);
        ViewState["DocID"] = null;
        // Load the Drop downlist
        //LoadDocumentFlags();
        //grvConsDocuments.Rows[0].Visible = false;
        ViewState["Documents"] = dtConstitutions;
        //FunGetConstitutionCodeDetails();
    }

    private void BindGrid(DataTable dtConstitutions)
    {
        grvConsDocuments.DataSource = dtConstitutions;
        grvConsDocuments.DataBind();
        if (grvConsDocuments.Rows.Count > 0)
        {
            DataRow[] drow = dtConstitutions.Select("Doc_cat_Flag=''");
            if (drow.Length > 0)
            {
                   grvConsDocuments.Rows[0].Visible = false;
            }
        }
        else
        {
            if (ViewState["PageMode"].ToString() == PageModes.Query.ToString())
            {
                Utility.FunShowAlertMsg(this.Page, "No Constitutional documents found to load", strRedirectPage);
                return;
            }
            else
            {
                AddNewRowTable("", "", false, false, "");
                grvConsDocuments.FooterRow.Visible = grvConsDocuments.Columns[8].Visible = true;
                grvConsDocuments.Rows[0].Visible = false;
            }
        }
    }

    private int FunPriGenerateLOBDocDet()
    {
        try
        {
            string strLOBID = string.Empty;
            string strCatID = string.Empty;
            string strOptMan = string.Empty;
            string strImageCopy = string.Empty;
            string strRemarks = string.Empty;

            strbLOBDet.Append("<Root>");
            CheckBox chkLOB = null;
            CheckBox chkConsDoc = null;
            RequiredFieldValidator rfvRemarks = null;
            bool isAtleastOneLOBSelected = false;
            foreach (GridViewRow grvData in grvLOB.Rows)
            {
                chkLOB = ((CheckBox)grvData.FindControl("chkSelectLOB"));
                if (chkLOB.Checked)
                {
                    isAtleastOneLOBSelected = true;
                    strLOBID = ((Label)grvData.FindControl("lblLOBID")).Text;
                    strbLOBDet.Append(" <Details LOB_ID='" + strLOBID + "' /> ");
                }
            }
            if (isAtleastOneLOBSelected == false)
                return 3;
            strbLOBDet.Append("</Root>");
            strXMLLOBDet = strbLOBDet.ToString();
            strbConsDocumentsDet.Append("<Root>");
            bool isMandatorySelected = false;
            bool isRemarksEmpty=false;
            foreach (GridViewRow grvConsDoc in grvConsDocuments.Rows)
            {
                strCatID = ((Label)grvConsDoc.FindControl("lblDocCatID")).Text;
                if (strCatID !=int.MaxValue.ToString())
                {                 
                    strOptMan = ((CheckBox)grvConsDoc.FindControl("chkOptMan")).Checked == true ? "1" : "0";
                    strImageCopy = ((CheckBox)grvConsDoc.FindControl("chkImageCopy")).Checked == true ? "1" : "0";
                    strRemarks = ((TextBox)grvConsDoc.FindControl("txtRemarks")).Text.Trim().Replace("'", "~");
                    //if (strRemarks.Length == 0)
                    //    isRemarksEmpty = true;       
                    if(((CheckBox)grvConsDoc.FindControl("chkOptMan")).Checked==true)
                        isMandatorySelected = true;
                    strbConsDocumentsDet.Append(" <Details Doc_Cat_ID='" + strCatID + "' Doc_Cat_OptMan='" + strOptMan + "' Doc_Cat_ImageCopy='" + strImageCopy + "' Doc_Cat_Remarks='" + formatXMLString(strRemarks) + "' /> ");
                }
                
            }            
            strbConsDocumentsDet.Append("</Root>");
            strXMLConsDocumentsDet = strbConsDocumentsDet.ToString();
            //if (isMandatorySelected == false)
            //    return 1;

            //if (isRemarksEmpty == true)
            //    return 0;
           
            return 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    string formatXMLString(string newString)
    { 
            // >
        newString = newString.Replace(">","&gt;");
            //  <
        newString = newString.Replace("<","&lt;");
            //&
        newString = newString.Replace("&","&amp;");

        //Double Quote " –This does not work here…
        // newString = newString.Replace(ControlChars.Quote,"&quot;"); 
        // newString = newString.Replace(CHR(32),"&quot;");
        //Single Quote '
        newString = newString.Replace("'", "&apos;");

        return newString;
    }

    //This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {

        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

                if (!bCreate)
                {
                    btnSave.Enabled = false;
                    //btnSaveDocType.Enabled = false;
                }
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

                if (!bModify)
                {
                    btnSave.Enabled = false;
                    //btnSaveDocType.Enabled = false;
                }

                trConstitutionCode.Visible = true;
                txtConstitutionCode.Enabled = false;
                // Changed by Bhuvana
                txtConstitutionName.Enabled = true;
                //end here
                lnkCopyProfile.Visible = false;
                btnClear.Enabled = false;
                break;

            case -1:// Query Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                trConstitutionCode.Visible = true;
                txtConstitutionCode.ReadOnly = true;
                txtConstitutionName.ReadOnly = true;
             
                
                lnkCopyProfile.Visible = false;
                //if (bClearList)
                //{
                //    ddlLOB.ClearDropDownList();
                //    ddlProductCode.ClearDropDownList();
                //}
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                break;
        }

    }

    protected void txtRemarks_TextChanged(object sender, EventArgs e)
    {
        TextBox txtremark = (TextBox)sender;
        txtremark.Text = txtremark.Text.TrimStart().TrimEnd(); 
    }

    //Code end

    #endregion

    #region Foorer row Add Button - To add Other Document Type
    protected void btnAddDocType_Click(object sender, EventArgs e)
    {
        DropDownList ddlDocCatGird = (DropDownList)grvConsDocuments.FooterRow.FindControl("ddlDocCatGird");
        TextBox txtOthersGrid = (TextBox)grvConsDocuments.FooterRow.FindControl("txtOthersGrid");
        if (ddlDocCatGird.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Select the Document Category");
            ddlDocCatGird.Focus();
            return;
        }
        else if (txtOthersGrid.Text.Trim() == "")
        {
            Utility.FunShowAlertMsg(this.Page, "Enter the Document Description");
            txtOthersGrid.Focus();
            return;
        }

        PRDDCMgtServicesReference.PRDDCMgtServicesClient ObjPRDDCMasterClient;
        PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjS3G_ORG_PRDDCMasterDataTable = new PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable();
        PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCRow;
        ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
        ObjPRDDCRow = ObjS3G_ORG_PRDDCMasterDataTable.NewS3G_ORG_PRDDCMasterRow();
        try
        {
            ObjPRDDCRow.DocType = "Consti";           
            ObjPRDDCRow.DocCategory = ddlDocCatGird.SelectedValue;            
            ObjPRDDCRow.DocDesc = txtOthersGrid.Text;
            ObjPRDDCRow.Created_By = intUserID;

            ObjS3G_ORG_PRDDCMasterDataTable.AddS3G_ORG_PRDDCMasterRow(ObjPRDDCRow);
            ObjPRDDCMasterClient = new PRDDCMgtServicesReference.PRDDCMgtServicesClient();
            intErrCode = ObjPRDDCMasterClient.FunPubCreateOtherDocDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_PRDDCMasterDataTable, SerMode));

            if (intErrCode == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Constitution other document added successfully");
                if (Request.QueryString["qsConstitutionId"] != null)
                    intConstitutionID = Convert.ToInt32(Request.QueryString["qsConstitutionId"]);
                FunGetConstitutionCodeDetails();
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Constitution other document already exists");
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
        }
        finally
        {
            ObjPRDDCMasterClient.Close();
        }
    }
    #endregion

    #region "grvConstitution Row Created event "
    protected void grvConstitution_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView grvSender = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkSel");
            chkBoxSelect.Attributes["onclick"] = string.Format
            (
            "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
            grvSender.ClientID
            );
        }
    }
    #endregion 

    protected void SaveRow(object sender, EventArgs e)
    {
        UserControls_S3GAutoSuggest drCodeName = (UserControls_S3GAutoSuggest)grvConsDocuments.FooterRow.FindControl("ddlDocCatGird");
        //DropDownList drCodeName = grvConsDocuments.FooterRow.FindControl("ddlDocCatGird") as DropDownList;
        //DropDownList drpLst = lvContacts.InsertItem.FindControl("drplstDesc") as DropDownList;
        TextBox txtDesc=grvConsDocuments.FooterRow.FindControl("txtOthersGrid") as TextBox;
        CheckBox chkOptMan = grvConsDocuments.FooterRow.FindControl("chkOptManIns") as CheckBox;
        CheckBox chkImageCopy = grvConsDocuments.FooterRow.FindControl("chkImageCopyIns") as CheckBox;
        TextBox txtRemarks = grvConsDocuments.FooterRow.FindControl("txtRemarks") as TextBox;
        if (drCodeName.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this.Page, "Select Document Type");
            drCodeName.Focus();
            return;
        }
        if (txtDesc.Text == string.Empty)
        {
            Utility.FunShowAlertMsg(this.Page, "Enter the Other document description.");
            txtDesc.Focus();
            return;
        }

        AddNewRowTable(drCodeName.SelectedText, txtDesc.Text.Trim(), chkOptMan.Checked, chkImageCopy.Checked, txtRemarks.Text.Trim());

        
    }

    protected void RemoveRow(object sender, EventArgs e)
    {
        LinkButton lnkRemovePRDDC = (LinkButton)sender;
        GridViewRow gvRow = (GridViewRow)lnkRemovePRDDC.Parent.Parent;
        Label lbl = gvRow.FindControl("lblDocCatID") as Label;
        removeRow(lbl.Text);
    }
    void removeRow(string sno)
    {
        DataTable dtConstitutions = ViewState["Documents"] as DataTable;
        DataRow[] dtrRow = dtConstitutions.Select("Doc_Cat_ID=" + sno);
        if (dtrRow.Length > 0)
            dtConstitutions.Rows.Remove(dtrRow[0]);
        if (dtConstitutions.Rows.Count == 0)
        {
            ViewState["PageMode"] = PageModes.Create.ToString();
            AddNewRowTable("", "", false, false, "");
            dtConstitutions = ViewState["Documents"] as DataTable;
            dtConstitutions.AcceptChanges();
            BindGrid(dtConstitutions);            
        }
        else
            BindGrid(dtConstitutions);

        
        ViewState["Documents"] = dtConstitutions;
        //LoadDocumentFlags();
    }

    protected void txtOthersGrid_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (System.Web.HttpContext.Current.Session["dtConCodes"] != null)
            {
                DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["dtConCodes"];
                if (dt.Rows.Count > 0)
                {
                    string filterExpression = "Document_Description = '" + (grvConsDocuments.FooterRow.FindControl("txtOthersGrid") as TextBox).Text.Trim() + "'";
                    DataRow[] dtSuggestions = dt.Select(filterExpression);
                    if (dtSuggestions.Length > 0)
                    {
                        ViewState["DocID"] = dtSuggestions[0][ConstitutionDocumentCategory_ID].ToString();
                        ViewState["DocumentFlag"] = dtSuggestions[0]["Document_Flag"].ToString();
                    }
                }
            }
            grvConsDocuments.FooterRow.FindControl("chkOptManIns").Focus();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
        }
    }

    [WebMethod]
    public static string[] getDocumentsList(String prefixText, int count)
    {
        //dictParam = new Dictionary<string, string>();
        //dictParam.Add("@CatName", drCodeName.SelectedValue);
        //drpLst.BindDataTable(SPNames.S3G_GetCons_Category, dictParam, new string[] { "ConstitutionDocumentCategory_ID", "Document_Description" });        
        List<String> suggetions = null;
        DataTable dtDesc = new DataTable();
        try
        {
            if (System.Web.HttpContext.Current.Session["dtConCodes"] != null)
            {
                dtDesc = (DataTable)System.Web.HttpContext.Current.Session["dtConCodes"];
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
            string filterExpression = "Document_Description like '" + key + "%'";
            DataRow[] dtSuggestions = dt1.Select(filterExpression);
            foreach (DataRow dr in dtSuggestions)
            {
                string suggestion = dr["Document_Description"].ToString();
                suggestions.Add(suggestion);
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
        return suggestions;
    }

    protected void ddlDocCatGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Commnted and Added by Saranya 29-Feb-2012 to bind the Document Description Details
        ViewState["DocID"] = null;
        dictParam = new Dictionary<string, string>();
        //DropDownList ddlCode = grvConsDocuments.FooterRow.FindControl("ddlDocCatGird") as DropDownList;
        UserControls_S3GAutoSuggest ddlCode = grvConsDocuments.FooterRow.FindControl("ddlDocCatGird") as UserControls_S3GAutoSuggest;
        TextBox txtDescription = grvConsDocuments.FooterRow.FindControl("txtOthersGrid") as TextBox;

        txtDescription.Text = string.Empty;
        if (Convert.ToInt32(ddlCode.SelectedValue) > 0)
        {
            dictParam.Add("@Doc_Id", ddlCode.SelectedValue);
           
            DataSet dsCOSDesc = Utility.GetTableValues(SPNames.S3G_GetCons_Category, dictParam);
            System.Web.HttpContext.Current.Session["dtConCodes"] = dsCOSDesc.Tables[0];
            if (dsCOSDesc.Tables[0].Rows[0]["Document_Description"].ToString().ToLower() == "others")
            {
                txtDescription.Enabled = true;
                txtDescription.Focus();
            }
            else
            {

                txtDescription.Enabled = false;
                txtDescription.Text = dsCOSDesc.Tables[0].Rows[0]["Document_Description"].ToString();
                ViewState["DocID"] = dsCOSDesc.Tables[0].Rows[0][ConstitutionDocumentCategory_ID].ToString();
            }
            
        }
        //End Here
        
        //modified by Saran 29-April-2011 for the dropdown(ddlDocCatGird) was appended by Document Desription
        //if (ddlCode.SelectedValue.EndsWith("OTH"))
        //{
        //    dictParam.Add("@Doc_Description", txtDescription.Text);        
        //    Session["dtConCodes"] = Utility.GetDefaultData(SPNames.S3G_GetCons_Category, dictParam);
        //    txtDescription.Text = String.Empty;
        //    ViewState["DocID"] = null;
        //    txtDescription.Enabled = true;
        //    txtDescription.Focus();
        //}
        //else
        //    using (DataTable dt = Utility.GetDefaultData(SPNames.S3G_GetCons_Category, dictParam))
        //    {
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            txtDescription.Text = dt.Rows[0]["Document_Description"].ToString();
        //            ViewState["DocID"] = dt.Rows[0]["ConstitutionDocumentCategory_ID"].ToString();
        //            txtDescription.Enabled = (txtDescription.Enabled == true) ? false : false;
        //        }
        //    }
       
    }

    protected void grvConsDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }

    

    #region Propertites


    /// <summary>
    /// Added by PSK reg. Oracle Conversion     -   09/Jan/2012
    /// </summary>
    public string ConstitutionDocumentCategory_ID
    {
        get 
        {
            return "Constdoccateg_id";
        }

    }

    #endregion
}




