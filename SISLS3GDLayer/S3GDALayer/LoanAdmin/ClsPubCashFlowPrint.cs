#region Page Header
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Cash Flow Print
/// Created By			: Sangeetha R
/// Created Date		: 06-06-2015
/// <Program Summary>
#endregion

#region Namespaces
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
using System;
using S3GDALayer.S3GAdminServices;
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace ContractMgtServices
    {
        public class ClsPubCashFlowPrint
        {
             #region Declaration
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintDataTable ObjCashFlow_DataTable = new S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintDataTable();
            Database db;
            public ClsPubCashFlowPrint()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }
            #endregion

            public int FunPubGenerateCashFlowPrint(SerializationMode SerMode, byte[] bytesObjCashFlow_DataTable, out Int64 intCashFlw_No, out string strErrorMsg)
            {
                strErrorMsg = "";
                intCashFlw_No = 0;
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjCashFlow_DataTable = (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintDataTable)ClsPubSerialize.DeSerialize(bytesObjCashFlow_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable));
                    foreach (S3GBusEntity.LoanAdmin.ContractMgtServices.S3G_LoanAd_CashFlowPrintRow ObjCashFlowRow in ObjCashFlow_DataTable.Rows)
                    {
                    DbCommand command = db.GetStoredProcCommand("S3G_LoanAd_GenerateCashFlowPrt");
                    db.AddInParameter(command, "@CompanyId", DbType.Int32, ObjCashFlowRow.Company_ID);
                    db.AddInParameter(command, "@UserId", DbType.Int32, ObjCashFlowRow.User_Id);

                    if (!ObjCashFlowRow.IsStartDateNull())
                        db.AddInParameter(command, "@StartDate", DbType.DateTime, ObjCashFlowRow.StartDate);
                    if (!ObjCashFlowRow.IsEndDateNull())
                        db.AddInParameter(command, "@Enddate", DbType.DateTime, ObjCashFlowRow.EndDate);

                    db.AddInParameter(command, "@Due_Date", DbType.DateTime, ObjCashFlowRow.Due_Date);

                    if (!ObjCashFlowRow.IsXmlAccCashFlowNull())
                    {
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XmlAccCashFlow", OracleType.Clob,
                            ObjCashFlowRow.XmlAccCashFlow.Length, ParameterDirection.Input, true,
                            0, 0, String.Empty, DataRowVersion.Default, ObjCashFlowRow.XmlAccCashFlow);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XmlAccCashFlow", DbType.String,
                            ObjCashFlowRow.XmlAccCashFlow);
                        }
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                    db.AddOutParameter(command, "@CashFlw_No", DbType.Int64, sizeof(Int64));
                    using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                intCashFlw_No = (Int64)command.Parameters["@CashFlw_No"].Value;
                                strErrorMsg = Convert.ToString(command.Parameters["@ErrorMsg"].Value);

                                trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }
                }
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }

            public DataSet FunPriGetCashFlowInt(Dictionary<string, string> dctProcParams)
            {
                DataSet ds = new DataSet();
                try
                {
                    DbCommand command = db.GetStoredProcCommand("S3G_Fund_RSBulkAct_Print");
                    foreach (KeyValuePair<string, string> ProcPair in dctProcParams)
                    {
                        db.AddInParameter(command, ProcPair.Key, DbType.String, ProcPair.Value);
                    }
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            command.CommandTimeout = 1800;
                            ds = db.FunPubExecuteDataSet(command, ref trans);

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
                        catch (Exception ex)
                        {
                            if (intRowsAffected == 0)
                                intRowsAffected = 50;
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
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return ds;


            }
        }
    }
}
