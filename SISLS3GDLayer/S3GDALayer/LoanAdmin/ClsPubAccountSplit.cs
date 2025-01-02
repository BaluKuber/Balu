#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Account Activation DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 09-SEP-2010
/// Purpose	            : DAL Class for Account Split Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;
namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        
        public class ClsPubAccountSplit
        {
            #region Initialization
            int intErrorCode;
            Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountSplitDataTable ObjAccountSplitDataTable_DAL = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAccountSplit()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion
            #region Create Account Split
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjAccountSplitDataTable"></param>
            /// <returns></returns>
            public int FunPubAccountSplitInt(SerializationMode SerMode, byte[] bytesObjAccountSplitDataTable,out string strSplitNum)
            {
                strSplitNum = "";
                try
                {
                    ObjAccountSplitDataTable_DAL = (Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountSplitDataTable)ClsPubSerialize.DeSerialize(bytesObjAccountSplitDataTable, SerMode, typeof(Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountSplitDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountSplitRow ObjAccountSplit in ObjAccountSplitDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertAccountSplit");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAccountSplit.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAccountSplit.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAccountSplit.Branch_ID);
                        db.AddInParameter(command, "@SplitDate", DbType.DateTime, ObjAccountSplit.Split_Date);
                        db.AddInParameter(command, "@Qunatity", DbType.Int32, ObjAccountSplit.Quantity);
                        db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, ObjAccountSplit.Finance_Amount);
                        db.AddInParameter(command, "@PANumber", DbType.String, ObjAccountSplit.PANum);
                        db.AddInParameter(command, "@SANumber", DbType.String, ObjAccountSplit.SANum);
                        db.AddInParameter(command, "@XMLSplitDetails", DbType.String, ObjAccountSplit.XMLSplitDetails);
                        db.AddInParameter(command, "@No_of_Days", DbType.Int32, ObjAccountSplit.No_of_Days);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAccountSplit.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@SplitNumber", DbType.String, 100);
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.ExecuteNonQuery(command, trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
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
                                    trans.Commit();
                                    strSplitNum = (string)command.Parameters["@SplitNumber"].Value;
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

        }
    }
}
