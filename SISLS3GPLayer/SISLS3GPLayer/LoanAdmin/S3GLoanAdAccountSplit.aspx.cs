#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin 
/// Screen Name			: Account Split
/// Created By			: Nataraj Y
/// Created Date		: 08-Sep-2010
/// Purpose	            : 
/// <Program Summary>
#endregion

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
using System.Globalization;
using S3GBusEntity.LoanAdmin;

public partial class LoanAdmin_S3GLoanAdAccountSplit : ApplyThemeForProject
{
    #region Variable declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    Dictionary<string, string> Procparam;
    // const int ArrayAccountGrpMax = 100;
    // int[]  intArrayAccountGrp = new int[ArrayAccountGrpMax];
    Dictionary<int, int> dctAccountGrp;
    int _SlNo = 0;
    int intCompany_Id;
    int intUserId;
    int intResult;
    bool bCreate = false;
    bool bModify = false;
    bool bDelete = false;
    bool bQuery = false;
    bool bClearList = false;
    decimal decTotalAmount;
    decimal decPerAssetAmount;
    int intQty;
    int intTotalQuty;
    public string strDateFormat;
    string strSplitNo;
    static string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    bool blnIsWorkflowApplicable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWorkflowApplicable"]);
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACSP';";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAdAccountSplit.aspx';";
    string strRedirectPage = "~/LoanAdmin/S3gLoanAdTransLander.aspx?Code=ACSP";
    string strRedirectHomePage = "window.location.href='../Common/HomePage.aspx';";
    string strNewWin = "window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromEnquiry=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');return false;";
    DataTable dtSplit_Details;
    ContractMgtServicesReference.ContractMgtServicesClient ObjContarctMgtServices;
    ContractMgtServices.S3G_LOANAD_AccountSplitDataTable ObjAccountSplitTable;
    public static LoanAdmin_S3GLoanAdAccountSplit obj_Page;
    #endregion

    /*
    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
    {
        try
        {
            UserInfo ObjUserInfo = new UserInfo();

            if (ObjUserInfo.ProUserLevelIdRW < 3)
            {

                strAlert = "alert('You do not have access rights to this page');" + strRedirectPageView + "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
            }

            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }
    */
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            obj_Page = this;
            intCompany_Id = ObjUserInfo.ProCompanyIdRW;

            intUserId = ObjUserInfo.ProUserIdRW;
            bDelete = ObjUserInfo.ProDeleteRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            if (Request.QueryString["qsMode"] != null)
                strMode = Request.QueryString["qsMode"];
            //if (Request.QueryString["Popup"] != null)  //transaction screen page load
            //    btnCancel.Enabled = false;
            if (Request.QueryString.Get("qsViewId") != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
                strSplitNo = Convert.ToString(fromTicket.Name);
            }
            if (dctAccountGrp == null)
                dctAccountGrp = new Dictionary<int, int>();
            calExeSplitDate.Format = strDateFormat;

