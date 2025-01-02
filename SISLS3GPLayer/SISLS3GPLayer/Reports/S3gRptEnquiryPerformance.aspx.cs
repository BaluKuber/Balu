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
using System.Globalization;
using System.Collections.Generic;
using S3GBusEntity;
using S3GBusEntity.Reports;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using S3GBusEntity;
using System.Net.Mail;
using System.Net;
using System.ServiceModel;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Runtime.Serialization;
using ReportAccountsMgtServicesReference;
using ReportOrgColMgtServicesReference;
public partial class Reports_S3gRptEnquiryPerformance : ApplyThemeForProject
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    string RegionId;
    bool Is_Active = true;
    string strPageName = "Enquiry";
    int ProgramId = 186;
    int LOBId;
    public int LobId;
    int LocationId;
    int TotalRecCount;
    decimal TotalRecVal;
    int TotalSucCount;
    decimal TotalSucVal;
    int TotalFollowCount;
    decimal TotalFollowVal;
    int TotalRejCount;
    decimal TotalRejVal;
   // public int LocationId;
    Dictionary<string, string> Procparam;
    DataTable dtTable = new DataTable();

    string strFilePath = string.Empty;
    //ReportAccountsMgtServicesClient objSerClient = new ReportAccountsMgtServicesClient();
    //ReportOrgColMgtServicesClient ObjOrgColClient = new ReportOrgColMgtServicesClient();
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient ObjOrgColClient;
    ReportAccountsMgtServicesReference.ReportAccountsMgtServicesClient objSerClient;
    S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient;
    //string cutoff;
    //decimal denomination;
    public string strDateFormat;

    protected void Page_Load(object sender, EventArgs e)
    {
        FunPriLoadPage();
        //ClearSession();
    }

    private void FunPriLoadPage()
    {
        try
        {
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            Session["Company"] = ObjUserInfo.ProCompanyNameRW;

            intUserId = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();
            Session["AccountingCurrency"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";

            strDateFormat = ObjS3GSession.ProDateFormatRW;
            CalendarExtender1.Format = strDateFormat;
            CalendarExtender2.Format = strDateFormat;
            /* Changed Date Control start - 30-Nov-2012 */
            //txtStartDate.Attributes.Add("readonly", "readonly");
            //txtEndDate.Attributes.Add("readonly", "readonly");
            txtStartDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtStartDate.ClientID + "','" + strDateFormat + "',true,  false);");
            txtEndDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtEndDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            //int intCompanyId, int intUser_id, int ProgramId, int LOBId
            //(int ProgramId, int UserId, int CompanyId, int LOBId, int LocationId)

            if (!IsPostBack)
            {
                //FunPriLoadLob(intCompanyId, intUserId, Is_Active, ProgramId);
                FunPriLoadLob(intCompanyId, intUserId, ProgramId);
                //FunPriLoadRegion(intCompanyId, Is_Active, intUserId);intCompanyId, intUserId, ProgramId, LOBId
                //FunPriLoadBranch(intCompanyId, intUserId, ProgramId, LOBId);
                FunPriLoadLocation1(intCompanyId, intUserId, ProgramId, LobId);
                //FunPriLoadLocation2(ProgramId, intUserId, intCompanyId, LOBId, LocationId);
                FunPriLoadLocation(intCompanyId, intUserId, ProgramId, LobId);
                FunPubLoadProduct(intCompanyId, LOBId);
                FunPubLoadDenomination();

            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load Enquiry page");
        }
    }
    private void FunPriLoadLob(int intCompanyId, int intUserId, int ProgramId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubSOAGetLOB(intCompanyId, intUserId, ProgramId);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddLOB.DataSource = LOB;
            ddLOB.DataTextField = "Description";
            ddLOB.DataValueField = "ID";
            ddLOB.DataBind();
            ddLOB.Items[0].Text = "Select";
            if (ddLOB.Items.Count == 2)
            {
                ddLOB.SelectedIndex = 1;
            }
            else
            {
                ddLOB.SelectedIndex = 0;
            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to load LOB");
        }
        finally
        {
            objSerClient.Close();
        }
    }
    //private void FunPriLoadLob(int intCompanyId, int intUser_id, bool Is_Active, int ProgramId)
    //{
    //    try
    //    {

    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        byte[] byteLobs = objSerClient.FunPubEnQGetLOB(intCompanyId, intUser_id, true, ProgramId);
    //        List<ClsPubDropDownList> lobs = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
    //        ddLOB.DataSource = lobs;
    //        ddLOB.DataTextField = "Description";
    //        ddLOB.DataValueField = "ID";
    //        ddLOB.DataBind();

    //        if (ddLOB.Items.Count == 2)
    //            ddLOB.SelectedIndex = 1;
    //        else
    //            ddLOB.SelectedIndex = 0;
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
    //private void FunPriLoadRegion(int intCompanyId, bool Is_active, int intUserId)
    //{

    //    try
    //    {

    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        byte[] byteLobs = objSerClient.FunPubGetRegion(intCompanyId, true, intUserId);
    //        List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
    //        DDRegion.DataSource = Region;
    //        DDRegion.DataTextField = "Description";
    //        DDRegion.DataValueField = "ID";
    //        //if (DDRegion.Items.Count == 2)
    //        //    DDRegion.SelectedIndex = 1;
    //        //else
    //        //    DDRegion.SelectedIndex = 0;
    //        DDRegion.DataBind();

    //        if (DDRegion.Items.Count == 2)
    //            DDRegion.SelectedIndex = 1;
    //        else
    //            DDRegion.SelectedIndex = 0;
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
    private void FunPriLoadBranch(int intCompanyId, int intUser_id, int ProgramId, int LOBId)
    {
        try
        {

            objSerClient = new ReportAccountsMgtServicesClient();
            DDRegion.Items.Clear();
            //int LOBId = 0;
            if (ddLOB.SelectedIndex > 0)
                LOBId = Convert.ToInt32(ddLOB.SelectedValue);

            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUser_id, ProgramId, LOBId);
            List<ClsPubDropDownList> Region = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            DDRegion.DataSource = Region;
            DDRegion.DataTextField = "Description";
            DDRegion.DataValueField = "ID";
            DDRegion.DataBind();
            DDRegion.Items[0].Text = "--ALL--";
            if (DDRegion.Items.Count == 2)
                DDRegion.SelectedIndex = 1;
            else
                DDRegion.SelectedIndex = 0;
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
    /// To Load Location 2
    /// </summary>
    /// <param name="ProgramId"></param>
    /// <param name="UserId"></param>
    /// <param name="CompanyId"></param>
    /// <param name="LobId"></param>
    /// <param name="LocationId"></param>
    //private void FunPriLoadLocation2(int ProgramId, int UserId, int intCompanyId, int LOBId, int LocationId)
    //{
    //    try
    //    {
    //        objSerClient = new ReportAccountsMgtServicesClient();
    //        if (ddLOB.SelectedIndex > 0)
    //        {
    //            LOBId = Convert.ToInt32(ddLOB.SelectedValue);
    //        }

    //        if (DDRegion.SelectedIndex > 0)
    //        {
    //            LocationId = Convert.ToInt32(DDRegion.SelectedValue);
    //        }
    //        byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, UserId, intCompanyId, LOBId, LocationId);
    //        List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
    //        ddlBranch.DataSource = Branch;
    //        //ddllocation2.DataSource = Branch;
    //        ddlBranch.DataTextField = "Description";
    //        ddlBranch.DataValueField = "ID";
    //        ddlBranch.DataBind();
    //        ddlBranch.Items[0].Text = "--ALL--";
    //        if (ddlBranch.Items.Count == 2)
    //            ddlBranch.SelectedIndex = 1;
    //        else
    //            ddlBranch.SelectedIndex = 0;

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

    public void FunPubLoadProduct(int intCompanyId, int LOBId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.FunPubGetProductDetails(intCompanyId, LOBId);
            List<ClsPubDropDownList> Product = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            ddlProduct.DataSource = Product;
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataValueField = "ID";
            ddlProduct.DataBind();
            ddlProduct.Items[0].Text = "--ALL--";
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


    public void FunPubLoadDenomination()
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            byte[] byteLobs = objSerClient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            DropDenomination.DataSource = Denomination;
            DropDenomination.DataTextField = "Description";
            DropDenomination.DataValueField = "ID";
            DropDenomination.DataBind();
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

    private void FunPriValidateFromEndDate()
    {
        try
        {
            //DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDate.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDate.Text, dtformat))) // start date should be less than or equal to the enddate
            //{
            if ((!(string.IsNullOrEmpty(txtStartDate.Text))) && (!(string.IsNullOrEmpty(txtEndDate.Text))))                                   // If start and end date is not empty
            {
                //if (Convert.ToDateTime(DateTime.Parse(Txtdatefm.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(Txtdateto.Text, dtformat))) // start date should be less than or equal to the enddate
                //Utility.StringToDate
                if ((Utility.StringToDate(txtStartDate.Text)) > (Utility.StringToDate(txtEndDate.Text))) // start date should be less than or equal to the enddate
                {
                    Utility.FunShowAlertMsg(this, "End Date should be greater than or equal to Start Date");
                    txtEndDate.Text = "";
                    pnlEnquiry.Visible = false;
                    btnPrint.Visible = false;
                    return;
                }
            }
            //}
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
            FuncgridBind();
            ClsPubEnquiryHeaderDetails objHeader = new ClsPubEnquiryHeaderDetails();
            objHeader.LOB_ID = (ddLOB.SelectedItem.Text.Split('-'))[1].ToString();
            if (Convert.ToInt32(DDRegion.SelectedValue) > 0)
            {
                //objHeader.RegionId = (DDRegion.SelectedItem.Text.Split('-'))[1].ToString();
                objHeader.LOCATION_ID1 = (DDRegion.SelectedItem.Text.ToString());
            }
            else
            {
                objHeader.LOCATION_ID1 = "All";
            }
            if (Convert.ToInt32(ddlBranch.SelectedValue) > 0)
            {
                objHeader.LOCATION_ID2 = (ddlBranch.SelectedItem.Text.ToString());
            }
            else
            {
                objHeader.LOCATION_ID2 = "All";

            }
            if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
            {
                objHeader.ProductId = (ddlProduct.SelectedItem.Text.Split('-'))[1].ToString();
            }
            else
            {
                objHeader.ProductId = "All";
            }

            objHeader.StartDate = Utility.StringToDate(txtStartDate.Text);
            objHeader.EndDate = Utility.StringToDate(txtEndDate.Text);
            objHeader.Denomination = Convert.ToDecimal(DropDenomination.SelectedValue);
            Session["EnquiryHeader"] = objHeader;
            if (DropDenomination.SelectedValue =="1")
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Denomination"] = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + DropDenomination.SelectedItem.Text + "]";
            }
            //TextObject Currency = (TextObject)rptd.ReportDefinition.Sections["ReportHeaderSection1"].ReportObjects["txtMoney"];
            //Currency.Text = Session["Denomination"].ToString();
            if (grvEnquirydetails.Rows.Count > 0)
            {
                btnPrint.Visible = true;
                // BtnEmail.Visible = true;
                lblAmounts.Visible = true;
            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    private void FunPriValidateGrid()
    {
        
        pnlEnquiry.Visible = false;
        grvEnquirydetails.DataSource = null;
        grvEnquirydetails.DataBind();
        pnlRC.Visible = false;
        grvRC.DataSource = null;
        grvRC.DataBind();
        PnlSc.Visible = false;
        GrvSc.DataSource = null;
        GrvSc.DataBind();
        PnlUFC.Visible = false;
        GrvFollowUp.DataSource = null;
        GrvFollowUp.DataBind();
        PRejcount.Visible = false;
        GrvRejected.DataSource = null;
        GrvRejected.DataBind();
        lblAmounts.Visible = false;
        btnPrint.Visible = false;
        //BtnEmail.Visible = false;
        //txtStartDate.Text = "";
        //txtEndDate.Text = "";


    }

    protected void ddLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunPriLoadProduct(intCompanyId, LOBId);
        try
        {
            FunRptClear();
            DropDenomination.ClearSelection();
            ddlBranch.Enabled = false;
            FunPriLoadLocation(intCompanyId, intUserId, ProgramId, LobId);
            FunPriValidateGrid();
            FunPriLoadBranch(intCompanyId, intUserId, ProgramId, LOBId);
            FunPriLoadLocation1(intCompanyId, intUserId, ProgramId, LobId);

            //FunPriLoadLocation2(ProgramId, intUserId, intCompanyId, LOBId, LocationId);
            FunPubLoadProduct(intCompanyId, LOBId);
            txtStartDate.Text = "";
            txtEndDate.Text = "";

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVLAN.IsValid = false;
        }
        //if (ddLOB.SelectedValue == "-1")
        //{
        //    ddlProduct.Items.Clear();
        //}
    }

    protected void DropDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FunPriLoadProduct(intCompanyId, LOBId);
        try
        {
            FunRptClear();
             FunPriValidateGrid();
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVLAN.IsValid = false;
        }
        //if (ddLOB.SelectedValue == "-1")
        //{
        //    ddlProduct.Items.Clear();
        //}
    }
    protected void DDRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunRptClear();
            DropDenomination.ClearSelection();
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            FunPriValidateGrid();
            //FunPubLoadProduct(intCompanyId,LOBId);
            //ddlBranch.Items.Clear();
            //string Region = string.Empty;
            //if (DDRegion.SelectedIndex != 0)
            //{
            //    Region = DDRegion.SelectedValue;
            //}
            ////if (DDRegion.Items.Count == 2)
            ////    DDRegion.SelectedIndex = 1;
            ////else
            ////    DDRegion.SelectedIndex = 0;
            //objSerClient = new ReportAccountsMgtServicesClient();

            //byte[] byteLobs = objSerClient.FunPubGetRegBranch(intCompanyId, intUserId, Region, true);
            //List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSeriliaze(byteLobs);
            //ddlBranch.DataSource = Branch;
            //ddlBranch.DataTextField = "Description";
            //ddlBranch.DataValueField = "ID";
            //ddlBranch.DataBind();

            //int ProgramId, int UserId, int intCompanyId, int LOBId, int LocationId
            //if (DDRegion.SelectedValue == "-1")
            //{
            //    FunPriLoadLocation2(ProgramId, intUserId, intCompanyId, LOBId, LocationId);
            //    ddlBranch.Enabled = false;
            //}
            //else
            //{
            //    ddlBranch.Enabled = true;
            //    FunPriLoadLocation2(ProgramId, intUserId, intCompanyId, LOBId, LocationId);
            //}

            //if (ddlBranch.Items.Count == 2)
            //    ddlBranch.SelectedIndex = 1;
            //else
            //    ddlBranch.SelectedIndex = 0;

            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
        if (DDRegion.SelectedIndex > 0)
        {
            ddlBranch.Enabled = true;
            FunPriLoadLocation2(ProgramId, intUserId, intCompanyId, LobId, LocationId);
         
        }
        else
        {
            FunPriLoadLocation(intCompanyId, intUserId, ProgramId, LobId);
            ddlBranch.Enabled = false;
          
        }
    }

        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVLAN.IsValid = false;
        }
        finally
        {
            objSerClient.Close();
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunRptClear();
            DropDenomination.ClearSelection();
            FunPriValidateGrid();

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Branch.";
            CVLAN.IsValid = false;
        }
    }
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunRptClear();
            DropDenomination.ClearSelection();
            FunPriValidateGrid();
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load product.";
            CVLAN.IsValid = false;
        }
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        FunRptClear();
        DropDenomination.ClearSelection();
        FunPriValidateGrid();
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        FunRptClear();
        DropDenomination.ClearSelection();
        FunPriValidateGrid();

    }


    private void FuncgridBind()
    {
        try
        {
            pnlEnquiry.Visible = true;
            grvEnquirydetails.Visible = true;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubEnquiryParameters objParam = new ClsPubEnquiryParameters();

            objParam.CompanyId = intCompanyId;
            objParam.USER_ID = intUserId;
            objParam.LobId = ddLOB.SelectedValue.ToString();
            Session["lob"] = (ddLOB.SelectedItem.Text.Split('-'))[1].ToString();
            objParam.PROGRAM_ID = 186;
            //DDRegion.SelectedIndex.item[0].text = "All";
            //SelectedIndex.item[0].text="All"
            if (DDRegion.SelectedIndex != 0)
            {
                // objParam.RegionId = DDRegion.SelectedValue;//.ToString();
                objParam.LOCATION_ID1 = DDRegion.SelectedValue;
            }
            else
            {
                // objParam.RegionId = " ";
                objParam.LOCATION_ID1 = "";
            }


            if (ddlBranch.SelectedIndex != 0)
            {
                //objParam.BranchId = ddlBranch.SelectedValue;//.ToString();
                objParam.LOCATION_ID2 = ddlBranch.SelectedValue;
            }
            else
            {
                //objParam.BranchId = " ";
                objParam.LOCATION_ID2 = "";
            }



            if (ddlProduct.SelectedIndex != 0)
            {
                objParam.ProductId = ddlProduct.SelectedValue;//.ToString();
            }
            else
            {
                objParam.ProductId = " ";
            }
            //  Utility.StringToDate(       
            objParam.StartDate = Utility.StringToDate(txtStartDate.Text);
            objParam.EndDate = Utility.StringToDate(txtEndDate.Text);
            objParam.Denomination = Convert.ToDecimal(DropDenomination.Text);
            if (DropDenomination.SelectedValue == "1")
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            }
            else
            {
                lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + " " + "in" + " " + DropDenomination.SelectedItem.Text + "]";
            }

            Session["Title"] = "Enquiry Performance Details For the Period From " + " " + txtStartDate.Text + " " + "To" + " " + txtEndDate.Text;
            // byte[] byteEnquiryDetails = ClsPubSerialize.Serialize(objParam, SerializationMode.Binary);


            //ClsPubEnquiryLocation = (ClsPubEnquiryLocation)DeSeriliaze(byteLobs);
            //List<ClsPubEnquiryPerformanceDetails> details = (List<ClsPubEnquiryPerformanceDetails>)DeSeriliaze(byteLobs);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryDetails(objParam);
            ClsPubEnquiryLocation details = (ClsPubEnquiryLocation)DeSeriliaze(byteLobs);

            //if (ddlBranch.SelectedIndex > 0)
            //{
            //    grvEnquirydetails.Columns[0].Visible = false;
            //}
            //else
            //{
            //    grvEnquirydetails.Columns[0].Visible = true;
            //}

            //if (ddlProduct.SelectedIndex > 0)
            //{
            //    grvEnquirydetails.Columns[1].Visible = false;
            //    grvEnquirydetails.Columns[2].Visible = true;
            //}
            //else
            //{
            //    grvEnquirydetails.Columns[1].Visible = true;
            //    grvEnquirydetails.Columns[2].Visible = true;
            //}


            //if ((ddlBranch.SelectedIndex < 0)&&(ddlProduct.SelectedIndex > 0))
            //{
            //    grvEnquirydetails.Columns[0].Visible = true;
            //}
            //if ((ddlBranch.SelectedIndex > 0) && (ddlProduct.SelectedIndex < 0))
            //{
            //    grvEnquirydetails.Columns[2].Visible = true;
            //}
            //if ((ddlBranch.SelectedIndex < 0) && (ddlProduct.SelectedIndex < 0))
            //{
            //    grvEnquirydetails.Columns[0].Visible = true;
            //    grvEnquirydetails.Columns[2].Visible = true;
            //}
            ////TotalRecCount = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.ReceivedCount);
            ////TotalRecVal = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.ReceivedValue);
            ////TotalSucCount = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.SuccessfulCount);
            ////TotalSucVal = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.SuccessfulValue);
            ////TotalFollowCount = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.UnderFollowupCount);
            ////TotalFollowVal = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.UnderFollowupValue);
            ////TotalRejCount = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.RejectedCount);
            ////TotalRejVal = EnquiryDetails.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.RejectedValue);

            TotalRecCount = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.ReceivedCount);
            TotalRecVal = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.ReceivedValue);
            TotalSucCount = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.SuccessfulCount);
            TotalSucVal = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.SuccessfulValue);
            TotalFollowCount = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.UnderFollowupCount);
            TotalFollowVal = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.UnderFollowupValue);
            TotalRejCount = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.RejectedCount);
            TotalRejVal = details.Disbursement.Sum(ClsPubEnquiryPerformanceDetails => ClsPubEnquiryPerformanceDetails.RejectedValue);
            Session["lobloc"] = details;
            Session["EnquiryPerformance"] = details;
            //Session["EnquiryPerformance"] = details.Disbursement;

            //Session["EnquiryPerformance"] = EnquiryDetails;
            //grvEnquirydetails.DataSource = EnquiryDetails;


            grvEnquirydetails.DataSource = details.Disbursement;
            grvEnquirydetails.DataBind();
            if (grvEnquirydetails.Rows.Count == 0)
            {
                grvEnquirydetails.EmptyDataText = "No Details Found";
                grvEnquirydetails.DataBind();
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
            ObjOrgColClient.Close();
        }
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint.Visible = true;
            pnlRC.Visible = false;
            grvRC.Visible = false;
            PnlSc.Visible = false;
            GrvSc.Visible = false;
            PnlUFC.Visible = false;
            GrvFollowUp.Visible = false;
            PRejcount.Visible = false;
            GrvRejected.Visible = false;
            ClearSession();
            //Session["EnquiryHeader"] = null;

            lblAmounts.Text = "[All Amounts are in" + " " + ObjS3GSession.ProCurrencyNameRW + "]";
            FunPriValidateFromEndDate();
            

        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Enquiry Grid";
            CVLAN.IsValid = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddLOB.SelectedValue = "-1";
            DDRegion.SelectedValue = "-1";
            ddlBranch.SelectedValue = "-1";
            ddlProduct.SelectedValue = "-1";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            DropDenomination.ClearSelection();
            pnlEnquiry.Visible = false;
            pnlRC.Visible = false;
            PnlSc.Visible = false;
            PnlUFC.Visible = false;
            PRejcount.Visible = false;
            btnPrint.Visible = false;
            lblAmounts.Visible = false;
            ClearSession();
        }
        catch (Exception ex)
        {
            CVLAN.ErrorMessage = "Due to Data Problem, Unable to Load Enquiry Grid";
            CVLAN.IsValid = false;
        }
    }
    //protected void txtEndDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (Convert.ToInt32(txtStartDate.Text) > Convert.ToInt32(txtEndDate.Text))
    //    {
    //        Utility.FunShowAlertMsg(this,"Start Date should be lesser than or equal to End Date");
    //        txtEndDate.Text = "";
    //        return;
    //    }

    //}

    private void FunPriDisplayTotal()
    {
        ((LinkButton)grvEnquirydetails.FooterRow.FindControl("LnkBtnRCAll")).Text = TotalRecCount.ToString();
        ((Label)grvEnquirydetails.FooterRow.FindControl("lblRecVal")).Text = TotalRecVal.ToString();
        ((LinkButton)grvEnquirydetails.FooterRow.FindControl("LnkBtnSucAll")).Text = TotalSucCount.ToString();
        ((Label)grvEnquirydetails.FooterRow.FindControl("lblSuccessVal")).Text = TotalSucVal.ToString();
        ((LinkButton)grvEnquirydetails.FooterRow.FindControl("LnkBtnFucAll")).Text = TotalFollowCount.ToString();
        ((Label)grvEnquirydetails.FooterRow.FindControl("lblFollowVal")).Text = TotalFollowVal.ToString();
        ((LinkButton)grvEnquirydetails.FooterRow.FindControl("LnkBtnRejAll")).Text = TotalRejCount.ToString();
        ((Label)grvEnquirydetails.FooterRow.FindControl("lblRejVal")).Text = TotalRejVal.ToString();

        //((Label)grvEnquirydetails.FooterRow.FindControl("lblReceive")).Text = TotalRecCount.ToString();
        //((Label)grvEnquirydetails.FooterRow.FindControl("lblSuccess")).Text = TotalSucCount.ToString();
        //((Label)grvEnquirydetails.FooterRow.FindControl("lblFollow")).Text = TotalFollowCount.ToString();
        //((Label)grvEnquirydetails.FooterRow.FindControl("lblReject")).Text = TotalRejCount.ToString();
    }
    //<summary>
    //To set the Suffix to total
    //</summary>
    //<returns></returns>
    private string Funsetsuffix()
    {

        int suffix = 1;
        //S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }

    private void FunRptClear()
    {
        try
        {
            //lblAmounts.Visible = false;
            //pnlEnquiry.Visible = false;
            //btnPrint.Visible = false;
            ////BtnEmail.Visible = false;
            //ddLOB.ClearSelection();
            //ddLOB.Enabled = true;
            //DDRegion.ClearSelection();
            //ddlBranch.ClearSelection();
            //ddlBranch.Enabled = false;
            //// FunPriLoadRegion(intCompanyId, Is_Active, intUserId);
            //FunPriLoadBranch(intCompanyId, intUserId, ProgramId, LOBId);
            //ddlProduct.ClearSelection();
            //DropDenomination.ClearSelection();
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            pnlRC.Visible = false;
            grvRC.Visible = false;
            PnlSc.Visible = false;
            GrvSc.Visible = false;
            PnlUFC.Visible = false;
            GrvFollowUp.Visible = false;
            PRejcount.Visible = false;
            GrvRejected.Visible = false;
            //grvEnquirydetails.DataSource = null;
            //grvEnquirydetails.DataBind();
            ClearSession();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    private void ClearSession()
    {
        Session["EnquiryPerformance"] = null;
        Session["EnquiryHeader"] = null;
        Session["Denomination"] = null;
        Session["Received"] = null;
        Session["Successful"] = null;
        Session["Rejected"] = null;
        Session["Followup"] = null;

    }
    protected void ReceivedCountDetails(object sender, EventArgs e)
    {
        try
        {
            pnlRC.Visible = true;
            grvRC.Visible = true;

            PnlSc.Visible = false;
            GrvSc.DataSource = null;
            GrvSc.DataBind();
            Session["Successful"] = null;


            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;
            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvEnquirydetails_")).Replace("grvEnquirydetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblBranch = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblBranch");
            Label lblBranchId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblBranchId");
            Label lblProductId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblProductId");
            ClsPubEnquiryParameters objParameter = new ClsPubEnquiryParameters();
            objParameter.CompanyId = intCompanyId;
            objParameter.USER_ID = intUserId;
            objParameter.LobId = ddLOB.SelectedValue.ToString();
            objParameter.PROGRAM_ID = 186;
            objParameter.Option = 1;
            objParameter.LOCATION_ID1 = lblBranchId.Text;

            if (DDRegion.SelectedIndex != 0)
            {
                //objParameter.RegionId = DDRegion.SelectedValue;//.ToString();
                objParameter.LOCATION_ID1 = DDRegion.SelectedValue;

            }
            else
            {
                //objParameter.RegionId = " ";
                objParameter.LOCATION_ID1 = " ";
            }
            objParameter.LOCATION_ID2 = lblBranchId.Text;
            objParameter.ProductId = lblProductId.Text;
            objParameter.StartDate = Utility.StringToDate(txtStartDate.Text);
            objParameter.EndDate = Utility.StringToDate(txtEndDate.Text);
            objParameter.Denomination = Convert.ToDecimal(DropDenomination.Text);
            byte[] byteRecDetails = ClsPubSerialize.Serialize(objParameter, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryRecCount(byteRecDetails);
            List<ClsPubEnquiryCount> Enqcounts = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Received"] = Enqcounts;
            grvRC.DataSource = Enqcounts;
            grvRC.DataBind();
            if (grvRC.Rows.Count == 0)
            {
                grvRC.EmptyDataText = "No Details Found";
                grvRC.DataBind();
                Session["Received"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }

    protected void AllReceivedDetails(object sender, EventArgs e)
    {
        try
        {

            pnlRC.Visible = true;
            grvRC.Visible = true;

            PnlSc.Visible = false;
            GrvSc.DataSource = null;
            GrvSc.DataBind();
            Session["Successful"] = null;

            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;


            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubEnquiryParameters objParameter = new ClsPubEnquiryParameters();
            objParameter.CompanyId = intCompanyId;
            objParameter.USER_ID = intUserId;
            objParameter.PROGRAM_ID = 186;
            objParameter.LobId = ddLOB.SelectedValue.ToString();
            if (DDRegion.SelectedIndex != 0)
            {
                objParameter.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                objParameter.LOCATION_ID1 = " ";
            }
            objParameter.Option = 2;
            objParameter.LOCATION_ID2 = ddlBranch.SelectedValue;
            objParameter.ProductId = ddlProduct.SelectedValue;
            objParameter.StartDate = Utility.StringToDate(txtStartDate.Text);
            objParameter.EndDate = Utility.StringToDate(txtEndDate.Text);
            objParameter.Denomination = Convert.ToDecimal(DropDenomination.Text);
            byte[] byteRecDetails = ClsPubSerialize.Serialize(objParameter, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryRecCount(byteRecDetails);
            List<ClsPubEnquiryCount> Enqcounts = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Received"] = Enqcounts;
            grvRC.DataSource = Enqcounts;
            grvRC.DataBind();
            if (grvRC.Rows.Count == 0)
            {
                grvRC.EmptyDataText = "No Details Found";
                grvRC.DataBind();
                Session["Received"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }

    protected void SuccessfulCountDetails(object sender, EventArgs e)
    {
        try
        {
            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;

            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvEnquirydetails_")).Replace("grvEnquirydetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblBranchId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblBranchId");
            Label lblProductId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblProductId");
            PnlSc.Visible = true;
            GrvSc.Visible = true;
            ClsPubEnquiryParameters objSuccess = new ClsPubEnquiryParameters();
            objSuccess.CompanyId = intCompanyId;
            objSuccess.USER_ID = intUserId;
            objSuccess.PROGRAM_ID = 186;
            objSuccess.LobId = ddLOB.SelectedValue.ToString();
            objSuccess.Option = 1;
            objSuccess.Denomination = Convert.ToDecimal(DropDenomination.Text);
            if (DDRegion.SelectedIndex != 0)
            {
                objSuccess.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                objSuccess.LOCATION_ID1 = " ";
            }
            objSuccess.RegionId = DDRegion.SelectedValue.ToString();
            objSuccess.LOCATION_ID2 = lblBranchId.Text;//ddlBranch.SelectedValue;
            //lblBranchId.Text;
            objSuccess.ProductId = lblProductId.Text;
            objSuccess.StartDate = Utility.StringToDate(txtStartDate.Text);
            objSuccess.EndDate = Utility.StringToDate(txtEndDate.Text);
            byte[] byteSucDetails = ClsPubSerialize.Serialize(objSuccess, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquirySucCount(byteSucDetails);
            List<ClsPubEnquiryCount> EnqSuccess = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Successful"] = EnqSuccess;
            GrvSc.DataSource = EnqSuccess;
            GrvSc.DataBind();
            if (GrvSc.Rows.Count == 0)
            {
                GrvSc.EmptyDataText = "No Details Found";
                GrvSc.DataBind();
                Session["Successful"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    protected void AllSuccessfulDetails(object sender, EventArgs e)
    {
        try
        {

            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;

            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;

            PnlSc.Visible = true;
            GrvSc.Visible = true;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubEnquiryParameters objSuccess = new ClsPubEnquiryParameters();
            objSuccess.CompanyId = intCompanyId;
            objSuccess.LobId = ddLOB.SelectedValue.ToString();
            objSuccess.USER_ID = intUserId;
            objSuccess.PROGRAM_ID = 186;
            objSuccess.Option = 2;
            objSuccess.Denomination = Convert.ToDecimal(DropDenomination.Text);
            if (DDRegion.SelectedIndex != 0)
            {
                objSuccess.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                objSuccess.LOCATION_ID1 = " ";
            }
            //objSuccess.RegionId = DDRegion.SelectedValue.ToString();
            objSuccess.LOCATION_ID2 = ddlBranch.SelectedValue;
            objSuccess.ProductId = ddlProduct.SelectedValue;
            objSuccess.StartDate = Utility.StringToDate(txtStartDate.Text);
            objSuccess.EndDate = Utility.StringToDate(txtEndDate.Text);
            byte[] byteSucDetails = ClsPubSerialize.Serialize(objSuccess, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquirySucCount(byteSucDetails);
            List<ClsPubEnquiryCount> EnqSuccess = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Successful"] = EnqSuccess;
            GrvSc.DataSource = EnqSuccess;
            GrvSc.DataBind();
            if (GrvSc.Rows.Count == 0)
            {
                GrvSc.EmptyDataText = "No Details Found";
                GrvSc.DataBind();
                Session["Successful"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    protected void FollowupCountDetails(object sender, EventArgs e)
    {
        try
        {

            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            PnlSc.Visible = false;
            GrvSc.DataSource = null;
            GrvSc.DataBind();
            Session["Successful"] = null;

            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvEnquirydetails_")).Replace("grvEnquirydetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblBranchId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblBranchId");
            Label lblProductId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblProductId");
            PnlUFC.Visible = true;
            GrvFollowUp.Visible = true;
            ClsPubEnquiryParameters ObjFollow = new ClsPubEnquiryParameters();
            ObjFollow.CompanyId = intCompanyId;
            ObjFollow.USER_ID = intUserId;
            ObjFollow.LobId = ddLOB.SelectedValue.ToString();
            ObjFollow.PROGRAM_ID = 186;
            ObjFollow.Option = 1;
            ObjFollow.Denomination = Convert.ToDecimal(DropDenomination.Text);
            if (DDRegion.SelectedIndex != 0)
            {
                ObjFollow.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                ObjFollow.LOCATION_ID1 = " ";
            }
            ObjFollow.RegionId = DDRegion.SelectedValue.ToString();
            ObjFollow.LOCATION_ID2 = lblBranchId.Text; //ddlBranch.SelectedValue;
            //lblBranchId.Text;
            ObjFollow.ProductId = lblProductId.Text;
            ObjFollow.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjFollow.EndDate = Utility.StringToDate(txtEndDate.Text);
            byte[] byteFollowDetails = ClsPubSerialize.Serialize(ObjFollow, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryUnderFollowupCount(byteFollowDetails);
            List<ClsPubEnquiryCount> EnqFollowups = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Followup"] = EnqFollowups;
            GrvFollowUp.DataSource = EnqFollowups;
            GrvFollowUp.DataBind();
            if (GrvFollowUp.Rows.Count == 0)
            {
                GrvFollowUp.EmptyDataText = "No Details Found";
                GrvFollowUp.DataBind();
                Session["Followup"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }

    }
    protected void AllFollowupDetails(object sender, EventArgs e)
    {
        try
        {

            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            PnlSc.Visible = false;
            GrvSc.DataSource = null;
            GrvSc.DataBind();
            Session["Successful"] = null;

            PRejcount.Visible = false;
            GrvRejected.DataSource = null;
            GrvRejected.DataBind();
            Session["Rejected"] = null;

            PnlUFC.Visible = true;
            GrvFollowUp.Visible = true;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubEnquiryParameters ObjFollow = new ClsPubEnquiryParameters();
            ObjFollow.CompanyId = intCompanyId;
            ObjFollow.USER_ID = intUserId;
            ObjFollow.PROGRAM_ID = 186;
            ObjFollow.LobId = ddLOB.SelectedValue.ToString();
            if (DDRegion.SelectedIndex != 0)
            {
                ObjFollow.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                ObjFollow.LOCATION_ID1 = " ";
            }
            //ObjFollow.RegionId = DDRegion.SelectedValue.ToString();
            ObjFollow.LOCATION_ID2 = ddlBranch.SelectedValue;
            ObjFollow.ProductId = ddlProduct.SelectedValue;
            ObjFollow.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjFollow.EndDate = Utility.StringToDate(txtEndDate.Text);
            ObjFollow.Option = 2;
            ObjFollow.Denomination = Convert.ToDecimal(DropDenomination.Text);
            byte[] byteFollowDetails = ClsPubSerialize.Serialize(ObjFollow, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryUnderFollowupCount(byteFollowDetails);
            List<ClsPubEnquiryCount> EnqFollowups = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Followup"] = EnqFollowups;
            GrvFollowUp.DataSource = EnqFollowups;
            GrvFollowUp.DataBind();
            if (GrvFollowUp.Rows.Count == 0)
            {
                GrvFollowUp.EmptyDataText = "No Details Found";
                GrvFollowUp.DataBind();
                Session["Followup"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }

    }
    protected void RejectedValueDetails(object sender, EventArgs e)
    {
        try
        {

            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            GrvSc.DataSource = null;
            GrvSc.DataBind();
            PnlSc.Visible = false;
            Session["Successful"] = null;

            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;
         
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            string strFieldAtt = ((LinkButton)sender).ClientID;
            string strVal = strFieldAtt.Substring(strFieldAtt.LastIndexOf("grvEnquirydetails_")).Replace("grvEnquirydetails_ctl", "");
            int gRowIndex = Convert.ToInt32(strVal.Substring(0, strVal.LastIndexOf("_")));
            gRowIndex = gRowIndex - 2;
            Label lblBranchId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblBranchId");
            Label lblProductId = (Label)grvEnquirydetails.Rows[gRowIndex].FindControl("lblProductId");
            PRejcount.Visible = true;
            GrvRejected.Visible = true;
            ClsPubEnquiryParameters ObjReject = new ClsPubEnquiryParameters();
            ObjReject.CompanyId = intCompanyId;
            ObjReject.USER_ID = intUserId;
            ObjReject.PROGRAM_ID = 186;
            ObjReject.LobId = ddLOB.SelectedValue.ToString();
            ObjReject.Option = 1;
            if (DDRegion.SelectedIndex != 0)
            {
                ObjReject.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                ObjReject.LOCATION_ID1 = " ";
            }
            ObjReject.LOCATION_ID2 = lblBranchId.Text;
            ObjReject.ProductId = lblProductId.Text;
            ObjReject.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjReject.EndDate = Utility.StringToDate(txtEndDate.Text);
            ObjReject.Denomination = Convert.ToDecimal(DropDenomination.Text);
            byte[] byteRejectedDetails = ClsPubSerialize.Serialize(ObjReject, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryRejectedCount(byteRejectedDetails);
            List<ClsPubEnquiryCount> EnqRejects = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Rejected"] = EnqRejects;
            GrvRejected.DataSource = EnqRejects;
            GrvRejected.DataBind();
            if (GrvRejected.Rows.Count == 0)
            {
                GrvRejected.EmptyDataText = "No Details Found";
                GrvRejected.DataBind();
                Session["Rejected"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    protected void AllRejectedDetails(object sender, EventArgs e)
    {
        try
        {
            pnlRC.Visible = false;
            grvRC.DataSource = null;
            grvRC.DataBind();
            Session["Received"] = null;

            GrvSc.DataSource = null;
            GrvSc.DataBind();
            PnlSc.Visible = false;
            Session["Successful"] = null;

            PnlUFC.Visible = false;
            GrvFollowUp.DataSource = null;
            GrvFollowUp.DataBind();
            Session["Followup"] = null;

            PRejcount.Visible = true;
            GrvRejected.Visible = true;
            ObjOrgColClient = new ReportOrgColMgtServicesClient();
            ClsPubEnquiryParameters ObjReject = new ClsPubEnquiryParameters();
            ObjReject.CompanyId = intCompanyId;
            ObjReject.USER_ID = intUserId;
            ObjReject.PROGRAM_ID = 186;
            ObjReject.LobId = ddLOB.SelectedValue.ToString();
            ObjReject.Option = 2;
            ObjReject.Denomination = Convert.ToDecimal(DropDenomination.Text);
            if (DDRegion.SelectedIndex != 0)
            {
                ObjReject.LOCATION_ID1 = DDRegion.SelectedValue;//.ToString();
            }
            else
            {
                ObjReject.LOCATION_ID1 = " ";
            }
            ObjReject.LOCATION_ID2 = ddlBranch.SelectedValue;
            ObjReject.ProductId = ddlProduct.SelectedValue;
            ObjReject.StartDate = Utility.StringToDate(txtStartDate.Text);
            ObjReject.EndDate = Utility.StringToDate(txtEndDate.Text);
            byte[] byteRejectedDetails = ClsPubSerialize.Serialize(ObjReject, SerializationMode.Binary);
            byte[] byteLobs = ObjOrgColClient.FunPubGetEnquiryRejectedCount(byteRejectedDetails);
            List<ClsPubEnquiryCount> EnqRejects = (List<ClsPubEnquiryCount>)DeSeriliaze(byteLobs);
            Session["Rejected"] = EnqRejects;
            GrvRejected.DataSource = EnqRejects;
            GrvRejected.DataBind();
            if (GrvRejected.Rows.Count == 0)
            {
                GrvRejected.EmptyDataText = "No Details Found";
                GrvRejected.DataBind();
                Session["Rejected"] = null;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            ObjOrgColClient.Close();
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
       
        string strScipt = "window.open('../Reports/S3GRptEnquiryPerformanceReport.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Enquire", strScipt, true);


    }
    //Changes by Gomathi

    private void FunPriLoadLocation1(int intCompanyId, int intUserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            DDRegion.DataSource = Branch;
            DDRegion.DataTextField = "Description";
            DDRegion.DataValueField = "ID";
            DDRegion.DataBind();
            DDRegion.Items[0].Text = "All";
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
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    private void FunPriLoadLocation(int intCompanyId, int intUserId, int ProgramId, int LobId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddLOB.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubBranch(intCompanyId, intUserId, ProgramId, LobId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "All";
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.SelectedIndex = 1;
            }
            else
            {
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
    private void FunPriLoadLocation2(int ProgramId, int intUserId, int intCompanyId, int LobId, int LocationId)
    {
        try
        {
            objSerClient = new ReportAccountsMgtServicesClient();
            if (ddLOB.SelectedIndex > 0)
            {
                LobId = Convert.ToInt32(ddLOB.SelectedValue);
            }
            if (DDRegion.SelectedIndex > 0)
            {
                LocationId = Convert.ToInt32(DDRegion.SelectedValue);
            }
            byte[] byteLobs = objSerClient.FunPubGetLocation2(ProgramId, intUserId, intCompanyId, LobId, LocationId);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);

            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();
            ddlBranch.Items[0].Text = "All";
            if (ddlBranch.Items.Count == 2)
            {
                ddlBranch.SelectedIndex = 1;
            }
            else
            {
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
   
}



