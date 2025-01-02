/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Insurance 
/// Screen Name               : Insurance Remainder
/// Created By                : Gangadharrao
/// Created Date              : 08-April-2011
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#region NameSpaces

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
using System.Text;
using System.Globalization;
using System.ServiceModel;
using AjaxControlToolkit;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

#endregion

public partial class Insurance_S3GInsInsurancRemainder : ApplyThemeForProject
{
    #region [Common Variable declaration]
    DataTable dtInsDetails;
    int intCompanyID, intUserID = 0;
    string strPageTitle = "Insurance Reminder";
    Dictionary<string, string> Procparam = null;
    int intErrCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    StringBuilder strInsDetailsBuilder = new StringBuilder();
    int intResult;
    string strKey = "Update";
    string strpath = "";
    StringBuilder strPASABuilder = new StringBuilder();
    //  StringBuilder strChallanBuilder = new StringBuilder();
    // DataTable dtAddLessDetails;
    //string strAccountXml = "";
    //StringBuilder strAccountDetailsBuilder = new StringBuilder();
    //string strKey = "Insert";
    S3GSession ObjS3GSession = new S3GSession();
    string strAlert = "alert('__ALERT__');";
    //string strRedirectPage = "~/Collection/S3gCLNTransLander.aspx?Code=CRP";
    string strRedirectPage = "window.location.href='../Insurance/S3GInsInsuranceRemainder.aspx';";
    //string strRedirectPageView = "window.location.href='../Collection/S3GCLNTranslander.aspx?Code=CRP';";
    public static Insurance_S3GInsInsurancRemainder obj_Page;
    ////User Authorization
    //string strMode = string.Empty;
    //bool bCreate = false;
    //bool bClearList = false;
    //bool bModify = false;
    //bool bQuery = false;
    //bool bDelete = false;
    //bool bMakerChecker = false;
    //Code end
    #endregion

