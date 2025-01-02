#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Constitution Master
/// Created By			: Kaliraj Y
/// Created Date		: 01-Jun-2010
/// Purpose	            : 

/// <Program Summary>
#endregion
using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using S3GBusEntity;
using System.Data.OracleClient;
namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {
        public class ClsPubConstitutionMaster
        {
            int intRowsAffected;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable ObjConstitutionMaster_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubConstitutionMaster()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreateConstitutionCode(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ConstitutionMasterDataTable, out string ConstitutionCode)
            {
                try
                {
                    ConstitutionCode = "-1";
                    ObjConstitutionMaster_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ConstitutionMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterRow ObjConstitutionMasterRow in ObjConstitutionMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertConstitutionDetails");
                        db.AddInParameter(command, "@Company_Id", DbType.Int32, ObjConstitutionMasterRow.Company_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjConstitutionMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@ConstitutionName", DbType.String, ObjConstitutionMasterRow.ConstitutionName);

                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param , param1; 
                            param = new OracleParameter("@XMLLOBDet",
                                   OracleType.Clob, ObjConstitutionMasterRow.XMLLOBDet.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjConstitutionMasterRow.XMLLOBDet);
                            command.Parameters.Add(param);

                            param1 = new OracleParameter("@XMLConsDocumentsDet",
                                   OracleType.Clob, ObjConstitutionMasterRow.XMLConsDocumentsDet.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjConstitutionMasterRow.XMLConsDocumentsDet);
                            command.Parameters.Add(param1);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLLOBDet", DbType.String, ObjConstitutionMasterRow.XMLLOBDet);
                            db.AddInParameter(command, "@XMLConsDocumentsDet", DbType.String, ObjConstitutionMasterRow.XMLConsDocumentsDet);
                        }
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjConstitutionMasterRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ConstitutionCode", DbType.String, 2000);
//                        db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        if (!string.IsNullOrEmpty((string)command.Parameters["@ConstitutionCode"].Value))
                            ConstitutionCode = (string)command.Parameters["@ConstitutionCode"].Value;
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

            #region Query Constitution Details
            /// <summary>
            /// Gets a Constitution details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_ConstitutionMasterDataTable"></param>
            /// <returns>Datatable containing Constitution LOB and Documents Details</returns>

            public DataSet FunPubQueryConstitutionDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ConstitutionMasterDataTable)
            {
                DataSet dsConstitution = new DataSet();
                ObjConstitutionMaster_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ConstitutionMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetConstitutionLOBDocumentsDetails");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterRow ObjConstitutionMasterRow in ObjConstitutionMaster_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjConstitutionMasterRow.Company_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjConstitutionMasterRow.Constitution_ID);
                        db.AddInParameter(command, "@User_ID", DbType.Int32, ObjConstitutionMasterRow.Created_By);
                        db.AddInParameter(command, "@IsQueryMode", DbType.Boolean, ObjConstitutionMasterRow.IsQueryMode);

                        //dsConstitution = db.ExecuteDataSet(command);
                        dsConstitution = db.FunPubExecuteDataSet(command);
                        
                        //LoadDataSet(command, dsBranchRole, dsBranchRole.S3G_ORG_UserManagement.TableName);
                    }
                    return dsConstitution;
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dsConstitution.Dispose();
                    dsConstitution = null;
                }

            }

            #endregion

            #region Query Constitution Details
            /// <summary>
            /// Gets a Constitution details using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_ConstitutionMasterDataTable"></param>
            /// <returns>Datatable containing Constitution Details</returns>

            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable FunPubQueryConstitutionMaster(SerializationMode SerMode, byte[] bytesObjSNXG_ConstitutionMasterDataTable)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices dsConstitutionDetails = new S3GBusEntity.Origination.OrgMasterMgtServices();
                ObjConstitutionMaster_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable)ClsPubSerialize.DeSerialize(bytesObjSNXG_ConstitutionMasterDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ConstitutionMasterRow ObjConstitutionMasterRow in ObjConstitutionMaster_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetConstitutionDetails");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjConstitutionMasterRow.Company_ID);
                        db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjConstitutionMasterRow.Constitution_ID);
                        //db.LoadDataSet(command, dsConstitutionDetails, dsConstitutionDetails.S3G_ORG_ConstitutionMaster.TableName);
                        db.FunPubLoadDataSet(command, dsConstitutionDetails, dsConstitutionDetails.S3G_ORG_ConstitutionMaster.TableName);
                    }
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsConstitutionDetails.S3G_ORG_ConstitutionMaster;
            }

            #endregion
              

        }
    }
}
