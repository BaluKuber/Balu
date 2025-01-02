using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using S3GBusEntity;
using S3GDALayer.S3GAdminServices;
using System;

namespace S3GDALayer.LoanAdmin
{
    public class ClsPubInterim
    {
         int intRowsAffected;

         S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimDataTable ObjInterim_DataTable = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimDataTable();
           Database db;
            public ClsPubInterim()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #region Create

            public int FunPubCreateOrModifyInterim(SerializationMode SerMode, byte[] bytesObjInterim_DataTable, out Int32 intInterim, out string strErrorMsg, out string StrInterimNumber)
            {
                strErrorMsg = "";
                intInterim =  0;
                StrInterimNumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjInterim_DataTable = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimDataTable)ClsPubSerialize.DeSerialize(bytesObjInterim_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimDataTable));
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3g_LAD_InterimRow ObjNoteRow in ObjInterim_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FUNDMGT_INS_Interim");
                        db.AddInParameter(command, "@Interim_id", DbType.Int64, ObjNoteRow.Interim_id);
                        db.AddInParameter(command, "@User_id", DbType.Int32, ObjNoteRow.user_id);
                        db.AddInParameter(command, "@customer_id", DbType.Int32, ObjNoteRow.customer_id);
                        db.AddInParameter(command, "@Tranche_id", DbType.Int32, ObjNoteRow.Tranche_id);
                       
                        db.AddInParameter(command, "@Interim_Date", DbType.DateTime, ObjNoteRow.Interim_Date);

                        db.AddInParameter(command, "@Due_Date", DbType.DateTime, ObjNoteRow.Due_Date);
                       
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjNoteRow.Company_ID);
                        db.AddOutParameter(command, "@Interim_Number", DbType.String, 1000);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        db.AddOutParameter(command, "@Intreim_ID_OUT", DbType.Int32, sizeof(Int32));

                        if (!ObjNoteRow._Is_Xml_InterimNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_Interim", OracleType.Clob,
                                    ObjNoteRow.Xml_Interim.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjNoteRow.Xml_Interim);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_Interim", DbType.String,
                                   ObjNoteRow.Xml_Interim);
                            }
                        }
                      
                       
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                intInterim = (int)command.Parameters["@Intreim_ID_OUT"].Value;
                                StrInterimNumber = (string)command.Parameters["@Interim_Number"].Value;
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
