#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: ORG
/// Screen Name			: PRDDT Creation DAL Class
/// Created By			: Kannan RC
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// Last Updated By		: Kannan RC
/// Last Updated Date   : 
/// Reason              : Orgination Module Developement
/// <Program Summary>
#endregion

#region Namespaces
using System;
using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
#endregion

namespace S3GDALayer.Origination
{
    namespace PRDDCMgtServices
    {

        public class ClsPubPRDDTMaster
        {
            #region
            int intRowsAffected;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable ObjPRDDTLookup_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable ObjPRDDCDocCategory_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable ObjPRDDCTrans_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable ObjPRDDCTransDoc_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable ObjPRDDCTransMaster_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable ObjPRDDCTransDocMaster_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable ObjPRDDCExitsTransMaster_Datatable_DAL;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPRDDTMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Lookup
            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable FunPubPRDDTLookup(SerializationMode Sermode, byte[] bytesPRDDTLookup)
            {
                S3GBusEntity.Origination.PRDDCMgtServices DsPRDDT = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDTLookup_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable)ClsPubSerialize.DeSerialize(bytesPRDDTLookup, Sermode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCLookUp");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpRow ObjPRDDTRow in ObjPRDDTLookup_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjPRDDTRow.Lookup_ID);
                        db.AddInParameter(command, "@Param1", DbType.String, ObjPRDDTRow.Type);
                    }
                    //db.LoadDataSet(command, DsPRDDT, DsPRDDT.S3G_ORG_GetPRDDTLookUp.TableName);
                    db.FunPubLoadDataSet(command, DsPRDDT, DsPRDDT.S3G_ORG_GetPRDDTLookUp.TableName);
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsPRDDT.S3G_ORG_GetPRDDTLookUp;
            }


            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable FunPubGetTypeTrans(SerializationMode Sermode, byte[] bytesPRDDTLookup)
            {
                S3GBusEntity.Origination.PRDDCMgtServices DsPRDDType = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCDocCategory_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable)ClsPubSerialize.DeSerialize(bytesPRDDTLookup, Sermode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDTLookup");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryRow ObjPRDDocCategoryRow in ObjPRDDCDocCategory_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPRDDocCategoryRow.LOB_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPRDDocCategoryRow.Constitution_ID);
                        db.AddInParameter(command, "@COMPANY_ID", DbType.Int32, ObjPRDDocCategoryRow.Company_ID);
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjPRDDocCategoryRow.Option);
                        db.AddInParameter(command, "@Ref_ID", DbType.Int32, ObjPRDDocCategoryRow.Ref_ID);
                        db.AddInParameter(command, "@PRDDC_ID", DbType.Int32, ObjPRDDocCategoryRow.PRDDC_ID);
                        //db.LoadDataSet(command, DsPRDDType, DsPRDDType.S3G_ORG_PRDDCDocumentCategory.TableName);
                        db.FunPubLoadDataSet(command, DsPRDDType, DsPRDDType.S3G_ORG_PRDDCDocumentCategory.TableName);
                    }

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsPRDDType.S3G_ORG_PRDDCDocumentCategory;
            }
            #endregion

            #region Get PRDDT
            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable FunPubGetPRDDTrans(SerializationMode Sermode, byte[] bytesPRDDTrans)
            {
                S3GBusEntity.Origination.PRDDCMgtServices DsPRDDTrans = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCTrans_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable)ClsPubSerialize.DeSerialize(bytesPRDDTrans, Sermode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCTransDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsRow ObjPRDDTransRow in ObjPRDDCTrans_Datatable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTransRow.Company_ID);
                        db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID", DbType.Int32, ObjPRDDTransRow.PreDisbursement_Doc_Tran_ID);
                        //db.LoadDataSet(command, DsPRDDTrans, DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails.TableName);
                        db.FunPubLoadDataSet(command, DsPRDDTrans, DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails.TableName);
                    }

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails;
            }

            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable FunPubGetPRDDTransDoc(SerializationMode Sermode, byte[] bytesPRDDTransDoc)
            {
                S3GBusEntity.Origination.PRDDCMgtServices DsPRDDTransDoc = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCTransDoc_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable)ClsPubSerialize.DeSerialize(bytesPRDDTransDoc, Sermode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDCTransDocDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsRow ObjPRDDTransDocRow in ObjPRDDCTransDoc_Datatable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID", DbType.Int32, ObjPRDDTransDocRow.PreDisbursement_Doc_Tran_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTransDocRow.Company_ID);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, ObjPRDDTransDocRow.Enquiry_Response_ID);
                        //db.LoadDataSet(command, DsPRDDTransDoc, DsPRDDTransDoc.S3G_ORG_GetPRDDCTransDocDetails.TableName);
                        db.FunPubLoadDataSet(command, DsPRDDTransDoc, DsPRDDTransDoc.S3G_ORG_GetPRDDCTransDocDetails.TableName);
                    }

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsPRDDTransDoc.S3G_ORG_GetPRDDCTransDocDetails;
            }



            public S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable FunPubGetPRDDTransPaging(SerializationMode Sermode, byte[] bytesPRDDTrans, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.Origination.PRDDCMgtServices DsPRDDTrans = new S3GBusEntity.Origination.PRDDCMgtServices();
                ObjPRDDCTrans_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable)ClsPubSerialize.DeSerialize(bytesPRDDTrans, Sermode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetPRDDTPaging");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsRow ObjPRDDTransRow in ObjPRDDCTrans_Datatable_DAL.Rows)
                    {

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTransRow.Company_ID);
                        db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID", DbType.Int32, ObjPRDDTransRow.PreDisbursement_Doc_Tran_ID);
                        db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                        db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                        db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                        db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                        db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));
                        //db.LoadDataSet(command, DsPRDDTrans, DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails.TableName);
                        db.FunPubLoadDataSet(command, DsPRDDTrans, DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails.TableName);
                        if ((int)command.Parameters["@TotalRecords"].Value > 0)
                            intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                    }

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return DsPRDDTrans.S3G_ORG_GetPRDDCTransDetails;
            }


            #endregion

            #region Create
            public int FunPubCreatePRDDTrans(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_PRDDTransSetupDataTable, out string strPRDDTNumber_Out)
            {
                strPRDDTNumber_Out = string.Empty;
                try
                {

                    ObjPRDDCTransMaster_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_PRDDTransSetupDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow ObjPRDDTransMasterRow in ObjPRDDCTransMaster_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertPRDDTransDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTransMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPRDDTransMasterRow.LOB_ID);
                        if (!ObjPRDDTransMasterRow.IsBranch_IDNull())
                        {
                            db.AddInParameter(command, "@Location_Id", DbType.Int32, ObjPRDDTransMasterRow.Branch_ID);
                        }
                        db.AddInParameter(command, "@Document_Type", DbType.Int32, ObjPRDDTransMasterRow.Document_Type);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, ObjPRDDTransMasterRow.Enquiry_Response_ID);
                        if (!ObjPRDDTransMasterRow.IsCustomer_IDNull())
                        {
                            db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPRDDTransMasterRow.Customer_ID);
                        }
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPRDDTransMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@PRDDC_Date", DbType.DateTime, ObjPRDDTransMasterRow.PRDDC_Date);
                        db.AddInParameter(command, "@Status", DbType.String, ObjPRDDTransMasterRow.Status);
                        db.AddInParameter(command, "@PRDDC_ID", DbType.Int32, ObjPRDDTransMasterRow.PRDDC_ID);
                        // db.AddInParameter(command, "@PRDDC_Number", DbType.String, ObjPRDDTransMasterRow.PRDDC_Number);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjPRDDTransMasterRow.Created_By);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjPRDDTransMasterRow.Created_On);
                        //db.AddInParameter(command, "@XML_PRDDTDeltails", DbType.String, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            if (ObjPRDDTransMasterRow.XML_PRDDTDeltails != null)
                            {
                                OracleParameter param = new OracleParameter("@XML_PRDDTDeltails",
                                       OracleType.Clob, ObjPRDDTransMasterRow.XML_PRDDTDeltails.Length,
                                       ParameterDirection.Input, true, 0, 0, String.Empty,
                                       DataRowVersion.Default, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                                command.Parameters.Add(param);
                            }
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_PRDDTDeltails", DbType.String, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                        }

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@PRDDT_No", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strPRDDTNumber_Out = (string)command.Parameters["@PRDDT_No"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

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

            #region Modify
            public int FunPubModifyPRDDTrans(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_PRDDTransSetupDataTable)
            {
                try
                {

                    ObjPRDDCTransMaster_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_PRDDTransSetupDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow ObjPRDDTransMasterRow in ObjPRDDCTransMaster_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdatePRDTransaDetails");
                        db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID ", DbType.Int32, ObjPRDDTransMasterRow.PreDisbursement_Doc_Tran_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTransMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPRDDTransMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPRDDTransMasterRow.Branch_ID);
                        db.AddInParameter(command, "@Document_Type", DbType.Int32, ObjPRDDTransMasterRow.Document_Type);
                        db.AddInParameter(command, "@Document_Type_ID", DbType.Int32, ObjPRDDTransMasterRow.Enquiry_Response_ID);
                        if (!ObjPRDDTransMasterRow.IsCustomer_IDNull())
                        {
                            db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPRDDTransMasterRow.Customer_ID);
                        }
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPRDDTransMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@PRDDC_Date", DbType.DateTime, ObjPRDDTransMasterRow.PRDDC_Date);
                        db.AddInParameter(command, "@Status", DbType.String, ObjPRDDTransMasterRow.Status);
                        db.AddInParameter(command, "@PRDDC_Number", DbType.String, ObjPRDDTransMasterRow.PRDDC_Number);
                        db.AddInParameter(command, "@Modified_BY", DbType.Int32, ObjPRDDTransMasterRow.Modified_By);
                        db.AddInParameter(command, "@Modified_ON", DbType.DateTime, ObjPRDDTransMasterRow.Modified_On);
                        //db.AddInParameter(command, "@XML_PRDDTDeltails", DbType.String, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_PRDDTDeltails",
                                   OracleType.Clob, ObjPRDDTransMasterRow.XML_PRDDTDeltails.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_PRDDTDeltails", DbType.String, ObjPRDDTransMasterRow.XML_PRDDTDeltails);
                        }

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
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

            #region Delete Cashflow
            public int FunPubExitsPRDDTrans(SerializationMode SerMode, byte[] bytesObjS3G_ORG_DeletePRDDTMasterDataTable)
            {
                try
                {

                    ObjPRDDCExitsTransMaster_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_DeletePRDDTMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_ExitsPRDDTDetails");
                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterRow ObjPRDDTMasterRow in ObjPRDDCExitsTransMaster_Datatable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPRDDTMasterRow.Company_ID);
                        db.AddInParameter(command, "@Document_Type", DbType.Int32, ObjPRDDTMasterRow.Document_Type);
                        db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID", DbType.Int32, ObjPRDDTMasterRow.PreDisbursement_Doc_Tran_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
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
        }
    }
}
