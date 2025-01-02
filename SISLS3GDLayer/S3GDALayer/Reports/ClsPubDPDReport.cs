using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;


namespace S3GDALayer.Reports
{
   namespace ReportMgtServices
   {
       public class ClsPubDPDReport:ClsPubDalReportBase
       {
          public DataSet FunPubGetDPDBucketDetails(int LOBID, int Bucket)
           {
               try
               {
                   DataSet ObjDS = new DataSet();
                   DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_BUCKETGENERATION);
                   db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                   db.AddInParameter(command, "@Bucket", DbType.Int32, Bucket);
                   db.LoadDataSet(command, ObjDS, "dtTable");
                   db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                   return ObjDS;
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }

          public DataSet FunPubGetDPDReportDetails(int LOBID,string CutOff,int Denomination,string Bucketdetails,string RcptUpto,int CompanyID, int Branch,int Region)
          {
              try
              {
                  
                  DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_DPD_REPORT);
                  db.AddInParameter(command, "@BUCKETPARAM", DbType.String, Bucketdetails);
                  db.AddInParameter(command, "@CUTOFF", DbType.String, CutOff);
                  db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                  db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, CompanyID);
                  db.AddInParameter(command, "@RCPT_UPTO", DbType.String, RcptUpto);
                  db.AddInParameter(command, "@BRANCH_ID", DbType.Int32, Branch);
                  db.AddInParameter(command, "@DENOM", DbType.Int32, Denomination);
                  db.AddInParameter(command, "@REGION_ID", DbType.Int32, Region);
                  S3GBusEntity.Reports.ReportMgtServices DSDPDReport = new S3GBusEntity.Reports.ReportMgtServices();
                  db.FunPubLoadDataSet(command, DSDPDReport, "##DPDTable");
                  return DSDPDReport;
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          public DataSet FunPubGetDPDReportDetailsForRPT(int LOBID, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Branch, int Region)
          {
              try
              {

                  DbCommand command = db.GetStoredProcCommand("S3G_RPT_DPD_REPORT_RPT");
                  db.AddInParameter(command, "@BUCKETPARAM", DbType.String, Bucketdetails);
                  db.AddInParameter(command, "@CUTOFF", DbType.String, CutOff);
                  db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                  db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, CompanyID);
                  db.AddInParameter(command, "@RCPT_UPTO", DbType.String, RcptUpto);
                  db.AddInParameter(command, "@BRANCH_ID", DbType.Int32, Branch);
                  db.AddInParameter(command, "@DENOM", DbType.Int32, Denomination);
                  db.AddInParameter(command, "@REGION_ID", DbType.Int32, Region);
                  S3GBusEntity.Reports.ReportMgtServices DSDPDReport = new S3GBusEntity.Reports.ReportMgtServices();
                  db.FunPubLoadDataSet(command, DSDPDReport, "##DPDTable");
                  return DSDPDReport;
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          public DataSet FunPubGetAssetDPDReportDetails(int LOBID, string LOB, int Class,string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Branch, int Region)
          {
              try
              {
                  
                  DbCommand command = db.GetStoredProcCommand(SPNames.S3G_RPT_DPD_ASSET_REPORT);
                  db.AddInParameter(command, "@BUCKETPARAM", DbType.String, Bucketdetails);
                  db.AddInParameter(command, "@CUTOFF", DbType.String, CutOff);
                  db.AddInParameter(command, "@LOB_ID", DbType.Int32, LOBID);
                  db.AddInParameter(command, "@LOB", DbType.String, LOB);
                  db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, CompanyID);
                  db.AddInParameter(command, "@RCPT_UPTO", DbType.String, RcptUpto);
                  db.AddInParameter(command, "@BRANCH_ID", DbType.Int32, Branch);
                  db.AddInParameter(command, "@DENOM", DbType.Int32, Denomination);
                  db.AddInParameter(command, "@REGION_ID", DbType.Int32, Region);
                  db.AddInParameter(command, "@CLASS_ID", DbType.Int32, Class);
                  S3GBusEntity.Reports.ReportMgtServices DSDPDReport = new S3GBusEntity.Reports.ReportMgtServices();
                  db.FunPubLoadDataSet(command, DSDPDReport, "##DPDTable");
                  return DSDPDReport;
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          
       }
   }
}
