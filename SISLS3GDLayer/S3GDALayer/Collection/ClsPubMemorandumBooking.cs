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
    namespace ClnMemoMgtServices
    {
        public class ClsPubMemorandumBooking
        {
            int intRowsAffected;  
            S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingDataTable objMemorandumBooking_DAL;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubMemorandumBooking()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the Memorandum Booking Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns> 
            /// 
            public int FunPubCreateModifyMemorandumBooking(SerializationMode SerMode, byte[] bytesObjS3G_Colection_MemorandumBooking_DataTable, out string strMemoBookNo)
            {
                strMemoBookNo = string.Empty;
                try
                {
                    objMemorandumBooking_DAL = (S3GBusEntity.Collection.ClnMemoMgtServices.
                        S3G_CLN_MemorandumBookingDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_MemorandumBooking_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnMemoMgtServices.S3G_CLN_MemorandumBookingDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingRow objMemorandumBookingRow in objMemorandumBooking_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertUpdateMemoBooking");
                        db.AddInParameter(command, "@Memo_Link_Key", DbType.Int32, objMemorandumBookingRow.Memo_Link_Key);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objMemorandumBookingRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objMemorandumBookingRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objMemorandumBookingRow.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objMemorandumBookingRow.Customer_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, objMemorandumBookingRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objMemorandumBookingRow.SANum);
                        //db.AddInParameter(command, "@Memo_Type_Code", DbType.Int32, objMemorandumBookingRow.Memo_Type_Code);
                        //db.AddInParameter(command, "@Memo_Type", DbType.Int32, objMemorandumBookingRow.Memo_Type);
                        //db.AddInParameter(command, "@Due_Amount", DbType.Decimal, objMemorandumBookingRow.Due_Amount);
                       // db.AddInParameter(command, "@Received_Amount", DbType.Decimal, objMemorandumBookingRow.Received_Amount);

                        db.AddInParameter(command, "@Created_By", DbType.Int32, objMemorandumBookingRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objMemorandumBookingRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objMemorandumBookingRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objMemorandumBookingRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objMemorandumBookingRow.Txn_ID);
                        db.AddInParameter(command, "@XMLAddDetail", DbType.String, objMemorandumBookingRow.MemorandumAddDetails);
                        db.AddInParameter(command, "@XMLDeleteDetail", DbType.String, objMemorandumBookingRow.MemorandumDeleteDetails);
                        db.AddInParameter(command, "@XMLUpdateDetail", DbType.String, objMemorandumBookingRow.MemorandumUpdateDetails);
                        db.AddInParameter(command, "@OperationType", DbType.Int32, objMemorandumBookingRow.OperationType);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@RefNo", DbType.String, 1000);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected > 0)
                                {
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                    strMemoBookNo = (string)command.Parameters["@RefNo"].Value;
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
                        }

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
             

            /// <summary>
            /// To use  Market Entry Value Details 
            /// </summary>
            /// <param name="Market_Value_ID">Pass Market_Value_ID</param>
            /// <returns></returns>

            public DataTable FunModifyMemorandumBooking(string strMarketValueNo)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetMarketEntryValue");
                    db.AddInParameter(command, "@MarketValueNo", DbType.String, strMarketValueNo);
                    db.LoadDataSet(command, ObjDS, "dtTable");

                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
