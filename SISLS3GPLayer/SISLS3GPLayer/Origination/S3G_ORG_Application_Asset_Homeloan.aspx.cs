#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using S3GBusEntity;
using System.Globalization;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
using System.IO;
#endregion

public partial class Origination_S3G_ORG_Application_Asset_Homeloan : ApplyThemeForProject
{
    #region Initialization

    /// <summary>
    /// To Initialize Objects and Variables
    /// </summary>
    /// 
    int intCompanyId = 0;
    int intUserId = 0;
    int intBranchId = 2;
    int intSerialNo = 0;
    int intErrorCode = 0;
    string strKey = "Insert";
    string strAlert = "alert('__ALERT__');";
    UserInfo ObjUserInfo;
    S3GSession objS3GSession;
    Dictionary<string, string> Procparam;
    //User Authorization
    string strMode = string.Empty;
    string strDateFormat = "";
    bool bCreate = false;
    bool bModify = false;
    bool bQuery = false;
    bool bDelete = false;
    bool bMakerChecker = false;
    bool bClearList = false;
    string[] ErrorList = new string[3];
    public static Origination_S3G_ORG_Application_Asset_Homeloan obj_Page;
    string strNewWin = " window.showModalDialog('../Origination/Origination_S3G_ORG_Application_Asset_Homeloan.aspx?qsMaster=Yes&qsRowID=";
    string NewWinAttributes = "', 'Home Details', 'location:no;toolbar:no;menubar:no;dialogwidth:700px;dialogHeight:400px;');";
    DataTable dtasset;
    DataSet dsasset;
    DataTable dtapplicationasset;
    DataTable dtdocuments;
    DataTable dtheader;
    //Code end
    #endregion




    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjUserInfo = new UserInfo();
        intCompanyId = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        objS3GSession = new S3GSession();
        obj_Page = this;
        //User Authorization
        bCreate = ObjUserInfo.ProCreateRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        strDateFormat = objS3GSession.ProDateFormatRW;
        bClearList = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ClearListValues"));
        //Code end


        if (Request.QueryString["qsMode"] != null)
        {
            btnOK.Enabled = false;
        }

