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



public partial class Reports_S3G_RPT_DiscountedRepaymentReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    string intCustomerId;
    int intUserId;
    string PANum;
    string SANum;
    bool Is_Active;
    int Active;
    int intProgramId = 132;
    decimal total;
    decimal TotalInstallmentAmount;
    decimal TotalPrincipalAmount;
    decimal TotalFinanceCharges;
    decimal TotalUMFC;
    decimal TotalInsuranceAmount;
    decimal TotalOthers;
    decimal TotalVatrecovery;
    decimal TotalVatSetoff;
    decimal TotalTax;
    decimal decUnbilledFC;
    public string strDateFormat;
    string Flag = string.Empty;
    //decimal decAltFC;
    decimal TotFinAmt = 0;
    DataTable dtlob = new DataTable();
    DataTable dtbranch = new DataTable();
    Dictionary<string, string> Procparam;
    string strPageName = "Repayment Schedule";
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient;
protected void Page_Load(object sender, EventArgs e)
    {
        try
            {
            // this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPriLoadPage();
            
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Repayment Schedule Page.";
            CVRepaymentSchedule.IsValid = false;
            
            throw ex;
        }
    }

    

    # region Page Methods
    /// <summary>
    /// This Method is called when page is Loding
    /// </summary>
    private void FunPriLoadPage()
    {
        try
        {
            
            ucCustomerCodeLov.strControlID = ucCustomerCodeLov.ClientID;
            TextBox txtName = (TextBox)ucCustomerCodeLov.FindControl("txtName");
            txtName.Attributes.Add("onfocus", "fnLoadCustomer()");
            txtName.Attributes.Add("ReadOnly", "ReadOnly");


            ObjUserInfo = new UserInfo();
            ObjS3GSession = new S3GSession();

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            Session["Date"] = DateTime.Now.ToString(strDateFormat);

            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;
            intUserId = ObjUserInfo.ProUserIdRW;


            if (!IsPostBack)
            {
                // txtCustomerCode.Focus();
                FunPriGetLob();
                FunPriGetBranch();
                FunPriLoadLocation();
            }
            FunPriloadCustomercodes();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Repayment Schedule page");
        }
    }

    private void FunPriloadCustomercodes()
    {
        if (ddlBranch.SelectedIndex > 0 && ddlLOB.SelectedIndex > 0)
        {
            if (ViewState["Flag"] != null)
            {
                if (ViewState["Flag"].ToString() == "C")
                {

                    ucCustomerCodeLov.strLOBID = ddlLOB.SelectedValue.ToString();
                    ucCustomerCodeLov.strBranchID = ddlBranch.SelectedValue.ToString();
                    ucCustomerCodeLov.strRegionID = ddlLocation2.SelectedItem.Value.ToString();
                    ucCustomerCodeLov.strLOV_Code = "REPAY";


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

    /// <summary>
    /// This Method is called when page is Loading.
    /// To Load the Line of Business in Dropdown List.
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id">       
    private void FunPriGetLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ddlLOB.Items.Clear();
            byte[] byteLobs = objSerClient.FunPubLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
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
    ///  This Method is called when page is Loading.
    ///  To Load the Branch in Dropdown List.
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    /// <param name="Is_active"></param>
    private void FunPriGetBranch()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            // ddlBranch.Items.Clear();
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);

            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.SelectedIndex = 1;
            //}
            //else
            //    ddlBranch.SelectedIndex = 0;

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

    private void FunPriLoadLocation()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            //ddlBranch.Items.Clear();
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);

            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlLocation2.DataSource = Branch;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            ddlLocation2.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.SelectedIndex = 1;
            //}
            //else
            //    ddlBranch.SelectedIndex = 0;

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

    private void FunPriLoadLocation2()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int lobId = 0;
            if (ddlLOB.SelectedIndex > 0)
                lobId = Convert.ToInt32(ddlLOB.SelectedValue);
            int Location1 = 0;
            if (ddlBranch.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlBranch.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, lobId, Location1);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlLocation2.DataSource = Branch;
            ddlLocation2.DataTextField = "Description";
            ddlLocation2.DataValueField = "ID";
            ddlLocation2.DataBind();
            //if (ddlLocation2.Items.Count == 2)
            //{
            //    if (ddlBranch.SelectedIndex != 0)
            //    {
            //        ddlLocation2.SelectedIndex = 1;
            //        Utility.ClearDropDownList(ddlLocation2);
            //    }
            //    else
            //        ddlLocation2.SelectedIndex = 0;
            //}
            //else
            //{
            ddlLocation2.Items[0].Text = "--ALL--";
            //ddlLocation2.SelectedIndex = 0;
            //}

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
    /// This Method is called after select the Customer Name.
    /// To Load the Product in Dropdown List.
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intcutomer_id"></param>
    //private void FunPriLoadProduct(int intCompany_id, string intcutomer_id)
    //{
    //    objSerClient = new ReportAccountsMgtServicesClient();
    //    try
    //    {
    //        //ddlProduct.Items.Clear();
    //        byte[] byteLobs = objSerClient.FunPubGetProduct(intCompanyId, hdnCustID.Value);
    //        List<ClsPubDropDownList> product = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
    //    //    ddlProduct.DataSource = product;
    //    //    ddlProduct.DataTextField = "Description";
    //    //    ddlProduct.DataValueField = "ID";
    //    //    ddlProduct.DataBind();
    //    //    if (ddlProduct.Items.Count == 2)
    //    //    {
    //    //        ddlProduct.SelectedIndex = 1;
    //    //    }
    //    //    else
    //    //        ddlProduct.SelectedIndex = 0;
    //    //}
    //    catch (Exception ex)
    //    {
    //          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
    //        throw ex;
    //    }
    //    //finally
    //    //{
    //    //    objSerClient.Close();
    //    //}

    //}

    /// <summary>
    /// This Method is called after select the Customer Name.
    /// To Load the Prime Account Number in Dropdown List.
    /// </summary>
    private void FunPriLoadPrimeAccount()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ClsPubPrimeAccountDetails ObjPrimeAccounts = new ClsPubPrimeAccountDetails();
            ObjPrimeAccounts.Type = "1";
            ObjPrimeAccounts.CompanyId = intCompanyId;
            ObjPrimeAccounts.IsActivated = 1;
            ObjPrimeAccounts.CustomerId = hdnCustID.Value;
            ObjPrimeAccounts.CheckAccess = Convert.ToString(intUserId);
            ObjPrimeAccounts.ProgramId = intProgramId;
            byte[] bytePrimeAccounts = ClsPubSerialize.Serialize(ObjPrimeAccounts, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetMLA(bytePrimeAccounts);
            List<ClsPubDropDownList> PANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPNum.DataSource = PANum;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            //if (ddlPNum.Items.Count == 2)
            //{
            //    ddlPNum.SelectedIndex = 1;
            //}
            //else
            //    ddlPNum.SelectedIndex = 0;
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

    private void FunPriLoadPrimeAccountBasedLobCustomer()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ClsPubPrimeAccountDetails ObjPrimeAccount = new ClsPubPrimeAccountDetails();
            ObjPrimeAccount.Type = "1";
            ObjPrimeAccount.CompanyId = intCompanyId;
            if (ddlLOB.SelectedIndex > 0)
                ObjPrimeAccount.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            //ObjPrimeAccount.BranchId = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjPrimeAccount.IsActivated = 1;
            ObjPrimeAccount.CustomerId = hdnCustID.Value;
            ObjPrimeAccount.CheckAccess = Convert.ToString(intUserId);
            ObjPrimeAccount.ProgramId = intProgramId;
            byte[] bytePrimeAccounts = ClsPubSerialize.Serialize(ObjPrimeAccount, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetMLA(bytePrimeAccounts);
            List<ClsPubDropDownList> PANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPNum.DataSource = PANum;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            //if (ddlPNum.Items.Count == 2)
            //{
            //    ddlPNum.SelectedIndex = 1;
            //}
            //else
            //    ddlPNum.SelectedIndex = 0;
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

    private void FunPriLoadPrimeAccountBasedLobBranchCustomer()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ClsPubPrimeAccountDetails ObjPrimeAccount = new ClsPubPrimeAccountDetails();
            ObjPrimeAccount.Type = "1";
            ObjPrimeAccount.CompanyId = intCompanyId;
            if (ddlLOB.SelectedIndex > 0)
                ObjPrimeAccount.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlLocation2.SelectedIndex > 0)
                ObjPrimeAccount.locationId = Convert.ToInt32(ddlLocation2.SelectedValue);
            else if (ddlBranch.SelectedIndex > 0)
                ObjPrimeAccount.locationId = Convert.ToInt32(ddlBranch.SelectedValue);
            ObjPrimeAccount.IsActivated = 1;
            ObjPrimeAccount.CustomerId = hdnCustID.Value;
            ObjPrimeAccount.CheckAccess = Convert.ToString(intUserId);
            ObjPrimeAccount.ProgramId = intProgramId;
            byte[] bytePrimeAccounts = ClsPubSerialize.Serialize(ObjPrimeAccount, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetMLA(bytePrimeAccounts);
            List<ClsPubDropDownList> PANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlPNum.DataSource = PANum;
            ddlPNum.DataTextField = "Description";
            ddlPNum.DataValueField = "ID";
            ddlPNum.DataBind();
            //if (ddlPNum.Items.Count == 2)
            //{
            //    ddlPNum.SelectedIndex = 1;
            //}
            //else
            //    ddlPNum.SelectedIndex = 0;
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
    /// This Method is called after select the Customer Name.
    /// To Load the Csutomer Informations in Appropriate Fields.
    /// </summary>
    /// <param name="CustomerID"></param>
    private void FunPubGetCustomerDetails(string CustomerID)
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
                txtCustomerCode.Text = ucCustDetails.CustomerName;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to display Customer Details");
        }
    }

    private void FunPriLoadLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ddlLOB.Items.Clear();
            byte[] byteLobs = objSerClient.FunPubGetLOB(intCompanyId, intUserId, intProgramId, hdnCustID.Value);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
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
    private void FunPriLoadBranch()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            ddlBranch.Items.Clear();
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetBranch(intCompanyId, intUserId, intProgramId, hdnCustID.Value, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            //if (ddlBranch.Items.Count == 2)
            //{
            //    ddlBranch.SelectedIndex = 1;
            //}
            //else
            //    ddlBranch.SelectedIndex = 0;
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
    /// This Method is called after Clicking the Ok button.
    /// To Load the Repayment Details in Grid.
    /// </summary>
    /// <param name="PANum"></param>
    /// <param name="SANum"></param>
    private void FunPriLoadRepayDetails(int intCompany_id, string PANum, string SANum)
    {
        //objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            pnlRepayDetails.Visible = true;
            lblgridError.Text = "";
            divRepayDetails.Style.Add("display", "block");
            //string saNum = string.Empty;
            //if (ddlSNum.SelectedIndex != 0)
            //{
            //    saNum = ddlSNum.SelectedValue;
            //}
            //string Type = string.Empty;
            //if (chkAmort.Checked)
            //    Type = "A";
            //else
            //    Type = "R";
            //byte[] byteLobs = objSerClient.FunPubGetRepayDetails(intCompanyId, ddlPNum.SelectedValue, saNum, Type);
            //List<ClsPubRepayDetails> repayDetails1 = ((List<ClsPubRepayDetails>)DeSeriliaze (byteLobs));

            //List<ClsPubRepayDetails> repayDetails = repayDetails1.GroupBy(x => x.InstallmentNo).Select(y => y.First()).ToList();

            //TotalInstallmentAmount = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.InstallmentAmount);
            //TotalPrincipalAmount = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.PrincipalAmount);
            //TotalFinanceCharges = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.FinanceCharges);
            ////TotalUMFC = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.Umfc);
            //TotalInsuranceAmount = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.InsuranceAmount);
            //TotalOthers = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.Others);
            //TotalVatrecovery = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.VatRecovery);
            //TotalTax = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.Tax);
            //TotalVatSetoff = repayDetails.Sum(ClsPubRepayDetails => ClsPubRepayDetails.TaxSetOff);

            //Session["Repay"] = null;
            //Session["Repay"] = repayDetails;

            Dictionary<string, string> Procparam = new Dictionary<string, string>();//creating a dictionary object to get the parameters from the store procedure
            Procparam.Add("@PANUM", ddlPNum.SelectedValue);
            Procparam.Add("@COMPANY_ID", intCompany_id.ToString());
            Procparam.Add("@Others", txtDis.Text);
            


        

            

            DataSet ds = Utility.GetDataset("S3G_RPT_GetDiscountedRepayment", Procparam);            
            pnlRepayDetails.Visible = true;
            
            grvRepayDetails.Visible = true;

            grvRepayDetails.DataSource = ds.Tables[0];
           
            grvRepayDetails.DataBind();
            Session["Repay"] = ds.Tables[0];

            Label lblInstallmentAmount = grvRepayDetails.FooterRow.FindControl("lblInstallmentAmount") as Label;
            lblInstallmentAmount.Text = Convert.ToDecimal(ds.Tables[2].Compute("Sum(InstallmentAmount1)", "")).ToString(Funsetsuffix());
            Label lblOthers = (grvRepayDetails).FooterRow.FindControl("lblOthers") as Label;
            lblOthers.Text = Convert.ToDecimal(ds.Tables[2].Compute("Sum(Others1)", "")).ToString(Funsetsuffix());
            //ds.Tables.Add((DataTable)ViewState["DT_Header"]);
            ViewState["DT_Header"] = ds.Tables[1].Rows[0]["total"].ToString();
            txtTotal.Text = (Convert.ToDecimal(lblOthers.Text)+Convert.ToDecimal( ViewState["DT_Header"].ToString())).ToString();
            //txtTotal.text = Convert.ToDecimal(ds.tables[1].Compute("Sum(Total)", "")).ToString(Funsetsuffix());
            
            

            if (chkAmort.Checked)
            {
                pnlRepayDetails.GroupingText = "Repayment Structure";
                grvRepayDetails.Columns[3].Visible = false;
                grvRepayDetails.Columns[4].Visible = false;
            }
            //else
            //{
            //    pnlRepayDetails.GroupingText = "Amortization Schedule";
            //    grvRepayDetails.Columns[3].Visible = true;
            //    grvRepayDetails.Columns[4].Visible = true;
            //}


            //if (grvRepayDetails.Rows.Count != 0)
            //{
            //    grvRepayDetails.HeaderRow.Style.Add("position", "relative");
            //    grvRepayDetails.HeaderRow.Style.Add("z-index", "auto");
            //    grvRepayDetails.HeaderRow.Style.Add("top", "auto");

            //}

            //if (grvRepayDetails.Rows.Count == 0)
            //{
            //    Session["Repay"] = null;
            //    btnPrint.Enabled = false;
            //    lblgridError.Text = "No Repayment Details Found";
            //    grvRepayDetails.DataBind();
            //}
            //else
            //{
            //    FunPriDisplayTotal();
            //}
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
    protected void grvAssetDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFinanceAmt = e.Row.FindControl("lblFinanceAmt") as Label;
            // TotFinAmt = Convert.ToDecimal(lblFinanceAmt.Text);
            TotFinAmt = TotFinAmt + Convert.ToDecimal(lblFinanceAmt.Text);
        }
        Session["FinAmt"] = TotFinAmt;
    }
    protected void grvRepayDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            int i = e.Row.RowIndex;
            Label lblFinanceCharges = e.Row.FindControl("lblFinanceCharges") as Label;
            Label lblUMFC = e.Row.FindControl("lblUMFC") as Label;

            if (i == 0)
                decUnbilledFC = Convert.ToDecimal(lblUMFC.Text);
            else
                decUnbilledFC = decUnbilledFC - Convert.ToDecimal(lblFinanceCharges.Text);

            lblUMFC.Text = decUnbilledFC.ToString(Funsetsuffix());

        }
    }

    /// <summary>
    ///  This Method is called after Clicking the Ok button.
    ///  To Load the Asset Details in Grid.
    /// </summary>
    /// <param name="PANum"></param>
    /// <param name="SANum"></param>
    //private void FunPriLoadAsset(int intCompany_id, string PANum, string SANum)
    //{
    //    objSerClient = new ReportAccountsMgtServicesClient();
    //    try
    //    {
    //        pnlAssetDetails.Visible = true;
    //        lblError.Text = "";
    //        string saNum = string.Empty;
    //        if (ddlSNum.SelectedIndex != 0)
    //        {
    //            saNum = ddlSNum.SelectedValue;
    //        }
    //        byte[] byteLobs = objSerClient.FunPubGetAssestDetails(intCompanyId, ddlPNum.SelectedValue, saNum);
    //        List<ClsPubAssestDetails> assetDetails = (List<ClsPubAssestDetails>)DeSeriliaze(byteLobs);
    //        Session["Asset"] = assetDetails;
    //        grvAssetDetails.DataSource = assetDetails;
    //        grvAssetDetails.DataBind();
    //        if (grvAssetDetails.Rows.Count == 0)
    //        {
    //            Session["Asset"] = null;
    //            //grvAssetDetails.EmptyDataText = "No Asset Details Found";
    //            lblError.Text = "No Asset Details Found";
    //            grvAssetDetails.DataBind();

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

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }

    /// <summary>
    /// To assign the Repayment Details Total in footer row
    /// </summary>
    //private void FunPriDisplayTotal()
    //{
    //    //DataTable dt = (DataTable)Session["Repay"];
    //    //((Label)grvRepayDetails.FooterRow.FindControl("lblInstallmentAmount")).Text = Convert.ToDecimal(dt.Compute("sum(InstallmentAmount", "")).ToString(Funsetsuffix());

    //    //((Label)grvRepayDetails.FooterRow.FindControl("lblInstallmentAmount")).Text = Convert.ToDecimal(ds.Tables[0].Compute("Sum(InstallmentAmount)", "")).ToString();
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblPrincipalAmount")).Text = TotalPrincipalAmount.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblFinanceCharges")).Text = TotalFinanceCharges.ToString(Funsetsuffix());
    //    //((Label)grvRepayDetails.FooterRow.FindControl("lblUMFC")).Text = TotalUMFC.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblInsuranceAmount")).Text = TotalInsuranceAmount.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblOthers")).Text = TotalOthers.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblVatRecovery")).Text = TotalVatrecovery.ToString(Funsetsuffix());
    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblTax")).Text = TotalTax.ToString(Funsetsuffix());

    //    ((Label)grvRepayDetails.FooterRow.FindControl("lblTaxSetOff")).Text = TotalVatSetoff.ToString(Funsetsuffix());


    //}

    /// <summary>
    /// To set the Suffix to total
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
    private void FunPriClearRepayment()
    {
        try
        {
            ViewState["Flag"] = null;
            FunPriGetLob();
            FunPriGetBranch();
            ddlLocation2.Enabled = false;

            FunPriLoadLocation2();
            //if (ddlProduct.Items.Count > 0)
            //{
            //    ddlProduct.Items.Clear();
            //}
            if (ddlPNum.Items.Count > 0)
            {
                ddlPNum.Items.Clear();
            }
            if (ddlSNum.Items.Count > 0)
            {
                ddlSNum.Items.Clear();
            }
            //lblPoff.Enabled = true;
            //chkPoff.Enabled = true;
            FunPriSetProductEnabled();
            ucCustDetails.ClearCustomerDetails();
            ucCustomerCodeLov.FunPubClearControlValue();
            //txtCustomerCode.Focus();
            ucCustomerCodeLov.ReRegisterSearchControl("CMD");
            txtCustomerCode.Text = "";
            //lblError.Text = "";
            lblgridError.Text = "";
            FunPriValidateGrid();
            btnPrint.Visible = false;
            //chkPoff.Checked = false;
            chkAmort.Checked = false;
            lblAmort.Enabled = false;
            chkAmort.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void FunPriValidateGrid()
    {
        //pnlAssetDetails.Visible = false;
        //grvAssetDetails.DataSource = null;
        //grvAssetDetails.DataBind();
        pnlRepayDetails.Visible = false;
        grvRepayDetails.DataSource = null;
        grvRepayDetails.DataBind();
        lblAmounts.Visible = false;
        //lblPoff.Enabled = false;
        //chkPoff.Checked = false;
        //chkPoff.Enabled = false;
        btnPrint.Visible = false;
        //lblCurrency.Visible = false;
    }


    #endregion

    #region Page Events

    #region DropdownList Events

    /// <summary>
    /// To Load the Prime Account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Flag"] == null)
            {
                ViewState["Flag"] = "C";
            }
            ddlLocation2.Enabled = true;
            if (ddlBranch.SelectedIndex > 0)
            {
                FunPriLoadLocation2();

            }
            else
            {
                //FunPriGetBranch();
                FunPriLoadLocation();
                ddlLocation2.Enabled = false;

            }
            chkAmort.Checked = false;
            btnPrint.Visible = false;
            FunPriValidateGrid();
            //ddlProduct.ClearSelection();
            FunPriSetProductEnabled();
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            //ddlProduct.Items.Clear();
            //FunPriLoadProduct(intCompanyId, intCustomerId);
            if (txtCustomerCode.Text != string.Empty)
            {
                FunPriLoadPrimeAccountBasedLobBranchCustomer();
            }
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVRepaymentSchedule.IsValid = false;
        }
        //finally
        //{
        //   // if (objSerClient != null)
        //        objSerClient.Close();
        //}
    }

    protected void ddlLocation2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkAmort.Checked = false;

            if (txtCustomerCode.Text != string.Empty)
            {
                FunPriLoadPrimeAccountBasedLobBranchCustomer();
            }
            if (ddlSNum.SelectedIndex > 0 && ddlSNum.Items.Count > 0)
            {
                ddlSNum.Items.Clear();
            }
        }

        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Prime Account Number.";
            CVRepaymentSchedule.IsValid = false;
        }
    }
    /// <summary>
    /// To Load Branch and Product after selecting the LOB
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Flag"] == null)
            {
                ViewState["Flag"] = "C";
            }
            chkAmort.Checked = false;
            btnPrint.Visible = false;
            FunPriValidateGrid();
            //ddlBranch.ClearSelection();
            if (txtCustomerCode.Text == string.Empty)
            {
                FunPriGetBranch();

            }
            else
            {
                FunPriLoadBranch();
            }
            ddlLocation2.Enabled = false;
            FunPriLoadLocation2();
            //ddlProduct.ClearSelection();
            FunPriSetProductEnabled();
            //FunPriLoadBranch(intCompanyId, intUserId, "");
            //ddlProduct.Items.Clear();
            //FunPriLoadProduct(intCompanyId, intCustomerId);
            //if (txtCustomerCode.Text == string.Empty)
            //{
            //    FunProPopulateCustomer();
            //}
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            if (txtCustomerCode.Text != string.Empty)
            {
                FunPriLoadPrimeAccountBasedLobCustomer();
            }
            FunPriValidateGrid();

            //if (ddlLOB.SelectedValue == "-1")
            //{
            //    FunPriLoadPrimeAccount();
            //}
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVRepaymentSchedule.IsValid = false;
        }
        //finally
        //{
        //   // if (objSerClient != null)
        //        objSerClient.Close();
        //}
    }

    /// <summary>
    /// To Load Sub account Number
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            FunPriValidateGrid();
            chkAmort.Checked = false;
            btnPrint.Visible = false;
            ddlSNum.Items.Clear();
            lblSNum.CssClass = "styleDisplayLabel";
            byte[] byteLobs = objSerClient.FunPubGetSLA("2", intCompanyId, ddlPNum.SelectedValue, 1, hdnCustID.Value, intProgramId);
            List<ClsPubDropDownList> SANum = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlSNum.DataSource = SANum;
            ddlSNum.DataTextField = "Description";
            ddlSNum.DataValueField = "ID";
            ddlSNum.DataBind();
            byteLobs = null;
            byteLobs = objSerClient.FunPunGetHeaderLobBranchProductDetails(ddlPNum.SelectedValue);
            ClsPubHeaderLobBranchProductDetails headerLobBranchProductDetails = (ClsPubHeaderLobBranchProductDetails)DeSeriliaze(byteLobs);

            if (ddlSNum.Items.Count == 2)
            {
                ddlSNum.SelectedIndex = 1;
            }
            else
                ddlSNum.SelectedIndex = 0;


            //Lob
            //Branch
            //Product

            ddlLOB.SelectedIndex = ddlLOB.Items.IndexOf(ddlLOB.Items.FindByValue(headerLobBranchProductDetails.LobId.ToString()));
            Session["lob"] = ddlLOB.SelectedValue;
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(headerLobBranchProductDetails.LocationId.ToString()));

            //ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(headerLobBranchProductDetails.ProductId.ToString()));
            //for freeze the Product
            //Utility.ClearDropDownList(ddlProduct);
            FunPriSetProductEnabled();
            if (ddlSNum.Items.Count > 1)
            {
                lblSNum.CssClass = "styleReqFieldLabel";
                rfvSNum.Enabled = true;
            }
            else
            {
                rfvSNum.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Sub Account Number.";
            CVRepaymentSchedule.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    private void FunPriSetProductEnabled()
    {
        //try
        //{
        //    if (ddlPNum.SelectedIndex > 0 && ddlProduct.SelectedIndex > 0)
        //    {
        //        ddlProduct.Enabled = false;
        //    }
        //    else
        //    {
        //        ddlProduct.Enabled = true;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Sub Account Number.";
        //    CVRepaymentSchedule.IsValid = false;

        //}
    }



    #endregion

    #region Button (Customer / Ok / Clear / Print)

    /// <summary>
    /// To Load the Customer Details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLoadCustomer_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Flag"] == null)
            {
                ViewState["Flag"] = "LB";
            }

            HiddenField hdnCustomerId = (HiddenField)ucCustomerCodeLov.FindControl("hdnID");
            if (hdnCustomerId != null && hdnCustomerId.Value != "")
            {
                hdnCustID.Value = hdnCustomerId.Value;
                FunPubGetCustomerDetails(hdnCustID.Value);
            }
            lblAmort.Enabled = true;
            chkAmort.Enabled = true;
            chkAmort.Checked = false;
            ddlPNum.Items.Clear();
            ddlSNum.Items.Clear();
            lblSNum.CssClass = "styleDisplayLabel";
            FunPriSetProductEnabled();

            if (ViewState["Flag"] != null)
            {
                if (ViewState["Flag"].ToString() == "LB")
                {

                    if (ddlLOB.SelectedIndex <= 0)
                    {
                        FunPriLoadLob();
                    }
                    else
                    {
                        //  string s = ddlLOB.SelectedValue;
                        ddlLOB.ClearSelection();
                        FunPriLoadLob();
                        // ddlLOB.SelectedValue = s;

                    }

                }
            }
            if (ddlLOB.Items.Count <= 1)
            {
                Utility.FunShowAlertMsg(this.Page, "User doesn’t have access to appropriate Line of Business");
                FunPriClearRepayment();
                return;
            }

            if (ViewState["Flag"] != null)
            {

                if (ViewState["Flag"].ToString() == "LB")
                {
                    if (ddlBranch.SelectedIndex <= 0)
                    {

                        FunPriLoadBranch();
                        ddlLocation2.Enabled = false;
                        FunPriLoadLocation2();
                    }
                    else
                    {
                        // string b = ddlBranch.SelectedValue;
                        ddlBranch.ClearSelection();
                        FunPriLoadBranch();
                        ddlLocation2.Enabled = false;
                        FunPriLoadLocation2();
                        //  ddlBranch.SelectedValue = b;
                    }
                }
            }
            if (ddlLOB.SelectedIndex == 0 && ddlBranch.SelectedIndex == 0)
            {
                FunPriLoadPrimeAccount();
            }
            else if (ddlLOB.SelectedIndex > 0 && ddlBranch.SelectedIndex == 0)
            {
                FunPriLoadPrimeAccountBasedLobCustomer();
            }
            else
            {
                FunPriLoadPrimeAccountBasedLobBranchCustomer();
            }
            //FunPriLoadProduct(intCompanyId, intCustomerId);
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Customer Informations.";
            CVRepaymentSchedule.IsValid = false;
        }

    }

    /// <summary>
    /// To bind the Repayment and Asset Details Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            
            Session["Repay"] = null;
            FunPriValidateGrid();
            lblAmounts.Visible = true;
            //lblCurrency.Visible = true;
            btnPrint.Visible = true;
            //lblPoff.Enabled = true;
            //chkPoff.Enabled = true;
            lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            //FunPriLoadAsset(intCompanyId, PANum, SANum);
            FunPriLoadRepayDetails(intCompanyId, PANum, SANum);
            if (chkAmort.Checked && grvRepayDetails.Rows.Count <= 0)
            {
                Session["Repay"] = null;
                btnPrint.Enabled = false;
                Utility.FunShowAlertMsg(this.Page, "Account not Activated. Amortization Schedule will not be generated.");
                chkAmort.Checked = false;
                FunPriValidateGrid();
                // FunPriClearRepayment();
            }
            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            if (ddlLocation2.SelectedIndex == 0)
            {
                objHeader.Branch = ddlBranch.SelectedItem.Text;
            }
            else
            {
                objHeader.Branch = ddlLocation2.SelectedItem.Text;
            }
            objHeader.PANum = ddlPNum.SelectedItem.ToString();
            if (ddlSNum.Items.Count > 1)
            {
                objHeader.SANum = ddlSNum.SelectedItem.ToString();

            }
            else
            {
                objHeader.SANum = "--";
            }
            //objHeader.Product = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
            //objHeader.Customer = ucCustDetails.CustomerName;
            Session["Header"] = objHeader;
            ClsPubCustomer ObjCustomer = new ClsPubCustomer();
            //ObjCustomer.Customer = ucCustDetails.CustomerName + " (" + ucCustDetails.CustomerCode+")";
            int intcustomerTypeID = 13;//12 individual , 13 Non- Individual
            //ObjCustomer.CustomerCode = ucCustDetails.CustomerCode;
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Customer_Id", hdnCustID.Value);
            Procparam.Add("@COMPANY_ID", intCompanyId.ToString());
            DataTable dtaddress = Utility.GetDefaultData("S3G_RPT_GetCustomerAddress1", Procparam);
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
                intcustomerTypeID = Convert.ToInt32(dtaddress.Rows[0]["TYPEID"].ToString());
            }
            else
            {
                ObjCustomer.Address = ucCustDetails.CustomerAddress;
            }
            //ObjCustomer.Mobile = ucCustDetails.Mobile;
            //ObjCustomer.EMail = ucCustDetails.EmailID;
            //ObjCustomer.WebSite = ucCustDetails.Website;
            Session["CustomerInfo"] = ObjCustomer;
            Session["customerTypeID"] = intcustomerTypeID;

            //Fin Amount
            //if (grvAssetDetails != null)
            //{
            //    if (grvAssetDetails.Rows.Count > 0)
            //    {

            //        Session["FinanceAmt"] = ((Label)grvAssetDetails.Rows[0].FindControl("lblFinanceAmt")).Text;
            //        Session["Terms"] = ((Label)grvAssetDetails.Rows[0].FindControl("lblTerms")).Text;
            //    }
            //    else
            //    {
            //        Session["FinanceAmt"] = "";
            //        Session["Terms"] = "";

            //    }
            //}
            //IRR
            //if (grvAssetDetails != null)
            //{
            //    if (grvAssetDetails.Rows.Count > 0)
            //    {
            //        Session["IRR"] = ((Label)grvAssetDetails.Rows[0].FindControl("lblIRR")).Text;
            //    }
            //    else
            //    {
            //        Session["IRR"] = "";
            //    }
            //}

            if (grvRepayDetails.Rows.Count > 0)
            {
                btnPrint.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Due to Data Problem, Unable to Load Asset/Repayment Details Grid.";
            CVRepaymentSchedule.IsValid = false;
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
            FunPriClearRepayment();

        }
        catch (Exception ex)
        {
            CVRepaymentSchedule.ErrorMessage = "Error in Clear.";
            CVRepaymentSchedule.IsValid = false;
        }
    }

    //protected void chkPoff_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkPoff.Checked == true)
    //    {
    //        Utility.FunShowAlertMsg(this.Page, "Asset details will not be printed in Report");
    //    }
    //}

    protected void chkArrear_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAmort.Checked == true)
        {
           //Utility.FunShowAlertMsg(this.Page, " ");
            lblTotal.Visible = true;
            txtTotal.Visible = true;
            txtTotal.ReadOnly = true;
        }
        
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //if (chkPoff.Checked)
        //{
        //    Session["IsAssetPrintOff"] = "1";
        //}
        //else
        //{
        //    Session["IsAssetPrintOff"] = "0";
        //}
        //if (chkAmort.Checked)
        //{
        //    Session["Heading"] = "Amortization Schedule";
        //    Session["Type"] = "A";
        //}
        //else
        if (chkAmort.Checked)

        {
            Session["Heading"] = "Repayment Structure";
            Session["Type"] = "R";
        }
        string strScipt = "window.open('../Reports/DiscountedRepaymentReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Repay", strScipt, true);

    }

    #endregion

    #endregion
}