
#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Collateral
/// Screen Name         :   Collateral Valuation
/// Created By          :   muni kavitha
/// Created Date        :   4-May-2011
/// Purpose             :  
/// 
/// <Program Summary>
#endregion



#region Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Web.Security;
using System.IO;
using System.Xml;
using System.Configuration;
using S3GBusEntity.Collateral;
using System.Collections;

#endregion

public partial class Collateral_S3GCLTCollateralValuation : ApplyThemeForProject
{
    #region Common Variable declaration
    int intCompanyID, intUserID = 0;
    int intCapture_ID = 0;
    Dictionary<string, string> Procparam = null;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    string strCLT_ValuationNo = null;
    string strCLT_CaptureID = "0";
    int intErrCode = 0;
    bool hasValue = false;
    
    UserInfo ObjUserInfo = new UserInfo();
 

    SerializationMode SerMode = SerializationMode.Binary;
    S3GSession ObjS3GSession;
    //S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
   // List<StringBuilder> lstXML = new List<StringBuilder>();
    DataSet ds = null;
       
    CollateralMgtServicesReference.CollateralMgtServicesClient objCollateralValuation;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDataTable objS3G_CLT_ValuationDataTable = null;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationRow objS3G_CLT_ValuationRow = null;


    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "S3GCLTTransLander.aspx?Code=KVA";
    string strRedirectPage = "S3GCLTCollateralValuation.aspx?qsMode=C";
    
    string strRedirectPageAdd = "window.location.href='../Collateral/S3GCLTCollateralValuation.aspx?qsMode=C';";
    string strRedirectPageView1 = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KVA';";
    //string strcode = "";
    static string strPageName = "Collateral Valuation";
    //User Authorization
    string strMode = "";
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bYesNo = false;
    //Code end

    int intNoofSearch = 2;
    string[] arrSortCol = new string[] { "Collateral_Tran_No", "Customer_Name" };
    string strProcName = "";

    ArrayList arrSearchVal = new ArrayList(1);
    PagingValues ObjPaging = new PagingValues();

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

    private void FunPriBindGrid()
    {
        try
        {
            //Paging Properties set
            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;       
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            FunPriGetSearchValue();


            Procparam = new Dictionary<string, string>();
            //Procparam.Add("@Workflow_Sequence_ID", "0");
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

           // Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Option", "1");
            if (rbtCustomer.Checked == true)
                Procparam.Add("@Param", "1");
            if (rbtAgent.Checked == true)
                Procparam.Add("@Param", "2");
            //Procparam.Add("@CurrentPage", ProPageNumRW.ToString ());
            //Procparam.Add("@PageSize", ProPageNumRW.ToString());
            //Procparam.Add("@SearchValue", hdnSearch.Value );
            //Procparam.Add("@OrderBy", hdnOrderBy.Value );

            
            grvPaging.BindGridView("S3G_CLT_GetCustomerORAgent", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }

    }

    #region Paging and Searching Methods For Grid


    private void FunPriGetSearchValue()
    {
        arrSearchVal = grvPaging.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        grvPaging.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        grvPaging.FunPriSetSearchValue(arrSearchVal);
    }

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        if (rbtCustomer.Checked == true)
            arrSortCol[1] = "Customer_Name";
        if (rbtAgent.Checked == true)
            arrSortCol[1] = "UTPA_Name";

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver
            
            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like '" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }

            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvPaging.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

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

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        if (rbtCustomer.Checked == true)
            arrSortCol[1] = "Customer_Name";
        if (rbtAgent.Checked == true)
            arrSortCol[1] = "UTPA_Name";
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            for (int iCount = 0; iCount < arrSearchVal.Capacity; iCount++)
            {
                if (lnkbtnSearch.ID == "lnkbtnSort" + (iCount + 1).ToString())
                {
                    strSortColName = arrSortCol[iCount].ToString();
                    break;
                }
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();
            arrSortCol[1] = "";
            if (strDirection == "ASC")
            {
                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvPaging.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
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

    #endregion

    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
           
        }
        catch (Exception ex)
        {
            cvCollateralValuation.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_PageLoad + this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;
        }
        
    }



    #region Load Page
    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            FunPubSetIndex(1);
            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end

            //Date Format
            ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            _DateFormat = ObjS3GSession.ProDateFormatRW;
            

            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;


            strMode = Request.QueryString.Get("qsMode");
            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                strMode = Request.QueryString.Get("qsMode");

                if (fromTicket != null)
                {
                    strCLT_CaptureID = fromTicket.Name;

                }
            }



            #region Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
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


            SetTabIndexChanged();

