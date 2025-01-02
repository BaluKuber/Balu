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

namespace S3GDALayer.LoanAdmin
{
    namespace AssetMgtServices
    {

        public class ClsPubAssetVerification
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable objAssetVerification_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubAssetVerification()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// Inserting the Asset Verification details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubCreateAssetVerification(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AssetVerification_DataTable, out string strErrMsg)
            {
                try
                {
                    string strSize;
                    strErrMsg = string.Empty;
                    objAssetVerification_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AssetVerification_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsRow objAssetVerificationRow in objAssetVerification_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertAssetVerification");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetVerificationRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objAssetVerificationRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objAssetVerificationRow.Branch_ID);
                        db.AddInParameter(command, "@PANum", DbType.String, objAssetVerificationRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAssetVerificationRow.SANum);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAssetVerificationRow.Customer_ID);
                        db.AddInParameter(command, "@AssetDate", DbType.DateTime, objAssetVerificationRow.Asset_Verification_Date);
                        db.AddInParameter(command, "@AssetID", DbType.Int32, objAssetVerificationRow.Asse_ID);
                        db.AddInParameter(command, "@InspectionBy", DbType.Int32, objAssetVerificationRow.Inspection_By_Type_Code);
                        db.AddInParameter(command, "@Insepection_By_Code", DbType.Int32, objAssetVerificationRow.Insepection_By_Code);
                        db.AddInParameter(command, "@InspectionCode", DbType.Int32, objAssetVerificationRow.Inspection_Code);
                        db.AddInParameter(command, "@InspectionDate", DbType.DateTime, objAssetVerificationRow.Inspection_Date);
                        db.AddInParameter(command, "@Location", DbType.String, objAssetVerificationRow.Location);
                        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objAssetVerificationRow.Created_By);
                        db.AddInParameter(command, "@CreatedOn", DbType.DateTime, objAssetVerificationRow.Created_On);
                        db.AddInParameter(command, "@InspectionTime", DbType.String , objAssetVerificationRow.Inspection_Time );
                        db.AddInParameter(command, "@InspectionDate_To", DbType.DateTime, objAssetVerificationRow.Inspection_Date_To);
                        db.AddInParameter(command, "@InspectionTime_To", DbType.String, objAssetVerificationRow.Inspection_Time_To);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command,trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                                }
                                else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == -1)
                                        throw new Exception("Document Sequence no not-defined");
                                    if (intRowsAffected == -2)
                                        throw new Exception("Document Sequence no exceeds defined limit");
                                }
                                else
                                {
                                    strErrMsg = (string)command.Parameters["@DSNO"].Value;
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                // To identify if journal entry is failed
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                }
                                if (intRowsAffected == 0)
                                {
                                    intRowsAffected = 50;
                                }
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
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
                    throw ex;
                }
                return intRowsAffected;
            }


            /// <summary>
            /// Inserting the Asset Verification details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubModifyAssetVerification(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_AssetVerification_DataTable,out string strErrMsg)
            {
                try
                {
                    string strSize;
                    strErrMsg = string.Empty;
                    objAssetVerification_DAL = (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_AssetVerification_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_AssetVerficiationDetailsRow objAssetVerificationRow in objAssetVerification_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_UpdateAssetVerification");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objAssetVerificationRow.Company_ID);
                        db.AddInParameter(command, "@AssetVerificationNo", DbType.String, objAssetVerificationRow.Asset_Verification_No);
                        if (!objAssetVerificationRow.IsVerification_ChargesNull())
                            db.AddInParameter(command, "@VerificationCharges", DbType.Int64, objAssetVerificationRow.Verification_Charges);
                        if (!objAssetVerificationRow.IsMemo_ChargesNull())
                            db.AddInParameter(command, "@MemoCharges", DbType.Int64 , objAssetVerificationRow.Memo_Charges);
                        db.AddInParameter(command, "@PANum", DbType.String, objAssetVerificationRow.PANum);
                        db.AddInParameter(command, "@SANum", DbType.String, objAssetVerificationRow.SANum);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, objAssetVerificationRow.Customer_ID);
                        db.AddInParameter(command, "@InspectionBy", DbType.Int32, objAssetVerificationRow.Insepection_By_Code);
                        db.AddInParameter(command, "@InspectionCode", DbType.Int32, objAssetVerificationRow.Inspection_Code);
                        db.AddInParameter(command, "@InspectionDate", DbType.DateTime, objAssetVerificationRow.Inspection_Date);

                        db.AddInParameter(command, "@InspectionDate_To", DbType.DateTime, objAssetVerificationRow.Inspection_Date_To);
                        db.AddInParameter(command, "@InspectionTime_To", DbType.String, objAssetVerificationRow.Inspection_Time_To);
                        
                        db.AddInParameter(command, "@Location", DbType.String, objAssetVerificationRow.Location);
                        db.AddInParameter(command, "@InspectionTime", DbType.String, objAssetVerificationRow.Inspection_Time);
                        db.AddInParameter(command, "@UserType", DbType.String, objAssetVerificationRow.User_Type);
                        db.AddInParameter(command, "@Inspection_Date_Resp", DbType.DateTime, objAssetVerificationRow.Inspection_Date_Resp);
                        db.AddInParameter(command, "@Inspection_Time_Resp", DbType.String, objAssetVerificationRow.InspectionTime_Resp);
                        db.AddInParameter(command, "@Verified", DbType.Boolean, objAssetVerificationRow.Physically_Verified);
                        db.AddInParameter(command, "@Asset_Status_Type_Code", DbType.Int32, objAssetVerificationRow.Asset_Status_Type_Code);
                        db.AddInParameter(command, "@Asset_Status_code", DbType.Int32, objAssetVerificationRow.Asset_Status_code);
                        db.AddInParameter(command, "@Location_Resp", DbType.String, objAssetVerificationRow.Location_Resp);
                        db.AddInParameter(command, "@Remarks", DbType.String, objAssetVerificationRow.Remarks);
                        db.AddInParameter(command, "@Verification_Image", DbType.String, objAssetVerificationRow.Verification_Image);
                        db.AddInParameter(command, "@ModifiedBy", DbType.Int32, objAssetVerificationRow.Modified_By);
                        db.AddInParameter(command, "@ModifiedOn", DbType.DateTime, objAssetVerificationRow.Modified_On);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                        //db.ExecuteNonQuery(command);

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
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                    trans.Rollback();
                                }
                                else
                                {
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
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
                                conn.Close();
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

            /// <summary>
            /// To use verified assets details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass Asset_Verification_No</param>
            /// <returns></returns>

            public DataTable FunVerifedAssetForModification(string strAssetNo)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetsVerifiedForModify");
                    db.AddInParameter(command, "@Asset_Verification_No", DbType.String, strAssetNo);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
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
