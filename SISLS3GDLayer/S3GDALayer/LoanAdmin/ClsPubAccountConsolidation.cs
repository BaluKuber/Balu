#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Account Activation DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 04-SEP-2010
/// Purpose	            : DAL Class for Account Consolidation Methods
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
        public class ClsPubAccountConsolidation
        {
            #region Initialization
            int intErrorCode;
            Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountConsolidationDataTable ObjAccountConsolidationDataTable_DAL = null;
            Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountConsolidationRow ObjAccountConsolidationRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAccountConsolidation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Account Consolidation
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjAccountActivationDataTable"></param>
            /// <returns></returns>
            public int FunPubAccountConsolidationInt(SerializationMode SerMode, byte[] bytesObjAccountConsolidationDataTable,out string strAcConNo)
            {
                strAcConNo = "";
                try
                {
                    ObjAccountConsolidationDataTable_DAL = ( Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountConsolidationDataTable)ClsPubSerialize.DeSerialize(bytesObjAccountConsolidationDataTable, SerMode, typeof( Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountConsolidationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountConsolidationRow ObjAccountConsolidation in ObjAccountConsolidationDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertAccountConsolidation);


                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAccountConsolidation.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAccountConsolidation.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAccountConsolidation.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjAccountConsolidation.Consolidated_Customer_ID);
                        db.AddInParameter(command, "@ConsoldationDate", DbType.DateTime, ObjAccountConsolidation.Account_Consolidation_Date);
                        db.AddInParameter(command, "@ConAccDate", DbType.DateTime, ObjAccountConsolidation.Account_Consolidation_Date);
                        db.AddInParameter(command, "@New_Amount_Finanaced", DbType.Decimal, ObjAccountConsolidation.New_Amount_Finanaced);
                        db.AddOutParameter(command, "@Consolidation_No", DbType.String, 100);
                        //db.AddOutParameter(command, "@ConSLANumber", DbType.String, 100);
                        db.AddInParameter(command, "@XMLConDetails", DbType.String, ObjAccountConsolidation.XMLConsolDetails);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAccountConsolidation.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
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
                                    strAcConNo = (string)command.Parameters["@Consolidation_No"].Value;
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
