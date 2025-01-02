using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using S3GBusEntity;
using S3GBusEntity.Reports;
using System.Web.UI.WebControls;
using ReportAccountsMgtServicesReference;

public partial class Reports_S3G_RPT_Collateral : ApplyThemeForProject
{
    #region variable declaration
    int intProgramId = 230;    
    public string strDateFormat;
    int intCompanyId;
    int intUserId;
    ReportAccountsMgtServicesClient objSerClient;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {       
        FunPriLoadPage();
    }

    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtCustomerName.Text = txtName.Text;            
        }
        catch (Exception ex)
        {
            CVPDCAcknowledgement.ErrorMessage = "Due to Data Problem, Unable to Load Customer Name";
            CVPDCAcknowledgement.IsValid = false;
        }
    }
    private void FunPriLoadPage()
    {
        ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.ToolTip = "Customer Name";
        Button btnGetLOV = (Button)ucCustomerCodeLov.FindControl("btnGetLOV");
        btnGetLOV.Focus();
        txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
        txtName.Attributes.Add("ReadOnly", "ReadOnly");
        txtFromReportDate.Attributes.Add("ReadOnly", "ReadOnly");
        txtToReportDate.Attributes.Add("ReadOnly", "ReadOnly");
        #region Application Standard Date Format
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
        ceFromReportDate.Format = strDateFormat;                       // assigning the first textbox with the End date
        ceToReportDate.Format = strDateFormat;                       // assigning the first textbox with the start date        
        UserInfo ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        #endregion
        if (!IsPostBack)
        {
            FunPriLoadLob();
            FunPriLoadBranch();
            FunPriLoadLocation();
            FunPriLoadLocation2();
            FunPubLoadCollateralType();
            ddllocation2.Enabled = false;
            pnlCollateraldetails.Visible = false;
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
            //if (ddlLOB.Items.Count == 2)
            //    ddlLOB.SelectedIndex = 1;
            //else
            //    ddlLOB.SelectedIndex = 0;
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
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLocation1.DataSource = Branch;
            ddlLocation1.DataTextField = "Description";
            ddlLocation1.DataValueField = "ID";
            ddlLocation1.DataBind();
            ddlLocation1.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //    ddlBranch.SelectedIndex = 1;
            //else
            //    ddlBranch.SelectedIndex = 0;

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLocation1.DataSource = Branch;
            ddlLocation1.DataTextField = "Description";
            ddlLocation1.DataValueField = "ID";
            ddlLocation1.DataBind();
            ddlLocation1.Items[0].Text = "--ALL--";

        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
            if (ddlLocation1.SelectedIndex != 0)
                Location1 = Convert.ToInt32(ddlLocation1.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, LobId, Location1);
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddllocation2.DataSource = Location;
            ddllocation2.DataTextField = "Description";
            ddllocation2.DataValueField = "ID";
            ddllocation2.DataBind();
            if (ddllocation2.Items.Count == 2)
            {
                if (ddlLocation1.SelectedIndex != 0)
                {
                    ddllocation2.SelectedIndex = 1;
                    //Utility.ClearDropDownList(ddlLocation2);
                }
                else
                    ddllocation2.SelectedIndex = 0;
            }
            else
            {
                ddllocation2.Items[0].Text = "--ALL--";
                ddllocation2.SelectedIndex = 0;
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

    private void FunPubLoadCollateralType()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteCollateralType=objSerClient.FunPubGetCollateralType(intCompanyId);
            List<ClsPubDropDownList> obj = (List<ClsPubDropDownList>)DeSeriliaze(byteCollateralType);
            ddlCollateralType.DataSource = obj;
            ddlCollateralType.DataTextField = "Description";
            ddlCollateralType.DataValueField = "ID";
            ddlCollateralType.DataBind();
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

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    protected void ddlLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddllocation2.Enabled = true;
        if (ddlLocation1.SelectedIndex > 0)
        {
            FunPriLoadLocation2();
        }
        else
        {
            ddllocation2.Enabled = false;
            FunPriLoadLocation();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            //if (hdnCustomerId.Value == "")
            //{
            //    Utility.FunShowAlertMsg(this, "Select a Customer");
            //    return;
            //}
            if (Utility.StringToDate(txtFromReportDate.Text) > Utility.StringToDate(txtToReportDate.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date");
                return;
            }
            ClsPubCollateralHeader objHeader = new ClsPubCollateralHeader();
            objHeader.LOBId = Convert.ToString(ddlLOB.SelectedValue);
            objHeader.LocationId1 = Convert.ToString(ddlLocation1.SelectedValue);
            objHeader.LocationId2 = Convert.ToString(ddllocation2.SelectedValue);
            objHeader.CompanyId = Convert.ToString(intCompanyId);
            objHeader.ProgramId = intProgramId;
            if (hdnCustomerId.Value == "")
            {
                objHeader.CustomerId = "0";
            }
            else
            {
                objHeader.CustomerId = Convert.ToString(hdnCustomerId.Value);
            }
            objHeader.UserId = Convert.ToString(intUserId);
            objHeader.CollateralTypeId = Convert.ToString(ddlCollateralType.SelectedValue);
            objHeader.StatusId = Convert.ToString(ddlCollateralStatus.SelectedValue);
            objHeader.StartDate = Utility.StringToDate(txtFromReportDate.Text);
            objHeader.EndDate = Utility.StringToDate(txtToReportDate.Text);
            byte[] byteHeader = ClsPubSerialize.Serialize(objHeader, SerializationMode.Binary);
            byte[] byteCollateral = objSerClient.FunPubGetCollateralDetails(byteHeader);
            List<ClsPubCollateralReport> objlist = new List<ClsPubCollateralReport>();
            objlist = (List<ClsPubCollateralReport>)DeSeriliaze(byteCollateral);
            grvCollateraldetails.DataSource = objlist;
            grvCollateraldetails.DataBind();
            pnlCollateraldetails.Visible = true;
            if (grvCollateraldetails.Rows.Count == 0)
            {
                grvCollateraldetails.EmptyDataText = "No Records Found";
                grvCollateraldetails.DataBind();
                //lblCurrency.Visible = false;
                btnPrint.Visible = false;                
            }
            else
            {
                btnPrint.Visible = true;                
            }
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

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlCollateralStatus.SelectedValue = "1";
        ddlCollateralType.SelectedValue = "-1";
        ddlLOB.SelectedValue = "-1";
        ddlLocation1.SelectedValue = "-1";
        ddllocation2.SelectedValue = "-1";        
        hdnCustID.Value = string.Empty;
        ddllocation2.Enabled = false;
        txtFromReportDate.Text = string.Empty;
        txtToReportDate.Text = string.Empty;
        HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
        hdnCustomerId.Value = string.Empty;
        TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
        txtName.Text = string.Empty;
        grvCollateraldetails.DataSource = null;
        grvCollateraldetails.DataBind();
        btnPrint.Visible = false;
        pnlCollateraldetails.Visible = false;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string strScipt = "window.open('../Reports/S3G_RPT_CollateralReport.aspx?qsCompanyId=" + intCompanyId 
            + "&qsLOBID=" + ddlLOB.SelectedValue + "&qsLOCATION1ID=" + ddlLocation1.SelectedValue 
            + "&qsLOCATION2ID=" + ddllocation2.SelectedValue + "&qsCustomerID=" + hdnCustID.Value 
            + "&qsCollateralTypeID=" + ddlCollateralType.SelectedValue + "&qsCollateralStatusID=" 
            + ddlCollateralStatus.SelectedValue
            + "&qsFromReportDate=" + Utility.StringToDate(txtFromReportDate.Text).ToString(strDateFormat)
            + "&qsToReportDate=" + Utility.StringToDate(txtToReportDate.Text).ToString(strDateFormat)
            + "&qsUserID=" + intUserId.ToString()
            +  "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Collateral", strScipt, true);
    }
}
