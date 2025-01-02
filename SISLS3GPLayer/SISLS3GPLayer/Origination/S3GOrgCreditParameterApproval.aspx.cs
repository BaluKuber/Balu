#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Orgination
/// Screen Name         :   Credit Parameter Approval
/// Created By          :   S.Kannan
/// Created Date        :   22-June-2010
/// Purpose             :   To approve the credit parameters
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using S3GBusEntity;
using S3GBusEntity.Origination;
#endregion

public partial class Origination_S3GOrgCreditParameterApproval : ApplyThemeForProject
{
    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    string strSearchVal6 = string.Empty;
    string strSearchVal7 = string.Empty;
    string strSearchVal8 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bModify = true;
    PagingValues ObjPaging = new PagingValues();

    private static int _NumOfApproved;

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);

    public int ProPageNumRW
    {
        get;
        set;
    }

    public int ProPageSizeRW
    {
        get;
        set;

    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }
    #endregion

    #region initial Values
    Dictionary<string, string> Procparam = null;
    string _strEnquiryLevel = "../Origination/S3GOrgCreditParameterApproval_EN_Add.aspx"; // Approval based on enquiry level

    int _CreditParameter = 0;
    /* _CreditParameter = 1   => Unapproved
                              -- Not approved even once
                              -- Approved Partially
     _CreditParameter = 2     => All
                              -- Not approved even once
                              -- approved partially 
                              -- approved */

    CreditMgtServicesReference.CreditMgtServicesClient ObjMgtCreditMgtClient;  // credit service client
    CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable _ObjCreditParameterApprovalDataTable = new CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable(); // datatable

    CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable _ObjCreditParameterTransactionDataTable = new CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable(); // datatable


    string _strApproved = "Approved";           // values mentioned in dbo.S3G_Status_LookUP where type='CREDIT_PARAMETER_APPROVAL'
    string _strRejected = "Rejected";           // values mentioned in dbo.S3G_Status_LookUP where type='CREDIT_PARAMETER_APPROVAL'
    string _strUnderProcess = "Under Process";  // values mentioned in dbo.S3G_Status_LookUP where type='CREDIT_PARAMETER_APPROVAL'
    string _strNotStarted = "Not Started";
    static int _IsEnquiryMode;
    
    string _strEnquiryDate = "Doc_Date";     // sql table S3G_ORG_EnquiryUpdation's columnname
    string strDateFormat;
    static string _TransLanderMode = string.Empty;
    string _TransLander_CPT_ID = string.Empty;
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        FunPubSetIndex(1); // to select the orgination accordion
        S3GSession ObjS3GSession = new S3GSession();
        strDateFormat = ObjS3GSession.ProDateFormatRW;



        #region Paging Config

        ProPageNumRW = 1;
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        #endregion

        UserInfo ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;

        //if (!IsPostBack)
        //{
        //    btnShowAll.Visible =
        //    ucCustomPaging.Visible = false;
        //}
        //pnlSearchOption.Visible = false;
        if (string.Compare("C", Request.QueryString.Get("qsMode")) == 0) // if Modify Mode
        {
            _TransLanderMode = "C";
            // pnlSearchOption.Visible = true;
        }

        // this is to get the query string from the translander
        if (Request.QueryString.Get("qsViewId") != null)   // here I will get the Credit parameter transaction ID
        {
            FormsAuthenticationTicket TicketViewID = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));
            if (!(string.IsNullOrEmpty(TicketViewID.Name)))
            {
                _TransLander_CPT_ID = TicketViewID.Name;    // this is to save the credit parameter transaction ID

                if (Request.QueryString.Get("qsMode") != null)
                {
                    if (string.Compare("Q", Request.QueryString.Get("qsMode")) == 0)   // if Query Mode
                    {
                        _TransLanderMode = "Q";
                        if (!(string.IsNullOrEmpty(_TransLander_CPT_ID)))
                        {
                            FunPriPassToAddPage();
                        }

                        //This is to hide first row if grid is empty
                        //if (bIsNewRow)
                        //{
                        //    gvCreditParameter.Rows[0].Visible = false;
                        //}

                        //FunPriSetSearchValue();

                        //ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                        //ucCustomPaging.setPageSize(ProPageSizeRW);
                    }
                    else if (string.Compare("M", Request.QueryString.Get("qsMode")) == 0) // if Modify Mode
                    {
                        _TransLanderMode = "M";
                        if (!(string.IsNullOrEmpty(_TransLander_CPT_ID)))
                        {
                            FunPriPassToAddPage();
                        }

                    }
                    //else if (string.Compare("C", Request.QueryString.Get("qsMode")) == 0) // if Modify Mode
                    //{
                    //    _TransLanderMode = "C";
                    //    pnlSearchOption.Visible  = true;
                    //}
                }
            }
        }



        if (!IsPostBack)
        {
            FunPriLoadDocumentType();
            //FunPriBindGrid();
        }
        //if (string.Compare("C", _TransLanderMode) == 0) // if Modify Mode
        //{
       // RBEnquiryNumber.Visible = RBCustomerCode.Visible =
            RBAll.Visible = RBUnapproved.Visible = true;

        // }           
    }
    #endregion

    /// <summary>
    /// Link button event - to hide/show the credit parameter approval box
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 

    /// <summary>
    /// This event will bind the credit parameter approval datasource to the gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnFindRecords_Click(object sender, EventArgs e)
    {
        gvCreditParameter.Visible = true;
        ucCustomPaging.Visible = true;
        btnShowAll.Visible = true;
        FunPriClearSearchValue();
        hdnSearch.Value = string.Empty;
        hdnOrderBy.Value = string.Empty;

        //if (RBCustomerCode.Checked)
        //    _IsEnquiryMode = false;
        //else
        //    _IsEnquiryMode = true;
        if (ddlDocumentType.SelectedIndex > 0)
        {
            if (ddlDocumentType.SelectedValue == "1") //Enquiry
            {
                _IsEnquiryMode = 1;
            }
            else if (ddlDocumentType.SelectedValue == "2") //Prcing
            {
                _IsEnquiryMode = 2;
            }
            else if (ddlDocumentType.SelectedValue == "3") //Application
            {
                _IsEnquiryMode = 3;
            }
            else if (ddlDocumentType.SelectedValue == "6") //Customer
            {
                _IsEnquiryMode = 6;
            }
        }

        FunPriBindGrid();
    }
    private void FunPriLoadDocumentType()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@OptionValue", "4");
            DataTable dt = Utility.GetDefaultData("S3G_ORG_GetProformaLookup", Procparam);
            DataRow[] dr = dt.Select("Value NOT IN(4,5)", "Name", DataViewRowState.CurrentRows);
            dt = dr.CopyToDataTable();
            ddlDocumentType.BindDataTable(dt, new string[] { "Value", "Name" });
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    #region Bind Grid dataSource
    /// <summary>
    /// This method will bind the Credit parameter approval datasource to the gridview.
    /// </summary>
    private void FunPriBindGrid()
    {
        int intTotalRecords = 0;
        DataTable dtCPGridSource = FunGetGridData(out intTotalRecords);
        if (dtCPGridSource == null) return;
        // Added for TransLander
        if (!(string.IsNullOrEmpty(_TransLander_CPT_ID)))
        {
            dtCPGridSource.DefaultView.RowFilter = "CreditParamTransID =  " + _TransLander_CPT_ID; // to filter depends on the TransLander Selection.
            dtCPGridSource = dtCPGridSource.DefaultView.ToTable();
            intTotalRecords = dtCPGridSource.Rows.Count;
            dtCPGridSource = FunPriAddColumns(dtCPGridSource);
        }
        FunPriGetSearchValue();

        //This is to show grid header
        bool bIsNewRow = false;
        if (dtCPGridSource.Rows.Count == 0)
        {
            dtCPGridSource.DefaultView.AddNew();
            dtCPGridSource = dtCPGridSource.DefaultView.ToTable();
            dtCPGridSource.Rows[0]["Approve"] = false;
            bIsNewRow = true;
        }
        else
        {//saranya
            //btnShowAll.Visible =
            //ucCustomPaging.Visible = true;  // if records exists
        }

        gvCreditParameter.DataSource = dtCPGridSource;
        gvCreditParameter.DataBind();

        if (ddlDocumentType.SelectedValue=="6")
        {
            //gvCreditParameter.Columns[1].Visible = false;
            gvCreditParameter.Columns[2].Visible = false;
            gvCreditParameter.Columns[3].Visible = false;
            gvCreditParameter.Columns[4].Visible = false;
            gvCreditParameter.Columns[5].Visible = false;
            gvCreditParameter.Columns[6].Visible = false;
            gvCreditParameter.Columns[7].Visible = true;

            //gvCreditParameter.Columns[8].Visible = true;
        }
        else
        {
            gvCreditParameter.Columns[1].Visible = true;
            gvCreditParameter.Columns[2].Visible = true;
            gvCreditParameter.Columns[3].Visible = true;
            gvCreditParameter.Columns[4].Visible = false;
            gvCreditParameter.Columns[5].Visible = true;
            gvCreditParameter.Columns[6].Visible = true;
            gvCreditParameter.Columns[7].Visible = true;
            //gvCreditParameter.Columns[8].Visible = false;
        }
        if (!(string.IsNullOrEmpty(_TransLander_CPT_ID)))
        {
            FunPriPassToAddPage();
        }

        //This is to hide first row if grid is empty
        if (bIsNewRow)
        {
            gvCreditParameter.Rows[0].Visible = false;
        }

        FunPriSetSearchValue();

        ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
        ucCustomPaging.setPageSize(ProPageSizeRW);
    }
    /// <summary>
    /// This event will check the approval status - of the credit parameter
    /// </summary>
    /// <param name="sender"></param>   
    /// <param name="e"></param>
    protected void gvCreditParameter_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEnquiryDate = ((Label)e.Row.FindControl("lblEnquiryDate"));
            CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");
            string strApprovalStatus = ((Label)e.Row.FindControl("lblApprovedStatus")).Text;

            if ((lblEnquiryDate != null) && (!(string.IsNullOrEmpty(lblEnquiryDate.Text))))// if date exists
            {
                lblEnquiryDate.Text = DateTime.Parse(lblEnquiryDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            }

            if ((string.Compare(strApprovalStatus, _strApproved)) == 0)
            {
                chkApprove.Checked = true;
                chkApprove.Enabled = false;
            }
            if ((string.Compare(strApprovalStatus, _strRejected)) == 0)
            {
                chkApprove.Enabled = false;
            }

            if ((string.Compare("Q", _TransLanderMode) == 0))        // added for translander
            {
                chkApprove.Enabled = false;
            }
        }
    }
    protected void chkApprove_Changed(object sender, EventArgs e)
    {
        CheckBox chkCurSelect = (CheckBox)sender;
        GridViewRow gvRow = null;
        if (chkCurSelect != null)
            gvRow = (GridViewRow)chkCurSelect.Parent.Parent;
        using (UserInfo sessionInfo = new UserInfo())
        {
            if (sessionInfo.ProUserLevelIdRW < 3)
            {
                if (((CheckBox)gvRow.FindControl("chkApprove")).Checked == true)
                    Utility.FunShowAlertMsg(this, "Only level 3 and above users can approve the record");
                return;
            }
        }


        //_IsEnquiryMode = ((RBEnquiryNumber.Checked) ? RBEnquiryNumber.Checked : false);
        if (ddlDocumentType.SelectedIndex > 0)
        {
            if (ddlDocumentType.SelectedValue == "1") //Enquiry
            {
                _IsEnquiryMode = 1;
            }
            else if (ddlDocumentType.SelectedValue == "2") //Prcing
            {
                _IsEnquiryMode = 2;
            }
            else if (ddlDocumentType.SelectedValue == "3") //Application
            {
                _IsEnquiryMode = 3;
            }
            else if (ddlDocumentType.SelectedValue == "6") //Customer
            {
                _IsEnquiryMode = 6;
            }
        }
        string strChkApprove = ((CheckBox)sender).ClientID;
        CheckBox chkApprove = null;
        foreach (GridViewRow grvCPARow in gvCreditParameter.Rows)
        {
            chkApprove = ((CheckBox)grvCPARow.FindControl("chkApprove"));
            if ((string.Compare(strChkApprove, chkApprove.ClientID)) == 0)
            {
                string strCustomerId = ((Label)grvCPARow.FindControl("lblCustomerId")).Text;
                string strEnquiryNo = ((Label)grvCPARow.FindControl("lblEnquiryNoHdn")).Text;//grvCPARow.Cells[_GridCustCodeIndex].ToString();

                string strCompanyId = ((Label)grvCPARow.FindControl("lblCompanyId")).Text;
                string strLOBId = ((Label)grvCPARow.FindControl("lblLOBId")).Text;
                string strProductId = ((Label)grvCPARow.FindControl("lblProductId")).Text;
                string strConstitutionId = ((Label)grvCPARow.FindControl("lblConstitutionID")).Text;
                string strLocaId = ((Label)grvCPARow.FindControl("lblLocationId")).Text;
                string strCreditParamTransID = ((Label)grvCPARow.FindControl("lblCreditParamTransID")).Text;
                _NumOfApproved = Convert.ToInt32(((Label)grvCPARow.FindControl("lblNumOfApproved")).Text);
                string strMode = ""; // Enquiry or customer

                if (_IsEnquiryMode==1)
                    strMode = "Enq";
                else if (_IsEnquiryMode == 2)
                    strMode = "Pri";
                else if (_IsEnquiryMode == 3)
                    strMode = "App";
                else if (_IsEnquiryMode == 6)
                    strMode = "Cus";

                FormsAuthenticationTicket TicketCustomerId = new FormsAuthenticationTicket(strCustomerId, false, 0);
                FormsAuthenticationTicket TicketEnquiryNo = new FormsAuthenticationTicket(strEnquiryNo, false, 0);

                FormsAuthenticationTicket TicketCreditParamTransID = new FormsAuthenticationTicket(strCreditParamTransID, false, 0);
                FormsAuthenticationTicket TicketCompanyId = new FormsAuthenticationTicket(strCompanyId, false, 0);
                FormsAuthenticationTicket TicketLOBId = new FormsAuthenticationTicket(strLOBId, false, 0);
                FormsAuthenticationTicket TicketProductId = new FormsAuthenticationTicket(strProductId, false, 0);
                FormsAuthenticationTicket TicketConstitutionId = new FormsAuthenticationTicket(strConstitutionId, false, 0);
                FormsAuthenticationTicket TicketLocationId = new FormsAuthenticationTicket(strLocaId, false, 0);

                FormsAuthenticationTicket TicketNumOfApproved = new FormsAuthenticationTicket(_NumOfApproved.ToString(), false, 0);

                FormsAuthenticationTicket TicketMode = new FormsAuthenticationTicket(strMode, false, 0);


                string strredirect = _strEnquiryLevel + "?qsEnqNo=" + FormsAuthentication.Encrypt(TicketEnquiryNo) + "&CustId=" + FormsAuthentication.Encrypt(TicketCustomerId)
                    + "&CompanyId=" + FormsAuthentication.Encrypt(TicketCompanyId)
                    + "&LOBId=" + FormsAuthentication.Encrypt(TicketLOBId)
                //    + "&ConstitutionId=" + FormsAuthentication.Encrypt(TicketConstitutionId)
                + "&ProductId=" + FormsAuthentication.Encrypt(TicketProductId)
                + "&LocationID=" + FormsAuthentication.Encrypt(TicketLocationId)
                + "&CreditParamTransId=" + FormsAuthentication.Encrypt(TicketCreditParamTransID)
                + "&Mode=" + FormsAuthentication.Encrypt(TicketMode)
                + "&NumOfApproved=" + FormsAuthentication.Encrypt(TicketNumOfApproved)
                + "&qsMode=" + _TransLanderMode;


                Response.Redirect(strredirect,false); // EnquiryNo

            }
        }
    }
    #region Get Credit parameter datasource from DB
    /// <summary>
    /// This method will reterive the credit parameter approval data from the database,  
    /// It will return the rows depend on the credit prameter settings of the end-user.
    /// </summary>
    /// <returns>CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable</returns>
    private DataTable FunGetGridData(out int intTotalRecords)
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            intTotalRecords = 0;

            CreditMgtServices.S3G_ORG_CreditParamTransactionRow ObjCreditParameterTransactionRow;
            CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable ObjCreditParameterTransactionDataTable = new CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable();

            ObjCreditParameterTransactionRow = ObjCreditParameterTransactionDataTable.NewS3G_ORG_CreditParamTransactionRow();

            ObjCreditParameterTransactionRow.CreditParamStatus_ID = 95;
            ObjCreditParameterTransactionRow.Document_Type =Convert.ToInt32(ddlDocumentType.SelectedValue);

            string filtercondition = "";
            
            if ((string.Compare("Q", _TransLanderMode) != 0)
                &&
            (string.Compare("M", _TransLanderMode) != 0))
            {
                if (RBUnapproved.Checked) // to diaply only the unapproved records
                {
                    filtercondition += " ApprovalStatus equal to Under Process ";
                }

                if (RBAll.Checked)  // to dispplay all the records
                {
                    filtercondition += " ApprovalStatus not Rejected ";
                }
               
                //if (RBEnquiryNumber.Checked)    // by enquiry Number
                //{
                //    filtercondition += " EnquiryNo ISNOT NULL ";
                //}

                //if (RBCustomerCode.Checked) // by customer code
                //{
                //    filtercondition += " EnquiryNo ISEqualTo NULL ";
                //}
                
                 

            }


            ObjCreditParameterTransactionRow.FilterType = filtercondition;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;
            ObjCreditParameterTransactionDataTable.AddS3G_ORG_CreditParamTransactionRow(ObjCreditParameterTransactionRow);
            SerializationMode SerMode = new SerializationMode();
            byte[] bytesCreditParameterTransaction = ObjMgtCreditMgtClient.FunPubQueryCreditParameterTransaction(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjCreditParameterTransactionDataTable, SerMode), ObjPaging);
            _ObjCreditParameterTransactionDataTable = (CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable));
            DataTable dt = _ObjCreditParameterTransactionDataTable;
            // dt = FunPriFilterColumns(dt); // this line was commented because of TransLAnder Functionality
            dt = FunPriAddColumns(dt); // this line was commented because of TransLAnder Functionality
            return dt;
        }

        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            lblErrorMessage.InnerText = ae.Message;
            throw ae;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }

    private DataTable FunGetCreditParamData()
    {
        ObjMgtCreditMgtClient = new CreditMgtServicesReference.CreditMgtServicesClient();
        try
        {
            SerializationMode SerMode = new SerializationMode();

            byte[] bytesCreditParameterTransaction = ObjMgtCreditMgtClient.FunPubQueryCreditParameterTransactionID(SerMode, int.Parse(_TransLander_CPT_ID));
            _ObjCreditParameterTransactionDataTable = (CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, SerializationMode.Binary, typeof(CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable));
            DataTable dt = _ObjCreditParameterTransactionDataTable;
            dt = FunPriAddColumns(dt); // this line was commented because of TransLAnder Functionality
            return dt;
        }
        catch (Exception e)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        finally
        {
            ObjMgtCreditMgtClient.Close();  // closing the WCF connection
        }
    }
    /// <summary>
    /// This is to add the columns (SerialNumber,Approval) to the Credit Parameter Datatable
    /// </summary>
    /// <param name="_ObjCreditParameterApprovalDataTable">S3G_ORG_CreditParameterApprovalDataTable</param>
    /// <returns>S3G_ORG_CreditParameterApprovalDataTable</returns>
    private DataTable FunPriAddColumns(DataTable dt)
    {


        DataColumn dcSerialNumber = new DataColumn("SerialNumber");
        DataColumn dcApprove = new DataColumn("Approve", System.Type.GetType("System.Boolean"));

        if (!(dt.Columns.Contains("SerialNumber")))
        {
            dt.Columns.Add(dcSerialNumber);
            dt.Columns.Add(dcApprove);

            for (int i_addSerial = 1; i_addSerial <= dt.Rows.Count; i_addSerial++)
            {
                dt.Rows[i_addSerial - 1]["SerialNumber"] = i_addSerial.ToString();
                dt.Rows[i_addSerial - 1]["Approve"] = false;
            }

        }
        return dt;
    }
    /// <summary>
    /// It will filter the records by credit parameters - set by the end-user
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private DataTable FunPriFilterColumns(DataTable dt)
    {

        if (RBUnapproved.Checked) // to diaply only the unapproved records
        {
            //dt.DefaultView.RowFilter = "ApprovalStatus = '" + _strNotStarted + "' or ApprovalStatus = '" + _strUnderProcess + "'";
            //dt.DefaultView.RowFilter = "ApprovalStatus <> '" + _strUnderProcess + "' and ApprovalStatus <> '" + _strRejected + "'";
            dt.DefaultView.RowFilter = "ApprovalStatus = '" + _strUnderProcess + "'";
            dt = dt.DefaultView.ToTable();

        }

        if (RBAll.Checked)  // to dispplay all the records
        {
            dt.DefaultView.RowFilter = "ApprovalStatus <> '" + _strRejected + "'";
            dt = dt.DefaultView.ToTable();
        }
/*
        if (RBEnquiryNumber.Checked)    // by enquiry Number
        {
            dt.DefaultView.RowFilter = "EnquiryNo IS NOT NULL";
            dt = dt.DefaultView.ToTable();
        }

        if (RBCustomerCode.Checked) // by customer code
        {
            dt.DefaultView.RowFilter = "EnquiryNo IS NULL";
            dt = dt.DefaultView.ToTable();
        }
        */
        return dt;
    }
    /// <summary>
    /// This method will Filter the records - depend on the Credit Parameters set by the end-user
    /// </summary>
    /// <param name="_ObjCreditParameterApprovalDataTable"></param>
    /// <returns></returns>
    private DataTable FunPriSortColumns(DataTable dt)
    {

        string strSortBy = "";

        if (ddlDocumentType.SelectedValue=="6")
        {
            strSortBy = "CustomerCode";
        }
        else 
        {
            strSortBy = _strEnquiryDate;
        }
       
        dt = _ObjCreditParameterApprovalDataTable;
        dt.DefaultView.Sort = strSortBy + " asc, Created_on asc";




        return dt.DefaultView.ToTable();
    }
    #endregion
    #region Paging and Searching Methods For Grid
    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>
    private void FunPriGetSearchValue()
    {
        if (gvCreditParameter.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch1")).Text.Trim();
            strSearchVal2 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch2")).Text.Trim();
            strSearchVal3 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch3")).Text.Trim();
            strSearchVal4 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            strSearchVal5 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();

            strSearchVal7 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch7")).Text.Trim();
            strSearchVal8 = ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch8")).Text.Trim();
        }
    }
    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (gvCreditParameter.HeaderRow != null)
        {
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch1")).Text = "";
            for (int intHRow = 1; intHRow < gvCreditParameter.Columns.Count; intHRow++)
            {
                GridViewRow gvHead = gvCreditParameter.HeaderRow;
                if (gvHead.RowType == DataControlRowType.Header)
                {
                    if (gvHead.FindControl("txtHeaderSearch" + intHRow.ToString()) != null)
                        ((TextBox)gvHead.FindControl("txtHeaderSearch" + intHRow.ToString())).Text = "";
                }
            }
            //((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch3")).Text = "";
            //((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";


        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (gvCreditParameter.HeaderRow != null)
        {
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch1")).Text = strSearchVal1;
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch2")).Text = strSearchVal2;
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch3")).Text = strSearchVal3;
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;

            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch7")).Text = strSearchVal7;
            ((TextBox)gvCreditParameter.HeaderRow.FindControl("txtHeaderSearch8")).Text = strSearchVal8;
        }
    }
    /// <summary>
    /// Will Perform Sorting On Colunm base upon the link button id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnSortLOB":
                    strSortColName = "LOB.LOB_Code + ' - ' + LOB.LOB_Name";
                    break;
                case "lnkbtnSortBranch":
                    strSortColName = "BM.Location_Code + ' - ' + LC.LocationCat_Description";
                    break;
                case "lnkbtnSortProduct":
                    strSortColName = "PM.Product_Code + ' - ' + PM.Product_Name";
                    break;
                case "lnkbtnSortEnquiryNo":
                    strSortColName = "EU.Enquiry_No";
                    break;
                case "lnkbtnSortEnquiryDate":
                    strSortColName = "EU.Enquiry_Date";
                    break;
                case "lnkbtnCustomerName":
                    strSortColName = "CM.Customer_Name";
                    break;
                case "lnkbtnConstitutionName":
                    strSortColName = "ConsMas.Constitution_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvCreditParameter.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)gvCreditParameter.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>
    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                strSearchVal += " and LOB.LOB_Code + ' - ' + LOB.LOB_Name like '%" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and BM.Location_Code + ' - ' + LC.LocationCat_Description like '%" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and PM.Product_Code + ' - ' + PM.Product_Name  like '%" + strSearchVal3 + "%'";
            }
            if (strSearchVal4 != "")
            {
                strSearchVal += " and EU.Enquiry_No like '%" + strSearchVal4 + "%'";
            }
            if (strSearchVal5 != "")
            {
                strSearchVal += " and convert(varchar,EU.Enquiry_Date,103) like '%" + Utility.StringToDate(strSearchVal5).ToString("dd/MM/yyyy") + "%'";
            }
            //if (strSearchVal6 != "")
            //{
            //    strSearchVal += " and CM.Customer_Code like '%" + strSearchVal7 + "%'";
            //}
            if (strSearchVal7 != "")
            {
                strSearchVal += " and (CM.Customer_Code+' - '+CM.Customer_Name) like '%" + strSearchVal7 + "%'";
            }
            if (strSearchVal8 != "")
            {
                strSearchVal += " and (cast(ConsMas.Constitution_Id as varchar(5))+'-'+ConsMas.Constitution_Name) like '%" + strSearchVal8 + "%'";
            }

            if ((strSearchVal.Length > 4) && (strSearchVal.Substring(0, 4) == " and"))
            {
                strSearchVal = strSearchVal.Substring(4, strSearchVal.Length - 4);
            }
            hdnSearch.Value = strSearchVal;
            if (strSearchVal != null && strSearchVal != string.Empty)
            {
                FunPriBindGrid();
                FunPriSetSearchValue();
            }
            else
            {
                FunPriBindGrid();
                FunPriClearSearchValue();
            }
            if (txtboxSearch.Text != "")
                ((TextBox)gvCreditParameter.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    /// <summary>
    /// Gets the Sort Direction of the strColumn in the Grid View Using hidden control
    /// </summary>
    /// <param name="strColumn"> Colunm Name is Passed</param>
    /// <returns>Sort Direction as a String </returns>
    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }
    #endregion
    #endregion


    protected void RBUnapproved_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void RBAll_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void RBCustomerCode_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void RBEnquiryNumber_CheckedChanged(object sender, EventArgs e)
    {

    }


    #region User defined methods
    private void FunPriPassToAddPage()
    {
        try
        {
            //Build the Query String 
            DataTable dtCPT = FunGetCreditParamData();
            if (dtCPT != null && dtCPT.Rows.Count > 0)
            {
                DataRow dtrCPTRow = dtCPT.Rows[0];
                FormsAuthenticationTicket TicketCustomerId = new FormsAuthenticationTicket(dtrCPTRow["CustomerId"].ToString(), false, 0);
                FormsAuthenticationTicket TicketEnquiryNo = new FormsAuthenticationTicket(dtrCPTRow["DOC_NUMBER"].ToString(), false, 0);

                FormsAuthenticationTicket TicketCreditParamTransID = new FormsAuthenticationTicket(dtrCPTRow["CreditParamTransID"].ToString(), false, 0);
                FormsAuthenticationTicket TicketCompanyId = new FormsAuthenticationTicket(dtrCPTRow["CompanyId"].ToString(), false, 0);
                FormsAuthenticationTicket TicketLOBId = new FormsAuthenticationTicket(dtrCPTRow["LOBId"].ToString(), false, 0);
                FormsAuthenticationTicket TicketProductId = new FormsAuthenticationTicket(dtrCPTRow["ProductId"].ToString(), false, 0);
                FormsAuthenticationTicket TicketConstitutionId = new FormsAuthenticationTicket(dtrCPTRow["ConstitutionID"].ToString(), false, 0);
                FormsAuthenticationTicket TicketLocationId = new FormsAuthenticationTicket(dtrCPTRow["LocationID"].ToString(), false, 0);
                FormsAuthenticationTicket TicketNumOfApproved = new FormsAuthenticationTicket(dtrCPTRow["NumOfApproved"].ToString(), false, 0);
                FormsAuthenticationTicket TicketMode = ((dtrCPTRow["LocationID"].ToString() == "" || dtrCPTRow["LocationID"].ToString() == string.Empty) ? new FormsAuthenticationTicket("Cus", false, 0) : new FormsAuthenticationTicket("Enq", false, 0));


                string strredirect = _strEnquiryLevel + "?qsEnqNo=" + FormsAuthentication.Encrypt(TicketEnquiryNo)
                + "&CustId=" + FormsAuthentication.Encrypt(TicketCustomerId)
                          + "&CompanyId=" + FormsAuthentication.Encrypt(TicketCompanyId)
                          + "&LOBId=" + FormsAuthentication.Encrypt(TicketLOBId)
                          //+ "&ConstitutionId=" + FormsAuthentication.Encrypt(TicketConstitutionId)
                      //+ "&ProductId=" + FormsAuthentication.Encrypt(TicketProductId)
                      + "&LocationID=" + FormsAuthentication.Encrypt(TicketLocationId)
                      + "&CreditParamTransId=" + FormsAuthentication.Encrypt(TicketCreditParamTransID)
                +"&Mode=" + FormsAuthentication.Encrypt(TicketMode)
                +"&NumOfApproved=" + FormsAuthentication.Encrypt(TicketNumOfApproved)
                +"&qsMode=" + _TransLanderMode;
                Response.Redirect(strredirect,false); // EnquiryN

            }

        }
        catch (Exception ex)
        {


            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        /* 
         //CheckBox chkApprove = null;
         foreach (GridViewRow grvCPARow in gvCreditParameter.Rows)
         {
             string strCreditParamTransID = ((Label)grvCPARow.FindControl("lblCreditParamTransID")).Text;
             //chkApprove = ((CheckBox)grvCPARow.FindControl("chkApprove"));
             if ((string.Compare(strCreditParamTransID, _TransLander_CPT_ID)) == 0)
             {
                 string strCustomerId = ((Label)grvCPARow.FindControl("lblCustomerId")).Text;
                 string strEnquiryNo = ((Label)grvCPARow.FindControl("lblEnquiryNoHdn")).Text;//grvCPARow.Cells[_GridCustCodeIndex].ToString();

                 string strCompanyId = ((Label)grvCPARow.FindControl("lblCompanyId")).Text;
                 string strLOBId = ((Label)grvCPARow.FindControl("lblLOBId")).Text;
                 string strProductId = ((Label)grvCPARow.FindControl("lblProductId")).Text;
                 string strConstitutionId = ((Label)grvCPARow.FindControl("lblConstitutionID")).Text;
                 string strBranchId = ((Label)grvCPARow.FindControl("lblBranchId")).Text;

                 _NumOfApproved = Convert.ToInt32(((Label)grvCPARow.FindControl("lblNumOfApproved")).Text);
                 string strMode = ""; // Enquiry or customer

                 if (_IsEnquiryMode)
                     strMode = "Enq";
                 else
                     strMode = "Cus";

                 FormsAuthenticationTicket TicketCustomerId = new FormsAuthenticationTicket(strCustomerId, false, 0);
                 FormsAuthenticationTicket TicketEnquiryNo = new FormsAuthenticationTicket(strEnquiryNo, false, 0);

                 FormsAuthenticationTicket TicketCreditParamTransID = new FormsAuthenticationTicket(strCreditParamTransID, false, 0);
                 FormsAuthenticationTicket TicketCompanyId = new FormsAuthenticationTicket(strCompanyId, false, 0);
                 FormsAuthenticationTicket TicketLOBId = new FormsAuthenticationTicket(strLOBId, false, 0);
                 FormsAuthenticationTicket TicketProductId = new FormsAuthenticationTicket(strProductId, false, 0);
                 FormsAuthenticationTicket TicketConstitutionId = new FormsAuthenticationTicket(strConstitutionId, false, 0);
                 FormsAuthenticationTicket TicketBranchId = new FormsAuthenticationTicket(strBranchId, false, 0);
                 FormsAuthenticationTicket TicketNumOfApproved = new FormsAuthenticationTicket(_NumOfApproved.ToString(), false, 0);
                 FormsAuthenticationTicket TicketMode = ((strBranchId == "" || strBranchId == string.Empty) ? new FormsAuthenticationTicket("Cus", false, 0) : new FormsAuthenticationTicket("Enq", false, 0));                
                    
                 string strredirect = _strEnquiryLevel + "?qsEnqNo=" + FormsAuthentication.Encrypt(TicketEnquiryNo) + "&CustId=" + FormsAuthentication.Encrypt(TicketCustomerId)
                     + "&CompanyId=" + FormsAuthentication.Encrypt(TicketCompanyId)
                     + "&LOBId=" + FormsAuthentication.Encrypt(TicketLOBId)
                     + "&ConstitutionId=" + FormsAuthentication.Encrypt(TicketConstitutionId)
                 + "&ProductId=" + FormsAuthentication.Encrypt(TicketProductId)
                 + "&BranchID=" + FormsAuthentication.Encrypt(TicketBranchId)
                 + "&CreditParamTransId=" + FormsAuthentication.Encrypt(TicketCreditParamTransID)
                 + "&Mode=" + FormsAuthentication.Encrypt(TicketMode)
                 + "&NumOfApproved=" + FormsAuthentication.Encrypt(TicketNumOfApproved)
                 + "&qsMode=" + _TransLanderMode;
                 Response.Redirect(strredirect); // EnquiryNo
             }
         }
         * */
    }
    #endregion

    #region ShowAll and Cancel
    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid();
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Origination/S3GORGTransLander.aspx?Code=CPA&create=1&MultipleDNC=1&DNCOption=1&Modify=0");
    }
    #endregion
}
