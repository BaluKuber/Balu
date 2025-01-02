
//Module Name      :   Origination
//Screen Name      :   S3GORGCreditScoreGuide_Add.aspx
//Created By       :   Kaliraj K
//Created Date     :   23-JUN-2010
//Purpose          :   To insert and update Credit score guide details

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
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Linq;



public partial class S3GORGCreditScoreGuide_Add : ApplyThemeForProject
{
    #region intColCountalization

    CreditMgtServicesReference.CreditMgtServicesClient ObjCreditScoreClient;
    CreditMgtServices.S3G_ORG_CreditScoreDataTable ObjS3G_ORG_CreditScoreDataTable = new CreditMgtServices.S3G_ORG_CreditScoreDataTable();
    SerializationMode SerMode = SerializationMode.Binary;
    int intErrCode = 0;

    int intCreditScoreID = 0;
    int intUserID = 0;
    int intCompanyID = 0;
    double dScore = 0;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    //Code end

    DataSet dsCreditScore = new DataSet();
    Dictionary<string, string> Procparam = null;
    Dictionary<string, string> dictLOB = null;
    bool bYearBind = true;
    string strXMLCreditScoreDet = "<Root><Details Desc='0' /></Root>";
    StringBuilder strbLOBDet = new StringBuilder();
    StringBuilder strbCreditScoreDet = new StringBuilder();
    string strRedirectPage = "../Origination/S3GORGCreditScoreGuide_View.aspx";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../Origination/S3GORGCreditScoreGuide_Add.aspx';";
    string strRedirectPageView = "window.location.href='../Origination/S3GORGCreditScoreGuide_View.aspx';";
    DataTable dtDefault = new DataTable();
    int iCount = 0;
    int intCountYearValue = 0;
    string strDateFormat = string.Empty;
    int iYearsPresent = 0;
    int intNoofYears = 0;
    DataTable dtCreditScore = new DataTable();
    #endregion

    #region PageLoad

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            iCount = 0;
            intNoofYears = 1;
            UserInfo ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                if (fromTicket != null)
                {
                    intCreditScoreID = Convert.ToInt32(fromTicket.Name);
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
                if (PageMode == PageModes.Create)
                {
                    FunPriBindLOB();
                    FunPriBindLookup();
                    FunProInitializNumberGrid();
                    FunProInitializeMainGrid();
                }
                ddlYear.Items.Add(new ListItem("Year1", "1"));
                
                hdnCreditScoreID.Value = intCreditScoreID.ToString();
                hdnCreditScoreUpdatedID.Value = intCreditScoreID.ToString();
                //FunGetCreditScoreParameterDetails();
                txtTotalScore.Attributes.Add("readonly", "readonly");
                //User Authorization
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if ((intCreditScoreID > 0) && (strMode == "M"))
                {
                    FunProLoadCreditScoreDetails();
                    FunPriDisableControls(1);
                }
                else if ((intCreditScoreID > 0) && (strMode == "Q")) // Query // Modify
                {
                    FunProLoadCreditScoreDetails();
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);
                    ddlLOB.Focus();
                }

