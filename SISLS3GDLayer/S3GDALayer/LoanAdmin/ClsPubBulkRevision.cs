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
using System.Data.OracleClient;
using S3GBusEntity;


namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        public class ClsPubBulkRevision
        {
            int intErrorCode;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_BulkRevisionDataTable ObjBulkRevision_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubBulkRevision()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateBulkRevisionDetails(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_BulkRevisionDataTable, out string strBulkRevNo)
            {
                strBulkRevNo = "";
                try
                {

                    ObjBulkRevision_DAL = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_BulkRevisionDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_BulkRevisionDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_BulkRevisionDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LOANAD_BulkRevisionRow ObjBulkRevisionRow in ObjBulkRevision_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertBulkRevision");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBulkRevisionRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjBulkRevisionRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjBulkRevisionRow.Branch_ID);
                        db.AddInParameter(command, "@Bulk_Revision_Date", DbType.DateTime, ObjBulkRevisionRow.Bulk_Revision_Date);
                        db.AddInParameter(command, "@Intervening_Interest", DbType.String, ObjBulkRevisionRow.Intervening_Interest);
                        db.AddInParameter(command, "@Schedule_Date", DbType.DateTime, ObjBulkRevisionRow.Schedule_Date);
                        db.AddInParameter(command, "@Schedule_Time", DbType.String, ObjBulkRevisionRow.Schedule_Time);
                        db.AddInParameter(command, "@Bulk_Revision_Effective_Date", DbType.DateTime, ObjBulkRevisionRow.Bulk_Revision_Effective_Date);
                        db.AddInParameter(command, "@Changed_Interest_Rate", DbType.Decimal, ObjBulkRevisionRow.Changed_Interest_Rate);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjBulkRevisionRow.Created_By);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjBulkRevisionRow.Modified_By);
                        //db.AddInParameter(command, "@XMLBulkRevisionDetails", DbType.String, ObjBulkRevisionRow.XMLBulkRevisionDetails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLBulkRevisionDetails", OracleType.Clob,
                                ObjBulkRevisionRow.XMLBulkRevisionDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjBulkRevisionRow.XMLBulkRevisionDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLBulkRevisionDetails", DbType.String,
                                ObjBulkRevisionRow.XMLBulkRevisionDetails);
                        }
                        db.AddOutParameter(command, "@Bulk_RevNo", DbType.String, 500);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command,ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    if (command.Parameters["@Bulk_RevNo"].Value != null)
                                    {
                                        strBulkRevNo = (string)command.Parameters["@Bulk_RevNo"].Value;
                                    }
                                    throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intErrorCode == -1)
                                        throw new Exception("Document Number Sequence not defined");
                                    if (intErrorCode == -2)
                                        throw new Exception("Document Number Sequence exceeds");
                                }
                                else
                                {
                                    trans.Commit();
                                    strBulkRevNo = (string)command.Parameters["@Bulk_RevNo"].Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intErrorCode == 0)
                                    intErrorCode = 50;
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
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            public DataSet FunPubGetService(string ServiceName)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3g_LoanAd_GetSerivce");
                    db.AddInParameter(command, "@Service_Name", DbType.String, ServiceName);
                    db.FunPubLoadDataSet(command, ObjDS, "Service");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void FunPubUpdateService(string ServiceName, string strStatus, string strDocNo, int? intRecCount, int intCompanyId)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertS3gService");
                    db.AddInParameter(command, "@ServiceName", DbType.String, ServiceName);
                    db.AddInParameter(command, "@Status", DbType.String, strStatus);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyId);
                    db.AddInParameter(command, "@Doc_No", DbType.String, strDocNo);

                    if (intRecCount.HasValue)
                    {
                        db.AddInParameter(command, "@RECORD_COUNT", DbType.Int32, intRecCount);
                    }
                    db.FunPubExecuteNonQuery(command);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public DataTable FunPubGetBulkProcess(int intCompany_Id)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetBulkRevison_List");
                db.AddInParameter(command, "@Option", DbType.String, "3");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                return db.FunPubExecuteDataSet(command).Tables[0];
            }

            public DataTable FunPubGetBulkRevDetails(int intCompany_Id, int intBulkRevId)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetBulkRevison_List");
                db.AddInParameter(command, "@Option", DbType.String, "6");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                db.AddInParameter(command, "@Param1", DbType.Int32, intBulkRevId);
                return db.FunPubExecuteDataSet(command).Tables[0];
            }

            public DataSet FunPubGetOldEMIStructure(string strPANum, string strSANum, DateTime dtEffectivedate, int intCompany_Id)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetBulkRevison_List");
                db.AddInParameter(command, "@Option", DbType.String, "4");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                db.AddInParameter(command, "@Param1", DbType.String, strPANum);
                db.AddInParameter(command, "@Param2", DbType.String, strSANum);
                db.AddInParameter(command, "@Param3", DbType.String, dtEffectivedate);
                return db.FunPubExecuteDataSet(command);
            }

            public DataSet FunPubGetCashInflowOutflow(string strPANum, string strSANum, DateTime dtEffectivedate, int intCompany_Id)
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetBulkRevison_List");
                db.AddInParameter(command, "@Option", DbType.String, "5");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                db.AddInParameter(command, "@Param1", DbType.String, strPANum);
                db.AddInParameter(command, "@Param2", DbType.String, strSANum);
                db.AddInParameter(command, "@Param3", DbType.String, dtEffectivedate);
                return db.FunPubExecuteDataSet(command);
            }

            public int FunPubProcessBulkRevision(int intCompany_Id, string strBulk_RevNo, string strPANum, string strSANum, double CompIRR, double BusIRR, double AccIRR, decimal decinterInterest, string strEMIXML, decimal decRevisedRate, string strRepaySummary)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_ProcessBulkRevision");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                    db.AddInParameter(command, "@Bulk_RevNo", DbType.String, strBulk_RevNo);
                    db.AddInParameter(command, "@PANum", DbType.String, strPANum);
                    db.AddInParameter(command, "@SANum", DbType.String, strSANum);
                    db.AddInParameter(command, "@Comapny_IRR", DbType.Decimal, CompIRR);
                    db.AddInParameter(command, "@Business_IRR", DbType.Decimal, BusIRR);
                    db.AddInParameter(command, "@Accounting_IRR", DbType.Decimal, AccIRR);
                    db.AddInParameter(command, "@InterPeriodInterest", DbType.Decimal, decinterInterest);
                    //db.AddInParameter(command, "@Xml_RepayDetails", DbType.String, strEMIXML);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param = new OracleParameter("@Xml_RepayDetails", OracleType.Clob,
                            strEMIXML.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, strEMIXML);
                        command.Parameters.Add(param);

                        OracleParameter param1 = new OracleParameter("@Xml_RepaySummary", OracleType.Clob,
                         strRepaySummary.Length, ParameterDirection.Input, true,
                         0, 0, String.Empty, DataRowVersion.Default, strRepaySummary);
                        command.Parameters.Add(param1);
                    }
                    else
                    {
                        db.AddInParameter(command, "@Xml_RepayDetails", DbType.String,
                            strEMIXML);
                        db.AddInParameter(command, "@Xml_RepaySummary", DbType.String, strRepaySummary);//Added by Tamilselvan.S on 03/12/2011
                    }
                    db.AddInParameter(command, "@Revised_rate", DbType.Decimal, decRevisedRate);
                    //db.AddInParameter(command, "@Xml_RepaySummary", DbType.String, strRepaySummary);//Added by Tamilselvan.S on 03/12/2011
                    db.AddOutParameter(command, "@Error_Code", DbType.Int32, 100);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {

                            db.FunPubExecuteNonQuery(command,ref trans);
                            if ((int)command.Parameters["@Error_Code"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@Error_Code"].Value;
                                throw new Exception("Error thrown Error No" + intErrorCode.ToString());
                            }
                            else
                            {
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                            trans.Rollback();
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intErrorCode;
            }

            public int FunPubLogErrorBulkRevision(int intCompany_Id, string strBulk_RevNo, string strPANum, string strSANum)
            {

                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetBulkRevison_List");
                db.AddInParameter(command, "@Option", DbType.String, "6");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_Id);
                db.AddInParameter(command, "@Param1", DbType.String, strPANum);
                db.AddInParameter(command, "@Param2", DbType.String, strSANum);
                db.AddInParameter(command, "@Param3", DbType.String, strBulk_RevNo);
                return db.FunPubExecuteNonQuery(command);

            }
        }
    }
}
