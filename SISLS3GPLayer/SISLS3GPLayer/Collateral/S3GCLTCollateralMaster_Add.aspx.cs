#region Page Header
//Module Name      :   Collateral
//Screen Name      :   S3GCLTCollateralMaster_Add.aspx
//Created By       :   Anitha V
//Created Date     :   06-May-2011
//Purpose          :   To Insert and Update Collateral Master Details

#endregion
#region Namespace
using System;
using System.Collections;
using System.Collections.Generic;
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
using Resources;
using System.Text;
using System.Globalization;
#endregion

public partial class Collateral_S3GCLTCollateralMaster_Add : ApplyThemeForProject
{
    #region VariableDeclaration

    int intCompanyID = 0;
    int intUserID = 0;
    int intErrorCode = 0;
    int intCMId = 0;
    int intRowId = 0;
    //int intCollateralSecurities = 0;

    string strCollateralRefNo = string.Empty;
    string strCLDesc = string.Empty;  //To store Collateral Level Description
    string strMode = string.Empty;
    string strDateFormat = string.Empty;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "../Collateral/S3GCLTCollateralMaster_View.aspx";
    string strRedirectPageView = "window.location.href='../Collateral/S3GCLTCollateralMaster_View.aspx';";
    string strRedirectPageAdd = "window.location.href='../Collateral/S3GCLTCollateralMaster_Add.aspx';";

    decimal decWeightage = 0;
    decimal decfrom = 0;
    decimal decto = 0;

    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;

    UserInfo objUserInfo = new UserInfo();
    S3GSession objS3GSession = new S3GSession();

    Dictionary<string, string> proceparam = null;
    StringBuilder strXML = new StringBuilder();

    DataTable dtCollateral = new DataTable();
    CollateralMgtServicesReference.CollateralMgtServicesClient objClient;
    S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterDataTable objDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;



    #endregion

    #region PageLoad

