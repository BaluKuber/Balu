using System;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GDALayer.Common;
using S3GBusEntity;

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubFunderReceipt
        {
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_FunderReceiptDataTable objReceiptProcessingTable;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubFunderReceipt()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateFunderReceipt(SerializationMode SerMode, byte[] bytesObjReceiptProcessing_DataTable, out string strReceiptNumber)
            {
                int intErrorCode = 0;
                strReceiptNumber = "";
                try
                {
                    objReceiptProcessingTable = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_FunderReceiptDataTable)
                                        ClsPubSerialize.DeSerialize(bytesObjReceiptProcessing_DataTable,
                                    SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_FunderReceiptDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_FunderReceiptRow objReceiptRow in objReceiptProcessingTable)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_INSERT_FunderRcpt");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objReceiptRow.Company_ID);
                        db.AddInParameter(command, "@Funder_Receipt_ID", DbType.Int64, objReceiptRow.Funder_Receipt_ID);
                        db.AddInParameter(command, "@Receipt_No", DbType.String, objReceiptRow.Receipt_No);
                        db.AddInParameter(command, "@Lob_ID", DbType.Int32, objReceiptRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objReceiptRow.Location_ID);
                        db.AddInParameter(command, "@Receipt_Mode", DbType.Int32, objReceiptRow.Receipt_Mode);
                        db.AddInParameter(command, "@Doc_Date", DbType.DateTime, objReceiptRow.Doc_Date);
                        db.AddInParameter(command, "@Doc_Amount", DbType.Double, objReceiptRow.Doc_Amount);
                        db.AddInParameter(command, "@Value_Date", DbType.DateTime, objReceiptRow.Value_Date);
                        db.AddInParameter(command, "@Tranche_ID", DbType.Int64, objReceiptRow.Tranche_ID);
                        db.AddInParameter(command, "@Account_Based", DbType.Int32, objReceiptRow.Account_Based);
                        db.AddInParameter(command, "@Funder_ID", DbType.Int64, objReceiptRow.Funder_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int64, objReceiptRow.Customer_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objReceiptRow.Created_By);
                        if (!objReceiptRow.IsInstrument_NoNull())
                            db.AddInParameter(command, "@Instrument_No", DbType.String, objReceiptRow.Instrument_No);
                        if (!objReceiptRow.IsInstrument_DateNull())
                            db.AddInParameter(command, "@Instrument_Date", DbType.DateTime, objReceiptRow.Instrument_Date);
                        if (!objReceiptRow.IsBank_LocationNull())
                            db.AddInParameter(command, "@Bank_Location", DbType.String, objReceiptRow.Bank_Location);
                        if (!objReceiptRow.IsPayment_Gateway_NoNull())
                            db.AddInParameter(command, "@Payment_Gateway_No", DbType.String, objReceiptRow.Payment_Gateway_No);
                        if (!objReceiptRow.IsACK_NoNull())
                            db.AddInParameter(command, "@ACK_No", DbType.String, objReceiptRow.ACK_No);
                        if (!objReceiptRow.IsDrawee_Bank_IDNull())
                            db.AddInParameter(command, "@Drawee_Bank_ID", DbType.Int32, objReceiptRow.Drawee_Bank_ID);
                        if (!objReceiptRow.IsDrawee_Bank_NameNull())
                            db.AddInParameter(command, "@Drawee_Bank_Name", DbType.String, objReceiptRow.Drawee_Bank_Name);
                        if (!objReceiptRow.IsOther_Bank_NameNull())
                            db.AddInParameter(command, "@Other_Bank_Name", DbType.String, objReceiptRow.Other_Bank_Name);
                        db.AddInParameter(command, "@XML_ReceiptDtl", DbType.String, objReceiptRow.XML_ReceiptDtl);

                        db.AddOutParameter(command, "@ERRORCODE", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@Doc_No", DbType.String, 200);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intErrorCode = (int)command.Parameters["@ERRORCODE"].Value;
                                strReceiptNumber = Convert.ToString(command.Parameters["@Doc_No"].Value);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
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
                    throw ex;
                }

                return intErrorCode;
            }
        }
    }
}
