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
using System.IO;
using S3GBusEntity;
using System.Net;

public partial class Origination_Default : ApplyThemeForProject
{
    #region Initalize
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> dictLOB;
    int intModuleId, intProgramId;
    #endregion

    #region Page Event
    protected void Page_Load(object sender, EventArgs e)
    {
        //test
        if (!IsPostBack)
        {
            lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
            hdnWorkFlowStatus.Value = "";
            FunPriLoadLOB_LIST();
            //test
        }

        DataTable DT = new DataTable();
        DataColumn DC = new DataColumn("TEST");
        DT.Columns.Add(DC);
        DataRow DR = DT.NewRow();
        DR["TEST"] = "DSF";
        DT.Rows.Add(DR);

        gv1.DataSource = DT;
        gv1.DataBind();

        if (!Page.IsPostBack)
        {
            DataTable dtUC = new DataTable();
            dtUC.Columns.Add("Sno");
            DataRow drUS = dtUC.NewRow();
            drUS["Sno"] = "0";
            dtUC.Rows.Add(drUS);

            DataRow DRTTT = dtUC.NewRow();
            DRTTT["Sno"] = "1";
            dtUC.Rows.Add(DRTTT);

            GridView1.DataSource = dtUC;
            GridView1.DataBind();
        }
    }

    protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        AjaxControlToolkit.AsyncFileUpload AsyncFileUpload1 = (AjaxControlToolkit.AsyncFileUpload)sender;
        System.Web.UI.WebControls.Label uploadResult = (System.Web.UI.WebControls.Label)AsyncFileUpload1.Parent.Parent.FindControl("uploadResult");
        System.Web.UI.WebControls.Image Image1 = (System.Web.UI.WebControls.Image)AsyncFileUpload1.Parent.Parent.FindControl("Image1");
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "size", "top.$get(\"" + uploadResult.ClientID + "\").innerHTML = 'Uploaded size: " + AsyncFileUpload1.FileBytes.Length.ToString() + "';", true);

        if (!Directory.Exists(MapPath("~/Data/DocPath/temp_PRDDTData/")))
        {
            Directory.CreateDirectory(MapPath("~/Data/DocPath/temp_PRDDTData/"));
        }
        string savePath = MapPath("~/Data/DocPath/temp_PRDDTData/" + System.IO.Path.GetFileName(e.filename));
        AsyncFileUpload1.SaveAs(savePath);
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "image", "top.$get(\"" + Image1.ClientID + "\").src = 'Uploads/" + System.IO.Path.GetFileName(e.filename) + "';", true);
    }   

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        //blnStatus = true;

        //hdnWorkFlowStatus.Value = blnStatus ? "Y" : "N";

        string strPageUrl = Request.Url.ToString();
        FunPubGetPageUrl(strPageUrl, out intModuleId, out intProgramId);

        txtModuleID.Text = intModuleId.ToString();
        txtProgramID.Text = intProgramId.ToString();

        dictLOB = new Dictionary<string, string>();
        dictLOB.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        dictLOB.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
        dictLOB.Add("@Product_ID", ddlProduct.SelectedValue);
        dictLOB.Add("@Module_ID", intModuleId.ToString());
        dictLOB.Add("@Program_ID", intProgramId.ToString());
        DataTable dtWorkflow = Utility.GetDefaultData("S3G_GET_WorkFlowIsSupported", dictLOB);
        DataRow drWorkflow = dtWorkflow.Rows[0];
        hdnWorkFlowStatus.Value = Convert.ToInt32(drWorkflow["IsWorkFlowSupported"]) == 0 ? "Y" : "N";
    }
    #endregion
    #region [Save/ Clear/ Cancel]
    protected void Save_Click(object sender, EventArgs e)
    {
        hdnWorkFlowStatus.Value = "Y";
        int intApplicationNo = 100;
        if (hdnWorkFlowStatus.Value == "Y")
        {
            dictLOB = new Dictionary<string, string>();
            dictLOB.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
            dictLOB.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
            dictLOB.Add("@Product_ID", ddlProduct.SelectedValue);
            dictLOB.Add("@Module_ID", intModuleId.ToString());
            dictLOB.Add("@Program_ID", intProgramId.ToString());

            DataTable dtWorkflow = Utility.GetDefaultData("S3G_GET_WORKFLOW", dictLOB);
            DataRow drWorkflow = dtWorkflow.Rows[0];
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    #endregion
    #region Load Dropdown
    private void FunPriLoadLOB_LIST()
    {
        dictLOB = new Dictionary<string, string>();
        dictLOB.Add("@Company_ID", Convert.ToString(ObjUserInfo.ProCompanyIdRW));
        dictLOB.Add("@User_ID", Convert.ToString(ObjUserInfo.ProUserIdRW));
        dictLOB.Add("@Is_Active", "1");
        ddlLineofBusiness.BindDataTable(SPNames.LOBMaster, dictLOB, new string[] { "LOB_ID", "LOB_CODE", "LOB_NAME" });
    }
    protected void ddlLOB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Bind Product 
        dictLOB = new Dictionary<string, string>();
        dictLOB.Add("@LOB_ID", ddlLineofBusiness.SelectedValue);
        dictLOB.Add("@Company_ID", Convert.ToString(ObjUserInfo.ProCompanyIdRW));
        ddlProduct.BindDataTable(SPNames.SYS_ProductMaster, dictLOB, new string[] { "Product_ID", "Product_Code", "Product_Name" });
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        //try
        //{
        //    foreach (GridViewRow grvRow in GridView1.Rows)
        //    {
        //        S3GFileUpload Ucf1 = (S3GFileUpload)grvRow.FindControl("Ucf1");
        //        //System.Web.UI.WebControls.TextBox txtFilePath = (System.Web.UI.WebControls.TextBox)Ucf1.FindControl("txtFilePath");
        //        //string source = txtFilePath.Text.Trim();
        //        //string path = "\\\\chnwsfsr01\\Smartlend NXG\\Team\\kavitha\\sample\\";
        //        //UploadFile(path, source);
        //        //Utility.saveFilePath(this.Page, txtFilePath.Text.Trim(), path);


        //        System.Web.UI.WebControls.TextBox txtFilePath = (System.Web.UI.WebControls.TextBox)Ucf1.FindControl("txtFilePath");
        //        string source = txtFilePath.Text;
        //        string path = "\\\\chnwsfsr01\\Smartlend NXG\\Team\\kavitha\\sample\\";
        //        string dest = path + Path.GetFileName(txtFilePath.Text);
        //        UploadFile(dest, source);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Utility.FunShowAlertMsg(this, ex.Message);
        //}
    }


    //protected byte[] UploadFile(string ftppath, string locpath)
    //{
    //    byte[] UpLoadData;
    //    try
    //    {
    //        FileStream fs = new FileStream(locpath, FileMode.Open);
    //        byte[] filedata = new byte[fs.Length];
    //        int btoread = (int)fs.Length;
    //        int bread = 0;
    //        while (btoread > 0)
    //        {
    //            int n = fs.Read(filedata, bread, btoread);
    //            if (n == 0)
    //                break;
    //            bread += n;
    //            btoread -= n;
    //        }
    //        btoread = filedata.Length;
    //        fs.Close();
    //        UpLoadData = uploaddata(ftppath, filedata);
    //    }
    //    catch (Exception ex)
    //    {
    //        Utility.FunShowAlertMsg(this, ex.Message);
    //    }
    //    return UpLoadData;
    //}

    //protected byte[] uploaddata(string path, byte[] data)
    //{
    //    byte[] UpLoadData;
    //    try
    //    {
    //        WebClient req = new WebClient();
    //        UpLoadData = req.UploadData(path, data);
    //    }
    //    catch (Exception ex)
    //    {
    //        Utility.FunShowAlertMsg(this, ex.Message);
    //    }
    //    return UpLoadData;
    //}
    #endregion
}
