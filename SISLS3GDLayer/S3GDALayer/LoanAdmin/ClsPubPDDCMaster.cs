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

namespace S3GDALayer.LoanAdmin
{
    namespace PDDCMgtServices
    {
        public class ClsPubPDDCMaster
        {
            int intRowsAffected;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable ObjPDDCMaster_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPDDCMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreatePDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable)
            {
                try
                {

                    ObjPDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCMasterRow in ObjPDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPDDCDetails");
                        db.AddInParameter(command, "@PDDC_ID", DbType.Int32, ObjPDDCMasterRow.PRDDC_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDDCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPDDCMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPDDCMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@Document_Path", DbType.String, ObjPDDCMasterRow.DocPath);
                        
                        db.AddInParameter(command, "@XMLPDDCDocumentsDet", DbType.String, ObjPDDCMasterRow.XMLPRDDCDocumentsDet);
                        
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPDDCMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                               // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());

                                }

                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }

                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                else if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                        //db.ExecuteNonQuery(command);

                        //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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

            public int FunPubCreateOtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable)
            {
                try
                {

                    ObjPDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCMasterRow in ObjPDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertOtherDocDetails");
                        db.AddInParameter(command, "@DocType", DbType.String, ObjPDDCMasterRow.DocType);
                        db.AddInParameter(command, "@DocCategory", DbType.String, ObjPDDCMasterRow.DocCategory);
                        db.AddInParameter(command, "@Description", DbType.String, ObjPDDCMasterRow.DocDesc);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPDDCMasterRow.Created_By);
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

            public DataSet FunPubQueryPDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable)
            {
                DataSet dsPRDDC = new DataSet();
                ObjPDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_PDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPDDCDocumentsDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCMasterRow in ObjPDDCMaster_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@PDDC_ID", DbType.Int32, ObjPDDCMasterRow.PRDDC_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjPDDCMasterRow.Created_By);
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

            #endregion

            #region Query PDDC Details
            /// <summary>
            /// Gets a PRDDC details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns>Datatable containing PRDDC Details</returns>

            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable FunPubQueryPDDCMaster(SerializationMode SerMode, byte[] bytesObjSNXG_PDDCMasterDataTable)
            {
                S3GBusEntity.Origination.PRDDCMgtServices dsPDDCDetails = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_PDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCMasterRow in ObjPDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPDDCDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@PDDC_ID", DbType.Int32, ObjPDDCMasterRow.PRDDC_ID);
                        //db.LoadDataSet(command, dsPDDCDetails, dsPDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                        db.FunPubLoadDataSet(command, dsPDDCDetails, dsPDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsPDDCDetails.S3G_ORG_PRDDCMaster;
            }

            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable FunPubQueryPDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjSNXG_PDDCMasterDataTable)
            {
                S3GBusEntity.Origination.PRDDCMgtServices dsPDDCDetails = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPDDCMaster_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_PDDCMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCMasterRow ObjPDDCMasterRow in ObjPDDCMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetPDDCCombinationDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDDCMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDDCMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjPDDCMasterRow.Product_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPDDCMasterRow.Constitution_ID);
                       // db.LoadDataSet(command, dsPDDCDetails, dsPDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                        db.FunPubLoadDataSet(command, dsPDDCDetails, dsPDDCDetails.S3G_ORG_PRDDCMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsPDDCDetails.S3G_ORG_PRDDCMaster;
            }
            #endregion
              

        }
    }
}
