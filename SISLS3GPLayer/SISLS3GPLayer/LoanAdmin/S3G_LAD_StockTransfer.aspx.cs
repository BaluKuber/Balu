using AjaxControlToolkit;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
//using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;
using System.Collections;
using System.Text.RegularExpressions;
using QRCoder;
using System.Drawing;

public partial class LoanAdmin_S3G_LAD_StockTransfer : ApplyThemeForProject
{
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";

    UserInfo ObjUserInfo = null;
    static string strPageName = "Stock Transfer";
    int intErrCode = 0;
    int intErrCount = 0;
    int intWBTID = 0;
    int intUserId = 0;
    int intCompanyID = 0;

    string strDateFormat = string.Empty;
    string strMode = string.Empty;
    string PA_SA_Ref_ID = string.Empty;
    string Invoice_ID = string.Empty;
    int strNote_id;
    Dictionary<string, string> dictparam;

    DataTable dtStock;
    DataTable dtData;
    DataSet dsStock;
    public static LoanAdmin_S3G_LAD_StockTransfer obj_Page;
    LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient ObjStockClient;
    ContractMgtServices.S3G_LAD_Stock_MstDataTable objStockeDatatable;
    ContractMgtServices.S3G_LAD_Stock_MstRow objStockRow;
    S3GSession ObjS3GSession = new S3GSession();
    SerializationMode ObjSerMode = SerializationMode.Binary;
    DataTable dtFinal = new DataTable();
    string strRedirectPage = "~/LoanAdmin/S3G_LAD_StockTransfer_View.aspx?Code=STKT";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3G_LAD_StockTransfer.aspx?qsMode=C'";
    string strRedirectPageView = "window.location.href='../LoanAdmin/S3G_LAD_StockTransfer_View.aspx?Code=STKT';";

    PagingValues ObjMappingPaging = new PagingValues();

    public delegate void PageAssignValueMapping(int ProPageNumRW, int intPageSize);

    public int ProMapPageNumRW
    {
        get;
        set;
    }

    public int ProMapPageSizeRW
    {
        get;
        set;
    }

