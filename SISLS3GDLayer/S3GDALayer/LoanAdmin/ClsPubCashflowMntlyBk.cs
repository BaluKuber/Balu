#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Cashflow Monthly Booking DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 14-SEP-2010
/// Purpose	            : DAL Class for Cashflow Monthly Booking Methods
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubCashflowMntlyBk
        {
            #region Initialization
            int intErrorCode;
            Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingDataTable ObjCashflowMntlyBkDataTable_DAL = null;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCashflowMntlyBk()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Cashflow Montly Booking
              /// <summary>
              /// 
              /// </summary>
              /// <param name="SerMode"></param>
              /// <param name="bytesObjCashflowMntlyBkDataTable"></param>
              /// <returns></returns>
            public int FunPubCreateCFMBkInt(SerializationMode SerMode, byte[] bytesObjCashflowMntlyBkDataTable,out string strCFMNumber)
            {
                strCFMNumber = "";
                try
                {
                    
                    ObjCashflowMntlyBkDataTable_DAL = (Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingDataTable)ClsPubSerialize.DeSerialize(bytesObjCashflowMntlyBkDataTable, SerMode, typeof(Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (Entity_LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CashFlowMonthlyBookingRow ObjCFMBkRow in ObjCashflowMntlyBkDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertCFMBkDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCFMBkRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCFMBkRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjCFMBkRow.Branch_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjCFMBkRow.Branch_ID);
                        db.AddInParameter(command, "@CFMDate", DbType.DateTime, ObjCFMBkRow.CashFlowMonthly_Date);
                        db.AddInParameter(command, "@XMLCFMDetails", DbType.String, ObjCFMBkRow.XMlCFMDetails);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCFMBkRow.Created_By);
                        db.AddInParameter(command, "@CashFlow_Year", DbType.String, ObjCFMBkRow.CashFlow_Year);
                        db.AddInParameter(command, "@CashFlow_Month", DbType.String, ObjCFMBkRow.CashFlow_Month);
                        db.AddInParameter(command, "@CashFlow_Type", DbType.String, ObjCFMBkRow.CashFlow_Type);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@CFMNumber", DbType.String, 500);
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
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    if ((string)command.Parameters["@CFMNumber"].Value != null)
                                    {
                                        strCFMNumber = (string)command.Parameters["@CFMNumber"].Value;
                                    }
                                    throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intErrorCode == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intErrorCode == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    strCFMNumber = (string)command.Parameters["@CFMNumber"].Value;
                                    trans.Commit();
                                }
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
                }
                return intErrorCode;
            }
            #endregion

            public int FunPubRevokeCFMBkInt(string strCFMNumber, int intCompany_ID)
            {
                try
                {

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_RevokeCashMntlyBk");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
                    db.AddInParameter(command, "@CashFlowMonthly_No", DbType.String, strCFMNumber);
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
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                            }
                            if (intErrorCode == 0)
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

                catch (Exception ex)
                {
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
        }
    }
}
