#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Debt Collector Master DAL Class
/// Created By			: Suresh P
/// Created Date		: 06-Oct-2010
/// Purpose	            : DAL Class for Debt Collector Master Methods
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
        public class ClsPubDebtCollectorMaster
        {
            #region Initialization

            int intErrorCode;

            Entity_Collection.ClnDebtMgtServices dsDebtCollectorDetails = null;

            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterDataTable ObjDebtCollectorMasterDataTable_DAL = null;
            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterRow ObjDebtCollectorMasterRow = null;

            Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsDataTable ObjDebtCollectorDetailsDataTable_DAL = null;
            //Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsRow

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDebtCollectorMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create DebtCollector
            /// <summary>
            /// Insert Debt Collector
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjDebtCollectorDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateDebtCollectorInt(SerializationMode SerMode, byte[] bytesObjDebtCollectorDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertDebtCollConditions");
                    ObjDebtCollectorMasterDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterDataTable));
                    ObjDebtCollectorMasterRow = ObjDebtCollectorMasterDataTable_DAL.Rows[0] as Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDebtCollectorMasterRow.Company_ID);
                    if (ObjDebtCollectorMasterRow.LOB_ID != 0)
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorMasterRow.LOB_ID);
                    //Parameter added by Tamilselvan on 28/12/2010
                    if (ObjDebtCollectorMasterRow.Region_ID != 0)
                        db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjDebtCollectorMasterRow.Region_ID);
                    if (ObjDebtCollectorMasterRow.Branch_ID != 0)
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDebtCollectorMasterRow.Branch_ID);
                    db.AddInParameter(command, "@DebtCollector_Type", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_Type);
                    db.AddInParameter(command, "@DebtCollector_Code", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_Code);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDebtCollectorMasterRow.Is_Active);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDebtCollectorMasterRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjDebtCollectorMasterRow.Created_On);
                    //Parameter added by Tamilselvan on 3/1/2011
                    db.AddInParameter(command, "@Emp_UTPA_ID", DbType.Int32, ObjDebtCollectorMasterRow.Emp_UTPA_ID);
                    db.AddInParameter(command, "@Next_Level", DbType.Int32, ObjDebtCollectorMasterRow.Next_Level);
                    db.AddInParameter(command, "@Super_Level", DbType.Int32, ObjDebtCollectorMasterRow.Super_Level);
                    //db.AddInParameter(command, "@XMLParamtDebtCollectorDet", DbType.String, ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@XMLParamtDebtCollectorDet", OracleType.Clob,
                            ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLParamtDebtCollectorDet", DbType.String,
                            ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                    }
                    //db.AddInParameter(command, "@DebtCollector_ID", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_ID);
                    if (!ObjDebtCollectorMasterRow.IsTxn_IDNull())
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjDebtCollectorMasterRow.Txn_ID);
                    db.AddInParameter(command, "@Frequency_Type", DbType.Int32,ObjDebtCollectorMasterRow .Frequency_Type );
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

            #region Get DebtCollector
            /// <summary>
            /// Get DebtCollector
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjDebtCollectorDetailsDataTable"></param>
            /// <returns></returns>
            public Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsDataTable FunPubQueryDebtCollector(SerializationMode SerMode, byte[] bytesObjDebtCollectorDetailsDataTable)
            {
                dsDebtCollectorDetails = new S3GBusEntity.Collection.ClnDebtMgtServices();
                ObjDebtCollectorDetailsDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorDetailsDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsDataTable));
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetDebtCollectorDetails");
                try
                {
                    foreach (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsRow ObjDebtCollectorDetailsRow in ObjDebtCollectorDetailsDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@DebtCollector_Code", DbType.String, ObjDebtCollectorDetailsRow.DebtCollector_Code);
                        db.FunPubLoadDataSet(command, dsDebtCollectorDetails, dsDebtCollectorDetails.S3G_CLN_DebtCollectorDetails.TableName);
                    }
                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDebtCollectorDetails.S3G_CLN_DebtCollectorDetails;
            }
            #endregion

            #region Modify DebtCollector
            public int FunPubModifyDebtCollectorInt(SerializationMode SerMode, byte[] bytesObjDebtCollectorDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_ModifyDebtCollConditions");

                    ObjDebtCollectorMasterDataTable_DAL = (Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjDebtCollectorDataTable, SerMode, typeof(Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterDataTable));
                    ObjDebtCollectorMasterRow = ObjDebtCollectorMasterDataTable_DAL.Rows[0] as Entity_Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorMasterRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDebtCollectorMasterRow.Company_ID);
                    /*
                    if (!ObjDebtCollectorMasterRow.IsLOB_IDNull())
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorMasterRow.LOB_ID);
                    if (!ObjDebtCollectorMasterRow.IsBranch_IDNull())
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjDebtCollectorMasterRow.Branch_ID);
                    if (!ObjDebtCollectorMasterRow.IsDebtCollector_TypeNull())
                        db.AddInParameter(command, "@DebtCollector_Type", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_Type);
                    */
                    //db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorMasterRow.LOB_ID);
                    //db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjDebtCollectorMasterRow.Region_ID);
                    //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjDebtCollectorMasterRow.Branch_ID);

                    if (ObjDebtCollectorMasterRow.LOB_ID != 0)
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDebtCollectorMasterRow.LOB_ID);
                    //Parameter added by Tamilselvan on 28/12/2010
                    if (ObjDebtCollectorMasterRow.Region_ID != 0)
                        db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjDebtCollectorMasterRow.Region_ID);
                    if (ObjDebtCollectorMasterRow.Branch_ID != 0)
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDebtCollectorMasterRow.Branch_ID);


                    db.AddInParameter(command, "@DebtCollector_Code", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_Code);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDebtCollectorMasterRow.Is_Active);
                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjDebtCollectorMasterRow.Modified_By);
                    //Source Modified by Tamilselvan 4/01/2011   
                    db.AddInParameter(command, "@DebtCollector_ID", DbType.String, ObjDebtCollectorMasterRow.DebtCollector_ID);
                    db.AddInParameter(command, "@Next_Level", DbType.Int32, ObjDebtCollectorMasterRow.Next_Level);
                    db.AddInParameter(command, "@Super_Level", DbType.Int32, ObjDebtCollectorMasterRow.Super_Level);
                    //db.AddInParameter(command, "@XMLParamtDebtCollectorDet", DbType.String, ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@XMLParamtDebtCollectorDet", OracleType.Clob,
                            ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                        command.Parameters.Add(param);
                    }
                    else
                    {
                        db.AddInParameter(command, "@XMLParamtDebtCollectorDet", DbType.String,
                            ObjDebtCollectorMasterRow.XMLParamtDebtCollectorDet);
                    }
                   // db.AddInParameter(command, "@Frequency_Type", DbType.Int32, ObjDebtCollectorMasterRow .Frequency_Type );
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
