using System;using S3GDALayer.S3GAdminServices;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using S3GBusEntity;
namespace S3GDALayer.Origination
{
    namespace CreditMgtServices
    {
        public class ClsPubGlobalCreditParameter
        {
            int intRowsAffected;
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable ObjCreditScoreGuideParameter_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable ObjGlobalCreditParameter_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable ObjGlobalCreditParamApproval = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable();
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable ObjGlobalCreditParamScore = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable();


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubGlobalCreditParameter()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable FunPubQueryCreditScoreGuideParameter(SerializationMode Sermode, byte[] bytesCreditScoreGuideParameter)
            {
                S3GBusEntity.Origination.CreditMgtServices DsCreditScoreGuideParameter = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditScoreGuideParameter_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable)ClsPubSerialize.DeSerialize(bytesCreditScoreGuideParameter, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditScoreItem");
                    //db.LoadDataSet(command, DsCreditScoreGuideParameter, DsCreditScoreGuideParameter.S3G_ORG_CreditScoreGuideParameter.TableName);
                    db.FunPubLoadDataSet(command, DsCreditScoreGuideParameter, DsCreditScoreGuideParameter.S3G_ORG_CreditScoreGuideParameter.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsCreditScoreGuideParameter.S3G_ORG_CreditScoreGuideParameter;
            }
            public int FunPubCreateGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameter_DataTable)
            {
                try
                {
                    ObjGlobalCreditParameter_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParameter_DataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertGlobalCreditParameter");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParameterRow in ObjGlobalCreditParameter_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalCreditParameterRow.Company_ID);
                        if (!ObjGlobalCreditParameterRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjGlobalCreditParameterRow.LOB_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsProduct_IDNull())
                        {
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjGlobalCreditParameterRow.Product_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsConstitution_IDNull())
                        {
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjGlobalCreditParameterRow.Constitution_ID);
                        }
                        db.AddInParameter(command, "@Negative_Deviation", DbType.Decimal, ObjGlobalCreditParameterRow.Negative_Deviation);
                        db.AddInParameter(command, "@Exposure_Variance", DbType.Decimal, ObjGlobalCreditParameterRow.Exposure_Variance);
                        db.AddInParameter(command, "@XMLCreditScore", DbType.String, ObjGlobalCreditParameterRow.XMLCreditScore);
                        db.AddInParameter(command, "@XMLApprovallimit", DbType.String, ObjGlobalCreditParameterRow.XMLApprovallimit);
                        db.AddInParameter(command, "@XMLApprover", DbType.String, ObjGlobalCreditParameterRow.XMLApprover);
                        db.AddInParameter(command, "@XMLOthers", DbType.String, ObjGlobalCreditParameterRow.XMLOthers);
                        db.AddInParameter(command, "@Entity_Type_Id", DbType.Int32, ObjGlobalCreditParameterRow.Entity_Type_Id);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalCreditParameterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjGlobalCreditParameterRow.Created_By);
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
            public int FunPubModifyGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameter_DataTable)
            {
                try
                {
                    ObjGlobalCreditParameter_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjGlobalCreditParameter_DataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateGlobalCreditParameter");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParameterRow in ObjGlobalCreditParameter_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Globalcreditparameter_ID", DbType.Int32, ObjGlobalCreditParameterRow.Global_Credit_Parameter_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalCreditParameterRow.Company_ID);
                        if (!ObjGlobalCreditParameterRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjGlobalCreditParameterRow.LOB_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsProduct_IDNull())
                        {
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjGlobalCreditParameterRow.Product_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsConstitution_IDNull())
                        {
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjGlobalCreditParameterRow.Constitution_ID);
                        }
                        db.AddInParameter(command, "@Negative_Deviation", DbType.Decimal, ObjGlobalCreditParameterRow.Negative_Deviation);
                        db.AddInParameter(command, "@Exposure_Variance", DbType.Decimal, ObjGlobalCreditParameterRow.Exposure_Variance);
                        db.AddInParameter(command, "@XMLCreditScore", DbType.String, ObjGlobalCreditParameterRow.XMLCreditScore);
                        db.AddInParameter(command, "@XMLApprovallimit", DbType.String, ObjGlobalCreditParameterRow.XMLApprovallimit);
                        db.AddInParameter(command, "@XMLApprover", DbType.String, ObjGlobalCreditParameterRow.XMLApprover);
                        db.AddInParameter(command, "@XMLOthers", DbType.String, ObjGlobalCreditParameterRow.XMLOthers);
                        db.AddInParameter(command, "@Entity_Type_Id", DbType.Int32, ObjGlobalCreditParameterRow.Entity_Type_Id);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalCreditParameterRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjGlobalCreditParameterRow.Created_By);
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
            public DataSet FunPubQueryGlobalCreditParameter(SerializationMode SerMode, byte[] bytesGlobalCreditParameter)
            {
                DataSet DsGlobalCreditParameter = new DataSet();
                ObjGlobalCreditParameter_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable)ClsPubSerialize.DeSerialize(bytesGlobalCreditParameter, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetGlobalCreditParameter");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterRow ObjGlobalCreditParameterRow in ObjGlobalCreditParameter_DataTable.Rows)
                    {
                        if (!ObjGlobalCreditParameterRow.IsGlobal_Credit_Parameter_IDNull())
                        {
                            db.AddInParameter(command, "@Global_Credit_Parameter_ID", DbType.Int32, ObjGlobalCreditParameterRow.Global_Credit_Parameter_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsCompany_IDNull())
                        {
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjGlobalCreditParameterRow.Company_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjGlobalCreditParameterRow.LOB_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsProduct_IDNull())
                        {
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjGlobalCreditParameterRow.Product_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsConstitution_IDNull())
                        {
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjGlobalCreditParameterRow.Constitution_ID);
                        }
                        if (!ObjGlobalCreditParameterRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjGlobalCreditParameterRow.Is_Active);
                        }
                        // DsGlobalCreditParameter = db.ExecuteDataSet(command);
                        DsGlobalCreditParameter = db.FunPubExecuteDataSet(command);
                    }

