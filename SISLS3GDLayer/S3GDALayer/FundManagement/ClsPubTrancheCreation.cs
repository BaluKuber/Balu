using System;
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
using S3GDALayer.S3GAdminServices;
using System.Data.OracleClient;

namespace S3GDALayer.FundManagement
{
    namespace FundMgtService
    {
        public class ClsPubTrancheCreation
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable ObjTranche_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable();
            

            Database db;
            public ClsPubTrancheCreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create

            public int FunPubCreateOrModifyTrancheMaster(SerializationMode SerMode, byte[] bytesObjTranche_DataTable,  out Int64 intTranche_No, out string strErrorMsg)
            {
                 strErrorMsg = "";
                intTranche_No = 0;
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjTranche_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable)ClsPubSerialize.DeSerialize(bytesObjTranche_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_Tranche_CreationRow ObjTrancheRow in ObjTranche_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FUNDMGT_INS_Tranche");
                        db.AddInParameter(command, "@Tranche_id", DbType.Int64, ObjTrancheRow.Tranche_Header_id);
                        db.AddInParameter(command, "@Funder_ID", DbType.Int32, ObjTrancheRow.Funder_Id);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTrancheRow.Company_id);
                        db.AddInParameter(command, "@Created_On", DbType.String, ObjTrancheRow.Created_On);
                        db.AddInParameter(command, "@Created_by", DbType.String, ObjTrancheRow.Created_by);
                        db.AddInParameter(command, "@Customer_id", DbType.Int32, ObjTrancheRow.Customer_id);
                        db.AddInParameter(command, "@Tranche_name", DbType.String, ObjTrancheRow.Tranche_name);
                        db.AddInParameter(command, "@Tranche_date", DbType.String, ObjTrancheRow.Tranche_date);
                      //  db.AddInParameter(command, "@Status_ID", DbType.String, ObjTrancheRow.Status_ID);
                        db.AddInParameter(command, "@Disbursement_Date", DbType.String, ObjTrancheRow.Disbursement_Date);
                        db.AddInParameter(command, "@Tenure", DbType.Int32, ObjTrancheRow.Tenure);

                        db.AddInParameter(command, "@Invoice_Cov_Letter", DbType.Int32, ObjTrancheRow.Invoice_Cov_Letter);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        db.AddOutParameter(command, "@Tranche_ID_OUT", DbType.Int64, sizeof(Int64));
                       
                        if (!ObjTrancheRow._Is_xmlCashflowNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@xmlCashflow", OracleType.Clob,
                                    ObjTrancheRow.xmlCashflow.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.xmlCashflow);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@xmlCashflow", DbType.String,
                                   ObjTrancheRow.xmlCashflow);
                            }
                        }
                        if (!ObjTrancheRow._Is_xmlTrancheDetailNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@xmlTrancheDetail", OracleType.Clob,
                                    ObjTrancheRow.xmlTrancheDetail.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.xmlTrancheDetail);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@xmlTrancheDetail", DbType.String,
                                   ObjTrancheRow.@xmlTrancheDetail);
                            }
                        }
                        if (!ObjTrancheRow._Is_xmlTrancheDetailArcheiveNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@xmlTrancheDetailArcheive", OracleType.Clob,
                                    ObjTrancheRow.xmlTrancheDetailArcheive.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.xmlTrancheDetailArcheive);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@xmlTrancheDetailArcheive", DbType.String,
                                   ObjTrancheRow.xmlTrancheDetailArcheive);
                            }
                        }
                        if (!ObjTrancheRow._Is_xmlTrancheamortNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@xmlTrancheamort", OracleType.Clob,
                                    ObjTrancheRow.xmlTrancheamort.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.xmlTrancheamort);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@xmlTrancheamort", DbType.String,
                                   ObjTrancheRow.xmlTrancheamort);
                            }
                        }
                        if (ObjTrancheRow.XmlFollowupDetail!=null)
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XmlFollowupDetail", OracleType.Clob,
                                    ObjTrancheRow.XmlFollowupDetail.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.XmlFollowupDetail);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XmlFollowupDetail", DbType.String,
                                   ObjTrancheRow.XmlFollowupDetail);
                            }
                        }
                        if (ObjTrancheRow.XmlAlertDetails != null)
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XmlAlertDetails", OracleType.Clob,
                                    ObjTrancheRow.XmlAlertDetails.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjTrancheRow.XmlAlertDetails);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XmlAlertDetails", DbType.String,
                                   ObjTrancheRow.XmlAlertDetails);
                            }
                        }

                        if (ObjTrancheRow.XmlCustEmailDetails != null)
                        {
                            db.AddInParameter(command, "@XmlCustEmailDetails", DbType.String,
                                   ObjTrancheRow.XmlCustEmailDetails);
                        }

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                               
                                intTranche_No = (Int64)command.Parameters["@Tranche_ID_OUT"].Value;
                                strErrorMsg = Convert.ToString(command.Parameters["@ErrorMsg"].Value);

                                trans.Commit();

                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 50;
                                trans.Rollback();
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    }
                }
                catch (Exception objException)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(objException);
                }
                return intRowsAffected;
            }


            #endregion

           
        }
    }
}
