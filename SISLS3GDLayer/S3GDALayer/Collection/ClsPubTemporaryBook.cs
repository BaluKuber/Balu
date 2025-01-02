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

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubTemporaryBook
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterDataTable objTempBook_DAL;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubTemporaryBook()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region TempBook
            public int FunPubCreateTempBookDetails(SerializationMode SerMode, byte[] byteObjS3G_Collection_TempReceiptBookMaster_DataTable)
            {
                try
                {
                    objTempBook_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterDataTable)
                        ClsPubSerialize.DeSerialize(byteObjS3G_Collection_TempReceiptBookMaster_DataTable, SerMode,
                        typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterRow objTempBookRow in objTempBook_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertTempBookDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objTempBookRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objTempBookRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objTempBookRow.Branch_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objTempBookRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objTempBookRow.Created_On);
                        //db.AddInParameter(command, "@Modified_On", DbType.DateTime, objTempBookRow.Modified_On);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objTempBookRow.Modified_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objTempBookRow.Modified_By);
                        db.AddInParameter(command, "@XMLAddDetail", DbType.String, objTempBookRow.XMLAddDetail);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected > 0)
                                {
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

            public int FunPubUpdateTempBookDetails(SerializationMode SerMode, byte[] byteObjS3G_Collection_TempReceiptBookMaster_DataTable, out string strUsedleafsNumber)
            {
                strUsedleafsNumber = "";
                try
                {
                    objTempBook_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterDataTable)
                        ClsPubSerialize.DeSerialize(byteObjS3G_Collection_TempReceiptBookMaster_DataTable, SerMode,
                        typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_TempReceiptBookMasterRow objTempBookRow in objTempBook_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateTempBook_Leafs");
                        db.AddInParameter(command, "@Temp_Receipt_Book_Id", DbType.Int32, objTempBookRow.Temp_Receipt_Book_Id);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objTempBookRow.Modified_By);
                        db.AddInParameter(command, "@LeafsXML", DbType.String, objTempBookRow.XMLAddDetail);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@UsedLeafs", DbType.String, 4000);
                         using (DbConnection conn = db.CreateConnection())
                     
                        //using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ERRORCODE"].Value == -3)
                                {
                                    intRowsAffected = (int)command.Parameters["@ERRORCODE"].Value;
                                    strUsedleafsNumber = Convert.ToString(command.Parameters["@UsedLeafs"].Value);
                                    trans.Commit();
                                }
                                else if ((int)command.Parameters["@ERRORCODE"].Value != 0 && (int)command.Parameters["@ERRORCODE"].Value != -3)
                                {
                                    intRowsAffected = (int)command.Parameters["@ERRORCODE"].Value;
                                    strUsedleafsNumber = Convert.ToString(command.Parameters["@UsedLeafs"].Value);
                                }
                                else
                                {
                                    strUsedleafsNumber = Convert.ToString(command.Parameters["@UsedLeafs"].Value);
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                intRowsAffected = 50;
                                trans.Rollback();
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
                }
                return intRowsAffected;
            }
            #endregion
        }
    }
}
