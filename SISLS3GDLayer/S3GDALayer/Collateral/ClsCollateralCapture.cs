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
using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;


namespace S3GDALayer.Collateral
{
    namespace CollateralMgtServices
    {
      public class ClsCollateralCapture
        {
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
            public int FunPubCreateCollateralCapture(SerializationMode SerMode, byte[] bytesObjCollateralCaptureDataTable, out int intCollCap_ID)
            {
                intCollCap_ID = 0;
                try
                {

                    Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLT_InsertCollateralCapture");

                    ObjCollateralCaptureDataTable_DAL = (S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable)ClsPubSerialize.DeSerialize(bytesObjCollateralCaptureDataTable, SerMode, typeof(S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureDataTable));
                    ObjCollateralCaptureRow = ObjCollateralCaptureDataTable_DAL.Rows[0] as S3GBusEntity.Collateral.CollateralMgtServices.S3G_CLT_CollateralCaptureRow;
                    
                    
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCollateralCaptureRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Currency_Code", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Collateral_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Collateral_Ref_No", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Collateral_Tran_Date", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Ref_Point_Type_Code", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Ref_Point_Code", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Ref_Point_ID", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Collateral_Valuation_No", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Collateral_Valuation_Date", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@Is_Active", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@XMLHIGHLIQSecurityDetails", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@XMLMEDIUMLIQSecurityDetails", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@XMLLOWLIQSecurityDetails", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@XMLCOMMODITIESLIQSecurityDetails", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);
                    db.AddInParameter(command, "@XMLFINANACIALLIQSecurityDetails", DbType.Int32, ObjCollateralCaptureRow.LOB_ID);

                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCollateralCaptureRow.Created_By);
                    db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjCollateralCaptureRow.Created_On);

                    db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCollateralCaptureRow.Modified_By);
                    db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjCollateralCaptureRow.Modified_On);

                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@Collateral_Tran_No", DbType.String, 100);
                    //db.AddOutParameter(command, "@strRN_ID", DbType.Int32, sizeof(Int32));

                    db.ExecuteNonQuery(command);

                    if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                    if ((int)command.Parameters["@Collateral_Capture_ID"].Value > 0)
                        intCollCap_ID = (int)command.Parameters["@Collateral_Capture_ID"].Value;

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
                    Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_CLT_GetCustomerDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCustomerID);
                    db.AddInParameter(command, "@RefPoint", DbType.Int32, intRefPoint);
                    db.AddInParameter(command, "@RefPointDetails", DbType.Int32, intRefPointDetails);
                    db.LoadDataSet(command, ObjDS, "dtTable");
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
