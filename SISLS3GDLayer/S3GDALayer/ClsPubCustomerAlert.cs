/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminCustomerAlert
/// Created By      :   Ramesh M
/// Created Date    :   21-May-2010
/// Purpose         :   To insert and update Customer Alert Details
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
        public class ClsPubCustomerAlert
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable ObjS3G_EntityTypeDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeDataTable ObjS3G_EventTypeDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable ObjS3G_CusrtomerAlertDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCustomerAlert()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #endregion

            #region Create Customer Alert
            public int FunCreateCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDataTable)
            {
                try
                {
                    ObjS3G_CusrtomerAlertDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(bytesObjCustomerAlertDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                    // DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_CustomerAlert");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerRow in ObjS3G_CusrtomerAlertDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCustomerRow.Company_ID);
                        db.AddInParameter(command, "@Entity_Type_ID", DbType.Int32, ObjCustomerRow.Entity_Type_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCustomerRow.LOB_ID);
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjCustomerRow.Role_Code_ID);
                        db.AddInParameter(command, "@Event_Type_ID", DbType.Int32, ObjCustomerRow.Event_Type_ID);
                        db.AddInParameter(command, "@Target_Email", DbType.Boolean, ObjCustomerRow.Target_Email);
                        db.AddInParameter(command, "@Target_SMS", DbType.Boolean, ObjCustomerRow.Target_SMS);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCustomerRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCustomerRow.Created_By);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCustomerRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }
            #endregion

            #region Update Customer Alert
            public int FunUpdateCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDataTable)
            {
                try
                {                    ObjS3G_CusrtomerAlertDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(bytesObjCustomerAlertDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjCustomerRow in ObjS3G_CusrtomerAlertDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Update_CustomerAlert");

                        db.AddInParameter(command, "@Customer_Alert_ID", DbType.Int32, ObjCustomerRow.Customer_Alert_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCustomerRow.Company_ID);
                        db.AddInParameter(command, "@Entity_Type_ID", DbType.Int32, ObjCustomerRow.Entity_Type_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCustomerRow.LOB_ID);
                        db.AddInParameter(command, "@Role_Code_ID", DbType.Int32, ObjCustomerRow.Role_Code_ID);
                        db.AddInParameter(command, "@Event_Type_ID", DbType.Int32, ObjCustomerRow.Event_Type_ID);
                        db.AddInParameter(command, "@Target_Email", DbType.Boolean, ObjCustomerRow.Target_Email);
                        db.AddInParameter(command, "@Target_SMS", DbType.Boolean, ObjCustomerRow.Target_SMS);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCustomerRow.Is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCustomerRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }
            #endregion

            #region Query Entity Master 
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable FunPubQueryEntityTypeMasterList(SerializationMode SerMode, byte[] bytesS3G_entityTypedataTable)
            {
                S3GBusEntity.CompanyMgtServices DsEntityType = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_EntityTypeDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable)ClsPubSerialize.DeSerialize(bytesS3G_entityTypedataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_EntityType");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EntityTypeRow ObjEntityRow in ObjS3G_EntityTypeDataTable_DAL.Rows)
                    {
                        if (ObjEntityRow.IsEntity_Type_IDNull())
                        {
                            db.AddInParameter(command, "@Entity_Type_ID", DbType.Int32, ObjEntityRow.Entity_Type_ID);
                        }
                    }
                    //db.LoadDataSet(command, DsEntityType, DsEntityType.S3G_SYSAD_EntityType.TableName);
                    db.FunPubLoadDataSet(command, DsEntityType, DsEntityType.S3G_SYSAD_EntityType.TableName);

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsEntityType.S3G_SYSAD_EntityType;
            }
            #endregion

            #region Query Event Type Master
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeDataTable FunPubQueryEventTypeMasterList(SerializationMode SerMode, byte[] bytesObjS3GEventtypeMasterListTable)
            {
                S3GBusEntity.CompanyMgtServices DsEventTypeMaster = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_EventTypeDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeDataTable)ClsPubSerialize.DeSerialize(bytesObjS3GEventtypeMasterListTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_EventType");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_EventTypeRow ObjEventTypwRow in ObjS3G_EventTypeDataTable_DAL.Rows)
                    {
                        if (!ObjEventTypwRow.IsEvent_Type_IDNull())
                        {
                            db.AddInParameter(command, "@Event_Type_ID", DbType.Int32, ObjEventTypwRow.Event_Type_ID);
                        }
                    }
                    db.FunPubLoadDataSet(command, DsEventTypeMaster, DsEventTypeMaster.S3G_SYSAD_EventType.TableName);

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsEventTypeMaster.S3G_SYSAD_EventType;
            }
            #endregion

            #region Query Customer Alert 
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable FunPubQueryCustomerAlert(SerializationMode SerMode, byte[] bytesObjS3GCustomerAlertTable)
            {
                S3GBusEntity.CompanyMgtServices DscustomerAlaert = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_CusrtomerAlertDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(bytesObjS3GCustomerAlertTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjcustomerRow in ObjS3G_CusrtomerAlertDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_customerAlert");
                        if (!ObjcustomerRow.IsCustomer_Alert_IDNull())
                        {
                            db.AddInParameter(command, "@Customer_Alert_ID", DbType.Int32, ObjcustomerRow.Customer_Alert_ID);
                        }
                        if (!ObjcustomerRow.IsCompany_IDNull())
                        {
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjcustomerRow.Company_ID);
                        }
                        if (!ObjcustomerRow.IsIs_ActiveNull())
                        {
                            db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjcustomerRow.Is_Active);
                        }
                        db.FunPubLoadDataSet(command, DscustomerAlaert, DscustomerAlaert.S3G_SYSAD_CustomerAlert.TableName);

                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DscustomerAlaert.S3G_SYSAD_CustomerAlert;
            }
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable FunPubQueryCustomerAlertPaging(SerializationMode SerMode, byte[] bytesObjCustomerAlertDataTable, out int intTotalRecords, PagingValues ObjPaging)  ///Searching for kali
            {

                intTotalRecords = 0;
                S3GBusEntity.CompanyMgtServices dsCustomerAlertDetails = new S3GBusEntity.CompanyMgtServices();
               // ObjS3G_CusrtomerAlertDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(bytesObjCustomerAlertDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_CustomerAlert_Paging");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                    db.FunPubLoadDataSet(command, dsCustomerAlertDetails, dsCustomerAlertDetails.S3G_SYSAD_CustomerAlert.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                    intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsCustomerAlertDetails.S3G_SYSAD_CustomerAlert;
            }
            #endregion

            #region Delete Customer Alert
            public int FunPubDeleteCustomerAlert(SerializationMode SerMode, byte[] bytesObjS3GCustomerAlertTable)
            {
                S3GBusEntity.CompanyMgtServices DscustomerAlaert = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_CusrtomerAlertDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable)ClsPubSerialize.DeSerialize(bytesObjS3GCustomerAlertTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CustomerAlertRow ObjcustomerRow in ObjS3G_CusrtomerAlertDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Delete_CustomerAlert");
                        db.AddInParameter(command, "@Customer_Alert_ID", DbType.Int32, ObjcustomerRow.Customer_Alert_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected; 
            }
            #endregion
        }
    }
}
