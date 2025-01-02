#region Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : Reports
/// Screen Name               : CIBIL Mapping
/// Created By                : Manikandan. R
/// Created Date              : 29-Oct-2011
/// Purpose                   : Creating Mapping Table for CIBIL with S3G
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#endregion

#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Web.Security;
using System.Web.UI;
using S3GBusEntity;
using System.IO;
using System.Configuration;
using S3GBusEntity.Collection;
using System.Globalization;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
#endregion

public partial class Reports_S3GRPTCIBILMapping : ApplyThemeForProject
{
    #region Variable declaration
    int intCompanyID, intUserID = 0;
    Dictionary<string, string> Procparam = null;
    int intErrorCode = 0;
    UserInfo ObjUserInfo = new UserInfo();
    SerializationMode ObjSerializationMode = SerializationMode.Binary;
    string strDateFormat = string.Empty;
    static string strPageName = "CIBIL Report Generation";
    int intCIBILId;
    string strErrorMessagePrefix = @"Please correct the following validation(s): </br></br>   ";
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPage = "~/Common/HomePage.aspx";
    string strRedirectPageAdd = "window.location.href='../Reports/S3GRPTCIBILReportMapping.aspx';";
    string strRedirectPageView = "~/Common/HomePage.aspx";
    string strNumberFormat;
    //User Authorization
    string strMode = string.Empty;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bClearList = false;
    bool bMakerChecker = false;

    //S3GCIBILMappingMgtServices.S3GCIBILMappingMgtServicesClient objClient;
    //S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsDataTable objCIBILMapTable = null;
    //SerializationMode SerMode = SerializationMode.Binary;
    
    
    //Code end
    #endregion

