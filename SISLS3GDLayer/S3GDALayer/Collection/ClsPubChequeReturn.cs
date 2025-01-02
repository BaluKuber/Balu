#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection 
/// Screen Name			: 
/// Created By			: Saishree Ramasubbu 
/// Created Date		: 
/// Purpose	            : 
/// 
/// Modified By         : 
/// Modified On         : 
/// Reason              : 
///
#endregion


using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using System.Data.OracleClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {

        public class ClsPubChequeReturn
        {
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessDataTable objCheque_DAL = null;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessRow objChequeRow = null;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeAuthorizationDataTable objAuthorize_DAL = null;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeAuthorizationRow objAuthorizeRow = null;
            //Source Modified by Tamilselvan.S on 19/01/2011
            S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingDataTable ObjMemorandumBooking_DAL = null;
            S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingRow ObjMemorandumBookingRow = null;
            //Code added for getting common connection string  from config file
            Database db;
            //Added by Palani Kumar.A on 07/12/2013
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessExcelDataTable objChequeExcel_DAL = null;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessExcelRow objChequeExcelRow = null;
            public ClsPubChequeReturn()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the Cheque Return table
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            //public int FunPubCreateChequeReturns(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChequeTable)
            //{
            //    try
            //    {
            //        objCheque_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_ChequeTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessDataTable));
            ////        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //        objChequeRow = objCheque_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessRow;

            //        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsetChequeProcessing");
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, objChequeRow.Company_ID);
            //        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objChequeRow.LOB_ID);
            //        db.AddInParameter(command, "@Branch_ID", DbType.Int32, objChequeRow.Branch_ID);
            //        db.AddInParameter(command, "@ChequeReturn_Advise_No", DbType.String, objChequeRow.Cheque_Return_Advice_No);
            //        db.AddInParameter(command, "@ChequeReturn_Advise_Date", DbType.DateTime, objChequeRow.Cheque_Return_Advice_Date);
            //        db.AddInParameter(command, "@BankAdvice_No", DbType.String, objChequeRow.Bank_Advice_Number);
            //        db.AddInParameter(command, "@Deposit_Bank", DbType.String, objChequeRow.Deposit_Bank_Code);
            //        //db.AddInParameter(command, "@Bank_No", DbType.Int32, objChequeRow.Bank_Account_Number);
            //        db.AddInParameter(command, "@Receipt_No", DbType.String, objChequeRow.Receipt_No);
            //        db.AddInParameter(command, "@Cheque_No", DbType.Int32, objChequeRow.Cheque_Number);
            //        db.AddInParameter(command, "@Cheque_Amount", DbType.Int32, objChequeRow.Cheque_Amount);
            //        db.AddInParameter(command, "@BankCharges", DbType.Int32, objChequeRow.Bank_Charges);
            //        db.AddInParameter(command, "@Reasons", DbType.Int32, objChequeRow.Cheque_Reason_Type);
            //        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objChequeRow.Created_By);

            //        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
            //        db.ExecuteNonQuery(command);

            //        if ((int)command.Parameters["@ErrorCode"].Value > 0)
            //            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;


            //    }
            //    catch (Exception ex)
            //    {
            //        intRowsAffected = 50;
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intRowsAffected;
            //}

            /// <summary>
            /// Created by Tamilselvan.S
            /// Created Date 19/01/2011
            /// Purpose To Cheque return process 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_CLN_ChequeTable"></param>
            /// <param name="bytesObjS3G_CLN_MemorandumBookingTable"></param>
            /// <returns></returns>
            public int FunPubCreateChequeReturns(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChequeTable, byte[] bytesObjS3G_CLN_MemorandumBookingTable, out string ChequeReturnNo)
            {
                try
                {
                    ChequeReturnNo = "";
                    objCheque_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_ChequeTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objChequeRow = objCheque_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertChequeProcessing");
                    //Common Param's
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objChequeRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, objChequeRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, objChequeRow.Branch_ID);
                    //db.AddInParameter(command, "@Branch_ID", DbType.Int32, objChequeRow.Branch_ID);
                    //For Cheque Return Processing params
                    db.AddInParameter(command, "@ChequeReturn_Advise_No", DbType.String, objChequeRow.Cheque_Return_Advice_No);
                    db.AddInParameter(command, "@ChequeReturn_Advise_Date", DbType.DateTime, objChequeRow.Cheque_Return_Advice_Date);
                    db.AddInParameter(command, "@BankAdvice_No", DbType.String, objChequeRow.Bank_Advice_Number);
                    db.AddInParameter(command, "@Deposit_Bank", DbType.String, objChequeRow.Deposit_Bank_Code);
                    db.AddInParameter(command, "@Receipt_No", DbType.String, objChequeRow.Receipt_No);
                    db.AddInParameter(command, "@Cheque_No", DbType.Int64, objChequeRow.Cheque_Number);
                    db.AddInParameter(command, "@Cheque_Amount", DbType.Decimal, objChequeRow.Cheque_Amount);
                    db.AddInParameter(command, "@BankCharges", DbType.Decimal, objChequeRow.Bank_Charges);
                    db.AddInParameter(command, "@Reasons", DbType.Int32, objChequeRow.Cheque_Reason_Type);
                    db.AddInParameter(command, "@CreatedBy", DbType.Int32, objChequeRow.Created_By);
                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);

                    //For MemorandumBooking Params
                    ObjMemorandumBooking_DAL = (S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_MemorandumBookingTable, SerMode, typeof(S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemoMasterDataTable));
                    ObjMemorandumBookingRow = ObjMemorandumBooking_DAL.Rows[0] as S3GBusEntity.Collection.ClnMemoMgtServices.S3G_CLN_MemorandumBookingRow;

                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjMemorandumBookingRow.Customer_ID);
                    db.AddInParameter(command, "@PANum", DbType.String, ObjMemorandumBookingRow.PANum);
                    db.AddInParameter(command, "@SANum", DbType.String, ObjMemorandumBookingRow.SANum);
                    db.AddInParameter(command, "@MemoTypeCode", DbType.Int32, ObjMemorandumBookingRow.Memo_Type_Code);
                    db.AddInParameter(command, "@MemoType", DbType.Int32, ObjMemorandumBookingRow.Memo_Type);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjMemorandumBookingRow.Modified_By);
                    db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjMemorandumBookingRow.Txn_ID);
                    db.AddInParameter(command, "@XMLAddDetail", DbType.String, ObjMemorandumBookingRow.MemorandumAddDetails);
                    db.AddInParameter(command, "@XMLDeleteDetail", DbType.String, ObjMemorandumBookingRow.MemorandumDeleteDetails);
                    db.AddInParameter(command, "@XMLUpdateeDetail", DbType.String, ObjMemorandumBookingRow.MemorandumUpdateDetails);
                    db.AddInParameter(command, "@OperationType", DbType.Int32, ObjMemorandumBookingRow.OperationType);
                    //Common Param's
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    using (DbConnection con = db.CreateConnection())
                    {
                        con.Open();
                        DbTransaction trans = con.BeginTransaction();
                        try
                        {

                            //db.ExecuteNonQuery(command, trans);
                            db.FunPubExecuteNonQuery(command, ref trans);

                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            if (intRowsAffected == 0)
                            {
                                ChequeReturnNo = command.Parameters["@DSNO"].Value.ToString();
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        con.Close();
                    }
                    //db.ExecuteNonQuery(command);
                    //if ((int)command.Parameters["@ErrorCode"].Value > 0 || (int)command.Parameters["@ErrorCode"].Value < 0)
                    //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    //else
                    //    ChequeReturnNo = command.Parameters["@DSNO"].Value.ToString();
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }


            public int FunPubChequeAuthorization(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChequeAuthorizationDataTable)
            {
                try
                {
                    objAuthorize_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeAuthorizationDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_ChequeAuthorizationDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeAuthorizationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objAuthorizeRow = objAuthorize_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeAuthorizationRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_ChequeAuthorization");
                    db.AddInParameter(command, "@XMLListValues", DbType.String, objAuthorizeRow.XMLListValues);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, objAuthorizeRow.User_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    //db.ExecuteNonQuery(command);
                    //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    using (DbConnection con = db.CreateConnection())
                    {
                        con.Open();
                        DbTransaction trans = con.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command, trans);
                            db.FunPubExecuteNonQuery(command, ref trans);

                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        con.Close();
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

            public int FunPubCreateChequeReturnsExcel(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChequeExcelTable, out string ChequeReturnNo)
            {
                try
                {
                    ChequeReturnNo = "";
                    objChequeExcel_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessExcelDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_ChequeExcelTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessExcelDataTable));
                    objChequeExcelRow = objChequeExcel_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChequeReturnProcessExcelRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertChequeReturnThroughExcel");
                   // db.AddInParameter(command, "@XMLChequeRtnExcel", DbType.String, objChequeExcelRow.XMLChequeRtnExcel);

                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                                       
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param;
                        param = new OracleParameter("@XMLChequeRtnExcel",
                            OracleType.Clob, objChequeExcelRow.XMLChequeRtnExcel.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, objChequeExcelRow.XMLChequeRtnExcel);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLChequeRtnExcel", DbType.String,
                            objChequeExcelRow.XMLChequeRtnExcel);
                    }
                    db.AddInParameter(command, "@CreatedBy", DbType.Int32, objChequeExcelRow.Created_By);
                    db.AddInParameter(command, "@File_Path", DbType.String, objChequeExcelRow.File_Path);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, objChequeExcelRow.Modified_By);
                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);       
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection con = db.CreateConnection())
                    {
                        con.Open();
                        DbTransaction trans = con.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);

                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            if (intRowsAffected == 0)
                            {
                                ChequeReturnNo = command.Parameters["@DSNO"].Value.ToString();
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            intRowsAffected = 50;
                        }
                        con.Close();
                    }
                    //ChequeReturnNo = command.Parameters["@DSNO"].Value.ToString();
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
