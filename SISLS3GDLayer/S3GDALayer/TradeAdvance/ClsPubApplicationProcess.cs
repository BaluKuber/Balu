#region Page Header

/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Trade Advance
/// Screen Name			: Application Processing
/// Created By			: Thangam M
/// Start Date		    : 23-Nov-2011
/// End Date		    : 
/// Purpose	            : To get Application Details [Trade Advance]
/// Modified By         : 
/// Modified Date       : 

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

namespace S3GDALayer.TradeAdvance
{
    namespace TradeAdvanceMgtServices
    {
        public class ClsPubApplicationProcess
        {
            int intRowsAffected;
            S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessDataTable ObjApplicationProcess_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubApplicationProcess()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable, out string strApplilcationNumber)
            {
                try
                {
                    strApplilcationNumber = "";
                    
                    ObjApplicationProcess_DAL = (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_TA_ApplicationProcDataTable, SerMode, typeof(S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessDataTable));

                    foreach (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessRow ObjApplicationRow in ObjApplicationProcess_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertApplicationProcess");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApplicationRow.Company_ID);
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjApplicationRow.Entity_ID);
                        db.AddInParameter(command, "@Application_Date", DbType.DateTime, ObjApplicationRow.Application_Date);
                        db.AddInParameter(command, "@Finance_Amount", DbType.Decimal, ObjApplicationRow.Finance_Amount);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApplicationRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjApplicationRow.Created_On);
                        db.AddInParameter(command, "@XMLReqDetail", DbType.String, ObjApplicationRow.XMLReqDtls);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ApplicationNumber", DbType.String, 100);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                strApplilcationNumber = (string)command.Parameters["@ApplicationNumber"].Value;
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined for Application");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit for Application");
                                    if (intRowsAffected == -3)
                                        throw new Exception("Document Sequence no not-defined for Tranche");
                                    if (intRowsAffected == -4)
                                        throw new Exception("Document Sequence no exceeds defined limit for Tranche");
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

            public int FunPubApproveApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable)
            {
                try
                {
                    
                    ObjApplicationProcess_DAL = (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_TA_ApplicationProcDataTable, SerMode, typeof(S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessDataTable));

                    foreach (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.S3G_TA_AppProcessRow ObjApplicationRow in ObjApplicationProcess_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertApplicationApproval");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjApplicationRow.Company_ID);
                        db.AddInParameter(command, "@Application_ID", DbType.Int32, ObjApplicationRow.Application_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjApplicationRow.Created_By);

                        if (!ObjApplicationRow.IsXMLReqDtlsNull())
                        {
                            db.AddInParameter(command, "@XMLReqDetail", DbType.String, ObjApplicationRow.XMLReqDtls);
                        }

                        if (!ObjApplicationRow.IsXMLApprovalDtlsNull())
                        {
                            db.AddInParameter(command, "@XMLApprovalDtls", DbType.String, ObjApplicationRow.XMLApprovalDtls);
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
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined for Application");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit for Application");
                                    if (intRowsAffected == -3)
                                        throw new Exception("Document Sequence no not-defined for Tranche");
                                    if (intRowsAffected == -4)
                                        throw new Exception("Document Sequence no exceeds defined limit for Tranche");
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
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }
        }
    }
}
