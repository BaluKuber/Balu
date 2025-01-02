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

public partial class Reports_S3GRptDisbursementReport : ApplyThemeForProject
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
    
    string strPageName = "DisbursementReport";
    ReportAccountsMgtServicesClient objSerClient;
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
              
                FunPriLoadPage();// This screen
                      
    }
    private void FunPriLoadPage()
    {
        try
        {
            
            //txtStartDateSearch.Text = "";
            //txtEndDateSearch.Text = "";
            ObjUserInfo = new UserInfo();
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            ObjS3GSession = new S3GSession();
            Session["Currency"] = ObjS3GSession.ProCurrencyNameRW;
            CompanyId = ObjUserInfo.ProCompanyIdRW;
            ProgramId = 145;
            ddlbranch.Enabled = false;
            UserId = ObjUserInfo.ProUserIdRW;
            strDateFormat = ObjS3GSession.ProDateFormatRW;                  // to get the standard date format of the Application
            CalendarExtender1.Format = strDateFormat;                       // assigning the first textbox with the End date
            CalendarExtender2.Format = strDateFormat;                       // assigning the first textbox with the start date
            //todaydate = DateTime.Now.ToString();
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            /* Changed Date Control start - 30-Nov-2012 */
            txtStartDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            //txtStartDateSearch.Attributes.Add("readonly", "readonly");
            //txtEndDateSearch.Attributes.Add("readonly", "readonly");
            /* Changed Date Control end - 30-Nov-2012 */
            if (!IsPostBack)
            {
                ClearSession();
                PnlAbstract.Visible = false;
                PnlDetailedView.Visible = false;
                btnPrint.Visible = false;
                
                FunPriLoadLob(CompanyId, UserId, ProgramId);
                //FunPriLoadRegion(CompanyId, Is_Active, UserId);
                FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
                FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
                //FunPriLoadBranch(CompanyId, UserId, Region_Id, Is_Active);
                FunPriLoadProduct(CompanyId, LobId);
            }

          ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable Load DisbursementReport page");
        }
    }
    #endregion

    #region Load LOB
    private void FunPriLoadLob(int CompanyId, int UserId, int ProgramId)
    {
       try
       {
            ddlLOB.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubLOB(CompanyId, UserId,ProgramId);
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
    #endregion

    #region Load Region
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

            ddlRegion.DataSource = Branch;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "All";
            
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

            ddlbranch.DataSource = Branch;
            ddlbranch.DataTextField = "Description";
            ddlbranch.DataValueField = "ID";
            ddlbranch.DataBind();
            ddlbranch.Items[0].Text = "All";
            if (ddlbranch.Items.Count == 2)
            {
                ddlbranch.SelectedIndex = 1;
            }
            else
            {
                ddlbranch.SelectedIndex = 0;
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

    private void FunPriLoadLocation2(int ProgramId, int UserId, int CompanyId, int LobId,int LocationId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            if (ddlRegion.SelectedIndex > 0)
            {
                LocationId = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId,LocationId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlbranch.DataSource = Branch;
            ddlbranch.DataTextField = "Description";
            ddlbranch.DataValueField = "ID";
            ddlbranch.DataBind();
            ddlbranch.Items[0].Text = "All";
            if (ddlbranch.Items.Count == 2)
            {
                ddlbranch.SelectedIndex = 1;
            }
            else
            {
                ddlbranch.SelectedIndex = 0;
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
    //private void FunPriLoadRegion(int CompanyId, bool Is_Active, int UserId)
    //{
    //    try
    //    {
    //        ddlRegion.Items.Clear();
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        byte[] byteLobs = objSerClient.FunPubGetRegion(CompanyId, true, UserId);
    //        List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
    //        ddlRegion.DataSource = lobs;
    //        ddlRegion.DataTextField = "Description";
    //        ddlRegion.DataValueField = "ID";
    //        ddlRegion.DataBind();
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
    #endregion

    #region Load branch
    //private void FunPriLoadBranch(int CompanyId, int UserId, string Region_Id, bool Is_active)
    //{
    //    try
    //    {
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        string Region = string.Empty;
    //        ddlbranch.Items.Clear();
    //        if (ddlRegion.SelectedIndex != 0)
    //        {
    //            Region = ddlRegion.SelectedValue;
    //        }
    //        byte[] byteLobs = objSerClient.FunPubGetRegBranch(CompanyId, UserId, Region, true);
    //        List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
    //        ddlbranch.DataSource = Branch;
    //        ddlbranch.DataTextField = "Description";
    //        ddlbranch.DataValueField = "ID";
    //        ddlbranch.DataBind();
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
    #endregion

    #region Product
    public void FunPriLoadProduct(int CompanyId, int LobId)
    {
        try
        {
            ddlProduct.Items.Clear();
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddlLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetProductDetails(CompanyId,LobId);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlProduct.DataSource = Product;
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();
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
    #endregion
   
    #region Events
    protected void btnGo_Click(object sender, EventArgs e)
    {

       
        try
        {
            if (chkAbstract.Checked == false && chkDetailed.Checked==false)
            {
                chkAbstract.Checked = true;
            }
            FunPriValidateFromEndDate();
        }
        catch (Exception ex)
        {
  
            CVDisbursement.ErrorMessage = "Due to Data Problem, Unable to Load Grid";
            CVDisbursement.IsValid = false;
        }
        #region commented
        //FunPriValidateFromEndDate();
        //ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
        //if (Convert.ToInt32(ddlLOB.SelectedValue)>0)
        //{
        //     objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
                      
        //}
        //else
        //{
        //    objHeader.Lob = "ALL";
        //}
        //if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
        //{
        //    objHeader.Region = (ddlRegion.SelectedItem.Text.Split('-'))[1].ToString();
        //}
        //else
        //{
        //    objHeader.Region = "ALL";
        //    //((Label)grvSummary.FooterRow.FindControl("lblRegion")).Text;
        //}

        //if (Convert.ToInt32(ddlbranch.SelectedValue)>0)
        //{
        //    objHeader.Branch = (ddlbranch.SelectedItem.Text.Split('-'))[1].ToString();

        //}
        //else
        //{

        //    objHeader.Branch = "ALL";
        //}
        //if (Convert.ToInt32(ddlProduct.SelectedValue )>0)
        //{
        //   objHeader.Product = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
       

        //}
        //else
        //{
        //    objHeader.Product = "ALL";
        //}
        //objHeader.StartDate = txtStartDateSearch.Text;
        //objHeader.EndDate = txtEndDateSearch.Text;
        ////if (chkAbstract.Checked == true)
        ////{
        ////    objHeader.IsAbstract = true;
        ////}
        ////if (chkDetailed.Checked == true)
        ////{
        ////    objHeader.IsDetails = true;
        ////}
        //Session["Header"] = objHeader;     
        ////FunPriLoadAbstract();
        ////FunPriLoadDetails();
        //if (chkAbstract.Checked == true)
        //{
        //    grvAbstract.Visible = true;
        //    FunPriLoadAbstract();
        //}
       
        //if (chkDetailed.Checked == true)
        //{
        //    GrvDetails.Visible = true;
        //    FunPriLoadDetails();
        //}
       
        //else if(chkAbstract.Checked ==true && chkDetailed.Checked==true)
        //{
        //    FunPriLoadAbstract();
        //    FunPriLoadDetails();
        //}

        //if(Chkreport.SelectedValue == "0")
        //{
        //    FunPriLoadAbstract();
        //}
        //else(Chkreport.SelectedValue=="1")
        //{

        //    FunPriLoadDetails();
        //}
        //else (Chkreport.SelectedValue == "0" && Chkreport.SelectedValue == "1")
        //{
        //    FunPriLoadAbstract();
        //    FunPriLoadDetails();
        //}
        #endregion


    }
    #endregion

    #region Deserialize
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    #endregion

    #region Validate Functions
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
         lblAmounts.Visible = true;
        lblCurrency.Visible = true;
        lblCurrency.Text = ObjS3GSession.ProCurrencyNameRW+"]";
      ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
    //    if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
    //    {
    //        objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();

    //    }
    //    else
    //    {
    //        objHeader.Lob = "ALL";
    //    }
    //    if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
    //    {
    //        objHeader.Region = (ddlRegion.SelectedItem.Text.Split('-'))[1].ToString();
    //    }
    //    else
    //    {
    //        objHeader.Region = "ALL";
    //        //((Label)grvSummary.FooterRow.FindControl("lblRegion")).Text;
    //    }

    //    if (Convert.ToInt32(ddlRegion.SelectedValue) <=0)
    //    {
    //        objHeader.Branch = "ALL";

    //    }
    //    else
    //    {
    //  if(Convert.ToInt32(ddlbranch.SelectedValue)==0)
    // {
    // objHeader.Branch = "ALL";
    // }
    //else

    //  {
    //        objHeader.Branch = ddlbranch.SelectedItem.Text;

      
    //  }
    //    }
    //    if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
    //    {
    //        objHeader.Product = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();


    //    }
    //    else
    //    {
    //        objHeader.Product = "ALL";
    //    }
        objHeader.StartDate = txtStartDateSearch.Text;
        Session["StartDate"] = txtStartDateSearch.Text;
        Session["EndDate"] = txtEndDateSearch.Text;
        objHeader.EndDate = txtEndDateSearch.Text;
        Session["Header"] = objHeader;
        if (chkAbstract.Checked == true && chkDetailed.Checked == true)
        {
            grvAbstract.Visible = true;
            FunPriLoadAbstract();
            FunPriLoadDetails();
        }
        else if (chkDetailed.Checked == true && chkAbstract.Checked == false)
        {
            GrvDetails.Visible = true;
            PnlAbstract.Visible = false;
            FunPriLoadDetails();
        }
        else if (chkAbstract.Checked == true && chkDetailed.Checked == false)
        {
            GrvDetails.Visible = true;
            PnlDetailedView.Visible = false;
            FunPriLoadAbstract();
        }


    }
    #endregion

    #region Load Details Grid
    private void FunPriLoadDetails()
    {
        try
        {
            
            divDetails.Style.Add("display", "block");
            PnlDetailedView.Visible = true;
            objSerClient = new ReportAccountsMgtServicesClient();
            //string Region = string.Empty;
            //string LOB = string.Empty;
            //string Product = string.Empty;
            //string Branch = string.Empty;
            //if (ddlRegion.SelectedIndex != 0)
            //{
            //    Region = ddlRegion.SelectedValue;
            //}
            //if (ddlLOB.SelectedIndex != 0)
            //{
            //    LOB = ddlLOB.SelectedValue;
            //}
            //if (ddlProduct.SelectedIndex != 0)
            //{
            //    Product = ddlProduct.SelectedValue;
            //}
            //if (ddlbranch.SelectedIndex != 0)
            //{
            //    Branch = ddlbranch.SelectedValue;
            //}
            ClsPubDisburseSelectionCriteria disburseSelectionCriteria = new ClsPubDisburseSelectionCriteria();
            disburseSelectionCriteria.CompanyId = CompanyId;
            disburseSelectionCriteria.LobId= string.Empty;
            disburseSelectionCriteria.LocationID1 = string.Empty;
            disburseSelectionCriteria.LocationID2 = string.Empty;
            disburseSelectionCriteria.ProductId = string.Empty;
            disburseSelectionCriteria.StartDate = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();
            disburseSelectionCriteria.EndDate = Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString();
            disburseSelectionCriteria.IsDetail = true;
            disburseSelectionCriteria.UserId = UserId;
            if (ddlLOB.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LobId = ddlLOB.SelectedValue;
            }
            if (ddlRegion.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            if (ddlProduct.SelectedIndex != 0)
            {
                disburseSelectionCriteria.ProductId = ddlProduct.SelectedValue;
            }
            byte[] bytedisburse = objSerClient.FunPubGetDisburseDetails(disburseSelectionCriteria);
            //List<ClsPubDisbursementDetails> details = (List<ClsPubDisbursementDetails>)DeSerialize(bytedisburse);
            ClsPubDisbursement details = (ClsPubDisbursement)DeSerialize(bytedisburse);
            TotalApprovedAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
            //TotalApprovedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
            TotalPaidAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.PaidAmount);
            TotalRemainingAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.RemainingAmount);
            TotalAccountYettoBeCreatedAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.AccountYetToBeCreated);
            Totalageing0days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing0days);
            Totalageing30days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing30days);
            Totalageing60days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing60days);
            Session["lobloc"] = details;
            Session["Details"] = details;
            if (GrvDetails.Rows.Count != 0)
            {
                GrvDetails.HeaderRow.Style.Add("position", "relative");
                GrvDetails.HeaderRow.Style.Add("z-index", "auto");
                GrvDetails.HeaderRow.Style.Add("top", "auto");

            }
            if (chkDetailed.Checked == true)
            {
                GrvDetails.DataSource = details.Disbursement;
                GrvDetails.EmptyDataText = "No Records Found";
                GrvDetails.DataBind();
            }
            else
            {
                GrvDetails.Visible = false;
            }

            FunPriDisplayTotalDetails();
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
    #endregion

    #region Load Abstract Grid
    private void FunPriLoadAbstract()
    {
        try
        {
            divAbstract.Style.Add("display", "block");
            PnlAbstract.Visible = true;
            objSerClient = new ReportAccountsMgtServicesClient();
            string Region = string.Empty;
            string LOB = string.Empty;
            string Product = string.Empty;
            string Branch = string.Empty;
            //if (ddlRegion.SelectedIndex != 0)
            //{
            //    Region = ddlRegion.SelectedValue;
            //}
            //if (ddlLOB.SelectedIndex != 0)
            //{
            //    LOB = ddlLOB.SelectedValue;
            //}
            //if (ddlProduct.SelectedIndex != 0)
            //{
            //    Product = ddlProduct.SelectedValue;
            //}
            //if (ddlbranch.SelectedIndex != 0)
            //{
            //    Branch = ddlbranch.SelectedValue;
            //}
            ClsPubDisburseSelectionCriteria disburseSelectionCriteria = new ClsPubDisburseSelectionCriteria();
            disburseSelectionCriteria.CompanyId = CompanyId;
            disburseSelectionCriteria.LobId = string.Empty;
            disburseSelectionCriteria.LocationID1 = string.Empty;
            disburseSelectionCriteria.LocationID2 = string.Empty;
            disburseSelectionCriteria.ProductId = string.Empty;
            disburseSelectionCriteria.StartDate = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();
            disburseSelectionCriteria.EndDate = Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString();
            disburseSelectionCriteria.IsDetail = false;
            if (ddlLOB.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LobId = ddlLOB.SelectedValue;
            }
            if (ddlRegion.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
            }
            if (ddlbranch.SelectedIndex != 0)
            {
                disburseSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
            }
            if (ddlProduct.SelectedIndex != 0)
            {
                disburseSelectionCriteria.ProductId = ddlProduct.SelectedValue;
            }
            disburseSelectionCriteria.UserId = UserId;
            byte[] bytedisburse = objSerClient.FunPubGetDisburseDetails(disburseSelectionCriteria);
            //List<ClsPubDisbursement> details = (List<ClsPubDisbursementDetails>)DeSerialize(bytedisburse);
            ClsPubDisbursement details = (ClsPubDisbursement)DeSerialize(bytedisburse);
            //Response.Write(details.Count.ToString());
            TotalApprovedAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
            TotalPaidAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.PaidAmount);
            TotalRemainingAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.RemainingAmount);
            TotalAccountYettoBeCreatedAmount = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.AccountYetToBeCreated);
            Totalageing0days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing0days);
            Totalageing30days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing30days);
            Totalageing60days = details.Disbursement.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing60days);
            Session["lobloc"] = details;
            Session["Abstract"] = details;
            if (grvAbstract.Rows.Count != 0)
            {
                grvAbstract.HeaderRow.Style.Add("position", "relative");
                grvAbstract.HeaderRow.Style.Add("z-index", "auto");
                grvAbstract.HeaderRow.Style.Add("top", "auto");

            }
            if (chkAbstract.Checked == true)
            {
                grvAbstract.DataSource = details.Disbursement;
                grvAbstract.EmptyDataText = "No Records Found";
                grvAbstract.DataBind();
            }
            else
            {
                grvAbstract.Visible = false;
            }
            FunPriDisplayTotalAbstract();
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
    //private void FunPriLoadAbstract()
    //{
    //    try
    //    {
    //        divAbstract.Style.Add("display", "block");
    //        PnlAbstract.Visible = true;
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        string Region = string.Empty;
    //        string LOB = string.Empty;
    //        string Product = string.Empty;
    //        string Branch = string.Empty;
    //        //if (ddlRegion.SelectedIndex != 0)
    //        //{
    //        //    Region = ddlRegion.SelectedValue;
    //        //}
    //        //if (ddlLOB.SelectedIndex != 0)
    //        //{
    //        //    LOB = ddlLOB.SelectedValue;
    //        //}
    //        //if (ddlProduct.SelectedIndex != 0)
    //        //{
    //        //    Product = ddlProduct.SelectedValue;
    //        //}
    //        //if (ddlbranch.SelectedIndex != 0)
    //        //{
    //        //    Branch = ddlbranch.SelectedValue;
    //        //}
    //        ClsPubDisburseSelectionCriteria disburseSelectionCriteria = new ClsPubDisburseSelectionCriteria();
    //        disburseSelectionCriteria.CompanyId = CompanyId;
    //        disburseSelectionCriteria.LobId = string.Empty;
    //        disburseSelectionCriteria.LocationID1 = string.Empty;
    //        disburseSelectionCriteria.LocationID2 = string.Empty;
    //        disburseSelectionCriteria.ProductId = string.Empty;
    //        disburseSelectionCriteria.StartDate = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.EndDate = Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.IsDetail = false;
    //        if (ddlLOB.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LobId = ddlLOB.SelectedValue;
    //        }
    //        if (ddlRegion.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
    //        }
    //        if (ddlbranch.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
    //        }
    //        if (ddlProduct.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.ProductId = ddlProduct.SelectedValue;
    //        }
    //        disburseSelectionCriteria.UserId = UserId;
    //        byte[] bytedisburse = objSerClient.FunPubGetDisburseDetails(disburseSelectionCriteria);
    //        List<ClsPubDisbursementDetails> details = (List<ClsPubDisbursementDetails>)DeSerialize(bytedisburse);
    //        Response.Write(details.Count.ToString());
    //        TotalApprovedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
    //        TotalPaidAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.PaidAmount);
    //        TotalRemainingAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.RemainingAmount);
    //        TotalAccountYettoBeCreatedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.AccountYetToBeCreated);
    //        Totalageing0days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing0days);
    //        Totalageing30days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing30days);
    //        Totalageing60days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing60days);
    //        Session["Abstract"] = details;
    //        if (grvAbstract.Rows.Count != 0)
    //        {
    //            grvAbstract.HeaderRow.Style.Add("position", "relative");
    //            grvAbstract.HeaderRow.Style.Add("z-index", "auto");
    //            grvAbstract.HeaderRow.Style.Add("top", "auto");

    //        }
    //        if (chkAbstract.Checked == true)
    //        {
    //            grvAbstract.DataSource = details;
    //            grvAbstract.EmptyDataText = "No Records Found";
    //            grvAbstract.DataBind();
    //        }
    //        else
    //        {
    //            grvAbstract.Visible = false;
    //        }
    //        FunPriDisplayTotalAbstract();
    //        btnPrint.Visible = true;

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
    //private void FunPriLoadAbstract()
    //{
    //    try
    //    {
    //        divAbstract.Style.Add("display", "block");
    //        PnlAbstract.Visible = true;
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        string Region = string.Empty;
    //        string LOB = string.Empty;
    //        string Product = string.Empty;
    //        string Branch = string.Empty;
    //        //if (ddlRegion.SelectedIndex != 0)
    //        //{
    //        //    Region = ddlRegion.SelectedValue;
    //        //}
    //        //if (ddlLOB.SelectedIndex != 0)
    //        //{
    //        //    LOB = ddlLOB.SelectedValue;
    //        //}
    //        //if (ddlProduct.SelectedIndex != 0)
    //        //{
    //        //    Product = ddlProduct.SelectedValue;
    //        //}
    //        //if (ddlbranch.SelectedIndex != 0)
    //        //{
    //        //    Branch = ddlbranch.SelectedValue;
    //        //}
    //        ClsPubDisburseSelectionCriteria disburseSelectionCriteria = new ClsPubDisburseSelectionCriteria();
    //        disburseSelectionCriteria.CompanyId = CompanyId;
    //        disburseSelectionCriteria.LobId = string.Empty;
    //        disburseSelectionCriteria.LocationID1 = string.Empty;
    //        disburseSelectionCriteria.LocationID2 = string.Empty;
    //        disburseSelectionCriteria.ProductId = string.Empty;
    //        disburseSelectionCriteria.StartDate = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.EndDate = Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.IsDetail = false;
    //        if (ddlLOB.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LobId = ddlLOB.SelectedValue;
    //        }
    //        if (ddlRegion.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
    //        }
    //        if (ddlbranch.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
    //        }
    //        if (ddlProduct.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.ProductId = ddlProduct.SelectedValue;
    //        }
    //        disburseSelectionCriteria.UserId = UserId;
    //        byte[] bytedisburse = objSerClient.FunPubGetDisburseDetails(disburseSelectionCriteria);
    //        List<ClsPubDisbursementDetails> details = (List<ClsPubDisbursementDetails>)DeSerialize(bytedisburse);
    //        Response.Write(details.Count.ToString());
    //        TotalApprovedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
    //        TotalPaidAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.PaidAmount);
    //        TotalRemainingAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.RemainingAmount);
    //        TotalAccountYettoBeCreatedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.AccountYetToBeCreated);
    //        Totalageing0days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing0days);
    //        Totalageing30days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing30days);
    //        Totalageing60days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing60days);
    //        Session["Abstract"] = details;
    //        if (grvAbstract.Rows.Count != 0)
    //        {
    //            grvAbstract.HeaderRow.Style.Add("position", "relative");
    //            grvAbstract.HeaderRow.Style.Add("z-index", "auto");
    //            grvAbstract.HeaderRow.Style.Add("top", "auto");

    //        }
    //        if (chkAbstract.Checked == true)
    //        {
    //            grvAbstract.DataSource = details;
    //            grvAbstract.EmptyDataText = "No Records Found";
    //            grvAbstract.DataBind();
    //        }
    //        else
    //        {
    //            grvAbstract.Visible = false;
    //        }
    //        FunPriDisplayTotalAbstract();
    //        btnPrint.Visible = true;

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
    //private void FunPriLoadAbstractDetail()
    //{
    //    try
    //    {
    //        divAbstract.Style.Add("display", "block");
    //        PnlAbstract.Visible = true;
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        string Region = string.Empty;
    //        string LOB = string.Empty;
    //        string Product = string.Empty;
    //        string Branch = string.Empty;
    //        //if (ddlRegion.SelectedIndex != 0)
    //        //{
    //        //    Region = ddlRegion.SelectedValue;
    //        //}
    //        //if (ddlLOB.SelectedIndex != 0)
    //        //{
    //        //    LOB = ddlLOB.SelectedValue;
    //        //}
    //        //if (ddlProduct.SelectedIndex != 0)
    //        //{
    //        //    Product = ddlProduct.SelectedValue;
    //        //}
    //        //if (ddlbranch.SelectedIndex != 0)
    //        //{
    //        //    Branch = ddlbranch.SelectedValue;
    //        //}
    //        ClsPubDisburseSelectionCriteria disburseSelectionCriteria = new ClsPubDisburseSelectionCriteria();
    //        disburseSelectionCriteria.CompanyId = CompanyId;
    //        disburseSelectionCriteria.LobId = string.Empty;
    //        disburseSelectionCriteria.LocationID1 = string.Empty;
    //        disburseSelectionCriteria.LocationID2 = string.Empty;
    //        disburseSelectionCriteria.ProductId = string.Empty;
    //        disburseSelectionCriteria.StartDate = Utility.StringToDate(txtStartDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.EndDate = Utility.StringToDate(txtEndDateSearch.Text).ToShortDateString();
    //        disburseSelectionCriteria.IsDetail = false;
    //        if (ddlLOB.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LobId = ddlLOB.SelectedValue;
    //        }
    //        if (ddlRegion.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID1 = ddlRegion.SelectedValue;
    //        }
    //        if (ddlbranch.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.LocationID2 = ddlbranch.SelectedValue;
    //        }
    //        if (ddlProduct.SelectedIndex != 0)
    //        {
    //            disburseSelectionCriteria.ProductId = ddlProduct.SelectedValue;
    //        }
    //        disburseSelectionCriteria.UserId = UserId;
    //        byte[] bytedisburse = objSerClient.FunPubGetDisburseDetails(disburseSelectionCriteria);
    //        List<ClsPubDisbursementDetails> details = (List<ClsPubDisbursementDetails>)DeSerialize(bytedisburse);
    //        
    //        TotalApprovedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ApprovedAmount);
    //        TotalPaidAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.PaidAmount);
    //        TotalRemainingAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.RemainingAmount);
    //        TotalAccountYettoBeCreatedAmount = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.AccountYetToBeCreated);
    //        Totalageing0days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing0days);
    //        Totalageing30days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing30days);
    //        Totalageing60days = details.Sum(ClsPubDisbursementDetails => ClsPubDisbursementDetails.ageing60days);
    //        Session["Abstract"] = details;
    //        if (grvAbstract.Rows.Count != 0)
    //        {
    //            grvAbstract.HeaderRow.Style.Add("position", "relative");
    //            grvAbstract.HeaderRow.Style.Add("z-index", "auto");
    //            grvAbstract.HeaderRow.Style.Add("top", "auto");

    //        }
    //        if (chkAbstract.Checked == true)
    //        {
    //            grvAbstract.DataSource = details;
    //            grvAbstract.EmptyDataText = "No Records Found";
    //            grvAbstract.DataBind();
    //        }
    //        else
    //        {
    //            grvAbstract.Visible = false;
    //        }
    //        FunPriDisplayTotalAbstract();
    //        btnPrint.Visible = true;

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
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        lblAmounts.Visible = false;
        lblCurrency.Visible = false;
        PnlDetailedView.Visible = false;
        ddlbranch.Enabled = false;
        ddlLOB.SelectedValue = "-1";
        ddlRegion.SelectedValue = "-1";
        ddlbranch.SelectedValue = "-1";
        ddlProduct.SelectedValue = "-1";
        txtStartDateSearch.Text = "";
        txtEndDateSearch.Text = "";
        PnlAbstract.Visible = false;
        grvAbstract.DataSource = null;
        grvAbstract.DataBind();
        GrvDetails.Visible = false;
        GrvDetails.DataBind();
        chkDetailed.Checked = false;
        chkAbstract.Checked = true;
        btnPrint.Visible = false;
        ClearSession();
       
    }
    private void FunPriDisplayTotalDetails()
    {
        if (GrvDetails.Rows.Count > 0)
        {
            ((Label)GrvDetails.FooterRow.FindControl("lblFApprovedAmountGD")).Text = TotalApprovedAmount.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblFPaidAmountGD")).Text = TotalPaidAmount.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblFRemainingAmountGD")).Text = TotalRemainingAmount.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblFAccountYetToBeCreatedGD")).Text = TotalAccountYettoBeCreatedAmount.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblageing0daysfd")).Text = Totalageing0days.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblageing30daysfd")).Text = Totalageing30days.ToString(Funsetsuffix());
            ((Label)GrvDetails.FooterRow.FindControl("lblageing60daysfd")).Text = Totalageing60days.ToString(Funsetsuffix());
        }

    }
    private void FunPriDisplayTotalAbstract()
    {
        if (grvAbstract.Rows.Count > 0)
        {
            ((Label)grvAbstract.FooterRow.FindControl("lblFApprovedAmountA")).Text = TotalApprovedAmount.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblFPaidAmountA")).Text = TotalPaidAmount.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblFRemainingAmountA")).Text = TotalRemainingAmount.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblFAccountYetToBeCreatedA")).Text = TotalAccountYettoBeCreatedAmount.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblageing0daysf")).Text =  Totalageing0days.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblageing30daysf")).Text = Totalageing30days.ToString(Funsetsuffix());
            ((Label)grvAbstract.FooterRow.FindControl("lblageing60daysf")).Text = Totalageing60days.ToString(Funsetsuffix());
        }

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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if(chkAbstract.Checked)
        {
            Session["IsAbstract"] ="true";
        }
        else
        {
            Session["IsAbstract"] ="false";
        }

        
        string strScipt = "window.open('../Reports/S3GDisbursementReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Disbursement", strScipt, true);
    }
    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Abstract"] = null;
        Session["Details"] = null;
    }
     protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
      {
          if (ddlRegion.SelectedIndex > 0)
          {

              //FunPriLoadBranch(CompanyId, UserId, Region_Id, Is_Active);
              FunPriLoadLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
              ddlbranch.Enabled = true;
              PnlAbstract.Visible = false;
              PnlDetailedView.Visible = false;
              lblAmounts.Visible = false;
              lblCurrency.Visible = false;
              btnPrint.Visible = false;
              ddlProduct.SelectedIndex =-1;
              txtStartDateSearch.Text = "";
              txtEndDateSearch.Text = "";
          }
          else
          {
              FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
              ddlbranch.Enabled = false;
              PnlAbstract.Visible = false;
              PnlDetailedView.Visible = false;
              lblAmounts.Visible = false;
              lblCurrency.Visible = false;
              btnPrint.Visible = false;
              ddlProduct.SelectedIndex = -1;
              txtStartDateSearch.Text = "";
              txtEndDateSearch.Text = "";
          }
            
      }
      protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
      {
            FunPriLoadLocation1(CompanyId, UserId, ProgramId, LobId);
            FunPriLoadProduct(CompanyId, LobId);
            FunPriLoadLocation(CompanyId, UserId, ProgramId, LobId);
            PnlAbstract.Visible = false;
            PnlDetailedView.Visible = false;
            lblAmounts.Visible = false;
            lblCurrency.Visible = false;
            btnPrint.Visible = false;
            txtStartDateSearch.Text = "";
            txtEndDateSearch.Text = "";
            
      }

      protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
      {
          
          PnlAbstract.Visible = false;
          PnlDetailedView.Visible = false;
          lblAmounts.Visible = false;
          lblCurrency.Visible = false;
          btnPrint.Visible = false;
          txtStartDateSearch.Text = "";
          txtEndDateSearch.Text = "";

      }
      protected void chkAbstract_CheckedChanged(object sender, EventArgs e)
      {
          PnlAbstract.Visible = false;
          PnlDetailedView.Visible = false;
          btnPrint.Visible = false;
          lblAmounts.Visible = false;
          lblCurrency.Visible = false;
          Session["Abstract"] = null;
          Session["Details"] = null;
          Session["lobloc"] = null;
         
      }
      protected void chkDetailed_CheckedChanged(object sender, EventArgs e)
      {
          PnlAbstract.Visible = false;
          PnlDetailedView.Visible = false;
          btnPrint.Visible = false;
          lblAmounts.Visible = false;
          lblCurrency.Visible = false;
          Session["Abstract"] = null;
          Session["Details"] = null;
          Session["lobloc"] = null;
      }
      protected void txtStartDateSearch_TextChanged(object sender, EventArgs e)
      {
          PnlAbstract.Visible = false;
          PnlDetailedView.Visible = false;
          btnPrint.Visible = false;
          lblAmounts.Visible = false;
          lblCurrency.Visible = false;
       
      }
      protected void txtEndDateSearch_TextChanged(object sender, EventArgs e)
      {
          PnlAbstract.Visible = false;
          PnlDetailedView.Visible = false;
          btnPrint.Visible = false;
          lblAmounts.Visible = false;
          lblCurrency.Visible = false;
       
      }
}
