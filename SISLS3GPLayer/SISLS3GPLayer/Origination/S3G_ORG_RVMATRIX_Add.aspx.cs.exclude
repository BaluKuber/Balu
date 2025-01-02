/// Module Name     :   Origination
/// Screen Name     :   RVMATRIX Master
/// Created By      :   Swarna
/// Created Date    :   22-Sep-2014
/// Purpose         :   To insert and update RV Matrix details


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using S3GBusEntity.Origination;
using System.ServiceModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.html;
using System.Web.Security;
using System.Text;
using S3GBusEntity.LoanAdmin;
using System.Configuration;
using LoanAdminMgtServicesReference;
using ApprovalMgtServicesReference;
using ORGSERVICE = ApprovalMgtServicesReference;

public partial class Origination_S3G_ORG_RVMATRIX_Add : ApplyThemeForProject
{
    #region [Intialization]

    Dictionary<string, string> dictParam = null;
    int intCAPDetailID;
    int intDEVDetailID;
    int intUserId;
    int intUserLevelID;
    int intCompanyId;
    int intErrCode;
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    string strErrorMsg;
    string strMode;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    string strRedirectPageView = "window.location.href='../Origination/S3G_ORG_RVMATRIX_Add.aspx';";
    DataTable ObjDTRange = new DataTable();
    DataSet dsCAPDetails = new DataSet();
    UserInfo ObjUserInfo = new UserInfo();
    ORGSERVICE.ApprovalMgtServicesClient objApprovalMgtServicesClient;
    ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrDataTable ObjS3G_CAPMasterListDataTable;
    string strDateFormat = string.Empty;
    #endregion [Intialization]
    protected void Page_Load(object sender, EventArgs e)
    {
        FunProPageLoad();
    }

    protected void FunProPageLoad()
    {
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        intUserLevelID = ObjUserInfo.ProUserLevelIdRW;
        intCAPDetailID = Convert.ToInt32(hdnCAPApp.Value);
        intDEVDetailID = Convert.ToInt32(hdnDEV.Value);

        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;

        if (!IsPostBack)
        {
            //if (intUserLevelID == 5)
            //{
            //    ViewState["Mode"] = strMode = "M";
            //}
            //else
            //{
            //    ViewState["Mode"] = strMode = "Q";
            //}
            ViewState["Mode"] = strMode = "M";
            FunPubGetDesignationDetails();
            FunPubGetRVMDetails();
            

            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));
            Procparam.Add("@Is_Active", "1");
            Procparam.Add("@User_ID", intUserId.ToString());
            Procparam.Add("@Program_ID", "283");
            ddlLOB.BindDataTable(SPNames.LOBMaster, Procparam, new string[] { "LOB_ID", "LOB_Code", "LOB_Name" });
            ddlLOB.SelectedIndex = 1;
            if (strMode == "M")
            {
                FunPriDisableControls(0);
            }
            else
            {
                FunPriDisableControls(-1);
            }

