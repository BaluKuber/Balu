#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: COLLATERAL
/// Screen Name			: COLLATERAL CAPTURE DAL Class
/// Created By			: NARASIMHA RAO.P
/// Created Date		: 16-May-2011
/// Purpose	            : DAL Class for Collateral Capture Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using System.Data.OracleClient;

namespace S3GDALayer.TradeAdvance
{
    namespace CollateralMgtServices
    {
        public class ClsPubCollateralCapture
        {

            Database db;
            public ClsPubCollateralCapture()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #region Initialization

            int intErrorCode = 0;

            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable ObjCollateralCaptureDataTable_DAL = null;
            S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureRow ObjCollateralCaptureRow = null;

            #endregion

            #region Create Collateral Capture
            /// <summary>
            /// Insert Collateral Valuation Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjCollateralCaptureDataTable"></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateCollateralCapture(SerializationMode SerMode, byte[] bytesObjCollateralCaptureDataTable, out string StrCollCap_No)
            {
                StrCollCap_No = "";
                try
                {

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_TA_InsertCollateralCapture");

                    ObjCollateralCaptureDataTable_DAL = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable)ClsPubSerialize.DeSerialize(bytesObjCollateralCaptureDataTable, SerMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable));
                    ObjCollateralCaptureRow = ObjCollateralCaptureDataTable_DAL.Rows[0] as S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureRow;

                    db.AddInParameter(command, "@Collateral_Capture_ID", DbType.Int32, ObjCollateralCaptureRow.Collateral_Capture_ID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCollateralCaptureRow.Company_ID);
                    db.AddInParameter(command, "@Type_ID", DbType.Int32, ObjCollateralCaptureRow.Type_ID);

                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjCollateralCaptureRow.Branch_ID);
                    db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCollateralCaptureRow.Product_ID);
                    db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjCollateralCaptureRow.Constitution_ID);
                    db.AddInParameter(command, "@Collateral_Collected_By", DbType.Int32, ObjCollateralCaptureRow.Collateral_Collected_By);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjCollateralCaptureRow.Customer_ID);
                    db.AddInParameter(command, "@Collection_Agent_ID", DbType.Int32, ObjCollateralCaptureRow.Collection_Agent_ID);
                    db.AddInParameter(command, "@Currency_Code", DbType.String, ObjCollateralCaptureRow.Currency_Code);
                    db.AddInParameter(command, "@Collateral_Tran_No", DbType.String, ObjCollateralCaptureRow.Collateral_Tran_No);
                    db.AddInParameter(command, "@Collateral_Tran_Date", DbType.DateTime, ObjCollateralCaptureRow.Collateral_Tran_Date);
                    db.AddInParameter(command, "@Ref_Point_Type_Code", DbType.Int32, ObjCollateralCaptureRow.Ref_Point_Type_Code);
                    db.AddInParameter(command, "@Ref_Point_Code", DbType.Int32, ObjCollateralCaptureRow.Ref_Point_Code);
                    db.AddInParameter(command, "@Ref_Point_ID", DbType.String, ObjCollateralCaptureRow.Ref_Point_ID);
                    db.AddInParameter(command, "@Total", DbType.String, ObjCollateralCaptureRow.Total);
                    db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjCollateralCaptureRow.Is_Active);
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;

                    if (enumDBType == S3GDALDBType.ORACLE)
                    {
                        OracleParameter param;
                        param = new OracleParameter("@XMLHIGHLIQSecurityDetails",
                            OracleType.Clob, ObjCollateralCaptureRow.XML_HighLiqSecDetails.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjCollateralCaptureRow.XML_HighLiqSecDetails);
                        command.Parameters.Add(param);

                        param = new OracleParameter("@XMLMEDIUMLIQSecurityDetails",
                            OracleType.Clob, ObjCollateralCaptureRow.XML_MedLiqSecDetails.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjCollateralCaptureRow.XML_MedLiqSecDetails);
                        command.Parameters.Add(param);

                        param = new OracleParameter("@XMLLOWLIQSecurityDetails",
                            OracleType.Clob, ObjCollateralCaptureRow.XML_LowLiqSecDetails.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjCollateralCaptureRow.XML_LowLiqSecDetails);
                        command.Parameters.Add(param);

                        param = new OracleParameter("@XMLCOMMODITIESLIQSecDetails",
                            OracleType.Clob, ObjCollateralCaptureRow.XML_CommoditiesDetails.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjCollateralCaptureRow.XML_CommoditiesDetails);
                        command.Parameters.Add(param);

                        param = new OracleParameter("@XMLFINANCIALLIQSecDetails",
                            OracleType.Clob, ObjCollateralCaptureRow.XML_FinancialDetails.Length,
                            ParameterDirection.Input, true, 0, 0, String.Empty,
                            DataRowVersion.Default, ObjCollateralCaptureRow.XML_FinancialDetails);
                        command.Parameters.Add(param);

                    }
                    else
                    {

                        db.AddInParameter(command, "@XMLHIGHLIQSecurityDetails", DbType.String, ObjCollateralCaptureRow.XML_HighLiqSecDetails);
                        db.AddInParameter(command, "@XMLMEDIUMLIQSecurityDetails", DbType.String, ObjCollateralCaptureRow.XML_MedLiqSecDetails);
                        db.AddInParameter(command, "@XMLLOWLIQSecurityDetails", DbType.String, ObjCollateralCaptureRow.XML_LowLiqSecDetails);
                        db.AddInParameter(command, "@XMLCOMMODITIESLIQSecDetails", DbType.String, ObjCollateralCaptureRow.XML_CommoditiesDetails);
                        db.AddInParameter(command, "@XMLFINANCIALLIQSecDetails", DbType.String, ObjCollateralCaptureRow.XML_FinancialDetails);
                    }
                    db.AddInParameter(command, "@Collateral_Valuation_No", DbType.String, ObjCollateralCaptureRow.Collateral_Valuation_No);
                    db.AddInParameter(command, "@Collateral_Valuation_Date", DbType.DateTime, ObjCollateralCaptureRow.Collateral_Valuation_Date);

                    

                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCollateralCaptureRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjCollateralCaptureRow.Created_On);

                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCollateralCaptureRow.Modified_By);
                    db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjCollateralCaptureRow.Modified_On);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@DS_NO", DbType.String, 100);
                   
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command,ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0 || (int)command.Parameters["@ErrorCode"].Value < 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            }
                            if ((int)command.Parameters["@ErrorCode"].Value == 0)
                            {
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                                trans.Commit();
                                StrCollCap_No = (string)command.Parameters["@DS_NO"].Value;
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (intErrorCode == 0)
                                intErrorCode = 50;
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }


            #endregion

            public DataTable FunGetCustomerDetails(int intCustomerID, int intRefPoint, int intRefPointDetails)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_TA_GetCustomerDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCustomerID);
                    db.AddInParameter(command, "@RefPoint", DbType.Int32, intRefPoint);
                    db.AddInParameter(command, "@RefPointDetails", DbType.Int32, intRefPointDetails);
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public DataTable FunGetCollateralDetailsID(int intCompany, int intLiquidityType, int intCollLevelCode)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLT_GetCustomerDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany);
                    db.AddInParameter(command, "@Liquidity_Type", DbType.Int32, intLiquidityType);
                    db.AddInParameter(command, "@Collateral_Level_Code", DbType.Int32, intCollLevelCode);
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
