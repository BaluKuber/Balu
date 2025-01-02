using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;
using System.Data.OracleClient;

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubECSProcess
        {
            int intRowsAffected;
            string strECSNo = "";
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSDataTable objECSProcess_DAl;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubECSProcess()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the ECS Process Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateECSProcess(SerializationMode SerMode, byte[] bytesObjS3G_Colection_ECSProcess_DataTable, out string strECSNumber)
            {
                strECSNumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    objECSProcess_DAl = (S3GBusEntity.Collection.ClnReceiptMgtServices.
                        S3G_CLN_ECSDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_ECSProcess_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnReceiptMgtServices.S3G_CLN_ECSDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");


                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSRow objECSProcessRow in objECSProcess_DAl)
                    {
                        DbCommand command = db.GetStoredProcCommand("[S3G_CLN_InsertECSProcess]");
                        command.CommandTimeout = 1200;
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objECSProcessRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objECSProcessRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objECSProcessRow.Branch_ID);
                        db.AddInParameter(command, "@ECS_No", DbType.String, objECSProcessRow.ECS_No);
                        db.AddInParameter(command, "@ECS_DocDate", DbType.DateTime, objECSProcessRow.ECS_DocDate);
                        db.AddInParameter(command, "@Fixed_Billing_date", DbType.DateTime, objECSProcessRow.Fixed_Billing_date);
                        db.AddInParameter(command, "@Authorized_By", DbType.Int32, objECSProcessRow.Authorized_By);
                        db.AddInParameter(command, "@Authorized_Date", DbType.DateTime, objECSProcessRow.Authorized_Date);
                        db.AddInParameter(command, "@Authorization_Status", DbType.String, objECSProcessRow.Authorization_Status);
                        db.AddInParameter(command, "@FilePath", DbType.String, objECSProcessRow.FilePath);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, objECSProcessRow.Account_Link_key);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objECSProcessRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objECSProcessRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objECSProcessRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objECSProcessRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objECSProcessRow.Txn_ID);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            /*  added by senthilkumar p on 07/mar/2012 */

                            OracleParameter param, param1;
                            param = new OracleParameter("@XMLDetail",
                                 OracleType.Clob, objECSProcessRow.XMLDetails.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, objECSProcessRow.XMLDetails);
                            command.Parameters.Add(param);

                            param1 = new OracleParameter("@XMLReceipt",
                                 OracleType.Clob, objECSProcessRow.XMLReceipt.Length,
                                 ParameterDirection.Input, true, 0, 0, String.Empty,
                                  DataRowVersion.Default, objECSProcessRow.XMLReceipt);
                            command.Parameters.Add(param1);

                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDetail", DbType.String, objECSProcessRow.XMLDetails);
                            db.AddInParameter(command, "@XMLReceipt", DbType.String, objECSProcessRow.XMLReceipt);
                        }
                        db.AddInParameter(command, "@Password", DbType.String, objECSProcessRow.Password);                        

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@strECSNo", DbType.String, 100);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    throw new Exception("Error in ECS " + intRowsAffected.ToString());
                                }
                                else
                                {
                                    strECSNo = Convert.ToString(command.Parameters["@strECSNo"].Value);
                                    strECSNumber = Convert.ToString(command.Parameters["@strECSNo"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                throw ex;
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
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }
        }
    }
}
