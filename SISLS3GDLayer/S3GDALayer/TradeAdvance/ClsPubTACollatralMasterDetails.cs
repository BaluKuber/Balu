using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace S3GDALayer.TradeAdvance
{
    namespace ClsPubTACollatralMasterDetails
    {
        public class ClsPubTACollateralMaster
        {

            Database db;
            public ClsPubTACollateralMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            int intRowsAffected = 0;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterDataTable objS3G_CLT_CollateralMasterDataTable;
            public int FunPubTACreateCollateralMaster(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollateralMasterDataTable, out string strCollateralRefNo)
            {
                strCollateralRefNo = string.Empty;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    objS3G_CLT_CollateralMasterDataTable = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterDataTable)ClsPubSerialize.DeSerialize(byteobjS3G_CLT_CollateralMasterDataTable, SerMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterDataTable));
                    foreach (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralMasterRow objS3G_CLT_CollateralMasterRow in objS3G_CLT_CollateralMasterDataTable.Rows)
                    {
                        DbCommand dbcmd = db.GetStoredProcCommand("S3G_TA_InsertCollateralMaster");
                        db.AddInParameter(dbcmd, "@Collateral_ID", DbType.Int32, objS3G_CLT_CollateralMasterRow.Collateral_ID);
                        db.AddInParameter(dbcmd, "@Company_ID", DbType.Int32, objS3G_CLT_CollateralMasterRow.Company_ID);
                        if (!objS3G_CLT_CollateralMasterRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(dbcmd, "@LOB_ID", DbType.Int32, objS3G_CLT_CollateralMasterRow.LOB_ID);
                        }
                        if (!objS3G_CLT_CollateralMasterRow.IsProduct_IdNull())
                        {
                            db.AddInParameter(dbcmd, "@Product_Id", DbType.Int32, objS3G_CLT_CollateralMasterRow.Product_Id);
                        }
                        if (!objS3G_CLT_CollateralMasterRow.IsConstitution_IDNull())
                        {
                            db.AddInParameter(dbcmd, "@Constitution_ID", DbType.Int32, objS3G_CLT_CollateralMasterRow.Constitution_ID);
                        }
                        db.AddInParameter(dbcmd, "@Collateral_Level_Description", DbType.String, objS3G_CLT_CollateralMasterRow.Collateral_Level_Description);
                        db.AddInParameter(dbcmd, "@Is_Active", DbType.Boolean, objS3G_CLT_CollateralMasterRow.Is_Active);
                        db.AddInParameter(dbcmd, "@User_ID", DbType.Int32, objS3G_CLT_CollateralMasterRow.Created_By);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XmlCollateralDetails", OracleType.Clob,
                                objS3G_CLT_CollateralMasterRow.XmlCollateralDetails.Length, ParameterDirection.Input, true,
                                0, 0, String.Empty, DataRowVersion.Default, objS3G_CLT_CollateralMasterRow.XmlCollateralDetails);
                            dbcmd.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(dbcmd, "@XmlCollateralDetails", DbType.String,
                                objS3G_CLT_CollateralMasterRow.XmlCollateralDetails);
                        }

                        //db.AddInParameter(dbcmd, "@XmlCollateralDetails", DbType.String, objS3G_CLT_CollateralMasterRow.XmlCollateralDetails);

                        db.AddOutParameter(dbcmd, "@Collateral_Ref_No", DbType.String, 50);
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
                                    strCollateralRefNo = dbcmd.Parameters["@Collateral_Ref_No"].Value.ToString();
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
