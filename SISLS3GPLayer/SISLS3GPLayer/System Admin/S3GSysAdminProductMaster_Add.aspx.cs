/// Module Name        :   System Admin
/// Screen Name        :   S3GSysAdminProductMaster_Add
/// Created By         :   Ramesh M
/// Created Date       :   10-May-2010
/// Purpose            :   To insert and update product master details
/// 
/// Last updated By    :   Gunasekar.K
/// Last updated Date  :   18-Oct-2010
/// Purpose            :   To address the bug -1757

/// Module Name        :   System Admin
/// Screen Name        :   Product Master
/// Created By         :   Palani Kumar.A
/// Created Date       :   17-Sep-2013
/// Purpose            :   To insert and update product master details

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.ServiceModel;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using AjaxControlToolkit;
#endregion
public partial class S3GSysAdminProductMaster_Add : ApplyThemeForProject
{
    #region Intialization
    int intErrCode = 0;
    int intProductId = 0;
    int intCompanyID = 0;
    int intUserID = 0;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    int inthdUserid;
    StringBuilder strXml = new StringBuilder();
    StringBuilder strXmlAssign = new StringBuilder();
    string strRedirectPageMC;
    UserInfo ObjUserInfo = new UserInfo();
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode SerMode = SerializationMode.Binary;
    CompanyMgtServicesReference.CompanyMgtServicesClient objProduct_MasterClient;
    CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable ObjS3G_ProductMaster_CUDataTable = new CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable();
    CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable ObjS3G_ProductMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable();
    CompanyMgtServices.S3G_SYSAD_ProductMasterDataTable ObjS3G_ProductMasterDataTable = new CompanyMgtServices.S3G_SYSAD_ProductMasterDataTable();
    CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjS3G_LOBMasterListDataTable = new CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable();
    string strKey = "Insert";
    string strMode = string.Empty;
    int strDecMaxLength = 0;
    int strPrefixLength = 0;
    Dictionary<string, string> Procparm = new Dictionary<string, string>();
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../System Admin/S3GSysAdminProductMaster_View.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminProductMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../System Admin/S3GSysAdminProductMaster_Add.aspx';";
    int ddlCashflowrowcommand;
    static string Mcashflowid;
    static string Mchargetype;
    static bool IsAcitve;

    string ctype;


    #endregion

    #region PageLoad
    /// <summary>
    ///Page Load Events
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            if (Request.QueryString["qsPId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsPId"));
                strMode = Request.QueryString.Get("qsMode");
                if (fromTicket != null)
                {
                    intProductId = Convert.ToInt32(fromTicket.Name);
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }


