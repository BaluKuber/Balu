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
        #region Create Methods
        public class ClsPubLoanToValueSpooling
        {
            int intRowsAffected = 0;
            S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_LoanToValueDataTable S3G_CLN_LoanToValueDataTable;

              //Code added for getting common connection string  from config file
            Database db;
            public ClsPubLoanToValueSpooling()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateLoanToValue(SerializationMode SerMode, byte[] bytesLoanToValue_Datatable)
            {
                try
                {                    
                    S3G_CLN_LoanToValueDataTable = (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_LoanToValueDataTable)ClsPubSerialize.DeSerialize(bytesLoanToValue_Datatable, SerMode, typeof(S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_LoanToValueDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertLoanToValue");
                    foreach (S3GBusEntity.Collection.ClnReceivableMgtServices.S3G_CLN_LoanToValueRow ObjSpoolingRow in S3G_CLN_LoanToValueDataTable.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjSpoolingRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjSpoolingRow.LOB_ID);
                        if ( ObjSpoolingRow.Branch_ID != -1)
                            db.AddInParameter(command,"@Location_ID", DbType.Int32, ObjSpoolingRow.Branch_ID);
                        if(!ObjSpoolingRow.IsAsset_ClassNull())
                        db.AddInParameter(command, "@Asset_Class",DbType.Int32, ObjSpoolingRow.Asset_Class);
                        if(!ObjSpoolingRow.IsAsset_MakeNull())
                        db.AddInParameter(command, "@Asset_Make",DbType.Int32, ObjSpoolingRow.Asset_Make);
                        if(!ObjSpoolingRow.IsAsset_TypeNull())
                        db.AddInParameter(command, "@Asset_Type",DbType.Int32, ObjSpoolingRow.Asset_Type);
                        if(!ObjSpoolingRow.IsAsset_ModelNull())
                        db.AddInParameter(command, "@Asset_Model",DbType.Int32, ObjSpoolingRow.Asset_Model);
                        if(!ObjSpoolingRow.IsSpool_PathNull())
                        db.AddInParameter(command, "@Spool_Path", DbType.String, ObjSpoolingRow.Spool_Path);
                        db.AddInParameter(command, "@Spool_By", DbType.Int32, ObjSpoolingRow.Spool_By);
                        //db.AddInParameter(command, "@XMLLOANTOVALUEDETAILS", DbType.String, ObjSpoolingRow.XMLS3G_CLN_LoanToValueDetails);
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLLOANTOVALUEDETAILS", OracleType.Clob,
                                ObjSpoolingRow.XMLS3G_CLN_LoanToValueDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, ObjSpoolingRow.XMLS3G_CLN_LoanToValueDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLLOANTOVALUEDETAILS", DbType.String,
                                ObjSpoolingRow.XMLS3G_CLN_LoanToValueDetails);
                        }
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjSpoolingRow.Txn_ID);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

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
        #endregion
    }
}
