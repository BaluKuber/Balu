#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDC Master
/// Created By			: Kaliraj Y
/// Created Date		: 01-Jun-2010
/// Purpose	            : 

/// <Program Summary>
#endregion
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;
using System.Data.OracleClient;

namespace S3GDALayer.Origination
{
    namespace PRDDCMgtServices
    {
        public class ClsPubPRDDCMaster
        {
            int intRowsAffected;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjPRDDCMaster_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable ObjFIRMaster_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterDataTable ObjlegalMaster_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPRDDCMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreatePRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
            {
                try
                {

                    ObjPRDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PRDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCMasterRow in ObjPRDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertPRDDCDetails");
                        db.AddInParameter(command, "@PRDDC_ID", DbType.Int32, ObjPRDDCMasterRow.PRDDC_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPRDDCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPRDDCMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPRDDCMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@Document_Path", DbType.String, ObjPRDDCMasterRow.DocPath);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLPRDDCDocumentsDet",
                                   OracleType.Clob, ObjPRDDCMasterRow.XMLPRDDCDocumentsDet.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjPRDDCMasterRow.XMLPRDDCDocumentsDet);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLPRDDCDocumentsDet", DbType.String, ObjPRDDCMasterRow.XMLPRDDCDocumentsDet);
                        }
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPRDDCMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);


                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
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

            /// <summary>
            /// To Create FIR Doc Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_FIRMasterDataTable"></param>
            /// <returns></returns>

            public int FunPubCreateFIRDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
            {
                try
                {

                    ObjFIRMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterRow ObjFIRMasterRow in ObjFIRMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertFIRDetails");
                        db.AddInParameter(command, "@FIR_ID", DbType.Int32, ObjFIRMasterRow.FIR_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFIRMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjFIRMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjFIRMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjFIRMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@Document_Path", DbType.String, ObjFIRMasterRow.DocPath);
                        

                         S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                         if (enumDBType == S3GDALDBType.ORACLE)
                         {
                             OracleParameter param = new OracleParameter("@XMLFIRDocumentsDet",
                                    OracleType.Clob, ObjFIRMasterRow.XMLFIRDocumentDet.Length,
                                    ParameterDirection.Input, true, 0, 0, String.Empty,
                                    DataRowVersion.Default, ObjFIRMasterRow.XMLFIRDocumentDet);
                             command.Parameters.Add(param);
                         }
                         else
                         {
                             db.AddInParameter(command, "@XMLFIRDocumentsDet", DbType.String, ObjFIRMasterRow.XMLFIRDocumentDet);
                         }
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjFIRMasterRow.Created_By);
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

            public int FunPubCreatelegalDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable)
            {
                try
                {

                    ObjlegalMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_LegalMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterRow ObjlegalMasterRow in ObjlegalMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertlegalDetails");
                        db.AddInParameter(command, "@legal_ID", DbType.Int32, ObjlegalMasterRow.legal_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjlegalMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjlegalMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjlegalMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjlegalMasterRow.Constitution_ID);
                     
                        db.AddInParameter(command, "@Document_Path", DbType.String, ObjlegalMasterRow.DocPath);

                        //db.AddInParameter(command, "@XMLlegalDocumentsDet", DbType.String, ObjlegalMasterRow.XMLlegalDocumentDet);
                        db.AddInParameter(command, "@type", DbType.String, ObjlegalMasterRow.Type);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLlegalDocumentsDet",
                                   OracleType.Clob, ObjlegalMasterRow.XMLlegalDocumentDet.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjlegalMasterRow.XMLlegalDocumentDet);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLlegalDocumentsDet", DbType.String, ObjlegalMasterRow.XMLlegalDocumentDet);
                        }
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjlegalMasterRow.Created_By);
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
            //END
            public int FunPubCreateOtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
            {
                try
                {

                    ObjPRDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PRDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCMasterRow in ObjPRDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertOtherDocDetails");
                        db.AddInParameter(command, "@DocType", DbType.String, ObjPRDDCMasterRow.DocType);
                        db.AddInParameter(command, "@DocCategory", DbType.String, ObjPRDDCMasterRow.DocCategory);
                        db.AddInParameter(command, "@Description", DbType.String, ObjPRDDCMasterRow.DocDesc);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPRDDCMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
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

            public int FunPubCreateFIROtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
            {
                try
                {

                    ObjFIRMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterRow ObjFIRMasterRow in ObjFIRMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertFIROtherDocDetails");
                        db.AddInParameter(command, "@DocType", DbType.String, ObjFIRMasterRow.DocType);
                        db.AddInParameter(command, "@DocCategory", DbType.String, ObjFIRMasterRow.DocCategory);
                        db.AddInParameter(command, "@Description", DbType.String, ObjFIRMasterRow.DocDesc);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjFIRMasterRow.Created_By);
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
            #region Query PRDDC Details
            /// <summary>
            /// Gets a PRDDC details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns>Datatable containing PRDDC LOB and Documents Details</returns>

            public DataSet FunPubQueryPRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable)
            {


                DataSet dsPRDDC = new DataSet();
                ObjPRDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PRDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCDocumentsDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCMasterRow in ObjPRDDCMaster_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@PRDDC_ID", DbType.Int32, ObjPRDDCMasterRow.PRDDC_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPRDDCMasterRow.Created_By);
                        //dsPRDDC = db.ExecuteDataSet(command);
                        dsPRDDC = db.FunPubExecuteDataSet(command);
                    }
                    return dsPRDDC;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dsPRDDC.Dispose();
                    dsPRDDC = null;
                }

            }


