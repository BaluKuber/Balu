#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: Manual Journal
/// Created By			: Kaliraj K
/// Created Date		: 07-Sep-2010
/// Purpose	            : To access and Insert Manual Journal Details

/// <Program Summary>
#endregion
using System;
using S3GDALayer.S3GAdminServices;
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
namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubManualJournal
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable ObjMJV_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubManualJournal()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateManualJournal(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_ManualJournalDataTable, out string strMJVNumber)
            {
                strMJVNumber = string.Empty;
                try
                {
                    ObjMJV_DAL = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_ManualJournalDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ManualJournalRow ObjMJVRow in ObjMJV_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertMJVDetails");
                        db.AddInParameter(command, "@MJV_ID", DbType.Int32, ObjMJVRow.MJV_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMJVRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjMJVRow.LOB_ID);
                        db.AddInParameter(command, "@Location_Id", DbType.Int32, ObjMJVRow.Branch_ID);
                        db.AddInParameter(command, "@MJVDate", DbType.DateTime, ObjMJVRow.Approved_Date);
                        db.AddInParameter(command, "@ValueDate", DbType.DateTime, ObjMJVRow.Value_Date);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param;
                            param = new OracleParameter("@XMLMJVDetails",
                                   OracleType.Clob, ObjMJVRow.XMLMJVDetails.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjMJVRow.XMLMJVDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLMJVDetails", DbType.String, ObjMJVRow.XMLMJVDetails);
                        }

                        if (!ObjMJVRow.IsXML_InvoiceDtlsNull())
                            db.AddInParameter(command, "@XMLInvoiceDetails", DbType.String, ObjMJVRow.XML_InvoiceDtls);
                        if (!ObjMJVRow.IsXML_SalesInvoiceDtlsNull())
                            db.AddInParameter(command, "@XMLSalesInvoiceDetails", DbType.String, ObjMJVRow.XML_SalesInvoiceDtls);
                        if (!ObjMJVRow.IsXML_AccDtlsNull())
                            db.AddInParameter(command, "@XML_AccDtls", DbType.String, ObjMJVRow.XML_AccDtls);
                        if (!ObjMJVRow.IsTranche_Header_IDNull())
                            db.AddInParameter(command, "@Tranche_Header_ID", DbType.Int32, ObjMJVRow.Tranche_Header_ID);

                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjMJVRow.Created_By);
                        db.AddInParameter(command, "@CancelStatus", DbType.Int32, ObjMJVRow.CancelStatus);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@MJV_No", DbType.String, 30);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                // db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //strMJVNumber = command.Parameters["@MJV_No"].Value.ToString();
                                    trans.Rollback();
                                }
                                else
                                {
                                    strMJVNumber = command.Parameters["@MJV_No"].Value.ToString();
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
