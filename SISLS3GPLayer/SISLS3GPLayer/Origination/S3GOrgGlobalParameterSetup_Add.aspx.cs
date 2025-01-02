#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Global Parameter Setup Org
/// Created By			: Kannan RC
/// Created Date		: 10-July-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 03-Aug-2010   
/// <Program Summary>
#endregion

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Configuration;


public partial class Origination_S3GOrgGlobalParameterSetup : ApplyThemeForProject
{

    #region Intialization
    int intErrCode = 0;
    int intGlobalParamId = 0;
    int intLOBID = 0;
    int intCompanyId;
    int intUserId;
    string strMode;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    static bool boolGPSDone = true;
    bool bClearList = false;
       
    OrgMasterMgtServicesReference.OrgMasterMgtServicesClient objGlobalSetup_MasterClient;
    //OrgMasterMgtServices.S3G_Global_LookUPDataTable ObjS3G_GlobalLookUpList = new OrgMasterMgtServices.S3G_Global_LookUPDataTable();
    //OrgMasterMgtServices.S3G_Global_LookUPDataTable ObjS3G_GlobalLookUpListRow;
    OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable ObjS3G_GlobalProgram;// = new OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable();

    OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable ObjS3G_GlobalParameterMaster;// = new OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable();

    OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable ObjS3G_GlobalParameterROIMaster;// (not used) = new OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable();

    OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable ObjS3G_GlobalParameterLOBMaster;// = new OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable();

    OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable ObjS3G_GlobalParameterIRR;//(not used) = new OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable();

    OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable ObjS3G_GlobalParameterSetupMaster;// = new OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable();

    OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable ObjS3G_GlobalParameterLOB;// = new OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable();
    OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable ObjS3G_GlobalParameterROI;// = new OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable();

    bool bChkBox = false;
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
       
