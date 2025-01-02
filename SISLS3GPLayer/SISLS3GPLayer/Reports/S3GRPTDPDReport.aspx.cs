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
using S3GBusEntity.Reports;
using ReportAccountsMgtServicesReference;
using System.Globalization;
using System.IO;
using System.Data.Linq;
using System.Data.Common;
using System.Text;
using System.Reflection;
using System.Data.Linq;

public partial class Reports_S3GRPTDPDReport : ApplyThemeForProject
{
    UserInfo ObjUserInfo;
    S3GSession ObjS3GSession = new S3GSession();
    int intCompanyId;
    int intUserId;
    string RegionId;
    bool Is_Active;
    Dictionary<string, string> ProcParam;
    string strPageName = "Days Past Dues";
    string BucketDetails = "";
    DataTable dtTable = new DataTable();
    ReportAccountsMgtServicesClient objSerClient = new ReportAccountsMgtServicesClient();
    ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient ObjOrgColClient = new ReportOrgColMgtServicesReference.ReportOrgColMgtServicesClient();
    //string cutoff;
    //decimal denomination;
    public string strCurerncy, strDateFormat;
    decimal DecMinAdjVar;
    decimal DecMaxAdjVar;
    decimal DecCompVar;
    public void GetGPSSuffixVar(int Suffix)
    {
        if (Suffix == 1)
        {
            DecMinAdjVar = -0.5m;
            DecMaxAdjVar = 0.5m;
            DecCompVar = 0.0m;
        }
        else if (Suffix == 2)
        {
            DecMinAdjVar = -0.05m;
            DecMaxAdjVar = 0.05m;
            DecCompVar = 0.00m;
        }
        else if (Suffix == 3)
        {
            DecMinAdjVar = -0.005m;
            DecMaxAdjVar = 0.005m;
            DecCompVar = 0.000m;
        }
        else if (Suffix == 4)
        {
            DecMinAdjVar = -0.0005m;
            DecMaxAdjVar = 0.0005m;
            DecCompVar = 0.000m;
        }
        else if (Suffix == 5)
        {
            DecMinAdjVar = -0.00005m;
            DecMaxAdjVar = 0.00005m;
            DecCompVar = 0.00000m;
        }
        else if (Suffix == 6)
        {
            DecMinAdjVar = -0.000005m;
            DecMaxAdjVar = 0.000005m;
            DecCompVar = 0.000000m;
        }
        else if (Suffix == 7)
        {
            DecMinAdjVar = -0.0000005m;
            DecMaxAdjVar = 0.0000005m;
            DecCompVar = 0.0000000m;
        }
        else if (Suffix == 8)
        {
            DecMinAdjVar = -0.00000005m;
            DecMaxAdjVar = 0.00000005m;
            DecCompVar = 0.00000000m;
        }
        else if (Suffix == 9)
        {
            DecMinAdjVar = -0.000000005m;
            DecMaxAdjVar = 0.000000005m;
            DecCompVar = 0.000000000m;
        }
        else if (Suffix == 10)
        {
            DecMinAdjVar = -0.0000000005m;
            DecMaxAdjVar = 0.0000000005m;
            DecCompVar = 0.0000000000m;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        FunPriLoadPage();

    }

    private void FunPriLoadPage()
    {
        try
        {
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            strCurerncy = ObjS3GSession.ProCurrencyNameRW;
            //            Session["Date"] = DateTime.Now.ToString(strDateFormat) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString();
            Session["Date"] = DateTime.Now.ToString(ObjS3GSession.ProDateFormatRW) + " " + DateTime.Now.ToString().Split(' ')[1].ToString() + " " + DateTime.Now.ToString().Split(' ')[2].ToString(); ;
            ObjUserInfo = new UserInfo();
            intCompanyId = ObjUserInfo.ProCompanyIdRW;
            intUserId = ObjUserInfo.ProUserIdRW;
            CalendarExtenderSD_TxtRcptDate.Format = ObjS3GSession.ProDateFormatRW;
            /* Changed Date Control start - 30-Nov-2012 */
            TxtRcptDate.Attributes.Add("onblur", "fnDoDate(this,'" + TxtRcptDate.ClientID + "','" + strDateFormat + "',true,  false);");
            /* Changed Date Control end - 30-Nov-2012 */
            if (!IsPostBack)
            {
                FunPriLoadLob(intCompanyId, intUserId);
                FunPriLoadRegion(intCompanyId, Is_Active);
                FunPriLoadAccountStatus();
                //FunPriLoadBranch(intCompanyId, intUserId, RegionId, Is_Active);
                FunPubLoadDenomination();
                PnlAccDPDReport.Visible = false;
                PnlAsstDPDReport.Visible = false;
                ChkSubRep_CheckedChanged(this, new EventArgs());
                ddlBranch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load DPD page");
        }
    }

    private bool FunPriChkDemandMonthsExists(int CompanyID, int LobID, string DemandMonth)
    {
        try
        {

            byte[] byteDemandMonths = ObjOrgColClient.FunPubCheckDemandMonthExists(CompanyID, LobID, DemandMonth);
            DataSet DSDemand = (DataSet)ClsPubSerialize.DeSerialize(byteDemandMonths, SerializationMode.Binary, typeof(DataSet));
            if (DSDemand.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriLoadLob(int intCompany_id, int intUser_id)
    {
        try
        {
            ProcParam = new Dictionary<string, string>();
            if (ProcParam != null)
                ProcParam.Clear();

            ProcParam.Add("@Company_ID", Convert.ToString(intCompany_id));
            ProcParam.Add("@User_ID", intUserId.ToString());
            //Procparam.Add("@Program_Code", ProgramCode);
            ProcParam.Add("@Program_Id", "182");
            ProcParam.Add("@Is_Active", "1");
            ddlLOB.BindDataTable(SPNames.LOBMaster, ProcParam, true, "-- Select --", new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            if (ddlLOB.Items.Count == 2)
            {
                ddlLOB.Items.RemoveAt(0);
                ddlLOB.Items[0].Selected = true;
                ddlLOB_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                ddlLOB.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
        finally
        {
            //objSerClient.Close();
        }
    }
    private void FunPriLoadRegion(int intCompany_id, bool Is_active)
    {
        try
        {
            ProcParam = new Dictionary<string, string>();
            if (ProcParam != null)
                ProcParam.Clear();
            ProcParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            ProcParam.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
            ProcParam.Add("@Is_Active", "1");
            ProcParam.Add("@Program_Id", "182");
            ProcParam.Add("@LOB_ID", ddlLOB.SelectedValue);
            ProcParam.Add("@Is_Report", "1");
            ddlRegion.BindDataTable(SPNames.BranchMaster_LIST, ProcParam, true, "-- Select --", new string[] { "Location_ID", "Location" });
            ddlRegion.Items[0].Text = "--ALL--";
            if (ddlRegion.Items.Count == 2)
            {
                ddlRegion.Items[1].Selected = true;
                DDRegion_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, ProcParam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                ddlBranch.Items[0].Text = "--ALL--";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }

    private void FunPriLoadAccountStatus()
    {
        try
        {
            ProcParam = new Dictionary<string, string>();
            if (ProcParam != null)
                ProcParam.Clear();
            ProcParam.Add("@Option", "789");
            ddlAccountStatus.BindDataTable("S3G_ORG_GetCustomerLookUp", ProcParam, true, "-- Select --", new string[] { "Lookup_Code", "Lookup_Description" });
            ddlAccountStatus.Items[0].Text = "--ALL--";
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }

    private void FunPriLoadBranch(int intCompany_id, int intUser_id, string RegionId, bool Is_active)
    {
        try
        {
            string Region = string.Empty;
            if (ddlRegion.SelectedIndex != 0)
            {
                Region = ddlRegion.SelectedValue;
            }

            byte[] byteLobs = objSerClient.FunPubGetRegBranch(intCompany_id, intUser_id, Region, true);
            List<ClsPubDropDownList> Branch = (List<ClsPubDropDownList>)DeSerialize(byteLobs);
            ddlBranch.DataSource = Branch;
            ddlBranch.DataTextField = "Description";
            ddlBranch.DataValueField = "ID";
            ddlBranch.DataBind();

            if (ddlBranch.Items.Count == 2)
            {
                if (ddlRegion.SelectedIndex != 0)
                {
                    ddlBranch.SelectedIndex = 1;
                    Utility.ClearDropDownList(ddlBranch);
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
            //throw ex;
        }
    }
    public void FunPubLoadDenomination()
    {
        try
        {
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
            //throw ex;
        }
    }
    private object DeSerialize(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
    protected void DDRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRegion.SelectedIndex == 0)
            {
                ddlBranch.Enabled = false;
                ddlBranch.Items.Clear();
                ProcParam = new Dictionary<string, string>();
                if (ProcParam != null)
                    ProcParam.Clear();
                ProcParam.Add("@Company_ID", Convert.ToString(intCompanyId));
                ProcParam.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
                ProcParam.Add("@Is_Active", "1");
                ProcParam.Add("@Program_Id", "182");
                ProcParam.Add("@LOB_ID", ddlLOB.SelectedValue);
                ProcParam.Add("@Is_Report", "1");
                ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, ProcParam, true, "-- Select --", new string[] { "Location_ID", "Location" });
                ddlBranch.Items[0].Text = "--ALL--";
                GrvAccDPDReport.DataSource = null;
                GrvAccDPDReport.DataBind();
                // btnPrintExcel.Visible = false;
                btnPrintAccDPDReport.Visible = false;
                PnlAccDPDReport.Visible = false;
            }
            else
            {
                FunPubLoadChildLocation(Convert.ToInt32(ddlRegion.SelectedValue));
                GrvAccDPDReport.DataSource = null;
                GrvAccDPDReport.DataBind();
                //btnPrintExcel.Visible = false;
                btnPrintAccDPDReport.Visible = false;
                PnlAccDPDReport.Visible = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    //public DataTable FunPriCalculateSumAmount(DataTable objDataTable, string strGroupByField, string strGroupByField2, string strSumField)
    //{
    //    DataTable objReturnDataTable = new DataTable();
    //    objReturnDataTable.Columns.Add(strGroupByField);
    //    objReturnDataTable.Columns.Add(strSumField);
    //    objReturnDataTable.Columns.Add(strGroupByField2);
    //    var varQuery = from row in objDataTable.AsEnumerable()
    //                   group row by new { Column1 = row.Field<string>("Column1"), Column2 = row.Field<string>("Column2") } into grp
    //                   orderby grp.Key
    //                   select new { Id = grp.Key.Column1,Name=grp.Key.Column2,Sum = grp.Sum(r => r.Field<Decimal>(strSumField)) };


    //    foreach (var varCollection in varQuery)
    //    {
    //        DataRow objDataRow = objReturnDataTable.NewRow();
    //        objDataRow[strGroupByField] = varCollection.Id;
    //        objDataRow[strGroupByField2] = varCollection.Name;
    //        objDataRow[strSumField] = varCollection.Sum;
    //        objReturnDataTable.Rows.Add(objDataRow);
    //    }
    //    return objReturnDataTable;
    //}
    public void FunPubLoadChildLocation(int LocationID)
    {
        try
        {
            ProcParam = new Dictionary<string, string>();
            if (ProcParam != null)
                ProcParam.Clear();
            ProcParam.Add("@PROGRAM_ID", "182");
            ProcParam.Add("@USER_ID", intUserId.ToString());
            ProcParam.Add("@COMPANY_ID", intCompanyId.ToString());
            ProcParam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            ProcParam.Add("@LOCATION_ID", LocationID.ToString());
            DataSet DSChild = new DataSet();
            DSChild = Utility.GetDataset("S3G_RPT_LEVELVALUE", ProcParam);

            ddlBranch.DataSource = DSChild.Tables[0];
            ddlBranch.DataTextField = "Location";
            ddlBranch.DataValueField = "Location_ID";
            ddlBranch.DataBind();
            if ((DSChild.Tables[0].Rows.Count == 1))
            {
                //if ((DSChild.Tables[0].Rows[0]["Location"].ToString() != ddlBranch.SelectedItem.Text))
                //{
                //    ListItem Lst = new ListItem("--ALL--", "ALL");
                //    ddlLoc2.Items.Insert(0, Lst);
                //    ddlLoc2.Enabled = true;
                //}
                //else
                //{
                ddlBranch.Items[0].Selected = true;
                ddlBranch.Enabled = false;
                //}
            }
            else if (DSChild.Tables[0].Rows.Count > 1)
            {
                ListItem Lst = new ListItem("--ALL--", "0");
                ddlBranch.Items.Insert(0, Lst);
                ddlBranch.Enabled = true;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPubBindGrid();
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
    public void FunPubBindGrid()
    {
        try
        {
            string date = "01" + "/" + TxtCutOff.Text.Substring(4, 2) + "/" + TxtCutOff.Text.Substring(0, 4);
            if (Utility.CompareDates(date, DateTime.Now.ToString()) == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Demand month should not be greater than the system month");
                return;
            }

            if (!FunPriChkDemandMonthsExists(ObjUserInfo.ProCompanyIdRW, Convert.ToInt32(ddlLOB.SelectedValue), TxtCutOff.Text))
            {
                Utility.FunShowAlertMsg(this.Page, "The Demand has not been run for the Demand month which has been selected");
                return;
            }
            if (ChkSubRep.Checked == true)
            {
                if (TxtRcptDate.Text.ToString().Trim() == "")
                {
                    Utility.FunShowAlertMsg(this.Page, "Receipt date should not be empty!");
                    return;
                }
                if (Utility.CompareDates(TxtRcptDate.Text.ToString(), DateTime.Now.ToString()) == -1)
                {
                    Utility.FunShowAlertMsg(this.Page, "Receipt date should not be greater than the System date");
                    return;
                }
                if (Utility.CompareDates(TxtRcptDate.Text.ToString(), date.ToString()) == 1)
                {
                    Utility.FunShowAlertMsg(this.Page, "Receipt month should not be less than the Demand month");
                    return;
                }
            }
            Session["Month"] = TxtCutOff.Text.ToString();
            //if (Rdreport.SelectedValue == "1")
            //{
            bool isSelect = false;
            PnlAccDPDReport.Visible = true;
            PnlAsstDPDReport.Visible = false;
            foreach (GridViewRow gvrRow in GRVbucket.Rows)
            {
                CheckBox ChkSelect = (CheckBox)gvrRow.FindControl("ChkSelect");
                Label lblbucket = (Label)gvrRow.FindControl("lblbucket");
                Label lblFromDays = (Label)gvrRow.FindControl("lblFromDays");
                Label lblToDays = (Label)gvrRow.FindControl("lblToDays");
                if (ChkSelect.Checked == true)
                {
                    isSelect = true;
                    BucketDetails += "Bucket" + lblbucket.Text + "|" + lblFromDays.Text + " and " + lblToDays.Text + "|~";
                }
            }
            if (isSelect == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Select atleast one bucket to proceed");
                PnlAccDPDReport.Visible = false;
                PnlAsstDPDReport.Visible = false;
                btnPrintAccDPDReport.Visible = false;
                // btnPrintExcel.Visible = false;
                return;
            }
            string RcptDate;
            if (TxtRcptDate.Text != string.Empty)
            {
                RcptDate = Convert.ToDateTime(TxtRcptDate.Text).Date.ToString("MM/dd/yyyy");
            }
            else
            {
                RcptDate = "";
            }
            Session["Bucket"] = BucketDetails.ToString();
            Session["LOB"] = ddlLOB.SelectedValue;
            Session["Denom"] = Convert.ToInt32(ddlDenomination.SelectedValue);
            Session["RCPT"] = RcptDate;
            Session["Location1"] = ddlRegion.SelectedValue;
            Session["Location2"] = ddlBranch.SelectedValue;
            Session["AccountStatus"] = ddlAccountStatus.SelectedValue;
            byte[] byteDSDPDReport = ObjOrgColClient.FunPubGetDPDReportDetails(Convert.ToInt32(ddlLOB.SelectedValue), TxtCutOff.Text, Convert.ToInt32(ddlDenomination.SelectedValue), BucketDetails.ToString(), RcptDate, intCompanyId, Convert.ToInt32(ddlRegion.SelectedValue), Convert.ToInt32(ddlBranch.SelectedValue), 182, intUserId, Convert.ToInt32(ddlAccountStatus.SelectedValue));
            ReportMgtServices DSDPDReport = new ReportMgtServices();
            DSDPDReport = (ReportMgtServices)ClsPubSerialize.DeSerialize(byteDSDPDReport, SerializationMode.Binary, typeof(ReportMgtServices));

            if (DSDPDReport.Tables.Count == 2)
            {
                if (DSDPDReport.Tables[1].Rows[0]["RESULT"].ToString() == "No records Found")
                {
                    GrvAccDPDReport.EmptyDataText = "No Records were found for the Region/Branch for this month";
                    GrvAccDPDReport.DataBind();
                    btnPrintAccDPDReport.Visible = false;
                    // btnPrintExcel.Visible = false;
                    return;
                }
            }
            if (DSDPDReport.Tables[2].Rows.Count > 0)
            {

                //for (int rowindex = 0; rowindex <= DSDPDReport.Tables[2].Rows.Count - 1; rowindex++)
                //{
                //    decimal total = 0;
                //    for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                //    {
                //        total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString());
                //    }
                //    if ((total < 100) && ((total - 100) < Convert.ToDecimal(-0.0005)))
                //    {
                //        total = 0;
                //        for (int colindex4 = 7; colindex4 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex4 = colindex4 + 2)
                //        {
                //            total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex4].ToString());
                //        }
                //        DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                //        DSDPDReport.Tables[2].AcceptChanges();
                //        continue;
                //    }
                //    decimal diff = total - 100;
                //    if ((diff > 0))
                //    {
                //        total = 0;
                //        for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                //        {
                //            if (DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString() != "0.0000")
                //            {
                //                DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex]) - diff;
                //                DSDPDReport.Tables[2].AcceptChanges();
                //                for (int colindex2 = 7; colindex2 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex2 = colindex2 + 2)
                //                {
                //                    total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex2].ToString());
                //                }
                //                DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                //                DSDPDReport.Tables[2].AcceptChanges();
                //                break;
                //            }
                //        }
                //    }
                //    else if (((diff < Convert.ToDecimal(-0.0001)) || (diff > Convert.ToDecimal(-0.0005))) && (diff != 0))
                //    {
                //        total = 0;
                //        for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                //        {
                //            if (DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString() != "0.0000")
                //            {
                //                DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex]) - (diff);
                //                DSDPDReport.Tables[2].AcceptChanges();
                //                for (int colindex3 = 7; colindex3 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex3 = colindex3 + 2)
                //                {
                //                    total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex3].ToString());
                //                }
                //                DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                //                DSDPDReport.Tables[2].AcceptChanges();
                //                break;
                //            }
                //        }
                //    }
                //}

                for (int rowindex = 0; rowindex <= DSDPDReport.Tables[2].Rows.Count - 1; rowindex++)
                {
                    try
                    {
                        decimal total = 0;
                        for (int colindex = 5; colindex <= DSDPDReport.Tables[2].Columns.Count - 1; colindex++)
                        {
                            DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal(Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString()).ToString(Funsetsuffix()));
                        }
                        DSDPDReport.Tables[2].AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        Utility.FunShowAlertMsg(this.Page, "Unable to generate DPD Report!");
                        PnlAccDPDReport.Visible = false;
                          ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
                        //throw ex;
                        return;
                    }
                }


                for (int rowindex = 0; rowindex <= DSDPDReport.Tables[2].Rows.Count - 1; rowindex++)
                {
                    decimal total = 0;
                    for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                    {
                        total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString());
                    }

                    if ((total < Convert.ToDecimal(99.95)) || (total > Convert.ToDecimal(100.05)))
                    {
                        total = 0;
                        for (int colindex4 = 7; colindex4 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex4 = colindex4 + 2)
                        {
                            total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex4].ToString());
                        }
                        DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                        DSDPDReport.Tables[2].AcceptChanges();
                        continue;
                    }
                    //decimal diff = 100-total;

                    if ((total >= Convert.ToDecimal(99.95)) && (total <= Convert.ToDecimal(100.05)))
                    {
                        decimal diff = 100 - total;

                        //if (total > 100)
                        //{
                        //    total = total + diff;
                        //}
                        //else if(total < 100)
                        //{
                        //    total = total + diff;
                        //}
                        total = 0;
                        for (int colindex = 7; colindex <= DSDPDReport.Tables[2].Columns.Count - 2; colindex = colindex + 2)
                        {
                            if (Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex].ToString()) != DecCompVar)
                            {
                                DSDPDReport.Tables[2].Rows[rowindex][colindex] = Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex]) + diff;
                                DSDPDReport.Tables[2].AcceptChanges();
                                for (int colindex2 = 7; colindex2 <= DSDPDReport.Tables[2].Columns.Count - 2; colindex2 = colindex2 + 2)
                                {
                                    total += Convert.ToDecimal(DSDPDReport.Tables[2].Rows[rowindex][colindex2].ToString());
                                }
                                DSDPDReport.Tables[2].Rows[rowindex][DSDPDReport.Tables[2].Columns.Count - 1] = total;
                                DSDPDReport.Tables[2].AcceptChanges();
                                break;
                            }
                        }
                    }

                }
                GrvAccDPDReport.DataSource = DSDPDReport.Tables[2];
                GrvAccDPDReport.DataBind();
                btnPrintAccDPDReport.Visible = true;
                // btnPrintExcel.Visible = true;
                if (ddlDenomination.SelectedItem.Text != "Actual")
                {
                    LblDenomination.Text = "[All Amounts are " + ObjS3GSession.ProCurrencyNameRW + " in " + ddlDenomination.SelectedItem.Text.ToString() + "]";
                }
                else
                {
                    LblDenomination.Text = "[All Amounts are in " + ObjS3GSession.ProCurrencyNameRW + "]";
                }
                DataSet DSBucket = (DataSet)Session["Buckets"];
                btnPrintAccDPDReport.Visible = true;
                FunPubNameColumns(GrvAccDPDReport, DSDPDReport, DSBucket);

            }
            else
            {
                GrvAccDPDReport.EmptyDataText = "No Records Found!";
                GrvAccDPDReport.DataBind();
                btnPrintAccDPDReport.Visible = false;
                // btnPrintExcel.Visible = false;
            }
            //}
            //else
            //{
            //    PnlAccDPDReport.Visible = false;
            //    PnlAsstDPDReport.Visible = true;
            //    DataSet DSASSETCLASS = new DataSet();
            //    ProcParam = new Dictionary<string, string>();
            //    ProcParam.Add("@Company_ID", intCompanyId.ToString());
            //    ProcParam.Add("@Category_Type", "CLASS");
            //    DSASSETCLASS = Utility.GetDataset("S3G_CLN_GetAssetCategory", ProcParam);
            //    GrvAsstDPDReport.DataSource = DSASSETCLASS.Tables[0];
            //    GrvAsstDPDReport.DataBind();
            //}

        }
        catch (Exception ex)
        {
            Utility.FunShowAlertMsg(this.Page, "Unable to generate DPD Report!");
            PnlAccDPDReport.Visible = false;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
            return;
        }
    }

    public void FunPubNameColumns(GridView Grv, DataSet DSColumnDef, DataSet DsBucket)
    {
        try
        {

            for (int RowIndex = 0; RowIndex < DSColumnDef.Tables[1].Rows.Count; RowIndex++)
            {
                string strColumns = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString();

                if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "LOCATION_NAME")
                {
                    Grv.HeaderRow.Cells[RowIndex].Text = "Location Name";
                }
                else if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "CUSTOMER_CODE")
                {
                    Grv.HeaderRow.Cells[RowIndex].Text = "Customer Code";
                }
                else if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "CUSTOMER_NAME" && !string.IsNullOrEmpty(DSColumnDef.Tables[1].Rows[RowIndex][0].ToString()))
                {
                    Grv.HeaderRow.Cells[RowIndex].Text = "Customer Name";
                }
                else if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "PANUM")
                {
                    Grv.HeaderRow.Cells[RowIndex].Text = "Prime Account Number";
                }
                else if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "Sanum")
                {
                    Grv.HeaderRow.Cells[RowIndex].Text = "Sub Account Number";
                }
                else
                {
                    if (RowIndex == 5)
                    {
                        if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "DEMAND_ARREARS")
                        {
                            Grv.HeaderRow.Cells[RowIndex].Text = "Demand Arrears";
                        }
                    }

                    else if (RowIndex > 5)
                    {

                        if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Contains("%") == false)
                        {
                            if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "TOTAL")
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString();
                            }
                            else
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString() + "<br>" + (DsBucket.Tables[0].Select("Bucket = " + DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_From"].ToString() + " - " + DsBucket.Tables[0].Select("Bucket = " + DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_To"].ToString());
                            }
                        }
                        else if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Contains("_PERCENTAGE") == false)
                        {
                            if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().ToUpper() == "TOTAL")
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString();
                            }
                            else
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString() + "<br>" + (DsBucket.Tables[0].Select("Bucket = " + DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_From"].ToString() + " - " + DsBucket.Tables[0].Select("Bucket = " + DSColumnDef.Tables[1].Rows[RowIndex][0].ToString().Replace("Bucket", ""))[0]["Bucket_To"].ToString());
                            }
                        }
                        else
                        {
                            if (DSColumnDef.Tables[1].Rows[RowIndex][0].ToString() == "total%" || DSColumnDef.Tables[1].Rows[RowIndex][0].ToString() == "TOTAL_PERCENTAGE")
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString();
                            }
                            else
                            {
                                Grv.HeaderRow.Cells[RowIndex].Text = "Branch%";
                            }
                        }
                    }
                    else
                    {
                        Grv.HeaderRow.Cells[RowIndex].Text = DSColumnDef.Tables[1].Rows[RowIndex][0].ToString();
                    }
                }


            }

            for (int rowindex = 0; rowindex < Grv.Rows.Count; rowindex++)
            {

                for (int i = 0; i < Grv.Rows[rowindex].Cells.Count; i++)
                {
                    if (i > 4)
                    {
                        Grv.Rows[rowindex].Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    }
                    else
                    {
                        Grv.Rows[rowindex].Cells[i].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void ChkSubRep_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ChkSubRep.Checked == true)
            {
                TxtRcptDate.Enabled = true;
                RqvTxtRcptDate.Enabled = true;
                CalendarExtenderSD_TxtRcptDate.Enabled = true;
                GrvAccDPDReport.ClearGrid();
                PnlAccDPDReport.Visible = false;
            }
            else
            {
                TxtRcptDate.Enabled = false;
                TxtRcptDate.Clear();
                RqvTxtRcptDate.Enabled = false;
                CalendarExtenderSD_TxtRcptDate.Enabled = false;
                GrvAccDPDReport.ClearGrid();
                PnlAccDPDReport.Visible = false;
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;

        }

    }
    protected void btnPrintAccDPDReport_Click(object sender, EventArgs e)
    {
        Session["Denomination"] = ddlDenomination.SelectedItem.Text;
        string strScipt = "window.open('../Reports/S3GDPDRPT.aspx', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "dpd", strScipt, true);
    }
    protected void LnkAssetClass_Click(object sender, EventArgs e)
    {
        GridViewRow GvrClass = (GridViewRow)((LinkButton)sender).NamingContainer;
        string ClassID = ((LinkButton)GvrClass.FindControl("LnkAssetClass")).CommandArgument.ToString();
        foreach (GridViewRow gvrRow in GRVbucket.Rows)
        {
            CheckBox ChkSelect = (CheckBox)gvrRow.FindControl("ChkSelect");
            Label lblbucket = (Label)gvrRow.FindControl("lblbucket");
            Label lblFromDays = (Label)gvrRow.FindControl("lblFromDays");
            Label lblToDays = (Label)gvrRow.FindControl("lblToDays");
            if (ChkSelect.Checked == true)
            {
                BucketDetails += "Bucket" + lblbucket.Text + "|" + lblFromDays.Text + " and " + lblToDays.Text + "|~";
            }
        }
        byte[] byteDSDPDReport = ObjOrgColClient.FunPubGetAssetDPDReportDetails(Convert.ToInt32(ddlLOB.SelectedValue), Convert.ToInt32(ClassID), ddlLOB.SelectedItem.Text, TxtCutOff.Text, Convert.ToInt32(ddlDenomination.SelectedValue), BucketDetails.ToString(), TxtRcptDate.Text, intCompanyId, Convert.ToInt32(ddlBranch.SelectedValue), Convert.ToInt32(ddlRegion.SelectedValue));
        ReportMgtServices DSDPDReport = new ReportMgtServices();
        DSDPDReport = (ReportMgtServices)ClsPubSerialize.DeSerialize(byteDSDPDReport, SerializationMode.Binary, typeof(ReportMgtServices));
        Session["DPDRPT"] = DSDPDReport;
        PnlAccDPDReport.Visible = true;
        if (DSDPDReport.Tables.Count == 2)
        {
            if (DSDPDReport.Tables[1].Rows[0]["RESULT"].ToString() == "No records Found")
            {
                GrvAccDPDReport.EmptyDataText = "No Records were found for the Region/Branch for this month";
                GrvAccDPDReport.DataBind();
                btnPrintAccDPDReport.Visible = false;
                return;
            }
        }
        if (DSDPDReport.Tables[2].Rows.Count > 0)
        {
            GrvAccDPDReport.DataSource = DSDPDReport.Tables[2];
            GrvAccDPDReport.DataBind();
            btnPrintAccDPDReport.Visible = true;
            for (int RowIndex = 0; RowIndex < DSDPDReport.Tables[1].Rows.Count + 2; RowIndex++)
            {
                if (RowIndex < DSDPDReport.Tables[1].Rows.Count)
                {
                    if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Panum")
                    {
                        GrvAccDPDReport.HeaderRow.Cells[RowIndex].Text = "Prime Account Number";
                    }
                    else if (DSDPDReport.Tables[1].Rows[RowIndex][0].ToString() == "Sanum")
                    {
                        GrvAccDPDReport.HeaderRow.Cells[RowIndex].Text = "Sub Account Number";
                    }
                    else
                    {
                        GrvAccDPDReport.HeaderRow.Cells[RowIndex].Text = DSDPDReport.Tables[1].Rows[RowIndex][0].ToString();
                    }
                }
                else if (RowIndex == DSDPDReport.Tables[1].Rows.Count)
                {
                    GrvAccDPDReport.HeaderRow.Cells[RowIndex].Text = "Total";
                }
                else if (RowIndex == DSDPDReport.Tables[1].Rows.Count + 1)
                {
                    GrvAccDPDReport.HeaderRow.Cells[RowIndex].Text = "Total%";
                }

            }

        }
        else
        {
            GrvAccDPDReport.EmptyDataText = "No Records Found!";
            GrvAccDPDReport.DataBind();
            btnPrintAccDPDReport.Visible = false;


        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLOB.ClearSelection();
            ddlDenomination.ClearSelection();
            ddlRegion.ClearSelection();
            ddlBranch.ClearSelection();
            ddlRegion.Items.Clear();
            ddlBranch.Items.Clear();
            ddlAccountStatus.Items.Clear();
            FunPriLoadRegion(intCompanyId, true);
            FunPriLoadAccountStatus();
            ddlBranch.Enabled = false;
            TxtCutOff.Clear();
            TxtRcptDate.Clear();
            //Rdreport.ClearSelection();
            ChkSubRep.Checked = false;
            ChkSubRep_CheckedChanged(this, new EventArgs());
            GrvAccDPDReport.DataSource = null;
            GrvAccDPDReport.DataBind();
            GrvAsstDPDReport.DataSource = null;
            GrvAsstDPDReport.DataBind();
            GRVbucket.DataSource = null;
            GRVbucket.DataBind();
            GrvAccDPDReport.EmptyDataText = "";
            GrvAccDPDReport.ClearGrid();
            PnlAccDPDReport.Visible = false;
            PnlAsstDPDReport.Visible = false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }

    }

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlLOB.SelectedIndex == 0)
            {
                ddlBranch.Enabled = false;
                FunPriLoadRegion(intCompanyId, true);
            }
            else
            {
                FunPriLoadRegion(intCompanyId, true);
            }

            GRVbucket.Visible = true;
            byte[] byteLobs = ObjOrgColClient.FunPubGetDPDBucketDetails(Convert.ToInt32(ddlLOB.SelectedValue), 10);
            GRVbucket.DataSource = DeSerialize(byteLobs);
            GRVbucket.DataBind();
            if (GRVbucket.Rows.Count == 0)
            {
                TxtCutOff.Text = "";
                TxtRcptDate.Text = "";
                ChkSubRep.Checked = false;
                ChkSubRep_CheckedChanged(this, new EventArgs());
                GRVbucket.EmptyDataText = "No Details Found";
                GRVbucket.DataBind();
                GrvAccDPDReport.DataSource = null;
                GrvAccDPDReport.DataBind();
                // btnPrintExcel.Visible = false;
                btnPrintAccDPDReport.Visible = false;
                PnlAccDPDReport.Visible = false;
            }
            else
            {
                GrvAccDPDReport.DataSource = null;
                GrvAccDPDReport.DataBind();
                // btnPrintExcel.Visible = false;
                btnPrintAccDPDReport.Visible = false;
                PnlAccDPDReport.Visible = false;
            }
            Session["Buckets"] = DeSerialize(byteLobs);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }


    protected void TxtCutOff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(ddlLOB.SelectedValue) == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select the  Line of Business to check if the demand has been run for the month");
                TxtCutOff.Clear();
                TxtCutOff.Focus();
                return;
            }
            string date = "01" + "/" + TxtCutOff.Text.Substring(4, 2) + "/" + TxtCutOff.Text.Substring(0, 4);
            GrvAccDPDReport.DataSource = null;
            GrvAccDPDReport.DataBind();
            // btnPrintExcel.Visible = false;
            btnPrintAccDPDReport.Visible = false;
            PnlAccDPDReport.Visible = false;
            if (Utility.CompareDates(date, DateTime.Now.ToString()) == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Demand month should not be greater than the system month");
                TxtCutOff.Clear();
                TxtCutOff.Focus();
                return;
            }
            if (!FunPriChkDemandMonthsExists(ObjUserInfo.ProCompanyIdRW, Convert.ToInt32(ddlLOB.SelectedValue), TxtCutOff.Text))
            {
                Utility.FunShowAlertMsg(this.Page, "The Demand has not been run for the Demand month which has been selected");
                TxtCutOff.Clear();
                TxtCutOff.Focus();
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;


        }
    }
    protected void TxtRcptDate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TxtCutOff.Text == string.Empty)
            {
                Utility.FunShowAlertMsg(this.Page, "Select the Demand Month");
                TxtRcptDate.Clear();
                TxtCutOff.Focus();
                return;
            }
            GrvAccDPDReport.DataSource = null;
            GrvAccDPDReport.DataBind();
            // btnPrintExcel.Visible = false;
            btnPrintAccDPDReport.Visible = false;
            PnlAccDPDReport.Visible = false;
            string date = "01" + "/" + TxtCutOff.Text.Substring(4, 2) + "/" + TxtCutOff.Text.Substring(0, 4);
            if (Utility.CompareDates(TxtRcptDate.Text.ToString(), DateTime.Now.ToString()) == -1)
            {
                Utility.FunShowAlertMsg(this.Page, "Receipt date should not be greater than the System date");
                TxtRcptDate.Clear();
                TxtRcptDate.Focus();
                return;
            }
            if (Utility.CompareDates(TxtRcptDate.Text.ToString(), date.ToString()) == 1)
            {
                Utility.FunShowAlertMsg(this.Page, "Receipt date should not be less than the Demand month");
                TxtRcptDate.Clear();
                TxtRcptDate.Focus();
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void btnPrintExcel_Click(object sender, EventArgs e)
    {
        string FileName = "test";
        //DataSet DSBucket = (DataSet)Session["Buckets"];
        //GridView GrvExcel = new GridView();
        //GrvExcel.DataSource = ((DataSet)Session["DPDRPT"]).Tables[2];
        //GrvExcel.DataBind();
        //FunPubNameColumns(GrvExcel, ((DataSet)Session["DPDRPT"]), DSBucket);
        GrvAccDPDReport.FunPubExportGrid("DCIncentiveProcessed", enumFileType.Excel);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrvAccDPDReport.DataSource = null;
            GrvAccDPDReport.DataBind();
            // btnPrintExcel.Visible = false;
            btnPrintAccDPDReport.Visible = false;
            PnlAccDPDReport.Visible = false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }
    protected void ddlDenomination_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrvAccDPDReport.DataSource = null;
            GrvAccDPDReport.DataBind();
            // btnPrintExcel.Visible = false;
            btnPrintAccDPDReport.Visible = false;
            PnlAccDPDReport.Visible = false;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            //throw ex;
        }
    }

    protected void GrvAccDPDReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrvAccDPDReport.PageIndex = e.NewPageIndex;
        FunPubBindGrid();
    }
}