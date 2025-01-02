using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;


namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
        public class ClsPubCollateralSaleApproval : ClsPubDalCollateralBase
        {
            int intRowsAffected=0;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalDataTable objS3G_CLT_ApprovalDataTable;
            public int FunPubCreateCollateralSaleApproval(SerializationMode SerMode, byte[] byteobjS3G_CLT_ApprovalDataTable)
            {
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objS3G_CLT_ApprovalDataTable = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalDataTable)ClsPubSerialize.DeSerialize(byteobjS3G_CLT_ApprovalDataTable, SerMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalDataTable));
                    foreach (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ApprovalRow objS3G_CLT_ApprovalRow in objS3G_CLT_ApprovalDataTable.Rows)
                    {
                        DbCommand dbcmd = db.GetStoredProcCommand("S3G_CLT_InsertCollateralSaleApproval");
                        db.AddInParameter(dbcmd, "@Collateral_Sale_ID", DbType.Int32, objS3G_CLT_ApprovalRow.Collateral_Sale_ID);
                        db.AddInParameter(dbcmd, "@Collateral_Sale_No", DbType.String, objS3G_CLT_ApprovalRow.Collateral_Sale_No);
                        db.AddInParameter(dbcmd, "@Status_Type_Code", DbType.Int32, objS3G_CLT_ApprovalRow.Status_Type_Code);
                        db.AddInParameter(dbcmd, "@Status_Code", DbType.Int32, objS3G_CLT_ApprovalRow.Status_Code);
                        db.AddInParameter(dbcmd, "@UserId", DbType.Int32, objS3G_CLT_ApprovalRow.Task_ApprovalUserID);
                        db.AddInParameter(dbcmd, "@Created_By", DbType.Int32, objS3G_CLT_ApprovalRow.Created_By);
                        db.AddInParameter(dbcmd, "@Remarks", DbType.String, objS3G_CLT_ApprovalRow.Remarks);
                        db.AddInParameter(dbcmd, "@Password", DbType.String, objS3G_CLT_ApprovalRow.Password);
                        db.AddInParameter(dbcmd, "@CompanyId", DbType.Int32, objS3G_CLT_ApprovalRow.Company_ID);
                        try
                        {
                            db.AddInParameter(dbcmd, "@Approval_ID", DbType.Int32, objS3G_CLT_ApprovalRow.Approval_ID);
                        }
                        catch (Exception)
                        {
                        }
                        db.AddOutParameter(dbcmd, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(dbcmd, ref trans);
                                if ((int)dbcmd.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)dbcmd.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (dbcmd.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)dbcmd.Parameters["@ErrorCode"].Value;
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
                                con.Close();
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
