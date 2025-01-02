#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Global Parameter Setup
/// Created By			: Kannan RC
/// Created Date		: 26-Aug-2010
/// Modified by         : Tamilselvan.S
/// Modified date       : 14-Feb-2011
/// Purpose             : Bug Fixing
/// <Program Summary>
#endregion

#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
#endregion

public partial class LoanAdmin_S3GLoanAdGlobalParameterSetup_Add : ApplyThemeForProject
{

    #region [Intialization]

    Dictionary<string, string> dictParam = null;
    int intGlobalID;
    int intUserId;
    int intCompanyId;
    int intErrCode;
    int intGpsSuffixRW = 0;
    string strMode;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bIsActive = false;
    bool bClearList = false;
    public string strDateFormat = string.Empty;
    public int intMaxDigit = 0;
    string strKey = "Insert";
    string strPageName = "Loan Admin - Global Parameter Setup";
    string strAlert = "alert('__ALERT__');";
    //string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdGlobalParameterSetup_Add.aspx';";
    //string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdGlobalParameterSetup_Add.aspx';";
    string strRedirectPageAdd = "window.location.href='../Common/HomePage.aspx';";
    string strRedirectPageView = "window.location.href='../Common/HomePage.aspx';";

    DataTable dtAccount = new DataTable();
    DataTable dttemp = new DataTable();
    SerializationMode SerMode = SerializationMode.Binary;

    LoanAdminMgtServicesClient objLoanAdmin_MasterClient;
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objGlobalSetup_MasterClient;
    OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable ObjS3G_GlobalParameterLOBMasters = new OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable();
    LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable ObjS3G_GlobalParameterLOBMaster = new LoanAdminMgtServices.S3G_LOANAD_GetLOBDetailsDataTable();
    LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable ObjS3G_GlobalParameterMaster = new LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable();
    DataTable dtHeader = null;
    DataTable dtDetails = null;

    #endregion [Intialization]

    #region [Event's]

    #region [Page Event's]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPubPageLoad();

