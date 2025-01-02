#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Number Control Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 22-May-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : System Admin Module Developement
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
    namespace DocMgtServices
    {

        public class ClsPubDoucmentNoControl
        {
            #region Initialization
            int intRowsAffected;
            int intErrorCode;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable ObjDocTypeListTable_DAL;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable ObjGetDNCDetailsTable_DAL;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterDataTable ObjGetDNCMasterTable_DAL;
            S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable ObjGetBatchList_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDoucmentNoControl()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion


            #region DocTypeList

            public S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable FunPubDocTypeList(SerializationMode SerMode, byte[] bytesObjSNXG_DocTypeListDataTable)
            {
                S3GBusEntity.DocMgtServices dsDocTypeDetails = new S3GBusEntity.DocMgtServices();
                ObjDocTypeListTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_DocTypeListDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListRow ObjDocTypeListRow in ObjDocTypeListTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_DocumentType_List");
                        //db.LoadDataSet(command, dsDocTypeDetails, dsDocTypeDetails.S3G_SYSAD_DocumentationType_List.TableName);
                        db.FunPubLoadDataSet(command, dsDocTypeDetails, dsDocTypeDetails.S3G_SYSAD_DocumentationType_List.TableName);

                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsDocTypeDetails.S3G_SYSAD_DocumentationType_List;
            }
            #endregion

            #region Create DNS
            public int FunPubCreateDNS(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_DNSMasterDataTable)
            {
                try
                {
                    ObjGetDNCMasterTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_DNSMasterDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_InsertDNCDetails");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterRow ObjDNSMasterRow in ObjGetDNCMasterTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDNSMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDNSMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDNSMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, ObjDNSMasterRow.Document_Type_ID);
                        db.AddInParameter(command, "@Fin_Year", DbType.String, ObjDNSMasterRow.Fin_Year);
                        db.AddInParameter(command, "@Batch", DbType.String, ObjDNSMasterRow.Batch);
                        db.AddInParameter(command, "@From_Number", DbType.Decimal, ObjDNSMasterRow.From_Number);
                        db.AddInParameter(command, "@To_Number", DbType.Decimal, ObjDNSMasterRow.To_Number);
                        db.AddInParameter(command, "@Level", DbType.String, ObjDNSMasterRow.Level);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDNSMasterRow.Is_Active);
                        db.AddInParameter(command, "@Last_Used_Number", DbType.String, ObjDNSMasterRow.Last_Used_Number);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjDNSMasterRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjDNSMasterRow.Created_On);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        //db.ExecuteNonQuery(command);
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

            public int FunPubDeleteDNS(int intDoc_Number_Seq_ID)
            {
                try
                {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_Delete_DNCDetails");
                db.AddInParameter(command, "@Doc_Number_Seq_ID", DbType.Int32, intDoc_Number_Seq_ID);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                db.FunPubExecuteNonQuery(command);
                    //db.ExecuteNonQuery(command);
                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;

            }
            #region Modify DNS
            public int FunPubModifyDNS(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_DNSMasterDataTable)
            {
                try
                {
                    ObjGetDNCMasterTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_DNSMasterDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_DNC_Details");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_DNCMasterRow ObjDNSMasterRow in ObjGetDNCMasterTable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Doc_Number_Seq_ID", DbType.Int32, ObjDNSMasterRow.Doc_Number_Seq_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDNSMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjDNSMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjDNSMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, ObjDNSMasterRow.Document_Type_ID);
                        db.AddInParameter(command, "@Fin_Year", DbType.String, ObjDNSMasterRow.Fin_Year);
                        db.AddInParameter(command, "@Batch", DbType.String, ObjDNSMasterRow.Batch);
                        db.AddInParameter(command, "@From_Number", DbType.Decimal, ObjDNSMasterRow.From_Number);
                        db.AddInParameter(command, "@To_Number", DbType.Decimal, ObjDNSMasterRow.To_Number);
                        db.AddInParameter(command, "@Level", DbType.String, ObjDNSMasterRow.Level);                        
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjDNSMasterRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjDNSMasterRow.Modified_On);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjDNSMasterRow.Is_Active);
                        db.AddInParameter(command, "@Last_Used_Number", DbType.String, ObjDNSMasterRow.Last_Used_Number);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
//                        db.ExecuteNonQuery(command);
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

            #region Get DNS Details

            public S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable FunPubQueryDNS(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_DNSDetailsDataTable)
            {
                S3GBusEntity.DocMgtServices dsDNCDetails = new S3GBusEntity.DocMgtServices();
                ObjGetDNCDetailsTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_DNSDetailsDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_DNCDetails");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsRow ObjDNCDetailsRow in ObjGetDNCDetailsTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Doc_Number_Seq_ID", DbType.Int32, ObjDNCDetailsRow.Doc_Number_Seq_ID);
                        //db.LoadDataSet(command, dsDNCDetails, dsDNCDetails.S3G_SYSAD_Get_DNCDetails.TableName);
                        db.FunPubLoadDataSet(command, dsDNCDetails, dsDNCDetails.S3G_SYSAD_Get_DNCDetails.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDNCDetails.S3G_SYSAD_Get_DNCDetails;

            }


            public S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable FunPubQueryDNS_View(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_DNSDetailsDataTable, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.DocMgtServices dsDNCDetails = new S3GBusEntity.DocMgtServices();
                ObjGetDNCDetailsTable_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_DNSDetailsDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_DNCDetails_View");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsRow ObjDNCDetailsRow in ObjGetDNCDetailsTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Doc_Number_Seq_ID", DbType.Int32, ObjDNCDetailsRow.Doc_Number_Seq_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjDNCDetailsRow.Company_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddInParameter(command, "@Program_ID", DbType.Int32, ObjPaging.ProProgram_ID);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        //db.LoadDataSet(command, dsDNCDetails, dsDNCDetails.S3G_SYSAD_Get_DNCDetails.TableName);
                        db.FunPubLoadDataSet(command, dsDNCDetails, dsDNCDetails.S3G_SYSAD_Get_DNCDetails.TableName);

                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsDNCDetails.S3G_SYSAD_Get_DNCDetails;

            }
            #endregion

            #region Get Batch
            public S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable FunPubGetBatch(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_BatchDetailsDataTable)
            {
                S3GBusEntity.DocMgtServices dsBatchDetails = new S3GBusEntity.DocMgtServices();
                ObjGetBatchList_DAL = (S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_BatchDetailsDataTable, SerMode, typeof(S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_GetBatch_List");
                    foreach (S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListRow ObjBatchDetailsRow in ObjGetBatchList_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBatchDetailsRow.Company_ID);
                        //db.LoadDataSet(command, dsBatchDetails, dsBatchDetails.S3G_SYSAD_GetBatch_List.TableName);
                        db.FunPubLoadDataSet(command, dsBatchDetails, dsBatchDetails.S3G_SYSAD_GetBatch_List.TableName);
                    }

                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsBatchDetails.S3G_SYSAD_GetBatch_List;

            }
#endregion 

          




        }

    }
}
