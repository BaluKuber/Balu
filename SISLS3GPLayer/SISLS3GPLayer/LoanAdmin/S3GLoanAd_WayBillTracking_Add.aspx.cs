/// Module Name     :   Loan Admin
/// Screen Name     :   Way Bill Tracking
/// Created By      :   Vinodha M
/// Created Date    :   16-12-2014

#region NAMESPACES

using AjaxControlToolkit;
using S3GBusEntity;
using S3GBusEntity.LoanAdmin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

#endregion

public partial class LoanAdmin_S3GLoanAd_WayBillTracking_Add : ApplyThemeForProject
{
    # region INITILIZATION AND DELARATION OF VARIABLES

    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageAdd = "window.location.href='../LoanAdmin/S3GLoanAd_WayBillTracking_Add.aspx'";

    UserInfo ObjUserInfo = null;

    int intErrCode = 0;
    int intErrCount = 0;
    int intWBTID = 0;
    int intUserId = 0;
    int intCompanyID = 0;

    #region  User Authorization
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    #endregion

    string strDateFormat = string.Empty;
    string strMode = string.Empty;
    string PA_SA_Ref_ID = string.Empty;
    string Invoice_ID = string.Empty;

    Dictionary<string, string> dictparam;

    DataTable dtWBTDetails;

    public static LoanAdmin_S3GLoanAd_WayBillTracking_Add obj_Page;
    LoanAdminMgtServicesReference.LoanAdminMgtServicesClient ObjWayBillTrackingClient;