            //Button btnAdd = ((Button)gvAccount.FooterRow.FindControl("btnAddrow"));
            //DropDownList ddllist = ((DropDownList)gvAccount.FooterRow.FindControl("ddlLOB"));

        }
        catch (Exception ex)
        {
            cvGlobal.ErrorMessage = "Unable to Load the page";
            cvGlobal.IsValid = false;
        }
    }

    //protected void Page_Upload(object sender, EventArgs e)
    //{ }

    #endregion [Page Event's]

    #region [Drop down Event's]

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string strFieldAtt = ((DropDownList)sender).ClientID;
            //string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvAccount")).Replace("gvAccount_ctl", "");
            //int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            //gRowIndex = gRowIndex - 2;
            //DropDownList ddllob = gvAccount.FooterRow.FindControl("ddlLOB") as DropDownList;
            //DropDownList ddlProduct = gvAccount.FooterRow.FindControl("ddlProduct") as DropDownList;
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //Procparam.Add("@LOB_ID", ddllob.SelectedValue);
            //Procparam.Add("@Company_ID", intCompanyId.ToString());
            //FunPubFillProduct(Procparam, ddlProduct);
        }
        catch (Exception ex)
        {
            cvGlobal.ErrorMessage = "Unable to load the Product";
            cvGlobal.IsValid = false;
        }
    }

    #endregion [Drop down Event's]

    #region [Grid Event's]

    //protected void gvAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Footer)
    //        {
    //            dtAccount = (DataTable)ViewState["currenttable"];
    //            DropDownList ddllob = e.Row.FindControl("ddlLOB") as DropDownList;
    //            DropDownList ddlProduct = e.Row.FindControl("ddlProduct") as DropDownList;
    //            Dictionary<string, string> Procparam = new Dictionary<string, string>();
    //            //if (intGlobalID == 0)
    //            //{
    //            Procparam.Add("@Is_Active", "1");
    //            //}
    //            Procparam.Add("@User_Id", intUserId.ToString());
    //            Procparam.Add("@Company_ID", intCompanyId.ToString());
    //            ddllob.BindDataTable("S3G_ORG_GetLOBDetails", Procparam, new string[] { "LOB_ID", "LOB" });

    //            if ((intGlobalID > 0) && (strMode == "Q"))
    //            {
    //                Button btnAdd = e.Row.FindControl("btnAddrow") as Button;
    //                btnAdd.Enabled = false;
    //                ddllob.Enabled = false;
    //                ddlProduct.Enabled = false;
    //            }
    //        }
    //        if ((intGlobalID > 0) && (strMode == "Q"))
    //        {
    //            if (e.Row.RowType == DataControlRowType.DataRow)
    //            {
    //                LinkButton Ltnbtn = e.Row.FindControl("btnRemove") as LinkButton;
    //                Ltnbtn.Enabled = false;
    //            }
    //        }
    //        else
    //        {
    //            if (e.Row.RowType == DataControlRowType.DataRow)
    //            {
    //                Label lblLobID = e.Row.FindControl("lblLobID") as Label;
    //                Label lblProductID = e.Row.FindControl("lblProductID") as Label;
    //                TextBox txtProduct = e.Row.FindControl("txtProduct") as TextBox;
    //                LinkButton Ltnbtn = e.Row.FindControl("btnRemove") as LinkButton;

    //                if (lblProductID.Text != null && lblProductID.Text != "" && lblProductID.Text != string.Empty && txtProduct.Text != null && txtProduct.Text != "" && txtProduct.Text != string.Empty)
    //                    Ltnbtn.Enabled = true;
    //                else
    //                    Ltnbtn.Enabled = false;
    //                if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
    //                    e.Row.Enabled = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        cvGlobal.ErrorMessage = ex.Message;
    //        cvGlobal.IsValid = false;
    //    }
    //}

    //protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName == "AddNew")
    //        {
    //            CheckBox CbxInvoice = (CheckBox)gvAccount.FooterRow.FindControl("CbxInvoice");
    //            CheckBox CbxAsset = (CheckBox)gvAccount.FooterRow.FindControl("CbxAsset");
    //            CheckBox CbxAssetIns = (CheckBox)gvAccount.FooterRow.FindControl("CbxAssetIns");
    //            TextBox txtlob = (TextBox)gvAccount.FindControl("ddlLOB");
    //            TextBox txtproduct = (TextBox)gvAccount.FindControl("ddlProduct");

    //            DropDownList ddlLob = (DropDownList)gvAccount.FooterRow.FindControl("ddlLOB");
    //            if (ddlLob.SelectedIndex == 0)
    //            {
    //                cvGlobal.ErrorMessage = "Select the Line of Business";
    //                cvGlobal.IsValid = false;
    //                return;
    //            }
    //            DropDownList ddlProduct = (DropDownList)gvAccount.FooterRow.FindControl("ddlProduct");
    //            foreach (GridViewRow gr in gvAccount.Rows)
    //            {
    //                TextBox txtRowlob = (TextBox)gr.FindControl("txtLOB");
    //                TextBox txtRowproduct = (TextBox)gr.FindControl("txtProduct");
    //                if (txtRowproduct.Text.Length != 0)
    //                {
    //                    string[] str_Product = txtRowproduct.Text.Split('-');
    //                    string[] str_ddlProduct = ddlProduct.SelectedItem.Text.Split('-');
    //                    if (ddlLob.SelectedItem.Text == txtRowlob.Text && str_Product[0].ToString().Trim().ToLower() == str_ddlProduct[0].ToString().Trim().ToLower())
    //                    {
    //                        Utility.FunShowAlertMsg(this.Page, "Selected Line of Business and Product already exists");
    //                        return;
    //                    }
    //                }
    //            }
    //            foreach (GridViewRow gr in gvAccount.Rows)
    //            {
    //                TextBox txtRowproduct = (TextBox)gr.FindControl("txtProduct");
    //                TextBox txtRowlob = (TextBox)gr.FindControl("txtLOB");
    //                if (txtRowproduct.Text.Length == 0)
    //                {
    //                    string[] str_Product = txtRowproduct.Text.Split('-');
    //                    string[] str_ddlProduct = ddlProduct.SelectedItem.Text.Split('-');
    //                    if (ddlLob.SelectedItem.Text == txtRowlob.Text && str_Product[0].ToString().Trim().ToLower() == str_ddlProduct[0].ToString().Trim().ToLower())
    //                    {
    //                        Utility.FunShowAlertMsg(this.Page, "Selected Line of Business already exists");
    //                        return;
    //                    }
    //                }
    //            }
    //            foreach (GridViewRow gr in gvAccount.Rows)
    //            {
    //                TextBox txtRowproduct = (TextBox)gr.FindControl("txtProduct");
    //                TextBox txtRowlob = (TextBox)gr.FindControl("txtLOB");
    //                string[] str_Lob = txtRowlob.Text.Split('-');
    //                string[] str_ddllob = ddlLob.SelectedItem.Text.Split('-');
    //                string[] str_Product = txtRowproduct.Text.Split('-');
    //                string[] str_ddlProduct = ddlProduct.SelectedItem.Text.Split('-');
    //                if (ddlLob.SelectedItem.Text == txtRowlob.Text && str_Product[0].ToString().Trim().ToLower() == str_ddlProduct[0].ToString().Trim().ToLower())
    //                {
    //                    Utility.FunShowAlertMsg(this.Page, "Selected Line of Business already exists");
    //                    return;
    //                }
    //            }
    //            string strProduct = string.Empty;
    //            if (Convert.ToInt32(ddlProduct.SelectedValue) != 0)
    //            {
    //                strProduct = ddlProduct.SelectedItem.Text;
    //            }
    //            FunPubGetAccountGridLastValues();
    //            FunPubAddAccountDetails(Convert.ToInt32(ddlLob.SelectedValue), ddlLob.SelectedItem.Text, Convert.ToInt32(ddlProduct.SelectedValue), strProduct);
    //            CbxInvoice.Checked = CbxAsset.Checked = CbxAssetIns.Checked = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        cvGlobal.ErrorMessage = "Unable to Add the record in grid";
    //        cvGlobal.IsValid = false;
    //    }
    //}

    //protected void gvAccount_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dtDelete;
    //        dtDelete = (DataTable)ViewState["currenttable"];
    //        dtDelete.Rows.RemoveAt(e.RowIndex);
    //        CheckBox CbxInvoice = (CheckBox)gvAccount.Rows[0].FindControl("CbxFInvoice");
    //        CheckBox CbxAsset = (CheckBox)gvAccount.Rows[0].FindControl("CbxFAsset");
    //        CheckBox CbxAssetIns = (CheckBox)gvAccount.Rows[0].FindControl("CbxFAssetIns");
    //        if (dtDelete.Rows.Count <= 0)
    //        {
    //            dtDelete = null;
    //            dtAccount.Clear();
    //            ViewState["currenttable"] = null;
    //            DataRow dr;
    //            dtAccount.Columns.Add("LOB_ID");
    //            dtAccount.Columns.Add("LOB");
    //            dtAccount.Columns.Add("Product_Code");
    //            dtAccount.Columns.Add("Product_ID");
    //            DataColumn dc = new DataColumn("Parameter_Value", System.Type.GetType("System.Boolean"));
    //            DataColumn dc1 = new DataColumn("Parameter_Value1", System.Type.GetType("System.Boolean"));
    //            DataColumn dc2 = new DataColumn("Parameter_Value2", System.Type.GetType("System.Boolean"));
    //            DataColumn dc3 = new DataColumn("IsActive", System.Type.GetType("System.Boolean"));
    //            dc.DefaultValue = false;
    //            dc1.DefaultValue = false;
    //            dc2.DefaultValue = false;
    //            dc3.DefaultValue = false;
    //            dtAccount.Columns.Add(dc);
    //            dtAccount.Columns.Add(dc1);
    //            dtAccount.Columns.Add(dc2);
    //            dtAccount.Columns.Add(dc3);
    //            dr = dtAccount.NewRow();
    //            dtAccount.Rows.Add(dr);
    //            ViewState["currenttable"] = dtAccount;
    //            gvAccount.DataSource = dtAccount;
    //            gvAccount.DataBind();
    //            gvAccount.Rows[0].Visible = false;
    //            return;
    //        }
    //        gvAccount.DataSource = dtDelete;
    //        gvAccount.DataBind();
    //        ViewState["currenttable"] = dtDelete;
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        cvGlobal.ErrorMessage = ex.Message;
    //        cvGlobal.IsValid = false;
    //    }
    //}

    #endregion [Grid Event's]

    #region [Check Box Event's]

    protected void CbxEsm_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncome_")).Replace("gvIncome_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxIRR")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxSOD")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxProduct")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    protected void CbxIRR_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncome_")).Replace("gvIncome_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxEsm")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxSOD")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxProduct")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    protected void CbxSOD_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncome_")).Replace("gvIncome_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                ((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxEsm")).Checked = false;
                ((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxIRR")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxProduct")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    protected void CbxProduct_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncome_")).Replace("gvIncome_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxEsm")).Checked = false;
                ((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxIRR")).Checked = false;
                //((CheckBox)gvIncome.Rows[gRowIndex].FindControl("CbxSOD")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    protected void CbxIns_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncomeType_")).Replace("gvIncomeType_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                ((CheckBox)gvIncomeType.Rows[gRowIndex].FindControl("CbxAccounting")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    protected void CbxAccounting_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((CheckBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("gvIncomeType_")).Replace("gvIncomeType_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            if (((CheckBox)sender).Checked == true)
            {
                ((CheckBox)gvIncomeType.Rows[gRowIndex].FindControl("CbxIns")).Checked = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = ex.Message;
            cvGlobal.IsValid = false;
        }
    }

    #endregion [Check Box Event's]

    #region [Button Event's]

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (FunPubCheckEmptyAllGrid())
            {
                FunPubSaveGlobalParamDetails();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvGlobal.ErrorMessage = "Unable to Save the Details";
            cvGlobal.IsValid = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../LoanAdmin/S3gLoanAdTransLander.aspx?Code=GPS");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPubClearAllControls();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion [Button Event's]

    #endregion [Event's]

    #region [Function's]

    #region [Page Load]

    public void FunPubPageLoad()
    {
        try
        {
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                intGlobalID = Convert.ToInt32(fromTicket.Name);
                strMode = Request.QueryString["qsMode"];
            }
            if (ViewState["HEADER_TABLE"] == null)
            {
                dtHeader = new DataTable();
                dtHeader.Columns.Add("ID", typeof(int));
                dtHeader.Columns.Add("LOB_ID", typeof(int));
                dtHeader.Columns.Add("Company_ID", typeof(int));
                dtHeader.Columns.Add("Product_ID", typeof(int));
                dtHeader.Columns.Add("Module_ID", typeof(int));
                dtHeader.Columns.Add("Param_Set_Desc", typeof(string));
                dtHeader.Columns.Add("Param_Set_Date", typeof(DateTime));
                dtHeader.Columns.Add("Created_By", typeof(int));
                dtHeader.Columns.Add("Created_On", typeof(DateTime));
                dtHeader.Columns.Add("Txn_ID", typeof(int));

                ViewState["HEADER_TABLE"] = dtHeader;
            }
            if (ViewState["DETAILS_TABLE"] == null)
            {
                dtDetails = new DataTable();
                dtDetails.Columns.Add("Program_Set_ID", typeof(int));
                dtDetails.Columns.Add("Company_ID", typeof(int));
                dtDetails.Columns.Add("Parameter_Code", typeof(int));    // CheckBox(1,2,3), textbox 4, checkbox(5,6,7,8)            
                dtDetails.Columns.Add("Parameter_Value", typeof(string));
                ViewState["DETAILS_TABLE"] = dtDetails;
            }
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            intMaxDigit = ObjS3GSession.ProGpsPrefixRW;
            intGpsSuffixRW = ObjS3GSession.ProGpsSuffixRW;
            btnCancel.Visible = false;
            intGlobalID = Convert.ToInt32(hdnGlobalParamId.Value);
            bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
            if (!IsPostBack)
            {
                txtWaivedOffAmount.MaxLength = intMaxDigit;
                FunPriGetLookUpList();
                FunPriGetGeneralLookUpList();
                FunPriGPSDetails();
                intGlobalID = Convert.ToInt32(hdnGlobalParamId.Value);
                if (!bIsActive || ObjUserInfo.ProUserLevelIdRW != 5)
                {
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    strMode = "Q";
                }
                else
                {
                    if (intGlobalID > 0)
                    {
                        if ((bModify) && ObjUserInfo.ProUserLevelIdRW == 5) //(ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(hdnUserId.Value), Convert.ToInt32(hdnUserLevelId.Value))))
                        {
                            strMode = "M";

                        }
                        else
                        {
                            strMode = "Q";
                        }
                    }
                    else
                    {
                        strMode = "C";
                    }
                }

                if (strMode == "M")
                {
                    FunPriDisableControls(1);

                }
                else if (strMode == "Q")
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
            FunPriSetMaxLength();
            //if (strMode == "C")
            //{
            //    ((DropDownList)gvAccount.FooterRow.FindControl("ddlLOB")).Focus();
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Page Load]

    public void FunPubGetAccountGridLastValues()
    {
        if (((DataTable)ViewState["currenttable"]).Rows.Count == 1 && Convert.ToString(((DataTable)ViewState["currenttable"]).Rows[0]["Lob_ID"]) == "" && Convert.ToString(((DataTable)ViewState["currenttable"]).Rows[0]["Product_ID"]) == "")
        {
            return;
        }
        else
        {
            //foreach (GridViewRow gvr in gvAccount.Rows)
            //{
            //    Label lblLobID = gvr.FindControl("lblLobID") as Label;
            //    Label lblProductID = gvr.FindControl("lblProductID") as Label;
            //    CheckBox CbxFInvoice = gvr.FindControl("CbxFInvoice") as CheckBox;
            //    CheckBox CbxFAsset = gvr.FindControl("CbxFAsset") as CheckBox;
            //    CheckBox CbxFAssetIns = gvr.FindControl("CbxFAssetIns") as CheckBox;
            //    DataRow[] drlist = ((DataTable)ViewState["currenttable"]).Select("Lob_ID='" + lblLobID.Text + "' and Product_ID='" + lblProductID.Text + "'");
            //    if (drlist.Length > 0)
            //    {
            //        drlist[0].BeginEdit();
            //        drlist[0]["Parameter_Value"] = CbxFInvoice.Checked;
            //        drlist[0]["Parameter_Value1"] = CbxFAsset.Checked;
            //        drlist[0]["Parameter_Value2"] = CbxFAssetIns.Checked;
            //        drlist[0].EndEdit();
            //        drlist[0].AcceptChanges();
            //    }
            //}
        }
    }

    public void FunPubAddAccountDetails(int intLob, string strLob, int intProduct, string strProduct)
    {
        try
        {
            DataRow dr;
            dtAccount = (DataTable)ViewState["currenttable"];
            dr = dtAccount.NewRow();
            if (dtAccount.Rows.Count > 0)
            {
                if (dtAccount.Rows[0]["Lob_ID"].ToString() == string.Empty)
                {
                    dtAccount.Rows[0].Delete();
                }
            }
            dr["LOB_ID"] = intLob.ToString();
            dr["LOB"] = strLob;
            dr["Product_Code"] = strProduct;
            dr["Product_ID"] = intProduct.ToString();
            dr["IsActive"] = true;
            dr["Parameter_Value"] = false;
            dr["Parameter_Value1"] = false;
            dr["Parameter_Value2"] = false;
            dtAccount.Rows.Add(dr);

            //gvAccount.DataSource = dtAccount;
            //gvAccount.DataBind();
            ViewState["currenttable"] = dtAccount;
            //if (dtAccount.Rows.Count == 0)
            //    SetInitialRow();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [Fill Product based on LOB]

    public void FunPubFillProduct(Dictionary<string, string> Procparam, DropDownList ddlProduct)
    {
        try
        {
            ddlProduct.BindDataTable("S3G_LOANAD_GetLOBProductList", Procparam, new string[] { "Product_ID", "Product_Code" });
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #endregion [Fill Product based on LOB]

    #region [Set Initial Row]

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add("LOB_ID");
        dt.Columns.Add("LOB");
        dt.Columns.Add("Product_ID");
        dt.Columns.Add("Product_Code");
        dt.Columns.Add("Parameter_Code");
        dt.Columns.Add("Company_ID");
        DataColumn dc = new DataColumn("Parameter_Value", System.Type.GetType("System.Boolean"));
        DataColumn dc1 = new DataColumn("Parameter_Value1", System.Type.GetType("System.Boolean"));
        DataColumn dc2 = new DataColumn("Parameter_Value2", System.Type.GetType("System.Boolean"));
        DataColumn dc3 = new DataColumn("IsActive", System.Type.GetType("System.Boolean"));

        dc.DefaultValue = false;
        dc1.DefaultValue = false;
        dc2.DefaultValue = false;
        dc3.DefaultValue = false;
        dt.Columns.Add(dc);
        dt.Columns.Add(dc1);
        dt.Columns.Add(dc2);
        dt.Columns.Add(dc3);

        dr = dt.NewRow();
        dr["LOB"] = string.Empty;
        dr["LOB_ID"] = string.Empty;
        dr["Product_ID"] = string.Empty;
        dr["Product_Code"] = string.Empty;
        dr["Company_ID"] = string.Empty;
        dr["Parameter_Value"] = false;
        dr["Parameter_Value1"] = false;
        dr["Parameter_Value2"] = false;
        dr["IsActive"] = false;
        dr["Parameter_Code"] = string.Empty;

        dt.Rows.Add(dr);
        ViewState["currenttable"] = dt;

        //gvAccount.DataSource = dt;
        //gvAccount.DataBind();
        //gvAccount.Rows[0].Visible = false;

    }

    #endregion [Set Initial Row]

    #region LookUp

    private void FunPriGetLookUpList()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@LookupType_Code", "18");
            ddlInterest.BindDataTable("S3G_LOANAD_GetLookUpValues", dictParam, new string[] { "Lookup_Code", "Lookup_Description" });
            ddlInterest.SelectedIndex = 2;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGeneralLookUpList()
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", intCompanyId.ToString());
        dictParam.Add("@LookupType_Code", "61");
        ddlOD1.BindDataTable("S3G_LOANAD_GetLookUpValues", dictParam, new string[] { "Lookup_Code", "Lookup_Description" });
    }

    #endregion

    #region [Button Save]

    public bool FunPubCheckEmptyAllGrid()
    {
        //check Account Grid for empty 
        ////foreach (GridViewRow grvRow in gvAccount.Rows)
        ////{
        ////    TextBox txtlob = (TextBox)grvRow.FindControl("txtLOB");
        ////    if (txtlob.Text == string.Empty && txtlob.Text == "")
        ////    {
        ////        cvGlobal.ErrorMessage = "Enter the Account Activation";
        ////        cvGlobal.IsValid = false;
        ////        return false;
        ////    }
        ////} 
        //check Income Grid for not tick 
        foreach (GridViewRow gvrRow in gvIncome.Rows)
        {
            //CheckBox chkESM = gvrRow.FindControl("CbxEsm") as CheckBox;
            CheckBox chkIRR = gvrRow.FindControl("CbxIRR") as CheckBox;
           // CheckBox chkSOD = gvrRow.FindControl("CbxSOD") as CheckBox;
           // CheckBox chkProduct = gvrRow.FindControl("CbxProduct") as CheckBox;
            if (!chkIRR.Checked)
            {
                cvGlobal.ErrorMessage = "Select the Income Recognition method for all the Line of Business ";
                cvGlobal.IsValid = false;
                return false;
            }
        }
        //check Income type Grid for not tick 
        foreach (GridViewRow gvrRow in gvIncomeType.Rows)
        {
            CheckBox chkIBasis = gvrRow.FindControl("CbxIns") as CheckBox;
            CheckBox chkAccBasis = gvrRow.FindControl("CbxAccounting") as CheckBox;
            if (!chkIBasis.Checked && !chkAccBasis.Checked)
            {
                cvGlobal.ErrorMessage = "Select the Income Recognition type for all the Line of Business ";
                cvGlobal.IsValid = false;
                return false;
            }
        }
        //check gapDays Grid for empty 
        //foreach (GridViewRow grvRow in gvGapDays.Rows)
        //{
        //    TextBox txtRegNo = (TextBox)grvRow.FindControl("txtDays");
        //    if (txtRegNo.Text == string.Empty && txtRegNo.Text == "")
        //    {
        //        cvGlobal.ErrorMessage = "Enter the Gap Days for all the Line of Business ";
        //        cvGlobal.IsValid = false;
        //        return false;
        //    }
        //}
        //check Revision Grid for empty 
        foreach (GridViewRow grvRow in gvRevision.Rows)
        {
            Label lblLOB = grvRow.FindControl("lblLOB") as Label;
            TextBox txtRegNo = (TextBox)grvRow.FindControl("txtAlpha");
            if (txtRegNo.Text == string.Empty && txtRegNo.Text == "")
            {
                cvGlobal.ErrorMessage = "Enter the Revision for all the Line of Business ";
                cvGlobal.IsValid = false;
                return false;
            }
        }
        //check OverDue Grid for empty 
        //foreach (GridViewRow grvRow in gvOverdue.Rows)
        //{
        //    Label lblLOB = grvRow.FindControl("lblLOB") as Label;
        //    TextBox txt1 = (TextBox)grvRow.FindControl("txtDenoDays");
        //    TextBox txt2 = (TextBox)grvRow.FindControl("txtGraceDays");
        //    TextBox txt3 = (TextBox)grvRow.FindControl("txtODIRate");

        //    if ((txt1.Text == string.Empty && txt1.Text == "") || (txt2.Text == string.Empty && txt2.Text == "") || (txt3.Text == string.Empty && txt3.Text == ""))
        //    {
        //        cvGlobal.ErrorMessage = "Enter the Overdue Interest for all the Line of Business ";
        //        cvGlobal.IsValid = false;
        //        return false;
        //    }
        //}
        //check account Preclosure Gap Days Grid for empty 
        foreach (GridViewRow grvRow in gvGapDaysBWPMCDate.Rows)
        {
            Label lblLOB = grvRow.FindControl("lblLobID") as Label;
            TextBox txtDays = (TextBox)grvRow.FindControl("txtDays");
            if (txtDays.Text == string.Empty && txtDays.Text == "")
            {
                cvGlobal.ErrorMessage = "Enter the Account Preclosure Gap Days for all the Line of Business ";
                cvGlobal.IsValid = false;
                return false;
            }
        }
        //check Future Installment Grid for empty 
        //foreach (GridViewRow grvRow in gvFutureInstallments.Rows)
        //{
        //    Label lblLOB = grvRow.FindControl("lblLobID") as Label;
        //    TextBox txtInstallment = (TextBox)grvRow.FindControl("txtInstallment");
        //    if (txtInstallment.Text == string.Empty && txtInstallment.Text == "")
        //    {
        //        cvGlobal.ErrorMessage = "Enter the Minimum No of Installment eligible for Account consolidation, for all the Line of Business ";
        //        cvGlobal.IsValid = false;
        //        return false;
        //    }
        //}
        //check account No of Split Grid for empty 
        //foreach (GridViewRow grvRow in gvSplit.Rows)
        //{
        //    Label lblLOB = grvRow.FindControl("lblLobID") as Label;
        //    TextBox txtSplit = (TextBox)grvRow.FindControl("txtSplit");
        //    if (txtSplit.Text == string.Empty && txtSplit.Text == "")
        //    {
        //        cvGlobal.ErrorMessage = "Enter the Minimum Time for split accepted, for all the Line of Business ";
        //        cvGlobal.IsValid = false;
        //        return false;
        //    }
        //}

        return true;
    }

    public void FunPubSaveGlobalParamDetails()
    {
        StringBuilder strXMLHeader;// = new StringBuilder();
        StringBuilder strbuXMLDetails = new StringBuilder();
        string strKey = "Insert";
        string strAlert = "alert('__ALERT__');";
        //string strRedirectPageView = "window.location.href='../LoanAdmin/S3GLoanAdGlobalParameterSetup_Add.aspx';";
        //string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdGlobalParameterSetup_Add.aspx';";
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            // FunProGetXml(out strXMLHeader, out strbuXMLDetails);

            FunProWriteXML(out strXMLHeader);

            LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable ObjS3G_GlobalInsDataTable = new LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersDataTable();
            LoanAdminMgtServices.S3G_lOANAD_InsGLOBALParametersRow ObjGlobalInsRow;
            ObjGlobalInsRow = ObjS3G_GlobalInsDataTable.NewS3G_lOANAD_InsGLOBALParametersRow();

            ObjGlobalInsRow.XML_GlobalParamDeltails = strXMLHeader.ToString();
            ObjGlobalInsRow.XML_ParamDeltails = strbuXMLDetails.ToString();

            ObjS3G_GlobalInsDataTable.AddS3G_lOANAD_InsGLOBALParametersRow(ObjGlobalInsRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjS3GGlobalInsDataTable = ClsPubSerialize.Serialize(ObjS3G_GlobalInsDataTable, SerMode);

            if (intGlobalID > 0)
            {
                intErrCode = objLoanAdmin_MasterClient.FunPubModifyGPSDetails(SerMode, ObjS3GGlobalInsDataTable);
            }
            else
            {
                intErrCode = objLoanAdmin_MasterClient.FunPubCreateGlobalParameterDetails(SerMode, ObjS3GGlobalInsDataTable);
            }
            if (intErrCode == 0)
            {
                if (intGlobalID > 0)
                {
                    strKey = "Modify";
                    strAlert = "Global Parameter Details updated successfully";
                    //strAlert = strAlert.Replace("__ALERT__", "");
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    strRedirectPageView = "";
                }
                else
                {
                    strAlert = "Global Parameter Details added successfully";
                    strAlert += @"\n\nWould you like to Modify the record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    strRedirectPageView = "";

                }
            }
            else if (intErrCode == 50 || intErrCode == -3)  //Added by Tamilselvan.S On 5/03/2011
            {
                if (intGlobalID > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Unable to update the Global Parameter Details");
                    strRedirectPageView = "";
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Unable to insert the Global Parameter Details");
                    strRedirectPageView = "";

                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
            throw;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
                objLoanAdmin_MasterClient.Close();
        }
    }

    #endregion [Button Save]

    protected void FunProWriteXML(out StringBuilder strbXMLHnD)
    {
        //Read as Account details in XML
        strbXMLHnD = new StringBuilder();
        strbXMLHnD.Append("<Root>");
        int intHnDCount = 1;

        #region [Account Details] 1,2,3
        //foreach (GridViewRow gvr in gvAccount.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
        //    {
        //        string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strproductID = Convert.ToString(((Label)gvr.FindControl("lblProductID")).Text);
        //        string strModuleID = Convert.ToString(((Label)gvr.FindControl("lblModuleID")).Text);
        //        string strlblInvoice = Convert.ToString(((CheckBox)gvr.FindControl("CbxFInvoice")).Checked);
        //        string strAsset = Convert.ToString(((CheckBox)gvr.FindControl("CbxFAsset")).Checked);
        //        string strAssetIns = Convert.ToString(((CheckBox)gvr.FindControl("CbxFAssetIns")).Checked);

        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, strModuleID, strproductID, "1", strlblInvoice);
        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, strModuleID, strproductID, "2", strAsset);
        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, strModuleID, strproductID, "3", strAssetIns);
        //        intHnDCount++;
        //    }
        //}
        #endregion [Account Details]

        #region [Gap Days Details] 5
        //foreach (GridViewRow gvr in gvGapDays.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
        //    {
        //        string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strDays = Convert.ToString(((TextBox)gvr.FindControl("txtDays")).Text);
        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "5", strDays);
        //        intHnDCount++;
        //    }
        //}
        #endregion [Gap Days Details]

        #region [Income Details] 8,9,10,11
        foreach (GridViewRow gvr in gvIncome.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
            {
                string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
                //string strlblESM = Convert.ToString(((CheckBox)gvr.FindControl("CbxEsm")).Checked);
                string strlblIRR = Convert.ToString(((CheckBox)gvr.FindControl("CbxIRR")).Checked);
               // string strlblSOD = Convert.ToString(((CheckBox)gvr.FindControl("CbxSOD")).Checked);
               // string strlblProduct = Convert.ToString(((CheckBox)gvr.FindControl("CbxProduct")).Checked);

              //  strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "8", strlblESM);
                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "9", strlblIRR);
               // strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "10", strlblSOD);
               // strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "11", strlblProduct);
                intHnDCount++;
            }
        }
        #endregion [Income Details]

        #region [Income Type Details] 13,14
        foreach (GridViewRow gvr in gvIncomeType.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
            {
                string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
                string strlblIns = Convert.ToString(((CheckBox)gvr.FindControl("CbxIns")).Checked);
                string strlblAccounting = Convert.ToString(((CheckBox)gvr.FindControl("CbxAccounting")).Checked);

                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "13", strlblIns);
                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "14", strlblAccounting);
                intHnDCount++;
            }
        }
        #endregion [Income Type Details]

        #region [Revision Details] 18
        foreach (GridViewRow gvr in gvRevision.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
            {
                string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
                string strAlpha = Convert.ToString(((TextBox)gvr.FindControl("txtAlpha")).Text);

                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "18", strAlpha);
                intHnDCount++;
            }
        }
        #endregion [Revision Details]

        #region [Over due Details] 23,24,25
        //foreach (GridViewRow gvr in gvOverdue.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
        //    {
        //        string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strDeno = Convert.ToString(((TextBox)gvr.FindControl("txtDenoDays")).Text);
        //        string strGrance = Convert.ToString(((TextBox)gvr.FindControl("txtGraceDays")).Text);
        //        string strODI = Convert.ToString(((TextBox)gvr.FindControl("txtODIRate")).Text);

        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "23", strDeno);
        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "24", strGrance);
        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "25", strODI);
        //        intHnDCount++;
        //    }
        //}
        #endregion [Over due Details]

        #region [Account Pre Closure Type Details] 28,29,30,35
        foreach (GridViewRow gvr in gvAccountPreClosureType.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
            {
                string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
                string strIRR = Convert.ToString(((CheckBox)gvr.FindControl("chkIRR_Type")).Checked);
                string strNPV = Convert.ToString(((CheckBox)gvr.FindControl("chkNPV_Type")).Checked);
                string strPrinciple = Convert.ToString(((CheckBox)gvr.FindControl("chkPrinciple_Type")).Checked);
                string strPrincipleInterest = Convert.ToString(((TextBox)gvr.FindControl("txtPrincipleInterestRate")).Text);  // Added by Tamilselvan.S on 26/04/2011 for Principle Interest Rate 

                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "28", strIRR);
                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "29", strNPV);
                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "30", strPrinciple);
                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "35", strPrincipleInterest);  // Added by Tamilselvan.S on 26/04/2011 for Principle Interest Rate 

                intHnDCount++;
            }
        }
        #endregion [Account Pre Closure Type Details]

        #region [Gap Days Between PMC AND Approval Date Details] 31
        foreach (GridViewRow gvr in gvGapDaysBWPMCDate.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
            {
                string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
                string strGDays = Convert.ToString(((TextBox)gvr.FindControl("txtDays")).Text);

                strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "31", strGDays);
                intHnDCount++;
            }
        }
        #endregion [Gap Days Between PMC AND Approval Date Details]

        #region [Future Installment Details] 33
        //foreach (GridViewRow gvr in gvFutureInstallments.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
        //    {
        //        string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strInsallment = Convert.ToString(((TextBox)gvr.FindControl("txtInstallment")).Text);

        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "33", strInsallment);
        //        intHnDCount++;
        //    }
        //}
        #endregion [Future Installment Details]

        #region [Number of Split Details] 34
        string strLOB_ID = "3";
        //foreach (GridViewRow gvr in gvSplit.Rows)
        //{
        //    if (((CheckBox)gvr.FindControl("chkActive")).Checked == true)
        //    {
        //        strLOB_ID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strlobID = Convert.ToString(((Label)gvr.FindControl("lblLobID")).Text);
        //        string strSplit = Convert.ToString(((TextBox)gvr.FindControl("txtSplit")).Text);

        //        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strlobID, "7", "0", "34", strSplit);
        //        intHnDCount++;
        //    }
        //}
        #endregion [Number of Split Details]

        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "6", Convert.ToString(txtPaise.Text));  //6
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "15", Convert.ToString(txtCurrentRate.Text));  //15
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "16", Convert.ToString(ddlInterest.SelectedValue));  //16
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "19", Convert.ToString(txtGapRevision.Text));  //19
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "20", Convert.ToString(txtCollateral.Text));  //20
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "21", Convert.ToString(txtOperating.Text));  //21
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "26", Convert.ToString(ddlOD1.SelectedValue));  //26
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "27", Convert.ToString(txtFactory.Text));  //27
        strbXMLHnD = FunProWriteXMLCommonData(strbXMLHnD, intHnDCount, strLOB_ID, "7", "0", "32", Convert.ToString(txtWaivedOffAmount.Text));  //32
        strbXMLHnD.Append("</Root>");

    }

    protected StringBuilder FunProWriteXMLCommonData(StringBuilder strbDetails, int intID, string strLOBID, string strModuleId, string strProductID, string strParamCode, string strParamValue)
    {
        strbDetails.Append("<Details ");
        strbDetails.Append(" Header_ID = '" + intID.ToString() + "'");
        strbDetails.Append(" Company_ID = '" + Convert.ToString(intCompanyId) + "'");
        strbDetails.Append(" LOB_ID = '" + strLOBID.ToString() + "'");
        strbDetails.Append(" Product_ID = '" + strProductID.ToString() + "'");
        strbDetails.Append(" Module_ID='" + strModuleId.ToString() + "'");
        strbDetails.Append(" Param_Set_Desc = 'Test'");
        strbDetails.Append(" Param_Set_Date ='" + Convert.ToString(DateTime.Now) + "'");
        strbDetails.Append(" Txn_ID ='1'");
        strbDetails.Append(" CREATED_BY ='" + Convert.ToString(intUserId) + "'");
        strbDetails.Append(" CREATED_ON ='" + Convert.ToString(DateTime.Now) + "'");
        strbDetails.Append(" Parameter_Code ='" + strParamCode.ToString() + "'");
        strbDetails.Append(" Parameter_Value ='" + strParamValue.ToString() + "'");
        strbDetails.Append(" />");
        return strbDetails;
    }

    protected void FunProGetXml(out StringBuilder strXMLHeader, out StringBuilder strbuXMLDetails)
    {
        try
        {
            #region [Def]
            strXMLHeader = new StringBuilder();
            strbuXMLDetails = new StringBuilder();

            string strlobID;
            string strproductID;
            string strModuleID;
            string strCompanyID;
            string strDate;
            string strDesc;
            string strTxID;
            string strCreatedBy;
            string strCreatedOn;

            string strlblInvoice;   //1
            string strAsset;        //2
            string strAssetIns;     //3

            string strlblESM;       //5 
            string strlblIRR;       //6
            string strlblSOD;       //7
            string strlblProduct;   //8

            //if (ViewState["HEADER_TABLE"] == null)
            //{
            dtHeader = new DataTable();
            dtHeader.Columns.Add("ID", typeof(int));
            dtHeader.Columns.Add("LOB_ID", typeof(int));
            dtHeader.Columns.Add("Company_ID", typeof(int));
            dtHeader.Columns.Add("Product_ID", typeof(int));
            dtHeader.Columns.Add("Module_ID", typeof(int));
            dtHeader.Columns.Add("Param_Set_Desc", typeof(string));
            dtHeader.Columns.Add("Param_Set_Date", typeof(DateTime));
            dtHeader.Columns.Add("Created_By", typeof(int));
            dtHeader.Columns.Add("Created_On", typeof(DateTime));
            dtHeader.Columns.Add("Txn_ID", typeof(int));

            dtDetails = new DataTable();
            dtDetails.Columns.Add("Program_Set_ID", typeof(int));
            dtDetails.Columns.Add("Company_ID", typeof(int));
            dtDetails.Columns.Add("Parameter_Code", typeof(int));    // CheckBox(1,2,3), textbox 4, checkbox(5,6,7,8)            
            dtDetails.Columns.Add("Parameter_Value", typeof(string));
            ViewState["DETAILS_TABLE"] = dtDetails;
            //}
            int intRowNumber = 0;
            DataRow drHeaderRow = null;
            DataRow drDetailsRow = null;
            #endregion [Def]

            #region [Account Details]
            //Account
            //foreach (GridViewRow grvData in gvAccount.Rows)
            //{

            //    strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
            //    strproductID = ((Label)grvData.FindControl("lblProductID")).Text;
            //    strModuleID = ((Label)grvData.FindControl("lblModuleID")).Text;
            //    bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
            //    strCompanyID = Convert.ToString(intCompanyId);
            //    strDate = Convert.ToString(DateTime.Now).ToString();
            //    strDesc = "Test";
            //    strTxID = Convert.ToString("1");
            //    strCreatedBy = Convert.ToString(intUserId);
            //    strCreatedOn = Convert.ToString(DateTime.Now).ToString();
            //    if (dtHeader.Rows.Count == 0)
            //    {
            //        intRowNumber = 1;
            //    }
            //    else
            //    {
            //        intRowNumber = dtHeader.Rows.Count + 1;
            //    }
            //    drHeaderRow = null;
            //    drHeaderRow = dtHeader.NewRow();
            //    drHeaderRow["Company_ID"] = strCompanyID.ToString();

            //    drHeaderRow["LOB_ID"] = Convert.ToInt32(strlobID.ToString());
            //    if (strproductID == "0")
            //    {
            //        drHeaderRow["PRODUCT_ID"] = 0;
            //    }
            //    else
            //    {
            //        drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
            //    }
            //    drHeaderRow["Module_ID"] = strModuleID.ToString();
            //    drHeaderRow["ID"] = intRowNumber;
            //    drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
            //    drHeaderRow["Param_Set_Date"] = strDate.ToString();
            //    drHeaderRow["Created_By"] = strCreatedBy.ToString();
            //    drHeaderRow["Created_On"] = strDate.ToString();
            //    drHeaderRow["Txn_ID"] = strTxID.ToString();
            //    dtHeader.Rows.Add(drHeaderRow);

            //    strlblInvoice = Convert.ToString(((CheckBox)grvData.FindControl("CbxFInvoice")).Checked);
            //    strAsset = Convert.ToString(((CheckBox)grvData.FindControl("CbxFAsset")).Checked);
            //    strAssetIns = Convert.ToString(((CheckBox)grvData.FindControl("CbxFAssetIns")).Checked);

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;  //Convert.ToInt32(strParam_set_ID);// intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "1";
            //    drDetailsRow["Parameter_Value"] = strlblInvoice.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;  // Convert.ToInt32(strParam_set_ID);// intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "2";
            //    drDetailsRow["Parameter_Value"] = strAsset.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;  // Convert.ToInt32(strParam_set_ID);// intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "3";
            //    drDetailsRow["Parameter_Value"] = strAssetIns.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);
            //}
            #endregion [Account Details]

            #region [Gap Days]
            //foreach (GridViewRow grvData in gvGapDays.Rows)
            //{
            //    strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
            //    strproductID = "1";
            //    strModuleID = Convert.ToString("7");
            //    strCompanyID = Convert.ToString(intCompanyId);
            //    strDate = Convert.ToString(DateTime.Now).ToString();
            //    strDesc = "Test";
            //    strTxID = Convert.ToString("1");
            //    strCreatedBy = Convert.ToString(intUserId);
            //    strCreatedOn = Convert.ToString(DateTime.Now).ToString();
            //    bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;

            //    strproductID = "1";
            //    if (dtHeader.Rows.Count == 0)
            //    {
            //        intRowNumber = 1;
            //    }
            //    else
            //    {
            //        intRowNumber = dtHeader.Rows.Count + 1;
            //    }

            //    drHeaderRow = null;
            //    drHeaderRow = dtHeader.NewRow();
            //    drHeaderRow["ID"] = intRowNumber;
            //    drHeaderRow["Company_ID"] = strCompanyID.ToString();
            //    drHeaderRow["LOB_ID"] = strlobID.ToString();
            //    drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
            //    drHeaderRow["Module_ID"] = strModuleID.ToString();
            //    drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
            //    drHeaderRow["Param_Set_Date"] = strDate.ToString();
            //    drHeaderRow["Created_By"] = strCreatedBy.ToString();
            //    drHeaderRow["Created_On"] = strDate.ToString();
            //    drHeaderRow["Txn_ID"] = strTxID.ToString();
            //    dtHeader.Rows.Add(drHeaderRow);

            //    string strValues = ((TextBox)grvData.FindControl("txtDays")).Text;

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "5";
            //    drDetailsRow["Parameter_Value"] = strValues.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);
            //}
            #endregion [Gap Days]

            #region [Income Details]
            //Income
            foreach (GridViewRow grvData in gvIncome.Rows)
            {
                strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
                strproductID = "1";
                strModuleID = Convert.ToString("7");
                strCompanyID = Convert.ToString(intCompanyId);
                strDate = Convert.ToString(DateTime.Now).ToString();
                strDesc = "Test";
                strTxID = Convert.ToString("1");
                strCreatedBy = Convert.ToString(intUserId);
                strCreatedOn = Convert.ToString(DateTime.Now).ToString();
                bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
                if (dtHeader.Rows.Count == 0)
                {
                    intRowNumber = 1;
                }
                else
                {
                    intRowNumber = dtHeader.Rows.Count + 1;
                }

                drHeaderRow = null;
                drHeaderRow = dtHeader.NewRow();
                drHeaderRow["ID"] = intRowNumber;
                drHeaderRow["Company_ID"] = strCompanyID.ToString();
                drHeaderRow["LOB_ID"] = strlobID.ToString();
                drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
                drHeaderRow["Module_ID"] = strModuleID.ToString();
                drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
                drHeaderRow["Param_Set_Date"] = strDate.ToString();
                drHeaderRow["Created_By"] = strCreatedBy.ToString();
                drHeaderRow["Created_On"] = strDate.ToString();
                drHeaderRow["Txn_ID"] = strTxID.ToString();
                dtHeader.Rows.Add(drHeaderRow);

                //strlblESM = Convert.ToString(((CheckBox)grvData.FindControl("CbxEsm")).Checked);
                strlblIRR = Convert.ToString(((CheckBox)grvData.FindControl("CbxIRR")).Checked);
                //strlblSOD = Convert.ToString(((CheckBox)grvData.FindControl("CbxSOD")).Checked);
                //strlblProduct = Convert.ToString(((CheckBox)grvData.FindControl("CbxProduct")).Checked);

                //drDetailsRow = null;
                //drDetailsRow = dtDetails.NewRow();
                //drDetailsRow["Program_Set_ID"] = intRowNumber;
                //drDetailsRow["Company_ID"] = strCompanyID.ToString();
                //drDetailsRow["Parameter_Code"] = "8";
                //drDetailsRow["Parameter_Value"] = strlblESM.ToString();
                //if (bolActive == true)
                //    dtDetails.Rows.Add(drDetailsRow);

                drDetailsRow = null;
                drDetailsRow = dtDetails.NewRow();
                drDetailsRow["Program_Set_ID"] = intRowNumber;
                drDetailsRow["Company_ID"] = strCompanyID.ToString();
                drDetailsRow["Parameter_Code"] = "9";
                drDetailsRow["Parameter_Value"] = strlblIRR.ToString();
                if (bolActive == true)
                    dtDetails.Rows.Add(drDetailsRow);

                //drDetailsRow = null;
                //drDetailsRow = dtDetails.NewRow();
                //drDetailsRow["Program_Set_ID"] = intRowNumber;
                //drDetailsRow["Company_ID"] = strCompanyID.ToString();
                //drDetailsRow["Parameter_Code"] = "10";
                //drDetailsRow["Parameter_Value"] = strlblSOD.ToString();
                //if (bolActive == true)
                //    dtDetails.Rows.Add(drDetailsRow);

                //drDetailsRow = null;
                //drDetailsRow = dtDetails.NewRow();
                //drDetailsRow["Program_Set_ID"] = intRowNumber;
                //drDetailsRow["Company_ID"] = strCompanyID.ToString();
                //drDetailsRow["Parameter_Code"] = "11";
                //drDetailsRow["Parameter_Value"] = strlblProduct.ToString();
                //if (bolActive == true)
                //    dtDetails.Rows.Add(drDetailsRow);
            }
            #endregion [Income Details]

            #region [Income Type Details]
            //Income Type
            foreach (GridViewRow grvData in gvIncomeType.Rows)
            {
                strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
                strproductID = "1";
                strModuleID = Convert.ToString("7");
                strCompanyID = Convert.ToString(intCompanyId);
                strDate = Convert.ToString(DateTime.Now).ToString();
                strDesc = "Test";
                strTxID = Convert.ToString("1");
                strCreatedBy = Convert.ToString(intUserId);
                strCreatedOn = Convert.ToString(DateTime.Now).ToString();
                bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
                if (dtHeader.Rows.Count == 0)
                {
                    intRowNumber = 1;
                }
                else
                {
                    intRowNumber = dtHeader.Rows.Count + 1;
                }

                drHeaderRow = null;
                drHeaderRow = dtHeader.NewRow();
                drHeaderRow["ID"] = intRowNumber;
                drHeaderRow["Company_ID"] = strCompanyID.ToString();
                drHeaderRow["LOB_ID"] = strlobID.ToString();
                drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
                drHeaderRow["Module_ID"] = strModuleID.ToString();
                drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
                drHeaderRow["Param_Set_Date"] = strDate.ToString();
                drHeaderRow["Created_By"] = strCreatedBy.ToString();
                drHeaderRow["Created_On"] = strDate.ToString();
                drHeaderRow["Txn_ID"] = strTxID.ToString();
                dtHeader.Rows.Add(drHeaderRow);

                string strlblIns = Convert.ToString(((CheckBox)grvData.FindControl("CbxIns")).Checked);
                string strlblAccounting = Convert.ToString(((CheckBox)grvData.FindControl("CbxAccounting")).Checked);

                drDetailsRow = null;
                drDetailsRow = dtDetails.NewRow();
                drDetailsRow["Program_Set_ID"] = intRowNumber;
                drDetailsRow["Company_ID"] = strCompanyID.ToString();
                drDetailsRow["Parameter_Code"] = "13";
                drDetailsRow["Parameter_Value"] = strlblIns.ToString();
                if (bolActive == true)
                    dtDetails.Rows.Add(drDetailsRow);

                drDetailsRow = null;
                drDetailsRow = dtDetails.NewRow();
                drDetailsRow["Program_Set_ID"] = intRowNumber;
                drDetailsRow["Company_ID"] = strCompanyID.ToString();
                drDetailsRow["Parameter_Code"] = "14";
                drDetailsRow["Parameter_Value"] = strlblAccounting.ToString();
                if (bolActive == true)
                    dtDetails.Rows.Add(drDetailsRow);
            }
            #endregion [Income Type Details]

            #region [Revision Details]
            foreach (GridViewRow grvData in gvRevision.Rows)
            {
                strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
                strproductID = "1";
                strModuleID = Convert.ToString("7");
                strCompanyID = Convert.ToString(intCompanyId);
                strDate = Convert.ToString(DateTime.Now).ToString();
                strDesc = "Test";
                strTxID = Convert.ToString("1");
                strCreatedBy = Convert.ToString(intUserId);
                strCreatedOn = Convert.ToString(DateTime.Now).ToString();
                bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
                if (dtHeader.Rows.Count == 0)
                {
                    intRowNumber = 1;
                }
                else
                {
                    intRowNumber = dtHeader.Rows.Count + 1;
                }

                drHeaderRow = null;
                drHeaderRow = dtHeader.NewRow();
                drHeaderRow["ID"] = intRowNumber;
                drHeaderRow["Company_ID"] = strCompanyID.ToString();
                drHeaderRow["LOB_ID"] = strlobID.ToString();
                drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
                drHeaderRow["Module_ID"] = strModuleID.ToString();
                drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
                drHeaderRow["Param_Set_Date"] = strDate.ToString();
                drHeaderRow["Created_By"] = strCreatedBy.ToString();
                drHeaderRow["Created_On"] = strDate.ToString();
                drHeaderRow["Txn_ID"] = strTxID.ToString();
                dtHeader.Rows.Add(drHeaderRow);

                string strAlpha = ((TextBox)grvData.FindControl("txtAlpha")).Text;
                drDetailsRow = null;
                drDetailsRow = dtDetails.NewRow();
                drDetailsRow["Program_Set_ID"] = intRowNumber;
                drDetailsRow["Company_ID"] = strCompanyID.ToString();
                drDetailsRow["Parameter_Code"] = "18";
                drDetailsRow["Parameter_Value"] = strAlpha.ToString();
                if (bolActive == true)
                    dtDetails.Rows.Add(drDetailsRow);
            }
            #endregion [Revision Details]

            #region [Over due Details]
            //foreach (GridViewRow grvData in gvOverdue.Rows)
            //{
            //    strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
            //    strproductID = "1";
            //    strModuleID = Convert.ToString("7");
            //    strCompanyID = Convert.ToString(intCompanyId);
            //    strDate = Convert.ToString(DateTime.Now).ToString();
            //    strDesc = "Test";
            //    strTxID = Convert.ToString("1");
            //    strCreatedBy = Convert.ToString(intUserId);
            //    strCreatedOn = Convert.ToString(DateTime.Now).ToString();
            //    bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
            //    if (dtHeader.Rows.Count == 0)
            //    {
            //        intRowNumber = 1;
            //    }
            //    else
            //    {
            //        intRowNumber = dtHeader.Rows.Count + 1;
            //    }

            //    drHeaderRow = null;
            //    drHeaderRow = dtHeader.NewRow();
            //    drHeaderRow["ID"] = intRowNumber;
            //    drHeaderRow["Company_ID"] = strCompanyID.ToString();
            //    drHeaderRow["LOB_ID"] = strlobID.ToString();
            //    drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
            //    drHeaderRow["Module_ID"] = strModuleID.ToString();
            //    drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
            //    drHeaderRow["Param_Set_Date"] = strDate.ToString();
            //    drHeaderRow["Created_By"] = strCreatedBy.ToString();
            //    drHeaderRow["Created_On"] = strDate.ToString();
            //    drHeaderRow["Txn_ID"] = strTxID.ToString();
            //    dtHeader.Rows.Add(drHeaderRow);

            //    string strDeno = ((TextBox)grvData.FindControl("txtDenoDays")).Text;
            //    string strGrance = ((TextBox)grvData.FindControl("txtGraceDays")).Text;
            //    string strODI = ((TextBox)grvData.FindControl("txtODIRate")).Text;

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "23";
            //    drDetailsRow["Parameter_Value"] = strDeno.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "24";
            //    drDetailsRow["Parameter_Value"] = strGrance.ToString();  //Modified by Tamilselvan.S on 11/02/2011
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);

            //    drDetailsRow = null;
            //    drDetailsRow = dtDetails.NewRow();
            //    drDetailsRow["Program_Set_ID"] = intRowNumber;
            //    drDetailsRow["Company_ID"] = strCompanyID.ToString();
            //    drDetailsRow["Parameter_Code"] = "25";
            //    drDetailsRow["Parameter_Value"] = strODI.ToString();
            //    if (bolActive == true)
            //        dtDetails.Rows.Add(drDetailsRow);
            //}
            #endregion [Over due Details]

            #region [Gap Days Between PMC AND Approval Date Details]
            // Gap Days Between PMC AND Approval Date
            foreach (GridViewRow grvData in gvGapDaysBWPMCDate.Rows)
            {
                strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
                strproductID = "0";
                strModuleID = Convert.ToString("7");
                strCompanyID = Convert.ToString(intCompanyId);
                strDate = Convert.ToString(DateTime.Now).ToString();
                strDesc = "Test";
                strTxID = Convert.ToString("1");
                strCreatedBy = Convert.ToString(intUserId);
                strCreatedOn = Convert.ToString(DateTime.Now).ToString();
                bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
                intRowNumber = dtHeader.Rows.Count + 1;

                drHeaderRow = null;
                drHeaderRow = dtHeader.NewRow();
                drHeaderRow["ID"] = intRowNumber;
                drHeaderRow["Company_ID"] = strCompanyID.ToString();
                drHeaderRow["LOB_ID"] = strlobID.ToString();
                drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
                drHeaderRow["Module_ID"] = strModuleID.ToString();
                drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
                drHeaderRow["Param_Set_Date"] = strDate.ToString();
                drHeaderRow["Created_By"] = strCreatedBy.ToString();
                drHeaderRow["Created_On"] = strDate.ToString();
                drHeaderRow["Txn_ID"] = strTxID.ToString();
                dtHeader.Rows.Add(drHeaderRow);

                string strDays = ((TextBox)grvData.FindControl("txtDays")).Text;

                drDetailsRow = null;
                drDetailsRow = dtDetails.NewRow();
                drDetailsRow["Program_Set_ID"] = intRowNumber;
                drDetailsRow["Company_ID"] = strCompanyID.ToString();
                drDetailsRow["Parameter_Code"] = "31";
                drDetailsRow["Parameter_Value"] = strDays.ToString();
                if (bolActive == true)
                    dtDetails.Rows.Add(drDetailsRow);
            }
            #endregion [Gap Days Between PMC AND Approval Date Details]

            #region [Account Pre Closure Type Details]
            //Account Pre Closure Type
            foreach (GridViewRow grvData in gvAccountPreClosureType.Rows)
            {
                strlobID = ((Label)grvData.FindControl("lblLobID")).Text;
                bool bolActive = ((CheckBox)grvData.FindControl("chkActive")).Checked;
                intRowNumber = dtHeader.Rows.Count + 1;

                if (bolActive == true)
                {
                    drHeaderRow = null;
                    drHeaderRow = dtHeader.NewRow();
                    drHeaderRow["ID"] = intRowNumber;
                    drHeaderRow["Company_ID"] = Convert.ToString(intCompanyId);
                    drHeaderRow["LOB_ID"] = strlobID.ToString();
                    drHeaderRow["PRODUCT_ID"] = "0";
                    drHeaderRow["Module_ID"] = "7";
                    drHeaderRow["Param_Set_Desc"] = "Test";
                    drHeaderRow["Param_Set_Date"] = Convert.ToString(DateTime.Now).ToString();
                    drHeaderRow["Created_By"] = Convert.ToString(intUserId);
                    drHeaderRow["Created_On"] = Convert.ToString(DateTime.Now).ToString();
                    drHeaderRow["Txn_ID"] = "1";
                    dtHeader.Rows.Add(drHeaderRow);

                    bool bolIRR = ((CheckBox)grvData.FindControl("chkIRR_Type")).Checked;
                    bool bolNPV = ((CheckBox)grvData.FindControl("chkNPV_Type")).Checked;
                    bool bolPrinciple = ((CheckBox)grvData.FindControl("chkPrinciple_Type")).Checked;

                    drDetailsRow = dtDetails.NewRow();
                    drDetailsRow["Program_Set_ID"] = intRowNumber;
                    drDetailsRow["Company_ID"] = Convert.ToString(intCompanyId);
                    drDetailsRow["Parameter_Code"] = "28";
                    drDetailsRow["Parameter_Value"] = bolIRR.ToString();
                    dtDetails.Rows.Add(drDetailsRow);

                    drDetailsRow = dtDetails.NewRow();
                    drDetailsRow["Program_Set_ID"] = intRowNumber;
                    drDetailsRow["Company_ID"] = Convert.ToString(intCompanyId);
                    drDetailsRow["Parameter_Code"] = "29";
                    drDetailsRow["Parameter_Value"] = bolNPV.ToString();
                    dtDetails.Rows.Add(drDetailsRow);

                    drDetailsRow = dtDetails.NewRow();
                    drDetailsRow["Program_Set_ID"] = intRowNumber;
                    drDetailsRow["Company_ID"] = Convert.ToString(intCompanyId);
                    drDetailsRow["Parameter_Code"] = "30";
                    drDetailsRow["Parameter_Value"] = bolPrinciple.ToString();
                    dtDetails.Rows.Add(drDetailsRow);
                }
            }
            #endregion [Account Pre Closure Type Details]


            #region  ""
            strlobID = "1";
            strproductID = "1";
            strModuleID = Convert.ToString("7");
            strCompanyID = Convert.ToString(intCompanyId);
            strDate = Convert.ToString(DateTime.Now).ToString();
            strDesc = "Test";
            strTxID = Convert.ToString("1");
            strCreatedBy = Convert.ToString(intUserId);
            strCreatedOn = Convert.ToString(DateTime.Now).ToString();

            if (dtHeader.Rows.Count == 0)
            {
                intRowNumber = 1;
            }
            else
            {
                intRowNumber = dtHeader.Rows.Count + 1;
            }
            drHeaderRow = null;
            drHeaderRow = dtHeader.NewRow();
            drHeaderRow["ID"] = intRowNumber;
            drHeaderRow["Company_ID"] = strCompanyID.ToString();
            drHeaderRow["LOB_ID"] = strlobID.ToString();
            drHeaderRow["PRODUCT_ID"] = strproductID.ToString();
            drHeaderRow["Module_ID"] = strModuleID.ToString();
            drHeaderRow["Param_Set_Desc"] = strDesc.ToString();
            drHeaderRow["Param_Set_Date"] = strDate.ToString();
            drHeaderRow["Created_By"] = strCreatedBy.ToString();
            drHeaderRow["Created_On"] = strDate.ToString();
            drHeaderRow["Txn_ID"] = strTxID.ToString();
            dtHeader.Rows.Add(drHeaderRow);
            #endregion ""

            string strPaise = txtPaise.Text;
            string strCurrentLendRate = txtCurrentRate.Text;
            string strInverting = ddlInterest.SelectedValue;
            string strGapValue = txtGapRevision.Text;
            string strCollatral = txtCollateral.Text;
            string strBaseValue = txtOperating.Text;
            string strODIValue = ddlOD1.SelectedValue;
            string strFactory = txtFactory.Text;

            #region [6]
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "6";
            drDetailsRow["Parameter_Value"] = strPaise.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion [6]

            #region 15
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "15";
            drDetailsRow["Parameter_Value"] = strCurrentLendRate.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 15

            #region 16
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "16";
            drDetailsRow["Parameter_Value"] = strInverting.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 16

            #region 19
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "19";
            drDetailsRow["Parameter_Value"] = strGapValue.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 19

            #region 20
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "20";
            drDetailsRow["Parameter_Value"] = strCollatral.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 20

            #region 21
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "21";
            drDetailsRow["Parameter_Value"] = strBaseValue.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 21

            #region 26
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "26";
            drDetailsRow["Parameter_Value"] = strODIValue.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 26
            #region 27
            drDetailsRow = null;
            drDetailsRow = dtDetails.NewRow();
            drDetailsRow["Program_Set_ID"] = intRowNumber;
            drDetailsRow["Company_ID"] = strCompanyID.ToString();
            drDetailsRow["Parameter_Code"] = "27";
            drDetailsRow["Parameter_Value"] = strFactory.ToString();
            dtDetails.Rows.Add(drDetailsRow);
            #endregion 27

            ViewState["HEADER_TABLE"] = dtHeader;
            ViewState["DETAILS_TABLE"] = dtDetails;

            dtHeader = (DataTable)ViewState["HEADER_TABLE"];
            dtDetails = (DataTable)ViewState["DETAILS_TABLE"];

            strXMLHeader.Append("<Root>");
            foreach (DataRow drRow in dtHeader.Rows)
            {
                string strHeader_ID = drRow["ID"].ToString();
                strlobID = drRow["LOB_ID"].ToString();
                strCompanyID = drRow["Company_ID"].ToString();
                string strproductIDs = drRow["Product_ID"].ToString();
                strModuleID = drRow["Module_ID"].ToString();
                strDesc = drRow["Param_Set_Desc"].ToString();
                strDate = drRow["Param_Set_Date"].ToString();
                strCreatedBy = drRow["Created_By"].ToString();
                strDate = drRow["Created_On"].ToString();
                strproductID = drRow["Txn_ID"].ToString();

                strXMLHeader.Append("<Details ");
                strXMLHeader.Append(" Header_ID = '" + strHeader_ID.ToString() + "'");
                strXMLHeader.Append(" Company_ID = '" + strCompanyID.ToString() + "'");
                strXMLHeader.Append(" LOB_ID = '" + strlobID.ToString() + "'");
                strXMLHeader.Append(" Product_ID = '" + strproductIDs.ToString() + "'");
                strXMLHeader.Append(" Module_ID='" + strModuleID.ToString() + "'");
                strXMLHeader.Append(" Param_Set_Desc = '" + strDesc.ToString() + "'");
                strXMLHeader.Append(" Param_Set_Date ='" + strDate.ToString() + "'");
                strXMLHeader.Append(" Txn_ID ='" + strTxID.ToString() + "'");
                strXMLHeader.Append(" CREATED_BY ='" + strCreatedBy.ToString() + "'");
                strXMLHeader.Append(" CREATED_ON ='" + strCreatedOn.ToString() + "'");
                strXMLHeader.Append(" />");
            }
            strXMLHeader.Append("</Root>");
            strbuXMLDetails.Append("<Root>");
            foreach (DataRow drRow in dtDetails.Rows)
            {
                string strProgram_Set_ID = drRow["Program_Set_ID"].ToString();
                string strCompany_ID = drRow["Company_ID"].ToString();
                string strProgram_Code = drRow["Parameter_Code"].ToString();
                string strParameter_Value = drRow["Parameter_Value"].ToString();

                strbuXMLDetails.Append(" <Details  ");
                strbuXMLDetails.Append(" Header_ID = '" + strProgram_Set_ID.ToString() + "'");
                strbuXMLDetails.Append(" Company_ID ='" + strCompanyID.ToString() + "'");
                strbuXMLDetails.Append(" Parameter_Code='" + strProgram_Code.ToString() + "'");
                strbuXMLDetails.Append(" Parameter_Value = '" + strParameter_Value.ToString() + "'/>");
            }
            strbuXMLDetails.Append("</Root>");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region Role Access Setup

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode                
                rfvCollateral.Visible = true;
                //rfvBase.Visible = true;
                rfvInterest.Visible = true;
                rfvGap.Visible = true;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                FunPriGetLookUpList();
                // SetInitialRow();
                FunPriGetGPSDetails();
                FunPriGetGeneralLookUpList();
                FunProLoadAllGPSGrid();
                break;

            case 1: // Modify Mode
               // tpCollateral.Visible = true;
                tpBaseValue.Visible = true;
                rfvCollateral.Visible = true;
                //rfvBase.Visible = true;
                tbInvert.Visible = true;
                rfvInterest.Visible = true;
                tbRevision.Visible = true;
                //tpRevision.Visible = true;
                rfvGap.Visible = true;
                FunPriGetGPSDetails();
                btnClear.Enabled = false;
                FunProLoadAllGPSGrid();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                UserInfo ObjUserInfo = new UserInfo();
                if (!bModify || ObjUserInfo.ProUserLevelIdRW != 5)
                {
                    btnSave.Enabled = false;
                }
                break;

            case -1:// Query Mode
                //tpCollateral.Visible = true;
                tpBaseValue.Visible = true;
                rfvCollateral.Visible = true;
                //rfvBase.Visible = true;
                tbInvert.Visible = true;
                rfvInterest.Visible = true;
                tbRevision.Visible = true;
                //tpRevision.Visible = true;
                rfvGap.Visible = true;
                FunPriGetGPSDetails();

                FunProLoadAllGPSGrid();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                if (bClearList)
                {
                    ddlInterest.ClearDropDownList();
                    ddlOD1.ClearDropDownList();
                }
                FunProDisableControlsQuery();
                break;
        }
    }

    #endregion

    protected void FunProDisableControlsQuery()
    {
        txtPaise.Enabled = txtCurrentRate.Enabled = txtGapRevision.Enabled = txtCollateral.Enabled = txtOperating.Enabled = false;
        txtFactory.Enabled = btnSave.Enabled = btnClear.Enabled = txtWaivedOffAmount.Enabled = false;
        //foreach (GridViewRow grv in gvAccount.Rows)
        //{
        //    ((CheckBox)grv.FindControl("CbxFInvoice")).Enabled = false;
        //    ((CheckBox)grv.FindControl("CbxFAsset")).Enabled = false;
        //    ((CheckBox)grv.FindControl("CbxFAssetIns")).Enabled = false;
        //    ((LinkButton)grv.FindControl("btnRemove")).Enabled = false;
        //    ((TextBox)grv.FindControl("txtLOB")).Enabled = false;
        //    ((TextBox)grv.FindControl("txtProduct")).Enabled = false;
        //}
        //foreach (GridViewRow grv in gvGapDays.Rows)
        //{
        //    ((TextBox)grv.FindControl("txtDays")).Enabled = false;
        //}
        foreach (GridViewRow grv in gvIncome.Rows)
        {
            //((CheckBox)grv.FindControl("CbxEsm")).Enabled = false;
            ((CheckBox)grv.FindControl("CbxIRR")).Enabled = false;
            //((CheckBox)grv.FindControl("CbxSOD")).Enabled = false;
           // ((CheckBox)grv.FindControl("CbxProduct")).Enabled = false;
        }
        foreach (GridViewRow grv in gvIncomeType.Rows)
        {
            ((CheckBox)grv.FindControl("CbxIns")).Enabled = false;
            ((CheckBox)grv.FindControl("CbxAccounting")).Enabled = false;
        }
        foreach (GridViewRow grv in gvRevision.Rows)
        {
            ((TextBox)grv.FindControl("txtAlpha")).Enabled = false;
        }
        //foreach (GridViewRow grv in gvOverdue.Rows)
        //{
        //    ((TextBox)grv.FindControl("txtDenoDays")).Enabled = false;
        //    ((TextBox)grv.FindControl("txtGraceDays")).Enabled = false;
        //    ((TextBox)grv.FindControl("txtODIRate")).Enabled = false;
        //}
        foreach (GridViewRow grv in gvGapDaysBWPMCDate.Rows)
        {
            ((TextBox)grv.FindControl("txtDays")).Enabled = false;
        }
        foreach (GridViewRow grv in gvAccountPreClosureType.Rows)
        {
            ((CheckBox)grv.FindControl("chkIRR_Type")).Enabled = false;
            ((CheckBox)grv.FindControl("chkNPV_Type")).Enabled = false;
            ((CheckBox)grv.FindControl("chkPrinciple_Type")).Enabled = false;
            ((TextBox)grv.FindControl("txtPrincipleInterestRate")).Enabled = false;  // Added by Tamilselvan.S on 26/04/2011 for Principle Interest Rate 
        }
        //foreach (GridViewRow grv in gvFutureInstallments.Rows)
        //{
        //    ((TextBox)grv.FindControl("txtInstallment")).Enabled = false;
        //}
    //    foreach (GridViewRow grv in gvSplit.Rows)
    //    {
    //        ((TextBox)grv.FindControl("txtSplit")).Enabled = false;
    //    }
    }

    protected void FunProLoadAllGPSGrid()
    {
        FunPriGetGPSGapDetails();
        FunPriGetGPSExtraDetails();
        FunPriGetGPSIncomdDetails();
        FunPriGetGPSIncomdRegDetails();
        FunPriGetGPSIncomdRevisionDetails();
        FunPriGetGPSOverdueDetails();
        FunPriGetGPSPreClosureTypeDetails();
        FunPriGetGPSGapDaysBWPMCandApprovalDate();
        FunPriGetGPSFutureInstallmentDetails();
        FunPriGetGPSSplitDetails();
    }

    private void FunPriGetGPSDetails()
    {
        objLoanAdmin_MasterClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupRow ObjGlobalMasterLOBRow;
            ObjGlobalMasterLOBRow = ObjS3G_GlobalParameterMaster.NewS3G_LOANAD_GetGlobalParameterSetupRow();
            ObjGlobalMasterLOBRow.Company_ID = intCompanyId;
            ObjGlobalMasterLOBRow.User_ID = intUserId;
            if (intGlobalID == 0)
            {
                ObjGlobalMasterLOBRow.Is_Active = false;
            }
            else
            {
                ObjGlobalMasterLOBRow.Is_Active = true;
            }
            ObjS3G_GlobalParameterMaster.AddS3G_LOANAD_GetGlobalParameterSetupRow(ObjGlobalMasterLOBRow);

            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteGPSDetails = objLoanAdmin_MasterClient.FunPubGetGPSDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterMaster, SerMode));
            ObjS3G_GlobalParameterMaster = (LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable)ClsPubSerialize.DeSerialize(byteGPSDetails, SerializationMode.Binary, typeof(LoanAdminMgtServices.S3G_LOANAD_GetGlobalParameterSetupDataTable));

            DataTable dtList = new DataTable();
            dtList = ObjS3G_GlobalParameterMaster.Clone();
            dtList.Columns["Parameter_Value"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value1"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value2"].DataType = typeof(System.Boolean);
            dtList.Columns["IsActive"].DataType = typeof(System.Boolean);

            foreach (DataRow dr in ObjS3G_GlobalParameterMaster.Rows)
            {
                DataRow drow = dtList.NewRow();
                drow["Param_Set_ID"] = dr["Param_Set_ID"];
                drow["Company_ID"] = dr["Company_ID"];
                drow["LOB_ID"] = dr["LOB_ID"];
                drow["LOB"] = dr["LOB"];
                drow["Product_ID"] = dr["Product_ID"];
                drow["IsActive"] = Convert.ToBoolean(dr["IsActive"]);
                drow["Product_Code"] = Convert.ToString(dr["Product_Code"]);
                drow["Module_ID"] = dr["Module_ID"];
                drow["Parameter_Value"] = Convert.ToBoolean(dr["Parameter_Value"]);
                drow["Parameter_Value1"] = Convert.ToBoolean(dr["Parameter_Value1"]);
                drow["Parameter_Value2"] = Convert.ToBoolean(dr["Parameter_Value2"]);
                dtList.Rows.Add(drow);
            }
            //if (dtList.Rows.Count == 0)
            //{
            //    SetInitialRow();
            //}
            //else
            //{
            ViewState["currenttable"] = dtList;
            //gvAccount.DataSource = dtList;
            //gvAccount.DataBind();
            // }
        }
        catch (FaultException<LoanAdminMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp, strPageName);
            throw;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
        finally
        {
            if (objLoanAdmin_MasterClient != null)
                objLoanAdmin_MasterClient.Close();
        }
    }

    private void FunPriGetGPSGapDetails()
    {
        //try
        //{
        //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //    dictParam.Add("@Company_ID", intCompanyId.ToString());
        //    dictParam.Add("@User_ID", intUserId.ToString());
        //    dictParam.Add("@Is_Active", "1");
        //    DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterGAP", dictParam);
        //    gvGapDays.DataSource = dtHesderDetails;
        //    gvGapDays.DataBind();
        //}
        //catch (Exception ex)
        //{
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        //    throw;
        //}
    }

    private void FunPriGetGPSExtraDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtExtraDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterExtra", dictParam);
            if (dtExtraDetails.Rows.Count > 0)
            {
                if (dtExtraDetails.Rows[0]["Parameter_Code"].ToString() == "6")
                {
                    txtPaise.Text = dtExtraDetails.Rows[0]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[1]["Parameter_Code"].ToString() == "15")
                {
                    txtCurrentRate.Text = dtExtraDetails.Rows[1]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[2]["Parameter_Code"].ToString() == "16")
                {
                    ddlInterest.SelectedValue = dtExtraDetails.Rows[2]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[3]["Parameter_Code"].ToString() == "19")
                {
                    txtGapRevision.Text = dtExtraDetails.Rows[3]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[4]["Parameter_Code"].ToString() == "20")
                {
                    txtCollateral.Text = dtExtraDetails.Rows[4]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[5]["Parameter_Code"].ToString() == "21")
                {
                    txtOperating.Text = dtExtraDetails.Rows[5]["Parameter_Value"].ToString();
                }

                if (dtExtraDetails.Rows[6]["Parameter_Code"].ToString() == "26")
                {
                    FunPriGetGeneralLookUpList();
                    ddlOD1.SelectedValue = dtExtraDetails.Rows[6]["Parameter_Value"].ToString();
                }

                if (dtExtraDetails.Rows[7]["Parameter_Code"].ToString() == "27")
                {
                    txtFactory.Text = dtExtraDetails.Rows[7]["Parameter_Value"].ToString();
                }
                if (dtExtraDetails.Rows[8]["Parameter_Code"].ToString() == "32")
                {
                    txtWaivedOffAmount.Text = dtExtraDetails.Rows[8]["Parameter_Value"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSIncomdDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterIncome", dictParam);
            DataTable dtList = new DataTable();
            dtList = dtHesderDetails.Clone();
            dtList.Columns["Parameter_Value"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value1"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value2"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value3"].DataType = typeof(System.Boolean);
            dtList.Columns["Is_Active"].DataType = typeof(System.Boolean);
            foreach (DataRow dr in dtHesderDetails.Rows)
            {
                DataRow drow = dtList.NewRow();
                drow["Param_Set_ID"] = dr["Param_Set_ID"];
                drow["Company_ID"] = dr["Company_ID"];
                drow["LOB_ID"] = dr["LOB_ID"];
                drow["LOB"] = dr["LOB"];
                drow["Parameter_Value"] = Convert.ToBoolean(dr["Parameter_Value"]);
                drow["Parameter_Value1"] = Convert.ToBoolean(dr["Parameter_Value1"]);
                drow["Parameter_Value2"] = Convert.ToBoolean(dr["Parameter_Value2"]);
                drow["Parameter_Value3"] = Convert.ToBoolean(dr["Parameter_Value3"]);
                drow["Is_Active"] = Convert.ToBoolean(dr["Is_Active"]);
                dtList.Rows.Add(drow);
            }
            gvIncome.DataSource = dtList;
            gvIncome.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSIncomdRegDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterIncomeReg", dictParam);
            DataTable dtList = new DataTable();
            dtList = dtHesderDetails.Clone();
            dtList.Columns["Parameter_Value"].DataType = typeof(System.Boolean);
            dtList.Columns["Parameter_Value1"].DataType = typeof(System.Boolean);
            dtList.Columns["Is_Active"].DataType = typeof(System.Boolean);
            foreach (DataRow dr in dtHesderDetails.Rows)
            {
                DataRow drow = dtList.NewRow();
                drow["Param_Set_ID"] = dr["Param_Set_ID"];
                drow["Company_ID"] = dr["Company_ID"];
                drow["LOB_ID"] = dr["LOB_ID"];
                drow["LOB"] = dr["LOB"];
                drow["Parameter_Value"] = Convert.ToBoolean(dr["Parameter_Value"]);
                drow["Parameter_Value1"] = Convert.ToBoolean(dr["Parameter_Value1"]);
                drow["Is_Active"] = Convert.ToBoolean(dr["Is_Active"]);
                dtList.Rows.Add(drow);
            }

            gvIncomeType.DataSource = dtList;
            gvIncomeType.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSIncomdRevisionDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterIncomeRevision", dictParam);

            gvRevision.DataSource = dtHesderDetails;
            gvRevision.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSFutureInstallmentDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterFutureInstallment", dictParam);

            //gvFutureInstallments.DataSource = dtHesderDetails;
            //gvFutureInstallments.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSSplitDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterAccSplit", dictParam);

            //gvSplit.DataSource = dtHesderDetails;
            //gvSplit.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSGapDaysBWPMCandApprovalDate()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterGapDaysBWPMCandApprovalDate", dictParam);

            gvGapDaysBWPMCDate.DataSource = dtHesderDetails;
            gvGapDaysBWPMCDate.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSPreClosureTypeDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterPreClosureType", dictParam);

            gvAccountPreClosureType.DataSource = dtHesderDetails;
            gvAccountPreClosureType.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSOverdueDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            dictParam.Add("@User_ID", intUserId.ToString());
            dictParam.Add("@Is_Active", "1");
            //DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobalParameterOverdue", dictParam);

            //gvOverdue.DataSource = dtHesderDetails;
            //gvOverdue.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    private void FunPriGetGPSLOBDetails()
    {
        //try
        //{
        //    Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //    dictParam.Add("@User_ID", intUserId.ToString());
        //    dictParam.Add("@Company_ID", intCompanyId.ToString());
        //    if (intGlobalID == 0)
        //    {
        //        dictParam.Add("@Is_Active", "1");
        //    }
        //    DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetLOBDetails1", dictParam);
        //    gvGapDays.DataSource = dtHesderDetails;
        //    gvGapDays.DataBind();
        //}
        //catch (Exception ex)
        //{
        //      ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        //    throw;
        //}
    }

    private void FunPriGPSDetails()
    {
        try
        {
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", intCompanyId.ToString());
            DataTable dtHesderDetails = Utility.GetDefaultData("S3G_LOANAD_GetGlobal", dictParam);
            if (dtHesderDetails.Rows.Count > 0)
            {
                hdnGlobalParamId.Value = dtHesderDetails.Rows[0]["Param_Set_ID"].ToString();
                hdnUserId.Value = dtHesderDetails.Rows[0]["Created_By"].ToString();
                hdnUserLevelId.Value = dtHesderDetails.Rows[0]["User_Level_ID"].ToString();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw;
        }
    }

    #region [Clear Controls]

    /// <summary>
    /// Clearing All the Controls 
    /// </summary>
    public void FunPubClearAllControls()
    {
        //gvAccount.DataSource = null;
        //gvAccount.DataBind();
        FunPriGetGPSDetails();
        //SetInitialRow();
        FunPubClearIncomeGrid();
        FunPubClearIncomeTypeGrid();
        FunPubClearOverdueGrid();
        FunPubClearRevisionGrid();
        //FunPubClearGapDaysGrid();
        FunPubClearTextBoxAndDropDown();
        FunPubClearAccountClosureGapDaysGrid();
        FunPubClearAccountClosureType();
        FunPubClearAccountConsolidationFuntureInsallmentGrid();
        FunPubClearAccountConsolidationNoSplit();
    }

    /// <summary>
    /// Clearing Text box and Dropdown
    /// </summary>
    public void FunPubClearTextBoxAndDropDown()
    {
        txtCurrentRate.Text = txtGapRevision.Text = txtPaise.Text = txtCollateral.Text = txtOperating.Text = txtFactory.Text = txtWaivedOffAmount.Text = "";
        ddlInterest.SelectedIndex = ddlOD1.SelectedIndex = 0;
    }

    /// <summary>
    /// Clearing Income Grid Check box 
    /// </summary>
    public void FunPubClearIncomeGrid()
    {
        foreach (GridViewRow gvr in gvIncome.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                //((CheckBox)gvr.FindControl("CbxEsm")).Checked = false;
                ((CheckBox)gvr.FindControl("CbxIRR")).Checked = false;
                //((CheckBox)gvr.FindControl("CbxSOD")).Checked = false;
                //((CheckBox)gvr.FindControl("CbxProduct")).Checked = false;
            }
        }
    }

    /// <summary>
    /// Clearing Income Type Grid Check box 
    /// </summary>
    public void FunPubClearIncomeTypeGrid()
    {
        foreach (GridViewRow gvr in gvIncomeType.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)gvr.FindControl("CbxIns")).Checked = false;
                ((CheckBox)gvr.FindControl("CbxAccounting")).Checked = false;
            }
        }
    }

    /// <summary>
    /// Clearing Overdue Grid Text box 
    /// </summary>
    public void FunPubClearOverdueGrid()
    {
        //foreach (GridViewRow gvr in gvOverdue.Rows)
        //{
        //    if (gvr.RowType == DataControlRowType.DataRow)
        //    {
        //        ((TextBox)gvr.FindControl("txtDenoDays")).Text = "";
        //        ((TextBox)gvr.FindControl("txtGraceDays")).Text = "";
        //        ((TextBox)gvr.FindControl("txtODIRate")).Text = "";
        //    }
        //}
    }

    /// <summary>
    /// Clearing Revision Grid Text Box
    /// </summary>
    public void FunPubClearRevisionGrid()
    {
        foreach (GridViewRow gvr in gvRevision.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                ((TextBox)gvr.FindControl("txtAlpha")).Text = "";
            }
        }
    }

    /// <summary>
    /// Clearing Gap Days Grid Text Box
    /// </summary>
    //public void FunPubClearGapDaysGrid()
    //{
    //    foreach (GridViewRow gvr in gvGapDays.Rows)
    //    {
    //        if (gvr.RowType == DataControlRowType.DataRow)
    //        {
    //            ((TextBox)gvr.FindControl("txtDays")).Text = "";
    //        }
    //    }
    //}

    public void FunPubClearAccountClosureGapDaysGrid()
    {
        foreach (GridViewRow gvr in gvGapDaysBWPMCDate.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                ((TextBox)gvr.FindControl("txtDays")).Text = "";
            }
        }
    }

    public void FunPubClearAccountClosureType()
    {
        foreach (GridViewRow gvr in gvAccountPreClosureType.Rows)
        {
            if (gvr.RowType == DataControlRowType.DataRow)
            {
                ((CheckBox)gvr.FindControl("chkIRR_Type")).Checked = false;
                ((CheckBox)gvr.FindControl("chkNPV_Type")).Checked = false;
                ((CheckBox)gvr.FindControl("chkPrinciple_Type")).Checked = false;
                ((TextBox)gvr.FindControl("txtPrincipleInterestRate")).Text = "";  // Added by Tamilselvan.S on 26/04/2011 for Principle Interest Rate 
            }
        }
    }

    public void FunPubClearAccountConsolidationFuntureInsallmentGrid()
    {
        //foreach (GridViewRow gvr in gvFutureInstallments.Rows)
        //{
        //    if (gvr.RowType == DataControlRowType.DataRow)
        //    {
        //        ((TextBox)gvr.FindControl("txtInstallment")).Text = "";
        //    }
        //}
    }

    public void FunPubClearAccountConsolidationNoSplit()
    {
        //foreach (GridViewRow gvr in gvSplit.Rows)
        //{
        //    if (gvr.RowType == DataControlRowType.DataRow)
        //    {
        //        ((TextBox)gvr.FindControl("txtSplit")).Text = "";
        //    }
        //}
    }

    #endregion [Clear Controls]

    #region [Set Prefix and Suffix for Text Box]

    private void FunPriSetMaxLength()
    {
        txtCurrentRate.SetDecimalPrefixSuffix(2, 4, true, "Current Lending Rate");
        txtGapRevision.SetDecimalPrefixSuffix(2, 0, true, "Revision Gap Days");
        txtFactory.SetDecimalPrefixSuffix(3, 0, true, "Factoring Invoice gap days");
        foreach (GridViewRow grv in gvRevision.Rows)
        {
            TextBox txtAlpha = grv.FindControl("txtAlpha") as TextBox;
            //txtAlpha.SetDecimalPrefixSuffix(2, 4, true, "+/-%");
            txtAlpha.SetDecimalWithPlusMinPrefixSuffix(2, intGpsSuffixRW, false, "+/-%");
        }
        //foreach (GridViewRow grva in gvOverdue.Rows)
        //{
        //    TextBox txtODI = grva.FindControl("txtODIRate") as TextBox;
        //    txtODI.SetDecimalPrefixSuffix(2, 2, false, "ODI Rate");
        //}
        foreach (GridViewRow grva in gvAccountPreClosureType.Rows)
        {
            TextBox txtPrincipleInterestRate = grva.FindControl("txtPrincipleInterestRate") as TextBox;
            txtPrincipleInterestRate.SetDecimalPrefixSuffix(2, 2, false, "Principal Interest Rate");
        }
    }

    #endregion [Set Prefix and Suffix for Text Box]

    #region [DateFormat]

    public string FormatDate(string strDate)
    {
        return DateTime.Parse(strDate, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
    }

    #endregion [DateFormat]

    #endregion [Function's]

    #region [Grid View RowDataBound Event's]

    protected void gvGapDays_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
        }
    }

    protected void gvIncome_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            //((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAll('" +
            //        ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
            Label lblLOB = (Label)e.Row.FindControl("lblLOB");
            string strLOB = (lblLOB.Text.Split('-')[0]).Replace(" ", "");
        }
    }

    protected void gvIncomeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
        }
    }

    protected void gvRevision_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
            TextBox txtPlusMinus = (TextBox)e.Row.FindControl("txtAlpha");
            if (!string.IsNullOrEmpty(txtPlusMinus.Text))
                txtPlusMinus.Text = Convert.ToDecimal(txtPlusMinus.Text).ToString(Utility.SetSuffix());
        }
    }

    //protected void gvOverdue_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
    //            e.Row.Enabled = false;
    //    }
    //}

    protected void gvAccountPreClosureType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
        }
    }

    protected void gvGapDaysBWPMCDate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
                e.Row.Enabled = false;
        }
    }

    //protected void gvFutureInstallments_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
    //            e.Row.Enabled = false;
    //    }
    //}

    //protected void gvSplit_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (((CheckBox)e.Row.FindControl("chkActive")).Checked == false)
    //            e.Row.Enabled = false;
    //    }
    //}

    #endregion [Grid View RowDataBound Event's]

    #region [Grid View RowCreated Event's]

    protected void gvIncome_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CheckBox CbxEsm = (CheckBox)e.Row.FindControl("CbxEsm");
            CheckBox CbxIRR = (CheckBox)e.Row.FindControl("CbxIRR");
           // CheckBox CbxSOD = (CheckBox)e.Row.FindControl("CbxSOD");
            //CheckBox CbxProduct = (CheckBox)e.Row.FindControl("CbxProduct");
            int intRowindex = e.Row.RowIndex + 2;
            string strRowIndex = Convert.ToString(Convert.ToInt32(intRowindex) <= 9 ? ("0" + intRowindex.ToString()) : intRowindex.ToString());
           // CbxEsm.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeMethod('{0}',this,'{1}','{2}');", gvIncome.ClientID, "CbxEsm", strRowIndex);
            CbxIRR.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeMethod('{0}',this,'{1}','{2}');", gvIncome.ClientID, "CbxIRR", strRowIndex);
           // CbxSOD.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeMethod('{0}',this,'{1}','{2}');", gvIncome.ClientID, "CbxSOD", strRowIndex);
            //CbxProduct.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeMethod('{0}',this,'{1}','{2}');", gvIncome.ClientID, "CbxProduct", strRowIndex);
        }
    }

    protected void gvIncomeType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox CbxIns = (CheckBox)e.Row.FindControl("CbxIns");
            CheckBox CbxAccounting = (CheckBox)e.Row.FindControl("CbxAccounting");
            int intRowindex = e.Row.RowIndex + 2;
            string strRowIndex = Convert.ToString(Convert.ToInt32(intRowindex) <= 9 ? ("0" + intRowindex.ToString()) : intRowindex.ToString());
            CbxIns.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeType('{0}',this,'{1}','{2}');", gvIncomeType.ClientID, "Ins", strRowIndex);
            CbxAccounting.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowIncomeType('{0}',this,'{1}','{2}');", gvIncomeType.ClientID, "Accounting", strRowIndex);
        }
    }

    protected void gvAccountPreClosureType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkIRR_Type = (CheckBox)e.Row.FindControl("chkIRR_Type");
            CheckBox chkNPV_Type = (CheckBox)e.Row.FindControl("chkNPV_Type");
            CheckBox chkPrinciple_Type = (CheckBox)e.Row.FindControl("chkPrinciple_Type");
            int intRowindex = e.Row.RowIndex + 2;
            string strRowIndex = Convert.ToString(Convert.ToInt32(intRowindex) <= 9 ? ("0" + intRowindex.ToString()) : intRowindex.ToString());
            chkIRR_Type.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowPreClosureType('{0}',this,'{1}','{2}');", gvAccountPreClosureType.ClientID, "IRR_Type", strRowIndex);
            chkNPV_Type.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowPreClosureType('{0}',this,'{1}','{2}');", gvAccountPreClosureType.ClientID, "NPV_Type", strRowIndex);
            chkPrinciple_Type.Attributes["onclick"] = string.Format("javascript:fnDGUnselectAllCoulmnRowPreClosureType('{0}',this,'{1}','{2}');", gvAccountPreClosureType.ClientID, "Principle_Type", strRowIndex);
        }
    }

    #endregion [Grid View RowCreated Event's]

}
