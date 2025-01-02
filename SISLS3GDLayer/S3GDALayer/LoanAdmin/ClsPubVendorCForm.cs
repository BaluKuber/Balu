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
        public class ClsPubVendorCForm
        {
            #region Initialization

            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_VENDORCDataTable ObjVendorCDataTableDataTable_DAL;

            //Below Code To Get The Connection string from the config file
            Database db;
            public ClsPubVendorCForm()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion
            #region Update Asset SerialNo
            public int FunPubUpdateVendorC(SerializationMode SerMode, byte[] bytesObjVendorCDataTable)
            {
                try
                {
                    ObjVendorCDataTableDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_VENDORCDataTable)ClsPubSerialize.DeSerialize(bytesObjVendorCDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_VENDORCDataTable));
                    DbCommand command = null;

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_VENDORCRow ObjVendorCRow in ObjVendorCDataTableDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_LAD_InsertUpdateVendorC");
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjVendorCRow.Company_ID);
                        db.AddInParameter(command, "@User_ID", DbType.String, ObjVendorCRow.User_ID);
                        db.AddInParameter(command, "@CType", DbType.String, ObjVendorCRow.CType);
                        db.AddInParameter(command, "@XMLPurchaseDetails", DbType.String, ObjVendorCRow.XMLPurchaseDetails);
                        db.AddInParameter(command, "@XMLRSDetails", DbType.String, ObjVendorCRow.XMLRSDetails);
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
