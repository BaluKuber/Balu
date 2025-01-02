#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   PDC Reminder Report
/// Created By          :   Ganapathy Subramanian.G
/// Created Date        :   26/04/2011
/// Purpose             :   To get the details of PDC reminder to be sent for customers within the date range
/// Last Updated By		:   Ganapathy Subramanian.G
/// Last Updated Date   :   14/10/2011
/// Reason              :   For Bug Fixing
/// <Program Summary>
#endregion






using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Globalization;
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
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using CrystalDecisions.CrystalReports.Engine;
public partial class Reports_S3GRptPDCReminderReport : ApplyThemeForProject
{
    #region variable declaration
    ReportOrgColMgtServicesClient objserclient;
    ReportAccountsMgtServicesClient objSerClient;
    CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient;    
    UserInfo objuserinfo = new UserInfo();
    S3GSession objsession = new S3GSession();    
    public int intCompanyID, intUserID,intProgramID;    
    public string strDateFormat,strFromEmail;
    public bool chk;
    ClsPubRptPDCReminderHeaderDetails objpdcheader = new ClsPubRptPDCReminderHeaderDetails();
  
    #endregion
    #region page load

    protected void Page_Load(object sender, EventArgs e)
    {      
        intCompanyID = objuserinfo.ProCompanyIdRW;
        intUserID = objuserinfo.ProUserIdRW;
        intProgramID = 147;
        strDateFormat = objsession.ProDateFormatRW;
        CalendarExtenderStartDateSearch.Format = objsession.ProDateFormatRW;
        CalendarExtenderEndDateSearch.Format = objsession.ProDateFormatRW;
        /* Changed Date Control start - 30-Nov-2012 */
        //txtStartDateSearch.Attributes.Add("readonly", "readonly");
        //txtEndDateSearch.Attributes.Add("readonly", "readonly");
        txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
        txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
        /* Changed Date Control end - 30-Nov-2012 */
        try
        {
            if (!IsPostBack)
            {
                //lblCurrency.Text = "All amounts are in" + " " + objsession.ProCurrencyNameRW;
                ViewState["CompanyAddr"] = GetCompanyAddressDetails(intCompanyID);
                FunPriDisabelEnableControls(false);
                FunPubLoadLOB();
                FunPriLoadRegion();
                FunPriLoadLocation();
                //lblCurrency.Visible = false;
                txtFilePath.Enabled = false;
                //BtnPrint.Enabled = false;
                //ComboBoxBranchSearch.Enabled = false;
                ComboBoxLocationSearch.Enabled = false;
                //ComboBoxLOBSearch.Focus();
                txtStartDateSearch.Focus();
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        grvPDCDetails.EmptyDataText = "";
    }
    #endregion
    #region methods
    #region Public methods
    /// <summary>
    /// Function loads the LOB Details into the dropdownlist
    /// </summary>
    public void FunPubLoadLOB()
    {
        objserclient = new ReportOrgColMgtServicesClient();
        try
        {            
            byte[] objbytelob = objserclient.FunPubGetPDCReminderLOBDetails(intCompanyID,intUserID,intProgramID);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(objbytelob);
            ComboBoxLOBSearch.DataSource = LOB;
            ComboBoxLOBSearch.DataTextField = "Description";
            ComboBoxLOBSearch.DataValueField = "ID";
            ComboBoxLOBSearch.DataBind();
            ComboBoxLOBSearch.Items.RemoveAt(0);
            ListItem item = new ListItem("--ALL--", "0");
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
            objserclient.Close();
        }
    }
    /// <summary>
    /// Function will load the branches into the dropdownlist
    /// </summary>
    private void FunPriLoadRegion()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int intlob_Id = 0;
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyID, intUserID, intProgramID, intlob_Id);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxBranchSearch.DataSource = Region;
            ComboBoxBranchSearch.DataTextField = "Description";
            ComboBoxBranchSearch.DataValueField = "ID";
            ComboBoxBranchSearch.DataBind();
            ComboBoxBranchSearch.Items[0].Text = "--ALL--";
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyID, intUserID, intProgramID, intlob_Id);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ComboBoxLocationSearch.DataSource = Region;
            ComboBoxLocationSearch.DataTextField = "Description";
            ComboBoxLocationSearch.DataValueField = "ID";
            ComboBoxLocationSearch.DataBind();
            ComboBoxLocationSearch.Items[0].Text = "--ALL--";
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            if (ComboBoxLOBSearch.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ComboBoxLOBSearch.SelectedValue);
            int Location1 = 0;
            if (ComboBoxBranchSearch.SelectedIndex != 0)
                Location1 = Convert.ToInt32(ComboBoxBranchSearch.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramID, intUserID, intCompanyID, intlob_Id, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ComboBoxLocationSearch.DataSource = Branch;
            ComboBoxLocationSearch.DataTextField = "Description";
            ComboBoxLocationSearch.DataValueField = "ID";
            ComboBoxLocationSearch.DataBind();
            if (ComboBoxLocationSearch.Items.Count == 2)
            {
                if (ComboBoxBranchSearch.SelectedIndex != 0)
                {
                    ComboBoxLocationSearch.SelectedIndex = 1;
                    Utility.ClearDropDownList(ComboBoxLocationSearch);
                }
                else
                    ComboBoxLocationSearch.SelectedIndex = 0;
            }
            else
            {
                ComboBoxLocationSearch.Items[0].Text = "--ALL--";
                ComboBoxLocationSearch.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    #endregion
    #region private method
    /// <summary>
    /// function used to deserialize the data
    /// </summary>
    /// <param name="byteObj"></param>
    /// <returns></returns>
    private object DeSerialize(byte[] byteObj)
    {
        try
        {
            return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;            
        }
    }
    #endregion
    #endregion

    #region Events
    #region GoButtonClickEvent
    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPriClearSession();
        objserclient = new ReportOrgColMgtServicesClient();
        try
        {
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to Start Date");
                return;
            }            
            objpdcheader.CompanyId = intCompanyID;
            objpdcheader.UserId = intUserID;
            objpdcheader.LOBId = (ComboBoxLOBSearch.SelectedValue).ToString();
            objpdcheader.LineOfBusiness = (ComboBoxLOBSearch.SelectedItem).Text.Split('-')[1].ToString();
            objpdcheader.LocationId1 = (ComboBoxBranchSearch.SelectedValue).ToString();
            objpdcheader.LocationId2 = (ComboBoxLocationSearch.SelectedValue).ToString();
            objpdcheader.ProgramId = Convert.ToString(intProgramID);
            objpdcheader.Branch = ComboBoxBranchSearch.SelectedItem.ToString();
            objpdcheader.StartDate = Utility.StringToDate((txtStartDateSearch.Text));           
            objpdcheader.EndDate = Utility.StringToDate((txtEndDateSearch.Text));           
            List<ClsPubRptPDCReminderHeaderDetails> objlistpdcheader = new List<ClsPubRptPDCReminderHeaderDetails>();
            objlistpdcheader.Add(objpdcheader);
            Session["PDCReminderHeaderDetails"] = objlistpdcheader;
            FunPriDisabelEnableControls(true);
            byte[] pdcremindergriddetails = ClsPubSerialize.Serialize(objpdcheader, SerializationMode.Binary);
            byte[] objpdcreminder = objserclient.FunPubGetPDCReminderGridDetails(pdcremindergriddetails);
            //OBJPDCREMINDERGRID = (List<ClsPubRptPDCReminderGridDetails>)DeSerialize(objpdcreminder);
            ClsPubPDCReminderDetails PDCReminderDetails = (ClsPubPDCReminderDetails)DeSerialize(objpdcreminder);
            grvPDCDetails.DataSource = PDCReminderDetails.GridDetails;
            grvPDCDetails.DataBind();           
            
            if (grvPDCDetails.Rows.Count == 0)
            {
                grvPDCDetails.EmptyDataText = "No Records Found";
                grvPDCDetails.DataBind();
                //lblCurrency.Visible = false;
                BtnPrint.Enabled = false;
                btnGeneratePDF.Enabled = false;
                btnEMail.Enabled = false;
            }
            else
            {
                Session["PDCReminderAssets"] = PDCReminderDetails.AssetDetails;
                Session["PDCReminderGridDetails"] = PDCReminderDetails.GridDetails;
                //ViewState["CustomerMail"] = PDCReminderDetails.CustomerList;
                //lblCurrency.Visible = true;
                BtnPrint.Enabled = true;
                btnGeneratePDF.Enabled = true;
                btnEMail.Enabled = false;
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            objserclient.Close();
        }
    }
    #region checkbox Events    

    protected void grvPDCDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox CbAssets = (CheckBox)e.Row.FindControl("chkSelectAccount");
                CbAssets.Attributes.Add("onclick", "javascript:fnGridUnSelect('" + grvPDCDetails.ClientID + "','chkExcludeAll','chkSelectAccount');");               
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chAll = (CheckBox)e.Row.FindControl("chkExcludeAll");
                chAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + grvPDCDetails.ClientID + "',this,'chkSelectAccount');");
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }   
   
    private int FunPriSelectUnCheckedRows()
    {
        DataTable dtCustomerDetails = new DataTable();
        bool bisChecked;
        try
        {            
            dtCustomerDetails.Columns.Add("Customer_Name");
            dtCustomerDetails.Columns.Add("Comm_Address1");
            dtCustomerDetails.Columns.Add("Comm_Address2");
            dtCustomerDetails.Columns.Add("Comm_City");
            dtCustomerDetails.Columns.Add("Comm_State");
            dtCustomerDetails.Columns.Add("Comm_Country");
            dtCustomerDetails.Columns.Add("Comm_Pincode");
            dtCustomerDetails.Columns.Add("CustomerName");
            dtCustomerDetails.Columns.Add("E-Mail");
            dtCustomerDetails.Columns.Add("Mobile");
            List<ClsPubRptPDCReminderGridDetails> objsessiongriddetails = new List<ClsPubRptPDCReminderGridDetails>();
            List<ClsPubRptPDCReminderGridDetails> griddetails = (List<ClsPubRptPDCReminderGridDetails>)Session["PDCReminderGridDetails"];
            List<ClsPubCustomerList> objCustomerMail = new List<ClsPubCustomerList>();
            for (int intindex = 0; intindex < grvPDCDetails.Rows.Count; intindex++)
            {
                DataRow drRow = dtCustomerDetails.NewRow();
                GridViewRow row = grvPDCDetails.Rows[intindex];
                bisChecked = ((CheckBox)row.FindControl("chkSelectAccount")).Checked;

                if (!bisChecked)
                {
                    ClsPubCustomerList objcustomerMail = new ClsPubCustomerList();
                    ClsPubRptPDCReminderGridDetails objcheckedrowdetails = new ClsPubRptPDCReminderGridDetails();
                    drRow["Customer_Name"]=objcustomerMail.CustomerName = objcheckedrowdetails.CUSTOMER_NAME = griddetails[intindex].CUSTOMER_NAME;
                    drRow["CustomerName"] = objcheckedrowdetails.CUSTOMERNAME = griddetails[intindex].CUSTOMERNAME;
                    objcheckedrowdetails.PRIMEACCOUNTNO = griddetails[intindex].PRIMEACCOUNTNO;
                    objcheckedrowdetails.SUBACCOUNTNO = griddetails[intindex].SUBACCOUNTNO;
                    objcheckedrowdetails.LASTCOLLECTEDPDCDATE = griddetails[intindex].LASTCOLLECTEDPDCDATE;
                    objcheckedrowdetails.RepaymentDateFrom = griddetails[intindex].RepaymentDateFrom;
                    objcheckedrowdetails.RepaymentDateTo = griddetails[intindex].RepaymentDateTo;
                    objcheckedrowdetails.Report_Date = griddetails[intindex].Report_Date;
                    drRow["Comm_Address1"] = objcheckedrowdetails.Comm_Address1 = griddetails[intindex].Comm_Address1;
                    drRow["Comm_Address2"] = objcheckedrowdetails.Comm_Address2 = griddetails[intindex].Comm_Address2;
                    drRow["Comm_City"] = objcheckedrowdetails.Comm_City = griddetails[intindex].Comm_City;
                    drRow["Comm_Country"] = objcheckedrowdetails.Comm_country = griddetails[intindex].Comm_country;
                    drRow["Comm_Pincode"] = objcheckedrowdetails.Comm_PinCode = griddetails[intindex].Comm_PinCode;
                    drRow["Comm_State"] = objcheckedrowdetails.Comm_State = griddetails[intindex].Comm_State;
                    drRow["E-Mail"] = objcheckedrowdetails.CustomerMail = griddetails[intindex].CustomerMail;
                    drRow["Mobile"] = objcheckedrowdetails.Comm_Mobile = griddetails[intindex].Comm_Mobile;
                    objcheckedrowdetails.CompanyAddress = ViewState["CompanyAddr"].ToString();
                    objcheckedrowdetails.CompanyName = griddetails[intindex].CompanyName;
                    objcheckedrowdetails.CustomerAddress = SetCustomerAddress(drRow);
                    objcustomerMail.CustomerMail=objcheckedrowdetails.CustomerMail = griddetails[intindex].CustomerMail;
                    objsessiongriddetails.Add(objcheckedrowdetails);
                    objCustomerMail.Add(objcustomerMail);
                }
                row.Dispose();
            }
           
            Session["sessionGridDetails"] = objsessiongriddetails;
            ViewState["CustomerMail"] = objCustomerMail;
            return objsessiongriddetails.Count;
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }

        finally
        {
            dtCustomerDetails.Clear();
            dtCustomerDetails.Dispose();            
        }
    }

    //private string GetCompanyAddressDetails(int companyId)
    //{
    //    objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
    //    try
    //    {            
    //        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjS3G_CompanyMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable();
    //        StringBuilder sbCompanyAddr = new StringBuilder();
    //        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable dtCompany;
    //        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjCompanyMasterRow;
    //        ObjCompanyMasterRow = ObjS3G_CompanyMaster_ViewDataTable.NewS3G_SYSAD_CompanyMaster_ViewRow();
    //        ObjCompanyMasterRow.Company_ID = 0;
    //        ObjS3G_CompanyMaster_ViewDataTable.AddS3G_SYSAD_CompanyMaster_ViewRow(ObjCompanyMasterRow);            
    //        SerializationMode SerMode = SerializationMode.Binary;
    //        byte[] byteCompanyDetails = objCompanyMasterClient.FunPubQueryCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_CompanyMaster_ViewDataTable, SerMode));
    //        dtCompany = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCompanyDetails, SerializationMode.Binary, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));

    //        if (dtCompany.Rows.Count > 0)
    //        {
    //            DataRow drCompanyAddr = dtCompany.Rows[0];
    //            sbCompanyAddr.Append(drCompanyAddr["Address1"].ToString().Trim());
    //            sbCompanyAddr.Append(",");
    //            sbCompanyAddr.Append(drCompanyAddr["Address2"].ToString());
    //            sbCompanyAddr.Append(",\n");
    //            sbCompanyAddr.Append(drCompanyAddr["City"].ToString());
    //            sbCompanyAddr.Append(",");
    //            sbCompanyAddr.Append(drCompanyAddr["State"].ToString());
    //            sbCompanyAddr.Append(",");
    //            sbCompanyAddr.Append(drCompanyAddr["Country"].ToString());
    //            sbCompanyAddr.Append(" - ");
    //            sbCompanyAddr.Append(drCompanyAddr["Zip_Code"].ToString());
    //            sbCompanyAddr.Append("\nTelephone - ");
    //            sbCompanyAddr.Append(drCompanyAddr["CD_Telephone_Number"].ToString());
    //            sbCompanyAddr.Append("\n");
    //            sbCompanyAddr.Append("E-Mail - ");
    //            sbCompanyAddr.Append(drCompanyAddr["CD_Email_ID"].ToString());
    //            sbCompanyAddr.Append("\n");
    //            ViewState["FromMail"] = drCompanyAddr["CD_Email_ID"].ToString();
    //            //ViewState["FromMail"] = "ganapathy.g@sundaraminfotech.in";
    //            sbCompanyAddr.Append("Website - ");
    //            sbCompanyAddr.Append(drCompanyAddr["CD_Website"].ToString());                
    //        }
    //        return sbCompanyAddr.ToString();
    //    }
    //    catch (Exception e)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(e);
    //        throw e;
    //    }

    //    finally
    //    {
    //        objCompanyMasterClient.Close();
    //    }      

    //}

    private string GetCompanyAddressDetails(int companyId)
    {
        CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient;
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjS3G_CompanyMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable();
        StringBuilder sbCompanyAddr = new StringBuilder();
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable dtCompany;
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjCompanyMasterRow;
        ObjCompanyMasterRow = ObjS3G_CompanyMaster_ViewDataTable.NewS3G_SYSAD_CompanyMaster_ViewRow();
        ObjCompanyMasterRow.Company_ID = 0;
        ObjS3G_CompanyMaster_ViewDataTable.AddS3G_SYSAD_CompanyMaster_ViewRow(ObjCompanyMasterRow);
        objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] byteCompanyDetails = objCompanyMasterClient.FunPubQueryCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_CompanyMaster_ViewDataTable, SerMode));
        dtCompany = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCompanyDetails, SerializationMode.Binary, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));

        if (dtCompany.Rows.Count > 0)
        {
            DataRow drCompanyAddr = dtCompany.Rows[0];
            sbCompanyAddr.Append("<center>");
            sbCompanyAddr.Append(drCompanyAddr["Address1"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["Address2"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["City"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["State"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["Country"].ToString());
            sbCompanyAddr.Append(" - ");
            sbCompanyAddr.Append(drCompanyAddr["Zip_Code"].ToString());
            sbCompanyAddr.Append("</br>Ph :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Telephone_Number"].ToString());
            sbCompanyAddr.Append("  Mobile :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Mobile_Number"].ToString());
            sbCompanyAddr.Append("</br>EMail :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Email_ID"].ToString());
            sbCompanyAddr.Append("</br>");
            sbCompanyAddr.Append(drCompanyAddr["CD_Website"].ToString());
            sbCompanyAddr.Append("</center>");
            ViewState["FromMail"] = drCompanyAddr["CD_Email_ID"].ToString();

        }
        return sbCompanyAddr.ToString();
    }

    #endregion
    #endregion
    private void FunPriDisabelEnableControls(bool IsVisible)
    {
        try
        {
            pnlPDCDetails.Visible = IsVisible;
            BtnPrint.Visible = IsVisible;
            btnGeneratePDF.Visible = IsVisible;
            btnEMail.Visible = IsVisible;
            //btnEMail.Enabled = IsVisible;
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;            
        }
    }
    #region Clear Button Event
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ComboBoxLOBSearch.SelectedValue = "0";
            FunPriLoadRegion();
            FunPriLoadLocation();
            if (ComboBoxLocationSearch.Enabled == false)
            {
                ComboBoxLocationSearch.Enabled = true;
            }
            ComboBoxLocationSearch.DataSource = null;
            ComboBoxLocationSearch.DataBind();
            ComboBoxLocationSearch.Enabled = false;
            txtStartDateSearch.Text = string.Empty;
            txtEndDateSearch.Text = string.Empty;
            grvPDCDetails.DataSource = null;
            grvPDCDetails.DataBind();
            grvPDCDetails.EmptyDataText = "";          
            FunPriClearSession();
            FunPriDisabelEnableControls(false);
            txtFilePath.Text="";
            if (ViewState["DocumentPath1"] != null)
            {
                ViewState["DocumentPath1"] = null;
            }
            //lblCurrency.Text = "";
            //ComboBoxBranchSearch.Enabled = false;
            RFVComboBranch.Enabled = false;
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }
    #endregion
    #endregion
    #region Print
    protected void BtnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (FunPriSelectUnCheckedRows() > 0)
            {
                string strScript = "window.open('../Reports/PDCreminder.aspx','newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PDCReminder", strScript, true);
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Cannot Print. All Accounts are Excluded");
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;            
        }
    }
    protected void btnGeneratePDF_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["DocumentPath"] != null)
            {
                ViewState["DocumentPath"] = null;
            }
            FunPubGetPDCDocumentPath();           
            FunPubGeneratePDF(); 
        }
         catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }       
    }  

    protected void ComboBoxLOBSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["DocumentPath"] != null)
        {
            ViewState["DocumentPath"] = null;
        }
        FunPubGetPDCDocumentPath();
        if (txtStartDateSearch.Text != string.Empty)
        {
            txtStartDateSearch.Text = string.Empty;
        }
        if (txtEndDateSearch.Text != string.Empty)
        {
            txtEndDateSearch.Text = string.Empty;
        }
        if (pnlPDCDetails.Visible == true)
        {
            FunPriValidateGrid();
        }                
        if (ViewState["DocumentPath1"] != null)
        {
            ViewState["DocumentPath1"] = null;
        }
        if (ComboBoxLOBSearch.SelectedIndex != -1)
        {
            FunPriLoadRegion();
            ComboBoxLocationSearch.Enabled = false;
            FunPriLoadLocation();
        }        
        if (ComboBoxBranchSearch.Enabled == false)
        {
            ComboBoxBranchSearch.Enabled = true;
            RFVComboBranch.Enabled = true;
        }
    }
    protected void ComboBoxBranchSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ComboBoxLocationSearch.Enabled = true;
        if (ComboBoxBranchSearch.SelectedIndex > 0)
        {
            FunPriLoadBranch();
        }
        else
        {
            ComboBoxLocationSearch.Enabled = false;
            FunPriLoadLocation();
        }
        if (pnlPDCDetails.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ViewState["DocumentPath1"] != null)
        {
            ViewState["DocumentPath1"] = null;
        }
    }
    protected void ComboBoxLocationSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pnlPDCDetails.Visible == true)
        {
            FunPriValidateGrid();
        }
        if (ViewState["DocumentPath1"] != null)
        {
            ViewState["DocumentPath1"] = null;
        }
    }
    private string SetCustomerAddress(DataRow drCust)
    {
        try
        {
            string strAddress = "";
            if (!string.IsNullOrEmpty(drCust["CustomerName"].ToString()))
                strAddress += drCust["CustomerName"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_Address1"].ToString()))
                strAddress += " \n" + drCust["Comm_Address1"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_Address2"].ToString()))
                strAddress += " \n" + drCust["Comm_Address2"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_City"].ToString()))
                strAddress += " \n" + drCust["Comm_City"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_State"].ToString()))
                strAddress += " , " + drCust["Comm_State"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_Country"].ToString()))
                strAddress += " \n" + drCust["Comm_Country"].ToString();
            if (!string.IsNullOrEmpty(drCust["Comm_Pincode"].ToString()))
                strAddress += " - " + drCust["Comm_Pincode"].ToString();
            if (!string.IsNullOrEmpty(drCust["Mobile"].ToString()))
                strAddress += " \n" + "Mobile  :  " + drCust["Mobile"].ToString();
            if (!string.IsNullOrEmpty(drCust["E-Mail"].ToString()))
                strAddress += "  " + "Email  :  " + drCust["E-Mail"].ToString();
            return strAddress;
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;            
        }
    }
    private void FunPriClearSession()
    {
        Session.Remove("PDCReminderGridDetails");
        Session.Remove("PDCReminderHeaderDetails");
        Session.Remove("sessionGridDetails");
        Session.Remove("PDCReminderAssets");
    }

    private void FunPubGetPDCDocumentPath()
    {        
        objserclient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbyte;
            List<ClsPubPDCDocumentPathDetails> objPdcDocumentPath = new List<ClsPubPDCDocumentPathDetails>();
            objbyte = objserclient.FunPubGetPDCDocPathDetails(intCompanyID, Convert.ToInt32(ComboBoxLOBSearch.SelectedValue), intProgramID);
            objPdcDocumentPath = (List<ClsPubPDCDocumentPathDetails>)DeSerialize(objbyte);
            if (objPdcDocumentPath.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Document Path not defined");
                txtFilePath.Text = string.Empty;
            }
            else
            {
                ViewState["DocumentPath"] = objPdcDocumentPath[0].DocumentPath;
                txtFilePath.Text = Convert.ToString(ViewState["DocumentPath"]);
            }
        }

        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            objserclient.Close();
        }
    }
    public void FunPubGeneratePDF()
    {       
        try
        {
            if (ViewState["DocumentPath"] != null)
            {
                if (FunPriSelectUnCheckedRows() > 0)
                {
                    List<ClsPubRptPDCReminderGridDetails> griddetails = (List<ClsPubRptPDCReminderGridDetails>)Session["sessionGridDetails"];
                    string path = ViewState["DocumentPath"].ToString();
                    foreach (ClsPubRptPDCReminderGridDetails items in griddetails)
                    {
                        FunPubGenerate(path, items);
                    }
                    Utility.FunShowAlertMsg(this, "File(s) Generated Successfully");
                    //txtFilePath.Text = path;
                    btnEMail.Enabled = true;
                    ViewState["DocumentPath"] = null;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Cannot Generate PDF. All accounts are Excluded");
                }
            }
            else
            {
                Utility.FunShowAlertMsg(this, "Document Path not defined");
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }        
    }
    public void FunPubGenerate(string path,ClsPubRptPDCReminderGridDetails objgriddetails)
    {
        List<ClsPubPDCReminderAssetDetails> AssetDetails = (List<ClsPubPDCReminderAssetDetails>)Session["PDCReminderAssets"];        
        ReportDocument rptd = new ReportDocument();
        string filePath = "";
        string strPath = path+"\\"+"PDC Reminder";
        try
        {
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            strPath = strPath + "\\" + ComboBoxLOBSearch.SelectedItem.Text;            
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            //strPath = strPath + "\\" + ComboBoxBranchSearch.SelectedItem.Text.Replace("|", ""); 
            //if (!Directory.Exists(strPath))
            //{
            //    Directory.CreateDirectory(strPath);
            //}
            strPath = strPath + "\\" + ComboBoxLocationSearch.SelectedItem.Text.Replace("|","");
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            strPath = strPath + "\\" + txtStartDateSearch.Text+"-" + txtEndDateSearch.Text;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            ViewState["DocumentPath1"]=strPath;
            strPath = strPath+"\\" + objgriddetails.CUSTOMER_NAME;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }
            if (objgriddetails.SUBACCOUNTNO == string.Empty || objgriddetails.SUBACCOUNTNO == null)
            {
                filePath = strPath + "\\" + objgriddetails.CUSTOMER_NAME + objgriddetails.PRIMEACCOUNTNO.Replace("/", "") + ".pdf";
            }
            else
            {
                filePath = strPath + "\\" + objgriddetails.CUSTOMER_NAME + objgriddetails.SUBACCOUNTNO.Replace("/", "") + ".pdf";
            }
            List<ClsPubRptPDCReminderGridDetails> items = new List<ClsPubRptPDCReminderGridDetails>();
            items.Add(objgriddetails);
            rptd.Load(Server.MapPath(@"Report\PDCReminderFormat.rpt"));
            rptd.SetDataSource(items);
            rptd.Subreports["PDCReminderAssetDetails.rpt"].SetDataSource(AssetDetails);            
            rptd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);            
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            rptd.Close();
            rptd.Dispose();
        }
    }
    protected void btnEMail_Click(object sender, EventArgs e)
    {
       // StringBuilder  strCustomerName=new StringBuilder();
        try
        {            
            List<ClsPubCustomerList> objMail = (List<ClsPubCustomerList>)ViewState["CustomerMail"];           
            StringBuilder strBody = new StringBuilder();
            strBody.Append("PDC Reminder created sucessfully");
            string strFolderPath = Convert.ToString(ViewState["DocumentPath1"]);
            if (strFolderPath == "")
            {
                Utility.FunShowAlertMsg(this, "unable to send EMail. Generate PDF");
                return;
            }
            if (Directory.Exists(strFolderPath))
            {
                string[] arrInnerDirectories = Directory.GetDirectories(strFolderPath);
                for (int i = 0; i < arrInnerDirectories.Length; i++)
                 {
                    Dictionary<string, string> dictMail = new Dictionary<string, string>();
                    string CustomerName = arrInnerDirectories[i];
                    if (CustomerName.Contains(objMail[i].CustomerName))
                    {
                        dictMail.Add("FromMail", ViewState["FromMail"].ToString());
                        dictMail.Add("Subject", "PDC REMINDER");
                        dictMail.Add("ToCC", "");
                        dictMail.Add("ToBCC", "");
                        dictMail.Add("ToMail", objMail[i].CustomerMail);
                    }                   
                    string strInnerDirectoryPath = arrInnerDirectories[i].ToString();
                    string[] arrCustomerFiles = Directory.GetFiles(strInnerDirectoryPath);
                    if (arrCustomerFiles.Length != 0)
                    {
                        ArrayList arrMailAttachement = new ArrayList();
                        for (int j = 0; j < arrCustomerFiles.Length; j++)
                        {
                            arrMailAttachement.Add(arrCustomerFiles[j]);
                        }
                        Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
                    }
                    dictMail.Clear();
                }                
                Utility.FunShowAlertMsg(this, "Mail has been sent successfully");             
            }
            else
            {
                Utility.FunShowAlertMsg(this, "EMail not sent. Generate PDF");
            }
        }
        catch(Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            cvPDCReminder.IsValid = false;
            cvPDCReminder.ErrorMessage = ae.Message.ToString();
        }
    }
   
    private void FunPriValidateGrid()
    {
        grvPDCDetails.DataSource = null;
        grvPDCDetails.DataBind();
        pnlPDCDetails.Visible = false;
        //lblCurrency.Visible = false;
        BtnPrint.Visible = false;
        btnGeneratePDF.Visible = false;
        btnEMail.Visible = false;
    }
    #endregion
}
