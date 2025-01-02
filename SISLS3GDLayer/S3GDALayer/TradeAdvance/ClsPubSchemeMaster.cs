#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Trade Advance
/// Screen Name			: Trade Advance DAL Class
/// Created By			: Sathish R
/// Created Date		: 14-Nov-2012
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;

#endregion


namespace S3GDALayer.TradeAdvance
{
    namespace TradeAdvanceMgtServices
    {
        public class ClsPubSchemeMaster
        {
            int intRowsAffected = 0;

            Database db;
            public ClsPubSchemeMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.TASchemeMasterDataTable ObjTradeAdvanceAdd_DAL;

            public int FunPubTradeAdvance(SerializationMode SerMode, byte[] bytesObjS3G_TradeAdvance_AddDataTable)
            {

                try
                {
                    ObjTradeAdvanceAdd_DAL = (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.TASchemeMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_TradeAdvance_AddDataTable, SerMode, typeof(S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.TASchemeMasterDataTable));



                    foreach (S3GBusEntity.TradeAdvance.TradeAdvanceMgtServices.TASchemeMasterRow ObjTradeAdvanceMasterRow in ObjTradeAdvanceAdd_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertSchemeDetails");
                        db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjTradeAdvanceMasterRow.Product_ID);
                        db.AddInParameter(command, "@Product_Code", DbType.String, ObjTradeAdvanceMasterRow.Product_Code);
                        db.AddInParameter(command, "@Product_Desc", DbType.String, ObjTradeAdvanceMasterRow.Product_Desc);
                        db.AddInParameter(command, "@Product_Type", DbType.String, ObjTradeAdvanceMasterRow.Product_Type);
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjTradeAdvanceMasterRow.Company_Id);
                        db.AddInParameter(command, "@LOB", DbType.String, ObjTradeAdvanceMasterRow.LOB);
                        db.AddInParameter(command, "@Is_Active", DbType.String, ObjTradeAdvanceMasterRow.IsActive);
                        db.AddInParameter(command, "@Xml_Rate", DbType.String, ObjTradeAdvanceMasterRow.Xml_Rate);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected > 0)
                                {
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
                                {
                                    intRowsAffected = 50;
                                }
                                trans.Rollback();
                                throw ex;
                            }
                        }


                    }
                }
                catch (Exception exp)
                {
                    intRowsAffected = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return intRowsAffected;
            }

        }
    }
}
