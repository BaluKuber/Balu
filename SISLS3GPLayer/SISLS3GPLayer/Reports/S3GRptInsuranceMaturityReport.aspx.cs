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
using S3GBusEntity;
using System.Collections.Generic;
using ReportAccountsMgtServicesReference;
using ReportOrgColMgtServicesReference;
using S3GBusEntity.Reports;
using System.Globalization;

public partial class Reports_S3GRptInsuranceMaturityReport : ApplyThemeForProject
{
    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    public int CompanyId;
    public int ProgramId;
    public int UserId;
    public int LobId;
    public int LocationId;
    decimal TotalApprovedAmount;
    decimal TotalPaidAmount;
    decimal TotalRemainingAmount;
    decimal TotalAccountYettoBeCreatedAmount;
    decimal Totalageing0days;
    decimal Totalageing30days;
    decimal Totalageing60days;
    bool Is_Active;
    string EndDate;
    string LOB_ID;
    string Branch_ID;
    string Region_Id;
    string Product_Id;

    string strPageName = "InsuranceMaturityReport";
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient objReportOrgMgtServices;
    ReportAccountsMgtServicesClient objSerClient;    


    protected void Page_Load(object sender, EventArgs e)
    {
        #region Application Standard Date Format
        
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
        ceStartDate.Format = strDateFormat;                       // assigning the first textbox with the End date
        ceEndDate.Format = strDateFormat;                       // assigning the first textbox with the start date
        /* Changed Date Control start - 30-Nov-2012 */
        //txtStartDateSearch.Attributes.Add("readonly", "readonly");
        //txtEndDateSearch.Attributes.Add("readonly", "readonly");
        txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
        txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',false,  false);");
        /* Changed Date Control end - 30-Nov-2012 */

        //today = DateTime.Now.Date.ToString(strDateFormat);

        #endregion

        FunPriLoadPage();// This screen

    }


    private void FunPriLoadPage()
    {
        try
        {
            
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            ProgramId = 242;
            UserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application                      
            //todaydate = DateTime.Now.ToString();
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //Session["Date"] = DateTime.Now.Date.ToString();
            if (!IsPostBack)
            {
                FunPriLoadLob(CompanyId, UserId, ProgramId);                
                FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
                EnableDisableDetailsGrid(false);
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load DisbursementReport page");
        }
    }


    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {      

        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlLOB.DataSource = LOB;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            ddlLOB.Items[0].Text = "All";
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.SelectedIndex = 1;
            }
            else
            {
                ddlLOB.SelectedIndex = 0;
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


    private void FunPriLoadLocation1(int CompanyId, int UserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlLocation1.DataSource = Branch;
            ddlLocation1.DataTextField = "Description";
            ddlLocation1.DataValueField = "ID";
            ddlLocation1.DataBind();
            ddlLocation1.Items[0].Text = "All";
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


    //private void FunPriLoadLocation2(int ProgramId, int UserId, int CompanyId, int LobId, int LocationId)
    //{
    //    try
    //    {
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        if (ddlLOB.SelectedIndex > 0)
    //        {
    //            LobId = Convert.ToInt32(ddlLOB.SelectedValue);
    //        }
    //        if (ddlLocation1.SelectedIndex > 0)
    //        {
    //            LocationId = Convert.ToInt32(ddlLocation1.SelectedValue);
    //        }
    //        byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
    //        List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

    //        ddlLocation2.DataSource = Branch;
    //        ddlLocation2.DataTextField = "Description";
    //        ddlLocation2.DataValueField = "ID";
    //        ddlLocation2.DataBind();
    //        if (ddlLocation2.Items.Count == 2)
    //        {
    //            ddlLocation2.SelectedIndex = 1;
    //        }
    //        else
    //        {
    //            ddlLocation2.SelectedIndex = 0;
    //        }

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
   

    private List<ClsPubDropDownList> DeSeriliaze(byte[] byteLobs)
    {
        throw new NotImplementedException();
    }


    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }


    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Abstract"] = null;
        Session["Details"] = null;
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        divInsuranceMaturity.Visible = true;
        btnPrint.Visible = true;
        //FunPriLoadDetails();
        FunPriValidateFromEndDate();

    }

    
    protected void btnClear_Click(object sender, EventArgs e)
    {
        divInsuranceMaturity.Visible = false;
        ddlLOB.SelectedIndex = 0;
        ddlLocation1.SelectedIndex = 0;
        //ddlLocation2.SelectedIndex = 0;
        txtStartDateSearch.Text = "";
        txtEndDateSearch.Text = "";
        btnPrint.Visible = false;

    }


    private void EnableDisableDetailsGrid(bool isVisible)
    {
        //PnlFactoringMaturityreport.Visible = isVisible;
        btnPrint.Visible = isVisible;
       
        if (!isVisible)
        {
            grvInsuranceMaturity.DataSource = null;
            grvInsuranceMaturity.DataBind();

            DisableSession();
        }
    }


    private void DisableSession()
    {
        Session["PASA"] = null;
        Session["Transaction"] = null;
        Session["Assets"] = null;
        Session["Summary"] = null;
        Session["Memorandum"] = null;
    }


    private void FunPriValidateFromEndDate()
    {
        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtStartDateSearch.Text) > Utility.StringToDate(txtEndDateSearch.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to the Start Date");
                txtEndDateSearch.Text = "";
                return;
            }

        }
        if ((!(string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
           ((string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtEndDateSearch.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtStartDateSearch.Text))) &&
            (!(string.IsNullOrEmpty(txtEndDateSearch.Text))))
        {
            txtStartDateSearch.Text = txtEndDateSearch.Text;

        }
        EnableDisableDetailsGrid(true);
        //lblAmounts.Visible = true; // 2 b edited
        //lblCurrency.Visible = true;
        

        Session["Startdate"] = txtStartDateSearch.Text;
        //objheader.EndDate = txtEndDateSearch.Text;
        Session["Enddate"] = txtEndDateSearch.Text;

        Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();         
        DataSet dsAccountDetails;

        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", Convert.ToString(CompanyId));
        if (ddlLOB.SelectedIndex != 0)
            Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
        if (ddlLocation1.SelectedIndex != 0)
            Procparam.Add("@Location_ID1", Convert.ToString(ddlLocation1.SelectedValue));
        //if (ddlLocation2.SelectedIndex != 0)
        //    Procparam.Add("@Location_ID2", Convert.ToString(ddlLocation2.SelectedValue));
        Procparam.Add("@Start_Date", Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString());
        Procparam.Add("@End_Date", Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString());
        Procparam.Add("@USER_ID", Convert.ToString(UserId));
        dsAccountDetails = Utility.GetDataset("S3G_Rpt_GetInsuranceMaturity", Procparam);


        DataTable dtInsuranceMaturity = new DataTable();
        dtInsuranceMaturity = dsAccountDetails.Tables[0];

        
        grvInsuranceMaturity.DataSource = dtInsuranceMaturity;
        grvInsuranceMaturity.DataBind();

        Session["InsuranceMaturity"] = (DataTable)dtInsuranceMaturity;
        
        divInsuranceMaturity.Visible = true;
        if (grvInsuranceMaturity.Rows.Count <= 0)
        {
            grvInsuranceMaturity.EmptyDataText = "No Insurance details Found";
            grvInsuranceMaturity.DataBind();
        }

    }    


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        hdnPrint.Value = "1";
        string strScipt = "window.open('../Reports/S3GInsuranceMaturityReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Factoring Maturity", strScipt, true);

    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
            //FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);            
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
}
