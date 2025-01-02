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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using S3GBusEntity;
using S3GBusEntity.Collection;

namespace S3GDALayer.Collection
{
    namespace ClnReceivableMgtServices
    {
        public class ClsPubDelinquentParameter
        {
            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable objDelinquency_DAL = null;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterRow objDelinquencyRow = null;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDelinquentParameter()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            /// <summary>
            /// Inserting the Delinquent Parameters List values
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            public int FunPubCreateDelinquentParameters(SerializationMode SerMode, byte[] bytesObjS3G_CLN_DelinquencyTable, out string DSNO)
            {
                try
                {
                    DSNO = "";
                    objDelinquency_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_DelinquencyTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    objDelinquencyRow = objDelinquency_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertDelinquentParameters");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objDelinquencyRow.Company_ID);
                    if (!objDelinquencyRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objDelinquencyRow.LOB_ID);
                   // db.AddInParameter(command, "@Delinquency_code", DbType.Int32, objDelinquencyRow.Delinquency_Type_Code);
                    db.AddInParameter(command, "@DelinquentType", DbType.String, objDelinquencyRow.Delinquency_Type);
                    db.AddInParameter(command, "@Cutoff", DbType.Int32, objDelinquencyRow.Cut_Off_Month);
                    db.AddInParameter(command, "@CreatedBy", DbType.Int32, objDelinquencyRow.Created_By);
                    //db.AddInParameter(command, "@XMLDelinquentDetails", DbType.String, objDelinquencyRow.XMLDelinquentDetails);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@XMLDelinquentDetails", OracleType.Clob,
                            objDelinquencyRow.XMLDelinquentDetails.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, objDelinquencyRow.XMLDelinquentDetails);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLDelinquentDetails", DbType.String,
                            objDelinquencyRow.XMLDelinquentDetails);
                    }
                    db.AddInParameter(command, "@IsAdd", DbType.Boolean, objDelinquencyRow.IsAdd);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, objDelinquencyRow.Is_Active);
                    db.AddInParameter(command, "@period_type", DbType.String, objDelinquencyRow.Period_Type);
                    if (!objDelinquencyRow.IsDelinquent_Param_IDNull())
                    {
                        db.AddInParameter(command, "@Delinquency_No", DbType.String, objDelinquencyRow.Delinquent_Param_ID);
                    }

                    db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);


                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    if (!string.IsNullOrEmpty((string)command.Parameters["@DSNO"].Value))
                        DSNO = (string)command.Parameters["@DSNO"].Value;


                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }
            public int FunPubModifyDelinquentParameters(SerializationMode SerMode, byte[] bytesObjS3G_CLN_DelinquencyTable)
            {
                try
                {
                    
                    objDelinquency_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_DelinquencyTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    objDelinquencyRow = objDelinquency_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateDelinquentParameters");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objDelinquencyRow.Company_ID);
                    db.AddInParameter(command, "@ModifiedBy", DbType.Int32, objDelinquencyRow.Modified_By);
                    if (!objDelinquencyRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objDelinquencyRow.LOB_ID);
                    db.AddInParameter(command, "@Cutoff", DbType.Int32, objDelinquencyRow.Cut_Off_Month);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, objDelinquencyRow.Is_Active);
                    db.AddInParameter(command, "@DelinquentType", DbType.String, objDelinquencyRow.Delinquency_Type);
                    db.AddInParameter(command, "@Delinquent_Param_ID", DbType.String, objDelinquencyRow.Delinquent_Param_ID);
                    db.AddInParameter(command, "@XMLDelinquentDetails", DbType.String, objDelinquencyRow.XMLDelinquentDetails);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                   


                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

            /// <summary>
            /// Inserting the Delinquent Parameters List values
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>
            /// 
            public int FunPubDeactivateDelinquencyType(SerializationMode SerMode, byte[] bytesObjS3G_CLN_DelinquencyTable)
            {
                try
                {
                    objDelinquency_DAL = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_DelinquencyTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    objDelinquencyRow = objDelinquency_DAL.Rows[0] as S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencyParameterRow;

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_DeactivateDelinquencyType");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, objDelinquencyRow.Company_ID);
                   
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    //db.ExecuteNonQuery(command);
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
