#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Region and Branch Creation DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin Module Developement
/// Last Updated By		: Thalaiselvam N
/// Last Updated Date   : 21-Sep-2011
/// Reason              : Oracle Conversion
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
    namespace CompanyMgtServices
    {
        public class ClsPubRegionMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable ObjRegionMasterCUDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable ObjRegionMasterViewDataTable_View_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable ObjRegionMasterCUDataTable_Modify_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubRegionMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create New Region
            /// <summary>
            /// Creates a new region by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>

            public int FunPubCreateRegionInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_RegionMasterDataTable)
            {
                try
                {
                    ObjRegionMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_RegionMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_Region_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CURow ObjRegionMasterRow in ObjRegionMasterCUDataTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjRegionMasterRow.Company_ID);
                        db.AddInParameter(command, "@Region_Code", DbType.String, ObjRegionMasterRow.Region_Code);
                        db.AddInParameter(command, "@Region_Name", DbType.String, ObjRegionMasterRow.Region_Name);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRegionMasterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjRegionMasterRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjRegionMasterRow.Created_On);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    }
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }
            #endregion

            #region Update Region
            /// <summary>
            /// Creates a Update region by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>

            public int FunPubModifyRegionInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_RegionMasterDataTable)
            {
                try
                {
                    ObjRegionMasterCUDataTable_Modify_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_RegionMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_Region_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_CURow ObjRegionMasterModifyRow in ObjRegionMasterCUDataTable_Modify_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Region_Id", DbType.Int32, ObjRegionMasterModifyRow.Region_ID);
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjRegionMasterModifyRow.Company_ID);
                        db.AddInParameter(command, "@Region_Code", DbType.String, ObjRegionMasterModifyRow.Region_Code);
                        db.AddInParameter(command, "@Region_Name", DbType.String, ObjRegionMasterModifyRow.Region_Name);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjRegionMasterModifyRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjRegionMasterModifyRow.Modified_By);
                        db.AddInParameter(command, "@Modified_on", DbType.DateTime, ObjRegionMasterModifyRow.Modified_On);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }
            #endregion


            #region GetRegion
            /// <summary>
            /// Creates a Get region by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable FunPubQueryRegionInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_RegionMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.CompanyMgtServices dsRegionDetails = new S3GBusEntity.CompanyMgtServices();
                ObjRegionMasterViewDataTable_View_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_RegionMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Region_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewRow ObjRegionMasterRow in ObjRegionMasterViewDataTable_View_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjRegionMasterRow.Region_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRegionMasterRow.Company_ID);

                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        db.FunPubLoadDataSet(command, dsRegionDetails, dsRegionDetails.S3G_SYSAD_RegionMaster_View.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;


                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsRegionDetails.S3G_SYSAD_RegionMaster_View;

            }

            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable FunPubQueryRegionInt_View(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_RegionMasterDataTable)
            {
              
                S3GBusEntity.CompanyMgtServices dsRegionDetails = new S3GBusEntity.CompanyMgtServices();
                ObjRegionMasterViewDataTable_View_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_RegionMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Region_Details_View");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionMaster_ViewRow ObjRegionMasterRow in ObjRegionMasterViewDataTable_View_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjRegionMasterRow.Region_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRegionMasterRow.Company_ID);

                        db.FunPubLoadDataSet(command, dsRegionDetails, dsRegionDetails.S3G_SYSAD_RegionMaster_View.TableName);
                      

                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsRegionDetails.S3G_SYSAD_RegionMaster_View;

            }

            #endregion
        }
    }
}