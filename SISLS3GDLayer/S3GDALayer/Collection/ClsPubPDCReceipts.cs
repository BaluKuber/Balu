#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: PDC
/// Created By			: Kannan RC
/// Created Date		: 05-Oct-2010
/// Purpose	            : 
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
using System.Data.OracleClient;
using S3GBusEntity.Collection;

#endregion

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {

        public class ClsPubPDCReceipts
        {
            #region Initialization

            //S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsDataTable objPDDMaster_DAL = null;
            //S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptDataTable objPDDBulk_DAL = null;

            int intRowsAffected;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptDataTable objPDCBulkRec_DAL;
            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptRow ObjPDCBulkRecRow = null;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPDCReceipts()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            //#region Get PDC

            //public S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsDataTable FunPubGetPDC(SerializationMode SerMode, byte[] bytesObjS3G_CLN_PDCMasterDataTable)
            //{
            //    S3GBusEntity.Collection.ClnReceiptMgtServices ObjPDCDetils = new S3GBusEntity.Collection.ClnReceiptMgtServices(); 
            //    objPDDMaster_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_PDCMasterDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsDataTable));
            //    try
            //    {
            //        Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            //        DbCommand command = db.GetStoredProcCommand("S3G_CLN_GetPDCDetails");
            //        foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_GetPDCDetailsRow ObjAcctDescMasterRow in objPDDMaster_DAL.Rows)
            //        {
            //            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAcctDescMasterRow.Company_ID);
            //            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAcctDescMasterRow.LOB_ID);
            //            db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjAcctDescMasterRow.Branch_ID);
            //            db.AddInParameter(command, "@PDC_Entry_Date", DbType.DateTime, ObjAcctDescMasterRow.PDC_Entry_Date);
            //            db.AddInParameter(command, "@Include", DbType.Boolean, ObjAcctDescMasterRow.Include);
            //            db.LoadDataSet(command, ObjPDCDetils, ObjPDCDetils.S3G_CLN_GetPDCDetails.TableName);
            //        }

            //    }
            //    catch (Exception exp)
            //    {
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
            //    }
            //    return ObjPDCDetils.S3G_CLN_GetPDCDetails;

            //}
            //#endregion


            #region Create PDC
            public int FunPubCreatePDCBulkReceiptProcess(SerializationMode SerMode, byte[] bytesobjS3G_cln_PDCDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objPDCBulkRec_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptDataTable)ClsPubSerialize.DeSerialize(bytesobjS3G_cln_PDCDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptDataTable));

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_PDCBulkReceiptRow ObjPDCRow in objPDCBulkRec_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertPDCBulkReceipts");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPDCRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjPDCRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjPDCRow.Branch_ID);
                        db.AddInParameter(command, "@PDCDate", DbType.DateTime, ObjPDCRow.PDCDate);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPDCRow.Created_By);
                        //db.AddInParameter(command, "@XML_ReceiptsDeltails", DbType.String, ObjPDCRow.XML_ReceiptsDeltails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XML_ReceiptsDeltails", OracleType.Clob,
                                ObjPDCRow.XML_ReceiptsDeltails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjPDCRow.XML_ReceiptsDeltails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XML_ReceiptsDeltails", DbType.String,
                                ObjPDCRow.XML_ReceiptsDeltails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //  throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    // if (intRowsAffected == -1)
                                    //   throw new Exception("Document Sequence no not-defined");
                                    //  if (intRowsAffected == -2)
                                    // throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            { conn.Close(); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (intRowsAffected == 0)
                        intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }
            #endregion

        }
    }
}
