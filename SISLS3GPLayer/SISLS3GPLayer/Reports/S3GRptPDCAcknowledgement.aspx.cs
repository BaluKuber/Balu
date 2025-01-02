#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   PDC Acknowledgement Report
/// Created By          :   Sangeetha R
/// Created Date        :   24-Mar-2011
/// Purpose             :   To Generate Acknowledgement for a PDC.
/// Last Updated By		:   
/// Last Updated Date   :   
/// Reason              :  
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using S3GBusEntity;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Resources;
using System.Xml.Serialization;
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using S3GBusEntity.Reports;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

#endregion

public partial class Reports_S3GRptPDCAcknowledgement : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int CompanyId;
    string CustomerId;
    int UserId;
    int Lob_Id;
    int BranchId;
    int ProgramId;
    int LocationId1;
    string PANum;
    string SANum;
    string PDC_NO;
    string PDC_Date;
    bool Is_Active;
    int Active;
    string Type;
    Dictionary<string, string> Procparam;
    public string strFromEmail;
    public string strDateFormat;
    string strPageName = "PDC Acknowledgement";
    DataTable dtTable = new DataTable();
    ReportOrgColMgtServicesClient objSerClient;
    ReportAccountsMgtServicesClient objAccMgtSerClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServices;
    #endregion

    #region Page Load
    // <summary>
    /// This event is handled for load the page
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjUserInfo = new UserInfo();
            Session["Companyname"] = ObjUserInfo.ProCompanyNameRW;
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load PDC Acknowledgement Page.";
            CVPDCAcknowledgement.IsValid = false;
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
            objSerClient = new ReportOrgColMgtServicesClient();
            txtPDCDate.Attributes.Add("ReadOnly", "ReadOnly");

            ObjUserInfo = new UserInfo();
            Session["Companyname"] = ObjUserInfo.ProCompanyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            UserId = ObjUserInfo.ProUserIdRW;
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;    
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            ProgramId = 146;
            Session["Date"] = DateTime.Now.ToString(strDateFormat);
            if (!IsPostBack)
            {
                ViewState["CompanyAddr"] = GetCompanyAddressDetails(CompanyId);
                ClearSession();
                ddllocation2.Enabled = false;
            }

            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.ToolTip = "Customer Name";
            Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
            btnGetLOV.Focus();
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtName.Attributes.Add("ReadOnly", "ReadOnly");
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load PDC Acknowledgement page");
        }
    }

    /// <summary>
    /// To Get the LOB based on the Customer
    /// </summary>
    /// <param name="CompanyId"></param>
    /// <param name="UserId"></param>
    /// <param name="ProgramId"></param>
    /// <param name="Customer_Id"></param>
    private void FunPriGetLob(int CompanyId, int UserId, int ProgramId, string Customer_Id)
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            ddlLOB.Items.Clear();
            byte[] byteLobs = objSerClient.FunPubGetPDCLOB(CompanyId, UserId, ProgramId, Customer_Id);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            //ddlLOB.Items[0].Text = "All";
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

    /// <summary>
    /// To Get the Branch based on LOB and Customer
    /// </summary>
    /// <param name="CompanyId"></param>
    /// <param name="UserId"></param>
    /// <param name="ProgramId"></param>
    /// <param name="Customer_Id"></param>
    /// <param name="Lob_Id"></param>
    private void FunPriGetBranch(int CompanyId, int UserId, int ProgramId, string Customer_Id, int Lob_Id)
    {
        try
        {
            objAccMgtSerClient = new ReportAccountsMgtServicesClient();
            ddlBranch.Items.Clear();
            if (ddlLOB.SelectedIndex > 0)
            {
                Lob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            }

            byte[] byteLobs = objAccMgtSerClient.FunPubGetBranch(CompanyId, UserId, ProgramId, Customer_Id, Lob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--All--";
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.SelectedIndex = 1;
            }
            else
                ddlBranch.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccMgtSerClient.Close();
        }
    }

    /// <summary>
    /// To Load Location 2
    /// </summary>
    /// <param name="ProgramId"></param>
    /// <param name="UserId"></param>
    /// <param name="CompanyId"></param>
    /// <param name="LobId"></param>
    /// <param name="LocationId"></param>
    private void FunPriLoadLocation2(int ProgramId, int UserId, int CompanyId, int Lob_Id, int LocationId1)
    {
        try
        {
            objAccMgtSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                Lob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            }

            if (ddlBranch.SelectedIndex > 0)
            {
                LocationId1 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            byte[] byteLobs = objAccMgtSerClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, Lob_Id, LocationId1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddllocation2.DataSource = Branch;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            ddllocation2.Items[0].Text = "--All--";
            if (ddllocation2.Items.Count == 2)
            {
                ddllocation2.SelectedIndex = 1;
            }
            else
                ddllocation2.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objAccMgtSerClient.Close();
        }
    }

    /// <summary>
    /// To Load PDC Prime Account Number
    /// </summary>
    private void FunPriLoadPrimeAccount()
    {
        try
        {
            objSerClient = new ReportOrgColMgtServicesClient();
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            ddlPNum.Items.Clear();
            ClsPubStockReceivableparameters PDCPAN = new ClsPubStockReceivableparameters();
            PDCPAN.CompanyId = CompanyId;
            PDCPAN.UserId = UserId;
            PDCPAN.CustomerId = hdnCustomerId.Value;
            if (ddlLOB.SelectedIndex > 0)
            {
                PDCPAN.LobId = ddlLOB.SelectedValue;
            }
            if (ddlBranch.SelectedIndex > 0)
            {
                PDCPAN.LocationId1 = ddlBranch.SelectedValue;
            }
            if (ddllocation2.SelectedIndex > 0)
            {
                PDCPAN.LocationId2 = ddllocation2.SelectedValue;
            }
            byte[] byteLobs = objSerClient.FunPubPDCAckPAN(PDCPAN);
            List<ClsPubDropDownList> PANs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPNum.DataSource = PANs;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            if (ddlPNum.Items.Count == 2)
            {
                ddlPNum.SelectedIndex = 1;
                FunPriLoadSANPDC();

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
    /// This Method is called after Clicking the GO button.
    /// To Load the PDC Details in Grid.
    /// </summary>
    /// <param name="PDC_NO"></param>
    /// <param name="CompanyId"></param>
    private void FunBindgrid()
    {
        try
        {
            pnlPdcdetails.Visible = true;
            byte[] byteLobs = objSerClient.FunPubGetPDCDetails(ddlPDCNo.SelectedValue, CompanyId);
            List<ClsPubPDCDetails> PDC = (List<ClsPubPDCDetails>)DeSeriliaze(byteLobs);

            Session["PDCAcknow"] = PDC;
            grvPdcdetails.DataSource = PDC;
            grvPdcdetails.DataBind();

            if (grvPdcdetails.Rows.Count != 0)
            {
                grvPdcdetails.HeaderRow.Style.Add("position", "relative");
                grvPdcdetails.HeaderRow.Style.Add("z-index", "auto");
                grvPdcdetails.HeaderRow.Style.Add("top", "auto");
            }

            if (grvPdcdetails.Rows.Count == 0)
            {
                pnlPdcdetails.Visible = false;
                grvPdcdetails.EmptyDataText = "No PDC Details Found";
                grvPdcdetails.DataBind();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
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

    /// <summary>
    /// To set the Suffix to Amount
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
    /// This Method is called after Clicking the Clear Button
    /// </summary>
    private void FunPriClearPDC()
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            lblAmounts.Visible = false;
            lblCurrency.Visible = false;
            //btnGo.Enabled = false;
            ddlLOB.Items.Clear();
            //FunPriGetLob(CompanyId, UserId, true, hdnCustomerId.Value);
            ddlBranch.Items.Clear();
            ddllocation2.Items.Clear();
            ddllocation2.Enabled = false;
            if (ddlPNum.Items.Count > 0)
            {
                ddlPNum.Items.Clear();
            }
            if (ddlSNum.Items.Count > 0)
            {
                ddlSNum.Items.Clear();
            }
            if (ddlPDCNo.Items.Count > 0)
            {
                ddlPDCNo.Items.Clear();
            }
            ucCustomerCodeLov.FunPubClearControlValue();
            FunPriValidateGrid();
            txtCustomerName.Text = "";
            ucCustomerCodeLov.ReRegisterSearchControl("CMD");
            txtPDCDate.Text = string.Empty;
            txtDOCPath.Text = string.Empty;
            btnPrint.Visible = false;
            btnSave.Visible = false;
            BtnEMail.Visible = false;
            BtnEMail.Enabled = false;
            ClearSession();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Validate the Grid
    /// </summary>
    private void FunPriValidateGrid()
    {
        btnPrint.Visible = false;
        BtnEMail.Visible = false;
        btnSave.Visible = false;
        lblAmounts.Visible = false;
        lblCurrency.Visible = false;
        pnlPdcdetails.Visible = false;
        grvPdcdetails.DataSource = null;
        grvPdcdetails.DataBind();
    }

    /// <summary>
    /// To Clear the Session
    /// </summary>
    private void ClearSession()
    {
        Session["PDCHeader"] = null;
        Session["PDCAcknow"] = null;
    }

    /// <summary>
    /// To Get the Document Path
    /// </summary>
    private void FunPubGetPDCDocumentPath()
    {
        objSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbyte;
            List<ClsPubPDCDocumentPathDetails> objPdcDocumentPath = new List<ClsPubPDCDocumentPathDetails>();
            objbyte = objSerClient.FunPubGetPDCDocPathDetails(CompanyId, Convert.ToInt32(ddlLOB.SelectedValue), 146);
            objPdcDocumentPath = (List<ClsPubPDCDocumentPathDetails>)DeSeriliaze(objbyte);
            ViewState["DocumentPath"] = null;
            if (objPdcDocumentPath.Count == 0)
            {
                Utility.FunShowAlertMsg(this, "Document Path not defined");
            }
            else
            {
                if (Directory.Exists(objPdcDocumentPath[0].DocumentPath))
                {
                    ViewState["DocumentPath"] = objPdcDocumentPath[0].DocumentPath;
                }
                else
                {
                    Utility.FunShowAlertMsg(this, "Directory not found");
                    ViewState["DocumentPath"] = "0";
                }
            }
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
    /// To Build the Header Details
    /// </summary>
    /// <param name="CustomerID"></param>
    private void BuildHeaderDetails(string CustomerID)
    {
        ClsPubHeaderDetails headerDetails = new ClsPubHeaderDetails();
        headerDetails.CompanyName = ObjUserInfo.ProCompanyNameRW;
        headerDetails.CompanyAddress = ViewState["CompanyAddr"].ToString();
        headerDetails.CustomerName = ((TextBox)ucCustomerCodeLov.FindControl("txtName")).Text;

        DataTable dtCustomer = new DataTable();
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Option", "56");
        Procparam.Add("@Param1", CustomerID);
        dtCustomer = Utility.GetDefaultData("S3G_ORG_GetCustomerLookUp", Procparam);
        DataRow drow = dtCustomer.Rows[0];
        ViewState["CustomerEMail"] = dtCustomer.Rows[0][9].ToString();
        headerDetails.CustomerAddress = SetCustomerAddress(drow);
        headerDetails.NoOf = grvPdcdetails.Rows.Count.ToString();
        if (ddlPNum.SelectedIndex == 0 || ddlPNum.SelectedIndex == -1)
        {
            headerDetails.PANum = "";
        }
        else
        {
            headerDetails.PANum = ddlPNum.SelectedItem.Text;
        }

        if (ddlSNum.SelectedIndex == 0 || ddlSNum.SelectedIndex == -1)
        {
            headerDetails.SANum = "";
        }
        else
        {
            headerDetails.SANum = ddlSNum.SelectedItem.Text;
        }
        Session["PDCHeader"] = headerDetails;
    }

    /// <summary>
    /// To Generate the PDF
    /// </summary>
    /// <param name="path"></param>
    private void GeneratePDF()
    {
        //ReportDocument Rptd = new ReportDocument();
        string path = ViewState["DocumentPath"].ToString();
        if (Session["PDCAcknow"] != null)
        {
            ReportDocument Rptd = new ReportDocument();
            Rptd.Load(Server.MapPath("Report/PDCAcknowledgementRPT.rpt"));

            TextObject Companyname = (TextObject)Rptd.ReportDefinition.Sections["Section3"].ReportObjects["txtCompanyname"];
            Companyname.Text = Session["Companyname"].ToString();

            TextObject Currency = (TextObject)Rptd.ReportDefinition.Sections["Section2"].ReportObjects["txtcurrency"];
            Currency.Text = "[All Amounts are in" + " " + Session["Currency"].ToString() + "]";

            TextObject Date = (TextObject)Rptd.Subreports["HeaderDetails"].ReportDefinition.Sections["ReportHeaderSection2"].ReportObjects["txtdate"];
            Date.Text = Session["Date"].ToString();

            Rptd.Load(Server.MapPath("Report/PDCAcknowledgementRPT.rpt"));
            Rptd.SetDataSource(GetPDCDetails());
            Rptd.Subreports[0].SetDataSource(GetHeaderDetails());
            Rptd.ExportToDisk(ExportFormatType.PortableDocFormat, path + "\\PDC Acknowledgement.pdf");
            Rptd.Close();
        }
    }

    /// <summary>
    /// To Get the Company Address Details
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
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

    /// <summary>
    /// To Load the PDC Details in the PDC Report Screen
    /// </summary>
    /// <returns></returns>
    private List<ClsPubPDCDetails> GetPDCDetails()
    {
        List<ClsPubPDCDetails> PDCDetails;

        if (Session["PDCAcknow"] == null)
        {
            PDCDetails = new List<ClsPubPDCDetails>();
        }
        else
        {
            PDCDetails = (List<ClsPubPDCDetails>)Session["PDCAcknow"];
        }
        return PDCDetails;
    }

    /// <summary>
    /// To Get the Header Details in the PDC Report Screen
    /// </summary>
    /// <returns></returns>
    private List<ClsPubHeaderDetails> GetHeaderDetails()
    {
        List<ClsPubHeaderDetails> PDCHeader = new List<ClsPubHeaderDetails>();
        if (Session["PDCHeader"] != null)
        {
            ClsPubHeaderDetails PDCHead = (ClsPubHeaderDetails)Session["PDCHeader"];
            PDCHeader.Add(PDCHead);
        }
        return PDCHeader;
    }

    #endregion

    #region Page Events

    #region DropdownList Events
    /// <summary>
    /// To Load the Prime Account Number and PDC Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            FunPriGetBranch(CompanyId, UserId, ProgramId, hdnCustomerId.Value, Lob_Id);
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, Convert.ToInt32(ddlLOB.SelectedValue), LocationId1);
            //ddllocation2.Items.Clear();
            ddlPDCNo.Items.Clear();
            FunPriLoadPrimeAccount();
            FunPriValidateGrid();
            txtPDCDate.Text = "";
            txtDOCPath.Text = "";
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVPDCAcknowledgement.IsValid = false;
        }
    }
    /// <summary>
    /// To Load the Prime Account Number and PDC Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            ddlPDCNo.Items.Clear();
            txtDOCPath.Text = txtPDCDate.Text = "";
            FunPriValidateGrid();

            if (ddlBranch.SelectedValue == "-1")
            {
                FunPriLoadLocation2(ProgramId, UserId, CompanyId, Convert.ToInt32(ddlLOB.SelectedValue), LocationId1);
                ddllocation2.Enabled = false;
            }
            else
            {
                ddllocation2.Enabled = true;
                FunPriLoadLocation2(ProgramId, UserId, CompanyId, Convert.ToInt32(ddlLOB.SelectedValue), LocationId1);
            }
            FunPriLoadPrimeAccount();
            

        }

        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVPDCAcknowledgement.IsValid = false;
        }

    }
    /// <summary>
    /// To Load Sub account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void ddlPNum_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlPNum.SelectedIndex > 0)
                FunPriLoadSANPDC();
            else
            {
                if (ddllocation2.Items.Count > 0)
                {
                    ddllocation2.SelectedIndex = 0;
                }
                else
                {
                    ddlBranch.SelectedIndex = 0;
                }
                if (ddlPNum.SelectedIndex == 0)
                {
                    ddlLOB.SelectedIndex = 0;
                    ddlSNum.Items.Clear();
                    ddlPDCNo.Items.Clear();
                    txtPDCDate.Text = txtDOCPath.Text = "";
                    FunPriValidateGrid();
                }
            }
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Sub Account Number.";
            CVPDCAcknowledgement.IsValid = false;
        }
    }

    /// <summary>
    /// To Load the Sub Account and PDC number together based on Account Number
    /// </summary>
    private void FunPriLoadSANPDC()
    {
        try
        {
            byte[] byteLobss;
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            FunPriValidateGrid();
            btnPrint.Visible = false;
            btnSave.Visible = false;
            BtnEMail.Visible = false;
            objAccMgtSerClient = new ReportAccountsMgtServicesClient();

            objSerClient = new ReportOrgColMgtServicesClient();
            byteLobss = null;
            byteLobss = objSerClient.FunPubGetHeaderLobBranchDetails(ddlPNum.SelectedValue);
            ClsPubPDCDateLOBBranch PDCDateLOBBranch = (ClsPubPDCDateLOBBranch)DeSeriliaze(byteLobss);
            ddlLOB.SelectedIndex = ddlLOB.Items.IndexOf(ddlLOB.Items.FindByValue(PDCDateLOBBranch.LOBId.ToString()));
            if (ddllocation2.SelectedIndex == 0)
            {
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(PDCDateLOBBranch.BranchId.ToString()));
                ddllocation2.Items.Clear();
                ddllocation2.Enabled = false;
            }


            ddlSNum.Items.Clear();
            lblSNum.CssClass = "styleDisplayLabel";
            byte[] byteLobs = objSerClient.FunPubPDCAckSAN(CompanyId, ddlPNum.SelectedValue);
            List<ClsPubDropDownList> SANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlSNum.DataSource = SANum;
            ddlSNum.DataTextField = "Description";
            ddlSNum.DataValueField = "ID";
            ddlSNum.DataBind();
            if (ddlSNum.Items.Count == 2)
            {
                ddlSNum.SelectedIndex = 1;
                FunProLoadPDConSAN();
            }
            else
                ddlSNum.SelectedIndex = 0;


            ddlPDCNo.Items.Clear();
            txtPDCDate.Text = "";
            txtDOCPath.Text = "";
           // lblPDCNo.CssClass = "styleReqFieldLabel";
            objSerClient = new ReportOrgColMgtServicesClient();
            byteLobss = objSerClient.FunPubPDCNumber(ddlPNum.SelectedValue, string.Empty);
            List<ClsPubDropDownList> PDCNo = (List<ClsPubDropDownList>)DeSeriliaze(byteLobss);
            ddlPDCNo.DataSource = PDCNo;
            ddlPDCNo.DataTextField = "Description";
            ddlPDCNo.DataValueField = "ID";
            ddlPDCNo.DataBind();
            if (ddlPDCNo.Items.Count == 2)
            {
                ddlPDCNo.SelectedIndex = 1;
                FunProLoadPDConSAN();
            }
            else
                ddlPDCNo.SelectedIndex = 0;

            //objSerClient = new ReportOrgColMgtServicesClient();
            //byteLobss = null;
            //byteLobss = objSerClient.FunPubGetHeaderLobBranchDetails(ddlPNum.SelectedValue);
            //ClsPubPDCDateLOBBranch PDCDateLOBBranch = (ClsPubPDCDateLOBBranch)DeSeriliaze(byteLobss);
            //ddlLOB.SelectedIndex = ddlLOB.Items.IndexOf(ddlLOB.Items.FindByValue(PDCDateLOBBranch.LOBId.ToString()));
            //if (ddllocation2.SelectedIndex == 0)
            //{
            //    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(PDCDateLOBBranch.BranchId.ToString()));
            //    ddllocation2.Items.Clear();
            //    ddllocation2.Enabled = false;
            //}
            //ddllocation2.SelectedIndex=ddllocation2.Items.IndexOf(ddllocation2.Items.FindByValue(PDCDateLOBBranch
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
    /// To Load the Account Number based on Location2
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddllocation2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            FunPriValidateGrid();
            FunPriLoadPrimeAccount();
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVPDCAcknowledgement.IsValid = false;
        }
    }



    /// <summary>
    /// To Load the PDC Number based on Sub Account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPNum.SelectedIndex > 0)
        {
            if (ddlSNum.Items.Count > 1 && ddlSNum .SelectedIndex >0)
                FunProLoadPDConSAN();
            else
            {
                ddlPDCNo.Items.Clear();
                txtPDCDate.Text = txtDOCPath.Text = "";
            }
        }
        if (ddlSNum.SelectedIndex == 0)
        {
            ddlPDCNo.SelectedIndex = -1;
            txtPDCDate.Text = string.Empty;
            txtDOCPath.Text = string.Empty;
            FunPriValidateGrid();
        }
    }

    /// <summary>
    /// To Load the PDC Number based on Sub Account Number
    /// </summary>
    protected void FunProLoadPDConSAN()
    {
        ddlPDCNo.Items.Clear();
        //lblPDCNo.CssClass = "styleDisplayLabel";
        byte[] byteLobss = objSerClient.FunPubPDCNumber(ddlPNum.SelectedValue, ddlSNum.SelectedValue);
        List<ClsPubDropDownList> PDCNo = (List<ClsPubDropDownList>)DeSeriliaze(byteLobss);
        ddlPDCNo.DataSource = PDCNo;
        ddlPDCNo.DataTextField = "Description";
        ddlPDCNo.DataValueField = "ID";
        ddlPDCNo.DataBind();
        if (ddlPDCNo.Items.Count == 2)
        {
            ddlPDCNo.SelectedIndex = 1;
            FunLoadPDCDetailsonPDC();
        }
        else
            ddlPDCNo.SelectedIndex = 0;
    }

    /// <summary>
    /// To Load PDC Date,LOB and Branch after selecting the PDC Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPDCNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunLoadPDCDetailsonPDC();
    }

    /// <summary>
    /// To Load PDC Date,LOB and Branch after selecting the PDC Number
    /// </summary>
    protected void FunLoadPDCDetailsonPDC()
    {
        objS3GAdminServices = new S3GAdminServicesReference.S3GAdminServicesClient();
        try
        {
            FunPriValidateGrid();
            btnPrint.Visible = false;
            btnSave.Visible = false;
            BtnEMail.Visible = false;
            txtPDCDate.Text = "";
            //txtPDCDate.CssClass = "styleDisplayLabel";


            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@PDC_NO", ddlPDCNo.SelectedValue);

            txtPDCDate.Text = objS3GAdminServices.FunGetScalarValue(SPNames.GetPDCDate, Procparam).ToString();
            if (ddlPDCNo.SelectedValue == "0")
            {
                txtPDCDate.Text = string.Empty;
                txtDOCPath.Text = string.Empty;
                FunPriValidateGrid();
            }
            if (ddlPDCNo.Items.Count > 0 && ddlPDCNo.SelectedValue != "-1")
            {
                //btnGo.Enabled = true;
                FunPubGetPDCDocumentPath();
                txtDOCPath.Text = ViewState["DocumentPath"].ToString();
            }
            else
            {
                //btnGo.Enabled = false;
                txtDOCPath.Text = "";
            }

        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load PDC Date.";
            CVPDCAcknowledgement.IsValid = false;
        }
        finally
        {
            objS3GAdminServices.Close();
        }
    }


    #endregion

    #region Button (Customer / Go / Clear / Print)

    /// <summary>
    /// To Load the Prime Account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtCustomerName.Text = txtName.Text;

            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            if (ddlLOB.SelectedIndex <= 0)
            {
                FunPriGetLob(CompanyId, UserId, ProgramId, hdnCustomerId.Value);
            }
            else
            {
                ddlLOB.ClearSelection();
                FunPriGetLob(CompanyId, UserId, ProgramId, hdnCustomerId.Value);
            }
            ddlPDCNo.Items.Clear();
            FunPriGetBranch(CompanyId, UserId, ProgramId, hdnCustomerId.Value, Lob_Id);
            FunPriLoadLocation2(ProgramId, UserId, CompanyId, Lob_Id, LocationId1);
            ddllocation2.Enabled = false;
            FunPriLoadPrimeAccount();

            txtPDCDate.Text = "";
            txtDOCPath.Text = "";

            btnSave.Visible = false;
            btnPrint.Visible = false;
            BtnEMail.Visible = false;

            FunPriValidateGrid();

            if (ddlLOB.Items.Count <= 1)
            {
                Utility.FunShowAlertMsg(this.Page, "User doesn’t have access to appropriate Line of Business");
                FunPriClearPDC();
                return;
            }
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Customer Name";
            CVPDCAcknowledgement.IsValid = false;
        }
    }

    /// <summary>
    /// To bind the PDC Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            FunBindgrid();
            

            ClsPubHeaderDetails objPDCHeader = new ClsPubHeaderDetails();
            if (ddlLOB.Items.Count > 0 && ddlLOB.SelectedValue != "-1")
            {
                objPDCHeader.Lob = ddlLOB.SelectedItem.ToString();
            }
            else
            {
                objPDCHeader.Lob = "";
            }
            if (ddlBranch.Items.Count > 0 && ddlBranch.SelectedValue != "-1")
            {
                objPDCHeader.Branch = ddlBranch.SelectedItem.ToString();
            }
            else
            {
                objPDCHeader.Branch = "";
            }
            if (ddlPNum.Items.Count > 0 && ddlPNum.SelectedValue != "-1")
            {
                objPDCHeader.PANum = ddlPNum.SelectedItem.ToString();
            }
            else
            {
                objPDCHeader.PANum = "";
            }
            if (ddlSNum.Items.Count > 1)
            {
                objPDCHeader.SANum = ddlSNum.SelectedItem.ToString();
            }
            else
            {
                objPDCHeader.SANum = "";
            }
            objPDCHeader.PDCNo = ddlPDCNo.SelectedItem.ToString();
            objPDCHeader.PDCDate = txtPDCDate.Text.ToString();
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");

            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");

            objPDCHeader.Customer = txtName.Text;
            Session["PDCHeader"] = objPDCHeader;

            BuildHeaderDetails(hdnCustomerId.Value);
            Response.Write(grvPdcdetails.Rows.Count.ToString());
            if (grvPdcdetails.Rows.Count > 0)
            {
                lblAmounts.Visible = true;
                lblCurrency.Visible = true;
                lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW;
                grvPdcdetails.Visible = true;
                btnPrint.Visible = true;
                btnSave.Visible = true;
                BtnEMail.Visible = true;
            }
            else
            {
                FunPriValidateGrid();
            }
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load PDC Details Grid.";
            CVPDCAcknowledgement.IsValid = false;
        }
    }

    /// <summary>
    /// To clear the fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPDC();
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Error in Clearing";
            CVPDCAcknowledgement.IsValid = false;
        }
    }

    /// <summary>
    /// To Save the PDC Document as PDF
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            GeneratePDF();
            Utility.FunShowAlertMsg(this, "The PDC Acknowledgement generated has been Saved");
            BtnEMail.Enabled = true;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

    }

    /// <summary>
    /// To Print the PDC Acknowledgement Document
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    
    {
        string strScipt = "window.open('../Reports/S3GRptPDCAcknowledgementReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PDCAcknow", strScipt, true);
    }

    /// <summary>
    /// To EMail the Generated PDF
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnEMail_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> dictMail = new Dictionary<string, string>();
        dictMail.Add("FromMail", ViewState["FromMail"].ToString());
        string name = ViewState["CustomerEMail"].ToString();
        dictMail.Add("ToMail", name);
        dictMail.Add("Subject", "PDC ACKNOWLEDGEMENT");
        dictMail.Add("ToCC", "");
        dictMail.Add("ToBCC", "");
        StringBuilder strBody = new StringBuilder();
        strBody.Append("PDC Acknowledgement created sucessfully");
        string strFilePath = txtDOCPath.Text;
        strFilePath = strFilePath + "\\";
        string[] arrCustomerFiles = Directory.GetFiles(strFilePath);
        if (arrCustomerFiles.Length != 0)
        {
            ArrayList arrMailAttachement = new ArrayList();
            for (int j = 0; j < arrCustomerFiles.Length; j++)
            {
                arrMailAttachement.Add(arrCustomerFiles[j]);
            }
            Utility.FunPubSentMail(dictMail, arrMailAttachement, strBody);
        }
        Utility.FunShowAlertMsg(this, "Mail has been sent successfully");
    }
    #endregion

    /// <summary>
    /// To Set the Customer Address
    /// </summary>
    /// <param name="drCust"></param>
    /// <returns></returns>
    public static string SetCustomerAddress(DataRow drCust)
    {
        string strAddress = "";
        if (!string.IsNullOrEmpty(drCust["Comm_Address1"].ToString()))
            strAddress += drCust["Comm_Address1"].ToString() + System.Environment.NewLine;
        if (!string.IsNullOrEmpty(drCust["Comm_Address2"].ToString())) 
            strAddress += drCust["Comm_Address2"].ToString() + System.Environment.NewLine;
        if (!string.IsNullOrEmpty(drCust["Comm_City"].ToString())) 
            strAddress += drCust["Comm_City"].ToString() + " , ";
        if (!string.IsNullOrEmpty(drCust["Comm_State"].ToString())) 
            strAddress += drCust["Comm_State"].ToString() + System.Environment.NewLine;
        if (!string.IsNullOrEmpty(drCust["Comm_Country"].ToString())) 
            strAddress += drCust["Comm_Country"].ToString() + "-";
        if (!string.IsNullOrEmpty(drCust["Comm_Pincode"].ToString()))
            strAddress += drCust["Comm_Pincode"].ToString() + System.Environment.NewLine;
        strAddress += "Mobile  : ";
        if (!string.IsNullOrEmpty(drCust["Comm_Mobile"].ToString()))
            strAddress += drCust["Comm_Mobile"].ToString();
        else
            strAddress += "\t\t\t\t";
        strAddress += "\tEmail  :  ";
        if (!string.IsNullOrEmpty(drCust["Comm_EMail"].ToString())) 
            strAddress +=  drCust["Comm_EMail"].ToString();
        return strAddress;
    }
    #endregion
}