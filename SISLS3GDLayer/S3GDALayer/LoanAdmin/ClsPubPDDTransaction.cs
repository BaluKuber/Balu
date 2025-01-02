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

namespace S3GDALayer.LoanAdmin
{
    namespace PDDCMgtServices
    {
        public class ClsPubPDDTransaction
        {
            #region
            int intRowsAffected;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDTLookUpDataTable ObjPDDTLookup_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDCDocumentCategoryDataTable ObjPDDCDocCategory_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDetailsDataTable ObjPDDCTrans_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_GetPRDDCTransDocDetailsDataTable ObjPDDCTransDoc_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable ObjPDDCTransMaster_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PRDDTransDocMasterDataTable ObjPDDCTransDocMaster_Datatable_DAL;
            S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_ExitsPRDDTMasterDataTable ObjPDDCExitsTransMaster_Datatable_DAL;
            #endregion

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPDDTransaction()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            #region Create
            public int FunPubCreatePDDTrans(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_PDDTransSetupDataTable, out string strPDDTNumber_Out)
            {
                strPDDTNumber_Out = string.Empty;
                try
                {

                    ObjPDDCTransMaster_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_PDDTransSetupDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow ObjPDDTransMasterRow in ObjPDDCTransMaster_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertPDDTransDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDDTransMasterRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDDTransMasterRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPDDTransMasterRow.Branch_ID);
                        db.AddInParameter(command, "@PA_SA_REF_ID", DbType.Int32, ObjPDDTransMasterRow.Enquiry_Response_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjPDDTransMasterRow.Customer_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjPDDTransMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@PDDT_Date", DbType.DateTime, ObjPDDTransMasterRow.PRDDC_Date);
                        db.AddInParameter(command, "@Status", DbType.String, ObjPDDTransMasterRow.Status);
                        db.AddInParameter(command, "@PDDC_ID", DbType.Int32, ObjPDDTransMasterRow.PRDDC_ID);
                        db.AddInParameter(command, "@CREATED_BY", DbType.Int32, ObjPDDTransMasterRow.Created_By);
                        db.AddInParameter(command, "@CREATED_ON", DbType.DateTime, ObjPDDTransMasterRow.Created_On);
                        db.AddInParameter(command, "@XML_PDDTDeltails", DbType.String, ObjPDDTransMasterRow.XML_PRDDTDeltails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@PDDT_No", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strPDDTNumber_Out = (string)command.Parameters["@PDDT_No"].Value;
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
                                {
                                    intRowsAffected = 50;
                                }
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

            public int FunPubModifyPDDTrans(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_PDDTransSetupDataTable)
            {
                try
                {

                    ObjPDDCTransMaster_Datatable_DAL = (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_SYSAD_PDDTransSetupDataTable, SerMode, typeof(S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.PRDDCMgtServices.S3G_ORG_PPDDTransMasterRow ObjPDDTransMasterRow in ObjPDDCTransMaster_Datatable_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_UpdatePDTransaDetails");
                        db.AddInParameter(command, "@PostDisbursement_Doc_Tran_ID ", DbType.Int32, ObjPDDTransMasterRow.PreDisbursement_Doc_Tran_ID);
                        db.AddInParameter(command, "@Modified_BY", DbType.Int32, ObjPDDTransMasterRow.Modified_By);
                        db.AddInParameter(command, "@XML_PDDTDeltails", DbType.String, ObjPDDTransMasterRow.XML_PRDDTDeltails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception exp)
                            {
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;

            }

            #endregion
       
        }
    }
}
