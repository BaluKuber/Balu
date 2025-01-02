#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Demand Collection Customer Level
/// Created By          :   Sangeetha R
/// Created Date        :   
/// Purpose             :   To show the comparison between Demand Vs Collection Customer Level for a given period.
/// Last Updated By     :   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Collections;
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
using System.Collections.Generic;
using System.Globalization;
using S3GBusEntity.Reports;
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using S3GBusEntity;
#endregion

public partial class Reports_S3GRptDemandCCL : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int CompanyId;
    int UserId;
    bool Is_Active;
    string Option;
    string Value;
    string PANum;
    int ProgramId;
    public string strDateFormat;
    int MaxMonth;
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    Dictionary<int, string> dictDemandmonth = new Dictionary<int, string>();
    //Dictionary<string, string> Procparam;
    string strPageName = "Demand Collection Customer Level";
    DataTable dtTable = new DataTable();
    //For Demand Collection Grid
    decimal TotOpnDemand;
    decimal TotOpnCollection;
    decimal TotOpnPercentage;
    decimal TotClsDemand;
    decimal TotClsCollection;
    decimal TotClsPercentage;
    decimal TotMonDemand;
    decimal TotMonCollection;
    decimal TotMonPercentage;
    ReportOrgColMgtServicesClient objSerClient;
    ReportAccountsMgtServicesClient ObjAccClient; 
    #endregion

    #region Page Load
    /// <summary>
    /// This event is handled during Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Due to Data Problem, Unable to Load Demand Collection Page.";
            CVDemandCollection.IsValid = false;
        }
    }
    #endregion

    #region Page Methods
    /// <summary>
    /// This Method is called when page is getting Loading
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtReportDate.Attributes.Add("readonly", "readonly");
            txtReportDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtReportDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            #endregion
         
            objSerClient = new ReportOrgColMgtServicesClient();
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.ToolTip = "Customer Name";
            //btnLoadCustomer.ToolTip = "Customer Name";
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtName.Attributes.Add("ReadOnly", "ReadOnly");
            //if (ddlLOB.SelectedValue != "-1")
            //{
            //    ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue;
            //}
            int ProgramId = 176;

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            UserId = ObjUserInfo.ProUserIdRW;

            Session["Date"] = DateTime.Now.ToString(strDateFormat) + "  " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;

            if (!IsPostBack)
            {
                
                ClearSession();
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                FunPriLoadGroup(CompanyId);
               // FunPriLoadDemandPAN(CompanyId);
                //FunPriLoadCustomerGroupPAN(Option, Value, CompanyId);
                FunPubLoadDenomination();
                //FunPriLoadFrequency();
                //FunPriLoadAssetCategoriesTypes();
                ////FunPriLoadAssetsCategories();

                //FunPriLoadAssetCategories();
                LoadFinancialYears(ddlFinacialYearBase);
                ddlFinacialYearBase.SelectedValue = FunPriLoadCurrentFinancialYear();
                ViewState["BaseMonths"] = FunPriFillArrayDemandMonth(ddlFinacialYearBase);
                LoadCompareFinancialYears(ddlFinancialYearCompare);
                ddlFinancialYearCompare.SelectedValue = ddlFinacialYearBase.SelectedValue;
                ViewState["CompaareMonths"] = FunPriFillArrayDemandMonth(ddlFinancialYearCompare);
                ddlFromYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ddlToYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
                ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Demand Collection Customer Level page");
        }
        finally
        {
            objSerClient.Close();
        }   
    }
    /// <summary>
    /// To Load LOB
    /// </summary>
    /// <param name="Company_id"></param>
    /// <param name="User_id"></param>
    private void FunPriLoadLob(int Company_id, int User_id, int Program_id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            ddlLOB.Items.Clear();
            ddlLOB.Focus();
            byte[] byteLobs = objSerClient.FunPubGetDemandCustLOB(Company_id, User_id, Program_id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
                ddlLOB.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }


    private string FunPriLoadCurrentFinancialYear()
    {
        int intCurrentYear = DateTime.Now.Year;
        int intCurrentMonth = DateTime.Now.Month;
        int intFinancialYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        if (intCurrentMonth >= intFinancialYearStartMonth)
        {
            return Convert.ToString(intCurrentYear) + "-" + Convert.ToString(intCurrentYear + 1);
        }
        else
        {
            return Convert.ToString(intCurrentYear - 1) + "-" + Convert.ToString(intCurrentYear);
        }
    }

    /// <summary>
    /// To Load Customer Group
    /// </summary>
    /// <param name="Company_id"></param>
    private void FunPriLoadGroup(int Company_id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetGroup(Company_id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlCustomerGroup.DataSource = lobs;
            ddlCustomerGroup.DataTextField = "Description";
            ddlCustomerGroup.DataValueField = "ID";
            ddlCustomerGroup.DataBind();
            if (ddlCustomerGroup.Items.Count == 2)
            {
                ddlCustomerGroup.SelectedIndex = 1;
            }
            else
                ddlCustomerGroup.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To load Prime Account Number
    /// </summary>
    /// <param name="Company_id"></param>
    private void FunPriLoadDemandPAN(int Company_id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetDemandPAN(Company_id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPNum.DataSource = lobs;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            if (ddlPNum.Items.Count == 2)
            {
                ddlPNum.SelectedIndex = 1;
                
            }
            else
                ddlPNum.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Group based on Customer or Prime Account Number
    /// </summary>
    /// <param name="Option"></param>
    /// <param name="Value"></param>
    /// <param name="Company_id"></param>
    private void FunPriLoadCustomerGroup(string Option, string Value, int Company_id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetCustomerGroupPAN(Option, Value, Company_id);
            ClsPubCustomerGroupPAN CustomerGroupPAN = (ClsPubCustomerGroupPAN)DeSeriliaze(byteLobs);
            ddlCustomerGroup.DataSource = CustomerGroupPAN.CustomerGroupdetails;
            ddlCustomerGroup.DataTextField = "Description";
            ddlCustomerGroup.DataValueField = "ID";
            ddlCustomerGroup.DataBind();
            //if (ddlCustomerGroup.Items.Count == 2)
            //{
            //    ddlCustomerGroup.SelectedIndex = 1;
            //}
            //else
            //    ddlCustomerGroup.SelectedIndex = 0;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlCustomerGroup.Items.Insert(0, liSelect);
            //if (CustomerGroupPAN.CustomerGroupdetails.Count == 0)
            //{
            //    //ddlCustomerGroup.Items.Add("--Select--");
                

            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
   
    
    private void FunPriLoadGroupPANLOB(string opt)
    {
        try
        {
            ClsPubDCHeaderParams objHeaderParams = new ClsPubDCHeaderParams();
            objHeaderParams.Company_ID = ObjUserInfo.ProCompanyIdRW.ToString ();

            //if (!string.IsNullOrEmpty (txtCustomerName .Text ))
            if( !string .IsNullOrEmpty (hdnCustID.Value))
                objHeaderParams.Customer_ID = hdnCustID .Value;

            if (ddlCustomerGroup.SelectedValue != "-1")
                objHeaderParams.Group_ID = ddlCustomerGroup.SelectedValue;

            if (ddlLOB.SelectedValue!="-1")
                objHeaderParams.Lob_ID = ddlLOB.SelectedValue;
            if (ddlPNum.SelectedValue != "-1")
                objHeaderParams.PANum = ddlPNum.SelectedValue;
                
            objHeaderParams.Option = opt;


            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetGroupPAN(objHeaderParams);
            ClsPubCustomerGroupPAN CustomerGroupPAN = (ClsPubCustomerGroupPAN)DeSeriliaze(byteLobs);

            if (objHeaderParams.Option == "L")
            {
                ddlPNum.DataSource = CustomerGroupPAN.Panumdetails;
                ddlPNum.DataTextField = "Description";
                ddlPNum.DataValueField = "ID";
                ddlPNum.DataBind();
                if (ddlPNum.Items.Count == 1)
                {
                    ddlPNum.SelectedIndex = 0;
                    FunPriLoadSAN();
                }
                else
                    ddlPNum.SelectedIndex = -1;
                System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlPNum.Items.Insert(0, liSelect1);
                
                //ddlCustomerGroup.DataSource = CustomerGroupPAN.CustomerGroupdetails;
                //ddlCustomerGroup.DataTextField = "Description";
                //ddlCustomerGroup.DataValueField = "ID";
                //ddlCustomerGroup.DataBind();
                //if (ddlCustomerGroup.Items.Count == 1)
                //{
                //    ddlCustomerGroup.SelectedIndex = 0;
                //}
                //else
                //    ddlCustomerGroup.SelectedIndex = -1;
                //System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                //ddlCustomerGroup.Items.Insert(0, liSelect2);
            }
            else if (objHeaderParams.Option == "LG")
            {
                ddlPNum.DataSource = CustomerGroupPAN.Panumdetails;
                ddlPNum.DataTextField = "Description";
                ddlPNum.DataValueField = "ID";
                ddlPNum.DataBind();
                if (ddlPNum.Items.Count == 1)
                {
                    ddlPNum.SelectedIndex = 0;
                    FunPriLoadSAN();
                }
                else
                    ddlPNum.SelectedIndex = -1;
                System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlPNum.Items.Insert(0, liSelect1);
            }
            else if (objHeaderParams.Option == "P")
            {
                //ddlLOB.SelectedValue = CustomerGroupPAN.Lob_ID;
                if(!string .IsNullOrEmpty (CustomerGroupPAN.Group_ID))
                    ddlCustomerGroup.SelectedValue = CustomerGroupPAN.Group_ID;
                //TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
                //txtName.Text = CustomerGroupPAN.Customer_ID;
                
            }
            else if (objHeaderParams.Option == "C")
            {
                ddlPNum.DataSource = CustomerGroupPAN.Panumdetails;
                ddlPNum.DataTextField = "Description";
                ddlPNum.DataValueField = "ID";
                ddlPNum.DataBind();
                if (ddlPNum.Items.Count == 1)
                {
                    ddlPNum.SelectedIndex = 0;
                    FunPriLoadSAN();
                }
                else
                    ddlPNum.SelectedIndex = -1;
                System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlPNum.Items.Insert(0, liSelect1);

                ddlCustomerGroup.DataSource = CustomerGroupPAN.CustomerGroupdetails;
                ddlCustomerGroup.DataTextField = "Description";
                ddlCustomerGroup.DataValueField = "ID";
                ddlCustomerGroup.DataBind();
                if (ddlCustomerGroup.Items.Count == 1)
                {
                    ddlCustomerGroup.SelectedIndex = 0;
                }
                else
                    ddlCustomerGroup.SelectedIndex = -1; 
                System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlCustomerGroup.Items.Insert(0, liSelect2);

            }

            else 
            { 
            ddlLOB.DataSource = CustomerGroupPAN.LOBdetails;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count == 1)
            {
                ddlLOB.SelectedIndex = 0;
            }
            else
                ddlLOB.SelectedIndex = -1;
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlLOB.Items.Insert(0, liSelect);

            ddlPNum.DataSource = CustomerGroupPAN.Panumdetails;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            if (ddlPNum.Items.Count == 1)
            {
                ddlPNum.SelectedIndex = 0;
                FunPriLoadSAN();
            }
            else
                ddlPNum.SelectedIndex = -1;
            System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlPNum.Items.Insert(0, liSelect1);

            ddlCustomerGroup.DataSource = CustomerGroupPAN.CustomerGroupdetails;
            ddlCustomerGroup.DataTextField = "Description";
            ddlCustomerGroup.DataValueField = "ID";
            ddlCustomerGroup.DataBind();
            if (ddlCustomerGroup.Items.Count == 1)
                {
                    ddlCustomerGroup.SelectedIndex = 0;
                }
                else
                    ddlCustomerGroup.SelectedIndex = -1; 
            System.Web.UI.WebControls.ListItem liSelect2 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlCustomerGroup.Items.Insert(0, liSelect2);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    
    /// <summary>
    /// To Load Prime Account Number based on Customer or Group
    /// </summary>
    /// <param name="Option"></param>
    /// <param name="Value"></param>
    /// <param name="Company_id"></param>
    private void FunPriLoadPrime(string Option, string Value, int Company_id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetCustomerGroupPAN(Option, Value, Company_id);
            ClsPubCustomerGroupPAN CustomerGroupPAN = (ClsPubCustomerGroupPAN)DeSeriliaze(byteLobs);
            ddlPNum.ClearDropDownList();
            ddlPNum.DataSource = CustomerGroupPAN.Panumdetails;
            ddlPNum.DataBind();
            //if (ddlPNum.Items.Count == 2)
            //{
            //    ddlPNum.SelectedIndex = 1;
            //    FunPriLoadSAN();
            //}
            //else
            //    ddlPNum.SelectedIndex = 0;
            ddlSNum.Items.Clear();
            System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlPNum.Items.Insert(0, liSelect);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Denomination
    /// </summary>
    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlDenomination.DataSource = Denomination;
            ddlDenomination.DataTextField = "Description";
            ddlDenomination.DataValueField = "ID";
            ddlDenomination.DataBind();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Deseriliaze the service Object
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    #region Frequency
    ///// <summary>
    ///// To Load Frequency
    ///// </summary>
    //private void FunPriLoadFrequency()
    //{
    //    try
    //    {
    //        objSerClient = new ReportOrgColMgtServicesClient();
    //        byte[] byteLobs = objSerClient.FunPubGetFrequencyDetails();
    //        List<ClsPubFrequencyDetails> FrequencyDetails = (List<ClsPubFrequencyDetails>)DeSeriliaze(byteLobs);
    //        grvFrequency.DataSource = FrequencyDetails;
    //        grvFrequency.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    finally
    //    {
    //        objSerClient.Close();
    //    }
    //}
    #endregion


    /// <summary>
    /// To Validate From Report Date
    /// </summary>
    private void FunPriValidateFromEndDate()
    {
        try
        {
            #region Validate Report Date
            int Compareyear = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(0, 4));
            int Comparemonth = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(4, 2));

            if (Utility.StringToDate(txtReportDate.Text).Year < Compareyear)
            {
                Utility.FunShowAlertMsg(this, "Report Date should be greater than the last date of To month and Year");
                txtReportDate.Text = "";
                //((CheckBox)grvAssets.HeaderRow.FindControl("chkAssetClass")).Checked = false;
                FunPriValidateGrid();
                return;
            }
            else
            {
                if (Utility.StringToDate(txtReportDate.Text).Year == Compareyear)
                {
                    if (Utility.StringToDate(txtReportDate.Text).Month < Comparemonth)
                    {
                        Utility.FunShowAlertMsg(this, "Report Date should be greater than the last date of To month and Year");
                        txtReportDate.Text = "";
                        //((CheckBox)grvAssets.HeaderRow.FindControl("chkAssetClass")).Checked = false;
                        FunPriValidateGrid();
                        return;
                    }
                }
            }
            #endregion

            #region Validate Gap Days
            //if (Convert.ToInt32(ddlFromYearMonthCompare.SelectedIndex) > 0 && Convert.ToInt32(ddlToYearMonthCompare) > 0)
            //{
            //    int Baseyear = int.Parse((ddlToYearMonthCompare.SelectedItem.Value).Substring(0, 4));
            //    int Basemonth = int.Parse((ddlToYearMonthCompare.SelectedItem.Value).Substring(4, 2));
            //    int BaseFromYear = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(0, 4));
            //    int BaseFromMonth = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(4, 2));
            //    int CompareFromYear = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(0, 4));
            //    int CompareFromMonth = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(4, 2));
            //    if (FunPriGapDays(BaseFromYear, BaseFromMonth, Baseyear, Comparemonth) != FunPriGapDays(CompareFromYear, CompareFromMonth, Compareyear, Basemonth))
            //    {
            //        Utility.FunShowAlertMsg(this, "Gap Month Between From Year Month and To Year month should be equal");
            //        ddlFromYearMonthBase.ClearSelection();
            //        ddlFromYearMonthCompare.ClearSelection();
            //        ddlToYearMonthCompare.ClearSelection();
            //        ddlToYearMonthBase.ClearSelection();
            //        return;
            //    }
            //}
            #endregion

            //if (ddlRegion.SelectedIndex > 0 && ddlBranch.SelectedIndex == 0)
            //{
            // RequiredFieldValidator rfvchkAssetClass=(RequiredFieldValidator)grvAssets.FindControl("rfvchkAssetClass");
            // rfvchkAssetClass.Enabled=true;
            //string assetkeyword = "";// To assign asset keyowrd(i.e.)CMT
            //int chk = 0;
            //for (int i = 0; i < grvFrequency.Rows.Count; i++)
            //{
            //    if (!((CheckBox)grvFrequency.Rows[i].FindControl("ChkSelect")).Checked)
            //    {
            //        chk++;
            //    }

            //}
            //if (chk == grvFrequency.Rows.Count)
            //{
            //    CVFrequency.ErrorMessage = "Select Any Frequency";
            //    CVFrequency.IsValid = false;
            //    return;
            //}
            //else
            //{
            //    CVFrequency.ErrorMessage = "";
            //    CVFrequency.IsValid = true;
            //}
            //if (!((CheckBox)grvAssets.HeaderRow.FindControl("chkAssetClass")).Checked)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Select Asset Class");
            //    pnlDemandCollection.Visible = false;
            //    grvDemand.DataSource = null;
            //    grvDemand.DataBind();
            //    btnPrint.Visible = false;
            //    lblAmounts.Visible = false;
            //    return;
            //}
            //else
            //{
            //    CVAssetClass.ErrorMessage = "";
            //    CVAssetClass.IsValid = true;
            //}

            FunPriLoadDemandCollectionDetails();    // bind grid

            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            Session["LOB"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            //TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            //txtCustomerName.Text = txtName.Text;
            //if (txtCustomerName.Text != string.Empty)
            //{
            //    objHeader.Customer = txtCustomerName.Text;
            //}
            //else
            //{
            //    objHeader.Customer = "";
            //}
            //if (ddlCustomerGroup.SelectedIndex > 0)
            //{
            //    objHeader.CustomerGroup = ddlCustomerGroup.SelectedItem.Text;
            //}
            //else
            //{
            //    objHeader.CustomerGroup = "";
            //}
            //if (ddlPNum.SelectedIndex > 0)
            //{
            //    objHeader.PANum = ddlPNum.SelectedValue;
            //    if (ddlSNum.SelectedIndex > 0)
            //    {
            //        objHeader.SANum = ddlSNum.SelectedValue;
            //    }
            //    else
            //    {
            //        objHeader.SANum = "";

            //    }
            //}
            //else
            //{
            //    objHeader.PANum = "";
            //    objHeader.SANum = "";

            //}
            objHeader.FromYearMonth = ddlFromYearMonthBase.SelectedItem.Text;
            objHeader.ToYearMonth = ddlToYearMonthBase.SelectedItem.Text;
            Session["Header"] = objHeader;
            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            lblAmounts.Visible = true;
            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Load Financial Years
    /// </summary>
    /// <param name="ddlSourceControl"></param>
    public static void LoadFinancialYears(DropDownList ddlSourceControl)
    {
        try
        {
            int intCurrentYear = DateTime.Now.Year;
            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");

            //ddlSourceControl.Items.Insert(0, liSelect);
            if (DateTime.Now.Month > Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]))
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear - 1)), ((intCurrentYear - 2) + "-" + (intCurrentYear - 1)));
                ddlSourceControl.Items.Insert(0, liPSelect);
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }
            else
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear - 1)), ((intCurrentYear - 2) + "-" + (intCurrentYear - 1)));
                ddlSourceControl.Items.Insert(0, liPSelect);
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    public static void LoadCompareFinancialYears(DropDownList ddlSourceControl)
    {
        try
        {
            int intCurrentYear = DateTime.Now.Year;
            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");

            //ddlSourceControl.Items.Insert(0, liSelect);
            if (DateTime.Now.Month > Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]))
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 3) + "-" + (intCurrentYear - 2)), ((intCurrentYear - 3) + "-" + (intCurrentYear - 2)));
                ddlSourceControl.Items.Insert(0, liPSelect);
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear-1)), ((intCurrentYear - 2) + "-" + (intCurrentYear-1)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear-1) + "-" + (intCurrentYear)), ((intCurrentYear-1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }
            else
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 3) + "-" + (intCurrentYear - 2)), ((intCurrentYear - 3) + "-" + (intCurrentYear - 2)));
                ddlSourceControl.Items.Insert(0, liPSelect);
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear-1)), ((intCurrentYear - 2) + "-" + (intCurrentYear-1)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear-1) + "-" + (intCurrentYear )), ((intCurrentYear-1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }
        }
        catch (Exception exp)
        {
            throw exp;
        }
    }

    /// <summary>
    /// To find the Gap Months 
    /// </summary>
    /// <param name="FromYear"></param>
    /// <param name="Frommonth"></param>
    /// <param name="ToYear"></param>
    /// <param name="ToMonth"></param>
    /// <returns></returns>
    private int FunPriGapDays(int FromYear, int Frommonth, int ToYear, int ToMonth)
    {
        try
        {
            DateTime dtFrom = new DateTime(FromYear, Frommonth, 1);
            DateTime dtTo = new DateTime(ToYear, ToMonth, 1);

            int totalMonths = ((dtTo.Year - dtFrom.Year) * 12) + dtTo.Month - dtFrom.Month;
            return totalMonths++;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To validate the future date 
    /// </summary>
    /// <param name="YearMonth"></param>
    private void FunPriValidateFutureDate(DropDownList ddl)
    {
        try
        {
            #region To find Current Year and Month
            //string Today = Convert.ToString(DateTime.Now);
            string YearMonth = ddl.SelectedItem.Text;
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            #endregion

            int year = int.Parse(YearMonth.Substring(0, 4));
            int Month = int.Parse(YearMonth.Substring(4, 2));
            if (year > Currentyear)
            {
                ddl.ClearSelection();
                Utility.FunShowAlertMsg(this, "Year should not be Greater than the Current Year");
                return;
            }
            else if (year == Currentyear)
            {
                if (Month > Currentmonth)
                {
                    ddl.ClearSelection();
                    Utility.FunShowAlertMsg(this, "Month should not be Greater than the Current Month");
                    return;
                }
            }            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #region Validate Frequency
    ///// <summary>
    ///// To Validate the Frequency
    ///// </summary>
    //private void FunPriDisableFrequency()
    //{
    //    try
    //    {
    //        if (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text)
    //        {
    //            ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = true;
    //        }
    //        else if ((ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[6].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[7].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text))
    //        {
    //            ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //        }
    //        else if ((ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[3].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[6].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[7].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[10].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //            (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //            (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text))
    //        {
    //            ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = false;
    //            ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //        }
    //        else
    //        {
    //            ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //            ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = false;
    //            ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = false;
    //            ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //}
    #endregion

    /// <summary>
    /// To Clear the fields
    /// </summary>
    private void FunPriClearSelection()
    {
        try
        {
            
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            ucCustomerCodeLov.FunPubClearControlValue();
            string s = hdnCustomerId.Value;
            hdnCustID.Value = null;
            ddlLOB.ClearSelection();
            ddlPNum.Items.Clear();
            FunPriLoadGroup(CompanyId);
            ddlSNum.Items.Clear();
            txtReportDate.Text = "";
            ddlDenomination.ClearSelection();
            ddlFinacialYearBase.SelectedValue = FunPriLoadCurrentFinancialYear();
            ddlFinancialYearCompare.SelectedValue = ddlFinacialYearBase.SelectedValue;
            ddlFromYearMonthBase.ClearSelection();
            ddlFromYearMonthCompare.ClearSelection();
            ddlToYearMonthBase.ClearSelection();
            ddlToYearMonthCompare.ClearSelection();
            ddlFromYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
            ddlToYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
            ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
            ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
            FunPriValidateGrid();
            btnPrint.Visible = false;

            
            //lblCustomerGroup.CssClass = "styleReqFieldLabel";
            //lblPNum.CssClass = "styleReqFieldLabel";
            //lblCustomerName.CssClass = "styleReqFieldLabel";



        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #region Clear Frequency
    ///// <summary>
    ///// To Clear the selected frequency
    ///// </summary>
    //private void FunPriClearFrequency()
    //{
    //    ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = false;
    //    ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = false;
    //    ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = false;
    //    ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;

    //    ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Checked = false;
    //    ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Checked = false;
    //    ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Checked = false;
    //    ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Checked = false;
    //}
    #endregion

    /// <summary>
    /// To Load the Demand Collection Grid
    /// </summary>
    private void FunPriLoadDemandCollectionDetails()
    {
        try
        {
            btnPrint.Visible = true;
            btnPrint.Enabled = true;
            lblError.Text = "";
            pnlDemandCollection.Visible = true;
            divDemand.Style.Add("display", "block");
            objSerClient = new ReportOrgColMgtServicesClient();
            ClsPubDemandParameterDetails DemandParameter = new ClsPubDemandParameterDetails();
            DemandParameter.CompanyId = CompanyId;
            DemandParameter.UserId = UserId;
            DemandParameter.LobId = ddlLOB.SelectedValue;
           // if (txtCustomerName.Text != "")
            if(hdnCustID.Value !=null)
            {
                DemandParameter.CustomerId = hdnCustID.Value;
            }
            else
            {
                DemandParameter.CustomerId = "";
            }
            if (ddlCustomerGroup.SelectedIndex != 0)
            {
                DemandParameter.GroupId = ddlCustomerGroup.SelectedValue;
            }
            else
            {
                DemandParameter.GroupId = "";
            }
            if (ddlPNum.SelectedIndex != 0)
            {
                DemandParameter.PANum = ddlPNum.SelectedValue;
                if (ddlSNum.SelectedIndex != 0)
                {
                    DemandParameter.SANum = ddlSNum.SelectedValue;
                }
                else
                {
                    DemandParameter.SANum = "";

                }
            }
            else
            {
                DemandParameter.PANum = "";
                DemandParameter.SANum = "";
                
            }
            //DemandParameter.AssetTypeId = Convert.ToInt32(ddlCategories.SelectedValue);
            //if (ddlSNum.SelectedIndex != 0)
            //{
            //    DemandParameter.SANum = ddlSNum.SelectedValue;
            //}
            //else
            //{
            //    DemandParameter.SANum = "";

            //}
            //if (ViewState["Id"] != null)
            //    DemandParameter.FrequencyId = ViewState["Id"].ToString();
            string FinYearStarYearMonthStartDate = ddlFromYearMonthBase.Items[1].Text;
            int FinYearStartyear = int.Parse(FinYearStarYearMonthStartDate.Substring(0, 4));
            int FinYearStartMonth = int.Parse(FinYearStarYearMonthStartDate.Substring(4, 2));
            DateTime FinYear_StartMonth_StartDate = Convert.ToDateTime(FinYearStartMonth + "/" + "1" + "/" + FinYearStartyear);
            DemandParameter.FinYearStartMonthStartDate = FinYear_StartMonth_StartDate;

            // From month Start Date
            int BaseFrommonth = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(4, 2));
            int BaseFromYear = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(0, 4));
            DateTime FromMonth_StartDate = Convert.ToDateTime(BaseFrommonth + "/" + "1" + "/" + BaseFromYear);
            DemandParameter.FromMonthStartDate = FromMonth_StartDate;

            //From Month Previous Month End Date
            int PreviousMonth;
            if (BaseFrommonth == 1)
            {
                PreviousMonth = 12;
            }
            else
            {
                PreviousMonth = BaseFrommonth - 1;
            }
            string PreFromMonthEndDate = System.DateTime.DaysInMonth(BaseFromYear, PreviousMonth).ToString();
            DateTime FromMonth_PreMonth_EndDate = Convert.ToDateTime(PreviousMonth + "/" + PreFromMonthEndDate + "/" + BaseFromYear);
            DemandParameter.FromMonthPreMonthEndDate = FromMonth_PreMonth_EndDate;

            //To Month End Date
            int BaseTomonth = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(4, 2));
            int BaseToYear = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(0, 4));
            string ToMonthEndDate = System.DateTime.DaysInMonth(BaseToYear, BaseTomonth).ToString();
            DateTime ToMonth_EndDate = Convert.ToDateTime(BaseTomonth + "/" + ToMonthEndDate + "/" + BaseToYear);
            DemandParameter.ToMonthEndDate = ToMonth_EndDate;

            //Compare Funancial Year Start Month Start Date
            if (ddlFromYearMonthCompare.SelectedIndex > 0)
            {
                string CompareFinYearStarYearMonthStartDate = ddlFromYearMonthCompare.Items[1].Text;
                int CompareFinYearStartyear = int.Parse(CompareFinYearStarYearMonthStartDate.Substring(0, 4));
                int CompareFinYearStartMonth = int.Parse(CompareFinYearStarYearMonthStartDate.Substring(4, 2));
                DateTime Compare_FinYear_StartMonth_StartDate = Convert.ToDateTime(CompareFinYearStartMonth + "/" + "1" + "/" + CompareFinYearStartyear);
                DemandParameter.CompareFinYearStartMonthStartDate = Compare_FinYear_StartMonth_StartDate;
            }


            //Compare From month Start Date
            if (ddlFromYearMonthCompare.SelectedIndex > 0)
            {
                int CompareFrommonth = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(4, 2));
                int CompareFromYear = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(0, 4));
                DateTime Compare_FromMonth_StartDate = Convert.ToDateTime(CompareFrommonth + "/" + "1" + "/" + CompareFromYear);
                DemandParameter.CompareFromMonthStartDate = Compare_FromMonth_StartDate;



            }
            // else
            // {
            //DemandParameter.CompareFromMonthStartDate = "";
            // }

            //Compare From Month Previous Month End Date
            if (ddlFromYearMonthCompare.SelectedIndex > 0)
            {
                int ComparePreviousMonth;
                int ComparePreFrommonth = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(4, 2));
                int ComparePreFromYear = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(0, 4));

                if (ComparePreFrommonth == 1)
                {
                    ComparePreviousMonth = 12;
                }
                else
                {
                    ComparePreviousMonth = ComparePreFrommonth - 1;
                }
                string ComparePreFromMonthEndDate = System.DateTime.DaysInMonth(ComparePreFromYear, ComparePreviousMonth).ToString();
                DateTime Compare_FromMonth_PreMonth_EndDate = Convert.ToDateTime(ComparePreviousMonth + "/" + ComparePreFromMonthEndDate + "/" + ComparePreFromYear);
                DemandParameter.CompareFromMonthPreMonthEndDate = Compare_FromMonth_PreMonth_EndDate;
            }
            //else
            //{
            //    DemandParameter.CompareFromMonthPreMonthEndDate = "";
            //}
            //Compare To Month End Date
            if (ddlToYearMonthCompare.SelectedIndex > 0)
            {
                int CompareTomonth = int.Parse((ddlToYearMonthCompare.SelectedItem.Value).Substring(4, 2));
                int CompareToYear = int.Parse((ddlToYearMonthCompare.SelectedItem.Value).Substring(0, 4));
                string CompareToMonthEndDate = System.DateTime.DaysInMonth(CompareToYear, CompareTomonth).ToString();
                DateTime Compare_ToMonth_EndDate = Convert.ToDateTime(CompareTomonth + "/" + CompareToMonthEndDate + "/" + CompareToYear);
                DemandParameter.CompareToMonthEndDate = Compare_ToMonth_EndDate;
            }
            DemandParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            byte[] byteDemandDetail = ClsPubSerialize.Serialize(DemandParameter, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetDCCLDetails(byteDemandDetail);
            List<ClsPubDemandCollection> DemandCollectionDetails = (List<ClsPubDemandCollection>)DeSeriliaze(byteLobs);
            TotOpnDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.OpeningDemand);
            TotOpnCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.OpeningCollection);
            //TotOpnPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.OpeningPercentage);
            TotMonDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.MonthlyDemand);
            TotMonCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.MonthlyCollection);
            TotMonPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.MonthlyPercentage);
            TotClsDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.ClosingDemand);
            TotClsCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.ClosingCollection);
            TotClsPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.ClosingPercentage);
            Session["DemandCollection"] = DemandCollectionDetails;
            grvDemand.DataSource = DemandCollectionDetails;
            grvDemand.DataBind();

            if (grvDemand.Rows.Count == 0)
            {
                Session["DemandCollection"] = null;
                btnPrint.Enabled = false;
                lblError.Text = "No Records Found";
                grvDemand.DataBind();
            }
            else
            {
                FunPriDisplayTotal();
            }
            //if (grvDemand.Rows.Count != 0)
            //{
            //    grvDemand.HeaderRow.Style.Add("position", "relative");
            //    grvDemand.HeaderRow.Style.Add("z-index", "auto");
            //    grvDemand.HeaderRow.Style.Add("top", "auto");
            //}

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To find the Total
    /// </summary>
    private void FunPriDisplayTotal()
    {
        ((Label)grvDemand.FooterRow.FindControl("lbltotOpeningDemand")).Text = TotOpnDemand.ToString(Funsetsuffix());
        ((Label)grvDemand.FooterRow.FindControl("lbltotOpeningCollection")).Text = TotOpnCollection.ToString(Funsetsuffix());
        //((Label)grvDemand.FooterRow.FindControl("lbltotOpeningPercentage")).Text = TotOpnPercentage.ToString(Funsetsuffix());
        ((Label)grvDemand.FooterRow.FindControl("lbltotMonthlyDemand")).Text = TotMonDemand.ToString(Funsetsuffix());
        ((Label)grvDemand.FooterRow.FindControl("lbltotMonthlyCollection")).Text = TotMonCollection.ToString(Funsetsuffix());
        //((Label)grvDemand.FooterRow.FindControl("lbltotMonthlyPercentage")).Text = TotMonPercentage.ToString(Funsetsuffix());
        ((Label)grvDemand.FooterRow.FindControl("lbltotClosingDemand")).Text = TotClsDemand.ToString(Funsetsuffix());
        ((Label)grvDemand.FooterRow.FindControl("lbltotClosingCollection")).Text = TotClsCollection.ToString(Funsetsuffix());
        //((Label)grvDemand.FooterRow.FindControl("lbltotClosingPercentage")).Text = TotClsPercentage.ToString(Funsetsuffix());

    }

    /// <summary>
    /// To set the suffix from GPS
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// To Clear the Session
    /// </summary>
    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Denomination"] = null;
        //Session["Currency"] = null;
        Session["DemandCollection"] = null;
        Session["Company"] = null;
        Session["Date"] = null;
    }

    private Dictionary<int, string> FunPriFillArrayDemandMonth(DropDownList ddlFinacialYear)
    {
        dictDemandmonth = new Dictionary<int, string>();
        int intActualMonth = Convert.ToInt32(ClsPubConfigReader.FunPubReadConfig("StartMonth"));
        string[] Years = ddlFinacialYear.SelectedItem.Text.Split('-');
        string strActualYear = Years[0].ToString();
        for (int intMonthCnt = 1; intMonthCnt <= 12; intMonthCnt++)
        {
            if (intActualMonth >= 13)
            {
                intActualMonth = 1;
                strActualYear = Years[1].ToString();
            }
            System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(strActualYear + intActualMonth.ToString("00"), strActualYear + intActualMonth.ToString("00"));
            //DemandArrayList[intMonthCnt - 1] = liPSelect.Text;
            dictDemandmonth.Add(intMonthCnt, liPSelect.Text);
            intActualMonth = intActualMonth + 1;
        }
        return dictDemandmonth;
    }

    private string FunPriGetDemandMonths(DropDownList ddlFrom, DropDownList ddlTo, Dictionary<int, string> dictDemandmonths)
    {
        string strDemandmnth = "";
        //dictDemandmonths = (Dictionary<int, string>)ViewState["DemandMonth"];
        strDemandmnth = "<Root>";
        foreach (KeyValuePair<int, string> kvp in dictDemandmonths)
        {

            if (Convert.ToInt64(kvp.Value) >= Convert.ToInt64(ddlFrom.SelectedItem.Text) && Convert.ToInt64(kvp.Value) <= Convert.ToInt64(ddlTo.SelectedItem.Text))
            {
                strDemandmnth += "<Details  Demand_Month= '" + kvp.Value + "' />";
            }

        }
        strDemandmnth += "</Root>";

        return strDemandmnth;
    }

    /// <summary>
    ///To Validate the Grid 
    /// </summary>
    private void FunPriValidateGrid()
    {
        lblAmounts.Visible = false;
        pnlDemandCollection.Visible = false;
        grvDemand.DataSource = null;
        grvDemand.DataBind();
        btnPrint.Visible = false;
    }

    public string GetMonthName(int month)
    {
        string monthName = string.Empty;
        switch (month)
        {
            case 1:
                monthName = "Jan";
                break;
            case 2:
                monthName = "Feb";
                break;
            case 3:
                monthName = "Mar";
                break;
            case 4:
                monthName = "April";
                break;
            case 5:
                monthName = "May";
                break;
            case 6:
                monthName = "June";
                break;
            case 7:
                monthName = "July";
                break;
            case 8:
                monthName = "Aug";
                break;
            case 9:
                monthName = "Sep";
                break;
            case 10:
                monthName = "Oct";
                break;
            case 11:
                monthName = "Nov";
                break;
            case 12:
                monthName = "Dec";
                break;
        }

        return monthName;

    }

    #endregion

    #region Page Events

    #region DropdownList Events
    /// <summary>
    /// To fill the Financial Months Based on FinacialYear Base
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFinacialYearBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFinacialYearBase.SelectedIndex > -1)
            {

                ddlFromYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ddlToYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ViewState["BaseMonths"] = FunPriFillArrayDemandMonth(ddlFinacialYearBase);
                //return;
            }
            if (pnlDemandCollection.Visible == true)
            {
                FunPriValidateGrid();
            }
            //if (ddlToYearMonthBase.SelectedIndex <= 0 && ddlFromYearMonthBase.SelectedIndex <= 0)
            //{
            //    FunPriClearFrequency();
            //}
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Load Financial Months.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To fill the Financial Months Based on FinacialYear Compare.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFinancialYearCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFinancialYearCompare.SelectedIndex > -1)
            {

                ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
                ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
                ViewState["CompaareMonths"] = FunPriFillArrayDemandMonth(ddlFinancialYearCompare);
                //return;
            }
            if (pnlDemandCollection.Visible == true)
            {
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Load Financial Months.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate the Base To Year Month. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlToYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select a Line of Business");
                ddlToYearMonthBase.ClearSelection();
                return;
            }
            if (ddlFromYearMonthBase.SelectedIndex > ddlToYearMonthBase.SelectedIndex)
            {
                Utility.FunShowAlertMsg(this, "To Year Month should be greater than or Equal From Year and month ");
                ddlToYearMonthBase.ClearSelection();
                return;
            }
            if (ddlToYearMonthBase.SelectedIndex > 0 && ddlFromYearMonthBase.SelectedIndex > 0)
            {
                //FunPriClearFrequency();
                FunPriValidateFutureDate(ddlToYearMonthBase);
                //FunPriDisableFrequency();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Select From Year and month ");
                ddlToYearMonthBase.ClearSelection();
                return;
            }
            if (grvDemand.Rows.Count > 0)
            {
                FunPriValidateGrid();
            }
            if (ddlToYearMonthBase.SelectedIndex > 0 && ddlFromYearMonthBase.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", CompanyId.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@XMLParamtDemandMonthLists", FunPriGetDemandMonths(ddlFromYearMonthBase, ddlToYearMonthBase, (Dictionary<int, string>)ViewState["BaseMonths"]));
                DataSet ds = new DataSet();
                ds = Utility.GetDataset("S3G_RPT_GetDemandMonthDetails", Procparam);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string StrErrorMsg = "Demand not run for the selected months ";
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        StrErrorMsg += "(";
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            StrErrorMsg += dr["Demand_Month"].ToString() + ",";
                        }
                        StrErrorMsg = StrErrorMsg.Substring(0, StrErrorMsg.Length - 1);
                        StrErrorMsg += ")";
                        Utility.FunShowAlertMsg(this, StrErrorMsg);
                        ddlToYearMonthBase.ClearSelection();
                        return;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Base To Year Month.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate the Compare To Year Month. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlToYearMonthCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select a Line of Business");
                ddlToYearMonthCompare.ClearSelection();
                return;
            }
            if (ddlFromYearMonthCompare.SelectedIndex > ddlToYearMonthCompare.SelectedIndex)
            {
                Utility.FunShowAlertMsg(this, "To Year Month should be greater than or Equal to From Year and month ");
                ddlToYearMonthCompare.ClearSelection();
                return;
            }
            if (ddlToYearMonthCompare.SelectedIndex > 0)
            {
                FunPriValidateFutureDate(ddlToYearMonthCompare);
                
                if (ddlFromYearMonthCompare.SelectedIndex == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select From Year and month ");
                    ddlToYearMonthCompare.ClearSelection();
                    return;
                }
            }
            if (ddlToYearMonthBase.SelectedIndex > 0)
            {
                int BaseTomonth = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(4, 2));
                int CompareToMonth = int.Parse((ddlToYearMonthCompare.SelectedItem.Value).Substring(4, 2));
                if (CompareToMonth > BaseTomonth || CompareToMonth < BaseTomonth)
                {
                    Utility.FunShowAlertMsg(this, "Compare To Month should be equal to Base To month ");
                    ddlToYearMonthCompare.ClearSelection();
                    return;
                }
            }
            if (pnlDemandCollection.Visible == true)
            {
                FunPriValidateGrid();
            }
            if (ddlToYearMonthCompare.SelectedIndex > 0 && ddlFromYearMonthCompare.SelectedIndex > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", CompanyId.ToString());
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@XMLParamtDemandMonthLists", FunPriGetDemandMonths(ddlFromYearMonthCompare, ddlToYearMonthCompare, (Dictionary<int, string>)ViewState["CompaareMonths"]));
                DataSet ds = new DataSet();
                ds = Utility.GetDataset("S3G_RPT_GetDemandMonthDetails", Procparam);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string StrErrorMsg = "Demand not runned for the selected months ";
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        StrErrorMsg += "(";
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            StrErrorMsg += dr["Demand_Month"].ToString() + ",";
                        }
                        StrErrorMsg = StrErrorMsg.Substring(0, StrErrorMsg.Length - 1);
                        StrErrorMsg += ")";
                        Utility.FunShowAlertMsg(this, StrErrorMsg);
                        ddlToYearMonthCompare.ClearSelection();
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Compare To Year Month.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate Base From Year Month 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFromYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select a Line of Business");
                ddlFromYearMonthBase.ClearSelection();
                return;
            }
            if (ddlToYearMonthBase.SelectedIndex > 0)
            {
                if (ddlFromYearMonthBase.SelectedIndex > ddlToYearMonthBase.SelectedIndex)
                {
                    Utility.FunShowAlertMsg(this, "From Year Month should not be greater than the To Year and month ");
                    ddlFromYearMonthBase.ClearSelection();
                    return;
                }
            }
            if (ddlFromYearMonthBase.SelectedIndex > 0)
            {
                FunPriValidateFutureDate(ddlFromYearMonthBase);
            }
            if (pnlDemandCollection.Visible == true)
            {
                if (ddlToYearMonthBase.SelectedIndex > 0)
                {
                    ddlToYearMonthBase.ClearSelection();
                }
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Base From Year Month.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate Base From Year Month 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlFromYearMonthCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select a Line of Business");
                ddlFromYearMonthCompare.ClearSelection();
                return;
            }
            if (ddlToYearMonthCompare.SelectedIndex > 0)
            {
                if (ddlFromYearMonthCompare.SelectedIndex > ddlToYearMonthCompare.SelectedIndex)
                {
                    Utility.FunShowAlertMsg(this, "From Year Month should not be greater than the To Year and month ");
                    ddlFromYearMonthCompare.ClearSelection();
                    return;
                }
            }
            if (ddlFromYearMonthCompare.SelectedIndex > 0)
            {
                FunPriValidateFutureDate(ddlFromYearMonthCompare);
            }
            if (ddlFromYearMonthBase.SelectedIndex > 0)
            {
                int Basemonth = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(4, 2));
                int CompareMonth = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(4, 2));
                if (CompareMonth > Basemonth || CompareMonth < Basemonth)
                {
                    Utility.FunShowAlertMsg(this, "Compare From Month should be equal to Base From month ");
                    ddlFromYearMonthCompare.ClearSelection();
                    return;
                }
            }
            if (pnlDemandCollection.Visible == true)
            {
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Compare From Year Month.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (grvDemand.Rows.Count >= 0)
            {
                FunPriValidateGrid();
            }

            ucCustomerCodeLov.FunPubClearControlValue();
            hdnCustID.Value = "";
            //ddlCustomerGroup.SelectedValue = "-1";
            FunPriLoadGroup(CompanyId);
            ddlPNum.ClearSelection();

            ddlSNum.Items.Clear();
            System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
            ddlSNum.Items.Insert(0, liSelect1);
            FunPriLoadGroupPANLOB("L");
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Grid.";
            CVDemandCollection.IsValid = false;
        }
    }
    /// <summary>
    /// To Load Customer and Prime Account Number based on group 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (grvDemand.Rows.Count > 0)
            {
                FunPriValidateGrid();
            }
            //ddlSNum.Items.Clear();
            //objSerClient = new ReportOrgColMgtServicesClient();
            //HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if ((ddlCustomerGroup.SelectedIndex > 0))
            {
                ddlSNum.Items.Clear();
                //FunPriLoadPrime("G", ddlCustomerGroup.SelectedValue, CompanyId);
                FunPriLoadGroupPANLOB("LG");
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Sub Account Number, Customer and Group based on Prime Account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
                if (grvDemand.Rows.Count > 0)
                {
                    FunPriValidateGrid();
                }
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                btnPrint.Visible = false;
                ddlSNum.Items.Clear();
                lblSNum.CssClass = "styleDisplayLabel";
                FunPriLoadGroupPANLOB("P");
                FunPriLoadSAN();
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Sub Account Number";
            CVDemandCollection.IsValid = false;
        }
    }

    private void FunPriLoadSAN()
    {
        try
        {
            ddlSNum.Items.Clear();
            objSerClient = new ReportOrgColMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetDemandSAN(ddlPNum.SelectedValue);
            List<ClsPubDropDownList> SANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlSNum.DataSource = SANum;
            ddlSNum.DataTextField = "Description";
            ddlSNum.DataValueField = "ID";
            ddlSNum.DataBind();
            if (ddlSNum.Items.Count == 2)
            {
                ddlSNum.SelectedIndex = 1;
            }
            else
                ddlSNum.SelectedIndex = 0;
            byteLobs = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Grid based on the Denomination.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (grvDemand.Rows.Count > 0)
            {
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Validate Grid.";
            CVDemandCollection.IsValid = false;
        }
    }

    #endregion

    #region Button (Customer/Ok/Clear/Print)
    /// <summary>
    /// To Load group and Prime Account Number based on Customer 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedValue != "-1")
            {
                if (grvDemand.Rows.Count > 0)
                {
                    FunPriValidateGrid();
                }
                objSerClient = new ReportOrgColMgtServicesClient();
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                if (hdnCustomerId != null && hdnCustomerId.Value != "")
                {
                    hdnCustID.Value = hdnCustomerId.Value;
                }

                ddlPNum.Items.Clear();
                System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlPNum.Items.Insert(0, liSelect);

                ddlSNum.Items.Clear();
                System.Web.UI.WebControls.ListItem liSelect1 = new System.Web.UI.WebControls.ListItem("--Select--", "-1");
                ddlSNum.Items.Insert(0, liSelect1);

                FunPriLoadGroupPANLOB("C");
            }
            else
            {

                Utility.FunShowAlertMsg(this .Page, "Select Line of Business ");
                ucCustomerCodeLov.FunPubClearControlValue();
                return;
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Bind the Grid and Validate the Fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if ((hdnCustID.Value == string.Empty) && (ddlCustomerGroup.SelectedIndex == 0) && (ddlPNum.SelectedIndex == 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Select either a Customer Name, Customer Group Name or Account Number");
                return;
            }
            
            ClearSession();
            FunPriValidateFromEndDate();

        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Load Asset Categories.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Clear the Fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearSelection();
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Load Asset Categories.";
            CVDemandCollection.IsValid = false;
        }
    }

    /// <summary>
    /// To Redirect to the Report Page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strScipt = "window.open('../Reports/S3GRptDemandCCLReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Demand", strScipt, true);
        }
        catch (Exception ex)
        {
            CVDemandCollection.ErrorMessage = "Unable to Open Report Page.";
            CVDemandCollection.IsValid = false;
        }
    }

    #endregion

    #region Grid Events

    protected void grvDemand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbltotOpeningDemand = e.Row.FindControl("lbltotOpeningDemand") as Label;
            Label lbltotOpeningCollection = e.Row.FindControl("lbltotOpeningCollection") as Label;
            Label lbltotOpeningPercentage = e.Row.FindControl("lbltotOpeningPercentage") as Label;

            Label lbltotMonthlyDemand = e.Row.FindControl("lbltotMonthlyDemand") as Label;
            Label lbltotMonthlyCollection = e.Row.FindControl("lbltotMonthlyCollection") as Label;
            Label lbltotMonthlyPercentage = e.Row.FindControl("lbltotMonthlyPercentage") as Label;

            Label lbltotClosingDemand = e.Row.FindControl("lbltotClosingDemand") as Label;
            Label lbltotClosingCollection = e.Row.FindControl("lbltotClosingCollection") as Label;
            Label lbltotClosingPercentage = e.Row.FindControl("lbltotClosingPercentage") as Label;


            lbltotOpeningDemand.Text = TotOpnDemand.ToString(Funsetsuffix());
            lbltotOpeningCollection.Text = TotOpnCollection.ToString(Funsetsuffix());

            if (TotOpnDemand == 0)
                lbltotOpeningPercentage.Text = 0.ToString(Funsetsuffix());
            else
                lbltotOpeningPercentage.Text = ((TotOpnCollection / TotOpnDemand) * 100).ToString(Funsetsuffix());

            lbltotMonthlyDemand.Text = TotMonDemand.ToString(Funsetsuffix());
            lbltotMonthlyCollection.Text = TotMonCollection.ToString(Funsetsuffix());

            if (TotMonDemand == 0)
                lbltotMonthlyPercentage.Text = 0.ToString(Funsetsuffix());
            else
                lbltotMonthlyPercentage.Text = ((TotMonCollection / TotMonDemand) * 100).ToString(Funsetsuffix());

            lbltotClosingDemand.Text = TotClsDemand.ToString(Funsetsuffix());
            lbltotClosingCollection.Text = TotClsCollection.ToString(Funsetsuffix());

            if (TotClsDemand == 0)
                lbltotClosingPercentage.Text = 0.ToString(Funsetsuffix());
            else
                lbltotClosingPercentage.Text = ((TotClsCollection / TotClsDemand) * 100).ToString(Funsetsuffix());
        }
    }
    #endregion

    #endregion
}