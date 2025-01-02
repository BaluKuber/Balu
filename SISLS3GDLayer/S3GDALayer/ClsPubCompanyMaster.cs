#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Company Creation DAL Class
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 10-May-2010
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
    /// Added the Name Space For Logical Grouping
    /// This Class belongs CompanyMgtServices to the service group
    namespace CompanyMgtServices
    {
        public class ClsPubCompanyMaster
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable ObjCompanyMasterCUDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable ObjCompanyMasterDataTable_DAL;
            S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable ObjCompanyMaster_ViewDataTable_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCompanyMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create New Company
            /// <summary>
            /// Creates a new company by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubCreateCompanyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CompanyMasterDataTable)
            {
                try
                {

                    ObjCompanyMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CompanyMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CURow ObjCompanyMasterRow in ObjCompanyMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Insert_Company_Details");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjCompanyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Company_Code", DbType.String, ObjCompanyMasterRow.Company_Code);
                        db.AddInParameter(command, "@Company_Name", DbType.String, ObjCompanyMasterRow.Company_Name);
                        db.AddInParameter(command, "@Address1", DbType.String, ObjCompanyMasterRow.Address1);
                        db.AddInParameter(command, "@Address2", DbType.String, ObjCompanyMasterRow.Address2);
                        db.AddInParameter(command, "@City", DbType.String, ObjCompanyMasterRow.City);
                        db.AddInParameter(command, "@State", DbType.String, ObjCompanyMasterRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, ObjCompanyMasterRow.Country);
                        db.AddInParameter(command, "@Zip_Code", DbType.String, ObjCompanyMasterRow.Zip_Code);
                        db.AddInParameter(command, "@Cons_Status_id", DbType.Int32, ObjCompanyMasterRow.Constitutional_Status_Id);
                        db.AddInParameter(command, "@CD_CEO_Head_Name", DbType.String, ObjCompanyMasterRow.CD_CEO_Head_Name);
                        db.AddInParameter(command, "@CD_Telephone_Number", DbType.String, ObjCompanyMasterRow.CD_Telephone_Number);
                        db.AddInParameter(command, "@CD_Mobile_Number", DbType.String, ObjCompanyMasterRow.CD_Mobile_Number);
                        db.AddInParameter(command, "@CD_Email_ID", DbType.String, ObjCompanyMasterRow.CD_Email_ID);
                        db.AddInParameter(command, "@CD_Website", DbType.String, ObjCompanyMasterRow.CD_Website);
                        db.AddInParameter(command, "@CD_Sys_Admin_User_Code", DbType.String, ObjCompanyMasterRow.CD_Sys_Admin_User_Code);
                        db.AddInParameter(command, "@CD_Sys_Admin_User_Password", DbType.String, ObjCompanyMasterRow.CD_Sys_Admin_User_Password);
                        db.AddInParameter(command, "@OD_Communication_Address", DbType.String, ObjCompanyMasterRow.OD_Communication_Address);
                        db.AddInParameter(command, "@OD_Address1", DbType.String, ObjCompanyMasterRow.OD_Address1);
                        db.AddInParameter(command, "@OD_City", DbType.String, ObjCompanyMasterRow.OD_City);
                        db.AddInParameter(command, "@OD_State", DbType.String, ObjCompanyMasterRow.OD_State);
                        db.AddInParameter(command, "@OD_Country", DbType.String, ObjCompanyMasterRow.OD_Country);
                        db.AddInParameter(command, "@OD_Zip_Code", DbType.String, ObjCompanyMasterRow.OD_Zip_Code);
                        db.AddInParameter(command, "@OD_Date_Of_Incorporation", DbType.DateTime, ObjCompanyMasterRow.OD_Date_Of_Incorporation);
                        db.AddInParameter(command, "@OD_Reg_Lic_Number", DbType.String, ObjCompanyMasterRow.OD_Reg_Lic_Number);
                        db.AddInParameter(command, "@CST_NO", DbType.String, ObjCompanyMasterRow.CST_NO);
                        db.AddInParameter(command, "@ST_NO", DbType.String, ObjCompanyMasterRow.ST_NO);
                        db.AddInParameter(command, "@OD_Validity_Of_Reg_Lic_No", DbType.DateTime, ObjCompanyMasterRow.OD_Validity_Of_Reg_Lic_Number);
                        db.AddInParameter(command, "@OD_Income_Tax_PAN_Number", DbType.String, ObjCompanyMasterRow.OD_Income_Tax_PAN_Number);
                        db.AddInParameter(command, "@Accounting_Currency", DbType.String, ObjCompanyMasterRow.Accounting_Currency);
                        db.AddInParameter(command, "@OD_Remarks", DbType.String, ObjCompanyMasterRow.OD_Remarks);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCompanyMasterRow.is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCompanyMasterRow.Create_By);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCompanyMasterRow.Modified_By);
                        db.AddInParameter(command, "@XML_VATTIN", DbType.String, ObjCompanyMasterRow.XML_VATTIN);
                        db.AddInParameter(command, "@CGSTIN", DbType.String, ObjCompanyMasterRow.CGSTIN);
                        db.AddInParameter(command, "@CGST_Reg_date", DbType.DateTime, ObjCompanyMasterRow.CGST_Reg_Date);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                //intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                        
                            
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

            #region Modify Comapny Details
            /// <summary>
            /// Modifies an Exsisting company by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubModifyCompanyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CompanyMasterDataTable)
            {
                try
                {

                    ObjCompanyMasterCUDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CompanyMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CUDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_CURow ObjCompanyMasterRow in ObjCompanyMasterCUDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Update_Company_Details");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjCompanyMasterRow.Company_ID);
                        db.AddInParameter(command, "@Company_Code", DbType.String, ObjCompanyMasterRow.Company_Code);
                        db.AddInParameter(command, "@Company_Name", DbType.String, ObjCompanyMasterRow.Company_Name);
                        db.AddInParameter(command, "@Address1", DbType.String, ObjCompanyMasterRow.Address1);
                        db.AddInParameter(command, "@Address2", DbType.String, ObjCompanyMasterRow.Address2);
                        db.AddInParameter(command, "@City", DbType.String, ObjCompanyMasterRow.City);
                        db.AddInParameter(command, "@State", DbType.String, ObjCompanyMasterRow.State);
                        db.AddInParameter(command, "@Country", DbType.String, ObjCompanyMasterRow.Country);
                        db.AddInParameter(command, "@Zip_Code", DbType.String, ObjCompanyMasterRow.Zip_Code);
                        db.AddInParameter(command, "@Cons_Status_id", DbType.Int32, ObjCompanyMasterRow.Constitutional_Status_Id);
                        db.AddInParameter(command, "@CD_CEO_Head_Name", DbType.String, ObjCompanyMasterRow.CD_CEO_Head_Name);
                        db.AddInParameter(command, "@CD_Telephone_Number", DbType.String, ObjCompanyMasterRow.CD_Telephone_Number);
                        db.AddInParameter(command, "@CD_Mobile_Number", DbType.String, ObjCompanyMasterRow.CD_Mobile_Number);
                        db.AddInParameter(command, "@CD_Email_ID", DbType.String, ObjCompanyMasterRow.CD_Email_ID);
                        db.AddInParameter(command, "@CD_Website", DbType.String, ObjCompanyMasterRow.CD_Website);
                        db.AddInParameter(command, "@CD_Sys_Admin_User_Code", DbType.String, ObjCompanyMasterRow.CD_Sys_Admin_User_Code);
                        db.AddInParameter(command, "@CST_NO", DbType.String, ObjCompanyMasterRow.CST_NO);
                        db.AddInParameter(command, "@ST_NO", DbType.String, ObjCompanyMasterRow.ST_NO);
                        db.AddInParameter(command, "@CD_Sys_Admin_User_Password", DbType.String, ObjCompanyMasterRow.CD_Sys_Admin_User_Password);
                        db.AddInParameter(command, "@OD_Communication_Address", DbType.String, ObjCompanyMasterRow.OD_Communication_Address);
                        db.AddInParameter(command, "@OD_Address1", DbType.String, ObjCompanyMasterRow.OD_Address1);
                        db.AddInParameter(command, "@OD_City", DbType.String, ObjCompanyMasterRow.OD_City);
                        db.AddInParameter(command, "@OD_State", DbType.String, ObjCompanyMasterRow.OD_State);
                        db.AddInParameter(command, "@OD_Country", DbType.String, ObjCompanyMasterRow.OD_Country);
                        db.AddInParameter(command, "@OD_Zip_Code", DbType.String, ObjCompanyMasterRow.OD_Zip_Code);
                        db.AddInParameter(command, "@OD_Date_Of_Incorporation", DbType.DateTime, ObjCompanyMasterRow.OD_Date_Of_Incorporation);
                        db.AddInParameter(command, "@OD_Reg_Lic_Number", DbType.String, ObjCompanyMasterRow.OD_Reg_Lic_Number);
                        db.AddInParameter(command, "@OD_Validity_Of_Reg_Lic_No", DbType.DateTime, ObjCompanyMasterRow.OD_Validity_Of_Reg_Lic_Number);
                        db.AddInParameter(command, "@OD_Income_Tax_PAN_Number", DbType.String, ObjCompanyMasterRow.OD_Income_Tax_PAN_Number);
                        db.AddInParameter(command, "@Accounting_Currency", DbType.String, ObjCompanyMasterRow.Accounting_Currency);
                        db.AddInParameter(command, "@OD_Remarks", DbType.String, ObjCompanyMasterRow.OD_Remarks);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCompanyMasterRow.is_Active);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCompanyMasterRow.Modified_By);
                        db.AddInParameter(command, "@XML_VATTIN", DbType.String, ObjCompanyMasterRow.XML_VATTIN);
                        if (!ObjCompanyMasterRow.IsCGSTINNull())
                            db.AddInParameter(command, "@CGSTIN", DbType.String, ObjCompanyMasterRow.CGSTIN);
                        if (!ObjCompanyMasterRow.IsCGST_Reg_DateNull())
                            db.AddInParameter(command, "@CGST_Reg_date", DbType.DateTime, ObjCompanyMasterRow.CGST_Reg_Date);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }
            #endregion

            #region Delete Exsisting Company
            /// <summary>
            /// Deletes an Exsisting company by getting Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_SYSAD_CompanyMasterDataTable"></param>
            /// <returns>Error Code (it is 0 if no error is found)</returns>
            public int FunPubDeleteCompanyInt(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CompanyMasterDataTable)
            {
                try
                {

                    ObjCompanyMasterDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CompanyMasterDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterRow ObjCompanyMasterRow in ObjCompanyMasterDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Delete_Company_Details");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjCompanyMasterRow.Company_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }
            #endregion

            #region Query Company Details
            /// <summary>
            /// Gets a company details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_ProductMasterDataTable"></param>
            /// <returns>Datatable containing Company details</returns>

            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable FunPubQueryCompanyMasterList(SerializationMode SerMode, byte[] bytesObjS3G_SYSAD_CompanyMaster_ViewDataTable)
            {
                S3GBusEntity.CompanyMgtServices dsComapanyDetails = new S3GBusEntity.CompanyMgtServices();
                ObjCompanyMaster_ViewDataTable_DAL = (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_SYSAD_CompanyMaster_ViewDataTable, SerMode, typeof(S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    
                    foreach (S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMaster_ViewRow ObjComapnyMasterRow in ObjCompanyMaster_ViewDataTable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Get_Company_Details");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjComapnyMasterRow.Company_ID);
                        db.FunPubLoadDataSet(command, dsComapanyDetails, dsComapanyDetails.S3G_SYSAD_CompanyMaster_View.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsComapanyDetails.S3G_SYSAD_CompanyMaster_View;
            }
            #endregion

            #region Get Company List
            /// <summary>
            /// Gets List of Company Name and Code using Comapny id as Input
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjSNXG_ProductMasterDataTable"></param>
            /// <returns>Datatable containing Company List</returns>

            public S3GBusEntity.CompanyMgtServices.S3G_SYSAD_CompanyMasterDataTable FunPubGetCompanyList(int CompanyId)
            {
                S3GBusEntity.CompanyMgtServices dsComapanyDetails = new S3GBusEntity.CompanyMgtServices();

                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_CompanyMaster_List");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, CompanyId);
                    db.FunPubLoadDataSet(command, dsComapanyDetails, dsComapanyDetails.S3G_SYSAD_CompanyMaster.TableName);

                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return dsComapanyDetails.S3G_SYSAD_CompanyMaster;
            }
            #endregion

            /// <summary>
            /// Gets branch details using Serialized data table object and serialized mode
            /// </summary>
            /// <returns>Datatable containing role Details</returns>

            public void FunPubQueryCompanyName(int intCompanyID, out string strCompanyCode)
            {
               try
                {
                    strCompanyCode = null;
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_CompanyCode");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    db.AddOutParameter(command, "@Company_Code", DbType.String,sizeof(Int32));
                    db.FunPubExecuteNonQuery(command);
                    if ((string)command.Parameters["@Company_Code"].Value !=null)
                        strCompanyCode = (string)command.Parameters["@Company_Code"].Value;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
               

            }
        }
    }
}
