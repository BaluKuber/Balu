#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: ECS Details DAL Class
/// Created By			: Kannan RC
/// Created Date		: 05-Oct-2010
/// Purpose	            : 
/// <Program Summary>
#endregion


#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity.Collection;
#endregion



namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubECSSpoolFormats
        {
            #region
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolDataTable objECSSpool_DAL = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubECSSpoolFormats()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion


            #region Create ECS
            public int FunPubCreateECSFormat(SerializationMode SerMode, byte[] bytesobjS3G_cln_ECSDataTable)
            {
                try
                {

                    objECSSpool_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_cln_ECSDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolRow ObjECSRow in objECSSpool_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertECSSpoolFormat");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjECSRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjECSRow.LOB_ID);
                        if (!ObjECSRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjECSRow.Branch_ID);
                        db.AddInParameter(command, "@Bank_ID", DbType.Int32, ObjECSRow.Bank_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjECSRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjECSRow.Created_On);
                        //db.AddInParameter(command, "@XML_ECSDetails", DbType.String, ObjECSRow.XML_ECSDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_ECSDetails", OracleType.Clob,
                                ObjECSRow.XML_ECSDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjECSRow.XML_ECSDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_ECSDetails", DbType.String,
                                ObjECSRow.XML_ECSDeltails);
                        }
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjECSRow.Txn_ID);
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
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }

                }

                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion



            #region Modify ECS
            public int FunPubModifyECSFormat(SerializationMode SerMode, byte[] bytesobjS3G_cln_ECSDataTable)
            {
                try
                {

                    objECSSpool_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_cln_ECSDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ECSSpoolRow ObjECSRow in objECSSpool_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateECSFormatDetails");
                        db.AddInParameter(command, "@ECS_Spool_ID", DbType.Int32, ObjECSRow.ECS_Spool_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjECSRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjECSRow.LOB_ID);
                        if (!ObjECSRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjECSRow.Branch_ID);
                        db.AddInParameter(command, "@Bank_ID", DbType.Int32, ObjECSRow.Bank_ID);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjECSRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjECSRow.Modified_On);
                        //db.AddInParameter(command, "@XML_ECSDetails", DbType.String, ObjECSRow.XML_ECSDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_ECSDetails", OracleType.Clob,
                                ObjECSRow.XML_ECSDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjECSRow.XML_ECSDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_ECSDetails", DbType.String,
                                ObjECSRow.XML_ECSDeltails);
                        }
                        db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjECSRow.Txn_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //                                db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }

                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }

                }

                catch (Exception exp)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion
        }
    }
}
