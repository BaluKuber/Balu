#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: CreditScore Master
/// Created By			: Kaliraj K
/// Created Date		: 23-Jun-2010
/// Purpose	            : 

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
    namespace CreditMgtServices
    {
        public class ClsPubCreditScore
        {
            int intRowsAffected;
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable ObjCreditScore_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCreditScore()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateCreditScoreDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreDataTable, out int outNoofYear, out int outCreditScoreID)
            {
                try
                {
                    outNoofYear = 0;
                    outCreditScoreID = 0;
                    ObjCreditScore_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CreditScoreDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow in ObjCreditScore_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertCreditScoreDetails");
                        db.AddInParameter(command, "@CreditScore_ID", DbType.Int32, ObjCreditScoreRow.CreditScore_Guide_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCreditScoreRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCreditScoreRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCreditScoreRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjCreditScoreRow.Constitution_ID);
                        db.AddInParameter(command, "@NoOfYears", DbType.Int32, ObjCreditScoreRow.NoOfYears);
                        db.AddInParameter(command, "@XMLCreditScoreParamDet", DbType.String, ObjCreditScoreRow.XMLCreditScoreParameterValues);
                        db.AddInParameter(command, "@YearValue", DbType.Int32, ObjCreditScoreRow.YearValue);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjCreditScoreRow.Created_By);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCreditScoreRow.Is_Active);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@outNoofYear", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@outCreditScoreID", DbType.Int32, sizeof(Int64));
                        
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        }
                        if (command.Parameters["@outNoofYear"].Value != null)
                            outNoofYear = (int)command.Parameters["@outNoofYear"].Value;
                        if (command.Parameters["@outCreditScoreID"].Value != null)
                            outCreditScoreID = (int)command.Parameters["@outCreditScoreID"].Value;
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

            public int FunPubDeleteCreditScoreParamDetails(int intCreditScoreParamId)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_DeleteCreditScoreParamDetails");
                    db.AddInParameter(command, "@CreditScoreParamId", DbType.Int32, intCreditScoreParamId);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    {
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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

        
            #region Query CreditScore Details
            /// <summary>
            /// Gets a CreditScore details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_CreditScoreDataTable"></param>
            /// <returns>Datatable containing CreditScore Parameter Details</returns>

            public DataSet FunPubQueryCreditScoreParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreDataTable)
            {
                DataSet dsCreditScore = new DataSet();
                ObjCreditScore_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CreditScoreDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow in ObjCreditScore_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_GetCreditScoreParameterDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCreditScoreRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjCreditScoreRow.Created_By);
                        db.AddInParameter(command, "@CreditScore_ID", DbType.Int32, ObjCreditScoreRow.CreditScore_Guide_ID);
                        db.AddInParameter(command, "@YearValue", DbType.Int32, ObjCreditScoreRow.YearValue);

                        dsCreditScore = db.FunPubExecuteDataSet(command);
                    }
                    return dsCreditScore;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dsCreditScore.Dispose();
                    dsCreditScore = null;
                }

            }

            #endregion

            #region Query CreditScore Details
            /// <summary>
            /// Gets a CreditScore details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_CreditScoreDataTable"></param>
            /// <returns>Datatable containing CreditScore Details</returns>

            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable FunPubQueryCreditScoreDetails(SerializationMode SerMode, byte[] bytesObjSNXG_CreditScoreDataTable)
            {
                S3GBusEntity.Origination.CreditMgtServices dsCreditScoreDetails = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditScore_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_CreditScoreDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreRow ObjCreditScoreRow in ObjCreditScore_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_GetCreditScoreDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCreditScoreRow.Company_ID);
                        db.AddInParameter(command, "@CreditScore_ID", DbType.Int32, ObjCreditScoreRow.CreditScore_Guide_ID);
                        db.FunPubLoadDataSet(command, dsCreditScoreDetails, dsCreditScoreDetails.S3G_ORG_CreditScore.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsCreditScoreDetails.S3G_ORG_CreditScore;
            }

            #endregion
              

        }
    }
}
