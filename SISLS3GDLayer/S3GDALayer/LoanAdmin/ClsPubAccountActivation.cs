#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Account Activation DAL Class
/// Created By			: Suresh P
/// Created Date		: 19-Aug-2010
/// Purpose	            : DAL Class for Account Activation Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;
using System.Data.OracleClient;
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        public class ClsPubAccountActivation
        {
            #region Initialization

            int intErrorCode;
            //int intRowsAffected;
            Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable ObjAccountActivationDataTable_DAL = null;
            Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationRow ObjAccountActivationRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAccountActivation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Account Activation
            /// <summary>
            /// 
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjAccountActivationDataTable"></param>
            /// <returns></returns>
            public int FunPubAccountActivationInt(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable, out string strErrMsg)
            {
                //int intRowsAffected;
                strErrMsg = string.Empty;
                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertAccountActivation);

                    ObjAccountActivationDataTable_DAL = (Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable)ClsPubSerialize.DeSerialize(bytesObjAccountActivationDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable));
                    ObjAccountActivationRow = ObjAccountActivationDataTable_DAL.Rows[0] as Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAccountActivationRow.Company_ID);
                    if (!ObjAccountActivationRow.IsLOB_IDNull())
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAccountActivationRow.LOB_ID);
                    }
                    if (!ObjAccountActivationRow.IsBranch_IDNull())
                    {
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAccountActivationRow.Branch_ID);
                    }
                    db.AddInParameter(command, "@PANum", DbType.String, ObjAccountActivationRow.PANum);
                    db.AddInParameter(command, "@SANum", DbType.String, ObjAccountActivationRow.SANum);
                    if (!ObjAccountActivationRow.IsAccount_Activation_NumberNull())
                    {
                        db.AddInParameter(command, "@Activation_No", DbType.String, ObjAccountActivationRow.Account_Activation_Number);
                    }
                    if (!ObjAccountActivationRow.IsAccount_Activated_DateNull())
                    {
                        db.AddInParameter(command, "@ActivationDate", DbType.DateTime, ObjAccountActivationRow.Account_Activated_Date);
                    }
                    db.AddInParameter(command, "@MLAStatus", DbType.Int32, ObjAccountActivationRow.MLAStatus);
                    if (!ObjAccountActivationRow.IsCreated_ByNull())
                    {
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAccountActivationRow.Created_By);
                    }
                    //db.AddInParameter(command, "@XMLInvoice", DbType.String, ObjAccountActivationRow.XMLInvoice);
                    //db.AddInParameter(command, "@XMLProforma", DbType.String, ObjAccountActivationRow.XMLProforma);
                    db.AddInParameter(command, "@IsModify", DbType.String, ObjAccountActivationRow.IsModify);
                    db.AddInParameter(command, "@IsRevoke", DbType.String, ObjAccountActivationRow.IsRevoke);
                    if (!ObjAccountActivationRow.IsInterim_RateNull())
                    {
                        db.AddInParameter(command, "@Interim_Rate", DbType.String, ObjAccountActivationRow.Interim_Rate);
                    }
                    if (!ObjAccountActivationRow.IsInterim_amountNull())
                    {
                        db.AddInParameter(command, "@interim_amount", DbType.String, ObjAccountActivationRow.Interim_amount);
                    }
                    if (!ObjAccountActivationRow.Isinterim_typeNull())
                    {
                        db.AddInParameter(command, "@interim_type", DbType.String, ObjAccountActivationRow.interim_type);
                    }
                    //Chnages For TL and TLE start
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param;
                        if (!ObjAccountActivationRow.IsXMLRepayStructNull())
                        {
                            param = new OracleParameter("@XMLRepayStruct", OracleType.Clob,
                                ObjAccountActivationRow.XMLRepayStruct.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjAccountActivationRow.XMLRepayStruct);
                            command.Parameters.Add(param);
                        }

                        if (!ObjAccountActivationRow.IsXMLRepayDtlsNull())
                        {
                            param = new OracleParameter("@XMLRepayDtls", OracleType.Clob,
                                ObjAccountActivationRow.XMLRepayDtls.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjAccountActivationRow.XMLRepayDtls);
                            command.Parameters.Add(param);
                        }

                        if (!ObjAccountActivationRow.IsXMLOutFlowNull())
                        {
                            param = new OracleParameter("@XMLOutFlow", OracleType.Clob,
                                ObjAccountActivationRow.XMLOutFlow.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjAccountActivationRow.XMLOutFlow);
                            command.Parameters.Add(param);
                        }

                        if (!ObjAccountActivationRow.IsXMLInFlowNull())
                        {
                            param = new OracleParameter("@XMLInFlow", OracleType.Clob,
                                ObjAccountActivationRow.XMLInFlow.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjAccountActivationRow.XMLInFlow);
                            command.Parameters.Add(param);
                        }

                        if (!ObjAccountActivationRow.IsXMLRepayDetailsOthersNull())
                        {
                            param = new OracleParameter("@XMLRepayDetailsOthers", OracleType.Clob,
                                ObjAccountActivationRow.XMLRepayDetailsOthers.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjAccountActivationRow.XMLRepayDetailsOthers);
                            command.Parameters.Add(param);
                        }
                    }
                    else
                    {

                        if (!ObjAccountActivationRow.IsXMLRepayStructNull())
                            db.AddInParameter(command, "@XMLRepayStruct", DbType.String, ObjAccountActivationRow.XMLRepayStruct);
                        if (!ObjAccountActivationRow.IsXMLRepayDtlsNull())
                            db.AddInParameter(command, "@XMLRepayDtls", DbType.String, ObjAccountActivationRow.XMLRepayDtls);
                        if (!ObjAccountActivationRow.IsXMLOutFlowNull())
                            db.AddInParameter(command, "@XMLOutFlow", DbType.String, ObjAccountActivationRow.XMLOutFlow);
                        if (!ObjAccountActivationRow.IsXMLInFlowNull())
                            db.AddInParameter(command, "@XMLInFlow", DbType.String, ObjAccountActivationRow.XMLInFlow);
                        if (!ObjAccountActivationRow.IsAccountIRRNull())
                            db.AddInParameter(command, "@AccountIRR", DbType.String, ObjAccountActivationRow.AccountIRR);
                        if (!ObjAccountActivationRow.IsBusinessIRRNull())
                            db.AddInParameter(command, "@BusinessIRR", DbType.String, ObjAccountActivationRow.BusinessIRR);
                        if (!ObjAccountActivationRow.IsCompanyIRRNull())
                            db.AddInParameter(command, "@CompanyIRR", DbType.String, ObjAccountActivationRow.CompanyIRR);
                        if (!ObjAccountActivationRow.IsXMLRepayDetailsOthersNull())
                            db.AddInParameter(command, "@XMLRepayDetailsOthers", DbType.String, ObjAccountActivationRow.XMLRepayDetailsOthers);

                    }
                    //Chnages For TL and TLE end

                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command,trans);
                            db.FunPubExecuteNonQuery(command, ref trans);

                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                //strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
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
                                strErrMsg = (string)command.Parameters["@DSNO"].Value;
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (command.Parameters["@ErrorCode"].Value != null)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                            }
                            //else 
                            if (intErrorCode == 0)
                            {
                                intErrorCode = 50;
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
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public int FunPubBulkActivationInt(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable, out string strErrMsg)
            {
                //int intRowsAffected;
                strErrMsg = string.Empty;
                try
                {

                    DbCommand command = db.GetStoredProcCommand("S3G_LoadAd_InsertBulkActivation");

                    ObjAccountActivationDataTable_DAL = (Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable)ClsPubSerialize.DeSerialize(bytesObjAccountActivationDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationDataTable));
                    ObjAccountActivationRow = ObjAccountActivationDataTable_DAL.Rows[0] as Entity_LoanAdmin.ContractMgtServices.S3G_LOANAD_AccountActivationRow;
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAccountActivationRow.Company_ID);
                    if (!ObjAccountActivationRow.IsAccount_Activation_NumberNull())
                        db.AddInParameter(command, "@Activation_No", DbType.String, ObjAccountActivationRow.Account_Activation_Number);
                    if (!ObjAccountActivationRow.IsAccount_Activated_DateNull())
                        db.AddInParameter(command, "@ActivationDate", DbType.DateTime, ObjAccountActivationRow.Account_Activated_Date);
                    if (!ObjAccountActivationRow.IsCreated_ByNull())
                        db.AddInParameter(command, "@UserID", DbType.Int32, ObjAccountActivationRow.Created_By);
                    db.AddInParameter(command, "@IsRevoke", DbType.String, ObjAccountActivationRow.IsRevoke);
                    if (!ObjAccountActivationRow.IsInterim_RateNull())
                        db.AddInParameter(command, "@Interim_Rate", DbType.String, ObjAccountActivationRow.Interim_Rate);
                    if (!ObjAccountActivationRow.IsInterim_amountNull())
                        db.AddInParameter(command, "@interim_amount", DbType.String, ObjAccountActivationRow.Interim_amount);
                    if (!ObjAccountActivationRow.Isinterim_typeNull())
                        db.AddInParameter(command, "@interim_type", DbType.String, ObjAccountActivationRow.interim_type);
                    if (!ObjAccountActivationRow.IsXMLRepayStructNull())
                        db.AddInParameter(command, "@XMLRepayStruct", DbType.String, ObjAccountActivationRow.XMLRepayStruct);
                    if (!ObjAccountActivationRow.IsXMLRepayDtlsNull())
                        db.AddInParameter(command, "@XMLRepayDtls", DbType.String, ObjAccountActivationRow.XMLRepayDtls);
                    if (!ObjAccountActivationRow.IsXMLOutFlowNull())
                        db.AddInParameter(command, "@XMLOutFlow", DbType.String, ObjAccountActivationRow.XMLOutFlow);
                    if (!ObjAccountActivationRow.IsXMLInFlowNull())
                        db.AddInParameter(command, "@XMLInFlow", DbType.String, ObjAccountActivationRow.XMLInFlow);
                    if (!ObjAccountActivationRow.IsAccountIRRNull())
                        db.AddInParameter(command, "@AccountIRR", DbType.String, ObjAccountActivationRow.AccountIRR);
                    if (!ObjAccountActivationRow.IsBusinessIRRNull())
                        db.AddInParameter(command, "@BusinessIRR", DbType.String, ObjAccountActivationRow.BusinessIRR);
                    if (!ObjAccountActivationRow.IsCompanyIRRNull())
                        db.AddInParameter(command, "@CompanyIRR", DbType.String, ObjAccountActivationRow.CompanyIRR);
                    if (!ObjAccountActivationRow.IsXMLRepayDetailsOthersNull())
                        db.AddInParameter(command, "@XMLRepayDetailsOthers", DbType.String, ObjAccountActivationRow.XMLRepayDetailsOthers);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            command.CommandTimeout = 600;
                            db.FunPubExecuteNonQuery(command, ref trans);

                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                //strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
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
                            }
                        }
                        catch (Exception ex)
                        {
                            if (command.Parameters["@ErrorCode"].Value != null)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                            }
                            //else 
                            if (intErrorCode == 0)
                            {
                                intErrorCode = 50;
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
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            #endregion

            /// <summary>
            /// To get the PA_SA_Ref_ID
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN and SAN number</param>
            /// <returns></returns>

            public DataTable FunActiavtedAccountForModification(string strAccountNo)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAccountActivationforModify");
                    db.AddInParameter(command, "@ActivationNumber", DbType.String, strAccountNo);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
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
