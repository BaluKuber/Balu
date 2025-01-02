#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Branch Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 12-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 12-May-2010
/// Reason              : System Admin Module Developement
/// Last Updated By		: Thalaiselvam N
/// Last Updated Date   : 22-Sep-2011
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
        public class ClsPubBranchMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable ObjBranchMasterCUDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable ObjBranchMasterViewDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable ObjRegionCodeDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameDataTable ObjRegion_NameDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable ObjBranch_ListDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubBranchMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            #endregion

            #region Create New Branch
            /// <summary>
            /// Creates a new branch by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>

            public int FunPubCreateBranchInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BranchMasterDataTable)
            {
                try
                {
                    ObjBranchMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BranchMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_Branch_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CURow ObjBranchMasterRow in ObjBranchMasterCUDataTable_DAL.Rows)
                    {
                                                
                        db.AddInParameter(command, "@Region_Id", DbType.Int32, ObjBranchMasterRow.Region_ID);
                        db.AddInParameter(command, "@Branch_Code", DbType.String, ObjBranchMasterRow.Branch_Code);
                        db.AddInParameter(command, "@Branch_Name", DbType.String, ObjBranchMasterRow.Branch_Name);
                        db.AddInParameter(command, "@State_Name", DbType.String, ObjBranchMasterRow.State_Name);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjBranchMasterRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjBranchMasterRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjBranchMasterRow.Created_On);                        
                        db.AddInParameter(command, "@Prev_Region_ID", DbType.Int32, ObjBranchMasterRow.Prev_Region_ID);
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

            public int FunPubModifyBranchInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BranchMasterDataTable)
            {
                try
                {
                    ObjBranchMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BranchMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_Branch_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchMaster_CURow ObjBranchMasterRow in ObjBranchMasterCUDataTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Branch_Id", DbType.Int32, ObjBranchMasterRow.Branch_ID);                        
                        db.AddInParameter(command, "@Region_Id", DbType.Int32, ObjBranchMasterRow.Region_ID);
                        db.AddInParameter(command, "@Prev_Region_ID", DbType.Int32, ObjBranchMasterRow.Prev_Region_ID);
                        db.AddInParameter(command, "@State_Name", DbType.String, ObjBranchMasterRow.State_Name);
                        db.AddInParameter(command, "@IS_Active", DbType.Boolean, ObjBranchMasterRow.Is_Active);                        
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjBranchMasterRow.Modified_By);
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

            #region regioncode&name
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable FunPubRegionCodeInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_RegionCodeDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsRegionCode = new S3GBusEntity.CompanyMgtServices();
                ObjRegionCodeDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_RegionCodeDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Region_Code");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_RegionCodeRow ObjRegionCodeRow in ObjRegionCodeDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRegionCodeRow.Company_ID);
                        db.FunPubLoadDataSet(command, dsRegionCode, dsRegionCode.S3G_SYSAD_RegionCode.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsRegionCode.S3G_SYSAD_RegionCode;

            }


            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameDataTable FunPubRegionNameInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_Region_NameDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsRegionName = new S3GBusEntity.CompanyMgtServices();
                ObjRegion_NameDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_Region_NameDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Region_Name");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Region_NameRow ObjRegionNameRow in ObjRegion_NameDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Branch_ID", DbType.String, ObjRegionNameRow.Branch_ID);
                        db.FunPubLoadDataSet(command, dsRegionName, dsRegionName.S3G_SYSAD_Region_Name.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsRegionName.S3G_SYSAD_Region_Name;

            }



            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable FunPubQueryBranchInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BranchMasterDataTable,  out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.CompanyMgtServices dsBranchDetails = new S3GBusEntity.CompanyMgtServices();
                ObjBranchMasterViewDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BranchMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Branch_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewRow ObjBranchMasterRow in ObjBranchMasterViewDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjBranchMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBranchMasterRow.Company_ID);

                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        db.FunPubLoadDataSet(command, dsBranchDetails, dsBranchDetails.S3G_SYSAD_BranchDetails_View.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsBranchDetails.S3G_SYSAD_BranchDetails_View;

            }

            // DAL

            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable FunPubQueryBranchInt_List(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BranchMasterDataTable)
            {

                S3GBusEntity.CompanyMgtServices dsBranchDetails = new S3GBusEntity.CompanyMgtServices();
                ObjBranchMasterViewDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BranchMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Branch_Details_List");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_BranchDetails_ViewRow ObjBranchMasterRow in ObjBranchMasterViewDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjBranchMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBranchMasterRow.Company_ID);



                        db.FunPubLoadDataSet(command, dsBranchDetails, dsBranchDetails.S3G_SYSAD_BranchDetails_View.TableName);

                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsBranchDetails.S3G_SYSAD_BranchDetails_View;

            }
           
            #endregion


            #region BranchList
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable FunPubBranchList(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BranchListDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsBranchList = new S3GBusEntity.CompanyMgtServices();
                ObjBranch_ListDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BranchListDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Branch_List");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_Branch_ListRow ObjBranchListRow in ObjBranch_ListDataTable_DAL.Rows)
                    {
                        if (!ObjBranchListRow.IsRegion_IDNull())
                        {
                            db.AddInParameter(command, "@Region_ID", DbType.Int32, ObjBranchListRow.Region_ID);
                        }
                        if (!ObjBranchListRow.IsCompany_IDNull())
                        {

                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBranchListRow.Company_ID);
                        }
                        db.FunPubLoadDataSet(command, dsBranchList, dsBranchList.S3G_SYSAD_Branch_List.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsBranchList.S3G_SYSAD_Branch_List;

            }


            #endregion



        }
    }
}
