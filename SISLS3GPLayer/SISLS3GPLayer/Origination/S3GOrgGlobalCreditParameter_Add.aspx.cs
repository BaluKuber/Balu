/// Module Name     :   Origination
/// Screen Name     :   S3GOrgGlobalCreditParameter_Add
/// Created By      :   Ramesh M
/// Created Date    :   10-July-2010
/// Purpose         :   To insert and update Pricing Approval details
/// Purpose         :   To view product master details
/// Modify By       :   R. Manikandan
/// Purpose         :   To fix the Bug
/// Date of Modification :  17 - 09 - 2010
/// Remodified By   :   M.Saran
/// Purpose         :   To Refix the Bug
/// Date of Modification :  21 - 10 - 2010

using System;
using System.Web.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Collections.Generic;

public partial class Origination_S3GOrgGlobalCreditParameter_Add : ApplyThemeForProject
{
    int intGCount = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    static int intApprovCount = 0;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    //Code end
    int intErrCode = 0;
    decimal Importance = 0;
    decimal Score = 0;
    int intRowCount = 0;
    int intGlobal_CreditParameterCopyProfileID = 0;
    int intGlobal_CreditParameter_ID = 0;
    string temp = "";
    string strRedirectPage = "window.location.href='../Origination/S3GOrgGlobalCreditParameter_View.aspx'";
    DataTable dtApprovals = new DataTable();
    DataSet DsGlobalCreditParameterAll = new DataSet();
    SerializationMode Sermode = SerializationMode.Binary;
    StringBuilder strScoreDtls = new StringBuilder();
    Dictionary<string, string> Procparam = null;
    StringBuilder strApprovalLimit = new StringBuilder();
    CreditMgtServicesReference.CreditMgtServicesClient ObjCreditMgtServicesClient;
    S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable ObjCreditParam_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable();
    S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable ObjGlobalCreditParameter_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable();
    S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable ObjGlobalCreditParamApproval_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable();
    S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable ObjGlobalCreditScore_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable();
    UserInfo ObjUserInfo = new UserInfo();


    protected void Page_Load(object sender, EventArgs e)
    {
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        //btnCopyProfile.Enabled = false;
        bClearList = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("ClearListValues"));
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        //Code end

        txtExposureVariation.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");
        txtNegativeVariation.Attributes.Add("onkeypress", "fnAllowNumbersOnly(true,false,this)");

        if (Request.QueryString["qsGCPID"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsGCPID"));
            intGlobal_CreditParameter_ID = Convert.ToInt32(fromTicket.Name);
            strMode = Request.QueryString.Get("qsMode").ToString();
        }

        if (!IsPostBack)
        {
            panelApproval.Visible = false;  //visible false
            panelGlobalCredit.Visible = false; //visible false
            FunProInitializOtherGrid();
            if (intGlobal_CreditParameter_ID > 0)
            {
                FunPubGetGlobalCreditParamDetailsUpdate();
                btnReset.Visible = false;
                btnGo.Visible = false;
                btnCopyProfile.Visible = false;
                btnClear.Enabled = false;
                //btnSave.Visible = true;
                //btnClear.Visible = true;
                //btnSave.Style.Add("display","inline");
                //btnClear.Style.Add("display", "inline");
            }
            else
            {

                tcGloblaCreditParameter.ActiveTabIndex = 0;
                FunPubBind();
                chkActive.Enabled = false;

            }
            //User Authorization

            if ((intGlobal_CreditParameter_ID > 0) && (strMode == "M"))
            {
                FunPriDisableControls(1);
            }
            else if ((intGlobal_CreditParameter_ID > 0) && (strMode == "Q")) // Query // Modify
            {
                FunPriDisableControls(-1);
            }
            else
            {
                FunPriDisableControls(0);
                btnSave.Style.Add("display", "none");
                btnClear.Style.Add("display", "none");
                ddlLOB.Focus();
            }
            //Code end
        }
        txtExposureVariation.SetPercentagePrefixSuffix(3, 2, false, false, "Exposure Variation %");
        txtNegativeVariation.SetPercentagePrefixSuffix(3, 2, false, false, " Negative Variation %");


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
                        btnSave.Enabled = false;
                    }
                    break;

