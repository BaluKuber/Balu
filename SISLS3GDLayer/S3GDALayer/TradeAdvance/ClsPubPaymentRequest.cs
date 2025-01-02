#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: PaymentRequest DAL Class
/// Created By			: S.Kannan
/// Created Date		: 18-Jul-2010
/// Purpose	            : DAL Class for Payment Request methods
/// <Program Summary>
#endregion

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


namespace S3GDALayer.TradeAdvance
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubPaymentRequest
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable ObjPaymentRequest_DataTable = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable();
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable ObjPaymentDetails_DataTable;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDetailsDataTable ObjPaymentRequestDetails_DataTable;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsDataTable ObjPaymentRequestInstrumentDetails_DataTable;

            Database db;
            public ClsPubPaymentRequest()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Mode
            // this is to insert the Payment request - SQL table
            public int FunPubCreateOrModifyPaymentRequest(SerializationMode SerMode, byte[] bytesObjPaymentRequest_DataTable, out string paynum, out int request_No)
            {
                request_No = 0;
                paynum = "";
                try
                {
                   
                    ObjPaymentRequest_DataTable = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable)ClsPubSerialize.DeSerialize(bytesObjPaymentRequest_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestRow ObjPaymentRequestRow in ObjPaymentRequest_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertPaymentRequest");

                        if (ObjPaymentRequestRow.Company_ID != -1)
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaymentRequestRow.Company_ID);
                        if (ObjPaymentRequestRow.LOB_ID != -1)
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPaymentRequestRow.LOB_ID);
                        if (ObjPaymentRequestRow.Branch_ID != -1)
                        {
                            //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjPaymentRequestRow.Branch_ID);
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPaymentRequestRow.Branch_ID);
                        }
                        //db.AddInParameter(command, "@Payment_Request_No", DbType.String, ObjPaymentRequestRow.Payment_Request_No);
                       // db.AddInParameter(command, "@Request_No", DbType.Int32, ObjPaymentRequestRow.Request_No);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Account_Based)))
                            db.AddInParameter(command, "@Account_Based", DbType.Int32, ObjPaymentRequestRow.Account_Based);
                        if (ObjPaymentRequestRow.Value_Date != Convert.ToDateTime("1/1/1700"))
                        db.AddInParameter(command, "@Payment_Request_Date", DbType.DateTime, ObjPaymentRequestRow.Payment_Request_Date);
                        if (ObjPaymentRequestRow.Value_Date != Convert.ToDateTime("1/1/1700"))
                            db.AddInParameter(command, "@Value_Date", DbType.DateTime, ObjPaymentRequestRow.Value_Date);
                        if (ObjPaymentRequestRow.Pay_Type_Code != -1)
                            db.AddInParameter(command, "@Pay_Type_Code", DbType.Int32, ObjPaymentRequestRow.Pay_Type_Code);
                        if (ObjPaymentRequestRow.Pay_Mode_Code != -1)
                            db.AddInParameter(command, "@Pay_Mode_Code", DbType.Int32, ObjPaymentRequestRow.Pay_Mode_Code);
                        if (ObjPaymentRequestRow.Currency_ID != -1)
                            db.AddInParameter(command, "@Currency_Code", DbType.Int32, ObjPaymentRequestRow.Currency_ID);
                        if (ObjPaymentRequestRow.Exchange_Rate_ID != -1)
                            db.AddInParameter(command, "@Exchange_Rate_ID", DbType.Int32, ObjPaymentRequestRow.Exchange_Rate_ID);
                        if (Convert.ToDecimal(ObjPaymentRequestRow.Pay_Amount) != -1)
                            db.AddInParameter(command, "@Pay_Amount", DbType.Decimal, ObjPaymentRequestRow.Pay_Amount);
                        if (ObjPaymentRequestRow.Pay_To_Type_Code != -1)
                            db.AddInParameter(command, "@Pay_To_Type_Code", DbType.Int32, ObjPaymentRequestRow.Pay_To_Type_Code);
                        if (ObjPaymentRequestRow.Pay_To_Code != -1)
                            db.AddInParameter(command, "@Pay_To_Code", DbType.Int32, ObjPaymentRequestRow.Pay_Type_Code);
                        if (ObjPaymentRequestRow.Vendor_Code != -1) 
                            db.AddInParameter(command, "@Vendor_Code", DbType.Int32, ObjPaymentRequestRow.Vendor_Code);
                        if (ObjPaymentRequestRow.Customer_ID != -1)
                            db.AddInParameter(command, "@Customer_Code", DbType.Int32, ObjPaymentRequestRow.Customer_ID);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Pay_To_Name)))
                            db.AddInParameter(command, "@Pay_To_Name", DbType.String, ObjPaymentRequestRow.Pay_To_Name);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Pay_To_Address)))
                            db.AddInParameter(command, "@Pay_To_Address", DbType.String, ObjPaymentRequestRow.Pay_To_Address);
                        db.AddInParameter(command, "@Pmt_Voucher_status", DbType.Boolean, ObjPaymentRequestRow.Pmt_Voucher_status);
                        if (ObjPaymentRequestRow.Cancelled_Date != Convert.ToDateTime("1/1/1700"))
                            db.AddInParameter(command, "@Cancelled_Date", DbType.DateTime, ObjPaymentRequestRow.Cancelled_Date);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.IsVoucher_Print)))
                            db.AddInParameter(command, "@IsVoucher_Print", DbType.String, ObjPaymentRequestRow.IsVoucher_Print);  // character
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.IsVoucher_Print)))
                            db.AddInParameter(command, "@IsCheque_Print", DbType.String, ObjPaymentRequestRow.IsCheque_Print);    // character
                        if (ObjPaymentRequestRow.Created_By != -1)
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPaymentRequestRow.Created_By);
                        if (ObjPaymentRequestRow.Created_On != Convert.ToDateTime("1/1/1700"))
                            db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjPaymentRequestRow.Created_On);
                        if (ObjPaymentRequestRow.Modified_By != -1)
                            db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjPaymentRequestRow.Modified_By);
                        if (ObjPaymentRequestRow.Modified_Date != Convert.ToDateTime("1/1/1700"))
                            db.AddInParameter(command, "@Modified_Date", DbType.DateTime, ObjPaymentRequestRow.Modified_Date);
                        if (ObjPaymentRequestRow.TXN_ID != -1)
                            db.AddInParameter(command, "@TXN_ID", DbType.Int32, ObjPaymentRequestRow.TXN_ID);
                        
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.XML_PaymentDetails)))
                            db.AddInParameter(command, "@XML_PaymentDetails", DbType.String, ObjPaymentRequestRow.XML_PaymentDetails);
                        
                        
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.XML_PaymentAdjustment)))
                            db.AddInParameter(command, "@XML_PaymentAdjustment", DbType.String, ObjPaymentRequestRow.XML_PaymentAdjustment);
                        
                        
                        
                        if (ObjPaymentRequestRow.Bank_ID != -1 )
                            db.AddInParameter(command, "@Bank_ID", DbType.Int32, ObjPaymentRequestRow.Bank_ID);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Instrument_Type)))
                            db.AddInParameter(command, "@Instrument_Type", DbType.String, ObjPaymentRequestRow.Instrument_Type);
                        if (ObjPaymentRequestRow.Is_Update_Req != -1)
                            db.AddInParameter(command, "@Is_Update_Req", DbType.Int32, ObjPaymentRequestRow.Is_Update_Req);
                        db.AddInParameter(command, "@Instrument_Status", DbType.Boolean, ObjPaymentRequestRow.Instrument_Status);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Instrument_No)))
                            db.AddInParameter(command, "@Instrument_No", DbType.String, ObjPaymentRequestRow.Instrument_No);
                        if (!(string.IsNullOrEmpty(ObjPaymentRequestRow.Instrument_Date)))
                            db.AddInParameter(command, "@Instrument_Date", DbType.String, ObjPaymentRequestRow.Instrument_Date);
                        db.AddInParameter(command, "@Is_Instrument", DbType.Int32, ObjPaymentRequestRow.Is_Instrument);
                        if (ObjPaymentRequestRow.Mode != -1)
                            db.AddInParameter(command, "@Mode", DbType.Int16, ObjPaymentRequestRow.Mode);
                        if(ObjPaymentRequestRow.Requestno !=-1)
                            db.AddInParameter(command, "@Requestno", DbType.Int16, ObjPaymentRequestRow.Requestno);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@PayNum", DbType.String, 50);
                        db.AddOutParameter(command, "@Request_No", DbType.Int32, sizeof(Int32));
                        
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                               // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                paynum = (string)command.Parameters["@PayNum"].Value;
                                request_No = (int)command.Parameters["@Request_No"].Value;
                                //if (intRowsAffected == 0)
                                    trans.Commit();
                                //else
                                //    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
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
                }
                return intRowsAffected;
            }



            // this is to insert the Payment request - SQL table
            public int FunPubCreateOrModifyPaymentRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails_DataTable)
            {
                try
                {
                    ObjPaymentRequestDetails_DataTable = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjPaymentRequestDetails_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDetailsDataTable));
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_PaymentRequestDetailsRow ObjPaymentRequestDetailsRow in ObjPaymentRequestDetails_DataTable.Rows)
                    {
                        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPaymentRequestDetails");
                  
                        db.AddInParameter(command, "@Payment_Request_ID", DbType.Int32, ObjPaymentRequestDetailsRow.Payment_Request_ID); // send 0 in insert mode

                        db.AddInParameter(command, "@Company_ID ", DbType.Int32, ObjPaymentRequestDetailsRow.Company_ID);
                        db.AddInParameter(command, "@Request_No", DbType.Int32, ObjPaymentRequestDetailsRow.Request_No);
                        db.AddInParameter(command, "@PANum", DbType.String, ObjPaymentRequestDetailsRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, ObjPaymentRequestDetailsRow.SANum);
                        db.AddInParameter(command, "@Asset_ID", DbType.Int32, ObjPaymentRequestDetailsRow.Asset_ID);
                       // db.AddInParameter(command, "@Pay_Type_Code", DbType.Int32, ObjPaymentRequestDetailsRow.Pay_Type_Code);
                        db.AddInParameter(command, "@Pay_Code", DbType.Int32, ObjPaymentRequestDetailsRow.Pay_Code);
                        db.AddInParameter(command, "@Pay_Adjust_Type", DbType.Int32, ObjPaymentRequestDetailsRow.Pay_Adjust_Type);
                        db.AddInParameter(command, "@GL_Code", DbType.String, ObjPaymentRequestDetailsRow.GL_Code);
                        db.AddInParameter(command, "@SL_Code", DbType.String, ObjPaymentRequestDetailsRow.SL_Code);
                        db.AddInParameter(command, "@Amount", DbType.Decimal, ObjPaymentRequestDetailsRow.Amount);
                        db.AddInParameter(command, "@Pay_To_Name", DbType.String, ObjPaymentRequestDetailsRow.Pay_To_Name);
                        db.AddInParameter(command, "@Pay_To_Address", DbType.String, ObjPaymentRequestDetailsRow.Pay_To_Address);
                        db.AddInParameter(command, "@Description", DbType.String, ObjPaymentRequestDetailsRow.Description);
                        

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                               // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                //if (intRowsAffected == 0)
                                    trans.Commit();
                                //else
                                //    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
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
                }
                return intRowsAffected;
            }

            #endregion

            #region Query Mode
            // this is to select the payment request details SQL - table.
            public S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable FunPubQueryGetCreditParameterRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails)
            { 
                S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices dsPaymentRequestDetails = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices();
                ObjPaymentDetails_DataTable = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjPaymentRequestDetails, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPaymentDetails");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsRow ObjPaymentDetailsRow in ObjPaymentDetails_DataTable.Rows)
                    {
                        if (ObjPaymentDetailsRow.Entity_ID != -1)
                            db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjPaymentDetailsRow.Entity_ID);
                        if (ObjPaymentDetailsRow.Payment_Request_ID != -1)
                            db.AddInParameter(command, "@Payment_Request_ID", DbType.Int32, ObjPaymentDetailsRow.Payment_Request_ID);
                        //if (ObjPaymentDetailsRow.Request_No != -1)
                            db.AddInParameter(command, "@Request_No", DbType.Int32, ObjPaymentDetailsRow.Request_No);
                        if (ObjPaymentDetailsRow.LOB_ID != -1)
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPaymentDetailsRow.LOB_ID);
                        if (ObjPaymentDetailsRow.Branch_ID != -1)
                        {
                            //db.AddInParameter(command, "@Branch_ID ", DbType.String, ObjPaymentDetailsRow.Branch_ID);
                            db.AddInParameter(command, "@Location_ID ", DbType.String, ObjPaymentDetailsRow.Branch_ID);
                        }

                      //  db.LoadDataSet(command, dsPaymentRequestDetails, dsPaymentRequestDetails.S3G_LOANAD_GetPaymentDetails.TableName);
                        db.FunPubLoadDataSet(command, dsPaymentRequestDetails, dsPaymentRequestDetails.S3G_LOANAD_GetPaymentDetails.TableName);

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsPaymentRequestDetails.S3G_LOANAD_GetPaymentDetails;




            }
            #endregion


            #region Insertion In Payment Instrument table
            // this is to select the payment request details SQL - table.
            public int FunPubInsPaymentRequestInstrument(SerializationMode SerMode, byte[] bytesObjPaymentRequestInstrumentDetails_DataTable)
            {
                try
                {

                    ObjPaymentRequestInstrumentDetails_DataTable = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjPaymentRequestInstrumentDetails_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_InstrumentdetailsRow ObjPaymentRequestRow in ObjPaymentRequestInstrumentDetails_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPmtReqInstrumentDetails");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaymentRequestRow.Company_ID);
                        db.AddInParameter(command, "@PaymentRequest_ID", DbType.Int32, ObjPaymentRequestRow.Payment_Request_ID);
                        db.AddInParameter(command, "@Instrument_Type", DbType.String, ObjPaymentRequestRow.Instrument_Type);
                        db.AddInParameter(command, "@Instrument_Status", DbType.Boolean, ObjPaymentRequestRow.Instrument_Status);
                        db.AddInParameter(command, "@Instrument_No", DbType.String, ObjPaymentRequestRow.Instrument_No);
                        db.AddInParameter(command, "@Instrument_Amount", DbType.Decimal, ObjPaymentRequestRow.Instrument_Amount);
                        db.AddInParameter(command, "@Instrument_Date", DbType.DateTime, ObjPaymentRequestRow.Instrument_Date);
                        if (ObjPaymentRequestRow.Bank_ID != null && ObjPaymentRequestRow.Bank_ID > 0)
                        db.AddInParameter(command, "@Bank_ID", DbType.Int32, ObjPaymentRequestRow.Bank_ID);
                        if (ObjPaymentRequestRow.Remarks != null)
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjPaymentRequestRow.Remarks);
                        if (ObjPaymentRequestRow.Is_Update_Req != null)
                        db.AddInParameter(command, "Is_Update_Req", DbType.Int32, ObjPaymentRequestRow.Is_Update_Req);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPaymentRequestRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                       
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
                                intRowsAffected = 50;
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
                }
                return intRowsAffected;
            }
            #endregion




        }
    }
}