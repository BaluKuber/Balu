#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Customer at a Glance Report
/// Created By          :   Ganapathy Subramanian.G
/// Created Date        :   01/04/2011
/// Purpose             :   To show the entire details of a customer
/// Last Updated By		:   Ganapathy Subramanian.G
/// Last Updated Date   :   20/10/2011
/// Reason              :   For Bug Fixing
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
using S3GBusEntity.Reports;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Resources;
using System.Xml.Serialization;
using ReportAccountsMgtServicesReference;
using ReportOrgColMgtServicesReference;

public partial class Reports_S3GRptCustomerGlance : ApplyThemeForProject
{
    #region variable declaration
    ReportAccountsMgtServicesClient objclient;
    ReportOrgColMgtServicesClient objorgClient;
    UserInfo userinfo = new UserInfo();
    Dictionary<string, string> Procparam;
    public int CompanyID, UserID, ProgramID = 121;
    public string LOBID, RegionID, BranchID, ProductID;
    public string strCurerncy, strDateFormat;
    public bool IsChk;
    decimal Totalyettobebilled;
    decimal Totalbilled;
    decimal totalbilledbalance;
    decimal TotalAmountFinanced;
    decimal totalDues;
    decimal totalReceipts;
    decimal totalBalance;
    S3GSession objsession = new S3GSession();
    ClsPubSOAAsset assets;
    Label strPANum;
    Label strSANum;
    string today;
    #endregion
    #region events
    #region Pageload event
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");
            //txtEndDateSearch.Attributes.Add("readonly", "readonly");
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtName.Attributes.Add("ReadOnly", "ReadOnly");
            Session["CompanyName"] = userinfo.ProCompanyNameRW;
            strDateFormat = objsession.ProDateFormatRW;
            CompanyID = userinfo.ProCompanyIdRW;
            UserID = userinfo.ProUserIdRW;
            strCurerncy = objsession.ProCurrencyNameRW;
            Session["GPSDecimalFormat"] = objsession.ProGpsSuffixRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            CalendarExtenderEndDateSearch.Format = objsession.ProDateFormatRW;
            CalendarExtenderStartDateSearch.Format = objsession.ProDateFormatRW;
            txtName.ToolTip = "Customer Name";
            if (!IsPostBack)
            {
                today = DateTime.Now.Date.ToString(strDateFormat);
                txtStartDateSearch.Text = today;  //.Substring(0, 11);
                txtEndDateSearch.Text = today;   //.Substring(0, 11);
                FunPriEnableDisableGrid(false);               
                FunPubLoadLOB();
                FunPubLoadRegion();
                FunPubLoadBranch();
                FunPubLoadProduct();
                ViewState["Flag"] = "CB";
                //ComboBoxBranch.Enabled = false;
            }            
            FunPriloadCustomercodes();
            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }

        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }
    #endregion
    #region Button Events
    #region GoButton Event
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to Start Date");
                today = DateTime.Now.Date.ToString(strDateFormat);
                //txtStartDateSearch.Text = today.Substring(0, 11);
                txtEndDateSearch.Text = today.Substring(0, 11);
                return;
            }
            if (((HiddenField)ucCustomerCodeLov.FindControl("hdnID")).Value == string.Empty)
            {
                Utility.FunShowAlertMsg(this, "Select a Customer");
                return;
            }
            List<ClsPubPASA> PASAs = new List<ClsPubPASA>();
            foreach (GridViewRow item in grvprimeaccount.Rows)
            {
                if (((CheckBox)item.FindControl("chkSelectAccount")).Checked)
                {
                    IsChk = true;
                    ClsPubPASA pasa = new ClsPubPASA();
                    pasa.PrimeAccountNo = ((Label)item.FindControl("lblMLA")).Text;
                    pasa.SubAccountNo = ((Label)item.FindControl("lblSLA")).Text;
                    PASAs.Add(pasa);
                }
            }
            if (!IsChk)
            {
                Utility.FunShowAlertMsg(this, "Please select atleast one account");
                PnlCustomerDetails.Visible = false;
                pnlTransactionDetails.Visible = false;
                BtnPrint.Visible = false;
            }
            else
            {
                Session["Currency"] = LblCurrency.Text = "[All Amounts are in " + strCurerncy+"]";
                LOBID = (ComboBoxLOBSearch.SelectedValue);
                //if ((ComboBoxLOBSearch.SelectedValue) != "-1")
                //{
                //    Session["LOBName"] = (ComboBoxLOBSearch.SelectedItem.Text.Split('-')[1].ToString());
                //}
                //else
                //{
                //    Session["LOBName"] = "ALL";
                //}
                //if (((ComboBoxBranch.SelectedValue) != "-1") && ((ComboBoxBranch.SelectedValue) != ""))
                //{
                //    Session["BranchName"] = (ComboBoxBranch.SelectedItem.Text.Split('-')[1].ToString());
                //}
                //else
                //{
                //    Session["BranchName"] = "ALL";
                //}
                //if (((ComboBoxRegion.SelectedValue) != "-1") && ((ComboBoxRegion.SelectedValue) != ""))
                //{
                //    Session["RegionName"] = (ComboBoxRegion.SelectedItem.Text.Split('-')[1].ToString());
                //}
                //else
                //{
                //    Session["RegionName"] = "ALL";
                //}
                //if (((ComboBoxProductCode.SelectedValue) != "-1") && ((ComboBoxProductCode.SelectedValue) != ""))
                //{
                //    Session["ProductName"] = (ComboBoxProductCode.SelectedItem.Text.Split('-')[1].ToString());
                //}
                //else
                //{
                //    Session["ProductName"] = "ALL";
                //}

                RegionID = (ComboBoxRegion.SelectedValue);
                BranchID = (ComboBoxBranch.SelectedValue);
                ProductID = (ComboBoxProductCode.SelectedValue);
                string StartDate = (txtStartDateSearch.Text);
                Session["StartDate"] = StartDate;
                string EndDate = (txtEndDateSearch.Text);
                //Session["EndDate"] = EndDate;
                Session["Title"] = "Customer at a Glance Report for the period from " + (txtStartDateSearch.Text) + " to " + (txtEndDateSearch.Text);
                HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
                FunPriEnableDisableGrid(true);
                //FunPubGetCustomerDetails(hdnCustomerId.Value);
                ClsPubCustomerGlanceHeaderDetails header = new ClsPubCustomerGlanceHeaderDetails();
                header.CompanyId = Convert.ToString(CompanyID);
                header.UserId = Convert.ToString(UserID);
                header.CustomerId = hdnCustomerId.Value;
                header.LOBId = LOBID;
                header.RegionId = RegionID;
                header.BranchId = BranchID;
                header.ProductId = ProductID;
                header.ProgramId = ProgramID;
                header.StartDate = Utility.StringToDate(StartDate);
                header.EndDate = Utility.StringToDate(EndDate);
                byte[] customerglancedetails = ClsPubSerialize.Serialize(header, SerializationMode.Binary);
                byte[] bytePASAs = ClsPubSerialize.Serialize(PASAs, SerializationMode.Binary);
                objclient = new ReportAccountsMgtServicesClient();
                byte[] objCustomerGrid = objclient.FunPubGetCustomerAtAGlanceDetails(customerglancedetails, bytePASAs);
                List<ClsPubCustomerGlanceDetails> CUSTOMER = (List<ClsPubCustomerGlanceDetails>)DeSerialize(objCustomerGrid);
                grvCustomer.DataSource = CUSTOMER;
                grvCustomer.DataBind();
                objclient.Close();
                for (int index = 0; index < grvCustomer.Rows.Count; index++)
                {
                    grvCustomer.Rows[index].Cells[0].ToolTip = "Line of Business";
                    grvCustomer.Rows[index].Cells[1].ToolTip = "Location";
                    grvCustomer.Rows[index].Cells[2].ToolTip = "Product";
                    grvCustomer.Rows[index].Cells[3].ToolTip = "Status";
                    grvCustomer.Rows[index].Cells[4].ToolTip = "Account No.";
                    grvCustomer.Rows[index].Cells[5].ToolTip = "Sub Account No.";
                    grvCustomer.Rows[index].Cells[6].Text = CUSTOMER[index].AppliedAmt.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[6].ToolTip = "Applied Amount";
                    grvCustomer.Rows[index].Cells[8].Text = CUSTOMER[index].SancAmt.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[8].ToolTip = "Sanctioned Amount";
                    grvCustomer.Rows[index].Cells[9].Text = CUSTOMER[index].DisbursedAmount.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[9].ToolTip = "Disbursed Amount";
                    grvCustomer.Rows[index].Cells[10].Text = CUSTOMER[index].GrossExposure.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[10].ToolTip = "Gross Exposure";
                    grvCustomer.Rows[index].Cells[11].Text = CUSTOMER[index].NetExposure.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[11].ToolTip = "Net Exposure";
                    grvCustomer.Rows[index].Cells[12].Text = CUSTOMER[index].Dues.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[12].ToolTip = "Dues";
                    grvCustomer.Rows[index].Cells[13].Text = CUSTOMER[index].Collected.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[13].ToolTip = "Collected";
                    grvCustomer.Rows[index].Cells[15].Text = CUSTOMER[index].ODIDue.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[15].ToolTip = "ODI Due";
                    grvCustomer.Rows[index].Cells[16].Text = CUSTOMER[index].MemoDue.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[16].ToolTip = "Memo Due";
                    grvCustomer.Rows[index].Cells[17].Text = CUSTOMER[index].Others.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[17].ToolTip = "Others";
                    grvCustomer.Rows[index].Cells[14].Text = CUSTOMER[index].AverageDueDates.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[14].ToolTip = "Average Due Dates";
                    grvCustomer.Rows[index].Cells[7].Text = CUSTOMER[index].CollateralValue.ToString(Funsetsuffix());
                    grvCustomer.Rows[index].Cells[7].ToolTip = "Collateral Value";
                    //grvCustomer.Rows[index].Cells[14].Text = CUSTOMER[index].Pending.ToString(Funsetsuffix());
                }               
                Session["CustomerGlanceDetails"] = CUSTOMER;
                if (grvCustomer.Rows.Count == 0)
                {
                    grvCustomer.EmptyDataText = "No Records Found";
                    grvCustomer.DataBind();
                }
                FunPubLoadAssetDetails();
                //FunPubLoadAccountTransactionDetails();
                if ((grvCustomer.Rows.Count == 0) && (grvtransaction.Rows.Count == 0))
                {
                    BtnPrint.Enabled = false;
                }
                else
                {
                    BtnPrint.Enabled = true;
                }
            }
        }
        catch (Exception btn)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(btn);
            throw btn;
        }
    }

    #endregion
    #region Clear Button Event
    /// <summary>
    /// Clears the contents in the page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            grvCustomer.DataSource = null;
            grvCustomer.DataBind();
            ViewState["Flag"] = null;
            grvprimeaccount.DataSource = null;
            grvprimeaccount.DataBind();
            grvtransaction.DataSource = null;
            grvtransaction.DataBind();
            txtEndDateSearch.Text = string.Empty;
            txtStartDateSearch.Text = string.Empty;
            FunPubLoadLOB();
            FunPubLoadRegion();
            FunPriGetBranch();
            FunPubLoadProduct();
            divCustomerGlance.Style["width"] = "100%";
            txtCustomerCode.Text = string.Empty;
            if (btnGo.Enabled == false)
            {
                btnGo.Enabled = true;
            }
            hdnCustID.Value = "";
            LblCurrency.Text = string.Empty;
            ucCustDetails.ClearCustomerDetails();
            FunPriEnableDisableGrid(false);
            txtStartDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            txtEndDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            divPASA.Style.Add("display", "none");           
            ucCustomerCodeLov.FunPubClearControlValue();
            ucCustomerCodeLov.ReRegisterSearchControl("CMD");
            FunPriDisableSession();
        }
        catch (Exception cl)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(cl);
            throw cl;
        }
    }

    #endregion
    #region CustomerControl Button Event
    /// <summary>
    /// Gets the customer details 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Flag"] == null)
            {
                ViewState["Flag"] = "C";
            }
            if (pnlAccountDetails.Visible == true && pnlasset.Visible == true && PnlCustomerDetails.Visible == true && pnlTransactionDetails.Visible == true)
            {
                ValidateGridData();
            }
            FunPriDisableSession();
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                hdnCustID.Value = hdnCustomerId.Value;
                FunPubGetCustomerDetails(hdnCustID.Value);               
                FunPriLoadLob(CompanyID, UserID, true, hdnCustID.Value);
                FunPriLoadRegion();                
            }
           // pnlPrimeandsubAcountDetails.Visible = true;
             divPASA.Style.Add("display", "block");
            btnClear.Visible = true;
            btnGo.Visible = true;
            string LOBID=string.Empty;
            txtStartDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            txtEndDateSearch.Text = DateTime.Now.Date.ToString(strDateFormat);
            string LocationID1=string.Empty;
            string LocationID2=string.Empty;
            if(ComboBoxLOBSearch.SelectedIndex>0)
            {
                LOBID=ComboBoxLOBSearch.SelectedValue;
            }
            if(ComboBoxRegion.SelectedIndex>0)
            {
                LocationID1=ComboBoxRegion.SelectedValue;
            }
            if(ComboBoxBranch.SelectedIndex>0)
            {
                LocationID2=ComboBoxBranch.SelectedValue;
            }
            if (ComboBoxLOBSearch.SelectedIndex == 0 && ComboBoxBranch.SelectedIndex ==-1)
            {
                FunPriLoadPASA(CompanyID, hdnCustID.Value, "", ProgramID, "", "", UserID);
            }
            if (ComboBoxLOBSearch.SelectedIndex ==0 && ComboBoxBranch.SelectedIndex ==0)
            {
                FunPriLoadPASA(CompanyID, hdnCustID.Value, "", ProgramID, "", "", UserID);
            }
            if (ComboBoxLOBSearch.SelectedIndex !=0 && ComboBoxBranch.SelectedIndex !=0)
            {
                FunPriLoadPASA(CompanyID, hdnCustID.Value, ComboBoxLOBSearch.SelectedValue.ToString(), ProgramID, ComboBoxRegion.SelectedValue.ToString(), ComboBoxBranch.SelectedValue.ToString(), UserID);
            }
            FunPriLoadPASA(CompanyID, hdnCustID.Value, LOBID, ProgramID, LocationID1, LocationID2, UserID);
            FunPubLoadProductDetails();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
    #endregion
    #region Print Button Event
    /// <summary>
    /// This button will redirect to the page containing the crystal report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        string strScript = "window.open('../Reports/CustomerAtaGlanceReport.aspx','newwindow','toolbar=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Customer At a Glance", strScript, true);
    }
    #endregion
    #endregion
    #region Combobox Events
    /// <summary>
    /// This event is used to load the values of branch dropdownlist based on the region selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ComboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["Flag"] != null)
        {
            if (ViewState["Flag"].ToString() == "CB")
            {
                ViewState["Flag"] = "C";
            }
        }
        else
        {
            ViewState["Flag"] = "C";            
        }
        if (pnlAccountDetails.Visible == true && pnlasset.Visible == true && PnlCustomerDetails.Visible == true && pnlTransactionDetails.Visible == true)
        {
            ValidateGridData();
        }
        if (IsPostBack)
        {
            if (ComboBoxRegion.SelectedIndex ==0)
            {
                ComboBoxBranch.Enabled = false;
                FunPubLoadBranch();
            }
            else
            {
                ComboBoxBranch.Enabled = true;
                FunPubLoadBranch();
            }
            //FunPriGetBranch();
        }
        if (ComboBoxBranch.Items.Count == 1)
        {
            ComboBoxBranch_SelectedIndexChanged(sender, e);
        }
        if (hdnCustID.Value != string.Empty)
        {
            FunPriLoadPASA(CompanyID, hdnCustID.Value, LOBID, ProgramID, RegionID, BranchID, UserID);
        }
    }

    protected void ComboBoxLOBSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtStartDateSearch.Text = string.Empty;
        //txtEndDateSearch.Text = string.Empty;
        ComboBoxProductCode.SelectedIndex = 0;        
        //grvprimeaccount.DataSource = null;
        //grvprimeaccount.DataBind();
        //pnlPrimeandsubAcountDetails.Visible = false;        
        if (ViewState["Flag"] == null)
        {
            ViewState["Flag"] = "CB";
        }
        LOBID = string.Empty;
        if (ComboBoxLOBSearch.SelectedIndex == 0)
        {
            ComboBoxBranch.Enabled = false;
            FunPubLoadRegion();
            FunPubLoadBranch();
        }       
        else
        {
            FunPubLoadRegion();
            FunPubLoadBranch();
        }
        if (pnlAccountDetails.Visible == true && pnlasset.Visible == true && PnlCustomerDetails.Visible == true && pnlTransactionDetails.Visible == true)
        {
            ValidateGridData();
        }
        if (ComboBoxLOBSearch.SelectedIndex > 0)
        {
            LOBID = ComboBoxLOBSearch.SelectedValue.ToString();
        }
        string BranchID = ComboBoxBranch.SelectedValue.ToString();
        if (BranchID == "-1")
        {
            BranchID = "";
            if (hdnCustID.Value.ToString() != string.Empty)
            {
                FunPriLoadPASA(CompanyID, hdnCustID.Value, LOBID, ProgramID, RegionID, BranchID, UserID);
            }
        }
        else
        {
            if (hdnCustID.Value.ToString() != string.Empty)
            {
                FunPriLoadPASA(CompanyID, hdnCustID.Value, LOBID, ProgramID, RegionID, BranchID, UserID);
            }
        }
    }

    protected void grvCustomer_RowDataBound(object sender, EventArgs e)
    {
         
    }

    protected void ComboBoxBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pnlAccountDetails.Visible == true && pnlasset.Visible == true && PnlCustomerDetails.Visible == true && pnlTransactionDetails.Visible == true)
        {
            ValidateGridData();
        }
        BranchID = ComboBoxBranch.SelectedValue.ToString();
        if (hdnCustID.Value.ToString() != string.Empty)
        {
            FunPriLoadPASA(CompanyID, hdnCustID.Value.ToString(), ComboBoxLOBSearch.SelectedValue, ProgramID, ComboBoxRegion.SelectedValue, BranchID, UserID);
        }
    }

    #endregion
    #region GridView Events
    /// <summary>
    /// checks all the checkbox in the checkbox row of the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    bool isCheck = false;
    //    CheckBox chkAll = (CheckBox)sender;

    //    if (chkAll.Checked)
    //    {
    //        isCheck = true;
    //    }

    //    foreach (GridViewRow item in grvprimeaccount.Rows)
    //    {
    //        ((CheckBox)item.FindControl("chkSelectAccount")).Checked = isCheck;
    //    }
    //}
    #endregion
    #endregion
    #region methods
    #region public methods
    /// <summary>
    /// method is used to load the LOB values in the dropdownlist
    /// </summary>
    public void FunPubLoadLOB()
    {
        try
        {
            objorgClient = new ReportOrgColMgtServicesClient();
            byte[] objbytelob = objorgClient.FunPubGetPDCReminderLOBDetails(CompanyID, UserID, ProgramID);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(objbytelob);
            ComboBoxLOBSearch.DataSource = LOB;
            ComboBoxLOBSearch.DataTextField = "Description";
            ComboBoxLOBSearch.DataValueField = "ID";
            ComboBoxLOBSearch.DataBind();
            ComboBoxLOBSearch.Items.RemoveAt(0);
            ListItem item = new ListItem("--All--", "0");
            ComboBoxLOBSearch.Items.Insert(0, item);
            if (ComboBoxLOBSearch.Items.Count == 2)
            {
                ComboBoxLOBSearch.Items.RemoveAt(0);
            }
        }
        catch (Exception lob)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(lob);
            throw lob;
        }
        finally
        {
            objorgClient.Close();
        }
    }
    /// <summary>
    /// method is used to load the region values in the dropdownlist
    /// </summary>
    public void FunPubLoadRegion()
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            //if (ComboBoxLOBSearch.SelectedValue == "0")
            //{
            //    LOBID = "0";
            //}
            //else
            //{
            //    LOBID = ComboBoxLOBSearch.SelectedValue.ToString();
            //}
            //objorgClient = new ReportOrgColMgtServicesClient();
            //byte[] objbytereg = objorgClient.FunPubGetPDCReminderBranchDetails(CompanyID, UserID, true, LOBID, ProgramID);
            //List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSerialize(objbytereg);
            //ComboBoxRegion.DataSource = Region;
            //ComboBoxRegion.DataTextField = "Description";
            //ComboBoxRegion.DataValueField = "ID";
            //ComboBoxRegion.DataBind();
            //ComboBoxRegion.Items.RemoveAt(0);
            //ListItem item = new ListItem("--All--", "0");
            //ComboBoxRegion.Items.Insert(0, item);
            //if (ComboBoxRegion.Items.Count == 2)
            //{
            //    ComboBoxRegion.Items.RemoveAt(0);
            //    ComboBoxRegion.Enabled = false;
            //}
            int intlob_Id = 0;
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            byte[] byteLobs = objclient.FunPubBranch(CompanyID, UserID, ProgramID, intlob_Id);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxRegion.DataSource = Region;
            ComboBoxRegion.DataTextField = "Description";
            ComboBoxRegion.DataValueField = "ID";
            ComboBoxRegion.DataBind();
            ComboBoxRegion.Items[0].Text = "--ALL--";
        }
        catch (Exception re)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(re);
            throw re;
        }
        finally
        {
            objclient.Close();
        }
    }
    /// <summary>
    /// method is used to load the Product values in the dropdownlist
    /// </summary>
    public void FunPubLoadProduct()
    {
        try
        {
            objclient = new ReportAccountsMgtServicesClient();
            byte[] objbytepro = objclient.FunPubGetProductDetails(CompanyID, 0);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            ComboBoxProductCode.DataSource = Product;
            ComboBoxProductCode.DataTextField = "Description";
            ComboBoxProductCode.DataValueField = "ID";
            ComboBoxProductCode.DataBind();
            if (ComboBoxProductCode.Items.Count == 2)
            {
                ComboBoxProductCode.Items.RemoveAt(0);
            }
        }
        catch (Exception pro)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(pro);
            throw pro;
        }
        finally
        {
            objclient.Close();
        }
    }
    public void FunPubLoadProductDetails()
    {
        try
        {
            objclient = new ReportAccountsMgtServicesClient();
            byte[] objbytepro = objclient.FunPubGetProduct(CompanyID, hdnCustID.Value);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            ComboBoxProductCode.DataSource = Product;
            ComboBoxProductCode.DataTextField = "Description";
            ComboBoxProductCode.DataValueField = "ID";
            ComboBoxProductCode.DataBind();
            if (ComboBoxProductCode.Items.Count == 2)
            {
                ComboBoxProductCode.Items.RemoveAt(0);
            }
        }
        catch (Exception pro)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(pro);
            throw pro;
        }
        finally
        {
            objclient.Close();
        }
    }
    /// <summary>
    /// method is used to load the Branch values in the dropdownlist
    /// </summary>
    public void FunPubLoadBranch()
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            //objorgClient = new ReportOrgColMgtServicesClient();
            //int regionID = Convert.ToInt32(ComboBoxRegion.SelectedValue);
            //byte[] objbytepro = objorgClient.FunPubGetLocationDetails(CompanyID, UserID, ProgramID, Convert.ToInt32(LOBID), Convert.ToInt32(ComboBoxRegion.SelectedValue));
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            //ComboBoxBranch.DataSource = Branch;
            //ComboBoxBranch.DataTextField = "Description";
            //ComboBoxBranch.DataValueField = "ID";
            //ComboBoxBranch.DataBind();
            //ComboBoxBranch.Items.RemoveAt(0);
            //ListItem item = new ListItem("--All--", "0");
            //ComboBoxBranch.Items.Insert(0, item);
            //ComboBoxBranch.Enabled = true;
            //if (ComboBoxBranch.Items.Count == 2)
            //{
            //    ComboBoxBranch.Items.RemoveAt(0);
            //    ComboBoxBranch.Enabled = false;
            //}
            int intlob_Id = 0;
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            int Location1 = 0;
            if (ComboBoxRegion.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ComboBoxRegion.SelectedValue);
            byte[] byteLobs = objclient.FunPubGetLocation2(ProgramID, UserID, CompanyID, intlob_Id, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ComboBoxBranch.DataSource = Branch;
            ComboBoxBranch.DataTextField = "Description";
            ComboBoxBranch.DataValueField = "ID";
            ComboBoxBranch.DataBind();
            if (ComboBoxBranch.Items.Count == 2)
            {
                if (ComboBoxRegion.SelectedIndex != 0)
                {
                    ComboBoxBranch.SelectedIndex = 1;
                    Utility.ClearDropDownList(ComboBoxBranch);
                }
                else
                    ComboBoxBranch.SelectedIndex = 0;
            }
            else
            {
                ComboBoxBranch.Items[0].Text = "--ALL--";
                ComboBoxBranch.SelectedIndex = 0;
            }

        }
        catch (Exception re)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(re);
            throw re;
        }

        finally
        {
            objclient.Close();
        }
    }
    /// <summary>
    /// gets the details of the customer selected and displays it using the customer details user control
    /// </summary>
    /// <param name="CustomerID"></param>
    public void FunPubGetCustomerDetails(string CustomerID)
    {
        try
        {
            DataTable dtCustomer = new DataTable();
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Option", "56");
            Procparam.Add("@Param1", CustomerID);
            dtCustomer = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
            if (dtCustomer.Rows.Count > 0)
            {
                ucCustDetails.SetCustomerDetails(dtCustomer.Rows[0], true);
                ClsPubCustomer customer = new ClsPubCustomer();
                customer.CustomerCode = ucCustDetails.CustomerCode;
                customer.Customer = ucCustDetails.CustomerName;
                customer.Address = ucCustDetails.CustomerAddress;
                customer.Mobile = ucCustDetails.Mobile;
                customer.EMail = ucCustDetails.EmailID;
                customer.WebSite = ucCustDetails.Website;
                List<ClsPubCustomer> customerlist = new List<ClsPubCustomer>();
                customerlist.Add(customer);
                //Session["CustomerName"] = customerlist;
            }           
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }

    public void FunPubLoadAssetDetails()
    {
        try
        {
            objclient = new ReportAccountsMgtServicesClient();
            List<ClsPubPASA> PASAs = new List<ClsPubPASA>();
            foreach (GridViewRow item in grvprimeaccount.Rows)
            {
                if (((CheckBox)item.FindControl("chkSelectAccount")).Checked)
                {
                    IsChk = true;
                    ClsPubPASA pasa = new ClsPubPASA();
                    pasa.PrimeAccountNo = ((Label)item.FindControl("lblMLA")).Text;
                    pasa.SubAccountNo = ((Label)item.FindControl("lblSLA")).Text;
                    PASAs.Add(pasa);
                }
            }
            FunPubGetCustomerInformation();
            byte[] bytePASAs = ClsPubSerialize.Serialize(PASAs, SerializationMode.Binary);
            decimal OpeningBalance;
            byte[] byteAsset = objclient.FunPubGetSOAAssetDetails(out OpeningBalance, CompanyID, Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString(), Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString(), bytePASAs);
            lblOpeningBalance.Text = GetOpeningBalance(OpeningBalance);
            assets = (ClsPubSOAAsset)DeSerialize(byteAsset);
            TotalAmountFinanced = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.AmountFinanced);
            Totalyettobebilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.YetToBeBilled);
            Totalbilled = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Billed);
            totalbilledbalance = assets.AccountDetails.Sum(ClsPubSOAAsset => ClsPubSOAAsset.Balance);
            totalDues = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Dues);
            totalReceipts = assets.Transaction.Sum(clspubTransaction => clspubTransaction.Receipts);
            totalBalance = OpeningBalance + totalDues - totalReceipts;
            Session["Assets"] = assets;
            gvasset.DataSource = assets.AssetDetails;
            gvasset.DataBind();
            grvAccount.DataSource = assets.AccountDetails;
            grvAccount.DataBind();
            grvtransaction.DataSource = assets.Transaction;
            grvtransaction.DataBind();
            Utility.GetAlternativeColorToGrid(grvtransaction, "lblPANum");
            if (gvasset.Rows.Count <= 0)
            {
                gvasset.EmptyDataText = "No Assets Found";
                gvasset.DataBind();
            }
            if (grvAccount.Rows.Count <= 0)
            {
                grvAccount.EmptyDataText = "No Asset accounts Found";
                grvAccount.DataBind();
            }
            if (grvtransaction.Rows.Count <= 0)
            {
                grvtransaction.EmptyDataText = "No Transactions Found";
                grvtransaction.DataBind();
                LblCurrency.Text = string.Empty;
                //lblOpeningBalance.Visible = false;
            }

            if (grvAccount.FooterRow != null)
            {
                ((Label)grvAccount.FooterRow.FindControl("lblTotalYetbilled")).Text = Totalyettobebilled.ToString();
                ((Label)grvAccount.FooterRow.FindControl("lblTotalbilled")).Text = Totalbilled.ToString();
                ((Label)grvAccount.FooterRow.FindControl("lblTotalbilledbalance")).Text = totalbilledbalance.ToString();
                ((Label)grvAccount.FooterRow.FindControl("lblTotalAmount")).Text = TotalAmountFinanced.ToString();
            }
            if (grvtransaction.FooterRow != null)
            {
                ((Label)grvtransaction.FooterRow.FindControl("lblTotalDues")).Text = totalDues.ToString(Funsetsuffix());
                ((Label)grvtransaction.FooterRow.FindControl("lblTotalReceipts")).Text = totalReceipts.ToString(Funsetsuffix());
                ((Label)grvtransaction.FooterRow.FindControl("lblTotalbalance")).Text = totalBalance.ToString(Funsetsuffix());
            }
        }

        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }

        finally
        {
            objclient.Close();
        }
    }

    #endregion
    #region Private Methods
    /// <summary>
    /// this method is used to deserialize the data from service layer
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    /// <summary>
    /// This method is used to load the prime and subaccount details
    /// </summary>
    /// <param name="CompanyId"></param>
    /// <param name="Customer_ID"></param>
    private void FunPriLoadPASA(int CompanyId, string Customer_ID, string LobID, int ProgramID, string RegionID, string BranchID, int UserID)
    {
        objclient = new ReportAccountsMgtServicesClient(); 
        try
        {
            string ProductID=Convert.ToString(ComboBoxProductCode.SelectedValue);
            if (ProductID == "-1")
            {
                ProductID = "";
            }
            ClsPubSOASelectionCriteria objPASA=new ClsPubSOASelectionCriteria();
            objPASA.CompanyId=CompanyId;
            if (Customer_ID != "")
            {
                objPASA.CustomerId = Convert.ToInt32(Customer_ID);
            }
            if (ComboBoxLOBSearch.SelectedIndex != 0)
            {
                objPASA.LobId = Convert.ToString(ComboBoxLOBSearch.SelectedValue);
            }
            if (ComboBoxRegion.SelectedIndex != 0)
            {
                objPASA.LocationID1 = Convert.ToString(ComboBoxRegion.SelectedValue);
            }
            //if (ComboBoxBranch.SelectedIndex != 0)
            //{
            //    objPASA.LocationID2 = Convert.ToString(ComboBoxBranch.SelectedValue);
            //}       
            objPASA.LocationID2 = Convert.ToString(ComboBoxBranch.SelectedValue);

            objPASA.ProgramId = ProgramID;
            if (ComboBoxProductCode.SelectedIndex != 0)
            {
                objPASA.ProductId = Convert.ToString(ComboBoxProductCode.SelectedValue);
            }
            objPASA.UserId=UserID;
            byte[] bytePASA = objclient.FunPubGetPASA(objPASA); 
            List<ClsPubPASA> PASADetails = (List<ClsPubPASA>)DeSerialize(bytePASA);
            grvprimeaccount.DataSource = PASADetails;
            grvprimeaccount.DataBind();
            objclient.Close();
            if (grvprimeaccount.Rows.Count == 0)
            {
                if (hdnCustID.Value == "")
                {
                    LblCurrency.Text = string.Empty;
                    FunPriEnableDisableGrid(false);
                    btnGo.Enabled = true;
                }
                else
                {
                    grvprimeaccount.EmptyDataText = "No Records Found";
                    grvprimeaccount.DataBind();
                    LblCurrency.Text = string.Empty;
                    FunPriEnableDisableGrid(false);
                    btnGo.Enabled = false;
                }
            }
            else
            {
                btnGo.Enabled = true;
            }
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            objclient.Close();
        }
    }

    protected void ComboBoxProductCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pnlAccountDetails.Visible == true && pnlasset.Visible == true && PnlCustomerDetails.Visible == true && pnlTransactionDetails.Visible == true)
        {
            ValidateGridData();
        }
        if (hdnCustID.Value != string.Empty)
        {
            FunPriLoadPASA(CompanyID, hdnCustID.Value, LOBID, ProgramID, RegionID, BranchID, UserID);
        }
    }


    /// <summary>
    /// sets the decimal precision in the grid according to the GPS format
    /// </summary>
    /// <returns></returns>
    private string Funsetsuffix()
    {

        int suffix = 1;

        suffix = objsession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private void FunPriEnableDisableGrid(bool IsVisible)
    {
        PnlCustomerDetails.Visible = IsVisible;
        pnlTransactionDetails.Visible = IsVisible;
        pnlasset.Visible = IsVisible;
        pnlAccountDetails.Visible = IsVisible;
        BtnPrint.Visible = IsVisible;
        ComboBoxBranch.Enabled = IsVisible;
    }

    private string GetOpeningBalance(decimal val)
    {
        string OpeningBalance = string.Empty;
        if (val < 0)
        {

            OpeningBalance = "Opening Balance as on " + txtStartDateSearch.Text + " is " + val.ToString(Funsetsuffix()).Replace("-", " ").TrimStart() + " " + "Cr.";
        }
        else
        {
            OpeningBalance = "Opening Balance as on " + txtStartDateSearch.Text + " is " + val.ToString(Funsetsuffix()) + " " + "Dr.";

        }
        return OpeningBalance;
    }

    private void FunPriDisableSession()
    {
        Session["CustomerName"] = null;
        //Session["PASA"]=null;
        Session["OpeningBalance"] = null;
        Session["AccountTransactionDetails"] = null;
        Session["CustomerGlanceDetails"] = null;
        Session["OpeningBalance"] = null;
        Session["TotalDues"] = null;
        Session["TotalReceipts"] = null;
        Session["TotalBalance"] = null;
        Session["Currency"] = null;
        Session["GPSDecimalFormat"] = null;
        Session["CompanyName"] = null;
        Session["Assets"] = null;
        Session["ProductName"] = null;
        Session["RegionName"] = null;
        Session["LOBName"] = null;
        Session["BranchName"] = null;
    }


    protected void grvtransaction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMLA = (Label)e.Row.FindControl("lblPANum");
                Label lblSLA = (Label)e.Row.FindControl("lblSANum");
                if (lblSLA.Text.Trim() == lblMLA.Text.Trim() + "DUMMY")
                {
                    lblSLA.Text = "";
                }
            }
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    protected void grvtransaction_DataBound(object sender, EventArgs e)
    {

    }

    protected void grvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMLA = (Label)e.Row.FindControl("Primeac");
                Label lblSLA = (Label)e.Row.FindControl("Subac");

                if (lblSLA.Text.Trim() == lblMLA.Text.Trim() + "DUMMY")
                {
                    lblSLA.Text = "";
                }
            }
        }

        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void grvAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPANum = (Label)e.Row.FindControl("lblPANum");
                Label lblSANum = (Label)e.Row.FindControl("lblSANum");

                if (lblSANum.Text.Trim() == lblPANum.Text.Trim() + "DUMMY")
                {
                    lblSANum.Text = "";
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvasset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPANum = (Label)e.Row.FindControl("lblPANum");
                Label lblSANum = (Label)e.Row.FindControl("lblSANum");

                if (lblSANum.Text.Trim() == lblPANum.Text.Trim() + "DUMMY")
                {
                    lblSANum.Text = "";
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    #endregion


    private void FunPriLoadLob(int intCompany_id, int intUser_id, bool Is_Active, string intCustomer_Id)
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            ComboBoxLOBSearch.Items.Clear();
            byte[] byteLobs = objclient.FunPubGetCustomerBasedLOB(intCompany_id, intUser_id, Is_Active, intCustomer_Id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxLOBSearch.DataSource = lobs;
            ComboBoxLOBSearch.DataTextField = "Description";
            ComboBoxLOBSearch.DataValueField = "ID";
            ComboBoxLOBSearch.DataBind();
            ComboBoxLOBSearch.Items.RemoveAt(0);
            ListItem item = new ListItem("--All--", "0");
            ComboBoxLOBSearch.Items.Insert(0, item);
            if (ComboBoxLOBSearch.Items.Count == 2)
            {
                ComboBoxLOBSearch.SelectedIndex = 1;
            }
            else
            {
                ComboBoxLOBSearch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objclient.Close();
        }
    }
    private void FunPriLoadBranch(int intCompany_id, int intUser_id, bool Is_Active, string intCustomer_Id)
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            ComboBoxBranch.Items.Clear();
            byte[] byteLobs = objclient.FunPubGetBranch(intCompany_id, intUser_id, ProgramID, intCustomer_Id, Convert.ToInt32(LOBID));
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxBranch.DataSource = Branch;
            ComboBoxBranch.DataTextField = "Description";
            ComboBoxBranch.DataValueField = "ID";
            ComboBoxBranch.DataBind();
            if (ComboBoxBranch.Items.Count == 2)
            {
                ComboBoxBranch.SelectedIndex = 1;
            }
            else
            {
                ComboBoxBranch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objclient.Close();
        }
    }

    private void FunPriGetBranch()
    {
        try
        {
            objclient = new ReportAccountsMgtServicesClient();
            ComboBoxBranch.Items.Clear();
            byte[] byteLobs = objclient.FunPubBranch(CompanyID, UserID, ProgramID, Convert.ToInt32(LOBID));
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxBranch.DataSource = Branch;
            ComboBoxBranch.DataTextField = "Description";
            ComboBoxBranch.DataValueField = "ID";
            ComboBoxBranch.DataBind();
            ComboBoxBranch.Items.RemoveAt(0);
            ListItem item = new ListItem("--All--", "0");
            ComboBoxBranch.Items.Insert(0, item);
            if (ComboBoxBranch.Items.Count == 2)
            {
                ComboBoxBranch.SelectedIndex = 1;
                ComboBoxBranch.Enabled = false;
            }
            else
            {
                ComboBoxBranch.SelectedIndex = 0;
                ComboBoxBranch.Enabled = false;
            }
            ComboBoxBranch.Enabled = true;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objclient.Close();
        }
    }

    private void ValidateGridData()
    {
        grvAccount.DataSource = null;
        grvAccount.DataBind();
        grvCustomer.DataSource = null;
        grvCustomer.DataBind();
        grvtransaction.DataSource = null;
        grvtransaction.DataBind();
        pnlAccountDetails.Visible = false;
        pnlasset.Visible = false;
        PnlCustomerDetails.Visible = false;
        pnlTransactionDetails.Visible = false;
        LblCurrency.Visible = false;
        BtnPrint.Visible = false;
    }
    /// <summary>
    /// Created this Event to check or uncheck the all check box in the grid automatically
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvprimeaccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CbAssets = (CheckBox)e.Row.FindControl("chkSelectAccount");
                CbAssets.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvprimeaccount.ClientID + "','chkSelectAll','chkSelectAccount');");
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chAll = (CheckBox)e.Row.FindControl("chkSelectAll");
                chAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvprimeaccount.ClientID + "',this,'chkSelectAccount');");
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    private void FunPriLoadRegion()
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            ComboBoxRegion.Items.Clear();
            int intlob_Id = 0;
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            byte[] byteLobs = objclient.FunPubGetBranch(CompanyID, UserID, ProgramID, hdnCustID.Value, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxRegion.DataSource = Branch;
            ComboBoxRegion.DataTextField = "Description";
            ComboBoxRegion.DataValueField = "ID";
            ComboBoxRegion.DataBind();
            ComboBoxRegion.Items[0].Text = "--ALL--";
            if (ComboBoxRegion.Items.Count == 2)
            {
                ComboBoxRegion.SelectedIndex = 1;
            }
            else
            {
                ComboBoxRegion.SelectedIndex = 0;
            }
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            objclient.Close();
        }
    }

    private void FunPriloadCustomercodes()
    {
        if (ComboBoxRegion.SelectedIndex > 0 || ComboBoxLOBSearch.SelectedIndex > 0)
        {
            if (ViewState["Flag"] == null)
            {
                if (ViewState["Flag"].ToString() == "C")
                {

                    ucCustomerCodeLov.strLOBID = ComboBoxLOBSearch.SelectedValue.ToString();
                    ucCustomerCodeLov.strBranchID = ComboBoxRegion.SelectedValue.ToString();
                    ucCustomerCodeLov.strRegionID = ComboBoxBranch.SelectedItem.Value.ToString();
                    ucCustomerCodeLov.strLOV_Code = "CAG";
                }
                if (ViewState["Flag"].ToString() == "CB")
                {
                    ucCustomerCodeLov.strLOBID = ComboBoxLOBSearch.SelectedValue.ToString();
                    ucCustomerCodeLov.strBranchID = "";
                    ucCustomerCodeLov.strRegionID = "";
                    ucCustomerCodeLov.strLOV_Code = "CAG";
                }
            }
        }
        else
            ucCustomerCodeLov.strLOV_Code = "CMD";


        //if (ddlLOB.SelectedValue != "-1")
        //{
        //    ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue.ToString();
        //    ucCustomerCodeLov.strLOV_Code = "REPAY";
        //}
        //else
        //    ucCustomerCodeLov.strLOV_Code = "CMD";

        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
        txtName.Attributes.Add("ReadOnly", "ReadOnly");

    }

    private void FunPubGetCustomerInformation()
    {
        ClsPubCustomer ObjCustomer = new ClsPubCustomer();
        List<ClsPubCustomer> objCustomerList = new List<ClsPubCustomer>();
        ObjCustomer.Customer = ucCustDetails.CustomerName + " (" + ucCustDetails.CustomerCode + ")";
        // int intcustomerTypeID = 13;//12 individual , 13 Non- Individual
        //ObjCustomer.CustomerCode = ucCustDetails.CustomerCode;
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Customer_Id", hdnCustID.Value);
        Procparam.Add("@COMPANY_ID", Convert.ToString(CompanyID));
        DataTable dtaddress = Utility.GetDefaultData("S3G_RPT_GetCustomerAddress", Procparam);
        //ucCustDetails.SetCustomerDetails(dtaddress.Rows[0], true);
        if (dtaddress.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["NAME"].ToString()))
                ObjCustomer.Address += dtaddress.Rows[0]["NAME"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["ADDRESS"].ToString()))
                ObjCustomer.Address += " \n" + dtaddress.Rows[0]["ADDRESS"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["ADDRESS2"].ToString()))
                ObjCustomer.Address += " \n" + dtaddress.Rows[0]["ADDRESS2"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["CITY"].ToString()))
                ObjCustomer.Address += " \n" + dtaddress.Rows[0]["CITY"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["STATE"].ToString()))
                ObjCustomer.Address += " , " + dtaddress.Rows[0]["STATE"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["COUNTRY"].ToString()))
                ObjCustomer.Address += " \n" + dtaddress.Rows[0]["COUNTRY"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["PINCODE"].ToString()))
                ObjCustomer.Address += " - " + dtaddress.Rows[0]["PINCODE"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["MOBILE"].ToString()))
                ObjCustomer.Address += " \n" + "Mobile  :  " + dtaddress.Rows[0]["MOBILE"].ToString();
            if (!string.IsNullOrEmpty(dtaddress.Rows[0]["EMAIL"].ToString()))
                ObjCustomer.Address += "  " + "Email  :  " + dtaddress.Rows[0]["EMAIL"].ToString();
            //intcustomerTypeID = Convert.ToInt32(dtaddress.Rows[0]["TYPEID"].ToString());
        }
        else
        {
            ObjCustomer.Address = ucCustDetails.CustomerAddress;
        }
        //ObjCustomer.Mobile = ucCustDetails.Mobile;
        //ObjCustomer.EMail = ucCustDetails.EmailID;
        //ObjCustomer.WebSite = ucCustDetails.Website;
        objCustomerList.Add(ObjCustomer);
        Session["CustomerName"] = objCustomerList;
    }
}