                    return DsGlobalCreditParameter;

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    DsGlobalCreditParameter.Dispose();
                    DsGlobalCreditParameter = null;
                }
            }
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable FunPubQueryGlobalCreditScoreParameter(SerializationMode Sermode, byte[] bytesGlobalCreditScoreParameter)
            {
                S3GBusEntity.Origination.CreditMgtServices DsGlobalCreditScoreParameter = new S3GBusEntity.Origination.CreditMgtServices();
                ObjGlobalCreditParamScore = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable)ClsPubSerialize.DeSerialize(bytesGlobalCreditScoreParameter, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetStatusLookUp");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreRow ObjRow in ObjGlobalCreditParamScore.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjRow.Global_Credit_Parameter_CreditScore_ID);//// option parameter send thro this ID
                        db.AddInParameter(command, "@Param1", DbType.Int32, ObjRow.Global_Credit_Parameter_ID);
                        if (!ObjRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Param2", DbType.Boolean, ObjRow.Is_Active);

                        }
                    }
                    //    db.LoadDataSet(command, DsGlobalCreditScoreParameter, DsGlobalCreditScoreParameter.S3G_ORG_GlobalCreditParameterCreditScore.TableName);
                    db.FunPubLoadDataSet(command, DsGlobalCreditScoreParameter, DsGlobalCreditScoreParameter.S3G_ORG_GlobalCreditParameterCreditScore.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobalCreditScoreParameter.S3G_ORG_GlobalCreditParameterCreditScore;
            }
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable FunPubQueryGlobalCreditParameterApproval(SerializationMode Sermode, byte[] bytesGlobalCreditParameterApproval)
            {
                S3GBusEntity.Origination.CreditMgtServices DsGlobalCreditParameterApproval = new S3GBusEntity.Origination.CreditMgtServices();
                ObjGlobalCreditParamApproval = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable)ClsPubSerialize.DeSerialize(bytesGlobalCreditParameterApproval, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetStatusLookUp");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalRow ObjRow in ObjGlobalCreditParamScore.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjRow.Global_Credit_Parameter_Approval_ID);  // option parameter send thro this ID
                        db.AddInParameter(command, "@Param1", DbType.Int32, ObjRow.Global_Credit_Parameter_ID);
                        if (!ObjRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Param2", DbType.Boolean, ObjRow.Is_Active);

                        }
                    }
                    //    db.LoadDataSet(command, DsGlobalCreditParameterApproval, DsGlobalCreditParameterApproval.S3G_ORG_GlobalCreditParameterApproval.TableName);
                    db.FunPubLoadDataSet(command, DsGlobalCreditParameterApproval, DsGlobalCreditParameterApproval.S3G_ORG_GlobalCreditParameterApproval.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsGlobalCreditParameterApproval.S3G_ORG_GlobalCreditParameterApproval;
            }
        }
    }
}
