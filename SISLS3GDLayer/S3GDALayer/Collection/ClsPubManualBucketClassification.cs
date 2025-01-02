using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using S3GBusEntity;
using System.Data.Common;
using System.Data;
using System.Data.OracleClient;

namespace S3GDALayer.Collection
{
    namespace ClnReceivableMgtServices
    {
        public class ClsPubManualBucketClassification
        {
            int intRowsAffected = 0;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassifcationDataTable S3G_CLN_ManualBucketClassifcationDataTable;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassificationCategoryDataTable S3G_CLN_ManualBucketClassificationCategoryDataTable;
            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
            //Code added for getting common connection string  from config file
            Database db;            
            public ClsPubManualBucketClassification()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateManualBucketClassifcation(SerializationMode SerMode, byte[] bytesManualBucketClassifcation_Datatable)
            {
                try
                {                    

                    S3G_CLN_ManualBucketClassifcationDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassifcationDataTable)ClsPubSerialize.DeSerialize(bytesManualBucketClassifcation_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassifcationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertManualBucketClassification");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassifcationRow ObjClassifcationRow in S3G_CLN_ManualBucketClassifcationDataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjClassifcationRow.Company_ID);
                        db.AddInParameter(command, "@From_DebtCollector_Code", DbType.String, ObjClassifcationRow.From_DebtCollector_Code);
                        db.AddInParameter(command, "@To_DebtCollector_Code", DbType.String, ObjClassifcationRow.To_DebtCollector_Code);
                        db.AddInParameter(command, "@Demand_Month", DbType.Decimal, ObjClassifcationRow.Demand_Month);
                        db.AddInParameter(command, "@Transfer_Amount", DbType.Decimal, ObjClassifcationRow.Transfer_Amount);
                        db.AddInParameter(command, "@Transfer_Date", DbType.DateTime, ObjClassifcationRow.Transfer_Date);
                        db.AddInParameter(command, "@DebtCollector_Code", DbType.String, ObjClassifcationRow.DebtCollector_Code);
                        db.AddInParameter(command, "@OLD_Demand_Month_Debit", DbType.Decimal, ObjClassifcationRow.OLD_Demand_Month_Debit);
                        db.AddInParameter(command, "@OLD_Demand_Month_Credit", DbType.Decimal, ObjClassifcationRow.OLD_Demand_Month_Credit);
                        db.AddInParameter(command, "@NEW_Demand_Month_Debit", DbType.Decimal, ObjClassifcationRow.NEW_Demand_Month_Debit);
                        db.AddInParameter(command, "@NEW_Demand_Month_Credit", DbType.Decimal, ObjClassifcationRow.NEW_Demand_Month_Credit);

                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;

                            param = new OracleParameter("@XMLDues",
                                 OracleType.Clob, ObjClassifcationRow.XMLDues.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjClassifcationRow.XMLDues);

                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDues", DbType.String, ObjClassifcationRow.XMLDues);
                        }

                       // db.AddInParameter(command, "@XMLDues", DbType.String, ObjClassifcationRow.XMLDues);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if (intRowsAffected == 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 20;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

            //For Category

            public int FunPubModifyManualBucketClassifcationcategory(SerializationMode SerMode, byte[] bytesManualBucketClassifcationCategory_Datatable)
            {
                try
                {
                    S3G_CLN_ManualBucketClassificationCategoryDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassificationCategoryDataTable)ClsPubSerialize.DeSerialize(bytesManualBucketClassifcationCategory_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassificationCategoryDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateManualBucketClassificationCategory");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_ManualBucketClassificationCategoryRow ObjClassifcationRow in S3G_CLN_ManualBucketClassificationCategoryDataTable.Rows)
                    {

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjClassifcationRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjClassifcationRow.User_Id);
                        if (!ObjClassifcationRow.IsLob_IdNull())
                            db.AddInParameter(command, "@Lob_Id", DbType.Int32, ObjClassifcationRow.Lob_Id);
                        db.AddInParameter(command, "@Category_Type", DbType.Int32, ObjClassifcationRow.Category_Type);
                        db.AddInParameter(command, "@Category_Code", DbType.Int32, ObjClassifcationRow.Category_Code);
                        db.AddInParameter(command, "@Demand_Month", DbType.Decimal, ObjClassifcationRow.Demand_Month);
                        db.AddInParameter(command, "@DebtCollector_Code", DbType.String, ObjClassifcationRow.DebtCollector_Code);
                        //db.AddInParameter(command, "@XMLDues", DbType.String, ObjClassifcationRow.XMLDues);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;

                            param = new OracleParameter("@XMLDues",
                                 OracleType.Clob, ObjClassifcationRow.XMLDues.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, ObjClassifcationRow.XMLDues);

                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDues", DbType.String, ObjClassifcationRow.XMLDues);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        }

                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }


        }
    }
}