    #region [PageLoad Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = null;
        try
        {
            obj_Page = this;
            this.Page.Title = strPageTitle;
            lblHeading.Text = strPageTitle;
            #region [Assign DateFormate]
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            calInsDueFromDate.Format = ObjS3GSession.ProDateFormatRW;
            calInsDueToDate.Format = ObjS3GSession.ProDateFormatRW;
            // making the end date textbox readonly
            //txtInsDueFromDate.Attributes.Add("readonly", "readonly");
            //txtInsDueToDate.Attributes.Add("readonly", "readonly");
            txtInsDueFromDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInsDueFromDate.ClientID + "','" + strDateFormat + "',false,  false);");
            txtInsDueToDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtInsDueToDate.ClientID + "','" + strDateFormat + "',false,  false);");

            #endregion
            FunPriLoadPage();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            ObjS3GSession = null;
            ObjUserInfo = null;
        }

    }
    #endregion

    #region [Methods]

    private void FunToolTip()
    {
        ddlLOB.ToolTip = lblLOB.Text;
        ddlBranch.ToolTip = lblBranch.Text;
        ddlRemainder.ToolTip = lblRemainder.Text;
        ddlInsDoneBy.ToolTip = lblInsDoneBy.Text;
        ddlOutput.ToolTip = lblOutput.Text;
        txtPath.ToolTip = lblPath.Text;
        ddlPANum.ToolTip = lblPANum.Text;
        ddlSANum.ToolTip = lblSANum.Text;
        txtInsDueFromDate.ToolTip = lblInsDueFromDate.Text;
        txtInsDueToDate.ToolTip = lblInsDueToDate.Text;
    }


    private void FunClear()
    {
        ddlBranch.SelectedValue = "0";
        ddlLOB.SelectedValue = "0";
        ddlOutput.SelectedValue = "0";
        ddlPANum.SelectedValue = "0";
        ddlSANum.SelectedValue = "0";
        ddlRemainder.SelectedValue = "0";
        ddlInsDoneBy.SelectedValue = "0";
     //   ddlOutput.SelectedValue = "0";
        txtInsDueFromDate.Text = "";
        txtInsDueToDate.Text = "";
        txtInsDueFromDate.Visible = false;
        txtInsDueToDate.Visible = false;
        imgInsDueFromDate.Visible = false;
        imgInsDueToDate.Visible = false;
        txtPath.Text = "";
        ddlLOB.Focus();
        lblPANum.Visible = false;
        lblSANum.Visible = false;
        lblInsDueFromDate.Visible = false;
        lblInsDueToDate.Visible = false;
        pnlInsDetails.Visible = false;
        btnSave.Visible = false;
        btnClear.Visible = false;
        ddlPANum.Visible = false;
        ddlSANum.Visible = false;
        btnGenerate.Enabled = false;

    }

    private void FunSaveRemainderDetails()
    {
        InsuranceMgtServicesReference.InsuranceMgtServicesClient objInsuranceClient = new InsuranceMgtServicesReference.InsuranceMgtServicesClient();
        S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable objInsuraceDataTable = new S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryDataTable();
        S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceEntryRow objInsuranceRow = objInsuraceDataTable.NewS3G_INS_AssetInsuranceEntryRow();
        try
        {

            objInsuranceRow.Company_ID = intCompanyID;
            objInsuranceRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            objInsuranceRow.Branch_ID = Convert.ToInt32(ddlBranch.SelectedValue);
            objInsuranceRow.Customer_ID = 1;
            objInsuranceRow.PANum = "1";
            objInsuranceRow.Pay_Type = 1;
            objInsuranceRow.Ins_Company_Address = "1";
            objInsuranceRow.Ins_Company_City = "1";
            objInsuranceRow.Ins_Company_State = "1";
            objInsuranceRow.Ins_Company_Pin = "1";
            objInsuranceRow.Ins_Company_Country = "1";
            objInsuranceRow.Ins_Company_Telephone = "1";
            objInsuranceRow.Ins_Company_Email_ID = "1";
            objInsuranceRow.Ins_Company_Web_Site = "1";


            objInsuranceRow.XmlPolicyDetails = FunPASADetails();
            objInsuraceDataTable.AddS3G_INS_AssetInsuranceEntryRow(objInsuranceRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] objbyteInsuranceTable = ClsPubSerialize.Serialize(objInsuraceDataTable, SerMode);

            intResult = objInsuranceClient.FunPubModifyInsDetails(SerMode, objbyteInsuranceTable);


            if (intResult == 0)
            {
                Utility.FunShowAlertMsg(this, "Reminder Sent Successfully");
                FunClear();
                return;


            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {
            objInsuranceClient.Close();
            objInsuraceDataTable = null;
            Session["dtInsDetails"] = null;

        }
    }

    private void FunPriLoadPage()
    {

        try
        {
            // dtInsDetails = FunCreateTable();
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;

            if (!Page.IsPostBack)
            {
                ViewState["Path"] = "";
                FunPriLoadLOV();
                FunToolTip();
                btnGenerate.Enabled = false;
                if (ddlRemainder.SelectedIndex == 0)
                {
                    lblSANum.Visible = lblPANum.Visible = ddlPANum.Visible = ddlSANum.Visible = false;
                }
                if (ddlRemainder.SelectedIndex == 0 && ddlInsDoneBy.SelectedIndex == 0)
                {

                    lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = false;
                    imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;

                }
                if (grvInsDetails.Rows.Count == 0)
                {
                    btnClear.Visible = btnSave.Visible = false;
                }
            }

            if (grvInsDetails.Rows.Count ==1)
            {
                if (grvInsDetails.Rows[0].Cells[0].Text == "Insurance Renewal Details Not Found.")
                {
                    grvInsDetails.Rows[0].Cells.Clear();
                    grvInsDetails.Rows[0].Cells.Add(new TableCell());
                    //grvInsDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    grvInsDetails.HeaderRow.Visible = false;
                    grvInsDetails.Rows[0].Cells[0].Text = "Insurance Renewal Details Not Found.";
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
        finally
        {

        }
    }

    private void FunInsDetailsbyPAnum()
    {


        //if (dtInsDetails.Rows.Count > 0)
        //{
        //  var dataRowInsDetails = from row in dtInsDetails.AsEnumerable()
        //                            where row.Field<string>("PANum").Trim() == Convert.ToString(ddlPANum.SelectedItem.Text.Trim())
        //                            select new
        //                            {
        //                               PANum=row.Field<string>("PANum"),
        //                               SANum=row.Field<string>("SANum"),
        //                               CustomerName=row.Field<string>("CustomerName"),
        //                               AssetDescription = row.Field<string>("AssetDescription"),
        //                               AssetRegNo = row.Field<string>("AssetRegNo"),
        //                               InsuredBy = row.Field<string>("InsuredBy"),
        //                               PolicyNo = row.Field<string>("PolicyNo"),
        //                               PolicyExpiryDate = row.Field<string>("PolicyExpiryDate"),
        //                               InsuredAmount = row.Field<string>("InsuredAmount")
        //                            }; 

        //    grvInsDetails.DataSource = dataRowInsDetails;
        //    grvInsDetails.DataBind();
        //}
    }

    private void FunPriLoadLOV()
    {
        try
        {
            //if (Procparam != null)
            //    Procparam.Clear();
            //else
            //    Procparam = new Dictionary<string, string>();

            //Procparam.Add("@OPTION", "6");
            //Procparam.Add("@COMPANYID", Convert.ToString(intCompanyID));
            //Procparam.Add("@USERID", Convert.ToString(intUserID));
            //Procparam.Add("@Mode", "0");
            //ddlLOB.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            //Procparam.Clear();
            //Procparam.Add("@OPTION", "4");
            //Procparam.Add("@USERID", Convert.ToString(intUserID));
            //ddlBranch.BindDataTable("S3G_CLN_LOADLOV", Procparam, new string[] { "BRANCH_ID", "BRANCH_CODE", "BRANCH_NAME" });
        

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@FilterOption", "'HP','FL','LN','TL','TE'");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "150");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

            Procparam.Clear();
            Procparam.Add("@OPTION", "3");
            //  Procparam.Add("@USERID", Convert.ToString(intUserID));
            ddlInsDoneBy.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            Procparam.Clear();
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunPrimeAccount()
    {
        if (ddlRemainder.SelectedItem.Text == "Specific" && ddlInsDoneBy.SelectedIndex>0 )
        {
            grvInsDetails.DataSource = null;
            grvInsDetails.DataBind();
            pnlInsDetails.Visible = false;
            
            Procparam = new Dictionary<string, string>();
            lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
            lblSANum.Visible = lblPANum.Visible = ddlPANum.Visible = ddlSANum.Visible = true;

            try
            {
                Procparam.Add("@OPTION", "13");
                Procparam.Add("@COMPANYID", Convert.ToString(intCompanyID));
                if (ddlLOB.SelectedIndex > 0)
                {
                    Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                }
                if (ddlBranch.SelectedValue != "")
                {
                    Procparam.Add("@LOCATION_ID", Convert.ToString(ddlBranch.SelectedValue));
                }
                ddlPANum.BindDataTable("S3G_INS_INSLOV", Procparam, new string[] { "PANUM", "PANUM" });
            }
            catch (Exception ex)
            {
                  ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw ex;
            }
        }
        if (ddlRemainder.SelectedItem.Text == "ALL" && ddlInsDoneBy.SelectedIndex > 0)
        {
            lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = true;
            ddlPANum.Visible = ddlSANum.Visible = false;
            lblPANum.Visible = lblSANum.Visible = false;
        }
      





    }

    [System.Web.Services.WebMethod]
    public static string[] GetBranchList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@Type", "GEN");
        Procparam.Add("@User_ID", obj_Page.intUserID.ToString());
        Procparam.Add("@Program_Id", "150");
        Procparam.Add("@Lob_Id", obj_Page.ddlLOB.SelectedValue);
        Procparam.Add("@Is_Active", "1");
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData(SPNames.S3G_SA_GET_BRANCHLIST, Procparam));

        return suggetions.ToArray();
    }

    private void FunSubAccount()
    {
        try
        {

            if (ddlPANum.SelectedIndex > 0)
            {

                Procparam = new Dictionary<string, string>();
                Procparam.Add("@OPTION", "7");
                Procparam.Add("@COMPANYID", Convert.ToString(intCompanyID));
                Procparam.Add("@PANUM", Convert.ToString(ddlPANum.SelectedItem.Text));
                ddlSANum.BindDataTable("S3G_INS_INSLOV", Procparam, new string[] { "SANUM", "SANUM" });
            }
            if (ddlSANum.Items.Count > 1)
            {
                ddlSANum.Enabled = true;
            }
            else
            {
                ddlSANum.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunGetInsuranceDetails()
    {


        if (((ddlBranch.SelectedValue != "" && ddlLOB.SelectedIndex > 0) && (ddlRemainder.SelectedIndex > 0 && ddlInsDoneBy.SelectedIndex > 0)))
        {
            DateTime dtInsDueFromDate;
            DateTime dtInsDueToDate;
            btnGenerate.Visible = false;
            btnSave.Visible = btnClear.Visible = pnlInsDetails.Visible = false;
            grvInsDetails.DataSource = null;
            grvInsDetails.DataBind();

            try
            {

                Procparam = new Dictionary<string, string>();
                DataTable dtInsDetails = new DataTable();


                //if (ddlOutput.SelectedIndex <= 0)
                //{
                //    Utility.FunShowAlertMsg(this, "Select A Output");
                //    return;
                //}

                if (string.IsNullOrEmpty(txtPath.Text))
                {
                    Utility.FunShowAlertMsg(this, "Document path is not defined");
                    return;
                }






                if (ddlRemainder.SelectedItem.Text == "ALL")
                {
                    if (!string.IsNullOrEmpty(txtInsDueFromDate.Text))
                    {

                        // Procparam.Add("@StartDate", Utility.StringToDate(txtInsDueFromDate.Text).ToString());

                        //if (strDateFormat == "dd/MM/yyyy")
                        //{
                        //    dtInsDueFromDate = DateTime.Parse(txtInsDueFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
                        //    Procparam.Add("@StartDate", Convert.ToString(dtInsDueFromDate));


                        //}
                        //if (strDateFormat == "dd-MM-yyyy")
                        //{
                        //    dtInsDueFromDate = DateTime.Parse(txtInsDueFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
                        //    Procparam.Add("@StartDate", Convert.ToString(dtInsDueFromDate));
                        //}
                        //if(strDateFormat!="dd/MM/yyyy" && strDateFormat!="dd-MM-yyyy")
                        //{
                        //   

                        //}


                        Procparam.Add("@StartDate", Convert.ToString(Utility.StringToDate(txtInsDueFromDate.Text)));

                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Enter a Insurance Due From Date");
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtInsDueToDate.Text))
                    {
                        //if (strDateFormat == "dd/MM/yyyy")
                        //{
                        //    dtInsDueToDate = DateTime.Parse(txtInsDueToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
                        //    Procparam.Add("@EndDate", Convert.ToString(dtInsDueToDate));


                        //}
                        //if (strDateFormat == "dd-MM-yyyy")
                        //{
                        //    dtInsDueToDate = DateTime.Parse(txtInsDueToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
                        //    Procparam.Add("@EndDate", Convert.ToString(dtInsDueToDate));
                        //}
                        //if (strDateFormat != "dd/MM/yyyy" && strDateFormat != "dd-MM-yyyy")
                        //{
                        //   

                        //}
                        //Utility.StringToDate
                        Procparam.Add("@EndDate", Convert.ToString(Utility.StringToDate(txtInsDueToDate.Text)));
                        //Procparam.Add("@EndDate", Utility.StringToDate(txtInsDueToDate.Text).ToString());


                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Enter a Insurance Due To Date");
                        return;
                    }
                    if ((!(string.IsNullOrEmpty(txtInsDueFromDate.Text))) && (!(string.IsNullOrEmpty(txtInsDueToDate.Text))))                                   // If start and end date is not empty
                    {
                        //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
                        if (Utility.StringToDate(txtInsDueFromDate.Text) >= Utility.StringToDate(txtInsDueToDate.Text)) // start date should be less than or equal to the enddate
                        {
                            Utility.FunShowAlertMsg(this, "Insurance Due From Date should be lesser than Insurance Due To Date");
                            txtInsDueToDate.Text = "";
                            return;

                        }

                    }
                    //  FunPriValidateFromEndDate();


                    // dtInsDetails = Utility.GetDefaultData("S3G_INS_GETInsuranceDetails", Procparam);
                }

                Procparam.Add("@COMPANY_ID", intCompanyID.ToString());
                Procparam.Add("@LOB_ID", Convert.ToString(ddlLOB.SelectedValue));
                Procparam.Add("@LOCATION_ID", Convert.ToString(ddlBranch.SelectedValue));
                Procparam.Add("@INS_DONE_BY", Convert.ToString(ddlInsDoneBy.SelectedItem.Text));
                Procparam.Add("@REMAINDERTYPE", Convert.ToString(ddlRemainder.SelectedItem.Text));
                if (ddlRemainder.SelectedItem.Text == "Specific")
                {
                    if (ddlPANum.SelectedIndex > 0)
                    {
                        Procparam.Add("@PANUM", Convert.ToString(ddlPANum.SelectedItem.Text));
                    }
                    else
                    {
                        Utility.FunShowAlertMsg(this, "Select A Prime Account");
                        return;
                    }
                    if (ddlSANum.SelectedIndex > 0)
                    {
                        Procparam.Add("@SANUM", Convert.ToString(ddlSANum.SelectedItem.Text));
                    }
                    else if (ddlSANum.Items.Count > 1)
                    {
                        Utility.FunShowAlertMsg(this, "Select A Sub Account");
                        return;
                    }


                    //  dtInsDetails = Utility.GetDefaultData("S3G_INS_GETclaim", Procparam);
                }



                dtInsDetails = Utility.GetDefaultData("S3G_INS_GETInsuranceDetails", Procparam);

                if (dtInsDetails.Rows.Count > 0)
                {
                    btnGenerate.Visible = true;
                    pnlInsDetails.Visible = true;
                    grvInsDetails.DataSource = dtInsDetails;
                    grvInsDetails.DataBind();
                    Session["dtInsDetails"] = dtInsDetails;
                }
                else
                {
                    pnlInsDetails.Visible = true;
                    DataRow emptyrow = dtInsDetails.NewRow();
                    dtInsDetails.Rows.Add(emptyrow);
                    grvInsDetails.DataSource = dtInsDetails;
                    grvInsDetails.DataBind();
                    int columnCount = grvInsDetails.Rows[0].Cells.Count;
                    grvInsDetails.Rows[0].Cells.Clear();
                    grvInsDetails.Rows[0].Cells.Add(new TableCell());
                    grvInsDetails.Rows[0].Cells[0].ColumnSpan = columnCount;
                    grvInsDetails.HeaderRow.Visible = false;
                    grvInsDetails.Rows[0].Cells[0].Text = "Insurance Renewal Details Not Found.";

                }
                Procparam.Clear();
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                throw ex;
            }

        }
    }
    

    private void FunPriValidateFromEndDate()
    {

        //if ((!(string.IsNullOrEmpty(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()))) &&
        if ((!(string.IsNullOrEmpty(txtInsDueFromDate.Text))) &&
           (!(string.IsNullOrEmpty(txtInsDueToDate.Text))))                                   // If start and end date is not empty
        {
            //if (Convert.ToDateTime(DateTime.Parse(txtStartDateSearch.Text, dtformat).ToString()) > Convert.ToDateTime(DateTime.Parse(txtEndDateSearch.Text, dtformat))) // start date should be less than or equal to the enddate
            if (Utility.StringToDate(txtInsDueFromDate.Text) > Utility.StringToDate(txtInsDueToDate.Text)) // start date should be less than or equal to the enddate
            {
                Utility.FunShowAlertMsg(this, "Start Date should be lesser than or equal to the End Date");
                txtInsDueToDate.Text = "";
                return;

            }

        }
        if ((!(string.IsNullOrEmpty(txtInsDueFromDate.Text))) &&
           ((string.IsNullOrEmpty(txtInsDueToDate.Text))))
        {
            txtInsDueToDate.Text = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);

        }
        if (((string.IsNullOrEmpty(txtInsDueFromDate.Text))) &&
            (!(string.IsNullOrEmpty(txtInsDueToDate.Text))))
        {
            txtInsDueFromDate.Text = txtInsDueToDate.Text;

        }

    }

    private void FunPDFGenerate()
    {
        int rowCount = 0;

        try
        {
            foreach (GridViewRow grvIns in grvInsDetails.Rows)
            {
                rowCount = rowCount + 1;
                CheckBox chkInsdetails = (CheckBox)grvIns.FindControl("chkExclude");

                if (!chkInsdetails.Checked)
                {
                    dtInsDetails = (DataTable)Session["dtInsDetails"];

                    if (dtInsDetails.Rows.Count > 0)
                    {
                        ReportDocument rptd = new ReportDocument();

                        rptd.Load(Server.MapPath(@"~\Insurance\Report\RenewalRemainder.rpt"));


                        string path = @txtPath.Text;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        Label lblCustomer = (Label)grvInsDetails.Rows[rowCount - 1].FindControl("lblCustName");
                        Label lblPANum = (Label)grvInsDetails.Rows[rowCount - 1].FindControl("lblPrimeAccount");
                        Label lblSANum = (Label)grvInsDetails.Rows[rowCount - 1].FindControl("lblSubAccount");
                        string strCustomer = lblCustomer.Text;
                        string strPANum = lblPANum.Text;
                        strPANum = strPANum.Replace("/", "");
                        string strSANum = lblSANum.Text;
                        //if (strSANum != "")
                        //{
                        //    strSANum = strSANum.Replace("/", "");
                        //}
                        strSANum = (string.IsNullOrEmpty(strSANum)) ? "" : strSANum.Replace("/", "");
                        
                        /*Revision History 
                         * Done by: Srivatsan.S
                         * Done on: 03/11/2011
                         * Purpose: To resolve the file creation issue.
                         */
                        //string filePath = path + "\\InsuranceRenewalReminder" + "_" + strCustomer + "_" + strPANum + "_" + strSANum + ".pdf";

                        string filePath = path + "\\InsuranceRenewalReminder" + strPANum + "_" + strSANum ;
                        filePath = filePath + ".pdf";
                        ViewState["Path"] = filePath;
                        DataTable dtCopyInsDetails = new DataTable();
                        dtCopyInsDetails = dtInsDetails.Select("SINo=" + rowCount.ToString()).CopyToDataTable();
                        //+ " " + DateTime.Parse(DateTime.Today.ToString(), CultureInfo.CurrentCulture).ToString(ObjS3GSession.ProDateFormatRW) 
                        //var r = from row in dtInsDetails.AsEnumerable()
                        //        where row.Field<string>("PANum").Trim() == ((Label)grvIns.FindControl("lblPrimeAccount")).Text && row.Field<string>("PolicyNo") == ((Label)grvIns.FindControl("lblPolicyNo")).Text
                        //        select row;
                        //  dtInsDetails = r.CopyToDataTable();

                        //if (dtInsDetails.Rows[0]["Status"].ToString() != "Duplicate")
                        //{
                        rptd.SetDataSource(dtCopyInsDetails);
                        rptd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
                        Utility.FunShowAlertMsg(this, "PDF Generated");
                        dtCopyInsDetails.Clear();
                        btnClear.Visible = true;
                        btnSave.Visible = true;
                        btnGenerate.Visible = false;
                        // dtCopyInsDetails = "";
                        //}
                        //else
                        //{


                        //rptd.SetDataSource(dtInsDetails);
                        //rptd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
                        //Utility.FunShowAlertMsg(this, "Dulplicate PDF Have been Generated");

                        //}

                    }

                    //   var r = from row in dtInsDetails.AsEnumerable()
                    //             where row.Field<string>("PANum").Trim() == ((Label)grvIns.FindControl("lblPrimeAccount")).Text
                    // //var r = from c in dtInsDetails.AsEnumerable()
                    // //        orderby c.Field<String>(((Label)grvIns.FindControl("lblPrimeAccount")).Text), c.Field<String>(((Label)grvIns.FindControl("lblSubAccount")).Text) ascending
                    // //        select c;
                    //// dtInsDetails = r.CopyToDataTable();
                    //// items.Add(item);
                    //((Label)grvIns.FindControl("lblPrimeAccount")).Text;
                    //rptd.Load(Server.MapPath("PDCReminderFormat.rpt"));
                    //rptd.SetDataSource(dtInsDetails);
                    ////rptd.Subreports["HeaderDetailsSubReport.rpt"].SetDataSource(headerDetails);
                    //CRVPDCReminder.ReportSource = rptd;
                    //CRVPDCReminder.DataBind();

                    //if (dtInsDetails.Rows.Count > 0)
                    //{
                    //}

                    //if (dtAppropriationCalList.Rows.Count > 0)
                    //{
                    //    var r = from c in dtInsDetails.AsEnumerable()
                    //            orderby c.Field<String>("PrimeAccountNo"), c.Field<String>("SubAccountNo") ascending
                    //            select c;
                    //    dtInsDetails = r.CopyToDataTable();
                    //        select new
                    //                            {
                    //                               PANum=row.Field<string>("PANum"),
                    //                               SANum=row.Field<string>("SANum"),
                    //                               CustomerName=row.Field<string>("CustomerName"),
                    //                               AssetDescription = row.Field<string>("AssetDescription"),
                    //                               AssetRegNo = row.Field<string>("AssetRegNo"),
                    //                               InsuredBy = row.Field<string>("InsuredBy"),
                    //                               PolicyNo = row.Field<string>("PolicyNo"),
                    //                               PolicyExpiryDate = row.Field<string>("PolicyExpiryDate"),
                    //                               InsuredAmount = row.Field<string>("InsuredAmount")
                    //                            }; 

                    //}
                    //strInsDetailsBuilder.Append(lblLOB.Text + "='" + ddlLOB.SelectedItem.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append(lblBranch.Text + "='" + ddlBranch.SelectedItem.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append(lblRemainder.Text + "='" + ddlRemainder.SelectedItem.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append(lblInsDoneBy.Text + "='" + ddlInsDoneBy.SelectedItem.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append(lblInsDueFromDate.Text + "='" + txtInsDueFromDate.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append(lblInsDueToDate.Text + "='" + txtInsDueToDate.Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Prime Account" + "='" + ((Label)grvIns.FindControl("lblPrimeAccount")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Sub Account" + "='" + ((Label)grvIns.FindControl("lblSubAccount")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Customer Name" + "='" + ((Label)grvIns.FindControl("lblCustName")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Asset Description" + "='" + ((Label)grvIns.FindControl("lblAssetDesc")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Asset Reg No" + "='" + ((Label)grvIns.FindControl("lblAssetRegNo")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Insured By" + "='" + ((Label)grvIns.FindControl("lblInsuredBy")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Policy No" + "='" + ((Label)grvIns.FindControl("lblPolicyNo")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Policy Expiry Date" + "='" + ((Label)grvIns.FindControl("lblPolicyExpiryDate")).Text + "'" + System.Environment.NewLine);
                    //strInsDetailsBuilder.Append("Insured Amount" + "='" + ((Label)grvIns.FindControl("lblPolicyInsuredAmount")).Text + "'" + System.Environment.NewLine);





                }
                else
                {
                    if (rowCount == grvInsDetails.Rows.Count)
                    {
                        Utility.FunShowAlertMsg(this, "Select Atleast one Account");
                        return;
                    }
                }



            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }


    }

    private string FunPASADetails()
    {

        string strIns_Done_By = string.Empty;
        int columsCount = 0;
        strPASABuilder.Append("<ROOT>");
        string StrOutput = ViewState["Path"].ToString();
        string[] StrArrayOutput=StrOutput.Split('\\');

        try
        {
            foreach (GridViewRow grvIns in grvInsDetails.Rows)
            {
                string strSANum = "";
                CheckBox chkInsdetails = (CheckBox)grvIns.FindControl("chkExclude");

                if (!chkInsdetails.Checked)
                {
                    strIns_Done_By = (ddlInsDoneBy.SelectedItem.Text == "Customer") ? "1" : "2";
                    strPASABuilder.Append(" <DETAILS ");
                    strPASABuilder.Append("PRIMEACCOUNTNO='" + ((Label)grvIns.FindControl("lblPrimeAccount")).Text + "'");
                    strPASABuilder.Append(" ");
                    strSANum = ((Label)grvIns.FindControl("lblSubAccount")).Text;
                    strSANum = (string.IsNullOrEmpty(strSANum)) ? ((Label)grvIns.FindControl("lblPrimeAccount")).Text + "DUMMY" : strSANum;
                    strPASABuilder.Append("SUBACCOUNTNO='" + strSANum + "'");
                    strPASABuilder.Append(" ");
                    strPASABuilder.Append("REMAINDER_OUTPUT='" + StrArrayOutput[2].Trim() + "'");
                    strPASABuilder.Append(" ");
                    strPASABuilder.Append("REMAINDER_PATH='" + txtPath.Text.Trim() + "'");
                    strPASABuilder.Append(" ");
                    strPASABuilder.Append("INS_DONE_BY='" + strIns_Done_By + "'");
                    strPASABuilder.Append(" ");
                    strPASABuilder.Append("Policy_No='" + ((Label)grvIns.FindControl("lblPolicyNo")).Text + "'");
                    strPASABuilder.Append(" ");
                    strPASABuilder.Append("/>");

                }

            }
            strPASABuilder.Append("</ROOT>");
            // return strbXml.ToString();
            return strPASABuilder.ToString();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    //private bool FunPDFGenerate(string strInsDetails, string strPath)
    //{
    //    bool boolpdfGenerater = false;


    //    Utility.FunPubGeneratePDF(strpath, strInsDetails, "Insurance Details");
    //    boolpdfGenerater = true;


    //    return boolpdfGenerater;
    //}

    #endregion

    #region [DropDownList Events]
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32( ddlBranch.SelectedValue) >= 0)
        {
            FunClearFieldValues();
            ddlPANum.Visible = ddlSANum.Visible = lblPANum.Visible = lblSANum.Visible = false;
            txtInsDueFromDate.Visible = txtInsDueToDate.Visible = lblInsDueFromDate.Visible = lblInsDueToDate.Visible = false;
            imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
            //ddlInsDoneBy.SelectedIndex = 0;
            ddlRemainder.ClearSelection();

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@OPTION", "3");
            //  Procparam.Add("@USERID", Convert.ToString(intUserID));
            ddlInsDoneBy.BindDataTable("S3G_INS_LOADLOV", Procparam, new string[] { "Lookup_Code", "Lookup_Description" });
            Procparam.Clear();
        }
    }

    
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblBranch.CssClass = "styleReqFieldLabel";
        txtPath.Text = "";
        if (ddlLOB.SelectedIndex >= 0)
        {
            FunClearFieldValues();
            ddlInsDoneBy.SelectedIndex = 0;
            ddlRemainder.SelectedIndex = 0;
            ddlPANum.Visible = ddlSANum.Visible = lblPANum.Visible = lblSANum.Visible = false;
            txtInsDueFromDate.Visible = txtInsDueToDate.Visible = lblInsDueFromDate.Visible = lblInsDueToDate.Visible = false;
            imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
            ddlBranch.Clear();
           
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserID.ToString());
            Procparam.Add("@Program_ID", "150");
            Procparam.Add("@Lob_Id", Convert.ToString(ddlLOB.SelectedValue));
            //ddlBranch.BindDataTable(SPNames.BranchMaster_LIST, Procparam, new string[] { "Location_ID", "Location_Code", "Location_Name" });
           
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            Procparam.Add("@LOB_ID", ddlLOB.SelectedValue);
            Procparam.Add("@Program_Id", "150");
            Procparam.Add("@Is_Active", "1");
            DataTable dtDocPath = Utility.GetDefaultData("S3G_ORG_GetDocPathforLOB", Procparam);

            if (dtDocPath != null && dtDocPath.Rows.Count > 0)
            {
                txtPath.Text = dtDocPath.Rows[0][0].ToString();
                
            }
            else
            {
                txtPath.Text = "";
            }

        }
    }
    protected void ddlPANum_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSave.Visible = btnClear.Visible = btnGenerate.Visible = pnlInsDetails.Visible = false;
        ddlSANum.Enabled = true;
        grvInsDetails.DataSource = null;
        ddlSANum.SelectedIndex = -1;
        grvInsDetails.DataBind();
        if (ddlRemainder.SelectedIndex > 0 && ddlPANum.SelectedIndex > 0)
        {
            FunSubAccount();
        }


    }

    private void FunClearFieldValues()
    {
        btnSave.Visible = btnClear.Visible = btnGenerate.Visible = pnlInsDetails.Visible = false;
        ddlPANum.SelectedIndex = -1;
        ddlSANum.SelectedIndex = -1;
        txtInsDueFromDate.Text = "";
        txtInsDueToDate.Text = "";
        ddlOutput.SelectedIndex = 0;
        
        grvInsDetails.DataSource = null;
        grvInsDetails.DataBind();
        ddlSANum.Enabled = false;
    }

    protected void ddlInsDoneBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunClearFieldValues();
        lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
        ddlPANum.Visible = ddlSANum.Visible = false;
        lblPANum.Visible = lblSANum.Visible = false;
        if (ddlRemainder.SelectedIndex > 0)
        {
            FunPrimeAccount();
        }
        
       
        //if (ddlRemainder.SelectedItem.Text == "Specific")
        //{

        //    lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = false;
        //    imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
        //    lblSANum.Visible = lblPANum.Visible = ddlPANum.Visible = ddlSANum.Visible = true;


        //}
        //if (ddlRemainder.SelectedItem.Text == "ALL")
        //{
        //    lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = true;
        //    imgInsDueFromDate.Visible = imgInsDueToDate.Visible = true;
        //    lblSANum.Visible = lblPANum.Visible = ddlPANum.Visible = ddlSANum.Visible = false;

        //}

        //if (ddlInsDoneBy.SelectedIndex == 0)
        //{
        //    lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
        //    ddlPANum.Visible = ddlSANum.Visible = false;
        //    lblPANum.Visible = lblSANum.Visible = false;
        //}


    }
    protected void ddlRemainder_SelectedIndexChanged(object sender, EventArgs e)
    {

        FunClearFieldValues();
        ddlInsDoneBy.SelectedIndex = 0;

        lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
        ddlPANum.Visible = ddlSANum.Visible = false;
        lblPANum.Visible = lblSANum.Visible = false;
        
        //btnSave.Visible = btnClear.Visible = btnGenerate.Visible = pnlInsDetails.Visible = false;

        //grvInsDetails.DataSource = null;
        //grvInsDetails.DataBind();
        //txtInsDueFromDate.Text = "";
        //txtInsDueToDate.Text = "";
        //ddlSANum.SelectedIndex = -1;
        //ddlSANum.Enabled = false;
        //if (ddlLOB.SelectedIndex > 0 && ddlBranch.SelectedIndex > 0)
        //{
          //  lblInsDueFromDate.Visible = lblInsDueToDate.Visible = txtInsDueFromDate.Visible = txtInsDueToDate.Visible = imgInsDueFromDate.Visible = imgInsDueToDate.Visible = false;
       
        
        //}


        //if (ddlRemainder.SelectedValue == "2")
        //{
        //    lblInsDoneBy.Visible = ddlInsDoneBy.Visible = rfvInsDoneBy.Enabled = false;
        //}
        //else
        //{
        //    lblInsDoneBy.Visible = ddlInsDoneBy.Visible = rfvInsDoneBy.Enabled = true;
        //}

    }
    protected void ddlOutput_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlSANum_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSave.Visible = btnClear.Visible = btnGenerate.Visible = pnlInsDetails.Visible = false;
        grvInsDetails.DataSource = null;
        grvInsDetails.DataBind();
    }
    #endregion

    #region [Button Events]
    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunGetInsuranceDetails();

    }


    public bool GVBoolFormat(string val)
    {

        if (val =="DUPLICATE")
        {

            return true;
            
        }
        else
            return false;
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        FunPDFGenerate();


        //if (ddlOutput.SelectedItem.Text == "PDF" && (!string.IsNullOrEmpty(txtPath.Text)))
        //{
            

        //}
        //else if (ddlOutput.SelectedIndex > 1)
        //{
        //    Utility.FunShowAlertMsg(this, "Select A PDF Only");
        //    return;
        //}

        //else
        //{
        //    Utility.FunShowAlertMsg(this, "Enter A Output and Path");
        //    return;
        //}





        //string strDocPath = txtPath.Text.Trim();
        ////strpath = @strDocPath;
        ////strpath.Replace(strpath, txtPath.Text);
        //strpath = @strDocPath + "\\InsDetails.pdf";
        //if (!File.Exists(strpath))
        //{
        //    File.Create(strpath);
        //}

        //else
        //{
        //    Utility.FunShowAlertMsg(this, "Unable to Generate a PDF Document ");
        //    return;

        //}
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../Insurance/S3GINSInsuranceRemainder.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunSaveRemainderDetails();

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //   Utility.FunShowAlertMsg(this, "Are you Sure to Clear Remainder Values");
        FunClear();
        //    return;

        //strAlert = "Are you Sure to Clear Remainder Values";
        //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPage + "}else {" + strRedirectPage + "}";
        //strRedirectPage = "";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPage, true);
    }
    
    protected void grvInsDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion

    protected void grvInsDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkExclude = e.Row.FindControl("chkExclude") as CheckBox;

            if (chkExclude.Checked)
            {
                chkExclude.Enabled = false;
            }
            else
            {
                btnGenerate.Enabled = true;
            }
        }
    }
}