    protected new void Page_PreInit(object sender, EventArgs e) //transaction screen page init
    {
        try
        {
            if (Request.QueryString["Popup"] != null)
            {
                this.Page.MasterPageFile = "~/Common/MasterPage.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
            else
            {
                this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                UserInfo ObjUserInfo = new UserInfo();
                this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            intCompanyID = objUserInfo.ProCompanyIdRW;
            intUserID = objUserInfo.ProUserIdRW;
            strDateFormat = objS3GSession.ProDateFormatRW;

            bCreate = objUserInfo.ProCreateRW;
            bModify = objUserInfo.ProModifyRW;
            bQuery = objUserInfo.ProViewRW;

            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString.Get("qsViewId"));

                if (fromTicket != null)
                {
                    intCMId = Convert.ToInt32(fromTicket.Name);
                    strMode = Request.QueryString.Get("qsMode");
                }

                else
                {

                    strAlert = strAlert.Replace("__ALERT__", "Invalid URL");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                }
            }
            if (!IsPostBack)
            {
                FunToolTip();
                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                txtDate.Text = DateTime.Now.ToString(strDateFormat);
                //FunPriBindGridCollateral ();
                rfvLineOfBusiness.ErrorMessage = ValidationMsgs.S3G_ValMsg_LOB;

               

                if (intCMId > 0 && strMode == "M")
                {
                    FunGetCollateralDetails();
                    FunPriDisableControls(1);
                }
                else if (intCMId > 0 && strMode == "Q")
                {
                    FunGetCollateralDetails();
                    FunPriDisableControls(-1);

                }
                else
                {
                    FunPriBindLOB();
                    //FunPriBindProduct();
                    //FunPriBindConstitution();

                    FunPriBindGridCollateral();

                    FunPriDisableControls(0);
                }

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }

    }


    #endregion
    #region BindGridCollateral
    private void FunPriBindGridCollateral()
    {
        try
        {
            dtCollateral = new DataTable();

            DataRow dr;

            dtCollateral.Columns.Add("SlNo");
            dtCollateral.Columns.Add("Description");
            dtCollateral.Columns.Add("CollateralType");
            dtCollateral.Columns.Add("CollateralSecurities");
            dtCollateral.Columns.Add("FromOwnership");
            dtCollateral.Columns.Add("ToOwnership");
            dtCollateral.Columns.Add("FromLoan");
            dtCollateral.Columns.Add("ToLoan");
            dtCollateral.Columns.Add("Weightage");
            dtCollateral.Columns.Add("CollateralTypeValue");
            dtCollateral.Columns.Add("CollateralSecuritiesValue");
            dtCollateral.Columns.Add("ID");
            dtCollateral.Columns.Add("Mode");


            dr = dtCollateral.NewRow();

            dtCollateral.Rows.Add(dr);

            grdCollateral.DataSource = dtCollateral;
            grdCollateral.DataBind();
            grdCollateral.Rows[0].Visible = false;

            ViewState["CollateralDetail"] = dtCollateral;
            if (grdCollateral.FooterRow != null)
            {
                DropDownList ddlCollateralType = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");
                FunPriBindCollateralType(ddlCollateralType);
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region FunDisableControls
    private void FunPriDisableControls(int intModeId)
    {
        try
        {
            switch (intModeId)
            {
                case 0:
                    //Create Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                    if (!bCreate)
                    {
                        btnSave.Enabled = false;
                    }
                    Panel1.Visible = false;
                    btnGo.Enabled = true;
                    break;
                case 1:
                    //Modify Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    if (!bModify)
                    {
                        btnSave.Enabled = false;
                    }
                    btnClear.Enabled = false;
                   // ddlLOB.Enabled = false;
                   //ddlProduct.Enabled = false;
                    //ddlConstitution.Enabled = false;
                   //ddlLOB.ClearDropDownList();
                   // ddlProduct.ClearDropDownList();
                   // ddlConstitution.ClearDropDownList();
                    chkActive.Enabled = true;
                    break;
                case -1:
                    //Query Mode
                    lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                    if (!bQuery)
                    {
                        Response.Redirect(strRedirectPage,false);
                    }
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                   // ddlLOB.Enabled = false;
                 // ddlProduct.Enabled = false;
                    //ddlConstitution.Enabled = false;
                    ////ddlLOB.ClearDropDownList();
                    ////ddlProduct.ClearDropDownList();
                    ////ddlConstitution.ClearDropDownList();
                    grdCollateral.FooterRow.Visible = false;
                    grdCollateral.Columns[grdCollateral.Columns.Count - 1].Visible = false;
                    for (int i = 0; i < grdCollateral.Rows.Count; i++)
                    {
                        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkEdit")).Visible = false;
                        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkDelete")).Visible = false;
                    }

                    break;

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {
        proceparam = new Dictionary<string, string>();
        proceparam.Add("@Company_ID", intCompanyID.ToString());
        if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
        {
            proceparam.Add("@LOB_ID", ddlLOB.SelectedValue);
        }

        if (Convert.ToInt32(ddlConstitution.SelectedValue) > 0)
        {
            proceparam.Add("@Constitution_ID", ddlConstitution.SelectedValue);
        }
        if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
        {
            proceparam.Add("@Product_Id", ddlConstitution.SelectedValue);
        }

        DataTable dtExists = Utility.GetDefaultData("S3G_CLT_Chk_CollateralMaster_Exists", proceparam);
        if (dtExists.Rows[0][0].ToString() == "1")
        {
            Utility.FunShowAlertMsg(this, "Given combination already exists.");
            grdCollateral.DataSource = null;
            grdCollateral.DataBind();
            Panel1.Visible = false;
            return;
        }
        else
        {
            FunPriBindGridCollateral();
            Panel1.Visible = true;
        }

    }
    #region GetCollateralDetails
    private void FunGetCollateralDetails()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Collateral_ID", intCMId.ToString());
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            DataSet ds = new DataSet();

            ds = Utility.GetDataset("S3G_CLT_GetCollateralMasterDetails", proceparam);
            txtCollateralRefNo.Text = ds.Tables[0].Rows[0]["Collateral_Ref_No"].ToString();
            txtDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["Transaction_Date"].ToString(), CultureInfo.CurrentCulture.DateTimeFormat).ToString(strDateFormat);
            //FunPriBindLOB();
            //ddlLOB.FillDataTable(ds.Tables[0], "LOB_ID", "LOB_Name");

            ListItem lst = new ListItem(ds.Tables[0].Rows[0]["LOB_Name"].ToString(), ds.Tables[0].Rows[0]["LOB_ID"].ToString());
            ddlLOB.Items.Add(lst);

            ddlLOB.SelectedValue = ds.Tables[0].Rows[0]["LOB_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["LOB_ID"].ToString();
            //FunPriBindProduct();
            //ddlProduct.FillDataTable(ds.Tables[0], "Product_ID", "Product_Name");

            ListItem lst1 = new ListItem(ds.Tables[0].Rows[0]["Product_Name"].ToString(), ds.Tables[0].Rows[0]["Product_Id"].ToString());
            ddlProduct.Items.Add(lst1);

            ddlProduct.SelectedValue = ds.Tables[0].Rows[0]["Product_ID"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["Product_ID"].ToString();
            //FunPriBindConstitution();

            ListItem lst3 = new ListItem(ds.Tables[0].Rows[0]["Constitution_Name"].ToString(), ds.Tables[0].Rows[0]["Constitution_ID"].ToString());
            ddlConstitution.Items.Add(lst3);

            ddlConstitution.SelectedValue = ds.Tables[0].Rows[0]["Constitution_ID"].ToString();
            chkActive.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Is_Active"].ToString());


            grdCollateral.DataSource = ds.Tables[1];
            grdCollateral.DataBind();
            dtCollateral = ds.Tables[1];
            ViewState["CollateralDetail"] = ds.Tables[1];
            FunPriCalculateToalWeightage();
            //if (chkActive.Checked == false)
            //{
            //    //btnSave.Enabled = false;
            //    grdCollateral.FooterRow.Visible = false;
            //    for (int i = 0; i < grdCollateral.Rows.Count; i++)
            //    {
            //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkEdit")).Visible = false;
            //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkDelete")).Visible = false;
            //    }
            //}
            if (grdCollateral.FooterRow != null && strMode != "Q")
            {
                DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                FunPriBindCollateralType(ddlCollateralType1);
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region BindLOB
    private void FunPriBindLOB()
    {
        try
        {
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@Is_Active", "1");
            proceparam.Add("@User_Id", intUserID.ToString());
            proceparam.Add("@Program_ID", "162");
            ddlLOB.BindDataTable(SPNames.LOBMaster, proceparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });

        }
          
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    #endregion
    #region BindProduct
    private void FunPriBindProduct()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@Is_Active", "1");
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                proceparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());

            ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, proceparam, new string[] { "Product_ID", "Product_Code", "Product_Name" });

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region BindConstitution
    private void FunPriBindConstitution()
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@Is_Active", "1");
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
                proceparam.Add("@LOB_ID", ddlLOB.SelectedValue.ToString());
            ddlConstitution.BindDataTable(SPNames.S3G_Get_ConstitutionMaster, proceparam, new string[] { "Constitution_ID", "ConstitutionName" });
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region BindCollateralType
    private void FunPriBindCollateralType(DropDownList ddlCollateralType)
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@LookupType_Code", "3");


            ddlCollateralType.BindDataTable("S3G_CLT_GetLookupTypeCode", proceparam, new string[] { "Lookup_Code", "Lookup_Description" });

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region BindCollateralSecurities
    private void FunPriBindCollateralSecurities(DropDownList ddlCollateralSecurities, DropDownList ddlCollateralType)
    {
        try
        {
            if (proceparam != null)
                proceparam.Clear();
            proceparam = new Dictionary<string, string>();
            //if (Convert .ToInt32 (ddlCollateralType .SelectedValue )>0)
            //{
            proceparam.Add("@Company_ID", intCompanyID.ToString());
            proceparam.Add("@Collateral_Type", ddlCollateralType.SelectedValue.ToString());

            ddlCollateralSecurities.BindDataTable("S3G_CLT_GetCollateralSecurities", proceparam, new string[] { "Collateral_Securities_ID", "Collateral_Securities_Name" });

            //}
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region ToolTip
    private void FunToolTip()
    {
        //throw new NotImplementedException();
        txtCollateralRefNo.ToolTip = lblCollateralRefNo.Text;
        txtDate.ToolTip = lblDate.Text;
        txtTotal.ToolTip = lblTotal.Text;
        ddlConstitution.ToolTip = lblConstitution.Text;
        ddlLOB.ToolTip = lblLineOfBusiness.Text;
        ddlProduct.ToolTip = lblProduct.Text;
        chkActive.ToolTip = lblActive.Text;

    }
    #endregion
    #region FormXMLDetails
    private string FunPubFormXMLDetails()
    {
        try
        {
            strXML.Append("<Root>");
            foreach (GridViewRow grvRow in grdCollateral.Rows)
            {
                string strCollateralCode = ((Label)grvRow.FindControl("lblCollateralTypeValue")).Text;
                string strCollateralSecurities = ((Label)grvRow.FindControl("lblCollateralSecuritiesValue")).Text;
                string strDescription = ((TextBox)grvRow.FindControl("lblDescription")).Text;
                //string strLiquidityType = ((Label)grvRow.FindControl("lblLiquidityValue")).Text;
                string strOwershipPercentageFrom = ((Label)grvRow.FindControl("lblFromO")).Text;
                string strOwershipPercentageTo = ((Label)grvRow.FindControl("lblToO")).Text;
                string strLoanworthPercentageFrom = ((Label)grvRow.FindControl("lblFromL")).Text;
                string strLoanworthPercentageTo = ((Label)grvRow.FindControl("lblToL")).Text;
                string strWeightagePercentage = ((Label)grvRow.FindControl("lblWeightage")).Text;
                string strMode = ((Label)grvRow.FindControl("lblMode")).Text;
                string strID = ((Label)grvRow.FindControl("lblID")).Text;
                strXML.Append("<Details Collateral_Securities_ID='" + strCollateralSecurities.ToString() + "' Collateral_Code='" + strCollateralCode.ToString() + "' Description='" + strDescription.ToString() + "' Owership_Percentage_From='" + strOwershipPercentageFrom.ToString() + "' Owership_Percentage_To='" + strOwershipPercentageTo.ToString() + "' Loanworth_Percentage_From='" + strLoanworthPercentageFrom.ToString() + "' Loanworth_Percentage_To='" + strLoanworthPercentageTo.ToString() + "' Weightage_Percentage='" + strWeightagePercentage.ToString() + "' Mode='" + strMode.ToString() + "' ID='" + strID.ToString() + "'/>");


            }
            strXML.Append("</Root>");


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
        return strXML.ToString();

    }
    #endregion
    #region FunToCalculate Total Weightage
    private void FunPriCalculateToalWeightage()
    {
        try
        {
            decWeightage = 0;
            for (int i = 0; i < dtCollateral.Rows.Count; i++)
            {
                if (dtCollateral.Rows[i]["Weightage"].ToString() != "")
                {
                    decWeightage += Convert.ToDecimal(dtCollateral.Rows[i]["Weightage"].ToString());
                }
            }

            txtTotal.Text = decWeightage.ToString();

            //if (Convert.ToInt32(decWeightage) > 100)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Total Weightage Percentage should not exceed 100");
            //    btnSave.Enabled = false;
            //}
            //else
            //{
            //    btnSave.Enabled = true;
            //}
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region FunPriClear
    private void FunPriClear()
    {
        try
        {
            ddlLOB.SelectedIndex = 0;
            FunPriBindProduct();
            ddlProduct.SelectedIndex = 0;
            FunPriBindConstitution();
            ddlConstitution.SelectedIndex = 0;
            txtTotal.Text = "";
            grdCollateral.DataSource = null;
            grdCollateral.DataBind();
            FunPriBindGridCollateral();
            if (grdCollateral.FooterRow != null)
            {
                DropDownList ddlCollateralType = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                FunPriBindCollateralType(ddlCollateralType);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region ClickEvents
    #region Save
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objClient = new CollateralMgtServicesReference.CollateralMgtServicesClient();

        try
        {

            objDataTable = new S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterDataTable();
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterRow objRow;

            dtCollateral = (DataTable)ViewState["CollateralDetail"];
            if (dtCollateral.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one Collateral Details");
                return;
            }
            if (dtCollateral.Rows[0]["Description"].ToString() == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Add Collateral Details");
                return;
            }

            objRow = objDataTable.NewS3G_CLT_CollateralMasterRow();
            objRow.Collateral_ID = intCMId;
            objRow.Company_ID = intCompanyID;
            strCLDesc = "C";
            if (Convert.ToInt32(ddlLOB.SelectedValue) > 0)
            {
                objRow.LOB_ID = Convert.ToInt32(ddlLOB.SelectedValue);
                strCLDesc = strCLDesc + "L";
            }

            if (Convert.ToInt32(ddlConstitution.SelectedValue) > 0)
            {
                objRow.Constitution_ID = Convert.ToInt32(ddlConstitution.SelectedValue);
                strCLDesc = strCLDesc + "C";
            }
            if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
            {
                objRow.Product_Id = Convert.ToInt32(ddlProduct.SelectedValue);
                strCLDesc = strCLDesc + "P";
            }
            objRow.Collateral_Level_Description = strCLDesc;
            objRow.Is_Active = chkActive.Checked;
            objRow.Created_By = intUserID;
            objRow.XmlCollateralDetails = FunPubFormXMLDetails();
            objDataTable.AddS3G_CLT_CollateralMasterRow(objRow);
            intErrorCode = objClient.FunPubCreateCollateralMaster(out strCollateralRefNo, SerMode, ClsPubSerialize.Serialize(objDataTable, SerMode));
            if (intErrorCode == 0)
            {
                //Added by Bhuvana on 24/Oct/2012 to avoid double save click
                btnSave.Enabled = false;
                //End here
                if (intCMId > 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Collateral " + ValidationMsgs.S3G_ValMsg_Update, strRedirectPage);
                    return;
                }
                else
                {
                    strAlert = "Collateral " + strCollateralRefNo + " " + ValidationMsgs.S3G_ValMsg_Save;
                    strAlert += @"\n" + ValidationMsgs.S3G_ValMsg_Next + " Collateral Details ";
                    strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                    strRedirectPageView = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    return;
                }

            }
            else
            {
                if ((intErrorCode == -1) || (intErrorCode == -2) || (intErrorCode == 50))
                    Utility.FunShowValidationMsg(this.Page, "", intErrorCode);
                else if (intErrorCode == 8)
                    Utility.FunShowAlertMsg(this.Page, "Collateral Details already defined for this combination");
                else
                    Utility.FunShowValidationMsg(this.Page, "Collateral Details", intErrorCode);
                return;

            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
        finally
        {
            objClient.Close();
        }
    }
    #endregion
    #region Clear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {

            FunPriClear();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region Cancel
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage,false);

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }

    }
    #endregion
    #endregion
    #region LOBSelectedIndexChangedEvent

    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FunPriBindProduct();
            FunPriBindConstitution();
            FunPriBindGridCollateral();
            txtTotal.Text = string.Empty;

            grdCollateral.DataSource = null;
            grdCollateral.DataBind();
            Panel1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void ddlConstitution_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdCollateral.DataSource = null;
            grdCollateral.DataBind();
            Panel1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdCollateral.DataSource = null;
            grdCollateral.DataBind();
            Panel1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }


    #endregion
    protected void ddlCollateralType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlCollateralType = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");
            DropDownList ddlCollateralSecurities = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralSecurities");
            if (Convert.ToInt32(ddlCollateralType.SelectedValue) > 0)
            {
                FunPriBindCollateralSecurities(ddlCollateralSecurities, ddlCollateralType);
            }
            else
            {
                ddlCollateralSecurities.SelectedValue = "0";
                ddlCollateralSecurities.ClearDropDownList();
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    protected void ddlCollateralType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Two ways to find controls in Edit Item Template

            //first way
            intRowId = Convert.ToInt32(hdnRowId.Value);
            GridViewRow grvRow = (GridViewRow)grdCollateral.Rows[intRowId];


            //second way
            //DropDownList ddlCollateralType1 = (DropDownList)sender;
            //GridViewRow grvRow = (GridViewRow)ddlCollateralType1.Parent.Parent;

            DropDownList ddlCollateralSecurities = (DropDownList)grvRow.FindControl("ddlCollateralSecurities");
            DropDownList ddlCollateralType2 = (DropDownList)grvRow.FindControl("ddlCollateralType1");
            if (Convert.ToInt32(ddlCollateralType2.SelectedValue) > 0)
            {
                FunPriBindCollateralSecurities(ddlCollateralSecurities, ddlCollateralType2);
            }
            else
            {
                ddlCollateralSecurities.SelectedValue = "0";
                ddlCollateralSecurities.ClearDropDownList();
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #region GridEvents
    #region RowCommand
    protected void grdCollateral_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            TextBox txtDescription = (TextBox)grdCollateral.FooterRow.FindControl("txtDescription");
            DropDownList ddlCollateralType = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");
            DropDownList ddlCollateralSecurities = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralSecurities");

            TextBox txtFromO = (TextBox)grdCollateral.FooterRow.FindControl("txtFromO");
            TextBox txtToO = (TextBox)grdCollateral.FooterRow.FindControl("txtToO");
            TextBox txtFromL = (TextBox)grdCollateral.FooterRow.FindControl("txtFromL");
            TextBox txtToL = (TextBox)grdCollateral.FooterRow.FindControl("txtToL");
            TextBox txtWeightage = (TextBox)grdCollateral.FooterRow.FindControl("txtWeightage");

            if (e.CommandName == "Add")
            {
                //dtCollateral = new DataTable();
                dtCollateral = (DataTable)ViewState["CollateralDetail"];
                if (dtCollateral.Rows.Count > 0)
                {
                    if (dtCollateral.Rows[0]["Description"].ToString() == "")
                    {
                        dtCollateral.Rows.RemoveAt(0);
                    }
                }
                DataRow dr;
                dr = dtCollateral.NewRow();
                decfrom = Convert.ToDecimal(txtFromO.Text);
                decto = Convert.ToDecimal(txtToO.Text);
                if (decfrom >= decto)
                {
                    Utility.FunShowAlertMsg(this.Page, "From  Ownership Percentage should be less than To Ownership Percentage");
                    // txtFromO.Focus();
                    //txtFromO.Attributes.Add("onfocus","select()");
                    txtToO.Focus();

                    txtToO.Attributes.Add("onfocus", "select()");
                    return;
                }
                decfrom = Convert.ToDecimal(txtFromL.Text);
                decto = Convert.ToDecimal(txtToL.Text);
                if (decfrom >= decto)
                {
                    Utility.FunShowAlertMsg(this.Page, "From Loan Percentage should be less than To Loan Percentage");
                    //txtFromL.Focus();
                    //txtFromL.Attributes.Add("onfocus","select()");
                    txtToL.Focus();
                    txtToL.Attributes.Add("onfocus", "select()");
                    return;
                }
                //To check Collateral type already defined in the grid

                //for (int i=0; i < dtCollateral.Rows.Count; i++) 
                //{
                //    if (ddlCollateralType.SelectedValue.ToString() == dtCollateral.Rows[i]["CollateralTypeValue"].ToString())
                //    {
                //        Utility.FunShowAlertMsg(this.Page, "Selected Collateral Type already defined");
                //        return;
                //    }
                //}

                //if (!string.IsNullOrEmpty(txtTotal.Text) && Convert.ToDecimal(txtTotal.Text) + Convert.ToDecimal(txtWeightage.Text) > 100)
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Total Weightage Percentage should not exceed 100");
                //    return;
                //}

                dr["Description"] = txtDescription.Text.ToString();
                dr["CollateralType"] = ddlCollateralType.SelectedItem.Text.ToString();
                dr["CollateralSecurities"] = ddlCollateralSecurities.SelectedItem.Text.ToString();
                dr["FromOwnership"] = txtFromO.Text.ToString();
                dr["ToOwnership"] = txtToO.Text.ToString();
                dr["FromLoan"] = txtFromL.Text.ToString();
                dr["ToLoan"] = txtToL.Text.ToString();
                dr["Weightage"] = txtWeightage.Text.ToString();
                dr["CollateralTypeValue"] = ddlCollateralType.SelectedValue.ToString();
                dr["CollateralSecuritiesValue"] = ddlCollateralSecurities.SelectedValue.ToString();
                dr["ID"] = "0";
                dr["Mode"] = "I";
                dtCollateral.Rows.Add(dr);

                grdCollateral.DataSource = dtCollateral;
                grdCollateral.DataBind();

                ViewState["CollateralDetail"] = dtCollateral;
                FunPriCalculateToalWeightage();
                if (grdCollateral.FooterRow != null)
                {
                    DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                    FunPriBindCollateralType(ddlCollateralType1);
                }
                grdCollateral.FooterRow.Visible = true;

            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }

    #endregion
    #region RowDataBound
    protected void grdCollateral_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFromO = (TextBox)e.Row.FindControl("txtFromO");
                TextBox txtToO = (TextBox)e.Row.FindControl("txtToO");
                TextBox txtFromL = (TextBox)e.Row.FindControl("txtFromL");
                TextBox txtToL = (TextBox)e.Row.FindControl("txtToL");
                TextBox txtWeightage = (TextBox)e.Row.FindControl("txtWeightage");
                txtFromO.SetDecimalPrefixSuffix(8, 2, false, "From Ownership percentage");
                txtToO.SetDecimalPrefixSuffix(8, 2, false, "To Ownership Percentage");
                txtFromL.SetDecimalPrefixSuffix(8, 2, false, "From Loan Percentage");
                txtToL.SetDecimalPrefixSuffix(8, 2, false, "To Loan Percentage");
                txtWeightage.SetDecimalPrefixSuffix(8, 2, false, "Weightage percentage");
                txtFromO.Attributes.Add("onkeyup", "fncheckvalue('" + txtFromO.ClientID + "')");
                txtToO.Attributes.Add("onkeyup", "fncheckvalue('" + txtToO.ClientID + "')");
                txtFromL.Attributes.Add("onkeyup", "fncheckvalue('" + txtFromL.ClientID + "')");
                txtToL.Attributes.Add("onkeyup", "fncheckvalue('" + txtToL.ClientID + "')");
                //txtWeightage.Attributes.Add("onkeyup", "fncheckvalue('" + txtWeightage.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region RowEditing
    protected void grdCollateral_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grdCollateral.EditIndex = e.NewEditIndex;
            intRowId = e.NewEditIndex;
            hdnRowId.Value = intRowId.ToString();
            dtCollateral = (DataTable)ViewState["CollateralDetail"];
            grdCollateral.DataSource = dtCollateral;
            grdCollateral.DataBind();

            GridViewRow grvRow = grdCollateral.Rows[intRowId];
            TextBox txtDescription = (TextBox)grvRow.FindControl("txtDescription");
            DropDownList ddlCollateralType = (DropDownList)grvRow.FindControl("ddlCollateralType1");
            DropDownList ddlCollateralSecurities = (DropDownList)grvRow.FindControl("ddlCollateralSecurities");
            TextBox txtFromO = (TextBox)grvRow.FindControl("txtFromO");
            TextBox txtToO = (TextBox)grvRow.FindControl("txtToO");
            TextBox txtFromL = (TextBox)grvRow.FindControl("txtFromL");
            TextBox txtToL = (TextBox)grvRow.FindControl("txtToL");
            TextBox txtWeightage = (TextBox)grvRow.FindControl("txtWeightage");

            txtFromO.SetDecimalPrefixSuffix(8, 2, false, "From Ownership percentage");
            txtToO.SetDecimalPrefixSuffix(8, 2, false, "To Ownership Percentage");
            txtFromL.SetDecimalPrefixSuffix(8, 2, false, "From Loan Percentage");
            txtToL.SetDecimalPrefixSuffix(8, 2, false, "To Loan Percentage");
            txtWeightage.SetDecimalPrefixSuffix(8, 2, false, "Weightage percentage");

            txtFromO.Attributes.Add("onkeyup", "fncheckvalue('" + txtFromO.ClientID + "')");
            txtToO.Attributes.Add("onkeyup", "fncheckvalue('" + txtToO.ClientID + "')");
            txtFromL.Attributes.Add("onkeyup", "fncheckvalue('" + txtFromL.ClientID + "')");
            txtToL.Attributes.Add("onkeyup", "fncheckvalue('" + txtToL.ClientID + "')");
            //txtWeightage.Attributes.Add("onkeyup", "('" + txtWeightage.ClientID + "')");

            txtDescription.Text = dtCollateral.Rows[intRowId]["Description"].ToString();
            FunPriBindCollateralType(ddlCollateralType);
            ddlCollateralType.SelectedValue = dtCollateral.Rows[intRowId]["CollateralTypeValue"].ToString(); ;

            FunPriBindCollateralSecurities(ddlCollateralSecurities, ddlCollateralType);
            ddlCollateralSecurities.SelectedValue = dtCollateral.Rows[intRowId]["CollateralSecuritiesValue"].ToString();


            txtFromO.Text = dtCollateral.Rows[intRowId]["FromOwnership"].ToString();
            txtToO.Text = dtCollateral.Rows[intRowId]["ToOwnership"].ToString();
            txtFromL.Text = dtCollateral.Rows[intRowId]["FromLoan"].ToString();
            txtToL.Text = dtCollateral.Rows[intRowId]["ToLoan"].ToString();
            txtWeightage.Text = dtCollateral.Rows[intRowId]["Weightage"].ToString();
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            chkActive.Enabled = false;
            grdCollateral.FooterRow.Visible = false;


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #region RowUpdating
    protected void grdCollateral_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            intRowId = e.RowIndex;
            hdnRowId.Value = intRowId.ToString();
            dtCollateral = (DataTable)ViewState["CollateralDetail"];

            GridViewRow grvRow = grdCollateral.Rows[intRowId];
            TextBox txtDescription = (TextBox)grvRow.FindControl("txtDescription");
            DropDownList ddlCollateralType = (DropDownList)grvRow.FindControl("ddlCollateralType1");
            DropDownList ddlCollateralSecurities = (DropDownList)grvRow.FindControl("ddlCollateralSecurities");
            TextBox txtFromO = (TextBox)grvRow.FindControl("txtFromO");
            TextBox txtToO = (TextBox)grvRow.FindControl("txtToO");
            TextBox txtFromL = (TextBox)grvRow.FindControl("txtFromL");
            TextBox txtToL = (TextBox)grvRow.FindControl("txtToL");
            TextBox txtWeightage = (TextBox)grvRow.FindControl("txtWeightage");
            decfrom = Convert.ToDecimal(txtFromO.Text);
            decto = Convert.ToDecimal(txtToO.Text);
            if (decfrom >= decto)
            {
                Utility.FunShowAlertMsg(this.Page, "From  Ownership Percentage should be less than To Ownership Percentage");
                //txtFromO.Focus();
                //txtFromO.Attributes.Add("onfocus","select()");
                txtToO.Focus();
                txtToO.Attributes.Add("onfocus", "select()");
                return;
            }
            decfrom = Convert.ToDecimal(txtFromL.Text);
            decto = Convert.ToDecimal(txtToL.Text);
            if (decfrom >= decto)
            {
                Utility.FunShowAlertMsg(this.Page, "From Loan Percentage should be less than To Loan Percentage");
                //txtFromL.Focus();
                //txtFromL.Attributes.Add("onfocus","select()");
                txtToL.Focus();
                txtToL.Attributes.Add("onfocus", "select()");
                return;
            }

            //To check Collateral Type already defined in the grid

            //for (int i = 0; i < dtCollateral.Rows.Count; i++)  
            //{
            //    if (ddlCollateralType.SelectedValue.ToString() == dtCollateral.Rows[i]["CollateralTypeValue"].ToString())
            //    {
            //        Utility.FunShowAlertMsg(this.Page, "Selected Collateral Type already defined");
            //        return;
            //    }
            //}

            decWeightage = 0;
            for (int i = 0; i < dtCollateral.Rows.Count; i++)
            {
                if (dtCollateral.Rows[i]["Weightage"].ToString() != "" && i != e.RowIndex)
                {
                    decWeightage += Convert.ToDecimal(dtCollateral.Rows[i]["Weightage"].ToString());
                }
            }

            //if (!string.IsNullOrEmpty(txtTotal.Text) && decWeightage + Convert.ToDecimal(txtWeightage.Text) > 100)
            //{
            //    Utility.FunShowAlertMsg(this.Page, "Total Weightage Percentage should not exceed 100");
            //    return;
            //}


            dtCollateral.Rows[intRowId]["Description"] = txtDescription.Text;
            dtCollateral.Rows[intRowId]["CollateralType"] = ddlCollateralType.SelectedItem.Text.ToString();
            dtCollateral.Rows[intRowId]["CollateralSecurities"] = ddlCollateralSecurities.SelectedItem.Text.ToString();
            dtCollateral.Rows[intRowId]["FromOwnership"] = txtFromO.Text;
            dtCollateral.Rows[intRowId]["ToOwnership"] = txtToO.Text;
            dtCollateral.Rows[intRowId]["FromLoan"] = txtFromL.Text;
            dtCollateral.Rows[intRowId]["ToLoan"] = txtToL.Text;
            dtCollateral.Rows[intRowId]["Weightage"] = txtWeightage.Text;
            dtCollateral.Rows[intRowId]["CollateralTypeValue"] = ddlCollateralType.SelectedValue.ToString();
            dtCollateral.Rows[intRowId]["CollateralSecuritiesValue"] = ddlCollateralSecurities.SelectedValue.ToString();
            dtCollateral.Rows[intRowId]["Mode"] = "U";
            grdCollateral.EditIndex = -1;
            grdCollateral.DataSource = dtCollateral;
            grdCollateral.DataBind();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            if (strMode == "M")
            {
                chkActive.Enabled = true;
            }
            if (intCMId > 0)
            {
                btnClear.Enabled = false;
            }
            grdCollateral.FooterRow.Visible = true;
            FunPriCalculateToalWeightage();
            if (grdCollateral.FooterRow != null)
            {
                DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                FunPriBindCollateralType(ddlCollateralType1);
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    //#region RowDeleting
    //protected void grdCollateral_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        grdCollateral.EditIndex = -1;
    //        dtCollateral = (DataTable)ViewState["CollateralDetail"];
    //        dtCollateral.Rows.RemoveAt(e.RowIndex);
    //        if (dtCollateral.Rows.Count == 0)
    //        {
    //            FunPriBindGridCollateral();
    //        }
    //        else
    //        {
    //            grdCollateral.DataSource = dtCollateral;
    //            grdCollateral.DataBind();
    //            grdCollateral.FooterRow.Visible = true;
    //            if (grdCollateral.FooterRow != null)
    //            {
    //                DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

    //                FunPriBindCollateralType(ddlCollateralType1);
    //            }
    //        }
    //        FunPriCalculateToalWeightage();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }
    //}
    //#endregion
    #region RowCancelingEdit
    protected void grdCollateral_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            grdCollateral.EditIndex = -1;
            dtCollateral = (DataTable)ViewState["CollateralDetail"];
            grdCollateral.DataSource = dtCollateral;
            grdCollateral.DataBind();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            if (strMode == "M")
            {
                chkActive.Enabled = true;
            }
            if (intCMId > 0)
            {
                btnClear.Enabled = false;
            }
            grdCollateral.FooterRow.Visible = true;
            if (grdCollateral.FooterRow != null)
            {
                DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                FunPriBindCollateralType(ddlCollateralType1);
            }

        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.ToString();
        }
    }
    #endregion
    #endregion

    #region Active Checkbox CheckedChanged Event
    //protected void chkActive_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //if (chkActive.Checked == false)
    //        //{
    //        //    grdCollateral.FooterRow.Visible = false;
    //        //    for (int i = 0; i < grdCollateral.Rows.Count; i++)
    //        //    {
    //        //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkEdit")).Visible = false;
    //        //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkDelete")).Visible = false;
    //        //    }

    //        //}
    //        //else
    //        //{
    //        //    //dtCollateral =(DataTable )ViewState [""];
    //        //    grdCollateral.FooterRow.Visible = true;
    //        //    for (int i = 0; i < grdCollateral.Rows.Count; i++)
    //        //    {
    //        //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkEdit")).Visible = true;
    //        //        ((LinkButton)grdCollateral.Rows[i].FindControl("lnkDelete")).Visible = true;
    //        //    }
    //        //}

    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.ToString();
    //    }

    //}
    #endregion
    protected void grdCollateral_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            dtCollateral = (DataTable)ViewState["CollateralDetail"];
            if (dtCollateral.Rows.Count == 1)
            {
                Utility.FunShowAlertMsg(this, "Atleast one Collateral detail required");
            }
            else
            {
                dtCollateral.Rows.RemoveAt(e.RowIndex);
                grdCollateral.DataSource = dtCollateral;
                grdCollateral.DataBind();
            }
            ViewState["CollateralDetail"] = dtCollateral;
            FunPriCalculateToalWeightage();
            if (dtCollateral.Rows.Count == 0)
            { FunPriBindGridCollateral(); }
            if (grdCollateral.FooterRow != null)
            {
                DropDownList ddlCollateralType1 = (DropDownList)grdCollateral.FooterRow.FindControl("ddlCollateralType");

                FunPriBindCollateralType(ddlCollateralType1);
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }
}