    #region Events
    #region Page Load Events
    protected void Page_Load(object sender, EventArgs e)
    {
        S3GSession ObjS3GSession = new S3GSession();
        try
        {
            //User Authorization

            ObjUserInfo = new UserInfo();
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            //Code end
            //Date Format
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
           // strNumberFormat = Funsetsuffix();
            
           

            if (Request.QueryString["qsViewId"] != null)
            {
                FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
                 string strCIB = fromTicket.Name;
                 //IsAdd = 1;

                bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
                if (fromTicket != null)
                {
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
                tcRepossession.ActiveTabIndex = 0;
                FunGetS3GState();
                FunGetS3GCons();
                FunGetS3GColl();
                FunProAssetDetails();
                //FunGetS3GAsset();
                FunProIntializeGridData();
                
                FunFillAccountGrid();
              
            }

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw new ApplicationException("Unable to Load CIBIL Mapping Related Details");
           
        }
        finally
        {
            ObjS3GSession = null;
        }
    }
    #endregion

    #region Save Mappings
    protected void btnMapState_Click(object sender, EventArgs e)
    {
        //objClient = new S3GCIBILMappingMgtServices.S3GCIBILMappingMgtServicesClient();
        try
        {
            //objCIBILMapTable = new S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsDataTable();
            //S3GBusEntity.Reports.CIBILMappingMgtServices.S3G_RPT_CIBILMappingDetailsRow objRow;
            //objRow = objCIBILMapTable.NewS3G_RPT_CIBILMappingDetailsRow();
            //objRow.Company_ID = Convert.ToInt32(intCompanyID);
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            Procparam.Add("@XMLCIBILMapping", LoadStateXML(gvState));
            Procparam.Add("@Option","1");
            DataTable dt = Utility.GetDefaultData("S3G_RPT_InsertCIBILMapping", Procparam);
            Utility.FunShowAlertMsg(this, "The State Mapping Updated Successfully");

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
          //  objClient.Close();
        }
    }
    protected void btnMapCons_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@XMLCIBILMapping", LoadStateXML(gvCons));
        Procparam.Add("@Option", "3");
        DataTable dt = Utility.GetDefaultData("S3G_RPT_InsertCIBILMapping", Procparam);
        Utility.FunShowAlertMsg(this, "The Constitution Mapping Updated Successfully");


    }
    protected void btnMapColl_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@XMLCIBILMapping", LoadStateXML(gvColl));
        Procparam.Add("@Option", "5");
        DataTable dt = Utility.GetDefaultData("S3G_RPT_InsertCIBILMapping", Procparam);
        Utility.FunShowAlertMsg(this, "The Collateral Securities Mapping Updated Successfully");


    }
    protected void btnMapAsset_Click(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyID.ToString());
        Procparam.Add("@XMLCIBILMapping", LoadAssetXML(gvAccount));
        Procparam.Add("@Option", "2");
        DataTable dt = Utility.GetDefaultData("S3G_RPT_InsertCIBILAssetMapping", Procparam);
        Utility.FunShowAlertMsg(this, "The Asset Type Updated Successfully");


    }
    #endregion

    #region Cancel Mappings
    protected void btnClearState_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
    protected void btnClearCons_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
    protected void btnCollClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
    protected void btnAssetClear_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPageView);
    }
    #endregion

    #region Grid Row Data Bound
    protected void gvState_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlState = (DropDownList)e.Row.FindControl("ddlState");
                Label lblCIBILState = (Label)e.Row.FindControl("lblCIBILState"); 
                ddlState.BindDataTable(((DataTable)ViewState["tblCIBILState"]));
                ddlState.SelectedValue = lblCIBILState.Text;
            }
        }
        catch (Exception ex)
        {
           
        }
    }

    protected void gvAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
              if (e.Row.RowType == DataControlRowType.Footer)
            {

                DropDownList ddlLOBF = (DropDownList)e.Row.FindControl("ddlLOBF");
                DropDownList ddlAssetTypeF = (DropDownList)e.Row.FindControl("ddlAssetTypeF");
                DropDownList ddlAssetClassF = (DropDownList)e.Row.FindControl("ddlAssetClassF");
                DropDownList ddlProductF = (DropDownList)e.Row.FindControl("ddlProductF");
                DropDownList ddlCIBILAccountF = (DropDownList)e.Row.FindControl("ddlCIBILAccountF");
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
                if (ddlLOBF.SelectedIndex > 0)
                {
                    Procparam.Add("@LOB_ID", ddlLOBF.SelectedValue);
                }
                if (ddlAssetTypeF.SelectedIndex > 0)
                {
                    Procparam.Add("@ASSET_TYPE_ID", ddlAssetTypeF.SelectedValue);
                }
                DataSet dsAccountDataSet = Utility.GetDataset("S3G_RPT_CIBILAccountMapping", Procparam);
                // To Load Line of Business
                ViewState["LineofBusiness"] = dsAccountDataSet.Tables[0];
                ddlLOBF.BindDataTable((DataTable)ViewState["LineofBusiness"]);

                ViewState["AssetType"] = dsAccountDataSet.Tables[1];
                ddlAssetTypeF.BindDataTable((DataTable)ViewState["AssetType"]);

                ViewState["AssetClass"] = dsAccountDataSet.Tables[2];
                ddlAssetClassF.BindDataTable((DataTable)ViewState["AssetClass"]);

                ViewState["Product"] = dsAccountDataSet.Tables[3];
                ddlProductF.BindDataTable((DataTable)ViewState["Product"]);


                ViewState["CIBIL_Asset_ID"] = dsAccountDataSet.Tables[4];
                ddlCIBILAccountF.BindDataTable((DataTable)ViewState["CIBIL_Asset_ID"]);

         }
                       
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlState = (DropDownList)e.Row.FindControl("ddlState");
                Label lblCIBILCons = (Label)e.Row.FindControl("lblCIBILCons");
                ddlState.BindDataTable(((DataTable)ViewState["tblCIBILCons"]));
                ddlState.SelectedValue = lblCIBILCons.Text;
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    protected void gvColl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlState = (DropDownList)e.Row.FindControl("ddlState");
                Label lblCIBILColl = (Label)e.Row.FindControl("lblCIBILColl");
                ddlState.BindDataTable(((DataTable)ViewState["tblCIBILColl"]));
                ddlState.SelectedValue = lblCIBILColl.Text;
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    #endregion
    
    #endregion
    #region Functions
    // To load State from S3G
    protected void FunGetS3GState()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dtS3GState = Utility.GetDefaultData("S3G_RPT_GetS3GStateRecords", Procparam);
            if (dtS3GState.Rows.Count > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                ViewState["tblCIBILState"] = Utility.GetDefaultData("S3G_RPT_GetCIBILStateRecords", Procparam);

                gvState.DataSource = dtS3GState;
                gvState.DataBind();
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }

    }
    // To load Constitution from S3G
    protected void FunGetS3GCons()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dtS3GState = Utility.GetDefaultData("S3G_RPT_GetS3GConsRecords", Procparam);
            if (dtS3GState.Rows.Count > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                ViewState["tblCIBILCons"] = Utility.GetDefaultData("S3G_RPT_GetCIBILConsRecords", Procparam);
                gvCons.DataSource = dtS3GState;
                gvCons.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    // To Load S3G Collaterals 
    protected void FunGetS3GColl()
    {
        try
        {
            if (Procparam != null)
                Procparam.Clear();
            else
                Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", intCompanyID.ToString());
            DataTable dtS3GState = Utility.GetDefaultData("S3G_RPT_GetS3GCollateralRecords", Procparam);
            if (dtS3GState.Rows.Count > 0)
            {
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_ID", intCompanyID.ToString());
                ViewState["tblCIBILColl"] = Utility.GetDefaultData("S3G_RPT_GetCIBILCollateralRecords", Procparam);
                gvColl.DataSource = dtS3GState;
                gvColl.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    //protected void FunGetS3GAsset()
    //{
    //    try
    //    {
    //        if (Procparam != null)
    //            Procparam.Clear();
    //        else
    //            Procparam = new Dictionary<string, string>();
    //        Procparam.Add("@Company_ID", intCompanyID.ToString());
    //        DataTable dtS3GState = Utility.GetDefaultData("S3G_RPT_GetS3GAccountType", Procparam);
    //        if (dtS3GState.Rows.Count > 0)
    //        {
    //            Procparam = new Dictionary<string, string>();
    //            Procparam.Add("@Company_ID", intCompanyID.ToString());
    //            ViewState["tblCIBILAsset"] = Utility.GetDefaultData("S3G_RPT_GetCIBILAssetType", Procparam);
    //            gvAccount.DataSource = dtS3GState;
    //            gvAccount.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}


    #region Grid XML Forming
    protected string LoadStateXML(GridView grv)
    {
        StringBuilder strStateBuilder = null;
        strStateBuilder = new StringBuilder();
        strStateBuilder.Append("<Root>");
        foreach (GridViewRow gvr in grv.Rows)
        {

            string Value_ID;
            string Look_Up_ID;
            string Lookup_Type_Code;

            DropDownList ddlState = (DropDownList)gvr.FindControl("ddlState");
            Label lblID = (Label)gvr.FindControl("lblID");
            Lookup_Type_Code = ddlState.SelectedValue;
            Value_ID = lblID.Text;
            if (Lookup_Type_Code != "0")
                strStateBuilder.Append("<Details Lookup_Type_Code = '" + Lookup_Type_Code + "' Value_ID = '" + Value_ID + "'/>");

        }
        strStateBuilder.Append("</Root>");
        return strStateBuilder.ToString();
    }

    protected string LoadAssetXML(GridView grv)
    {
        StringBuilder strStateBuilder = null;
        strStateBuilder = new StringBuilder();
        strStateBuilder.Append("<Root>");
        foreach (GridViewRow gvr in grv.Rows)
        {
            string LOB_ID;
            string Type_ID;
            string Class_ID;
            string Product_ID;
            string Look_Up_ID;
            string Lookup_Type_Code;

            //DropDownList ddlState = (DropDownList)gvr.FindControl("ddlState");
            Label lblLOB_ID = (Label)gvr.FindControl("lblLOB_ID");
            Label lblAssetType_ID = (Label)gvr.FindControl("lblAssetType_ID");
            Label lblAssetClass_ID = (Label)gvr.FindControl("lblAssetClass_ID");
            Label lblProduct_ID = (Label)gvr.FindControl("lblProduct_ID");
            Label lblCIBIL_ID = (Label)gvr.FindControl("lblCIBIL_ID");
            LOB_ID = lblLOB_ID.Text;
            Type_ID = lblAssetType_ID.Text;
            Class_ID = lblAssetClass_ID.Text;
            Product_ID = lblProduct_ID.Text;
            Lookup_Type_Code = lblCIBIL_ID.Text;
            if (Lookup_Type_Code != "0")
            {
                strStateBuilder.Append("<Details  LOB_ID= '" + LOB_ID);
                if ((Type_ID != "0") && (Type_ID != ""))
                {
                    strStateBuilder.Append("' Type_ID = '" + Type_ID);
                }
                if ((Class_ID != "0") && (Class_ID != ""))
                {
                    strStateBuilder.Append("' Class_ID = '" + Class_ID);
                }
                if ((Product_ID != "0") && (Product_ID != ""))
                {
                    strStateBuilder.Append("' Product_ID = '" + Product_ID);
                }

                strStateBuilder.Append("' Lookup_Type_Code = '" + Lookup_Type_Code + "'/>");
            }

        }
        strStateBuilder.Append("</Root>");
        return strStateBuilder.ToString();
    }

    #endregion
    #endregion




    protected void gvAccount_DeleteClick(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtDelete;
            dtDelete = (DataTable)ViewState["AccountGridDetails"];
            ((DataRow)dtDelete.Select("Sno=" + (e.RowIndex + 1).ToString()).GetValue(0)).Delete();
            dtDelete.AcceptChanges();
            FunProSetSerielNum(ref dtDelete);
            FunProFillgrid(dtDelete);
            if (dtDelete.Rows.Count == 0)
            {
                FunProIntializeGridData();
                FunFillAccountGrid();
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

    protected void ddlLOBF_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlLOBF = (DropDownList)gvAccount.FooterRow.FindControl("ddlLOBF");
        DropDownList ddlAssetTypeF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetTypeF");
        DropDownList ddlAssetClassF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetClassF");
        DropDownList ddlProductF = (DropDownList)gvAccount.FooterRow.FindControl("ddlProductF");
        DropDownList ddlCIBILAccountF = (DropDownList)gvAccount.FooterRow.FindControl("ddlCIBILAccountF");
        try
        {
            ddlAssetTypeF.Items.Clear();
            ddlAssetClassF.Items.Clear();
            ddlProductF.Items.Clear();
            if (ddlLOBF.SelectedIndex > 0)
            {
                FunFillDropDownAccountGrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataRow DRow;
            DataTable dtAccountDetails = (DataTable)ViewState["AccountGridDetails"];
            DropDownList ddlLOBF = (DropDownList)gvAccount.FooterRow.FindControl("ddlLOBF");
            DropDownList ddlAssetTypeF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetTypeF");
            DropDownList ddlAssetClassF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetClassF");
            DropDownList ddlProductF = (DropDownList)gvAccount.FooterRow.FindControl("ddlProductF");
            DropDownList ddlCIBILAccountF = (DropDownList)gvAccount.FooterRow.FindControl("ddlCIBILAccountF");
            if (ddlLOBF.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Line of Business");
                return;
            }
            if (ddlCIBILAccountF.SelectedIndex <= 0)
            {
                Utility.FunShowAlertMsg(this, "Select the Account Type");
                return;
            }
            string LOB_ID;
            string ASSET_TYPE_ID ;
            string ASSET_CLASS_ID ;
            string Product_ID ;

            ASSET_TYPE_ID = ddlAssetTypeF.SelectedValue;
            ASSET_CLASS_ID = ddlAssetClassF.SelectedValue;
            Product_ID = ddlProductF.SelectedValue;



            //if (ASSET_TYPE_ID == "0")
            //{
            //    ASSET_TYPE_ID = "";
            //}
            //if (ASSET_CLASS_ID == "0")
            //{
            //    ASSET_CLASS_ID = "";
            //}
            //if (Product_ID == "0")
            //{
            //    Product_ID = "";
            //}


          //  string LOBfilterExpression = ((" LOB_ID = '" + Convert.ToString(ddlLOBF.SelectedValue)) + "' and " + (" ASSET_TYPE_ID = '" + Convert.ToString(ASSET_TYPE_ID)) + "' and " + (" ASSET_CLASS_ID = '" + Convert.ToString(ASSET_CLASS_ID)) + "' and " + (" Product_ID = '" + Convert.ToString(Product_ID) + "'"));
            
          

            DRow = dtAccountDetails.NewRow();
          //  DRow = dtAccountDetails.NewRow();
            DRow["Sno"] = dtAccountDetails.Rows.Count + 1;
            DRow["LOB_ID"] = ddlLOBF.SelectedValue;
            DRow["LOB"] = ddlLOBF.SelectedItem.Text;
            DRow["ASSET_TYPE_ID"] = ddlAssetTypeF.SelectedValue;
            DRow["ASSET_CLASS_ID"] = ddlAssetClassF.SelectedValue;
            DRow["PRODUCT_ID"] = ddlProductF.SelectedValue;
            if (ddlAssetTypeF.SelectedIndex > 0)
            {
                DRow["ASSET_TYPE_NAME"] = ddlAssetTypeF.SelectedItem.Text;
            }
            if (ddlAssetClassF.SelectedIndex > 0)
            {
                DRow["ASSET_CLASS_NAME"] = ddlAssetClassF.SelectedItem.Text;
            }
            if (ddlProductF.SelectedIndex > 0)
            {
                DRow["PRODUCT_NAME"] = ddlProductF.SelectedItem.Text;
            }
            DRow["CIBIL_Asset_ID"] = ddlCIBILAccountF.SelectedValue;
            DRow["Asset_Type"] = ddlCIBILAccountF.SelectedItem.Text;
            //DataTable dtAccountDetails = new DataTable();
            string LOBfilterExpression = ((" LOB_ID = '" + Convert.ToString(ddlLOBF.SelectedValue)) + "' and " + (" ASSET_TYPE_ID = '" + Convert.ToString(ASSET_TYPE_ID)) + "' and " + (" ASSET_CLASS_ID = '" + Convert.ToString(ASSET_CLASS_ID)) + "' and " + (" Product_ID = '" + Convert.ToString(Product_ID) + "'"));
            DataRow[] dtLOBFilterDetails = dtAccountDetails.Select(LOBfilterExpression);
            if (dtLOBFilterDetails.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Already Selected Line of Business is Mapped with Diffrent Combination");
                //dtAccountDetails.Rows.Remove(DRow);
                ViewState["AccountGridDetails"] = dtAccountDetails;
                return;
            }

            if (ASSET_TYPE_ID != "0" || Product_ID != "0"  || ASSET_CLASS_ID != "0")
            {
                string LOBComfilterExpressionComp = ((" LOB_ID = '" + Convert.ToString(ddlLOBF.SelectedValue)) + "' and " + (" ASSET_TYPE_ID = '" + Convert.ToString("0")) + "' and " + (" ASSET_CLASS_ID = '" + Convert.ToString("0")) + "' and " + (" Product_ID = '" + Convert.ToString("0") + "'"));
                DataRow[] dtLOBFilterDetailsComp = dtAccountDetails.Select(LOBComfilterExpressionComp);
                if (dtLOBFilterDetailsComp.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "The Line of Business cannot mapped with other combinations");
                    //dtAccountDetails.Rows.Remove(DRow);
                    ViewState["AccountGridDetails"] = dtAccountDetails;
                    return;
                }
            }

            if (ASSET_TYPE_ID == "0" && Product_ID == "0" && ASSET_CLASS_ID == "0")
            {
                string LOBComfilterExpressionCompOt = ((" LOB_ID = '" + Convert.ToString(ddlLOBF.SelectedValue)) + "' OR " + (" ASSET_TYPE_ID > '" + Convert.ToString("0")) + "' OR " + (" ASSET_CLASS_ID > '" + Convert.ToString("0")) + "' or " + (" Product_ID > '" + Convert.ToString("0") + "'"));
                DataRow[] dtLOBFilterDetailsCompOt = dtAccountDetails.Select(LOBComfilterExpressionCompOt);
                if (dtLOBFilterDetailsCompOt.Length > 0)
                {
                    Utility.FunShowAlertMsg(this, "The Line of Business is already mapped with other combination");
                    //dtAccountDetails.Rows.Remove(DRow);
                    ViewState["AccountGridDetails"] = dtAccountDetails;
                    return;
                }
            }
            







            dtAccountDetails.Rows.Add(DRow);
            dtAccountDetails = (DataTable)ViewState["AccountGridDetails"];
            FunProFillgrid(dtAccountDetails);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FunProFillgrid(DataTable dtAccountDetails)
    {
        try
        {
          
                gvAccount.DataSource = ViewState["AccountGridDetails"] = dtAccountDetails;
                gvAccount.DataBind();
                FunFillAccountGrid();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load Grid");
        }
    }
    protected void FunFillAccountGrid()
    {
        try
        {
            
            DropDownList ddlLOBF = (DropDownList)gvAccount.FooterRow.FindControl("ddlLOBF");
            DropDownList ddlAssetTypeF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetTypeF");
            DropDownList ddlAssetClassF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetClassF");
            DropDownList ddlProductF = (DropDownList)gvAccount.FooterRow.FindControl("ddlProductF");
            DropDownList ddlCIBILAccountF = (DropDownList)gvAccount.FooterRow.FindControl("ddlCIBILAccountF");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));

            if (ddlLOBF.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", ddlLOBF.SelectedValue);
            }
           
            DataSet dsAccountDataSet = Utility.GetDataset("S3G_RPT_CIBILAccountMapping", Procparam);
            // To Load Line of Business
            ViewState["LineofBusiness"] = dsAccountDataSet.Tables[0];
            ddlLOBF.BindDataTable((DataTable)ViewState["LineofBusiness"]);

            ViewState["AssetType"] = dsAccountDataSet.Tables[1];
            ddlAssetTypeF.BindDataTable((DataTable)ViewState["AssetType"]);

            ViewState["AssetClass"] = dsAccountDataSet.Tables[2];
            ddlAssetClassF.BindDataTable((DataTable)ViewState["AssetClass"]);

            ViewState["Product"] = dsAccountDataSet.Tables[3];
            ddlProductF.BindDataTable((DataTable)ViewState["Product"]);


            ViewState["CIBIL_Asset_ID"] = dsAccountDataSet.Tables[4];
            ddlCIBILAccountF.BindDataTable((DataTable)ViewState["CIBIL_Asset_ID"]);

        }
        catch(Exception ex)
        {
            throw ex;
        }

    }
    protected void FunFillDropDownAccountGrid()
    {
        try
        {
            DropDownList ddlLOBF = (DropDownList)gvAccount.FooterRow.FindControl("ddlLOBF");
            DropDownList ddlAssetTypeF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetTypeF");
            DropDownList ddlAssetClassF = (DropDownList)gvAccount.FooterRow.FindControl("ddlAssetClassF");
            DropDownList ddlProductF = (DropDownList)gvAccount.FooterRow.FindControl("ddlProductF");
            DropDownList ddlCIBILAccountF = (DropDownList)gvAccount.FooterRow.FindControl("ddlCIBILAccountF");
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            if (ddlLOBF.SelectedIndex > 0)
            {
                Procparam.Add("@LOB_ID", ddlLOBF.SelectedValue);
                DataSet dsAccountDataSet = Utility.GetDataset("S3G_RPT_CIBILAccountMapping", Procparam);
                ddlAssetTypeF.BindDataTable(dsAccountDataSet.Tables[1]);
                ddlAssetClassF.BindDataTable(dsAccountDataSet.Tables[2]);
                ddlProductF.BindDataTable(dsAccountDataSet.Tables[3]);
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
            if (ViewState["AccountGridDetails"] == null)
            {
                DataTable dtAccountDetails;
                dtAccountDetails = new DataTable("AccountGridDetails");
                dtAccountDetails.Columns.Add("Sno");
                dtAccountDetails.Columns.Add("LOB_ID");
                dtAccountDetails.Columns.Add("LOB");
                dtAccountDetails.Columns.Add("ASSET_TYPE_ID");
                dtAccountDetails.Columns.Add("ASSET_TYPE_NAME");
                dtAccountDetails.Columns.Add("ASSET_CLASS_ID");
                dtAccountDetails.Columns.Add("ASSET_CLASS_NAME");
                dtAccountDetails.Columns.Add("PRODUCT_ID");
                dtAccountDetails.Columns.Add("PRODUCT_NAME");
                dtAccountDetails.Columns.Add("CIBIL_Asset_ID");
                dtAccountDetails.Columns.Add("Asset_Type");
                DataRow DRow = dtAccountDetails.NewRow();
                DRow["Sno"] = 0;
                DRow["LOB_ID"] = "";
                DRow["LOB"] = "";
                DRow["ASSET_TYPE_ID"] = "";
                DRow["ASSET_TYPE_NAME"] = "";
                DRow["ASSET_CLASS_ID"] = "";
                DRow["ASSET_CLASS_NAME"] = "";
                DRow["PRODUCT_ID"] = "";
                DRow["PRODUCT_NAME"] = "";
                DRow["CIBIL_Asset_ID"] = "";
                DRow["Asset_Type"] = "";
                gvAccount.EditIndex = -1;
                dtAccountDetails.Rows.Add(DRow);
                gvAccount.DataSource = dtAccountDetails;
                gvAccount.DataBind();
                gvAccount.Rows[0].Visible = false;
                dtAccountDetails.Rows[0].Delete();
                ViewState["AccountGridDetails"] = dtAccountDetails;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Unable to Load Grid data");
        }
    }
    protected void FunProAssetDetails()
    {
        try
        {
            Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DataTable dtAccountDatatable = Utility.GetDefaultData("S3G_RPT_GetAccountMapping", Procparam);
            FunProSetSerielNum(ref dtAccountDatatable);
            gvAccount.DataSource = ViewState["AccountGridDetails"] = dtAccountDatatable;
            gvAccount.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}