    protected void AssignValueMapping(int intPageNum, int intPageSize)
    {
        ProMapPageNumRW = intPageNum;
        ProMapPageSizeRW = intPageSize;
        FunPriMappingBindGrid();
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];
        CalendarExtender1.Format = strDateFormat;
        CalendarExtender2.Format = strDateFormat;
        CalendarExtender3.Format = strDateFormat;
        txtInvoiceDateFrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDateFrom.ClientID + "','" + strDateFormat + "',null,null);");
        txtInvoiceDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDateTo.ClientID + "','" + strDateFormat + "',null,null);");
        txtDocumentDate.Attributes.Add("onblur", "fnDoDate(this,'" + txtDocumentDate.ClientID + "','" + strDateFormat + "',null,null);");
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
            strMode = Request.QueryString.Get("qsMode");
            strNote_id = Convert.ToInt32(fromTicket.Name);
        }
        PageAssignValueMapping obj = new PageAssignValueMapping(this.AssignValueMapping);
        ucInvoiceMapping.callback = obj;
        ucInvoiceMapping.ProPageNumRW = ProMapPageNumRW;
        ucInvoiceMapping.ProPageSizeRW = ProMapPageSizeRW;
        if (!IsPostBack)
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            if (intCompanyID > 0)
            {
                Procparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            }
            DataTable dtAddr = Utility.GetDefaultData("S3G_SYSAD_GetStateLookup", Procparam);
            Utility.FillDataTable(ddlComState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            Utility.FillDataTable(ddltodeliverystate, dtAddr, "Location_Category_ID", "LocationCat_Description");

            Utility.FillDataTable(ddlShipFromState, dtAddr, "Location_Category_ID", "LocationCat_Description");
            Utility.FillDataTable(ddlShipToState, dtAddr, "Location_Category_ID", "LocationCat_Description");

            if (strMode == "Q")                //Query Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                FunPriLoadStockDtls();
                btnPrint.Visible = true;
            }
            else if (strMode == "M")                //Query Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                FunPriLoadStockDtls();
            }
            else                                   //Create Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
                txtDocumentDate.Text = DateTime.Now.ToString(strDateFormat);
            }
            FunPriEnableDisableControls();

        }


        // CalendarExtender1.Format = strDateFormat;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);



    }


    private void FunPriMappingBindGrid()
    {
        try
        {
            if (Convert.ToString(hdnIsMapInvoiceChanged.Value) == "1")
            {
                Utility.FunShowAlertMsg(this, "Some Changes made. kindly click Update before proceeding.");
                return;
            }


            int intTotalRecords = 0;
            bool bIsNewRow = false;
            ObjMappingPaging.ProCompany_ID = intCompanyID;
            ObjMappingPaging.ProUser_ID = intUserId;
            ObjMappingPaging.ProTotalRecords = intTotalRecords;
            ObjMappingPaging.ProCurrentPage = ProMapPageNumRW;
            ObjMappingPaging.ProPageSize = ProMapPageSizeRW;
            ObjMappingPaging.ProSearchValue = "";
            ObjMappingPaging.ProOrderBy = "";

            dictparam = new Dictionary<string, string>();

            dictparam.Add("@CustomerId", Convert.ToString(ddlLesseeName.SelectedValue));
            if (ddlVendName.SelectedValue != "0" && (ddlVendName.SelectedText != "" && ddlVendName.SelectedText != "--Select--"))
            {
                dictparam.Add("@VendorId", Convert.ToString(ddlVendName.SelectedValue));
            }
            if (txtInvoiceDateFrom.Text.ToString() != "")
                dictparam.Add("@InvoicePostingFromDate", Utility.StringToDate(txtInvoiceDateFrom.Text.ToString()).ToString());
            if (txtInvoiceDateTo.Text.ToString() != "")
                dictparam.Add("@InvoicePostingToDate", Utility.StringToDate(txtInvoiceDateTo.Text.ToString()).ToString());
            dictparam.Add("@RentalScheduleNo", Convert.ToString(ddlRSNO.SelectedValue));
            dictparam.Add("@Type", Convert.ToString(ddlType.SelectedValue));
            dictparam.Add("@DeliveryState", Convert.ToString(ddlComState.SelectedValue));
            if (ddlInvoiceType.SelectedValue != "0")
                dictparam.Add("@invoice_type", Convert.ToString(ddlInvoiceType.SelectedValue));

            if (ddlInvoiceNumber.SelectedValue != "0" && (ddlInvoiceNumber.SelectedText != "" && ddlInvoiceNumber.SelectedText != "--Select--"))
            {
                dictparam.Add("@invoice_Number", Convert.ToString(ddlInvoiceNumber.SelectedText));
            }

            gvInvoiceMapping.BindGridView("S3G_LAD_GetStockInvoice", dictparam, out intTotalRecords, ObjMappingPaging, out bIsNewRow);

            if (bIsNewRow)
            {
                gvInvoiceMapping.Rows[0].Visible = false;
                btnUpdate.Visible = false;
                btnSave.Visible = false;
                btnclear.Visible = true;
                btncancel.Visible = true;
            }
            else
            {
                btnUpdate.Visible = true;
                btnSave.Visible = true;
                btnclear.Visible = true;
                btncancel.Visible = true;

            }
            //This is to hide first row if grid is empty
            pnlStock.Visible = true;
            ucInvoiceMapping.Visible = true;
            ucInvoiceMapping.Navigation(intTotalRecords, ProMapPageNumRW, ProMapPageSizeRW);
            ucInvoiceMapping.setPageSize(ProMapPageSizeRW);
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btnMapInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            ProMapPageNumRW = 1;
            ProMapPageSizeRW = 10;
            FunPriMappingBindGrid();
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }


    private void FunPriLoadStockDtls()
    {
        try
        {
            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Stock_Hdr_ID", Convert.ToString(strNote_id));
            dictparam.Add("@COMPANY_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@USER_ID", Convert.ToString(intUserId));

            dsStock = Utility.GetDataset("S3G_LAD_GETStockDtls", dictparam);
            if (dsStock.Tables[0].Rows.Count > 0)
            {
                txtInvoiceNo.Text = dsStock.Tables[0].Rows[0]["Invoice_No"].ToString();
                txtDocumentDate.Text = dsStock.Tables[0].Rows[0]["Document_Date"].ToString();
                txtInvoiceDateFrom.Text = dsStock.Tables[0].Rows[0]["Invoice_From_Date"].ToString();
                txtInvoiceDateTo.Text = dsStock.Tables[0].Rows[0]["Invoice_To_Date"].ToString();
                ddlComState.SelectedValue = dsStock.Tables[0].Rows[0]["From_Delivery_State"].ToString();
                ddltodeliverystate.SelectedValue = dsStock.Tables[0].Rows[0]["To_Delivery_State"].ToString();
                ddlInvoiceType.SelectedValue = dsStock.Tables[0].Rows[0]["Invoice_Type"].ToString();
                ddlInvoiceNumber.SelectedValue = dsStock.Tables[0].Rows[0]["Invoice_Number"].ToString();
                ddlType.SelectedValue = dsStock.Tables[0].Rows[0]["Type"].ToString();
                ddlLesseeName.SelectedValue = dsStock.Tables[0].Rows[0]["Customer_Id"].ToString();
                ddlVendName.SelectedValue = dsStock.Tables[0].Rows[0]["Vendor_Id"].ToString();
                ddlRSNO.SelectedValue = dsStock.Tables[0].Rows[0]["Pa_sa_ref_Id"].ToString();
                ddlLesseeName.SelectedText = dsStock.Tables[0].Rows[0]["Customer_Name"].ToString();
                ddlVendName.SelectedText = dsStock.Tables[0].Rows[0]["vendor_Name"].ToString();
                ddlRSNO.SelectedText = dsStock.Tables[0].Rows[0]["Panum"].ToString();

                txtShip_From.Text = dsStock.Tables[0].Rows[0]["Ship_From_Address"].ToString();
                txtShip_To.Text = dsStock.Tables[0].Rows[0]["Ship_To_Address"].ToString();

                txtShipFromPin.Text = dsStock.Tables[0].Rows[0]["Ship_From_Pin"].ToString();
                txtShipToPin.Text = dsStock.Tables[0].Rows[0]["Ship_To_Pin"].ToString();

                ddlToLessee.SelectedValue = dsStock.Tables[0].Rows[0]["To_Customer_Id"].ToString();
                ddlToLessee.SelectedText = dsStock.Tables[0].Rows[0]["To_Customer_Name"].ToString();
                ddlTransferType.SelectedValue = dsStock.Tables[0].Rows[0]["Transfer_Type"].ToString();

                ddlShipFromState.SelectedValue = dsStock.Tables[0].Rows[0]["Ship_From_State"].ToString();
                ddlShipToState.SelectedValue = dsStock.Tables[0].Rows[0]["Ship_To_State"].ToString();
                txtShipFromGSTIN.Text = dsStock.Tables[0].Rows[0]["Ship_From_GSTIN"].ToString();
                txtShipToGSTIN.Text = dsStock.Tables[0].Rows[0]["Ship_To_GSTIN"].ToString();
                txtNote.Text = dsStock.Tables[0].Rows[0]["Note"].ToString();
            }

            dtFinal = dsStock.Tables[1];

            if (dsStock.Tables[1].Rows.Count > 0)
            {
                pnlStock.Visible = true;
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                btnclear.Visible = true;
                btncancel.Visible = true;
                divinterim.Style.Add("display", "block");
                gvInvoiceMapping.DataSource = dsStock.Tables[1];
                gvInvoiceMapping.DataBind();
                ViewState["dtFinal"] = dtFinal;

            }


        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetLesseeNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_FU_LesseeDtl", dictparam));
        return suggetions.ToArray();
    }

    [System.Web.Services.WebMethod]
    public static string[] GetVendorName(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        DataTable dtCommon = new DataTable();
        DataSet Ds = new DataSet();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_GETVENDORS", Procparam), false);
        return suggetions.ToArray();
    }


    [System.Web.Services.WebMethod]
    public static string[] GetRSNODetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@program_id", "524");
        if (Convert.ToInt32(obj_Page.ddlLesseeName.SelectedValue.ToString()) > 0)
            dictparam.Add("@cust_id", obj_Page.ddlLesseeName.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetPANUMNO", dictparam));
        return suggetions.ToArray();
    }



    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceNumberDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        if (Convert.ToInt32(obj_Page.ddlInvoiceType.SelectedValue.ToString()) > 0)
            dictparam.Add("@Invoice_Type", obj_Page.ddlInvoiceType.SelectedValue.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_ORG_Load_InvoiceNo", dictparam));
        return suggetions.ToArray();
    }




    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            ProMapPageNumRW = 1;
            ProMapPageSizeRW = 10;
            FunPriMappingBindGrid();
            dtFinal = FunPriDefaultDataTable();
            ViewState["dtFinal"] = dtFinal;
            btnUpdate.Enabled = true;
            gvInvoiceMapping.Enabled = true;
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }


    private void FunPubXmlDetails()
    {
        string strXML = "";

        try
        {
            strXML = "<Root> ";
            foreach (GridViewRow grv in gvInvoiceMapping.Rows)
            {
                strXML = strXML + "<Details ";
                CheckBox chkgvISelectInv = (CheckBox)grv.FindControl("chkgvISelectInv");
                TextBox txtapplquantity = (TextBox)grv.FindControl("txtapplquantity");
                Label lblgvmapAccountNumber = (Label)grv.FindControl("lblgvmapAccountNumber");
                Label lblgvmapInvoiceType = (Label)grv.FindControl("lblgvmapInvoiceType");
                Label lblgvmapPIDtlID = (Label)grv.FindControl("lblgvmapPIDtlID");
                Label lblgvmapVIDtlID = (Label)grv.FindControl("lblgvmapVIDtlID");
                Label lblgvmapInvoicedtlID = (Label)grv.FindControl("lblgvmapInvoicedtlID");
                Label lblgvmapHSNID = (Label)grv.FindControl("lblgvmapHSNID");
                TextBox txtUnitPrice = (TextBox)grv.FindControl("txtUnitPrice");
                TextBox txtgvmapFacilityAmount = (TextBox)grv.FindControl("txtgvmapFacilityAmount");
                Label lblShipFrom = (Label)grv.FindControl("lblShipFrom");

                if (strMode == "M" || chkgvISelectInv.Checked)
                {
                    dtFinal = (DataTable)ViewState["dtFinal"];

                    if (strMode == "M")
                        dtFinal.Rows.Clear();

                    txtShip_From.Text = lblShipFrom.Text;

                    if (txtapplquantity.Text == "")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter the Appl Quantity");
                        txtapplquantity.Focus();
                        return;
                    }

                    if (txtgvmapFacilityAmount.Text == "")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Enter the Transfer Amount");
                        txtgvmapFacilityAmount.Focus();
                        return;
                    }

                    if (txtgvmapFacilityAmount.Text == "0.00")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Transfer Amount should be greater than 0.");
                        txtgvmapFacilityAmount.Focus();
                        return;
                    }

                    if (txtapplquantity.Text == "0")
                    {
                        Utility.FunShowAlertMsg(this.Page, "Appl Quantity should be greater than Zero");
                        txtapplquantity.Focus();
                        return;
                    }

                    if (dtFinal.Select("Invoice_Dtl_Id = '" + lblgvmapInvoicedtlID.Text + "'").Length == 0)
                    {
                        DataRow dremptyrow;
                        dremptyrow = dtFinal.NewRow();
                        dremptyrow["Quantity"] = Convert.ToString(txtapplquantity.Text);
                        dremptyrow["Invoice_Flag"] = ((Convert.ToString(lblgvmapInvoicedtlID.Text) == "") ? "N" : "O");
                        dremptyrow["Invoice_Type"] = Convert.ToString(lblgvmapInvoiceType.Text);
                        dremptyrow["PIVI_Dtl_ID"] = ((Convert.ToString(lblgvmapInvoiceType.Text) == "2") ? Convert.ToString(lblgvmapVIDtlID.Text) : Convert.ToString(lblgvmapPIDtlID.Text));
                        dremptyrow["Invoice_Dtl_Id"] = Convert.ToString(lblgvmapInvoicedtlID.Text);
                        dremptyrow["Unit_Price"] = Convert.ToDecimal(txtUnitPrice.Text);
                        dremptyrow["Transfer_Amount"] = Convert.ToDecimal(txtgvmapFacilityAmount.Text);
                        if (!String.IsNullOrEmpty(lblgvmapHSNID.Text))
                            dremptyrow["HSN_ID"] = Convert.ToInt32(lblgvmapHSNID.Text);
                        dtFinal.Rows.Add(dremptyrow);
                        ViewState["dtFinal"] = dtFinal;
                    }

                }
            }

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }

    }

    protected void ddlComState_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlShipFromState.SelectedValue = ddlComState.SelectedValue;

        if (ddlTransferType.SelectedValue == "2")
        {
            ddltodeliverystate.SelectedValue = ddlShipToState.SelectedValue = ddlComState.SelectedValue;
            ddltodeliverystate.Enabled = ddlShipToState.Enabled = false;
        }

        FunPriClearGrid();

    }

    protected void ddlTransferType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();

        ddltodeliverystate.SelectedValue = ddlShipToState.SelectedValue = "0";

        if (ddlTransferType.SelectedValue == "2")
        {
            ddlComState.SelectedValue = "0";
            ddltodeliverystate.Enabled = ddlShipToState.Enabled = false;
        }
        else
        {
            ddlComState.SelectedValue = "0";
            ddltodeliverystate.Enabled = ddlShipToState.Enabled = true;
        }

    }

    private DataTable FunPriDefaultDataTable()
    {
        DataTable dtFinal = new DataTable();
        try
        {
            dtFinal.Columns.Add("Quantity", typeof(int));
            dtFinal.Columns.Add("Invoice_Flag");
            dtFinal.Columns.Add("Invoice_Type");
            dtFinal.Columns.Add("PIVI_Dtl_ID", typeof(int));
            dtFinal.Columns.Add("Invoice_Dtl_Id", typeof(int));
            dtFinal.Columns.Add("Unit_Price", typeof(decimal));
            dtFinal.Columns.Add("Transfer_Amount", typeof(decimal));
            dtFinal.Columns.Add("HSN_ID", typeof(int));

        }
        catch (Exception objException)
        {
            throw objException;
        }
        return dtFinal;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            FunPubXmlDetails();

            if (dtFinal.Rows.Count == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Select Invoice to Transfer");
                return;
            }

            Utility.FunShowAlertMsg(this.Page, "Invoices Updated Successfully");
            hdnIsMapInvoiceChanged.Value = "0";
            btnUpdate.Enabled = false;
            gvInvoiceMapping.Enabled = false;
            return;

        }

        catch (Exception objException)
        {
            throw objException;
        }

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            objStockeDatatable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LAD_Stock_MstDataTable();
            objStockRow = objStockeDatatable.NewS3G_LAD_Stock_MstRow();
            dtFinal = (DataTable)ViewState["dtFinal"];
            if (hdnIsMapInvoiceChanged.Value == "1")
            {
                Utility.FunShowAlertMsg(this, "Some Changes made. kindly click Update before proceeding.");
                return;

            }
            if (hdnIsMapInvoiceChanged.Value == "0")
            {
                if (dtFinal.Rows.Count <= 0)
                {
                    Utility.FunShowAlertMsg(this.Page, "Select Invoice to Transfer");
                    return;
                }
            }
            //return;
            if (strMode == "C")
            {
                objStockRow.Stock_Hdr_Id = "0";
            }
            else
                objStockRow.Stock_Hdr_Id = strNote_id.ToString();

            objStockRow.Transfer_Type = Convert.ToInt32(ddlTransferType.SelectedValue);

            objStockRow.Company_Id = Convert.ToInt32(intCompanyID.ToString());
            objStockRow.Created_By = Convert.ToInt32(intUserId.ToString());
            objStockRow.Customer_id = Convert.ToInt32(ddlLesseeName.SelectedValue.ToString());
            objStockRow.Vendor_Id = Convert.ToInt32(ddlVendName.SelectedValue.ToString());
            objStockRow.From_Delivery_State = Convert.ToInt32(ddlComState.SelectedValue.ToString());
            objStockRow.To_Delivery_State = Convert.ToInt32(ddltodeliverystate.SelectedValue.ToString());
            objStockRow.Invoice_Type = ddlInvoiceType.SelectedValue.ToString();

            objStockRow.Ship_From_State = Convert.ToInt32(ddlShipFromState.SelectedValue.ToString());
            objStockRow.Ship_To_State = Convert.ToInt32(ddlShipToState.SelectedValue.ToString());
            objStockRow.Ship_From_GSTIN = txtShipFromGSTIN.Text;
            objStockRow.Ship_To_GSTIN = txtShipToGSTIN.Text;

            if (!String.IsNullOrEmpty(ddlInvoiceNumber.SelectedValue))
            {
                objStockRow.Invoice_Number = ddlInvoiceNumber.SelectedText.ToString();
                objStockRow.PIVI_Hdr_Id = Convert.ToInt32(ddlInvoiceNumber.SelectedValue.ToString());
            }
            else
            {
                objStockRow.Invoice_Number = "";
                objStockRow.PIVI_Hdr_Id = Convert.ToInt32("0");
            }

            objStockRow.Pa_sa_Ref_Id = Convert.ToInt32(ddlRSNO.SelectedValue.ToString());

            if (!String.IsNullOrEmpty(txtInvoiceDateFrom.Text))
                objStockRow.Invoice_From_Date = Utility.StringToDate(txtInvoiceDateFrom.Text.ToString());
            if (!String.IsNullOrEmpty(txtInvoiceDateTo.Text))
                objStockRow.Invoice_To_Date = Utility.StringToDate(txtInvoiceDateTo.Text.ToString());
            
                objStockRow.Type = Convert.ToString(ddlType.SelectedValue.ToString());

            if (!String.IsNullOrEmpty(txtDocumentDate.Text))
                objStockRow.Document_Date = Utility.StringToDate(txtDocumentDate.Text);

            objStockRow.Ship_From = txtShip_From.Text;
            objStockRow.Ship_To = txtShip_To.Text;

            objStockRow.Ship_From_Pin = txtShipFromPin.Text;
            objStockRow.Ship_To_Pin = txtShipToPin.Text;

            objStockRow.Note = txtNote.Text;

            objStockRow.To_Customer_Id = Convert.ToInt32(ddlToLessee.SelectedValue.ToString());

            objStockRow.Xml_Stock_Details = dtFinal.FunPubFormXml();

            objStockeDatatable.AddS3G_LAD_Stock_MstRow(objStockRow);

            ObjStockClient = new LoanAdminAccMgtServicesReference.LoanAdminAccMgtServicesClient();
            string strNoteCode, strErrorMsg = string.Empty;
            Int32 intstock = 0;
            string StrDocumentNumber = string.Empty;
            int iErrorCode = ObjStockClient.FunPubCreateStockMaster(out intstock, out strErrorMsg, out StrDocumentNumber, ObjSerMode, ClsPubSerialize.Serialize(objStockeDatatable, ObjSerMode));
            switch (iErrorCode)
            {
                case 0:
                    btnSave.Enabled = false;
                    if (strNote_id > 0)
                    {
                        strAlert = strAlert.Replace("__ALERT__", "Stock Transfer details updated successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    else
                    {
                        strAlert = "Stock Transfer" + StrDocumentNumber + "Created successfully";
                        strAlert += @"\n\nWould you like to Create one more Stock Transfer?";
                        strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                        strRedirectPage = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert, true);
                        lblErrorMessage.Text = string.Empty;
                    }
                    break;
                case 5:

                    Utility.FunShowAlertMsg(this.Page, "Document Number Not Defined");
                    return;
                    break;

                case 4:
                    Utility.FunShowAlertMsg(this, "Error in Saving Details");
                    break;

                case 3:
                    Utility.FunShowAlertMsg(this, "CashFlow Master Not Defined");
                    break;
                case 6:
                    Utility.FunShowAlertMsg(this, "Document Path not defined");
                    break;
                case 7:
                    Utility.FunShowAlertMsg(this, "Template Master not defined");
                    break;
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }


    private void FunPriEnableDisableControls()
    {
        try
        {
            if (strMode == "Q" || strMode == "M")
            {
                FunPriDisableControls();
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    private void FunPriDisableControls()
    {
        try
        {
            ddlLesseeName.ReadOnly = ddlToLessee.ReadOnly = ddlInvoiceNumber.ReadOnly = true;
            ddlVendName.ReadOnly = true;
            ddlRSNO.ReadOnly = true;
            txtInvoiceDateFrom.ReadOnly = true;
            txtInvoiceDateTo.ReadOnly = true;
            txtDocumentDate.ReadOnly = true;
            ddlInvoiceType.ClearDropDownList();
            ddlType.ClearDropDownList();
            //ddlComState.ClearDropDownList();
            //ddltodeliverystate.ClearDropDownList();
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnclear.Enabled = false;
            btnGo.Enabled = false;
            gvInvoiceMapping.Columns[0].Visible = false;
            ddlTransferType.Enabled = ddlComState.Enabled = false;
            ddltodeliverystate.Enabled = false;
            txtShip_From.ReadOnly = txtShip_To.ReadOnly = true;
            imgdatefrom.Enabled = imgdateTo.Enabled = imgLCdatefrom.Enabled = false;
            ddlShipFromState.ClearDropDownList();
            ddlShipToState.ClearDropDownList();
            txtShipFromGSTIN.ReadOnly = txtShipToGSTIN.ReadOnly = true;
            if (strMode == "M")
            {
                txtShip_From.ReadOnly = txtShip_To.ReadOnly = false;
                btnUpdate.Enabled = btnSave.Enabled = true;
            }
        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txtapplquantity_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((TextBox)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvInvoiceMapping", strFieldAtt);
            Label lblquantity = gvInvoiceMapping.Rows[gRowIndex].FindControl("lblquantity") as Label;
            Label lblgvmapInvoiceAmount = gvInvoiceMapping.Rows[gRowIndex].FindControl("lblgvmapInvoiceAmount") as Label;
            TextBox txtapplquantity = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtapplquantity") as TextBox;
            TextBox txtgvmapFacilityAmount = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtgvmapFacilityAmount") as TextBox;
            TextBox txtUnitPrice = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtUnitPrice") as TextBox;

            if (txtapplquantity.Text != "" && txtapplquantity.Text != "0" && txtUnitPrice.Text != "")
            {
                if (txtgvmapFacilityAmount.Text == "")
                {
                    txtgvmapFacilityAmount.Text = lblgvmapInvoiceAmount.Text;
                }

                if (Convert.ToDecimal(lblquantity.Text.ToString()) < Convert.ToDecimal(txtapplquantity.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Transfer Quantity should not be greater than Original Quantity");
                    txtapplquantity.Text = lblquantity.Text;
                    txtgvmapFacilityAmount.Text = Convert.ToString((Convert.ToDecimal(txtUnitPrice.Text.ToString())) * Convert.ToDecimal(txtapplquantity.Text.ToString()));
                    return;
                }

                //txtgvmapFacilityAmount.Text = Convert.ToString((Convert.ToDecimal(txtgvmapFacilityAmount.Text.ToString())
                //    / Convert.ToDecimal(lblquantity.Text.ToString())) * Convert.ToDecimal(txtapplquantity.Text.ToString()));

                txtgvmapFacilityAmount.Text = Convert.ToString((Convert.ToDecimal(txtUnitPrice.Text.ToString())) * Convert.ToDecimal(txtapplquantity.Text.ToString()));
            }
            else
            {
                txtgvmapFacilityAmount.Text = "";
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void txtUnitPrice_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strFieldAtt = ((TextBox)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("gvInvoiceMapping", strFieldAtt);
            Label lblquantity = gvInvoiceMapping.Rows[gRowIndex].FindControl("lblquantity") as Label;
            Label lblgvmapInvoiceAmount = gvInvoiceMapping.Rows[gRowIndex].FindControl("lblgvmapInvoiceAmount") as Label;
            TextBox txtapplquantity = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtapplquantity") as TextBox;
            TextBox txtgvmapFacilityAmount = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtgvmapFacilityAmount") as TextBox;
            TextBox txtUnitPrice = gvInvoiceMapping.Rows[gRowIndex].FindControl("txtUnitPrice") as TextBox;

            if (txtapplquantity.Text != "" && txtapplquantity.Text != "0" && txtUnitPrice.Text != "")
            {
                if (txtgvmapFacilityAmount.Text == "")
                {
                    txtgvmapFacilityAmount.Text = lblgvmapInvoiceAmount.Text;
                }

                if (Convert.ToDecimal(lblquantity.Text.ToString()) < Convert.ToDecimal(txtapplquantity.Text.ToString()))
                {
                    Utility.FunShowAlertMsg(this.Page, "Transfer Quantity should not be greater than Original Quantity");
                    txtapplquantity.Text = lblquantity.Text;
                    return;
                }

                txtgvmapFacilityAmount.Text = Convert.ToString((Convert.ToDecimal(txtUnitPrice.Text.ToString())) * Convert.ToDecimal(txtapplquantity.Text.ToString()));
            }
            else
            {
                txtgvmapFacilityAmount.Text = "";
            }
        }
        catch (Exception objException)
        {
            cvTranche.ErrorMessage = objException.Message;
            cvTranche.IsValid = false;
        }
    }

    protected void gvInvoiceMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtgvmapFacilityAmount = e.Row.FindControl("txtgvmapFacilityAmount") as TextBox;
            Label lblgvmapAccountNumber = e.Row.FindControl("lblgvmapAccountNumber") as Label;

            txtgvmapFacilityAmount.SetDecimalPrefixSuffix(ObjS3GSession.ProGpsPrefixRW, ObjS3GSession.ProGpsSuffixRW, false, "Transfer Amount");//Allow zero check as true - 

            TextBox txtapplquantity = e.Row.FindControl("txtapplquantity") as TextBox;

            //if (lblgvmapAccountNumber.Text == "")
            //{
            //    txtapplquantity.ReadOnly = true;
            //}
            //else
            //{
            //    txtapplquantity.ReadOnly = true;
            //}

            //txtapplquantity.ReadOnly = true;

            if (strMode == "Q")
            {
                txtgvmapFacilityAmount.ReadOnly = txtapplquantity.ReadOnly = true;
            }
        }
    }

    protected void btnclear_Click(object sender, EventArgs e)
    {
        ddlLesseeName.SelectedValue = "0";
        ddlLesseeName.SelectedText = "--Select--";
        ddlToLessee.SelectedValue = "0";
        ddlToLessee.SelectedText = "--Select--";
        ddlVendName.SelectedValue = "0";
        ddlVendName.SelectedText = "--Select--";
        txtInvoiceDateFrom.Text = "";
        txtInvoiceDateTo.Text = "";

        txtShip_From.Text = "";
        txtShip_To.Text = "";

        ddlTransferType.SelectedValue = "0";
        ddlInvoiceType.SelectedValue = "0";
        ddlType.SelectedValue = "1";
        ddlInvoiceNumber.SelectedValue = "0";
        ddlInvoiceNumber.SelectedText = "--Select--";
        pnlStock.Visible = false;
        btnUpdate.Visible = false;
        btnSave.Visible = false;
        hdnIsMapInvoiceChanged.Value = "0";

        ddlRSNO.SelectedValue = "0";
        ddlRSNO.SelectedText = "--Select--";
        ddlComState.SelectedValue = "0";
        ddltodeliverystate.SelectedValue = "0";

        ddlShipFromState.SelectedValue = ddlShipToState.SelectedValue = "0";
        txtShipFromGSTIN.Text = txtShipToGSTIN.Text = "";

        ViewState["dtFinal"] = null;
        gvInvoiceMapping.DataSource = null;
        gvInvoiceMapping.DataBind();
        btnUpdate.Enabled = true;

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string strHTML = string.Empty;

            if (ddlTransferType.SelectedValue == "1" & strNote_id > 635)
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 58, Convert.ToString(strNote_id));
            else if (ddlTransferType.SelectedValue == "1" & strNote_id <= 635)
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 63, Convert.ToString(strNote_id));
            else
                strHTML = PDFPageSetup.FunPubGetTemplateContent(intCompanyID, 0, 0, 61, Convert.ToString(strNote_id));

            if (strHTML == "")
            {
                Utility.FunShowAlertMsg(this, "Template Master not defined");
                return;
            }

            string FileName = PDFPageSetup.FunPubGetFileName(Convert.ToString(strNote_id) + intUserId + DateTime.Now.ToString("ddMMMyyyyHHmmss"));

            string FilePath = Server.MapPath(".") + "\\PDF Files\\";
            string DownFile = FilePath + FileName + ".pdf";
            SaveDocument(strHTML, Convert.ToString(strNote_id), FilePath, FileName);
            if (!File.Exists(DownFile))
            {
                Utility.FunShowAlertMsg(this, "File not exists");
                return;
            }

            //Response.AppendHeader("content-disposition", "attachment; filename=DebitCreditNote.pdf");
            //Response.ContentType = "application/pdf";
            //Response.WriteFile(DownFile);

            FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(DownFile, false, 0);
            string strScipt1 = "";
            if (DownFile.Contains("/File.pdf"))
            {
                strScipt1 = "window.open('../Common/S3GShowPDF.aspx?qsFileName=" + DownFile + "', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            else
            {
                strScipt1 = "window.open('../Common/S3GDownloadPage.aspx?qsFileName=" + DownFile.Replace("\\", "/") + "&qsHelp=Yes', 'newwindow','toolbar=no,location=no,menubar=no,width=900,height=600,resizable=yes,scrollbars=no,top=50,left=50');";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PDF", strScipt1, true);

        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    protected void SaveDocument(string strHTML, string ReferenceNumber, string FilePath, string FileName)
    {
        try
        {
            string strQRCode = string.Empty;
            int nDigi_Flag = 0;
            DataSet dsPrintDetails = new DataSet();

            if (dictparam != null)
                dictparam.Clear();
            else
                dictparam = new Dictionary<string, string>();

            dictparam.Add("@Stock_Hdr_ID", Convert.ToString(strNote_id));
            dictparam.Add("@Company_ID", Convert.ToString(intCompanyID));
            dictparam.Add("@User_ID", Convert.ToString(intUserId));
            dsPrintDetails = Utility.GetDataset("S3G_LAD_Get_Stock_Print", dictparam);

            //if (ddlTransferType.SelectedValue == "1" && String.IsNullOrEmpty(dsPrintDetails.Tables[0].Rows[0]["QRCode"].ToString()))
            //{
            //    Utility.FunShowAlertMsg(this, "Please upload IRN details to generate the invoice.");
            //    return;
            //}

            if (dsPrintDetails.Tables[0].Rows[0]["Digi_Sign_Enable"].ToString() == "1")
                nDigi_Flag = 1;

            if (strHTML.Contains("~InvoiceTable1~"))
            {
                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dsPrintDetails.Tables[1]/*Invoice Breakup*/);
            }

            FunPubGetQrCode(Convert.ToString(dsPrintDetails.Tables[0].Rows[0]["QRCode"]));

            strQRCode = Server.MapPath(".") + "\\PDF Files\\BTNQRCode.png";

            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dsPrintDetails.Tables[0]/*HDR*/);

            List<string> listImageName = new List<string>();
            listImageName.Add("~CompanyLogo~");
            listImageName.Add("~InvoiceSignStamp~");
            //listImageName.Add("~POSignStamp~");
            listImageName.Add("~BTNQRCode~");
            List<string> listImagePath = new List<string>();

            if (Convert.ToInt32(ReferenceNumber) > 1126)
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo.png"));
            }
            else
            {
                listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/CompanyLogo_Old.png"));
            }

            listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/InvoiceSignStamp.png"));
            //listImagePath.Add(Server.MapPath("../Images/TemplateImages/" + CompanyId + "/POSignStamp.png"));
            listImagePath.Add(strQRCode);

            strHTML = PDFPageSetup.FunPubBindImages(listImageName, listImagePath, strHTML);

            string text = "";
            if (Convert.ToInt32(ReferenceNumber) > 1132)
            {
                text = "\nRegd. Office: Ground Floor, Block No B, 809, EGA Trade Centre, Poonamallee High Road, Kilpauk, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            }
            else
            {
                text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069. ";
            }

            string SignedFile = "Signed" + intUserId.ToString() ;
            string DownFile = FilePath + FileName + ".pdf";

            if (nDigi_Flag == 1 )
            {
                FunPubSaveDocument(strHTML, FilePath, SignedFile, "P", text);
                S3GPdfSign.PDFDigiSign ObjPDFSign = new S3GPdfSign.PDFDigiSign();
                ObjPDFSign.DigiPDFSign(FilePath + SignedFile + ".pdf", DownFile, "RIGHT");
            }
            else
                FunPubSaveDocument(strHTML, FilePath, FileName, "P", text);

            
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLog.CustomErrorRoutine(ex, strPageName);
        }
    }

    private void FunPubGetQrCode(string ReferenceNumber)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\BTNQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }

    public static void FunPubSaveDocument(string strHTML, string FilePath, string FileName, string DocumentType, string FooterNote)
    {
        string strhtmlFile = FilePath + "\\" + FileName + ".html";
        string strwordFile = FilePath + "\\" + FileName + ".doc";
        string strpdfFile = FilePath + "\\" + FileName + ".pdf";
        object file = strhtmlFile;
        object oMissing = System.Reflection.Missing.Value;
        object readOnly = false;
        object oFalse = false;
        object fileFormat = null;

        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
        Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();

        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            if (File.Exists(strwordFile) == true)
            {
                File.Delete(strwordFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            if (Utility.StringToDate(obj_Page.txtDocumentDate.Text) < Utility.StringToDate("30-Jun-2020"))
            {
                string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
                oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
                oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                oDoc.ActiveWindow.Selection.Font.Size = 7;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(textDisc);
            }

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            if (FooterNote != "")
            {
                oDoc.ActiveWindow.Selection.Font.Size = 9;
                oDoc.ActiveWindow.Selection.Font.Name = "Arial";
                oDoc.ActiveWindow.Selection.TypeText(FooterNote);
            }
            else
            {
                oDoc.ActiveWindow.Selection.TypeText("\t");
                oDoc.ActiveWindow.Selection.TypeText("           ");
            }

            string footerimagepath = HttpContext.Current.Server.MapPath("../Images/TemplateImages/1/OPCFooter.png");
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            if (DocumentType == "P")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                file = strpdfFile;
            }
            else if (DocumentType == "W")
            {
                fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;
                file = strwordFile;
            }

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw objException;
        }
        finally
        {
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);
        }
    }

    protected void FunPriClearGrid()
    {
        pnlStock.Visible = false;
        btnUpdate.Visible = false;
        btnSave.Visible = false;
        hdnIsMapInvoiceChanged.Value = "0";
        ViewState["dtFinal"] = null;
        gvInvoiceMapping.DataSource = null;
        gvInvoiceMapping.DataBind();
        ucInvoiceMapping.Visible = false;
    }

    protected void ddlLesseeName_Item_Selected(object Sender, EventArgs e)
    {
        FunPriClearGrid();
        ddlRSNO.SelectedValue = "0";
        ddlRSNO.SelectedText = "--Select--";
    }
    protected void ddlRSNO_Item_Selected(object Sender, EventArgs e)
    {
        FunPriClearGrid();
    }
    protected void txtInvoiceDateFrom_TextChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();
        txtInvoiceDateTo.Text = "";

    }
    protected void txtInvoiceDateTo_TextChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();
    }
    protected void ddlVendName_Item_Selected(object Sender, EventArgs e)
    {
        FunPriClearGrid();
    }
    protected void txtDocumentDate_TextChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();
    }
    protected void ddlInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();
        ddlInvoiceNumber.SelectedValue = "0";
        ddlInvoiceNumber.SelectedText = "--Select--";
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FunPriClearGrid();
    }
    protected void ddlInvoiceNumber_Item_Selected(object Sender, EventArgs e)
    {
        FunPriClearGrid();
    }
}
