using System;using S3GDALayer.S3GAdminServices;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using S3GBusEntity; 

namespace S3GDALayer.Collection
{
    namespace ClnReceivableMgtServices
    {

        public class ClsPubDelinquencySpooling 
        {
            int intRowsAffected = 0;


            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencySpoolingDataTable S3G_CLN_DelinquencySpoolingDataTable;
              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDelinquencySpooling()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            public int FunPubCreateDelinquencySpooling(SerializationMode SerMode, byte[] bytesDelinquencySpooling_Datatable)
            {
                try
                {
                    S3G_CLN_DelinquencySpoolingDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencySpoolingDataTable)ClsPubSerialize.DeSerialize(bytesDelinquencySpooling_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencySpoolingDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertDelinquencySpooling");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_DelinquencySpoolingRow ObjSpoolingRow in S3G_CLN_DelinquencySpoolingDataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSpoolingRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSpoolingRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjSpoolingRow.Branch_ID);
                        db.AddInParameter(command, "@DelinquencySpool_Date", DbType.DateTime, ObjSpoolingRow.DelinquencySpool_Date);
                        db.AddInParameter(command, "@Delinquency_Month", DbType.String, ObjSpoolingRow.Delinquency_Month);
                        db.AddInParameter(command, "@Delinquency_Type", DbType.String, ObjSpoolingRow.Delinquency_Type);
                        db.AddInParameter(command, "@Tot_Prin_Prov_Sec", DbType.Decimal, ObjSpoolingRow.Total_Principal_Provisioning_Secured);
                        db.AddInParameter(command, "@To_Prin_Prov_UnSec", DbType.Decimal, ObjSpoolingRow.Total_Principal_Provisioning_UnSecured);
                        db.AddInParameter(command, "@Total_Income_Deffered", DbType.Decimal, ObjSpoolingRow.Total_Income_Deffered);
                        db.AddInParameter(command, "@DelinquencySpool_By", DbType.Int32, ObjSpoolingRow.DelinquencySpool_By);
                        //db.AddInParameter(command, "@XMLSpoolDetails", DbType.String, ObjSpoolingRow.XMLSpoolDetails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLSpoolDetails", OracleType.Clob,
                                ObjSpoolingRow.XMLSpoolDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjSpoolingRow.XMLSpoolDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLSpoolDetails", DbType.String,
                                ObjSpoolingRow.XMLSpoolDetails);
                        }

                        //db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjSpoolingRow.Txn_ID);
                        db.AddOutParameter(command,"@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                if (intRowsAffected == 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 20;
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }
        }
    }
}