          //  ScriptManager.RegisterStartupScript(this, GetType(), "te", "DivSize();", true);
            if (!IsPostBack)
            {
                if (PageMode == PageModes.Create)
                {
                    FunPriBindGrid();
                }
                //FunPriLoadType();
                //GetCustORAgent1();
               if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriCLT_ValuationModify(strCLT_CaptureID );
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunPriCLT_ValuationModify(strCLT_CaptureID);
                    FunPriDisableControls(1);
                    //ddlLOB.Focus();
                   

                }
                else
                {
                   FunPriDisableControls(0);
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "te", "Resize();", true);
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }

    }
    #endregion


    #region  Button Events
    //save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriSaveRecord();
        }
        catch (Exception ex)
        {
            cvCollateralValuation.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_InsertUpdate + this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;

        }
    }
    //cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView,false);
        
        //if (strMode == "C")
        //{
        //    tabCustORAgent.Enabled = true;
        //    if (tcCollateralValuation.ActiveTabIndex == 0)

        //        Response.Redirect(strRedirectPageView);
        //    else
                
        //        tcCollateralValuation.ActiveTabIndex = 0;
        //        SetTabIndexChanged();
        //        FunPriBindGrid();
        //    //Response.Redirect(strRedirectPage);
        //}

        //else
        //{
        //    Response.Redirect(strRedirectPageView);

        //}
    }

    //clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            //FunPriClearPage();
            tabCustORAgent.Enabled = true;
            tcCollateralValuation.ActiveTabIndex = 0;
            SetTabIndexChanged();
            FunPriBindGrid();
        }
        catch (Exception ex)
        {

            cvCollateralValuation.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_Clear + this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;
        }
    }
    #endregion

    #region Save
    private void FunPriSaveRecord()
    {
        objCollateralValuation = new CollateralMgtServicesReference.CollateralMgtServicesClient();
        try
        {
           
            objS3G_CLT_ValuationDataTable = new CollateralMgtServices.S3G_CLT_ValuationDataTable();
            objS3G_CLT_ValuationRow = null;
            objS3G_CLT_ValuationRow = objS3G_CLT_ValuationDataTable.NewS3G_CLT_ValuationRow();
                       
            objS3G_CLT_ValuationRow.Company_ID = ObjUserInfo.ProCompanyIdRW;
            objS3G_CLT_ValuationRow.Customer_ID = Convert .ToInt32 (ViewState["Cust_ID"]);

            if (ViewState["CID"] != null)
                strCLT_CaptureID = Convert.ToString(ViewState["CID"]);

            objS3G_CLT_ValuationRow.Collateral_Capture_ID = Convert.ToInt32(strCLT_CaptureID); //intCapture_ID;

            if (ViewState["Collateral_VAL_No"] != null)
                objS3G_CLT_ValuationRow.Collateral_Valuation_No = ViewState["Collateral_VAL_No"].ToString();
            else
                objS3G_CLT_ValuationRow.Collateral_Valuation_No = "";
         
            StringBuilder sbXML = new StringBuilder();
               
            objS3G_CLT_ValuationRow.XML_HighLiqSecDetails = FunPriGenerateXML(sbXML ,gvHighLiqDetails1).ToString(); 
            objS3G_CLT_ValuationRow.XML_MedLiqSecDetails = FunPriGenerateXML(sbXML, gvMedLiqDetails1).ToString(); 
            objS3G_CLT_ValuationRow.XML_LowLiqSecDetails = FunPriGenerateXML(sbXML, gvLowLiqDetails1).ToString();
            objS3G_CLT_ValuationRow.XML_CommoditiesDetails = FunPriGenerateXML(sbXML, gvCommoDetails1).ToString();
            objS3G_CLT_ValuationRow.XML_FinancialDetails = FunPriGenerateXML(sbXML, gvFinDetails1).ToString();

            if (hasValue == false)
            {
                Utility.FunShowAlertMsg(this.Page, "Valuate atleast one Collateral");
                return;
            }

         
            objS3G_CLT_ValuationRow.Modified_By = ObjUserInfo.ProUserIdRW;
            objS3G_CLT_ValuationRow.Modified_On = DateTime.Now;

                      
            objS3G_CLT_ValuationDataTable.AddS3G_CLT_ValuationRow(objS3G_CLT_ValuationRow);


            intErrCode = objCollateralValuation.FunPubCreateCollateralValuation(out strCLT_ValuationNo, SerMode, ClsPubSerialize.Serialize(objS3G_CLT_ValuationDataTable, SerMode));
            //intErrCode = 0;
            //strCLT_ValuationNo = "KVA/50";
            switch (intErrCode)
            {
                case 0:

                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    if(strMode =="C")
                    {
                        // Utility.FunShowAlertMsg(this.Page, "Debt Collector Master " + Resources.ValidationMsgs.S3G_ValMsg_Save, strRedirectPageView);

                        strAlert = "Collateral Valuation details added successfully - " + strCLT_ValuationNo;
                        //strAlert += @"\n\nCollateral Valuation Number - " + strCLT_ValuationNo;
                        strAlert += @"\n\nWould you like to add one more Collateral Valuation details?";
                       // strAlert += @"\n" + Resources.ValidationMsgs.S3G_ValMsg_Next;
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView1 + "}";
                        strRedirectPageView1 = string.Empty;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView1, true);
                        // return;
                    }
                    //else if (strCLT_CaptureID != null || strCLT_CaptureID != "")
                    else if(strMode =="M")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Collateral Valuation " + Resources.ValidationMsgs.S3G_ValMsg_Update, strRedirectPageView);
                    }

                    break;

                case 1:
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._1, strRedirectPage);
                    return;
                    break;
                case 2:
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._2, strRedirectPage);
                    return;
                    break;
                case 50:
                    Utility.FunShowAlertMsg(this.Page, Resources.ValidationMsgs._50, strRedirectPage);
                    return;
                    break;
                default:
                    Utility.FunShowAlertMsg(this.Page, "Due to data problem unable to Save");
                    break;


            }
        }

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //if (objCollateralValuation != null)
                objCollateralValuation.Close();
        }
    }
    #endregion

    #region To clear the Data in the Page
    private void FunPriClearPage()
    {
        try
        {


        }
        catch (Exception ex)
        {

            cvCollateralValuation.ErrorMessage = Resources.ValidationMsgs.S3G_ErrMsg_Clear + this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;
        }

    }

    #endregion



    #region "User Authorization"
    ////This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                btnModifyH.Enabled = false;
                if (!bModify)
                {

                }
               
                break;

           
            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                btnSave.Enabled = btnClear.Enabled = false;

                gvHighLiqDetails.Columns[11].Visible = false;
                gvMedLiqDetails.Columns[10].Visible = false;
                gvLowLiqDetails.Columns[9].Visible = false;
                gvCommoDetails.Columns[9].Visible = false;
                gvFinDetails.Columns[8].Visible = false;

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }

                txthValuationDate.ReadOnly = true;
                txthValuationDate.Attributes.Remove("onblur");
                txthRemarks.ReadOnly = true;
                txthCurrentValue.ReadOnly = true;
                txthCurrentTotalValue.ReadOnly = true;

                btnClearH.Enabled = false;

                txtMValuationDate.ReadOnly = true;

                txtMValuationDate.Attributes.Remove("onblur");
                txtMRemarks.ReadOnly = true;
                txtMCurrentMarketValue.ReadOnly = true;

             
                btnModifyF.Enabled = false;
                btnModifyH.Enabled = false;
                btnModifyC.Enabled = false;
                btnModifyL.Enabled = false;
                btnModifyM.Enabled = false;

                txtLValuationDate.ReadOnly = true;
                txtLValuationDate.Attributes.Remove("onblur");
                txtLRemarks.ReadOnly = true;
                txtLCurrentValuePerUnit.ReadOnly = true;
                txtLCurrentTotalValue.ReadOnly = true;


                txtFCurrentMarketValue.ReadOnly = true;
                txtFRemarks.ReadOnly = true;
                txtCCurrentValuePerUnit.ReadOnly = true;
                txtCRemarks.ReadOnly = true;
                txtCCurrentTotalValue.ReadOnly = true;

                btnClearL.Enabled = false;
                btnClearF.Enabled = false;
                btnClearC.Enabled = false;
                btnClearM.Enabled = false;
            


                break;
        }
    }
    ////Code end
    #endregion

   
    private void PopulateCustomerDetails(string custID, int optCA)
    {
        try
        {
            bool bCLT_Available = false;
            if(ViewState["CID"]!=null )
             strCLT_CaptureID = Convert.ToString(ViewState["CID"]);

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@CA_ID", custID);
            Procparam.Add("@Option", optCA .ToString ());
            Procparam.Add("@Collateral_Capture_ID", strCLT_CaptureID);

            DataSet dsCustAndAccDetails = Utility.GetDataset("S3G_CLT_GetCustomerAccDetails", Procparam);
            
            
              if(optCA ==1)
                {
                    if (dsCustAndAccDetails.Tables[0].Rows.Count > 0)
                    {
                        pnlAccDetails.Visible = true;
                        gvAccDetails.DataSource = dsCustAndAccDetails.Tables[0];
                        gvAccDetails.DataBind();
                        //gvAccDetails.HeaderRow.Style.Add("position", "relative");
                        //gvAccDetails.HeaderRow.Style.Add("z-index", "auto");
                        //gvAccDetails.HeaderRow.Style.Add("top", "auto");
                    }
                    else
                    {
                        pnlAccDetails.Visible = false;
                    }
                    if (dsCustAndAccDetails.Tables[1].Rows.Count > 0)
                    {
             
                        pnlHeader.Visible = true;
                        S3GCustomerAddress1.SetCustomerDetails(dsCustAndAccDetails.Tables[1].Rows[0], true);
                        pnlAgent.Visible = false;
                    }

                }
                else if (optCA == 2)
                {
                    if (dsCustAndAccDetails.Tables[0].Rows.Count > 0)
                    {
                        
                     pnlAgent.Visible = true;
                    txtACode.Text = dsCustAndAccDetails.Tables[0].Rows[0]["UTPA_Code"].ToString();
                    txtAName.Text = dsCustAndAccDetails.Tables[0].Rows[0]["UTPA_Name"].ToString();
                    txtAaddress.Text = dsCustAndAccDetails.Tables[0].Rows[0]["Address"].ToString();
                    txtAcity.Text = dsCustAndAccDetails.Tables[0].Rows[0]["City"].ToString();
                    txtAState.Text = dsCustAndAccDetails.Tables[0].Rows[0]["State"].ToString();
                    txtACountry.Text = dsCustAndAccDetails.Tables[0].Rows[0]["Country"].ToString();
                    pnlHeader.Visible = false;
                    
                    }
                    pnlAccDetails.Visible = false;
                    gvAccDetails.DataSource = null;
                    gvAccDetails.DataBind();
           }
                
            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            //Procparam.Add("@Customer_ID", custID);
            Procparam.Add("@Collateral_Capture_ID", strCLT_CaptureID);
            ds = Utility.GetDataset("S3G_CLT_GetCollateralCaptures", Procparam);

            if (ds.Tables[5].Rows.Count > 0)
            {
              
                txtCapNo.Text = ds.Tables[5].Rows[0]["Collateral Capture Number"].ToString();
                txtCapDate.Text = ds.Tables[5].Rows[0]["Collateral Captured Date"].ToString();
                //txtLOB.Text = ds.Tables[5].Rows[0]["Line of Business"].ToString();
                //txtBranch.Text = ds.Tables[5].Rows[0]["Branch"].ToString();
               txtValNo.Text = ds.Tables[5].Rows[0]["Collateral Valuation Number"].ToString();
                if (strMode == "C")
                    txtValuationDate.Text = DateTime.Now.ToString(strDateFormat);//DateTime.Parse(dtLRNNumDetails.Rows[0]["Created_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                else
                    txtValuationDate.Text = ds.Tables[5].Rows[0]["Valuation Date"].ToString();  
            }

            // High Liquid Securities
            if (ds.Tables[0].Rows.Count > 0)
                {
             
              //  pnlHighLiqDetails.Visible = true;
                lblHighLiqDetails.Visible = false;

                ViewState["HighLiq"] = gvHighLiqDetails.DataSource = gvHighLiqDetails1.DataSource = ds.Tables[0];
                gvHighLiqDetails.DataBind();
                gvHighLiqDetails1.DataBind();
                               
                Label lblTotal = gvHighLiqDetails.FooterRow.FindControl("lblTotal") as Label;
                lblTotal.Text = ds.Tables[0].Rows[0]["GTotal"].ToString();//d1.ToString(Funsetsuffix());
               
                bCLT_Available = true;
            }
            else
            {
                //lblHighLiqDetails.Visible = true;
                gvHighLiqDetails.DataSource = null ;
                gvHighLiqDetails.DataBind();
                //pnlHighLiqDetails.Visible = false;
               // bCLT_Available = false;

                tcCollateralValuation.Tabs[2].Visible = false;
            }

            // Medium Liquid Securities
            if (ds.Tables[1].Rows.Count > 0)
            {
                //pnlMedLiqDetails.Visible = true;
                lblMedLiqDetails.Visible = false;
                ViewState["MedLiq"] = gvMedLiqDetails.DataSource = gvMedLiqDetails1.DataSource = ds.Tables[1];
                gvMedLiqDetails.DataBind();
                gvMedLiqDetails1.DataBind();
               

                //decimal d1 = 0;
                              //decimal d1 = Convert.ToDecimal(ds.Tables[0].Compute("sum(tot)", "tot <> 0"));
               
                Label lblTotal = gvMedLiqDetails.FooterRow.FindControl("lblTotal") as Label;
                lblTotal.Text = ds.Tables[1].Rows[0]["GTotal"].ToString();
      
                bCLT_Available = true;
            }
            else
            {
                //lblMedLiqDetails.Visible = true;
                gvMedLiqDetails.DataSource = null ;
                gvMedLiqDetails.DataBind();
                //pnlMedLiqDetails .Visible = false;
               //bCLT_Available = false;

                tcCollateralValuation.Tabs[3].Visible = false;
            }

            // Low Liquid Securities
            if (ds.Tables[2].Rows.Count > 0)
            {

                //pnlLowLiqDetails.Visible = true;
               lblLowLiqDetails.Visible = false ;
               ViewState["LowLiq"] = gvLowLiqDetails.DataSource = gvLowLiqDetails1.DataSource = ds.Tables[2];
               gvLowLiqDetails.DataBind();
               gvLowLiqDetails1.DataBind();

                Label lblTotal = gvLowLiqDetails.FooterRow.FindControl("lblTotal") as Label;
                lblTotal.Text = ds.Tables[2].Rows[0]["GTotal"].ToString();//d1.ToString(Funsetsuffix());

                bCLT_Available = true;
            }
            else
            {
                //lblLowLiqDetails.Visible = true;
                gvLowLiqDetails.DataSource = null ;
                gvLowLiqDetails.DataBind();
                // pnlLowLiqDetails.Visible = false;
                //bCLT_Available = false;

                tcCollateralValuation.Tabs[4].Visible = false;
            }

            // Commodities
            if (ds.Tables[3].Rows.Count > 0)
            {
                lblCommodityDetails.Visible = false;
                //pnlCommodityDetails.Visible = true;
                ViewState["CommLiq"] = gvCommoDetails.DataSource = gvCommoDetails1.DataSource = ds.Tables[3];
                gvCommoDetails.DataBind();
                gvCommoDetails1.DataBind();

                                
                Label lblTotal = gvCommoDetails.FooterRow.FindControl("lblTotal") as Label;
                lblTotal.Text = ds.Tables[3].Rows[0]["GTotal"].ToString();

                bCLT_Available = true;
            }
            else
            {
                //lblCommodityDetails.Visible = true;
                gvCommoDetails.DataSource = null ;
                gvCommoDetails.DataBind();
                // pnlCommodityDetails.Visible = false;
                //bCLT_Available = false;

                tcCollateralValuation.Tabs[5].Visible = false;
            }


            // Financial Securities
            if (ds.Tables[4].Rows.Count > 0)
            {
                lblFinDetails.Visible = false;
                //pnlFinDetails.Visible = true;
                ViewState["FinLiq"] = gvFinDetails.DataSource = gvFinDetails1.DataSource = ds.Tables[4];
                gvFinDetails.DataBind();
                gvFinDetails1.DataBind();

                Label lblTotal = gvFinDetails.FooterRow.FindControl("lblTotal") as Label;
                lblTotal.Text = ds.Tables[4].Rows[0]["GTotal"].ToString();

                bCLT_Available = true;
            }
            else
            {
                //lblFinDetails.Visible = true;
                gvFinDetails.DataSource = null ;
                gvFinDetails.DataBind();
                // pnlFinDetails.Visible = false;
               // bCLT_Available = false;

                tcCollateralValuation.Tabs[6].Visible = false;
            }

            if  (bCLT_Available == false)
            {
                Utility.FunShowAlertMsg(this.Page, "No Collateral Details are Available");
                return;
            }
            
           
        }
        catch (Exception ex)
        {
           // lblErrorMessage.Text = ex.ToString();
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw (ex);
        }
    }
    protected void rdHSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvHighLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["HighLiq"];

        FunPriResetRdButton(gvHighLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblhSlNo.Text = drow["SlNo"].ToString();
        txthCollSecurities.Text = drow["Collateral_Securities_Name"].ToString();
        txthIssuedBy.Text = drow["ISSUE_BY"].ToString();
        txthAsquisitionDate.Text = drow["Created_On"].ToString();
        txthMaturityDate.Text = drow["Maturity_Date"].ToString();
        txthFaceValue.Text = drow["Unit_Face_Value"].ToString();
        txthAvailableUnits.Text = drow["No_Of_Units"].ToString();
        txthCurrentValue.Text = drow["Valuation_Current_Market_Value"].ToString();
        txthCurrentTotalValue.Text = drow["tot"].ToString();
        txthValuationDate.Text = drow["Valuation_Date"].ToString();
        txthRemarks.Text = drow["Valuation_Remarks"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyH.Enabled = false;
        }
        else
        {
            btnModifyH.Enabled = true;
        }

    }
    protected void btnModifyH_Click(object sender, EventArgs e)
    {
        FunHighLIQSecuritiesGridViewRowUpdating();
    }
    private void FunHighLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["HighLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblhSlNo.Text) - 1];
            drow.BeginEdit();
            drow["Valuation_Current_Market_Value"] = txthCurrentValue.Text.Trim();
            drow["tot"] = txthCurrentTotalValue.Text.Trim();
            drow["Valuation_Date"] = txthValuationDate.Text.Trim();
            drow["Valuation_Remarks"] = txthRemarks.Text.Trim();
            drow.EndEdit();


            ViewState["HighLiq"] = gvHighLiqDetails1.DataSource = dtSecurities;
            gvHighLiqDetails1.DataBind();

            FunClearHDetails();

            btnModifyH.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearHDetails()
    {
        lblhSlNo.Text = "";
        txthCollSecurities.Text = "";
        txthIssuedBy.Text = "";
        txthAsquisitionDate.Text = "";
        txthMaturityDate.Text = "";
        txthFaceValue.Text = "";
        txthAvailableUnits.Text = "";
        txthCurrentValue.Text = "";
        txthCurrentTotalValue.Text = "";
        txthValuationDate.Text = "";
        txthRemarks.Text = "";

        btnModifyH.Enabled = false;
        btnClearH.Enabled = true;
        FunPriResetRdButton(gvHighLiqDetails1, -1);

    }
    protected void btnClearH_Click(object sender, EventArgs e)
    {
        FunClearHDetails();
        btnModifyH.Enabled = false;
    }
 
    private void FunPriResetRdButton(GridView grv, int intRowIndex)
    {
        for (int i = 0; i <= grv.Rows.Count - 1; i++)
        {
            if (i != intRowIndex)
            {
                RadioButton rdSelect = grv.Rows[i].FindControl("rdSelect") as RadioButton;
                rdSelect.Checked = false;
            }
        }
    }


    protected void btnModifyM_Click(object sender, EventArgs e)
    {
        FunMedLIQSecuritiesGridViewRowUpdating();
    }

    protected void rdMSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvMedLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["MedLiq"];

        FunPriResetRdButton(gvMedLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];
        lblMSlNo.Text = drow["SlNo"].ToString();
        txtMCollSecurities.Text = drow["Collateral_Securities_Name"].ToString();
        txtMDescription.Text = drow["Description"].ToString();
        txtMModel.Text = drow["Model"].ToString();
        txtMYear.Text = drow["Year"].ToString();
        txtMRegistrationNo.Text = drow["Registration_No"].ToString();
        txtMSerialNo.Text = drow["Serial_No"].ToString();
        txtMValue.Text = drow["Value"].ToString();
        txtMCurrentMarketValue.Text = drow["Valuation_Current_Market_Value"].ToString();
        txtMValuationDate.Text = drow["Valuation_Date"].ToString();
        txtMRemarks.Text = drow["Valuation_Remarks"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyM.Enabled = false;
        }
        else
        {
            btnModifyM.Enabled = true;
        }

    }

    private void FunMedLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["MedLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblMSlNo.Text) - 1];
            drow.BeginEdit();
            drow["Valuation_Current_Market_Value"] = txtMCurrentMarketValue.Text.Trim();
            drow["Valuation_Date"] = txtMValuationDate.Text.Trim();
            drow["Valuation_Remarks"] = txtMRemarks.Text.Trim();
            drow.EndEdit();


            ViewState["MedLiq"] = gvMedLiqDetails1.DataSource = dtSecurities;
            gvMedLiqDetails1.DataBind();

            FunClearMDetails();

            btnModifyM.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearMDetails()
    {
        lblMSlNo.Text =
        txtMCollSecurities.Text =
        txtMValuationDate.Text =
        txtMRemarks.Text =
        txtMDescription.Text =
        txtMModel.Text = 
        txtMYear.Text =
        txtMRegistrationNo.Text =
        txtMSerialNo.Text =
        txtMCurrentMarketValue.Text =
        txtMValue.Text =
        txtMValuationDate.Text = "";



        btnModifyM.Enabled = false;
        btnClearM.Enabled = true;
        FunPriResetRdButton(gvMedLiqDetails1, -1);

    }
    protected void btnClearM_Click(object sender, EventArgs e)
    {
        FunClearMDetails();
        btnModifyM.Enabled = false;
    }

    


 

    protected void btnModifyL_Click(object sender, EventArgs e)
    {
        FunLowLIQSecuritiesGridViewRowUpdating();
    }

    protected void rdLSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvLowLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["LowLiq"];

        FunPriResetRdButton(gvLowLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];
        lblLSlNo.Text = drow["SlNo"].ToString();
        txtLCollSecurities.Text = drow["Collateral_Securities_Name"].ToString();
        txtLLocationDetails.Text = drow["Location_Details"].ToString();
        txtLMeasurement.Text = drow["Measurement"].ToString();
        //txtLExtent.Text = drow["Measurement_Area"].ToString();
        txtLUnitRate.Text = drow["Unit_Rate"].ToString();
        txtLCurrentValuePerUnit.Text = drow["Valuation_Current_Market_Value"].ToString();
        txtLCurrentTotalValue.Text = drow["tot"].ToString();
        txtLValuationDate.Text = drow["Valuation_Date"].ToString();
        txtLRemarks.Text = drow["Valuation_Remarks"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyL.Enabled = false;
        }
        else
        {
            btnModifyL.Enabled = true;
        }

    }

    private void FunLowLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["LowLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblLSlNo.Text) - 1];
            drow.BeginEdit();
            drow["Valuation_Current_Market_Value"] = txtLCurrentValuePerUnit.Text.Trim();
            drow["tot"] = txtLCurrentTotalValue.Text.Trim();
            drow["Valuation_Date"] = txtLValuationDate.Text.Trim();
            drow["Valuation_Remarks"] = txtLRemarks.Text.Trim();
            drow.EndEdit();


            ViewState["LowLiq"] = gvLowLiqDetails1.DataSource = dtSecurities;
            gvLowLiqDetails1.DataBind();

            FunClearLDetails();

            btnModifyL.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearLDetails()
    {
        lblLSlNo.Text =
        txtLCollSecurities.Text =
        txtLValuationDate.Text =
        txtLRemarks.Text =
        txtLLocationDetails.Text =
        txtLMeasurement.Text =
        txtLExtent.Text =
        txtLUnitRate.Text =
        txtLCurrentTotalValue.Text =
        txtLCurrentValuePerUnit.Text = "";

        btnModifyL.Enabled = false;
        btnClearL.Enabled = true;
        FunPriResetRdButton(gvLowLiqDetails1, -1);

    }
    protected void btnClearL_Click(object sender, EventArgs e)
    {
        FunClearLDetails();
        btnModifyL.Enabled = false;
    }


    // Commodities

    protected void btnModifyC_Click(object sender, EventArgs e)
    {
        FunCommLIQSecuritiesGridViewRowUpdating();
    }

    protected void rdCSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvCommoDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["CommLiq"];

        FunPriResetRdButton(gvCommoDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];
        lblCSlNo.Text = drow["SlNo"].ToString();
        txtCCollSecurities.Text = drow["Collateral_Securities_Name"].ToString();
        txtCDescription.Text = drow["Description"].ToString();
        txtCUnitOfMeasure.Text = drow["Unit_Of_Measure"].ToString();
        txtCUnitQty.Text = drow["Unit_Quantity"].ToString();
        txtCUnitPrice.Text = drow["Unit_Price"].ToString();
        txtCCurrentValuePerUnit.Text = drow["Valuation_Current_Market_Value"].ToString();
        txtCCurrentTotalValue.Text = drow["tot"].ToString();
        txtCValuationDate.Text = drow["Valuation_Date"].ToString();
        txtCRemarks.Text = drow["Valuation_Remarks"].ToString();
                            
        if (PageMode == PageModes.Query)
        {
            btnModifyC.Enabled = false;
        }
        else
        {
            btnModifyC.Enabled = true;
        }

    }

    private void FunCommLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["CommLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblCSlNo.Text) - 1];
            drow.BeginEdit();
            drow["Valuation_Current_Market_Value"] = txtCCurrentValuePerUnit.Text.Trim();
            drow["tot"] = txtCCurrentTotalValue.Text.Trim();
            drow["Valuation_Date"] = txtCValuationDate.Text.Trim();
            drow["Valuation_Remarks"] = txtCRemarks.Text.Trim();
            drow.EndEdit();


            ViewState["LowLiq"] = gvCommoDetails1.DataSource = dtSecurities;
            gvCommoDetails1.DataBind();

            FunClearCDetails();

            btnModifyL.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearCDetails()
    {
        lblCSlNo.Text =
        txtCCollSecurities.Text =
        txtCDescription.Text =
        txtCUnitOfMeasure.Text =
        txtCUnitQty.Text =
        txtCUnitPrice.Text =
        txtCCurrentValuePerUnit.Text =
        txtCCurrentTotalValue.Text =
        txtCValuationDate.Text =
        txtCRemarks.Text = "";
      
        btnModifyC.Enabled = false;
        btnClearC.Enabled = true;
        FunPriResetRdButton(gvCommoDetails1, -1);

    }
    protected void btnClearC_Click(object sender, EventArgs e)
    {
        FunClearCDetails();
        btnModifyC.Enabled = false;
    }

    // Comm end

    //Fin 

    protected void btnModifyF_Click(object sender, EventArgs e)
    {
        FunFinLIQSecuritiesGridViewRowUpdating();
    }

    protected void rdFSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvFinDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["FinLiq"];

        FunPriResetRdButton(gvFinDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];
        lblFSlNo.Text = drow["SlNo"].ToString();
        txtFCollSecurities.Text = drow["Collateral_Securities_Name"].ToString();
        txtFInsuranceIssuedBy.Text = drow["Insurance_Issued_By"].ToString();
        txtFPolicyNo.Text = drow["Policy_No"].ToString();
        txtFPolicyValue.Text = drow["Policy_Value"].ToString();
        txtFMaturityDate.Text = drow["Maturity_Date"].ToString();
        txtFCurrentMarketValue.Text = drow["Valuation_Current_Market_Value"].ToString();
        txtFValuationDate.Text = drow["Valuation_Date"].ToString();
        txtFRemarks.Text = drow["Valuation_Remarks"].ToString();

       

        if (PageMode == PageModes.Query)
        {
            btnModifyF.Enabled = false;
        }
        else
        {
            btnModifyF.Enabled = true;
        }

    }

    private void FunFinLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["FinLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblFSlNo.Text) - 1];
            drow.BeginEdit();
            drow["Valuation_Current_Market_Value"] = txtFCurrentMarketValue.Text.Trim();
            drow["Valuation_Date"] = txtFValuationDate.Text.Trim();
            drow["Valuation_Remarks"] = txtFRemarks.Text.Trim();
            drow.EndEdit();


            ViewState["FinLiq"] = gvFinDetails1.DataSource = dtSecurities;
            gvFinDetails1.DataBind();

            FunClearFDetails();

            btnModifyF.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearFDetails()
    {
        lblFSlNo.Text =
        txtFCollSecurities.Text =
        txtFInsuranceIssuedBy.Text =
        txtFMaturityDate.Text = 
        txtFPolicyNo.Text = 
        txtFPolicyValue.Text = 
        txtFCurrentMarketValue.Text =
        txtFValuationDate.Text =
        txtFRemarks.Text = "";

        btnModifyF.Enabled = false;
        btnClearF.Enabled = true;
        FunPriResetRdButton(gvFinDetails1, -1);

    }
    protected void btnClearF_Click(object sender, EventArgs e)
    {
        FunClearFDetails();
        btnModifyF.Enabled = false;
    }

    // end


    private void FunPriCLT_ValuationModify(string strCLT_CaptureID)
    {
      
        tcCollateralValuation.ActiveTabIndex = 1;
        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@Collateral_Capture_ID", strCLT_CaptureID);
        Procparam.Add("@Option", "3");
        DataTable dt = Utility.GetDefaultData("S3G_CLT_GetCustomerAccDetails", Procparam);
       if(dt.Rows .Count >0)
       {
           ViewState["Collateral_VAL_No"] = dt.Rows[0]["CVN"].ToString();
           PopulateCustomerDetails(dt.Rows [0]["ID"].ToString (),Convert .ToInt32 (dt.Rows [0]["Cby"]));
       }
    }


    protected StringBuilder FunPriGenerateXML(StringBuilder sbXML, GridView gvXML)
    {
        try
        {
            string strCMV = "";
            string strDate = "";
            string strRemarks = "";

            if (gvXML.Rows.Count > 0)
            {
                Label lblTotal = (Label)gvXML.FooterRow.FindControl("lblTotal");
                if (lblTotal != null)
                {
                    if (Convert.ToDecimal(lblTotal.Text) > 0)
                        hasValue = true;
                }
            }
            sbXML = new StringBuilder();
            sbXML.Append("<Root>");
            foreach (GridViewRow gvRow in gvXML.Rows)
            {

                Label lblUnitCMV = gvRow.FindControl("lblUnitCMV") as Label; //gvRow.Cells[5].FindControl("txtHigUnitCMV") as TextBox;
                Label lblValDate = gvRow.FindControl("lblValDate") as Label;
                Label lblRemarks = gvRow.FindControl("lblRemarks") as Label;
                Label lblCollateralTypeID = gvRow.FindControl("lblCollateralTypeID") as Label;

                //HiddenField hdnCType_ID = gvRow.FindControl("hdnCType_ID") as HiddenField;

                if (!string.IsNullOrEmpty(lblUnitCMV.Text))
                    strCMV = lblUnitCMV.Text;
                else
                    strCMV = "0";

                if (!string.IsNullOrEmpty(lblValDate.Text))
                    strDate = Utility.StringToDate(lblValDate.Text).ToString();
                else
                    strDate = null; //DateTime.Now.ToString();

                if (!string.IsNullOrEmpty(lblRemarks.Text))
                    strRemarks = lblRemarks.Text;
                else
                    strRemarks = "";

                sbXML.Append("<Details  CMVPerUnit='" + strCMV + "' Val_Date='" + strDate + "' ");
                sbXML.Append("Remarks = '" + strRemarks + "' CType_ID='" + lblCollateralTypeID.Text + "'");
                sbXML.Append(" />");

                //sbXML.Append("<Details  CMVPerUnit='" + txtUnitCMV.Text + "' Val_Date='" + txtValDate.Text + "' ");
                //sbXML.Append("Remarks = '" + txtRemarks.Text + "' CType_ID='" + hdnCType_ID.Value + "'");
                //sbXML.Append(" />");
            }
            sbXML.Append("</Root>");
            // lstXML.Add(sbXML);

        }

        catch (Exception ex)
        {
            cvCollateralValuation.ErrorMessage = "Unable to Save Security Details" + this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;
        }
       
       return sbXML;
     
    }

    protected void High_CMV(object sender, EventArgs e)
    {

        Calc_TotalMarketValue(sender, gvHighLiqDetails, "Current value per unit");
        //FunPriGenerateXML();
    }

    protected void Medium_CMV(object sender, EventArgs e)
    {
        Calc_TotalMarketValue(sender, gvMedLiqDetails, "Current Market value");

    }

    protected void Low_CMV(object sender, EventArgs e)
    {

        Calc_TotalMarketValue(sender, gvLowLiqDetails, "Current value per unit");
    }

    protected void Commodity_CMV(object sender, EventArgs e)
    {
        Calc_TotalMarketValue(sender, gvCommoDetails, "Current value per unit");

    }

    protected void Finaicial_CMV(object sender, EventArgs e)
    {
        Calc_TotalMarketValue(sender, gvFinDetails, "Current Market value");

    }

    protected void Calc_TotalMarketValue(object sender, GridView gv, string alt)
    {
        try
        {
            Label lblTotal = (Label)gv.FooterRow.FindControl("lblTotal");
           // HiddenField hid_GT = (HiddenField)gv.FooterRow.FindControl("hid_GT");

            TextBox txt = (TextBox)sender;
            GridViewRow grvData = (GridViewRow)txt.Parent.Parent;

            Label lblTotCMV = (Label)grvData.FindControl("lblTotCMV");
            TextBox txtUnitCMV = (TextBox)grvData.FindControl("txtUnitCMV");
            Label lblAVUnits = (Label)grvData.FindControl("lblAVUnits");
            TextBox txtValDate = (TextBox)grvData.FindControl("txtValDate");

           // Label lblHeader = (Label)gv.HeaderRow.FindControl("");

            //*************************//
            int intGPSPrefix = 0;
            int intGPSSuffix = 0;
            int pf =8;
            int sf = 4;
            //S3GSession S3GSession = new S3GSession();
            intGPSPrefix = ObjS3GSession.ProGpsPrefixRW;
            intGPSSuffix = ObjS3GSession.ProGpsSuffixRW;

            if (pf > intGPSPrefix)
            {
                pf = intGPSPrefix;
            }
            if (sf > intGPSSuffix)
            {
                sf = intGPSSuffix;
            }

            string s = ""; 
            s = txtUnitCMV.Text.Substring (0, Convert .ToDecimal ( txtUnitCMV .Text).ToString (Funsetsuffix ()) .IndexOf('.'));
            //***************************//

            if (s.Length > pf)
            {
                //Utility .FunShowAlertMsg (this .Page ,"Value precision should not exceed"+pf+"digits");
               // txtUnitCMV.Focus();
                if(lblTotCMV!=null )
                txtUnitCMV.Text = lblTotCMV.Text = 0.ToString(Funsetsuffix());
                else
                    txtUnitCMV.Text= 0.ToString(Funsetsuffix());
                decimal dec = 0;

                foreach (GridViewRow row in gv.Rows)
                {

                    Label lblTotCMV1 = (Label)row.FindControl("lblTotCMV");
                    if (lblTotCMV1 != null)
                    {
                        if (!string.IsNullOrEmpty(lblTotCMV1.Text))
                            dec = dec + Convert.ToDecimal(lblTotCMV1.Text);
                    }
                    else
                    {
                        TextBox txtUnitCMV1 = (TextBox)row.FindControl("txtUnitCMV");
                        if (!string.IsNullOrEmpty(txtUnitCMV1.Text))
                            dec = dec + Convert.ToDecimal(txtUnitCMV1.Text);
                    }

                }
                lblTotal.Text = dec.ToString(Funsetsuffix());
                txtUnitCMV.Focus();
                Utility.FunShowAlertMsg(this.Page, alt +" should not exceed "+ pf +" digits");
                return;
            }
            else
            {
                txtUnitCMV.Text = Convert.ToDecimal(txtUnitCMV.Text).ToString(Funsetsuffix());
                decimal i = 0;
                if (lblAVUnits != null)
                {
                    if ((!string.IsNullOrEmpty(txtUnitCMV.Text)) && (!string.IsNullOrEmpty(lblAVUnits.Text)))
                        lblTotCMV.Text = (Convert.ToDecimal(txtUnitCMV.Text) * Convert.ToDecimal(lblAVUnits.Text)).ToString(Funsetsuffix());
                    else
                    {
                        lblTotCMV.Text = i.ToString(Funsetsuffix());
                        //txtUnitCMV.Focus();
                    }
                }

               


                foreach (GridViewRow row in gv.Rows)
                {

                    Label lblTotCMV1 = (Label)row.FindControl("lblTotCMV");
                    if (lblTotCMV1 != null)
                    {
                        if (!string.IsNullOrEmpty(lblTotCMV1.Text))
                            i = i + Convert.ToDecimal(lblTotCMV1.Text);
                    }
                    else
                    {
                        TextBox txtUnitCMV1 = (TextBox)row.FindControl("txtUnitCMV");
                        if (!string.IsNullOrEmpty(txtUnitCMV1.Text))
                            i = i + Convert.ToDecimal(txtUnitCMV1.Text);
                    }

                }

                //hid_GT.Value =  
                lblTotal.Text = i.ToString(Funsetsuffix());
                txtValDate.Focus();
            }
        }
        catch (Exception ex)
        {
            cvCollateralValuation.ErrorMessage = "Unable to calculate Total Market Value "+ this.Page.Header.Title;
            cvCollateralValuation.IsValid = false;
        }
    }


  
    

    protected void GetCust(object sender, EventArgs e)
    {
        rbtAgent.Checked = false;
        arrSortCol[1] = hdnSearch.Value = "";
        TextBox txtSearch = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch1");
        txtSearch.Text = "";
        TextBox txtSearch2 = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch2");
        txtSearch2.Text = "";
        //FunProHeaderSearch(txtSearch, e);
        hdnSortDirection.Value = "";
        LinkButton lnkSort = (LinkButton)grvPaging.HeaderRow.FindControl("lnkbtnSort1");
        FunProSortingColumn(lnkSort, e);
        

        FunPriBindGrid();

    }

    protected void GetAgent(object sender, EventArgs e)
     {
           rbtCustomer.Checked = false;
           arrSortCol[1] = hdnSearch.Value = "";
           TextBox txtSearch = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch1");
           txtSearch.Text = "";
           TextBox txtSearch2 = (TextBox)grvPaging.HeaderRow.FindControl("txtHeaderSearch2");
           txtSearch2.Text = "";
           //FunProHeaderSearch(txtSearch, e);
           hdnSortDirection.Value = "";
           LinkButton lnkSort = (LinkButton)grvPaging.HeaderRow.FindControl("lnkbtnSort1");

           FunProSortingColumn(lnkSort, e);
          
           FunPriBindGrid();
     }



    protected void Move(object sender, EventArgs  e)
    {
       /// TextBox txt = (TextBox)sender;
        CheckBox chk = (CheckBox)sender;
        GridViewRow grvData = (GridViewRow)chk.Parent.Parent;

        HiddenField hidCLT_ID = (HiddenField)grvData.FindControl("hidCLT_ID");
        HiddenField hidCA_ID = (HiddenField)grvData.FindControl("hidCA_ID");

      //  HiddenField hidCLT_ID=grvPaging .Rows [e].FindControl ("hidCLT_ID") as HiddenField ;
        ViewState["CID"] = hidCLT_ID.Value;

        tcCollateralValuation.ActiveTabIndex = 1;
        SetTabIndexChanged();
        if (rbtCustomer .Checked ==true )
            PopulateCustomerDetails(hidCA_ID.Value.ToString(),1);
        if(rbtAgent .Checked ==true )
            PopulateCustomerDetails(hidCA_ID.Value.ToString(),2);


    }

    #region [Tab indexChange events]
    protected  void SetTabIndexChanged()
    {
        if (Request.QueryString["qsMode"] == "M" || Request.QueryString["qsMode"] == "Q")
        {
            tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = true ;
            tabCustORAgent.Enabled = false;
        }
        else
        {

            if (tcCollateralValuation.ActiveTab == tabCustORAgent)
            {
                tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = false;

            }
            else
            {
                tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = true;
                tabCustORAgent.Enabled = false;

            }

        }
    }


    protected void tcCollateralValuation_TabIndexChanged(object sender, EventArgs e)
    {

        //if (Convert .ToInt32 ( ViewState ["gvidx"]) ==2)
        //{
        //    Button btnUndo = (Button)gvHighLiqDetails.Rows[id].FindControl("btnUndo");
        //    FunPro_FindControls(btnUndo, e, 3);
        //}
        int id = Convert.ToInt32(ViewState["ID"]);            
        switch (Convert.ToInt32(ViewState["gvidx"]))
        {
            case 2:
                Button High = (Button)gvHighLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(High, 3);
                break;
            case 3:
                Button Medium = (Button)gvMedLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Medium, 3);
                break;
            case 4:
                Button Low = (Button)gvLowLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Low, 3);
                break;
            case 5:
                Button Commodity = (Button)gvCommoDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Commodity, 3);
                break;
            case 6:
                Button Finance = (Button)gvFinDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Finance, 3);
                break;
            default :
                break;
        }

           SetTabIndexChanged();
            if (tcCollateralValuation.ActiveTabIndex == 0)
            {
                FunPriBindGrid();
            }
     
    }


  

    #endregion


    protected string GVDateFormat(string val)
    {
        return Utility.StringToDate(val).ToString(strDateFormat);
        
    }

    private string Funsetsuffix()
    {

        int suffix = 1;

        // S3GSession ObjS3GSession = new S3GSession();
        suffix = ObjS3GSession.ProGpsSuffixRW;
        // suffix = 0;
        string strformat = "0.";
        for (int i = 1; i <= suffix; i++)
        {
            strformat += "0";
        }
        return strformat;
    }




    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AjaxControlToolkit.CalendarExtender calValDate = e.Row.FindControl("calValDate") as AjaxControlToolkit.CalendarExtender;
                if (calValDate != null)
                    calValDate.Format = strDateFormat;

               TextBox txtValDate = e.Row.FindControl("txtValDate") as TextBox;
                if (txtValDate != null)
                {
                    txtValDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtValDate.ClientID + "','" + strDateFormat + "',true,  false);");
                }
                if(PageMode == PageModes.Query)
                {
                    //txtValDate.Attributes.Add("readonly", "true");
                   // txtValDate.Attributes.Remove("onblur");
                }
                    //txtValDate.Attributes.Add("readonly", "true");

                Label lblUnitCMV = e.Row.FindControl("lblUnitCMV") as Label;
                Label lblAVUnits = e.Row.FindControl("lblAVUnits") as Label;
                Label lblTotCMV = e.Row.FindControl("lblTotCMV") as Label;
                TextBox txtRemarks = e.Row.FindControl("txtRemarks") as TextBox;
                // if(lblTotCMV.Text !="")
                //************//
                //if (txtUnitCMV != null)
                //{
                //    if (!string.IsNullOrEmpty(txtUnitCMV.Text))
                //    {
                //        txtUnitCMV.SetDecimalPrefixSuffix(8, 4, false, "Current Market Value Per Unit");
                //        txtUnitCMV.Focus();
                //    }
                //}
                //************//
                string strFieldName = "Collateral Value";
                //  txtUnitCMV.Attributes.Add("onchange", "javascript:return fnGetValue('" + lblAVUnits.ClientID + "','" + txtUnitCMV.ClientID + "','" + lblTotCMV.ClientID + "')");

                if (lblTotCMV != null)
                {
                    if (!string.IsNullOrEmpty(lblTotCMV.Text))
                    {
                        lblTotCMV.Text = Convert.ToDecimal(lblTotCMV.Text).ToString(Funsetsuffix());
                        ViewState["Total"] = Convert.ToDecimal(ViewState["Total"].ToString()) + Convert.ToDecimal(lblTotCMV.Text);
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = Convert.ToDecimal(ViewState["Total"].ToString()).ToString(Funsetsuffix());
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                ViewState["Total"] = "0";
            }

        }

        catch (Exception ex)
        {
            cvCollateralValuation.ErrorMessage = "Unable to bind the grid";
            cvCollateralValuation.IsValid = false;
        }
    }



     protected bool FunPro_FindControls(object sender, int idx)
    {
      
        Button btn = (Button)sender;
        
        GridView g = (GridView)btn.Parent.Parent.Parent.Parent;

        Label lblTotal = (Label)g.FooterRow.FindControl("lblTotal");
        HiddenField hid_GT = (HiddenField)g.FooterRow.FindControl("hid_GT");

        GridViewRow grvData = (GridViewRow)btn.Parent.Parent;
       
        Label lblUnitCMV = (Label)grvData.FindControl("lblUnitCMV");
        Label lblValDate = (Label)grvData.FindControl("lblValDate");
        Label lblRemarks = (Label)grvData.FindControl("lblRemarks");
        Label lblTotCMV = (Label)grvData.FindControl("lblTotCMV");
      
        TextBox txtUnitCMV = (TextBox)grvData.FindControl("txtUnitCMV");
        TextBox txtValDate = (TextBox)grvData.FindControl("txtValDate");
        TextBox txtRemarks = (TextBox)grvData.FindControl("txtRemarks");
        HiddenField hid_Tot = (HiddenField)grvData.FindControl("hid_Tot");

        Button btnChange = (Button)grvData.FindControl("btnChange");
        Button btnUpdate = (Button)grvData.FindControl("btnUpdate");
        Button btnUndo = (Button)grvData.FindControl("btnUndo");



       // Label lblTotal = (Label)gv.FooterRow.FindControl("lblTotal");
      //  HiddenField hid_GT = (HiddenField)gv.FooterRow.FindControl("hid_GT");

        if (idx == 1)
        {
            lblUnitCMV.Visible = lblValDate.Visible = lblRemarks.Visible = false;
            txtUnitCMV.Attributes.Add("style", "display:block");
            //txtUnitCMV.Visible =
                txtValDate.Visible = txtRemarks.Visible = true;
            hid_GT.Value = lblTotal.Text;

            btnUndo.Visible = btnUpdate.Visible = true;
            foreach (GridViewRow r in g.Rows)
            {
                Button btnChange1 = (Button)r.FindControl("btnChange");
                //if(btnChange ==btn)
                btnChange1.Enabled= false;
            }

            ViewState["ID"] = grvData.RowIndex;
            ViewState["gvidx"] = tcCollateralValuation.ActiveTabIndex;
            btnChange.Visible = false;
            
        }
        else if (idx == 2)
        {
            // {
            if (!string.IsNullOrEmpty(txtUnitCMV.Text))
            {
                if (Convert.ToDecimal(txtUnitCMV.Text) > 0)
                {
                    if (txtValDate.Text == "")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter Valuation Date");
                        return false ;
                    }
                                    
                }
            }

            if (!string.IsNullOrEmpty(txtValDate.Text))
            {

                if (Utility.StringToDate(txtValDate.Text) < Utility.StringToDate(txtCapDate.Text))
                {
                    Utility.FunShowAlertMsg(this.Page, "Valuation Date can not be lesser than Capture Date");
                    txtValDate.Text = DateTime.Now.ToString(strDateFormat);
                    return false;
                }
            }

            lblUnitCMV.Visible = lblValDate.Visible = lblRemarks.Visible = true;
            txtUnitCMV.Attributes.Add("style", "display:None");
            //  txtUnitCMV.Visible = 
                txtValDate.Visible = txtRemarks.Visible = false ;
                           
                    lblUnitCMV.Text = txtUnitCMV.Text;
                    lblValDate.Text = txtValDate.Text;
                    lblRemarks.Text = txtRemarks.Text;
                    if (hid_Tot != null && lblTotCMV != null)
                        hid_Tot.Value = lblTotCMV.Text;
                    hid_GT.Value = lblTotal.Text;

                    foreach (GridViewRow r in g.Rows)
                    {

                        Button btnChange1 = (Button)r.FindControl("btnChange");
                        //if(btnChange ==btn)
                        btnChange1.Enabled = true ;

                    }
            btnChange.Visible = true;
            btnUndo.Visible = false;
            btnUpdate.Visible = false;

        }
        else if (idx == 3)
        {
            txtUnitCMV.Text = lblUnitCMV.Text;
            txtValDate.Text = lblValDate.Text;
            txtRemarks.Text = lblRemarks.Text;

            if (hid_Tot != null && lblTotCMV != null)
            {
                decimal d1 = 0;
                  if (!string.IsNullOrEmpty(hid_Tot.Value))
                    {
                        d1 = Convert.ToDecimal(hid_Tot.Value);
                        lblTotCMV.Text = d1.ToString(Funsetsuffix());
                    }
               


               // lblTotCMV.Text = hid_Tot.Value;
            }
            lblTotal.Text = hid_GT.Value;
            lblUnitCMV.Visible = lblValDate.Visible = lblRemarks.Visible = true;
            txtUnitCMV.Attributes.Add("style", "display:None");
            //  txtUnitCMV.Visible = 
            txtValDate.Visible = txtRemarks.Visible = false;

            foreach (GridViewRow r in g.Rows)
            {

                Button btnChange1 = (Button)r.FindControl("btnChange");
                //if(btnChange ==btn)
                btnChange1.Enabled = true ;

            }
            btnChange.Visible = true;
            btnUpdate.Visible = false;
            btnUndo.Visible = false;

            ViewState["gvidx"] = null;
            ViewState["ID"] = null;
        }
        txtUnitCMV.Style.Add("text-align", "right");
        txtRemarks.MaxLength = 10;
        return true;
    }

   
    protected void btnChange_Click(object sender, EventArgs e)
     {
         if (!FunPro_FindControls(sender, 1))
             return;
        
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!FunPro_FindControls(sender, 2))
            return;
        
    }
    protected void btnUndo_Click(object sender, EventArgs e)
    {
       if (!FunPro_FindControls(sender, 3))
            return;
    }
    protected void txthCurrentValue_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txthCurrentValue.Text) && !string.IsNullOrEmpty(txthAvailableUnits.Text))
        {
            txthCurrentTotalValue.Text = (Convert.ToDecimal(txthCurrentValue.Text) * Convert.ToDecimal(txthAvailableUnits.Text)).ToString();
        }
        else
        {
            txthCurrentTotalValue.Text = string.Empty;
        }

    }
    protected void txtLCurrentValueUnit_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtLCurrentValuePerUnit.Text) && !string.IsNullOrEmpty(txtLMeasurement.Text))
        {
            txtLCurrentTotalValue.Text = (Convert.ToDecimal(txtLCurrentValuePerUnit.Text) * Convert.ToDecimal(txtLMeasurement.Text)).ToString();
        }
        else
        {
            txtLCurrentTotalValue.Text = string.Empty;
        }

    }

    protected void txtCCurrentValueUnit_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCCurrentValuePerUnit.Text) && !string.IsNullOrEmpty(txtCUnitQty.Text))
        {
            txtCCurrentTotalValue.Text = (Convert.ToDecimal(txtCCurrentValuePerUnit.Text) * Convert.ToDecimal(txtCUnitQty.Text)).ToString();
        }
        else
        {
            txtCCurrentTotalValue.Text = string.Empty;
        }

    }
}
