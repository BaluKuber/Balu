using System;using S3GDALayer.S3GAdminServices;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using S3GBusEntity;
using System.Data.OracleClient;

namespace S3GDALayer.Collection
{
    namespace ClnReceivableMgtServices
    {

        public class ClsPubDemandProcessing
        {
            int intErrorCode = 0;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable ObjS3G_CLN_DemandHeaderDataTable;
              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDemandProcessing()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateDemandProcessing(SerializationMode SerMode, byte[] bytesDemandProcessing_Datatable)
            {
                try
                {
                    ObjS3G_CLN_DemandHeaderDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable)ClsPubSerialize.DeSerialize(bytesDemandProcessing_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertDemandProcessing");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderRow ObjDemandHeaderRow in ObjS3G_CLN_DemandHeaderDataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDemandHeaderRow.Company_ID);

                        if (!ObjDemandHeaderRow.IsLOB_IDNull())
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDemandHeaderRow.LOB_ID);

                        db.AddInParameter(command, "@Demand_Type", DbType.Int32, ObjDemandHeaderRow.Demand_Type);
                        db.AddInParameter(command, "@Demand_Month", DbType.String, ObjDemandHeaderRow.Demand_Month);

                        if (!ObjDemandHeaderRow.IsCutoff_DateNull())
                            db.AddInParameter(command, "@Cutoff_Date", DbType.String, ObjDemandHeaderRow.Cutoff_Date);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_DemandDetails", OracleType.Clob,
                                    ObjDemandHeaderRow.Xml_DemandDetails.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjDemandHeaderRow.Xml_DemandDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_DemandDetails", DbType.String, ObjDemandHeaderRow.Xml_DemandDetails);
                        }
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDemandHeaderRow.Created_By);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                                if (intErrorCode > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public int FunPubModifyDemandProcessing(SerializationMode SerMode, byte[] bytesDemandProcessing_Datatable)
            {
                try
                {
                    ObjS3G_CLN_DemandHeaderDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable)ClsPubSerialize.DeSerialize(bytesDemandProcessing_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateDemandProcessing");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DemandHeaderRow ObjDemandHeaderRow in ObjS3G_CLN_DemandHeaderDataTable.Rows)
                    {
                        db.AddInParameter(command, "@Demand_ProceesId", DbType.Int32, ObjDemandHeaderRow.Demand_Process_ID);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_DemandDetails", OracleType.Clob,
                                    ObjDemandHeaderRow.Xml_DemandDetails.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjDemandHeaderRow.Xml_DemandDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_DemandDetails", DbType.String, ObjDemandHeaderRow.Xml_DemandDetails);
                        }

                        db.AddInParameter(command, "@Modified_by", DbType.Int32, ObjDemandHeaderRow.Created_By);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;

                                if (intErrorCode > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public DataSet FunPubGetService(string ServiceName)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_LoanAd_GetSerivce");
                    db.AddInParameter(command, "@Service_Name", DbType.String, ServiceName);
                    db.FunPubLoadDataSet(command, ObjDS, "Service");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void FunPriAssignDemandCollector(string strDCrule, string strDebtCollector, int? intSalesPerson, int? intLobid, int? intBranchId, int? intRegionId, int? ProductId, string strPinCode, string strRulePeriod, string strRuleValue, string strCategory, int intDemandId)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_Cln_AssignDC");
                db.AddInParameter(command, "@DCRuleCard_ID", DbType.String, strDCrule);
                db.AddInParameter(command, "@Debt_Collector", DbType.String, strDebtCollector);
                db.AddInParameter(command, "@Sale_Person_ID", DbType.Int32, intSalesPerson);
                db.AddInParameter(command, "@Lob_ID", DbType.Int32, intLobid);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchId);
                db.AddInParameter(command, "@Region_ID", DbType.Int32, intRegionId);
                db.AddInParameter(command, "@Product_Id", DbType.Int32, ProductId);
                db.AddInParameter(command, "@PIN_CODE", DbType.String, strPinCode);
                db.AddInParameter(command, "@Rule_Period", DbType.String, strRulePeriod);
                db.AddInParameter(command, "@Rule_Value", DbType.String, strRuleValue);
                db.AddInParameter(command, "@Category", DbType.String, strCategory);
                db.AddInParameter(command, "@DemandProcess_Id", DbType.Int32, intDemandId);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {

                        db.FunPubExecuteNonQuery(command,ref trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        trans.Rollback();
                    }
                }
            }

            public DataTable FunPubAssignDC(int intCompanyId,int intOption,int intUserId,int intProramId)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3g_CLN_DemandProcessing_List");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@Option", DbType.Int32, intOption);
                db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);
                db.AddInParameter(command, "@Program_Id", DbType.Int32, intProramId);
                DataTable dtDcRules = db.FunPubExecuteDataSet(command).Tables[0];
                return dtDcRules;
            }

            public void FunPubUpdateAssignDC(int intDemandProcess_Id)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_Cln_UpdateAssignDC");
                db.AddInParameter(command, "@DemandProcess_Id", DbType.Int32, intDemandProcess_Id);
                db.FunPubExecuteNonQuery(command);
            }

            public DataTable FunPriGetDcRuleDetails(int intCompanyId, int intOption, string strDCrule, int? intLobid, int? intBranchId, string strSequence,int intUserId,int intProramId)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3g_CLN_DemandProcessing_List");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                db.AddInParameter(command, "@Option", DbType.Int32, intOption);
                db.AddInParameter(command, "@DCRuleCard_ID", DbType.String, strDCrule);
                db.AddInParameter(command, "@Lob_ID", DbType.Int32, intLobid);
                db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchId);
                db.AddInParameter(command, "@Sequence_String", DbType.String, strSequence);
                db.AddInParameter(command, "@User_Id", DbType.Int32, intUserId);
                db.AddInParameter(command, "@Program_Id", DbType.Int32, intProramId);
                DataTable dtDcRulesDetails = db.FunPubExecuteDataSet(command).Tables[0];
                return dtDcRulesDetails;
            }

            public void FunPubUpdateService(string ServiceName, string strStatus, string strDocNo, int? intRecCount, int intCompanyId)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertS3gService");
                    db.AddInParameter(command, "@ServiceName", DbType.String, ServiceName);
                    db.AddInParameter(command, "@Status", DbType.String, strStatus);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Doc_No", DbType.String, strDocNo);

                    if (intRecCount.HasValue)
                    {
                        db.AddInParameter(command, "@RECORD_COUNT", DbType.Int32, intRecCount);
                    }
                    db.FunPubExecuteNonQuery(command);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
