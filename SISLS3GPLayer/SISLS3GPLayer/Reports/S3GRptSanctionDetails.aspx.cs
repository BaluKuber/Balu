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

public partial class Reports_S3GRptSanctionDetails : ApplyThemeForProject
{
    #region Variable Declaration
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    string PANum;
    string SANum;
    string RegionId;
    int intProgramId = 144;
    bool Is_Active;
    public string strDateFormat;
    decimal TotalFinamtSaught;
    decimal TotalFinamtOffered;
    decimal TotalDisbursableAmt;
    decimal TotalDisbursedAmt;

    Dictionary<string, string> Procparam;
    string strPageName = "Sanction Details";
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient;
    #endregion

    #region Page Load

    /// <summary>
    /// This event is handled for load the page
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Sanction Details Page.";
            CVSanctionDetails.IsValid = false;
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
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
            //txtStartDate.Attributes.Add("readonly", "readonly");
            //txtEndDate.Attributes.Add("readonly", "readonly");
            /* Changed Date Control end - 30-Nov-2012 */
            #endregion
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
                FunPubLoadProduct();
                FunPubLoadDenomination();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Sanction Details page");
        }

    }

    /// <summary>
    /// To Load LOB
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    private void FunPriLoadLob()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] byteLobs = objSerClient.FunPubLOB(intCompanyId, intUserId, intProgramId);
            List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlLOB.DataSource = lobs;
            ddlLOB.DataTextField = "Description";
            ddlLOB.DataValueField = "ID";
            ddlLOB.DataBind();
            if (ddlLOB.Items.Count == 2)
                ddlLOB.SelectedIndex = 1;
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
            int intlob_Id = 0;
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlRegion.DataSource = Branch;
            ddlRegion.DataTextField = "Description";
            ddlRegion.DataValueField = "ID";
            ddlRegion.DataBind();
            ddlRegion.Items[0].Text = "--ALL--";
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
            if (ddlLOB.SelectedIndex > 0)
                intlob_Id = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, intProgramId, intlob_Id);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";

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
            if (ddlRegion.SelectedIndex > 0)
                Location1 = Convert.ToInt32(ddlRegion.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetLocation2(intProgramId, intUserId, intCompanyId, LobId, Location1);
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);

            ddlBranch.DataSource = Location;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "--ALL--";
            if (ddlBranch.Items.Count == 2)
            {
                if (ddlRegion.SelectedIndex != 0)
                {
                    ddlBranch.SelectedIndex = 1;
                   // Utility.ClearDropDownList(ddlBranch);
                }
                else
                    ddlBranch.SelectedIndex = 0;
            }
            else
            {
                ddlBranch.Items[0].Text = "--ALL--";
                ddlBranch.SelectedIndex = 0;
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
    /// To Load Product
    /// </summary>
    /// <param name="intCompany_id"></param>
    /// <param name="intUser_id"></param>
    public void FunPubLoadProduct()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            int LobId = 0;
            if (ddlLOB.SelectedIndex > 0)
                LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            byte[] byteLobs = objSerClient.FunPubGetProductDetails(intCompanyId, LobId);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlProduct.DataSource = Product;
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();
            if (ddlProduct.Items.Count == 2)
                ddlProduct.SelectedIndex = 1;
            else
                ddlProduct.SelectedIndex = 0;
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

    /// <summary>
    /// To Validate the Dates
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

            btnPrint.Enabled = true;

            if (Convert.ToInt32(ddlRegion.SelectedValue) > 0 && Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                FunPriLoadSanctionDetails();
            }
            else if (Convert.ToInt32(ddlRegion.SelectedValue) < 0 && Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                FunPriLoadSanctionDetails();
            }
            else
            {
                FunPriLoadSanctionSummary();
            }
            #region To Get Header Details for Report
            ClsPubHeaderDetails objHeader = new ClsPubHeaderDetails();
            objHeader.Lob = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            Session["lob"] = (ddlLOB.SelectedItem.Text.Split('-'))[1].ToString();
            if (Convert.ToInt32(ddlRegion.SelectedValue) > 0)
            {
                objHeader.Region = ddlRegion.SelectedItem.Text;
            }
            else
            {
                objHeader.Region = "All";
                //((Label)grvSummary.FooterRow.FindControl("lblRegion")).Text;
            }
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                objHeader.Branch = ddlBranch.SelectedItem.Text;
                Session["Location2"] = ddlBranch.SelectedItem.Text;
            }
            else
            {
                objHeader.Branch = "All";
                Session["Location2"] ="All";
                //((Label)grvSummary.FooterRow.FindControl("lblBranch")).Text;
            }

            if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
            {
                objHeader.Product = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
                Session["Product"] = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
            }
            else
            {
                objHeader.Product = "All";
                Session["Product"] = "All";
                //((Label)grvSummary.FooterRow.FindControl("lblProduct")).Text;
            }
            objHeader.StartDate = txtStartDate.Text;
            objHeader.EndDate = txtEndDate.Text;
            Session["Title"] = "Sanction Details For the Period From " + " " + txtStartDate.Text + " " + "To" + " " + txtEndDate.Text;
            Session["Header"] = objHeader;
            if (grvSanctionDetails != null && grvSanctionDetails.FooterRow != null)
            {
                Session["DisbursableAmt"] = ((Label)grvSanctionDetails.FooterRow.FindControl("lblDisbursableAmt")).Text;
                Session["DisbursedAmt"] = ((Label)grvSanctionDetails.FooterRow.FindControl("lblDisbursedAmt")).Text;
            }
            else
            {
                Session["DisbursableAmt"] = Convert.ToInt32(0).ToString(Funsetsuffix());
                Session["DisbursedAmt"] = Convert.ToInt32(0).ToString(Funsetsuffix());
            }
            if (ddlDenomination.SelectedValue == "1")
            {
                Session["Denomination"] = "[All Amounts are In" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + ddlDenomination.SelectedItem.Text + "]";
            }
            #endregion

            if (grvSanctionDetails.Rows.Count > 0 || grvSummary.Rows.Count > 0)
            {
                btnPrint.Visible = true;
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
                btnPrint.Visible = true;
                lblAmounts.Visible = false;
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    /// <summary>
    /// To Load the Sanction Details Grid
    /// </summary>
    private void FunPriLoadSanctionDetails()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            pnlSummary.Visible = false;
            grvSummary.DataSource = null;
            grvSummary.DataBind();
            lblError.Text = "";
            pnlSanctionDetails.Visible = true;
            divSanctionDetails.Style.Add("display", "block");
            ClsPubSanctionParameterDetails ObjSanctionParameter = new ClsPubSanctionParameterDetails();
            ObjSanctionParameter.CompanyId = intCompanyId;
            ObjSanctionParameter.UserId = intUserId;
            ObjSanctionParameter.ProgramId = intProgramId;
            ObjSanctionParameter.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlRegion.SelectedIndex > 0)
            {
                ObjSanctionParameter.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            else
            {
                ObjSanctionParameter.LocationId1 = 0;
            }
            if (ddlBranch.SelectedIndex > 0)
            {
                ObjSanctionParameter.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            else
            {
                ObjSanctionParameter.LocationId2 = 0;
            }
            if (ddlProduct.SelectedIndex > 0)
            {
                ObjSanctionParameter.ProductId = ddlProduct.SelectedValue;
            }
            else
            {
                ObjSanctionParameter.ProductId = "0";
            }
            ObjSanctionParameter.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjSanctionParameter.EndDate = Utility.StringToDate(txtEndDate.Text);
            ObjSanctionParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            ObjSanctionParameter.IsDetail = true;

            byte[] byteSanctionDetail = ClsPubSerialize.Serialize(ObjSanctionParameter, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetSanctionDetails(byteSanctionDetail);
            byte[] byteSanction = objSerClient.FunPubGetSanctionReport(byteSanctionDetail);//for report
            ClsPubSumOfSanctionDetails sumOfSanctionDetails = (ClsPubSumOfSanctionDetails)DeSeriliaze(byteLobs);
            ClsPubSanctionDetailsReport SanctionDetailsReport = (ClsPubSanctionDetailsReport)DeSeriliaze(byteSanction); //for report
            Session["Sanction"] = SanctionDetailsReport.Sanction;
            Session["Disbursable"] = SanctionDetailsReport.Disbursable;
            Session["Disbursed"] = SanctionDetailsReport.Disbursed;

            grvSanctionDetails.DataSource = sumOfSanctionDetails.SanctionDetails;
            grvSanctionDetails.DataBind();
            if (ddlRegion.SelectedIndex > 0)
            {
                if (grvSanctionDetails != null)
                    grvSanctionDetails.Columns[0].Visible = false;
            }
            else
            {
                if (grvSanctionDetails != null)
                    grvSanctionDetails.Columns[0].Visible = true;
            }

            //if (ddlBranch.SelectedIndex != 0)
            //{
            //    if (grvSanctionDetails != null)
            //    grvSanctionDetails.Columns[1].Visible = false;
            //}
            //else
            //{
            //    if (grvSanctionDetails != null)
            //    grvSanctionDetails.Columns[1].Visible = true;
            //}
            if (ddlProduct.SelectedIndex > 0)
            {
                if (grvSanctionDetails != null)
                    grvSanctionDetails.Columns[1].Visible = false;
            }
            else
            {
                if (grvSanctionDetails != null)
                    grvSanctionDetails.Columns[1].Visible = true;
            }
            if (grvSanctionDetails != null && grvSanctionDetails.FooterRow != null && grvSanctionDetails.Columns[0].Visible)
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblGrandTotalR")).Visible = grvSanctionDetails.Columns[0].Visible;
            //else if (grvSanctionDetails != null && grvSanctionDetails.FooterRow != null && grvSanctionDetails.Columns[1].Visible)
            //    ((Label)grvSanctionDetails.FooterRow.FindControl("lblTotalB")).Visible = grvSanctionDetails.Columns[1].Visible;
            else if (grvSanctionDetails != null && grvSanctionDetails.FooterRow != null && grvSanctionDetails.Columns[1].Visible)
            {
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblTotP")).Visible = grvSanctionDetails.Columns[1].Visible;
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblGranTotalA")).Visible = false;
            }
            else
            {
                if (grvSanctionDetails.FooterRow != null)
                {
                    ((Label)grvSanctionDetails.FooterRow.FindControl("lblTotP")).Visible = false;
                    ((Label)grvSanctionDetails.FooterRow.FindControl("lblGranTotalA")).Visible = true;
                }
            }
            if (grvSanctionDetails.Rows.Count != 0)
            {
                grvSanctionDetails.HeaderRow.Style.Add("position", "relative");
                grvSanctionDetails.HeaderRow.Style.Add("z-index", "auto");
                grvSanctionDetails.HeaderRow.Style.Add("top", "auto");

            }

            if (grvSanctionDetails != null && grvSanctionDetails.Rows.Count == 0)
            {
                Session["Sanction"] = null;
                Session["Disbursable"] = null;
                Session["Disbursed"] = null;
                btnPrint.Enabled = false;
                //grvSanctionDetails.EmptyDataText = "No Records Found";
                lblError.Text = "No Records Found";
                grvSanctionDetails.DataBind();
            }
            else
            {
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblFinamtSaught")).Text = sumOfSanctionDetails.TOTALFINANCEAMOUNTSOUGHT.ToString(Funsetsuffix());
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblFinamtOffered")).Text = sumOfSanctionDetails.TOTALFINANCEAMOUNTOFFERED.ToString(Funsetsuffix());
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblDisbursableAmt")).Text = sumOfSanctionDetails.TOTALDISBURSABLE_AMOUNT.ToString(Funsetsuffix());
                ((Label)grvSanctionDetails.FooterRow.FindControl("lblDisbursedAmt")).Text = sumOfSanctionDetails.TOTALDISBURSED_AMOUNT.ToString(Funsetsuffix());
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
    /// To Load the Sanction Summary Details Grid
    /// </summary>
    private void FunPriLoadSanctionSummary()
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            btnPrint.Enabled = true;
            pnlSanctionDetails.Visible = false;
            pnlSummary.Visible = true;
            lblgridError.Text = "";
            divSummary.Style.Add("display", "block");
            ClsPubSanctionParameterDetails ObjSanctionParameter = new ClsPubSanctionParameterDetails();
            ObjSanctionParameter.CompanyId = intCompanyId;
            ObjSanctionParameter.UserId = intUserId;
            ObjSanctionParameter.ProgramId = intProgramId;
            ObjSanctionParameter.LobId = Convert.ToInt32(ddlLOB.SelectedValue);
            if (ddlRegion.SelectedIndex > 0)
            {
                ObjSanctionParameter.LocationId1 = Convert.ToInt32(ddlRegion.SelectedValue);
            }
            else
            {
                ObjSanctionParameter.LocationId1 = 0;
            }
            if (ddlBranch.SelectedIndex > 0)
            {
                ObjSanctionParameter.LocationId2 = Convert.ToInt32(ddlBranch.SelectedValue);
            }
            else
            {
                ObjSanctionParameter.LocationId2 = 0;
            }
            if (ddlProduct.SelectedIndex > 0)
            {
                ObjSanctionParameter.ProductId = ddlProduct.SelectedValue;
            }
            else
            {
                ObjSanctionParameter.ProductId = "0";
            }
            ObjSanctionParameter.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjSanctionParameter.EndDate = Utility.StringToDate(txtEndDate.Text);
            ObjSanctionParameter.Denomination = Convert.ToDecimal(ddlDenomination.SelectedValue);
            ObjSanctionParameter.IsDetail = false;
            byte[] byteSanctionDetail = ClsPubSerialize.Serialize(ObjSanctionParameter, SerializationMode.Binary);

            byte[] byteLobs = objSerClient.FunPubGetSanctionDetails(byteSanctionDetail);
            ClsPubSumOfSanctionDetails sumOfSanctionDetails = (ClsPubSumOfSanctionDetails)DeSeriliaze(byteLobs);
            TotalFinamtSaught = sumOfSanctionDetails.SanctionDetails.Sum(ClsPubSumOfSanctionDetails => ClsPubSumOfSanctionDetails.FINANCEAMOUNTSOUGHT);
            TotalFinamtOffered = sumOfSanctionDetails.SanctionDetails.Sum(ClsPubSumOfSanctionDetails => ClsPubSumOfSanctionDetails.FINANCEAMOUNTOFFERED);
            TotalDisbursableAmt = sumOfSanctionDetails.SanctionDetails.Sum(ClsPubSumOfSanctionDetails => ClsPubSumOfSanctionDetails.DISBURSABLE_AMOUNT);
            TotalDisbursedAmt = sumOfSanctionDetails.SanctionDetails.Sum(ClsPubSumOfSanctionDetails => ClsPubSumOfSanctionDetails.DISBURSED_AMOUNT);
            Session["Summary"] = sumOfSanctionDetails.SanctionDetails;
            grvSummary.DataSource = sumOfSanctionDetails.SanctionDetails;
            grvSummary.DataBind();
            if (ddlRegion.SelectedIndex > 0)
            {
                if (grvSummary != null)
                    grvSummary.Columns[0].Visible = false;


            }
            else
            {
                if (grvSummary != null)
                    grvSummary.Columns[0].Visible = true;

            }
            //if (ddlBranch.SelectedIndex > 0)
            //{
            //    if (grvSummary != null)
            //    grvSummary.Columns[1].Visible = false;

            //}
            //else
            //{
            //    if (grvSummary != null)
            //    grvSummary.Columns[1].Visible = true;


            //}
            if (ddlProduct.SelectedIndex > 0)
            {
                if (grvSummary != null)
                    grvSummary.Columns[1].Visible = false;

            }
            else
            {
                if (grvSummary != null)
                    grvSummary.Columns[1].Visible = true;

            }

            if (grvSummary != null && grvSummary.FooterRow != null && grvSummary.Columns[0].Visible)
                ((Label)grvSummary.FooterRow.FindControl("lblGrndTotalR")).Visible = grvSummary.Columns[0].Visible;
            //else if (grvSummary != null && grvSummary.FooterRow != null && grvSummary.Columns[1].Visible)
            //    ((Label)grvSummary.FooterRow.FindControl("lblGranTotalB")).Visible = grvSummary.Columns[1].Visible;
            else if (grvSummary != null && grvSummary.FooterRow != null && grvSummary.Columns[1].Visible)
                ((Label)grvSummary.FooterRow.FindControl("lblGranTotalP")).Visible = grvSummary.Columns[1].Visible;
            if (grvSummary.Rows.Count != 0)
            {
                grvSummary.HeaderRow.Style.Add("position", "relative");
                grvSummary.HeaderRow.Style.Add("z-index", "auto");
                grvSummary.HeaderRow.Style.Add("top", "auto");

            }
            if (grvSummary != null && grvSummary.Rows.Count == 0)
            {
                Session["Summary"] = null;
                btnPrint.Enabled = false;
                lblgridError.Text = "No Records Found";
                grvSummary.DataBind();
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

    private void FunPriDisplayTotal()
    {
        ((Label)grvSummary.FooterRow.FindControl("lblTotFinanceAmountSaught")).Text = TotalFinamtSaught.ToString(Funsetsuffix());
        ((Label)grvSummary.FooterRow.FindControl("lblTotFinanceAmountOffered")).Text = TotalFinamtOffered.ToString(Funsetsuffix());
        ((Label)grvSummary.FooterRow.FindControl("lblTotDisbursableAmount")).Text = TotalDisbursableAmt.ToString(Funsetsuffix());
        ((Label)grvSummary.FooterRow.FindControl("lblTotDisbursedAmount")).Text = TotalDisbursedAmt.ToString(Funsetsuffix());

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

    /// <summary>
    /// To Clear the Values
    /// </summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
    private void FunPriClearSanction()
    {
        try
        {
            ddlLOB.ClearSelection();
            ddlLOB.Focus();
            FunPriLoadBranch();
            FunPriLoadLocation();
            ddlBranch.Enabled = false;

            FunPubLoadProduct();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            lblError.Text = "";
            lblgridError.Text = "";
            ddlDenomination.ClearSelection();
            FunPriValidateGrid();
            ClearSession();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }

    private void FunPriValidateGrid()
    {

        pnlSanctionDetails.Visible = false;
        grvSanctionDetails.DataSource = null;
        grvSanctionDetails.DataBind();
        pnlSummary.Visible = false;
        grvSummary.DataSource = null;
        grvSummary.DataBind();
        btnPrint.Visible = false;
        lblAmounts.Visible = false;
        //lblCurrency.Visible = false;
    }

    private void ClearSession()
    {
        Session["Header"] = null;
        Session["Summary"] = null;
        Session["Detail"] = null;

    }
    #endregion

    #region Page Events

    //#region TextBox Events

    ///// <summary>
    ///// To validate the Grid
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void txtStartDate_TextChanged (object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriValidateGrid();

    //    }
    //    catch (Exception ex)
    //    {
    //        CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Grid.";
    //        CVSanctionDetails.IsValid = false;
    //    }

    //}

    ///// <summary>
    ///// To validate the Grid
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void txtEndDate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        FunPriValidateGrid();

    //    }
    //    catch (Exception ex)
    //    {
    //        CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Grid.";
    //        CVSanctionDetails.IsValid = false;
    //    }

    //}

    //#endregion

    #region DropdownList Events

    /// <summary>
    /// To Load the Branch
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        objSerClient = new ReportAccountsMgtServicesClient();
        try
        {
            FunPriValidateGrid();
            FunPubLoadProduct();
            //FunPubLoadProduct(intCompanyId);
            ddlBranch.Enabled = true;
            if (ddlRegion.SelectedIndex > 0)
            {
                FunPriLoadLocation2();
            }
            else
            {
                ddlBranch.Enabled = false;
                FunPriLoadLocation();
            }
            //added by saranya on 10/24/2011 
            ddlDenomination.ClearSelection();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            //end 
        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVSanctionDetails.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }

    /// <summary>
    /// To Validate the Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();
            FunPubLoadProduct();
            //added by saranya on 10/24/2011 
            ddlDenomination.ClearSelection();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            //end 

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVSanctionDetails.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate The Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();

            //Added after 1st round Completing in Tesing
            FunPriLoadLocation();
            ddlBranch.Enabled = false;
            FunPriLoadBranch();
            FunPubLoadProduct();
            ddlDenomination.ClearSelection();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ClearSession();
            //end

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVSanctionDetails.IsValid = false;
        }
    }

    /// <summary>
    /// To Validate The Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVSanctionDetails.IsValid = false;
        }
    }

    #endregion

    #region Grid Events

    protected void grvSanctionDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Label)e.Row.FindControl("lblFinSaught")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblFinSaught")).Text).ToString(Funsetsuffix());
                ((Label)e.Row.FindControl("lblFinOffered")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblFinOffered")).Text).ToString(Funsetsuffix());

            }
        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem.";
            CVSanctionDetails.IsValid = false;
        }
    }

    protected void grvDisbursable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //   if(e.Row.Cells[0] != null &&  e.Row.Cells[1].Text!=string.Empty)
            //        e.Row.Cells[1].Text = Convert.ToDecimal(e.Row.Cells[1].Text).ToString(Funsetsuffix());

            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Label)e.Row.FindControl("lblDisbursableAmnt")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblDisbursableAmnt")).Text).ToString(Funsetsuffix());

            }

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem.";
            CVSanctionDetails.IsValid = false;
        }
    }

    protected void grvDisbursed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //   if(e.Row.Cells[0] != null &&  e.Row.Cells[1].Text!=string.Empty)
            //        e.Row.Cells[1].Text = Convert.ToDecimal(e.Row.Cells[1].Text).ToString(Funsetsuffix());

            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Label)e.Row.FindControl("lblDisbursedAmnt")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblDisbursedAmnt")).Text).ToString(Funsetsuffix());

            }
        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem.";
            CVSanctionDetails.IsValid = false;
        }
    }

    protected void grvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Label)e.Row.FindControl("lblFinamountSaught")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblFinamountSaught")).Text).ToString(Funsetsuffix());
                ((Label)e.Row.FindControl("lblFinamountOffered")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblFinamountOffered")).Text).ToString(Funsetsuffix());
                ((Label)e.Row.FindControl("lblDisbursableAmount")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblDisbursableAmount")).Text).ToString(Funsetsuffix());
                ((Label)e.Row.FindControl("lblDisbursedAmount")).Text = Convert.ToDecimal(((Label)e.Row.FindControl("lblDisbursedAmount")).Text).ToString(Funsetsuffix());
            }
        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem.";
            CVSanctionDetails.IsValid = false;
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
            ClearSession();
            FunPriValidateFromEndDate();
            //if (grvSanctionDetails.Rows.Count > 0 || grvSummary.Rows.Count > 0)
            //{
            //    btnPrint.Visible = true;
            //}

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Due to Data Problem, Unable to Load Sanction Details Grid.";
            CVSanctionDetails.IsValid = false;
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
            FunPriClearSanction();

        }
        catch (Exception ex)
        {
            CVSanctionDetails.ErrorMessage = "Unable to Clear.";
            CVSanctionDetails.IsValid = false;
        }

    }

    /// <summary>
    /// export to crystal format
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (grvSummary.Visible)
        {
            Session["IsSummary"] = "TRUE";
        }
        else
        {
            Session["IsSummary"] = "FALSE";
        }
        string strScipt = "window.open('../Reports/S3GRptSanctionDetailsReport.aspx', 'sanc','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "sanc", strScipt, true);

    }

    #endregion

    #endregion
}

