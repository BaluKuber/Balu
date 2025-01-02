#region PageHeader
/// © 2010 
/// SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Collateral
/// Screen Name         :   Collateral Storage
/// Created By          :   MANI KANDAN. R
/// Created Date        :   19-May-2011
/// Purpose             :   To Know the Collateral Storing point
/// 
/// <Program Summary>
#endregion





#region Name Space
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


public partial class Collateral_S3G_CLTCollateralStorage : ApplyThemeForProject

{
    #region Common Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    string _DateFormat = "dd/MM/yyyy";
    string strDateFormat = string.Empty;
    string strApprLogicID = "0";
    int intCollateralStorage_ID = 0;
    int intErrCode = 0;
    string s = "";
    UserInfo ObjUserInfo = new UserInfo();
 

    SerializationMode SerMode = SerializationMode.Binary;
    S3GSession ObjS3GSession;
    //S3GAdminServicesReference.S3GAdminServicesClient objS3GAdminServicesClient = new S3GAdminServicesReference.S3GAdminServicesClient();
    List<StringBuilder> lstXML = new List<StringBuilder>();
    DataSet ds = null;
    DataSet dsm = null;
    CollateralMgtServicesReference.CollateralMgtServicesClient objCollateralStorage;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsDataTable obj_CLT_StorageDataTable = null;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsRow objS3G_CLT_StorageRow = null;


    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Collateral/S3GCLTTransLander.aspx?Code=KST";
    string strRedirectPageAdd = "window.location.href='../Collateral/S3G_CLTCollateralStorage.aspx';";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTTransLander.aspx?Code=KST';";
    string strPageName = "Collateral Storage";
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    public int tempColl_ID;
    //Code end
    int boolx;
    int intNoofSearch = 2;
    string[] arrSortCol = new string[] { "Collateral_Tran_No", "CM.Customer_Name" };
    string strProcName = "S3G_CLT_TransLanderCollateralStorage";

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
            Procparam.Add("@Option", "2");
            if (rbtCustomer.Checked == true)
                Procparam.Add("@Param", "1");
            if (rbtAgent.Checked == true)
                Procparam.Add("@Param", "2");
          