                tcCreditScore.ActiveTabIndex = 0;
            }

            if (grvCreditScore.FooterRow != null)
            {
                TextBox txtScore = (TextBox)grvCreditScore.FooterRow.FindControl("txtScore1F");
                txtScore.SetDecimalPrefixSuffix(10, 4, true, "Score");

                TextBox txtDiffPer = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
                //txtDiffPer.SetDecimalPrefixSuffix(2, 2, true, "Difference %");
                txtDiffPer.SetPercentagePrefixSuffix(2, 2, true, "Difference %");
            }
           

        }
        catch (Exception ex)
        {

            throw;
        }
    }


    #endregion

    #region Page Events

    /// <summary>
    /// This is used to save CreditScore details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            dtCreditScore = (DataTable)ViewState["CreditScoreYr"];
            if (dtCreditScore.Rows[0]["FieldAtt"].ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Add atleast one Credit Score Details');", true);
                return;
            }

            foreach (GridViewRow GRow in grvCreditScore.Rows)
            {
                TextBox txtDescF = (TextBox)GRow.FindControl("txtDesc");
                DropDownList ddlFieldAttF = (DropDownList)GRow.FindControl("ddlFieldAtt");
                DropDownList ddlNumericF = (DropDownList)GRow.FindControl("ddlNumeric");
                TextBox txtDiffPerF = (TextBox)GRow.FindControl("txtDiffPer");
                TextBox txtDiffMarkF = (TextBox)GRow.FindControl("txtDiffMark");
                TextBox txtReqValueF = (TextBox)GRow.FindControl("txtReqValue1");
                DropDownList ddlYes = (DropDownList)GRow.FindControl("ddlYes1");
                TextBox txtScoreF = (TextBox)GRow.FindControl("txtScore1");


                if (txtDescF.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Description');", true);
                    txtDescF.Focus();
                    return;

                }
                else if (ddlFieldAttF.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Field Attribute');", true);
                    ddlFieldAttF.Focus();
                    return;

                }
                else if ((ddlFieldAttF.SelectedValue != "4") && (ddlNumericF.SelectedIndex == 0))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Numeric Operator');", true);
                    ddlNumericF.Focus();
                    return;
                }
                //Code added by saran to skip validation for none and repeat operator
                int intNumericAtt = 0;
                intNumericAtt = Convert.ToInt32(ddlNumericF.SelectedValue);

                if (ddlFieldAttF.SelectedValue == "4")
                {

                    //Commende based on UAT
                    //Uncommmented for bug fixing - Bug_ID - 5404 - Kuppusamy.B - 22-Feb-2012

                    if (ddlYes.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Required Parameter');", true);
                        ddlYes.Focus();
                        return;
                    }
                }
                if ((ddlFieldAttF.SelectedValue != "4") && (txtReqValueF.Text.Trim() == string.Empty))
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Parameter');", true);
                    //txtReqValueF.Focus();
                    //return;

                }

                if (!(intNumericAtt > 5 && intNumericAtt < 8))////Code added by saran to skip validation for none and repeat operator
                {
                    if (ddlFieldAttF.SelectedValue == "4")
                    {
                        if (ddlYes.SelectedIndex > 0 && txtScoreF.Text.Trim() == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                            txtScoreF.Focus();
                            return;
                        }
                    }
                    else if (txtScoreF.Text.Trim() == string.Empty && (txtReqValueF.Text.Trim() != string.Empty))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                        txtScoreF.Focus();
                        return;

                    }
                    if (txtScoreF.Text.Trim() == string.Empty && (txtReqValueF.Text.Trim() == string.Empty))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                        txtScoreF.Focus();
                        return;
                    }
                    if (txtScoreF.Text == ".")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter a valid Score');", true);
                        txtScoreF.Focus();
                        return;
                    }
                    decimal tempScore;
                    tempScore = Convert.ToDecimal(txtScoreF.Text);

                    if (tempScore == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Score cannot be Zero');", true);
                        txtScoreF.Focus();
                        return;
                    }
                }

                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(".", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                        int RatioSep = txtReqValueF.Text.IndexOf('.');
                        if (txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf('.'))
                        {
                            txtReqValueF.Text = txtReqValueF.Text + "00";
                        }
                        if (RatioSep == 0)
                        {
                            txtReqValueF.Text = "0" + txtReqValueF.Text;
                        }
                    }
                    double temp;
                    temp = Convert.ToDouble(txtReqValueF.Text);
                    if (temp == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Required Parameter should be greater than Zero');", true);
                        txtReqValueF.Focus();
                        return;
                    }
                }

                // Modified Based on UAT
                //if ((ddlFieldAttF.SelectedValue != "4") && (ddlFieldAttF.SelectedValue != "5") && (txtDiffPerF.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference %');", true);
                //    txtDiffPerF.Focus();
                //    return;

                //}
                if ((ddlFieldAttF.SelectedValue != "4") && (ddlFieldAttF.SelectedValue != "5") && (txtDiffMarkF.Text.Trim() == string.Empty) && (txtDiffPerF.Text.Trim() != string.Empty))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference Mark');", true);
                    txtDiffMarkF.Focus();
                    return;
                }

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(".", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Percentage');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                        int RatioSep = txtReqValueF.Text.IndexOf('.');
                        if (txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf('.'))
                        {
                            txtReqValueF.Text = txtReqValueF.Text + "00";
                        }
                        if (RatioSep == 0)
                        {
                            txtReqValueF.Text = "0" + txtReqValueF.Text;
                        }
                    }
                }

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        int DecSep = txtReqValueF.Text.IndexOf('.');
                        string Frac = txtReqValueF.Text.Substring(DecSep + 1);
                        if (Frac.Length >= 5)
                        {
                            Frac = Frac.Substring(0, 5);
                            string ActValue = txtReqValueF.Text.Substring(0, DecSep) + "." + Frac;
                            if (Convert.ToDecimal(ActValue) >= 100)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }
                    }
                    else if (Convert.ToDecimal(txtReqValueF.Text) >= 100)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                        txtReqValueF.Focus();
                        return;
                    }
                }

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty) && Convert.ToDouble(txtReqValueF.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be Zero');", true);
                    txtReqValueF.Focus();
                    return;
                }

                if ((ddlFieldAttF.SelectedValue == "3") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains(":"))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(":", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }

                        int RatioSep = txtReqValueF.Text.IndexOf(':');
                        if (RatioSep == 0 || txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf(':'))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }

                        string FirstVal = txtReqValueF.Text.Substring(0, RatioSep);
                        string SecondVal = txtReqValueF.Text.Substring(RatioSep + 1);

                        if (FirstVal.Contains("."))
                        {
                            if (FirstVal.Length - 1 != FirstVal.Replace(".", "").Length)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }
                        if (SecondVal.Contains("."))
                        {
                            if (SecondVal.Length - 1 != SecondVal.Replace(".", "").Length)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }

                        if (Convert.ToDouble(txtReqValueF.Text.Substring(0, RatioSep)) == 0 || Convert.ToDouble(txtReqValueF.Text.Substring(RatioSep + 1)) == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                        txtReqValueF.Focus();
                        return;
                    }

                }

                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    string[] temp;
                    temp = (txtReqValueF.Text).Split('.');
                    if (temp.Length > 2)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                        txtReqValueF.Focus();
                        return;
                    }

                }

                if (txtDescF.Text.Trim() != string.Empty && ddlFieldAttF.SelectedIndex != 4 && ddlFieldAttF.SelectedIndex != 5)// || ddlFieldAttF.SelectedValue > 0)
                {
                    if (txtReqValueF.Text.Trim() == string.Empty)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Parameter');", true);
                        //txtReqValueF.Focus();
                        //return;
                    }
                    else
                    {
                        if (ddlYes.SelectedIndex != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Required Parameter');", true);
                            ddlYes.Focus();
                            return;
                        }
                    }
                }

                else
                {
                    if (txtReqValueF.Text.Trim() == string.Empty && ddlYes.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Required Parameter');", true);
                        ddlYes.Focus();
                        return;
                    }
                }

                //Code added for Bug Fixing - ID - 5463 - Kuppusamy.B - Feb-29-2012
                //For 'Amount' and '=' ,difference mark and per not needed.
                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (ddlNumericF.SelectedValue == "5") && (txtDiffPerF.Text != string.Empty) && (txtDiffMarkF.Text != string.Empty))
                {
                    txtDiffPerF.Text = string.Empty;
                    txtDiffMarkF.Text = string.Empty;
                    txtDiffPerF.ReadOnly = true;
                    txtDiffMarkF.ReadOnly = true;
                    //txtDiffMarkF.Attributes.Add("ContentEditable", "false");
                    //txtDiffPerF.Attributes.Add("ContentEditable", "false");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Mark and Percentage not applicable for EQUAL TO operator ');", true);
                    ddlNumericF.Focus();
                    //txtDiffMarkF.Attributes.Add("ContentEditable", "false");
                    //txtDiffPerF.Attributes.Add("ContentEditable", "false");

                    return;
                }



            }


            dtCreditScore = (DataTable)ViewState["CreditScoreYr"];

            if (dtCreditScore.Rows[0]["FieldAtt"].ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Add atleast one record');", true);
                return;
            }
            if (txtTotalScore.Text != "")
            {
                if (Convert.ToDecimal(txtTotalScore.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Add atleast one valid Credit Score Details');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Add atleast one valid Credit Score Details');", true);
                return;
            }

            ObjS3G_ORG_CreditScoreDataTable = new CreditMgtServices.S3G_ORG_CreditScoreDataTable();
            CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow;

            ObjCreditScoreRow = ObjS3G_ORG_CreditScoreDataTable.NewS3G_ORG_CreditScoreRow();

            ObjCreditScoreRow.Company_ID = intCompanyID;

            ObjCreditScoreRow.CreditScore_Guide_ID = Convert.ToInt32(hdnCreditScoreID.Value);

            if (ddlLOB.SelectedValue.Count() > 0)
            {
                ObjCreditScoreRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            if (ddlProductCode.SelectedValue.Count() > 0)
            {
                ObjCreditScoreRow.Product_ID = Convert.ToInt32(ddlProductCode.SelectedValue);
            }
            if (ddlConstitution.SelectedValue.Count() > 0)
            {
                ObjCreditScoreRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
            }
            ObjCreditScoreRow.NoOfYears = Convert.ToInt32(txtNoofYears.Text);
            ObjCreditScoreRow.PastYear = Convert.ToInt32(txtPrvYears.Text);
            ObjCreditScoreRow.FutureYear = Convert.ToInt32(txtNxtYears.Text);
            ObjCreditScoreRow.YearValue = rbtYears.Items[0].Value;
            ObjCreditScoreRow.Created_By = intUserID;
            ObjCreditScoreRow.Is_Active = chkActive.Checked;

            if (!FunPriGenerateCreditScoreParameterXMLDet())
            {
                return;
            }
            ObjCreditScoreRow.XMLNumberDet = ((DataTable)ViewState["dtNumber"]).FunPubFormXml();
            //ObjCreditScoreRow.XMLCreditScoreParameterValues = strXMLCreditScoreDet;
            ObjCreditScoreRow.XMLCreditScoreParameterValues = ((DataTable)ViewState["CreditScore"]).FunPubFormXml();

            ObjS3G_ORG_CreditScoreDataTable.AddS3G_ORG_CreditScoreRow(ObjCreditScoreRow);


            int outNoofYear = 0;
            int outCreditScoreID = 0;
            intErrCode = ObjCreditScoreClient.FunPubCreateCreditScoreDetails(out outNoofYear, out outCreditScoreID, SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CreditScoreDataTable, SerMode));

            if (intErrCode == 0)
            {
                if ((intCreditScoreID > 0) || (hdnCreditScoreUpdatedID.Value != "0"))
                {
                    //FunGetCreditScoreParameterDetails();

                    //if (ddlYear.Items.Count > 1)
                    //{
                    //    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    //    btnSave.Enabled = false;
                    //    //End here

                    //    strAlert = "Credit Score Guide updated successfully ";
                    //    strAlert += @"\n\nWould you like to modify other years?";
                    //    strAlert = "if(confirm('" + strAlert + "')){window.location.href='../Origination/S3GORGCreditScoreGuide_Add.aspx?" + Request.QueryString + "';}else {" + strRedirectPageView + "}";
                    //    strRedirectPageView = "";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //}
                    ////UAT
                    //else
                    //{
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    strAlert = strAlert.Replace("__ALERT__", "Credit Score Guide Updated Successfully");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //}

                    //FunPriBindYear(outNoofYear);
                }
                else
                {
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    //Utility.FunShowAlertMsg(this.Page, "Credit Score Guide details added successfully for " + ddlYear.SelectedItem.Text);
                    //if (((DataTable)ViewState["dtProcessed"]).Rows.Count == rbtYears.Items.Count)
                    //{
                    strAlert = "Credit Score Guide details added successfully ";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //}
                    //else
                    //{
                    //    strAlert = "Credit Score Guide details added successfully";
                    //    strAlert += @"\n\nWould you like to add one more Year?";
                    //    strAlert = "if(confirm('" + strAlert + "')){" + "}else {" + strRedirectPageView + "}";
                    //    strRedirectPageView = "";
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    //    lblErrorMessage.Text = string.Empty;

                    //    hdnCreditScoreID.Value = outCreditScoreID.ToString();
                    //    //FunGetCreditScoreParameterDetails();
                    //    ddlYear.Focus();
                    //    grvCreditScore.FooterRow.Visible = false;
                    //    grvCreditScore.Columns[8].Visible = false;
                    //}
                }
                ddlLOB.Enabled = false;
                ddlConstitution.Enabled = false;
                ddlProductCode.Enabled = false;
                btnClear.Enabled = false;
            }
            else if (intErrCode == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Active Credit Score Guide details exists for the given LOB, Product and Constitution combination");
                return;
            }
            else if (intErrCode == 2)
            {
                Utility.FunShowAlertMsg(this.Page, "Description is duplicated");
                return;
            }
            else if (intErrCode == 3)
            {
                Utility.FunShowAlertMsg(this.Page, "Credit Score Guide details cannot be updated.Transaction exists");
                return;
            }
            else if (intErrCode == 5)
            {
                Utility.FunShowAlertMsg(this.Page, "Active Credit Score Guide details exists for the given LOB, Product and Constitution combination");
                chkActive.Checked = false;
                return;
            }
            else
            {
                Utility.FunShowAlertMsg(this.Page, "Error in saving Credit Score Guide details");
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
            //if (ObjCreditScoreClient != null)
            ObjCreditScoreClient.Close();
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

    //protected void grvCreditScore_RowDataBound(object sender, GridViewRowEventArgs e)
    // {
    //     if (e.Row.RowType == DataControlRowType.DataRow)
    //     {
    //         CheckBox chkSel = (CheckBox)e.Row.FindControl("chkSel");
    //         Label lblCreditScoreID = (Label)e.Row.FindControl("lblCreditScoreID");
    //         if (lblCreditScoreID.Text == hdnCreditScore.Value)
    //         {
    //             chkSel.Checked = true;
    //         }
    //     }
    // }

    protected void ddlLOB_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindConstitutionProduct();
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
            if (ddlConstitution.SelectedValue.Count() > 0)
            {
                ddlConstitution.SelectedIndex = 0;
            }
            ddlYear.SelectedIndex = 0;
            if (ddlProductCode.SelectedValue.Count() > 0)
            {
                ddlProductCode.SelectedIndex = 0;
            }
            txtNoofYears.Text = txtPrvYears.Text = txtNxtYears.Text = "";
            FunPriBindLookup();
            //FunGetCreditScoreParameterDetails();
            rbtYears.Items.Clear();
            btnAddYear.Enabled = false;
            FunProInitializeMainGrid();
            FunProInitializNumberGrid();
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

    protected void grvCreditScore_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {


            DataRow dr;
            if (e.CommandName == "AddNew")
            {
                TextBox txtDescF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDescF");
                DropDownList ddlFieldAttF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlFieldAttF");
                DropDownList ddlNumericF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlNumericF");
                TextBox txtDiffPerF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
                TextBox txtDiffMarkF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffMarkF");
                TextBox txtReqValueF = (TextBox)grvCreditScore.FooterRow.FindControl("txtReqValue1F");
                DropDownList ddlYes = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlYes1F");
                TextBox txtScoreF = (TextBox)grvCreditScore.FooterRow.FindControl("txtScore1F");

                if (txtDescF.Text.Trim() == string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Description');", true);
                    txtDescF.Focus();
                    return;

                }
                else if (ddlFieldAttF.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Field Attribute');", true);
                    ddlFieldAttF.Focus();
                    return;

                }
                else if ((ddlFieldAttF.SelectedValue != "4") && (ddlNumericF.SelectedIndex == 0))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Numeric Operator');", true);
                    ddlNumericF.Focus();
                    return;
                }

                if (ddlFieldAttF.SelectedValue == "4")
                {
                    // Uncommmented for bug fixing - Bug_ID - 5404 - Kuppusamy.B - 22-Feb-2012
                    if (ddlYes.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Required Parameter');", true);
                        ddlYes.Focus();
                        return;
                    }
                }

                //if ((ddlFieldAttF.SelectedValue == "4") && (txtReqValueF.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Required Parameter');", true);
                //    txtReqValueF.Focus();
                //    return;

                //}

                // Commanded based on the UAT

                //if ((ddlFieldAttF.SelectedValue != "4") && (txtReqValueF.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Parameter');", true);
                //    txtReqValueF.Focus();
                //    return;

                //}
                if ((ddlFieldAttF.SelectedValue != "4") && (txtReqValueF.Text.Trim() == string.Empty))
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Parameter');", true);
                    //txtReqValueF.Focus();
                    //return;

                }

                //Code added by saran to skip validation for none and repeat operator
                int intNumericAtt = 0;
                intNumericAtt = Convert.ToInt32(ddlNumericF.SelectedValue);
                if (!(intNumericAtt > 5 && intNumericAtt < 8))////Code added by saran to skip validation for none and repeat operator
                {
                    if (ddlFieldAttF.SelectedValue == "4")
                    {
                        if (ddlYes.SelectedIndex > 0 && txtScoreF.Text.Trim() == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                            txtScoreF.Focus();
                            return;
                        }
                    }
                    if (txtScoreF.Text.Trim() == string.Empty && (txtReqValueF.Text.Trim() != string.Empty))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                        txtScoreF.Focus();
                        return;

                    }

                    if (txtScoreF.Text == ".")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter a valid Score');", true);
                        txtScoreF.Focus();
                        return;
                    }

                    //if (txtScoreF.Text == "0")
                    /* 
                     *  Commented by Srivatsan to resolve the bug:6016. This is to validate the the Score to 0.
                     */
                    if (Convert.ToDecimal(txtScoreF.Text == "" ? "0.0" : txtScoreF.Text) == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Score cannot be Zero');", true);
                        txtScoreF.Focus();
                        return;
                    }
                }

                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(".", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                        int RatioSep = txtReqValueF.Text.IndexOf('.');
                        if (txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf('.'))
                        {
                            txtReqValueF.Text = txtReqValueF.Text + "00";
                        }
                        if (RatioSep == 0)
                        {
                            txtReqValueF.Text = "0" + txtReqValueF.Text;
                        }
                    }
                    double temp;
                    temp = Convert.ToDouble(txtReqValueF.Text);
                    if (temp == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The Required Amount should be greater than Zero');", true);
                        txtReqValueF.Focus();
                        return;
                    }
                }

                // Modified Based on UAT
                //if ((ddlFieldAttF.SelectedValue != "4") && (ddlFieldAttF.SelectedValue != "5") && (txtDiffPerF.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference %');", true);
                //    txtDiffPerF.Focus();
                //    return;

                //}
                if ((ddlFieldAttF.SelectedValue != "4") && (ddlFieldAttF.SelectedValue != "5") && (txtDiffMarkF.Text.Trim() == string.Empty) && (txtDiffPerF.Text.Trim() != string.Empty))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference Mark');", true);
                    txtDiffMarkF.Focus();
                    return;
                }

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(".", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Percentage');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                        int RatioSep = txtReqValueF.Text.IndexOf('.');
                        if (txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf('.'))
                        {
                            txtReqValueF.Text = txtReqValueF.Text + "00";
                        }
                        if (RatioSep == 0)
                        {
                            txtReqValueF.Text = "0" + txtReqValueF.Text;
                        }
                    }
                }

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains("."))
                    {
                        int DecSep = txtReqValueF.Text.IndexOf('.');
                        string Frac = txtReqValueF.Text.Substring(DecSep + 1);
                        if (Frac.Length >= 5)
                        {
                            Frac = Frac.Substring(0, 5);
                            string ActValue = txtReqValueF.Text.Substring(0, DecSep) + "." + Frac;
                            if (Convert.ToDecimal(ActValue) >= 100)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }
                    }
                    else if ((txtReqValueF.Text.Trim() != string.Empty) && Convert.ToDecimal(txtReqValueF.Text) >= 100)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                        txtReqValueF.Focus();
                        return;
                    }
                }

                //if ((ddlFieldAttF.SelectedValue == "2") && Convert.ToDecimal(txtReqValueF.Text) >= 100) 
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                //    txtReqValueF.Focus();
                //    return;
                //}

                if ((ddlFieldAttF.SelectedValue == "2") && (txtReqValueF.Text.Trim() != string.Empty) && Convert.ToDouble(txtReqValueF.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be Zero');", true);
                    txtReqValueF.Focus();
                    return;
                }

                if ((ddlFieldAttF.SelectedValue == "3") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    if (txtReqValueF.Text.Contains(":"))
                    {
                        if (txtReqValueF.Text.Length - 1 != txtReqValueF.Text.Replace(":", "").Length)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }

                        int RatioSep = txtReqValueF.Text.IndexOf(':');
                        if (RatioSep == 0 || txtReqValueF.Text.Length - 1 == txtReqValueF.Text.IndexOf(':'))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }

                        string FirstVal = txtReqValueF.Text.Substring(0, RatioSep);
                        string SecondVal = txtReqValueF.Text.Substring(RatioSep + 1);

                        if (FirstVal.Contains("."))
                        {
                            if (FirstVal.Length - 1 != FirstVal.Replace(".", "").Length)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }
                        if (SecondVal.Contains("."))
                        {
                            if (SecondVal.Length - 1 != SecondVal.Replace(".", "").Length)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                                txtReqValueF.Focus();
                                return;
                            }
                        }

                        if (Convert.ToDouble(txtReqValueF.Text.Substring(0, RatioSep)) == 0 || Convert.ToDouble(txtReqValueF.Text.Substring(RatioSep + 1)) == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                            txtReqValueF.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                        txtReqValueF.Focus();
                        return;
                    }

                }

                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (txtReqValueF.Text.Trim() != string.Empty))
                {
                    string[] temp;
                    temp = (txtReqValueF.Text).Split('.');
                    if (temp.Length > 2)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                        txtReqValueF.Focus();
                        return;
                    }

                }

                //Code block commented to make textbox noneditable for Field Attribute - "Date" 
                //if (ddlFieldAttF.SelectedValue == "5")
                //{
                //    if (!string.IsNullOrEmpty(txtReqValueF.Text))
                //    {
                //        DateTime dt = new DateTime();
                //        if (!DateTime.TryParse(txtReqValueF.Text.Trim(), out dt))
                //        {
                //            Utility.FunShowAlertMsg(this, "Required date is not in correct format");
                //            txtReqValueF.Focus();
                //            return;
                //        }
                //    }
                //}

                if (txtDescF.Text.Trim() != string.Empty && ddlFieldAttF.SelectedIndex != 4 && ddlFieldAttF.SelectedIndex != 5)// || ddlFieldAttF.SelectedValue > 0)
                {
                    if (txtReqValueF.Text.Trim() == string.Empty)
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Parameter');", true);
                        //txtReqValueF.Focus();
                        //return;
                    }
                    else
                    {
                        if (ddlYes.SelectedIndex != 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Required Parameter');", true);
                            ddlYes.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    if (txtReqValueF.Text.Trim() == string.Empty && ddlYes.SelectedIndex == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Required Parameter');", true);
                        ddlYes.Focus();
                        return;
                    }
                }

                dtCreditScore = (DataTable)ViewState["CreditScoreYr"];
                //if (dtCreditScore.Rows[0]["FieldAtt"].ToString() == "")
                //{
                //    dtCreditScore.Rows[0].Delete();
                //}
                dr = dtCreditScore.NewRow();
                dr["CrScoreParam_ID"] = (dtCreditScore.Rows.Count + 1).ToString();
                dr["Desc"] = txtDescF.Text.Trim().Replace("  ", " ");
                dr["FieldAtt"] = ddlFieldAttF.SelectedValue;
                dr["NumericAtt"] = ddlNumericF.SelectedValue;
                if ((ddlFieldAttF.SelectedValue == "4") || (ddlFieldAttF.SelectedValue == "5"))
                {
                    dr["DiffPer"] = "-1";
                    dr["DiffMark"] = "-1";
                }
                else
                {
                    dr["DiffPer"] = txtDiffPerF.Text.Trim();//== "" ? "-1" : txtDiffPerF.Text.Trim();
                    dr["DiffMark"] = txtDiffMarkF.Text.Trim();// == "" ? "-1" : txtDiffMarkF.Text.Trim();
                }

                dr["ReqValue"] = ddlFieldAttF.SelectedValue == "4" ? ddlYes.SelectedValue : txtReqValueF.Text.Trim();
                dr["Score"] = txtScoreF.Text.Trim();
                dr["Year"] = rbtYears.SelectedValue;

                dtCreditScore.Rows.Add(dr);
                FunPriBindLookup();
                grvCreditScore.DataSource = dtCreditScore;
                grvCreditScore.DataBind();
                ViewState["CreditScoreYr"] = dtCreditScore;

                if (grvCreditScore.FooterRow != null)
                {
                    TextBox txtScoreF1 = (TextBox)grvCreditScore.FooterRow.FindControl("txtScore1F");
                    txtScoreF1.SetPercentagePrefixSuffix(10, 4, true, false, "Score");

                    TextBox txtDiffPerF1 = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
                    txtDiffPerF1.SetPercentagePrefixSuffix(2, 2, true, false, "Difference %");
                }

                TextBox txtDescF1 = (TextBox)grvCreditScore.FooterRow.FindControl("txtDescF");
                txtDescF1.Focus();

                btnAddYear.Enabled = true;

                //TextBox txtaddFromAmt1 = (TextBox)grvCreditScore.FooterRow.FindControl("txtAddFromAmount");
                //txtaddFromAmt1.Text = Convert.ToString(Convert.ToDecimal(txtAddToAmount.Text.Trim()) + Convert.ToInt32("1"));

                //For 'Amount' and '=' ,difference mark and per not needed.
                if ((ddlFieldAttF.SelectedValue == "1" || ddlFieldAttF.SelectedValue == "6") && (ddlNumericF.SelectedValue == "5") && (txtDiffPerF.Text != string.Empty) && (txtDiffMarkF.Text != string.Empty))
                {
                    txtDiffPerF.Text = string.Empty;
                    txtDiffMarkF.Text = string.Empty;
                    txtDiffPerF.ReadOnly = true;
                    txtDiffMarkF.ReadOnly = true;

                    //txtDiffMarkF.Attributes.Add("ContentEditable", "false");
                    //txtDiffPerF.Attributes.Add("ContentEditable", "false");
                    return;
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    protected void NoofYears_TextChanged(object sender, EventArgs e)
    {
        // for (int intColCount = 14; intColCount > 4; intColCount--)
        // {
        //     grvCreditScore.Columns[intColCount].Visible = true;
        // }
        //int intColVisbile = 2 + Convert.ToInt32(txtNoofYears.Text) * 2;
        //for (int intColCount = 14; intColCount > intColVisbile; intColCount--)
        // {
        //     grvCreditScorColumns[intColCount].Visible = false;
        // }
    }

    protected void ddlFieldAtt_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strFieldAtt = ((DropDownList)sender).ClientID;
        string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvCreditScore_")).Replace("grvCreditScore_ctl", "");
        int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        gRowIndex = gRowIndex - 2;
        //if (strFieldAtt.)
        //grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = false;
        //grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = true;

        DropDownList ddlFieldAtt = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlFieldAtt");

        DropDownList ddlNumericF = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlNumeric");
        TextBox txtDiffPerF = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffPer");
        TextBox txtDiffMarkF = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtDiffMark");
        TextBox txtReqValue1 = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1");
        TextBox txtScore = (TextBox)grvCreditScore.Rows[gRowIndex].FindControl("txtScore1");
        //Calendar
        DropDownList ddlYes = (DropDownList)grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1");
        ddlYes.SelectedIndex = 0;
        txtReqValue1.Text = "";
        txtScore.Text = txtDiffPerF.Text = txtDiffMarkF.Text = "";

        txtReqValue1.Attributes.Remove("readOnly");

        AjaxControlToolkit.CalendarExtender ceReqVal1 = grvCreditScore.Rows[gRowIndex].FindControl("CalendarExtender1") as AjaxControlToolkit.CalendarExtender;
        AjaxControlToolkit.FilteredTextBoxExtender fteAmount = grvCreditScore.Rows[gRowIndex].FindControl("fteAmount1") as AjaxControlToolkit.FilteredTextBoxExtender;

        if (((DropDownList)sender).SelectedValue == "1" || ((DropDownList)sender).SelectedValue == "6")
        {
            fteAmount.ValidChars = ".";
        }
        else if (((DropDownList)sender).SelectedValue == "2")
        {
            fteAmount.ValidChars = ".";
        }
        else if (((DropDownList)sender).SelectedValue == "3")
        {
            fteAmount.ValidChars = ".:";
        }
        else
        {
            fteAmount.ValidChars = "";
        }

        if (((DropDownList)sender).SelectedValue == "5")//date
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            //ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = true;
            //grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = true;
            txtReqValue1.Visible = true;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = false;

            //Calendar
            fteAmount.Enabled = false;
            //txtReqValue1.Attributes.Add("readonly", "readonly");
            ceReqVal1.Enabled = true;
            ceReqVal1.Format = strDateFormat;

            //txtReqValue1.Attributes.Add("readOnly", "readOnly");

            if (PageMode != PageModes.Query)
            {
                txtScore.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //txtReqValue1.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqValue1.ClientID + "','" + txtScore.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "', '0', '" + ddlYear.ClientID + "','" + ddlFieldAtt.ClientID + "','" + ddlNumericF.ClientID + "');");
                //txtScore.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1.ClientID + "','" + txtScore.ClientID + "');");
                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }
        }
        else if (((DropDownList)sender).SelectedValue == "4")//text
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = false;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = true;

            if (PageMode != PageModes.Query)
            {
                txtScore.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //ddlYes.Attributes.Add("onchange", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtScore.ClientID + "');");
                //txtScore.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtScore.ClientID + "');");
                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }

            //Calendar
            fteAmount.Enabled = false;
            txtReqValue1.Attributes.Remove("readonly");
            txtReqValue1.ReadOnly = false;
            ceReqVal1.Enabled = false;
        }
        //else if (((DropDownList)sender).SelectedValue == "3")//ratio
        //{

        //}
        else
        {
            txtDiffPerF.Enabled = true;
            txtDiffMarkF.Enabled = true;
            ddlNumericF.Enabled = true;
            grvCreditScore.Rows[gRowIndex].FindControl("txtReqValue1").Visible = true;
            grvCreditScore.Rows[gRowIndex].FindControl("ddlYes1").Visible = false;

            //Calendar
            fteAmount.Enabled = true;
            txtReqValue1.Attributes.Remove("readonly");
            ceReqVal1.Enabled = false;
            txtReqValue1.ReadOnly = false;

            if (PageMode != PageModes.Query)
            {
                txtScore.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //txtReqValue1.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqValue1.ClientID + "','" + txtScore.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "', '0', '" + ddlYear.ClientID + "','" + ddlFieldAtt.ClientID + "','" + ddlNumericF.ClientID + "');");
                //txtScore.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1.ClientID + "','" + txtScore.ClientID + "');");
                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }
        }

        ddlFieldAtt.Focus();

    }

    protected void ddlFieldAttF_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string strFieldAtt = ((DropDownList)sender).ClientID;
        //string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvCreditScore_")).Replace("grvCreditScore_ctl", "");
        //int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
        //gRowIndex = gRowIndex - 2;

        DropDownList ddlFieldAttF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlFieldAttF");

        DropDownList ddlNumericF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlNumericF");
        TextBox txtDiffPerF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
        TextBox txtDiffMarkF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffMarkF");
        TextBox txtReqValue1F = (TextBox)grvCreditScore.FooterRow.FindControl("txtReqValue1F");
        TextBox txtScore1F = (TextBox)grvCreditScore.FooterRow.FindControl("txtScore1F");
        //  DropDownList ddlFieldAttF1 = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlFieldAttF");
        //Calendar
        DropDownList ddlYesF = (DropDownList)grvCreditScore.FooterRow.FindControl("ddlYes1F");
        ddlYesF.SelectedIndex = 0;
        txtReqValue1F.Text = "";
        txtScore1F.Text = txtDiffPerF.Text = txtDiffMarkF.Text = "";
        AjaxControlToolkit.CalendarExtender ceReqVal1F = grvCreditScore.FooterRow.FindControl("CalendarExtender1F") as AjaxControlToolkit.CalendarExtender;
        AjaxControlToolkit.FilteredTextBoxExtender fteAmount1F = grvCreditScore.FooterRow.FindControl("fteAmount1F") as AjaxControlToolkit.FilteredTextBoxExtender;
        // AjaxControlToolkit.MaskedEditExtender meeAmount = grvCreditScore.FooterRow.FindControl("MEExttxtReqValue1F") as AjaxControlToolkit.MaskedEditExtender;

        txtReqValue1F.Attributes.Remove("readOnly");

        if (((DropDownList)sender).SelectedValue == "1" || ((DropDownList)sender).SelectedValue == "6")
        {
            fteAmount1F.ValidChars = ".";
        }
        else if (((DropDownList)sender).SelectedValue == "2")
        {
            fteAmount1F.ValidChars = ".";
        }
        else if (((DropDownList)sender).SelectedValue == "3")
        {
            fteAmount1F.ValidChars = ".:";
        }
        else
        {
            fteAmount1F.ValidChars = "";
        }

        if (((DropDownList)sender).SelectedValue == "5")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            //ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = true;

            txtReqValue1F.Visible = true;
            ddlYesF.Visible = false;

            //txtReqValue1F.Attributes.Add("readonly", "readonly");
            //AjaxControlToolkit.CalendarExtender ceReqVal1F = grvCreditScore.FooterRow.FindControl("CalendarExtender1F") as AjaxControlToolkit.CalendarExtender;
            fteAmount1F.Enabled = false;
            ceReqVal1F.Enabled = true;
            ceReqVal1F.Format = strDateFormat;

            //txtReqValue1F.Attributes.Add("readOnly", "readOnly");

            if (PageMode != PageModes.Query)
            {
                txtScore1F.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //txtReqValue1F.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqValue1F.ClientID + "','" + txtScore1F.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "', '1', '" + ddlYear.ClientID + "','" + ddlFieldAttF.ClientID + "','" + ddlNumericF.ClientID + "');");
                //txtScore1F.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtScore1F.ClientID + "');");

                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }
        }
        else if (((DropDownList)sender).SelectedValue == "4")
        {
            txtDiffPerF.Text = "";
            txtDiffMarkF.Text = "";
            txtDiffPerF.Enabled = false;
            txtDiffMarkF.Enabled = false;
            ddlNumericF.SelectedIndex = 0;
            ddlNumericF.Enabled = false;
            txtReqValue1F.Visible = false;
            ddlYesF.Visible = true;

            if (PageMode != PageModes.Query)
            {
                txtScore1F.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //ddlYesF.Attributes.Add("onchange", "javascript:FunReqScoreEnabled('" + ddlYesF.ClientID + "','" + txtScore1F.ClientID + "');");
                //txtScore1F.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYesF.ClientID + "','" + txtScore1F.ClientID + "');");
                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYesF.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }
            //Calendar
            //txtScore1F.Attributes.Add("readonly", "readonly");
            fteAmount1F.Enabled = false;
            ceReqVal1F.Enabled = false;

        }
        //else if (((DropDownList)sender).SelectedValue == "1")
        //{
        //    //AjaxControlToolkit.FilteredTextBoxExtender fteAmount = grvCreditScore.Rows[gRowIndex].FindControl("fteAmount1") as AjaxControlToolkit.FilteredTextBoxExtender;


        //  //  meeAmount.Enabled = true;
        //}
        else
        {
            txtDiffPerF.Enabled = true;
            txtDiffMarkF.Enabled = true;
            ddlNumericF.Enabled = true;
            txtReqValue1F.Visible = true;
            ddlYesF.Visible = false;

            //Calendar
            fteAmount1F.Enabled = true;
            txtReqValue1F.Attributes.Remove("readonly");
            ceReqVal1F.Enabled = false;

            if (PageMode != PageModes.Query)
            {
                txtScore1F.Attributes.Remove("onFocusIn");
                txtDiffPerF.Attributes.Remove("onFocusIn");

                //txtReqValue1F.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqValue1F.ClientID + "','" + txtScore1F.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "','1', '" + ddlYear.ClientID + "','" + ddlFieldAttF.ClientID + "','" + ddlNumericF.ClientID + "');");
                //txtScore1F.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtScore1F.ClientID + "');");
                //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtDiffPerF.ClientID + "');");
            }
        }
        // DropDownList ddlFieldAtt = null;
        //foreach (GridViewRow grvData in grvCreditScore.Rows[)
        //{
        //    ddlFieldAtt = ((DropDownList)grvData.FindControl("ddlFieldAtt"));
        //    if ((strFieldAtt == ddlFieldAtt.ClientID))
        //    {

        //    }
        //}

        ddlFieldAttF.Focus();
    }



    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bYearBind = false;
        FunGetCreditScoreParameterDetails();
    }

    protected void grvCreditScoreYearValues_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dtYear = new DataTable();
            dtYear = (DataTable)ViewState["CreditScoreYear"];
            if ((dtYear != null) && (dtYear.Rows[intCountYearValue]["Field_AttributeID"].ToString() == "5"))
            {
                string strReqParam1 = dtYear.Rows[intCountYearValue]["ReqParam1"].ToString();
                string strReqParam2 = dtYear.Rows[intCountYearValue]["ReqParam2"].ToString();
                string strReqParam3 = dtYear.Rows[intCountYearValue]["ReqParam3"].ToString();
                string strReqParam4 = dtYear.Rows[intCountYearValue]["ReqParam4"].ToString();
                string strReqParam5 = dtYear.Rows[intCountYearValue]["ReqParam5"].ToString();
                string strReqParam6 = dtYear.Rows[intCountYearValue]["ReqParam6"].ToString();

                Label lblReqParam1 = (Label)e.Row.FindControl("lblReqParam1");
                Label lblReqParam2 = (Label)e.Row.FindControl("lblReqParam2");
                Label lblReqParam3 = (Label)e.Row.FindControl("lblReqParam3");
                Label lblReqParam4 = (Label)e.Row.FindControl("lblReqParam4");
                Label lblReqParam5 = (Label)e.Row.FindControl("lblReqParam5");
                Label lblReqParam6 = (Label)e.Row.FindControl("lblReqParam6");

                lblReqParam1.Text = strReqParam1 != "" ? DateTime.Parse(strReqParam1, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                lblReqParam2.Text = strReqParam2 != "" ? DateTime.Parse(strReqParam2, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                lblReqParam3.Text = strReqParam3 != "" ? DateTime.Parse(strReqParam3, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                lblReqParam4.Text = strReqParam4 != "" ? DateTime.Parse(strReqParam4, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                lblReqParam5.Text = strReqParam5 != "" ? DateTime.Parse(strReqParam5, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";
                lblReqParam6.Text = strReqParam6 != "" ? DateTime.Parse(strReqParam6, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "";

            }

            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            lblDescription.Text = dtYear.Rows[intCountYearValue]["Description"].ToString().Replace(" ", "&nbsp;");

            Label lblScore1 = (Label)e.Row.FindControl("lblScore1");
            Label lblScore2 = (Label)e.Row.FindControl("lblScore2");
            Label lblScore3 = (Label)e.Row.FindControl("lblScore3");
            Label lblScore4 = (Label)e.Row.FindControl("lblScore4");
            Label lblScore5 = (Label)e.Row.FindControl("lblScore5");
            Label lblScore6 = (Label)e.Row.FindControl("lblScore6");
            if (lblScore1 != null && lblScore1.Text != string.Empty)
                lblScore1.Text = Convert.ToDecimal(lblScore1.Text).ToString(Funsetsuffix());
            if (lblScore2 != null && lblScore2.Text != string.Empty)
                lblScore2.Text = Convert.ToDecimal(lblScore2.Text).ToString(Funsetsuffix());
            if (lblScore3 != null && lblScore3.Text != string.Empty)
                lblScore3.Text = Convert.ToDecimal(lblScore3.Text).ToString(Funsetsuffix());
            if (lblScore4 != null && lblScore4.Text != string.Empty)
                lblScore4.Text = Convert.ToDecimal(lblScore4.Text).ToString(Funsetsuffix());
            if (lblScore5 != null && lblScore5.Text != string.Empty)
                lblScore5.Text = Convert.ToDecimal(lblScore5.Text).ToString(Funsetsuffix());
            if (lblScore6 != null && lblScore6.Text != string.Empty)
                lblScore6.Text = Convert.ToDecimal(lblScore6.Text).ToString(Funsetsuffix());

            intCountYearValue++;
        }


    }

    protected void grvCreditScore_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            intErrCode = 0;
            //DataTable dtDelete;
            dtCreditScore = (DataTable)ViewState["CreditScoreYr"];

            //if ((dtCreditScore.Rows.Count == 1)&&(hdnDelete.Value=="0"))
            //{
            //    Utility.FunShowAlertMsg(this.Page, "All records cannot be deleted.");
            //    return;
            //}

            //int intCreditScoreParamId = Convert.ToInt32(dtCreditScore.Rows[e.RowIndex]["CrScoreParam_ID"].ToString());
            ////dtDelete = (DataTable)ViewState["ClassTable"];
            //if (intCreditScoreParamId != 0)
            //{
            //    ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();
            //    intErrCode = ObjCreditScoreClient.FunPubDeleteCreditScoreParamDetails(intCreditScoreParamId);
            //}
            bool bNewRow = false;
            //if (intErrCode == 1)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Record cannot be deleted.Transaction exists");
            //    return;
            //}
            //else if (intErrCode == 2)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "All records cannot be deleted.There should be one parameter in Credit Score Guide");
            //    return;
            //}
            //else
            //{
            dtCreditScore.Rows.RemoveAt(e.RowIndex);
            if (dtCreditScore.Rows.Count <= 0)
            {
                DataRow dr;
                //dtCreditScore.Columns.Add("CrScoreParam_ID");
                //dtCreditScore.Columns.Add("Desc");
                //dtCreditScore.Columns.Add("FieldAtt");
                //dtCreditScore.Columns.Add("NumericAtt");
                //dtCreditScore.Columns.Add("DiffPer");
                //dtCreditScore.Columns.Add("DiffMark");
                //for (int intColCount = 1; intColCount <= intNoofYears; intColCount++)
                //{
                //    dtCreditScore.Columns.Add("ReqValue" + intColCount.ToString());
                //    dtCreditScore.Columns.Add("Score" + intColCount.ToString());
                //}
                dr = dtCreditScore.NewRow();
                dtCreditScore.Rows.Add(dr);
                bNewRow = true;
                //dtCreditScore = null;
            }
            FunPriBindLookup();
            grvCreditScore.DataSource = dtCreditScore;
            grvCreditScore.DataBind();
            if (bNewRow)
            {
                grvCreditScore.Rows[0].Visible = false;
                dtCreditScore.Rows.Clear();
            }
            ViewState["CreditScoreYr"] = dtCreditScore;
            //if (intCreditScoreParamId != 0)
            //{
            //    DataTable dtYear = new DataTable();
            //    dtYear = (DataTable)ViewState["CreditScoreYear"];
            //    dtYear.Rows.RemoveAt(e.RowIndex);
            //    if (dtYear.Rows.Count <= 0)
            //        dtYear = null;
            //    grvCreditScoreYearValues.DataSource = dtYear;
            //    grvCreditScoreYearValues.DataBind();
            //    ViewState["CreditScoreYear"] = dtYear;
            //}


            //}
        }
        catch (Exception ex)
        {
            throw ex;

        }
        finally
        {
            ObjCreditScoreClient.Close();
        }
    }

    protected void grvCreditScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        DataTable dtDefaultNew;
        string strReqValue = string.Empty;
        DataView dvSearchView;
        if (e.Row.RowType == DataControlRowType.Footer)
        {

            if (dtDefault != null)
            {
                DropDownList ddlFieldF = (DropDownList)e.Row.FindControl("ddlFieldAttF");
                DropDownList ddlNumericF = (DropDownList)e.Row.FindControl("ddlNumericF");
                DropDownList ddlYesF = (DropDownList)e.Row.FindControl("ddlYes1F");
                TextBox txtDiffPerF = (TextBox)e.Row.FindControl("txtDiffPerF");
                TextBox txtDiffMarkF = (TextBox)e.Row.FindControl("txtDiffMarkF");
                TextBox txtScoreF = ((TextBox)e.Row.FindControl("txtScore1F"));
                TextBox txtReqValue1F = ((TextBox)e.Row.FindControl("txtReqValue1F"));



                if (PageMode != PageModes.Query)
                {
                    //txtDiffPerF.Attributes.Add("onfocusOut", "javascript:FunDiffMarkEnabled('" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "');");
                    //txtDiffMarkF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "');");

                    //ddlYesF.Attributes.Add("onchange", "javascript:FunReqScoreEnabled('" + ddlYesF.ClientID + "','" + txtScoreF.ClientID + "');");
                    //txtReqValue1F.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqValue1F.ClientID + "','" + txtScoreF.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "', '1', '" + ddlYear.ClientID + "','" + ddlFieldF.ClientID + "','" + ddlNumericF.ClientID + "');");
                    //txtScoreF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtScoreF.ClientID + "');");

                    //txtDiffPerF.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqValue1F.ClientID + "','" + txtDiffPerF.ClientID + "');");
                }

                ddlYesF.Visible = false;
                dvSearchView = new DataView(dtDefault);
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_FieldAttribute'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlFieldF.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                dvSearchView = new DataView(dtDefault);
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_NumericOperator'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlNumericF.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();

                //txtScoreF.Attributes.Add("onblur", "javascript:FunDiFFPerandMarkEnabled('" + ddlFieldF.ClientID + "','" + ddlNumericF.ClientID + "','" + txtDiffPerF.ClientID + "','" + txtDiffMarkF.ClientID + "');");
            }
            if (ddlYear.SelectedIndex > 0)
            {
                e.Row.Visible = false;
            }
            if (strMode == "Q")
            {
                grvCreditScore.ShowFooter = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //LinkButton lnkbtnDelete = (LinkButton)e.Row.FindControl("lnkbtnDelete");
            dtCreditScore = (DataTable)ViewState["CreditScoreYr"];
            if ((dtCreditScore != null) && (dtCreditScore.Rows.Count > 0))
            {
                DropDownList ddlField = (DropDownList)e.Row.FindControl("ddlFieldAtt");

                Label lblParamID = (Label)e.Row.FindControl("lblParamID");

                if (lblParamID.Text != "0")
                {
                    ddlField.Enabled = false;
                }

                DropDownList ddlNumeric = (DropDownList)e.Row.FindControl("ddlNumeric");
                DropDownList ddlYes = (DropDownList)e.Row.FindControl("ddlYes1");
                ddlYes.Visible = false;

                TextBox txtReqVal = ((TextBox)e.Row.FindControl("txtReqValue1"));
                TextBox txtScore = ((TextBox)e.Row.FindControl("txtScore1"));
                TextBox txtDiffPer = ((TextBox)e.Row.FindControl("txtDiffPer"));
                TextBox txtDiffMark = ((TextBox)e.Row.FindControl("txtDiffMark"));

                txtReqVal.Attributes.Remove("readOnly");

                if (PageMode == PageModes.Modify)
                {
                    LinkButton lnkbtnDelete = (LinkButton)e.Row.FindControl("lnkbtnDelete");
                    //added by saran on 22-Nov-2011 for bug raised by malolan.
                    //lnkbtnDelete.Enabled = false;
                }

                if (PageMode != PageModes.Query)
                {
                    //txtDiffPer.Attributes.Add("onfocusOut", "javascript:FunDiffMarkEnabled('" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "');");
                    //txtDiffMark.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "');");

                    //ddlYes.Attributes.Add("onchange", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtScore.ClientID + "');");

                    //txtReqVal.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "','" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "', '0', '" + ddlYear.ClientID + "','" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "');");
                    //txtScore.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "');");
                    //txtDiffPer.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtDiffPer.ClientID + "');");
                }

                dvSearchView = new DataView(dtDefault);
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_FieldAttribute'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlField.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();
                ddlField.SelectedValue = dtCreditScore.Rows[iCount]["FieldAtt"].ToString();
                txtScore.SetPercentagePrefixSuffix(10, 4, true, false, "Score");
                txtDiffPer.SetPercentagePrefixSuffix(2, 2, true, false, "Difference %");
                dvSearchView = new DataView(dtDefault); ;
                dvSearchView.RowFilter = "[Type] LIKE 'ORG_NumericOperator'";
                dtDefaultNew = dvSearchView.ToTable();
                ddlNumeric.FillDataTable(dtDefaultNew, "Value", "Name");
                dvSearchView.Dispose();
                ddlNumeric.SelectedValue = dtCreditScore.Rows[iCount]["NumericAtt"].ToString();

                //txtScore.Attributes.Add("onblur", "javascript:FunDiFFPerandMarkEnabled('" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "','" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "');");

                AjaxControlToolkit.CalendarExtender ceReqVal1 = e.Row.FindControl("CalendarExtender1") as AjaxControlToolkit.CalendarExtender;
                AjaxControlToolkit.FilteredTextBoxExtender fteAmount = e.Row.FindControl("fteAmount1") as AjaxControlToolkit.FilteredTextBoxExtender;

                if (ddlField.SelectedValue == "1" || ddlField.SelectedValue == "6")
                {
                    fteAmount.ValidChars = ".";
                }
                else if (ddlField.SelectedValue == "2")
                {
                    fteAmount.ValidChars = ".";
                }
                else if (ddlField.SelectedValue == "3")
                {
                    fteAmount.ValidChars = ".:";
                }
                else
                {
                    fteAmount.ValidChars = "";
                }

                if (ddlField.SelectedValue == "5")
                {
                    txtDiffPer.Enabled = false;
                    txtDiffMark.Enabled = false;
                    //ddlNumeric.Enabled = false;
                    txtReqVal.Visible = true;
                    ddlYes.Visible = false;

                    //Calendar

                    ceReqVal1.Enabled = true;
                    fteAmount.Enabled = false;
                    ceReqVal1.Format = strDateFormat;
                    //txtReqVal.Attributes.Add("readonly", "readonly");
                    txtReqVal.Text = Convert.ToString(dtCreditScore.Rows[iCount]["ReqValue"].ToString() != "" ? DateTime.Parse(Utility.StringToDate(dtCreditScore.Rows[iCount]["ReqValue"].ToString()).ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat) : "");
                    txtDiffPer.Text = "";
                    txtDiffMark.Text = "";

                    //txtReqVal.Attributes.Add("readOnly", "readOnly");

                    if (PageMode != PageModes.Query)
                    {
                        txtScore.Attributes.Remove("onFocusIn");
                        txtDiffPer.Attributes.Remove("onFocusIn");

                        //txtReqVal.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "','" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "','0', '" + ddlYear.ClientID + "','" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "');");
                        //txtScore.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "');");
                        //txtDiffPer.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtDiffPer.ClientID + "');");
                    }
                }
                else if (ddlField.SelectedValue == "4")
                {
                    txtDiffPer.Text = "";
                    txtDiffMark.Text = "";
                    txtDiffPer.Enabled = false;
                    txtDiffMark.Enabled = false;
                    ddlNumeric.SelectedIndex = 0;
                    ddlNumeric.Enabled = false;
                    txtReqVal.Visible = false;
                    ddlYes.Visible = true;
                    ddlYes.SelectedValue = dtCreditScore.Rows[iCount]["ReqValue"].ToString();

                    if (PageMode != PageModes.Query)
                    {
                        txtScore.Attributes.Remove("onFocusIn");
                        txtDiffPer.Attributes.Remove("onFocusIn");

                        //ddlYes.Attributes.Add("onchange", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtScore.ClientID + "');");
                        //txtScore.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtScore.ClientID + "');");
                        //txtDiffPer.Attributes.Add("onFocusIn", "javascript:FunReqScoreEnabled('" + ddlYes.ClientID + "','" + txtDiffPer.ClientID + "');");
                    }
                    //txtScore.Attributes.Add("readonly", "readonly");
                    //Calendar
                    fteAmount.Enabled = false;
                    ceReqVal1.Enabled = false;
                }
                else
                {
                    txtDiffPer.Enabled = true;
                    txtDiffMark.Enabled = true;
                    ddlNumeric.Enabled = true;
                    txtReqVal.Visible = true;
                    ddlYes.Visible = false;

                    //Calendar
                    fteAmount.Enabled = true;
                    txtReqVal.ReadOnly = false;
                    ceReqVal1.Enabled = false;

                    if (PageMode != PageModes.Query)
                    {
                        txtScore.Attributes.Remove("onFocusIn");
                        txtDiffPer.Attributes.Remove("onFocusIn");

                        //txtReqVal.Attributes.Add("onfocusOut", "javascript:FunRowControlsEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "','" + txtDiffPer.ClientID + "','" + txtDiffMark.ClientID + "','0', '" + ddlYear.ClientID + "','" + ddlField.ClientID + "','" + ddlNumeric.ClientID + "');");
                        //txtScore.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtScore.ClientID + "');");
                        //txtDiffPer.Attributes.Add("onFocusIn", "javascript:FunDiffMarkEnabled('" + txtReqVal.ClientID + "','" + txtDiffPer.ClientID + "');");
                    }

                }

                //This is to disable desc,diffper,diffmark,fieldatt,numericope for all other years
                if (ddlYear.SelectedIndex > 0)
                {
                    TextBox txtDesc = (TextBox)e.Row.FindControl("txtDesc");
                    //TextBox txtDiffPer = (TextBox)e.Row.FindControl("txtDiffPer");
                    //TextBox txtDiffMark = (TextBox)e.Row.FindControl("txtDiffMark");
                    txtDesc.Enabled = false;
                    txtDiffPer.Enabled = false;
                    txtDiffMark.Enabled = false;
                    ddlField.Enabled = false;
                    ddlNumeric.Enabled = false;
                }

                dScore += txtScore.Text != "" ? Convert.ToDouble(txtScore.Text) : 0;
                iCount++;


                if (txtScore.Text != string.Empty)
                {
                    txtScore.Text = Convert.ToDecimal(txtScore.Text).ToString(Funsetsuffix());
                }

            }
            txtTotalScore.Text = dScore.ToString(Funsetsuffix());
            //if ((intCreditScoreID > 0) && (strMode == "Q"))
            //{
            //    lnkbtnDelete.Visible = false;
            //}

            //TextBox txtScoreF = (TextBox)grvCreditScore.FooterRow.FindControl("txtScore1F");
            //txtScoreF.SetDecimalPrefixSuffix(10, 4, true, "Score");

            //TextBox txtDiffPerF = (TextBox)grvCreditScore.FooterRow.FindControl("txtDiffPerF");
            //txtDiffPerF.SetDecimalPrefixSuffix(intCompanyID, 2, 2, true);



            if (strMode == "Q")
            {

            }
        }

    }

    #endregion

    #region Page Methods


    private void FunPriBindYear(int iYearsPresent)
    {
        if (bYearBind)
        {
            int index = ddlYear.SelectedIndex;
            ddlYear.Items.Clear();
            string strYear = "";
            string strYearValue = "";
            for (int intColCount = 1; intColCount <= iYearsPresent; intColCount++)
            {
                strYear = "Year" + (intColCount).ToString();
                strYearValue = (intColCount).ToString();
                ddlYear.Items.Add(new ListItem(strYear, strYearValue));
            }
            strYear = "Year" + (iYearsPresent + 1).ToString();
            strYearValue = (iYearsPresent + 1).ToString();
            if (iYearsPresent <= 5)
            {
                ddlYear.Items.Add(new ListItem(strYear, strYearValue));
            }
            ddlYear.SelectedIndex = index;
        }
    }

    /// <summary>
    /// to bind LOB and Product details
    /// </summary>
    private void FunPriBindLOB()
    {
        //LOB List7

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@Program_ID", "26");
        if (intCreditScoreID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }
        Procparam.Add("@User_ID", intUserID.ToString());
        ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

    }
    private void FunPriBindLookup()
    {
        dictLOB = new Dictionary<string, string>();
        dtDefault = Utility.GetDefaultData("S3G_ORG_GetCreditScoreLookup", dictLOB);

    }

    private void FunPriBindConstitutionProduct()
    {
        //Product Code
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        if (intCreditScoreID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }

        ddlProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });


        //Constitution
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
        Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        if (intCreditScoreID == 0)
        {
            Procparam.Add("@Is_Active", "1");
        }

        ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, Procparam, new string[] { "Constitution_ID", "ConstitutionName" });

    }

    /// <summary>
    /// This method is used to display User details
    /// </summary>
    private void FunGetCreditScoreParameterDetails()
    {
        ObjCreditScoreClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            dtCreditScore = new DataTable();
            DataRow dr;
            dtCreditScore.Columns.Add("CrScoreParam_ID");
            dtCreditScore.Columns.Add("Desc");
            dtCreditScore.Columns.Add("FieldAtt");
            dtCreditScore.Columns.Add("NumericAtt");
            dtCreditScore.Columns.Add("DiffPer");
            dtCreditScore.Columns.Add("DiffMark");

            dtCreditScore.Columns["DiffPer"].DataType = typeof(string);
            dtCreditScore.Columns["DiffMark"].DataType = typeof(string);

            for (int intColCount = 1; intColCount <= intNoofYears; intColCount++)
            {
                dtCreditScore.Columns.Add("ReqValue" + intColCount.ToString());
                dtCreditScore.Columns.Add("Score" + intColCount.ToString());
            }
            dr = dtCreditScore.NewRow();
            dtCreditScore.Rows.Add(dr);

            if ((intCreditScoreID == 0) && (hdnCreditScoreID.Value == "0"))
            {
                ViewState["CreditScoreYr"] = dtCreditScore;
                grvCreditScore.DataSource = dtCreditScore;
                grvCreditScore.DataBind();
                grvCreditScore.Rows[0].Visible = false;
                //for (int intColCount = 14; intColCount > 4; intColCount--)
                //{
                //    grvCreditScore.Columns[intColCount].Visible = false;
                //}

            }
            else
            {


                ObjS3G_ORG_CreditScoreDataTable = new CreditMgtServices.S3G_ORG_CreditScoreDataTable();
                CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow = null;

                ObjCreditScoreRow = ObjS3G_ORG_CreditScoreDataTable.NewS3G_ORG_CreditScoreRow();

                ObjCreditScoreRow.Company_ID = intCompanyID;
                ObjCreditScoreRow.Created_By = intUserID;
                ObjCreditScoreRow.CreditScore_Guide_ID = Convert.ToInt32(hdnCreditScoreID.Value);
                ObjCreditScoreRow.YearValue = rbtYears.Items[0].Value;

                ObjS3G_ORG_CreditScoreDataTable.AddS3G_ORG_CreditScoreRow(ObjCreditScoreRow);

                byte[] byteCreditScoreDetails = ObjCreditScoreClient.FunPubQueryCreditScoreParameterDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_ORG_CreditScoreDataTable, SerMode));
                dsCreditScore = (DataSet)ClsPubSerialize.DeSerialize(byteCreditScoreDetails, SerializationMode.Binary, typeof(DataSet));

                if ((intCreditScoreID > 0) && (dsCreditScore.Tables[0].Rows.Count > 0))
                {
                    ddlLOB.SelectedValue = dsCreditScore.Tables[0].Rows[0]["LOB_ID"].ToString();
                    FunPriBindConstitutionProduct();
                    ddlProductCode.SelectedValue = dsCreditScore.Tables[0].Rows[0]["Product_ID"].ToString();
                    ddlConstitution.SelectedValue = dsCreditScore.Tables[0].Rows[0]["Constitution_ID"].ToString();
                    chkActive.Checked = Convert.ToBoolean(dsCreditScore.Tables[0].Rows[0]["Is_Active"].ToString());
                    hdnCanEdit.Value = dsCreditScore.Tables[0].Rows[0]["CanEdit"].ToString();
                }
                //if ((dsCreditScore.Tables[1].Rows.Count > 0))
                //{
                if (dsCreditScore.Tables[1].Rows.Count > 0)
                {
                    dtCreditScore = dsCreditScore.Tables[1].Copy();
                    ViewState["CreditScoreYr"] = dtCreditScore;
                    FunPriBindLookup();
                    for (int i = 0; i <= dtCreditScore.Rows.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(dtCreditScore.Rows[i]["Score"].ToString()) && Convert.ToDecimal(dtCreditScore.Rows[i]["Score"].ToString()) == 0)
                        {
                            dtCreditScore.Rows[i]["Score"] = "";
                        }
                    }
                    grvCreditScore.DataSource = dtCreditScore;
                    grvCreditScore.DataBind();
                }
                else
                {
                    FunPriBindLookup();
                    ViewState["CreditScoreYr"] = dtCreditScore;
                    grvCreditScore.DataSource = dtCreditScore;
                    grvCreditScore.DataBind();
                    grvCreditScore.Rows[0].Visible = false;
                }
                if (dsCreditScore.Tables[2].Rows.Count > 0)
                {
                    int intNoofYear = Convert.ToInt32(dsCreditScore.Tables[0].Rows[0]["YearsPresent"].ToString());
                    ViewState["CreditScoreYear"] = dsCreditScore.Tables[2];
                    if (dsCreditScore.Tables[2].Rows.Count > 0)
                    {
                        dsCreditScore.Tables[2].Rows.Add();
                        grvCreditScoreYearValues.DataSource = FunSumScore(dsCreditScore.Tables[2]);
                        grvCreditScoreYearValues.DataBind();

                        for (int i = 0; i <= grvCreditScoreYearValues.Columns.Count - 1; i++)
                        {
                            grvCreditScoreYearValues.Rows[grvCreditScoreYearValues.Rows.Count - 1].Cells[i].Font.Bold = true;
                            grvCreditScoreYearValues.Rows[grvCreditScoreYearValues.Rows.Count - 1].Cells[i].CssClass = "styleGridHeader";
                        }
                    }
                    else
                    {
                        grvCreditScoreYearValues.DataSource = dsCreditScore.Tables[2];
                        grvCreditScoreYearValues.DataBind();
                    }
                    for (int intColCount = 14; intColCount > 4; intColCount--)
                    {
                        grvCreditScoreYearValues.Columns[intColCount].Visible = true;
                    }
                    int intColVisbile = 2 + (intNoofYear * 2);
                    for (int intColCount = 14; intColCount > intColVisbile; intColCount--)
                    {
                        grvCreditScoreYearValues.Columns[intColCount].Visible = false;
                    }
                }

                FunPriBindYear(Convert.ToInt32(dsCreditScore.Tables[0].Rows[0]["YearsPresent"].ToString()));


            }
            if (ddlYear.SelectedIndex > 0)
            {
                grvCreditScore.Columns[8].Visible = false;
            }
            else
            {
                grvCreditScore.Columns[8].Visible = true;
            }

            if (strMode == "")
            {
                if (ddlYear.SelectedIndex == ddlYear.Items.Count - 1)
                {
                    if (bCreate)
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        grvCreditScore.FooterRow.Visible = false;
                        grvCreditScore.Columns[8].Visible = false;
                    }
                }
                else
                {
                    btnSave.Enabled = false;
                    grvCreditScore.FooterRow.Visible = false;
                    grvCreditScore.Columns[8].Visible = false;
                }
            }
            else
            {
                //Based on UAT

                //grvCreditScore.FooterRow.Visible = false;
                //grvCreditScore.Columns[7].Visible = false;
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
            dsCreditScore.Dispose();
            dsCreditScore = null;
            ObjCreditScoreClient.Close();
        }
    }

    private DataTable FunSumScore(DataTable tblCreditScore)
    {
        try
        {
            decimal dblScore1, dblScore2, dblScore3, dblScore4, dblScore5, dblScore6;
            dblScore1 = dblScore2 = dblScore3 = dblScore4 = dblScore5 = dblScore6 = 0;

            for (int i = 0; i <= tblCreditScore.Rows.Count - 2; i++)
            {
                DataRow DRow = tblCreditScore.Rows[i];

                if (!(string.IsNullOrEmpty(DRow["Score"].ToString())))
                {
                    dblScore1 += Convert.ToDecimal(DRow["Score"].ToString());
                }
                if (!(string.IsNullOrEmpty(DRow["Score2"].ToString())))
                {
                    dblScore2 += Convert.ToDecimal(DRow["Score2"].ToString());
                }
                if (!(string.IsNullOrEmpty(DRow["Score3"].ToString())))
                {
                    dblScore3 += Convert.ToDecimal(DRow["Score3"].ToString());
                }
                if (!(string.IsNullOrEmpty(DRow["Score4"].ToString())))
                {
                    dblScore4 += Convert.ToDecimal(DRow["Score4"].ToString());
                }
                if (!(string.IsNullOrEmpty(DRow["Score5"].ToString())))
                {
                    dblScore5 += Convert.ToDecimal(DRow["Score5"].ToString());
                }
                if (!(string.IsNullOrEmpty(DRow["Score6"].ToString())))
                {
                    dblScore6 += Convert.ToDecimal(DRow["Score6"].ToString());
                }
            }

            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Description"] = "Total";
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score1"] = dblScore1.ToString();
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score2"] = dblScore2.ToString();
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score3"] = dblScore3.ToString();
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score4"] = dblScore4.ToString();
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score5"] = dblScore5.ToString();
            tblCreditScore.Rows[tblCreditScore.Rows.Count - 1]["Score6"] = dblScore6.ToString();

            return tblCreditScore;

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private bool FunPriGenerateCreditScoreParameterXMLDet()
    {
        try
        {

            string strDesc = string.Empty;
            string strFieldAtt = string.Empty;
            string strNumericAtt = string.Empty;
            string strReqValue = string.Empty;
            string strScore = string.Empty;
            string strDiffPer = string.Empty;
            string strDiffMark = string.Empty;
            string strCrScoreParam_ID = string.Empty;

            TextBox txtDesc = null;
            DropDownList ddlFieldAtt = null;
            DropDownList ddlNumericAtt = null;
            TextBox txtReqVal = null;
            TextBox txtScore = null;
            TextBox txtDiffPer = null;
            TextBox txtDiffMark = null;
            Label lblParamID = null;
            DropDownList ddlYes = null;

            dtCreditScore = (DataTable)ViewState["CreditScore"];
            strbCreditScoreDet.Append("<Root>");

            foreach (GridViewRow grvData in grvCreditScore.Rows)
            {

                lblParamID = ((Label)grvData.FindControl("lblParamID"));
                txtDesc = ((TextBox)grvData.FindControl("txtDesc"));
                ddlFieldAtt = ((DropDownList)grvData.FindControl("ddlFieldAtt"));
                ddlNumericAtt = ((DropDownList)grvData.FindControl("ddlNumeric"));
                txtReqVal = ((TextBox)grvData.FindControl("txtReqValue1"));
                txtScore = ((TextBox)grvData.FindControl("txtScore1"));
                txtDiffPer = ((TextBox)grvData.FindControl("txtDiffPer"));
                txtDiffMark = ((TextBox)grvData.FindControl("txtDiffMark"));
                ddlYes = (DropDownList)grvData.FindControl("ddlYes1");
                lblYear = ((Label)grvData.FindControl("lblYear"));
                //txtReqVal.Visible = false;
                //ddlYes.Visible = false;

                //if (txtDesc.Text.Trim() == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Description');", true);
                //    txtDesc.Focus();
                //    return false;

                //}
                //else if (ddlFieldAtt.SelectedIndex == 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Field Attribute');", true);
                //    ddlFieldAtt.Focus();
                //    return false;

                //}
                //else if ((ddlFieldAtt.SelectedValue != "4") && (ddlNumericAtt.SelectedIndex == 0))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select the Numeric Operator');", true);
                //    ddlNumericAtt.Focus();
                //    return false;

                //}
                //if ((ddlFieldAtt.SelectedValue == "3"))
                //{
                //    if (txtReqVal.Text.Contains(":"))
                //    {
                //        if (txtReqVal.Text.Length == txtReqVal.Text.IndexOf(':'))
                //        {
                //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                //            txtReqVal.Focus();

                //        }

                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid ratio');", true);
                //        txtReqVal.Focus();

                //    }

                //}


                //if ((ddlFieldAtt.SelectedValue == "1"))
                //{
                //    string[] temp;
                //    temp = (txtReqVal.Text).Split('.');
                //    if (temp.Length > 2)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                //        txtReqVal.Focus();
                //        return false;
                //    }

                //}
                //if ((ddlFieldAtt.SelectedValue == "1"))
                //{
                //    double temp;
                //    temp = Convert.ToDouble(txtReqVal.Text);
                //    if (temp == 0)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the valid Amount');", true);
                //        txtReqVal.Focus();
                //        return false;

                //    }
                //}

                //if (ddlFieldAtt.SelectedValue == "4")
                //{
                //    if (ddlYes.SelectedIndex == 0)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Select Yes or No');", true);
                //        ddlYes.Focus();
                //        return false;
                //    }

                //}

                //if ((txtReqVal.Text.Trim() == string.Empty) && (ddlFieldAtt.SelectedValue != "4"))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Required Value');", true);
                //    txtReqVal.Focus();
                //    return false;
                //}


                //if (txtScore.Text.Trim() == string.Empty)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Score');", true);
                //    txtScore.Focus();
                //    return false;

                //}

                //if ((ddlFieldAtt.SelectedValue != "4") && (ddlFieldAtt.SelectedValue != "5") && (txtDiffPer.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference %');", true);
                //    txtDiffPer.Focus();
                //    return false;

                //}
                //if ((ddlFieldAtt.SelectedValue != "4") && (ddlFieldAtt.SelectedValue != "5") && (txtDiffMark.Text.Trim() == string.Empty))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Enter the Difference Mark');", true);
                //    txtDiffMark.Focus();
                //    return false;
                //}

                //if ((ddlFieldAtt.SelectedValue == "2") && (txtReqVal.Text.Length >= 3))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                //    txtReqVal.Focus();
                //    return false;
                //}

                //if ((ddlFieldAtt.SelectedValue == "2") && Convert.ToDecimal(txtReqVal.Text) >= 100)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Percentage value should not be greater than or equal to 100');", true);
                //    txtReqVal.Focus();
                //    return false;
                //}

                strCrScoreParam_ID = lblParamID.Text;
                strDesc = txtDesc.Text.Trim();
                strFieldAtt = ddlFieldAtt.SelectedValue;
                strNumericAtt = ddlNumericAtt.SelectedValue;

                if (ddlFieldAtt.SelectedValue == "4")
                {
                    strReqValue = ddlYes.SelectedIndex == 0 ? "-1" : ddlYes.SelectedValue;
                }
                else if (ddlFieldAtt.SelectedValue == "5")
                {
                    strReqValue = "-1";
                    if (!string.IsNullOrEmpty(txtReqVal.Text))
                    {
                        strReqValue = txtReqVal.Text.Trim() == "" ? "-1" : Utility.StringToDate(txtReqVal.Text.ToString()).ToString();
                    }

                    //if (!string.IsNullOrEmpty(txtReqVal.Text))
                    //{
                    //    DateTime dt = new DateTime();
                    //    if (DateTime.TryParse(txtReqVal.Text.Trim(), out dt))
                    //    {
                    //        strReqValue = txtReqVal.Text.Trim() == "" ? "-1" : Utility.StringToDate(dt.ToString()).ToString();
                    //    }
                    //    else
                    //    {
                    //        Utility.FunShowAlertMsg(this, "Required date is not in correct format");
                    //        txtReqVal.Focus();
                    //        return false;
                    //    }
                    //}
                }
                else
                {
                    strReqValue = txtReqVal.Text.Trim() == "" ? "-1" : txtReqVal.Text.Trim();
                }
                //strReqValue = txtReqVal.Text;
                strScore = txtScore.Text == "" ? "-1" : txtScore.Text;
                strDiffPer = txtDiffPer.Text == "" ? "-1" : txtDiffPer.Text;
                strDiffMark = txtDiffMark.Text == "" ? "-1" : txtDiffMark.Text;

                //Calendar


                //strDesc = dtCreditScore.Rows[iCount]["Desc"].ToString();
                //strFieldAtt = dtCreditScore.Rows[iCount]["FieldAtt"].ToString();
                //strNumericAtt = dtCreditScore.Rows[iCount]["NumericAtt"].ToString();
                //strDiffPer = dtCreditScore.Rows[iCount]["DiffPer"].ToString();
                //strDiffMark = dtCreditScore.Rows[iCount]["DiffMark"].ToString();
                //strReqValue = dtCreditScore.Rows[iCount]["ReqValue1"].ToString();
                //strScore = dtCreditScore.Rows[iCount]["Score1"].ToString();

                strbCreditScoreDet.Append(" <Details CrScoreParam_ID='" + strCrScoreParam_ID + "' CrScoreDesc='" + strDesc + "' FieldAttribute='" + strFieldAtt + "'");
                strbCreditScoreDet.Append(" NumericOperator='" + strNumericAtt + "' DifferencePercentage='" + strDiffPer + "'");
                strbCreditScoreDet.Append(" DifferenceMark='" + strDiffMark + "'");
                strbCreditScoreDet.Append(" Year='" + lblYear.Text + "'");
                strbCreditScoreDet.Append(" RequiredParameter='" + strReqValue + "' Score='" + strScore + "' />");
            }
            strbCreditScoreDet.Append("</Root>");
            strXMLCreditScoreDet = strbCreditScoreDet.ToString();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //This is used to implement User Authorization

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                chkActive.Enabled = false;
                if (!bCreate)
                {

                    btnSave.Enabled = false;
                    grvCreditScore.Columns[8].Visible = false;
                }

                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnSave.Enabled = true;
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                chkActive.Enabled = true;
                ddlLOB.Enabled = false;
                ddlProductCode.Enabled = false;
                ddlConstitution.Enabled = false;
                txtNoofYears.ReadOnly = txtNxtYears.ReadOnly = txtPrvYears.ReadOnly = true;
                btnYrGo.Visible = false;
                btnClear.Enabled = false;
                btnAddYear.Visible = false;
                rdbLevel.Enabled = false;
                rfvLOB.Enabled = false;
                break;

            case -1:// Query Mode

                rdbLevel.Enabled = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage, false);
                }

                if (bClearList)
                {
                    ddlYear.ClearDropDownList();
                    ddlLOB.ClearDropDownList();
                    if (ddlProductCode.Items.Count > 0)
                    {
                        ddlProductCode.ClearDropDownList();
                    }
                    ddlConstitution.ClearDropDownList();
                }
                btnYrGo.Visible = false;
                grvCreditScore.Columns[8].Visible = false;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                chkActive.Enabled = false;

                foreach (GridViewRow e in grvNumbers.Rows)
                {
                    TextBox txtValue = (TextBox)e.FindControl("txtValue");
                    Label lblValue = (Label)e.FindControl("lblValue");
                    lblValue.Visible = true;
                    txtValue.Visible = false;
                }

                foreach (GridViewRow e in grvCreditScore.Rows)
                {
                    if (e.RowType == DataControlRowType.DataRow)
                    {
                        TextBox txtDesc = (TextBox)e.FindControl("txtDesc");
                        txtDesc.ReadOnly = true;
                        DropDownList ddlField = (DropDownList)e.FindControl("ddlFieldAtt");
                        ddlField.Enabled = false;
                        DropDownList ddlNumeric = (DropDownList)e.FindControl("ddlNumeric");
                        ddlNumeric.Enabled = false;
                        DropDownList ddlYes = (DropDownList)e.FindControl("ddlYes1");
                        ddlYes.Enabled = false;
                        TextBox txtReqV = (TextBox)e.FindControl("txtReqValue1");
                        txtReqV.ReadOnly = true;
                        TextBox txtScore = (TextBox)e.FindControl("txtScore1");
                        txtScore.ReadOnly = true;
                        TextBox txtdiffPerv = (TextBox)e.FindControl("txtDiffPer");
                        txtdiffPerv.ReadOnly = true;
                        TextBox txtdiffMark = (TextBox)e.FindControl("txtDiffMark");
                        txtdiffMark.ReadOnly = true;
                        AjaxControlToolkit.CalendarExtender CalendarExtender1 = (AjaxControlToolkit.CalendarExtender)e.FindControl("CalendarExtender1");
                        CalendarExtender1.Enabled = false;
                    }
                }

                break;
        }

    }

    //Code end


    private string Funsetsuffix()
    {

        int suffix = 1;
        S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    protected void Score_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txtScore = (TextBox)sender;
            //if (txtScore.Text != string.Empty)
            //{
            //    txtScore.Text = Convert.ToDecimal(txtScore.Text).ToString(Funsetsuffix());
            //}

            if (txtTotalScore.Text != string.Empty)
            {
                txtTotalScore.Text = Convert.ToDecimal(txtTotalScore.Text).ToString(Funsetsuffix());
            }

            foreach (GridViewRow GRow in grvCreditScore.Rows)
            {
                TextBox txtDescF = (TextBox)GRow.FindControl("txtDesc");
                DropDownList ddlField = (DropDownList)GRow.FindControl("ddlFieldAtt");
                DropDownList ddlNumeric = (DropDownList)GRow.FindControl("ddlNumeric");
                TextBox txtDiffPer = (TextBox)GRow.FindControl("txtDiffPer");
                TextBox txtDiffMark = (TextBox)GRow.FindControl("txtDiffMark");

                //For 'Amount' and '=' ,difference mark and per not needed.
                if ((ddlField.SelectedValue == "1" || ddlField.SelectedValue == "6") && (ddlNumeric.SelectedValue == "5"))
                {
                    txtDiffPer.Text = string.Empty;
                    txtDiffMark.Text = string.Empty;
                    txtDiffPer.ReadOnly = true;
                    txtDiffMark.ReadOnly = true;
                    //ddlNumericF.Focus();
                    //txtDiffMark.Attributes.Add("ContentEditable", "false");
                    //txtDiffPer.Attributes.Add("ContentEditable", "false");
                    return;
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void FunProLoadCreditScoreDetails()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@CreditScore_ID", intCreditScoreID.ToString());

        DataSet dsScore = Utility.GetDataset("S3G_ORG_GetCreditScoreParameterMaster", Procparam);

        ListItem lst;
        lst = new ListItem(dsScore.Tables[0].Rows[0]["LOB_Name"].ToString(), dsScore.Tables[0].Rows[0]["LOB_ID"].ToString());
        ddlLOB.Items.Add(lst);

        ddlLOB.SelectedValue = dsScore.Tables[0].Rows[0]["LOB_ID"].ToString();
        //FunPriBindConstitutionProduct();

        lst = new ListItem(dsScore.Tables[0].Rows[0]["Product_Name"].ToString(), dsScore.Tables[0].Rows[0]["Product_ID"].ToString());
        ddlProductCode.Items.Add(lst);

        ddlProductCode.SelectedValue = dsScore.Tables[0].Rows[0]["Product_ID"].ToString();

        lst = new ListItem(dsScore.Tables[0].Rows[0]["Constitution_Name"].ToString(), dsScore.Tables[0].Rows[0]["Constitution_ID"].ToString());
        ddlConstitution.Items.Add(lst);

        ddlConstitution.SelectedValue = dsScore.Tables[0].Rows[0]["Constitution_ID"].ToString();
        chkActive.Checked = Convert.ToBoolean(dsScore.Tables[0].Rows[0]["Is_Active"].ToString());
        txtNoofYears.Text = dsScore.Tables[0].Rows[0]["No_Of_Years"].ToString();
        txtPrvYears.Text = dsScore.Tables[0].Rows[0]["Past_Year"].ToString();
        txtNxtYears.Text = dsScore.Tables[0].Rows[0]["Future_Year"].ToString();
        hdnCanEdit.Value = dsScore.Tables[0].Rows[0]["CanEdit"].ToString();
        ViewState["CreditScore"] = dsScore.Tables[1];
        ViewState["dtNumber"] = dsScore.Tables[2];
        ViewState["dtProcessed"] = dsScore.Tables[3];

        ddlFinanceYear.BindDataTable(dsScore.Tables[3], new string[] { "Year", "Year" });

        rbtYears.Items.Clear();
        for (int i = 1; i <= ddlFinanceYear.Items.Count - 1; i++)
        {
            rbtYears.Items.Add(ddlFinanceYear.Items[i]);
        }

        rbtYears.SelectedValue = rbtYears.Items[0].Value;
        rbtYears_SelectedIndexChanged(null, null);

        FunGetFormulaString();

        if (rbtYears.Items.Count == 1 && PageMode != PageModes.Query)
        {
            btnUpdateYear.Visible = true;
        }


        if (ddlLOB.SelectedValue.ToString()!="0")
        {
            rdbLevel.SelectedValue= "1";
        }
        else
        {
            rdbLevel.SelectedValue = "2";
        }

        //for (int i = 0; i <= dsScore.Tables[3].Rows.Count - 1; i++)
        //{
        //    ListItem lst = new ListItem(dsScore.Tables[2].Rows[i]["Year"].ToString(), dsScore.Tables[2].Rows[i]["Year"].ToString());
        //    rbtYears.Items.Add(lst);
        //}
    }

    protected void FunProInitializeMainGrid()
    {
        try
        {
            DataTable CreditScoreYr = new DataTable();
            DataRow dr;
            CreditScoreYr.Columns.Add("CrScoreParam_ID");
            CreditScoreYr.Columns.Add("Desc");
            CreditScoreYr.Columns.Add("FieldAtt");
            CreditScoreYr.Columns.Add("NumericAtt");
            CreditScoreYr.Columns.Add("DiffPer");
            CreditScoreYr.Columns.Add("DiffMark");

            CreditScoreYr.Columns["DiffPer"].DataType = typeof(string);
            CreditScoreYr.Columns["DiffMark"].DataType = typeof(string);

            CreditScoreYr.Columns.Add("ReqValue");
            CreditScoreYr.Columns.Add("Score");
            CreditScoreYr.Columns.Add("Year");

            dr = CreditScoreYr.NewRow();
            CreditScoreYr.Rows.Add(dr);

            FunPriBindLookup();
            grvCreditScore.DataSource = CreditScoreYr;
            grvCreditScore.DataBind();

            CreditScoreYr.Rows[0].Delete();

            ViewState["CreditScore"] = CreditScoreYr;
            ViewState["CreditScoreYr"] = CreditScoreYr;

            grvCreditScore.Rows[0].Visible = false;
            grvCreditScore.Columns[grvCreditScore.Columns.Count - 1].Visible = true;
        }
        catch (Exception ex)
        {

            throw;
        }
    }



    protected void grvNumbers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddNew")
        {
            DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];
            DataRow dRow = dtNumber.NewRow();

            DropDownList ddlFLineType = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFLineType");
            DropDownList ddlFFlag = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFlag");
            AjaxControlToolkit.ComboBox txtFDesc = (AjaxControlToolkit.ComboBox)grvNumbers.FooterRow.FindControl("txtFDesc");
            TextBox txtFValue = (TextBox)grvNumbers.FooterRow.FindControl("txtFValue");
            TextBox txtFFormula = (TextBox)grvNumbers.FooterRow.FindControl("txtFFormula");
            CheckBox chkFLink = (CheckBox)grvNumbers.FooterRow.FindControl("chkFLink");
            DropDownList ddlFParamNum = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFParamNum");
            DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");
            Label lblFDesc = (Label)grvNumbers.FooterRow.FindControl("lblFDesc");
            CheckBox chkFCarryFml = (CheckBox)grvNumbers.FooterRow.FindControl("chkFCarryFml");

            Regex rx = new Regex(@"^((?<o1>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))[-+*/])+(?<o2>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))(?(p)(?!))$");
            decimal decScore = 0;

            if (txtFFormula.Text != "")
            {
                string strFormatedfml = FunProApplyFormula(txtFFormula.Text, ddlFFmlLines);
                if ((!rx.IsMatch(strFormatedfml.Replace("?", "+")) && !decimal.TryParse(strFormatedfml.Replace("?", "+"), out decScore)))
                {
                    Utility.FunShowAlertMsg(this, "Formula is not well formed");
                    return;
                }
                else
                {
                    var result = Convert.ToDecimal(0.0);
                    if (strFormatedfml.Contains("?"))
                    {
                        result = FunProGetMaxValue(strFormatedfml);
                    }
                    else
                    {
                        DataTable tempTable = new DataTable();
                        result = Convert.ToDecimal(tempTable.Compute(strFormatedfml, ""));
                    }
                    txtFValue.Text = result.ToString(Utility.SetSuffix());
                }
            }

            dRow["LineType"] = ddlFLineType.SelectedItem.Text;
            dRow["LineTypeID"] = ddlFLineType.SelectedValue;
            if (ddlFFlag.SelectedValue == "5")
            {
                dRow["Desc"] = txtFDesc.SelectedItem.Text;
            }
            else
            {
                dRow["Desc"] = lblFDesc.Text;
            }
            dRow["Flag_ID"] = ddlFFlag.SelectedValue;
            dRow["Value"] = txtFValue.Text;
            dRow["Formula"] = txtFFormula.Text;
            dRow["Flag"] = ddlFFlag.SelectedItem.Text;
            if (chkFLink.Checked)
                dRow["IsLink"] = "1";
            else
                dRow["IsLink"] = "0";
            if (ddlFParamNum.Items.Count > 0)
            {
                dRow["ParamNum"] = ddlFParamNum.SelectedValue;
                dRow["ParamText"] = ddlFParamNum.SelectedItem.Text;
            }
            else
            {
                dRow["ParamNum"] = "0";
                dRow["ParamText"] = "";
            }

            if (chkFCarryFml.Checked)
                dRow["CarryFml"] = "1";
            else
                dRow["CarryFml"] = "0";

            dRow["Year"] = rbtYears.SelectedValue;

            dtNumber.Rows.Add(dRow);

            for (int i = 0; i <= dtNumber.Rows.Count - 1; i++)
            {
                dtNumber.Rows[i]["RecordId"] = i.ToString();
            }

            grvNumbers.DataSource = dtNumber;
            grvNumbers.DataBind();
            FunProLoadNumFooterDDL(null);

            ViewState["dtNumberYr"] = dtNumber;
        }
    }

    protected decimal FunProGetMaxValue(string strFormula)
    {
        List<decimal> dcArray = strFormula.Split('?').ToList().ConvertAll<decimal>(s => Convert.ToDecimal(s));
        dcArray.Sort(); dcArray.Reverse();
        return dcArray[0];
    }

    protected string FunProApplyFormula(string strForumula, DropDownList ddlLines)
    {
        DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];

        if (dtNumber.Rows.Count > 0)
        {
            for (int i = 1; i <= ddlLines.Items.Count - 1; i++)
            {
                string strRplcVal = dtNumber.Rows[i - 1]["Value"].ToString();
                if (strRplcVal == "")
                {
                    strRplcVal = "0";
                }
                strForumula = strForumula.Replace("(" + ddlLines.Items[i].Text + ")", strRplcVal);
            }
        }

        return strForumula;
    }

    protected void FunGetFormulaString()
    {
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");

        for (int gv = 0; gv <= grvNumbers.Rows.Count - 1; gv++)
        {
            Label lblFormula = (Label)grvNumbers.Rows[gv].FindControl("lblFormula");

            //lblFormula.Text = lblFormula.Text.Replace("<<", "<b><font color=\"green\" >&lt;&lt;</font></b>").Replace(">>", "<b><font color=\"green\" >&gt;&gt;</font></b>");

            for (int i = 1; i <= ddlFFmlLines.Items.Count - 1; i++)
            {
                lblFormula.Text = lblFormula.Text.Replace("(" + ddlFFmlLines.Items[i].Text + ")", "<font color=\"green\" >(" + ddlFFmlLines.Items[i].Text + ")</font>");
            }
        }
    }

    protected void grvNumbers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];
        dtNumber.Rows.RemoveAt(e.RowIndex);

        grvNumbers.DataSource = dtNumber;
        grvNumbers.DataBind();

        for (int i = 0; i <= dtNumber.Rows.Count - 1; i++)
        {
            dtNumber.Rows[i]["RecordId"] = i.ToString();
        }

        ViewState["dtNumberYr"] = dtNumber;

        if (dtNumber.Rows.Count == 0)
        {
            FunProInitializNumberGrid();
        }
        else
        {
            FunProLoadNumFooterDDL(null);
        }
    }

    protected void grvNumbers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            HtmlButton btnAddfml = (HtmlButton)e.Row.FindControl("btnAddfml");
            DropDownList ddlFFmlLines = (DropDownList)e.Row.FindControl("ddlFFmlLines");
            TextBox txtFFormula = (TextBox)e.Row.FindControl("txtFFormula");
            btnAddfml.Attributes.Add("onclick", "javascript:fnAppendFormula('" + txtFFormula.ClientID + "', '" + ddlFFmlLines.ClientID + "')");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblLink = (Label)e.Row.FindControl("lblLink");
            CheckBox chkLink = (CheckBox)e.Row.FindControl("chkLink");
            Label lblCarryFml = (Label)e.Row.FindControl("lblCarryFml");
            CheckBox chkCarryFml = (CheckBox)e.Row.FindControl("chkCarryFml");

            chkCarryFml.Visible = false;

            chkLink.Attributes.Add("onclick", "javascript:fnChkFreeze('" + chkLink.ClientID + "')");

            if (lblLink.Text == "1")
            {
                chkLink.Checked = true;

                if (PageMode != PageModes.Query)
                {
                    chkCarryFml.Visible = true;
                    if (lblCarryFml.Text == "1")
                        chkCarryFml.Checked = true;
                    else
                        chkCarryFml.Checked = false;
                }
            }
        }
    }

    protected void FunProInitializNumberGrid()
    {
        DataTable dtNumber = new DataTable();
        dtNumber.Columns.Add("LineType");
        dtNumber.Columns.Add("LineTypeID");
        dtNumber.Columns.Add("Desc");
        dtNumber.Columns.Add("Flag_ID");
        dtNumber.Columns.Add("Value");
        dtNumber.Columns.Add("Formula");
        dtNumber.Columns.Add("Flag");
        dtNumber.Columns.Add("IsLink");
        dtNumber.Columns.Add("ParamNum");
        dtNumber.Columns.Add("ParamText");
        dtNumber.Columns.Add("Year");
        dtNumber.Columns.Add("RecordId");
        dtNumber.Columns.Add("CarryFml");

        DataRow dRow = dtNumber.NewRow();
        dtNumber.Rows.Add(dRow);

        grvNumbers.DataSource = dtNumber;
        grvNumbers.DataBind();

        grvNumbers.Rows[0].Visible = false;
        dtNumber.Rows.Clear();

        ViewState["dtNumber"] = dtNumber;
        ViewState["dtNumberYr"] = dtNumber;

        FunProLoadNumFooterDDL(null);

        DataTable dtProcessed = new DataTable();
        dtProcessed.Columns.Add("ID");
        dtProcessed.Columns.Add("Year");

        ViewState["dtProcessed"] = dtProcessed;

        btnAddYear.Enabled = false;
        btnAddYear.Visible = true;
        btnUpdateYear.Visible = false;

        grvNumbers.FooterRow.Visible = true;
        grvNumbers.Columns[grvNumbers.Columns.Count - 1].Visible = true;
    }

    protected void FunProLoadNumFooterDDL(DataSet Dset)
    {
        DropDownList ddlFLineType = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFLineType");
        DropDownList ddlFFlag = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFlag");
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");

        if (Dset == null)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@Is_Number", "1");

            Dset = Utility.GetDataset("S3G_ORG_GetCreidtScoreFlags", Procparam);
        }

        ddlFFlag.BindDataTable(Dset.Tables[0], new string[] { "Flag_ID", "Flag_Code" });
        ddlFLineType.BindDataTable(Dset.Tables[1], new string[] { "Lookup_Code", "Lookup_Description" });

        DataTable dtLinks = new DataTable();
        dtLinks.Columns.Add("Value");
        dtLinks.Columns.Add("Text");
        DataRow dRow;
        DataTable dtNumber = (DataTable)ViewState["dtNumberYr"];
        for (int i = 0; i <= dtNumber.Rows.Count - 1; i++)
        {
            dRow = dtLinks.NewRow();

            dRow["Value"] = (i + 1).ToString();
            dRow["Text"] = (i + 1).ToString() + "." + dtNumber.Rows[i]["Flag"].ToString();

            dtLinks.Rows.Add(dRow);
        }

        ddlFFmlLines.BindDataTable(dtLinks, new string[] { "Value", "Text" });

        FunGetFormulaString();
    }

    protected void ddlFFlag_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFFlag = ((DropDownList)sender);
        AjaxControlToolkit.ComboBox txtFDesc = (AjaxControlToolkit.ComboBox)grvNumbers.FooterRow.FindControl("txtFDesc");
        Label lblFDesc = (Label)grvNumbers.FooterRow.FindControl("lblFDesc");

        lblFDesc.Text = "";
        txtFDesc.SelectedIndex = -1;
        txtFDesc.Visible = false;

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@Is_Number", "1");

        if (ddlFFlag.SelectedItem.Text == "OTH")
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

    protected void ddlFLineType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFLineType = (DropDownList)sender;
        Panel pnlFFml = (Panel)grvNumbers.FooterRow.FindControl("pnlFFml");
        TextBox txtFFormula = (TextBox)grvNumbers.FooterRow.FindControl("txtFFormula");
        HtmlImage imgPrev1 = (HtmlImage)grvNumbers.FooterRow.FindControl("imgPrev1");
        HtmlImage imgPrev2 = (HtmlImage)grvNumbers.FooterRow.FindControl("imgPrev2");
        RequiredFieldValidator rfvFtxtFFormula = (RequiredFieldValidator)grvNumbers.FooterRow.FindControl("rfvFtxtFFormula");
        RequiredFieldValidator rfvFtxtFValue = (RequiredFieldValidator)grvNumbers.FooterRow.FindControl("rfvFtxtFValue");

        if (ddlFLineType.SelectedValue == "3")
        {
            pnlFFml.Enabled = rfvFtxtFFormula.Enabled = true;
            imgPrev1.Src = "../Images/Prev.gif";
            imgPrev2.Src = "../Images/Prev.gif";
            rfvFtxtFValue.Enabled = false;
        }
        else
        {
            txtFFormula.Text = "";
            pnlFFml.Enabled = rfvFtxtFFormula.Enabled = false;
            imgPrev1.Src = "../Images/Prev_Desabled.gif";
            imgPrev2.Src = "../Images/Prev_Desabled.gif";
            rfvFtxtFValue.Enabled = true;
        }
    }

    protected void chkFLink_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvNumbers", ((CheckBox)sender).ClientID);
        DropDownList ddlFParamNum = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFParamNum");

        if (((CheckBox)sender).Checked)
        {
            DataTable dtLinks = new DataTable();
            dtLinks.Columns.Add("Value");
            dtLinks.Columns.Add("Text");
            DataRow dRow;

            for (int i = 0; i <= grvCreditScore.Rows.Count - 1; i++)
            {
                dRow = dtLinks.NewRow();
                Label lblScoreAssigned = (Label)grvCreditScore.Rows[i].FindControl("lblScoreAssigned");
                DropDownList ddlFieldAtt = (DropDownList)grvCreditScore.Rows[i].FindControl("ddlFieldAtt");
                TextBox txtDesc = (TextBox)grvCreditScore.Rows[i].FindControl("txtDesc");

                if (lblScoreAssigned.Text == "0" && ddlFieldAtt.SelectedValue != "3") // && (ddlFieldAtt.SelectedValue == "1" || ddlFieldAtt.SelectedValue == "6"))
                {
                    dRow["Value"] = (i + 1).ToString();
                    dRow["Text"] = (i + 1).ToString() + " - " + txtDesc.Text;

                    dtLinks.Rows.Add(dRow);
                }
            }

            ddlFParamNum.BindDataTable(dtLinks, new string[] { "Value", "Text" });
        }
        else
        {
            ddlFParamNum.Items.Clear();
        }
    }

    protected void btnYear_Click(object sender, EventArgs e)
    {
        Button btnYear = (Button)sender;

        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];
        DataTable dtNumber = (DataTable)ViewState["dtNumber"];

        DataRow[] drProcessed = dtProcessed.Select("Button_ID='" + btnYear.ID + "'");
        DataRow[] drNumRows = dtNumber.Select("Year='" + btnYear.Text + "'");

        if (drProcessed.Length > 0)
        {
            if (drProcessed.Length > 0)
            {
                FunProSetNxtBtnStyle(btnYear);

                for (int i = 1; i <= 6; i++)
                {
                    Button btnAllYear = (Button)tblYearBtns.FindControl("btnYear" + i.ToString());
                    DataRow[] dr = dtProcessed.Select("Button_ID='" + btnAllYear.ID + "'");
                    if (dr.Length == 0)
                    {
                        btnAllYear.Enabled = false;
                        btnAllYear.Style.Add("color", grvCreditScore.HeaderRow.Cells[0].ForeColor.ToString());
                    }
                }

                DataTable dtFiltered = new DataTable();
                drProcessed.CopyToDataTable<DataRow>(dtFiltered, LoadOption.OverwriteChanges);
                grvNumbers.DataSource = dtFiltered;
                grvNumbers.DataBind();

                FunProLoadNumFooterDDL(null);

            }
            else
            {
                DataRow dRow = dtProcessed.NewRow();
                dRow["ID"] = (dtProcessed.Rows.Count + 1).ToString();
                dRow["Year"] = btnYear.ID;

                dtProcessed.Rows.Add(dRow);
                ViewState["dtProcessed"] = dtProcessed;

                btnYear.Style.Add("font-style", "normal");
                btnYear.Style.Add("font-weight", "bold");
                btnYear.Style.Add("color", "green");
            }
        }

        //RemainingPrincipalRows.CopyToDataTable(dtrpy, LoadOption.Upsert);



        bool blEnableSave = false;

        for (int i = 1; i <= 6; i++)
        {
            Button btnNxtYear = (Button)tblYearBtns.FindControl("btnYear" + i.ToString());
            if (btnNxtYear.Visible && !btnNxtYear.Enabled && ViewState["CurrYear"].ToString() == btnYear.Text)
            {
                blEnableSave = false;
                btnNxtYear.Enabled = true;
                FunProSetNxtBtnStyle(btnNxtYear);
                break;
            }
            else
            {
                blEnableSave = true;
            }
        }

        btnSave.Enabled = blEnableSave;
    }

    protected void btnYrGo_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(txtNoofYears.Text) < Convert.ToInt32(txtPrvYears.Text))
        {
            Utility.FunShowAlertMsg(this, "Number of Previous years cannot exceed Total years");
            return;
        }

        bool blNeedCurYear = true;
        if (txtNoofYears.Text == txtPrvYears.Text)
        {
            blNeedCurYear = false;
        }

        ddlFinanceYear.FillFinancialYears(Convert.ToInt32(txtPrvYears.Text), Convert.ToInt32(txtNxtYears.Text), false, blNeedCurYear);

        rbtYears.Items.Clear();
        for (int i = 0; i <= ddlFinanceYear.Items.Count - 1; i++)
        {
            rbtYears.Items.Add(ddlFinanceYear.Items[i]);
            if (i != 0)
            {
                rbtYears.Items[i].Enabled = false;
            }
        }
        rbtYears.SelectedValue = rbtYears.Items[0].Value;
        FunProInitializNumberGrid();
        FunProInitializeMainGrid();
        tcCreditScore.TabIndex = 1;
    }

    public void FunProSetNxtBtnStyle(Button btnYear)
    {
        btnYear.Enabled = true;
        btnYear.Style.Add("font-style", "italic");
        btnYear.Style.Add("font-weight", "normal");

        ViewState["CurrYear"] = btnYear.Text;
    }

    protected void btnAddYear_Click(object sender, EventArgs e)
    {
        try
        {
            FunProUpdateYearValues();

            DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

            DataRow dRow = dtProcessed.NewRow();
            dRow["ID"] = (dtProcessed.Rows.Count + 1).ToString();
            dRow["Year"] = rbtYears.SelectedValue;

            dtProcessed.Rows.Add(dRow);
            ViewState["dtProcessed"] = dtProcessed;

            FunProAddNewYear();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void FunProAddNewYear()
    {
        bool blIsLastYear = false;

        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];
        if (rbtYears.SelectedIndex != rbtYears.Items.Count - 1)
        {
            rbtYears.SelectedIndex = dtProcessed.Rows.Count;
            rbtYears.Items.FindByValue(rbtYears.SelectedValue).Enabled = true;
        }
        else
        {
            btnSave.Enabled = true;
            blIsLastYear = true;
            btnAddYear.Visible = false;
        }

        //For Number
        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];

        if (!blIsLastYear)
        {
            for (int i = 0; i <= dtNumberYr.Rows.Count - 1; i++)
            {
                dtNumberYr.Rows[i]["Value"] = "";
                dtNumberYr.Rows[i]["Year"] = rbtYears.SelectedValue;
            }
        }
        else if (rbtYears.Items.Count == 1 && PageMode != PageModes.Query)
        {
            btnUpdateYear.Visible = true;
        }
        ViewState["dtNumberYr"] = dtNumberYr;

        if (!blIsLastYear)
        {
            grvNumbers.DataSource = dtNumberYr;
            grvNumbers.DataBind();
            FunProLoadNumFooterDDL(null);
        }

        if (grvNumbers.FooterRow != null)
        {
            grvNumbers.FooterRow.Visible = false;
        }
        grvNumbers.Columns[grvNumbers.Columns.Count - 1].Visible = false;

        FunProToggleFmlControls();


        //For Main grid
        DataTable CreditScoreYr = (DataTable)ViewState["CreditScoreYr"];

        if (!blIsLastYear)
        {
            for (int i = 0; i <= CreditScoreYr.Rows.Count - 1; i++)
            {
                if (CreditScoreYr.Rows[i]["FieldAtt"].ToString() != "4" && CreditScoreYr.Rows[i]["FieldAtt"].ToString() != "5")
                {
                    CreditScoreYr.Rows[i]["ReqValue"] = "";
                    CreditScoreYr.Rows[i]["Score"] = "";
                }
                CreditScoreYr.Rows[i]["Year"] = rbtYears.SelectedValue;
            }
        }
        ViewState["CreditScoreYr"] = CreditScoreYr;

        if (!blIsLastYear)
        {
            FunPriBindLookup();
            grvCreditScore.DataSource = CreditScoreYr;
            grvCreditScore.DataBind();
        }

        grvCreditScore.FooterRow.Visible = false;
        grvCreditScore.Columns[grvCreditScore.Columns.Count - 1].Visible = false;
    }

    protected void FunProUpdateYearValues()
    {
        //For Number

        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];
        for (int i = 0; i <= dtNumberYr.Rows.Count - 1; i++)
        {
            TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");
            CheckBox chkCarryFml = (CheckBox)grvNumbers.Rows[i].FindControl("chkCarryFml");
            dtNumberYr.Rows[i]["Value"] = txtValue.Text;
            if (chkCarryFml.Checked)
            {
                dtNumberYr.Rows[i]["CarryFml"] = "1";
            }
            else
            {
                dtNumberYr.Rows[i]["CarryFml"] = "0";
            }
        }

        DataTable dtNumber = (DataTable)ViewState["dtNumber"];
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        DataTable dtExists = dtNumberYr.Clone();
        DataRow[] drExist = dtNumber.Select("Year<>'" + rbtYears.SelectedValue + "'");
        drExist.CopyToDataTable<DataRow>(dtExists, LoadOption.OverwriteChanges);

        dtExists.Merge(dtNumberYr);
        ViewState["dtNumber"] = dtExists;


        //For Main tab

        DataTable CreditScoreYr = (DataTable)ViewState["CreditScoreYr"];

        for (int i = 0; i <= CreditScoreYr.Rows.Count - 1; i++)
        {
            TextBox txtReqValue1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtReqValue1");
            TextBox txtScore1 = (TextBox)grvCreditScore.Rows[i].FindControl("txtScore1");
            TextBox txtDiffPer = (TextBox)grvCreditScore.Rows[i].FindControl("txtDiffPer");
            TextBox txtDiffMark = (TextBox)grvCreditScore.Rows[i].FindControl("txtDiffMark");
            DropDownList ddlNumeric = (DropDownList)grvCreditScore.Rows[i].FindControl("ddlNumeric");
            DropDownList ddlFieldAtt = (DropDownList)grvCreditScore.Rows[i].FindControl("ddlFieldAtt");

            if (ddlFieldAtt.SelectedValue == "1" || ddlFieldAtt.SelectedValue == "6")
            {
                DataRow[] drApply = dtNumberYr.Select("ISLink='1' AND ParamNum='" + (i + 1).ToString() + "' AND CarryFml='1'");
                if (drApply.Length > 0)
                {
                    foreach (DataRow dRow in drApply)
                        txtReqValue1.Text = dRow["Value"].ToString();
                }
            }

            CreditScoreYr.Rows[i]["NumericAtt"] = ddlNumeric.SelectedValue;
            CreditScoreYr.Rows[i]["DiffPer"] = txtDiffPer.Text;
            CreditScoreYr.Rows[i]["DiffMark"] = txtDiffMark.Text;

            CreditScoreYr.Rows[i]["ReqValue"] = txtReqValue1.Text;
            CreditScoreYr.Rows[i]["Score"] = txtScore1.Text;
        }

        DataTable CreditScore = (DataTable)ViewState["CreditScore"];

        DataTable dtExistsCS = CreditScoreYr.Clone();
        DataRow[] drExistsCS = CreditScore.Select("Year<>'" + rbtYears.SelectedValue + "'");
        drExistsCS.CopyToDataTable<DataRow>(dtExistsCS, LoadOption.OverwriteChanges);

        dtExistsCS.Merge(CreditScoreYr);
        ViewState["CreditScore"] = dtExistsCS;
    }

    protected void rbtYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        for (int i = rbtYears.Items.Count - 1; i >= dtProcessed.Rows.Count; i--)
        {
            rbtYears.Items[i].Enabled = false;
        }

        btnAddYear.Visible = false;
        if (PageMode != PageModes.Query)
        {
            btnUpdateYear.Visible = true;
        }

        //For Number

        DataTable dtNumber = (DataTable)ViewState["dtNumber"];
        DataRow[] drNumRows = dtNumber.Select("Year='" + rbtYears.SelectedValue + "'");

        DataTable dtNumberYr = dtNumber.Clone();
        drNumRows.CopyToDataTable<DataRow>(dtNumberYr, LoadOption.OverwriteChanges);

        if (dtNumberYr.Rows.Count == 0)
        {
            FunProInitializNumberGrid();
        }
        else
        {
            ViewState["dtNumberYr"] = dtNumberYr;
            grvNumbers.DataSource = dtNumberYr;
            grvNumbers.DataBind();
            FunProLoadNumFooterDDL(null);
        }

        FunProToggleFmlControls();
        grvNumbers.FooterRow.Visible = false;
        grvNumbers.Columns[grvNumbers.Columns.Count - 1].Visible = false;


        //For Main tab

        DataTable CreditScore = (DataTable)ViewState["CreditScore"];
        DataRow[] drMainRows = CreditScore.Select("Year='" + rbtYears.SelectedValue + "'");

        DataTable dtCreditScoreYr = CreditScore.Clone();
        drMainRows.CopyToDataTable<DataRow>(dtCreditScoreYr, LoadOption.OverwriteChanges);

        if (dtCreditScoreYr.Rows.Count == 0)
        {
            FunProInitializeMainGrid();
        }
        else
        {
            ViewState["CreditScoreYr"] = dtCreditScoreYr;

            FunPriBindLookup();
            grvCreditScore.DataSource = dtCreditScoreYr;
            grvCreditScore.DataBind();
        }

        grvCreditScore.FooterRow.Visible = false;
        grvCreditScore.Columns[grvCreditScore.Columns.Count - 1].Visible = false;

    }

    protected void txtValue_TextChanged(object sender, EventArgs e)
    {
        DataTable dtNumberYr = (DataTable)ViewState["dtNumberYr"];

        int intRowIndex = Utility.FunPubGetGridRowID("grvNumbers", ((TextBox)sender).ClientID);
        DropDownList ddlFFmlLines = (DropDownList)grvNumbers.FooterRow.FindControl("ddlFFmlLines");
        dtNumberYr.Rows[intRowIndex]["Value"] = ((TextBox)sender).Text;
        dtNumberYr.AcceptChanges();

        ViewState["dtNumberYr"] = dtNumberYr;

        for (int i = 0; i <= grvNumbers.Rows.Count - 1; i++)
        {
            Label lblLineTypeID = (Label)grvNumbers.Rows[i].FindControl("lblLineTypeID");
            if (lblLineTypeID.Text == "3")
            {
                TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");
                Label lblValue = (Label)grvNumbers.Rows[i].FindControl("lblValue");
                Label lblFormula = (Label)grvNumbers.Rows[i].FindControl("lblFormula");
                Regex rx = new Regex(@"^((?<o1>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))[-+*/])+(?<o2>[^-+*/()]+|\([^-+*/()]+\)|(?<p>\()+[^-+*/()]+|[^-+*/()]+(?<-p>\)))(?(p)(?!))$");
                decimal decScore = 0;

                string strFormatedfml = FunProApplyFormula(dtNumberYr.Rows[i]["Formula"].ToString(), ddlFFmlLines);
                if ((!rx.IsMatch(strFormatedfml.Replace("?", "+")) && !decimal.TryParse(strFormatedfml.Replace("?", "+"), out decScore)))
                {
                    lblValue.Text = txtValue.Text = "";
                }
                else
                {
                    var result = Convert.ToDecimal(0.0);
                    if (strFormatedfml.Contains("?"))
                    {
                        result = FunProGetMaxValue(strFormatedfml);
                    }
                    else
                    {
                        DataTable tempTable = new DataTable();
                        result = Convert.ToDecimal(tempTable.Compute(strFormatedfml, ""));
                    }
                    lblValue.Text = txtValue.Text = result.ToString(Utility.SetSuffix());
                }

                dtNumberYr.Rows[i]["Value"] = lblValue.Text;
                dtNumberYr.AcceptChanges();

                ViewState["dtNumberYr"] = dtNumberYr;
            }
        }
    }

    protected void btnUpdateYear_Click(object sender, EventArgs e)
    {
        DataTable dtProcessed = (DataTable)ViewState["dtProcessed"];

        FunProUpdateYearValues();

        if (dtProcessed.Rows.Count != rbtYears.Items.Count)
        {
            btnAddYear.Visible = true;
            btnUpdateYear.Visible = false;
            FunProAddNewYear();
        }
        else if (rbtYears.Items.Count > 1)
        {
            btnUpdateYear.Visible = false;
        }
    }

    protected void FunProToggleFmlControls()
    {

        for (int i = 0; i <= grvNumbers.Rows.Count - 1; i++)
        {
            Label lblLineTypeID = (Label)grvNumbers.Rows[i].FindControl("lblLineTypeID");
            Label lblValue = (Label)grvNumbers.Rows[i].FindControl("lblValue");
            TextBox txtValue = (TextBox)grvNumbers.Rows[i].FindControl("txtValue");

            if (lblLineTypeID.Text == "3" || PageMode == PageModes.Query)
            {
                lblValue.Visible = true;
                txtValue.Visible = false;
            }
            else
            {
                lblValue.Visible = false;
                txtValue.Visible = true;
            }
        }
    }
    protected void rdbLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbLevel.SelectedValue.ToString() == "1")
        {
            //FunPriBindLOB();
            ddlLOB.Enabled = ddlConstitution.Enabled = ddlProductCode.Enabled = true;
            rfvLOB.Enabled = rfvConstitution.Enabled = true;
        }
        else
        {
            ddlLOB.SelectedValue = "0";
            rfvLOB.Enabled = rfvConstitution.Enabled= false;            
            ddlProductCode.Items.Clear();
            ddlConstitution.Items.Clear();
            ddlLOB.Enabled = ddlConstitution.Enabled = ddlProductCode.Enabled = false;
        }
    }
}