            if (intCompanyID == 0)
                intCompanyID = ObjUserInfo.ProCompanyIdRW;
            if (intUserID == 0)
                intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            strPrefixLength = ObjS3GSession.ProGpsPrefixRW;
            strDecMaxLength = ObjS3GSession.ProGpsSuffixRW;


            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            if (!IsPostBack)
            {
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));

                if (((intProductId > 0)) && (strMode == "M"))
                {
                    FunPriDisableControls(1);
                }
                else if (((intProductId > 0)) && (strMode == "Q"))
                {
                    FunPriDisableControls(-1);
                }
                else
                {
                    FunPriDisableControls(0);

                }
            }
        }
        catch
        {

        }
    }
    #endregion

    #region Create And Modify
    /// <summary>
    ///Save and Modify Product Details
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {

            if (cmbProductCode.SelectedItem.Text.Trim().Length != 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code length should be 3 characters');", true);
                return;
            }

            if (!Regex.IsMatch(cmbProductCode.SelectedItem.Text.Trim(), "^[a-z,A-Z,0-9]+$"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code should be an alphabet');", true);
                return;
            }            
            int x, y;
            x = FunPubFormXML();           
            if (x == 1)
            {
                return;
            }                   
            CompanyMgtServices.S3G_SYSAD_ProductMaster_CURow ObjProductMasterRow;
            ObjProductMasterRow = ObjS3G_ProductMaster_CUDataTable.NewS3G_SYSAD_ProductMaster_CURow();
            ObjProductMasterRow.Product_ID = intProductId;
            ObjProductMasterRow.Is_Active = chkActive.Checked;
            ObjProductMasterRow.Created_By = intUserID;
            ObjProductMasterRow.Modified_By = intUserID;
            ObjProductMasterRow.Company_ID = intCompanyID;
            ObjProductMasterRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
            string strProduct_Code = Convert.ToString(cmbProductCode.SelectedItem.Text.Trim());
            ObjProductMasterRow.Product_Code = strProduct_Code.ToUpper().Trim();
            ObjProductMasterRow.Product_Name = txtProductDesc.Text.Trim();

            if (chkOtherCashflow.Checked)
            {
                if ((DataTable)ViewState["dtTempAuthApprover"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' Please add atleast one Cashflow Details ');", true);
                    return;
                }
                if (!string.IsNullOrEmpty(strXml.ToString()))
                    ObjProductMasterRow.XmlProdCharges = strXml.ToString();
                if ((DataTable)ViewState["dtTempAuthApprover"] != null)
                {
                    ObjProductMasterRow.XmlAssignValue = Convert.ToString(((DataTable)ViewState["dtTempAuthApprover"]).FunPubFormXml());
                }
                else
                {
                    ObjProductMasterRow.XmlAssignValue = null;
                }
            }
            else
            {
                ObjProductMasterRow.XmlProdCharges = "<Root></Root>";
                ObjProductMasterRow.XmlAssignValue = null;

            }
            ObjS3G_ProductMaster_CUDataTable.AddS3G_SYSAD_ProductMaster_CURow(ObjProductMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            if (intProductId > 0)
            {

                if (FunCheckLobisvalid(ddlLOB.SelectedValue))
                {
                    intErrCode = objProduct_MasterClient.FunPubModifyProduct(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMaster_CUDataTable, SerMode));
                }
                else
                {
                    strAlert = strAlert.Replace("__ALERT__", "Selected line of business rights not assigned");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }
            }
            else
            {
                intErrCode = objProduct_MasterClient.FunPubCreateProduct(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMaster_CUDataTable, SerMode));
            }
            if (intErrCode == 0)
            {
                ddlLOB.SelectedIndex = 0;
                cmbProductCode.SelectedIndex = 0;
                txtProductDesc.Text = string.Empty;
                chkActive.Checked = true;
            }
            if (intErrCode == 10)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert(' The Entered Product description is already exist for the Product code.');", true);
                return;

            }

            if (intErrCode == 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The select/entered Product code already exist. Please enter another Product code');", true);
                return;
            }
            else if (intErrCode == 2)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The entered Product description already exist. Please enter another Product description');", true);
                return;
            }
            else if (intErrCode == 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The entered Product mapping already exist.');", true);
                return;
            }
            else if (intErrCode == 50)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('The entered Product details added failed');", true);
            else
            {
                if (intProductId > 0)
                {
                    //Added by Bhuvana M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    ProductDetailsAdd.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product details updated successfully');window.location.href='../System Admin/S3GSysAdminProductMaster_View.aspx';", true);
                }
                else
                {
                    //Added by Bhuvana M on 18/Oct/2012 to avoid double save click
                    btnSave.Enabled = false;
                    //End here
                    strAlert = "Product details added successfully";
                    strAlert += @"\n\nWould you like to add one more Product?";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    lblErrorMessage.Text = string.Empty;
                }
            }

            lblErrorMessage.Text = string.Empty;
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objProduct_MasterClient.Close();
        }

        //objProduct_MasterClient.Close();

    }
    /// <summary>
    ///Navigate the View Mode
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/System Admin/S3GSysAdminProductMaster_View.aspx");
    }
    /// <summary>
    ///Delete Poroduct Details
    /// </summary>
    private void FnPubProductDelete()
    {
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {
            CompanyMgtServices.S3G_SYSAD_ProductMasterRow ObjProductMasterRow;
            ObjProductMasterRow = ObjS3G_ProductMasterDataTable.NewS3G_SYSAD_ProductMasterRow();
            ObjProductMasterRow.Product_ID = (intProductId);
            ObjS3G_ProductMasterDataTable.AddS3G_SYSAD_ProductMasterRow(ObjProductMasterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            intErrCode = objProduct_MasterClient.FunPubDeleteProduct(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMasterDataTable, SerMode));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Record deleted successfully');window.location.href='../System Admin/S3GSysAdminProductMaster_View.aspx';", true);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objProduct_MasterClient.Close();
        }

    }

    #endregion

    #region Page Load Methods
    /// <summary>
    /// This method is used to display product details
    /// </summary>
    ///     /// <summary>
    ///Get Poroduct Details
    /// </summary>
    private void FunGetProductDet()
    {
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {
            Dictionary<string, string> Procparm = new Dictionary<string, string>();

            Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            if (intProductId == 0)
            {
                Procparm.Add("@Is_Active", "1");
            }
            //--Added by Guna on 18th-Oct-2010 to address the Bud -1757
            if (strMode != "M" && strMode != "Q")
            {
                Procparm.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
            }
            //--Ends Here
            // Added by Saranya 
            Procparm.Add("@Program_ID", "6");
            //Procparm.Add("@Program_Code", "PROD");
            //--Ends Here

            ddlLOB.BindDataTable("S3G_Get_LOB_LIST", Procparm, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });


            Procparm = null;
            if (ddlLOB.Items.Count > 2)
            {
                ddlLOB.SelectedIndex = 0;
            }
            //Bind Product
            Procparm = new Dictionary<string, string>();
            Procparm.Add("@Company_ID", Convert.ToString(intCompanyID));

            cmbProductCode.BindDataTable(SPNames.SYS_ProductMaster, Procparm, new string[] { "Product_ID", "Product_Code" });
            // Added By Manikandan for the Changes given by Sakthi Finance
            if (intProductId > 0)
            {
                Procparm = new Dictionary<string, string>();
                Procparm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                // Procparm.Add("@User_ID", ObjUserInfo.ProUserIdRW.ToString());
                Procparm.Add("@Product_ID", intProductId.ToString());
                DataSet ds = Utility.GetDataset("S3G_Get_Product_Details", Procparm);
                ViewState["LobDataset"] = ds;
                ViewState["InflowDetails"] = ds.Tables[0];
                ViewState["dtTempAuthApprover"] = ds.Tables[2];
                //ViewState["dtTempAuthApproverPopUp"] = ds.Tables[2];
                DataSet dss = (DataSet)ViewState["chargeDataset"];
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Count > 1)
                    {
                        cmbProductCode.SelectedItem.Text = ds.Tables[0].Rows[0]["Product_Code"].ToString();
                        txtProductDesc.Text = ds.Tables[0].Rows[0]["Product_Name"].ToString();
                        ddlLOB.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
                        
                        hdnID.Value = ds.Tables[0].Rows[0]["Created_By"].ToString();
                        if (ds.Tables[0].Rows[0]["Is_Active"].ToString() == "True")
                        {
                            chkActive.Checked = true;
                        }
                        else
                        {
                            chkActive.Checked = false;
                        }
                        ddlLOB.ClearDropDownList();
                        cmbProductCode.ClearDropDownList();                       
                        txtProductDesc.Enabled = true;
                       
                    }
                    if (ds.Tables[1].Columns.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (PageMode == PageModes.Query)
                            {
                                gvInflow.DataSource = ds.Tables[1];
                                gvInflow.DataBind();
                            }
                            else
                            {
                                ViewState["InflowDetails"] = null;
                                gvInflow.DataSource = ds.Tables[1];
                                gvInflow.DataBind();
                                ViewState["InflowDetails"] = ds.Tables[1];
                                //FunProIntializeGridData();
                                //FunFillInflowGrid();
                            }
                        }
                        else
                        {
                            if (PageMode == PageModes.Modify)
                            {
                                FunProIntializeGridData();
                                FunFillInflowGrid();
                            }
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gvInflow.Visible = true;
                        chkOtherCashflow.Checked = true;
                    }
                    else
                    {
                        gvInflow.Visible = false;
                        chkOtherCashflow.Checked = false;
                    }
                  
                }
              

            }
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objProduct_MasterClient.Close();
        }

        //objProduct_MasterClient.Close();


    }
    ///     /// <summary>
    ///Get LOB Details
    /// </summary
    private void FunGetLOB()
    {
        //Bind LOB
        /*
        CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow;
        ObjLOBMasterRow = ObjS3G_LOBMasterListDataTable.NewS3G_SYSAD_LOBMasterRow();
        ObjLOBMasterRow.Is_Active = true;
        ObjLOBMasterRow.Company_ID = intCompanyID; 
        ObjS3G_LOBMasterListDataTable.AddS3G_SYSAD_LOBMasterRow(ObjLOBMasterRow);
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] bytesLOBListDetails = objProduct_MasterClient.FunPubQueryLOB_LIST(SerMode, ClsPubSerialize.Serialize(ObjS3G_LOBMasterListDataTable, SerMode));
        ObjS3G_LOBMasterListDataTable = (CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesLOBListDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));
        ddlLOB.FillDataTable(ObjS3G_LOBMasterListDataTable,ObjS3G_LOBMasterListDataTable.LOB_IDColumn.ColumnName,ObjS3G_LOBMasterListDataTable.LOBCode_LOBNameColumn.ColumnName);
        */



    }
    #endregion
    /// <summary>
    ///Clear product Details
    /// </summary
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (intProductId == 0)
        {
            ddlLOB.SelectedIndex = 0;
            txtProductDesc.Text = string.Empty;     
            //txtMaxAmount.Text=
            //    txtMaxTnr.Text=
            //    txtMinAmount.Text=
                //txtMinTnr.Text = string.Empty;                
            cmbProductCode.SelectedIndex = 0;
            chkActive.Checked = true;
            ViewState["InflowDetails"] = null;
            gvInflow.DataSource = null;
            gvInflow.DataBind();
            FunProIntializeGridData();
            lblErrorMessage.Text = string.Empty;

        }
    }
    /// <summary>
    ///Get Product Name Based on Product Code
    /// </summary
    protected void cmbProductCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProductDesc.Enabled = true;
        txtProductDesc.Text = string.Empty;
        cmbProductCode.AppendDataBoundItems = true;
        cmbProductCode.ItemInsertLocation = AjaxControlToolkit.ComboBoxItemInsertLocation.Append;
        if (!Regex.IsMatch(cmbProductCode.SelectedItem.Text.Trim(), "^[a-z,A-Z,0-9]+$"))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code should be an alphabet');", true);
            cmbProductCode.SelectedIndex = -1;
            return;
        }
        if (cmbProductCode.SelectedItem.Text.Trim().Length != 3)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code length should be 3 characters');", true);
            cmbProductCode.SelectedIndex = -1;
            return;
        }
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        try
        {
            if (!(string.IsNullOrEmpty(cmbProductCode.SelectedItem.Text.ToString().Trim())))
            {
                txtProductDesc.Text = "";
                Procparm.Add("@Company_ID", intCompanyID.ToString());
                Procparm.Add("@Product_Code", cmbProductCode.SelectedItem.Text.Trim());
                DataTable dtPRD = Utility.GetDefaultData("S3G_FA_GET_PROD_DESC", Procparm);
                if (dtPRD.Rows.Count > 0)
                {
                    if (dtPRD.Columns.Count > 1)
                    {
                        txtProductDesc.Text = dtPRD.Rows[0]["PRODUCT_NAME"].ToString();
                        txtProductDesc.Enabled = false;
                        btnSave.Focus();
                    }
                    else
                    {
                        txtProductDesc.Focus();
                    }
                }
                else
                {
                    txtProductDesc.Focus();
                }
                cmbProductCode.AppendDataBoundItems = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objProduct_MasterClient.Close();
            //UpdatePanel1.Update();
        }

        //objProduct_MasterClient.Close();

    }


    /// <summary>
    ///Get Product Name Based on Product Code
    /// </summary
    protected void cmbProductCode_ItemInserted(object sender, AjaxControlToolkit.ComboBoxItemInsertEventArgs e)
    {
        txtProductDesc.Enabled = true;
        txtProductDesc.Text = string.Empty;
        objProduct_MasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();

        if (!Regex.IsMatch(cmbProductCode.SelectedItem.Text.Trim(), "^[a-z,A-Z,0-9]+$"))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code should be an alphabet');", true);
            cmbProductCode.SelectedIndex = -1;
            return;
        }
        if (cmbProductCode.SelectedItem.Text.Trim().Length != 3)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('Product code length should be 3 characters');", true);
            cmbProductCode.SelectedIndex = -1;
            return;
        }
        try
        {
            cmbProductCode.AppendDataBoundItems = true;
            cmbProductCode.ItemInsertLocation = AjaxControlToolkit.ComboBoxItemInsertLocation.Append;
            if (!(string.IsNullOrEmpty(cmbProductCode.SelectedItem.Text.ToString().Trim())))
            {
                CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewRow ObjProductMasterRow;
                ObjProductMasterRow = ObjS3G_ProductMaster_ViewDataTable.NewS3G_SYSAD_ProductMaster_ViewRow();
                ObjProductMasterRow.Product_Code = cmbProductCode.SelectedItem.Text.Trim();
                ObjProductMasterRow.Company_ID = intCompanyID;
                ObjS3G_ProductMaster_ViewDataTable.AddS3G_SYSAD_ProductMaster_ViewRow(ObjProductMasterRow);

                SerializationMode SerMode = SerializationMode.Binary;
                byte[] byteProdcutDetails = objProduct_MasterClient.FunPubQueryProduct(SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMaster_ViewDataTable, SerMode));
                ObjS3G_ProductMaster_ViewDataTable = (CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteProdcutDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable));
                if (ObjS3G_ProductMaster_ViewDataTable.Rows.Count > 0)
                {
                    txtProductDesc.Text = string.Empty;
                    txtProductDesc.Text = ObjS3G_ProductMaster_ViewDataTable.Rows[0]["Product_Name"].ToString().Trim();
                    txtProductDesc.Enabled = false;
                    btnSave.Focus();
                }
                else
                {
                    txtProductDesc.Focus();
                }
            }
            cmbProductCode.AppendDataBoundItems = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            objProduct_MasterClient.Close();

        }

        //objProduct_MasterClient.Close();

    }

    #region ValueDisable
    /// <summary>
    ///Access Rights
    /// </summary
    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Create Mode
                if (!bCreate)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                ddlLOB.SelectedIndex = 0;
                chkActive.Enabled = false;
                FunGetProductDet();
                FunProIntializeGridData();
                FunFillInflowGrid();
                ddlLOB.SelectedIndex = 0;
                gvInflow.Visible = false;
                break;

            case 1: // Modify Mode
                if (!bModify)
                {
                    btnSave.Enabled = false;
                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunGetProductDet();
                FunFillInflowGrid();
                btnSave.Attributes.Remove("OnClientClick");
                rfvProductCode.Visible = false;
                btnClear.Enabled = false;
                break;

            case -1:// Query Mode

                FunGetProductDet();
                if (!bQuery)
                {
                    Response.Redirect(strRedirectPageView);
                }
                chkActive.Enabled = false;
                ddlLOB.Enabled = true;
                ddlLOB.ClearDropDownList();
                txtProductDesc.ReadOnly = true;
                txtProductDesc.Enabled = true;
                btnClear.Enabled = false;
                btnSave.Enabled = false;
                cmbProductCode.Enabled = false;
                if (gvInflow.FooterRow != null)
                {
                    gvInflow.FooterRow.Visible = false;
                }
                grvAssignValue.Columns[grvAssignValue.Columns.Count - 1].Visible = false;
                btnModalAdd.Enabled = false;
                chkOtherCashflow.Enabled = false;
                if (bClearList)
                {
                    cmbProductCode.ClearDropDownList();
                    ddlLOB.ClearDropDownList();

                }
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

                break;
        }

    }
    #endregion

    private bool FunCheckLobisvalid(string strLobId)
    {
        Procparm.Add("@Company_ID", intCompanyID.ToString());
        Procparm.Add("@User_Id", intUserID.ToString());
        Procparm.Add("@LOB_ID", strLobId);
        Procparm.Add("@Program_ID", "6");
        if (intProductId == 0)
        {
            Procparm.Add("@Is_Active", "1");
        }
        Procparm.Add("@Is_UserLobActive", "1");
        DataTable dtBool = Utility.GetDefaultData("S3G_GetUserLobMapping", Procparm);
        if (dtBool.Rows[0]["EXISTS"].ToString() == "1")
            return true;
        else
            return false;

    }
    private int FunPubFormXML()
    {
        //int i = 0;
        if (gvInflow.Rows.Count > 0)
        {
            strXml = new StringBuilder();
            strXml.Append("<Root>");
            foreach (GridViewRow grvRow in gvInflow.Rows)
            {
                Label lblCashInflow_ID = (Label)grvRow.FindControl("lblCashInflow_ID");
                Label Chargetype = (Label)grvRow.FindControl("lblchargetype");
                CheckBox ChkActive = (CheckBox)grvRow.FindControl("chkActive");
                Label lablCharge = (Label)grvRow.FindControl("lblchargetypevalue");
                if (!(string.IsNullOrEmpty(lblCashInflow_ID.Text)))
                {  
                    //txtAmount.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, false, "Amount");

                    if (!string.IsNullOrEmpty(lblCashInflow_ID.Text))
                    {
                        strXml.Append("<Details CashInflow_ID ='" + Convert.ToString(lblCashInflow_ID.Text) + "'");
                        strXml.Append(" Charge = '" + Convert.ToString(lablCharge.Text) + "'");
                        strXml.Append(" Is_Active  = '" + Convert.ToString(ChkActive.Checked) + "'/> ");
                    }
                }
            }
            strXml.Append("</Root>");
        }
        return 0;
    }

    private int FunPubFormXMLForGridAssign()
    {
        //int i = 0;
        if (grvAssignValue.Rows.Count > 0)
        {
            strXml = new StringBuilder();
            strXml.Append("<Root>");
            foreach (GridViewRow grvRow in gvInflow.Rows)
            {
                Label lblCashInflow_ID = (Label)grvRow.FindControl("lblCashInflow_ID");
                Label Chargetype = (Label)grvRow.FindControl("lblchargetype");
                CheckBox ChkActive = (CheckBox)grvRow.FindControl("chkActive");
                Label lablCharge = (Label)grvRow.FindControl("lblchargetypevalue");
                if (!(string.IsNullOrEmpty(lblCashInflow_ID.Text)))
                {                    
                    //txtAmount.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, false, "Amount");

                    if (!string.IsNullOrEmpty(lblCashInflow_ID.Text))
                    {
                        strXml.Append("<Details CashInflow_ID ='" + Convert.ToString(lblCashInflow_ID.Text) + "'");
                        strXml.Append(" Charge = ' " + Convert.ToString(lablCharge.Text) + "'");
                        strXml.Append(" Is_Active  = '" + Convert.ToString(ChkActive.Checked) + "'/> ");
                    }
                }
            }
            strXml.Append("</Root>");
        }
        return 0;
    }
    protected void gvInflow_Deleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["InflowDetails"];
            DataRow[] drdelete = dtDelete.Select("Sno='" + Convert.ToString(e.RowIndex + 1) + "'");
            DataRow[] drdeletee = dtDelete.Select("Cashflow_ID='" + Convert.ToString(e.RowIndex) + "'");
            string strCasflowid = dtDelete.Rows[e.RowIndex]["Cashflow_ID"].ToString();
            if (drdelete != null && drdelete.Length > 0)
            {
                dtDelete.Rows.RemoveAt(e.RowIndex);
            }
            dtDelete.AcceptChanges();
            FunProSetSerielNum(ref dtDelete);
            FunProFillgrid(dtDelete);

            if (dtDelete.Rows.Count == 0)
            {
                if (strMode == "M")
                {
                    FunProIntializeGridData();
                    FunFillInflowGrid();


                }
                else
                {
                    //ddlLOB.SelectedIndex = 0;
                    //cmbProductCode.SelectedIndex = 0;
                    //txtProductDesc.Text = "";

                }
            }

            if (strMode == "M")
            {
                DataTable dt11 = (DataTable)ViewState["dtTempAuthApprover"];
                DataRow[] rowList = dt11.Select("CASHFLOW_ID='" + strCasflowid + "'");
                foreach (DataRow dr in rowList)
                {
                    dr.Delete();
                }
                dt11.AcceptChanges();
            }
            else
            {
                DataTable dt11 = (DataTable)ViewState["dtTempAuthApprover"];
                DataRow[] rowList = dt11.Select("CASHFLOWID='" + strCasflowid + "'");
                foreach (DataRow dr in rowList)
                {
                    dr.Delete();
                }
                dt11.AcceptChanges();
                if (dtDelete.Rows.Count == 0)
                {
                    FunProIntializeGridData();
                    FunFillInflowGrid();
                }

            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FunProSetSerielNum(ref DataTable dt)
    {
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            dt.Rows[i][0] = (i + 1).ToString();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {


        try
        {

            DataRow DRow;
            DataTable dtInflowDetails = (DataTable)ViewState["InflowDetails"];
            DropDownList ddlCashInflow = (DropDownList)gvInflow.FooterRow.FindControl("ddlCashInflow");
            //TextBox txtAmountF = (TextBox)gvInflow.FooterRow.FindControl("txtAmountF");
            CheckBox ChkActiveF = (CheckBox)gvInflow.FooterRow.FindControl("ChkActiveF");
            DropDownList ddlcharge = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");



            if (ddlCashInflow.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Cash Inflow");
                return;
            }
            if (ddlcharge.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Charge Type");
                return;
            }

            //if (((DataTable)(ViewState["dtTempAuthApprover"])) == null)
            //{
            //    Utility.FunShowAlertMsg(this, "Assign Atleast One Value");
            //    return;
            //}
            //Label lblSNo = (Label)gvInflow.Rows[gvInflow.Rows.Count - 1].FindControl("lblSerialNo");
            //int s;
            //s = Convert.ToInt32(lblSNo.Text) + 1;
            if (((DataTable)(ViewState["dtTempAuthApproverPopUp"])) != null)
            {
                DataRow[] dArray = ((DataTable)(ViewState["dtTempAuthApproverPopUp"])).Select("cashflowid='" + ddlCashInflow.SelectedValue + "' ");
                if (dArray.Length == 0)
                {
                    Utility.FunShowAlertMsg(this, "Assign Atleast One Value");
                    return;
                }
            }


            int CashInflow_ID = Convert.ToInt32(ddlCashInflow.SelectedValue);

            DRow = dtInflowDetails.NewRow();
            DRow["Sno"] = dtInflowDetails.Rows.Count + 1;
            DRow["CashFlow_ID"] = ddlCashInflow.SelectedValue;
            DRow["CASHINFLOW"] = ddlCashInflow.SelectedItem.Text;
            DRow["Charge"] = ddlcharge.SelectedItem.Text;
            DRow["ChargeType"] = ddlcharge.SelectedValue;
            DRow["IS_Active"] = ChkActiveF.Checked;

            if (ddlCashInflow.SelectedIndex > 0)
            {
                DRow["CASHINFLOW"] = ddlCashInflow.SelectedItem.Text;
            }
            string InflowfilterExpression = (" CashFlow_ID = '" + Convert.ToString(ddlCashInflow.SelectedValue) + "'");
            //string InflowfilterExpression = (" CashFlow_ID = '" + Convert.ToString(ddlCashInflow.SelectedValue) + "' And Is_Active=True");
            //string isactive = (" Is_Active =True '" + Convert.ToString(ddlCashInflow.SelectedValue) + "'");

            DataRow[] dtLOBFilterDetails = dtInflowDetails.Select(InflowfilterExpression);

            if (dtLOBFilterDetails.Length > 0)
            {

                Utility.FunShowAlertMsg(this, "Selected cashflow already exist");
                ViewState["InflowDetails"] = dtInflowDetails;
                return;

            }
            dtInflowDetails.Rows.Add(DRow);
            dtInflowDetails = (DataTable)ViewState["InflowDetails"];
            FunProFillgrid(dtInflowDetails);

            if (Mcashflowid == null)
            {
                DataTable dtMain = new DataTable();
                if ((DataTable)ViewState["dtTempAuthApprover"] == null)
                    dtMain = ((DataTable)ViewState["dtTempAuthApproverPopUp"]).Clone();
                else
                    dtMain = (DataTable)ViewState["dtTempAuthApprover"];

                foreach (GridViewRow GvRow in grvAssignValue.Rows)
                {
                    Label sno = (Label)GvRow.FindControl("lblSNo");
                    Label lblFromAmount = (Label)GvRow.FindControl("lblFromAmount");
                    Label lblToAmount = (Label)GvRow.FindControl("lblToAmount");
                    Label lblpercentageoramount = (Label)GvRow.FindControl("lblpercentageoramount");
                    Label cashflowid = (Label)GvRow.FindControl("cashflowid");
                    DataRow dRow = dtMain.NewRow();
                    dRow["SNo"] = sno.Text;
                    dRow["FromAmount"] = lblFromAmount.Text;
                    dRow["ToAmount"] = lblToAmount.Text;
                    dRow["cashflowid"] = cashflowid.Text;
                    dRow["chargetype"] = Mchargetype;
                    dRow["PercentageorAmount"] = lblpercentageoramount.Text;
                    dtMain.Rows.Add(dRow);
                }
                ViewState["dtTempAuthApprover"] = dtMain;
                ViewState["dtTempAuthApproverPopUp"] = null;
            }


        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Add");
        }

    }
    protected void FunProFillgrid(DataTable dtInflowDetails)
    {
        try
        {
            gvInflow.DataSource = ViewState["InflowDetails"] = dtInflowDetails;
            gvInflow.DataBind();
            FunFillInflowGrid();
            if (dtInflowDetails.Rows.Count > 0)
            {
                gvInflow.Visible = true;
            }
            else
            {
                gvInflow.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load Grid");
        }
    }
    protected void FunFillInflowGrid()
    {
        try
        {
            DataTable dtInflowDetails = (DataTable)ViewState["InflowDetails"];
            DropDownList ddlCashInflow = (DropDownList)gvInflow.FooterRow.FindControl("ddlCashInflow");
            TextBox txtAmountF = (TextBox)gvInflow.FooterRow.FindControl("txtAmountF");
            CheckBox ChkActiveF = (CheckBox)gvInflow.FooterRow.FindControl("ChkActiveF");
            //Added by sathish
            DropDownList ddlchargetype = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");
            if (ddlLOB.SelectedIndex > 0)
            {

                Procparm = new Dictionary<string, string>();
                //Procparm = new Dictionary<string, string>();
                Procparm.Add("@Company_ID", Convert.ToString(intCompanyID));
                if (ddlLOB.SelectedIndex > 0)
                    Procparm.Add("@LOB_ID", ddlLOB.SelectedValue);
                //if(cmbProductCode.SelectedIndex > 0)
                //    Procparm.Add("@Product_ID", ddlLOB.SelectedValue);

                DataSet dsInflowDtls = Utility.GetDataset("S3G_SA_GET_INFLOW", Procparm);
                // To Load Inflow
                //ViewState["InflowDetails"] = dsInflowDtls.Tables[0];
                ddlCashInflow.BindDataTable("S3G_SA_GET_INFLOW", Procparm, new string[] { "Cashflow_Id", "Cashinflow" });
                //ddlchargetype.DataSource = dsInflowDtls.Tables["Table1"];
                //ddlchargetype.DataBind();
                ddlchargetype.BindDataTable(dsInflowDtls.Tables[1], new string[] { "Lookup_Id", "Name" });



            }
            else if (strMode == "M")
            {

                if ((DataSet)ViewState["LobDataset"] != null)
                {
                    DataSet ds = (DataSet)ViewState["LobDataset"];
                    string LobID;
                    LobID = ds.Tables[0].Rows[0]["LOB_ID"].ToString();
                    Procparm = new Dictionary<string, string>();
                    Procparm.Add("@Company_ID", Convert.ToString(intCompanyID));
                    Procparm.Add("@LOB_ID", LobID);
                    DataSet dsInflowDtls = Utility.GetDataset("S3G_SA_GET_INFLOW", Procparm);
                    ddlCashInflow.BindDataTable("S3G_SA_GET_INFLOW", Procparm, new string[] { "Cashflow_Id", "Cashinflow" });
                    ddlchargetype.BindDataTable(dsInflowDtls.Tables[1], new string[] { "Lookup_Id", "Name" });


                }


            }


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void FunProIntializeGridData()
    {
        try
        {
            if (ViewState["InflowDetailsNew"] == null)
            {
                DataTable dtInflowDetails;
                dtInflowDetails = new DataTable("InflowDetails");
                dtInflowDetails.Columns.Add("Sno");
                dtInflowDetails.Columns.Add("Cashflow_ID");
                dtInflowDetails.Columns.Add("Cashinflow");
                dtInflowDetails.Columns.Add("Charge");
                dtInflowDetails.Columns.Add("ChargeType");
                dtInflowDetails.Columns.Add("IS_Active");

                DataRow DRow = dtInflowDetails.NewRow();
                DRow["Sno"] = 0;
                DRow["Cashflow_ID"] = "";
                DRow["Cashinflow"] = "";
                DRow["Charge"] = "";
                DRow["ChargeType"] = "";
                DRow["Is_Active"] = "";


                dtInflowDetails.Rows.Add(DRow);
                gvInflow.DataSource = dtInflowDetails;
                gvInflow.DataBind();
                gvInflow.Rows[0].Visible = false;
                ViewState["InflowDetails"] = dtInflowDetails;
                dtInflowDetails.Rows[0].Delete();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load Grid data");
        }
    }

    protected void gvInflow_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //modifyddlobload = ddlLOB.SelectedValue;

            if (e.Row.RowType == DataControlRowType.Footer)
            {

                DropDownList ddlCashInflow = (DropDownList)e.Row.FindControl("ddlCashInflow");
                TextBox txtAmountF = (TextBox)e.Row.FindControl("txtAmountF");
                CheckBox ChkActiveF = (CheckBox)e.Row.FindControl("ChkActiveF");
                Label lblSerialNo = (Label)e.Row.FindControl("lblSerialNo");

                Procparm = new Dictionary<string, string>();
                Procparm.Add("@Company_ID", Convert.ToString(intCompanyID));
                if (ddlLOB.SelectedIndex > 0)
                {
                    Procparm.Add("@LOB_ID", ddlLOB.SelectedValue);
                }
                if (cmbProductCode.SelectedIndex > 0 && PageMode == PageModes.Modify)
                {
                    Procparm.Add("@Product_ID", intProductId.ToString());
                }

                DataTable dtInflowDtls = Utility.GetDefaultData("S3G_SA_GET_INFLOW", Procparm);

                // To Load Cash flow
                //ViewState["InflowDetails"] = dtInflowDtls ;
                ddlCashInflow.BindDataTable("S3G_SA_GET_INFLOW", Procparm, new string[] { "Cashflow_Id", "Cashinflow" });
                //txtAmountF.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, "Amount");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                CheckBox ChkActive = (CheckBox)e.Row.FindControl("ChkActive");
                Label lblSerialNo = (Label)e.Row.FindControl("lblSerialNo");
                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                if (PageMode != PageModes.Create)
                {
                    lnkDelete.Enabled = true;
                    lnkDelete.OnClientClick = "";
                    if (PageMode == PageModes.Modify)
                    {
                        ChkActive.Enabled = true;
                    }
                    else
                    {
                        ChkActive.Enabled = false;
                    }

                }
                if (PageMode == PageModes.Query)
                {
                    lnkDelete.Enabled = false;
                    //txtAmount.ReadOnly = true;
                    ChkActive.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gvInflow.DataSource = null;
            gvInflow.DataBind();
            if (ddlLOB.SelectedIndex > 0)
            {
                cmbProductCode.SelectedIndex = 0;
                txtProductDesc.Text = "";
                txtProductDesc.Enabled = true;
                FunProIntializeGridData();
                FunFillInflowGrid();
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load");
        }
    }


    //Added by sathish For Model Popup Grid

    protected void btnIAssignValue_Click(object sender, EventArgs e)
    {


        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable(); // Remove
        DataTable dtApprove = new DataTable(); // Remove
        Button btn = (Button)sender;
        GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
        GridView grv = (GridView)gvRow.Parent.Parent;
        dt = (DataTable)ViewState["dtTempAuthApprover"];
        dt2 = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        Label lblSNo = (Label)gvRow.FindControl("lblSerialNo");
        Label labelcashflowid = (Label)gvRow.FindControl("lblCashInflow_ID");

        Label labelchargetype = (Label)gvRow.FindControl("lblchargetypee");
        int casflowid = Convert.ToInt32(labelcashflowid.Text);
        DataRow[] drAuthApprover = dt.Select("cashflowid='" + casflowid + "'");
        DataRow[] drAuthApproverr = dt.Select("chargetype='" + casflowid + "'");
        Mcashflowid = casflowid.ToString();
        Mchargetype = labelchargetype.Text;
        if (drAuthApprover.Length > 0)
        {
            ViewState["dtTempAuthApproverPopUp"] = dtApprove = dt.Select("cashflowid='" + casflowid + "'").CopyToDataTable();
        }

        grvAssignValue.DataSource = dtApprove;
        grvAssignValue.DataBind();


        if (strMode == "Q")
        {
            grvAssignValue.FooterRow.Visible = false;
        }
        ModalPopupExtenderAssignValue.Show();




    }


    protected void btnFAssignValue_Click(object sender, EventArgs e)
    {



        string chargetype = "";
        Mcashflowid = null;
        DropDownList ddlcashflow = (DropDownList)gvInflow.FooterRow.FindControl("ddlCashInflow");
        DropDownList ddlCharge = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");
        if (ddlcashflow.SelectedIndex <= 0 || ddlCharge.SelectedIndex <= 0)
        {
            if (ddlcashflow.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Cash Flow");
            }
            if (ddlCharge.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Charge Type");
            }
        }
        else
        {
            DataTable dt = new DataTable();
            Button btn = (Button)sender;
            GridViewRow gvRow = (GridViewRow)btn.Parent.Parent;
            GridView grv = (GridView)gvRow.Parent.Parent;
            string strSLNo = "";
            Label lblSNo = (Label)gvInflow.Rows[gvInflow.Rows.Count - 1].FindControl("lblSerialNo");
            strSLNo = (Convert.ToInt32(lblSNo.Text) + 1).ToString();
            DropDownList labelcashflowid = (DropDownList)gvRow.FindControl("ddlCashInflow");
            int casflowid = Convert.ToInt32(labelcashflowid.SelectedValue);


            if (ViewState["dtTempAuthApproverPopUp"] == null || ((DataTable)ViewState["dtTempAuthApproverPopUp"]).Rows.Count == 0)
            {
                dt = FunProInitializePopup(strSLNo);
                grvAssignValue.DataSource = dt;
                grvAssignValue.DataBind();
                grvAssignValue.Rows[0].Visible = false;
                dt.Rows.RemoveAt(0);
                ViewState["dtTempAuthApproverPopUp"] = dt;

            }
            else
            {

                dt = (DataTable)ViewState["dtTempAuthApproverPopUp"];
                grvAssignValue.DataSource = dt;
                grvAssignValue.DataBind();
                DataRow[] dRow = dt.Select("ToAmount = ''");
                if (dRow.Length > 0)
                {
                    dt.Rows.RemoveAt(0);
                    grvAssignValue.Rows[0].Visible = false;
                }


            }


            ModalPopupExtenderAssignValue.Show();
        }


    }

    private DataTable FunProInitializePopup(string s)
    {
        DropDownList ddlCharge = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");
        DataTable dt = new DataTable();
        try
        {
            DataRow dr = dt.NewRow();
            dt.Columns.Add("SNo", typeof(string));
            dt.Columns.Add("cashflowid");
            dt.Columns.Add("FromAmount");
            dt.Columns.Add("ToAmount");
            dt.Columns.Add("chargetype");
            dt.Columns.Add("PercentageorAmount");
            dr["SNo"] = s;
            dr["cashflowid"] = string.Empty;
            dr["FromAmount"] = string.Empty;
            dr["ToAmount"] = string.Empty;
            dr["chargetype"] = string.Empty;
            dr["PercentageorAmount"] = string.Empty;
            dt.Rows.Add(dr);
            return dt;

        }
        catch (Exception ex)
        {

            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to initiate the row");
        }
        return dt;


    }


    protected void btnModalAdd_Click(object sender, EventArgs e)
    {


        Button btn = (Button)sender;
        DataTable dtModal = new DataTable();
        DataTable dtMain = new DataTable();
        dtModal = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        if (dtModal.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "Add at least one charge details");
            return;
        }

        dtMain = (DataTable)ViewState["dtTempAuthApprover"];

        int intMasterSeq = Convert.ToInt32(((Label)grvAssignValue.Rows[0].FindControl("cashflowid")).Text);

        if (Mcashflowid != null)
        {
            DataRow[] Drow = dtMain.Select("CASHFLOWID='" + intMasterSeq + "'");
            foreach (DataRow dr in Drow)
            {
                dtMain.Rows.Remove(dr);
            }

            foreach (GridViewRow GvRow in grvAssignValue.Rows)
            {
                Label sno = (Label)GvRow.FindControl("lblSNo");
                Label lblFromAmount = (Label)GvRow.FindControl("lblFromAmount");
                Label lblToAmount = (Label)GvRow.FindControl("lblToAmount");
                Label lblpercentageoramount = (Label)GvRow.FindControl("lblpercentageoramount");
                Label cashflowid = (Label)GvRow.FindControl("cashflowid");
                DataRow dRow = dtMain.NewRow();
                dRow["SNo"] = sno.Text;
                dRow["FromAmount"] = lblFromAmount.Text;
                dRow["ToAmount"] = lblToAmount.Text;
                dRow["cashflowid"] = cashflowid.Text;
                dRow["chargetype"] = Mchargetype;
                dRow["PercentageorAmount"] = lblpercentageoramount.Text;
                dtMain.Rows.Add(dRow);
            }
            ViewState["dtTempAuthApprover"] = dtMain;
            ViewState["dtTempAuthApproverPopUp"] = null;
        }
        ModalPopupExtenderAssignValue.Hide();


    }

    protected void btnModalCancel_Click(object sender, EventArgs e)
    {
        if (Mcashflowid != null)
            ViewState["dtTempAuthApproverPopUp"] = null;

        ModalPopupExtenderAssignValue.Hide();
    }


    private void FunPribindddlchargetype()
    {
        Dictionary<string, string> Procparm = new Dictionary<string, string>();
        Procparm = new Dictionary<string, string>();
        Procparm.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());

    }



    protected void grvassign_rowcommand(object sender, GridViewCommandEventArgs e)
    {


        TextBox txtfromamount = (TextBox)grvAssignValue.FooterRow.FindControl("txtFromAmount");
        TextBox txtToamount = (TextBox)grvAssignValue.FooterRow.FindControl("txtToamount");
        TextBox txtpercentageoramount = (TextBox)grvAssignValue.FooterRow.FindControl("txtpercentageoramount");
        int lblsno = Convert.ToInt32(((Label)grvAssignValue.Rows[0].FindControl("lblSNo")).Text);
        DropDownList ddlcashflow = (DropDownList)gvInflow.FooterRow.FindControl("ddlCashInflow");
        DropDownList ddlchargetype = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");

        Label Lblcashflow = (Label)gvInflow.Rows[0].FindControl("Cashflow_ID");
        ddlCashflowrowcommand = Convert.ToInt32(ddlcashflow.SelectedValue);

        if (Convert.ToInt32(ddlcashflow.SelectedValue) == 0)
        {
            ddlCashflowrowcommand = Convert.ToInt32(Mcashflowid);
        }


        if (e.CommandName == "Add")
        {

            decimal dcToAmount = 0;
            TextBox txtToAmount = (TextBox)grvAssignValue.FooterRow.FindControl("txtToamount");




            if (!string.IsNullOrEmpty(txtToAmount.Text.Trim()))
            {
                dcToAmount = Convert.ToDecimal(txtToAmount.Text.Trim());
                if (dcToAmount < Convert.ToDecimal(txtfromamount.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To amount should be greater than from amount');", true);
                    txtfromamount.Focus();
                    return;
                }
                if (dcToAmount == Convert.ToDecimal(txtfromamount.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('To amount should not be equal to from amount');", true);
                    txtfromamount.Focus();
                    return;
                }


            }


            if (txtToamount.Text == "" || txtpercentageoramount.Text == "")
            {
                if (txtToamount.Text == "")
                {
                    Utility.FunShowAlertMsg(this, "Enter To Amount");
                }
                else if (txtpercentageoramount.Text == "")
                {
                    Utility.FunShowAlertMsg(this, "Enter Amount or  Percentage ");
                }
            }
            else
            {

                DataTable dtassign = (DataTable)ViewState["dtTempAuthApproverPopUp"];
                DataRow drassign = dtassign.NewRow();
                drassign["SNo"] = lblsno;
                drassign["cashflowid"] = ddlCashflowrowcommand;
                drassign["FromAmount"] = txtfromamount.Text;
                drassign["ToAmount"] = txtToamount.Text;
                drassign["chargetype"] = Mchargetype;
                drassign["PercentageorAmount"] = txtpercentageoramount.Text;
                dtassign.Rows.Add(drassign);
                grvAssignValue.DataSource = dtassign;
                grvAssignValue.DataBind();
                ViewState["dtTempAuthApproverPopUp"] = dtassign;
                ModalPopupExtenderAssignValue.Show();

            }


        }

    }

    protected void grv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        DataTable dtTable = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        Label lblSNo = (Label)grvAssignValue.Rows[e.RowIndex].FindControl("lblSNo");
        LinkButton btnDelete = (LinkButton)grvAssignValue.Rows[e.RowIndex].FindControl("btnDelete");
        DataRow[] dArr = dtTable.Select("CASHFLOWID='" + ddlCashflowrowcommand + "'");

        if (dArr.Length > 0)
        {
            dtTable.Rows[e.RowIndex].Delete();
        }
        dtTable.AcceptChanges();
        if (dtTable.Rows.Count == 0)
        {
            dtTable = FunProInitializePopup(lblSNo.Text);
            grvAssignValue.DataSource = dtTable;
            grvAssignValue.DataBind();
            dtTable.Rows[0].Delete();
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvAssignValue.Rows[0].Visible = false;
        }
        else
        {
            ViewState["dtTempAuthApproverPopUp"] = dtTable;
            grvAssignValue.DataSource = dtTable;
            grvAssignValue.DataBind();
        }

    }
    protected void FunProDeleteRowsFromDataTable(ref DataTable dtApproval, int intRowIndex)
    {
        DataRow[] dRow = dtApproval.Select("SNo='" + intRowIndex + "'");
        if (dRow.Length > 0)
        {
            foreach (DataRow dr in dRow)
            {
                dtApproval.Rows.Remove(dr);
            }
        }
        dtApproval.AcceptChanges();
        ViewState["dtTempAuthApproverPopUp"] = dtApproval;
        grvAssignValue.DataSource = dtApproval;
        grvAssignValue.DataBind();


    }
    protected void grvAssignValue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtTempAuthApproverPopUp"];
        DropDownList ddlchargetype = (DropDownList)gvInflow.FooterRow.FindControl("ddlChargeType");


        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtFromAmount = (TextBox)e.Row.FindControl("txtFromAmount");
            TextBox txtFToAmount = (TextBox)e.Row.FindControl("txtToamount");
            TextBox txtpercentageoramount = (TextBox)e.Row.FindControl("txtpercentageoramount");
            Label cashflowid = (Label)e.Row.FindControl("cashflowid");
            FilteredTextBoxExtender FilterExtender = (FilteredTextBoxExtender)e.Row.FindControl("Filter");
            RangeValidator regsx = (RangeValidator)e.Row.FindControl("Regx");

            txtFToAmount.SetDecimalPrefixSuffix(strPrefixLength, strDecMaxLength, true, "To Amount");         
            if (Mcashflowid != null)
                ctype = Mchargetype;
            else
                ctype = ddlchargetype.SelectedValue;
            Mchargetype = ctype;
            if (ctype == "156")
            {
                //FilterExtender.ValidChars = ".";
                regsx.Visible = false;
                regsx.Enabled = false;               
                txtpercentageoramount.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, true, "Amount");
            }
            else if (ctype == "157")
            {
                regsx.Visible = true;
                regsx.Enabled = true;
                txtpercentageoramount.SetPercentagePrefixSuffix(3, 2, true, "Percentage");               
            }
            if (dt == null)
            {
                txtFromAmount.Text = "1";
                txtFromAmount.Enabled = false;
            }
            if (dt != null && dt.Rows.Count == 0)
            {
                txtFromAmount.Text = "1";
                txtFromAmount.Enabled = false;
            }
            else if (dt != null && dt.Rows.Count > 0)
            {
                txtFromAmount.Text = dt.Rows[dt.Rows.Count - 1]["ToAmount"].ToString();
                if (!string.IsNullOrEmpty(txtFromAmount.Text))
                    txtFromAmount.Text = Convert.ToString(Convert.ToDecimal(txtFromAmount.Text) + Convert.ToDecimal(1));
                else
                    txtFromAmount.Text = "1";
                txtFromAmount.Enabled = false;
            }
        }
    }



    protected void FunProDeleteRowsFromDataTableAssign(ref DataTable dtApprover, ref int intDeletedRow, int intRowIndex)
    {
        DataRow[] dRow = dtApprover.Select("SNo='" + intDeletedRow + "'");
        if (dRow.Length > 0)
        {
            foreach (DataRow dr in dRow)
            {
                dtApprover.Rows.Remove(dr);
            }
        }
        dtApprover.AcceptChanges();

    }

    protected void DropDownCashflow_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPripopupload();

    }
    protected void DropDownchargetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPripopupload();
    }
    private void FunPripopupload()
    {
        DataTable dt = new DataTable();
        dt = FunProInitializePopup("1");
        grvAssignValue.DataSource = dt;
        grvAssignValue.DataBind();
        ViewState["dtTempAuthApproverPopUp"] = dt;

    }
    //Added by sathish 
    protected void isactive_checkedchanged(object sender, EventArgs e)
    {

        if (strMode == "M")
        {

            DataTable dtinfl = new DataTable();
            int intRowIndex = Utility.FunPubGetGridRowID("gvInflow", ((CheckBox)sender).ClientID.ToString());
            Label lbcashflow = (Label)gvInflow.Rows[intRowIndex].FindControl("lblCashInflow_ID");
            CheckBox chkActivee = (CheckBox)gvInflow.Rows[intRowIndex].FindControl("chkActive");
            dtinfl = (DataTable)ViewState["InflowDetails"];
            if (chkActivee.Checked == true)
            {
                DataRow[] rowListActive = dtinfl.Select("CASHFLOW_ID='" + lbcashflow.Text + "' and Is_Active='" + chkActivee.Checked + "'");
                if (rowListActive.Count() > 0)
                {
                    chkActivee.Checked = false;
                    Utility.FunShowAlertMsg(this, "Already Innactive");
                    return;
                }

            }

            DataRow[] rowList = dtinfl.Select("CASHFLOW_ID='" + lbcashflow.Text + "'");
            foreach (DataRow dr in rowList)
                dr["Is_Active"] = chkActivee.Checked;
            dtinfl.AcceptChanges();
            ViewState["InflowDetails"] = dtinfl;

        }

    }
    // Added By M Thangam - 05-May-2014
    protected void chkOtherCashflow_CheckedChanged(object sender, EventArgs e)
    {
        if(chkOtherCashflow.Checked)
            gvInflow.Visible = true;
        else
            gvInflow.Visible = false;

    }
}
