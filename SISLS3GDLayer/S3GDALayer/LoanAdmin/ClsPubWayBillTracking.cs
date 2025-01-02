using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using S3GDALayer.S3GAdminServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminMgtServices
    {
        public class ClsPubWayBillTracking
        {
            #region Initialization

            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable ObjWayBillTrackingDataTable_DAL;

            //Below Code To Get The Connection string from the config file
            Database db;
            public ClsPubWayBillTracking()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region UPDATE WAY BILL TRACKING

            public int FunPubUpdateWayBillTracking(SerializationMode SerMode, byte[] bytesObjWayBillTrackingDataTable)
            {
                try
                {
                    ObjWayBillTrackingDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable)ClsPubSerialize.DeSerialize(bytesObjWayBillTrackingDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable));
                    DbCommand command = null;

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingRow ObjWayBillTrackingRow in ObjWayBillTrackingDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_LAD_WBT_Updation");
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjWayBillTrackingRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.String, ObjWayBillTrackingRow.User_ID);
                        db.AddInParameter(command, "@XML_WBTDtls", DbType.String, ObjWayBillTrackingRow.XMLWBTDtls);
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
                    ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

            public int FunPubSaveWayBillTracking(SerializationMode SerMode, byte[] bytesObjWayBillTrackingDataTable)
            {
                try
                {
                    ObjWayBillTrackingDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable)ClsPubSerialize.DeSerialize(bytesObjWayBillTrackingDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingDataTable));
                    DbCommand command = null;

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LOANAD_WayBillTrackingRow ObjWayBillTrackingRow in ObjWayBillTrackingDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_LAD_WBT_SAVE");
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjWayBillTrackingRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.String, ObjWayBillTrackingRow.User_ID);
                        db.AddInParameter(command, "@XML_WBTDtls", DbType.String, ObjWayBillTrackingRow.XMLWBTDtls);
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
                    ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }

            #endregion
        }
    }
}
