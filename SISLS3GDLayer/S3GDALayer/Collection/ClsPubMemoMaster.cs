#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Memo Details DAL Class
/// Created By			: Kannan RC
/// Created Date		: 05-Oct-2010
/// Purpose	            : 
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using S3GBusEntity.Collection;
#endregion


namespace S3GDALayer.Collection
{
    namespace ClnMemoMgtServices
    {
        public class ClsPubMemoMaster
        {

            #region Initialization
            S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable objMemoMaster_DAL = null;
            int intRowsAffected;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubMemoMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion


            #region Create Memo
            public int FunPubCreateMemoDetails(SerializationMode SerMode, byte[] bytesobjS3G_cln_MemoDataTable)
            {
                try
                {

                    objMemoMaster_DAL = (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_cln_MemoDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterRow ObjMemoRow in objMemoMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertMemoDetails");
                        //db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMemoRow.Company_ID);
                        //db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjMemoRow.LOB_ID);
                        //db.AddInParameter(command, "@Created_By", DbType.Int32, ObjMemoRow.Created_By);
                        //db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjMemoRow.Created_On);
                        //db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjMemoRow.Is_Active);
                        //db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjMemoRow.Txn_ID);
                        db.AddInParameter(command, "@Level", DbType.String, ObjMemoRow.Level);
                        //db.AddInParameter(command, "@XML_MemoDeltails", DbType.String, ObjMemoRow.XML_MemoDeltails);                       
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_MemoDeltails", OracleType.Clob,
                                ObjMemoRow.XML_MemoDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjMemoRow.XML_MemoDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_MemoDeltails", DbType.String,
                                ObjMemoRow.XML_MemoDeltails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        //using (DbConnection conn = db.CreateConnection())
                        //{
                        //    conn.Open();
                        //    DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command);
                                  intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                 

                                //    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                //}
                                //else
                                //{
                                //    trans.Commit();
                                //}

                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                               // trans.Rollback();
                            }
                            //finally
                            //{
                            //    conn.Close();
                            //}
                        }

                    }

                

                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion

            #region Modify Memo
            public int FunPubModifyMemoDetails(SerializationMode SerMode, byte[] bytesobjS3G_cln_MemoDataTable)
            {
                try
                {

                    objMemoMaster_DAL = (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_cln_MemoDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterRow ObjMemoRow in objMemoMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateMemoDetails");
                        //db.AddInParameter(command, "@Memo_ID", DbType.Int32, ObjMemoRow.Memo_ID);
                        //db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMemoRow.Company_ID);
                        //db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjMemoRow.LOB_ID);
                        //db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjMemoRow.Modified_By);
                        //db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjMemoRow.Modified_On);
                        //db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjMemoRow.Is_Active);
                        //db.AddInParameter(command, "@TXN_Id", DbType.Int32, ObjMemoRow.Txn_ID);
                        db.AddInParameter(command, "@Level", DbType.String, ObjMemoRow.Level);
                        //db.AddInParameter(command, "@XML_MemoDeltails", DbType.String, ObjMemoRow.XML_MemoDeltails);                        
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_MemoDeltails", OracleType.Clob,
                                ObjMemoRow.XML_MemoDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjMemoRow.XML_MemoDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_MemoDeltails", DbType.String,
                                ObjMemoRow.XML_MemoDeltails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans  = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                //{
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                //    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                //}
                                //else
                                //{
                                //    trans.Commit();
                                //}

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
