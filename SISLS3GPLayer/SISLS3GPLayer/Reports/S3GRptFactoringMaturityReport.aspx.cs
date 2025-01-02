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
using S3GBusEntity.Reports;
using System.Globalization;

public partial class Reports_S3GRptFactoringMaturityReport : ApplyThemeForProject
{
    Dictionary<string, string> Procparam;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    public string strDateFormat;
    public int CompanyId;
    public int ProgramId;
    public int UserId;
    public int LobId;


    string strPageName = "FactoringMaturityReport";
    ReportAccountsMgtServicesClient objSerClient;

    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();

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
            ProgramId = 237;            
            UserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            ceDate.Format = strDateFormat;                       // assigning the first textbox with the End date                        
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //Session["Date"] = DateTime.Now.Date.ToString();
            if (!IsPostBack) 
            {
                ClearSession();
                
                btnPrint.Visible = false;

                FunPriLoadLob(CompanyId, UserId, ProgramId);               
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                FunPubLoadDenomination();
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
        try
        {
            ddlLOB.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB_FT(CompanyId, UserId, ProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlLOB.DataSource = lobs;
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
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            objSerClient.Close();
        }
    }


    private void FunPriLoadLocation(int CompanyId, int UserId, int ProgramId, int LobId)
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

            ddlLocation.DataSource = Branch;
            ddlLocation.DataTextField = "Description";
            ddlLocation.DataValueField = "ID";
            ddlLocation.DataBind();
            ddlLocation.Items[0].Text = "All";

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
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
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
            if (objSerClient != null)
                objSerClient.Close();
        }
    }


    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }


    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Abstract"] = null;
        Session["Details"] = null;
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        divMaturity.Visible = true;
        btnPrint.Visible = true;
        FunPriLoadDetails();

    }


    private void FunPriLoadDetails()
    {
        try
        {

            divMaturity.Style.Add("display", "block");            
            objSerClient = new ReportAccountsMgtServicesClient();


            ClsPubFactoringMaturitySelectionCriteria factoringMaturity = new ClsPubFactoringMaturitySelectionCriteria();
            factoringMaturity.CompanyId = CompanyId;
            factoringMaturity.LobId = string.Empty;
            factoringMaturity.LocationID = string.Empty;            
            factoringMaturity.DenominationId = string.Empty;
            factoringMaturity.Date = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();         
            factoringMaturity.UserId = UserId;
            if (ddlLOB.SelectedIndex != 0)
            {
                factoringMaturity.LobId = ddlLOB.SelectedValue;
            }
            if (ddlLocation.SelectedIndex != 0)
            {
                factoringMaturity.LocationID = ddlLocation.SelectedValue;
            }            
            if (ddlDenomination.SelectedIndex != 0)
            {
                factoringMaturity.DenominationId = ddlDenomination.SelectedValue;
            }            
            byte[] bytefactoring = objSerClient.FunPubGetFactoringMaturityDetails(factoringMaturity);
            ClsPubFactoringMaturity details = (ClsPubFactoringMaturity)DeSerialize(bytefactoring);
            
            Session["Report"] = details;
            //Session["Report"] = details.FactoringMaturityReport;
            

            grvMaturity.DataSource = details.FactoringMaturity;
            //grvMaturity.EmptyDataText = "No Records Found";
            grvMaturity.DataBind();          

            
            btnPrint.Visible = true;

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


    protected void btnClear_Click(object sender, EventArgs e)
    {
        divMaturity.Visible = false;
        ddlLOB.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;        
        ddlDenomination.SelectedIndex = 0;
        txtStartDateSearch.Text = "";
        btnPrint.Visible = false;

    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        hdnPrint.Value = "1";
        string strScipt = "window.open('../Reports/S3GFactoringMaturityReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Factoring Maturity", strScipt, true);

    }

}