            /// <summary>
            /// To Get FIR Doc Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_FIRMasterDataTable"></param>
            /// <returns></returns>
            public DataSet FunPubQuerylegalDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable)
            {
                DataSet dsPRDDC = new DataSet();
                ObjlegalMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_LegalMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetlegalDocumentsDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_LegalMasterRow ObjLegalMasterRow in ObjlegalMaster_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLegalMasterRow.Company_ID);
                        db.AddInParameter(command, "@legal_ID", DbType.Int32, ObjLegalMasterRow.legal_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjLegalMasterRow.Created_By);
                        db.AddInParameter(command, "@type", DbType.String, ObjLegalMasterRow.Type);
                        //dsPRDDC = db.ExecuteDataSet(command);
                        dsPRDDC = db.FunPubExecuteDataSet(command);
                    }
                    return dsPRDDC;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dsPRDDC.Dispose();
                    dsPRDDC = null;
                }

            }
            

            public DataSet FunPubQueryFIRDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable)
            {
                DataSet dsPRDDC = new DataSet();
                ObjFIRMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetFIRDocumentsDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_FIRMasterRow ObjFIRMasterRow in ObjFIRMaster_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFIRMasterRow.Company_ID);
                        db.AddInParameter(command, "@FIR_ID", DbType.Int32, ObjFIRMasterRow.FIR_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjFIRMasterRow.Created_By);
                        //dsPRDDC = db.ExecuteDataSet(command);
                        dsPRDDC = db.FunPubExecuteDataSet(command);
                    }
                    return dsPRDDC;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dsPRDDC.Dispose();
                    dsPRDDC = null;
                }

            }
            //END
            #endregion

            #region Query PRDDC Details
            /// <summary>
            /// Gets a PRDDC details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns>Datatable containing PRDDC Details</returns>

            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable FunPubQueryPRDDCMaster(SerializationMode SerMode, byte[] bytesObjSNXG_PRDDCMasterDataTable)
            {
                S3GBusEntity.Origination.PRDDCMgtServices dsPRDDCDetails = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_PRDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCMasterRow in ObjPRDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@PRDDC_ID", DbType.Int32, ObjPRDDCMasterRow.PRDDC_ID);
                        //db.LoadDataSet(command, dsPRDDCDetails, dsPRDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                        db.FunPubLoadDataSet(command, dsPRDDCDetails, dsPRDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsPRDDCDetails.S3G_ORG_PRDDCMaster;
            }

            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable FunPubQueryPRDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjSNXG_PRDDCMasterDataTable)
            {
                S3GBusEntity.Origination.PRDDCMgtServices dsPRDDCDetails = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_PRDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPRDDCMasterRow in ObjPRDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCCombinationDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPRDDCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPRDDCMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPRDDCMasterRow.Constitution_ID);
                        //db.LoadDataSet(command, dsPRDDCDetails, dsPRDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                        db.FunPubLoadDataSet(command, dsPRDDCDetails, dsPRDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsPRDDCDetails.S3G_ORG_PRDDCMaster;
            }
            #endregion
              

        }
    }
}