            if (!bCreate)
            {
                grvRVMatrix.Columns[5].Visible = false;
                grvRVMatrix.FooterRow.Visible = false;
                btnSave.Enabled = false;
            }

        }
    }

    protected void btnSaveRange_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtenderDEVApprover.Hide();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

        DropDownList ddlFAssetCategory = (DropDownList)grvRVMatrix.FooterRow.FindControl("ddlFAssetCategory");
        TextBox txtValidupto = (TextBox)grvRVMatrix.FooterRow.FindControl("txtValidupto");
        txtValidupto.Text = "";
        ddlFAssetCategory.SelectedIndex = 0;
    }

    protected void FunProClearCaches()
    {
        foreach (GridViewRow grow in grvRange.Rows)
        {

        }
        for (int i = 0; i <= grvRVMatrix.Rows.Count - 1; i++)
        {
            string strViewst = "File" + i.ToString();
            if (Cache[strViewst] != null)
            {
                Cache.Remove(strViewst);
            }
        }
    }

    public void FunPubGetRVMDetails()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@COMPANY_ID", Convert.ToString(intCompanyId));
        dsCAPDetails.Clear();
        dsCAPDetails = Utility.GetDataset("S3G_ORG_RVMATRIX", dictParam);
        if (dsCAPDetails.Tables.Count > 0)
        {
            if (dsCAPDetails.Tables[0].Rows.Count > 0)
            {
                ViewState["currenttable"] = dsCAPDetails.Tables[0];
                grvRVMatrix.DataSource = dsCAPDetails.Tables[0];
                grvRVMatrix.DataBind();
            }
            else
            {
                FunPriInitializeRVMatrix();
            }
            if (dsCAPDetails.Tables[1].Rows.Count > 0)
            {
                ViewState["RangeTable"] = dsCAPDetails.Tables[1];
                grvRVMatrix.DataSource = dsCAPDetails.Tables[0];
                grvRVMatrix.DataBind();
            }
            else
            {
                FunPriInitializeRange();
            }
        }
    }

    private void FunPriInitializeRange()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("Tenure_Form");
            dt.Columns.Add("Tenure_To");
            dt.Columns.Add("RV_Percentage");
            dt.Columns.Add("CatgID");
            dt.Columns.Add("Eff_Date");
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            ViewState["RangeTable"] = dt;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

    }

    private void FunPriInitializeRVMatrix()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("Category_Description");
            dt.Columns.Add("Category_ID");
            dt.Columns.Add("Effective_From");
            dt.Columns.Add("Is_Active");
            dt.Columns.Add("SNo");
            dr = dt.NewRow();
            dr["Category_Description"] = "";
            dr["Effective_From"] = "";
            dr["Is_Active"] = "";
            dr["SNo"] = "1";
            dt.Rows.Add(dr);
            grvRVMatrix.DataSource = dt;
            grvRVMatrix.DataBind();
            dt.Rows[0].Delete();
            ViewState["currenttable"] = dt;
            grvRVMatrix.Rows[0].Visible = false;

           
        }
        catch (Exception objException)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(objException);
            throw new ApplicationException("Unable to initiate the row");
        }
    }

    protected void grvRange_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {

        //DataRow dRow;

        if (e.CommandName == "Add")
        {
            DataTable dtCAPApproval = (DataTable)ViewState["RangeTable"];
            DataTable dtapprove = new DataTable();
            DataRow[] datarow = dtCAPApproval.Select("CatgID ='" + ViewState["CatgID"] + "' and Eff_Date='" + ViewState["Eff_Date"] + "'");
            DataRow dRow;
            if (datarow.Length > 0)
            {
                // dtCAPApproval.Clear();
                dtapprove = dtCAPApproval.Clone();
                datarow.CopyToDataTable(dtapprove, LoadOption.OverwriteChanges);

            }
            else
            {
                dtapprove = dtCAPApproval.Clone();
            }

            string SerialNo = string.Empty;
            TextBox txtLeaseTenureTo = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureTo");
            TextBox txtLeaseTenureFrom = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
            TextBox txtRVPercent = (TextBox)grvRange.FooterRow.FindControl("txtRVPercent");

            if (txtRVPercent.Text.Trim() == "" || txtRVPercent.Text == ",")
            {
                Utility.FunShowAlertMsg(this, "Enter RV Percentage Value");
                ModalPopupExtenderDEVApprover.Show();
                return;
            }


            if (Convert.ToDecimal(txtRVPercent.Text.Replace(",", "")) > Convert.ToDecimal(100))
            {
                Utility.FunShowAlertMsg(this, "Maximum RV Percentage Value is 100");
                ModalPopupExtenderDEVApprover.Show();
                return;
            }


            if (txtLeaseTenureTo.Text.Trim() == "" || txtLeaseTenureTo.Text == ",")
            {
                Utility.FunShowAlertMsg(this, "Enter Lease Tenure To");
                ModalPopupExtenderDEVApprover.Show();
                return;
            }

            if (ViewState["RangeSwitch"].ToString() == "Edit")
            {
                int GetFrom = Convert.ToInt32(txtLeaseTenureTo.Text.Replace(",", "")) + 1;

                if (Convert.ToInt32(txtLeaseTenureTo.Text.Replace(",", "")) < Convert.ToInt32(txtLeaseTenureFrom.Text.Trim()))
                {
                    Utility.FunShowAlertMsg(this, "Lease Tenure To amount should be greater than Tenure From Amount");
                    ModalPopupExtenderDEVApprover.Show();
                    return;
                }

                dRow = dtapprove.NewRow();

                dRow["Tenure_Form"] = txtLeaseTenureFrom.Text.Trim();
                dRow["Tenure_To"] = txtLeaseTenureTo.Text.Replace(",", "").Trim();
                dRow["RV_Percentage"] = txtRVPercent.Text.Replace(",", "").Trim();
                dRow["CatgID"] = ViewState["CatgID"];
                dRow["Eff_Date"] = ViewState["Eff_Date"];
                dtapprove.Rows.Add(dRow);

                grvRange.DataSource = dtapprove;
                ObjDTRange.Clear();
                ObjDTRange = dtapprove;
                ViewState["ObjDTRange"] = ObjDTRange;
                grvRange.DataBind();

                DataRow[] NewRow = dtCAPApproval.Select("CatgID ='" + ViewState["CatgID"] + "' and Eff_Date='" + ViewState["Eff_Date"] + "'");
                if (NewRow.Length > 0)
                {
                    foreach (DataRow drow1 in NewRow)
                    {
                        dtCAPApproval.Rows.Remove(drow1);
                    }
                }

                dtCAPApproval.Merge(dtapprove);

                ViewState["RangeTable"] = dtCAPApproval;
                ModalPopupExtenderDEVApprover.Show();
                txtLeaseTenureTo.Text = String.Empty;
                TextBox txtLeaseTenureFrom1 = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
                txtLeaseTenureFrom1.Text = Convert.ToString(GetFrom);
            }
            else
            {

                dRow = dtapprove.NewRow();

                Label lblLeaseTenureTo = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblLeaseTenureTo");

                dRow["Tenure_Form"] = txtLeaseTenureFrom.Text.Trim();
                dRow["Tenure_To"] = txtLeaseTenureTo.Text.Replace(",", "").Trim();
                dRow["RV_Percentage"] = txtRVPercent.Text.Replace(",", "").Trim();
                dRow["CatgID"] = ViewState["CatgID"];
                dRow["Eff_Date"] = ViewState["Eff_Date"];
                dtapprove.Rows.Add(dRow);

                grvRange.DataSource = dtapprove;
                ObjDTRange.Clear();
                ObjDTRange = dtapprove;
                ViewState["ObjDTRange"] = ObjDTRange;
                grvRange.DataBind();

                Label lblLeaseTenureTo1 = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblLeaseTenureTo");
                Label lblRVPercent = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblRVPercent");
                TextBox txtLeaseTenureFrom1 = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
                try
                {
                    lblLeaseTenureTo1.Text = lblLeaseTenureTo1.Text.Replace(",", "");
                    lblRVPercent.Text = lblRVPercent.Text.Replace(",", "");
                    int GetFrom1 = Convert.ToInt32(lblLeaseTenureTo1.Text) + 1;
                    txtLeaseTenureFrom1.Text = Convert.ToString(GetFrom1);
                }
                catch (Exception ex)
                {
                    txtLeaseTenureFrom1.Text = "1";
                }


                dtCAPApproval.Merge(dtapprove);
                ViewState["RangeTable"] = dtCAPApproval;
                ViewState["RangeSwitch"] = "Edit";
                ModalPopupExtenderDEVApprover.Show();
            }
            txtLeaseTenureTo.Text = String.Empty;
            txtRVPercent.Text = String.Empty;
        }
       
    }


    protected void grvRange_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtNew = new DataTable();
            dtNew = (DataTable)ViewState["RangeTable"];

            DataTable dtDelete = new DataTable();
            dtDelete =(DataTable)ViewState["ObjDTRange"];
            if (dtDelete.Rows.Count != 1)
            {
                dtDelete.Rows.RemoveAt(e.RowIndex);
            }
            else
            {
                ObjDTRange = null;
                ObjDTRange = ((DataTable)ViewState["RangeTable"]).Clone();
                DataRow drow;
                drow = ObjDTRange.NewRow();
                ObjDTRange.Rows.Add(drow);
                ViewState["ObjDTRange"] = ObjDTRange;
                grvRange.DataSource = ObjDTRange;
                grvRange.DataBind();
                ViewState["RangeSwitch"] = "New";
                TextBox txtLeaseTenureFrom = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
                TextBox txtLeaseTenureTo = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureTo");
                TextBox txtRVPercent = (TextBox)grvRange.FooterRow.FindControl("txtRVPercent");
                txtLeaseTenureTo.Text = txtRVPercent.Text = String.Empty;
                txtLeaseTenureFrom.Text = "1";

                ModalPopupExtenderDEVApprover.Show();

                DataRow[] NewRow = dtNew.Select("CatgID ='" + ViewState["CatgID"] + "' and Eff_Date='" + ViewState["Eff_Date"] + "'");
                if (NewRow.Length > 0)
                {
                    foreach (DataRow drow1 in NewRow)
                    {
                        dtNew.Rows.Remove(drow1);
                    }
                }

                ViewState["ObjDTRange"] = dtNew;
                ViewState["RangeTable"] = dtNew;
                return;
            }

            
            grvRange.DataSource = dtDelete;
            ObjDTRange.Clear();
            ObjDTRange = dtDelete;
            ViewState["ObjDTRange"] = ObjDTRange;
            grvRange.DataBind();
           
            //dtDelete.Clear();
           
            DataRow[] datarow = dtNew.Select("CatgID ='" + ViewState["CatgID"] + "' and Eff_Date='" + ViewState["Eff_Date"] + "'");
            if (datarow.Length>0)
            {
                foreach (DataRow drow1 in datarow)
                {
                    dtNew.Rows.Remove(drow1);
                }
                dtNew.Merge(ObjDTRange);
                ViewState["RangeTable"] = dtNew;
                ModalPopupExtenderDEVApprover.Show();

                Label lblLeaseTenureTo1 = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblLeaseTenureTo");
                TextBox txtLeaseTenureFrom1 = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
                try
                {
                    int GetFrom1 = Convert.ToInt32(lblLeaseTenureTo1.Text) + 1;
                    txtLeaseTenureFrom1.Text = Convert.ToString(GetFrom1);
                }
                catch (Exception ex)
                {
                    txtLeaseTenureFrom1.Text = "1";
                }
            }  
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }

    
    protected void grvRVMatrix_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Add")
        {
            DataTable dtCAPApproval = (DataTable)ViewState["currenttable"];
            DataRow dRow;
            Label lbl = (Label)grvRVMatrix.Rows[grvRVMatrix.Rows.Count - 1].FindControl("lblSNo");
            int intSNo = (Convert.ToInt32(lbl.Text)) + 1;
            DropDownList ddlFAssetCategory = (DropDownList)grvRVMatrix.FooterRow.FindControl("ddlFAssetCategory");

            TextBox txtValidupto = (TextBox)grvRVMatrix.FooterRow.FindControl("txtValidupto");
            HiddenField hdnCheckActive = (HiddenField)grvRVMatrix.Rows[grvRVMatrix.Rows.Count - 1].FindControl("hdnCheckActive");
            CheckBox chkCAPFActive = (CheckBox)grvRVMatrix.FooterRow.FindControl("chkCAPFActive");
            if (ddlFAssetCategory.SelectedValue == "0")
            {
                Utility.FunShowAlertMsg(this, "Select Asset Category");
                return;
            }
            string find = "Category_Description = '" + ddlFAssetCategory.SelectedItem.ToString() + "' AND Effective_From='" + Convert.ToDateTime(Utility.StringToDate(txtValidupto.Text), CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy") + "'";
            DataRow[] foundRows = dtCAPApproval.Select(find);

            if (foundRows.Length > 0)
            {
                Utility.FunShowAlertMsg(this, "Asset Category for the effective date already exists");
                return;
            }

            if (txtValidupto.Text.Trim() == "")
            {
                Utility.FunShowAlertMsg(this, "Enter Effective Date");
                return;
            }

            if (((DataTable)(ViewState["RangeTable"])) == null)
            {
                Utility.FunShowAlertMsg(this, "Enter Range Details");
                return;
            }

            DataTable dtValidateRange = new DataTable();
            dtValidateRange = (DataTable)ViewState["RangeTable"];
            DataRow[] findRows = dtValidateRange.Select("CatgID = '" + ddlFAssetCategory.SelectedValue + "' and Eff_Date='" + Convert.ToDateTime(Utility.StringToDate(txtValidupto.Text), CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy")  + "'");
            if (findRows.Length == 0)
            {
                Utility.FunShowAlertMsg(this, "Enter Range Details");
                return;
            }

            if (chkCAPFActive.Checked == true)
            {
                hdnCheckActive.Value = "1";
            }
            else
            {
                hdnCheckActive.Value = "0";
            }


            dRow = dtCAPApproval.NewRow();
            dRow["Category_Description"] = ddlFAssetCategory.SelectedItem.ToString();
            dRow["Category_ID"] = ddlFAssetCategory.SelectedValue;
            dRow["Effective_From"] = Convert.ToDateTime(Utility.StringToDate(txtValidupto.Text), CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy");
            dRow["Is_Active"] = hdnCheckActive.Value;
            dRow["SNo"] = intSNo;
            dtCAPApproval.Rows.Add(dRow);
            DataView dtView = new DataView(dtCAPApproval);
            DataTable dtTemp = dtView.ToTable();
            dtTemp.AcceptChanges();
            grvRVMatrix.DataSource = dtTemp;
            grvRVMatrix.DataBind();
            ViewState["currenttable"] = dtTemp;
        }
    }

    public void FunPubGetDesignationDetails()
    {
        dictParam = new Dictionary<string, string>();
        dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
        dictParam.Add("@User_ID", Convert.ToString(intUserId));
        DataSet dassetcategory = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", dictParam);
        ViewState["AssetCategory"] = dassetcategory.Tables[4];
    }

    protected void btnRange_Click(object sender, EventArgs e)
    {
        FunProAssignApprover(sender, e);
    }

    protected void btnFRange_Click(object sender, EventArgs e)
    {
        FunProFAssignApprover(sender, e);
    }

    protected void FunProFAssignApprover(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        Label lblCatgID = (Label)gvr.FindControl("lblCatgID");
        TextBox txtValidupto = (TextBox)gvr.FindControl("txtValidupto");
        DropDownList ddlFAssetCategory = (DropDownList)gvr.FindControl("ddlFAssetCategory");
        if (ddlFAssetCategory.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Select Asset Category");
            return;
        }

        if (txtValidupto.Text.Trim() == "")
        {
            Utility.FunShowAlertMsg(this, "Enter Effective Date");
            return;
        }

        lblAssetCatg.Text = ddlFAssetCategory.SelectedItem.ToString();
        ViewState["CatgID"] = ddlFAssetCategory.SelectedValue;
        ViewState["Eff_Date"] = Convert.ToDateTime(Utility.StringToDate(txtValidupto.Text), CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy");
        
        DataTable ObjDTRange = new DataTable();
        DataRow[] ObjDRRange;
        ObjDTRange = (DataTable)ViewState["RangeTable"];

        ObjDRRange = ((DataTable)ViewState["RangeTable"]).Select("CatgID ='" + ddlFAssetCategory.SelectedValue + "' AND Eff_Date='" + Convert.ToDateTime(Utility.StringToDate(txtValidupto.Text), CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy") + "'");
        if (ObjDRRange.Length > 0)
        {
            ObjDTRange = ((DataTable)ViewState["RangeTable"]).Clone();
            ObjDRRange.CopyToDataTable(ObjDTRange, LoadOption.OverwriteChanges);
            ViewState["ObjDTRange"] = ObjDTRange;
            ViewState["RangeSwitch"] = "Edit";
            grvRange.DataSource = ObjDTRange;
            grvRange.DataBind();
            Label lblLeaseTenureTo2 = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblLeaseTenureTo");
            TextBox txtLeaseTenureFrom2 = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
            int GetFrom2 = Convert.ToInt32(lblLeaseTenureTo2.Text) + 1;
            txtLeaseTenureFrom2.Text = Convert.ToString(GetFrom2);
        }
        else
        {
           // ViewState["RangeTable"] = ObjDTRange;
            //ObjDTRange.Rows.Clear();
            ObjDTRange = null;
            ObjDTRange = ((DataTable)ViewState["RangeTable"]).Clone();
            DataRow drow;
            drow = ObjDTRange.NewRow();
            ObjDTRange.Rows.Add(drow);
            ViewState["ObjDTRange"] = ObjDTRange;
            grvRange.DataSource = ObjDTRange;
            grvRange.DataBind();
            ViewState["RangeSwitch"] = "New";
            TextBox txtLeaseTenureFrom = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");
            TextBox txtLeaseTenureTo = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureTo");
            TextBox txtRVPercent = (TextBox)grvRange.FooterRow.FindControl("txtRVPercent");
            txtLeaseTenureTo.Text = txtRVPercent.Text = String.Empty;
            txtLeaseTenureFrom.Text = "1";
        }

        PnlApprover.Visible = true;
        ModalPopupExtenderDEVApprover.Show();

    }

    protected void btnCancelRange_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow grow in grvRange.Rows)
        {
            Label lblRVPercent = (Label)grow.FindControl("lblRVPercent");
            Label lblLeaseTenureTo = (Label)grow.FindControl("lblLeaseTenureTo");
            Label lblLeaseTenureFrom = (Label)grow.FindControl("lblLeaseTenureFrom");

            if (lblRVPercent.Text == "" && lblLeaseTenureTo.Text == "" && lblLeaseTenureFrom.Text == "")
            {
                Utility.FunShowAlertMsg(this.Page, "Grid should have atleast one record");
                ModalPopupExtenderDEVApprover.Show();

                TextBox txtLeaseTenureTo = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureTo");
                TextBox txtRVPercent = (TextBox)grvRange.FooterRow.FindControl("txtRVPercent");
                txtLeaseTenureTo.Text = txtLeaseTenureTo.Text.Replace(",", "");
                txtRVPercent.Text = txtRVPercent.Text.Replace(",", "");
                return;
            }
            else
            {
                ModalPopupExtenderDEVApprover.Hide();
                return;
            }
        }
       
    }

    protected void FunProAssignApprover(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        Label lblCatgID = (Label)gvr.FindControl("lblCatgID");
        Label lblAssetCategory = (Label)gvr.FindControl("lblAssetCategory");
        DropDownList ddlFAssetCategory = (DropDownList)gvr.FindControl("ddlFAssetCategory");
        Label lblEffectiveDate = (Label)gvr.FindControl("lblEffectiveDate");
        ViewState["CatgID"] = lblCatgID.Text.Trim();
        ViewState["Eff_Date"] = lblEffectiveDate.Text.Trim();
        lblAssetCatg.Text = lblAssetCategory.Text.Trim();
               
        DataRow[] ObjDRRange;
        ObjDTRange = (DataTable)ViewState["RangeTable"];

        ObjDRRange = ((DataTable)ViewState["RangeTable"]).Select("CatgID ='" + lblCatgID.Text + "' AND Eff_Date='" + lblEffectiveDate .Text + "'");
        if (ObjDRRange.Length > 0)
        {
            ObjDTRange = ((DataTable)ViewState["RangeTable"]).Clone();
            ObjDRRange.CopyToDataTable(ObjDTRange, LoadOption.OverwriteChanges);
            ViewState["ObjDTRange"] = ObjDTRange;
            grvRange.DataSource = ObjDTRange;
            grvRange.DataBind();
        }
        else
        {
            ObjDTRange = ((DataTable)ViewState["RangeTable"]).Clone();
            DataRow NewRow = ObjDTRange.NewRow();
            ObjDTRange.Rows.Add(NewRow);
            ViewState["ObjDTRange"] = ObjDTRange;
            grvRange.DataSource = ObjDTRange;
            grvRange.DataBind();
        }
       

        Label lblLeaseTenureTo2 = (Label)grvRange.Rows[grvRange.Rows.Count - 1].FindControl("lblLeaseTenureTo");
        TextBox txtLeaseTenureFrom2 = (TextBox)grvRange.FooterRow.FindControl("txtLeaseTenureFrom");

        if (lblLeaseTenureTo2.Text.Trim() != "")
        {
            int GetFrom2 = Convert.ToInt32(lblLeaseTenureTo2.Text) + 1;
            txtLeaseTenureFrom2.Text = Convert.ToString(GetFrom2);
        }
        else
        {
            txtLeaseTenureFrom2.Text = "1";
        }

        ViewState["RangeSwitch"] = "Edit";
        PnlApprover.Visible = true;
        ModalPopupExtenderDEVApprover.Show();
    }

    protected void grvRange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtRVPercent = (TextBox)e.Row.FindControl("txtRVPercent");
            txtRVPercent.SetDecimalPrefixSuffix(3, 2, true, false, "");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            DataTable Dtable = (DataTable)ViewState["ObjDTRange"];
            Label lblLeaseTenureFrom = (Label)e.Row.FindControl("lblLeaseTenureFrom");
            Label lblLeaseTenureTo = (Label)e.Row.FindControl("lblLeaseTenureTo");

            if (Dtable.Rows.Count != 0)
            {
                if (e.Row.RowIndex == ObjDTRange.Rows.Count - 1)
                {
                    lnkDelete.Enabled = true;
                }
                else
                {
                    lnkDelete.Enabled = false;
                    lnkDelete.OnClientClick = "";
                }
            }
            else
            {
                lnkDelete.Visible = false;
                lnkDelete.OnClientClick = "";
            }
            
            if (lblLeaseTenureFrom.Text.Trim() == "" && lblLeaseTenureTo.Text.Trim() == "")
            {
                lnkDelete.Visible = false;
                lnkDelete.OnClientClick = "";
            }
          
        }
        if (!bCreate)
        {
            grvRange.Columns[3].Visible = false;
            btnSave.Enabled = false;
        }
    }

    protected void grvRVMatrix_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            S3GSession ObjS3GSession = new S3GSession();
            strDateFormat = ObjS3GSession.ProDateFormatRW;
            AjaxControlToolkit.CalendarExtender CalendarValidupto = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarValidupto");
            CalendarValidupto.Format = strDateFormat;

            DropDownList ddlFAssetCategory = (DropDownList)e.Row.FindControl("ddlFAssetCategory");
            HiddenField hdnCheckActive = (HiddenField)e.Row.FindControl("hdnCheckActive");
            TextBox txtValidupto = (TextBox)e.Row.FindControl("txtValidupto");
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyId));
            dictParam.Add("@User_ID", Convert.ToString(intUserId));
            DataSet dassetcategory = Utility.GetDataset("S3G_SYSAD_Get_StateDetails", dictParam);
            //ViewState["AssetCategory"]
            DataTable dt = dassetcategory.Tables[4];
            Utility.BindDataTable(ddlFAssetCategory, dt, new string[] { "AC_ID", "AC_DESC" });
            txtValidupto.Attributes.Add("onblur", "fnDoDate(this,'" + txtValidupto.ClientID + "','" + strDateFormat + "',false,  false);");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           

            HiddenField hdnCheckActive = (HiddenField)e.Row.FindControl("hdnCheckActive");
            CheckBox chkIActive = (CheckBox)e.Row.FindControl("chkIActive");
            Label lblEffectiveDate = (Label)e.Row.FindControl("lblEffectiveDate");

            if (lblEffectiveDate.Text.Trim() != "")
            {
                lblEffectiveDate.Text = DateTime.Parse(lblEffectiveDate.Text, CultureInfo.CurrentCulture.DateTimeFormat).ToString("dd-MMM-yyyy");
            }

            if (hdnCheckActive.Value == "1")
            {
                chkIActive.Checked = true;
            }
            else
            {
                chkIActive.Checked = false;
            }
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlLOB.SelectedValue == "0")
        {
            Utility.FunShowAlertMsg(this, "Select Line of Business");
            return;
        }
        FunPubSaveRVMatrix();
    }

    private void FunPubSaveRVMatrix()
    {
        try
        {
            if (((DataTable)ViewState["currenttable"]).Rows.Count == 0 || ViewState["currenttable"] == null)
            {
                Utility.FunShowAlertMsg(this, "Add atleast one record");
                return;
            }

            DataTable DtSaveStruct = new DataTable();
            DataTable DtSave = new DataTable();
            DtSaveStruct = (DataTable)ViewState["currenttable"];
            DtSave = DtSaveStruct.Clone();
            foreach(GridViewRow gvRow in grvRVMatrix.Rows)
            {
                Label lblAssetCategory = (Label)gvRow.FindControl("lblAssetCategory");
                Label lblCatgID = (Label)gvRow.FindControl("lblCatgID");
                Label lblEffectiveDate = (Label)gvRow.FindControl("lblEffectiveDate");
              
                CheckBox chkIActive = (CheckBox)gvRow.FindControl("chkIActive");
                DataRow drow = DtSave.NewRow();
                drow["Category_Description"] = lblAssetCategory.Text;
                drow["Category_ID"] = lblCatgID.Text;
                drow["Effective_From"] = lblEffectiveDate.Text;
                if (chkIActive.Checked == true)
                {
                    drow["Is_Active"] = true;
                }
                else
                {
                    drow["Is_Active"] = false;
                }
                DtSave.Rows.Add(drow);
                ;
            }

            ViewState["currenttable"] = DtSave;

            objApprovalMgtServicesClient = new ApprovalMgtServicesReference.ApprovalMgtServicesClient();
            ObjS3G_CAPMasterListDataTable = new ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrDataTable();
            S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrRow objS3G_ORG_RVM;
            objS3G_ORG_RVM = ObjS3G_CAPMasterListDataTable.NewS3G_ORG_RV_Matrix_HdrRow();
            objS3G_ORG_RVM.LOB_ID = Convert.ToString(ddlLOB.SelectedValue);
            objS3G_ORG_RVM.XML_GetRVMHeader = Convert.ToString(((DataTable)ViewState["currenttable"]).FunPubFormXml());
            DataTable dtRange = (DataTable)ViewState["RangeTable"];

            objS3G_ORG_RVM.XmlMatRange = Convert.ToString(((DataTable)dtRange).FunPubFormXml());
            objS3G_ORG_RVM.User_ID = Convert.ToString(intUserId);
            objS3G_ORG_RVM.COMPID = Convert.ToString(intCompanyId);
            ObjS3G_CAPMasterListDataTable.Rows.Add(objS3G_ORG_RVM);
            byte[] objbyteDataTable = ClsPubSerialize.Serialize(ObjS3G_CAPMasterListDataTable, SerializationMode.Binary);

            intErrCode = objApprovalMgtServicesClient.FunPubCreateRVMatrix(out strErrorMsg, SerializationMode.Binary, objbyteDataTable);
            switch (intErrCode)
            {
                case 0:
                    {
                        if (intCAPDetailID > 0)
                        {
                            strKey = "Modify";
                            strAlert = strAlert.Replace("__ALERT__", "RV Matrix details updated successfully");
                            //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            //strRedirectPageView = strRedirectPageView;
                            //Added by Thangam M on 17/Oct/2012 to avoid double save click
                            //btnSave.Enabled = false;
                            //End here
                        }
                        else
                        {
                            strAlert = strAlert.Replace("__ALERT__", "RV Matrix details added successfully");
                            //strAlert = "if(confirm('" + strAlert + "')){" + strRedirectPageAdd + "}else {" + strRedirectPageView + "}";
                            //strRedirectPageView = strRedirectPageView;
                            //Added by Thangam M on 17/Oct/2012 to avoid double save click
                            //btnSave.Enabled = false;
                            //End here

                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strAlert + strRedirectPageView, true);
                    }
                    break;
                case 20: Utility.FunShowAlertMsg(this.Page, "Error in Adding RV Matrix Details");
                    break;
            }
        }
        catch (Exception e)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(e);
            throw e;
        }
        //finally
        //{
        //    objApprovalMgtServicesClient.Close();
        //}
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Common/HomePage.aspx");
    }

    private void FunPriDisableControls(int intModeID)
    {
        switch (intModeID)
        {
            case 0: // Modify Mode                
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                grvRVMatrix.FooterRow.Visible = true;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                break;

            case -1:// Query Mode
                btnCancel.Enabled = true;
                btnSave.Enabled = false;
                grvRVMatrix.FooterRow.Visible = false;
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);
                break;
        }
    }

    protected void btnExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ExportExcel();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void ExportExcel()
    {
        try
        {
            DataTable dtGetdata = new DataTable();
            Dictionary<string, string> Procparam = new Dictionary<string, string>();
            Procparam.Add("@Company_ID", Convert.ToString(intCompanyId));

            dtGetdata = Utility.GetDefaultData("S3G_ORG_RVMatrix_Report", Procparam);

            GridView Grv = new GridView();
            Grv.DataSource = dtGetdata;
            Grv.DataBind();
            Grv.HeaderRow.Attributes.Add("Style", "background-color: #ebf0f7; font-family: calibri; font-size: 13px; font-weight: bold;");
            Grv.ForeColor = System.Drawing.Color.DarkBlue;

            if (Grv.Rows.Count > 0)
            {
                GridView grv1 = new GridView();
                DataTable dtHeader = new DataTable();
                dtHeader.Columns.Add("Column1");
                dtHeader.Columns.Add("Column2");

                DataRow row = dtHeader.NewRow();
                row["Column1"] = ObjUserInfo.ProCompanyNameRW.ToString();
                dtHeader.Rows.Add(row);

                row = dtHeader.NewRow();

                row["Column1"] = "RV Matrix Report";

                dtHeader.Rows.Add(row);
                row = dtHeader.NewRow();
                dtHeader.Rows.Add(row);
                grv1.DataSource = dtHeader;
                grv1.DataBind();
                grv1.HeaderRow.Visible = false;
                grv1.GridLines = GridLines.None;

                grv1.Rows[0].Cells[0].ColumnSpan = 7;
                grv1.Rows[1].Cells[0].ColumnSpan = 7;
                grv1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grv1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                grv1.Font.Bold = true;
                grv1.ForeColor = System.Drawing.Color.DarkBlue;
                grv1.Font.Name = "calibri";
                grv1.Font.Size = 10;

                string path = Server.MapPath("~/RVMatrixReport/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(path + "RVMatrix_Report.xls"))
                {
                    File.Delete(path + "RVMatrix_Report.xls");
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(path + "RVMatrix_Report.xls");
                        grv1.RenderControl(hw);
                        Grv.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
                }

                //Map the path where the zip file is to be stored
                string DestinationPath = Server.MapPath("~/AssetDetails/");

                //creating the directory when it is not existed
                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                //concatenation of the path and name
                string filePath = DestinationPath + "RVMatrix_Report.zip";

                //before creation of compressed folder,deleting it if exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                //checking the path is available or not
                if (!File.Exists(filePath))
                {
                    //creating a zip file from one folder to another folder
                    ZipFile.CreateFromDirectory(path, filePath);

                    //Delete The excel file which is created
                    if (File.Exists(path + "RVMatrix_Report.xls"))
                    {
                        File.Delete(path + "RVMatrix_Report.xls");
                    }
                    //Delete The folder where the excel file is created
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path);
                    }

                    //download compressed file                    
                    FileInfo file = new FileInfo(filePath);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + "RVMatrix_Report.zip");
                    Response.ContentType = "application/x-zip-compressed";
                    Response.WriteFile(filePath);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
}
