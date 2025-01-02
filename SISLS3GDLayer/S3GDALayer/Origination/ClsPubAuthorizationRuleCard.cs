/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Authorization Rule Card DAL Class
/// Created By			: Ramesh M
/// Created Date		: 28-May-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    namespace RuleCardMgtServices
    {
        public class ClsPubAuthorizationRuleCard
        {
            int intRowsAffected;
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable ObjConstitutionMster_Datatable;
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable ObjTransactionType_Datatable;
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable ObjAuthorization_DataTable;
            
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAuthorizationRuleCard()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable FunQueryConstitutionMaster(SerializationMode Sermode, byte[] bytesConstitutionMaster)
            {
                S3GBusEntity.Origination.RuleCardMgtServices DsConstitutionMaster = new S3GBusEntity.Origination.RuleCardMgtServices();
                ObjConstitutionMster_Datatable = (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable)ClsPubSerialize.DeSerialize(bytesConstitutionMaster, Sermode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetStatusLookUp");
                    foreach (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterRow ObjRow in ObjConstitutionMster_Datatable.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjRow.ID);
                    }
                    db.FunPubLoadDataSet(command, DsConstitutionMaster, DsConstitutionMaster.S3G_ORG_ConstitutionMaster.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsConstitutionMaster.S3G_ORG_ConstitutionMaster;

            }
            public S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable FunQueryTransactionTypeMaster(SerializationMode Sermode, byte[] bytesTransactionTypeMaster)
            {
                S3GBusEntity.Origination.RuleCardMgtServices DsTransactionType = new S3GBusEntity.Origination.RuleCardMgtServices();
                ObjTransactionType_Datatable = (S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable)ClsPubSerialize.DeSerialize(bytesTransactionTypeMaster, Sermode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetTransactionType");
                    foreach (S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPRow ObjRow in ObjTransactionType_Datatable.Rows)
                    {
                        db.AddInParameter(command, "@TransactionType", DbType.String, ObjRow.Type);
                        
                    }
                    db.FunPubLoadDataSet(command, DsTransactionType, DsTransactionType.S3G_Status_LookUP.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsTransactionType.S3G_Status_LookUP;
            }
            public DataSet FunQueryAuthorizationRuleCard(SerializationMode Sermode, byte[] bytesAuthorizationRuleCard)
            {
                DataSet DsAuthorizationRulecard = new DataSet();
                ObjAuthorization_DataTable = (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesAuthorizationRuleCard, Sermode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetAuthorizationRuleCard");
                    foreach (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardRow ObjRow in ObjAuthorization_DataTable.Rows)
                    {
                        if (!ObjRow.IsAuthorization_Rule_Card_IDNull())
                            db.AddInParameter(command, "@Authorization_Rule_Card_ID", DbType.Int32, ObjRow.Authorization_Rule_Card_ID);
                        if (!ObjRow.IsCompany_IDNull())
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRow.Company_ID);
                        if (!ObjRow.IsLOB_IDNull())
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjRow.LOB_ID);
                        if (!ObjRow.IsConstitution_IDNull())
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjRow.Constitution_ID);
                        if (!ObjRow.IsProduct_IDNull())
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjRow.Product_ID);
                        if (!ObjRow.IsTransaction_TypeNull())
                            db.AddInParameter(command, "@Transaction_Type", DbType.Int32, ObjRow.Transaction_Type);
                        if (!ObjRow.IsProgram_IdNull())
                            db.AddInParameter(command, "@Program_Id", DbType.Int32, ObjRow.Program_Id);
                        DsAuthorizationRulecard = db.FunPubExecuteDataSet(command);

                    }
                    return DsAuthorizationRulecard;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    DsAuthorizationRulecard.Dispose();
                    DsAuthorizationRulecard = null;
                }
                
            }
            public int FunPubCreateAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesAuthorizationRuleCard_Datatable)
            {
                try
                {
                    ObjAuthorization_DataTable = (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesAuthorizationRuleCard_Datatable, SerMode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertAuthorizationRuleCard");
                    foreach (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardRow ObjRow in ObjAuthorization_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjRow.LOB_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjRow.Constitution_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjRow.Product_ID);
                        db.AddInParameter(command, "@Transaction_Type", DbType.Int32, ObjRow.Transaction_Type);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRow.Company_ID);
                        db.AddInParameter(command, "@Program_Id", DbType.Int32, ObjRow.Program_Id);
                        db.AddInParameter(command, "@Entity_Type_ID", DbType.Int32, ObjRow.Entity_Type);
                        //Added By Kuppusamy.B on 30/Mar/2012 to change the XML to CLOB
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_RuleCard_Detail",
                                   OracleType.Clob, ObjRow.XMLRulecardDetail.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjRow.XMLRulecardDetail);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_RuleCard_Detail", DbType.String, ObjRow.XMLRulecardDetail);
                        }
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_RuleCard_Approvals",
                                   OracleType.Clob, ObjRow.XMLRuleCardApprovals.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjRow.XMLRuleCardApprovals);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_RuleCard_Approvals", DbType.String, ObjRow.XMLRuleCardApprovals);
                        }
                        //db.AddInParameter(command, "@XML_RuleCard_Detail", DbType.String, ObjRow.XMLRulecardDetail);

                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjRow.Created_By);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRow.Is_Active);
                        db.AddOutParameter(command,"@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if(intRowsAffected==0)
                                   trans.Commit();
                                else
                                    trans.Rollback();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 20;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
            public int FunPubModifyAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesAuthorizationRulecard_DataTable)
            {
                try
                {
                    ObjAuthorization_DataTable = (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesAuthorizationRulecard_DataTable, SerMode, typeof(S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateAuthorizationRuleCard");
                    foreach (S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardRow ObjRow in ObjAuthorization_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Authorization_Rule_Card_ID", DbType.Int32, ObjRow.Authorization_Rule_Card_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjRow.LOB_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjRow.Constitution_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjRow.Product_ID);
                        db.AddInParameter(command, "@Transaction_Type", DbType.Int32, ObjRow.Transaction_Type);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRow.Company_ID);
                        db.AddInParameter(command, "@Program_Id", DbType.Int32, ObjRow.Program_Id);
                        db.AddInParameter(command, "@Entity_Type_ID", DbType.Int32, ObjRow.Entity_Type);
                        //Added By Kuppusamy.B on 30/Mar/2012 to change the XML to CLOB
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_RuleCard_Detail",
                                   OracleType.Clob, ObjRow.XMLRulecardDetail.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjRow.XMLRulecardDetail);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_RuleCard_Detail", DbType.String, ObjRow.XMLRulecardDetail);
                        }
                        //db.AddInParameter(command, "@XML_RuleCard_Detail", DbType.String, ObjRow.XMLRulecardDetail);
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_RuleCard_Approvals",
                                   OracleType.Clob, ObjRow.XMLRuleCardApprovals.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjRow.XMLRuleCardApprovals);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_RuleCard_Approvals", DbType.String, ObjRow.XMLRuleCardApprovals);
                        }
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjRow.Modified_By);
                        db.AddOutParameter(command,"@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if(intRowsAffected==0)
                                    trans.Commit();
                                else
                                    trans.Rollback();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 20;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

        }
    }
}