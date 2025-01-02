#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: COLLATERAL
/// Screen Name			: COLLATERAL STORAGE
/// Created By			: Manikandan. R
/// Created Date		: 18-May-2011
/// Purpose	            : DAL Class for Collateral Storage Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;

namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
        public class ClsPubCollateralStorage : ClsPubDalCollateralBase
        {
            int intRowsAffected = 0;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsDataTable objCollateralStorage;
            public int FunPubCollateralStorage(SerializationMode serMode, byte[] byteobjCollateralStorage, out string StrCollateralStorage)
            {
                try
                {
                    StrCollateralStorage = string.Empty;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                objCollateralStorage = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsDataTable)ClsPubSerialize.DeSerialize(byteobjCollateralStorage, serMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsDataTable));
                foreach (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_StorageDetailsRow objCollateralStorageRow in objCollateralStorage.Rows) 
                {
                    DbCommand dbRow = db.GetStoredProcCommand("S3G_CLT_InsertCollateralStorage");
                    db.AddInParameter(dbRow, "@Company_ID", DbType.Int32, objCollateralStorageRow.Company_ID);
                    db.AddInParameter(dbRow, "@Collateral_Storage_ID", DbType.Int32, objCollateralStorageRow.Collateral_Storage_ID);
                    db.AddInParameter(dbRow, "@Collateral_Storage_Date", DbType.DateTime, objCollateralStorageRow.Collateral_Storage_Date);
                    db.AddInParameter(dbRow, "@Collateral_Capture_ID", DbType.Int32, objCollateralStorageRow.Collateral_Capture_ID);
                    db.AddInParameter(dbRow, "@Is_Active", DbType.Boolean, objCollateralStorageRow.Is_Active);
                    if(!objCollateralStorageRow.IsXmlHighLiquiditySecurityNull())
                    db.AddInParameter(dbRow, "@XmlHighLiquiditySecurity", DbType.String, objCollateralStorageRow.XmlHighLiquiditySecurity);
                    if(!objCollateralStorageRow.IsXmlMediumLiquiditySecurityNull())
                    db.AddInParameter(dbRow, "@XmlMediumLiquiditySecurity", DbType.String, objCollateralStorageRow.XmlMediumLiquiditySecurity);
                    if(!objCollateralStorageRow.IsXmlLowLiquiditySecurityNull())
                    db.AddInParameter(dbRow, "@XmlLowLiquiditySecurity", DbType.String, objCollateralStorageRow.XmlLowLiquiditySecurity);
                    if(!objCollateralStorageRow.IsXmlCommoditySecurityNull())
                    db.AddInParameter(dbRow, "@XmlCommoditySecurity", DbType.String, objCollateralStorageRow.XmlCommoditySecurity);
                    if(!objCollateralStorageRow.IsXmlFinancialSecuritySecurityNull())
                    db.AddInParameter(dbRow, "@XmlFinancialSecuritySecurity", DbType.String, objCollateralStorageRow.XmlFinancialSecuritySecurity);
                    db.AddInParameter(dbRow, "@Created_By", DbType.Int32, objCollateralStorageRow.Created_By);
                    db.AddInParameter(dbRow, "@Created_On", DbType.DateTime, objCollateralStorageRow.Created_On);
                    db.AddInParameter(dbRow, "@Modified_By", DbType.Int32, objCollateralStorageRow.Modified_By);
                    db.AddInParameter(dbRow, "@Modified_On", DbType.DateTime, objCollateralStorageRow.Modified_On);
                    db.AddOutParameter(dbRow, "@Collateral_Storage_No", DbType.String, 1000);
                    db.AddOutParameter(dbRow, "@ErrorCode", DbType.Int32, sizeof(Int64));

                     using (DbConnection con = db.CreateConnection())
                        {
                            con.Open();
                            DbTransaction trans = con.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(dbRow, ref trans);
                                if ((int)dbRow.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)dbRow.Parameters["@ErrorCode"].Value;
                                    trans.Rollback();

                                }
                                else
                                {
                                    StrCollateralStorage = dbRow.Parameters["@Collateral_Storage_No"].Value.ToString();
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (dbRow.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)dbRow.Parameters["@ErrorCode"].Value;
                                    
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