            txtOriginalAgreementDate.Attributes.Add("readonly", "readonly");
            //txtSplitDate.Attributes.Add("readonly", "readonly");
            txtSplitDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtSplitDate.ClientID + "','" + strDateFormat + "',false,  false);");
            //if (grvSplitDetails.FooterRow != null)
            //{
            //    ((TextBox)grvSplitDetails.FooterRow.FindControl("txtQty")).CheckGPSLength(true, "Quantity");
            //}
            if (!IsPostBack)
            {

                //txtSplitDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat); 

                FunPriPageLoad();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriPageLoad()
    {
        try
        {
            dctAccountGrp.Clear();
            if (strMode == "C")
            {
                FunPriLoadLObandBranch(intUserId, intCompany_Id);
            }
            if (strSplitNo != null)
            {
                FunPriLoadAccountSplitDtls(strSplitNo);

                if (strMode == "M")
                {
                    FunPriSplitControlStatus(1);
                }
                if (strMode == "Q")
                {
                    FunPriSplitControlStatus(-1);
                }
            }
            else
            {
                FunPriSplitControlStatus(0);
            }
            //ddlLineofBusiness.Focus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriLoadAccountSplitDtls(string strSplitNo)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@SplitNo", strSplitNo);
            DataSet dsSplitDetails = Utility.GetDataset("S3G_LOANAD_GetAccSplitDetails", Procparam);
            if (dsSplitDetails.Tables.Count == 2)
            {
                //grvAccountDetails.DataSource = dsSplitDetails.Tables[2];
                //grvAccountDetails.DataBind();
                if (dsSplitDetails.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.SelectedText = dsSplitDetails.Tables[0].Rows[0]["Location_Name"].ToString();
                    ddlBranch.SelectedValue = dsSplitDetails.Tables[0].Rows[0]["Location_ID"].ToString();
                    ddlBranch.ToolTip = dsSplitDetails.Tables[0].Rows[0]["Location_Name"].ToString();
                    // ddlBranch.ClearDropDownList();
                    ddlLineofBusiness.Items.Add(new ListItem(dsSplitDetails.Tables[0].Rows[0]["LOB_Name"].ToString(), dsSplitDetails.Tables[0].Rows[0]["LOB_ID"].ToString()));
                    ddlLineofBusiness.ToolTip = dsSplitDetails.Tables[0].Rows[0]["LOB_Name"].ToString();
                    //ddlLineofBusiness.ClearDropDownList();
                    txtSplitNo.Text = dsSplitDetails.Tables[0].Rows[0]["Account_Split_No"].ToString();
                    txtSplitDate.Text = dsSplitDetails.Tables[0].Rows[0]["Account_Split_Date"].ToString();
                    hdnStatus.Value = dsSplitDetails.Tables[0].Rows[0]["Split_Status_Code"].ToString();

                    hdnSplitNo.Value = dsSplitDetails.Tables[0].Rows[0]["Account_Split_No"].ToString();

                    ddlPANumber.SelectedValue = dsSplitDetails.Tables[0].Rows[0]["PA_SA_REF_ID"].ToString();
                    ddlPANumber.SelectedText = dsSplitDetails.Tables[0].Rows[0]["PANum"].ToString();
                    FunPriGetAccountDateandAmount(ddlPANumber.SelectedValue);

                    grvSplitDetails.DataSource = dsSplitDetails.Tables[1];
                    grvSplitDetails.DataBind();
                    if (hdnStatus.Value == "5")
                    {
                        grvSplitDetails.Columns[6].Visible = false;
                        btnCreateAcc1.Enabled = btnCreateAcc2.Enabled = false;
                    }
                    if (grvSplitDetails.FooterRow != null)
                    {
                        grvSplitDetails.FooterRow.Visible = false;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriSplitControlStatus(int intModeID)
    {
        try
        {
            switch (intModeID)
            {
                case 0: // Create Mode

                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    panAccountDetails.Visible = true;
                    break;
                case 1://Modify
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    imgSplitDate.Visible = false;
                    txtSplitDate.Attributes.Remove("onblur");
                    txtSplitDate.Attributes.Add("readonly", "readonly");
                    panAccountDetails.Visible = false;
                    calExeSplitDate.Enabled = false;
                    RefreshGridStatus();
                    ddlBranch.Enabled = ddlPANumber.Enabled = false;
                    btnAdd.Visible = false;
                    //if (hdnStatus.Value == "3")
                    //    btnCreateAcc.Visible = true;
                    //else
                    //    btnCreateAcc.Visible = false;
                    break;
                case -1://Query  
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    txtSplitDate.Attributes.Remove("onblur");
                    txtSplitDate.Attributes.Add("readonly", "readonly");
                    imgSplitDate.Visible = false;
                    panAccountDetails.Visible = false;
                    calExeSplitDate.Enabled = false;
                    btnCreateAcc1.Enabled = btnCreateAcc2.Enabled = ddlBranch.Enabled = ddlPANumber.Enabled = false;
                    btnAdd.Visible = false;
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriLoadLObandBranch(int intUser_id, int intCompany_id)
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            if (strSplitNo == null)
            {
                Procparam.Add("@Is_Active", "1");
            }
            Procparam.Add("@User_Id", intUser_id.ToString());
            Procparam.Add("@Company_ID", intCompany_id.ToString());
            Procparam.Add("@FilterOption", "'OL'");
            Procparam.Add("@Program_ID", "83");
            ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLineofBusiness.Items.Count > 0)
            {
                ddlLineofBusiness.SelectedIndex = 1;
                ddlLineofBusiness.ClearDropDownList();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    //added for call id 4186 on july 2,2016 by vinodha m
    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderApprover.Hide();
            FunPriSaveDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    //added for call id 4186 on july 2,2016 by vinodha m
    protected void btnModalCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ModalPopupExtenderApprover.Hide();
            btnClear_Click(null, null);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //added for call id 4186 on july 2,2016 by vinodha m
            if (ViewState["ODI_ErrorCode"] != null && ViewState["ODI_ErrorCode"].ToString() == "1")
            {
                    ModalPopupExtenderApprover.Show();
                    return;             
            }
            else
                FunPriSaveDetails();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void FunPriSaveDetails()
    {
        try
        {
            if (ViewState["SplitDetails"] == null)
            {
                Utility.FunShowAlertMsg(this, "Add Atleast one Split Detail.");
                return;
            }

            if (((DataTable)ViewState["SplitDetails"]).Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Add Atleast one Split Detail.");
                return;
            }

            if (Utility.CompareDates(txtSplitDate.Text, txtOriginalAgreementDate.Text) == 1)
            {
                Utility.FunShowAlertMsg(this, "Split Date cannot be less than original agreement date.");
                return;
            }

            //if (((Label)GVACCDetails.FooterRow.FindControl("lblTotalQuantity")).Text == ((Label)grvSplitDetails.FooterRow.FindControl("lblTotalQuantity")).Text)
            //{
            //    Utility.FunShowAlertMsg(this, "Total split asset count should be less than original account asset count.");
            //    return;
            //}
            string strSplitNum = "";
            //DataTable dtSplitDetails = (DataTable)ViewState["SplitDetails"];
            //var distinctRows = (from DataRow dRow in dtSplitDetails.Rows
            //                    select dRow["Account_Group"]).Distinct().Count();

            //if (distinctRows == 1)
            //{
            //    Utility.FunShowAlertMsg(this, "Split Reference should be different.");
            //    return;
            //}
            ObjAccountSplitTable = new ContractMgtServices.S3G_LOANAD_AccountSplitDataTable();
            ContractMgtServices.S3G_LOANAD_AccountSplitRow ObjAccountSplitRow;
            ObjAccountSplitRow = ObjAccountSplitTable.NewS3G_LOANAD_AccountSplitRow();
            ObjAccountSplitRow.Company_ID = intCompany_Id;
            ObjAccountSplitRow.LOB_ID = Convert.ToInt32(ddlLineofBusiness.SelectedValue);
            ObjAccountSplitRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjAccountSplitRow.Split_Date = Utility.StringToDate(txtSplitDate.Text);
            ObjAccountSplitRow.Finance_Amount = Convert.ToDecimal(hdnFinanceAmount.Value);
            ObjAccountSplitRow.Quantity = Convert.ToInt32(((Label)GVACCDetails.FooterRow.FindControl("lblTotalQuantity")).Text);
            ObjAccountSplitRow.PANum = ddlPANumber.SelectedText.Split('-')[0].Trim();
            ObjAccountSplitRow.SANum = ddlPANumber.SelectedText.Split('-')[0].Trim() + "DUMMY";
            ObjAccountSplitRow.Created_By = intUserId;
            ObjAccountSplitRow.XMLSplitDetails = grvSplitDetails.FunPubFormXml();
            //--Added By Thangam M

            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
            Procparam.Add("@Module_ID", "7");
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@ParameterCode", "34");//34 is the parameter code for no of split days in Global Parameter
            int intNoofSplitDays = 0;
            //Convert.ToInt32(Utility.GetDefaultData(SPNames.S3G_LOANAD_GetGlobalParameters, Procparam).Rows[0]["Parameter_Value"].ToString());

            ObjAccountSplitRow.No_of_Days = intNoofSplitDays;
            //--
            ObjContarctMgtServices = new ContractMgtServicesReference.ContractMgtServicesClient();
            ObjAccountSplitTable.AddS3G_LOANAD_AccountSplitRow(ObjAccountSplitRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objbyteAccountSplit = ClsPubSerialize.Serialize(ObjAccountSplitTable, SerMode);
            intResult = ObjContarctMgtServices.FunPubCreateAccountSplit(out strSplitNum, SerMode, objbyteAccountSplit);
            switch (intResult)
            {
                case 0:
                    strAlert = "Split No :" + strSplitNum + " Generated Successfully";
                    strAlert += @"\n\nWould you like to Continue?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    //Code Added by Ganapathy on 22/10/2012 to avoid double save click BEGINS
                    btnSave.Enabled = false;
                    //END
                    break;
                case -1:
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                    strRedirectPageView = "";
                    break;
                case -2:
                    strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                    strRedirectPageView = "";
                    break;
                case 3:
                    strAlert = strAlert.Replace("__ALERT__", "Split Date should not be less than  " + intNoofSplitDays.ToString() + " days from Activation date");
                    strRedirectPageView = "";
                    break;
                case 4:
                    strAlert = strAlert.Replace("__ALERT__", "Split date should be greater than the previous RS split date");
                    strRedirectPageView = "";
                    break;
                case 5:
                    strAlert = strAlert.Replace("__ALERT__", "Split Date cannot be in a closed month");
                    strRedirectPageView = "";
                    break;
                default:
                    strAlert = strAlert.Replace("__ALERT__", "Error in Account Split");
                    strRedirectPageView = "";
                    break;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls(1);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void grvSplitDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtSplit_Details = (DataTable)ViewState["SplitDetails"];
            dtSplit_Details.Rows.RemoveAt(e.RowIndex);
            if (dtSplit_Details.Rows.Count <= 0)
            {
                FunPriLoadRefNo("Add");
            }
            else
            {
                grvSplitDetails.DataSource = dtSplit_Details;
                grvSplitDetails.DataBind();

                ViewState["SplitDetails"] = dtSplit_Details;

                for (int i = 0; i < GVACCDetails.Rows.Count; i++)
                {
                    ((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).Items.Add((i + 1).ToString());
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
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
                FunPriClosePage();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void ddlPANumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls(4);

            if (ddlPANumber.SelectedValue != "0")
            {
                FunPriLoadAccountDetails(ddlPANumber.SelectedValue);
                if (ddlPANumber.SelectedValue != "0")
                    FunPriGetAccountDateandAmount(ddlPANumber.SelectedValue);
            }
            else
            {
                grvSplitDetails.DataSource = null;
                grvSplitDetails.DataBind();
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriClosePage()
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
            }
            else
            {
                Response.Redirect(strRedirectPage, false);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriLoadAccountDetails(string strPANumber)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Option", "3");
            Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Customer_ID", "1");
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@Param1", strPANumber);
            DataSet ds_AccountDetails = Utility.GetDataset(SPNames.S3G_LOANAD_GetAccCons_Split_List, Procparam);

            if (ds_AccountDetails.Tables[0].Rows[0]["Error_Code"].ToString() == "1")
            {
                //added for call id 4186 on july 2,2016 by vinodha m
                ViewState["ODI_ErrorCode"] = ds_AccountDetails.Tables[0].Rows[0]["Error_Code"].ToString();
                //Utility.FunShowAlertMsg(this, "Overdue arrears available..! Unable to proceed..!");
                //ddlPANumber.Clear();
                //return;
            }

            if (ds_AccountDetails.Tables[1].Rows.Count > 0)
            {
                GVACCDetails.DataSource = ds_AccountDetails.Tables[1].DefaultView;
                GVACCDetails.DataBind();
                ViewState["TotalLoanAmount"] = ds_AccountDetails.Tables[1].Rows[0]["Loan_Amount"].ToString();
                FunPriLoadRefNo("Add");
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls(3);

            grvSplitDetails.DataSource = null;
            grvSplitDetails.DataBind();
            ddlPANumber.Clear();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriGetAccountDateandAmount(string strPA_SA_REF_ID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Clear();
            Procparam.Add("@Option", "4");
            Procparam.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
            Procparam.Add("@Location_ID", ddlBranch.SelectedValue);
            Procparam.Add("@Customer_ID", "1");
            Procparam.Add("@Company_ID", intCompany_Id.ToString());
            Procparam.Add("@Param1", strPA_SA_REF_ID);

            DataTable dt_AccountDate = Utility.GetDefaultData(SPNames.S3G_LOANAD_GetAccCons_Split_List, Procparam);
            txtOriginalAgreementDate.Text = DateTime.Parse(dt_AccountDate.Rows[0]["PAN_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

            S3GCustomerCommAddress.SetCustomerDetails(dt_AccountDate.Rows[0], true);

            hdnFinanceAmount.Value = dt_AccountDate.Rows[0]["FinanceAmount"].ToString();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriLoadRefNo(string strMode)
    {
        try
        {
            if (strMode == "Add")
            {
                dtSplit_Details = new DataTable();
                dtSplit_Details.Columns.Add("Split_Ref_No");
                dtSplit_Details.Columns.Add("Asset_Id");
                dtSplit_Details.Columns.Add("Stock_Invoice_No");
                dtSplit_Details.Columns.Add("Invoice_No");
                dtSplit_Details.Columns.Add("Asset_Description");
                dtSplit_Details.Columns.Add("Qty");
                dtSplit_Details.Columns["Qty"].DataType = typeof(decimal);
                dtSplit_Details.Columns.Add("Finance_Amount");
                dtSplit_Details.Columns.Add("Account_Group");
                dtSplit_Details.Columns.Add("Split_Percentage");
                dtSplit_Details.Columns.Add("Split_PANum");
                dtSplit_Details.Columns.Add("Split_SANum");
                dtSplit_Details.Columns.Add("Account_Creation_ID");
                dtSplit_Details.Columns.Add("Account_Creation_ID1");
                dtSplit_Details.Columns.Add("Account_Creation_ID2");
                DataRow dr = dtSplit_Details.NewRow();
                dr["Split_Ref_No"] = "";
                dr["Asset_Id"] = "";
                dr["Invoice_No"] = "";
                dr["Asset_Description"] = "";
                dr["Qty"] = "0";
                dr["Finance_Amount"] = "";
                dr["Account_Group"] = "0";
                dr["Split_Percentage"] = "0";
                dr["Split_PANum"] = null;
                dr["Split_SANum"] = null;
                dr["Account_Creation_ID"] = null;
                dtSplit_Details.Rows.Add(dr);

            }
            else
            {
                dtSplit_Details = (DataTable)ViewState["SplitDetails"];
            }
            grvSplitDetails.DataSource = dtSplit_Details;
            grvSplitDetails.DataBind();
            if (strMode == "Add")
            {
                dtSplit_Details.Rows.Clear();
                ViewState["SplitDetails"] = dtSplit_Details;
                dtSplit_Details.Dispose();
                grvSplitDetails.Rows[0].Cells.Clear();
                grvSplitDetails.Rows[0].Visible = false;
            }
            //for (int i = 0; i < GVACCDetails.Rows.Count; i++)
            //{
            //    ((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).Items.Add((i + 1).ToString());
            //}
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int intSelect = 0;

            foreach (GridViewRow rw in GVACCDetails.Rows)
            {
                CheckBox chkSelect = (CheckBox)rw.FindControl("chkSelect");

                if (chkSelect.Checked)
                {
                    intSelect = 1;

                    TextBox txtqty = (TextBox)rw.FindControl("txtQty");
                    if (Convert.ToInt32(txtqty.Text) == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Split asset quantity cannot be Zero");
                        txtqty.Focus();
                        return;
                    }

                    dtSplit_Details = (DataTable)ViewState["SplitDetails"];
                    DataRow dr_splitrow = dtSplit_Details.NewRow();
                    
                    //dr_splitrow["Split_Ref_No"] = ((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).SelectedValue;
                    //dr_splitrow["Asset_Id"] = ((Label)GVACCDetails.Rows[Convert.ToInt32(((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).SelectedValue) - 1].FindControl("lblAssetId")).Text; ;
                    //dr_splitrow["Invoice_No"] = ((Label)GVACCDetails.Rows[Convert.ToInt32(((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).SelectedValue) - 1].FindControl("lblInvoiceNo")).Text;
                    //dr_splitrow["Asset_Description"] = ((Label)GVACCDetails.Rows[Convert.ToInt32(((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).SelectedValue) - 1].FindControl("lblAssetDesc")).Text;
                    //dr_splitrow["Qty"] = ((TextBox)grvSplitDetails.FooterRow.FindControl("txtQty")).Text;
                    //dr_splitrow["Finance_Amount"] = Math.Round(Convert.ToDecimal(((Label)GVACCDetails.Rows[Convert.ToInt32(((DropDownList)grvSplitDetails.FooterRow.FindControl("ddlSplitRefNo")).SelectedValue) - 1].FindControl("lblPer_Asset_Amount")).Text) * Convert.ToInt32(((TextBox)grvSplitDetails.FooterRow.FindControl("txtQty")).Text), 2);
                    //dr_splitrow["Split_Percentage"] = Math.Round((Convert.ToDouble(dr_splitrow["Finance_Amount"]) / Convert.ToDouble(((Label)GVACCDetails.FooterRow.FindControl("lblTotalFinanceAmount")).Text)) * 100, 2);

                    dr_splitrow["Split_Ref_No"] = ((Label)rw.FindControl("lblReferenceNo")).Text;
                    dr_splitrow["Asset_Id"] = ((Label)rw.FindControl("lblAssetId")).Text;
                    dr_splitrow["Stock_Invoice_No"] = ((Label)rw.FindControl("lblStockInvoiceNo")).Text;
                    dr_splitrow["Invoice_No"] = ((Label)rw.FindControl("lblInvoiceNo")).Text;
                    dr_splitrow["Asset_Description"] = ((Label)rw.FindControl("lblAssetDesc")).Text;
                    dr_splitrow["Qty"] = ((TextBox)rw.FindControl("txtQty")).Text;
                    dr_splitrow["Finance_Amount"] = Math.Round(Convert.ToDecimal(((Label)rw.FindControl("lblPer_Asset_Amount")).Text) * Convert.ToInt32(((TextBox)rw.FindControl("txtQty")).Text), 2);
                    dr_splitrow["Split_Percentage"] = Math.Round((Convert.ToDouble(dr_splitrow["Finance_Amount"]) / Convert.ToDouble(((Label)GVACCDetails.FooterRow.FindControl("lblTotalFinanceAmount")).Text)) * 100, 2);
                    

                    if (dtSplit_Details.Rows.Count > 0)
                    {
                        dr_splitrow["Account_Group"] = Convert.ToInt32(dtSplit_Details.Rows[dtSplit_Details.Rows.Count - 1]["Account_Group"]) + 1;

                    }
                    else
                        dr_splitrow["Account_Group"] = 1;
                    dtSplit_Details.Rows.Add(dr_splitrow);

                    foreach (GridViewRow drAssetRow in GVACCDetails.Rows)
                    {
                        string strAssetId = ((Label)drAssetRow.FindControl("lblAssetId")).Text;
                        string strStockInvoiceNo = ((Label)drAssetRow.FindControl("lblStockInvoiceNo")).Text; 
                        object intQty = dtSplit_Details.DefaultView.ToTable().Compute("Sum(Qty)", "Asset_Id = " + strAssetId);
                        object intCount = dtSplit_Details.DefaultView.ToTable().Compute("Count(Asset_Id)", "Asset_Id = " + strAssetId);

                        //if (strAssetId != "810405")
                        //{
                            //if ((!(string.IsNullOrEmpty(intCount.ToString()))) && (Convert.ToInt32(intCount) > 1))
                            //{
                            //    Utility.FunShowAlertMsg(this, "Split asset should not be duplicated");
                            //    dtSplit_Details.Rows.RemoveAt(dtSplit_Details.Rows.Count - 1);
                            //    return;
                            //}
                        //}

                        //if ((!(string.IsNullOrEmpty(intQty.ToString()))) && (Convert.ToInt32(intQty) > Convert.ToInt32(((Label)drAssetRow.FindControl("lblQuantity")).Text)))
                        //{
                        //    Utility.FunShowAlertMsg(this, "Split asset quantity should not be greater than total asset quantity");
                        //    dtSplit_Details.Rows.RemoveAt(dtSplit_Details.Rows.Count - 1);
                        //    return;
                        //}
                    }
                    chkSelect.Checked = false;
                }
            }

            if (GVACCDetails.Rows.Count > 0)
                ((CheckBox)GVACCDetails.HeaderRow.FindControl("chkSelectAllBranch")).Checked = false;

            if (intSelect == 0)
            {
                Utility.FunShowAlertMsg(this, "Select atleast one asset to Add");
                return;
            }

            dctAccountGrp.Add(dtSplit_Details.Rows.Count - 1, Convert.ToInt32(dtSplit_Details.Rows[dtSplit_Details.Rows.Count - 1]["Account_Group"]));
            ViewState["SplitDetails"] = dtSplit_Details;
            FunPriLoadRefNo("Modify");
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriSetAccGrp()
    {
        try
        {
            dctAccountGrp.Clear();
            foreach (GridViewRow drSplitRow in grvSplitDetails.Rows)
            {
                int intAccgrp = Convert.ToInt32(((TextBox)drSplitRow.FindControl("txtAccGroup")).Text);
                dctAccountGrp.Add(drSplitRow.RowIndex, intAccgrp);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriClearControls(int intOption)
    {
        try
        {
            switch (intOption)
            {
                case 1:
                    if (ddlLineofBusiness.Items.Count > 0)
                    {
                        ddlLineofBusiness.SelectedIndex = 0;
                    }
                    ddlBranch.Clear();
                    ddlPANumber.Clear();
                    GVACCDetails.DataSource = null;
                    GVACCDetails.DataBind();
                    grvSplitDetails.ClearGrid();
                    txtOriginalAgreementDate.Text = "";
                    txtSplitDate.Text = "";
                    S3GCustomerCommAddress.ClearCustomerDetails();
                    break;
                case 2:
                    if (ddlBranch.SelectedValue != "0")
                    {
                        ddlBranch.Clear();
                    }
                    ddlPANumber.Clear();
                    GVACCDetails.DataSource = null;
                    GVACCDetails.DataBind();
                    grvSplitDetails.ClearGrid();
                    txtOriginalAgreementDate.Text = "";
                    txtSplitDate.Text = "";
                    S3GCustomerCommAddress.ClearCustomerDetails();
                    break;
                case 3:
                    ddlPANumber.Clear();
                    GVACCDetails.DataSource = null;
                    GVACCDetails.DataBind();
                    grvSplitDetails.ClearGrid();
                    txtOriginalAgreementDate.Text = "";
                    txtSplitDate.Text = "";
                    S3GCustomerCommAddress.ClearCustomerDetails();
                    break;
                case 4:
                    GVACCDetails.DataSource = null;
                    GVACCDetails.DataBind();
                    grvSplitDetails.ClearGrid();
                    txtOriginalAgreementDate.Text = "";
                    txtSplitDate.Text = "";
                    S3GCustomerCommAddress.ClearCustomerDetails();
                    break;
                case 5:
                    //GVACCDetails.ClearGrid();
                    GVACCDetails.DataSource = null;
                    GVACCDetails.DataBind();
                    grvSplitDetails.ClearGrid();
                    txtOriginalAgreementDate.Text = "";
                    txtSplitDate.Text = "";
                    S3GCustomerCommAddress.ClearCustomerDetails();
                    break;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void txtAccGroup_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtSplitDetails = new DataTable();
            if (((TextBox)sender).Text.Length > 0 && ((TextBox)sender).Text != "0")
            {
                string strFieldAtt = ((TextBox)sender).ClientID;
                string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvSplitDetails_")).Replace("grvSplitDetails_ctl", "");
                int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
                gRowIndex = gRowIndex - 2;

                //To Update New Dictionary values
                FunPriSetAccGrp();
                dtSplitDetails = (DataTable)ViewState["SplitDetails"];
                dtSplitDetails.Rows[gRowIndex]["Account_Group"] = ((TextBox)sender).Text;
                FunPriSortDict();
                int prev = 0;

                // this is remove the duplicates.
                foreach (var objCollection in dctAccountGrp.Values.Distinct().ToList())
                {
                    if (prev == 0)
                    {
                        prev = Convert.ToInt32(objCollection);
                        continue;
                    }
                    if ((prev + 1) != Convert.ToInt32(objCollection) || (prev) == Convert.ToInt32(objCollection))
                    {

                        //((TextBox)sender).Focus();
                        ((TextBox)sender).Text = ((System.Web.UI.HtmlControls.HtmlInputHidden)grvSplitDetails.Rows[gRowIndex].FindControl("hdnAccgrp")).Value;
                        dtSplitDetails.Rows[gRowIndex]["Account_Group"] = ((TextBox)sender).Text;
                        Utility.FunShowAlertMsg(this, "Split Ref No should be in sequence");
                        ((TextBox)sender).Focus();
                        break;
                    }
                    prev = Convert.ToInt32(objCollection);
                }

            }
            else
            {
                Utility.FunShowAlertMsg(this, "Split Ref No Cannot be Empty or 0");
                ((TextBox)sender).Focus();
            }
            ViewState["SplitDetails"] = dtSplitDetails;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    private void FunPriSortDict()
    {
        try
        {
            int temp = -1;
            for (int i_row = 0; i_row < dctAccountGrp.Values.Count - 1; i_row++)
            {
                for (int i_Subrow = i_row + 1; i_Subrow < dctAccountGrp.Values.Count; i_Subrow++)
                {
                    if (dctAccountGrp[i_Subrow] < dctAccountGrp[i_row])
                    {
                        temp = dctAccountGrp[i_Subrow];
                        dctAccountGrp[i_Subrow] = dctAccountGrp[i_row];
                        dctAccountGrp[i_row] = temp;
                    }
                }
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void grvSplitDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(hdnSplitNo.Value, false, 0);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Button btnCreateAcc = (Button)e.Row.FindControl("btnCreateAcc");
                TextBox txtAccGroup = (TextBox)e.Row.FindControl("txtAccGroup");

                Label lblAccountId = (Label)e.Row.FindControl("lblAccountId");
                Label lblAccountId1 = (Label)e.Row.FindControl("lblAccount_Creation_ID1");
                Label lblAccountId2 = (Label)e.Row.FindControl("lblAccount_Creation_ID2");

                //Button btnViewAcc = (Button)e.Row.FindControl("btnViewAcc");

                if (strMode == "C")
                {
                    Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                    btnDelete.Enabled = true;
                }

                System.Web.UI.HtmlControls.HtmlInputHidden hdnValue = ((System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnAccgrp"));

                string strRefno = ((TextBox)e.Row.FindControl("txtAccGroup")).Text;
                FormsAuthenticationTicket Ticket_RefNo = new FormsAuthenticationTicket(strRefno, false, 0);

                txtAccGroup.Attributes["onfocus"] = string.Format("javascript:SetPrevValueOnFocus(this,'{0}')", hdnValue.ClientID);
                btnCreateAcc1.Attributes["OnClick"] = string.Format("javascript:ViewModal('{0}','{1}')", FormsAuthentication.Encrypt(Ticket), "1");
                btnCreateAcc2.Attributes["OnClick"] = string.Format("javascript:ViewModal('{0}','{1}')", FormsAuthentication.Encrypt(Ticket), "2");


                if (!String.IsNullOrEmpty(lblAccountId1.Text))
                {
                    FormsAuthenticationTicket AccountIdTicket = new FormsAuthenticationTicket(lblAccountId1.Text, false, 0);
                    btnViewAcc1.Attributes["OnClick"] = string.Format("javascript:ViewAccountModal('{0}')", FormsAuthentication.Encrypt(AccountIdTicket));
                    txtAccGroup.Attributes.Add("readonly", "readonly");
                    btnViewAcc1.Enabled = true;
                    btnCreateAcc1.Enabled = false;
                }
                else
                {
                    btnViewAcc1.Enabled = false;
                    if (hdnSplitNo.Value != "")
                        btnCreateAcc1.Enabled = true;
                }
                if (!String.IsNullOrEmpty(lblAccountId2.Text))
                {
                    FormsAuthenticationTicket AccountIdTicket = new FormsAuthenticationTicket(lblAccountId2.Text, false, 0);
                    btnViewAcc2.Attributes["OnClick"] = string.Format("javascript:ViewAccountModal('{0}')", FormsAuthentication.Encrypt(AccountIdTicket));
                    txtAccGroup.Attributes.Add("readonly", "readonly");
                    btnViewAcc2.Enabled = true;
                    btnCreateAcc2.Enabled = false;
                }
                else
                {
                    btnViewAcc2.Enabled = false;
                    if (hdnSplitNo.Value != "")
                        btnCreateAcc2.Enabled = true;
                }
                txtAccGroup.Attributes.Add("readonly", "readonly");
                intQty += Convert.ToInt32(((Label)e.Row.FindControl("lblQty")).Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalQuantity")).Text = Convert.ToString(intQty);
                btnCreateAcc1.Visible = true;
                btnViewAcc1.Visible = true;
                btnCreateAcc2.Visible = true;
                btnViewAcc2.Visible = true;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void RefreshGridStatus()
    {
        try
        {
            bool CanCancelAcc = true;
            foreach (GridViewRow drSplitRow in grvSplitDetails.Rows)
            {
                Label lblPANum = (Label)drSplitRow.FindControl("lblPANum");
                Label lblSANum = (Label)drSplitRow.FindControl("lblSANum");
                TextBox txtAccGroup = (TextBox)drSplitRow.FindControl("txtAccGroup");
                if (((string.IsNullOrEmpty(lblPANum.Text))) || ((string.IsNullOrEmpty(lblSANum.Text))) || lblPANum.Text.Length == 0 || lblSANum.Text.Length == 0)
                {
                    txtAccGroup.ReadOnly = true;
                    CanCancelAcc = false;
                }
                else
                {
                    txtAccGroup.ReadOnly = false;
                }
            }

            if (CanCancelAcc && hdnStatus.Value == "1" && bDelete)
            {
                //btnCancelAcc.Visible = true;
            }
            else
            {
                btnCancelAcc.Visible = false;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnCreateAcc1_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("ApplicationAssetDetails");
            Session.Remove("AccountAssetCustomer");
            FunPriPageLoad();
            RefreshGridStatus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnCreateAcc2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("ApplicationAssetDetails");
            Session.Remove("AccountAssetCustomer");
            FunPriPageLoad();
            RefreshGridStatus();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void btnCancelAcc_Click(object sender, EventArgs e)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@DocNumber", hdnSplitNo.Value);
            Procparam.Add("@Company_Id", "1");
            Procparam.Add("@Option", "8");
            DataTable dtTable = Utility.GetDefaultData("S3G_LoanAd_UpdateAccountStatus_SplitandConslidation", Procparam);
            if (dtTable.Rows[0]["Status"].ToString() == "Sucess")
            {
                strAlert = strAlert.Replace("__ALERT__", "Account Split " + hdnSplitNo.Value + " Cancelled Successfully");
            }
            else
            {
                strAlert = strAlert.Replace("__ALERT__", "Unable to Cancel the Split " + hdnSplitNo.Value);
                strRedirectPageView = "";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void ddlLineofBusiness_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriClearControls(2);
            ddlLineofBusiness.Focus();
            if (ddlLineofBusiness.SelectedIndex > 0)
            {
                ddlBranch.Clear();
                //Dictionary<string, string> Procparam = new Dictionary<string, string>();
                //Procparam.Clear();
                //if (strSplitNo == null)
                //{
                //    Procparam.Add("@Is_Active", "1");
                //}
                //Procparam.Add("@User_Id", intUserId.ToString());
                //Procparam.Add("@Company_ID", intCompany_Id.ToString());
                //Procparam.Add("@Lob_Id", ddlLineofBusiness.SelectedValue);
                //Procparam.Add("@Program_ID", "83");
                //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location" });
                // ddlBranch.SelectedIndex = 0;
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void GVACCDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decTotalAmount += Convert.ToDecimal(((Label)e.Row.FindControl("lblFinanceAmount")).Text);
                intTotalQuty += Convert.ToInt32(((Label)e.Row.FindControl("lblQuantity")).Text);
                //decPerAssetAmount = Convert.ToDecimal(((Convert.ToDecimal(((Label)e.Row.FindControl("lblFinanceAmount")).Text) / Convert.ToInt32(((Label)e.Row.FindControl("lblQuantity")).Text)) / (Convert.ToDecimal(hdnFinanceAmount.Value))).ToString("000.00")) * (Convert.ToDecimal(hdnFinanceAmount.Value));

                CheckBox chkSelectAccount = (CheckBox)e.Row.FindControl("chkSelect");
                chkSelectAccount.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + GVACCDetails.ClientID + "','chkSelectAllBranch','chkSelect');");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ((Label)e.Row.FindControl("lblTotalFinanceAmount")).Text = Convert.ToString(decTotalAmount);
                ((Label)e.Row.FindControl("lblTotalQuantity")).Text = Convert.ToString(intTotalQuty);
            }
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    // Added By Shibu 24-Sep-2013 Branch List 
    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Clear();

        Procparam.Add("@Company_ID", obj_Page.intCompany_Id.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Program_Id", "083");
        Procparam.Add("@Lob_Id", obj_Page.ddlLineofBusiness.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggestions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetPANNUmber(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggestions = new List<String>();
        Procparam.Add("@LOB_ID", obj_Page.ddlLineofBusiness.SelectedValue);
        Procparam.Add("@Location_ID", obj_Page.ddlBranch.SelectedValue);
        Procparam.Add("@Program_Id", "83");
        Procparam.Add("@Company_ID", obj_Page.intCompany_Id.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", "%" + prefixText + "%");
        suggestions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetRS_AGT", Procparam));

        return suggestions.ToArray();

    }

}
