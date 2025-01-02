#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collateral
/// Screen Name			: Collateral Valuation DAL Class
/// Created By			: Muni Kavitha
/// Created Date		: 4-May-2011
/// Purpose	            : DAL Class for Collateral Valuation Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces

using System;using S3GDALayer.S3GAdminServices;
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
#endregion


namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
        public class ClsPubCollateralValuation : ClsPubDalCollateralBase
        {

            #region Initialization

            int intErrorCode = 0;

            // S3GBusEntity.Collateral.CollateralMgtServices dsCollateralValuation = null;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDataTable ObjCollateralValuationDataTable_DAL = null;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationRow ObjCollateralValuationRow = null;

            //S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDetailsDataTable ObjCollValuationDetailsDatatable_DAL = null;
            //S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDetailsRow ObjCollValuationDetailsRow = null;

            #endregion

            #region Create Collateral Valuation
            /// <summary>
            /// Insert Collateral Valuation Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjCollateralValuationDataTable"></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateCollateralValuation(SerializationMode SerMode, byte[] bytesObjCollateralValuationDataTable, out string strCVal_No)
            {
                strCVal_No = "";
                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLT_InsertCollateralValuation");

                    ObjCollateralValuationDataTable_DAL = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDataTable)ClsPubSerialize.DeSerialize(bytesObjCollateralValuationDataTable, SerMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationDataTable));
                    ObjCollateralValuationRow = ObjCollateralValuationDataTable_DAL.Rows[0] as S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_ValuationRow;


                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCollateralValuationRow.Company_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjCollateralValuationRow.Customer_ID);
                    db.AddInParameter(command, "@Collateral_Capture_ID", DbType.Int32, ObjCollateralValuationRow.Collateral_Capture_ID);
                    db.AddInParameter(command, "@Collateral_Valuation_No", DbType.String, ObjCollateralValuationRow.Collateral_Valuation_No);

                    db.AddInParameter(command, "@XML_HighLiqSecurityDetails", DbType.String, ObjCollateralValuationRow.XML_HighLiqSecDetails);
                    db.AddInParameter(command, "@XML_MediumLiqSecurityDetails", DbType.String, ObjCollateralValuationRow.XML_MedLiqSecDetails);
                    db.AddInParameter(command, "@XML_LowLiqSecurityDetails", DbType.String, ObjCollateralValuationRow.XML_LowLiqSecDetails);
                    db.AddInParameter(command, "@XML_CommoditySecurityDetails", DbType.String, ObjCollateralValuationRow.XML_CommoditiesDetails);
                    db.AddInParameter(command, "@XML_FinancialSecurityDetails", DbType.String, ObjCollateralValuationRow.XML_FinancialDetails);

                    //db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCollateralValuationRow.Created_By);
                    //db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjCollateralValuationRow.Created_On);

                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCollateralValuationRow.Modified_By);
                    db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjCollateralValuationRow.Modified_On);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@strCVal_No", DbType.String, 100);

                    //db.AddOutParameter(command, "@strRN_ID", DbType.Int32, sizeof(Int32));

                    ////db.ExecuteNonQuery(command);

                    ////if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    ////    intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    ////if ((string)command.Parameters["@strCVal_No"].Value != "")
                    ////    strCVal_No = Convert.ToString(command.Parameters["@strCVal_No"].Value);

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command, trans);
                            db.FunPubExecuteNonQuery(command, ref trans);


                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            }
                            else
                            {
                                strCVal_No = Convert.ToString(command.Parameters["@strCVal_No"].Value);
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
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
                    intErrorCode = 50;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                }
                return intErrorCode;


                ////    }
                ////    catch (Exception ex)
                ////    {
                ////         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                ////    }
                ////    return intErrorCode;
                ////}
            #endregion



            }
        }
    }
}
