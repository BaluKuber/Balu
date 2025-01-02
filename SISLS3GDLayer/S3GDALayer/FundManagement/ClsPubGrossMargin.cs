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
        public class ClsPubGrossMarginCreation
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginDataTable ObjGM_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginDataTable();
            Database db;
            public ClsPubGrossMarginCreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create

            public int FunPubCreateOrModifyGrossMarginCalculation(SerializationMode SerMode, byte[] bytesObjGM_DataTable, out Int64 intGM_No, out string strErrorMsg, out string StrGMNumber)
            {
                strErrorMsg = "";
                intGM_No = 0;
                StrGMNumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjGM_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginDataTable)ClsPubSerialize.DeSerialize(bytesObjGM_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_Fundmgt_GrossMarginRow ObjGMRow in ObjGM_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FundMgt_Ins_GrossMargin");
                        db.AddInParameter(command, "@Company_id", DbType.Int32, ObjGMRow.Company_id);
                        db.AddInParameter(command, "@User_id", DbType.Int32, ObjGMRow.User_id);
                        db.AddInParameter(command, "@GrossMargin_Id", DbType.Int32, ObjGMRow.GrossMargin_Id);
                        db.AddInParameter(command, "@GrossMargin_Date", DbType.DateTime, ObjGMRow.GrossMargin_Date);
                        db.AddInParameter(command, "@Tranche_Header_Id", DbType.Int32, ObjGMRow.Tranche_Header_Id);
                        db.AddInParameter(command, "@PA_SA_REF_ID", DbType.Int32, ObjGMRow.PA_SA_REF_ID);
                        db.AddInParameter(command, "@Invoice_Value", DbType.Decimal, ObjGMRow.Invoice_Value);
                        db.AddInParameter(command, "@RS_Value", DbType.Decimal, ObjGMRow.RS_Value);
                        db.AddInParameter(command, "@Rental_Start_Date", DbType.DateTime, ObjGMRow.Rental_Start_Date);
                        db.AddInParameter(command, "@Rental_End_date", DbType.DateTime, ObjGMRow.Rental_End_date);
                        db.AddInParameter(command, "@ITC_Available", DbType.Decimal, ObjGMRow.ITC_Available);
                        db.AddInParameter(command, "@Rebate_Discount_Value", DbType.Decimal, ObjGMRow.Rebate_Discount_Value);
                        db.AddInParameter(command, "@Discounting_Rate", DbType.Decimal, ObjGMRow.Discounting_Rate);
                        db.AddInParameter(command, "@Security_Deposit", DbType.Decimal, ObjGMRow.Security_Deposit);
                        db.AddInParameter(command, "@EOT_Guaranteed_Value", DbType.Decimal, ObjGMRow.EOT_Guaranteed_Value);
                        db.AddInParameter(command, "@Gross_Margin", DbType.Decimal, ObjGMRow.Gross_Margin);

                        if (!ObjGMRow._Is_XML_GrossMarginNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_GrossMargin", OracleType.Clob,
                                    ObjGMRow.XML_GrossMargin.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjGMRow.XML_GrossMargin);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_GrossMargin", DbType.String,
                                   ObjGMRow.XML_GrossMargin);
                            }
                        }

                        db.AddOutParameter(command, "@GrossMargin_Number", DbType.String, 1000);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        db.AddOutParameter(command, "@GrossMargin_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                intGM_No = (Int64)command.Parameters["@GrossMargin_ID_OUT"].Value;
                                StrGMNumber = (string)command.Parameters["@GrossMargin_Number"].Value;
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
