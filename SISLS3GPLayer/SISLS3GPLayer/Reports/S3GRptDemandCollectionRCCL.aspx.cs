 
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
using S3GBusEntity;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;
using ReportOrgColMgtServicesReference;
 
public partial class Reports_S3GRptDemandCollectionRCCL : ApplyThemeForProject
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    int intProgramId = 177;
    string RegionId;
    bool Is_Active;
    int AssetTypeId;
    public string strDateFormat;
    public string strCurrency;
    //Dictionary<string, string> Procparam;
    string strPageName = "Demand Collection Region Customer Code Level";
    string Selection;
    decimal TotOpnDemand;
    decimal TotOpnCollection;
    decimal TotOpnPercentage;
    decimal TotClsDemand;
    decimal TotClsCollection;
    decimal TotClsPercentage;
    decimal TotMonDemand;
    decimal TotMonCollection;
    decimal TotMonPercentage;   
    ReportAccountsMgtServicesClient objSerClient;
    ReportOrgColMgtServicesClient ObjOrgColClient;
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    Dictionary<int, string> dictDemandmonth = new Dictionary<int, string>();
 
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();       
    }
    private void FunPriLoadPage()
    {
        try
        {
            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            ObjUserInfo = new UserInfo();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strCurrency = ObjS3GSession.ProCurrencyNameRW;
            Session["CompanyName"] = ObjUserInfo.ProCompanyNameRW;// to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                      // assigning the first textbox with the End date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtReportDate.Attributes.Add("readonly", "readonly");
            txtReportDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtReportDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            #endregion
               
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;            
            if (!IsPostBack)
            {
                FunPriClearSession();
                FunPriLoadLob();
                FunPriLoadRegion();
                FunPriLoadLocation();
                FunPubLoadDenomination();
                //FunPriLoadFrequency();
                //FunPriLoadAssetCategoriesType();
                LoadFinancialYears(ddlFinacialYearBase);
                ddlFinacialYearBase.SelectedValue = FunPriLoadCurrentFinancialYear();
                //LoadFinancialYears(ddlFinancialYearCompare);
                LoadCompareFinancialYears(ddlFinancialYearCompare);
                ViewState["BaseMonths"] = FunPriFillArrayDemandMonth(ddlFinacialYearBase);
                ViewState["CompaareMonths"] = FunPriFillArrayDemandMonth(ddlFinancialYearCompare);
                ddlFinancialYearCompare.SelectedValue = ddlFinacialYearBase.SelectedValue;
                ddlFromYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ddlToYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
                ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
                ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
                PnlDemandCollectionCusomerCodeLevel.Visible = false;
                BtnPrint.Visible = false;
                lblCurrency.Visible = true;
                ddlBranch.Enabled = false;
            }
            //ScriptManager.RegisterStartupScript(this, GetType(),"te", "Resize();", true);
            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "fnScaleFactorX();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load DC Customer Code Level page");
        }
    }
    private void FunPriLoadLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteLobs = objSerClient.FunPubLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count == 2)
                ddlLOB.SelectedIndex = 1;
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
    private void FunPriLoadRegion()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlRegion.DataSource = Region;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "--ALL--";
            //ddlRegion.Items[0].Text = "ALL";
            //if (ddlRegion.Items.Count == 2)
            //{
            //    ddlRegion.SelectedIndex = 1;
            //}
            //else
            //    ddlRegion.SelectedIndex = 0;
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
    private void FunPriLoadLocation()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Region;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            //ddlRegion.Items[0].Text = "ALL";
            //if (ddlRegion.Items.Count == 2)
            //{
            //    ddlRegion.SelectedIndex = 1;
            //}
            //else
            //    ddlRegion.SelectedIndex = 0;
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
    private void FunPriLoadBranch()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlRegion.SelectedIndex != 0)
                Location1 = Convert.ToInt32(ddlRegion.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, intlob_Id, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            if (ddlBranch.Items.Count == 2)
            {
                if (ddlRegion.SelectedIndex != 0)
                {
                    ddlBranch.SelectedIndex = 1;
                    Utility.ClearDropDownList(ddlBranch);
                }
                else
                    ddlBranch.SelectedIndex = 0;
            }
            else
            {
                ddlBranch.Items[0].Text = "--ALL--";
                ddlBranch.SelectedIndex = 0;
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
    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteDenomination = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSeriliaze(byteDenomination);
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

    //public void FunPriLoadAssetCategoriesType()
    //{
    //    try
    //    {
    //        ObjOrgColClient = new ReportOrgColMgtServicesClient();
    //        byte[] byteAssetCategoriesType = ObjOrgColClient.FunPubGetAssetCategoriesType();
    //        List<ClsPubDropDownList> AssetCategoriesType = (List<ClsPubDropDownList>)DeSeriliaze(byteAssetCategoriesType);
    //        DdlAssetCategoriesType.DataSource = AssetCategoriesType;
    //        DdlAssetCategoriesType.DataTextField = "Description";
    //        DdlAssetCategoriesType.DataValueField = "ID";
    //        DdlAssetCategoriesType.DataBind();
    //    }
    //    catch (Exception e)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(e);
    //        throw e;
    //    }

    //    finally
    //    {
    //        ObjOrgColClient.Close();
    //    }
    //}
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    //private void FunPriLoadFrequency()
    //{
    //    try
    //    {
    //        ObjOrgColClient = new ReportOrgColMgtServicesClient();
    //        byte[] byteLobs = ObjOrgColClient.FunPubGetFrequencyDetails();
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
    //        ObjOrgColClient.Close();
    //    }
    //}   
    private void FunPriValidateFromEndDate()
    {
        try
        {

            
            int Compareyear = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(0, 4));
            int Comparemonth = int.Parse((ddlToYearMonthBase.SelectedItem.Value).Substring(4, 2));

            if (Utility.StringToDate(txtReportDate.Text).Year < Compareyear)
            {
                Utility.FunShowAlertMsg(this, "Report Date should be greater than the last date of To month and Year");
                txtReportDate.Text = "";
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
                        return;
                    }

                }

            }

            //int BaseFromYear = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(0, 4));
            //int BaseFromMonth = int.Parse((ddlFromYearMonthBase.SelectedItem.Value).Substring(4, 2));
            //int CompareFromYear = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(0, 4));
            //int CompareFromMonth = int.Parse((ddlFromYearMonthCompare.SelectedItem.Value).Substring(4, 2));
            //if (FunPriGapDays(BaseFromYear, BaseFromMonth, Baseyear, Comparemonth) != FunPriGapDays(CompareFromYear, CompareFromMonth, Compareyear, Basemonth))
            //{
            //    Utility.FunShowAlertMsg(this, "Gap Month Between From Year Month and To Year and month should be equal");
            //    ddlFromYearMonthBase.ClearSelection();
            //    ddlFromYearMonthCompare.ClearSelection();
            //    ddlToYearMonthCompare.ClearSelection();
            //    ddlToYearMonthBase.ClearSelection();
            //    return;
            //}
            FunPriLoadDemandCollectionDetails();           
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    public static void LoadFinancialYears(DropDownList ddlSourceControl)
    {
        try
        {
            int intCurrentYear = DateTime.Now.Year;
            //System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");

            //ddlSourceControl.Items.Insert(0, liSelect);
            if (DateTime.Now.Month > 3)
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(0, liPSelect);

                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear) + "-" + (intCurrentYear + 1)), ((intCurrentYear) + "-" + (intCurrentYear + 1)));
                ddlSourceControl.Items.Insert(1, liCSelect);

                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear + 1) + "-" + (intCurrentYear + 2)), ((intCurrentYear + 1) + "-" + (intCurrentYear + 2)));
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
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear - 1)), ((intCurrentYear - 2) + "-" + (intCurrentYear - 1)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }
            else
            {
                System.Web.UI.WebControls.ListItem liPSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 3) + "-" + (intCurrentYear - 2)), ((intCurrentYear - 3) + "-" + (intCurrentYear - 2)));
                ddlSourceControl.Items.Insert(0, liPSelect);
                System.Web.UI.WebControls.ListItem liCSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 2) + "-" + (intCurrentYear - 1)), ((intCurrentYear - 2) + "-" + (intCurrentYear - 1)));
                ddlSourceControl.Items.Insert(1, liCSelect);
                System.Web.UI.WebControls.ListItem liNSelect = new System.Web.UI.WebControls.ListItem(((intCurrentYear - 1) + "-" + (intCurrentYear)), ((intCurrentYear - 1) + "-" + (intCurrentYear)));
                ddlSourceControl.Items.Insert(2, liNSelect);
            }


        }
        catch (Exception exp)
        {
            throw exp;
        }

    }
    private int FunPriGapDays(int FromYear, int Frommonth, int ToYear, int ToMonth)
    {
        DateTime dtFrom = new DateTime(FromYear, Frommonth, 1);
        DateTime dtTo = new DateTime(ToYear, ToMonth, 1);

        int totalMonths = ((dtTo.Year - dtFrom.Year) * 12) + dtTo.Month - dtFrom.Month;
        return totalMonths++;
    }
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
            //if (year > Currentyear || Month > Currentmonth)
            //{
            //    ddl.ClearSelection();
            //    Utility.FunShowAlertMsg(this, "Year Month should not be Greater than the Current Year and month");
            //    return;
            //}  
            if (year > Currentyear)
            {
                ddl.ClearSelection();
                Utility.FunShowAlertMsg(this, "Year Month should not be Greater than the Current Year and month");
                return;
            }
            else if (year == Currentyear)
            {
                if (Month > Currentmonth)
                {
                    ddl.ClearSelection();
                    Utility.FunShowAlertMsg(this, "Year Month should not be Greater than the Current Year and month");
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
    //private void FunPriDisableFrequency()
    //{
    //    if (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text)
    //    {
    //        ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = true;
    //    }
    //    else if ((ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[6].Text) ||
    //    (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[7].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text))
    //    {
    //        ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //    }
    //    else if ((ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[3].Text) ||
    //    (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[6].Text) ||
    //    (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[7].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //    (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[10].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text) ||
    //    (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[1].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[9].Text) ||
    //        (ddlFromYearMonthBase.SelectedItem.Text == ddlFromYearMonthBase.Items[4].Text && ddlToYearMonthBase.SelectedItem.Text == ddlToYearMonthBase.Items[12].Text))
    //    {
    //        ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = false;
    //        ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //    }
    //    else
    //    {
    //        ((CheckBox)grvFrequency.Rows[0].FindControl("ChkSelect")).Enabled = true;
    //        ((CheckBox)grvFrequency.Rows[1].FindControl("ChkSelect")).Enabled = false;
    //        ((CheckBox)grvFrequency.Rows[2].FindControl("ChkSelect")).Enabled = false;
    //        ((CheckBox)grvFrequency.Rows[3].FindControl("ChkSelect")).Enabled = false;
    //    }
    //}
    private void FunPriClearSelection()
    {
        ddlLOB.ClearSelection();
        ddlRegion.ClearSelection();
        ddlBranch.ClearSelection();
        //DdlAssetCategoriesType.ClearSelection();
        FunPriLoadLocation();
        ddlBranch.Enabled = false;
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
        //ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
        ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);       
        //FunPriClearFrequency();    
        FunPriClearGroupingCriteria();
        grvDemandCollectionCustomerCodeLevel.DataSource = null;
        grvDemandCollectionCustomerCodeLevel.DataBind(); 
        PnlDemandCollectionCusomerCodeLevel.Visible = false;
        lblCurrency.Visible = false;
        BtnPrint.Visible = false;
        FunPriClearSession();
    }
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

    private void FunPriClearGroupingCriteria()
    {
        ChkAccountLevel.Checked = false;
        chkCustomerCodeLevel.Checked = false;
        ChkCustomerGroupCodeLevel.Checked = false;
    }

    private void FunPriClearSession()
    {
        Session.Remove("CompanyName");
        Session.Remove("Denomination");
        Session.Remove("DemandParameter");
        Session.Remove("Currency");
        Session.Remove("DemandCollection");
        Session.Remove("TotOpnDemand");
        Session.Remove("TotOpnCollection");
        Session.Remove("OpeningPecentage");
        Session.Remove("TotMonDemand");
        Session.Remove("TotMonCollection");
        Session.Remove("TotMonPercentage");
        Session.Remove("TotClsDemand");
        Session.Remove("TotMonCollection");
        Session.Remove("TotMonPercentage");
        Session.Remove("Date");
    }

    protected void ddlFinacialYearBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriValidateGrid();
        
        if (ddlFinacialYearBase.SelectedIndex > -1)
        {

            ddlFromYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
            ddlToYearMonthBase.FillFinancialMonth(ddlFinacialYearBase.SelectedItem.Text);
            ViewState["BaseMonths"] = FunPriFillArrayDemandMonth(ddlFinacialYearBase);
            //return;
        }
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
    protected void ddlFinancialYearCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ddlFinancialYearCompare.SelectedIndex > -1)
        {

            ddlFromYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
            ddlToYearMonthCompare.FillFinancialMonth(ddlFinancialYearCompare.SelectedItem.Text);
            ViewState["CompaareMonths"] = FunPriFillArrayDemandMonth(ddlFinancialYearCompare);
            //return;
        }
    }
    protected void ddlToYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ddlLOB.SelectedIndex <= 0)
        {
            Utility.FunShowAlertMsg(this, "Select a Line of Business");
            ddlToYearMonthBase.ClearSelection();
            return;
        }

        if (ddlFromYearMonthBase.SelectedIndex > ddlToYearMonthBase.SelectedIndex)
        {
            Utility.FunShowAlertMsg(this, "To Year Month should be greater than or Equal to From Year and month");
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
        if (ddlToYearMonthBase.SelectedIndex > 0 && ddlFromYearMonthBase.SelectedIndex > 0)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
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
    protected void ddlToYearMonthCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ddlLOB.SelectedIndex <= 0)
        {
            Utility.FunShowAlertMsg(this, "Select a Line of Business");
            ddlToYearMonthCompare.ClearSelection();
            return;
        }
        if (ddlFromYearMonthCompare.SelectedIndex > ddlToYearMonthCompare.SelectedIndex)
        {
            Utility.FunShowAlertMsg(this, "To Year Month should be greater than the From Year and month ");
            ddlToYearMonthCompare.ClearSelection();
            return;
        }
        if (ddlToYearMonthCompare.SelectedIndex > 0)
        {
            FunPriValidateFutureDate(ddlToYearMonthCompare);
            //ddlToYearMonthCompare.ClearSelection();
            if (ddlFromYearMonthCompare.SelectedIndex == 0)
            {
                Utility.FunShowAlertMsg(this, "Select From Year and month ");
                ddlToYearMonthCompare.ClearSelection();
                return;
            }
        }
        if (ddlFromYearMonthCompare.SelectedIndex < 0 && ddlToYearMonthCompare.SelectedIndex > 0)
        {
            Utility.FunShowAlertMsg(this, "Select From Year and month ");
            ddlToYearMonthCompare.ClearSelection();
            return;
        }
        if (ddlToYearMonthCompare.SelectedIndex > 0 && ddlFromYearMonthCompare.SelectedIndex > 0)
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_Id", intCompanyId.ToString());
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
    protected void ddlFromYearMonthBase_SelectedIndexChanged(object sender, EventArgs e)
    {        
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            if (ddlToYearMonthBase.SelectedIndex > 0)
            {
                ddlToYearMonthBase.ClearSelection();
            }
            FunPriValidateGrid();
        }
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
                //FunPriClearFrequency();

                return;
            }
        }
        if (ddlFromYearMonthBase.SelectedIndex > 0)
        {
            //FunPriClearFrequency();
            FunPriValidateFutureDate(ddlFromYearMonthBase);           
            //FunPriDisableFrequency();

        }
        if (ddlFromYearMonthBase.SelectedIndex > 0 && ddlToYearMonthBase.SelectedIndex > 0)
        {
            //FunPriClearFrequency();
            //FunPriDisableFrequency();
        }

    }
    protected void ddlFromYearMonthCompare_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunPriValidateGrid();
        
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

    }
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        try
        {
            ddlBranch.Enabled = true;
            if (ddlRegion.SelectedIndex > 0)
            {
                FunPriLoadBranch();
            }
            else
            {
                ddlBranch.Enabled = false;
                FunPriLoadLocation();
            }
            FunPriValidateGrid();
            txtReportDate.Text = string.Empty;
            ddlFromYearMonthBase.SelectedIndex = 0;
            ddlToYearMonthBase.SelectedIndex = 0;
            ddlToYearMonthCompare.SelectedIndex = 0;
            ddlFromYearMonthCompare.SelectedIndex = 0;
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }

        finally
        {
            objSerClient.Close();
        }
    }
    protected void rbtlCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (ChkAccountLevel.Checked == true || chkCustomerCodeLevel.Checked == true || ChkCustomerGroupCodeLevel.Checked == true)
        {
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
            //    Utility.FunShowAlertMsg(this, "Please select a Frequency type");
            //    return;
            //}
           
            
                FunPriValidateFromEndDate();
                if (!IsPostBack)
                {
                    FunPriEnableGridColumns();
                }

               
        }
        else
        {
            Utility.FunShowAlertMsg(this, "Select one grouping criteria");
        }
       // string strGroupingCriteriaText;
       // if (ChkAccountLevel.Checked == true)
       // {
       //     strGroupingCriteriaText = "AccountLevel";
       //     Session["GroupingCriteria"] = strGroupingCriteriaText;
       // }
       // if (chkCustomerCodeLevel.Checked == true)
       // {
       //     strGroupingCriteriaText = "CustomerCodeLevel";
       //     Session["GroupingCriteria"] = strGroupingCriteriaText;
       // }
       //if(ChkCustomerGroupCodeLevel.Checked==true)
       // {
       //     strGroupingCriteriaText = "CustomerGroupCodeLevel";
       //     Session["GroupingCriteria"] = strGroupingCriteriaText;
       // }
       
        //FunPriEnableGridColumns();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearSelection();
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }


    protected void txtReportDate_TextChanged(object sender, EventArgs e)
    {
        //DateTime DtReportDateTime = Utility.StringToDate(txtReportDate.Text);
        //int ReportDateMonth = DtReportDateTime.Month;
        //int BaseFromYear = int.Parse((ddlToYearMonthBase.SelectedItem.Text).Substring(0, 4));
        //int BaseFromMonth = int.Parse((ddlToYearMonthBase.SelectedItem.Text).Substring(4, 2));
        //Utility.FunShowAlertMsg(this, "Report Date Cannot be less than selected ToYearMonth");        
    }
    protected void chkCustomerCodeLevel_CheckedChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (chkCustomerCodeLevel.Checked == true)
        {
            ChkCustomerGroupCodeLevel.Checked = false;
            ChkAccountLevel.Checked = false;            
        }
        //ddlToYearMonthBase.SelectedIndex = 0;
        //ddlToYearMonthCompare.SelectedIndex = 0;
        //ddlFromYearMonthBase.SelectedIndex = 0;
        //ddlFromYearMonthCompare.SelectedIndex = 0;
    }
    protected void ChkCustomerGroupCodeLevel_CheckedChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ChkCustomerGroupCodeLevel.Checked == true)
        {
            chkCustomerCodeLevel.Checked = false;
            ChkAccountLevel.Checked = false;           
        }
        //ddlToYearMonthBase.SelectedIndex = 0;
        //ddlToYearMonthCompare.SelectedIndex = 0;
        //ddlFromYearMonthBase.SelectedIndex = 0;
        //ddlFromYearMonthCompare.SelectedIndex = 0;
    }
    protected void ChkAccountLevel_CheckedChanged(object sender, EventArgs e)
    {
        if (PnlDemandCollectionCusomerCodeLevel.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ChkAccountLevel.Checked == true)
        {
            chkCustomerCodeLevel.Checked = false;
            ChkCustomerGroupCodeLevel.Checked = false;            
        }
        //ddlToYearMonthBase.SelectedIndex = 0;
        //ddlToYearMonthCompare.SelectedIndex = 0;
        //ddlFromYearMonthBase.SelectedIndex = 0;
        //ddlFromYearMonthCompare.SelectedIndex = 0;
    }

    //protected void Chkselect_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox Chkselect = (CheckBox)sender;
    //    string strFieldAtt = ((CheckBox)sender).ClientID;
    //    string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvFrequency_")).Replace("grvFrequency_ctl", "");
    //    int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
    //    gRowIndex = gRowIndex - 2;
    //    for (int i = 0; i <= grvFrequency.Rows.Count - 1; i++)
    //    {
    //        if (i != gRowIndex)
    //        {
    //            CheckBox chkselect = (CheckBox)grvFrequency.Rows[i].FindControl("Chkselect");
    //            chkselect.Checked = false;
    //        }
    //        else if (i == gRowIndex)
    //        {
    //            Label lblFrequencyId = (Label)grvFrequency.Rows[i].FindControl("lblFrequencyId");
    //            ViewState["Id"] = lblFrequencyId.Text;
    //        }
    //    }
    //}

    private void FunPriLoadDemandCollectionDetails()
    {
        try
        {
            BtnPrint.Visible = true;
            PnlDemandCollectionCusomerCodeLevel.Visible = true;
            divDemand.Style.Add("display", "block");
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubDemandParameterCCL DemandParameter1 = DemandHeaderRpt();


            ClsPubDemandParameterDetails DemandParameter = new ClsPubDemandParameterDetails();
            DemandParameter.CompanyId = intCompanyId;
            DemandParameter.LobId = ddlLOB.SelectedValue;
            Session["Denomination"] = ddlDenomination.SelectedItem.Text;

            if (ddlRegion.SelectedIndex != 0)
            {
                DemandParameter.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            else
            {
                DemandParameter.LocationId1 = 0;
            }
            if (ddlBranch.SelectedIndex != 0)
            {
                DemandParameter.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            else
            {
                DemandParameter.LocationId2 = 0;

            }
            // Funancial Year Start Month Start Date

            //string FinYearStarYearMonthStartDate="";

            //FinYearStarYearMonthStartDate =ClsPubConfigReader.FunPubReadConfig("StartMonth");
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
            if (chkCustomerCodeLevel.Checked == true)
            {
                DemandParameter.GroupingCriteriaId = Convert.ToString(1);
                Session["ReportLevel"] = " at Customer Level";
                Selection = "C";
                if (ddlBranch.SelectedIndex == 0)
                {
                    Selection += "B";
                }
                if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
                {
                    Selection += "D";
                }
                Session["Selection"] = Selection;
            }
            else if (ChkCustomerGroupCodeLevel.Checked == true)
            {
                DemandParameter.GroupingCriteriaId = Convert.ToString(2);
                Session["ReportLevel"] = " at Customer Group Level";
                Selection = "G";
                if (ddlBranch.SelectedIndex == 0)
                {
                    Selection += "B";
                }
                if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
                {
                    Selection += "D";
                }
                Session["Selection"] = Selection;
            }
            else
            {
                DemandParameter.GroupingCriteriaId = Convert.ToString(3);
                Session["ReportLevel"] = " at Account Level";
                Selection = "A";
                if (ddlBranch.SelectedIndex == 0)
                {
                    Selection += "B";
                }
                if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
                {
                    Selection += "D";
                }
                Session["Selection"] = Selection;
            }
            //if (ViewState["Id"] != null)
            //{
            //    DemandParameter.FrequencyId = ViewState["Id"].ToString();
            //}
            //DemandParameter.AssetTypeId = Convert.ToInt32(DdlAssetCategoriesType.SelectedValue);
            DemandParameter.FromMonth = ddlFromYearMonthBase.SelectedValue;
            DemandParameter.ToMonth = ddlToYearMonthBase.SelectedValue;
            if (ddlFromYearMonthCompare.SelectedIndex != 0)
            {
                DemandParameter.PreFromMonth = ddlFromYearMonthCompare.SelectedValue;
            }
            else
            {
                DemandParameter.PreFromMonth = null;
            }
            if (ddlToYearMonthCompare.SelectedIndex != 0)
            {
                DemandParameter.PreToMonth = ddlToYearMonthCompare.SelectedValue;
            }
            else
            {
                DemandParameter.PreToMonth = null;
            }
            DemandParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            DemandParameter.LobName = ddlLOB.SelectedItem.Text.Split('-')[1].ToString();
            DemandParameter.ProgramId = intProgramId;
            DemandParameter.UserId = intUserId;
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                DemandParameter.BranchName = ddlBranch.SelectedItem.ToString();
            }
            DemandParameter.RegionName = ddlRegion.SelectedItem.ToString();
            List<ClsPubDemandParameterDetails> ObjListDemandParameter = new List<ClsPubDemandParameterDetails>();
            ObjListDemandParameter.Add(DemandParameter);




            byte[] byteDemandDetail = ClsPubSerialize.Serialize(DemandParameter, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetDCRegionCLDetails(byteDemandDetail);
            List<ClsPubDCRegionCustomerCodeGridDetails> DemandCollectionDetails = (List<ClsPubDCRegionCustomerCodeGridDetails>)DeSeriliaze(byteLobs);
            TotOpnDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection => (ClsPubDemandCollection.OpeningDemand));
            TotOpnCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection =>(ClsPubDemandCollection.OpeningCollection));
            //TotOpnPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.OpeningPercentage);
            if (TotOpnDemand != 0)
            {
                TotOpnPercentage = (TotOpnCollection / TotOpnDemand) * 100;
            }
            else
            {
                TotOpnPercentage = 0;
                //Session["OpeningPecentage"] = TotOpnPercentage;
            }
            TotMonDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection =>(ClsPubDemandCollection.MonthlyDemand));
            TotMonCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection =>(ClsPubDemandCollection.MonthlyCollection));
            //TotMonPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.MonthlyPercentage);
            if (TotMonDemand != 0)
            {
                TotMonPercentage = (TotMonCollection / TotMonDemand) * 100;
            }
            else
            {
                TotMonPercentage = 0;
                //Session["TotMonPercentage"] = TotMonPercentage;
            }
            TotClsDemand = DemandCollectionDetails.Sum(ClsPubDemandCollection =>(ClsPubDemandCollection.ClosingDemand));
            TotClsCollection = DemandCollectionDetails.Sum(ClsPubDemandCollection =>(ClsPubDemandCollection.ClosingCollection));
            //TotClsPercentage = DemandCollectionDetails.Sum(ClsPubDemandCollection => ClsPubDemandCollection.ClosingPercentage);
            if (TotClsDemand != 0)
            {
                TotClsPercentage = (TotClsCollection / TotClsDemand) * 100;
            }
            else
            {
                TotClsPercentage = 0;
                //Session["TotClsPercentage"] = TotClsPercentage;
            }
            grvDemandCollectionCustomerCodeLevel.DataSource = DemandCollectionDetails;
            grvDemandCollectionCustomerCodeLevel.DataBind();
            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Currency"] = lblCurrency.Text = "[All Amounts are in" + " " +strCurrency + "]";
            }
            else
            {
                Session["Currency"] = lblCurrency.Text = "[All Amounts are" + " " +strCurrency + " " + "in"+ " " +Session["Denomination"].ToString()+"]";
            }
            lblCurrency.Visible = true;
            Session["DemandCollection"] = DemandCollectionDetails;
            FunPriEnableGridColumns();
            if (grvDemandCollectionCustomerCodeLevel.Rows.Count == 0)
            {
                grvDemandCollectionCustomerCodeLevel.EmptyDataText = "No Records Found";
                grvDemandCollectionCustomerCodeLevel.DataBind();
                BtnPrint.Enabled = false;
                lblCurrency.Visible = false;
            }
            else
            {
                FunPriDisableGridColumns();
                FunPriDisplayTotal();
                BtnPrint.Enabled = true;
            }
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }

        finally
        {
            ObjOrgColClient.Close();
        }
    }

    private ClsPubDemandParameterCCL DemandHeaderRpt()
    {
        ClsPubDemandParameterCCL DemandParameter = new ClsPubDemandParameterCCL();
        DemandParameter.CompanyId = intCompanyId;
        DemandParameter.LobId = ddlLOB.SelectedValue;
        Session["Denomination"] = ddlDenomination.SelectedItem.Text;

        if (ddlRegion.SelectedIndex != 0)
        {
            DemandParameter.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
        }
        else
        {
            DemandParameter.LocationId1 = 0;
        }
        if (ddlBranch.SelectedIndex != 0)
        {
            DemandParameter.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
        }
        else
        {
            DemandParameter.LocationId2 = 0;

        }
        // Funancial Year Start Month Start Date

        //string FinYearStarYearMonthStartDate="";

        //FinYearStarYearMonthStartDate =ClsPubConfigReader.FunPubReadConfig("StartMonth");
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
        if (chkCustomerCodeLevel.Checked == true)
        {
            DemandParameter.GroupingCriteriaId = Convert.ToString(1);
            Session["ReportLevel"] = " at Customer Level";
            Selection = "C";
            if (ddlBranch.SelectedIndex == 0)
            {
                Selection += "B";
            }
            if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
            {
                Selection += "D";
            }
            Session["Selection"] = Selection;
        }
        else if (ChkCustomerGroupCodeLevel.Checked == true)
        {
            DemandParameter.GroupingCriteriaId = Convert.ToString(2);
            Session["ReportLevel"] = " at Customer Group Level";
            Selection = "G";
            if (ddlBranch.SelectedIndex == 0)
            {
                Selection += "B";
            }
            if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
            {
                Selection += "D";
            }
            Session["Selection"] = Selection;
        }
        else
        {
            DemandParameter.GroupingCriteriaId = Convert.ToString(3);
            Session["ReportLevel"] = " at Account Level";
            Selection = "A";
            if (ddlBranch.SelectedIndex == 0)
            {
                Selection += "B";
            }
            if (ddlFromYearMonthBase.SelectedIndex != ddlToYearMonthBase.SelectedIndex)
            {
                Selection += "D";
            }
            Session["Selection"] = Selection;
        }
        //if (ViewState["Id"] != null)
        //{
        //    DemandParameter.FrequencyId = ViewState["Id"].ToString();
        //}
        //DemandParameter.AssetTypeId = Convert.ToInt32(DdlAssetCategoriesType.SelectedValue);
        DemandParameter.FromMonth = ddlFromYearMonthBase.SelectedValue;
        DemandParameter.ToMonth = ddlToYearMonthBase.SelectedValue;
        if (ddlFromYearMonthCompare.SelectedIndex != 0)
        {
            DemandParameter.PreFromMonth = ddlFromYearMonthCompare.SelectedValue;
        }
        else
        {
            DemandParameter.PreFromMonth = null;
        }
        if (ddlToYearMonthCompare.SelectedIndex != 0)
        {
            DemandParameter.PreToMonth = ddlToYearMonthCompare.SelectedValue;
        }
        else
        {
            DemandParameter.PreToMonth = null;
        }
        DemandParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
        DemandParameter.LobName = ddlLOB.SelectedItem.Text.Split('-')[1].ToString();
        
        if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
        {
            DemandParameter.BranchName = ddlBranch.SelectedItem.ToString();
        }
        DemandParameter.RegionName = ddlRegion.SelectedItem.ToString();
        List<ClsPubDemandParameterCCL> ObjListDemandParameter = new List<ClsPubDemandParameterCCL>();
        ObjListDemandParameter.Add(DemandParameter);
        Session["DemandParameter"] = ObjListDemandParameter;
        return DemandParameter;
    }
    private void FunPriDisableGridColumns()
    {
        if (ChkAccountLevel.Checked == true)
        {
            grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
            
            if (Convert.ToInt32(ddlBranch.SelectedValue) == -1)
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = true;
            }
            else
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            }
            //grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[5].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[5].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[1].Visible = false;
            
            //divDemand.Style["width"] = "50px";
        }
        if (chkCustomerCodeLevel.Checked == true)
        {
            grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;            
            if (Convert.ToInt32(ddlBranch.SelectedValue) == -1)
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = true;
            }
            else
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            }
            //grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[5].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[6].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[7].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[1].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
        }
        if (ChkCustomerGroupCodeLevel.Checked == true)
        {
            grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;            
            if (Convert.ToInt32(ddlBranch.SelectedValue) == -1)
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = true;
            }
            else
            {
                grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            }
            //grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[3].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[4].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[6].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[7].Visible = false;
            grvDemandCollectionCustomerCodeLevel.Columns[1].Visible = false;
            //grvDemandCollectionCustomerCodeLevel.Columns[2].Visible = false;
        }        
    }
    private void FunPriEnableGridColumns()
    {
        grvDemandCollectionCustomerCodeLevel.Columns[4].Visible = true;
        grvDemandCollectionCustomerCodeLevel.Columns[5].Visible = true;
        grvDemandCollectionCustomerCodeLevel.Columns[6].Visible = true;
        grvDemandCollectionCustomerCodeLevel.Columns[7].Visible = true;
        //divDemand.Style["width"] = "100%";
    }
    private void FunPriDisplayTotal()
    {
        Session["TotOpnDemand"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotOpeningDemand")).Text = TotOpnDemand.ToString(Funsetsuffix());
        Session["TotOpnCollection"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotOpeningCollection")).Text = TotOpnCollection.ToString(Funsetsuffix());
        Session["OpeningPecentage"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotOpeningPercentage")).Text = TotOpnPercentage.ToString(Funsetsuffix());
        Session["TotMonDemand"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotMonthlyDemand")).Text = TotMonDemand.ToString(Funsetsuffix());
        Session["TotMonCollection"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotMonthlyCollection")).Text = TotMonCollection.ToString(Funsetsuffix());
        Session["TotMonPercentage"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotMonthlyPercentage")).Text = TotMonPercentage.ToString(Funsetsuffix());
        Session["TotClsDemand"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotClosingDemand")).Text = TotClsDemand.ToString(Funsetsuffix());
        Session["TotClsCollection"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotClosingCollection")).Text = TotClsCollection.ToString(Funsetsuffix());
        Session["TotClsPercentage"]=((Label)grvDemandCollectionCustomerCodeLevel.FooterRow.FindControl("lbltotClosingPercentage")).Text = TotClsPercentage.ToString(Funsetsuffix());

    }
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

    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        string strScript = "window.open('../Reports/S3GRptDemandCollectionRCCLReport.aspx','newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Demand Collection Region Customer Code Level", strScript, true);   
    }

    private void FunPriValidateGrid()
    {
        lblCurrency.Visible = false;
        PnlDemandCollectionCusomerCodeLevel.Visible = false;
        grvDemandCollectionCustomerCodeLevel.DataSource = null;
        grvDemandCollectionCustomerCodeLevel.DataBind();
        BtnPrint.Visible = false;
        //if (chkCustomerCodeLevel.Checked == true)
        //{
        //    chkCustomerCodeLevel.Checked = false;
        //}
        //if (ChkAccountLevel.Checked==true)
        //{
        //    ChkAccountLevel.Checked = false;
        //}
        //if (ChkCustomerGroupCodeLevel.Checked==true)
        //{
        //    ChkCustomerGroupCodeLevel.Checked = false;
        //}
        //ddlFromYearMonthBase.SelectedIndex = 0;
        //ddlFromYearMonthCompare.SelectedIndex = 0;
        //ddlToYearMonthBase.SelectedIndex = 0;
        //ddlToYearMonthCompare.SelectedIndex = 0;
        //txtReportDate.Text = string.Empty;
        // btnExcel.Visible = false;
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkCustomerCodeLevel.Checked == true)
            {
                chkCustomerCodeLevel.Checked = false;
            }
            if (ChkAccountLevel.Checked == true)
            {
                ChkAccountLevel.Checked = false;
            }
            if (ChkCustomerGroupCodeLevel.Checked == true)
            {
                ChkCustomerGroupCodeLevel.Checked = false;
            }
            FunPriLoadLocation();
            ddlBranch.Enabled = false;
            FunPriLoadRegion();
            txtReportDate.Text = string.Empty;
            FunPriValidateGrid();
            ddlFromYearMonthCompare.SelectedIndex = 0;
            ddlFromYearMonthBase.SelectedIndex = 0;
            ddlToYearMonthCompare.SelectedIndex = 0;
            ddlToYearMonthBase.SelectedIndex = 0;
        }
        catch(Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
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
}
 
 
 
 
 
