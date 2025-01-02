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

public partial class LoanAdmin_S3G_LAD_InvoiceModification : ApplyThemeForProject
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
    public static LoanAdmin_S3G_LAD_InvoiceModification obj_Page;

    S3GSession ObjS3GSession = new S3GSession();

    protected void Page_Load(object sender, EventArgs e)
    {
        obj_Page = this;
        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserId = ObjUserInfo.ProUserIdRW;
        strDateFormat = ObjS3GSession.ProDateFormatRW;

        if (Request.QueryString["qsMode"] != null)
            strMode = Request.QueryString["qsMode"];

        CalendarExtender2.Format = strDateFormat;
        txtInvoiceDateTo.Attributes.Add("onblur", "fnDoDate(this,'" + txtInvoiceDateTo.ClientID + "','" + strDateFormat + "',null,null);");

        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);

        if (Request.QueryString["qsViewId"] != null)
        {
            FormsAuthenticationTicket fromTicket = FormsAuthentication.Decrypt(Request.QueryString["qsViewId"]);
            strMode = Request.QueryString.Get("qsMode");
            strNote_id = Convert.ToInt32(fromTicket.Name);
        }

        if (!IsPostBack)
        {
            if (strMode == "Q")                //Query Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View);

            }
            else if (strMode == "M")                //Query Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);

            }
            else                                   //Create Mode
            {
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);

            }
        }
        // CalendarExtender1.Format = strDateFormat;
        this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
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
    public static string[] GetTrancheName(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
            Procparam.Add("@Billing_Id", obj_Page.ddlInvoiveMonth.SelectedValue);
            Procparam.Add("@Program_id", "532");
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetTranche_AGT", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceMonth(String prefixText, int count)
    {
        try
        {
            Dictionary<string, string> Procparam;
            Procparam = new Dictionary<string, string>();
            List<String> suggetions = new List<String>();
            Procparam.Clear();
            Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
            Procparam.Add("@PrefixText", prefixText);
            suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GetInvoiceMonth", Procparam), true);
            return suggetions.ToArray();
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string[] GetInvoiceList(String prefixText, int count)
    {
        Dictionary<string, string> Procparam;
        UserInfo Ufo = new UserInfo();
        Procparam = new Dictionary<string, string>();
        List<String> suggetions = new List<String>();
        Procparam.Clear();
        Procparam.Add("@Company_ID", obj_Page.intCompanyID.ToString());
        Procparam.Add("@InvoiceType", obj_Page.ddlInvoiceType.SelectedValue);
        Procparam.Add("@Billing_Id", obj_Page.ddlInvoiveMonth.SelectedValue);
        Procparam.Add("@Tranche_Id", obj_Page.ddlTranche.SelectedValue);
        Procparam.Add("@Prefix", prefixText);
        suggetions = Utility.GetSuggestions(Utility.GetDefaultData("S3G_LAD_GET_INVOICENO", Procparam));
        return suggetions.ToArray();
    }

    protected void btnRegen_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> objProcParam = new Dictionary<string, string>();

        DataTable dtTran = new DataTable();

        /*Code commented for Rental invoice type and newly added from Windows service */
        if (ddlInvoiceType.SelectedValue == "1")
        {
            //DataTable dtTran = Utility.GetDefaultData("TepTra", objProcParam);

            //DataTable dtTran = new DataTable();

            //for (int k = 0; k < dtTran.Rows.Count; k++)
            //{

            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@User_ID", intUserId.ToString());
            objProcParam.Add("@Billing_ID", ddlInvoiveMonth.SelectedValue);
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);
            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());
            if (ddlInvoiceNo.SelectedValue != "0")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);
            objProcParam.Add("@Is_Final", "1");

            DataSet dSet = Utility.GetTableValues("S3G_LOANAD_Billing_PDF_ReGen", objProcParam);

            if (dSet.Tables[0].Columns.Contains("ErrorCode") && dSet.Tables[0].Rows[0]["ErrorCode"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Date should be in " + ddlInvoiveMonth.SelectedText + ".');", true);
                return;
            }

            if (dSet.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Not Found.');", true);
                return;
            }

            try
            {

                DataSet dss = new DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();
                Dictionary<string, string> Procparam;
                Procparam = new Dictionary<string, string>();
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                Procparam.Add("@Company_Id", "1");
                Procparam.Add("@Lob_Id", "3");
                if (dSet.Tables[0].Rows[0]["CESS_Amount1"].ToString() != "0")
                    Procparam.Add("@Template_Type_Code", "78");
                else
                    Procparam.Add("@Template_Type_Code", "51");
                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

                String strHTML = String.Empty;
                strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
                string strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
                string strFolderNo, strBillNo, strCustomerName, strDocumentPath, strInvoice_No, strInvoice_No_AMF,
                    strbillperiod, strnewFile, strAcocuntno, strBranchName, strtranche, strnewFile1;

                DataTable DTTranche = dSet.Tables[3].DefaultView.ToTable(true, "Tranche_Name");

                for (int i = 0; i < DTTranche.Rows.Count; i++)
                {
                    strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> "
                         + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                    DataRow DRaCC = DTTranche.Rows[i];

                    DataRow[] DRAccDtls = dSet.Tables[3].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

                    dt = dSet.Tables[3].Clone();
                    if (DRAccDtls.Length > 0)
                        dt = DRAccDtls.CopyToDataTable();

                    if (dt.Rows.Count > 0)
                    {
                        strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["Tranche_Name"].ToString();
                        strnewFile += "\\" + "Rental";


                        if (i == 0)
                        {

                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }

                        if (strCustomerName == "AXIS Bank Ltd.")
                            strnewFile1 = strAcocuntno.Replace("(a)", "") + "_Covering.pdf";
                        else
                            strnewFile1 = strAcocuntno + "_Covering.pdf";

                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }


                        DataSet dsHeader = new DataSet();
                        dsHeader.Tables.Add(dt);

                        if (dt.Rows[0]["IS_POS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~InvoiceTable~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable~", strHTML);

                            if (strHTML.Contains("~InvoiceTable1~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
                            }
                        }
                        else
                        {
                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
                            }
                        }

                        if (dt.Rows[0]["CHK_RTGS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Both~"))
                            {
                                strHTML = FunPubDeleteTableHeader("~RTGS_Both~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "2")
                        {
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = FunPubDeleteTable("~RTGS_Both~", strHTML);
                            if (strHTML.Contains("~RTGS_Funder~"))
                                strHTML = FunPubDeleteTable("~RTGS_Funder~", strHTML);

                            if (strHTML.Contains("~RTGS_Company~"))
                            {
                                strHTML = FunPubDeleteTableHeader("~RTGS_Company~", strHTML);
                            }
                        }
                        else if (dt.Rows[0]["CHK_RTGS"].ToString() == "3")
                        {
                            if (strHTML.Contains("~RTGS_Company~"))
                                strHTML = FunPubDeleteTable("~RTGS_Company~", strHTML);
                            if (strHTML.Contains("~RTGS_Both~"))
                                strHTML = FunPubDeleteTable("~RTGS_Both~", strHTML);

                            if (strHTML.Contains("~RTGS_Funder~"))
                            {
                                strHTML = FunPubDeleteTableHeader("~RTGS_Funder~", strHTML);
                            }
                        }

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

                        if (strHTML.Contains("~CompanyLogo~"))
                        {
                            strHTML = FunPubBindImages("~CompanyLogo~", strHTML, "1");
                        }
                        if (strHTML.Contains("~InvoiceSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, "1");
                        }
                        if (strHTML.Contains("~OPCFooter~"))
                        {
                            strHTML = FunPubBindImages("~OPCFooter~", strHTML, "1");
                        }

                        FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
                    }

                }

                DTTranche = new DataTable();

                Procparam = new Dictionary<string, string>();
                objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                Procparam.Add("@Company_Id", "1");
                Procparam.Add("@Lob_Id", "3");
                Procparam.Add("@Template_Type_Code", "53");

                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

                strHTML = String.Empty;

                strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
                strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
                DTTranche = dSet.Tables[4].DefaultView.ToTable(true, "Tranche_Name");

                for (int i = 0; i < DTTranche.Rows.Count; i++)
                {
                    strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> "
                         + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                    DataRow DRaCC = DTTranche.Rows[i];

                    DataRow[] DRAccDtls = dSet.Tables[4].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

                    dt = dSet.Tables[4].Clone();

                    if (DRAccDtls.Length > 0)
                        dt = DRAccDtls.CopyToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["Tranche_Name"].ToString();

                        strnewFile += "\\" + "AMF";
                        if (i == 0)
                        {
                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }

                        strnewFile1 = strAcocuntno.Replace("(b)", "") + "_Covering.pdf";

                        DataSet dsHeader = new DataSet();
                        dsHeader.Tables.Add(dt);


                        if (dt.Rows[0]["IS_POS"].ToString() == "1")
                        {
                            if (strHTML.Contains("~InvoiceTable~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable~", strHTML);

                            if (strHTML.Contains("~InvoiceTable1~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
                            }
                        }
                        else
                        {
                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
                            }
                        }

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

                        if (strHTML.Contains("~CompanyLogo~"))
                        {
                            strHTML = FunPubBindImages("~CompanyLogo~", strHTML, "1");
                        }
                        if (strHTML.Contains("~InvoiceSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, "1");
                        }
                        if (strHTML.Contains("~OPCFooter~"))
                        {
                            strHTML = FunPubBindImages("~OPCFooter~", strHTML, "1");
                        }

                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }

                        FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
                    }

                }

                DataTable DTAccounts = dSet.Tables[0].DefaultView.ToTable(true, "ACCOUNT_NO");
                Procparam = new Dictionary<string, string>();
                objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                Procparam.Add("@Company_Id", "1");
                Procparam.Add("@Lob_Id", "3");
                if (dSet.Tables[1].Rows[0]["Quantity_1"].ToString() == "1")
                    Procparam.Add("@Template_Type_Code", "15");
                else if (dSet.Tables[0].Rows[0]["CESS_Amount1"].ToString() != "0")
                    Procparam.Add("@Template_Type_Code", "77");
                else
                    Procparam.Add("@Template_Type_Code", "12");

                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

                strHTML = String.Empty;

                strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
                strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";

                for (int i = 0; i < DTAccounts.Rows.Count; i++)
                {
                    strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> "
                        + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                    DataRow DRaCC = DTAccounts.Rows[i];

                    DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
                    DataRow[] DRAccDtls1 = dSet.Tables[1].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
                    DataRow[] DRAccDtlsSAC = dSet.Tables[6].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString().Replace("(a)", "") + "'");
                    dt = dSet.Tables[0].Clone();
                    DataTable dt1 = dSet.Tables[1].Clone();
                    DataTable dtSAC = dSet.Tables[6].Clone();

                    if (DRAccDtls.Length > 0)
                        dt = DRAccDtls.CopyToDataTable();
                    if (DRAccDtls1.Length > 0)
                        dt1 = DRAccDtls1.CopyToDataTable();
                    if (DRAccDtlsSAC.Length > 0)
                        dtSAC = DRAccDtlsSAC.CopyToDataTable();

                    if (dt.Rows.Count > 0)
                    {
                        strBranchName = dt.Rows[0]["Location"].ToString();
                        strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                        strtranche = dt.Rows[0]["tranche_name"].ToString();
                        strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                        strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                        strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                        strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                        strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                        strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();
                        strInvoice_No = dt.Rows[0]["Invoice_No"].ToString();

                        strnewFile += "\\" + "Rental";
                        if (i == 0)
                        {
                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }


                        strnewFile1 = strtranche + "_" + strAcocuntno + "_" + strInvoice_No.Replace("/", "_") + ".pdf";

                        DataRow[] ObjIGSTDR = dt1.Select("InvTbl_IGST_Amount_Dbl > 0");

                        if (ObjIGSTDR.Length > 0)
                        {
                            if (strHTML.Contains("~InvoiceTable~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable~", strHTML);

                            if (strHTML.Contains("~InvoiceTable1~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
                            }
                        }
                        else
                        {
                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
                            }
                        }

                        if (strHTML.Contains("~SACTable~"))
                        {
                            strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC);
                        }

                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

                        if (strHTML.Contains("~CompanyLogo~"))
                        {
                            strHTML = FunPubBindImages("~CompanyLogo~", strHTML, "1");
                        }
                        if (strHTML.Contains("~InvoiceSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, "1");
                        }
                        if (strHTML.Contains("~OPCFooter~"))
                        {
                            strHTML = FunPubBindImages("~OPCFooter~", strHTML, "1");
                        }

                        FunPubGetQrCode(Convert.ToString(dt.Rows[0]["QRCode"]));

                        string strQRCode = Server.MapPath(".") + "\\PDF Files\\RentQRCode.png";

                        if (strHTML.Contains("~DCQRCode~"))
                            strHTML = strHTML.Replace("~DCQRCode~", "<img src='" + strQRCode + "' alt='Image'>");


                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }

                        FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
                    }

                }

                Procparam = new Dictionary<string, string>();
                objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                Procparam.Add("@Company_Id", "1");
                Procparam.Add("@Lob_Id", "3");
                Procparam.Add("@Template_Type_Code", "52");

                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

                strHTML = String.Empty;

                strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
                strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
                DataTable DTAccounts_Amf = dSet.Tables[5].DefaultView.ToTable(true, "ACCOUNT_NO");
                for (int i = 0; i < DTAccounts_Amf.Rows.Count; i++)
                {
                    strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'><TABLE><TR><TD> "
                         + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                    DataRow DRaCC = DTAccounts_Amf.Rows[i];

                    DataRow[] DRAccDtls = dSet.Tables[5].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
                    DataRow[] DRAccDtls1 = dSet.Tables[2].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
                    DataRow[] DRAccDtlsSAC1 = dSet.Tables[6].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString().Replace("(a)", "").Replace("(b)", "") + "'");
                    dt = dSet.Tables[5].Clone();
                    DataTable dt1 = dSet.Tables[2].Clone();
                    DataTable dtSAC1 = dSet.Tables[6].Clone();
                    if (DRAccDtls1.Length > 0)
                    {
                        if (DRAccDtls.Length > 0)
                            dt = DRAccDtls.CopyToDataTable();
                        if (DRAccDtls1.Length > 0)
                            dt1 = DRAccDtls1.CopyToDataTable();
                        if (DRAccDtlsSAC1.Length > 0)
                            dtSAC1 = DRAccDtlsSAC1.CopyToDataTable();

                        if (dt.Rows.Count > 0)
                        {
                            strBranchName = dt.Rows[0]["Location"].ToString();
                            strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
                            strBillNo = dt.Rows[0]["Bill_Number"].ToString();
                            strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                            strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
                            strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
                            strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
                            strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();
                            strtranche = dt.Rows[0]["tranche_name"].ToString();
                            strInvoice_No_AMF = dt.Rows[0]["Invoice_NO_AMF"].ToString();

                            strnewFile += "\\" + "AMF";

                            strnewFile1 = strtranche + "_" + strAcocuntno + "_" + strInvoice_No_AMF.Replace("/", "_") + ".pdf";
                            FileInfo fl = new FileInfo(strnewFile);

                            DataRow[] ObjIGSTDR = dt1.Select("AMF_IGST_Amount_Dbl > 0 ");

                            if (ObjIGSTDR.Length > 0)
                            {
                                if (strHTML.Contains("~InvoiceTable~"))
                                    strHTML = FunPubDeleteTable("~InvoiceTable~", strHTML);

                                if (strHTML.Contains("~InvoiceTable1~"))
                                {
                                    strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
                                }
                            }
                            else
                            {
                                if (strHTML.Contains("~InvoiceTable1~"))
                                    strHTML = FunPubDeleteTable("~InvoiceTable1~", strHTML);

                                if (strHTML.Contains("~InvoiceTable~"))
                                    strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
                            }

                            if (strHTML.Contains("~SACTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC1);
                            }

                            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

                            if (strHTML.Contains("~CompanyLogo~"))
                            {
                                strHTML = FunPubBindImages("~CompanyLogo~", strHTML, "1");
                            }
                            if (strHTML.Contains("~InvoiceSignStamp~"))
                            {
                                strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, "1");
                            }

                            if (strHTML.Contains("~OPCFooter~"))
                            {
                                strHTML = FunPubBindImages("~OPCFooter~", strHTML, "1");
                            }

                            FunPubGetQrCode(Convert.ToString(dt.Rows[0]["QRCode"]));

                            string strQRCode = Server.MapPath(".") + "\\PDF Files\\RentQRCode.png";

                            if (strHTML.Contains("~AMFQRCode~"))
                                strHTML = strHTML.Replace("~DCQRCode~", "<img src='" + strQRCode + "' alt='Image'>");


                            fl = new FileInfo(strnewFile);
                            if (fl.Exists == true)
                            {
                                fl.Delete();
                            }

                            FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
                lblErrorMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Error in Invoice Generation.');", true);
                return;
            }
        }
        /* Code commented for Rental invoice type and newly added from Windows service   */

        //Code moved from Windows service for Invoice type - rental - code starts

        //if (ddlInvoiceType.SelectedValue == "1")
        //{
        //    objProcParam = new Dictionary<string, string>();
        //    objProcParam.Add("@User_ID", intUserId.ToString());
        //    objProcParam.Add("@Billing_ID", ddlInvoiveMonth.SelectedValue);
        //    objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);
        //    if (txtInvoiceDateTo.Text != "")
        //        objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());
        //    if (ddlInvoiceNo.SelectedValue != "0")
        //        objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);
        //    objProcParam.Add("@Is_Final", "1");

        //    DataSet dSet = Utility.GetTableValues("S3G_LOANAD_Billing_PDF_ReGen", objProcParam);

        //    if (dSet.Tables[0].Columns.Contains("ErrorCode") && dSet.Tables[0].Rows[0]["ErrorCode"].ToString() == "1")
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Date should be in " + ddlInvoiveMonth.SelectedText + ".');", true);
        //        return;
        //    }

        //    if (dSet.Tables[0].Rows.Count == 0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Not Found.');", true);
        //        return;
        //    }


        //    try
        //    {
        //        DataSet dss = new DataSet();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        Dictionary<string, string> Procparam;
        //        Procparam = new Dictionary<string, string>();
        //        S3GDALayer.S3GAdminServices.ClsPubS3GAdmin objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
        //        Procparam.Add("@Company_Id", obj_Page.intCompanyID.ToString());
        //        Procparam.Add("@Lob_Id", "0");
        //        Procparam.Add("@Location_code", "0");
        //        Procparam.Add("@Template_Type_Code", "51");

        //        dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

        //        String strHTML = String.Empty;
        //        strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //        string strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
        //        string strFolderNo, strBillNo, strCustomerName, strDocumentPath,
        //            strbillperiod, strnewFile, strAcocuntno, strBranchName, strtranche, strnewFile1;

        //        DataTable DTTranche = dSet.Tables[3].DefaultView.ToTable(true, "Tranche_Name");
        //        for (int i = 0; i < DTTranche.Rows.Count; i++)
        //        {
        //            strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //            DataRow DRaCC = DTTranche.Rows[i];
        //            DataRow[] DRAccDtls = dSet.Tables[3].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

        //            dt = dSet.Tables[3].Clone();
        //            if (DRAccDtls.Length > 0)
        //                dt = DRAccDtls.CopyToDataTable();
        //            if (dt.Rows.Count > 0)
        //            {
        //                strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
        //                strBillNo = dt.Rows[0]["Bill_Number"].ToString();
        //                strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
        //                strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
        //                strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
        //                strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
        //                strAcocuntno = dt.Rows[0]["Tranche_Name"].ToString();
        //                strnewFile += "\\" + "Rental";

        //                if (i == 0)
        //                {

        //                    if (!Directory.Exists(strnewFile))
        //                    {
        //                        Directory.CreateDirectory(strnewFile);
        //                    }
        //                }

        //                if (strCustomerName == "AXIS Bank Ltd.")
        //                    strnewFile1 = strAcocuntno.Replace("(a)", "") + "_Covering.pdf";
        //                else
        //                    strnewFile1 = strAcocuntno + "_Covering.pdf";

        //                FileInfo fl = new FileInfo(strnewFile);
        //                if (fl.Exists == true)
        //                {
        //                    fl.Delete();
        //                }

        //                DataSet dsHeader = new DataSet();
        //                dsHeader.Tables.Add(dt);
        //                if (dt.Rows[0]["IS_POS"].ToString() == "1")
        //                {
        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
        //                    }
        //                }
        //                else
        //                {
        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
        //                    }
        //                }

        //                if (dt.Rows[0]["CHK_RTGS"].ToString() == "1")
        //                {
        //                    if (strHTML.Contains("~RTGS_Company~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
        //                    if (strHTML.Contains("~RTGS_Funder~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

        //                    if (strHTML.Contains("~RTGS_Both~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Both~", strHTML);
        //                    }
        //                }
        //                else if (dt.Rows[0]["CHK_RTGS"].ToString() == "2")
        //                {
        //                    if (strHTML.Contains("~RTGS_Both~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);
        //                    if (strHTML.Contains("~RTGS_Funder~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Funder~", strHTML);

        //                    if (strHTML.Contains("~RTGS_Company~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Company~", strHTML);
        //                    }
        //                }
        //                else if (dt.Rows[0]["CHK_RTGS"].ToString() == "3")
        //                {
        //                    if (strHTML.Contains("~RTGS_Company~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Company~", strHTML);
        //                    if (strHTML.Contains("~RTGS_Both~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~RTGS_Both~", strHTML);

        //                    if (strHTML.Contains("~RTGS_Funder~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Funder~", strHTML);
        //                    }
        //                }

        //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

        //                if (strHTML.Contains("~CompanyLogo~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~InvoiceSignStamp~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~OPCFooter~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, obj_Page.CompanyId.ToString());
        //                }

        //                FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
        //            }
        //        }

        //        DTTranche = new DataTable();

        //        Procparam = new Dictionary<string, string>();
        //        objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
        //        Procparam.Add("@Company_Id", obj_Page.CompanyId.ToString());
        //        Procparam.Add("@Lob_Id", "0");
        //        Procparam.Add("@Location_code", "0");
        //        Procparam.Add("@Template_Type_Code", "53");

        //        dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

        //        strHTML = String.Empty;
        //        strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //        strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
        //        DTTranche = dSet.Tables[4].DefaultView.ToTable(true, "Tranche_Name");
        //        for (int i = 0; i < DTTranche.Rows.Count; i++)
        //        {
        //            strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //            DataRow DRaCC = DTTranche.Rows[i];

        //            DataRow[] DRAccDtls = dSet.Tables[4].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

        //            dt = dSet.Tables[4].Clone();

        //            if (DRAccDtls.Length > 0)
        //                dt = DRAccDtls.CopyToDataTable();
        //            if (dt.Rows.Count > 0)
        //            {
        //                strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
        //                strBillNo = dt.Rows[0]["Bill_Number"].ToString();
        //                strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
        //                strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
        //                strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
        //                strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
        //                strAcocuntno = dt.Rows[0]["Tranche_Name"].ToString();

        //                strnewFile += "\\" + "AMF";
        //                if (i == 0)
        //                {
        //                    if (!Directory.Exists(strnewFile))
        //                    {
        //                        Directory.CreateDirectory(strnewFile);
        //                    }
        //                }

        //                strnewFile1 = strAcocuntno.Replace("(b)", "") + "_Covering.pdf";

        //                DataSet dsHeader = new DataSet();
        //                dsHeader.Tables.Add(dt);

        //                //strHTML = FunPriHeadPriDetails(strHTML, dsHeader);
        //                if (dt.Rows[0]["IS_POS"].ToString() == "1")
        //                {
        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt);
        //                    }
        //                }
        //                else
        //                {
        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
        //                    }
        //                }

        //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

        //                if (strHTML.Contains("~CompanyLogo~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~InvoiceSignStamp~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~OPCFooter~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, obj_Page.CompanyId.ToString());
        //                }

        //                FileInfo fl = new FileInfo(strnewFile);
        //                if (fl.Exists == true)
        //                {
        //                    fl.Delete();
        //                }

        //                FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
        //            }
        //        }
        //        DataTable DTAccounts = dSet.Tables[0].DefaultView.ToTable(true, "ACCOUNT_NO");
        //        Procparam = new Dictionary<string, string>();
        //        objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
        //        Procparam.Add("@Company_Id", obj_Page.CompanyId.ToString());
        //        Procparam.Add("@Lob_Id", "0");
        //        Procparam.Add("@Location_code", "0");
        //        Procparam.Add("@Template_Type_Code", "12");

        //        dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

        //        strHTML = String.Empty;
        //        //strHTML = PDFPageSetup.FormatHTML(dss.Tables[0].Rows[0]["Template_Content"].ToString());
        //        strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //        strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
        //        for (int i = 0; i < DTAccounts.Rows.Count; i++)
        //        {
        //            strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //            DataRow DRaCC = DTAccounts.Rows[i];
        //            // DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'", "CashFlow");
        //            DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //            DataRow[] DRAccDtls1 = dSet.Tables[1].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //            DataRow[] DRAccDtlsSAC = dSet.Tables[6].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString().Replace("(a)", "") + "'");
        //            dt = dSet.Tables[0].Clone();
        //            DataTable dt1 = dSet.Tables[1].Clone();
        //            DataTable dtSAC = dSet.Tables[6].Clone();

        //            if (DRAccDtls.Length > 0)
        //                dt = DRAccDtls.CopyToDataTable();
        //            if (DRAccDtls1.Length > 0)
        //                dt1 = DRAccDtls1.CopyToDataTable();
        //            if (DRAccDtlsSAC.Length > 0)
        //                dtSAC = DRAccDtlsSAC.CopyToDataTable();

        //            if (dt.Rows.Count > 0)
        //            {
        //                strBranchName = dt.Rows[0]["Location"].ToString();
        //                strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
        //                strtranche = dt.Rows[0]["tranche_name"].ToString();
        //                strBillNo = dt.Rows[0]["Bill_Number"].ToString();
        //                strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
        //                strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
        //                strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
        //                strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
        //                strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();

        //                strnewFile += "\\" + "Rental";
        //                if (i == 0)
        //                {
        //                    if (!Directory.Exists(strnewFile))
        //                    {
        //                        Directory.CreateDirectory(strnewFile);
        //                    }
        //                }

        //                strnewFile1 = strtranche + "_" + strAcocuntno + ".pdf";

        //                DataRow[] ObjIGSTDR = dt1.Select("InvTbl_IGST_Amount_Dbl > 0");

        //                if (ObjIGSTDR.Length > 0)
        //                {
        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (strHTML.Contains("~InvoiceTable1~"))
        //                        strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

        //                    if (strHTML.Contains("~InvoiceTable~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
        //                    }
        //                }

        //                if (strHTML.Contains("~SACTable~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC);
        //                }


        //                strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

        //                if (strHTML.Contains("~CompanyLogo~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~InvoiceSignStamp~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, obj_Page.CompanyId.ToString());
        //                }
        //                if (strHTML.Contains("~OPCFooter~"))
        //                {
        //                    strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, obj_Page.CompanyId.ToString());
        //                }

        //                FileInfo fl = new FileInfo(strnewFile);
        //                if (fl.Exists == true)
        //                {
        //                    fl.Delete();
        //                }

        //                FunPrintWord(strHTML, strnewFile, strnewFile1, "0");
        //            }

        //        }

        //        Procparam = new Dictionary<string, string>();
        //        objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
        //        Procparam.Add("@Company_Id", obj_Page.CompanyId.ToString());
        //        Procparam.Add("@Lob_Id", "0");
        //        Procparam.Add("@Location_code", "0");
        //        Procparam.Add("@Template_Type_Code", "52");

        //        dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);

        //        strHTML = String.Empty;
        //        //strHTML = PDFPageSetup.FormatHTML(dss.Tables[0].Rows[0]["Template_Content"].ToString());
        //        strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //        strContent = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + strHTML + "</TD></TR></TABLE>";
        //        DataTable DTAccounts_Amf = dSet.Tables[5].DefaultView.ToTable(true, "ACCOUNT_NO");
        //        for (int i = 0; i < DTAccounts_Amf.Rows.Count; i++)
        //        {
        //            strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
        //            DataRow DRaCC = DTAccounts_Amf.Rows[i];
        //            // DataRow[] DRAccDtls = dSet.Tables[0].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'", "CashFlow");
        //            DataRow[] DRAccDtls = dSet.Tables[5].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //            DataRow[] DRAccDtls1 = dSet.Tables[2].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString() + "'");
        //            DataRow[] DRAccDtlsSAC1 = dSet.Tables[6].Select("ACCOUNT_NO='" + DRaCC["ACCOUNT_NO"].ToString().ToString().Replace("(a)", "").Replace("(b)", "") + "'");
        //            dt = dSet.Tables[5].Clone();
        //            DataTable dt1 = dSet.Tables[2].Clone();
        //            DataTable dtSAC1 = dSet.Tables[6].Clone();
        //            if (DRAccDtls1.Length > 0)
        //            {
        //                if (DRAccDtls.Length > 0)
        //                    dt = DRAccDtls.CopyToDataTable();
        //                if (DRAccDtls1.Length > 0)
        //                    dt1 = DRAccDtls1.CopyToDataTable();
        //                if (DRAccDtlsSAC1.Length > 0)
        //                    dtSAC1 = DRAccDtlsSAC1.CopyToDataTable();

        //                if (dt.Rows.Count > 0)
        //                {
        //                    strBranchName = dt.Rows[0]["Location"].ToString();
        //                    strFolderNo = dt.Rows[0]["FolderNumber"].ToString();
        //                    strBillNo = dt.Rows[0]["Bill_Number"].ToString();
        //                    strCustomerName = dt.Rows[0]["CUSTOMER_NAME"].ToString();
        //                    strDocumentPath = dt.Rows[0]["Document_Path"].ToString();
        //                    strbillperiod = dt.Rows[0]["Bill_Period"].ToString();
        //                    strnewFile = strDocumentPath + "\\BillNo-" + strFolderNo;
        //                    strAcocuntno = dt.Rows[0]["ACCOUNT_NO"].ToString();
        //                    strtranche = dt.Rows[0]["tranche_name"].ToString();

        //                    strnewFile += "\\" + "AMF";

        //                    strnewFile1 = strtranche + "_" + strAcocuntno + ".pdf";
        //                    FileInfo fl = new FileInfo(strnewFile);

        //                    DataRow[] ObjIGSTDR = dt1.Select("AMF_IGST_Amount_Dbl > 0 ");

        //                    if (ObjIGSTDR.Length > 0)
        //                    {
        //                        if (strHTML.Contains("~InvoiceTable~"))
        //                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable~", strHTML);

        //                        if (strHTML.Contains("~InvoiceTable1~"))
        //                        {
        //                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable1~", strHTML, dt1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (strHTML.Contains("~InvoiceTable1~"))
        //                            strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

        //                        if (strHTML.Contains("~InvoiceTable~"))
        //                            strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt1);
        //                    }

        //                    if (strHTML.Contains("~SACTable~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindTable("~SACTable~", strHTML, dtSAC1);
        //                    }

        //                    strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

        //                    if (strHTML.Contains("~CompanyLogo~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindImages("~CompanyLogo~", strHTML, obj_Page.CompanyId.ToString());
        //                    }
        //                    if (strHTML.Contains("~InvoiceSignStamp~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindImages("~InvoiceSignStamp~", strHTML, obj_Page.CompanyId.ToString());
        //                    }

        //                    if (strHTML.Contains("~OPCFooter~"))
        //                    {
        //                        strHTML = PDFPageSetup.FunPubBindImages("~OPCFooter~", strHTML, obj_Page.CompanyId.ToString());
        //                    }

        //                    fl = new FileInfo(strnewFile);
        //                    if (fl.Exists == true)
        //                    {
        //                        fl.Delete();
        //                    }

        //                    FunPrintWord(strHTML, strnewFile, strnewFile1, "0");
        //                }
        //            }
        //        }


        //       ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);

        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        //        lblErrorMessage.Text = ex.Message;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Error in Invoice Generation.');", true);
        //        return;
        //    }
        //}
        //Code moved from Windows service for Invoice type - rental - code ends
        else if (ddlInvoiceType.SelectedValue == "2")
        {
            dtTran = new DataTable();
            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_Id", intCompanyID.ToString());
            objProcParam.Add("@User_Id", intUserId.ToString());
            objProcParam.Add("@Option", "2");
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());

            if (ddlInvoiceNo.SelectedValue != "0")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);

            dtTran = Utility.GetDefaultData("S3G_LAD_InvoiceModification", objProcParam);

            if (dtTran.Columns.Contains("ErrorCode") && dtTran.Rows[0]["ErrorCode"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Date should be in " + ddlInvoiveMonth.SelectedText + ".');", true);
                return;
            }

            if (dtTran.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Not Found.');", true);
                return;
            }

            if (dtTran.Rows[0]["ErrorCode"].ToString() == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
        }
        else if (ddlInvoiceType.SelectedValue == "3")
        {
            dtTran = new DataTable();
            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_Id", intCompanyID.ToString());
            objProcParam.Add("@User_Id", intUserId.ToString());
            objProcParam.Add("@Option", "3");
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());

            if (ddlInvoiceNo.SelectedValue != "0")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);

            dtTran = Utility.GetDefaultData("S3G_LAD_InvoiceModification", objProcParam);

            if (dtTran.Columns.Contains("ErrorCode") && dtTran.Rows[0]["ErrorCode"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Date should be in " + ddlInvoiveMonth.SelectedText + ".');", true);
                return;
            }

            if (dtTran.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Not Found.');", true);
                return;
            }

            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_ID", intCompanyID.ToString());
            objProcParam.Add("@CashFlowId", "1");
            objProcParam.Add("@StartDate", DateTime.Now.ToString());
            objProcParam.Add("@Enddate", DateTime.Now.ToString());
            DataSet dSet = FunPriGetCashFlowDet(objProcParam);

            DataTable dtSD = new DataTable();
            DataTable dtOthers = new DataTable();

            DataRow[] STResult = dSet.Tables[2].Select("cashflow_flag_iD = 30");
            if (STResult.Length != 0)
            {
                dtSD = STResult.CopyToDataTable();
            }

            DataRow[] OthersResult = dSet.Tables[1].Select("cashflow_flag_iD <> 30");
            if (OthersResult.Length != 0)
            {
                dtOthers = OthersResult.CopyToDataTable();
            }

            if (dtOthers.Rows.Count > 0)
            {
                string Template = string.Empty;
                DataSet dss = new DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();
                Dictionary<string, string> Procparam;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", intCompanyID.ToString());
                Procparam.Add("@Lob_Id", "0");
                Procparam.Add("@Location_ID", "0");
                Procparam.Add("@Template_Type_Code", "17");
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);
                String strHTML = String.Empty;
                //int strsuffix;
                //strsuffix = Convert.ToInt32(dtOthers.Rows[0]["GPS_Suffix"].ToString());
                //string strformat = "0.";
                //for (int i = 1; i <= strsuffix; i++)
                //{
                //    strformat += "0";
                //}

                try
                {
                    for (int i = 0; i < dtOthers.Rows.Count; i++)
                    {
                        //strHTML = PDFPageSetup.FormatHTML(dss.Tables[0].Rows[0]["Template_Content"].ToString());
                        strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                        string strnewFile, strnewFile1, grand_total;
                        strnewFile = dtOthers.Rows[i]["Document_Path"].ToString();

                        if (dtOthers.Columns.Contains("OtherDescr") == false)
                            dtOthers.Columns.Add("OtherDescr");
                        if (dtOthers.Columns.Contains("Grand_Total") == false)
                            dtOthers.Columns.Add("Grand_Total");
                        if (dtOthers.Columns.Contains("Total") == false)
                            dtOthers.Columns.Add("Total");
                        if (dtOthers.Columns.Contains("TotalDescr") == false)
                            dtOthers.Columns.Add("TotalDescr");
                        if (dtOthers.Columns.Contains("ServiceTaxAmount") == false)
                            dtOthers.Columns.Add("ServiceTaxAmount");
                        if (dtOthers.Columns.Contains("ServiceTaxRate") == false)
                            dtOthers.Columns.Add("ServiceTaxRate");
                        if (dtOthers.Columns.Contains("RupeesInWords") == false)
                            dtOthers.Columns.Add("RupeesInWords");

                        dtOthers.Rows[i]["OtherDescr"] = "";
                        dtOthers.Rows[i]["TotalDescr"] = "";
                        //dtOthers.Rows[i]["TotalDescr"] = 0.0;
                        dtOthers.Rows[i]["Grand_Total"] = 0.0;
                        dtOthers.Rows[i]["Total"] = 0.0;

                        if (dSet.Tables[3].Rows.Count > 0)
                        {
                            DataTable dtAmount = new DataTable();
                            DataRow[] dtAmountRow = dSet.Tables[3].Select("Pa_Sa_Ref_ID = " + dtOthers.Rows[i]["PA_SA_REF_ID"].ToString()
                                + "AND CashFlow_FlagID = " + dtOthers.Rows[i]["cashflow_flag_iD"].ToString());
                            dtAmount = dtAmountRow.CopyToDataTable();

                            DataView view = new DataView(dtAmount);
                            DataTable distinctValues = view.ToTable(true, "Other_Tax_Name", "OT_Rate");
                            for (int k = 0; k < distinctValues.Rows.Count; k++)
                            {
                                if (dtOthers.Columns.Contains(distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Rate") == false)
                                    dtOthers.Columns.Add(distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Rate");

                                dtOthers.Rows[i][distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Rate"] = distinctValues.Rows[k]["OT_Rate"].ToString();

                                if (dtOthers.Columns.Contains(distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Amount") == false)
                                    dtOthers.Columns.Add(distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Amount");

                                dtOthers.Rows[i][distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "Amount"] =
                                     Convert.ToDecimal((view.ToTable(true, "Other_Tax_Name", "OT_Amount").Select("Other_Tax_Name= '" + distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "'")).CopyToDataTable().Compute("sum(OT_Amount)", "")).ToString();

                                dtOthers.Rows[i]["Total"] = Convert.ToDecimal(dtOthers.Rows[i]["Total"]) + Convert.ToDecimal((view.ToTable(true, "Other_Tax_Name", "OT_Amount").Select("Other_Tax_Name= '" + distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "'")).CopyToDataTable().Compute("sum(OT_Amount)", ""));

                                dtOthers.Rows[i]["OtherDescr"] += "<br> " + distinctValues.Rows[k]["Other_Tax_Name"].ToString() + " @ " + Convert.ToDecimal(distinctValues.Rows[k]["OT_Rate"]).ToString() + " %";

                                dtOthers.Rows[i]["TotalDescr"] += "<br> " + Convert.ToDecimal((view.ToTable(true, "Other_Tax_Name", "OT_Amount").Select("Other_Tax_Name= '" + distinctValues.Rows[k]["Other_Tax_Name"].ToString() + "'")).CopyToDataTable().Compute("sum(OT_Amount)", "")).ToString();
                            }

                            dtOthers.Rows[i]["ServiceTaxAmount"] = Convert.ToDecimal((view.ToTable(true, "ST_Amount").Compute("sum(ST_Amount)", ""))).ToString();
                            dtOthers.Rows[i]["ServiceTaxRate"] = Convert.ToDecimal((view.ToTable(true, "RatePercentage").Compute("sum(RatePercentage)", ""))).ToString();
                            dtOthers.Rows[i]["OtherDescr"] = "<p align='left'><br>" + dtOthers.Rows[i]["Descr"] + dtOthers.Rows[i]["OtherDescr"] + "</p>";
                            //+ "<br>" + "ADD : Service Tax @ " + Convert.ToDecimal(dtOthers.Rows[i]["ServiceTaxRate"]).ToString() + " %" + dtOthers.Rows[i]["OtherDescr"] + "</p>";
                            dtOthers.Rows[i]["TotalDescr"] = "<p align='right'><br>" + Convert.ToDecimal(dtOthers.Rows[i]["Install_Amount"]).ToString() + dtOthers.Rows[i]["TotalDescr"] + "</p>";
                            //+ "<br>" + Convert.ToDecimal(dtOthers.Rows[i]["ServiceTaxAmount"]).ToString() + dtOthers.Rows[i]["TotalDescr"] + "</p>";
                            //dtOthers.Rows[i]["Total"] = (Convert.ToDecimal(dtOthers.Rows[i]["Total"]) + Convert.ToDecimal(dtOthers.Rows[i]["ServiceTaxAmount"])).ToString();
                            grand_total = (Convert.ToDecimal(dtOthers.Rows[i]["Total"]) + Convert.ToDecimal(dtOthers.Rows[i]["Install_Amount"])).ToString();
                        }
                        else
                        {
                            dtOthers.Rows[i]["ServiceTaxAmount"] = 0.0;
                            dtOthers.Rows[i]["ServiceTaxRate"] = 0.0;
                            dtOthers.Rows[i]["OtherDescr"] = "<p align='left'><br>" + dtOthers.Rows[i]["Descr"] + "</p>";
                            dtOthers.Rows[i]["TotalDescr"] = "<p align='right'><br>" + Convert.ToDecimal(dtOthers.Rows[i]["Install_Amount"]).ToString() + "</p>";
                            dtOthers.Rows[i]["Total"] = (Convert.ToDecimal(dtOthers.Rows[i]["Total"]));
                            grand_total = Convert.ToDecimal(dtOthers.Rows[i]["Install_Amount"]).ToString();
                        }
                        dtOthers.Rows[i]["Grand_Total"] = grand_total;
                        dtOthers.Rows[i]["RupeesInWords"] = GetAmountInWords(Convert.ToDecimal(grand_total)).ToString();
                        //DataRow dr = dtOthers.NewRow();
                        //foreach (DataColumn dcol in dtOthers.Columns)
                        //{
                        //    dr = dtOthers.Rows[i];
                        //    string ColName1 = string.Empty;
                        //    ColName1 = "~" + dcol.ColumnName + "~";
                        //    if (strHTML.Contains(ColName1))
                        //        strHTML = strHTML.Replace(ColName1, dr[dcol].ToString());
                        //}
                        DataTable tempdt = new DataTable();
                        tempdt = dtOthers.Clone();
                        for (int i1 = 0; i1 < dtOthers.Rows.Count; i1++)
                        {
                            if (i1 == i)
                            {
                                tempdt.ImportRow(dtOthers.Rows[i1]);
                            }
                        }
                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, tempdt);

                        if (strHTML.Contains("~CompanyLogo~"))
                        {
                            strHTML = FunPubBindImages("~CompanyLogo~", strHTML, intCompanyID.ToString());
                        }
                        if (strHTML.Contains("~InvoiceSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, intCompanyID.ToString());
                        }
                        if (strHTML.Contains("~POSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~POSignStamp~", strHTML, intCompanyID.ToString());
                        }

                        GunfPubGetQrCode(tempdt.Rows[0]["QRCode"].ToString(), strnewFile);

                        string strQRCode = strnewFile + "\\RentalQRCode.png";

                        if (strHTML.Contains("~OCQRCode~"))
                            strHTML = strHTML.Replace("~OCQRCode~", "<img src='" + strQRCode + "' alt='Image'>");

                        //strnewFile += "\\" + "OtherCashflow" + "\\";
                        if (i == 0)
                        {
                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }
                        strnewFile1 = dtOthers.Rows[i]["Tranche_Name"].ToString().Replace("/", "_") + "_" + dtOthers.Rows[i]["ACCOUNT_NO"].ToString() + "_" + dtOthers.Rows[i]["cashflow_flag_iD"].ToString() + ".pdf";
                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }

                        FunPrintWord(strHTML, strnewFile, strnewFile1, "0");
                    }

                    Procparam = new Dictionary<string, string>();
                    Procparam.Add("@Company_Id", intCompanyID.ToString());
                    Procparam.Add("@Lob_Id", "0");
                    Procparam.Add("@Location_ID", "0");
                    Procparam.Add("@Template_Type_Code", "19");
                    objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                    dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);
                    strHTML = String.Empty;

                    strHTML = "<META content='text/html; charset=utf-8' http-equiv='Content-Type'> <TABLE><TR><TD> " + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";

                    DataTable DTTranche = dSet.Tables[4].DefaultView.ToTable(true, "Tranche_Name");

                    for (int i = 0; i < DTTranche.Rows.Count; i++)
                    {
                        string strnewFile, strnewFile1;
                        string Tot_Rent = "0", tot_gst = "0", grand_tot = "0";
                        strnewFile = dtOthers.Rows[i]["Document_Path"].ToString();

                        strHTML = dss.Tables[0].Rows[0]["Template_Content"].ToString();
                        DataRow DRaCC = DTTranche.Rows[i];
                        DataRow[] DRAccDtls = dSet.Tables[4].Select("Tranche_Name='" + DRaCC["Tranche_Name"].ToString() + "'");

                        dt = dSet.Tables[4].Clone();

                        if (DRAccDtls.Length > 0)
                            dt = DRAccDtls.CopyToDataTable();

                        if (dt.Columns.Contains("grand_total") == false)
                            dt.Columns.Add("grand_total");

                        if (dt.Columns.Contains("RupeesInWords") == false)
                            dt.Columns.Add("RupeesInWords");

                        if (dt.Columns.Contains("tot_rental_amount") == false)
                            dt.Columns.Add("tot_rental_amount");

                        if (dt.Columns.Contains("tot_gst") == false)
                            dt.Columns.Add("tot_gst");

                        if (dt.Columns.Contains("grand_total") == false)
                            dt.Columns.Add("grand_total");

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Tot_Rent = (Convert.ToDecimal(Tot_Rent) + Convert.ToDecimal(dt.Rows[j]["rental_amount"])).ToString();
                            tot_gst = (Convert.ToDecimal(tot_gst) + Convert.ToDecimal(dt.Rows[j]["gst"])).ToString();
                            grand_tot = (Convert.ToDecimal(grand_tot) + Convert.ToDecimal(dt.Rows[j]["Total"])).ToString();
                        }

                        dt.Rows[0]["tot_rental_amount"] = Tot_Rent;
                        dt.Rows[0]["tot_gst"] = tot_gst;
                        dt.Rows[0]["grand_total"] = grand_tot;
                        dt.Rows[0]["RupeesInWords"] = GetAmountInWords(Convert.ToDecimal(grand_tot)).ToString();

                        dt.Rows[0]["Payment_Beneficiary"] = "Kindly issue a Cheque/DD for Rs. " + grand_tot.ToString() + " in favour of " + dt.Rows[0]["Payment_Beneficiary"].ToString();

                        if (dt.Rows.Count > 0)
                        {
                            if (i == 0)
                            {
                                if (!Directory.Exists(strnewFile))
                                {
                                    Directory.CreateDirectory(strnewFile);
                                }
                            }

                            strnewFile1 = DTTranche.Rows[i]["Tranche_Name"].ToString() + "_Covering.pdf";

                            FileInfo fl = new FileInfo(strnewFile);

                            if (fl.Exists == true)
                            {
                                fl.Delete();
                            }

                            if (strHTML.Contains("~InvoiceTable1~"))
                                strHTML = PDFPageSetup.FunPubDeleteTable("~InvoiceTable1~", strHTML);

                            if (strHTML.Contains("~InvoiceTable~"))
                            {
                                strHTML = PDFPageSetup.FunPubBindTable("~InvoiceTable~", strHTML, dt);
                            }

                            if (strHTML.Contains("~RTGS_Company~"))
                            {
                                strHTML = PDFPageSetup.FunPubDeleteTableHeader("~RTGS_Company~", strHTML);
                            }

                            strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, dt);

                            if (strHTML.Contains("~CompanyLogo~"))
                            {
                                strHTML = FunPubBindImages("~CompanyLogo~", strHTML, intCompanyID.ToString());
                            }
                            if (strHTML.Contains("~InvoiceSignStamp~"))
                            {
                                strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, intCompanyID.ToString());
                            }
                            if (strHTML.Contains("~OPCFooter~"))
                            {
                                strHTML = FunPubBindImages("~OPCFooter~", strHTML, intCompanyID.ToString());
                            }

                            FunPrintWord(strHTML, strnewFile, strnewFile1, "1");
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    //return;
                }
            }

            if (dtSD.Rows.Count > 0)
            {
                string Template = string.Empty;
                DataSet dss = new DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();
                Dictionary<string, string> Procparam;
                Procparam = new Dictionary<string, string>();
                Procparam.Add("@Company_Id", intCompanyID.ToString());
                Procparam.Add("@Lob_Id", "0");
                Procparam.Add("@Location_ID", "0");
                Procparam.Add("@Template_Type_Code", "18");
                S3GDALayer.S3GAdminServices.ClsPubS3GAdmin objS3gAdminClient = new S3GDALayer.S3GAdminServices.ClsPubS3GAdmin();
                dss = objS3gAdminClient.FunPubFillDataset("S3G_Get_TemplateCont", Procparam);
                String strHTML = String.Empty;
                try
                {
                    if (dtSD.Columns.Contains("RupeesInWords") == false)
                        dtSD.Columns.Add("RupeesInWords");

                    for (int i = 0; i < dtSD.Rows.Count; i++)
                    {
                        //strHTML = PDFPageSetup.FormatHTML(dss.Tables[0].Rows[0]["Template_Content"].ToString());
                        strHTML = "<TABLE><TR><TD> " + dss.Tables[0].Rows[0]["Template_Content"].ToString() + "</TD></TR></TABLE>";
                        string strnewFile, strnewFile1, grand_total;
                        strnewFile = dtSD.Rows[i]["Document_Path"].ToString();
                        grand_total = dtSD.Rows[i]["Install_Amount"].ToString();
                        dtSD.Rows[i]["RupeesInWords"] = GetAmountInWords(Convert.ToDecimal(grand_total)).ToString();
                        //DataRow dr = dtSD.NewRow();
                        //foreach (DataColumn dcol in dtSD.Columns)
                        //{
                        //    dr = dtSD.Rows[i];
                        //    string ColName1 = string.Empty;
                        //    ColName1 = "~" + dcol.ColumnName + "~";
                        //    if (strHTML.Contains(ColName1))
                        //        strHTML = strHTML.Replace(ColName1, dr[dcol].ToString());
                        //}
                        DataTable tempdt = new DataTable();
                        tempdt = dtSD.Clone();
                        for (int i1 = 0; i1 < dtSD.Rows.Count; i1++)
                        {
                            if (i1 == i)
                            {
                                tempdt.ImportRow(dtSD.Rows[i1]);
                            }
                        }
                        strHTML = PDFPageSetup.FunPubBindCommonVariables(strHTML, tempdt);

                        if (strHTML.Contains("~CompanyLogo~"))
                        {
                            strHTML = FunPubBindImages("~CompanyLogo~", strHTML, intCompanyID.ToString());
                        }
                        if (strHTML.Contains("~InvoiceSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~InvoiceSignStamp~", strHTML, intCompanyID.ToString());
                        }
                        if (strHTML.Contains("~POSignStamp~"))
                        {
                            strHTML = FunPubBindImages("~POSignStamp~", strHTML, intCompanyID.ToString());
                        }

                        //strnewFile += "\\" + "OtherCashflow" + "\\";
                        if (i == 0)
                        {
                            if (!Directory.Exists(strnewFile))
                            {
                                Directory.CreateDirectory(strnewFile);
                            }
                        }


                        strnewFile1 = dtSD.Rows[i]["Tranche_Name"].ToString().Replace("/", "_") + "_" + dtSD.Rows[i]["cashflow_flag_iD"].ToString() + ".pdf";
                        FileInfo fl = new FileInfo(strnewFile);
                        if (fl.Exists == true)
                        {
                            fl.Delete();
                        }

                        FunPrintWord(strHTML, strnewFile, strnewFile1, "0");

                    }

                }
                catch (Exception ex)
                {
                    return;
                }
            }


            if (dtTran.Rows[0]["ErrorCode"].ToString() == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
        }
        else if (ddlInvoiceType.SelectedValue == "4")
        {
            dtTran = new DataTable();
            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_Id", intCompanyID.ToString());
            objProcParam.Add("@User_Id", intUserId.ToString());
            objProcParam.Add("@Option", "4");
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());

            if (ddlInvoiceNo.SelectedValue != "")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);

            dtTran = Utility.GetDefaultData("S3G_LAD_InvoiceModification", objProcParam);

            if (dtTran.Rows[0]["ErrorCode"].ToString() == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
        }
        else if (ddlInvoiceType.SelectedValue == "5")
        {
            dtTran = new DataTable();
            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_Id", intCompanyID.ToString());
            objProcParam.Add("@User_Id", intUserId.ToString());
            objProcParam.Add("@Option", "5");
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());

            if (ddlInvoiceNo.SelectedValue != "")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);

            dtTran = Utility.GetDefaultData("S3G_LAD_InvoiceModification", objProcParam);

            if (dtTran.Rows[0]["ErrorCode"].ToString() == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
        }
        else if (ddlInvoiceType.SelectedValue == "6")
        {
            dtTran = new DataTable();
            objProcParam = new Dictionary<string, string>();
            objProcParam.Add("@Company_Id", intCompanyID.ToString());
            objProcParam.Add("@User_Id", intUserId.ToString());
            objProcParam.Add("@Option", "6");
            objProcParam.Add("@Tranche_Id", ddlTranche.SelectedValue);

            if (txtInvoiceDateTo.Text != "")
                objProcParam.Add("@Invoice_Date", Utility.StringToDate(txtInvoiceDateTo.Text).ToString());

            if (ddlInvoiceNo.SelectedValue != "")
                objProcParam.Add("@Invoice_No", ddlInvoiceNo.SelectedValue);

            dtTran = Utility.GetDefaultData("S3G_LAD_InvoiceModification", objProcParam);

            if (dtTran.Rows[0]["ErrorCode"].ToString() == "0")
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('Invoice Generated Successfully.');", true);
        }
    }

    private DataSet FunPriGetCashFlowDet(Dictionary<string, string> procParam)
    {
        DataSet dSet = new DataSet();
        S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubCashFlowPrint objCashflowPrint = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubCashFlowPrint();
        dSet = objCashflowPrint.FunPriGetCashFlowInt(procParam);
        return dSet;

    }

    private void GunfPubGetQrCode(string ReferenceNumber, string strnewFile)
    {
        try
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(ReferenceNumber, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            qrCodeImage.Save(strnewFile + "\\RentalQRCode.png");
        }
        catch (Exception ex)
        {

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

            qrCodeImage.Save(Server.MapPath(".") + "\\PDF Files\\RentQRCode.png");
        }
        catch (Exception ex)
        {

        }
    }

    public string GetAmountInWords(decimal dcmlVal)
    {
        try
        {
            string[] strarr = dcmlVal.ToString().Split('.');
            string strAmt = "";
            if (strarr[0] != null)
            {
                strAmt += FunGetAmountInWords(Convert.ToInt32(strarr[0]));
            }
            //if (strarr.Length > 1)
            //{
            //    if (strarr[1] != null && Convert.ToInt32(strarr[1]) > 0)
            //    {
            //        strAmt += " and " + FunGetAmountInWords(Convert.ToInt32(strarr[1]));
            //    }
            //}
            return strAmt + " only";
        }
        catch (Exception ex)
        {
            // ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    public string FunGetAmountInWords(int number)
    {
        try
        {
            if (number == 0) return "Zero";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units  
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands  
            num[3] = number / 10000000; // crores  
            num[2] = num[2] - 100 * num[3]; // lakhs  
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones  
                t = num[i] / 10;
                h = num[i] / 100; // hundreds  
                t = t - 10 * h; // tens  
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    //if (h > 0 || i == 0) sb.Append("and "); 
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        catch (Exception ex)
        {
            //ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPubDeleteTableHeader(string strtblName, string strHTML)
    {
        try
        {
            string row = "";
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strrow = "";
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);
            string strtempTable = strTable;

            startTag = "<TR";
            endTag = "</TR>";
            startIndex = strtempTable.IndexOf(startTag);
            endIndex = strtempTable.IndexOf(endTag, startIndex) + endTag.Length;
            strrow = strtempTable.Substring(startIndex, endIndex - startIndex);
            strtempTable = strtempTable.Replace(strrow, "");
            strHTML = strHTML.Replace(strTable, strtempTable);

            return strHTML;
        }
        catch (Exception ex)
        {
            // ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPubDeleteTable(string strtblName, string strHTML)
    {
        try
        {
            string newtr = String.Empty;
            var startTag = "";
            var endTag = "";
            int startIndex = 0;
            int endIndex = 0;
            string strTable;

            startTag = strtblName;
            endTag = "</TABLE>";
            startIndex = strHTML.LastIndexOf("<TABLE", strHTML.IndexOf(startTag) + startTag.Length);
            endIndex = strHTML.IndexOf(endTag, startIndex) + endTag.Length;
            strTable = strHTML.Substring(startIndex, endIndex - startIndex);

            strHTML = strHTML.Replace(strTable, "");
            return strHTML;
        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }
    }

    private string FunPubBindImages(string strImageName, string strHTML, string CompanyID)
    {
        try
        {
            string strFileName = @"D:\S3G_OPC\OPC_Service_LIVE_GST";

            string strImagePath = String.Empty;
            if (strImageName == "~CompanyLogo~")
            {

                strImagePath = strFileName + @"\TemplateImages\" + CompanyID + @"\CompanyLogo.png";
            }
            if (strImageName == "~InvoiceSignStamp~")
            {
                strImagePath = strFileName + @"\TemplateImages\" + CompanyID + @"\InvoiceSignStamp.png";
            }
            if (strImageName == "~POSignStamp~")
            {
                strImagePath = strFileName + @"\TemplateImages\" + CompanyID + @"\POSignStamp.png";
            }
            string ImageTag = "<img src='" + strImagePath + "' alt='Image'>";
            if (strHTML.Contains(strImageName))
                strHTML = strHTML.Replace(strImageName, ImageTag);
            return strHTML;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FunPrintWord(string strHTML, string strnewfile, string strnewfile1, string strIsCov)
    {
        string strhtmlFile = (strnewfile + "Bill_Html" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".html");
        string strwordFile = string.Empty;
        string strpdfFile = string.Empty;
        string strpdfFileName = string.Empty;

        strpdfFileName = strnewfile1;
        strpdfFile = strnewfile + "\\" + strnewfile1;

        try
        {
            if (File.Exists(strhtmlFile) == true)
            {
                File.Delete(strhtmlFile);
            }
            File.WriteAllText(strhtmlFile, strHTML);
            object file = strhtmlFile;
            object oMissing = System.Reflection.Missing.Value;
            object readOnly = false;
            object oFalse = false;

            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = new Microsoft.Office.Interop.Word.Document();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            oDoc = oWord.Documents.Open(ref file, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            Microsoft.Office.Interop.Word.Range rng = null;
            string img = string.Empty;

            //if (oDoc.InlineShapes.Count >= 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\login\s3g_logo.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}

            //if (oDoc.InlineShapes.Count == 1)
            //{
            //    System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            //    string strFileName = (string)AppReader.GetValue("ImagePath", typeof(string));// @"D:\S3G\SISLS3GPLayer\SISLS3GPLayer\Config.ini";// 
            //    img = strFileName + @"\Billsign.png";
            //    rng = oDoc.InlineShapes[1].Range;
            //    rng.Delete();
            //    rng.InlineShapes.AddPicture(img, false, true, Type.Missing);
            //}
            object fileFormat = null;


            fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            file = strpdfFile;

            //oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            ////oDoc.ActiveWindow.Selection.TypeText(" \t ");
            //oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            //Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            //oDoc.ActiveWindow.Selection.TypeText(" / ");
            //oDoc.ActiveWindow.Selection.Fields.Add(oDoc.ActiveWindow.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);
            oDoc = PDFPageSetup.SetWordProperties(oDoc);

            //if (strIsCov == "0")
            //{
            //    string textDisc = "* This is an electronically generated invoice and does not require any signature\n";
            //    oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            //    oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            //    oDoc.ActiveWindow.Selection.Font.Size = 7;
            //    oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            //    oDoc.ActiveWindow.Selection.TypeText(textDisc);
            //}

            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            Object CurrentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            oDoc.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            oDoc.ActiveWindow.Selection.TypeText(" / ");
            Object TotalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            oWord.ActiveWindow.Selection.Fields.Add(oWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //string text = "\nRegd. Office: D-16, Nelson Chambers, Nelson Manickam Road, Chennai, Tamil Nadu - 600029.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            string text = "\nRegd. Office: Door No 5, ALSA Tower, No 186/187, 7th Floor, Poonamallee High Road, Kilpak, Chennai- 600010.\nHead Office: 202, Natraj by Rustomjee, Sir M. V. Road, Western Express Highway, Andheri East, Mumbai - 400069.  ";
            oDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            oDoc.ActiveWindow.Selection.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oDoc.ActiveWindow.Selection.Font.Size = 9;
            oDoc.ActiveWindow.Selection.Font.Name = "Arial";
            oDoc.ActiveWindow.Selection.TypeText(text);
            //System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            string strFileName = @"D:\S3G_OPC\OPC_Service_LIVE_GST"; //(string)AppReader.GetValue("ImagePath", typeof(string));
            string footerimagepath = strFileName + @"\TemplateImages\1\OPCFooter.png";
            oDoc.ActiveWindow.Selection.InlineShapes.AddPicture(footerimagepath, oMissing, true, oMissing);

            oDoc.SaveAs(ref file, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                , ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.Close(ref oFalse, ref oMissing, ref oMissing);
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            File.Delete(strhtmlFile);


        }
        catch (Exception objException)
        {
            throw objException;
        }
    }

}