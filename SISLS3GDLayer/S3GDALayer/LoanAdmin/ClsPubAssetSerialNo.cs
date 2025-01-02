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
        public class ClsPubAssetSerialNo
        {
            #region Initialization

            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_AssetSerialNoDataTable ObjAssetSerialNoDataTableDataTable_DAL;

            //Below Code To Get The Connection string from the config file
            Database db;
            public ClsPubAssetSerialNo()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion
            #region Update Asset SerialNo
            public int FunPubUpdateAssetSerialNo(SerializationMode SerMode, byte[] bytesObjAssetSerialNoDataTable)
            {
                try
                {
                    ObjAssetSerialNoDataTableDataTable_DAL = (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_AssetSerialNoDataTable)ClsPubSerialize.DeSerialize(bytesObjAssetSerialNoDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_AssetSerialNoDataTable));
                    DbCommand command = null;

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminMgtServices.S3G_LAD_AssetSerialNoRow ObjAssetSerialNoRow in ObjAssetSerialNoDataTableDataTable_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_LAD_InsertAssetSerialNo");
                        db.AddInParameter(command, "@CompanyID", DbType.String, ObjAssetSerialNoRow.CompanyID);
                        db.AddInParameter(command, "@UserID", DbType.String, ObjAssetSerialNoRow.UserID);
                        db.AddInParameter(command, "@CustID", DbType.String, ObjAssetSerialNoRow.CustID);
                        db.AddInParameter(command, "@EntityID", DbType.String, ObjAssetSerialNoRow.EntityID);
                        db.AddInParameter(command, "@InvID", DbType.String, ObjAssetSerialNoRow.InvID);
                        db.AddInParameter(command, "@XmlAssetSerialNo", DbType.String, ObjAssetSerialNoRow.XmlAssetSerialNo);
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