                case 1: // Modify Mode


                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    ddlLOB.Enabled = false;
                    ddlConstitution.Enabled = false;
                    ddlProduct.Enabled = false;
                    // FunDisableEmptyParameter();
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }

                    btnClear.Enabled = false;
                    ddlEntityType.Enabled = false;
                    rdbLevel.Enabled = false;
                    RequiredFieldValidator2.Enabled = RequiredFieldValidator1.Enabled = false;
                    break;

                case -1:// Query Mode
                    rdbLevel.Enabled = false;
                    RequiredFieldValidator2.Enabled = RequiredFieldValidator1.Enabled = false;
                    ddlLOB.Enabled = ddlConstitution.Enabled = ddlProduct.Enabled = false;
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage, false);
                    }
                    grvApprovals.FooterRow.Visible = false;
                    //grvApprovals.Columns[5].Visible = false;
                    grvApprovals.Columns[grvApprovals.Columns.Count - 1].Visible = false;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    txtExposureVariation.ReadOnly = true;
                    txtNegativeVariation.ReadOnly = true;
                    grvApprovals.FooterRow.Enabled = false;
                    chkActive.Enabled = false;
                    //grvApprovals.Enabled = false;
                    GrvGlobalCreditParamUpdate.Enabled = false;
                    if (bClearList)
                    {
                        ddlLOB.ClearDropDownList();
                        ddlProduct.ClearDropDownList();
                        ddlConstitution.ClearDropDownList();
                    }
                    ddlEntityType.Enabled = false;

                    btnDEVModalAdd.Enabled = false;
                    if (grvApprover.FooterRow != null)
                        grvApprover.FooterRow.Visible = false;
                    grvApprover.Columns[grvApprover.Columns.Count - 1].Visible = false;

                    grvOtherInfo.FooterRow.Visible = false;
                    grvOtherInfo.Columns[grvOtherInfo.Columns.Count - 1].Visible = false;

                    break;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    //Code end
    public void FunPubBind()
    {
        //Bind LOB
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@User_Id", Convert.ToString(intUserID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Is_UserLobActive", "1");
            //@Program_ID - Parameter added for Usermgt 
            Procparam.Add("@Program_ID", "21");

            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            ddlLOB.AddItemToolTip();

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    public void FunBindingData()
    {
        try
        {
            DataRow dr;
            dtApprovals.Columns.Add("SNo");
            dtApprovals.Columns.Add("Facility_From");
            dtApprovals.Columns.Add("GCP_Approval_ID");
            dtApprovals.Columns.Add("Facility_To");
            //dtApprovals.Columns.Add("Number_Of_Approvals");
            //dtApprovals.Columns.Add("Hygiene_Percentage");
            //dtApprovals.Columns.Add("Minimal_Approval_Required");
            dtApprovals.Columns.Add("Range_From", typeof(decimal));
            dtApprovals.Columns.Add("Range_To", typeof(decimal));

            dr = dtApprovals.NewRow();
            dr["SNo"] = 0;
            dtApprovals.Rows.Add(dr);
            ViewState["Approvals"] = dtApprovals;
            grvApprovals.DataSource = dtApprovals;
            grvApprovals.DataBind();
            FunClearZeros();
            grvApprovals.Rows[0].Visible = false;
            //dtApprovals.Rows[0].Delete();
            ViewState["Approvals"] = dtApprovals;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Bind Constitut 
        try
        {
            ddlLOB.AddItemToolTip();
            ddlConstitution.Items.Clear();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Is_Active", "true");
            ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "ConstitutionName" });
            ddlConstitution.AddItemToolTip();
            //Bind Product 
            Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
            Procparam1.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam1.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam1.Add("@Is_Active", "1");
            ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam1, new string[] { "Product_ID", "Product_Code", "Product_Name" });
            ddlProduct.AddItemToolTip();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvApprovals_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["Approvals"];
            dtDelete.Rows.RemoveAt(e.RowIndex);

            if (dtDelete.Rows.Count <= 0)
            {
                dtDelete = null;
                dtApprovals.Clear();
                ViewState["Approvals"] = null;
                DataRow dr;
                dtApprovals.Columns.Add("SNo");
                dtApprovals.Columns.Add("GCP_Approval_ID");
                dtApprovals.Columns.Add("Facility_From");
                dtApprovals.Columns.Add("GCP_Approval_ID");
                dtApprovals.Columns.Add("Facility_To");
                //dtApprovals.Columns.Add("Number_Of_Approvals");
                //dtApprovals.Columns.Add("Hygiene_Percentage");
                //dtApprovals.Columns.Add("Minimal_Approval_Required");
                dtApprovals.Columns.Add("Range_From", typeof(decimal));
                dtApprovals.Columns.Add("Range_To", typeof(decimal));
                dr = dtApprovals.NewRow();
                dr["SNo"] = 0;
                dtApprovals.Rows.Add(dr);
                ViewState["Approvals"] = dtApprovals;
                grvApprovals.DataSource = dtApprovals;
                grvApprovals.DataBind();
                FunClearZeros();
                grvApprovals.Rows[0].Visible = false;
                ViewState["Approvals"] = dtApprovals;
                return;
            }
            grvApprovals.DataSource = dtDelete;
            grvApprovals.DataBind();
            FunClearZeros();
            ViewState["Approvals"] = dtDelete;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void grvApprovals_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                Label lbl = (Label)grvApprovals.Rows[grvApprovals.Rows.Count - 1].FindControl("lblSNo");
                TextBox txtaddFacilityFromAmt = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityFromAmt");
                TextBox txtaddFacilityToAmt = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityToAmt");
                //TextBox txtaddRange_From = (TextBox)grvApprovals.FooterRow.FindControl("txtaddRange_From");
                //TextBox txtaddHygiene = (TextBox)grvApprovals.FooterRow.FindControl("txtaddHygiene");
                //TextBox txtaddminApprovalreq = (TextBox)grvApprovals.FooterRow.FindControl("txtaddminApprovalreq");
                TextBox txtaddRange_From = (TextBox)grvApprovals.FooterRow.FindControl("txtaddRange_From");
                TextBox txtaddRange_To = (TextBox)grvApprovals.FooterRow.FindControl("txtaddRange_To");



                if (txtaddFacilityFromAmt.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Enter the Facility From');", true);
                    txtaddFacilityToAmt.Focus();
                    return;
                }
                if (txtaddFacilityToAmt.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter the Facility To');", true);
                    txtaddFacilityToAmt.Focus();
                    return;
                }

                if (txtaddRange_From.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter the Range From');", true);
                    txtaddRange_From.Focus();
                    return;
                }
                if (txtaddRange_To.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter the Range To');", true);
                    txtaddRange_To.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(txtaddRange_From.Text.Trim()))
                {
                    if (Convert.ToDecimal(txtaddRange_From.Text.Trim()) > 100)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Range From should not be greater than 100 %');", true);
                        txtaddRange_From.Focus();
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(txtaddRange_To.Text.Trim()))
                {
                    if (Convert.ToDecimal(txtaddRange_To.Text.Trim()) > 100)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Range To should not be greater than 100 %');", true);
                        txtaddRange_To.Focus();
                        return;
                    }
                }

                if (Convert.ToDecimal(txtaddFacilityToAmt.Text.Trim()) <= Convert.ToDecimal(txtaddFacilityFromAmt.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Facility To should be greater than Facility From');", true);
                    return;
                }

                if (Convert.ToDecimal(txtaddRange_To.Text.Trim()) <= Convert.ToDecimal(txtaddRange_From.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Range To should be greater than Range From');", true);
                    return;
                }

                int intSNo = (Convert.ToInt32(lbl.Text)) + 1;

                if (grvApprovals.Rows.Count == 1 && ViewState["dtTempAuthApprover"] == null)
                {
                    Utility.FunShowAlertMsg(this, "Add Atleast one Approver");
                    return;
                }

                if (((DataTable)(ViewState["dtTempAuthApprover"])) != null)
                {
                    DataRow[] dArray = ((DataTable)(ViewState["dtTempAuthApprover"])).Select("SNo='" + intSNo + "' and Location<>'' and ApprovPerson<>''");
                    if (dArray.Length == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Add Atleast one Approver");
                        return;
                    }
                }

                /*
                                if (txtaddFacilityFromAmt.Text.Trim() == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Enter the Facility From');", true);
                                    txtaddFacilityToAmt.Focus();
                                    return;
                                }
                                if (txtaddFacilityToAmt.Text.Trim() == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter the Facility To');", true);
                                    txtaddFacilityToAmt.Focus();
                                    return;
                                }
                                if (Convert.ToDecimal(txtaddFacilityToAmt.Text.Trim()) <= Convert.ToDecimal(txtaddFacilityFromAmt.Text.Trim()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Facility To should be greater than Facility From');", true);
                                    return;
                                }

                                if (txtaddRange_From.Text.Trim() == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter the Range From');", true);
                                    txtaddFacilityToAmt.Focus();
                                    return;
                                }
                                if (Convert.ToDecimal(txtaddFacilityToAmt.Text.Trim()) <= Convert.ToDecimal(txtaddFacilityFromAmt.Text.Trim()))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert5", "alert('Facility To should be greater than Facility From');", true);
                                    return;
                                }


                                else if (txtaddRange_From.Text.Trim() == string.Empty)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Enter the Number of Approvals');", true);
                                    txtaddRange_From.Focus();
                                    return;
                                }
                                else if (Convert.ToInt32(txtaddRange_From.Text.Trim()) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Number of Approvals cannot be zero');", true);
                                    txtaddRange_From.Focus();
                                    return;
                                }
                                else if ((!string.IsNullOrEmpty(txtaddRange_To.Text)) && (string.IsNullOrEmpty(txtaddminApprovalreq.Text)))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the Number of Approvals (Level 4/5)');", true);
                                    txtaddHygiene.Focus();
                                    return;
                                }
                                else if ((string.IsNullOrEmpty(txtaddHygiene.Text)) && (!string.IsNullOrEmpty(txtaddminApprovalreq.Text)))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the hygiene %');", true);
                                    txtaddHygiene.Focus();
                                    return;
                                }
                                else if ((!string.IsNullOrEmpty(txtaddHygiene.Text)) && (!string.IsNullOrEmpty(txtaddminApprovalreq.Text)))
                                {
                                    //if (Convert.ToDouble(txtaddHygiene.Text) < Convert.ToDouble(txtaddRange_From.Text) || Convert.ToDouble(txtaddHygiene.Text) < Convert.ToDouble(txtaddminApprovalreq.Text))
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Hygiene should be greater than No. of Approval and Minimum Approval Requirement');", true);
                                    //    txtaddHygiene.Focus();
                                    //    return;
                                    //}
                                    if (Convert.ToDouble(txtaddHygiene.Text) == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Hygiene should not be zero');", true);
                                        txtaddminApprovalreq.Focus();
                                        return;
                                    }
                                    else if ((Convert.ToInt32(txtaddRange_From.Text)) > Convert.ToInt32(txtaddminApprovalreq.Text))
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Number of Approvals (Level 4/5) Should be Greater than or equal to  Number of Approvals');", true);
                                        txtaddminApprovalreq.Focus();
                                        return;
                                    }
                                }
                                
                decimal dcToAmount = 0;
                TextBox txtFacilityToAmt = (TextBox)grvApprovals.Rows[grvApprovals.Rows.Count - 1].FindControl("txtFacilityToAmt");
                if (!string.IsNullOrEmpty(txtFacilityToAmt.Text.Trim()))
                {
                    dcToAmount = Convert.ToDecimal(txtFacilityToAmt.Text.Trim()) + 1;
                    if (dcToAmount != Convert.ToDecimal(txtaddFacilityFromAmt.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert4", "alert('Should not be overlap');", true);
                        return;
                    }
                }
                if (txtaddHygiene.Text.Trim() != "")
                    txtaddHygiene.Text = txtaddHygiene.Text;
                else
                    txtaddHygiene.Text = "0";
                if (txtaddminApprovalreq.Text.Trim() != "")
                    txtaddminApprovalreq.Text = txtaddminApprovalreq.Text;
                else
                    txtaddminApprovalreq.Text = "0";
                */
                dtApprovals = (DataTable)ViewState["Approvals"];
                dr = dtApprovals.NewRow();
                if (!string.IsNullOrEmpty(lbl.Text))
                    dr["SNo"] = Convert.ToInt32(lbl.Text) + 1;
                dr["Facility_From"] = txtaddFacilityFromAmt.Text.Trim();
                dr["Facility_To"] = txtaddFacilityToAmt.Text.Trim();
                //dr["Number_Of_Approvals"] = txtaddRange_From.Text.Trim();
                //dr["Hygiene_Percentage"] = txtaddHygiene.Text.Trim();
                //dr["Minimal_Approval_Required"] = txtaddminApprovalreq.Text.Trim();
                dr["Range_From"] = txtaddRange_From.Text;
                dr["Range_To"] = txtaddRange_To.Text;
                dtApprovals.Rows.Add(dr);
                grvApprovals.Visible = true;
                grvApprovals.DataSource = dtApprovals;
                grvApprovals.DataBind();
                FunClearZeros();
                ViewState["Approvals"] = dtApprovals;

                /* TextBox txtaddFacilityFromAmt1 = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityFromAmt");
                 if (txtaddFacilityFromAmt1 != null && txtaddFacilityToAmt != null && txtaddFacilityToAmt.Text.Trim() != "")
                     txtaddFacilityFromAmt1.Text = Convert.ToString(Convert.ToDecimal(txtaddFacilityToAmt.Text.Trim()) + Convert.ToInt32("1"));
                 */
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunClearZeros()
    {
        try
        {
            foreach (GridViewRow GRow in grvApprovals.Rows)
            {
                /* TextBox txtHygiene = (TextBox)GRow.FindControl("txtHygiene");
                 TextBox txtminApprovalreq = (TextBox)GRow.FindControl("txtminApprovalreq");

                 if (txtHygiene.Text != "" && Convert.ToDouble(txtHygiene.Text) == 0)
                 {
                     txtHygiene.Text = "";
                 }
                 if (txtminApprovalreq.Text != "" && Convert.ToDouble(txtminApprovalreq.Text) == 0)
                 {
                     txtminApprovalreq.Text = "";
                 }
 */
                TextBox txtFacilityToAmt = (TextBox)GRow.FindControl("txtFacilityToAmt");
                TextBox txtFacilityFromAmt = (TextBox)GRow.FindControl("txtFacilityFromAmt");

                txtFacilityToAmt.ReadOnly = true;
                txtFacilityFromAmt.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        decimal TotalImporance = 0;
        decimal Tottxtscore = 0;
        ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            decimal FromAmount = 0;
            decimal ToAmount = 0;
            decimal FromRange = 0;
            decimal ToRange = 0;
            //decimal Hygiene = 0;
            //decimal AdmiApproval = 0;
            //decimal MinAppr = 0;
            string strScore = "";
            string strImporance = "";
            string strHygiene = "";
            string strminApprovalreq = "";
            Session["ToAmount"] = null;
            if (grvApprovals.Rows.Count == 1 && grvApprovals.Rows[0].Visible == false)
            {
                Utility.FunShowAlertMsg(this, "Enter Atleast one Approval Details");
                return;
            }
            if (intGlobal_CreditParameter_ID > 0) //update for Approval table
            {
                strScoreDtls.Append("<Root>");
                foreach (GridViewRow grvCreditParamScore in GrvGlobalCreditParamUpdate.Rows)
                {
                    Label lblscoreItemID = (Label)grvCreditParamScore.FindControl("lblScore_Item_ID");
                    Label lblDec = (Label)grvCreditParamScore.FindControl("lblDec");
                    Label lblGlobal_Credit_Parameter_CreditScore_ID = (Label)grvCreditParamScore.FindControl("lblGlobal_Credit_Parameter_CreditScore_ID");
                    TextBox txtimportance = (TextBox)grvCreditParamScore.FindControl("txtImportance");
                    TextBox txtscore = (TextBox)grvCreditParamScore.FindControl("txtScore");
                    //txtscore.Enabled = false;
                    if ((txtimportance.Text != string.Empty) && (txtscore.Text == string.Empty))
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert5", "alert('Enter the Score');", true);
                        txtscore.Focus();
                        return;
                    }

                    if ((txtimportance.Text == string.Empty) && (txtscore.Text != string.Empty))
                    {
                        if (lblDec.Text.ToUpper().Trim() != "HYGIENE FACTOR")
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 0;
                            txtimportance.Focus();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert4", "alert('Enter the Importance%');", true);
                            return;
                        }
                    }

                    if ((txtimportance.Text != string.Empty) && (txtscore.Text != string.Empty))
                    {
                        intRowCount++;
                    }
                    if (grvCreditParamScore.RowIndex == GrvGlobalCreditParamUpdate.Rows.Count - 1)
                    {
                        if (txtscore.Text != string.Empty)
                        {
                            intRowCount++;
                        }
                    }
                    strScore = "";
                    strImporance = "";
                    if (txtimportance.Text.Trim() != "")
                        strImporance = txtimportance.Text.Trim();
                    //else
                    //    strImporance = "0";
                    if (txtscore.Text.Trim() != "")
                        strScore = txtscore.Text.Trim();
                    //else
                    //    strScore = "0";
                    strScoreDtls.Append(" <ScoreDetails CreditScore_Item_ID='" + lblscoreItemID.Text.Trim() + "'");
                    strScoreDtls.Append("  GLOBAL_CRDT_PARAM_CTSCORE_ID='" + lblGlobal_Credit_Parameter_CreditScore_ID.Text.Trim() + "'");
                    strScoreDtls.Append("  Percentage_Of_Importance='" + strImporance + "'");
                    strScoreDtls.Append("  Score='" + strScore + "'/>");
                }
                strScoreDtls.Append("</Root>");
            }
            else  ////Insert for Score table
            {
                strScoreDtls.Append("<Root>");
                for (int i = 0; i < GrvGlobalCreditParamUpdate.Rows.Count; i++)
                {
                    Label lblscoreItemID = (Label)GrvGlobalCreditParamUpdate.Rows[i].FindControl("lblScore_Item_ID");
                    Label lblDec = (Label)GrvGlobalCreditParamUpdate.Rows[i].FindControl("lblDec");
                    TextBox txtimportance = (TextBox)GrvGlobalCreditParamUpdate.Rows[i].FindControl("txtImportance");
                    TextBox txtscore = (TextBox)GrvGlobalCreditParamUpdate.Rows[i].FindControl("txtScore");
                    if (hidMode.Value == "FromProfile")
                    {
                        if (txtimportance.Text == "" && txtimportance.Enabled)
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 0;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert7", "alert('Enter the Importance %');", true);
                            txtimportance.Focus();
                            return;
                        }
                    }

                    if ((txtimportance.Text != string.Empty) && (txtscore.Text == string.Empty))
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert7", "alert('Enter the Score');", true);
                        txtscore.Focus();
                        return;
                    }
                    if ((txtimportance.Text == string.Empty) && (txtscore.Text != string.Empty))
                    {
                        if (lblDec.Text.ToUpper().Trim() != "HYGIENE FACTOR")
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 0;
                            txtimportance.Focus();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert4", "alert('Enter the Importance%');", true);
                            return;

                        }
                    }

                    if ((txtimportance.Text != string.Empty) && (txtscore.Text != string.Empty))
                    {
                        intRowCount++;
                    }
                    if (i == GrvGlobalCreditParamUpdate.Rows.Count - 1)
                    {
                        if (txtscore.Text != string.Empty)
                        {
                            intRowCount++;
                        }
                    }
                    strImporance = "";
                    strScore = "";
                    if (txtimportance.Text.Trim() != "")
                        strImporance = txtimportance.Text.Trim();
                    //else
                    //    strImporance = "0";
                    if (txtscore.Text.Trim() != "")
                        strScore = txtscore.Text.Trim();
                    //else
                    //    strScore = "0";
                    strScoreDtls.Append(" <ScoreDetails CreditScore_Item_ID='" + lblscoreItemID.Text.Trim() + "'");
                    strScoreDtls.Append("  Percentage_Of_Importance='" + strImporance + "'");
                    strScoreDtls.Append("  Score='" + strScore + "'/>");
                }// chkActive.Checked ? true : false;
                strScoreDtls.Append("</Root>");
            }

            foreach (GridViewRow grvScore in GrvGlobalCreditParamUpdate.Rows)
            {
                TextBox txtImportance = (TextBox)grvScore.FindControl("txtImportance");
                Label lblDec = (Label)grvScore.FindControl("lblDec");
                TextBox txtscore = (TextBox)grvScore.FindControl("txtScore");
                if (txtImportance.Text.Trim() != string.Empty)
                    TotalImporance += Convert.ToDecimal(txtImportance.Text);

                if (txtscore.Text.Trim() != string.Empty && lblDec.Text.ToUpper().Trim() != "HYGIENE FACTOR")
                    Tottxtscore += Convert.ToDecimal(txtscore.Text);

                if (lblDec.Text.ToUpper().Trim() == "HYGIENE FACTOR")
                {
                    if (txtscore.Text.Trim() != string.Empty)
                    {
                        if (Convert.ToDecimal(txtscore.Text) == 0)
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 0;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Enter a valid Hygiene Factor Score');", true);
                            return;
                        }
                    }
                    else
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Enter the Hygiene Factor Score');", true);
                        return;
                    }
                }

                if (lblDec.Text.ToUpper().Trim() == "CREDIT SCORE")
                {
                    if (txtscore.Text.Trim() != string.Empty)
                    {
                        if (Convert.ToDecimal(txtscore.Text) == 0)
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 0;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Enter a valid Credit Score');", true);
                            return;
                        }
                    }
                    else
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Enter the Credit Score');", true);
                        return;
                    }
                }
            }
            if (intRowCount < 3)
            {
                tcGloblaCreditParameter.ActiveTabIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Enter the minimum 2 Importance% and Score apart from Hygiene factor');", true);
                return;
            }
            TextBox txtTotalImportance = (TextBox)GrvGlobalCreditParamUpdate.FooterRow.FindControl("txtTotalImportance");
            if (txtTotalImportance != null) txtTotalImportance.Text = TotalImporance.ToString();

            if (TotalImporance != 100)
            {
                tcGloblaCreditParameter.ActiveTabIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert3", "alert('Importance should be 100 %');", true);
                return;
            }
            if (intGlobal_CreditParameter_ID > 0)  //update for Approval table
            {
                /* strApprovalLimit.Append("<Root>");
                 if (grvApprovals.Rows.Count <= 1)
                 {
                     TextBox txtFacilityToAmt = (TextBox)grvApprovals.Rows[0].FindControl("txtFacilityToAmt");
                     if (String.IsNullOrEmpty(txtFacilityToAmt.Text.Trim()))
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter atleast 1 approval detail');", true);
                         return;
                     }
                 }
                 foreach (GridViewRow grvApproval in grvApprovals.Rows)
                 {
                     Label lblApprovalID = (Label)grvApproval.FindControl("lblApprovallimitID");
                     TextBox txtFacilityFromAmt = (TextBox)grvApproval.FindControl("txtFacilityFromAmt");

                     if (txtFacilityFromAmt.Text == string.Empty)
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert9", "alert('Enter the Facility From');", true);
                         txtFacilityFromAmt.Focus();
                         return;
                     }
                     if (Session["ToAmount"] != null)
                     {
                         ToAmount = Convert.ToDecimal(Session["ToAmount"]) + Convert.ToInt32("1");
                         FromAmount = Convert.ToDecimal(txtFacilityFromAmt.Text);
                         if (ToAmount != FromAmount)
                         {
                             tcGloblaCreditParameter.ActiveTabIndex = 2;
                             ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert10", "alert('Should not be overlap');", true);
                             txtFacilityFromAmt.Focus();
                             return;
                         }
                     }
                     //TextBox txtFacilityFromAmt = (TextBox)grvApproval.FindControl("txtFacilityFromAmt");
                     TextBox txtFacilityToAmt = (TextBox)grvApproval.FindControl("txtFacilityToAmt");
                     TextBox txtnoofapprovals = (TextBox)grvApproval.FindControl("txtnoofapprovals");
                     //TextBox txtHygiene = (TextBox)grvApproval.FindControl("txtHygiene");
                     //TextBox txtminApprovalreq = (TextBox)grvApproval.FindControl("txtminApprovalreq");
                     TextBox txtRange_From = (TextBox)grvApproval.FindControl("txtRange_From");
                     TextBox txtRange_To = (TextBox)grvApproval.FindControl("txtRange_To");
                   if (txtHygiene.Text.Trim() != "")
                         strHygiene = txtHygiene.Text.Trim();
                     else
                         strHygiene = "0";

                     if (txtminApprovalreq.Text.Trim() != "")
                         strminApprovalreq = txtminApprovalreq.Text;
                     else
                         strminApprovalreq = "0";
                   
                     strApprovalLimit.Append(" <Approvallimit Facility_From='" + txtFacilityFromAmt.Text.Trim() + "'");
                     strApprovalLimit.Append("  Global_Credit_Parameter_Approval_ID='" + lblApprovalID.Text.Trim() + "'");
                     strApprovalLimit.Append("  Facility_To='" + txtFacilityToAmt.Text.Trim() + "'");
                     //strApprovalLimit.Append("  Number_Of_Approvals='" + txtnoofapprovals.Text.Trim() + "'");
                     //strApprovalLimit.Append("  Hygiene_Percentage='" + strHygiene + "'");
                     //strApprovalLimit.Append("  Minimal_Approval_Required='" + strminApprovalreq + "'/>");
                     strApprovalLimit.Append("  Range_From='" + txtRange_From.Text.Trim() + "'");
                     strApprovalLimit.Append("  Range_To='" + txtRange_To.Text.Trim() + "'");
                     if (txtnoofapprovals.Text.Trim() == string.Empty)
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Enter the Number of Approvals');", true);
                         txtnoofapprovals.Focus();
                         return;
                     }
                     else if (Convert.ToInt32(txtnoofapprovals.Text) == 0)
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Number of Approvals cannot be zero');", true);
                         txtnoofapprovals.Focus();
                         return;
                     }
                     else if ((!string.IsNullOrEmpty(txtRange_From.Text)))
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the Range From');", true);
                         txtRange_From.Focus();
                         return;

                     }
                     else if ((!string.IsNullOrEmpty(txtRange_To.Text)))
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the Range To');", true);
                         txtRange_To.Focus();
                         return;

                     }
                        
                     //else if ((string.IsNullOrEmpty(txtHygiene.Text)) && (!string.IsNullOrEmpty(txtminApprovalreq.Text)))
                     //{
                     //    tcGloblaCreditParameter.ActiveTabIndex = 2;
                     //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the hygiene %');", true);
                     //    txtHygiene.Focus();
                     //    return;
                     //}

                     FromAmount = Convert.ToDecimal(txtFacilityFromAmt.Text);
                     ToAmount = Convert.ToDecimal(txtFacilityToAmt.Text);
                     if ((string.IsNullOrEmpty(txtHygiene.Text)))
                     {
                         Hygiene = 0;
                     }
                     else
                     {
                         Hygiene = Convert.ToDecimal(txtHygiene.Text);
                     }
                     if (string.IsNullOrEmpty(txtminApprovalreq.Text))
                     {
                         MinAppr = 0;
                     }
                     else
                     {
                         MinAppr = Convert.ToDecimal(txtminApprovalreq.Text);
                     }
                     AdmiApproval = Convert.ToDecimal(txtnoofapprovals.Text);
                     if (FromAmount >= ToAmount)
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Facility To should be greater than Facility From');", true);
                         txtFacilityToAmt.Focus();
                         return;
                     }

                     if (FromRange >= ToRange)
                     {
                         tcGloblaCreditParameter.ActiveTabIndex = 2;
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Range To should be greater than Range From');", true);
                         FromRange.Focus();
                         return;
                     }

                     //if (MinAppr != 0 && Hygiene != 0)
                     if (txtminApprovalreq.Text != "" && txtHygiene.Text != "")
                     {
                         if (MinAppr < AdmiApproval)
                         {
                             tcGloblaCreditParameter.ActiveTabIndex = 2;
                             ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Number of Approvals (Level 4/5) Should be Greater than or equal to Number of Approvals');", true);
                             txtnoofapprovals.Focus();
                             return;
                         }
                     }
                     //if (txtHygiene.Text.Trim() != "")
                     //{
                     //    if ((AdmiApproval >= Hygiene) || (MinAppr >= Hygiene))
                     //    {
                     //        tcGloblaCreditParameter.ActiveTabIndex = 2;
                     //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Hygiene% Should be Greater than Minimum No of Approval and No of Approval');", true);
                     //        txtHygiene.Focus();
                     //        return;
                     //    }
                     //}

                     Session["ToAmount"] = ToAmount.ToString();
                 }
                 strApprovalLimit.Append("</Root>");*/
            }
            else  //Insert for Approval table
            {
                /*if (grvApprovals.Rows.Count <= 1)
                {
                    TextBox txtFacilityToAmt = (TextBox)grvApprovals.Rows[0].FindControl("txtFacilityToAmt");
                    if (String.IsNullOrEmpty(txtFacilityToAmt.Text.Trim()))
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert2", "alert('Enter atleast 1 approval detail');", true);
                        return;
                    }
                }
                strApprovalLimit.Append("<Root>");
                for (int i = 0; i < grvApprovals.Rows.Count; i++)
                {
                    TextBox txtFacilityFromAmt = (TextBox)grvApprovals.Rows[i].FindControl("txtFacilityFromAmt");
                    if (txtFacilityFromAmt.Text == string.Empty)
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert13", "alert('Enter the Facility From');", true);
                        txtFacilityFromAmt.Focus();
                        return;
                    }

                    //Mani11

                    if (Session["ToAmount"] != null)
                    {
                        ToAmount = Convert.ToDecimal(Session["ToAmount"]) + Convert.ToInt32("1");
                        FromAmount = Convert.ToDecimal(txtFacilityFromAmt.Text);
                        if (ToAmount != FromAmount)
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 2;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert14", "alert('Should not be overlap');", true);
                            txtFacilityFromAmt.Focus();
                            return;
                        }
                    }

                    TextBox txtFacilityToAmt = (TextBox)grvApprovals.Rows[i].FindControl("txtFacilityToAmt");
                    TextBox txtnoofapprovals = (TextBox)grvApprovals.Rows[i].FindControl("txtnoofapprovals");
                    TextBox txtHygiene = (TextBox)grvApprovals.Rows[i].FindControl("txtHygiene");
                    TextBox txtminApprovalreq = (TextBox)grvApprovals.Rows[i].FindControl("txtminApprovalreq");
                    // TextBox txtFacilityFromAmt = (TextBox)grvApprovals.Rows[i].FindControl("txtFacilityFromAmt");

                    if (txtnoofapprovals.Text.Trim() == string.Empty)
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Enter the Number of Approvals');", true);
                        txtnoofapprovals.Focus();
                        return;
                    }
                    else if (Convert.ToInt32(txtnoofapprovals.Text) == 0)
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert3", "alert('Number of Approvals cannot be zero');", true);
                        txtnoofapprovals.Focus();
                        return;
                    }
                    else if ((!string.IsNullOrEmpty(txtHygiene.Text)))
                    {
                        if (string.IsNullOrEmpty(txtminApprovalreq.Text))
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 2;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the Number of Approvals (Level 4/5)');", true);
                            txtHygiene.Focus();
                            return;
                        }
                    }
                    else if ((string.IsNullOrEmpty(txtHygiene.Text)) && (!string.IsNullOrEmpty(txtminApprovalreq.Text)))
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertApp", "alert('Enter the hygiene %');", true);
                        txtHygiene.Focus();
                        return;
                    }

                    if (txtHygiene.Text.Trim() != "")
                        strHygiene = txtHygiene.Text.Trim();
                    else
                        strHygiene = "0";

                    if (txtminApprovalreq.Text.Trim() != "")
                        strminApprovalreq = txtminApprovalreq.Text;
                    else
                        strminApprovalreq = "0";

                    strApprovalLimit.Append(" <Approvallimit Facility_From='" + txtFacilityFromAmt.Text.Trim() + "'");
                    strApprovalLimit.Append("  Facility_To='" + txtFacilityToAmt.Text.Trim() + "'");
                    strApprovalLimit.Append("  Number_Of_Approvals='" + txtnoofapprovals.Text.Trim() + "'");
                    strApprovalLimit.Append("  Hygiene_Percentage='" + strHygiene + "'");
                    strApprovalLimit.Append("  Minimal_Approval_Required='" + strminApprovalreq + "'/>");

                    FromAmount = Convert.ToDecimal(txtFacilityFromAmt.Text);
                    ToAmount = Convert.ToDecimal(txtFacilityToAmt.Text);
                    Hygiene = Convert.ToDecimal(strHygiene);

                    if (FromAmount >= ToAmount)
                    {
                        tcGloblaCreditParameter.ActiveTabIndex = 2;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Facility To should be greater than Facility From');", true);
                        txtFacilityToAmt.Focus();
                        return;
                    }
                    else
                        Session["ToAmount"] = ToAmount.ToString();

                    if ((string.IsNullOrEmpty(txtHygiene.Text)))
                    {
                        Hygiene = 0;
                    }
                    else
                    {
                        Hygiene = Convert.ToDecimal(txtHygiene.Text);
                    }
                    if (string.IsNullOrEmpty(txtminApprovalreq.Text))
                    {
                        MinAppr = 0;
                    }
                    else
                    {
                        MinAppr = Convert.ToDecimal(txtminApprovalreq.Text);
                    }
                    AdmiApproval = Convert.ToDecimal(txtnoofapprovals.Text);

                    if (txtminApprovalreq.Text.Trim() != "" && txtHygiene.Text.Trim() != "")
                    {
                        if (MinAppr < AdmiApproval)
                        {
                            tcGloblaCreditParameter.ActiveTabIndex = 2;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Number of Approvals (Level 4/5) Should be Greater than or equal to Number of Approvals');", true);
                            txtnoofapprovals.Focus();
                            return;
                        }
                    }
                    //if (txtHygiene.Text.Trim() != "" )
                    //{
                    //    if ((AdmiApproval >= Hygiene) || (MinAppr >= Hygiene))
                    //    {
                    //        tcGloblaCreditParameter.ActiveTabIndex = 2;
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert11", "alert('Hygiene% Should be Greater than Minimum No of Approval and No of Approval');", true);
                    //        txtHygiene.Focus();
                    //        return;
                    //    }
                    //}

                }
                strApprovalLimit.Append("</Root>");*/
            }

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParameterRow;
            ObjGlobalCreditParameterRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
            ObjGlobalCreditParameterRow.Global_Credit_Parameter_ID = intGlobal_CreditParameter_ID;
            ObjGlobalCreditParameterRow.Company_ID = intCompanyID;
            ObjGlobalCreditParameterRow.Entity_Type_Id = Convert.ToInt32(ddlEntityType.SelectedValue);
            if (ddlLOB.Items.Count > 0)
            {
                ObjGlobalCreditParameterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value);
            }
            if (ddlProduct.Items.Count > 0)
            {
                ObjGlobalCreditParameterRow.Product_ID = Convert.ToInt32(ddlProduct.SelectedItem.Value);
            }
            if (ddlConstitution.Items.Count > 0)
            {
                ObjGlobalCreditParameterRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedItem.Value);
            }
            ObjGlobalCreditParameterRow.Negative_Deviation = Convert.ToDecimal(txtNegativeVariation.Text.Trim());
            ObjGlobalCreditParameterRow.Exposure_Variance = Convert.ToDecimal(txtExposureVariation.Text.Trim());
            ObjGlobalCreditParameterRow.Is_Active = chkActive.Checked ? true : false;
            ObjGlobalCreditParameterRow.Created_By = intUserID;
            ObjGlobalCreditParameterRow.XMLCreditScore = strScoreDtls.ToString();
            ObjGlobalCreditParameterRow.XMLApprovallimit = grvApprovals.FunPubFormXml();
            ObjGlobalCreditParameterRow.XMLApprover = ((DataTable)ViewState["dtTempAuthApprover"]).FunPubFormXml();
            ObjGlobalCreditParameterRow.XMLOthers = ((DataTable)ViewState["dtOthers"]).FunPubFormXml();

            ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParameterRow);
            if (intGlobal_CreditParameter_ID > 0)
            {
                intErrCode = ObjCreditMgtServicesClient.FunPubModifyGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
                if (intErrCode == 3)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Global Credit Parameter record refered in transaction, Cannot be updated');", true);
                    return;
                }
            }
            else
            {
                intErrCode = ObjCreditMgtServicesClient.FunPubCreateGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
            }
            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intGlobal_CreditParameter_ID > 0)
                {
                    Utility.FunShowAlertMsg(this, "Global Credit Parameter details updated successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strRedirectPage, true);
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Global Credit Parameter details added successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strRedirectPage, true);
                }
            }
            if (intErrCode == 50)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Global Credit Parameter updation failed');", true);
                return;
            }
            else if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Selected Combination already exists in Active mode');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            TextBox txtTotalImportance = (TextBox)GrvGlobalCreditParamUpdate.FooterRow.FindControl("txtTotalImportance");
            if (txtTotalImportance != null) txtTotalImportance.Text = TotalImporance.ToString();

            TextBox txtTotalScore = (TextBox)GrvGlobalCreditParamUpdate.FooterRow.FindControl("txtTotalScore");
            if (txtTotalScore != null) txtTotalScore.Text = Tottxtscore.ToString();

            ObjCreditMgtServicesClient.Close();
        }
    }


    protected void grvApprovals_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //To Remove the AH_003 Bug (to remove Remove Link Button in the Modify mode)
            TextBox txtFacilityFrom = new TextBox();
            TextBox txtFacilityTo = new TextBox();
            TextBox txtFacilityToF = new TextBox();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //temp = "";
                if (strMode == "M")
                {
                    if (intApprovCount > grvApprovals.Rows.Count)
                    {
                        LinkButton lnkremove = (LinkButton)e.Row.FindControl("btnRemove");
                        lnkremove.Enabled = false;
                    }

                }
                txtFacilityFrom = (TextBox)e.Row.FindControl("txtFacilityFromAmt");
                txtFacilityTo = (TextBox)e.Row.FindControl("txtFacilityToAmt");




                // TextBox txtFacilityToF = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityFromAmt");
                //txtFacilityToF = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityFromAmt");                    
                //if (intGCount != 0)
                //    txtFacilityFrom.Text = temp;
                //else
                //    txtFacilityFrom.Text = "1";

                //intGCount++;
                //temp = ((Convert.ToInt64(txtFacilityTo.Text)) + 1).ToString();


                int rowcount;
                foreach (GridViewRow grRow in grvApprovals.Rows)
                {
                    TextBox txt = (TextBox)grRow.FindControl("txtFacilityFrom");
                }

                TextBox txtFacilityFromamount = (TextBox)e.Row.FindControl("txtFacilityFromAmt");
                TextBox txtFacilityToAmt = (TextBox)e.Row.FindControl("txtFacilityToAmt");
                TextBox txtRange_From = (TextBox)e.Row.FindControl("txtRange_From");
                TextBox txtRange_To = (TextBox)e.Row.FindControl("txtRange_To");

                txtRange_From.SetDecimalPrefixSuffix(3, 2, false, false, "Range From %");
                txtRange_To.SetDecimalPrefixSuffix(3, 2, false, false, "Range To %");



                if (strMode == "Q")
                {
                    txtFacilityFromamount.ReadOnly =
                        txtFacilityToAmt.ReadOnly =
                        txtRange_From.ReadOnly =
                        txtRange_To.ReadOnly = true;
                }
            }
            //}

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //txtFacilityToF = (TextBox)e.Row.FindControl("txtaddFacilityFromAmt");
                //txtFacilityToF.Text = temp;
                TextBox txtRange_From = (TextBox)e.Row.FindControl("txtaddRange_From");
                TextBox txtRange_To = (TextBox)e.Row.FindControl("txtaddRange_To");
                txtRange_From.SetDecimalPrefixSuffix(3, 2, false, false, "Range From %");
                txtRange_To.SetDecimalPrefixSuffix(3, 2, false, false, "Range To %");


                dtApprovals = (DataTable)ViewState["Approvals"];
                if (dtApprovals.Rows.Count != 0)
                {
                    if (dtApprovals.Rows[0]["Facility_From"].ToString() == "")
                    {
                        dtApprovals.Rows[0].Delete();
                    }
                }
                if (dtApprovals.Rows.Count == 0)
                {
                    TextBox txtFromamount = (TextBox)e.Row.FindControl("txtaddFacilityFromAmt");
                    txtFromamount.Text = "1";
                    txtFromamount.Enabled = false;
                }


            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            dtApprovals.Clear();
            ViewState["Approvals"] = null;
            FunBindingData();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {

        trCopyProfileMessage.Visible = false;

        ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            hidMode.Value = "New";

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow;
            ObjGlobalCreditParamRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
            ObjGlobalCreditParamRow.Company_ID = intCompanyID;
            if (ddlLOB.SelectedIndex > 0)
                ObjGlobalCreditParamRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value);
            if (ddlProduct.SelectedIndex > 0)
                ObjGlobalCreditParamRow.Product_ID = Convert.ToInt32(ddlProduct.SelectedItem.Value);
            if (ddlConstitution.SelectedIndex > 0)
                ObjGlobalCreditParamRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedItem.Value);
            ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow);
            SerializationMode Sermode = SerializationMode.Binary;
            byte[] bytesObjGlobalCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
            DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatable, SerializationMode.Binary, typeof(DataSet));

            //TC            
            if (DsGlobalCreditParameterAll.Tables[3] != null && DsGlobalCreditParameterAll.Tables[3].Rows.Count > 0)
            {
                if (DsGlobalCreditParameterAll.Tables[3].Rows[0]["CNT"].ToString() != "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert18", "alert('Selected combination already exists');", true);
                    return;
                }
            }
            ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            byte[] bytesObjCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryCreditScoreGuideParameter(Sermode, ClsPubSerialize.Serialize(ObjCreditParam_DataTable, Sermode));
            ObjCreditParam_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjCreditParamDatatable, SerializationMode.Binary, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable));
            ObjCreditParam_DataTable.Columns.Add("GLOBAL_CRDT_PARAM_CTSCORE_ID");
            ObjCreditParam_DataTable.Columns.Add("Percentage_Of_Importance");
            ObjCreditParam_DataTable.Columns.Add("Score");

            GrvGlobalCreditParamUpdate.DataSource = ObjCreditParam_DataTable;
            GrvGlobalCreditParamUpdate.DataBind();
            if (GrvGlobalCreditParamUpdate.Rows.Count > 0)
            {
                btnCopyProfile.Enabled = true;
            }
            btnSave.Style.Add("display", "inline");
            btnClear.Style.Add("display", "inline");

            panelApproval.Visible = true;  //visible true
            panelGlobalCredit.Visible = true; //visible true

            ViewState["table"] = ObjCreditParam_DataTable;
            FunBindingData();
            txtExposureVariation.Text = "";
            txtNegativeVariation.Text = "";
            divCopyProfile.Style.Add("display", "none");


            btnCopyProfile.Text = "Copy Profile";

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditMgtServicesClient.Close();
        }
    }

    protected void Funscoreenabled(object sender, EventArgs e)
    {
        try
        {
            decimal total = 0;
            string strFieldAtt = ((TextBox)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("GrvGlobalCreditParamUpdate")).Replace("GrvGlobalCreditParamUpdate_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            //TextBox Quly = (TextBox)gvDlivery.Rows[gRowIndex].FindControl("txtQuantity");
            int growLen = Convert.ToInt32(GrvGlobalCreditParamUpdate.Rows.Count);
            TextBox txtImportance = (TextBox)GrvGlobalCreditParamUpdate.Rows[gRowIndex].FindControl("txtImportance");
            TextBox txtscore = (TextBox)GrvGlobalCreditParamUpdate.Rows[gRowIndex].FindControl("txtScore");
            TextBox txtscoreH = (TextBox)GrvGlobalCreditParamUpdate.Rows[growLen - 1].FindControl("txtScore");
            txtscoreH.Enabled = true;
            // txtscore.Attributes.Add("onFocusIn", "javascript:jsscorefn('" + txtImportance.ClientID + "');");

            if (txtImportance.Text == "")
            {
                txtscore.Text = "";
                txtscore.Enabled = false;
            }
            else
            {
                txtscore.Enabled = true;
            }
            // txtFacilityFrom = (TextBox)e.Row.FindControl("txtFacilityToAmt");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnCopyProfile_Click(object sender, EventArgs e)
    {
        try
        {
            ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow;
            ObjGlobalCreditParamRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
            ObjGlobalCreditParamRow.Company_ID = intCompanyID;
            ObjGlobalCreditParamRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value);
            if (ddlProduct.SelectedIndex > 0)
                ObjGlobalCreditParamRow.Product_ID = Convert.ToInt32(ddlProduct.SelectedItem.Value);
            ObjGlobalCreditParamRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedItem.Value);
            ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow);
            SerializationMode Sermode = SerializationMode.Binary;
            byte[] bytesObjGlobalCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
            DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatable, SerializationMode.Binary, typeof(DataSet));
            if (DsGlobalCreditParameterAll.Tables[3].Rows.Count > 0)
            {
                if (DsGlobalCreditParameterAll.Tables[3].Rows[0]["CNT"].ToString() != "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert21", "alert('Selected combination already exists');", true);
                    return;
                }
            }

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable ObjGlobalCreditParameter_DataTables = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable();
            ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow1;
            ObjGlobalCreditParamRow1 = ObjGlobalCreditParameter_DataTables.NewS3G_ORG_Global_Credit_ParameterRow();
            ObjGlobalCreditParamRow1.Company_ID = intCompanyID;
            ObjGlobalCreditParameter_DataTables.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow1);
            byte[] bytesObjGlobalCreditParamDatatables = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTables, Sermode));
            DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatables, SerializationMode.Binary, typeof(DataSet));

            DataView dv = DsGlobalCreditParameterAll.Tables[0].DefaultView;
            S3GDALDBType DBType = FunPubGetDatabaseType();
            if (DBType == S3GDALDBType.ORACLE)
            {
                dv.RowFilter = "Is_Active='True'";
            }
            else
            {
                dv.RowFilter = "Is_Active=1";
            }
            GrvGlobalCreditMaster.DataSource = dv;
            GrvGlobalCreditMaster.DataBind();
            if (DBType == S3GDALDBType.SQL)
            {
                if (DsGlobalCreditParameterAll.Tables[0].Select("Is_Active=1").Length > 0)
                {
                    //btnSubmit.Visible = true;
                    //btnGo.Enabled = false;
                    //btnCopyProfile.Enabled = false;
                    divCopyProfile.Style.Add("display", "Block");
                    trCopyProfileMessage.Visible = false;

                }
                else
                {
                    divCopyProfile.Style.Add("display", "none");
                    trCopyProfileMessage.Visible = true;
                    btnSubmit.Visible = false;
                    btnGo.Enabled = true;
                    btnCopyProfile.Enabled = true;
                }
            }
            else
            {
                if (DsGlobalCreditParameterAll.Tables[0].Select("Is_Active='True'").Length > 0)
                {
                    //btnSubmit.Visible = true;
                    //btnGo.Enabled = false;
                    //btnCopyProfile.Enabled = false;
                    divCopyProfile.Style.Add("display", "Block");
                    trCopyProfileMessage.Visible = false;

                }
                else
                {
                    divCopyProfile.Style.Add("display", "none");
                    trCopyProfileMessage.Visible = true;
                    btnSubmit.Visible = false;
                    btnGo.Enabled = true;
                    btnCopyProfile.Enabled = true;
                }

            }

            //Bind Score Details Based on GlobalCreditParamerID
            GrvGlobalCreditMaster.Visible = true;
            panelApproval.Visible = false;  //visible false
            panelGlobalCredit.Visible = false; //visible false

            if (btnCopyProfile.Text == "Copy Profile")
            {
                btnCopyProfile.Text = "Hide Profile";
                btnSave.Enabled = btnClear.Enabled = false;
            }
            else
            {
                btnCopyProfile.Text = "Copy Profile";
                btnCopyProfile.Enabled = true;
                divCopyProfile.Style.Add("display", "none");
                panelApproval.Visible = true;  //visible false
                panelGlobalCredit.Visible = true; //visible false
                btnSave.Enabled = btnClear.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditMgtServicesClient.Close();
        }
    }

    protected void FunDisableEmptyParameter()
    {
        try
        {
            foreach (GridViewRow grvScore in GrvGlobalCreditParamUpdate.Rows)
            {
                TextBox txtImportance = (TextBox)grvScore.FindControl("txtImportance");
                TextBox txtscore = (TextBox)grvScore.FindControl("txtScore");

                if (txtImportance.Text == string.Empty && txtscore.Text == string.Empty)
                {
                    txtImportance.ReadOnly = true;
                    txtscore.ReadOnly = true;
                }
                else
                {
                    txtImportance.ReadOnly = false;
                    txtscore.ReadOnly = false;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void GrvGlobalCreditMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView grvSender = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBoxSelect = (CheckBox)e.Row.FindControl("chkselect");
                chkBoxSelect.Attributes["onclick"] = string.Format
                (
                "javascript:fnDGUnselectAllExpectSelected('{0}',this);",
                grvSender.ClientID
                );

            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (GrvGlobalCreditMaster.Rows.Count > 0)
            {
                for (int i = 0; i < GrvGlobalCreditMaster.Rows.Count; i++)
                {
                    CheckBox chkGlobalCreditParameter = (CheckBox)GrvGlobalCreditMaster.Rows[i].FindControl("chkselect");
                    int counts = 0;
                    foreach (GridViewRow grv in GrvGlobalCreditMaster.Rows)
                    {
                        if (((CheckBox)grv.FindControl("chkselect")).Checked)
                        {
                            counts++;
                        }

                    }
                    if (counts == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Select any one of the Global Parameter');", true);
                        return;
                    }

                    if (chkGlobalCreditParameter.Checked)
                    {
                        Label lblGlobalCreditParameterID = (Label)GrvGlobalCreditMaster.Rows[i].FindControl("lblGlobalCreditParameterID");
                        intGlobal_CreditParameterCopyProfileID = Convert.ToInt32(lblGlobalCreditParameterID.Text.Trim());

                        //Bind Header Details in GlobalCreditParameter
                        ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
                        S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow;
                        ObjGlobalCreditParamRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
                        ObjGlobalCreditParamRow.Global_Credit_Parameter_ID = intGlobal_CreditParameterCopyProfileID;
                        ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow);
                        byte[] bytesObjGlobalCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
                        DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatable, SerializationMode.Binary, typeof(DataSet));

                        //Bind Score Details Based on GlobalCreditParamerID
                        GrvGlobalCreditMaster.Visible = false;

                        panelApproval.Visible = true;  //visible false
                        panelGlobalCredit.Visible = true; //visible false

                        GrvGlobalCreditParamUpdate.DataSource = DsGlobalCreditParameterAll.Tables[1];
                        GrvGlobalCreditParamUpdate.DataBind();
                        // FunDisableEmptyParameter();

                        int Count = 0;
                        for (int j = 0; j <= DsGlobalCreditParameterAll.Tables[1].Rows.Count - 1; j++)
                        {
                            if ((DsGlobalCreditParameterAll.Tables[1].Rows[j]["Percentage_Of_Importance"].ToString() != "0") || (DsGlobalCreditParameterAll.Tables[1].Rows[j]["Score"].ToString() != "0"))
                            {
                                Count++;
                            }
                        }

                        //ViewState["Mode"] = "FromProfile";
                        hidMode.Value = "FromProfile";

                        txtExposureVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Exposure_Variance"].ToString();
                        txtNegativeVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Negative_Deviation"].ToString();
                        chkActive.Checked = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Is_Active"].ToString() == "True" ? true : false;

                        //Bind Approval Detail Based On GlobalCreditParamerID
                        if (DsGlobalCreditParameterAll.Tables[2].Rows.Count > 0)
                        {
                            ViewState["Approvals"] = DsGlobalCreditParameterAll.Tables[2];
                            grvApprovals.DataSource = DsGlobalCreditParameterAll.Tables[2];
                            grvApprovals.DataBind();
                            FunClearZeros();
                        }
                        btnSubmit.Visible = false;
                        btnGo.Enabled = true;
                        btnCopyProfile.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditMgtServicesClient.Close();
        }
    }

    public void FunPubGetGlobalCreditParamDetailsUpdate()
    {
        //Bind LOB
        try
        {
            //Dictionary<string, string> Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam.Add("@User_Id", Convert.ToString(intUserID));
            //Procparam.Add("@Program_ID", "21");
            //ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
            //ddlLOB.AddItemToolTip();

            ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow;
            ObjGlobalCreditParamRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
            ObjGlobalCreditParamRow.Global_Credit_Parameter_ID = intGlobal_CreditParameter_ID;
            ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow);
            byte[] bytesObjGlobalCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
            DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatable, SerializationMode.Binary, typeof(DataSet));

            ListItem lst;


            if (DsGlobalCreditParameterAll.Tables[0] != null)
            {
                txtExposureVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Exposure_Variance"].ToString();
                txtNegativeVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Negative_Deviation"].ToString();
                chkActive.Checked = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Is_Active"].ToString() == "True" ? true : false;

                lst = new ListItem(DsGlobalCreditParameterAll.Tables[0].Rows[0]["LOB_Code_Name"].ToString(), DsGlobalCreditParameterAll.Tables[0].Rows[0]["LOB_ID"].ToString());
                ddlLOB.Items.Add(lst);
                ddlLOB.SelectedValue = DsGlobalCreditParameterAll.Tables[0].Rows[0]["LOB_ID"].ToString();
            }

            //Bind Constitut 
            //Dictionary<string, string> Procparam1 = new Dictionary<string, string>();
            //Procparam1.Add("@Company_ID", Convert.ToString(intCompanyID));
            //Procparam1.Add("@LOB_ID", ddlLOB.SelectedValue);
            ////Procparam1.Add("@Is_Active", "true");
            //ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam1, new string[] { "Constitution_ID", "ConstitutionName" });

            lst = new ListItem(DsGlobalCreditParameterAll.Tables[0].Rows[0]["Constitution_Name"].ToString(), DsGlobalCreditParameterAll.Tables[0].Rows[0]["Constitution_ID"].ToString());
            ddlConstitution.Items.Add(lst);

            ddlConstitution.AddItemToolTip();
            //Bind Product 
            //Dictionary<string, string> Procparam2 = new Dictionary<string, string>();
            //Procparam2.Add("@LOB_ID", ddlLOB.SelectedValue);
            //Procparam2.Add("@Company_ID", Convert.ToString(intCompanyID));
            ////Procparam2.Add("@Is_Active", "1");
            //ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, Procparam2, new string[] { "Product_ID", "Product_Code", "Product_Name" });
            //ddlProduct.AddItemToolTip();
            ddlConstitution.SelectedValue = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Constitution_ID"].ToString();

            lst = new ListItem(DsGlobalCreditParameterAll.Tables[0].Rows[0]["Product_Code_Name"].ToString(), DsGlobalCreditParameterAll.Tables[0].Rows[0]["Product_ID"].ToString());
            ddlProduct.Items.Add(lst);

            ddlProduct.SelectedValue = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Product_ID"].ToString();

            //Entity Type
            ddlEntityType.SelectedValue = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Entity_Type_Id"].ToString();


            //scoer
            panelApproval.Visible = true;  //visible false
            panelGlobalCredit.Visible = true; //visible false

            GrvGlobalCreditParamUpdate.DataSource = DsGlobalCreditParameterAll.Tables[1];
            GrvGlobalCreditParamUpdate.DataBind();

            //Bind Approval Detail Based On GlobalCreditParamerID
            if (DsGlobalCreditParameterAll.Tables[2] != null && DsGlobalCreditParameterAll.Tables[2].Rows.Count > 0)
            {
                intApprovCount = DsGlobalCreditParameterAll.Tables[2].Rows.Count;

                ViewState["Approvals"] = DsGlobalCreditParameterAll.Tables[2];
                grvApprovals.DataSource = DsGlobalCreditParameterAll.Tables[2];
                grvApprovals.DataBind();
                FunClearZeros();

            }
            tcGloblaCreditParameter.ActiveTabIndex = 0;

            if (DsGlobalCreditParameterAll.Tables[5] != null && DsGlobalCreditParameterAll.Tables[5].Rows.Count > 0)
            {
                ViewState["dtTempAuthApprover"] = DsGlobalCreditParameterAll.Tables[5];
            }

            if (DsGlobalCreditParameterAll.Tables[6] != null && DsGlobalCreditParameterAll.Tables[6].Rows.Count > 0)
            {
                grvOtherInfo.DataSource = DsGlobalCreditParameterAll.Tables[6];
                grvOtherInfo.DataBind();

                ViewState["dtOthers"] = DsGlobalCreditParameterAll.Tables[6];

                if (PageMode != PageModes.Query)
                {
                    FunProLoadNumFooterDDL();
                }
            }
            else
            {
                FunProInitializOtherGrid();
            }

            if (ddlLOB.SelectedValue.ToString() != "0")
            {
                rdbLevel.SelectedValue = "1";
            }
            else
            {
                rdbLevel.SelectedValue = "2";
            }
            /* Commented by Srivatsan on 13/03/2012 to allow updation even if a Credit Transaction exists.
             * This is as per the advice of Kaliraj. Bug No:5431
             */
            //if (DsGlobalCreditParameterAll.Tables[4] != null && DsGlobalCreditParameterAll.Tables[4].Rows.Count > 0)
            //{
            //    FunPriDisableControls(-1);
            //    btnSave.Enabled = chkActive.Enabled = true;
            //}
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditMgtServicesClient.Close();
        }
    }

    protected void GrvGlobalCreditParamUpdate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtImportance = (TextBox)e.Row.FindControl("txtImportance");
                TextBox txtscore = (TextBox)e.Row.FindControl("txtScore");
                // txtscore.Attributes.Add("onFocusIn", "javascript:jsscorefn('" + txtImportance.ClientID + "');");
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    if (txtImportance.Text == "")
                //    {
                //        txtscore.Enabled = false;
                //    }
                //    else
                //    {
                //        txtscore.Enabled = true;
                //    }
                //   // txtFacilityFrom = (TextBox)e.Row.FindControl("txtFacilityToAmt");

                //}
                Label lblDec = (Label)e.Row.FindControl("lblDec");
                //txtImportance.Attributes.Add("onfocusOut", "javascript:FunscoreEnabled('" + txtImportance.ClientID + "','" + txtscore.ClientID + "','" + lblDec.Text.ToUpper() + "');");

                //txtscore.Attributes.Add("onfocusin", "javascript:FunscoreEnabled('" + txtImportance.ClientID + "','" + txtscore.ClientID + "','" + lblDec.Text.ToUpper() + "');");

                txtImportance.Attributes.Add("onblur", "javascript:FunscoreEnabled('" + txtImportance.ClientID + "','" + txtscore.ClientID + "','" + lblDec.Text.ToUpper() + "');SumImportance('" + txtImportance.ClientID + "');");

                txtscore.Attributes.Add("onblur", "javascript:FunscoreEnabled('" + txtImportance.ClientID + "','" + txtscore.ClientID + "','" + lblDec.Text.ToUpper() + "');OnSumScore('" + txtscore.ClientID + "');");
                if (txtImportance.Text != "")
                {
                    //if (txtImportance.Text.Trim().Substring(txtImportance.Text.Trim().Length - 4, 4) == ".000")
                    //    txtImportance.Text = txtImportance.Text.Replace(".000", "").Trim();

                    Importance += Convert.ToDecimal(txtImportance.Text);

                    //if (txtImportance.Text.Trim() == "0")
                    //    txtImportance.Text = "";
                }
                if (hidMode.Value == "FromProfile")
                {
                    if (txtImportance.Text == "")
                        txtImportance.Enabled = false;
                }

                TextBox txtScore = (TextBox)e.Row.FindControl("txtScore");
                if (txtScore.Text != "")
                {
                    //if (lblDec.Text.Trim().ToUpper() != "HYGIENE FACTOR" && txtScore.Text.Trim().Substring(txtScore.Text.Trim().Length - 4, 4) == ".000")
                    //    txtScore.Text = txtScore.Text.Replace(".000", "").Trim();

                    if (lblDec.Text.Trim().ToUpper() != "HYGIENE FACTOR")
                        Score += Convert.ToDecimal(txtScore.Text);

                    //if (txtScore.Text.Trim() == "0")
                    //    txtScore.Text = "";
                }
                if (lblDec.Text != string.Empty)
                {
                    if (lblDec.Text.ToUpper().Trim() == "HYGIENE FACTOR") //Hygiene Factor made enabled false
                    {
                        TextBox txtImportance1 = (TextBox)e.Row.FindControl("txtImportance");
                        txtImportance1.Enabled = false;
                        txtImportance1.BackColor = Color.LightGray;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txttotalImportance = (TextBox)e.Row.FindControl("txtTotalImportance");
                TextBox txttotalScore = (TextBox)e.Row.FindControl("txtTotalScore");
                //txttotalImportance.SetDecimalPrefixSuffix(3, 2, false, " Negative Variation %");
                if (Importance > 0)
                    txttotalImportance.Text = Importance.ToString();
                if (Score > 0)
                    txttotalScore.Text = Score.ToString();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3GOrgGlobalCreditParameter_View.aspx");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            ddlEntityType.SelectedIndex = 0;
            if (ddlConstitution.Items.Count > 0) ddlConstitution.SelectedIndex = 0;
            if (ddlProduct.Items.Count > 0) ddlProduct.SelectedIndex = 0;
            txtExposureVariation.Text = string.Empty;
            txtNegativeVariation.Text = string.Empty;

            GrvGlobalCreditParamUpdate.DataSource = null;
            GrvGlobalCreditParamUpdate.DataBind();
            ViewState["dtTempAuthApprover"] = null;
            ViewState["dtTempAuthApproverPopUp"] = null;
            dtApprovals.Clear();
            ViewState["Approvals"] = null;
            DataRow dr;
            dtApprovals.Columns.Add("SNo");
            dtApprovals.Columns.Add("Facility_From");
            dtApprovals.Columns.Add("GCP_Approval_ID");
            dtApprovals.Columns.Add("Facility_To");
            //dtApprovals.Columns.Add("Number_Of_Approvals");
            //dtApprovals.Columns.Add("Hygiene_Percentage");
            //dtApprovals.Columns.Add("Minimal_Approval_Required");
            dtApprovals.Columns.Add("Range_From", typeof(decimal));
            dtApprovals.Columns.Add("Range_To", typeof(decimal));
            dr = dtApprovals.NewRow();
            dr["SNo"] = 0;
            dtApprovals.Rows.Add(dr);
            ViewState["Approvals"] = dtApprovals;
            grvApprovals.DataSource = dtApprovals;
            grvApprovals.DataBind();
            FunClearZeros();
            grvApprovals.Rows[0].Visible = false;
            ViewState["Approvals"] = dtApprovals;
            //ViewState["Mode"] = "New";
            hidMode.Value = "New";

            btnGo.Enabled = true;
            panelGlobalCredit.Visible = false;
            divCopyProfile.Style.Add("display", "none");
            trCopyProfileMessage.Visible = false;
            btnCopyProfile.Enabled = false;
            btnSave.Style.Add("display", "none");
            btnClear.Style.Add("display", "none");
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void FunProDisableTabs(int intActiveTab)
    {
        try
        {
            for (int intTabIndex = 0; intTabIndex < tcGloblaCreditParameter.Tabs.Count; intTabIndex++)
            {
                if (intActiveTab != intTabIndex)
                {
                    tcGloblaCreditParameter.Tabs[intTabIndex].Enabled = false;
                }
                else
                {
                    tcGloblaCreditParameter.Tabs[intTabIndex].Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //if (GrvGlobalCreditMaster.Rows.Count > 0)
            //{
            //for (int i = 0; i < GrvGlobalCreditMaster.Rows.Count; i++)
            //{
            CheckBox chkGlobalCreditParameter = (CheckBox)sender;//GrvGlobalCreditMaster.Rows[i].FindControl("chkselect");
            GridViewRow gvRow = (GridViewRow)chkGlobalCreditParameter.Parent.Parent;
            if (chkGlobalCreditParameter.Checked)
            {
                Label lblGlobalCreditParameterID = (Label)gvRow.FindControl("lblGlobalCreditParameterID");
                intGlobal_CreditParameterCopyProfileID = Convert.ToInt32(lblGlobalCreditParameterID.Text.Trim());
                hidGridNo.Value = intGlobal_CreditParameterCopyProfileID.ToString();
                //Bind Header Details in GlobalCreditParameter
                ObjCreditMgtServicesClient = new CreditMgtServicesReference.CreditMgtServicesClient();
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParamRow;
                ObjGlobalCreditParamRow = ObjGlobalCreditParameter_DataTable.NewS3G_ORG_Global_Credit_ParameterRow();
                ObjGlobalCreditParamRow.Global_Credit_Parameter_ID = intGlobal_CreditParameterCopyProfileID;
                ObjGlobalCreditParameter_DataTable.AddS3G_ORG_Global_Credit_ParameterRow(ObjGlobalCreditParamRow);
                byte[] bytesObjGlobalCreditParamDatatable = ObjCreditMgtServicesClient.FunPubQueryGlobalCreditParameter(Sermode, ClsPubSerialize.Serialize(ObjGlobalCreditParameter_DataTable, Sermode));
                DsGlobalCreditParameterAll = (DataSet)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParamDatatable, SerializationMode.Binary, typeof(DataSet));

                //Bind Score Details Based on GlobalCreditParamerID
                GrvGlobalCreditMaster.Visible = false;

                panelApproval.Visible = true;  //visible false
                panelGlobalCredit.Visible = true; //visible false
                btnCopyProfile.Text = "Copy Profile";

                //ViewState["Mode"] = "FromProfile";
                hidMode.Value = "FromProfile";

                GrvGlobalCreditParamUpdate.DataSource = DsGlobalCreditParameterAll.Tables[1];
                GrvGlobalCreditParamUpdate.DataBind();
                // FunDisableEmptyParameter();

                txtExposureVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Exposure_Variance"].ToString();
                txtNegativeVariation.Text = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Negative_Deviation"].ToString();
                chkActive.Checked = DsGlobalCreditParameterAll.Tables[0].Rows[0]["Is_Active"].ToString() == "True" ? true : false;

                //Bind Approval Detail Based On GlobalCreditParamerID
                if (DsGlobalCreditParameterAll.Tables[2].Rows.Count > 0)
                {
                    ViewState["Approvals"] = DsGlobalCreditParameterAll.Tables[2];
                    grvApprovals.DataSource = DsGlobalCreditParameterAll.Tables[2];
                    grvApprovals.DataBind();
                    FunClearZeros();



                    TextBox txtaddFacilityToAmt = (TextBox)grvApprovals.Rows[grvApprovals.Rows.Count - 1].FindControl("txtFacilityToAmt");
                    TextBox txtaddFacilityFromAmt = (TextBox)grvApprovals.FooterRow.FindControl("txtaddFacilityFromAmt");
                    txtaddFacilityFromAmt.Text = Convert.ToString(Convert.ToDecimal(txtaddFacilityToAmt.Text.Trim()) + Convert.ToInt32("1"));
                }
                if (DsGlobalCreditParameterAll.Tables[5] != null && DsGlobalCreditParameterAll.Tables[5].Rows.Count > 0)
                {
                    ViewState["dtTempAuthApprover"] = DsGlobalCreditParameterAll.Tables[5];
                }

                if (DsGlobalCreditParameterAll.Tables[6] != null && DsGlobalCreditParameterAll.Tables[6].Rows.Count > 0)
                {
                    grvOtherInfo.DataSource = DsGlobalCreditParameterAll.Tables[6];
                    grvOtherInfo.DataBind();

                    ViewState["dtOthers"] = DsGlobalCreditParameterAll.Tables[6];

                    FunProLoadNumFooterDDL();
                }
                else
                {
                    FunProInitializOtherGrid();
                }

                btnSubmit.Visible = false;
                btnGo.Enabled = true;
                btnCopyProfile.Enabled = true;
                divCopyProfile.Style.Add("display", "none");
                btnSave.Enabled = btnClear.Enabled = true;
                //}
            }
            //}
            ddlLOB.AddItemToolTip();
            ddlProduct.AddItemToolTip();
            ddlConstitution.AddItemToolTip();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, lblHeading.Text);
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            ObjCreditMgtServicesClient.Close();
        }
    }
    #region  "Pop up"

    protected void grvApprover_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            RequiredFieldValidator rfvLocation = (RequiredFieldValidator)e.Row.FindControl("rfvLocation");
            //RequiredFieldValidator rfvApprovalPerson = (RequiredFieldValidator)e.Row.FindControl("rfvApprovalPerson");
            if (ddlEntityType.SelectedValue == "1" || ddlEntityType.SelectedValue == "2")
            {
                grvApprover.Columns[2].Visible = false;
                rfvLocation.Enabled = false;
                //rfvApprovalPerson.Enabled = false;
            }
            else
            {
                grvApprover.Columns[2].Visible = true;
                rfvLocation.Enabled = true;
                //rfvApprovalPerson.Enabled = true;
            }

            FunPriInitializeFooterControls(e);
        }
    }

    private void FunPriInitializeFooterControls(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Dictionary<string, string> procparam;
            DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");
            DropDownList ddlApprovPerson = (DropDownList)e.Row.FindControl("ddlApprovPerson");
            if (ddlEntityType.SelectedValue == "3")//If User
            {
                procparam = new Dictionary<string, string>();
                procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                if (PageMode == PageModes.Create)
                {
                    procparam.Add("@Is_Active", "1");
                }
                procparam.Add("@User_ID", intUserID.ToString());
                procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
                procparam.Add("@Program_ID", "21");
                ddlLocation.BindDataTable(SPNames.BranchMaster_LIST, procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
                ddlLocation.AddItemToolTip();
                ddlLocation.ToolTip = ddlLocation.SelectedItem.Text;
            }
            else
            {
                FunsetApproverPerson(ddlLocation, ddlApprovPerson);
            }

        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLoc = (DropDownList)grvApprover.FooterRow.FindControl("ddlLocation");
        DropDownList ddlApprovPerson = (DropDownList)grvApprover.FooterRow.FindControl("ddlApprovPerson");

        FunsetApproverPerson(ddlLoc, ddlApprovPerson);
    }

    private void FunsetApproverPerson(DropDownList ddlLoc, DropDownList ddlAPPPer)
    {
        Dictionary<string, string> procparam = new Dictionary<string, string>();
        if (ViewState["dtTempAuthApproverPopUp"] != null && ((DataTable)ViewState["dtTempAuthApproverPopUp"]).Rows.Count >= 1)
        {
            int intEntityID = Convert.ToInt32(((DataTable)ViewState["dtTempAuthApproverPopUp"]).Rows[(((DataTable)ViewState["dtTempAuthApproverPopUp"])).Rows.Count - 1]["ApprovPersonID"]);
            if (ddlEntityType.SelectedValue == "1")
                procparam.Add("@Entity_ID", Convert.ToString(intEntityID));
        }
        procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        procparam.Add("@Entity_Type_ID", ddlEntityType.SelectedValue);
        if (ddlLoc.SelectedIndex > 0)
            procparam.Add("@Location_Id", ddlLoc.SelectedValue);
        procparam.Add("@Lob_Id", ddlLOB.SelectedValue);
        ddlAPPPer.BindDataTable("S3G_ORG_GetEntityType", procparam, new string[] { "ID", "Name" });
    }

    protected DataTable FunProInitializePopup(string SNo)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SNo");
        dt.Columns.Add("SequenceNumber");
        dt.Columns.Add("ApprovPerson");
        dt.Columns.Add("ApprovPersonID");
        dt.Columns.Add("Location");
        dt.Columns.Add("LocationID");

        DataRow dRow = dt.NewRow();
        dRow["SNo"] = SNo;
        dRow["SequenceNumber"] = 0;
        dRow["ApprovPerson"] = string.Empty;
        dRow["ApprovPersonID"] = 0;
        dRow["LocationID"] = 0;
        dRow["Location"] = string.Empty;

        dt.Rows.Add(dRow);

        return dt;
        //grvApprover.DataSource = dt;
        //grvApprover.DataBind();
        //grvApprover.Rows[0].Visible = false;
        //dt.Rows.RemoveAt(0);
        //ViewState["dtTempAuthApprover"] = dt;        
    }

    protected void btnDEVModalCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderApprover.Hide();
        FunPriEnableRFVLoc(false);
    }

    protected void btnFApprover_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        string strSLNo = "";
        Label lblSNo = (Label)grvApprovals.Rows[grvApprovals.Rows.Count - 1].FindControl("lblSNo");
        strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();
        if (ViewState["dtTempAuthApprover"] == null)
        {
            dt = FunProInitializePopup(strSLNo);
            grvApprover.DataSource = dt;
            grvApprover.DataBind();
            grvApprover.Rows[0].Visible = false;
            dt.Rows.RemoveAt(0);
            ViewState["dtTempAuthApproverPopUp"] = ViewState["dtTempAuthApprover"] = dt;
        }
        else
        {
            DataTable dt1 = (DataTable)ViewState["dtTempAuthApprover"];
            DataTable dtApprove = new DataTable();
            DataRow[] dRow = dt1.Select("SNo ='" + strSLNo + "'");
            if (dRow.Length > 0)
            {
                dtApprove = dt1.Clone();
                ViewState["dtTempAuthApproverPopUp"] = dtApprove = dt1.Select("SNo ='" + strSLNo + "'").CopyToDataTable();
                grvApprover.DataSource = dtApprove;
                grvApprover.DataBind();
            }
            else
            {
                dt1 = FunProInitializePopup(strSLNo);
                grvApprover.DataSource = dt1;
                grvApprover.DataBind();
                grvApprover.Rows[0].Visible = false;
                dt1.Rows.RemoveAt(0);
                ViewState["dtTempAuthApproverPopUp"] = dt1;
            }
        }
        ModalPopupExtenderApprover.Show();
        FunPriEnableRFVLoc(true);

    }

    protected void btnIApprover_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtApprove = new DataTable();
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        GridView grv = (GridView)gvRow.Parent.Parent;
        dt = (DataTable)ViewState["dtTempAuthApprover"];
        Label lblSNo = (Label)gvRow.FindControl("lblSNo");
        int intSNo = Convert.ToInt32(lblSNo.Text);
        DataRow[] drAuthApprover = dt.Select("SNo='" + intSNo + "'");
        if (drAuthApprover.Length > 0)
        {
            ViewState["dtTempAuthApproverPopUp"] = dtApprove = dt.Select("SNo='" + intSNo + "'").CopyToDataTable();
        }
        grvApprover.DataSource = dtApprove;
        grvApprover.DataBind();
        ModalPopupExtenderApprover.Show();
        FunPriEnableRFVLoc(true);
        if (strMode == "Q")
            if (grvApprover.FooterRow != null)
                grvApprover.FooterRow.Visible = false;
    }

    protected void btnDEVModalAdd_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        DataTable dtModal = new DataTable();
        DataTable dtMain = new DataTable();
        dtModal = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        if (dtModal.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "Add Atlest on Approval Details");
            return;
        }
        dtMain = (DataTable)ViewState["dtTempAuthApprover"];

        int intMasterSeq = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        /*Write Coding To delete Existing record*/

        DataRow[] Drow = dtMain.Select("SNo='" + intMasterSeq + "'");
        foreach (DataRow dr in Drow)
        {
            dtMain.Rows.Remove(dr);
        }

        foreach (GridViewRow GvRow in grvApprover.Rows)
        {
            Label lblSNo = (Label)GvRow.FindControl("lblSNo");
            Label lblSequenceNumber = (Label)GvRow.FindControl("lblSequenceNumber");
            Label lblLocation = (Label)GvRow.FindControl("lblLocation");
            Label lblLocationID = (Label)GvRow.FindControl("lblLocationID");
            Label lblApprovalPerson = (Label)GvRow.FindControl("lblApprovalPerson");
            Label ApprovPersonID = (Label)GvRow.FindControl("ApprovPersonID");
            DataRow dRow = dtMain.NewRow();

            dRow["SNo"] = intMasterSeq;
            dRow["SequenceNumber"] = lblSequenceNumber.Text;
            dRow["ApprovPerson"] = lblApprovalPerson.Text;
            dRow["ApprovPersonID"] = ApprovPersonID.Text;
            dRow["LocationID"] = lblLocationID.Text;
            dRow["Location"] = lblLocation.Text;
            dtMain.Rows.Add(dRow);
        }
        ViewState["dtTempAuthApprover"] = dtMain;
        ViewState["dtTempAuthApproverPopUp"] = null;
        ModalPopupExtenderApprover.Hide();
        FunPriEnableRFVLoc(false);
    }

    private void FunPriEnableRFVLoc(bool BlnFlag)
    {
        if (ddlEntityType.SelectedValue == "3")
        {
            if (grvApprover.FooterRow != null)
            {
                RequiredFieldValidator rfvLocation = (RequiredFieldValidator)grvApprover.FooterRow.FindControl("rfvLocation");
                RequiredFieldValidator rfvApprovalPerson = (RequiredFieldValidator)grvApprover.FooterRow.FindControl("rfvApprovalPerson");
                rfvLocation.Enabled =
                rfvApprovalPerson.Enabled = BlnFlag;
            }
        }
        else
        {
            if (grvApprover.FooterRow != null)
            {
                RequiredFieldValidator rfvApprovalPerson = (RequiredFieldValidator)grvApprover.FooterRow.FindControl("rfvApprovalPerson");
                rfvApprovalPerson.Enabled = BlnFlag;
            }
        }
    }

    protected void btnDetails_Click(object sender, EventArgs e)
    {
        Label lblSequenceNumber = (Label)grvApprover.FooterRow.FindControl("lblSequenceNumber");
        DropDownList ddlLocation = (DropDownList)grvApprover.FooterRow.FindControl("ddlLocation");
        DropDownList ddlApprovPerson = (DropDownList)grvApprover.FooterRow.FindControl("ddlApprovPerson");
        int intSNo = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        DataTable dtModal = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        if (dtModal == null)
        {
            dtModal = (((DataTable)ViewState["dtTempAuthApprover"]).Select("SNo='" + intSNo + "'")).CopyToDataTable();
        }
        DataRow dRow = dtModal.NewRow();
        dRow["SNo"] = Convert.ToInt32(((Label)grvApprover.Rows[0].FindControl("LblSNo")).Text);
        if (ddlEntityType.SelectedValue == "3")
        {
            DataRow[] dArr = (dtModal.Select("SNo='" + intSNo + "' and LocationID='" + ddlLocation.SelectedValue + "' and ApprovPersonID='" + ddlApprovPerson.SelectedValue + "'"));
            if (dArr.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "The Selected user is already added for this location");
                return;
            }
        }

        dRow["ApprovPersonID"] = ddlApprovPerson.SelectedValue;
        dRow["LocationID"] = ddlLocation.SelectedValue;
        dRow["ApprovPerson"] = ddlApprovPerson.SelectedItem.Text;
        if (ddlLocation.SelectedItem != null)
        {
            DataRow[] DRowArr = dtModal.Select("LocationID='" + ddlLocation.SelectedValue + "'");
            if (DRowArr.Length > 0)
            {
                dRow["SequenceNumber"] = DRowArr.Length + 1;
            }
            else
            {
                dRow["SequenceNumber"] = 1;
            }
            dRow["Location"] = Convert.ToString(ddlLocation.SelectedItem.Text);
        }
        else
        {
            if (dtModal.Rows.Count > 0)
            {
                dRow["SequenceNumber"] = dtModal.Rows.Count + 1;
            }
            else
            {
                dRow["SequenceNumber"] = 1;
            }
            dRow["Location"] = "0";
        }
        dtModal.Rows.Add(dRow);
        grvApprover.DataSource = dtModal;
        grvApprover.DataBind();
        ViewState["dtTempAuthApproverPopUp"] = dtModal;
    }

    protected void grvApprover_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtTable = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        Label lblSNo = (Label)grvApprover.Rows[e.RowIndex].FindControl("lblSNo");
        LinkButton btnDelete = (LinkButton)grvApprover.Rows[e.RowIndex].FindControl("btnDelete");
        int intRowIndex = Utility.FunPubGetGridRowID("grvApprover", btnDelete.ClientID.ToString());
        //DropDownList ddlApprovPerson = (DropDownList)grvApprover.Rows[intRowIndex].FindControl("ddlApprovPerson");
        //Label lblSequenceNumber = (Label)grvApprover.Rows[intRowIndex].FindControl("lblSequenceNumber");
        //Label lblSNo = (Label)grvApprover.Rows[intRowIndex].FindControl("lblSNo");
        Label lblLocationID = (Label)grvApprover.Rows[e.RowIndex].FindControl("lblLocationID");
        //DataRow[] DRowArr = dtTable.Select("SNo='" + lblSNo.Text + "' and LocationID='" + lblLocationID.Text + "' and SequenceNumber>'"+lblSequenceNumber.Text+"'");
        dtTable.Rows[e.RowIndex].Delete();
        DataRow[] dArr = dtTable.Select("LocationID='" + lblLocationID.Text + "'");
        if (dArr.Length > 0)
        {
            DataRow[] dt = dtTable.Select("LocationID='" + lblLocationID.Text + "'");
            int intSequenceNumber;
            intSequenceNumber = 1;
            foreach (DataRow dr in dt)
            {
                dr["SequenceNumber"] = intSequenceNumber;
                intSequenceNumber++;
            }
        }
        dtTable.AcceptChanges();
        if (dtTable.Rows.Count == 0)
        {
            dtTable = FunProInitializePopup(lblSNo.Text);
            grvApprover.DataSource = dtTable;
            grvApprover.DataBind();
            dtTable.Rows[0].Delete();
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvApprover.Rows[0].Visible = false;
        }
        else
        {
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvApprover.DataSource = dtTable;
            grvApprover.DataBind();
        }
        //if (DRowArr.Length > 0)
        //{
        //    foreach(DataRow dr in DRowArr)
        //    {
        //        int intSeq = Convert.ToInt32(lblSequenceNumber.Text);
        //        dtTable.Rows[
        //    }
        //}
    }

    #endregion

    protected void FunProInitializOtherGrid()
    {
        DataTable dtOthers = new DataTable();
        dtOthers.Columns.Add("Desc");
        dtOthers.Columns.Add("Flag_ID");
        dtOthers.Columns.Add("Value");
        dtOthers.Columns.Add("Flag");
        dtOthers.Columns.Add("Group_Ref");
        dtOthers.Columns.Add("Remarks");
        dtOthers.Columns.Add("RecordId");

        DataRow dRow = dtOthers.NewRow();
        dtOthers.Rows.Add(dRow);

        grvOtherInfo.DataSource = dtOthers;
        grvOtherInfo.DataBind();

        grvOtherInfo.Rows[0].Visible = false;
        dtOthers.Rows.Clear();

        ViewState["dtOthers"] = dtOthers;

        if (PageMode != PageModes.Query)
        {
            FunProLoadNumFooterDDL();
        }
    }

    protected void FunProLoadNumFooterDDL()
    {
        DropDownList ddlFFlag = (DropDownList)grvOtherInfo.FooterRow.FindControl("ddlFFlag");

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Is_Number", "0");

        ddlFFlag.BindDataTable("S3G_ORG_GetCreidtScoreFlags", Procparam, new string[] { "Flag_ID", "Flag_Code" });
    }

    protected void ddlFFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFFlag = ((DropDownList)sender);
        AjaxControlToolkit.ComboBox txtFDesc = (AjaxControlToolkit.ComboBox)grvOtherInfo.FooterRow.FindControl("txtFDesc");
        Label lblFDesc = (Label)grvOtherInfo.FooterRow.FindControl("lblFDesc");

        lblFDesc.Text = "";
        txtFDesc.SelectedIndex = -1;
        txtFDesc.Visible = false;

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Is_Number", "0");

        if (ddlFFlag.SelectedValue == "5")
        {
            Procparam.Add("@Is_Other", "1");
            txtFDesc.Visible = true;
            lblFDesc.Visible = false;

            txtFDesc.BindDataTable("S3G_ORG_GetCreidtScoreFlags", Procparam, new string[] { "Flag_ID", "Description" });
        }
        else
        {
            Procparam.Add("@Flag_ID", ddlFFlag.SelectedValue);
            DataTable dtDesc = Utility.GetDefaultData("S3G_ORG_GetCreidtScoreFlags", Procparam);
            if (dtDesc != null && dtDesc.Rows.Count > 0)
            {
                lblFDesc.Text = dtDesc.Rows[0]["Description"].ToString();
                txtFDesc.Visible = false;
                lblFDesc.Visible = true;
            }
        }
    }

    protected void grvOtherInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grvOtherInfo_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            DataTable dtOthers = (DataTable)ViewState["dtOthers"];
            DataRow dRow = dtOthers.NewRow();

            DropDownList ddlFFlag = (DropDownList)grvOtherInfo.FooterRow.FindControl("ddlFFlag");
            AjaxControlToolkit.ComboBox txtFDesc = (AjaxControlToolkit.ComboBox)grvOtherInfo.FooterRow.FindControl("txtFDesc");
            TextBox txtFGroupRef = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFGroupRef");
            TextBox txtFRemarks = (TextBox)grvOtherInfo.FooterRow.FindControl("txtFRemarks");
            Label lblFDesc = (Label)grvOtherInfo.FooterRow.FindControl("lblFDesc");

            if (ddlFFlag.SelectedValue == "5")
            {
                dRow["Desc"] = txtFDesc.SelectedItem.Text.Trim();
            }
            else
            {
                dRow["Desc"] = lblFDesc.Text.Trim();
            }

            DataRow[] drDup = dtOthers.Select("Desc='" + dRow["Desc"].ToString() + "' AND Group_Ref='" + txtFGroupRef.Text.Trim() + "'");
            if (drDup.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Given combination already entered");
                return;
            }

            dRow["Flag_ID"] = ddlFFlag.SelectedValue;
            dRow["Flag"] = ddlFFlag.SelectedItem.Text;
            dRow["Group_Ref"] = txtFGroupRef.Text;
            dRow["Remarks"] = txtFRemarks.Text;

            dtOthers.Rows.Add(dRow);

            for (int i = 0; i <= dtOthers.Rows.Count - 1; i++)
            {
                dtOthers.Rows[i]["RecordId"] = i.ToString();
            }

            grvOtherInfo.DataSource = dtOthers;
            grvOtherInfo.DataBind();

            ViewState["dtOthers"] = dtOthers;

            FunProLoadNumFooterDDL();

        }
    }

    protected void grvOtherInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtOthers = (DataTable)ViewState["dtOthers"];
        dtOthers.Rows.RemoveAt(e.RowIndex);

        grvOtherInfo.DataSource = dtOthers;
        grvOtherInfo.DataBind();

        for (int i = 0; i <= dtOthers.Rows.Count - 1; i++)
        {
            dtOthers.Rows[i]["RecordId"] = i.ToString();
        }

        ViewState["dtOthers"] = dtOthers;

        if (dtOthers.Rows.Count == 0)
        {
            FunProInitializOtherGrid();
        }
        else
        {
            FunProLoadNumFooterDDL();
        }
    }
    protected void rdbLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbLevel.SelectedValue.ToString() == "1")
        {
            ddlLOB.Enabled = ddlConstitution.Enabled = ddlProduct.Enabled = true;
            RequiredFieldValidator2.Enabled = RequiredFieldValidator1.Enabled = true;
            GrvGlobalCreditParamUpdate.DataSource = null;
            GrvGlobalCreditParamUpdate.DataBind();
            panelGlobalCredit.Visible = false;
        }
        else
        {
            ddlLOB.SelectedValue = "0";
            RequiredFieldValidator2.Enabled = RequiredFieldValidator1.Enabled = false;
            ddlProduct.Items.Clear();
            ddlConstitution.Items.Clear();
            ddlLOB.Enabled = ddlConstitution.Enabled = ddlProduct.Enabled = false;
            GrvGlobalCreditParamUpdate.DataSource = null;
            GrvGlobalCreditParamUpdate.DataBind();
            panelGlobalCredit.Visible = false;
        }
    }
}

