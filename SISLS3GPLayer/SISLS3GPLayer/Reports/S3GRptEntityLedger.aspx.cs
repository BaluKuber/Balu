#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Reports
/// Screen Name         :   Vendor Details Report
/// Created By          :   Saranya I
/// Created Date        :   02-Aug-2011
/// Purpose             :   To Get the Entity Details.
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

public partial class Reports_S3GRptEntityLedger : ApplyThemeForProject
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
    decimal TotalReceipts;
    public string strDateFormat;
    Dictionary<string, string> Procparam=new Dictionary<string,string>();
    string strPageName = "Entity Ledger";
    DataTable dtTable = new DataTable();
    decimal OpeningBalance;
    ReportAccountsMgtServicesClient objSerClient;
    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Due to Data Problem, Unable to Load Vendor Details Page.";
            CVVendorDetails.IsValid = false;
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

            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;

            if (!IsPostBack)
            {
                FunPriLoadEntityName();
                FunPubLoadDenomination();
            }

            ddlEntityName.AddItemToolTip();
            if(ddlEntityName.SelectedIndex>0)
            ddlEntityName.ToolTip = ddlEntityName.SelectedItem.Text;
            ddlDenomination.AddItemToolTip();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// This Method is Used to Load Entity Names
    /// </summary>
    private void FunPriLoadEntityName()
    {
        try
        {

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ddlEntityName.BindDataTable("S3G_RPT_GetEntityName", Procparam, new string[] { "ENTITY_CODE", "ENTITY" });


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
       
    }

    /// <summary>
    /// To Load Denomination
    /// </summary>
    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
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
    /// <summary>
    /// To Validate From and To Date
    /// To Bind Grid
    /// </summary>
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
                    //changeds Validation Msg for Bug Id 5265
                    Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                    txtEndDate.Text = "";
                    // added on 11/11/2011 for Bug Id 5265
                    pnlVendorDetails.Visible = false;
                    grvVendorDetails.DataSource = null;
                    grvVendorDetails.DataBind();
                    btnPrint.Visible = false;
                    lblAmounts.Visible = false;
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

            lblAmounts.Visible = true;

            if (ddlDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }

            #region To Get Header Details for Report
            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            objHeader.EntityName = ddlEntityName.SelectedItem.Text;
            objHeader.StartDate = txtStartDate.Text;
            objHeader.EndDate = txtEndDate.Text;
            Session["Header"] = objHeader;

            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are In" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            #endregion

            // To Bind The Vendor Details
            FunPriLoadVendorDetails();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    /// <summary>
    /// This Method is Used to Clear the Values
    /// </summary>
    private void FunPriClearVendorDetails()
    {
        try
        {
            ddlEntityName.ClearSelection();
            ddlEntityName.ToolTip = "Entity Name";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            lblError.Text = "";
            btnPrint.Visible = false;
            ddlDenomination.ClearSelection();
            lblAmounts.Visible = false;
            FunPriValidateGrid();
            
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }
    /// <summary>
    /// This Method is used to Clear the Grid
    /// </summary>
    private void FunPriValidateGrid()
    {

        pnlVendorDetails.Visible = false;
        grvVendorDetails.DataSource = null;
        grvVendorDetails.DataBind();
        btnPrint.Visible = false;
        lblAmounts.Visible = false;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlDenomination.ClearSelection();
    }
    /// <summary>
    /// This Method is used to Bind the Vendor Details Grid. Called in Go click Event
    /// </summary>
    private void FunPriLoadVendorDetails()
    {
        try
        {
            btnPrint.Visible = true;
            btnPrint.Enabled = true;
            pnlVendorDetails.Visible = true;
            lblError.Text = "";
            divVendorDetails.Style.Add("display", "block");

            objSerClient = new ReportAccountsMgtServicesClient();

            byte[] byteLobs = objSerClient.FunPubGetVendorDetails(out OpeningBalance, intCompanyId, ddlEntityName.SelectedValue, Utility.StringToDate(txtStartDate.Text).ToShortDateString(), Utility.StringToDate(txtEndDate.Text).ToShortDateString(),Convert.ToDecimal(ddlDenomination.SelectedValue));
            List<ClsPubTransaction> VendorDetails = (List<ClsPubTransaction>)DeSeriliaze(byteLobs);
            TotalDues = VendorDetails.Sum(ClsPubTransaction => ClsPubTransaction.Dues);
            TotalReceipts = VendorDetails.Sum(ClsPubTransaction => ClsPubTransaction.Receipts);

            lblOpeningBalance.Text = GetOpeningBalance(OpeningBalance);
            Session["OpeningBalance"] = lblOpeningBalance.Text;
            lblOpeningBalance.Visible = true;
            Session["Vendor"] = VendorDetails;
            grvVendorDetails.DataSource = VendorDetails;
            grvVendorDetails.DataBind();
            if (grvVendorDetails.Rows.Count != 0)
            {
                grvVendorDetails.HeaderRow.Style.Add("position", "relative");
                grvVendorDetails.HeaderRow.Style.Add("z-index", "auto");
                grvVendorDetails.HeaderRow.Style.Add("top", "auto");

            }
            if (grvVendorDetails.Rows.Count == 0)
            {
                Session["Vendor"] = null;
                btnPrint.Enabled = false;
                //lblOpeningBalance.Visible = false;
                lblError.Text = "No Vendor Details Found";
                grvVendorDetails.DataBind();
            }
            else
            {
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

    /// <summary>
    /// To assign the total in Footer Row
    /// </summary>
    private void FunPriDisplayTotal()
    {
        ((Label)grvVendorDetails.FooterRow.FindControl("lblTotalDues")).Text = TotalDues.ToString(FunSetSuffix());
        ((Label)grvVendorDetails.FooterRow.FindControl("lblTotalReceipts")).Text = TotalReceipts.ToString(FunSetSuffix());

        ((Label)grvVendorDetails.FooterRow.FindControl("lblTotalBalance")).Text = ((Label)grvVendorDetails.Rows[grvVendorDetails.Rows.Count - 1].FindControl("lblBalance")).Text;

        
    }
    /// <summary>
    /// To set GPS Suffix for Opening Balance
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    private string GetOpeningBalance(decimal val)
    {
        string OpeningBalance = string.Empty;
        if (val < 0)
        {

            OpeningBalance = "Opening Balance as on " + txtStartDate.Text + " is " + val.ToString(FunSetSuffix()).Replace("-", " ").TrimStart() + " " + "Cr.";
        }
        else
        {
            OpeningBalance = "Opening Balance as on " + txtStartDate.Text + " is " + val.ToString(FunSetSuffix()) + " "+"Dr.";

        }
        return OpeningBalance;
    }
    /// <summary>
    /// This Method is used to set GPS Suffix for Grid Footer row Values
    /// </summary>
    /// <returns></returns>
    private string FunSetSuffix()
    {
       
        int suffix = 1;
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }
    #endregion

    # region Page Events
    protected void ddlEntityName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();
        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Due to Data Problem, Unable to Load Grid.";
            CVVendorDetails.IsValid = false;
        }
    }
    #endregion

    #region Button (Customer / Ok / Clear / Print)

    /// <summary>
    /// To validate the From and To Date
    /// To Bind the Sanction Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateFromEndDate();
            
            #region To Get Entity Address for Report
            Session["EntityName"] = ddlEntityName.SelectedItem.Text;


            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            if (ddlEntityName.SelectedIndex > 0)
            {
                Procparam.Add("@ENTITY_CODE", ddlEntityName.SelectedValue);
            }
            DataTable dtEntitydtls = Utility.GetDefaultData("S3G_RPT_GetEntityAddress", Procparam);
            if (dtEntitydtls.Rows.Count > 0)
            {
                ClsPubCustomer ObjCustomer = new ClsPubCustomer();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["ADDRESS"].ToString()))
                    ObjCustomer.Address += dtEntitydtls.Rows[0]["ADDRESS"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["ADDRESS2"].ToString()))
                    ObjCustomer.Address += " \n"+dtEntitydtls.Rows[0]["ADDRESS2"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["CITY"].ToString()))
                    ObjCustomer.Address += " \n" + dtEntitydtls.Rows[0]["CITY"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["STATE"].ToString()))
                    ObjCustomer.Address += " , " + dtEntitydtls.Rows[0]["STATE"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["COUNTRY"].ToString()))
                    ObjCustomer.Address += " \n" + dtEntitydtls.Rows[0]["COUNTRY"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["PINCODE"].ToString()))
                    ObjCustomer.Address += " - " + dtEntitydtls.Rows[0]["PINCODE"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["MOBILE"].ToString()))
                    ObjCustomer.Address += " \n" +"Mobile  :  "+ dtEntitydtls.Rows[0]["MOBILE"].ToString();
                if (!string.IsNullOrEmpty(dtEntitydtls.Rows[0]["EMAIL"].ToString()))
                    ObjCustomer.Address += "  " + "Email  :  " + dtEntitydtls.Rows[0]["EMAIL"].ToString();
                //ObjCustomer.Mobile = dtEntitydtls.Rows[0]["MOBILE"].ToString();
                //ObjCustomer.EMail = dtEntitydtls.Rows[0]["EMAIL"].ToString();
                ObjCustomer.WebSite = dtEntitydtls.Rows[0]["WEBSITE"].ToString();
                Session["VendorInfo"] = ObjCustomer;
                //Session["Address"] = ObjCustomer.Address;
                //Session["Mobile"] = ObjCustomer.Mobile;
                //Session["EMail"] = ObjCustomer.EMail;
                //Session["WebSite"] = ObjCustomer.WebSite;
                Session["Suffix"] = ObjS3GSession.ProGpsSuffixRW;
            #endregion
            }
        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Due to Data Problem, Unable to Load Vendor Details Grid.";
            CVVendorDetails.IsValid = false;
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
            FunPriClearVendorDetails();

        }
        catch (Exception ex)
        {
            CVVendorDetails.ErrorMessage = "Unable to Clear.";
            CVVendorDetails.IsValid = false;
        }

    }

    /// <summary>
    /// export to crystal format
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Session["Balance"] = ((Label)grvVendorDetails.FooterRow.FindControl("lblTotalBalance")).Text;
        string strScipt = "window.open('../Reports/S3GRptEntityLedgerReport.aspx', 'Entity','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Entity", strScipt, true);


    }

    #endregion

}
