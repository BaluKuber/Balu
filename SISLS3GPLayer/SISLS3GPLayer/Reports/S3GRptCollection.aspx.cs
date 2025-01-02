using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ReportOrgColMgtServicesReference;
using ReportAccountsMgtServicesReference;
using S3GBusEntity;
using S3GBusEntity.Reports;
using System.ServiceProcess;


public partial class Reports_S3GRptCollection : ApplyThemeForProject
{
    ReportAccountsMgtServicesClient objclient;
    ReportOrgColMgtServicesClient ObjSerClient;
    UserInfo userinfo = new UserInfo();
    public int intCompanyID, intUserID, intProgramID = 193;
    public string strCurerncy, strDateFormat;
    public bool IsChk;
    S3GSession objsession = new S3GSession();
    decimal TotalReceiptAmount;
    decimal TotalCollection;
    decimal TotalCurrentCollection;
    decimal TotalArrearsCollection;
    decimal TotalInsurance;
    decimal TotalInterest;
    decimal TotalODI;
    decimal TotalMemo;
    decimal TotalOthers;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            /* Changed Date Control start - 30-Nov-2012 */
            //txtFromDateSearch.Attributes.Add("readonly", "readonly");
            //txtToDateSearch.Attributes.Add("readonly", "readonly");
            txtFromDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtFromDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            txtToDateSearch.Attributes.Add("onblur", "fnDoDate(this,'" + txtToDateSearch.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */

            intCompanyID = userinfo.ProCompanyIdRW;
            Session["CompanyName"] = userinfo.ProCompanyNameRW;
            intUserID = userinfo.ProUserIdRW;
            strDateFormat = objsession.ProDateFormatRW;
            CalendarExtenderFromDateSearch.Format = objsession.ProDateFormatRW;
            CalendarExtenderToDateSearch.Format = objsession.ProDateFormatRW;           
            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            if (!IsPostBack)
            {
                FunPriLoadLOB();
                FunPubLoadLocation1(string.Empty);
                FunPubLoadLocation2(string.Empty);
                FunPriLoadDenomination();
                lblCurrency.Visible = false;
                PnlCollectionDetails.Visible = false;
                btnPrint.Visible = false;
                ddlLocation1.Enabled = false;
                DdlLocation2.Enabled = false;
            }
            //ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
            //string ID = divCollection.ClientID;
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    private void FunPriLoadLOB()
    {
        ObjSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbytelob = ObjSerClient.FunPubGetPDCReminderLOBDetails(intCompanyID, intUserID, intProgramID);
            List<ClsPubDropDownList> LOB = (List<ClsPubDropDownList>)DeSerialize(objbytelob);
            DdlLOB.DataSource = LOB;
            DdlLOB.DataTextField = "Description";
            DdlLOB.DataValueField = "ID";
            DdlLOB.DataBind();
            if (DdlLOB.Items.Count == 2)
            {
                DdlLOB.Items.RemoveAt(0);
            }
        }

        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }

