#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Debt Collector Commission Processing
/// Created By			: Rajendran K
/// Created Date		: 19-Oct-2010
/// Purpose	            : DAL Class for Debt Collector Commission Processing
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using EntityCollection = S3GBusEntity.Collection;


namespace S3GDALayer.Collection
{
    namespace ClnDebtMgtServices
    {
      public class ClsPubDCCommissionProcessing
      {
          #region  Declaration Part
          int errorCode;
          EntityCollection.ClnDebtMgtServices dsDCCommission=null;

          EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionDataTable objDebtCommissionPaymentTable= null;
          EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionPaymentDetailsDataTable objDebtCommissionPaymentDetailsTable=null;
          EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionRow ObjDebtCollectorMasterRow = null;
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDCCommissionProcessing()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

          DbCommand dbCmd;

          #endregion

          public int FunCreateDCCommisionProcessing(SerializationMode serMode, byte[] bytesObjDCCommisionPaymentTable)
          {
              try
              {
                  //db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                  DbCommand command = db.GetStoredProcCommand(SPNames.S3G_CLN_InsertDCIncenticeProcessing);

                  objDebtCommissionPaymentTable = (EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionDataTable)ClsPubSerialize.DeSerialize(bytesObjDCCommisionPaymentTable, serMode, typeof(EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionDataTable));
                  ObjDebtCollectorMasterRow = objDebtCommissionPaymentTable.Rows[0] as EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionRow;


                  db.AddInParameter(command, "@Company_ID", DbType.Int16, ObjDebtCollectorMasterRow.Company_ID);
                  if(!ObjDebtCollectorMasterRow.IsLOB_IDNull())
                  db.AddInParameter(command, "@LOB_ID",DbType.Int16, ObjDebtCollectorMasterRow.LOB_ID);
                   if(!ObjDebtCollectorMasterRow.IsBranch_IDNull())
                  db.AddInParameter(command, "@Location_ID",DbType.Int16, ObjDebtCollectorMasterRow.Branch_ID);
                  db.AddInParameter(command, "@Demand_Month",DbType.String, ObjDebtCollectorMasterRow.Demand_Month);
                  db.AddInParameter(command, "@Created_By",DbType.Int16, ObjDebtCollectorMasterRow.Created_By);
                  db.AddInParameter(command, "@Created_On",DbType.DateTime, ObjDebtCollectorMasterRow.Created_On);                  
                  db.AddInParameter(command, "@Txn_ID", DbType.Int16, ObjDebtCollectorMasterRow.Txn_ID);
                  db.AddInParameter(command, "@Frequency_Type", DbType.Int16, ObjDebtCollectorMasterRow.Frequency_Type);
                  db.AddInParameter(command, "@XMLParamtDCIncentiveProcess", DbType.String, ObjDebtCollectorMasterRow.XMLParamtDCIncentiveProcessing);
                  db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                  db.FunPubExecuteNonQuery(command);

                  if ((int)command.Parameters["@ErrorCode"].Value > 0)
                      errorCode= (int)command.Parameters["@ErrorCode"].Value;

              }
              catch (Exception ex)
              {
                   ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
              }
              return errorCode;
          }
          public EntityCollection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionDataTable FuncQueryDCCommissionDetails(int DC_CommissionId)
          {
              try
              {
                  //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                  dsDCCommission = new S3GBusEntity.Collection.ClnDebtMgtServices();

                  DbCommand command = db.GetStoredProcCommand(SPNames.S3G_CLN_GetDCCommissionDetails);
                  db.AddInParameter(command, "@DC_Commission_ID", DbType.Int16, DC_CommissionId);
                  db.FunPubLoadDataSet(command, dsDCCommission, dsDCCommission.S3G_CLN_DebtCollectorCommission.TableName);

                  return dsDCCommission.S3G_CLN_DebtCollectorCommission;
              }
              catch (Exception sqlEx)
              {
                  throw;
              }
          }


          /// <summary>
          /// 
          /// </summary>
          /// <param name="strProcName">Procedure name used for grid binding </param>
          /// <param name="dctProcParams">Parameter for the Procedure</param>
        
          /// <returns>DataSet</returns>
          public DataSet FunPubGetDatasetGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging)
          {
              intTotalRecords = 0;
              DataSet ds = new DataSet();
              try
              {
                  //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                  DbCommand command = db.GetStoredProcCommand(strProcName);
                  foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                  {
                      db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                  }
                  db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                  db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPaging.ProUser_ID);
                  db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                  db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                  db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                  db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                  db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                  db.FunPubLoadDataSet(command, ds, "GridTable");

                  if ((int)command.Parameters["@TotalRecords"].Value > 0)
                      intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;

              }
              catch (Exception ex)
              {
                   ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                  throw ex;
              }
              return ds;

          }
      }
    }
}
