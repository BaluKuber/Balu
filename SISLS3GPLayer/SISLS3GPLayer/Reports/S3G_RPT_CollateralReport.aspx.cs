using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using S3GBusEntity;
using S3GBusEntity.Reports;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Enterprise;
using ReportAccountsMgtServicesReference;

public partial class Reports_S3G_RPT_CollateralReport : System.Web.UI.Page
{
    S3GSession objSession = new S3GSession();
    string strCompanyId, strLOBID, strUser_ID, strLocation1ID, strLocation2ID, strUserID,
        strCustomerID, strCollateralTypeID, strCollateralStatusID, strFromReportDate, strToReportDate,strProgram_ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        strCompanyId = Request.QueryString.Get("qsCompanyId");
        strLOBID = Request.QueryString.Get("qsLOBID");
        strLocation1ID = Request.QueryString.Get("qsLOCATION1ID");
        strLocation2ID = Request.QueryString.Get("qsLOCATION2ID");
        strCustomerID = Request.QueryString.Get("qsCustomerID");
        strCollateralTypeID = Request.QueryString.Get("qsCollateralTypeID");
        strCollateralStatusID = Request.QueryString.Get("qsCollateralStatusID");
        strFromReportDate = Request.QueryString.Get("qsFromReportDate");
        strToReportDate = Request.QueryString.Get("qsToReportDate");
        strUser_ID = Request.QueryString.Get("qsUserID");
        strProgram_ID = "230";
        if (!IsPostBack)
        {
            ViewState["CompanyName"] = GetCompanyAddressDetails(Convert.ToInt32(strCompanyId));            
        }
        FunPriLoadReport();
    }

    private string GetCompanyAddressDetails(int companyId)
    {
        CompanyMgtServicesReference.CompanyMgtServicesClient objCompanyMasterClient;
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjS3G_CompanyMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable();
        StringBuilder sbCompanyAddr = new StringBuilder();
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable dtCompany;
        S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjCompanyMasterRow;
        ObjCompanyMasterRow = ObjS3G_CompanyMaster_ViewDataTable.NewS3G_SYSAD_CompanyMaster_ViewRow();
        ObjCompanyMasterRow.Company_ID = 0;
        ObjS3G_CompanyMaster_ViewDataTable.AddS3G_SYSAD_CompanyMaster_ViewRow(ObjCompanyMasterRow);
        objCompanyMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] byteCompanyDetails = objCompanyMasterClient.FunPubQueryCompany(SerMode, ClsPubSerialize.Serialize(ObjS3G_CompanyMaster_ViewDataTable, SerMode));
        dtCompany = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteCompanyDetails, SerializationMode.Binary, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));

        if (dtCompany.Rows.Count > 0)
        {
            DataRow drCompanyAddr = dtCompany.Rows[0];
            sbCompanyAddr.Append("<center>");
            sbCompanyAddr.Append(drCompanyAddr["Address1"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["Address2"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["City"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["State"].ToString());
            sbCompanyAddr.Append(",");
            sbCompanyAddr.Append(drCompanyAddr["Country"].ToString());
            sbCompanyAddr.Append(" - ");
            sbCompanyAddr.Append(drCompanyAddr["Zip_Code"].ToString());
            sbCompanyAddr.Append("</br>Ph :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Telephone_Number"].ToString());
            sbCompanyAddr.Append("  Mobile :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Mobile_Number"].ToString());
            sbCompanyAddr.Append("</br>EMail :");
            sbCompanyAddr.Append(drCompanyAddr["CD_Email_ID"].ToString());
            sbCompanyAddr.Append("</br>");
            sbCompanyAddr.Append(drCompanyAddr["CD_Website"].ToString());
            sbCompanyAddr.Append("</center>");
            ViewState["FromMail"] = drCompanyAddr["CD_Email_ID"].ToString();

        }
        objCompanyMasterClient.Close();
        return sbCompanyAddr.ToString();
    }

    private void FunPriLoadReport()
    {
        ReportAccountsMgtServicesClient objSerClient = new ReportAccountsMgtServicesClient();
        ReportDocument rpt = new ReportDocument();
        try
        {
            ClsPubCollateralHeader objHeader = new ClsPubCollateralHeader();
            objHeader.LOBId = strLOBID;
            objHeader.LocationId1 = strLocation1ID;
            objHeader.LocationId2 = strLocation2ID;
            objHeader.CompanyId = strCompanyId;
            objHeader.ProgramId = Convert.ToInt32(strProgram_ID);
            if (strCustomerID == "")
            {
                objHeader.CustomerId = "0";
            }
            else
            {
                objHeader.CustomerId = strCustomerID;
            }
            objHeader.UserId = strUser_ID;
            objHeader.CollateralTypeId = strCollateralTypeID;
            objHeader.StatusId = strCollateralStatusID;
            objHeader.StartDate = Utility.StringToDate(strFromReportDate);
            objHeader.EndDate = Utility.StringToDate(strToReportDate);
            byte[] byteHeader = ClsPubSerialize.Serialize(objHeader, SerializationMode.Binary);
            byte[] byteCollateral = objSerClient.FunPubGetCollateralDetails(byteHeader);
            List<ClsPubCollateralReport> objlist = new List<ClsPubCollateralReport>();
            objlist = (List<ClsPubCollateralReport>)DeSeriliaze(byteCollateral);
            rpt.Load(Server.MapPath(@"Report\S3G_Rpt_CollateralReport.rpt"));
            rpt.SetDataSource(objlist);
            TextObject txtReportDate = (TextObject)rpt.ReportDefinition.Sections["Section2"].ReportObjects["txtReportDate"];
            txtReportDate.Text = DateTime.Today.ToString(objSession.ProDateFormatRW);
            TextObject txtReportTitle = (TextObject)rpt.ReportDefinition.Sections["Section2"].ReportObjects["txtReportTitle"];
            txtReportTitle.Text = "Collateral Report for the period from "+ strFromReportDate + " to " + strToReportDate;                                  
            CRVCollateral.ReportSource = rpt;
            CRVCollateral.DataBind();
        }
        catch (Exception ae)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ae);
            throw ae;
        }
        finally
        {
            objSerClient.Close();            
        }
    }

    private object DeSeriliaze(byte[] byteObj)
    {
        return ClsPubSerialize.DeSerialize(byteObj, SerializationMode.Binary, null);
    }
}
