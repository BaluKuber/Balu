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

namespace S3GDALayer.Insurance
{
    namespace InsuranceMgtServices
    {
        public class ClsPubAssetInsuranceClaim : ClsPubDalInsuranceBase
        {
            #region Initialization
            int intRowsAffected;
            S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable objAssetInsuranceClaim_DAL;
            #endregion

            #region AssetEntry
            public int FunPubCreateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, out string strICNNO)
            {
                strICNNO = "";
                try
                {
                    objAssetInsuranceClaim_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable)
                        ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, SerMode,
                        typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimRow objAssetClaimRow in objAssetInsuranceClaim_DAL)
                    {

                        DbCommand command = db.GetStoredProcCommand("S3G_INS_InsertAssetClaim");

                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetClaimRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAssetClaimRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAssetClaimRow.Branch_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAssetClaimRow.Customer_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, objAssetClaimRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAssetClaimRow.SANum);
                        db.AddInParameter(command, "@UserId", DbType.Int32, objAssetClaimRow.UserId);
                        db.AddInParameter(command, "@XMLPolicyDetails", DbType.String, objAssetClaimRow.XmlPolicyClaimDetails);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, objAssetClaimRow.Is_Active);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ICNNO", DbType.String, 200);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                               // db.ExecuteNonQuery(command, trans);
                               db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                else
                                {
                                    strICNNO = (string)command.Parameters["@ICNNO"].Value;
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
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw;
                }

                return intRowsAffected;
            }
            public int FunPubUpdateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable)
            {
                int intErrorCode = 0;
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_INS_UpdateAssetClaimDetails");
                    objAssetInsuranceClaim_DAL = (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable)
                         ClsPubSerialize.DeSerialize(byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, SerMode,
                         typeof(S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimDataTable));

                    foreach (S3GBusEntity.Insurance.InsuranceMgtServices.S3G_INS_AssetInsuranceClaimRow objAssetClaimRow in objAssetInsuranceClaim_DAL)
                    {
                        db.AddInParameter(command, "@AINSEId", DbType.Int64, objAssetClaimRow.Ins_Claim_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetClaimRow.Company_ID);
                        db.AddInParameter(command, "@XMLPolicyDetails", DbType.String, objAssetClaimRow.XmlPolicyClaimDetails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, objAssetClaimRow.Is_Active);
                       // db.ExecuteNonQuery(command);
                       db.FunPubExecuteNonQuery(command);
                        if (command.Parameters["@ErrorCode"].Value != null)
                        {
                            intErrorCode = Convert.ToInt32(command.Parameters["@ErrorCode"].Value);
                        }
                    }
                    return intErrorCode;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            #endregion
        }
    }
}