        finally
        {
            ObjSerClient.Close();
        }
    }
    public void FunPubLoadLocation1(string LobId)
    {
        ObjSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbytepro = ObjSerClient.FunPubGetPDCReminderBranchDetails(intCompanyID, intUserID, true, LobId, intProgramID);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            ddlLocation1.DataSource = Branch;
            ddlLocation1.DataTextField = "Description";
            ddlLocation1.DataValueField = "ID";
            ddlLocation1.DataBind();
            ddlLocation1.Items.RemoveAt(0);
            ListItem item = new ListItem("--All--", "0");
            ddlLocation1.Items.Insert(0, item);
            if (ddlLocation1.Items.Count == 2)
            {
                ddlLocation1.Items.RemoveAt(0);
            }
        }
        catch (Exception br)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(br);
            throw br;
        }

        finally
        {
            ObjSerClient.Close();
        }
    }
    public void FunPubLoadLocation2()
    {
        ObjSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbytepro = ObjSerClient.FunPubGetLocationDetails(intCompanyID, intUserID, intProgramID, Convert.ToInt32(DdlLOB.SelectedValue), Convert.ToInt32(ddlLocation1.SelectedValue));
            List<ClsPubDropDownList> Location = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            DdlLocation2.DataSource = Location;
            DdlLocation2.DataTextField = "Description";
            DdlLocation2.DataValueField = "ID";
            DdlLocation2.DataBind();
            if (DdlLocation2.Items.Count > 2)
            {
                DdlLocation2.Items.RemoveAt(0);
                ListItem item = new ListItem("--All--", "0");
                DdlLocation2.Items.Insert(0, item);
                DdlLocation2.Enabled = true;
            }
            if (DdlLocation2.Items.Count == 2)
            {
                DdlLocation2.Items.RemoveAt(0);
                //ListItem item = new ListItem("--All--", "0");
                //ComboBoxLocationSearch.Items.Insert(0, item);
                DdlLocation2.Enabled = false;
            }
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            ObjSerClient.Close();
        }
    }
    public void FunPubLoadLocation2(string LobId)
    {
        ObjSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            byte[] objbytepro = ObjSerClient.FunPubGetPDCReminderBranchDetails(intCompanyID, intUserID, true, LobId, intProgramID);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(objbytepro);
            DdlLocation2.DataSource = Branch;
            DdlLocation2.DataTextField = "Description";
            DdlLocation2.DataValueField = "ID";
            DdlLocation2.DataBind();
            DdlLocation2.Items.RemoveAt(0);
            ListItem item = new ListItem("--All--", "0");
            DdlLocation2.Items.Insert(0, item);
            if (DdlLocation2.Items.Count == 2)
            {
                DdlLocation2.Items.RemoveAt(0);
            }
        }
        catch (Exception br)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(br);
            throw br;
        }

        finally
        {
            ObjSerClient.Close();
        }
    }

    private void FunPriLoadDenomination()
    {
        objclient = new ReportAccountsMgtServicesClient();
        try
        {
            byte[] objbytedenomination = objclient.GetDenominations();
            List<ClsPubDropDownList> Denomination = (List<ClsPubDropDownList>)DeSerialize(objbytedenomination);
            DdlDenomination.DataSource = Denomination;
            DdlDenomination.DataTextField = "Description";
            DdlDenomination.DataValueField = "ID";
            DdlDenomination.DataBind();
        }

        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }

        finally
        {
            objclient.Close();
        }
    }

    private object DeSerialize(byte[] byteObj)
    {
        try
        {
            return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //ddlHierarchy.SelectedValue = "-1";
            //DdlHierarchyName.SelectedValue = "-`";
            //DdlLOB.SelectedValue = "-1";
            FunPriLoadLOB();
            FunPubLoadLocation1(string.Empty);
            FunPubLoadLocation2(string.Empty);
            chkAccountLevel.Checked = false;
            txtFromDateSearch.Text = string.Empty;
            txtToDateSearch.Text = string.Empty;
            lblCurrency.Visible = false;
            ddlLocation1.Enabled = false;
            DdlLocation2.Enabled = false;
            grvCollection.DataSource = null;
            grvCollection.DataBind();
            PnlCollectionDetails.Visible = false;
            btnPrint.Visible = false;
            Session["CollectionHeader"] = null;
            Session["CollectionDetails"] = null;
            Session["CollectionPrecise"] = null;
            Session["AccountLevel"] = null;            
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        ObjSerClient = new ReportOrgColMgtServicesClient();
        try
        {
            divCollection.Style.Add("display", "block");
            ClsPubCollectionHeader objCollectionHeader = new ClsPubCollectionHeader();
            objCollectionHeader.CompanyId = intCompanyID;
            objCollectionHeader.LOBID = Convert.ToInt32(DdlLOB.SelectedValue);
            objCollectionHeader.UserId = intUserID;
            objCollectionHeader.LocationId1 = Convert.ToInt32(ddlLocation1.SelectedValue);
            objCollectionHeader.LocationId2 = Convert.ToInt32(DdlLocation2.SelectedValue);
            objCollectionHeader.ProgramId = intProgramID;
            objCollectionHeader.Denomination = Convert.ToDecimal(DdlDenomination.SelectedValue);
            objCollectionHeader.FromMonth = Utility.StringToDate(txtFromDateSearch.Text.ToString());
            objCollectionHeader.ToMonth = Utility.StringToDate(txtToDateSearch.Text.ToString());
            objCollectionHeader.LineOfBusiness = DdlLOB.SelectedItem.Text.Split('-')[1].ToString();
            objCollectionHeader.Location1 = ddlLocation1.SelectedItem.ToString();
            objCollectionHeader.Location2 = DdlLocation2.SelectedItem.ToString();
            Session["ReportName"] = "Collection Report for the period from" + " " + txtFromDateSearch.Text + " " + "to" + " " + txtToDateSearch.Text;
            Session["StartDate"] = txtFromDateSearch.Text;
            Session["EndDate"] = txtToDateSearch.Text;
            btnPrint.Enabled = true;
            if (DdlDenomination.SelectedValue == "1")
            {
                Session["Currency"] = lblCurrency.Text = "[All Amounts are in " + " " + objsession.ProCurrencyNameRW + "]";
            }
            else
            {
                Session["Currency"] = lblCurrency.Text = "[All Amounts are in " + " " + objsession.ProCurrencyNameRW + " " + "in" + " " + DdlDenomination.SelectedItem.Text + "]";
            }
            if (chkAccountLevel.Checked == true)
            {
                objCollectionHeader.IsDetailed = "Y";
            }
            else
            {
                objCollectionHeader.IsDetailed = "N";
            }
            List<ClsPubCollectionHeader> objlistCollectionHeader = new List<ClsPubCollectionHeader>();
            objlistCollectionHeader.Add(objCollectionHeader);
            Session["CollectionHeader"] = objlistCollectionHeader;
            byte[] objbyteCollectionHeader = ClsPubSerialize.Serialize(objCollectionHeader, SerializationMode.Binary);
            if (chkAccountLevel.Checked == true)
            {
                Session["AccountLevel"] = "TRUE";
                byte[] objbyteCollectionDetails = ObjSerClient.FunPubGetCollectionDetails(objbyteCollectionHeader);
                List<ClsPubCollectionDetails> CollectionDetails = (List<ClsPubCollectionDetails>)DeSeriliaze(objbyteCollectionDetails);
                grvCollection.DataSource = CollectionDetails;
                grvCollection.DataBind();
                if (grvCollection.Rows.Count == 0)
                {
                    grvCollection.EmptyDataText = "No Records Found";
                    grvCollection.DataBind();
                    lblCurrency.Visible = false;
                    PnlCollectionDetails.Visible = true;
                    btnPrint.Enabled = false;
                }
                else
                {
                    Session["CollectionDetails"] = CollectionDetails;
                    lblCurrency.Visible = true;
                    PnlCollectionDetails.Visible = true;
                    FunPriDisableEnableGridColumns(true);
                    grvCollection.Columns[7].Visible = false;
                    btnPrint.Visible = true;
                    TotalCollection = CollectionDetails.Sum(Collection => Collection.TotalCollectionAmount);
                    TotalArrearsCollection = CollectionDetails.Sum(Collection => Collection.ArrearsCollection);
                    TotalCurrentCollection = CollectionDetails.Sum(Collection => Collection.CurrentCollection);
                    TotalInsurance = CollectionDetails.Sum(Collection => Collection.Insurance);
                    TotalInterest = CollectionDetails.Sum(Collection => Collection.Interest);
                    TotalMemo = CollectionDetails.Sum(Collection => Collection.MemoCharges);
                    TotalODI = CollectionDetails.Sum(Collection => Collection.OverDueInterest);
                    TotalOthers = CollectionDetails.Sum(Collection => Collection.Others);
                    TotalReceiptAmount = CollectionDetails.Sum(Collection => Collection.ReceiptAmount);
                    FunPriDisplayTotal();
                }
            }

            else if (chkAccountLevel.Checked == false)
            {
                Session["AccountLevel"] = "FALSE";
                byte[] objbyteCollectionPreciseDetails = ObjSerClient.FunPubGetCollectionPreciseDetails(objbyteCollectionHeader);
                List<ClsPubCollectionDetails> CollectionPrecise = (List<ClsPubCollectionDetails>)DeSerialize(objbyteCollectionPreciseDetails);
                grvCollection.DataSource = CollectionPrecise;
                grvCollection.DataBind();
                if (grvCollection.Rows.Count == 0)
                {
                    grvCollection.EmptyDataText = "No Records Found";
                    grvCollection.DataBind();
                    lblCurrency.Visible = false;
                    PnlCollectionDetails.Visible = true;
                    btnPrint.Enabled = false;
                }
                else
                {
                    Session["CollectionPrecise"] = CollectionPrecise;
                    lblCurrency.Visible = true;
                    PnlCollectionDetails.Visible = true;
                    FunPriDisableEnableGridColumns(false);
                    grvCollection.Columns[7].Visible = true;
                    btnPrint.Visible = true;
                    TotalCollection = CollectionPrecise.Sum(Collection => Collection.TotalCollectionAmount);
                    TotalArrearsCollection = CollectionPrecise.Sum(Collection => Collection.ArrearsCollection);
                    TotalCurrentCollection = CollectionPrecise.Sum(Collection => Collection.CurrentCollection);
                    TotalInsurance = CollectionPrecise.Sum(Collection => Collection.Insurance);
                    TotalInterest = CollectionPrecise.Sum(Collection => Collection.Interest);
                    TotalMemo = CollectionPrecise.Sum(Collection => Collection.MemoCharges);
                    TotalODI = CollectionPrecise.Sum(Collection => Collection.OverDueInterest);
                    TotalOthers = CollectionPrecise.Sum(Collection => Collection.Others);
                    FunPriDisplayTotal();
                }
            }
        }

        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }

        finally
        {
            ObjSerClient.Close();
        }
    }

    private int FunPriValidateDate(int Year, int month, int day)
    {
        try
        {         
            string strCurrentFinancialYear = FunPriLoadCurrentFinancialYear();           
            int Startyear = Convert.ToInt32((strCurrentFinancialYear.Split('-')[0]));
            int Endyear = Convert.ToInt32((strCurrentFinancialYear.Split('-')[1]));
            int StartDate = 01;
            int StartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
            int EndMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"])-1;
            DateTime dt = new DateTime(Startyear, StartMonth, StartDate);
            int EndDate = FunPubGetEndDayOfMonth(Endyear, EndMonth);
            DateTime dt1 = new DateTime(Endyear, EndMonth, EndDate); 
            DateTime dt2 = new DateTime(Year, month, day);
            if (dt2 < dt || dt2 > dt1)
            {
                Utility.FunShowAlertMsg(this, "Selected date should be within the current financial year");
                return 1;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
    }

    public int FunPubGetEndDayOfMonth(int Year, int Month)
    {
        try
        {
            return (new DateTime(Year, (int)Month, DateTime.DaysInMonth(Year, (int)Month), 23, 59, 59, 999).Day);
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
    }

    protected void txtFromDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int year = (Utility.StringToDate(txtFromDateSearch.Text)).Year;
            int month = (Utility.StringToDate(txtFromDateSearch.Text)).Month;
            int day = (Utility.StringToDate(txtFromDateSearch.Text)).Day;
            int val = FunPriValidateDate(year, month, day);
            if (val == 1)
            {
                txtFromDateSearch.Text = string.Empty;
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }
    protected void txtToDateSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtFromDateSearch.Text == string.Empty)
            {                
                Utility.FunShowAlertMsg(this, "From Date cannot be empty");
                txtToDateSearch.Text = string.Empty;
                return;
            }
            int year = (Utility.StringToDate(txtToDateSearch.Text)).Year;
            int month = (Utility.StringToDate(txtToDateSearch.Text)).Month;
            int day = (Utility.StringToDate(txtToDateSearch.Text)).Day;
            int val = FunPriValidateDate(year, month, day);
            if (Utility.StringToDate(txtToDateSearch.Text) < Utility.StringToDate(txtFromDateSearch.Text))
            {
                Utility.FunShowAlertMsg(this, "To Date should be greater than or equal to From Date");
                txtToDateSearch.Text = string.Empty;
            }
            if (val == 1)
            {
                txtToDateSearch.Text = string.Empty;
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    private object DeSeriliaze(byte[] byteObj)
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

    private void FunPriDisableEnableGridColumns(bool IsDisable)
    {
        try
        {
            grvCollection.Columns[1].Visible = IsDisable;
            grvCollection.Columns[2].Visible = IsDisable;
            grvCollection.Columns[3].Visible = IsDisable;
            grvCollection.Columns[4].Visible = IsDisable;
            grvCollection.Columns[5].Visible = IsDisable;
            grvCollection.Columns[6].Visible = IsDisable;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strScript = "window.open('../Reports/S3GCollectionReport.aspx','newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Collection Report", strScript, true);
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    private void FunPriDisplayTotal()
    {
        try
        {
            ((Label)grvCollection.FooterRow.FindControl("lblTotalCollection")).Text = TotalCollection.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalArrearsCollection")).Text = TotalArrearsCollection.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalCurrentCollection")).Text = TotalCurrentCollection.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalInsurance")).Text = TotalInsurance.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalInterest")).Text = TotalInterest.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalMemoCharges")).Text = TotalMemo.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalODI")).Text = TotalODI.ToString(Funsetsuffix());
            ((Label)grvCollection.FooterRow.FindControl("lblTotalOthers")).Text = TotalOthers.ToString(Funsetsuffix());
            if (TotalReceiptAmount != 0)
            {
                ((Label)grvCollection.FooterRow.FindControl("lblTotalReceiptAmount")).Text = TotalReceiptAmount.ToString(Funsetsuffix());
            }
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
    }

    private string Funsetsuffix()
    {
        try
        {
            int suffix = 1;
            //S3GSession ObjS3GSession = new S3GSession();
            //suffix = ObjS3GSession.ProGpsSuffixRW;
            suffix = objsession.ProGpsSuffixRW;
            string strformat = "0.";
            for (int i = 1; i <= suffix; i++)
            {
                strformat += "0";
            }
            return strformat;
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    private void FunPriValidateGrid()
    {
        try
        {
            btnPrint.Visible = false;
            lblCurrency.Visible = false;
            PnlCollectionDetails.Visible = false;
            grvCollection.DataSource = null;
            grvCollection.DataBind();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    protected void DdlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriValidateGrid();
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    protected void DdlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtFromDateSearch.Text = string.Empty;
            txtToDateSearch.Text = string.Empty;
            FunPriValidateGrid();            
            FunPubLoadLocation1(DdlLOB.SelectedValue);
            FunPubLoadLocation2(DdlLOB.SelectedValue);
            ddlLocation1.Enabled = true;
            if (chkAccountLevel.Checked == true)
            {
                chkAccountLevel.Checked = false;
            }
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }
    protected void ddlLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPubLoadLocation2();
        }
        catch (Exception ae)
        {
            throw ae;
        }
    }

    private string FunPriLoadCurrentFinancialYear()
    {
        int CurrentYear = DateTime.Now.Year;
        int CuurentMonth = DateTime.Now.Month;
        int FinancialYearStartMonth = Convert.ToInt32(ConfigurationManager.AppSettings["StartMonth"]);
        if (CuurentMonth >= FinancialYearStartMonth)
        {
            return Convert.ToString(CurrentYear) + "-" + Convert.ToString(CurrentYear + 1);
        }
        else
        {
            return Convert.ToString(CurrentYear - 1) + "-" + Convert.ToString(CurrentYear);
        }
    }
}