        if (Request.QueryString["qsRowID"] != null)                               //When click Add for First Row//
        {
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);
        }

        else                                                                      //When click Add for Further Rows//  
        {
            DataTable dtapplicationasset = (DataTable)Session["dtapplicationasset"];
            if (dtapplicationasset != null && dtapplicationasset.Rows.Count > 0)
            {
                txtSlNo.Text = Convert.ToString(dtapplicationasset.Rows.Count + 1);
            }
        }


        if (!IsPostBack)
        {
            FunPriLoadAssetType();
            FunProClearCaches();
            intSerialNo = Convert.ToInt32(Request.QueryString["qsRowID"]);



            if (string.IsNullOrEmpty(txtSlNo.Text))
            {
                DataTable dtapplicationasset = (DataTable)Session["dtapplicationasset"];
                DataTable dtdocuments = (DataTable)Session["dtdocuments"];
                if (dtapplicationasset != null && dtapplicationasset.Rows.Count > 0)
                {
                   FunPriLoadAssetDetails(dtapplicationasset);
                   FunPriLoadDocumentDetails(dtdocuments);
                }
                else
                {
                    txtSlNo.Text = "1";
                }
            }
            /* these code executes for .Net 3.0 and above only.*/
     
        }
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
    }

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.QueryString["qsMaster"] != null)
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
        else
        {
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
        }
    }

    #endregion


    private void FunPriLoadAssetDetails(DataTable dtapplicationasset)
    {
        DataRow[] drAsset = dtapplicationasset.Select("Sl.No = " + intSerialNo.ToString());
        if (drAsset.Length == 0)
        {
            txtSlNo.Text = Convert.ToString(dtasset.Rows.Count + 1);
            return;
        }
        if (intSerialNo == 0)
        {
            txtSlNo.Text = (intSerialNo + 1).ToString();
        }
        else
        {
            txtSlNo.Text = (intSerialNo).ToString();
        }
        ddlAssettypeList.SelectedItem.Text= dtapplicationasset.Rows[0]["Asset_type"].ToString();
        txtremarks.Text = dtapplicationasset.Rows[0]["remarks"].ToString();
        gvsset.DataSource = dtapplicationasset;
        gvsset.DataBind();


    }
    private void FunPriLoadDocumentDetails(DataTable dtdocuments)
    {
        
     
        
        grvdoc.DataSource = dtdocuments;
        grvdoc.DataBind();


    }


    #region Page Events


    protected void btnOK_Click(object sender, EventArgs e)
    {
       
           
        if (Session["dtapplicationasset"] ==null)
        {
            FunPriInitializeAssetGridData();
        }
         DataTable dtapplicationasset = (DataTable)Session["dtapplicationasset"];
        
        foreach (GridViewRow grv in gvsset.Rows)
        {
            DataRow dr = dtapplicationasset.NewRow();
            Label lblflag = grv.FindControl("lblflag") as Label;
            TextBox txtvalues = grv.FindControl("txtvalues") as TextBox;
            dr["Sl.No"] = txtSlNo.Text.ToString();
            dr["Asset_Type"] = ddlAssettypeList.SelectedItem.Text.ToString();
            dr["Description"] = lblflag.Text.ToString();
            dr["Values"] = txtvalues.Text.ToString();
            dr["remarks"] = txtremarks.Text.ToString();
            dtapplicationasset.Rows.Add(dr);
            Session["dtapplicationasset"] = dtapplicationasset;
        }
        if (Session["dtdocuments"] == null)
        {
            FunPriInitializeDocGridData();
        }
        DataTable dtdocuments = (DataTable)Session["dtdocuments"];

        foreach (GridViewRow grv in grvdoc.Rows)
        {
            DataRow dr = dtdocuments.NewRow();
            Label lblflag = grv.FindControl("lblflag") as Label;
            TextBox txtDoc = grv.FindControl("txtDoc") as TextBox;
            TextBox txtdate = grv.FindControl("txtdate") as TextBox;
            CheckBox chkScanned = grv.FindControl("chkScanned") as CheckBox;
            CheckBox chkdocuments = grv.FindControl("chkdocuments") as CheckBox;
            Label lblActualPath = grv.FindControl("lblActualPath") as Label;
            int intRowindex = grv.RowIndex;
            string fileExtension = string.Empty;
            //if (chkdocuments.Checked && txtDoc.Text == "")
            //{
            //    Utility.FunShowAlertMsg(this, "All the Collected documents values should be entered.");

            //    return;
            //}
            //if (chkScanned.Checked && Cache["File" + intRowindex.ToString()] == null && string.IsNullOrEmpty(lblActualPath.Text))
            //{
            //    Utility.FunShowAlertMsg(this, "All the scanned documents file should be uploaded.");
            //    return;
            //}
            //else
            //{
            //    if (Cache["File" + intRowindex.ToString()] != null)
            //    {
            //        HttpPostedFile hpf = (HttpPostedFile)Cache["File" + intRowindex.ToString()];
            //        fileExtension = hpf.FileName;
            //    }
            //    else if (!string.IsNullOrEmpty(lblActualPath.Text))
            //    {
            //        fileExtension = lblActualPath.Text;
            //    }

            //}

            //if (!string.IsNullOrEmpty(fileExtension))
            //{
            //    fileExtension = fileExtension.Substring(fileExtension.LastIndexOf('.') + 1);
            //    if (fileExtension != "" && fileExtension.ToLower() != "bmp" && fileExtension.ToLower() != "jpeg" && fileExtension.ToLower() != "jpg" && fileExtension.ToLower() != "gif" && fileExtension.ToLower() != "png" && fileExtension.ToLower() != "pdf")
            //    {
            //        Utility.FunShowAlertMsg(this, "Image/PDF file only can be uploaded. Check the file format of " + fileExtension + "");
            //        return;
            //    }

            //}

            dr["SI.NO"] = txtSlNo.Text.ToString();
            dr["Description"] = lblflag.Text.ToString();
            dr["callby"] = txtDoc.Text.ToString();
            dr["doc_dat"] = txtdate.Text.ToString();
            dr["Mandatory_Scan"] = chkScanned.Checked.ToString();
            dr["Mandatory_Documents"] = chkdocuments.Checked.ToString();
            dr["Document_Path"] = lblActualPath.Text.ToString();

            dtdocuments.Rows.Add(dr);
            Session["dtdocuments"] = dtdocuments;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "window.close();", true);
    }

    #endregion


    protected void FunPriLoadAssetType()
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", intCompanyId.ToString());
        Procparam.Add("@user_id", intUserId.ToString());
        Procparam.Add("@Lookup_Desc", "Asset Type");
        ddlAssettypeList.BindDataTable("S3G_ORG_GetLookup_PL", Procparam, new string[] { "Id", "Value" });

    }

    protected void ddlAssetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Procparam = new Dictionary<string, string>();
        Procparam.Add("@company_id", intCompanyId.ToString());
        Procparam.Add("@user_id", intCompanyId.ToString());
        Procparam.Add("@Lookup_Desc", ddlAssettypeList.SelectedItem.Text.Trim());
        dsasset = Utility.GetDataset("Get_AssetMaster_HL_Appl", Procparam);
        if (dsasset.Tables[0].Rows.Count > 0)
        {
            gvsset.DataSource = dsasset.Tables[0];
            gvsset.DataBind();
            Session["AssetHL"] = dsasset.Tables[0];
        }
        else
        {
            gvsset.EmptyDataText = "Documents not defined in Asset master";
            gvsset.DataBind();

        }
        if (dsasset.Tables[1].Rows.Count > 0)
        {
            grvdoc.DataSource = dsasset.Tables[1];
            grvdoc.DataBind();
            Session["AssetHLDoc"] = dsasset.Tables[1];
        }
        else
        {
            grvdoc.EmptyDataText = "Documents not defined in Asset master";
            grvdoc.DataBind();

        }
       
    }

    protected void asyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {

    }

    private void FunPriInitializeAssetGridData()
    {
        DataRow dRow;
        dtapplicationasset = new DataTable();
        dtapplicationasset.Columns.Add("Sl.No");
        dtapplicationasset.Columns.Add("Asset_Type");
        dtapplicationasset.Columns.Add("Description");
        dtapplicationasset.Columns.Add("Values");
        dtapplicationasset.Columns.Add("remarks");
        dRow = dtapplicationasset.NewRow();
        dRow["Sl.No"] = "";
        dRow["Asset_Type"] = "";
        dRow["Description"] = "";
        dRow["Values"] = "";
        dRow["remarks"] = "";

        dtapplicationasset.Rows.Add(dRow);

        Session["dtapplicationasset"] = dtapplicationasset;

        ((DataTable)Session["dtapplicationasset"]).Rows.RemoveAt(0);
       


    }
    private void FunPriInitializeDocGridData()
    {
        DataRow dRow;
        dtdocuments = new DataTable();
        dtdocuments.Columns.Add("SI.NO");
        dtdocuments.Columns.Add("Description");
        dtdocuments.Columns.Add("callby");
        dtdocuments.Columns.Add("doc_dat");
        dtdocuments.Columns.Add("Document_Path");
        dtdocuments.Columns.Add("Mandatory_Scan");
        dtdocuments.Columns.Add("Mandatory_Documents");
        dRow = dtdocuments.NewRow();
        dRow["SI.NO"] = "";
        dRow["Description"] = "";
        dRow["callby"] = "";
        dRow["doc_dat"] = "";
        dRow["Document_Path"] = "";
        dRow["Mandatory_Scan"] = "";
        dRow["Mandatory_Documents"] = "";

        dtdocuments.Rows.Add(dRow);

        Session["dtdocuments"] = dtdocuments;

        ((DataTable)Session["dtdocuments"]).Rows.RemoveAt(0);



    }

    private void FunPriInitializeHdrGridData()
    {
        DataRow dRow;
        dtheader = new DataTable();
        dtheader.Columns.Add("SI.NO");
        dtheader.Columns.Add("Asset_Type");
        dtheader.Columns.Add("Remarks");
       
        dRow = dtheader.NewRow();
        dRow["SI.NO"] = "";
        dRow["Asset_Type"] = "";
        dRow["Remarks"] = "";
        

        dtheader.Rows.Add(dRow);

        Session["dtheader"] = dtheader;

        ((DataTable)Session["dtheader"]).Rows.RemoveAt(0);



    }


    protected void btnBrowse_OnClick(object sender, EventArgs e)
    {
        int intRowIndex = Utility.FunPubGetGridRowID("grvdoc", ((Button)sender).ClientID);

        HttpFileCollection hfc = Request.Files;
        for (int i = 0; i < hfc.Count; i++)
        {
            HttpPostedFile hpf = hfc[i];
            if (hpf.ContentLength > 0)
            {
               CheckBox chkScanned = (CheckBox)grvdoc.Rows[intRowIndex].FindControl("chkScanned");
                HiddenField hdnSelectedPath = (HiddenField)grvdoc.Rows[intRowIndex].FindControl("hdnSelectedPath");
                FileUpload flUpload = (FileUpload)grvdoc.Rows[intRowIndex].FindControl("flUpload");
                //Label lblActualPath = (Label)gvConstitutionalDocuments.Rows[intRowIndex].FindControl("lblActualPath");
                TextBox txtFileupld = (TextBox)grvdoc.Rows[intRowIndex].FindControl("txtFileupld");

                //chkSelect.Checked = true;
                //chkScanned.ToolTip = flUpload.ToolTip = hdnSelectedPath.Value;
                //lblActualPath.Text = hpf.FileName;
                txtFileupld.Text = hpf.FileName;
                string strViewst = "File" + intRowIndex.ToString();
                Cache[strViewst] = hpf;
            }
        }
    }
    protected void FunProUploadFilesNew()
    {
        try
        {
            for (int i = 0; i <= grvdoc.Rows.Count - 1; i++)
            {
                string strViewst = "File" + i.ToString();
                CheckBox chkScanned = (CheckBox)grvdoc.Rows[i].FindControl("chkScanned");
                Label lblCurrentPath = (Label)grvdoc.Rows[i].FindControl("lblActualPath");

                if (chkScanned.Checked)
                {
                    HttpPostedFile hpf = (HttpPostedFile)Cache[strViewst];
                    //string strServerPath = Server.MapPath(".").Replace("\\LoanAdmin", "");
                    //string strFilePath = "\\Data\\Invoice\\" + intCompanyId.ToString() + "_" + ddlMLA.SelectedText.Replace("/", "");

                    //if (!System.IO.Directory.Exists(strServerPath + strFilePath))
                    //{
                    //    System.IO.Directory.CreateDirectory(strServerPath + strFilePath);
                    //}

                    //if (ddlSLA.SelectedValue.ToString() != "0")
                    //{
                    //    strFilePath = strFilePath + @"\" + ddlSLA.SelectedItem.ToString().Replace("/", "") + "_" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}
                    //else
                    //{
                    //    strFilePath = strFilePath + @"\" + (i + 1).ToString() + "_" + System.IO.Path.GetFileName(hpf.FileName).Replace("%20", "_").Replace(" ", "_");
                    //}

                    string strNewFileName = @"\COMPANY" + intCompanyId.ToString();
                    string strPath = "";
                    //if (txOD.Text != "")
                    //{
                    strPath = strNewFileName;
                    strPath = Convert.ToString(ViewState["ConsDocPath"]) + strNewFileName;
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }

                    strPath += @"\" + System.IO.Path.GetFileName(hpf.FileName).Split('.')[0].ToString() + DateTime.Now.ToLocalTime().ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + System.IO.Path.GetFileName(hpf.FileName).Split('.')[1].ToString();


                    lblCurrentPath.Text = strPath;
                    hpf.SaveAs(strPath);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void FunProClearCaches()
    {
        for (int i = 0; i <= grvdoc.Rows.Count - 1; i++)
        {
            string strViewst = "File" + i.ToString();
            if (Cache[strViewst] != null)
            {
                Cache.Remove(strViewst);
            }
        }
    }
    protected void hlnkView_Click(object sender, EventArgs e)
    {
        try
        {
            FunPriShowPRDD(sender);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPriShowPRDD(object sender)
    {
        try
        {
            string strFieldAtt = ((LinkButton)sender).ClientID;
            int gRowIndex = Utility.FunPubGetGridRowID("grvdoc", strFieldAtt);
            //Label lblPath = gvConstitutionalDocuments.Rows[gRowIndex].FindControl("myThrobber") as Label;
            Label lblActualPath = grvdoc.Rows[gRowIndex].FindControl("lblActualPath") as Label;
            string strFileName = lblActualPath.Text.Replace("\\", "/").Trim();
            string strScipt = "window.open('../Common/S3GViewFile.aspx?qsFileName=" + strFileName + "', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=yes,scrollbars=yes,top=50,left=50');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), strKey, strScipt, true);
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw new ApplicationException("Due to Data Problem, Unable to View the Document");
        }
    }
    
}