    PagingValues ObjPaging = new PagingValues();
    S3GSession ObjS3GSession = new S3GSession();

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
        FunPriGetWBTGridDetails();
    }
    #endregion

    #region PAGE LOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;
        bCreate = ObjUserInfo.ProCreateRW;
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

        txtLCdatefrom.Attributes.Add("onblur", "fnDoDate(this,'" + txtLCdatefrom.ClientID + "','" + strDateFormat + "',null,null);");
        txtLCdateto.Attributes.Add("onblur", "fnDoDate(this,'" + txtLCdateto.ClientID + "','" + strDateFormat + "',null,null);");
        CalendarExtender1.Format = strDateFormat;
        CalendarExtender2.Format = strDateFormat;

        if (!IsPostBack)
        {
            ucCustomPaging.Visible = false;
            if (!bCreate)
            {
                btnmove.Enabled = false;
            }
        }
    }

    #endregion

    #region TEXT CHANGED EVENTS

    protected void txtLCdatefrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckDate();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void txtLCdateto_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckDate();
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region METHODS

    private DataTable FunPriGetWBTDataTable()
    {
        try
        {
            if (ViewState["WayBillTracking"] == null)
            {
                dtWBTDetails = new DataTable();
                dtWBTDetails.Columns.Add("S.No");
                dtWBTDetails.Columns.Add("PA_SA_Ref_ID");
                dtWBTDetails.Columns.Add("Invoice_ID");
                dtWBTDetails.Columns.Add("WayBill_No");
                dtWBTDetails.Columns.Add("WayBill_Issue_Date");
                dtWBTDetails.Columns.Add("Is_CounterFoil_Received");
                dtWBTDetails.Columns.Add("CounterFoil_Date");
                dtWBTDetails.Columns.Add("Remarks");
                ViewState["WayBillTracking"] = dtWBTDetails;
            }
            dtWBTDetails = (DataTable)ViewState["WayBillTracking"];
            return dtWBTDetails;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CheckDate()
    {
        if (txtLCdatefrom.Text != String.Empty && txtLCdateto.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtLCdatefrom.Text, txtLCdateto.Text);
            if (intErrCount == 2 || intErrCount == -1)
            {
                txtLCdateto.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "Lease Capitilization Date To Should Be Greater Than Lease Capitilization Date From");
                txtLCdateto.Focus();
                return;
            }
        }
    }

    private void CheckGridDate(TextBox txtWayBill_Issue_Date, TextBox txtCounterFoil_Date)
    {
        if (txtWayBill_Issue_Date.Text != String.Empty && txtCounterFoil_Date.Text != String.Empty)
        {
            intErrCount = Utility.CompareDates(txtWayBill_Issue_Date.Text, txtCounterFoil_Date.Text);
            if (intErrCount == -1)
            {
                txtCounterFoil_Date.Text = String.Empty;
                Utility.FunShowAlertMsg(this, "Counter Foil Date Should Be Greater Than or Equal To Way Bill Issue Date");
                txtCounterFoil_Date.Focus();
                return;
            }
        }
    }

    private void FunPriGetWBTGridDetails()
    {
        try
        {
            dictparam = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(strMode))
                dictparam.Add("@strMode", strMode.ToString());
            else
                dictparam.Add("@strMode", "FETCH");
            dictparam.Add("@Cust_ID", ddlLesseeName.SelectedValue.ToString());
            dictparam.Add("@Entity_ID", ddlVendorName.SelectedValue.ToString());
            dictparam.Add("@Loc_ID", ddlDeliveryState.SelectedValue.ToString());
            if (txtLCdatefrom.Text != String.Empty)
                dictparam.Add("@LCFrom_Date", Utility.StringToDate(txtLCdatefrom.Text).ToString());
            if (txtLCdateto.Text != String.Empty)
                dictparam.Add("@LCTo_Date", Utility.StringToDate(txtLCdateto.Text).ToString());
            if (ddlRSNO.SelectedValue != "0")
                dictparam.Add("@PA_SA_Ref_ID", ddlRSNO.SelectedValue);
            else
                dictparam.Add("@PA_SA_Ref_ID", null);
            if (!string.IsNullOrEmpty(Invoice_ID))
                dictparam.Add("@Invoice_ID", Invoice_ID.ToString());
            else
                dictparam.Add("@Invoice_ID", null);
            int intTotalRecords = 0;
            bool bIsNewRow = false;

            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserId;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;

            gvWayBillTracking.BindGridView("S3G_LAD_Get_WBT_Paging", dictparam, out intTotalRecords, ObjPaging, out bIsNewRow);
            //Cache["WayBillTracking"] = gvWayBillTracking.DataSource;

            ucCustomPaging.Visible = true;
            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                gvWayBillTracking.Rows[0].Visible = false;
                btnSave.Enabled = false;
            }
            else
            {
                if (bCreate)
                {
                    btnSave.Enabled = btnmove.Enabled = true;
                }
            }

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);
            ucCustomPaging.Visible = true;

            //Paging Config End
        }
        catch (FaultException<CompanyMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.Text = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    private void FunPubBindWBTEmptyGrid(DataTable dt)
    {
        try
        {
            gvWayBillTracking.EditIndex = -1;
            gvWayBillTracking.DataSource = dt;
            gvWayBillTracking.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region GRID EVENTS

    protected void gvWayBillTracking_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string Is_CF_Recvd;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Find the DropDownList in the Row
            DropDownList ddlCountries = (e.Row.FindControl("ddlIs_CounterFoil_Received") as DropDownList);

            if ((e.Row.FindControl("lblIs_CounterFoil_Received") as Label).Text != String.Empty)
            {
                Is_CF_Recvd = (e.Row.FindControl("lblIs_CounterFoil_Received") as Label).Text;
                ddlCountries.Items.FindByValue(Is_CF_Recvd).Selected = true;
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox gchkAll = (CheckBox)e.Row.FindControl("gchkAll");
            gchkAll.Attributes.Add("onclick", "javascript:fnDGSelectOrUnselectAll('" + gvWayBillTracking.ClientID + "',this,'gchkTo');");
        }
    }


    protected void chkFrom_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {            
            CheckBox btn = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;            
            Label lblSNo = (Label)gvr.FindControl("lblSNo");           
            
            string SerialNo = lblSNo.Text;            

            if (btn.Checked)
            {
                foreach (GridViewRow gvrow in gvWayBillTracking.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        if (((Label)gvrow.Cells[0].FindControl("lblSNo")).Text.ToString() == SerialNo.ToString())
                        {
                            ((CheckBox)gvrow.Cells[1].FindControl("gchkFrom")).Checked = true;
                            ((CheckBox)gvrow.Cells[2].FindControl("gchkTo")).Enabled = false;
                        }
                        else
                        {
                            ((CheckBox)gvrow.Cells[1].FindControl("gchkFrom")).Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow gvrow in gvWayBillTracking.Rows)
                {
                    if (gvrow.RowType == DataControlRowType.DataRow)
                    {
                        ((CheckBox)gvrow.Cells[1].FindControl("gchkFrom")).Enabled = true;
                        ((CheckBox)gvrow.Cells[1].FindControl("gchkFrom")).Checked = false;
                        ((CheckBox)gvrow.Cells[2].FindControl("gchkTo")).Checked = false;
                        ((CheckBox)gvrow.Cells[2].FindControl("gchkTo")).Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    //protected void gvWayBillTracking_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        gvWayBillTracking.EditIndex = e.NewEditIndex;
    //        int intRowId = Convert.ToInt32(gvWayBillTracking.DataKeys[e.NewEditIndex].Value.ToString()) - 1;

    //        DataView dtWBT = new DataView();
    //        dtWBT = (DataView)Cache["WayBillTracking"];
    //        gvWayBillTracking.DataSource = dtWBT;
    //        gvWayBillTracking.DataBind();

    //        GridViewRow grvRow = gvWayBillTracking.Rows[intRowId];

    //        CalendarExtender cal1 = (CalendarExtender)grvRow.FindControl("CalendarExtender1");
    //        cal1.Format = strDateFormat;

    //        CalendarExtender cal2 = (CalendarExtender)grvRow.FindControl("CalendarExtender2");
    //        cal2.Format = strDateFormat;

    //        TextBox txtWayBill_Issue_Date = (TextBox)grvRow.FindControl("txtWayBill_Issue_Date");
    //        txtWayBill_Issue_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtWayBill_Issue_Date.ClientID + "','" + strDateFormat + "',null,null);");

    //        TextBox txtCounterFoil_Date = (TextBox)grvRow.FindControl("txtCounterFoil_Date");
    //        txtCounterFoil_Date.Attributes.Add("onblur", "fnDoDate(this,'" + txtCounterFoil_Date.ClientID + "','" + strDateFormat + "',null,null);");

    //        DropDownList ddlIs_CounterFoil_Received = (DropDownList)grvRow.FindControl("ddlIs_CounterFoil_Received");
    //        ddlIs_CounterFoil_Received.SelectedValue = dtWBT[intRowId]["Is_CounterFoil_Received"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //protected void gvWayBillTracking_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    private void FunGetWBTGridData()
    {
        try
        {
            dtWBTDetails = new DataTable();
            foreach (GridViewRow row in gvWayBillTracking.Rows)
            {
                CheckBox gchkFrom = ((CheckBox)row.FindControl("gchkFrom"));
                CheckBox gchkTo = ((CheckBox)row.FindControl("gchkTo"));                
                
                    Label lblPA_SA_Ref_ID = (Label)row.FindControl("lblPA_SA_Ref_ID");
                    Label lblInvoice_ID = (Label)row.FindControl("lblInvoice_ID");
                    TextBox txtWayBill_No = (TextBox)row.FindControl("txtWayBill_No");
                    TextBox txtWayBill_Issue_Date = (TextBox)row.FindControl("txtWayBill_Issue_Date");
                    DropDownList ddlIs_CounterFoil_Received = (DropDownList)row.FindControl("ddlIs_CounterFoil_Received");
                    TextBox txtCounterFoil_Date = (TextBox)row.FindControl("txtCounterFoil_Date");
                    TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");                    

                    //Way Bill No  Validation For Not to accept Special characters

                    Regex reg = new Regex(@"[^a-zA-Z0-9]");
                    if (reg.IsMatch(txtWayBill_No.Text))
                    {
                        txtWayBill_No.Text = String.Empty;
                        Utility.FunShowAlertMsg(this, "Way Bill No cannot accept special characters");
                        txtWayBill_No.Focus();
                        return;
                    }

                    //if (!String.IsNullOrEmpty(((TextBox)row.FindControl("txtWayBill_No")).Text))
                    //{
                    //    Utility.FunShowAlertMsg(this, "WayBill No should not be empty");
                    //    return;
                    //}

                    //if (String.IsNullOrEmpty(((TextBox)row.FindControl("txtWayBill_Issue_Date")).Text))
                    //{
                    //    Utility.FunShowAlertMsg(this, "WayBill Issue Date should not be empty");
                    //    return;
                    //}

                    //if (((DropDownList)row.FindControl("ddlIs_CounterFoil_Received")).SelectedIndex == 0)
                    //{
                    //    Utility.FunShowAlertMsg(this, "Select Recd Counter Foil");
                    //    return;
                    //}

                    //if (String.IsNullOrEmpty(((TextBox)row.FindControl("txtCounterFoil_Date")).Text))
                    //{
                    //    Utility.FunShowAlertMsg(this, "Counter Foil Date should not be empty");
                    //    return;
                    //}                    

                    DataRow dr;
                    dtWBTDetails = FunPriGetWBTDataTable();
                    
                    if (dtWBTDetails.Rows.Count == 0)
                    {
                        dr = dtWBTDetails.NewRow();
                        dr["S.No"] = 1;
                        dr["PA_SA_Ref_ID"] = lblPA_SA_Ref_ID.Text;
                        dr["Invoice_ID"] = lblInvoice_ID.Text;
                        if (txtWayBill_No.Text != String.Empty)
                            dr["WayBill_No"] = txtWayBill_No.Text;
                        if (txtWayBill_Issue_Date.Text != String.Empty)
                            dr["WayBill_Issue_Date"] = Utility.StringToDate(txtWayBill_Issue_Date.Text).ToString();
                        if (ddlIs_CounterFoil_Received.SelectedValue != "-1")
                            dr["Is_CounterFoil_Received"] = ddlIs_CounterFoil_Received.SelectedValue;
                        if (txtCounterFoil_Date.Text != String.Empty)
                            dr["CounterFoil_Date"] = Utility.StringToDate(txtCounterFoil_Date.Text).ToString();
                        if (txtRemarks.Text != String.Empty)
                            dr["Remarks"] = txtRemarks.Text;
                        dtWBTDetails.Rows.Add(dr);
                    }
                    else
                    {
                        dr = dtWBTDetails.NewRow();
                        dr["S.No"] = dtWBTDetails.Rows.Count+ 1;
                        dr["PA_SA_Ref_ID"] = lblPA_SA_Ref_ID.Text;
                        dr["Invoice_ID"] = lblInvoice_ID.Text;
                        if(txtWayBill_No.Text!=String.Empty)
                        dr["WayBill_No"] = txtWayBill_No.Text;
                        if(txtWayBill_Issue_Date.Text!=String.Empty)
                        dr["WayBill_Issue_Date"] = Utility.StringToDate(txtWayBill_Issue_Date.Text).ToString();
                        if(ddlIs_CounterFoil_Received.SelectedValue!="-1")
                        dr["Is_CounterFoil_Received"] = ddlIs_CounterFoil_Received.SelectedValue;
                        if (txtCounterFoil_Date.Text != String.Empty)
                        dr["CounterFoil_Date"] = Utility.StringToDate(txtCounterFoil_Date.Text).ToString();
                        if (txtRemarks.Text != String.Empty)
                        dr["Remarks"] = txtRemarks.Text;
                        dtWBTDetails.Rows.Add(dr);
                    }  
            }
            ViewState["WayBillTracking"] = dtWBTDetails;            
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }
    //}

    //protected void gvWayBillTracking_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    try
    //    {
    //        gvWayBillTracking.EditIndex = -1;
    //        strMode = "UPDATE";
    //        FunPriGetWBTGridDetails();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    //protected void txtWayBill_Issue_Date_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
    //        Label lblAccount_Activated_Date = (Label)grv.FindControl("lblAccount_Activated_Date") as Label;
    //        TextBox txtWayBill_Issue_Date = (TextBox)grv.FindControl("txtWayBill_Issue_Date") as TextBox;
    //        TextBox txtCounterFoil_Date = (TextBox)grv.FindControl("txtCounterFoil_Date") as TextBox;
    //        if (lblAccount_Activated_Date.Text != String.Empty && txtWayBill_Issue_Date.Text != String.Empty)
    //        {
    //            intErrCount = Utility.CompareDates(lblAccount_Activated_Date.Text, txtWayBill_Issue_Date.Text);
    //            if (intErrCount == -1)
    //            {
    //                txtWayBill_Issue_Date.Text = String.Empty;
    //                Utility.FunShowAlertMsg(this, "Way Bill Issue Date Should Be Greater Than or Equal To Approved date");
    //                txtWayBill_Issue_Date.Focus();
    //                return;
    //            }
    //        }
    //        CheckGridDate(txtWayBill_Issue_Date, txtCounterFoil_Date);
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.Text = ex.Message;
    //    }
    //}

    protected void txtCounterFoil_Date_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow grv = (GridViewRow)((TextBox)sender).NamingContainer;
            TextBox txtWayBill_Issue_Date = (TextBox)grv.FindControl("txtWayBill_Issue_Date") as TextBox;
            TextBox txtCounterFoil_Date = (TextBox)grv.FindControl("txtCounterFoil_Date") as TextBox;
            CheckGridDate(txtWayBill_Issue_Date, txtCounterFoil_Date);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    #endregion

    #region BUTTON EVENTS

    protected void btnGo_Click(object sender, EventArgs e)
    {
        FunPriGetWBTGridDetails();
    }

    protected void btnmove_Click(object sender, EventArgs e)
    {
        int CheckedFromCount = 0, CheckedToCount = 0;
        string WayBill_No, WayBill_Issue_Date, Is_CounterFoil_Recvd_Value, Is_CounterFoil_Recvd, CounterFoil_Date, Remarks;
        WayBill_No=WayBill_Issue_Date=Is_CounterFoil_Recvd_Value=Is_CounterFoil_Recvd=CounterFoil_Date=Remarks= String.Empty;

        if (gvWayBillTracking.Rows.Count == 0)
        {
            Utility.FunShowAlertMsg(this, "Grid Details does not Exist");
            return;
        }

        foreach (GridViewRow row in gvWayBillTracking.Rows)
        {
            CheckBox gchkFrom = ((CheckBox)row.FindControl("gchkFrom"));
            CheckBox gchkTo = ((CheckBox)row.FindControl("gchkTo"));
            TextBox txtWayBill_No = ((TextBox)row.FindControl("txtWayBill_No"));
            TextBox txtWayBill_Issue_Date = ((TextBox)row.FindControl("txtWayBill_Issue_Date"));
            DropDownList ddlIs_CounterFoil_Received = ((DropDownList)row.FindControl("ddlIs_CounterFoil_Received"));
            TextBox txtCounterFoil_Date = ((TextBox)row.FindControl("txtCounterFoil_Date"));
            TextBox txtRemarks = ((TextBox)row.FindControl("txtRemarks"));            

            if (gchkFrom.Checked)
            {
                CheckedFromCount = CheckedFromCount + 1;

                if (String.IsNullOrEmpty(((TextBox)row.FindControl("txtWayBill_No")).Text))
                {
                    Utility.FunShowAlertMsg(this, "WayBill No should not be empty");
                    return;
                }

                if (String.IsNullOrEmpty(((TextBox)row.FindControl("txtWayBill_Issue_Date")).Text))
                {
                    Utility.FunShowAlertMsg(this, "WayBill Issue Date should not be empty");
                    return;
                }

                if (((DropDownList)row.FindControl("ddlIs_CounterFoil_Received")).SelectedIndex == 0)
                {
                    Utility.FunShowAlertMsg(this, "Select Recd Counter Foil");
                    return;
                }

                if (String.IsNullOrEmpty(((TextBox)row.FindControl("txtCounterFoil_Date")).Text))
                {
                    Utility.FunShowAlertMsg(this, "Counter Foil Date should not be empty");
                    return;
                }

                WayBill_No = txtWayBill_No.Text;
                WayBill_Issue_Date = txtWayBill_Issue_Date.Text;
                Is_CounterFoil_Recvd_Value = ddlIs_CounterFoil_Received.SelectedValue;
                Is_CounterFoil_Recvd = ddlIs_CounterFoil_Received.SelectedItem.Text;
                CounterFoil_Date = txtCounterFoil_Date.Text;
                Remarks = txtRemarks.Text;
            }
        }

        if (gvWayBillTracking.Rows.Count > 0 && CheckedFromCount == 0)
        {
            Utility.FunShowAlertMsg(this, "Select atleast one record to copy from");
            return;
        }

        foreach (GridViewRow row in gvWayBillTracking.Rows)
        {            
            CheckBox gchkTo = ((CheckBox)row.FindControl("gchkTo"));
            if (gchkTo.Checked)
            {
                CheckedToCount = CheckedToCount + 1;
                ((TextBox)row.FindControl("txtWayBill_No")).Text = WayBill_No;
                (((TextBox)row.FindControl("txtWayBill_Issue_Date")).Text) = WayBill_Issue_Date;
                ((DropDownList)row.FindControl("ddlIs_CounterFoil_Received")).SelectedValue = Is_CounterFoil_Recvd_Value;
                ((DropDownList)row.FindControl("ddlIs_CounterFoil_Received")).SelectedItem.Text = Is_CounterFoil_Recvd;
                ((TextBox)row.FindControl("txtCounterFoil_Date")).Text = CounterFoil_Date;
                ((TextBox)row.FindControl("txtRemarks")).Text = Remarks;
            }
        }

        if (gvWayBillTracking.Rows.Count > 0 && CheckedToCount == 0)
        {
            Utility.FunShowAlertMsg(this, "Select atleast one destination to copy");
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        FunGetWBTGridData();
        DataTable dt = (DataTable)ViewState["WayBillTracking"];
        if (dt == null)
        {
            Utility.FunShowAlertMsg(this.Page, "No Records To Save");
            return;
        }

        ObjWayBillTrackingClient = new LoanAdminMgtServicesReference.LoanAdminMgtServicesClient();
        try
        {
            LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable ObjS3G_LOANAD_WayBillTrackingDataTable = new LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable();
            LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingRow ObjS3G_LOANAD_WayBillTrackingRow;
            ObjS3G_LOANAD_WayBillTrackingRow = ObjS3G_LOANAD_WayBillTrackingDataTable.NewS3G_LOANAD_WayBillTrackingRow();
            ObjS3G_LOANAD_WayBillTrackingRow.Company_ID = intCompanyID.ToString();
            ObjS3G_LOANAD_WayBillTrackingRow.User_ID = intUserId.ToString();
            ObjS3G_LOANAD_WayBillTrackingRow.XMLWBTDtls = dtWBTDetails.FunPubFormXml();           
            ObjS3G_LOANAD_WayBillTrackingDataTable.AddS3G_LOANAD_WayBillTrackingRow(ObjS3G_LOANAD_WayBillTrackingRow);
            SerializationMode SerMode = SerializationMode.Binary;
            byte[] ObjDeliveryInsDataTable = ClsPubSerialize.Serialize(ObjS3G_LOANAD_WayBillTrackingDataTable, SerMode);

            intErrCode = ObjWayBillTrackingClient.FunPubSaveWayBillTracking(SerMode, ObjDeliveryInsDataTable);

            if (intErrCode == 0)
            {
                strAlert = "Way Bill Tracking Details added successfully";
                strAlert += @"\n\nWould you like to add one more record?";
                strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageAdd + "}";
                strRedirectPageAdd = "";                
            }
            else if (intErrCode == 2)
            {
                strAlert = "Way Bill Tracking Details Updated successfully";
            }
            else if (intErrCode == 50)
            {
                Utility.FunShowAlertMsg(this.Page, "Unable to save the record");
                return;
            }
            dtWBTDetails.Dispose();
            ViewState["WayBillTracking"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageAdd, true);
            lblErrorMessage.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
        finally
        {
            if (ObjWayBillTrackingClient != null)
            {
                ObjWayBillTrackingClient.Close();
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ddlLesseeName.Clear();
            ddlVendorName.Clear();
            ddlDeliveryState.Clear();
            ddlRSNO.Clear();
            txtLCdatefrom.Text = String.Empty;
            txtLCdateto.Text = String.Empty;
            DataTable dt = null;
            FunPubBindWBTEmptyGrid(dt);
            ucCustomPaging.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtPO = new DataTable();

            dictparam = new Dictionary<string, string>();
            dictparam.Add("@Company_ID", intCompanyID.ToString());
            dictparam.Add("@Customer_Id", ddlLesseeName.SelectedValue);
            dictparam.Add("@Vendor_ID", ddlVendorName.SelectedValue);
            dictparam.Add("@DeliveryState_ID", ddlDeliveryState.SelectedValue);
            if (ddlRSNO.SelectedValue != "0")
                dictparam.Add("@PA_SA_Ref_ID", ddlRSNO.SelectedValue);
            dictparam.Add("@User_Id", intUserId.ToString());

            dtPO = Utility.GetDefaultData("S3G_LAD_GetWayBillForExport", dictparam);

            string filename = "WayBill.xls";

            GridView Grv = new GridView();

            if (dtPO.Rows.Count > 0)
            {
                Grv.DataSource = dtPO;
                Grv.DataBind();
                Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
                Grv.ForeColor = System.Drawing.Color.DarkBlue;

                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                Grv.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
            else
            {
                Utility.FunShowAlertMsg(this, "No Records Found!");
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
    #endregion

    #region FETCH METHODS FOR AUTO SUGGEST CONTROLS

    //Lessee Name 
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

    //Vendor Name 
    [System.Web.Services.WebMethod]
    public static string[] GetVendorNameDetails(String prefixText, int count)
    {
        Dictionary<string, string> dictparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        dictparam.Clear();
        dictparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        dictparam.Add("@User_ID", obj_Page.intUserId.ToString());
        dictparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetVendorName", dictparam));
        return suggetions.ToArray();
    }

    //Delivery State Details
    [System.Web.Services.WebMethod]
    public static string[] GetDeliveryStateDetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_RSCM_Get_LocDtls", Procparam));
        return suggetions.ToArray();
    }

    //RS NO Details
    [System.Web.Services.WebMethod]
    public static string[] GetRSNODetails(String prefixText, int count)
    {
        Dictionary<string, string> Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();

        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@User_ID", obj_Page.intUserId.ToString());
        Procparam.Add("@Cust_ID", obj_Page.ddlLesseeName.SelectedValue.ToString());
        Procparam.Add("@Loc_ID", obj_Page.ddlDeliveryState.SelectedValue.ToString());
        Procparam.Add("@Ent_ID", obj_Page.ddlVendorName.SelectedValue.ToString());
        Procparam.Add("@PrefixText", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_WBT_GetRSNO", Procparam));
        return suggetions.ToArray();
    }

    #endregion
}