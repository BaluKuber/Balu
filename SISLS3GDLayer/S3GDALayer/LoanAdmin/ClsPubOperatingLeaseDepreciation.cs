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
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubOperatingLeaseDepreciation
        {
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable objDepreciation_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubOperatingLeaseDepreciation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            /// <summary>
            /// To get PANum realted customer details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN</param>
            /// <returns></returns>

            public DataTable FunGetAssetDepreciationDetail(int intBranchID, int intCompanyID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetDepreciationDetail");
                    db.AddInParameter(command, "@Location_ID", DbType.Int32, intBranchID);
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public int FunGetDenominationDays(int companyID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");                    
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_LOANAD_GetDenominationDays);
                    db.AddInParameter(command, "@Company_Id", DbType.Int16, companyID);

                    //return (int)db.ExecuteScalar(command);                    
                    return Convert.ToInt32(db.FunPubExecuteScalar(command));
                }
                catch (Exception ex)
                {

                    return 0;
                }
            }

            /// <summary>
            /// To get PANum realted customer details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN</param>
            /// <returns></returns>

            public DataTable FunGetOperatingLeaseLOBBranch(int intCompanyID, int intUserID)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3G_LOANAD_GetOperatingLeaseLOBBranch");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetAssetDepreciationDetail");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUserID);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return (DataTable)ObjDS.Tables["dtTable"];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// <summary>
            /// Inserting the Depreciation Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubCreateAssetDepreciation(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_Depreciation_DataTable, out string strErrMsg)
            {
                try
                {
                    strErrMsg = string.Empty;
                    objDepreciation_DAL = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_Depreciation_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationRow objDepreciationRow in objDepreciation_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_INS_OLD");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objDepreciationRow.Company_ID);
                        db.AddInParameter(command, "@DepreciationNo", DbType.String, objDepreciationRow.Depreciation_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objDepreciationRow.LOB_ID);
                        if(!objDepreciationRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, objDepreciationRow.Branch_ID);
                        if (!objDepreciationRow.IsLast_Calculated_DateNull())
                            db.AddInParameter(command, "@LastDate", DbType.DateTime, objDepreciationRow.Last_Calculated_Date);
                        db.AddInParameter(command, "@CurrentDate", DbType.DateTime, objDepreciationRow.Current_Calculated_Date);
                        db.AddInParameter(command, "@Method", DbType.String, objDepreciationRow.Depreciation_Method);
                        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objDepreciationRow.Created_By);
                        db.AddInParameter(command, "@CreatedDate", DbType.DateTime, objDepreciationRow.Created_Date);
                        
                        db.AddInParameter(command, "@XMLAccount", DbType.String, objDepreciationRow.XMLValues);
                        
                        if (objDepreciationRow.XMLValues.Length < 25)
                        {
                            if (objDepreciationRow.XMLValues.ToString() == int.MaxValue.ToString())
                                db.AddInParameter(command, "@Is_Revoke", DbType.Boolean, true);
                        }
                        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal

                        //db.ExecuteNonQuery(command);

                        //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        //intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        // Create the Journal
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
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
                                // To identify if journal entry is failed
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
            /// Inserting the Depreciation Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>

            public int FunPubDeleteAssetDepreciation(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_Depreciation_DataTable, out string strErrMsg)
            {
                try
                {
                    strErrMsg = string.Empty;
                    objDepreciation_DAL = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_Depreciation_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_LeaseAssetDepreciationRow objDepreciationRow in objDepreciation_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_Delete_AssetDepreciation");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objDepreciationRow.Company_ID);
                        db.AddInParameter(command, "@DepreciationNo", DbType.String, objDepreciationRow.Depreciation_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objDepreciationRow.LOB_ID);
                        if (!objDepreciationRow.IsBranch_IDNull())
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, objDepreciationRow.Branch_ID);
                        if (!objDepreciationRow.IsLast_Calculated_DateNull())
                            db.AddInParameter(command, "@LastDate", DbType.DateTime, objDepreciationRow.Last_Calculated_Date);
                        db.AddInParameter(command, "@CurrentDate", DbType.DateTime, objDepreciationRow.Current_Calculated_Date);
                        db.AddInParameter(command, "@Method", DbType.String, objDepreciationRow.Depreciation_Method);
                        db.AddInParameter(command, "@CreatedBy", DbType.Int32, objDepreciationRow.Created_By);
                        db.AddInParameter(command, "@CreatedDate", DbType.DateTime, objDepreciationRow.Created_Date);

                        db.AddInParameter(command, "@XMLAccount", DbType.String, objDepreciationRow.XMLValues);

                        if (objDepreciationRow.XMLValues.Length < 25)
                        {
                            if (objDepreciationRow.XMLValues.ToString() == int.MaxValue.ToString())
                                db.AddInParameter(command, "@Is_Revoke", DbType.Boolean, true);
                        }
                        db.AddOutParameter(command, "@DSNO", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal

                        //db.ExecuteNonQuery(command);

                        //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        //intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        // Create the Journal
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                //db.ExecuteNonQuery(command, trans);
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
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
                                // To identify if journal entry is failed
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
            /// To get SJV related Depreciation details
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>

            public DataSet FunGetOperatingDepreciationDetails(string intSJVNo, int CompanyId)
            {
                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_GetOperatingDepreciationDetails");
                    db.AddInParameter(command, "@System_JV_Number", DbType.String, intSJVNo);
                    db.AddInParameter(command, "@Company_ID", DbType.Int16, CompanyId);
                    //db.LoadDataSet(command, ObjDS, "dtTable");
                    db.FunPubLoadDataSet(command, ObjDS, "dtTable");
                    return ObjDS;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// To Delete SJV related Depreciation details
            /// </summary>
            /// <param name="Asset_Verification_No">Pass PAN</param>
            /// <returns></returns>
            //public int FunPubDeleteAssetDepreciation(int intSJVNumber, int intCompany_ID)
            //{
            //    try
            //    {
            //        //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

            //        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_DeleteDepreciationDetail");
            //        db.AddInParameter(command, "@SJVNumber", DbType.Int32, intSJVNumber);
            //        db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompany_ID);
            //        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
            //        //db.ExecuteNonQuery(command);
            //        db.FunPubExecuteNonQuery(command);
            //        if ((int)command.Parameters["@ErrorCode"].Value > 0)
            //            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
            //    }
            //    catch (Exception ex)
            //    {
            //        intRowsAffected = 50;
            //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
            //        throw ex;
            //    }
            //    return intRowsAffected;

            //}
        }
    }
}