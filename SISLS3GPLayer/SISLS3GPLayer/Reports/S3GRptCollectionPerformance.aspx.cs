#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Collection Performance
/// Created By          :   Srivatsan/Gangadharrao
/// Created Date        :   
/// Purpose             :   To show the Collection Amount, Cheque Return Amount and Comparative Analysis Amount
/// Last Updated By		:  
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
using S3GBusEntity;

using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;

#endregion
public partial class Reports_S3GRptCollectionPerformance : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    string RegionId;
    bool Is_Active;
    int AssetTypeId;
    public string strDateFormat;
    int intMaxMonth;
    string strPageName = "Collection Performance";
    DataTable dtTable = new DataTable();
    // For Summary Grid
    decimal TotalAgeing0to30;
    decimal TotalAgeing31to60;
    decimal TotalAgeing61to90;
    decimal TotalAgeingAbove90;
    decimal TotalBALDUE;
    decimal ChkReturn0to30;
    decimal ChkReturn31to60;
    decimal ChkReturn61to90;
    decimal ChkReturnAbove90;
    decimal TotalPeriod1;
    decimal TotalPeriod2;
    decimal TotalPeriod10to30;
    decimal TotalPeriod20to30;
    decimal TotalPeriod130to61;
    decimal TotalPeriod230to61;
    decimal TotalPeriod160to91;
    decimal TotalPeriod260to91;
    decimal TotalPeriod1Above90;
    decimal TotalPeriod2Above90;

    
    
    
    decimal TotClsDemand;
    decimal TotClsCollection;
    decimal TotClsPercentage;
    decimal TotMonDemand;
    decimal TotMonCollection;
    decimal TotMonPercentage;
    decimal TotalChkReturnAmount;
    // For Detailed Grid
    decimal TotalArrearsAmt;
    decimal TotalOpnDemand;
    decimal TotalOpnCollection;
    decimal TotalOpnPercentage;
    decimal TotalClsDemand;
    decimal TotalClsCollection;
    decimal TotalClsPercentage;
    decimal TotalMonDemand;
    decimal TotalMonCollection;
    decimal TotalMonPercentage;
    Dictionary<string, string> Procparam = null;
    ReportAccountsMgtServicesClient objSerClient;
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient ObjOrgColClient;
    //Dictionary<string, string> Procparam = null;
    #endregion
    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    #endregion
    #region PageLoad
    private void FunPriLoadPage()
    {
           #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
         //   CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
              calNormalFromDate.Format="MM/yyyy";
              calNormalToDate.Format = "MM/yyyy";
            //txtReportDate.Attributes.Add("readonly", "readonly");
              
            #endregion
              if ((!rdbRptBasis.Items[0].Selected == true) && (!rdbRptBasis.Items[1].Selected == true))
              {
                  lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
                  PnlCompare.Visible = txtComparativeFromDate.Visible = txtComparativeToDate.Visible = RqvtxtComparativeFromDate.Enabled = RqvtxtComparativeToDate.Enabled = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
                  //txtNormalFromDate.Text = "";
                  //txtNormalToDate.Text = "";
              }

              //if ((!rdbRptBasis.Items[0].Selected == true))
              //{
              //    lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
              //    txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
              //    //txtNormalFromDate.Text = "";
              //    //txtNormalToDate.Text = "";
              //}
             
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            //Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
         
            if (!IsPostBack)
            {
                pnlChequeReturn.Visible = false;
                pnlCollectionAmount.Visible = false;
                pnlComparative.Visible = false;
                //txtNormalFromDate.Text = DateTime.Today.ToString("MM") + "/" + DateTime.Today.Year;
                //txtNormalToDate.Text = DateTime.Today.ToString("MM") + "/" + DateTime.Today.Year;
                
                //Changed on 29/11/2011 based on the advice from Malolan.
                txtNormalToDate.Text = "";
                lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
                PnlCompare.Visible = txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
                ClearSession();
                FunPriLoadLob(intCompanyId, intUserId);
                FunPriLoadBranch();
                ddlLoc2.Enabled = false;
            }
    }
    #endregion
    #region Clear Session
    private void ClearSession()
    {
        Session["CollectionAmount"]=null;
        Session["ChequeReturnAmount"]=null;
        Session["ComparativeAnalysis"] = null;
        Session["CollPerformance"] = null;
        //Session["Header"] = null;
        Session["Denomination"] = null;
        Session["FromDate"] = null;
        Session["ToDate"] = null;
        //Session["Currency"] = null;
        //Session["DemandCollection"] = null;
        //Session["Company"] = null;
        //Session["Details"] = null;

    }
    #endregion
    #region Deseriliaze Data
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    #endregion
    #region To Load the LOB Details
    private void FunPriLoadLob(int intCompany_id, int intUser_id)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            if (Procparam != null)
                Procparam.Clear();

            Procparam.Add("@Company_ID", Convert.ToString(intCompany_id));
            Procparam.Add("@User_ID", intUserId.ToString());
            //Procparam.Add("@Program_Code", ProgramCode);
            Procparam.Add("@Program_Id", "189");
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@FilterOption", "'HP','TL','TE','OL','LN','FL'");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.Items.RemoveAt(0);
                ddlLOB.Items[0].Selected = true;
                ddlLOB_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                ddlLOB.Items[0].Selected = true;
            }
         }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //objSerClient.Close();
        }
    }
    #endregion
    #region AssetCategoriesDetails
    private void FunPriLoadAssetCategoriesDetails()
    {
        try
        {
            ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();

            //if (ddlAssetType.SelectedIndex > 0)
            //{
            //    byte[] byteLobs = ObjOrgColClient.FunPubGetAssetCategories(intCompanyId,Convert.ToInt32(ddlAssetType.SelectedValue));
            //    List<ClsPubAssetCategories> AssetCategoriesDetails = (List<ClsPubAssetCategories>)DeSeriliaze(byteLobs);
            //    ddlAssetClass.DataSource = AssetCategoriesDetails;
            //    ddlAssetClass.DataTextField = "AssetClass";
            //    ddlAssetClass.DataValueField = "ClassId";
            //    ddlAssetClass.DataBind();
            
            //    ddlAssetMake.DataSource = AssetCategoriesDetails;
            //    ddlAssetMake.DataTextField = "ASSETMAKE";
            //    ddlAssetMake.DataValueField = "MAKEID";
            //    ddlAssetMake.DataBind();
            //    System.Web.UI.WebControls.ListItem liSelect = new System.Web.UI.WebControls.ListItem("--Select--", "0");
            //    ddlAssetClass.Items.Insert(0, liSelect);
            //    ddlAssetMake.Items.Insert(0, liSelect);
            //}
            //else
            //{
            //    Level_ID = 0;
            //}

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    #endregion
    #region To Bind the Branch Details
    private void FunPriLoadBranch()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            if (Procparam != null)
                Procparam.Clear();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@Program_Id", "189");
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Is_Report", "1");
            ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
            ddlBranch.Items[0].Text = "--ALL--";
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.Items[1].Selected = true;
                ddlBranch_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                ddlLoc2.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                ddlLoc2.Items[0].Text = "--ALL--";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    #endregion
    #region To load Asset Details
    private void FunPriLoadAssetTypeDetails()
    {
        try
        {

            /*------------Commented to the Remove the Asset Class,Asset Type and the Asset Make fields as per the email :
             * Asset Class, Asset type and Product in the Report grid Collection Performance Report dated Thu 27/10/2011 12:27
           */

            ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
         
            //if (ddlLOB.SelectedIndex > 0)
            //{
            //    byte[] byteLobs = ObjOrgColClient.FunPubGetAssetTypeDetails(intUserId, intCompanyId);
            //    List<ClsPubDropDownList> AssetTypeDetails = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            //    ddlAssetType.DataSource = AssetTypeDetails;
            //    ddlAssetType.DataTextField = "Description";
            //    ddlAssetType.DataValueField = "ID";
            //    ddlAssetType.DataBind();
            //}
            //else
            //{
            //    Level_ID = 0;
            //}
           
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    #endregion
    #region Date Validation with System Date
    protected void FunDateValidation(object sender, EventArgs e)
    {
        pnlCollectionAmount.Visible = false;
        pnlComparative.Visible = false;
        pnlChequeReturn.Visible = false;
        TextBox txtDate;
        txtDate = ((TextBox)sender);
        try
        {
            #region To find Current Year and Month
            //string Today = Convert.ToString(DateTime.Now);
            string YearMonth = txtDate.Text;
            int Currentmonth = DateTime.Now.Month;
            int Currentyear = DateTime.Now.Year;
            #endregion

            int Month = int.Parse(YearMonth.Substring(0, 2));
            int year = int.Parse(YearMonth.Substring(3, 4));
            if (year > Currentyear || Month > Currentmonth)
            {
                
                ((TextBox)sender).Text = DateTime.Today.ToString("MM") + "/" + DateTime.Today.Year;
                Utility.FunShowAlertMsg(this, "Month/Year cannot be Greater than System Month/Year.");
            
                return;
            }
            //if (year == Currentyear && Month == Currentmonth)
            //{
            //    txtdate.Text = "";
            //    Utility.FunShowAlertMsg(this, "Month cannot be Greater than current month");
            //    return;
            //}
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }


    }
    #endregion
    #region To Bind the Product Details
    private void FunPriLoadProductDetails()
    {
        try
        {
            ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();

            if (Convert.ToInt32(ddlLOB.SelectedValue) !=0)
            {
                byte[] byteLobs = ObjOrgColClient.FunPubGetProductsDetails(intCompanyId,Convert.ToInt32(ddlLOB.SelectedValue));
                List<ClsPubDropDownList> ProductDetails = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
                ddlProduct.DataSource = ProductDetails;
                ddlProduct.DataTextField = "Description";
                ddlProduct.DataValueField = "ID";
                ddlProduct.DataBind();
                ddlProduct.Items[0].Text = "--ALL--";
            }
            //else
            //{
            //    Level_ID = 0;
            //}

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    #endregion 
    #region To Compare Form and To Date

    private bool FunCompareDate(TextBox txtFromDate, TextBox txtToDate)
    {
        bool Flag = false;
        int FromMonth = int.Parse(txtFromDate.Text.Substring(0, 2));
        int Fromyear = int.Parse(txtFromDate.Text.Substring(3, 4));
        int ToMonth = int.Parse(txtToDate.Text.Substring(0, 2));
        int Toyear = int.Parse(txtToDate.Text.Substring(3, 4));
        if (ToMonth <= FromMonth && Fromyear == Toyear)
        {
            Flag = true;
        }
        return Flag;
    }
    #endregion


    #region To get Collection Details
    private void FunCollectionAmount()
    {
        try
        {
            lblAnlyAmtMsg.Visible = false;
            lblCollAmtMsg.Visible = false;
            lblChqRtnMsg.Visible = false;
            grvChequeReturn.DataSource = null;
            grvChequeReturn.DataBind();
            grvCollectionAmount.DataSource = null;
            grvCollectionAmount.DataBind();
            GrvComparativeAnalysis.DataSource = null;
            GrvComparativeAnalysis.DataBind();
            pnlCollectionAmount.Visible = false;
            pnlComparative.Visible = false;
            pnlChequeReturn.Visible = false;
            //if ((!chkComparativeAnalysis.Checked) && (!chkNetofChequeReturn.Checked))
            //{
            //    Utility.FunShowAlertMsg(this, "Select Atleast one CheckBox.");
            //    return;
            //}
            
            /*Code added by Srivatsan to find the date difference in months. Declaration part of the date.*/

            string strFromDate = Convert.ToString(txtNormalFromDate.Text);
            string strToDate =Convert.ToString(txtNormalToDate.Text);
            string[] splitFromDate = strFromDate.Split(new char[] { '/' });
            string[] splitToDate = strToDate.Split(new char[] { '/' });

         
            if (string.IsNullOrEmpty(txtNormalFromDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Enter a From Date.");
                imgNormalFromDate.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtNormalToDate.Text))
            {
                Utility.FunShowAlertMsg(this, "Enter a To Date.");
                imgNormalToDate.Focus();
                return;
            }
            if (Utility.CompareDates(txtNormalFromDate.Text, txtNormalToDate.Text)==-1)
            {
                Utility.FunShowAlertMsg(this, "To Month/Year Should be greater than From Month/Year in the Normal Date Range.");
                return;
            }
            if (Utility.CompareDates(txtNormalFromDate.Text, txtNormalToDate.Text) == 0)
            {
                Utility.FunShowAlertMsg(this, "To Month/Year Should be greater than From Month/Year in the Normal Date Range.");
                return;
            }
            if (rdbRptBasis.SelectedValue == "Comparative Analysis")
            {
                if (string.IsNullOrEmpty(txtComparativeFromDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter a From Date.");
                    imgComparativeFromDate.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtComparativeToDate.Text))
                {
                    Utility.FunShowAlertMsg(this, "Enter a To Date.");
                    imgComparativeToDate.Focus();
                    return;
                }

                /*Commented by Srivatsan for finding the Date differences in months */

                //int FromMonth = int.Parse(txtNormalFromDate.Text.Substring(0, 2));
                //int ToMonth = int.Parse(txtNormalToDate.Text.Substring(0, 2));
                //int CompareFromDate = int.Parse(txtComparativeFromDate.Text.Substring(0, 2));
                //int CompareToDate=int.Parse(txtComparativeToDate.Text.Substring(0,2));
                
                int Toyear = int.Parse(txtNormalToDate.Text.Substring(3, 4));
                int CompareFromYear=int.Parse(txtComparativeFromDate.Text.Substring(3, 4));

                int spanOrig = 0;
                DateTime NormFromDate = Utility.StringToDate(txtNormalFromDate.Text);
                DateTime NormEndDate = Utility.StringToDate(txtNormalToDate.Text);
                TimeSpan span;
                if ((NormFromDate.Month == 2) || (NormEndDate.Month == 2))
                {
                    span = NormEndDate.Subtract(NormFromDate);
                    spanOrig = span.Days;
                    spanOrig += 2;
                }
                else
                {
                    span = NormEndDate.Subtract(NormFromDate);
                    spanOrig = span.Days;
                }

                int NormalMonth = ((spanOrig / 30) <= 0) ? 1 : (spanOrig / 30);


                string strComFromDate = Convert.ToString(txtComparativeFromDate.Text);
                string strComToDate = Convert.ToString(txtComparativeToDate.Text);
                string[] splitComFromDate = strComFromDate.Split(new char[] { '/' });
                string[] splitComToDate = strComToDate.Split(new char[] { '/' });

                DateTime CompFromDate = Utility.StringToDate(txtComparativeFromDate.Text);
                DateTime CompToDate = Utility.StringToDate(txtComparativeToDate.Text);

                if ((CompFromDate.Month == 2) || (CompToDate.Month == 2))
                {
                    span = CompToDate.Subtract(CompFromDate);
                    spanOrig = span.Days;
                    spanOrig += 2;
                }
                else
                {
                    span = CompToDate.Subtract(CompFromDate);
                    spanOrig = span.Days;
                }

                int CompMonth = ((spanOrig / 30) <= 0) ? 1 : (spanOrig / 30);
                //if (CompareFromDate >= FromMonth && CompareFromDate <= ToMonth)
                //{
                //    Utility.FunShowAlertMsg(this, "Normal Date and Comparative should not be equal/should not between Normal Dates");
                //    return;
                //}

                //if (CompareFromDate <= ToMonth && CompareFromYear <= Toyear)
                //{
                //    Utility.FunShowAlertMsg(this, "Normal Date Should be Lesser Than Comparative Date");
                //    return;

                //}


                if (Utility.CompareDates(txtNormalFromDate.Text, txtComparativeFromDate.Text) == 0)
                {
                    Utility.FunShowAlertMsg(this, "The Normal month and Comparative month in the From date should not be the same.");
                    return;
                }
                if (Utility.CompareDates(txtNormalToDate.Text, txtComparativeToDate.Text) == 0)
                {
                    Utility.FunShowAlertMsg(this, "The Normal month and Comparative month in the To Date should not be the same.");
                    return;
                }
                if (Utility.CompareDates(txtComparativeFromDate.Text, txtComparativeToDate.Text) == 0)
                {
                    Utility.FunShowAlertMsg(this, "To Month/Year Should be greater than From Month/Year in the Comparative Date Range.");
                    return;
                }
                if ((NormalMonth) != (CompMonth))
                {
                    Utility.FunShowAlertMsg(this, "The span of the Normal month range and the Comparative month range is not in sync.");
                    return;
                }
                if (CompFromDate>NormFromDate && CompFromDate<=NormEndDate)
                {
                    Utility.FunShowAlertMsg(this, "The Compare month should not be in the range of the Normal Month range.");
                    return;
                }
                if (NormFromDate > CompFromDate && NormFromDate <= CompToDate)
                {
                    Utility.FunShowAlertMsg(this, "The Normal month should not be in the range of the Compare Month range.");
                    return;
                }
                if (Utility.CompareDates(txtComparativeFromDate.Text, txtComparativeToDate.Text)==-1)
                {
                    Utility.FunShowAlertMsg(this, "To Month/Year Should be greater than From Month/Year  in the Comparitive Date Range.");
                    return;
                }

            }

 
          
            
            ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
       
            ClsPubPerformance ObjPerformance = new ClsPubPerformance();
            if (Convert.ToInt32(ddlLOB.SelectedValue)!=0)
            {
                ObjPerformance.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            else
            {
                ObjPerformance.LOB_ID = 0;
            }
            ObjPerformance.Company_ID = intCompanyId;
            ObjPerformance.User_ID = intUserId;
            ObjPerformance.Program_ID = 189;

            if (ddlBranch.SelectedValue != "ALL")
            {
                ObjPerformance.Location_ID1 = Convert.ToInt32(ddlBranch.SelectedValue);
            }

            if (ddlLoc2.SelectedValue != "ALL")
            {
                ObjPerformance.Location_ID2 = Convert.ToInt32(ddlLoc2.SelectedValue);
            }

            //if (ddlAssetType.SelectedIndex > 0)
            //{
            //    ObjPerformance.Asset_Type = Convert.ToInt32(ddlAssetType.SelectedValue);
            //}
            //else
            //{
                ObjPerformance.Asset_Type = 0;
            //}
            if (ddlProduct.SelectedIndex > 0)
            {
                ObjPerformance.Product_ID = Convert.ToInt32(ddlProduct.SelectedValue);
            }
            //else
            //{
            //    ObjPerformance.Product_ID = 0;
            //}
            DateTime NormFromDate1 = Utility.StringToDate(txtNormalFromDate.Text);
            DateTime NormToDate1 = Utility.StringToDate(txtNormalToDate.Text);
            if ((NormToDate1.Month == 2))
            {
                ObjPerformance.From_Date = splitFromDate[0] + "/" + "01/" + splitFromDate[1];
                ObjPerformance.To_Date = splitToDate[0] + "/" + "28/" + splitToDate[1];
            }
            else
            {
                ObjPerformance.From_Date = splitFromDate[0] + "/" + "01/" + splitFromDate[1];
                ObjPerformance.To_Date = splitToDate[0] + "/" + "30/" + splitToDate[1];
            }  
     
            #region To set From Date and To Date

            if((!string.IsNullOrEmpty(txtComparativeFromDate.Text)) && (!string.IsNullOrEmpty(txtComparativeToDate.Text)))
            {
            string strComFromDate = Convert.ToString(txtComparativeFromDate.Text);
            string strComToDate = Convert.ToString(txtComparativeToDate.Text);
            string[] splitComFromDate = strComFromDate.Split(new char[] { '/' });
            string[] splitComToDate = strComToDate.Split(new char[] { '/' });
           
             strComFromDate = Utility.StringToDate("01/" + strComFromDate).ToString(ObjS3GSession.ProDateFormatRW);

                //DateTime NormFromDate = Utility.StringToDate(txtNormalFromDate.Text);
                //DateTime NormToDate = Utility.StringToDate(txtNormalToDate.Text);

                DateTime CompFromDate = Utility.StringToDate(txtComparativeFromDate.Text);
                DateTime CompToDate = Utility.StringToDate(txtComparativeToDate.Text);


                //if ((NormToDate.Month == 2))
                //{
                //    ObjPerformance.From_Date = splitFromDate[0]+"/"+"01/"+splitFromDate[1];
                //    ObjPerformance.To_Date = splitToDate[0] + "/" + "28/" + splitToDate[1];
                //}
                //else
                //{
                //    ObjPerformance.From_Date = splitFromDate[0]+"/"+"01/"+splitFromDate[1];
                //    ObjPerformance.To_Date = splitToDate[0] + "/" + "30/" + splitToDate[1];
                //}

                if(CompToDate.Month==2)
                {
                     ObjPerformance.From_ComDate = splitComFromDate[0] + "/" + "01/" + splitComFromDate[1];
                     ObjPerformance.To_ComDate = splitComToDate[0] + "/" + "28/" + splitComToDate[1];
 
                    strComToDate = Utility.StringToDate("28/" + strComToDate).ToString(ObjS3GSession.ProDateFormatRW);
                }
                else 
                {
                    ObjPerformance.From_ComDate = splitComFromDate[0] + "/" + "01/" + splitComFromDate[1];
                    ObjPerformance.To_ComDate = splitComToDate[0] + "/" + "30/" + splitComToDate[1];
                 strComToDate = Utility.StringToDate("30/" + strComToDate).ToString(ObjS3GSession.ProDateFormatRW);
                }
               
          
            Session["FromComDate"] = strComFromDate.Replace("01-", "").ToString();
            if (CompToDate.Month == 2)
            {
                Session["ToComDate"] = strComToDate.Replace("28-", "").ToString();
            }
            else
            {
                Session["ToComDate"] = strComToDate.Replace("30-", "").ToString();
            }

            }
            #endregion

            if (rdbRptBasis.SelectedValue == "Net of ChequeReturn")
            {
                ObjPerformance.Mode = 1;
            }
            else if (rdbRptBasis.SelectedValue == "Comparative Analysis")
            {
                ObjPerformance.Mode = 2;
            }
            else
            {
                ObjPerformance.Mode = 0;
            }
            #region Bind Data to Collection Amount Gridview and Display Total to Footer
            byte[] byteLobs = ObjOrgColClient.FunPubGetCollectionAmount(ObjPerformance);
            ClspubCollectionReturnAmount CollectionPerformance = (ClspubCollectionReturnAmount)DeSeriliaze(byteLobs);
           // Session["COLANALDTLS"] = CollectionPerformance.GetCollectionAmount;

            if ((!rdbRptBasis.Items[0].Selected == true) && (!rdbRptBasis.Items[1].Selected == true))
            {
            //if ((!rdbRptBasis.Items[0].Selected == true))
            //{
                if (CollectionPerformance.GetCollectionAmount.Count != 0)
                {
                    btnPrint.Visible = true;
                    pnlCollectionAmount.Visible = true;
                    grvCollectionAmount.DataSource = CollectionPerformance.GetCollectionAmount;
                    grvCollectionAmount.DataBind();
                    TotalAgeing0to30 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing0to30);
                    TotalAgeing31to60 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing31to60);
                    TotalAgeing61to90 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing61to90);
                    TotalAgeingAbove90 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.AgeingAbove90);
                    TotalBALDUE = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.BALDUE);
                    FunPriDisplayTotalDetails();
                }
                else
                {
                  btnPrint.Visible = false;
                  pnlCollectionAmount.Visible = true;
                  lblCollAmtMsg.Visible = true;
                  lblCollAmtMsg.Text = "No Records Found";

                }
            }
          
            #endregion


            if (rdbRptBasis.SelectedValue == "Net of ChequeReturn")
             {

                 if (CollectionPerformance.GetChequeReturnAmount.Count != 0)
                 {
                     btnChkPrint.Visible = true; 
                     pnlChequeReturn.Visible = true;
                     grvChequeReturn.DataSource = CollectionPerformance.GetChequeReturnAmount;
                     grvChequeReturn.DataBind();
                     GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                     //Creating a table cell object
                     TableCell objtablecell = new TableCell();

                     AddMergedCells(objgridviewrow, objtablecell, 9, string.Empty);
                     AddMergedCells(objgridviewrow, objtablecell, 8, "Ageing Days");
                     grvChequeReturn.Controls[0].Controls.AddAt(0, objgridviewrow);

                     TotalAgeing0to30 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing0to30);
                     TotalAgeing31to60 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing31to60);
                     TotalAgeing61to90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing61to90);
                     TotalAgeingAbove90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.AgeingAbove90);
                     ChkReturn0to30 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing0to30);
                     ChkReturn31to60 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing31to60);
                     ChkReturn61to90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing61to90);
                     ChkReturnAbove90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_AgeingAbove90);
                     FunPriDisplayCheckReturnTotal();

                 }
                 else
                 {
                     btnChkPrint.Visible = false;
                     pnlChequeReturn.Visible = true;
                     lblChqRtnMsg.Visible = true;
                     lblChqRtnMsg.Text = "No Records Found";

                 }
                   
             }


            if (rdbRptBasis.SelectedValue == "Comparative Analysis")
            {
                if (CollectionPerformance.GetCollectionAnalysis.Count != 0)
                {
                    btnAnlyPrint.Visible = true;
                    pnlComparative.Visible = true;
                    GrvComparativeAnalysis.DataSource = CollectionPerformance.GetCollectionAnalysis;
                    GrvComparativeAnalysis.DataBind();
                    TotalPeriod10to30 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt030);
                    TotalPeriod130to61 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt3160);
                    TotalPeriod160to91 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt6190);
                    TotalPeriod1Above90 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmtAbv90);
                    TotalPeriod1 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstPrdDue);
                    TotalPeriod20to30 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt030);
                    TotalPeriod230to61 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt3160);
                    TotalPeriod260to91 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt6190);
                    TotalPeriod2Above90 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmtAbv90);
                    TotalPeriod2 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndPrdDue);
                    FunPriDisplayAnalysisTotal();
                    GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                    //Creating a table cell object
                    TableCell objtablecell = new TableCell();
                    AddMergedCells(objgridviewrow, objtablecell, 8, string.Empty);
                    AddMergedCells(objgridviewrow, objtablecell, 10, "Ageing Days");
                    GrvComparativeAnalysis.Controls[0].Controls.AddAt(0, objgridviewrow);
                }
                else
                {
                   btnAnlyPrint.Visible = false;
                   pnlComparative.Visible = true;
                   lblAnlyAmtMsg.Visible = true;
                   lblAnlyAmtMsg.Text = "No Records Found";
                }
             
            }

            #region Session

            Session["CollPerformance"] = CollectionPerformance;

            //code modified to pass Date values to report - kuppu - Nov-29-2012 
            //string FromDate = Utility.StringToDate("01/" + strFromDate).ToString(ObjS3GSession.ProDateFormatRW);
            //string ToDate = Utility.StringToDate("30/" + strToDate).ToString(ObjS3GSession.ProDateFormatRW);
            //starts
            int FromYear = int.Parse(txtNormalFromDate.Text.Substring(3, 4));
            int FromMonth = int.Parse(txtNormalFromDate.Text.Substring(0, 2));
            int FromNumberOfDays = DateTime.DaysInMonth(FromYear, FromMonth);
            DateTime FromFirstDay = new DateTime(FromYear, FromMonth, 1);
            string FromDate =  FromFirstDay.ToString(ObjS3GSession.ProDateFormatRW);
            
            int ToYear = int.Parse(txtNormalToDate.Text.Substring(3, 4));
            int ToMonth = int.Parse(txtNormalToDate.Text.Substring(0, 2));
            int ToNumberOfDays = DateTime.DaysInMonth(ToYear, ToMonth);
            DateTime ToLastDay = new DateTime(ToYear, ToMonth, ToNumberOfDays);
            string ToDate = ToLastDay.ToString(ObjS3GSession.ProDateFormatRW);

            Session["FromDate"] = FromDate.ToString(); ;
            Session["ToDate"] = ToDate.ToString();
            //ends
           
            #endregion
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
        finally
        {
            if (ObjOrgColClient != null)
            {
                ObjOrgColClient.Close();
            }
        }
    }

    #endregion
    //private void FunBindTotal(DataTable dsCollectAmount,int Option)
    //{
    //     if (dsCollectAmount.Compute("SUM(Ageing0to30)", "PANum IS NOT NULL").ToString() == "")
    //    {
    //        TotalAgeing0to30 = 0;
    //    }
    //    else
    //    {
    //        TotalAgeing0to30 = (decimal)dsCollectAmount.Compute("SUM(Ageing0to30)", "PANum IS NOT NULL");
    //    }

    //    if (dsCollectAmount.Compute("SUM(Ageing31to60)", "PANum IS NOT NULL").ToString() == "")
    //    {
    //        TotalAgeing31to60 = 0;
    //    }
    //    else
    //    {
    //    TotalAgeing31to60 = (decimal)dsCollectAmount.Compute("SUM(Ageing31to60)", "PANum IS NOT NULL");
    //    }
    //    if (dsCollectAmount.Compute("SUM(Ageing61to90)", "PANum IS NOT NULL").ToString() == "")
    //    {
    //        TotalAgeing61to90 = 0;
    //    }
    //    else
    //    {
    //    TotalAgeing61to90 = (decimal)dsCollectAmount.Compute("SUM(Ageing61to90)", "PANum IS NOT NULL");
    //    }
    //    if (string.IsNullOrEmpty(dsCollectAmount.Compute("SUM(AgeingAbove90)", "PANum IS NOT NULL").ToString()))
    //    {
    //        TotalAgeingAbove90 = 0;

    //    }
    //    else
    //    {
    //    TotalAgeingAbove90 = (decimal)dsCollectAmount.Compute("SUM(AgeingAbove90)", "PANum IS NOT NULL");
    //    }
    //    if (Option == 0)
    //    {
    //        FunPriDisplayTotalDetails();
    //    }
    //    if(Option==1)
    //    {
    //        if (string.IsNullOrEmpty(dsCollectAmount.Compute("SUM(CHEQUE_RETURN_AMOUNT)", "PANum IS NOT NULL").ToString()))
    //        {
    //            TotalChkReturnAmount = 0;

    //        }
    //        else
    //        {
    //            TotalChkReturnAmount = (decimal)dsCollectAmount.Compute("SUM(CHEQUE_RETURN_AMOUNT)", "PANum IS NOT NULL");
    //        }

    //        FunPriDisplayCheckReturnTotal();
    //    }
    //}
    #region To Display Grand Total Analysis Amount
    private void FunPriDisplayAnalysisTotal()
    {
        try
        {
            if (GrvComparativeAnalysis.Rows.Count > 0)
            {

                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtFirstDue")).Text = TotalPeriod1.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtSecondDue")).Text = TotalPeriod2.ToString();


                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt030FstPrdClnTotal")).Text = TotalPeriod10to30.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt030SndPrdClnTotal")).Text = TotalPeriod20to30.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt3160FstPrdClnTotal")).Text = TotalPeriod130to61.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt3160SndPrdClnTotal")).Text = TotalPeriod230to61.ToString();

                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt6190FstPrdClnTotal")).Text = TotalPeriod160to91.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt6190SndPrdClnTotal")).Text = TotalPeriod260to91.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtAbv90FstPrdClnTotal")).Text = TotalPeriod1Above90.ToString();
                ((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtAbv90SndPrdClnTotal")).Text = TotalPeriod2Above90.ToString();





                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtFirstDue")).SetDecimalPrefixSuffix(8, 4, false, "txtFirstDue");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtSecondDue")).SetDecimalPrefixSuffix(8, 4, false, "txtSecondDue");


                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt030FstPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt030FstPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt030SndPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt030SndPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt3160FstPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt3160FstPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt3160SndPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt3160SndPrdClnTotal");

                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt6190FstPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt6190FstPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txt6190SndPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txt6190SndPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtAbv90FstPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txtAbv90FstPrdClnTotal");
                //((TextBox)GrvComparativeAnalysis.FooterRow.FindControl("txtAbv90SndPrdClnTotal")).SetDecimalPrefixSuffix(8, 4, false, "txtAbv90SndPrdClnTotal");

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    #endregion
    #region To Display Grand Total Cheque Return Amount
    private void FunPriDisplayCheckReturnTotal()
    {
        try
        {
            if (grvChequeReturn.Rows.Count > 0)
            {
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal0to30")).Text = TotalAgeing0to30.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal31to60")).Text = TotalAgeing31to60.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal61to90")).Text = TotalAgeing61to90.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotalAbove90")).Text = TotalAgeingAbove90.ToString();

                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal0to30")).Text = ChkReturn0to30.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal31to60")).Text = ChkReturn31to60.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal61to90")).Text = ChkReturn61to90.ToString();
                ((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotalAbove90")).Text = ChkReturnAbove90.ToString();



                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal0to30")).SetDecimalPrefixSuffix(8, 4, false, "txtCMTotal0to30");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal31to60")).SetDecimalPrefixSuffix(8, 4, false, "txtCMTotal31to60");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotal61to90")).SetDecimalPrefixSuffix(8, 4, false, "txtCMTotal61to90");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCMTotalAbove90")).SetDecimalPrefixSuffix(8, 4, false, "txtCMTotalAbove90");

                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal0to30")).SetDecimalPrefixSuffix(8, 4, false, "txtCRTotal0to30");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal31to60")).SetDecimalPrefixSuffix(8, 4, false, "txtCRTotal31to60");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotal61to90")).SetDecimalPrefixSuffix(8, 4, false, "txtCRTotal61to90");
                //((TextBox)grvChequeReturn.FooterRow.FindControl("txtCRTotalAbove90")).SetDecimalPrefixSuffix(8, 4, false, "txtCRTotalAbove90");

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    #endregion
    #region To Display Total Amount Collection Amount
    private void FunPriDisplayTotalDetails()
    {
        try
        {
            if (grvCollectionAmount.Rows.Count > 0)
            {
                ((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal0to30")).Text = TotalAgeing0to30.ToString();
                //((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal0to30")).SetDecimalPrefixSuffix(8, 4, false, "Ageing0to30");
                ((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal31to60")).Text = TotalAgeing31to60.ToString();
                //((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal31to60")).SetDecimalPrefixSuffix(8, 4, false, "Ageing31to60");
                ((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal61to90")).Text = TotalAgeing61to90.ToString();
                //((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotal61to90")).SetDecimalPrefixSuffix(8, 4, false, "Ageing31to60");
                ((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotalAbove90")).Text = TotalAgeingAbove90.ToString();
                //((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotalAbove90")).SetDecimalPrefixSuffix(8, 4, false, "AgeingAbove90");
                ((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotalBalDue")).Text = TotalBALDUE.ToString();
                //((TextBox)grvCollectionAmount.FooterRow.FindControl("txtTotalBalDue")).SetDecimalPrefixSuffix(8, 4, false, "BALDUE");
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    #endregion
    #region Gridview Events
    protected void grvChequeReturn_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GrvComparativeAnalysis_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            //Creating a gridview object            
            GridView objGridView = (GridView)sender;

            //Creating a gridview row object
            GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //Creating a table cell object
            TableCell objtablecell = new TableCell();


            AddMergedCells(objgridviewrow, objtablecell, 8, string.Empty);

            AddMergedCells(objgridviewrow, objtablecell, 2, " 0 - 30 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " 31 - 60 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " 61 - 90 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " Above 90 ");

            objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);
        }

    }

    protected void grvCollectionAmount_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                //Creating a gridview object            
                GridView objGridView = (GridView)sender;

                //Creating a gridview row object
                GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //Creating a table cell object
                TableCell objtablecell = new TableCell();


                AddMergedCells(objgridviewrow, objtablecell, 10, string.Empty);

                AddMergedCells(objgridviewrow, objtablecell, 5, " Ageing Days");

                objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void grvChequeReturn_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            //Creating a gridview object            
            GridView objGridView = (GridView)sender;

            //Creating a gridview row object
            GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //Creating a table cell object
            TableCell objtablecell = new TableCell();


            AddMergedCells(objgridviewrow, objtablecell, 9, string.Empty);

            AddMergedCells(objgridviewrow, objtablecell, 2, " 0 - 30 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " 31 - 60 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " 61 - 90 ");

            AddMergedCells(objgridviewrow, objtablecell, 2, " Above 90 ");

            objGridView.Controls[0].Controls.AddAt(0, objgridviewrow);


        }

    }
    protected void grvCollectionAmount_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        TotalAgeing0to30 = 0;
        TotalAgeing31to60 = 0;
        TotalAgeingAbove90 = 0;
        TotalAgeing61to90 = 0;

        ClspubCollectionReturnAmount CollectionPerformance = new ClspubCollectionReturnAmount();
        CollectionPerformance = (ClspubCollectionReturnAmount)Session["CollPerformance"];
        grvCollectionAmount.PageIndex = e.NewPageIndex;
        //grvCollectionAmount.PageSize = 100;
        grvCollectionAmount.DataSource = CollectionPerformance.GetCollectionAmount;
        grvCollectionAmount.DataBind();
        TotalAgeing0to30 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing0to30);
        TotalAgeing31to60 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing31to60);
        TotalAgeing61to90 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing61to90);
        TotalAgeingAbove90 = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.AgeingAbove90);
        TotalBALDUE = CollectionPerformance.GetCollectionAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.BALDUE);
        FunPriDisplayTotalDetails();
    }
    protected void GrvComparativeAnalysis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        TotalPeriod1 = 0;
        TotalPeriod2 = 0;
        TotalPeriod10to30 = 0;
        TotalPeriod20to30 = 0;
        TotalPeriod130to61 = 0;
        TotalPeriod230to61 = 0;
        TotalPeriod160to91 = 0;
        TotalPeriod260to91 = 0;
        TotalPeriod1Above90 = 0;
        TotalPeriod2Above90 = 0;
        ClspubCollectionReturnAmount CollectionPerformance = new ClspubCollectionReturnAmount();
        CollectionPerformance = (ClspubCollectionReturnAmount)Session["CollPerformance"];
        GrvComparativeAnalysis.PageIndex = e.NewPageIndex;
        //GrvComparativeAnalysis.PageSize = 100;
        GrvComparativeAnalysis.DataSource = CollectionPerformance.GetCollectionAnalysis;
        GrvComparativeAnalysis.DataBind();
        TotalPeriod10to30 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt030);
        TotalPeriod130to61 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt3160);
        TotalPeriod160to91 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmt6190);
        TotalPeriod1Above90 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstprdClnAmtAbv90);
        TotalPeriod1 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.FstPrdDue);
        TotalPeriod20to30 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt030);
        TotalPeriod230to61 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt3160);
        TotalPeriod260to91 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmt6190);
        TotalPeriod2Above90 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndprdClnAmtAbv90);
        TotalPeriod2 = CollectionPerformance.GetCollectionAnalysis.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.SndPrdDue);
        FunPriDisplayAnalysisTotal();
        GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //Creating a table cell object
        TableCell objtablecell = new TableCell();
        AddMergedCells(objgridviewrow, objtablecell, 5, string.Empty);
        AddMergedCells(objgridviewrow, objtablecell, 10, "Ageing Days");
        GrvComparativeAnalysis.Controls[0].Controls.AddAt(0, objgridviewrow);
    }
    protected void grvCollectionAmount_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion
    #region Drop Down Events
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunClear();
            if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            {
                ddlLoc2.Enabled = false;
                Procparam = new Dictionary<string, string>();
                if (Procparam != null)
                    Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_Id", "189");
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Is_Report", "1");
                ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                ddlBranch.Items[0].Text = "--ALL--";
                if (ddlBranch.Items.Count == 2)
                {
                    ddlBranch.Items[1].Selected = true;
                    ddlBranch_SelectedIndexChanged(this, new EventArgs());
                }
                else
                {
                    ddlLoc2.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                    ddlLoc2.Items[0].Text = "--ALL--";
                }

            }
            else
            {
                FunPriLoadBranch();
            }

            if (ddlLOB.SelectedItem.Text != "FT - Factoring" && ddlLOB.SelectedItem.Text != "WC - Working Capital")
            {
                FunPriLoadAssetTypeDetails();
            }
            else
            {
                //ddlAssetType.Items.Clear();
                //ddlAssetType.Items.Add("--Select--");
            }
            FunPriLoadProductDetails();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPubClearGrid();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubClearGrid();
            if (ddlBranch.SelectedIndex == 0)
            {
                Procparam = new Dictionary<string, string>();
                if (Procparam != null)
                    Procparam.Clear();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
                Procparam.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
                Procparam.Add("@Is_Active", "1");
                Procparam.Add("@Program_Id", "189");
                Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
                Procparam.Add("@Is_Report", "1");
                ddlLoc2.BindDataTable(SPNames.BranchMaster_LIST, Procparam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                ddlLoc2.Items[0].Text = "--ALL--";
                ddlLoc2.Enabled = false;

            }
            else
            {
                FunPubLoadChildLocation(Convert.ToInt32(ddlBranch.SelectedValue));
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    public void FunPubLoadChildLocation(int LocationID)
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            if (Procparam != null)
                Procparam.Clear();
            Procparam.Add("@PROGRAM_ID", "189");
            Procparam.Add("@USER_ID", intUserId.ToString());
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            Procparam.Add("@LOCATION_ID", LocationID.ToString());
            DataSet DSChild = new DataSet();
            DSChild = Utility.GetDataset("S3G_RPT_LEVELVALUE", Procparam);

            ddlLoc2.DataSource = DSChild.Tables[0];
            ddlLoc2.DataTextField = "Location";
            ddlLoc2.DataValueField = "Location_ID";
            ddlLoc2.DataBind();
            if ((DSChild.Tables[0].Rows.Count == 1))
            {
                //if ((DSChild.Tables[0].Rows[0]["Location"].ToString() != ddlBranch.SelectedItem.Text))
                //{
                //    ListItem Lst = new ListItem("--ALL--", "ALL");
                //    ddlLoc2.Items.Insert(0, Lst);
                //    ddlLoc2.Enabled = true;
                //}
                //else
                //{
                ddlLoc2.Items[0].Selected = true;
                ddlLoc2.Enabled = false;
                //}
            }
            else if (DSChild.Tables[0].Rows.Count > 1)
            {
                ListItem Lst = new ListItem("--ALL--", "ALL");
                ddlLoc2.Items.Insert(0, Lst);
                ddlLoc2.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriLoadAssetCategoriesDetails();
    }
    #endregion
    #region Button Events
    protected void btnGo_Click(object sender, EventArgs e)
    {
         FunCollectionAmount();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
            string strScipt = "window.open('../Reports/S3GCollectionPerformance.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CollectionPerformance", strScipt, true);
            ((Label)grvCollectionAmount.FooterRow.FindControl("lblGrandTotal")).Text = "Grand Total";
            
    }

    private void FunClear()
    {
        ddlBranch.Items.Clear();
        ddlBranch.DataSource = null;
        ddlBranch.DataBind();
        ddlLoc2.Items.Clear();
        ddlLoc2.DataSource = null;
        ddlLoc2.DataBind();
        ddlProduct.Items.Clear();
        ddlProduct.DataSource = null;
        ddlProduct.DataBind();
        //ddlAssetType.Items.Clear();
        //ddlAssetType.DataSource = null;
        //ddlAssetType.DataBind();

        //ddlAssetClass.Items.Clear();
        //ddlAssetClass.DataSource = null;
        //ddlAssetClass.DataBind();

        //ddlAssetMake.Items.Clear();
        //ddlAssetMake.DataSource = null;
        //ddlAssetMake.DataBind();
        txtNormalFromDate.Text = "";
        txtComparativeFromDate.Text = "";
        txtNormalToDate.Text = "";
        txtComparativeToDate.Text = "";
        lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
        PnlCompare.Visible = txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;  
        rdbRptBasis.ClearSelection();
        pnlCollectionAmount.Visible = false;
        pnlComparative.Visible = false;
        pnlChequeReturn.Visible = false;
    }

    public void FunPubClearGrid()
    {
        txtNormalFromDate.Text = "";
        txtComparativeFromDate.Text = "";
        txtNormalToDate.Text = "";
        txtComparativeToDate.Text = "";
        rdbRptBasis.ClearSelection();
        lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
        PnlCompare.Visible= txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
        pnlCollectionAmount.Visible = false;
        pnlComparative.Visible = false;
        pnlChequeReturn.Visible = false;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //ddlLOB.DataSource = null;
        //ddlLOB.DataBind();
        ddlLOB.SelectedIndex = 0;
        ddlBranch.Items.Clear();
        ddlBranch.DataSource = null;
        ddlBranch.DataBind();
        ddlLoc2.Items.Clear();
        ddlLoc2.DataSource = null;
        ddlLoc2.DataBind();
        ddlLoc2.Enabled = false;
        ddlProduct.Items.Clear();
        ddlProduct.DataSource = null;
        ddlProduct.DataBind();
        FunPriLoadBranch();
        FunPriLoadProductDetails();
        //ddlAssetType.Items.Clear();
        //ddlAssetType.DataSource = null;
        //ddlAssetType.DataBind();

        //ddlAssetClass.Items.Clear();
        //ddlAssetClass.DataSource = null;
        //ddlAssetClass.DataBind();

        //ddlAssetMake.Items.Clear();
        //ddlAssetMake.DataSource = null;
        //ddlAssetMake.DataBind();
        txtNormalFromDate.Text = "";
        txtComparativeFromDate.Text = "";
        txtNormalToDate.Text = "";
        txtComparativeToDate.Text = "";
        rdbRptBasis.ClearSelection();
        if ((!rdbRptBasis.Items[0].Selected == true) && (!rdbRptBasis.Items[1].Selected == true))
        {
            lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
            PnlCompare.Visible = txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
            //txtNormalFromDate.Text = "";
            //txtNormalToDate.Text = "";
        }
        pnlCollectionAmount.Visible = false;
        pnlComparative.Visible = false;
        pnlChequeReturn.Visible = false;

    }
    protected void btnChkPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GChequeReturAmount.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CollectionPerformance", strScipt, true);
        ((Label)grvChequeReturn.FooterRow.FindControl("lblGrandTotal")).Text = "Grand Total";
        FunCollectionAmount();
    }
    protected void btnAnlyPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GCollectionAnalysisReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "CollectionPerformance", strScipt, true);
        ((Label)GrvComparativeAnalysis.FooterRow.FindControl("lblGrandTotal")).Text = "Grand Total";
        FunCollectionAmount();
    }
    #endregion
    //#region Checkbox Events
    //protected void chkNetofChequeReturn_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rdbRptBasis.SelectedValue == "Comparative Analysis")
    //    {
    //        lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
    //        txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = false;
    //        chkComparativeAnalysis.Checked = false;
    //        txtNormalFromDate.Text = "";
    //        txtNormalToDate.Text = "";
    //        txtComparativeFromDate.Text = "";
    //        txtComparativeToDate.Text = "";
    //    }
    //}
    //protected void chkComparativeAnalysis_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rdbRptBasis.SelectedValue == "Comparative Analysis")
    //    {
    //        lblComparativeFromDate.Visible = lblComparativeToDate.Visible = true;
    //        txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = imgComparativeToDate.Visible = true;
    //        chkNetofChequeReturn.Checked = false;
    //        txtNormalFromDate.Text = "";
    //        txtNormalToDate.Text = "";
    //        txtComparativeFromDate.Text = "";
    //        txtComparativeToDate.Text = "";
    //    }
    //}
    //#endregion
    #region Set Suffix
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
    #endregion
  

    #region To Add Cell Dymaically
    private static void AddMergedCells(GridViewRow objgridviewrow, TableCell objtablecell, int colspan, string celltext)
    {
        objtablecell = new TableCell();
        objtablecell.Text = celltext;
        objtablecell.ColumnSpan = colspan;
       
        objgridviewrow.Cells.Add(objtablecell);
    }
    #endregion

    protected void grvChequeReturn_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            ClspubCollectionReturnAmount CollectionPerformance = new ClspubCollectionReturnAmount();
            CollectionPerformance = (ClspubCollectionReturnAmount)Session["CollPerformance"];
            grvChequeReturn.PageIndex = e.NewPageIndex;
            //grvCollectionAmount.PageSize = 100;
            grvChequeReturn.DataSource = CollectionPerformance.GetChequeReturnAmount;
            grvChequeReturn.DataBind();
            GridViewRow objgridviewrow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            //Creating a table cell object
            TableCell objtablecell = new TableCell();

            AddMergedCells(objgridviewrow, objtablecell, 7, string.Empty);
            AddMergedCells(objgridviewrow, objtablecell, 8, "Ageing Days");
            grvChequeReturn.Controls[0].Controls.AddAt(0, objgridviewrow);

            TotalAgeing0to30 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing0to30);
            TotalAgeing31to60 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing31to60);
            TotalAgeing61to90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Ageing61to90);
            TotalAgeingAbove90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.AgeingAbove90);
            ChkReturn0to30 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing0to30);
            ChkReturn31to60 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing31to60);
            ChkReturn61to90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_Ageing61to90);
            ChkReturnAbove90 = CollectionPerformance.GetChequeReturnAmount.Sum(ClsPubCollectionPerformance => ClsPubCollectionPerformance.Chq_AgeingAbove90);
            FunPriDisplayCheckReturnTotal();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }




    protected void rdbRptBasis_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbRptBasis.SelectedValue != "Comparative Analysis")
            {
                lblComparativeFromDate.Visible = lblComparativeToDate.Visible = false;
                PnlCompare.Visible = txtComparativeFromDate.Visible = txtComparativeToDate.Visible = imgComparativeFromDate.Visible = RqvtxtComparativeFromDate.Enabled = RqvtxtComparativeToDate.Enabled = imgComparativeToDate.Visible = false;
                txtNormalFromDate.Text = "";
                txtNormalToDate.Text = "";
                txtComparativeFromDate.Text = "";
                txtComparativeToDate.Text = "";
                pnlCollectionAmount.Visible = false;
                pnlComparative.Visible = false;
                pnlChequeReturn.Visible = false;
            }
            if (rdbRptBasis.SelectedValue == "Comparative Analysis")
            {
                lblComparativeFromDate.Visible = lblComparativeToDate.Visible = true;
                PnlCompare.Visible= txtComparativeFromDate.Visible = txtComparativeToDate.Visible =RqvtxtComparativeFromDate.Enabled=RqvtxtComparativeToDate.Enabled= imgComparativeFromDate.Visible = imgComparativeToDate.Visible = true;
                txtNormalFromDate.Text = "";
                //txtNormalToDate.Text = DateTime.Today.ToString("MM") + "/" + DateTime.Today.Year;
                txtNormalToDate.Text = "";
                txtComparativeFromDate.Text = "";
                txtComparativeToDate.Text = "";
                pnlCollectionAmount.Visible = false;
                pnlComparative.Visible = false;
                pnlChequeReturn.Visible = false;
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "Delegate", "modifydelegatecomp();", true);
           }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void grvCollectionAmount_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
