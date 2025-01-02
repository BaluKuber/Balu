
#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Income Recognition Details
/// Created By			: Kaliraj K
/// Created Date		: 19-OCT-2010
/// Purpose	            : To insert IncomeRecognition Details

/// <Program Summary>
/// 

#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubIncomeRecognition
        {
            int intRowsAffected;
            string strChallanNo = "";
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_IncomeRecognitionDataTable objIncomeRecog_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubIncomeRecognition()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            /// <summary>LoanAdminAccMgtServices
            /// Inserting the Appropriation logic Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateIncomeRecognition(SerializationMode SerMode, byte[] bytesObjS3G_CLN_IncomeRecog_DataTable)
            {
                try
                {
                    objIncomeRecog_DAL = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_IncomeRecognitionDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_CLN_IncomeRecog_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_IncomeRecognitionDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_IncomeRecognitionRow objIncomeRecognitionRow in objIncomeRecog_DAL)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_ValidateIncomeRecognition");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objIncomeRecognitionRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objIncomeRecognitionRow.LOB_ID);
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, objIncomeRecognitionRow.Branch_ID);
                        db.AddInParameter(command, "@Frequency_Code", DbType.Int32, objIncomeRecognitionRow.Frequency_Type);
                        db.AddInParameter(command, "@CutOff_Date", DbType.DateTime, objIncomeRecognitionRow.CutOff_Date);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, objIncomeRecognitionRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
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
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }

            public DataSet FunPubGetIncomeRecognitionToSchedule(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg)
            {
                DataSet ds = new DataSet();
                intErrCode = 0;
                strErrMsg = string.Empty;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.LoadDataSet(command, ds, "ListTable", trans);
                            //ds = db.ExecuteDataSet(command, trans);
                            ds = db.FunPubExecuteDataSet(command, ref trans);
                            //db.ExecuteNonQuery(command, trans);
                            if ((int)command.Parameters["@ErrorCode"].Value != 0)
                            {
                                intErrCode = (int)command.Parameters["@ErrorCode"].Value;
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                trans.Rollback();
                            }
                            else
                            {
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds;

            }


            public DataSet FunPubGetIncomeRecognition(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg)
            {
                DataSet ds = new DataSet();
                intErrCode = 0;
                strErrMsg = string.Empty;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand(strProcName);
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        //DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.LoadDataSet(command, ds, "ListTable", trans);
                            //ds = db.ExecuteDataSet(command, trans);
                            //ds = db.FunPubExecuteDataSet(command, ref trans);
                            ds = db.FunPubExecuteDataSet(command);
                            //db.ExecuteNonQuery(command, trans);
                            if ((int)command.Parameters["@ErrorCode"].Value != 0)
                            {
                                intErrCode = (int)command.Parameters["@ErrorCode"].Value;
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                //trans.Rollback();
                            }
                            else
                            {
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                //trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            intErrCode = 50;
                            //trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ds;

            }

        }
    }
}
