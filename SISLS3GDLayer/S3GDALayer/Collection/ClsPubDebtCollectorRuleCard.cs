#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Debt Collector RuleCard DAL Class
/// Created By			: Suresh P
/// Created Date		: 11-Oct-2010
/// Purpose	            : DAL Class for Debt Collector RuleCard Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_Collection = S3GBusEntity.Collection;
#endregion

namespace S3GDALayer.Collection
{
    namespace ClnDebtMgtServices
    {
        public class ClsPubDebtCollectorRuleCard
        {
            #region Initialization

            int intErrorCode;

            Entity_Collection.ClnDebtMgtServices dsDebtCollectorDetails = null;

            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable ObjDebtCollectorRuleCardDataTable_DAL = null;
            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardRow ObjDebtCollectorRuleCardRow = null;

            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDetailsDataTable ObjDebtCollectorDetailsDataTable_DAL = null;
            //Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsRow
            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDebtCollectorRuleCard()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create DebtCollector RuleCard
            /// <summary>
            /// Insert Debt Collector RuleCard
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjDebtCollectorRuleCardDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateDebtCollectorRuleCardInt(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertDebtCollectorRuleCard");

                    ObjDebtCollectorRuleCardDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorRuleCardDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable));
                    ObjDebtCollectorRuleCardRow = ObjDebtCollectorRuleCardDataTable_DAL.Rows[0] as Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.Company_ID);
                    if (!ObjDebtCollectorRuleCardRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.LOB_ID);
                    if (!ObjDebtCollectorRuleCardRow.IsRegion_IDNull())
                        db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.Region_ID);
                    if (!ObjDebtCollectorRuleCardRow.IsBranch_IDNull())
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.Branch_ID);
                    db.AddInParameter(command, "@RuleCardEffective_From", DbType.DateTime, ObjDebtCollectorRuleCardRow.RuleCardEffective_From);
                    db.AddInParameter(command, "@RuleCardActive_Flag", DbType.Boolean, ObjDebtCollectorRuleCardRow.RuleCardActive_Flag);
                    db.AddInParameter(command, "@Sequence_String", DbType.String, ObjDebtCollectorRuleCardRow.Sequence_String);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDebtCollectorRuleCardRow.Created_By);
                    //db.AddInParameter(command, "@XMLPARDCRULECARDDET", DbType.String, ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@XMLPARDCRULECARDDET", OracleType.Clob,
                            ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLPARDCRULECARDDET", DbType.String,
                            ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                    }
                    if (!ObjDebtCollectorRuleCardRow.IsTxn_IDNull())
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.Txn_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    if ((int)command.Parameters["@ErrorCode"].Value < 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;


                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region Get DebtCollector
            /// <summary>
            /// Get DebtCollector Rulecard Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjDebtCollectorRuleCardDetailsDataTable"></param>
            /// <returns></returns>
            public Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable FunPubQueryDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDetailsDataTable)
            {
                dsDebtCollectorDetails = new S3GBusEntity.Collection.ClnDebtMgtServices();
                ObjDebtCollectorRuleCardDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorRuleCardDetailsDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable));
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetDebtCollectorRuleCardDetails");
                try
                {
                    foreach (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardRow ObjDebtCollectorDetailsRow in ObjDebtCollectorRuleCardDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@DebtCollectorRuleCard_ID", DbType.String, ObjDebtCollectorDetailsRow.DebtCollectorRuleCard_ID);
                        db.FunPubLoadDataSet(command, dsDebtCollectorDetails, dsDebtCollectorDetails.S3G_CLN_DebtCollectorRuleCard.TableName);
                    }
                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDebtCollectorDetails.S3G_CLN_DebtCollectorRuleCard;
            }
            #endregion

            #region Modify DebtCollector
            public int FunPubModifyDebtCollectorRuleCardInt(SerializationMode SerMode, byte[] bytesObjDebtCollectorDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_UpdateDebtCollectorRuleCard");

                    ObjDebtCollectorRuleCardDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable));
                    ObjDebtCollectorRuleCardRow = ObjDebtCollectorRuleCardDataTable_DAL.Rows[0] as Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardRow;


                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDebtCollectorRuleCardRow.Company_ID);
                    /*
                    if (!ObjDebtCollectorMasterRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorMasterRow.LOB_ID);
                    if (!ObjDebtCollectorMasterRow.IsBranch_IDNull())
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjDebtCollectorMasterRow.Branch_ID);
                    if (!ObjDebtCollectorMasterRow.IsDebtCollector_TypeNull())
                        db.AddInParameter(command, "@DebtCollector_Type", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_Type);
                    */
                    db.AddInParameter(command, "@DebtCollectorRuleCard_ID", DbType.String, ObjDebtCollectorRuleCardRow.DebtCollectorRuleCard_ID);
                    db.AddInParameter(command, "@RuleCardActive_Flag", DbType.Boolean, ObjDebtCollectorRuleCardRow.RuleCardActive_Flag);
                    //db.AddInParameter(command, "@XMLPARDCRULECARDDET", DbType.String, ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@XMLPARDCRULECARDDET", OracleType.Clob,
                            ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLPARDCRULECARDDET", DbType.String,
                            ObjDebtCollectorRuleCardRow.XMLParamtDebtCollectorRuleCardDet);
                    }
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjDebtCollectorRuleCardRow.Modified_By);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.FunPubExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion
        }

    }
}
