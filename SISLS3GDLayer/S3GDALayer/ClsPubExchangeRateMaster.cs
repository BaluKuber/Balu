﻿#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: LOB Creation DAL Class
/// Created By			: Suresh P
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Suresh P
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin LOB Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace AccountMgtServices
    {
        public class ClsPubExchangeRateMaster
        {
            #region Initialization
            int intErrorCode;

            S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable ObjLOBMasterCUDataTable_DAL;
            //S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable ObjLOBMasterDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubExchangeRateMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create New LOB

            /// <summary>
            /// Creates a new LOB by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_LOBMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubCreateExchangeMasterInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_LOBMasterDataTable)
            {
                try
                {
                    ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Insert_ExchangeRate_Details");
                        db.AddInParameter(command, "@Exchange_Currency_ID", DbType.Int32, ObjLOBMasterRow.Exchange_Currency_ID);
                        db.AddInParameter(command, "@Effective_Date", DbType.DateTime, ObjLOBMasterRow.Effective_Date);
                        db.AddInParameter(command, "@Exchange_Value", DbType.Decimal, ObjLOBMasterRow.Exchange_Value);
                        db.AddInParameter(command, "@Default_Value", DbType.Decimal, ObjLOBMasterRow.Default_Value);
                        if (!ObjLOBMasterRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjLOBMasterRow.Is_Active);
                        }
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLOBMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region Modify LOB Details

            /// <summary>
            /// Modifies an Exsisting LOB by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_LOBMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>


            public int FunPubModifyExchangeMasterInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_LOBMasterDataTable)
            {
                try
                {
                    ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Update_ExchangeRate_Details");
                        db.AddInParameter(command, "@Exchange_Rate_ID", DbType.Int32, ObjLOBMasterRow.Exchange_Rate_ID);
                        db.AddInParameter(command, "@Exchange_Currency_ID", DbType.Int32, ObjLOBMasterRow.Exchange_Currency_ID);
                        db.AddInParameter(command, "@Effective_Date", DbType.DateTime, ObjLOBMasterRow.Effective_Date);
                        db.AddInParameter(command, "@Exchange_Value", DbType.Decimal, ObjLOBMasterRow.Exchange_Value);
                        db.AddInParameter(command, "@Default_Value", DbType.Decimal, ObjLOBMasterRow.Default_Value);
                        if (!ObjLOBMasterRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjLOBMasterRow.Is_Active);
                        }

                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjLOBMasterRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable FunPubQueryExchangeRateDetails(SerializationMode SerMode, byte[] bytesObjSNXG_LOBMasterDataTable)
            {
                S3GBusEntity.AccountMgtServices dsLOBDetails = new S3GBusEntity.AccountMgtServices();
                ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_ExchangeRate_Details");

                        if (!ObjLOBMasterRow.IsExchange_Rate_IDNull())
                        {
                            db.AddInParameter(command, "@Exchange_Rate_ID", DbType.Int32, ObjLOBMasterRow.Exchange_Rate_ID);
                        }
                        db.FunPubLoadDataSet(command, dsLOBDetails, dsLOBDetails.S3G_SYSAD_ExchangeRateMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsLOBDetails.S3G_SYSAD_ExchangeRateMaster;
            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable FunPubQueryExchangeRatePaging(SerializationMode SerMode, byte[] bytesObjSNXG_LOBMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.AccountMgtServices dsLOBDetails = new S3GBusEntity.AccountMgtServices();
                ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_ExchangeRateMasterRow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_ExchangeRate_Paging");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);

                        if (!ObjLOBMasterRow.IsExchange_Rate_IDNull())
                        {
                            db.AddInParameter(command, "@Exchange_Rate_ID", DbType.Int32, ObjLOBMasterRow.Exchange_Rate_ID);
                        }
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        db.FunPubLoadDataSet(command, dsLOBDetails, dsLOBDetails.S3G_SYSAD_ExchangeRateMaster.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsLOBDetails.S3G_SYSAD_ExchangeRateMaster;
            }


            //public int FunPubModifyLOBInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_LOBMasterDataTable)
            //{
            //    try
            //    {
            //        ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable));

            //        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

            //        foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
            //        {
            //            DbCommand command = db.GetStoredProcCommand("S3G_Update_LOB_Details");
            //            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLOBMasterRow.LOB_ID);
            //            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLOBMasterRow.Company_ID);
            //            db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjLOBMasterRow.Is_Active);
            //            db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjLOBMasterRow.Modified_By);
            //            db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

            //            db.FunPubExecuteNonQuery(command);

            //            if ((int)command.Parameters["@ErrorCode"].Value > 0)
            //                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //    }
            //    return intErrorCode;

            //}
            #endregion

            #region Query LOB Details

            /// <summary>
            /// Gets a company details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_ProductMasterDataTable"></param>
            /// <returns>Datatable containing Company details</returns>

            //public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable FunPubQueryLOBDetails(SerializationMode SerMode, byte[] bytesObjSNXG_LOBMasterDataTable)
            //{
            //    S3GBusEntity.CompanyMgtServices dsLOBDetails = new S3GBusEntity.CompanyMgtServices();
            //    ObjLOBMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CUDataTable));
            //    try
            //    {
            //        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //        foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMaster_CURow ObjLOBMasterRow in ObjLOBMasterCUDataTable_DAL.Rows)
            //        {
            //            DbCommand command = db.GetStoredProcCommand("S3G_Get_LOB_Details");

            //            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLOBMasterRow.Company_ID);

            //            if (!ObjLOBMasterRow.IsLOB_IDNull())
            //            {
            //                db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLOBMasterRow.LOB_ID);
            //            }
            //            db.LoadDataSet(command, dsLOBDetails, dsLOBDetails.S3G_SYSAD_LOBMaster_CU.TableName);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //    }
            //    return dsLOBDetails.S3G_SYSAD_LOBMaster_CU;
            //}
            //public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable FunPubQueryLOBList(SerializationMode SerMode, byte[] bytesObjSNXG_LOBMasterDataTable)
            //{
            //    S3GBusEntity.CompanyMgtServices dsLOBDetails = new S3GBusEntity.CompanyMgtServices();
            //    ObjLOBMasterDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_LOBMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterDataTable));
            //    try
            //    {
            //        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //        foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_LOBMasterRow ObjLOBMasterRow in ObjLOBMasterDataTable_DAL.Rows)
            //        {
            //            DbCommand command = db.GetStoredProcCommand("S3G_Get_LOB_LIST");
            //            if (!ObjLOBMasterRow.IsCompany_IDNull())
            //            {
            //                db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLOBMasterRow.Company_ID);
            //            }
            //            if (!ObjLOBMasterRow.IsIs_ActiveNull())
            //            {
            //                db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjLOBMasterRow.Is_Active);
            //            }
            //            db.LoadDataSet(command, dsLOBDetails, dsLOBDetails.S3G_SYSAD_LOBMaster.TableName);

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //    }
            //    return dsLOBDetails.S3G_SYSAD_LOBMaster;
            //}
            #endregion
        }
    }
}
