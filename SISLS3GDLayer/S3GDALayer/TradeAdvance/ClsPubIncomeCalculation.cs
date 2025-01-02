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

using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using S3GBusEntity;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace S3GDALayer.TradeAdvance.TradeAdvanceMgtServices
{
    public class ClsPubIncomeCalculation
    {
        int intRowsAffected;

        //Code added for getting common connection string  from config file
            Database db;
            public ClsPubIncomeCalculation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

        public int FunPubCreateIncomeCalculation(BillingEntity objBillingEntity, out string strJournalMessage)
        {
            strJournalMessage = "";
            try
            {
                DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertIncomeProcess");
                db.AddInParameter(command, "@Company_Id", DbType.Int32, objBillingEntity.intCompanyId);
                db.AddInParameter(command, "@Month_Year", DbType.Int64, objBillingEntity.lngMonthYear);
                db.AddInParameter(command, "@User_Id", DbType.Int32, objBillingEntity.intUserId);
                db.AddInParameter(command, "@XmlDealerDetails", DbType.String, objBillingEntity.strXmlBranchDetails);
                db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
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
                            strJournalMessage = (string)command.Parameters["@ErrorMsg"].Value;
                            throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                        }
                        else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                        {
                            strJournalMessage = (string)command.Parameters["@ErrorMsg"].Value;
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                            if (intRowsAffected == -1)
                                throw new Exception("Document Sequence no not-defined");
                            if (intRowsAffected == -2)
                                throw new Exception("Document Sequence no exceeds defined limit");
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
