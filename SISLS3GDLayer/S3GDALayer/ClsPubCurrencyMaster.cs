#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Currency Master DAL Class
/// Created By			: Kaliraj K
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace S3GDALayer
{
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CurrencyMgtServices to the service group
    namespace AccountMgtServices
    {
        public class ClsPubCurrencyMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable ObjCurrencyMasterCUDataTable_DAL;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable ObjCurrencyMasterDataTable_DAL;
            S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable ObjCurrencyMaster_ViewDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCurrencyMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create New Currency
            /// <summary>
            /// Creates a new Currency by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CurrencyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubCreateCurrencyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CurrencyMasterDataTable)
            {
                try
                {

                    ObjCurrencyMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_Currency_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CURow ObjCurrencyMasterRow in ObjCurrencyMasterCUDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjCurrencyMasterRow.Currency_ID);
                        db.AddInParameter(command, "@Precision", DbType.Int32, ObjCurrencyMasterRow.Precision);
                        db.AddInParameter(command, "@Active", DbType.Boolean, ObjCurrencyMasterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCurrencyMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
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
            #endregion

            #region Modify Currency Details
            /// <summary>
            /// Modifies an Exsisting Currency by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CurrencyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubModifyCurrencyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CurrencyMasterDataTable)
            {
                try
                {

                    ObjCurrencyMasterCUDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CUDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_Currency_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_CURow ObjCurrencyMasterRow in ObjCurrencyMasterCUDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjCurrencyMasterRow.Currency_ID);
                        db.AddInParameter(command, "@Precision", DbType.Int32, ObjCurrencyMasterRow.Precision);
                        db.AddInParameter(command, "@Active", DbType.Boolean, ObjCurrencyMasterRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCurrencyMasterRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }
            #endregion

            #region Delete Exsisting Currency
            /// <summary>
            /// Deletes an Exsisting Currency by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CurrencyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubDeleteCurrencyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CurrencyMasterDataTable)
            {
                try
                {

                    ObjCurrencyMasterDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Delete_Currency_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow in ObjCurrencyMasterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Currency_Id", DbType.Int32, ObjCurrencyMasterRow.Currency_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        //db.ExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;

            }
            #endregion

            #region Query Currency Details
            /// <summary>
            /// Gets a Currency details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_CurrencyMasterDataTable"></param>
            /// <returns>Datatable containing Currency details</returns>

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable FunPubQueryCurrencyMasterList(SerializationMode SerMode, byte[] bytesObjSNXG_CurrencyMasterDataTable)
            {
                S3GBusEntity.AccountMgtServices dsCurrencyDetails = new S3GBusEntity.AccountMgtServices();
                ObjCurrencyMasterDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Currency_Details");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow in ObjCurrencyMasterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjCurrencyMasterRow.Currency_ID);
                        //db.LoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                        db.FunPubLoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View;
            }

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable FunPubQueryCurrencyPaging(SerializationMode SerMode, byte[] bytesObjSNXG_CurrencyMasterDataTable,out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.AccountMgtServices dsCurrencyDetails = new S3GBusEntity.AccountMgtServices();
                ObjCurrencyMasterDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Currency_Details_Paging");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow in ObjCurrencyMasterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Currency_ID", DbType.Int32, ObjCurrencyMasterRow.Currency_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        //db.LoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                        db.FunPubLoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;

                        
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View;
            }
          

            public S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMaster_ViewDataTable FunPubQueryCurrencyList(SerializationMode SerMode, byte[] bytesObjSNXG_CurrencyMasterDataTable)
            {
                S3GBusEntity.AccountMgtServices dsCurrencyDetails = new S3GBusEntity.AccountMgtServices();
                ObjCurrencyMasterDataTable_DAL = (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_CurrencyMasterDataTable, SerMode, typeof(S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.AccountMgtServices.S3G_SYSAD_CurrencyMasterRow ObjCurrencyMasterRow in ObjCurrencyMasterDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_Currency_LIST");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCurrencyMasterRow.Company_ID);
                        db.AddInParameter(command, "@ExcludeDefaultCurrency", DbType.Boolean, true);
//                        db.LoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                        db.FunPubLoadDataSet(command, dsCurrencyDetails, dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsCurrencyDetails.S3G_SYSAD_CurrencyMaster_View;
            }
            #endregion
        }
    }
}