        //tcGlobal.ActiveTabIndex = 0;
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        btnCancel.Visible = false;
        intGlobalParamId = Convert.ToInt32(hdnGlobalParamId.Value);
        if (!IsPostBack)
        {
            FunPriGetLookUpList();
            if (!bIsActive)
            {
                btnSave.Enabled = false;
                btnClear.Enabled = false;
            }
            else
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                FunGetGlobalSetupDetatils();
                intGlobalParamId = Convert.ToInt32(hdnGlobalParamId.Value);
                if (boolGPSDone)
                {
                    if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(hdnUserId.Value), Convert.ToInt32(hdnUserLevelId.Value))))
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
                //if (!bModify)
                //{
                //    FunPriDisableControls(-1);
                //}
            }
            if (strMode == "M")
            {
                FunPriDisableControls(1);
            }
            else if (strMode == "Q")
            {
                FunPriDisableControls(-1);
            }
            //else
            //{
            //    FunPriDisableControls(0);
            //}


        }
        FunPriSetMaxLength();
    }


    #region DDL
    private void FunPriGetLookUpList()
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Option", "1");
        dictParam.Add("@Param1", "ORG_ROI_RULES_RESIDUAL_VALUE");
        ddlModify.BindGlobalDataTable("S3G_ORG_GetGlobalLookUp", dictParam, new string[] { "Value", "Name" });
        ddlIRR.BindGlobalDataTable("S3G_ORG_GetGlobalLookUp", dictParam, new string[] { "Value", "Name" });
        ddlAccount.BindGlobalDataTable("S3G_ORG_GetGlobalLookUp", dictParam, new string[] { "Value", "Name" });
    }
    #endregion

    #region Get ProgramID-ROI Tab

    /// <summary>
    /// This methode used in Get Global Program details
    /// </summary>
    private void FunPriGetGlobalProgram()
    {
        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_GlobalProgram = new OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable();
            OrgMasterMgtServices.S3G_ORG_GetGlobalProgramRow ObjGlobalProgramRow;
            ObjGlobalProgramRow = ObjS3G_GlobalProgram.NewS3G_ORG_GetGlobalProgramRow();
            ObjS3G_GlobalProgram.AddS3G_ORG_GetGlobalProgramRow(ObjGlobalProgramRow);

            // objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteGlobalProgram = objGlobalSetup_MasterClient.FunPubGetGlobalProgram(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalProgram, SerMode));
            //OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable dtGlobalProgram = (OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable)ClsPubSerialize.DeSerialize(byteGlobalProgram, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_GetGlobalProgramDataTable));
            //gvProgram.DataSource = dtGlobalProgram;
            //gvProgram.DataBind();
            //objGlobalSetup_MasterClient.Close();
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }
    }
    #endregion

    /// <summary>
    /// This methode used in Create and Modify Global Parameter details
    /// </summary>
    #region Create GPS
    protected void btnSave_Click(object sender, EventArgs e)
    {


        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        ObjS3G_GlobalParameterMaster = new OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupDataTable();
        ObjS3G_GlobalParameterIRR = new OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBDataTable();
        ObjS3G_GlobalParameterROIMaster = new OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRuleDataTable();
        try
        {
            string strKey = "Insert";
            string strAlert = "alert('__ALERT__');";
            //string strRedirectPageView = "window.location.href='../Origination/S3GOrgGlobalParameterSetup_Add.aspx'";
            string strRedirectPageView = "window.location.href='../Common/HomePage.aspx'";
            string strRedirectPageAdd = "window.location.href='../Origination/S3GOrgGlobalParameterSetup_Add.aspx';";


            int count = 0;

            //Condition to validate process tab check box validation - Bug_ID-5511- Kuppusamy.B - 21-Feb-2012
            FunPriValidateCheckBox();
            if (bChkBox == false)
            {
                strAlert = strAlert.Replace("__ALERT__", "Select atleast one Starting point of the Process");
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                lblErrorMessage.Text = string.Empty;
                tcGlobal.ActiveTabIndex = 3;
                return;
            }

            //foreach (GridViewRow grv in gvIRR.Rows)
            //{
            //    if (((CheckBox)grv.FindControl("CbxIRR")).Checked)
            //    {

            //        count++;
            //    }

            //}
            //if (count == 0)
            //{
            //    cvGlobal.ErrorMessage = "Select IRR for atleast one Line of Business - IRR Details Tab";
            //    cvGlobal.IsValid = false;
            //    return;
            //}

            if (txtAmount.Text.Length > 0)
            {
                char charFirst = Convert.ToChar(txtAmount.Text.Substring(0, 1).ToLower());
                if (char.IsDigit(charFirst) && Convert.ToInt32(charFirst) == 48)
                {
                    strAlert = strAlert.Replace("__ALERT__", "Currency amount should not start with zero.");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                    lblErrorMessage.Text = string.Empty;
                    return;
                }
            }


            if (Convert.ToDouble(txtAmount.Text) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Invalid Currency Amount Round Off Function');", true);
                txtAmount.Text = "";
                txtAmount.Focus();
                return;

            }
            //foreach (GridViewRow grv in gvIRR.Rows)
            //{
            //    if (((CheckBox)grv.FindControl("CbxIRR")).Checked)
            //    {
            //        TextBox txttax1 = grv.FindControl("txtCTR") as TextBox;
            //        TextBox txtplr2 = grv.FindControl("txtPLR") as TextBox;
            //        //if (txttax1.Text.Length > 0)
            //        //{
            //        //    char charFirst = Convert.ToChar(txttax1.Text.Substring(0, 1).ToLower());
            //        //    if (char.IsDigit(charFirst) && Convert.ToInt32(charFirst) == 48)
            //        //    {
            //        //        strAlert = strAlert.Replace("__ALERT__", "CTR should not start with 0.");
            //        //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            //        //        lblErrorMessage.Text = string.Empty;
            //        //        return;
            //        //    }
            //        //}
            //        //if (txtplr2.Text.Length > 0)
            //        //{
            //        //    char charFirst = Convert.ToChar(txtplr2.Text.Substring(0, 1).ToLower());
            //        //    if (char.IsDigit(charFirst) && Convert.ToInt32(charFirst) == 48)
            //        //    {
            //        //        strAlert = strAlert.Replace("__ALERT__", "PLR should not start with 0.");
            //        //        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            //        //        lblErrorMessage.Text = string.Empty;
            //        //        return;
            //        //    }
            //        //}
            //        if (txttax1.Text.Length > 0)
            //        {
            //            if (Convert.ToDecimal(txttax1.Text) > Convert.ToDecimal(100.0000))
            //            {
            //                strAlert = strAlert.Replace("__ALERT__", "CTR % should not be greater than 100%.");
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            //                return;
            //            }
            //        }
            //        if (txtplr2.Text.Length > 0)
            //        {
            //            if (Convert.ToDecimal(txtplr2.Text) > Convert.ToDecimal(100.0000))
            //            {
            //                strAlert = strAlert.Replace("__ALERT__", "PLR % should not be greater than 100%.");
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            //                return;
            //            }
            //        }
            //    }
            //}

            //foreach (GridViewRow grv in gvIRR.Rows)
            //{
            //    if (((CheckBox)grv.FindControl("CbxIRR")).Checked)
            //    {
            //        TextBox txttax1 = grv.FindControl("txtCTR") as TextBox;
            //        TextBox txtplr2 = grv.FindControl("txtPLR") as TextBox;
            //        DropDownList ddlvoice = grv.FindControl("ddlvoice") as DropDownList;
            //        DropDownList ddldesc = grv.FindControl("ddlDesc") as DropDownList;
            //        if (txttax1.Text.Length == 0 & txtplr2.Text.Length == 0 & ddldesc.SelectedItem.Text == "--Select--")
            //        {
            //            //lblErrorMessage.Text = "Enter the IRR details";
            //            cvGlobal.ErrorMessage = "Enter the IRR details";
            //            cvGlobal.IsValid = false;
            //            return;
            //        }
            //        //if (txttax1.Text.Length == 0)
            //        //{
            //        //    //lblErrorMessage.Text = "Enter Corporate Rate Tax";
            //        //    cvGlobal.ErrorMessage = "Enter Corporate Tax Rate(CTR)";
            //        //    cvGlobal.IsValid = false;
            //        //    return;
            //        //}

            //        if (txtplr2.Text.Length == 0)
            //        {
            //            //lblErrorMessage.Text = "Enter Prime Lending Rate";
            //            cvGlobal.ErrorMessage = "Enter Prime Lending Rate(PLR) - IRR Details Tab";
            //            cvGlobal.IsValid = false;
            //            return;
            //        }
            //        //if (ddlvoice.SelectedValue == "0")
            //        //{
            //        //    //lblErrorMessage.Text = "Select the Invoice";
            //        //    cvGlobal.ErrorMessage = "Select a Invoice Required - IRR Details Tab";
            //        //    cvGlobal.IsValid = false;
            //        //    return;
            //        //}
            //        if (ddldesc.SelectedItem.Text == "--Select--")
            //        {
            //            //lblErrorMessage.Text = "Select the Deprection";
            //            cvGlobal.ErrorMessage = "Select a Depreciation Method - IRR Details Tab";
            //            cvGlobal.IsValid = false;
            //            return;
            //        }

            //    }

            //}


            int counts = 0;
            //foreach (GridViewRow grv in gvProgram.Rows)
            //{
            //    if (((CheckBox)grv.FindControl("CbxProgram")).Checked)
            //    {
            //        counts++;
            //    }

            //}
            //if (counts == 0)
            //{
            //    cvGlobal.ErrorMessage = "Select atleast one ROI Rule details - ROI Rule Tab.";
            //    cvGlobal.IsValid = false;
            //    return;
            //}

            foreach (GridViewRow grv in gvGeneral.Rows)
            {
                DropDownList ddlCashierAc = (DropDownList)grv.FindControl("ddlCashierAc");
                if (ddlCashierAc.SelectedValue == "0")
                {
                    cvGlobal.ErrorMessage = "Select Cashier Account in General Tab";
                    cvGlobal.IsValid = false;
                    return;
                }
            }


            OrgMasterMgtServices.S3G_ORG_GlobalParameterSetupRow ObjGlobalParameterMasterRow;
            ObjGlobalParameterMasterRow = ObjS3G_GlobalParameterMaster.NewS3G_ORG_GlobalParameterSetupRow();
            ObjGlobalParameterMasterRow.Global_Parameter_Org_ID = intGlobalParamId;
            ObjGlobalParameterMasterRow.Company_ID = Convert.ToInt32(intCompanyId);
            ObjGlobalParameterMasterRow.Modification = ddlModify.SelectedValue == "1" ? true : false;
            ObjGlobalParameterMasterRow.Offer_Number = CbxOfferNo.Checked;
            ObjGlobalParameterMasterRow.Enquiry_Number = CbxEnquiryNo.Checked;
            ObjGlobalParameterMasterRow.Application_Number = CbxApplicationNo.Checked;

            ObjGlobalParameterMasterRow.NegativeVariance = chkNegative.Checked;

            ObjGlobalParameterMasterRow.IRR_Details = ddlIRR.SelectedValue == "1" ? true : false;
            ObjGlobalParameterMasterRow.Amount = Convert.ToDecimal(txtAmount.Text);
            ObjGlobalParameterMasterRow.Create_Account = ddlAccount.SelectedValue == "1" ? true : false; ;
            ObjGlobalParameterMasterRow.XML_GLOBDeltails = FunProFormLOBXML();
            ObjGlobalParameterMasterRow.XML_GROIDeltails = FunProFormROIXML();
            ObjGlobalParameterMasterRow.XML_CashierAccounts = FunProFormLOBCashierXML();

            ObjGlobalParameterMasterRow.Product_Inflow_Adjust = cbxProductInflow.Checked;

            ObjGlobalParameterMasterRow.Created_By = Convert.ToInt32(intUserId);
            ObjGlobalParameterMasterRow.Created_On = DateTime.Now;
            ObjGlobalParameterMasterRow.Modified_By = Convert.ToInt32(intUserId);
            ObjS3G_GlobalParameterMaster.AddS3G_ORG_GlobalParameterSetupRow(ObjGlobalParameterMasterRow);

            SerializationMode SerMode = SerializationMode.Binary;
            // objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            if (intGlobalParamId > 0)
            {
                intErrCode = objGlobalSetup_MasterClient.FunPubUpdateGlobalParameter(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterMaster, SerMode));
            }
            else
            {
                intErrCode = objGlobalSetup_MasterClient.FunPubCreateGlobalParameter(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterMaster, SerMode));


            }

            if (intErrCode == 0)
            {
                //Added by Thangam M on 18/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here

                if (intGlobalParamId > 0)
                {
                    strKey = "Modify";
                    strAlert = strAlert.Replace("__ALERT__", "Global parameter setup updated successfully");
                }
                else
                {
                    strAlert = "Global parameter setup added successfully";
                    strAlert += @"\n\nWould you like to Modify the record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                }
            }
            //else if (intErrCode == 1)
            //{
            //    strAlert = strAlert.Replace("__ALERT__", "Cashflow already exists in the system. Kindly enter a new Cashflow");
            //    strRedirectPageView = "";
            //}

            //else if (intErrCode == 2)
            //{
            //    strAlert = strAlert.Replace("__ALERT__", "Program Ref Number already exists in the system. Kindly enter a new Program Ref Number");
            //    strRedirectPageView = "";
            //}
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
            lblErrorMessage.Text = string.Empty;
            //objGlobalSetup_MasterClient.Close();

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }
    }
    #endregion

    /// <summary>
    /// This methode used in XML Form in Global Parameter details    
    /// </summary>
    #region XML
    protected string FunProFormLOBXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("");
        //foreach (GridViewRow grvData in gvIRR.Rows)
        //{

        //    string strlblLOBID = ((Label)grvData.FindControl("lblLobID")).Text;
        //    string strInvoice = ((DropDownList)grvData.FindControl("ddlvoice")).SelectedValue.ToString();
        //    string strDesc = ((DropDownList)grvData.FindControl("ddlDesc")).SelectedValue.ToString();
        //    string strCTR = ((TextBox)grvData.FindControl("txtCTR")).Text;
        //    string strPLR = ((TextBox)grvData.FindControl("txtPLR")).Text;
        //    if (strCTR == "")
        //    {
        //        strCTR = "-1";
        //    }
        //    if (strPLR == "")
        //    {
        //        strPLR = "-1";
        //    }
        //    //if (strInvoice == "0")
        //    //{
        //    //    strInvoice = "NULL";
        //    //}

        //    string strISLOB = Convert.ToString(((CheckBox)grvData.FindControl("CbxIRR")).Checked);
        //    strbuXML.Append(" <Details  LOB_ID='" + strlblLOBID + "' Invoice_Required='" + strInvoice.ToString() + "' Depreciation_Method='" + strDesc.ToString() +
        //     "' Corporate_Tax_Rate='" + strCTR.ToString() + "' Prime_Lending_Rate='" + strPLR.ToString() + "' IS_LOB='" + strISLOB.ToString() + "'/>");
        //}
        //strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }


    protected string FunProFormROIXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("");
        //foreach (GridViewRow grvData in gvProgram.Rows)
        //{

        //    string strlblProgramID = ((Label)grvData.FindControl("lblProgramid")).Text;
        //    string strISProgram = Convert.ToString(((CheckBox)grvData.FindControl("CbxProgram")).Checked);

        //    strbuXML.Append(" <Details  Program_ID='" + strlblProgramID + "' IS_Program='" + strISProgram.ToString() + "'/>");
        //}
        //strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }

    protected string FunProFormLOBCashierXML()
    {
        StringBuilder strbuXML = new StringBuilder();
        strbuXML.Append("<Root>");
        foreach (GridViewRow grvData in gvGeneral.Rows)
        {

            string strlblLOBID = ((Label)grvData.FindControl("lblLOBID")).Text;
            string strCashierAc = ((DropDownList)grvData.FindControl("ddlCashierAc")).SelectedValue.ToString();


            strbuXML.Append(" <Details  LOB_ID='" + strlblLOBID + "' Cashier_Account='" + strCashierAc.ToString() + "'/>");
        }
        strbuXML.Append("</Root>");
        return strbuXML.ToString();
    }
    #endregion


    /// <summary>
    /// This methode used in LOB Details    
    /// </summary>
    #region GetLLobName
    private void FunPriGetGlobalSetupLOB()
    {
        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

        try
        {
            ObjS3G_GlobalParameterLOBMaster = new OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable();
            OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameRow ObjGlobalProgramLOBRow;
            ObjGlobalProgramLOBRow = ObjS3G_GlobalParameterLOBMaster.NewS3G_ORG_GlobalParameterLOBNameRow();
            ObjGlobalProgramLOBRow.Company_ID = intCompanyId;
            ObjGlobalProgramLOBRow.User_ID = intUserId;
            ObjGlobalProgramLOBRow.Is_Active = true;
            ObjS3G_GlobalParameterLOBMaster.AddS3G_ORG_GlobalParameterLOBNameRow(ObjGlobalProgramLOBRow);

            //objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();

            //SerializationMode SerMode = SerializationMode.Binary;
            //byte[] byteGlobalLOBProgram = objGlobalSetup_MasterClient.FunPubGetGlobalProgramLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterLOBMaster, SerMode));
            //OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable dtGlobalProgramLOB = (OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable)ClsPubSerialize.DeSerialize(byteGlobalLOBProgram, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_GlobalParameterLOBNameDataTable));
            //gvIRR.DataSource = dtGlobalProgramLOB;
            //gvIRR.DataBind();
            // objGlobalSetup_MasterClient.Close();


        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }
    }
    #endregion


    //protected void gvIRR_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //OrgMasterMgtServices.S3G_Global_LookUPDataTable ObjS3G_GlobalLookUpList = new OrgMasterMgtServices.S3G_Global_LookUPDataTable();
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DropDownList ddl = ((DropDownList)(e.Row.Cells[1].FindControl("ddlvoice")));
    //        DropDownList ddl1 = ((DropDownList)(e.Row.Cells[2].FindControl("ddlDesc")));

    //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
    //        dictParam.Add("@Option", "1");
    //        dictParam.Add("@Param1", "Global Parameter");
    //        ddl.BindDataTable("S3G_ORG_GetGlobalLookUp", dictParam, new string[] { "Value", "Name" });

    //        Dictionary<string, string> dictParam1 = new Dictionary<string, string>();
    //        dictParam1.Add("@Option", "2");
    //        dictParam1.Add("@Param1", "ORG_Global_PARAM");
    //        ddl1.BindDataTable("S3G_ORG_GetGlobalLookUp", dictParam1, new string[] { "Value", "Name" });


    //        TextBox txttax = (TextBox)e.Row.FindControl("txtCTR");
    //        TextBox txtplr = (TextBox)e.Row.FindControl("txtPLR");

    //        Label lblLob = (Label)e.Row.FindControl("lblLob");
    //        ddl1.Enabled = FunPubCheckDepreciationForLOB(lblLob.Text.Trim());
    //        //txttax.Attributes.Add("onblur", "fnCalculateMarginPercentage('" + txttax.ClientID + "' );");
    //        //txtplr.Attributes.Add("onblur", "fnCalculateMarginPercentage('" + txtplr.ClientID + "' );");


    //    }
    //    //if (e.Row.RowType == DataControlRowType.DataRow)
    //    //{
    //    //    CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxIRR");
    //    //    DropDownList Chebox1 = (DropDownList)e.Row.FindControl("ddlvoice");
    //    //    DropDownList Chebox2 = (DropDownList)e.Row.FindControl("ddlDesc");
    //    //    TextBox Chebox3 = (TextBox)e.Row.FindControl("txtCTR");
    //    //    TextBox Chebox4 = (TextBox)e.Row.FindControl("txtPLR");

    //    //    if (Convert.ToString(bQuery) == "True")
    //    //    {
    //    //        Chebox.Enabled = false;
    //    //        Chebox1.Enabled = false;
    //    //        Chebox2.Enabled = false;
    //    //        Chebox3.ReadOnly = true;
    //    //        Chebox4.ReadOnly = true;
    //    //    }



    //    //}


    //}

    /// <summary>
    /// Created by Tamilselvan.S 
    /// Created Date 10/11/2011
    /// For Hire Purchase, Loan, Term loan, Term Loan Extensible and Working 
    /// capital LOB'S Depreciation mothod's are Deactive mode as per BA Team 
    /// Only applicable HP,LN,FL,OL
    /// </summary>
    /// <param name="strLOBCode"></param>
    /// <returns></returns>
    public bool FunPubCheckDepreciationForLOB(string strLOBCode)
    {
        if ((strLOBCode.Trim().Split('-')[0]).Contains("TL"))
            return false;
        else if ((strLOBCode.Trim().Split('-')[0]).Contains("TE"))
            return false;
        else if ((strLOBCode.Trim().Split('-')[0]).Contains("WC"))
            return false;
        //else if ((strLOBCode.Trim().Split('-')[0]).Contains("FT")) //AS Per Malolan mail Date 16/11/2011
        //    return false;
        else
            return true;
    }

    /// <summary>
    /// This methode used in Get Global Setup Parameter Details    
    /// </summary>
    #region GetGPS
    private void FunGetGlobalSetupDetatils()
    {
        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_GlobalParameterSetupMaster = new OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable();
            OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterRow ObjGlobalMasterRow;
            ObjGlobalMasterRow = ObjS3G_GlobalParameterSetupMaster.NewS3G_ORG_GetGlobalParameterMasterRow();
            ObjGlobalMasterRow.Global_Parameter_Org_ID = 0;
            ObjGlobalMasterRow.Company_ID = intCompanyId;
            ObjS3G_GlobalParameterSetupMaster.AddS3G_ORG_GetGlobalParameterMasterRow(ObjGlobalMasterRow);
            // objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteGlobalDetails = objGlobalSetup_MasterClient.FunPubGetGlobalMaster(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterSetupMaster, SerMode));
            ObjS3G_GlobalParameterSetupMaster = (OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable)ClsPubSerialize.DeSerialize(byteGlobalDetails, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_GetGlobalParameterMasterDataTable));
            if (ObjS3G_GlobalParameterSetupMaster.Rows.Count > 0)
            {
                boolGPSDone = true;
                FunPriGetLookUpList();
                FunPriGetGlobalProgram();
                FunPriGetGlobalSetupLOB();                
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                ddlModify.SelectedValue = ObjS3G_GlobalParameterSetupMaster.Rows[0]["Modification"].ToString() == "True" ? "1" : "0";
                ddlIRR.SelectedValue = ObjS3G_GlobalParameterSetupMaster.Rows[0]["IRR_Details"].ToString() == "True" ? "1" : "0";

                if (ObjS3G_GlobalParameterSetupMaster.Rows[0]["Enquiry_Number"].ToString() == "True")
                    CbxEnquiryNo.Checked = true;
                else
                    CbxEnquiryNo.Checked = false;
                if (ObjS3G_GlobalParameterSetupMaster.Rows[0]["Offer_Number"].ToString() == "True")
                    CbxOfferNo.Checked = true;
                else
                    CbxOfferNo.Checked = false;
                if (ObjS3G_GlobalParameterSetupMaster.Rows[0]["Application_Number"].ToString() == "True")
                    CbxApplicationNo.Checked = true;
                else
                    CbxApplicationNo.Checked = false;
                if (ObjS3G_GlobalParameterSetupMaster.Rows[0]["CSG_Negative"].ToString() == "True")
                    chkNegative.Checked = true;
                else
                    chkNegative.Checked = false;
                if (ObjS3G_GlobalParameterSetupMaster.Rows[0]["Product_Inflow_Adjust"].ToString() == "True")
                    cbxProductInflow.Checked = true;
                else
                    cbxProductInflow.Checked = false;
                hdnUserId.Value = ObjS3G_GlobalParameterSetupMaster.Rows[0]["Created_By"].ToString();
                hdnUserLevelId.Value = ObjS3G_GlobalParameterSetupMaster.Rows[0]["User_Level_ID"].ToString();
                hdnGlobalParamId.Value = ObjS3G_GlobalParameterSetupMaster.Rows[0]["Global_Parameter_Org_ID"].ToString();
                txtAmount.Text = ObjS3G_GlobalParameterSetupMaster.Rows[0]["Amount"].ToString();
                ddlAccount.SelectedValue = ObjS3G_GlobalParameterSetupMaster.Rows[0]["Create_Account"].ToString() == "True" ? "1" : "0";
                boolGPSDone = true;
                FunGetGlobalLOBDetatils();
                FunGetGlobalROIDetatils();
                FunPriGetLOBCashierAccount();

            }
            else
            {

                FunPriDisableControls(0);

            }

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }
    }


    /// <summary>
    /// This methode used in LOB Details in Modify Mode
    /// </summary>
    private void FunGetGlobalLOBDetatils()
    {
        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_GlobalParameterLOB = new OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable();
            OrgMasterMgtServices.S3G_ORG_GlobalLOBRow ObjGlobalMasterLOBRow;
            ObjGlobalMasterLOBRow = ObjS3G_GlobalParameterLOB.NewS3G_ORG_GlobalLOBRow();
            ObjGlobalMasterLOBRow.Company_ID = intCompanyId.ToString();
            ObjGlobalMasterLOBRow.User_ID = intUserId.ToString();
            ObjS3G_GlobalParameterLOB.AddS3G_ORG_GlobalLOBRow(ObjGlobalMasterLOBRow);
            //objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteGlobalLOBDetails = objGlobalSetup_MasterClient.FunPubGetGlobalLOB(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterLOB, SerMode));
            ObjS3G_GlobalParameterLOB = (OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable)ClsPubSerialize.DeSerialize(byteGlobalLOBDetails, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_GlobalLOBDataTable));
            //for (int i = 0; i < gvIRR.Rows.Count; i++)
            //{

            //    if (gvIRR.Rows[i].RowType == DataControlRowType.DataRow)
            //    {

            //        int LOB_ID = Convert.ToInt32(gvIRR.DataKeys[i]["LOB_ID"].ToString());
            //        CheckBox Cbx1 = (CheckBox)gvIRR.Rows[i].FindControl("CbxIRR");
            //        DropDownList ddlvoice = (DropDownList)gvIRR.Rows[i].FindControl("ddlvoice");
            //        DropDownList ddldesc = (DropDownList)gvIRR.Rows[i].FindControl("ddlDesc");
            //        TextBox txtTax = (TextBox)gvIRR.Rows[i].FindControl("txtCTR");
            //        TextBox txtPLM = (TextBox)gvIRR.Rows[i].FindControl("txtPLR");
            //        Cbx1.Checked = false;
            //        for (int j = 0; j <= ObjS3G_GlobalParameterLOB.Rows.Count - 1; j++)
            //        {
            //            if (LOB_ID == Convert.ToInt32(ObjS3G_GlobalParameterLOB.Rows[j]["LOB_ID"].ToString()))
            //            {
            //                if (ObjS3G_GlobalParameterLOB.Rows[j]["LOB_Assign"].ToString() == "True")
            //                    Cbx1.Checked = true;
            //                else
            //                    Cbx1.Checked = false;


            //                if (ObjS3G_GlobalParameterLOB.Rows[j]["Invoice_Required"].ToString() == "1")
            //                {
            //                    ddlvoice.SelectedValue = "1";
            //                }
            //                else if (ObjS3G_GlobalParameterLOB.Rows[j]["Invoice_Required"].ToString() == "2")
            //                {
            //                    ddlvoice.SelectedValue = "2";
            //                }
            //                else
            //                {
            //                    ddlvoice.SelectedValue = "0";
            //                }

            //                //ddlvoice.SelectedValue = ObjS3G_GlobalParameterLOB.Rows[j]["Invoice_Required"].ToString() == "True" ? "1" : "0";
            //                int intDec = Convert.ToInt32(ObjS3G_GlobalParameterLOB.Rows[j]["Currency_Max_Dec_Digit"]);
            //                ddldesc.SelectedValue = Convert.ToString(ObjS3G_GlobalParameterLOB.Rows[j]["Depreciation_Method"]);
            //                txtTax.Text = Convert.ToString(ObjS3G_GlobalParameterLOB.Rows[j]["Corporate_Tax_Rate"]);
            //                txtPLM.Text = Convert.ToString(ObjS3G_GlobalParameterLOB.Rows[j]["Prime_Lending_Rate"]);
            //                //if (intDec == 1)
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {
            //                //        string str1 = txtPLM.Text.Substring(0, txtPLM.Text.Length - 3);
            //                //        txtPLM.Text = str1;
            //                //    }
            //                //    if (txtTax.Text.Trim() != "")
            //                //    {
            //                //        string str = txtTax.Text.Substring(0, txtTax.Text.Length - 3);
            //                //        txtTax.Text = str;
            //                //    }
            //                //}
            //                //if (intDec == 2)
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {

            //                //        string str1 = txtPLM.Text.Substring(0, txtPLM.Text.Length - intDec);
            //                //        txtPLM.Text = str1;
            //                //    }
            //                //    if (txtTax.Text.Trim() != "")
            //                //    {
            //                //        string str = txtTax.Text.Substring(0, txtTax.Text.Length - intDec);
            //                //        txtTax.Text = str;
            //                //    }
            //                //}
            //                //else if (intDec == 3)
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {
            //                //        string str1 = txtPLM.Text.Substring(0, txtPLM.Text.Length - 1);
            //                //        txtPLM.Text = str1;
            //                //    }
            //                //    if (txtTax.Text.Trim() != "")
            //                //    {
            //                //        string str = txtTax.Text.Substring(0, txtTax.Text.Length - 1);
            //                //        txtTax.Text = str;
            //                //    }
            //                //}
            //                //else if (intDec == 4)
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {

            //                //        string str1 = txtPLM.Text.Substring(0, txtPLM.Text.Length - 0);
            //                //        txtPLM.Text = str1;
            //                //    }
            //                //    if (txtTax.Text.Trim() != "")
            //                //    {
            //                //        string str = txtTax.Text.Substring(0, txtTax.Text.Length - 0);
            //                //        txtTax.Text = str;
            //                //    }
            //                //}
            //                //else if (intDec > 4)
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {
            //                //        txtTax.Text = Convert.ToString(ObjS3G_GlobalParameterLOB.Rows[j]["Corporate_Tax_Rate"]);
            //                //        txtPLM.Text = Convert.ToString(ObjS3G_GlobalParameterLOB.Rows[j]["Prime_Lending_Rate"]);
            //                //    }

            //                //}
            //                //else
            //                //{
            //                //    if (txtPLM.Text.Trim() != "")
            //                //    {
            //                //        string str = txtTax.Text.Substring(0, txtTax.Text.Length - intDec);
            //                //        string str1 = txtPLM.Text.Substring(0, txtPLM.Text.Length - intDec);
            //                //        txtTax.Text = str;
            //                //        txtPLM.Text = str1;
            //                //    }
            //                //}

            //            }
            //        }
            //    }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }
    }

    /// <summary>
    /// This methode used in Get ROI Details
    /// </summary>
    private void FunGetGlobalROIDetatils()
    {
        objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
        try
        {
            ObjS3G_GlobalParameterROI = new OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable();
            OrgMasterMgtServices.S3G_ORG_GlobalParameterROIRow ObjGlobalMasterROIRow;
            ObjGlobalMasterROIRow = ObjS3G_GlobalParameterROI.NewS3G_ORG_GlobalParameterROIRow();
            ObjGlobalMasterROIRow.Global_Parameter_Org_ID = Convert.ToInt32(hdnGlobalParamId.Value);
            ObjS3G_GlobalParameterROI.AddS3G_ORG_GlobalParameterROIRow(ObjGlobalMasterROIRow);
            //            objGlobalSetup_MasterClient = new OrgMasterMgtServicesReference.OrgMasterMgtServicesClient();
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] byteGlobalROIDetails = objGlobalSetup_MasterClient.FunPubGetGlobalROI(SerMode, ClsPubSerialize.Serialize(ObjS3G_GlobalParameterROI, SerMode));
            ObjS3G_GlobalParameterROI = (OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable)ClsPubSerialize.DeSerialize(byteGlobalROIDetails, SerializationMode.Binary, typeof(OrgMasterMgtServices.S3G_ORG_GlobalParameterROIDataTable));

            //for (int i = 0; i < gvProgram.Rows.Count; i++)
            //{

            //    if (gvProgram.Rows[i].RowType == DataControlRowType.DataRow)
            //    {

            //        int Program_ID = Convert.ToInt32(gvProgram.DataKeys[i]["Program_ID"].ToString());
            //        CheckBox Cbx1 = (CheckBox)gvProgram.Rows[i].FindControl("CbxProgram");
            //        Cbx1.Checked = false;
            //        if (Program_ID == Convert.ToInt32(ObjS3G_GlobalParameterROI.Rows[i]["Program_ID"].ToString()))
            //        {
            //            //Cbx1.Checked = Convert.ToBoolean(ObjS3G_GlobalParameterROI.Rows[i]["IS_Program"]);
            //            //Cbx1.Checked = true;
            //            if (ObjS3G_GlobalParameterROI.Rows[i]["IS_Program"].ToString() == "True")
            //            {
            //                Cbx1.Checked = true;
            //                txtPro.Text = "1";
            //            }
            //            else
            //                Cbx1.Checked = false;
            //        }
            //    }
            //}

        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objGlobalSetup_MasterClient.Close();
        }

    }
    #endregion


    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlModify.SelectedIndex = 0;
        CbxOfferNo.Checked = false;
        CbxApplicationNo.Checked = false;
        CbxEnquiryNo.Checked = false;
        ddlIRR.SelectedIndex = 0;
        ddlAccount.SelectedIndex = 0;
        txtAmount.Text = "";
        //for (int i = 0; i < gvIRR.Rows.Count; i++)
        //{
        //    if (gvIRR.Rows[i].RowType == DataControlRowType.DataRow)
        //    {
        //        DropDownList ddlInvoice = (DropDownList)gvIRR.Rows[i].FindControl("ddlvoice");
        //        ddlInvoice.SelectedIndex = 0;
        //        DropDownList ddlDes = (DropDownList)gvIRR.Rows[i].FindControl("ddlDesc");
        //        ddlDes.SelectedIndex = 0;

        //        TextBox txtCTR = (TextBox)gvIRR.Rows[i].FindControl("txtCTR");
        //        txtCTR.Text = "";

        //        TextBox txtPLR = (TextBox)gvIRR.Rows[i].FindControl("txtPLR");
        //        txtPLR.Text = "";

        //        CheckBox Cbx = (CheckBox)gvIRR.Rows[i].FindControl("CbxIRR");
        //        Cbx.Checked = false;
        //    }
        //}
        //for (int i = 0; i < gvProgram.Rows.Count; i++)
        //{
        //    if (gvProgram.Rows[i].RowType == DataControlRowType.DataRow)
        //    {
        //        CheckBox Cbx = (CheckBox)gvProgram.Rows[i].FindControl("CbxProgram");
        //        Cbx.Checked = false;
        //    }
        //}

        for (int i = 0; i < gvGeneral.Rows.Count; i++)
        {
            if (gvGeneral.Rows[i].RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCashierAc = (DropDownList)gvGeneral.Rows[i].FindControl("ddlCashierAc");
                ddlCashierAc.SelectedValue = "0";
            }
        }
    }

    /// <summary>
    /// This methode used in Role Access Setup
    /// </summary>
    #region FieldDisable
    private void FunPriDisableControls(int intMode)
    {
        switch (intMode)
        {

            case 0://Create Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                FunPriGetLookUpList();
                FunPriGetGlobalProgram();
                FunPriGetGlobalSetupLOB();
                boolGPSDone = false;
                ddlModify.Focus();
                break;

            case 1: // Modify Mode               
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                //FunGetGlobalSetupDetatils();
                btnClear.Enabled = false;
                break;

            case -1: // Query Mode               
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                // FunGetGlobalSetupDetatils();
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                CbxApplicationNo.Enabled = false;
                CbxEnquiryNo.Enabled = false;
                CbxOfferNo.Enabled = false;
                txtAmount.ReadOnly = true;
                //foreach (GridViewRow grv in gvIRR.Rows)
                //{

                //    CheckBox Chebox = grv.FindControl("CbxIRR") as CheckBox;
                //    DropDownList ddbox1 = grv.FindControl("ddlvoice") as DropDownList;
                //    DropDownList ddbox2 = grv.FindControl("ddlDesc") as DropDownList;
                //    TextBox txttax1 = grv.FindControl("txtCTR") as TextBox;
                //    TextBox txttax2 = grv.FindControl("txtPLR") as TextBox;
                //    Chebox.Enabled = false;
                //    ddbox1.Enabled = false;
                //    ddbox2.Enabled = false;
                //    txttax1.ReadOnly = true;
                //    txttax2.ReadOnly = true;

                //}
                //foreach (GridViewRow grv in gvProgram.Rows)
                //{

                //    CheckBox Chebox = grv.FindControl("CbxProgram") as CheckBox;
                //    Chebox.Enabled = false;
                //}

                foreach (GridViewRow grv in gvGeneral.Rows)
                {

                    DropDownList ddlCashierAc = grv.FindControl("ddlCashierAc") as DropDownList;
                    ddlCashierAc.Enabled = false;
                }

                if (bClearList)
                {
                    ddlModify.ClearDropDownList();
                    ddlIRR.ClearDropDownList();
                    ddlAccount.ClearDropDownList();

                }
                break;

        }
    }
    #endregion
    protected void gvProgram_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    CheckBox Chebox = (CheckBox)e.Row.FindControl("CbxProgram");
        //    Chebox.Attributes.Add("onclick", "fnAssignText(this);");
        //    //if (Chebox.Checked)
        //    //{
        //    //    txtPro.Text = "1";
        //    //}
        //    //if (Convert.ToString(bQuery) == "True")
        //    //{
        //    //    Chebox.Enabled = false;

        //    //}

        //}

    }
    //protected void ddlModify_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlModify.Focus();
    //}

    private void FunPriSetMaxLength()
    {
        txtAmount.SetDecimalPrefixSuffix(4, 0, true, "Currency Round off");
        //foreach (GridViewRow grv in gvIRR.Rows)
        //{
        //    TextBox txtPLR = grv.FindControl("txtPLR") as TextBox;
        //    TextBox txtCTR = grv.FindControl("txtCTR") as TextBox;

        //    //txtCTR.SetDecimalPrefixSuffix(3, 4, true, "CTR");
        //    //txtPLR.SetDecimalPrefixSuffix(3, 4, true, "PLR");

        //    txtCTR.SetPercentagePrefixSuffix(3, 2,true, true, "CTR");
            
        //    //txtPLR.SetDecimalPrefixSuffix(3, 2,true, true, "PLR");
        //    txtPLR.SetPercentagePrefixSuffix(3, 2, true, true, "PLR");
        //}
        //TextBox txtPLR = gvIRR.FindControl("txtPLR") as TextBox;
        //txtPLR.CheckGPSLength(intCompanyId, true);

        //TextBox txtCTR = gvIRR.FindControl("txtCTR") as TextBox;
        //txtCTR.CheckGPSLength(intCompanyId, true);

        //txtCTR.SetDecimalPrefixSuffix(intCompanyId, 3, 4, true);
        //txtMarginAmountAsset.CheckGPSLength(intCompany_Id, true);
        //txtPLR.SetDecimalPrefixSuffix(intCompanyId, 3, 4, true);
        //txtResidualValue_Cashflow.SetDecimalPrefixSuffix(intCompany_Id, 2, 2, true);
        //txtResidualAmt_Cashflow.CheckGPSLength(intCompany_Id, true);

        //txtFacilityAmt.CheckGPSLength(intCompany_Id, true);
        //txtUnitValue.SetDecimalPrefixSuffix(intCompany_Id, 10, 2, true);

    }

    private void FunPriValidateCheckBox()
    {
        if (CbxApplicationNo.Checked == true || CbxEnquiryNo.Checked == true || CbxOfferNo.Checked == true)
        {
            bChkBox = true;
        }
        else
        {
            bChkBox = false;
        }
    }

    private void FunPriGetLOBCashierAccount()
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID",Convert.ToString(intCompanyId));
        dictParam.Add("@Global_Parameter_Org_ID",hdnGlobalParamId.Value);
        DataTable dt = new DataTable();        
        dt = Utility.GetDefaultData("S3G_ORG_GetLOBCashierAccounts",dictParam);
        gvGeneral.DataSource = dt;
        gvGeneral.DataBind();
        for (int i = 0; i <= gvGeneral.Rows.Count-1; i++)
        {
            DropDownList ddlCashierAc = (DropDownList)gvGeneral.Rows[i].FindControl("ddlCashierAc");
            ddlCashierAc.SelectedValue = Convert.ToString(dt.Rows[i]["Cashier_GL_Code"]);
        }
    }


    protected void gvGeneral_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlCashierAc = ((DropDownList)(e.Row.FindControl("ddlCashierAc")));            

            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));            
            ddlCashierAc.BindDataTable("S3G_ORG_GetCashierAccounts", dictParam, new string[] { "Value", "Name" });
        }
    }
}
