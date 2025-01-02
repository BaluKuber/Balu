#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Loan Admin
/// Screen Name			: Asset Acquisition DAL Class
/// Created By			: Suresh P
/// Created Date		: 02-Sep-2010
/// Purpose	            : DAL Class for Asset Acquisition Methods
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using Entity_LoanAdmin = S3GBusEntity.LoanAdmin;
#endregion

namespace S3GDALayer.LoanAdmin
{
    namespace AssetMgtServices
    {
        public class ClsPubAssetAcquisition
        {
            #region Initialization

            int intErrorCode;

            Entity_LoanAdmin.AssetMgtServices dsAssetAcquisitionDetails = null;
            Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetAcquisitionDataTable ObjAssetAcquisitionDataTable_DAL = null;
            Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetAcquisitionRow ObjAssetAcquisitionRow = null;

            Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable ObjLeaseAssetRegisterDataTable_DAL = null;
            Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterRow ObjLeaseAssetRegisterRow = null;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAssetAcquisition()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create Asset Acquisition
            /// <summary>
            /// Create Asset Acquisition
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjAssetAcquisitionDataTable"></param>
            /// <returns></returns>
            public int FunPubCreateAssetAcquisitionInt(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable , out string DSN)
            {
                DSN = string.Empty;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_InsertAssetAcquisition);

                    ObjAssetAcquisitionDataTable_DAL = (Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetAcquisitionDataTable)ClsPubSerialize.DeSerialize(bytesObjAssetAcquisitionDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetAcquisitionDataTable));
                    ObjAssetAcquisitionRow = ObjAssetAcquisitionDataTable_DAL.Rows[0] as Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetAcquisitionRow;

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAssetAcquisitionRow.Company_ID);
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAssetAcquisitionRow.LOB_ID);
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjAssetAcquisitionRow.Branch_ID);
                    db.AddInParameter(command, "@Acquisition_Date", DbType.DateTime, ObjAssetAcquisitionRow.Acquisition_Date);

                    db.AddInParameter(command, "@Created_By", DbType.Int32, ObjAssetAcquisitionRow.Created_By);
                    db.AddInParameter(command, "@XMLParamtAssetAcquisitionDet", DbType.String, ObjAssetAcquisitionRow.XMLParamtAssetAcquisitionDet);
                    if (!ObjAssetAcquisitionRow.IsTxn_IDNull())
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, ObjAssetAcquisitionRow.Txn_ID);
                    db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    db.AddOutParameter(command, "@DSN", DbType.String, 100);

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            //db.ExecuteNonQuery(command, trans);
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intErrorCode = (int)command.Parameters["@ErrorCode"].Value;
                            if (intErrorCode == 1)
                            {
                                DSN = (string)command.Parameters["@DSN"].Value;
                                trans.Commit();
                            }
                            else
                            {
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intErrorCode;
            }
            #endregion

            #region Get Asset Acquisition
            /// <summary>
            /// Get Asset Acquisition
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjAssetAcquisitionDataTable"></param>
            /// <returns></returns>
            public Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable FunPubQueryAssetAcquisition(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable)
            {
                dsAssetAcquisitionDetails = new S3GBusEntity.LoanAdmin.AssetMgtServices();
                ObjLeaseAssetRegisterDataTable_DAL = (Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable)ClsPubSerialize.DeSerialize(bytesObjAssetAcquisitionDataTable, SerMode, typeof(Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable));
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_GetAssetAcquisition);
                try
                {
                    foreach (Entity_LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterRow ObjAssetAcquisitionRow in ObjLeaseAssetRegisterDataTable_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Acquisition_NO", DbType.String, ObjAssetAcquisitionRow.Acquisition_No);
                        /*
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjAssetAcquisitionRow.Company_ID);
                        db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjAssetAcquisitionRow.Branch_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjAssetAcquisitionRow.LOB_ID);
                        */
                        //db.LoadDataSet(command, dsAssetAcquisitionDetails, dsAssetAcquisitionDetails.S3G_LOANAD_LeaseAssetRegister.TableName);
                        db.FunPubLoadDataSet(command, dsAssetAcquisitionDetails, dsAssetAcquisitionDetails.S3G_LOANAD_LeaseAssetRegister.TableName);
                    }
                }
                catch (Exception exp)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(exp);
                }
                return dsAssetAcquisitionDetails.S3G_LOANAD_LeaseAssetRegister;
            }
            #endregion

        }

    }
}
