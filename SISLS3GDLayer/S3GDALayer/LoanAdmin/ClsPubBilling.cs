#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Application Process
/// Created By			: Narayanan
/// Created Date		: 06-07-2010
/// <Program Summary>
#endregion

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
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;

namespace S3GDALayer.LoanAdmin.LoanAdminMgtServices
{
    public class ClsPubBilling
    {
        int intRowsAffected;

        //Code added for getting common connection string  from config file
            Database db;
            public ClsPubBilling()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

        public int FunPubCreateBillingInt(BillingEntity objBillingEntity, out string strJournalMessage)
        {
            strJournalMessage = "";
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("s3g_cln_InsertBillingDetails_dummy");
                db.AddInParameter(command, "@CompanyId", DbType.Int32, objBillingEntity.intCompanyId);
                db.AddInParameter(command, "@LobId", DbType.Int32, objBillingEntity.intLobId);
                db.AddInParameter(command, "@BranchId", DbType.Int32, objBillingEntity.intBranchId);
                db.AddInParameter(command, "@MonthYear", DbType.Int64, objBillingEntity.lngMonthYear);
                db.AddInParameter(command, "@BillingDate", DbType.DateTime, objBillingEntity.dtBillingDate);
                db.AddInParameter(command, "@StartDate", DbType.DateTime, objBillingEntity.dtStartDate);
                db.AddInParameter(command, "@EndDate", DbType.DateTime, objBillingEntity.dtEndDate);
                db.AddInParameter(command, "@ScheduleDate", DbType.DateTime, objBillingEntity.dtScheduleDate);
                db.AddInParameter(command, "@ScheduleTime", DbType.String, objBillingEntity.strScheduleTime);
                db.AddInParameter(command, "@Frequency", DbType.String, objBillingEntity.intFrequency);
                db.AddInParameter(command, "@UserId", DbType.Int32, objBillingEntity.intUserId);
                db.AddInParameter(command, "@Is_Regular", DbType.String, objBillingEntity.Is_Regular);
                db.AddInParameter(command, "@Customer_ID", DbType.String, objBillingEntity.intCustomer_ID);  //Added On 07-Sep-2016
                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                if (enumDBType == S3GDALDBType.ORACLE)
                {
                    OracleParameter param = new OracleParameter("@XmlBranchDetails", OracleType.Clob,
                                objBillingEntity.strXmlBranchDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objBillingEntity.strXmlBranchDetails);
                    command.Parameters.Add(param);

                    OracleParameter param1 = new OracleParameter("@XmlControlDataDetails", OracleType.Clob,
                                objBillingEntity.strXmlControlDataDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objBillingEntity.strXmlControlDataDetails);
                    command.Parameters.Add(param1);

                    OracleParameter param2 = new OracleParameter("@XmlCashFlowDetails", OracleType.Clob,
                                objBillingEntity.strXmlCashFlowDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objBillingEntity.strXmlCashFlowDetails);
                    command.Parameters.Add(param2);
                }
                else
                {
                    db.AddInParameter(command, "@XmlBranchDetails", DbType.String, objBillingEntity.strXmlBranchDetails);
                    db.AddInParameter(command, "@XmlControlDataDetails", DbType.String, objBillingEntity.strXmlControlDataDetails);
                    db.AddInParameter(command, "@XmlCashFlowDetails", DbType.String, objBillingEntity.strXmlCashFlowDetails);
                    
                    if (objBillingEntity.strXmlAccDetails != null)
                        db.AddInParameter(command, "@XmlAccDetails", DbType.String, objBillingEntity.strXmlAccDetails);
                }

                db.AddInParameter(command, "@XmlCustomerDetails", DbType.String, objBillingEntity.strXmlCustomerDetails);

                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                db.AddOutParameter(command, "@JournalMessage", DbType.String, 500);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(command, ref trans);
                        
                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            strJournalMessage = (string)command.Parameters["@JournalMessage"].Value;
                            throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                        }
                        else
                        {
                            //strJournalMessage = Convert.ToString(command.Parameters["@JournalMessage"].Value);
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
            return intRowsAffected;

          
        }
        // To Insert Billing PDF Service Added By R. Manikandan JAN 31 - 2013
        public int FunPubGetPDF(BillingEntity objBillingEntity)
        {
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertS3gService");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, objBillingEntity.intCompanyId);
                db.AddInParameter(command, "@ServiceName", DbType.String, "BILLPDF");
                db.AddInParameter(command, "@Schedule_Date", DbType.DateTime, objBillingEntity.dtScheduleDate);
                db.AddInParameter(command, "@Schedule_Time", DbType.String, objBillingEntity.strScheduleTime);
                db.AddInParameter(command, "@User_Id", DbType.Int32, objBillingEntity.intUserId);
                db.AddInParameter(command, "@Status", DbType.String, "O");
                db.AddInParameter(command, "@Doc_No", DbType.Int32, objBillingEntity.intFrequency);
                
               
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        intRowsAffected = 0;
                        db.FunPubExecuteNonQuery(command, ref trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
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
            return intRowsAffected;


        }
        // End

        public DataSet FunPubCalculateBillingInt(Dictionary<string, string> dctProcParams)
        {
            DataSet ds = new DataSet();
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_CalcBilling");
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
                        command.CommandTimeout = 3600;
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

        /* Added by Suresh - Start */
        public int FunPubCancelInvoice(BillingEntity objBillingEntity, out string strInvoiceNo)
        {
            strInvoiceNo = "";
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_CancelInvoice");
                db.AddInParameter(command, "@Company_ID", DbType.Int32, objBillingEntity.intCompanyId);
                db.AddInParameter(command, "@User_Id", DbType.Int32, objBillingEntity.intUserId);
                db.AddInParameter(command, "@Billing_Id", DbType.Int32, objBillingEntity.intBillingId);
                db.AddInParameter(command, "@Invoice_Type", DbType.Int32, objBillingEntity.intInvoiceType);
                if (objBillingEntity.strXmlAccDetails != null)
                    db.AddInParameter(command, "@XmlAccDetails", DbType.String, objBillingEntity.strXmlAccDetails);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                db.AddOutParameter(command, "@InvoiceNo", DbType.String, 1000);

                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        intRowsAffected = 0;
                        db.FunPubExecuteNonQuery(command, ref trans);
                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            strInvoiceNo = (string)command.Parameters["@InvoiceNo"].Value;
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
            return intRowsAffected;
        }
        /* Added by Suresh - End */


        public int FunPubCreateBillEmailStatus(BillingEntity objBillingEntity, out string strJournalMessage)
        {
            strJournalMessage = "";
            try
            {
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("s3g_cln_InsertBillEmailStatus");
                db.AddInParameter(command, "@Email_Group", DbType.String, objBillingEntity.strGroupName);
                db.AddInParameter(command, "@Customer_ID", DbType.Int32, objBillingEntity.strCustomer_ID);
                db.AddInParameter(command, "@XMLRSDetail", DbType.String, objBillingEntity.XMLGroupRSDetails);
                db.AddInParameter(command, "@Billing_ID", DbType.Int32, objBillingEntity.intBilling_ID);
                db.AddInParameter(command, "@EmailTo", DbType.String, objBillingEntity.strEmailTo);
                db.AddInParameter(command, "@EmailCC", DbType.String, objBillingEntity.strEmailCC);
                db.AddInParameter(command, "@Status", DbType.String, objBillingEntity.strStatus);
                db.AddInParameter(command, "@ErrMsg", DbType.String, objBillingEntity.strErrMsg);
                db.AddInParameter(command, "@UserID", DbType.Int32, objBillingEntity.intUserId);

                S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        db.FunPubExecuteNonQuery(command, ref trans);

                        if ((int)command.Parameters["@ErrorCode"].Value != 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                        }
                        else
                        {
                            //strJournalMessage = Convert.ToString(command.Parameters["@JournalMessage"].Value);
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
            return intRowsAffected;

        }

    }
}
