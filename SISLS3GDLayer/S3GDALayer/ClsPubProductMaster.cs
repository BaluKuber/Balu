/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminProductMaster_Add
/// Created By      :   Ramesh M
/// Created Date    :   10-May-2010
/// Purpose         :   To insert and update product master details
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
        public class ClsPubProductMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable ObjS3G_ProductMaster_CUDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable ObjS3G_ProductMaster_ViewDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMasterDataTable  ObjS3G_ProductMasterDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubProductMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion
            #region DML Operation
            public int FunProductMasterInsertInt(SerializationMode SerMode, byte[] bytesObjSNXG_Product_MasterDataTable)
            {
                try
                {
                    ObjS3G_ProductMaster_CUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_Product_MasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Insert_Product_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CURow ObjProductMasterRow in ObjS3G_ProductMaster_CUDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.String, ObjProductMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_Code", DbType.String, ObjProductMasterRow.Product_Code);
                        db.AddInParameter(command, "@Product_Name", DbType.String, ObjProductMasterRow.Product_Name);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjProductMasterRow.Company_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjProductMasterRow.Is_Active);

                        db.AddInParameter(command, "@XmlProdCharges", DbType.String, ObjProductMasterRow.XmlProdCharges);                       
                        if (!ObjProductMasterRow.IsXmlAssignValueNull())
                        {
                            db.AddInParameter(command, "@XmlAssignValue", DbType.String, ObjProductMasterRow.XmlAssignValue);
                        }
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjProductMasterRow.Created_By);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjProductMasterRow.Modified_By);
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
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable FunPubQueryProductPaging(SerializationMode SerMode, byte[] bytesObjProductMasterDataTable, out int intTotalRecords, PagingValues ObjPaging)  ///Searching for kali
            {

                intTotalRecords = 0;
                S3GBusEntity.CompanyMgtServices dsProductDetails = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_ProductMaster_ViewDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjProductMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Product_Paging");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                        db.FunPubLoadDataSet(command, dsProductDetails, dsProductDetails.S3G_SYSAD_ProductMaster_View.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsProductDetails.S3G_SYSAD_ProductMaster_View;
            }
            public int FunProductMasterUpdateInt(SerializationMode SerMode, byte[] bytesObjSNXG_Product_MasterDataTable)
            {
                try
                {
                    ObjS3G_ProductMaster_CUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_Product_MasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CUDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Update_Product_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_CURow ObjProductMasterRow in ObjS3G_ProductMaster_CUDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjProductMasterRow.Product_ID);                       
                        db.AddInParameter(command, "@LOB_ID", DbType.String, ObjProductMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_Code", DbType.String, ObjProductMasterRow.Product_Code);
                        db.AddInParameter(command, "@Product_Name", DbType.String, ObjProductMasterRow.Product_Name);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjProductMasterRow.Company_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjProductMasterRow.Is_Active);
                        
                        db.AddInParameter(command, "@XmlProdCharges", DbType.String, ObjProductMasterRow.XmlProdCharges);                      
                        if (!ObjProductMasterRow.IsXmlAssignValueNull())
                        {
                            db.AddInParameter(command, "@XmlAssignValue", DbType.String, ObjProductMasterRow.XmlAssignValue);
                        }                        
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjProductMasterRow.Modified_By);
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
            public int FunProductMasterDeleteInt(SerializationMode SerMode, byte[] bytesObjSNXG_Product_MasterDataTable)
            {
                try
                {
                    ObjS3G_ProductMasterDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_Product_MasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Delete_Product_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMasterRow ObjProductMasterRow in ObjS3G_ProductMasterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjProductMasterRow.Product_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if((int)command.Parameters["@ErrorCode"].Value>0)
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
            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable FunPubQueryProductMasterList(SerializationMode SerMode, byte[] bytesObjSNXG_ProductMasterDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsProductDetails = new S3GBusEntity.CompanyMgtServices();
                ObjS3G_ProductMaster_ViewDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_ProductMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Product_Details");
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewRow ObjProductMasterRow in ObjS3G_ProductMaster_ViewDataTable_DAL.Rows)
                    {
                        
                        if(!ObjProductMasterRow.IsProduct_IDNull()) 
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjProductMasterRow.Product_ID);
                        if(!ObjProductMasterRow.IsCompany_IDNull())
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjProductMasterRow.Company_ID);
                        if(!ObjProductMasterRow.IsLOB_IDNull())
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjProductMasterRow.LOB_ID);
                        if (!ObjProductMasterRow.IsProduct_CodeNull())
                            db.AddInParameter(command, "@Product_Code", DbType.String, ObjProductMasterRow.Product_Code);
                        db.FunPubLoadDataSet(command, dsProductDetails, dsProductDetails.S3G_SYSAD_ProductMaster_View.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsProductDetails.S3G_SYSAD_ProductMaster_View;
            }
            #endregion
        }
            
    }
}
            