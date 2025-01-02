using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using S3GDALayer.Constants;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity.Reports;
using System.Data.Common;
using System.Data.Sql;
using System.Xml.Linq;
using System.Data;

namespace S3GDALayer.Reports
{
    namespace ReportAccountsMgtServices
    {
        public class ClsPubRptRepossession : ClsPubDalReportBase
        {
            public DataSet FunPubGetRepossessionDetails(int LOBID, int Location, string FromDate, string ToDate, int CompanyID, int GarageNo, int ProgramID, int UserID)
            {
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_RPT_REPOSSESSION_REPORT");
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                    db.AddInParameter(command, "@Location", DbType.Int32, LOBID);
                    db.AddInParameter(command, "@FromDate", DbType.String, FromDate);
                    db.AddInParameter(command, "@ToDate", DbType.String, ToDate);
                    db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@GarageNo", DbType.String, GarageNo);
                    db.AddInParameter(command, "@ProgramID", DbType.Int32, ProgramID);
                    db.AddInParameter(command, "@UserID", DbType.Int32, UserID);                   
                    S3GBusEntity.Reports.ReportMgtServices DSRepoReport = new S3GBusEntity.Reports.ReportMgtServices();
                    db.FunPubLoadDataSet(command, DSRepoReport, "##RepoTable");
                    return DSRepoReport;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