            grvPaging.BindGridView("S3G_CLT_GetCustomerORAgentCode", Procparam, out intTotalRecords, ObjPaging, out bIsNewRow);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvPaging.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            //FunProLoadAddressCombos();

            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);
            DataTable dtSource = new DataTable();
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }

            ViewState["dtCity"] = dtSource;

            //Paging Config End
        }
            
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
            ClsPubCommErrorLogDB.CustomErrorRoutine(objFaultExp);
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
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
                    strSearchVal += " and " + arrSortCol[iCount].ToString() + " like  '%" + arrSearchVal[iCount].ToString() + "%'";
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
                strApprLogicID = fromTicket.Name;
               
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


            //SetTabIndexChanged();


            if (!IsPostBack)
            {
                if (PageMode == PageModes.Create)
                {
                    FunPriBindGrid();
                }
                if (PageMode != PageModes.Query)
                {
                    FunProLoadAddressCombos1();
                }
            
                if (Request.QueryString["qsMode"] == "Q")
                {
                    FunPriCollValuationModify(strApprLogicID);
                    FunPriDisableControls(-1);
                }
                else if (Request.QueryString["qsMode"] == "M")
                {
                    FunPriCollValuationModify(strApprLogicID);
                    FunPriDisableControls(1);
                  

                }
                else
                {
                    FunPriDisableControls(0);
                }
            }
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
        //if (strMode == "C")
        //{
        //    tabCustORAgent.Enabled = true;
        //    if (tcCollateralValuation.ActiveTabIndex == 0)

        //        Response.Redirect(strRedirectPage,false);
        //    else

        //    tcCollateralValuation.ActiveTabIndex = 0;
        //    SetTabIndexChanged();
        //    FunPriBindGrid();
        //    //Response.Redirect(strRedirectPage);
        //}

        //else
        //{
            Response.Redirect(strRedirectPage,false);

        //}
       // Response.Redirect(strRedirectPage,false);
    }
    //clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriClearPage();
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
        objCollateralStorage = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {

            obj_CLT_StorageDataTable = new CollateralMgtServices.S3G_CLT_StorageDetailsDataTable();
            objS3G_CLT_StorageRow = null;
            objS3G_CLT_StorageRow = obj_CLT_StorageDataTable.NewS3G_CLT_StorageDetailsRow();
            objS3G_CLT_StorageRow.Company_ID = intCompanyID;
            if (PageMode == PageModes.Create)
                objS3G_CLT_StorageRow.Collateral_Capture_ID = ViewState["CID"].ToString();
            else
                objS3G_CLT_StorageRow.Collateral_Capture_ID = Convert.ToInt32(hdnCollateralCapture.Text).ToString();
            objS3G_CLT_StorageRow.Collateral_Storage_Date = DateTime.Now;
            objS3G_CLT_StorageRow.Collateral_Storage_ID = intCollateralStorage_ID;
            if (PageMode == PageModes.Modify)
                objS3G_CLT_StorageRow.Collateral_Storage_ID = Convert.ToInt32(strApprLogicID);
            objS3G_CLT_StorageRow.Is_Active = ChkActive.Checked;
            StringBuilder sbXML = new StringBuilder();
            
            objS3G_CLT_StorageRow.XmlHighLiquiditySecurity = FunPriGenerateXML(sbXML, gvHighLiqDetails1).ToString(); //gvHighLiqDetails.FunPubFormXml();
            objS3G_CLT_StorageRow.XmlMediumLiquiditySecurity = FunPriGenerateXML(sbXML, gvMedLiqDetails1).ToString(); //gvMedLiqDetails.FunPubFormXml();
            objS3G_CLT_StorageRow.XmlLowLiquiditySecurity = FunPriGenerateXML(sbXML, gvLowLiqDetails1).ToString(); //gvLowLiqDetails.FunPubFormXml();
            objS3G_CLT_StorageRow.XmlCommoditySecurity = FunPriGenerateXML(sbXML, gvCommoDetails1).ToString(); //gvCommoDetails.FunPubFormXml();
            objS3G_CLT_StorageRow.XmlFinancialSecuritySecurity = FunPriGenerateXML(sbXML, gvFindetails1).ToString();//
            objS3G_CLT_StorageRow.Created_By = ObjUserInfo.ProUserIdRW;
            objS3G_CLT_StorageRow.Created_On = DateTime.Now;
            objS3G_CLT_StorageRow.Modified_By = ObjUserInfo.ProUserIdRW;
            objS3G_CLT_StorageRow.Modified_On = DateTime.Now;
           
            obj_CLT_StorageDataTable.AddS3G_CLT_StorageDetailsRow(objS3G_CLT_StorageRow);
            int x, y, z, a, b;
            // To check the Grid is Empty
            x = FunPriFindEmpty(gvHighLiqDetails1);
            y = FunPriFindEmpty(gvMedLiqDetails1);
            z = FunPriFindEmpty(gvLowLiqDetails1);
            a = FunPriFindEmpty(gvCommoDetails1);
            b = FunPriFindEmpty(gvFindetails1);
            if ((x == 0) && (y == 0) && (z == 0) && (a == 0) && (b == 0))
            {
                Utility.FunShowAlertMsg(this.Page, "Give atleast one Storage Details");
                return;
            }
         
           intErrCode = objCollateralStorage.FunPubCollateralStorage(out strApprLogicID, SerMode, ClsPubSerialize.Serialize(obj_CLT_StorageDataTable, SerMode));

           
               if (intErrCode == -1)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoNotDefined);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        return;
                    }
                    else if (intErrCode == -2)
                    {
                        strAlert = strAlert.Replace("__ALERT__", Resources.LocalizationResources.DocNoExceeds);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        return;
                    }

               if (intErrCode == 50)
                 {
                     cvCollateralValuation.ErrorMessage = "Error in Saving";
                     cvCollateralValuation.IsValid = false;
                     return;                   
                 }
                           
            
          
                if (PageMode == PageModes.Modify)
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, "alert('Collateral Storage Details updated successfully');" + strRedirectPageView, true);
                    
                }

                else
                {
                    //Added by Bhuvana  on 24/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here

                    strAlert = "Collateral Storage " + strApprLogicID + " added successfully";
                    strAlert += @"\n\nWould you like to add one more record?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }


            }
        

        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
        finally
        {
            //if (objCollateralStorage != null)
                objCollateralStorage.Close();
        }
    }
    #endregion

    #region To clear the Data in the Page
    private void FunPriClearPage()
    {
        try
        {
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



    #region "User Authorization"
    //This is used to implement User Authorization
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                tcCollateralValuation.ActiveTabIndex = 0;
                ChkActive.Enabled = false;
                ChkActive.Checked = true;
                if (tcCollateralValuation.ActiveTabIndex == 0)
                {
                    tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = false;
                    //btnSave.Visible = false;
                    //btnClear.Visible = false;
                    //btnCancel.Visible = false;

                }
                else
                {
                    btnSave.Visible = true;
                    btnClear.Visible = true;
                    btnCancel.Visible = true;
                }
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                txtCollateralDate.Text = DateTime.Now.ToString(strDateFormat);
                break;

            case 1: // Modify Mode

                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                btnClear.Enabled = false;
                tcCollateralValuation.ActiveTabIndex = 1;
                tabCustORAgent.Enabled = false;
                
                                //tcCollateralValuation.

                //chkActive.Enabled = true;
                if (!bModify)
                {

                }


                break;

            case -1:// Query Mode
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                ChkActive.Enabled = false;
                btnSave.Enabled = btnClear.Enabled = false;
                btnClear.Enabled = false;
                tcCollateralValuation.ActiveTabIndex = 1;
                tabCustORAgent.Enabled = false;
                gvHighLiqDetails.Columns[9].Visible = false;
                gvMedLiqDetails.Columns[9].Visible = false;
                gvLowLiqDetails.Columns[9].Visible = false;
                gvCommoDetails.Columns[9].Visible = false;
                gvFinDetails.Columns[9].Visible = false;

                if (!bQuery)
                {
                    Response.Redirect(strRedirectPage,false);
                }
                txthRemarks.ReadOnly = txthStorage1.ReadOnly = txthStorage2.ReadOnly = txthStorage3.ReadOnly = txthAddress.ReadOnly = true;
                txtMRemarks.ReadOnly = txtMStorage1.ReadOnly = txtMStorage2.ReadOnly = txtMStorage3.ReadOnly = txtMAddress.ReadOnly = true;
                txtLRemarks.ReadOnly = txtLStorage1.ReadOnly = txtLStorage2.ReadOnly = txtLStorage3.ReadOnly = txtLAddress.ReadOnly = true;
                txtCRemarks.ReadOnly = txtCStorage1.ReadOnly = txtCStorage2.ReadOnly = txtCStorage3.ReadOnly = txtCAddress.ReadOnly = true;
                txtFRemarks.ReadOnly = txtFStorage1.ReadOnly = txtFStorage2.ReadOnly = txtFStorage3.ReadOnly = txtFAddress.ReadOnly = true;

                // txthCity.Enabled = txtMCity.Enabled = txtLCity.Enabled = false;
                // txtLCity.Attributes.Add("readonly","readonly");



                btnClearH.Enabled = btnClearM.Enabled = btnClearL.Enabled  = btnClearC.Enabled = btnClearF.Enabled = false;


                break;
        }
    }
    //Code end
    #endregion




    private void PopulateCustomerDetails(string hidCA_ID)
    {
        try
        {
            btnSave.Visible = true;
            btnClear.Visible = true;
            btnCancel.Visible = true;
            bool bCLT_Available = false;

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@CA_ID", Convert.ToInt32(hidCA_ID).ToString());
            if(rbtCustomer.Checked == true)
            Procparam.Add("@Option", "1");
            else
                Procparam.Add("@Option", "2");
            DataSet dsCustAndAccDetails = Utility.GetDataset("S3G_CLT_GetCustomerAgentDetailsCollateralStorage", Procparam);

            if (dsCustAndAccDetails.Tables[0].Rows.Count > 0)
                S3GCustomerAddress1.SetCustomerDetails(dsCustAndAccDetails.Tables[0].Rows[0], true);
           
           
            Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Collateral_Capture_ID", Convert.ToInt32(tempColl_ID).ToString());
            ds = Utility.GetDataset("S3G_CLT_GetCollateralCaptureDetails", Procparam);
            if (rbtAgent.Checked)
            {
                S3GCustomerAddress1.Caption = "Collection Agent";
                pnlHeader.GroupingText = "Collection Agent Details";

            }

            // High Liquid Securities
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["DT_HighLiq"] = ds.Tables[0];
                lblHighLiqDetails.Visible = false;
                ViewState["HighLiq"] = gvHighLiqDetails.DataSource = gvHighLiqDetails1.DataSource = ds.Tables[0];
                gvHighLiqDetails.DataBind();
                gvHighLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                //lblHighLiqDetails.Visible = true;
                gvHighLiqDetails.DataSource = null;
                gvHighLiqDetails.DataBind();

                tcCollateralValuation.Tabs[2].Visible = false;

            }

            // Medium Liquid Securities
            if (ds.Tables[1].Rows.Count > 0)
            {

                lblMedLiqDetails.Visible = false;
                ViewState["MedLiq"] = gvMedLiqDetails.DataSource = gvMedLiqDetails1.DataSource = ds.Tables[1];
                gvMedLiqDetails.DataBind();
                gvMedLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                //lblMedLiqDetails.Visible = true;
                gvMedLiqDetails.DataSource = null;
                gvMedLiqDetails.DataBind();

                tcCollateralValuation.Tabs[3].Visible = false;


            }

            // Low Liquid Securities
            if (ds.Tables[2].Rows.Count > 0)
            {

                //pnlLowLiqDetails.Visible = true;
                lblLowLiqDetails.Visible = false;
                ViewState["LowLiq"] = gvLowLiqDetails.DataSource = gvLowLiqDetails1.DataSource = ds.Tables[2];
                gvLowLiqDetails.DataBind();
                gvLowLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                //lblLowLiqDetails.Visible = true;
                gvLowLiqDetails.DataSource = null;
                gvLowLiqDetails.DataBind();

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
                bCLT_Available = true;
            }
            else
            {
                //lblCommodityDetails.Visible = true;
                gvCommoDetails.DataSource = null;
                gvCommoDetails.DataBind();

                tcCollateralValuation.Tabs[5].Visible = false;

            }


            // Financial Securities
            if (ds.Tables[4].Rows.Count > 0)
            {
                lblFinDetails.Visible = false;
                ViewState["FinLiq"] = gvFinDetails.DataSource = gvFindetails1.DataSource = ds.Tables[4];
                gvFinDetails.DataBind();
                gvFindetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                //lblFinDetails.Visible = true;
                gvFinDetails.DataSource = null;
                gvFinDetails.DataBind();

                tcCollateralValuation.Tabs[6].Visible = false;

            }

            if (bCLT_Available == false)
            {
                Utility.FunShowAlertMsg(this.Page, "No Collateral Details are Available");
                return;
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw (ex);
        }
    }
    // High liquid
    protected void rdHSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvHighLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["HighLiq"];

        FunPriResetRdButton(gvHighLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblhSlNo.Text = drow["SlNo"].ToString();
        txthCollRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txthCollSecurities.Text = drow["Description"].ToString();
        txthRemarks.Text = drow["Remarks"].ToString();

        if (!txthCity.Items.Contains(new ListItem(drow["City"].ToString(), drow["City"].ToString())))
        {
            txthCity.Items.Add(new ListItem(drow["City"].ToString(), drow["City"].ToString()));
        }

        txthCity.SelectedItem.Text = drow["City"].ToString();

        if (PageMode == PageModes.Query)
        {
            txthCity.ClearDropDownList();
            txthCity.Enabled = false;
        }

        //txthCity.SelectedValue = drow["City"].ToString();
        txthAddress.Text = drow["Address"].ToString();
        txthStorage1.Text = drow["Storage1"].ToString();
        txthStorage2.Text = drow["Storage2"].ToString();
        txthStorage3.Text = drow["Storage3"].ToString();

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

            drow["Remarks"] = txthRemarks.Text.Trim();
            drow["City"] = txthCity.SelectedItem.Text;
            drow["Address"] = txthAddress.Text.Trim();
            drow["Storage1"] = txthStorage1.Text.Trim();
            drow["Storage2"] = txthStorage2.Text.Trim();
            drow["Storage3"] = txthStorage3.Text.Trim();
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
        txthCollRefNo.Text = "";
        txthCollSecurities.Text = "";
        txthRemarks.Text = "";
        txthCity.SelectedItem.Text = "";
        txthStorage1.Text = "";
        txthStorage2.Text = "";
        txthStorage3.Text = "";
        txthAddress.Text = "";


        btnModifyH.Enabled = false;
        btnClearH.Enabled = true;
        FunPriResetRdButton(gvHighLiqDetails1, -1);

    }
    protected void btnClearH_Click(object sender, EventArgs e)
    {
        FunClearHDetails();
        btnModifyH.Enabled = false;
    }
    // End 
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

    // Medium Liquid
    protected void rdMSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvMedLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["MedLiq"];

        FunPriResetRdButton(gvMedLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblMSlNo.Text = drow["SlNo"].ToString();
        txtMCollRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txtMCollSecurities.Text = drow["Description"].ToString();
        txtMRemarks.Text = drow["Remarks"].ToString();

        if (!txtMCity.Items.Contains(new ListItem(drow["City"].ToString(), drow["City"].ToString())))
        {
            txtMCity.Items.Add(new ListItem(drow["City"].ToString(), drow["City"].ToString()));
        }

        txtMCity.SelectedItem.Text = drow["City"].ToString();

        if (PageMode == PageModes.Query)
        {
            txtMCity.ClearDropDownList();
            txtMCity.Enabled = false;
        }

        //txtMCity.SelectedValue = drow["City"].ToString();
        txtMAddress.Text = drow["Address"].ToString();
        txtMStorage1.Text = drow["Storage1"].ToString();
        txtMStorage2.Text = drow["Storage2"].ToString();
        txtMStorage3.Text = drow["Storage3"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyM.Enabled = false;
        }
        else
        {
            btnModifyM.Enabled = true;
        }

    }
    protected void btnModifyM_Click(object sender, EventArgs e)
    {
        FunMedLIQSecuritiesGridViewRowUpdating();
    }
    private void FunMedLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["MedLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblMSlNo.Text) - 1];
            drow.BeginEdit();

            drow["Remarks"] = txtMRemarks.Text.Trim();
            drow["City"] = txtMCity.SelectedItem.Text;
            drow["Address"] = txtMAddress.Text.Trim();
            drow["Storage1"] = txtMStorage1.Text.Trim();
            drow["Storage2"] = txtMStorage2.Text.Trim();
            drow["Storage3"] = txtMStorage3.Text.Trim();
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
        lblMSlNo.Text = "";
        txtMCollRefNo.Text = "";
        txtMCollSecurities.Text = "";
        txtMRemarks.Text = "";
        txtMCity.SelectedItem.Text = "";
        txtMStorage1.Text = "";
        txtMStorage2.Text = "";
        txtMStorage3.Text = "";
        txtMAddress.Text = "";


        btnModifyM.Enabled = false;
        btnClearM.Enabled = true;
        FunPriResetRdButton(gvMedLiqDetails1, -1);

    }
    protected void btnClearM_Click(object sender, EventArgs e)
    {
        FunClearMDetails();
        btnModifyM.Enabled = false;
    }
    // End 

    // Low Liquid
    protected void rdLSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvLowLiqDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["LowLiq"];

        FunPriResetRdButton(gvLowLiqDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblLSlNo.Text = drow["SlNo"].ToString();
        txtLCollRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txtLCollSecurities.Text = drow["Description"].ToString();
        txtLRemarks.Text = drow["Remarks"].ToString();
        if (!txtLCity.Items.Contains(new ListItem(drow["City"].ToString(), drow["City"].ToString())))
        {
            txtLCity.Items.Add(new ListItem(drow["City"].ToString(), drow["City"].ToString()));
        }

        txtLCity.SelectedItem.Text = drow["City"].ToString();

        if (PageMode == PageModes.Query)
        {
            txtLCity.ClearDropDownList();
            txtLCity.Enabled = false;
        }


        //txtLCity.SelectedValue = drow["City"].ToString();
        txtLAddress.Text = drow["Address"].ToString();
        txtLStorage1.Text = drow["Storage1"].ToString();
        txtLStorage2.Text = drow["Storage2"].ToString();
        txtLStorage3.Text = drow["Storage3"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyL.Enabled = false;
        }
        else
        {
            btnModifyL.Enabled = true;
        }

    }
    protected void btnModifyL_Click(object sender, EventArgs e)
    {
        FunLowLIQSecuritiesGridViewRowUpdating();
    }
    private void FunLowLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["LowLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblLSlNo.Text) - 1];
            drow.BeginEdit();

            drow["Remarks"] = txtLRemarks.Text.Trim();
            drow["City"] = txtLCity.SelectedItem.Text;
            drow["Address"] = txtLAddress.Text.Trim();
            drow["Storage1"] = txtLStorage1.Text.Trim();
            drow["Storage2"] = txtLStorage2.Text.Trim();
            drow["Storage3"] = txtLStorage3.Text.Trim();
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
        lblLSlNo.Text = "";
        txtLCollRefNo.Text = "";
        txtLCollSecurities.Text = "";
        txtLRemarks.Text = "";
        txtLCity.SelectedItem.Text = "";
        txtLStorage1.Text = "";
        txtLStorage2.Text = "";
        txtLStorage3.Text = "";
        txtLAddress.Text = "";


        btnModifyL.Enabled = false;
        btnClearL.Enabled = true;
        FunPriResetRdButton(gvLowLiqDetails1, -1);

    }
    protected void btnClearL_Click(object sender, EventArgs e)
    {
        FunClearLDetails();
        btnModifyL.Enabled = false;
    }
    // End 


    // Commodities Liquid
    protected void rdCSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvCommoDetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["CommLiq"];

        FunPriResetRdButton(gvCommoDetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblCSlNo.Text = drow["SlNo"].ToString();
        txtCCollRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txtCCollSecurities.Text = drow["Description"].ToString();
        txtCRemarks.Text = drow["Remarks"].ToString();
        if (!txtCCity.Items.Contains(new ListItem(drow["City"].ToString(), drow["City"].ToString())))
        {
            txtCCity.Items.Add(new ListItem(drow["City"].ToString(), drow["City"].ToString()));
        }

        txtCCity.SelectedItem.Text = drow["City"].ToString();

        if (PageMode == PageModes.Query)
        {
            txtCCity.ClearDropDownList();
            txtCCity.Enabled = false;
        }


        //txtLCity.SelectedValue = drow["City"].ToString();
        txtCAddress.Text = drow["Address"].ToString();
        txtCStorage1.Text = drow["Storage1"].ToString();
        txtCStorage2.Text = drow["Storage2"].ToString();
        txtCStorage3.Text = drow["Storage3"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyC.Enabled = false;
        }
        else
        {
            btnModifyC.Enabled = true;
        }

    }
    protected void btnModifyC_Click(object sender, EventArgs e)
    {
        FunCommLIQSecuritiesGridViewRowUpdating();
    }
    private void FunCommLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["CommLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblCSlNo.Text) - 1];
            drow.BeginEdit();

            drow["Remarks"] = txtCRemarks.Text.Trim();
            drow["City"] = txtCCity.SelectedItem.Text;
            drow["Address"] = txtCAddress.Text.Trim();
            drow["Storage1"] = txtCStorage1.Text.Trim();
            drow["Storage2"] = txtCStorage2.Text.Trim();
            drow["Storage3"] = txtCStorage3.Text.Trim();
            drow.EndEdit();


            ViewState["CommLiq"] = gvCommoDetails1.DataSource = dtSecurities;
            gvCommoDetails1.DataBind();

            FunClearCDetails();

            btnModifyC.Enabled = false;

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private void FunClearCDetails()
    {
        lblCSlNo.Text = "";
        txtCCollRefNo.Text = "";
        txtCCollSecurities.Text = "";
        txtCRemarks.Text = "";
        txtCCity.SelectedItem.Text = "";
        txtCStorage1.Text = "";
        txtCStorage2.Text = "";
        txtCStorage3.Text = "";
        txtCAddress.Text = "";


        btnModifyC.Enabled = false;
        btnClearC.Enabled = true;
        FunPriResetRdButton(gvCommoDetails1, -1);

    }
    protected void btnClearC_Click(object sender, EventArgs e)
    {
        FunClearCDetails();
        btnModifyC.Enabled = false;
    }
    // End 

    // Financial Liquid
    protected void rdFSelect_CheckedChanged(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("gvFindetails1", ((RadioButton)sender).ClientID);
        DataTable dtSecurities = (DataTable)ViewState["FinLiq"];

        FunPriResetRdButton(gvFindetails1, intRowIndex);

        DataRow drow = dtSecurities.Rows[intRowIndex];

        lblFSlNo.Text = drow["SlNo"].ToString();
        txtFCollRefNo.Text = drow["Collateral_Item_Ref_No"].ToString();
        txtFCollSecurities.Text = drow["Description"].ToString();
        txtFRemarks.Text = drow["Remarks"].ToString();
        if (!txtFCity.Items.Contains(new ListItem(drow["City"].ToString(), drow["City"].ToString())))
        {
            txtFCity.Items.Add(new ListItem(drow["City"].ToString(), drow["City"].ToString()));
        }

        txtFCity.SelectedItem.Text = drow["City"].ToString();

        if (PageMode == PageModes.Query)
        {
            txtFCity.ClearDropDownList();
            txtFCity.Enabled = false;
        }


        //txtLCity.SelectedValue = drow["City"].ToString();
        txtFAddress.Text = drow["Address"].ToString();
        txtFStorage1.Text = drow["Storage1"].ToString();
        txtFStorage2.Text = drow["Storage2"].ToString();
        txtFStorage3.Text = drow["Storage3"].ToString();

        if (PageMode == PageModes.Query)
        {
            btnModifyF.Enabled = false;
        }
        else
        {
            btnModifyF.Enabled = true;
        }

    }
    protected void btnModifyF_Click(object sender, EventArgs e)
    {
        FunFinLIQSecuritiesGridViewRowUpdating();
    }
    private void FunFinLIQSecuritiesGridViewRowUpdating()
    {
        try
        {
            DataTable dtSecurities = (DataTable)ViewState["FinLiq"];
            DataRow drow = dtSecurities.Rows[Convert.ToInt32(lblFSlNo.Text) - 1];
            drow.BeginEdit();

            drow["Remarks"] = txtFRemarks.Text.Trim();
            drow["City"] = txtFCity.SelectedItem.Text;
            drow["Address"] = txtFAddress.Text.Trim();
            drow["Storage1"] = txtFStorage1.Text.Trim();
            drow["Storage2"] = txtFStorage2.Text.Trim();
            drow["Storage3"] = txtFStorage3.Text.Trim();
            drow.EndEdit();


            ViewState["FinLiq"] = gvFindetails1.DataSource = dtSecurities;
            gvFindetails1.DataBind();

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
        lblFSlNo.Text = "";
        txtFCollRefNo.Text = "";
        txtFCollSecurities.Text = "";
        txtFRemarks.Text = "";
        txtFCity.SelectedItem.Text = "";
        txtFStorage1.Text = "";
        txtFStorage2.Text = "";
        txtFStorage3.Text = "";
        txtFAddress.Text = "";


        btnModifyF.Enabled = false;
        btnClearF.Enabled = true;
        FunPriResetRdButton(gvFindetails1, -1);

    }
    protected void btnClearF_Click(object sender, EventArgs e)
    {
        FunClearFDetails();
        btnModifyF.Enabled = false;
    }
    // End 
    private void FunPriCollValuationModify(string strColval_ID)
    {
        try
        {
            bool bCLT_Available = false;

            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            //Customer or Agent 
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Collateral_Storage_ID", Convert.ToInt32(strApprLogicID).ToString());
            
            DataSet dscust = Utility.GetDataset("S3G_CLT_GetCollateralStorageCusDetails", Procparam);
            if((dscust.Tables[0].Rows[0]["ForCust"]).ToString() == (Convert.ToInt32(2)).ToString())
            {
                S3GCustomerAddress1.Caption = "Collection Agent";
                pnlHeader.GroupingText = "Collection Agent Details";
            }
            if (dscust.Tables[0].Rows.Count > 0)
            {
                S3GCustomerAddress1.SetCustomerDetails(dscust.Tables[0].Rows[0], true);               
            
            }
            
            // Collateral Storage Details
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            Procparam.Add("@Collateral_Storage_ID", Convert.ToInt32(strApprLogicID).ToString());
            dsm = Utility.GetDataset("S3G_CLT_GetCollateralStorageDetails", Procparam);

            if (dsm.Tables[0].Rows.Count > 0)
            {
                hdnCollateralCapture.Text = dsm.Tables[0].Rows[0]["Collateral_Capture_ID"].ToString();
                txtCollateralNo.Text = dsm.Tables[0].Rows[0]["Collateral_Storage_No"].ToString();
                txtCollateralDate.Text = DateTime.Parse(dsm.Tables[0].Rows[0]["Collateral_Storage_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
                ChkActive.Checked = Convert.ToBoolean(dsm.Tables[0].Rows[0]["IS_Active"].ToString());
            }
           
            
            // High Liquid Securities
            if (dsm.Tables[1].Rows.Count > 0)
            {
                ViewState["DT_HighLiq"] = dsm.Tables[1];
                lblHighLiqDetails.Visible = false;
                ViewState["HighLiq"] = gvHighLiqDetails.DataSource = gvHighLiqDetails1.DataSource = dsm.Tables[1];
                gvHighLiqDetails.DataBind();
                gvHighLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                lblHighLiqDetails.Visible = true;
                gvHighLiqDetails.DataSource = null;
                gvHighLiqDetails.DataBind();

                tcCollateralValuation.Tabs[2].Visible = false;


            }

            // Medium Liquid Securities
            if (dsm.Tables[2].Rows.Count > 0)
            {

                lblMedLiqDetails.Visible = false;
                ViewState["MedLiq"] = gvMedLiqDetails.DataSource = gvMedLiqDetails1.DataSource = dsm.Tables[2];
                gvMedLiqDetails.DataBind();
                gvMedLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                lblMedLiqDetails.Visible = true;
                gvMedLiqDetails.DataSource = null;
                gvMedLiqDetails.DataBind();

                tcCollateralValuation.Tabs[3].Visible = false;


            }

            // Low Liquid Securities
            if (dsm.Tables[3].Rows.Count > 0)
            {
                lblLowLiqDetails.Visible = false;
                ViewState["LowLiq"] = gvLowLiqDetails.DataSource = gvLowLiqDetails1.DataSource = dsm.Tables[3];
                gvLowLiqDetails.DataBind();
                gvLowLiqDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                lblLowLiqDetails.Visible = true;
                gvLowLiqDetails.DataSource = null;
                gvLowLiqDetails.DataBind();

                tcCollateralValuation.Tabs[4].Visible = false;


            }

            // Commodities
            if (dsm.Tables[4].Rows.Count > 0)
            {
                lblCommodityDetails.Visible = false;
                ViewState["CommLiq"] = gvCommoDetails.DataSource = gvCommoDetails1.DataSource = dsm.Tables[4];
                gvCommoDetails.DataBind();
                gvCommoDetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                lblCommodityDetails.Visible = true;
                gvCommoDetails.DataSource = null;
                gvCommoDetails.DataBind();

                tcCollateralValuation.Tabs[5].Visible = false;


            }


            // Financial Securities
            if (dsm.Tables[5].Rows.Count > 0)
            {
                lblFinDetails.Visible = false;
                ViewState["FinLiq"] = gvFinDetails.DataSource = gvFindetails1.DataSource = dsm.Tables[5];
                gvFinDetails.DataBind();
                gvFindetails1.DataBind();
                bCLT_Available = true;
            }
            else
            {
                lblFinDetails.Visible = true;
                gvFinDetails.DataSource = null;
                gvFinDetails.DataBind();

                tcCollateralValuation.Tabs[6].Visible = false;


            }

            if (bCLT_Available == false)
            {
                Utility.FunShowAlertMsg(this.Page, "No Collateral Details are Available");
                return;
            }


        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw (ex);
        }
    
       
    }
       

    private StringBuilder FunPriGenerateXML(StringBuilder sbXML, GridView gvXML)
    {
        sbXML = new StringBuilder();
        sbXML.Append("<Root>");
        foreach (GridViewRow gvRow in gvXML.Rows)
        {


            Label lblFinCollateral_Type_ID = gvRow.FindControl("lblCollateral_Type_ID") as Label;
            //HiddenField hdnCType_ID = gvRow.Cells[3].FindControl("hdnCType_ID") as HiddenField;
            Label lblFinCollateralReferenceNo = gvRow.FindControl("lblCollateralReferenceNo") as Label;
            Label lblRemarks = gvRow.FindControl("lblRemarks") as Label;
            Label lblCity = gvRow.FindControl("lblCity") as Label;
            Label lblAddress = gvRow.FindControl("lblAddress") as Label;
            Label lblStorage1 = gvRow.FindControl("lblStorage1") as Label;
            Label lblStorage2 = gvRow.FindControl("lblStorage2") as Label;
            Label lblStorage3 = gvRow.FindControl("lblStorage3") as Label;
            if ((!string.IsNullOrEmpty(lblRemarks.Text)) && (!string.IsNullOrEmpty(lblCity.Text)) && (!string.IsNullOrEmpty(lblAddress.Text)))
            {
                sbXML.Append("<Details Collateral_Type_ID = '" + lblFinCollateral_Type_ID.Text + "' ");
                    sbXML.Append(" Collateral_Item_Ref_No =  '" + lblFinCollateralReferenceNo.Text + "' Remarks = '" + lblRemarks.Text + "' City = '" + lblCity.Text + "' Address = '" + lblAddress.Text + "' ");
                    if ((!string.IsNullOrEmpty(lblStorage1.Text)) && (!string.IsNullOrEmpty(lblStorage2.Text)) && (!string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((!string.IsNullOrEmpty(lblStorage1.Text)) && (!string.IsNullOrEmpty(lblStorage2.Text)) && (string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((!string.IsNullOrEmpty(lblStorage1.Text)) && (string.IsNullOrEmpty(lblStorage2.Text)) && (!string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((!string.IsNullOrEmpty(lblStorage1.Text)) && (string.IsNullOrEmpty(lblStorage2.Text)) && (string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((string.IsNullOrEmpty(lblStorage1.Text)) && (!string.IsNullOrEmpty(lblStorage2.Text)) && (string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((string.IsNullOrEmpty(lblStorage1.Text)) && (!string.IsNullOrEmpty(lblStorage2.Text)) && (!string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((string.IsNullOrEmpty(lblStorage1.Text)) && (string.IsNullOrEmpty(lblStorage2.Text)) && (!string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    else if ((string.IsNullOrEmpty(lblStorage1.Text)) && (string.IsNullOrEmpty(lblStorage2.Text)) && (string.IsNullOrEmpty(lblStorage3.Text)))
                        sbXML.Append(" Storage1 = '" + lblStorage1.Text + "' Storage2 = '" + lblStorage2.Text + "' Storage3 = '" + lblStorage3.Text + "' ");
                    sbXML.Append("/>");
                
            }
           
            
        }
          
        
            sbXML.Append("</Root>");
            return sbXML;

        
    }

    protected void GetCust(object sender, EventArgs e)
    {
        rbtAgent.Checked = false;
        FunPriBindGrid();
    }

    protected void GetAgent(object sender, EventArgs e)
    {
        rbtCustomer.Checked = false;
        FunPriBindGrid();
    }


    protected void GetCustORAgent1()
    {
        if (Procparam != null)
            Procparam.Clear();
        else
            Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@Option", "1");
        if (rbtCustomer.Checked == true)
            Procparam.Add("@Param", "1");
        if (rbtAgent.Checked == true)
            Procparam.Add("@Param", "2");
        DataTable dt = Utility.GetDefaultData("S3G_CLT_GetCustomerORAgentCode", Procparam);
 
    }

    protected void grvPaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "modify")
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
        }
        if (e.CommandName.ToLower() == "query")
        {
            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
            Response.Redirect(strRedirectPage + "?qsViewId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
        }
    }
    protected void grvPaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");

            if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
            {
                imgbtnEdit.Enabled = true;
            }
            else
            {
                imgbtnEdit.Enabled = false;
                imgbtnEdit.CssClass = "styleGridEditDisabled";
            }
            //Authorization code end

        }

    }


    protected void Move(object sender, EventArgs e)
    {
        /// TextBox txt = (TextBox)sender;
        CheckBox chk = (CheckBox)sender;
        GridViewRow grvData = (GridViewRow)chk.Parent.Parent;
        HiddenField hidCLT_ID = (HiddenField)grvData.FindControl("hidCLT_ID");
        HiddenField hidCA_ID = (HiddenField)grvData.FindControl("hidCA_ID");

        //  HiddenField hidCLT_ID=grvPaging .Rows [e].FindControl ("hidCLT_ID") as HiddenField ;
        ViewState["CID"] = hidCLT_ID.Value;
        tempColl_ID = Convert.ToInt32(hidCLT_ID.Value);
        btnSave.Visible = true;
        btnCancel.Visible = true;
        btnClear.Visible = true;
        tcCollateralValuation.ActiveTabIndex = 1;
       
        SetTabIndexChanged();

        Procparam = new Dictionary<string, string>();

        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@Collateral_Capture_ID", hidCLT_ID.Value.ToString());
        Procparam.Add("@CA_ID", hidCA_ID.Value.ToString());
        Procparam.Add("@Option", "1");
        DataSet dsCustAndAccDetails = Utility.GetDataset("S3G_CLT_GetCustomerAccDetails", Procparam);

        //ViewState["Cust_ID"] = objS3GAdminServicesClient.FunGetScalarValue("S3G_CLT_GetCustomerAccDetails", Procparam);
        //s = ViewState["Cust_ID"].ToString();
       
        PopulateCustomerDetails(hidCA_ID.Value);

    }

    void SetTabIndexChanged()
    {     
          if (Request.QueryString["qsMode"] == "M" || Request.QueryString["qsMode"] == "Q")
        {
            tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = true ;
            tabCustORAgent.Enabled = false;
        }
        else
        {
            if (tcCollateralValuation.ActiveTabIndex == 0)
            {
                tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = false;
                btnSave.Visible = false;
                btnClear.Visible = false;
                btnCancel.Visible = false;

            }
            else
            {
                tabgeneral.Enabled = tabHighLiq.Enabled = tabMedSeq.Enabled = tabLow.Enabled = tabCommodity.Enabled = tabFinance.Enabled = true;
                btnSave.Visible = true;
                btnClear.Visible = true;
                btnCancel.Visible = true;
                tabCustORAgent.Enabled = false;
               
            }

        }
    
    }


    protected void tcCollateralValuation_TabIndexChanged(object sender, EventArgs e)
    {
       // FunProLoadAddressCombos();
        int id = Convert.ToInt32(ViewState["ID"]);
        switch (Convert.ToInt32(ViewState["gvidx"]))
        {
            case 2:
                Button High = (Button)gvHighLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(High, e, 3);
                
                break;
            case 3:
                Button Medium = (Button)gvMedLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Medium, e, 3);
                break;
            case 4:
                Button Low = (Button)gvLowLiqDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Low, e, 3);
                break;
            case 5:
                Button Commodity = (Button)gvCommoDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Commodity, e, 3);
                break;
            case 6:
                Button Finance = (Button)gvFinDetails.Rows[id].FindControl("btnUndo");
                FunPro_FindControls(Finance, e, 3);
                break;
            default:
                break;
        }

        SetTabIndexChanged();
        if (tcCollateralValuation.ActiveTabIndex == 0)
        {
            //btnSave.Visible = false;
            FunPriBindGrid();
        }
       
    }

    protected string GVDateFormat(string val)
    {
        return Utility.StringToDate(val).ToString(strDateFormat);
    }




    protected void gvHighLiqDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        //Authorization code end

    }

    #region ToSearch Empty Grid
    private int FunPriFindEmpty(GridView gv)
    {
        foreach (GridViewRow grv in gv.Rows)
        {

            Label lblRemarks = grv.FindControl("lblRemarks") as Label;
            Label lblCity = grv.FindControl("lblCity") as Label;
            Label lblAddress = grv.FindControl("lblAddress") as Label;
            Label lblStorage1 = grv.FindControl("lblStorage1") as Label;
            Label lblStorage2 = grv.FindControl("lblStorage2") as Label;
            Label lblStorage3 = grv.FindControl("lblStorage3") as Label;
            if ((lblRemarks.Text.Trim() == "") && (lblCity.Text.Trim() == "") && (lblAddress.Text.Trim() == "") && (lblStorage1.Text.Trim() == "") && (lblStorage2.Text.Trim() == "") && (lblStorage3.Text.Trim() == ""))
            {
               boolx = 0;
            }
            else
            {
                boolx = 1;
                return 1;
            }
           
        }
        if (boolx == 0)
            return 0;
       else
        return 1;
    }
    # endregion

    protected void btnChange_Click(object sender, EventArgs e)
    {
        FunPro_FindControls(sender, e, 1);
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        FunPro_FindControls(sender, e, 2);
    }
    protected void btnUndo_Click(object sender, EventArgs e)
    {
        FunPro_FindControls(sender, e, 3);
    }


    protected void FunPro_FindControls(object sender, EventArgs e, int idx)
    {
        Button btn = (Button)sender;
        GridView g = (GridView)btn.Parent.Parent.Parent.Parent;
        GridViewRow grvData = (GridViewRow)btn.Parent.Parent;

        TextBox lblRemarks = (TextBox)grvData.FindControl("lblRemarks");
        Label lblCity = (Label)grvData.FindControl("lblCity");
        TextBox lblAddress = (TextBox)grvData.FindControl("lblAddress");
        TextBox lblStorage1 = (TextBox)grvData.FindControl("lblStorage1");
        TextBox lblStorage2 = (TextBox)grvData.FindControl("lblStorage2");
        TextBox lblStorage3 = (TextBox)grvData.FindControl("lblStorage3");

        TextBox txtRemarks = (TextBox)grvData.FindControl("txtRemarks");
        AjaxControlToolkit.ComboBox txtCity = (AjaxControlToolkit.ComboBox) grvData.FindControl("txtCity");
        TextBox txtAddress = (TextBox)grvData.FindControl("txtAddress");
        FunSetComboBoxAttributes(txtCity, "City", "29");
        TextBox txtStorage1 = (TextBox)grvData.FindControl("txtStorage1");
        TextBox txtStorage2 = (TextBox)grvData.FindControl("txtStorage2");
        TextBox txtStorage3 = (TextBox)grvData.FindControl("txtStorage3");

        Button btnChange = (Button)grvData.FindControl("btnChange");
        Button btnUpdate = (Button)grvData.FindControl("btnUpdate");
        Button btnUndo = (Button)grvData.FindControl("btnUndo");
        if (idx == 1)
        {

            txtRemarks.Text.Trim();
            txtAddress.Text.Trim();
            txtStorage1.Text.Trim();
            txtStorage2.Text.Trim();
            txtStorage3.Text.Trim();

            lblRemarks.Visible = false;
             lblCity.Visible = false;
             lblAddress.Visible = false;
             lblStorage1.Visible = false;
             lblStorage2.Visible = false;
             lblStorage3.Visible = false;
             txtRemarks.Visible = true;

             if (txtCity != null)
             {
                 txtCity.Visible = true;
                 txtCity.FillDataTable((DataTable)ViewState["dtCity"], "Name", "Name", false);
                 txtCity.SelectedItem.Text = lblCity.Text;
             }

             txtAddress.Visible = true;
             txtStorage1.Visible = true;
             txtStorage2.Visible = true;
             txtStorage3.Visible = true;


             
            btnUndo.Visible = btnUpdate.Visible = true;
            txtRemarks.Text.Trim();
            txtAddress.Text.Trim();
            txtStorage1.Text.Trim();
            txtStorage2.Text.Trim();
            txtStorage3.Text.Trim();
            foreach (GridViewRow r in g.Rows)
            {
                Button btnChange1 = (Button)r.FindControl("btnChange");
                btnChange1.Enabled = false;
            }

            ViewState["ID"] = grvData.RowIndex;
            ViewState["gvidx"] = tcCollateralValuation.ActiveTabIndex;
            btnChange.Visible = false;
        }

         else if (idx == 2)

        {
            txtRemarks.Text.Trim();
            txtAddress.Text.Trim();
            txtStorage1.Text.Trim();
            txtStorage2.Text.Trim();
            txtStorage3.Text.Trim();
           
            if ((string.IsNullOrEmpty(txtCity.SelectedItem.Text)) || (string.IsNullOrEmpty(txtAddress.Text)))  
            {
                if (string.IsNullOrEmpty(txtCity.SelectedItem.Text))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the City");
                    return;
                }
                if((string.IsNullOrEmpty(txtAddress.Text)))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the Address");
                    return;
                }
             }
             if((string.IsNullOrEmpty(txtStorage1.Text)) && (string.IsNullOrEmpty(txtStorage2.Text)) && (string.IsNullOrEmpty(txtStorage3.Text)))
             {
                 Utility.FunShowAlertMsg(this.Page, "Enter atleast one Storage details");
                 return;
             }
             if (!string.IsNullOrEmpty(txtStorage1.Text))
             {
                 if ((txtStorage1.Text == txtStorage2.Text) || (txtStorage1.Text == txtStorage3.Text))
                 {
                     Utility.FunShowAlertMsg(this.Page, "The Storage Details Should not be same");
                     txtStorage2.Focus();
                     return;
                 }
             }

             if ((!string.IsNullOrEmpty(txtStorage2.Text)) && (!string.IsNullOrEmpty(txtStorage3.Text)))
             {

                 if (txtStorage2.Text == txtStorage3.Text)
                 {
                     Utility.FunShowAlertMsg(this.Page, "The Storage Details Should not be same");
                     txtStorage3.Focus();
                     return;
                 }
             }
             txtRemarks.Text.Trim();
             txtAddress.Text.Trim();
             txtStorage1.Text.Trim();
             txtStorage2.Text.Trim();
             txtStorage3.Text.Trim();

             lblRemarks.Visible = lblCity.Visible = lblAddress.Visible = lblStorage1.Visible = lblStorage2.Visible = lblStorage3.Visible = true;
             txtRemarks.Visible = txtCity.Visible = txtAddress.Visible = txtStorage1.Visible = txtStorage2.Visible = txtStorage3.Visible = false;

             lblRemarks.Text = (txtRemarks.Text).Trim();
             lblCity.Text = txtCity.SelectedItem.Text;

             Procparam = new Dictionary<string, string>();
             //Procparam.Add("@Workflow_Sequence_ID", "0");
             if (Procparam != null)
                 Procparam.Clear();
             else
                 Procparam = new Dictionary<string, string>();

             // Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
             Procparam.Add("@Company_ID", intCompanyID.ToString());
             if(lblCity.Text.Trim() != "")
             Procparam.Add("@City", (lblCity.Text).Trim());
             DataSet dscust = Utility.GetDataset("S3g_CLT_InsertCity", Procparam);
             lblAddress.Text =  (txtAddress.Text).Trim();
             lblStorage1.Text = (txtStorage1.Text).Trim();
             lblStorage2.Text = (txtStorage2.Text).Trim();
             lblStorage3.Text = (txtStorage3.Text).Trim();
             foreach (GridViewRow r in g.Rows)
             {
                 Button btnChange1 = (Button)r.FindControl("btnChange");
                 btnChange1.Enabled = true;
             }
             btnChange.Visible = true;
             btnUndo.Visible = false;
             btnUpdate.Visible = false;
             
         }
        else if (idx == 3)
        {
            txtRemarks.Text = lblRemarks.Text;
            //if (txtCity.SelectedValue != "")
            //{
            //    if (lblCity.Text == "")
            //    {
            //        txtCity.SelectedIndex = 0;
            //        txtCity.SelectedItem.Text = "";
            //    }
            //    else
            //        txtCity.SelectedItem.Text = lblCity.Text;
            //}

            //else
            //{
            //    txtCity.SelectedValue = "";
            //    lblCity.Text = "";
            //}
            txtCity.SelectedItem.Text = lblCity.Text;
            (txtAddress.Text) = lblAddress.Text.Trim();
            (txtStorage1.Text)= lblStorage1.Text.Trim();
            (txtStorage2.Text) = lblStorage2.Text.Trim();
            (txtStorage3.Text) = lblStorage3.Text.Trim();


           


            lblRemarks.Visible = lblAddress.Visible = lblCity.Visible = lblStorage1.Visible = lblStorage2.Visible = lblStorage3.Visible = true;
            txtRemarks.Visible = txtAddress.Visible = txtCity.Visible = txtStorage1.Visible = txtStorage2.Visible = txtStorage3.Visible = false;
            foreach (GridViewRow r in g.Rows)
            {

                Button btnChange1 = (Button)r.FindControl("btnChange");
                btnChange1.Enabled = true;

            }
            btnChange.Visible = true;
            btnUpdate.Visible = false;
            btnUndo.Visible = false;

            ViewState["gvidx"] = null;
            ViewState["ID"] = null;
        }

        
    }

    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Button btnChange = (Button)e.Row.FindControl("btnChange");
            //TextBox txtStorage2 = (TextBox)e.Row.FindControl("txtStorage2");
            //if (PageMode == PageModes.Modify)
            //{
            //    btnChange.Text = "Modify";
            //}
        }
    }


    protected void FunProLoadAddressCombos()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }

            foreach (GridViewRow grvData in gvHighLiqDetails.Rows)
            {

                //Label lblFinCollateral_Type_ID = gvRow.FindControl("lblCollateral_Type_ID") as Label;
                AjaxControlToolkit.ComboBox txtCity = (AjaxControlToolkit.ComboBox) gvHighLiqDetails.FindControl("txtCity");
                txtCity.FillDataTable(dtSource, "Name", "Name", false);
            }

        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCollateralValuation.ErrorMessage = ex.Message;
            cvCollateralValuation.IsValid = false;
        }
    }

    protected void FunProLoadAddressCombos1()
    {
        try
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetAddressLoodup", Procparam);

            DataTable dtSource = new DataTable();
            if (dtAddr.Select("Category = 1").Length > 0)
            {
                dtSource = dtAddr.Select("Category = 1").CopyToDataTable();
            }
            else
            {
                dtSource = FunProAddAddrColumns(dtSource);
            }


            txthCity.FillDataTable(dtSource, "Name", "Name", false);
            txtMCity.FillDataTable(dtSource, "Name", "Name", false);
            txtLCity.FillDataTable(dtSource, "Name", "Name", false);
            txtCCity.FillDataTable(dtSource, "Name", "Name", false);
            txtFCity.FillDataTable(dtSource, "Name", "Name", false);



        }
        catch (Exception ex)
        {
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            cvCollateralValuation.ErrorMessage = ex.Message;
            cvCollateralValuation.IsValid = false;
        }
    }
    protected DataTable FunProAddAddrColumns(DataTable dt)
    {
        dt.Columns.Add("ID");
        dt.Columns.Add("Name");
        dt.Columns.Add("Category");

        return dt;
    }
    protected void FunSetComboBoxAttributes(AjaxControlToolkit.ComboBox cmb, string Type, string maxLength)
    {
        TextBox textBox = cmb.FindControl("TextBox") as TextBox;

        if (textBox != null)
        {
            textBox.Attributes.Add("onkeypress", "maxlengthfortxt('" + maxLength + "');fnCheckSpecialChars('true');");
            textBox.Attributes.Add("onblur", "funCheckFirstLetterisNumeric(this, '" + Type + "');");
        }
    }





}
