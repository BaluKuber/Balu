#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Journal Query Report
/// Created By          :   Saranya I
/// Created Date        :   09-Aug-2011
/// Purpose             :   To Get the Journal Details.
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
using System.IO;
using System.IO.Compression;
#endregion

public partial class Reports_S3GRptJournalQuery : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    string PANum;
    string SANum;
    string RegionId;
    bool Is_Active;
    decimal TotalDues;
    int intProgramId = 199;
    decimal TotalReceipts;
    public string strDateFormat;
    public static Reports_S3GRptJournalQuery obj_Page;
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    string strPageName = "Journal Query";
    DataTable dtTable = new DataTable();
    decimal OpeningBalance;
    ReportAccountsMgtServicesClient objSerClient;

    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    #endregion

    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get;
        set;
    }
    public int ProPageSizeRW
    {
        get;
        set;
    }

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load Journal Details Page.";
            CVJournalDetails.IsValid = false;
        }
    }
    #endregion

    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            obj_Page = new Reports_S3GRptJournalQuery();
            obj_Page = this;

            #region Application Standard Date Format
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDate.Attributes.Add("readonly", "readonly");
            //txtEndDate.Attributes.Add("readonly", "readonly");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */

            #endregion

            ProPageNumRW = 1;                                                           // to set the default page number
            TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));
            PageAssignValue obj = new PageAssignValue(this.AssignValue);
            ucCustomPaging.callback = obj;
            ucCustomPaging.ProPageNumRW = ProPageNumRW;
            ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            if (!IsPostBack)
            {
                ddlLOB.Focus();
                FunPriLoadLob();
                FunPriLoadBranch();
                FunPriLoadLocation();
                FunPubLoadDenomination();
            }

            
            //ddlBranch.AddItemToolTip();
            //if (ddlBranch.SelectedIndex > 0)
            //    ddlBranch.ToolTip = ddlBranch.SelectedItem.Text;
            //ddlLocation2.AddItemToolTip();
            //if (ddlLocation2.SelectedIndex > 0)
            //    ddlLocation2.ToolTip = ddlLocation2.SelectedItem.Text;
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        try
        {
            ProPageNumRW = intPageNum;              // To set the page Number
            ProPageSizeRW = intPageSize;            // To set the page size    
            FunPriBindJournalDetails();                       // Binding the Landing grid
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPriLoadLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = LOB;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count > 0)
            {
                if (ddlLOB.Items.Count == 2)
                    ddlLOB.SelectedIndex = 1;
                ddlLOB.Items.RemoveAt(0);
            }
            //else
            //    ddlLOB.SelectedIndex = 0;
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
             if (ddlLOB.Items.Count > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //    ddlBranch.SelectedIndex = 1;
            //else
            //    ddlBranch.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Branch");
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
            if (ddlLOB.Items.Count > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLocation2.DataSource = Branch;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            ddlLocation2.Items[0].Text = "--ALL--";

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load Branch");
        }
        finally
        {
            objSerClient.Close();
        }
    }

    private void FunPriLoadLocation2()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int LobId = 0;
            if (ddlLOB.SelectedIndex > 0)
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlBranch.SelectedIndex != 0)
                Location1 = Convert.ToInt32(ddlBranch.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, LobId, Location1);
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlLocation2.DataSource = Location;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            if (ddlLocation2.Items.Count == 2)
            {
                if (ddlBranch.SelectedIndex != 0)
                {
                    ddlLocation2.SelectedIndex = 1;
                    //Utility.ClearDropDownList(ddlLocation2);
                }
                else
                    ddlLocation2.SelectedIndex = 0;
            }
            else
            {
                ddlLocation2.Items[0].Text = "--ALL--";
                ddlLocation2.SelectedIndex = 0;
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

    private void FunPriLoadAccountNumbers()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            if (ddlLOB.Items.Count > 0)
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlBranch.SelectedValue));
            if (ddlLocation2.SelectedIndex > 0)
            {
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlLocation2.SelectedValue));
            }
            //else
            //{
            //    Procparam.Add("@LOCATION_ID2", "0");
            //}

            //ddlPNum.BindDataTable("S3G_RPT_GetAccountNoBasedLobBranch", Procparam, new string[] { "PANUM", "PANum" });

            //if (ddlPNum.Items.Count == 2)
            //    ddlPNum.SelectedIndex = 1;
            //else
            //    ddlPNum.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriLoadSubAccountNumbers()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            //Procparam.Add("@PANUM", ddlPNum.SelectedItem.Text);
            //ddlSNum.BindDataTable("S3G_RPT_GetSubAccountNoBasedAccount", Procparam, new string[] { "SANUM", "SANUM" });

            //if (ddlSNum.Items.Count == 2)
            //    ddlSNum.SelectedIndex = 1;
            //else
            //    ddlSNum.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriLoadLan()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            if(ddlLOB.SelectedIndex>0)
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlBranch.SelectedValue));
            if (ddlLocation2.SelectedIndex > 0)
            {
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlLocation2.SelectedValue));
            }
            if (txtPANum.Text != "")
            {
                Procparam.Add("@PANUM", txtPANum.Text);
            }
            if (ddlSNum.SelectedIndex > 0)
            {
                Procparam.Add("@SANUM", ddlSNum.SelectedValue);
            }
            ddlLAN.BindDataTable("S3G_RPT_GetLeaseAssetNumber", Procparam, new string[] { "LAN", "LAN" });

            if (ddlLAN.Items.Count == 2)
                ddlLAN.SelectedIndex = 1;
            else
                ddlLAN.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriLoadGLAccount()
    {
        try
        {
            //ddlGLAccount.ClearSelection();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@USER_ID", Convert.ToString(intUserId));
            Procparam.Add("@PROGRAM_ID", Convert.ToString(intProgramId));
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
            if (ddlBranch.SelectedIndex > 0)
                Procparam.Add("@LOCATION_ID1", Convert.ToString(ddlBranch.SelectedValue));
            if (ddlLocation2.SelectedIndex > 0)
            {
                Procparam.Add("@LOCATION_ID2", Convert.ToString(ddlLocation2.SelectedValue));
            }

            //ddlGLAccount.BindDataTable("S3G_RPT_GetGLAccount", Procparam, new string[] { "CODE", "GL_CODE" });

            //ddlGLAccount.AddItemToolTip();
            //if (ddlGLAccount.SelectedIndex > 0)
            //    ddlGLAccount.ToolTip = ddlGLAccount.SelectedItem.Text;

            //if (ddlGLAccount.Items.Count == 2)
            //    ddlGLAccount.SelectedIndex = 1;
            //else
            //    ddlGLAccount.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    public void FunPubLoadDenomination()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {

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

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
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

    private void FunPriValidateControls()
    {
        ddlBranch.ClearSelection();
        txtPANum.Text = hdnPANum.Value = txtGLAccount.Text = hdnGLAccount.Value = "";
        ddlSNum.Items.Clear();
        ddlLAN.Items.Clear();
        ddlDenomination.ClearSelection();
        txtStartDate.Text = "";
        txtEndDate.Text = "";

    }

    private void FunPriValidateGrid()
    {
        pnlJournalDetails.Visible = false;
        grvJournalDetails.DataSource = null;
        grvJournalDetails.DataBind();
        lblAmounts.Visible = false;
        btnPrint.Visible = false;
        BtnExcel.Visible = false;
    }

    private void FunPriClearJournalDetails()
    {
        ddlLOB.Focus();
        //if (ddlLOB.Items.Count == 2)
        //    ddlLOB.SelectedIndex = 1;
        //else
        //    ddlLOB.SelectedIndex = 0;
        ddlLOB.ClearSelection();
        ddlBranch.ClearSelection();
        FunPriLoadLocation();
        ddlLocation2.Enabled = false;
        //if (ddlBranch.Items.Count == 2)
        //    ddlBranch.SelectedIndex = 1;
        //else
        //    ddlBranch.SelectedIndex = 0;
        txtPANum.Text = hdnPANum.Value = txtGLAccount.Text = hdnGLAccount.Value = "";
        ddlSNum.Items.Clear();
        ddlLAN.Items.Clear();
        lblLAN.Enabled = false;
        ddlLAN.Enabled = false;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlDenomination.ClearSelection();
        pnlJournalDetails.Visible = false;
        grvJournalDetails.DataSource = null;
        grvJournalDetails.DataBind();
        btnPrint.Visible = false;
        lblAmounts.Visible = false;
        BtnExcel.Visible = false;
    }

    private void FunPriValidateFromEndDate()
    {
        try
        {

            #region Validate From and To Date
            //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
            if ((!(string.IsNullOrEmpty(txtStartDate.Text))) &&
               (!(string.IsNullOrEmpty(txtEndDate.Text))))                                   // If start and end date is not empty
            {
                // if (Convert.ToDateTime(DateTime.Parse(txtStartDate.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDate.Text, dtformat))) // start date should be less than or equal to the enddate
                if (Utility.StringToDate(txtStartDate.Text) > Utility.StringToDate(txtEndDate.Text)) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                    txtEndDate.Text = "";
                    //added on 11/11/2011
                    FunPriValidateGrid();
                    //end
                    return;
                }
            }
            if ((!(string.IsNullOrEmpty(txtStartDate.Text))) &&
               ((string.IsNullOrEmpty(txtEndDate.Text))))
            {
                txtEndDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            }
            if (((string.IsNullOrEmpty(txtStartDate.Text))) &&
                (!(string.IsNullOrEmpty(txtEndDate.Text))))
            {
                txtStartDate.Text = txtEndDate.Text;
            }
            #endregion

            //int Diff = txtEndDate.Text.CompareTo(txtStartDate.Text);
            //if (Utility.StringToDate(txtEndDate.Text) > Utility.StringToDate(txtStartDate.Text).AddMonths(6))
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Difference between start and end date should not be more than six months");
            //    FunPriValidateGrid();
            //    return;
            //}
            //btnPrint.Enabled = true;

            FunPriBindJournalDetails();
            if (grvJournalDetails.Rows.Count > 0)
            {
                //btnPrint.Visible = true;
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
            else
            {
                //btnPrint.Visible = true;
                lblAmounts.Visible = false;
            }
            #region To Get Header Details for Report
            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            Session["LineofBusiness"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                objHeader.Branch = ddlBranch.SelectedItem.Text;
                //objHeader.Branch = ddlBranch.SelectedItem.Text;
            }
            else
            {
                objHeader.Branch = "All";
                //((Label)grvSummary.FooterRow.FindControl("lblRegion")).Text;
            }
            if (Convert.ToInt32(ddlLocation2.SelectedValue) > 0)
            {
                objHeader.Region = ddlLocation2.SelectedItem.Text;
            }
            else
            {
                objHeader.Region = "All";
            }

            // Code Changed By Chandru K On 16 Aug 2016 For Bug Id : 4808

            //if (ddlPNum.Items.Count > 1)
            //{
            //    if (ddlPNum.SelectedIndex > 0)
            //    {
            //        objHeader.PANum = ddlPNum.SelectedItem.Text;
            //        Session["AccountNo"] = ddlPNum.SelectedItem.Text;
            //    }
            //    else
            //    {
            //        objHeader.PANum = "All";
            //        Session["AccountNo"] = "All";
            //        //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
            //    }
            //}
            //else
            //{
            //    objHeader.PANum = "---";
            //    Session["AccountNo"] = "---";
            //}

            if (txtPANum.Text != "")
            {
                objHeader.PANum = txtPANum.Text;
                Session["AccountNo"] = txtPANum.Text;
            }
            else
            {
                objHeader.PANum = "All";
                Session["AccountNo"] = "All";
            }

            // Code Changes End

            if (ddlSNum.Items.Count > 1)
            {
                if (ddlSNum.SelectedIndex > 0)
                {
                    objHeader.SANum = ddlSNum.SelectedItem.Text;
                    Session["SubAccountNo"] = ddlSNum.SelectedItem.Text;
                }
                else
                {
                    objHeader.SANum = "All";
                    Session["SubAccountNo"] = "All";
                    //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
                }
            }
            else
            {
                objHeader.SANum = "---";
                Session["SubAccountNo"] = "---";
                //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
            }
            if (ddlLAN.Items.Count > 1)
            {
                if (ddlLAN.SelectedIndex > 0)
                {
                    objHeader.Lan = ddlLAN.SelectedItem.Text;
                    Session["Lan"] = ddlLAN.SelectedItem.Text;
                }
                else
                {
                    objHeader.Lan = "All";
                    Session["Lan"] = "All";
                    //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
                }
            }
            else
            {
                objHeader.Lan = "---";
                Session["Lan"] = "---";
            }

            // Code Changed By Chandru K On 16 Aug 2016 For Bug Id : 4808

            //if (ddlGLAccount.Items.Count > 1)
            //{
            //    if (ddlGLAccount.SelectedIndex > 0)
            //    {
            //        objHeader.GlAccount = ddlGLAccount.SelectedItem.Text;
            //        Session["GLAccount"] = ddlGLAccount.SelectedItem.Text;
            //    }
            //    else
            //    {
            //        objHeader.GlAccount = "All";
            //        Session["GLAccount"] = "All";
            //        //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
            //    }
            //}
            //else
            //{
            //    objHeader.GlAccount = "---";
            //    Session["GLAccount"] = "---";
            //}

            if (txtGLAccount.Text != "")
            {
                objHeader.GlAccount = txtGLAccount.Text;
                Session["GLAccount"] = txtGLAccount.Text;
            }
            else
            {
                objHeader.GlAccount = "All";
                Session["GLAccount"] = "All";

            }

            // Code Changes End

            objHeader.StartDate = txtStartDate.Text;
            objHeader.EndDate = txtEndDate.Text;
            Session["Title"] = "Journal Query From " + txtStartDate.Text + " " + "To" + " " + txtEndDate.Text;
            Session["Header"] = objHeader;

            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are In" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

            if (s1 == "OL")
                Session["LOB"] = "OL";

            else
                Session["LOB"] = "ALL";

            #endregion

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }


    protected void grvJournalDetails_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        // string s = e.NewEditIndex.ToString();

    }


    private void FunPriBindJournalDetails()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            lblAmounts.Visible = true;
            //btnPrint.Visible = true;
            lblError.Text = "";
            pnlJournalDetails.Visible = true;
            
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            dictParam.Add("@PROGRAM_ID", intProgramId.ToString());
            dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);

            if (ddlBranch.SelectedIndex != 0)
                dictParam.Add("@Location_Id1", ddlBranch.SelectedValue);
            else
                dictParam.Add("@Location_Id1", "0");
            
            if (ddlLocation2.SelectedIndex != 0)
                dictParam.Add("@Location_Id2", ddlLocation2.SelectedValue);
            else
                dictParam.Add("@Location_Id2", "0");

            if (txtPANum.Text != "")
                dictParam.Add("@Panum", txtPANum.Text);

            if (txtGLAccount.Text != "")
                dictParam.Add("@GL_Code", txtGLAccount.Text);

            dictParam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
            dictParam.Add("@EndDate", Utility.StringToDate(txtEndDate.Text).ToString());
            dictParam.Add("@Denomination", ddlDenomination.SelectedValue);
            dictParam.Add("@Is_Export", "0");

            
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProCompany_ID = intCompanyId;

            grvJournalDetails.BindGridView("S3G_RPT_GetJournalDetails", dictParam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();
            //if(s1=="OL")
            //    grvJournalDetails.Columns[6].Visible = true;
            //else
            //    grvJournalDetails.Columns[6].Visible = false;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            if (bIsNewRow)
            {
                grvJournalDetails.Rows[0].Visible = false;
                grvJournalDetails.FooterRow.Visible = false;
            }

            if (bIsNewRow || grvJournalDetails.Rows.Count == 0)
            {
                BtnExcel.Visible = false;
            }
            else
            {
                BtnExcel.Visible = true;
                FunPriDisplayTotal();
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

    private void FunPriDisplayTotal()
    {
        ((Label)grvJournalDetails.FooterRow.FindControl("lblTotalDues")).Text = TotalDues.ToString(Funsetsuffix());
        ((Label)grvJournalDetails.FooterRow.FindControl("lblTotalReceipts")).Text = TotalReceipts.ToString(Funsetsuffix());

    }
    #endregion

    #region Page Events

    #region Dropdown List
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlLocation2.Enabled = true;
            if (ddlBranch.SelectedIndex > 0)
            {
                FunPriLoadLocation2();
            }
            else
            {
                ddlLocation2.Enabled = false;
                FunPriLoadLocation();
            }

            txtPANum.Text = hdnPANum.Value = "";
            ddlSNum.Items.Clear();
            ddlLAN.Items.Clear();
            txtGLAccount.Text = hdnGLAccount.Value = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlDenomination.ClearSelection();
            lblLAN.Enabled = false;
            ddlLAN.Enabled = false;
            string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

            if (s1 == "OL")
            {
                lblLAN.Enabled = true;
                ddlLAN.Enabled = true;
                FunPriLoadLan();
            }
            FunPriLoadAccountNumbers();
            //if (ddlBranch.SelectedIndex > 0)
            //{
                FunPriLoadGLAccount();
            //}
            FunPriValidateGrid();

            
            

        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load  Account Number.";
            CVJournalDetails.IsValid = false;
        }

    }
    protected void ddlLocation2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadAccountNumbers();
            FunPriLoadGLAccount();
            string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

            if (s1 == "OL")
            {
                
                FunPriLoadLan();
            }
            FunPriValidateGrid();
            txtGLAccount.Text = hdnGLAccount.Value = txtPANum.Text = hdnPANum.Value = "";
            ddlSNum.Items.Clear();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlDenomination.ClearSelection();
        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load  Account Number.";
            CVJournalDetails.IsValid = false;
        }

    }
    protected void ddlPNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadSubAccountNumbers();
            FunPriLoadLan();
            FunPriValidateGrid();
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            //ddlDenomination.ClearSelection();

        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load  Sub Account Number.";
            CVJournalDetails.IsValid = false;
        }

    }
    protected void ddlSNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLan();
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Clear Grid.";
            CVJournalDetails.IsValid = false;
        }
    }
    protected void ddlLAN_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Clear Grid.";
            CVJournalDetails.IsValid = false;
        }
    }
    protected void ddlGLAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();
        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Clear Grid.";
            CVJournalDetails.IsValid = false;
        }
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            FunPriLoadLocation();
            ddlLocation2.Enabled = false;
            FunPriLoadBranch();
            FunPriValidateControls();
            FunPriValidateGrid();
            FunPriLoadAccountNumbers();
            if (ddlLOB.SelectedIndex > 0)
            {
                FunPriLoadGLAccount();
            }
            lblLAN.Enabled = false;
            ddlLAN.Enabled = false;
            string s1 = ddlLOB.SelectedItem.Text.Substring(0, ddlLOB.SelectedItem.Text.IndexOf('-')).Trim();

            if (s1 == "OL")
            {
                lblLAN.Enabled = true;
                ddlLAN.Enabled = true;
                FunPriLoadLan();
            }
            
        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load  Sub Account Number.";
            CVJournalDetails.IsValid = false;
        }
    }
    #endregion

    #region Button (Customer / Ok / Clear / Print)

    /// <summary>
    /// To validate the From and To Date
    /// To Bind the Journal Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateFromEndDate();
        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Due to Data Problem, Unable to Load Journal Details Grid.";
            CVJournalDetails.IsValid = false;
        }


    }

    /// <summary>
    /// To Clear The Values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearJournalDetails();

        }
        catch (Exception ex)
        {
            CVJournalDetails.ErrorMessage = "Unable to Clear.";
            CVJournalDetails.IsValid = false;
        }

    }

    /// <summary>
    /// export to crystal format
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3GRptJournalQueryReport.aspx', 'Journal','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Journal", strScipt, true);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_Id", intCompanyId.ToString());
        dictParam.Add("@User_Id", intUserId.ToString());
        dictParam.Add("@PROGRAM_ID", intProgramId.ToString());
        dictParam.Add("@LOB_ID", ddlLOB.SelectedValue);

        if (ddlBranch.SelectedIndex != 0)
            dictParam.Add("@Location_Id1", ddlBranch.SelectedValue);
        else
            dictParam.Add("@Location_Id1", "0");

        if (ddlLocation2.SelectedIndex != 0)
            dictParam.Add("@Location_Id2", ddlLocation2.SelectedValue);
        else
            dictParam.Add("@Location_Id2", "0");

        if (txtPANum.Text != "")
            dictParam.Add("@Panum", txtPANum.Text);
        if (txtGLAccount.Text != "")
            dictParam.Add("@GL_Code", txtGLAccount.Text);

        dictParam.Add("@StartDate", Utility.StringToDate(txtStartDate.Text).ToString());
        dictParam.Add("@EndDate", Utility.StringToDate(txtEndDate.Text).ToString());
        dictParam.Add("@Denomination", ddlDenomination.SelectedValue);

        DataTable dtJournal = new DataTable();
        dtJournal = Utility.GetDefaultData("S3G_RPT_GetJournalDetails", dictParam);
        

        try
        {
            string strDr = dtJournal.Compute("Sum(Dues1)", "").ToString();
            string strCr = dtJournal.Compute("Sum(RECEIPTS1)", "").ToString();

            GridView Grv = new GridView();

            dtJournal.Columns.RemoveAt(10);
            dtJournal.Columns.RemoveAt(10);
            dtJournal.AcceptChanges();

            Grv.DataSource = dtJournal;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {  
                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Journal Query From " + txtStartDate.Text + " To " + txtEndDate.Text;
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Location : " + ddlBranch.SelectedItem.Text;
                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();
                row["Column1"] = "Journal Query Details:";
                dtHeader.Rows.Add(row);

                grv1.DataSource = dtHeader;
                grv1.DataBind();

                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 10;
                grv1.Rows[1].Cells[0].ColumnSpan = 10;
                grv1.Rows[2].Cells[0].ColumnSpan = 4;
                grv1.Rows[3].Cells[0].ColumnSpan = 4;
                grv1.Rows[4].Cells[0].ColumnSpan = 4;

                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[2].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[3].HorizontalAlign = HorizontalAlign.Left;
                grv1.Rows[4].HorizontalAlign = HorizontalAlign.Left;

                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;

                GridView grv2 = new GridView();
                DataTable dtFooter = new DataTable();
                dtFooter.Columns.Add("Column1");
                dtFooter.Columns.Add("Column2");
                dtFooter.Columns.Add("Column3");

                DataRow row1 = dtFooter.NewRow();
                row1 = dtFooter.NewRow();
                row1["Column1"] = "Total";
                row1["Column2"] = strDr;
                row1["Column3"] = strCr;
                dtFooter.Rows.Add(row1);

                grv2.DataSource = dtFooter;
                grv2.DataBind();
                
                //Included cashflow flag column in excel export by vinodha.m
                grv2.Rows[0].Cells[0].ColumnSpan = 8;
                //Included cashflow flag column in excel export by vinodha.m
                grv2.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Right;

                grv2.HeaderRow.Visible = false;
                grv2.Font.Bold = true;
                grv2.ForeColor = System.Drawing.Color.DarkBlue;
                grv2.Font.Name = "calibri";
                grv2.Font.Size = 10;

                string path = Server.MapPath("~/JournalQueryReport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "JournalQueryReport.xls"))
                {
                    File.Delete(path + "JournalQueryReport.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "JournalQueryReport.xls");
                        grv1.RenderControl(hw);
                        grv2.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/JournalDetails/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "JournalQueryReport.zip";

                //before creation of compressed folder,deleting it if exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                //checking the path is available or not
                if (!File.Exists(filePath))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(path, filePath);

                    //Delete The excel file which is created
                    if (File.Exists(path + "JournalQueryReport.xls"))
                    {
                        File.Delete(path + "JournalQueryReport.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "JournalQueryReport.zip");                    
                    Response.ContentType = "application/x-zip-compressed";
                    Response.WriteFile(filePath);                    
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion

    #endregion

    protected void gv_Narration(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            HiddenField HidNarration = e.Row.FindControl("HidNarration") as HiddenField;
            e.Row.ToolTip = HidNarration.Value;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.FindControl("lblDues")).Text != "")
                TotalDues = TotalDues + Convert.ToDecimal(((Label)e.Row.FindControl("lblDues")).Text);
            if (((Label)e.Row.FindControl("lblReceipts")).Text != "")
                TotalReceipts = TotalReceipts + Convert.ToDecimal(((Label)e.Row.FindControl("lblReceipts")).Text);
        }

    }

    // Code Added By Chandru K On 16 Aug 2016 For Bug Id : 4808

    protected void txtPANum_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnPANum.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtPANum.Text = string.Empty;
                hdnPANum.Value = string.Empty;
            }
            FunPriLoadSubAccountNumbers();
            FunPriLoadLan();
            FunPriValidateGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    protected void txtGLAccount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strhdnValue = hdnGLAccount.Value;
            if (strhdnValue == "-1" || strhdnValue == "")
            {
                txtGLAccount.Text = string.Empty;
                hdnGLAccount.Value = string.Empty;
            }
            FunPriValidateGrid();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Translander");
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetRentalScheduleNo(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@PrefixText", prefixText);
            Procparam.Add("@USER_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@PROGRAM_ID", obj_Page.intProgramId.ToString());
            Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue.ToString());
            Procparam.Add("@Location_ID1", obj_Page.ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Location_ID2", obj_Page.ddlLocation2.SelectedValue.ToString());
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_GET_RENTALSCHEDULE_NO]", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetGLAccount(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyId.ToString());
            Procparam.Add("@USER_ID", obj_Page.intUserId.ToString());
            Procparam.Add("@PROGRAM_ID", obj_Page.intProgramId.ToString());
            Procparam.Add("@LOB_ID", obj_Page.ddlLOB.SelectedValue.ToString());
            Procparam.Add("@Location_ID1", obj_Page.ddlBranch.SelectedValue.ToString());
            Procparam.Add("@Location_ID2", obj_Page.ddlLocation2.SelectedValue.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("[S3G_RPT_GetGLAccount]", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    // Code End

}

