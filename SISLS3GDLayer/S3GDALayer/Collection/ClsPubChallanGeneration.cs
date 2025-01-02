using System;
using S3GDALayer.S3GAdminServices;
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

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {
        public class ClsPubChallanGeneration
        {
            int intRowsAffected;

            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanGenerationDataTable objChallanGeneration_DAl;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubChallanGeneration()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            /// <summary>
            /// Inserting the Appropriation logic Details
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name=""></param>
            /// <returns></returns>
            /// 
            public int FunPubCreateChallanGenerationLogic(SerializationMode SerMode, byte[] bytesObjS3G_Colection_ChallanGeneration_DataTable, out string strChallanNumber)
            {
                strChallanNumber = "";
                try
                {
                    objChallanGeneration_DAl = (S3GBusEntity.Collection.ClnReceiptMgtServices.
                        S3G_CLN_ChallanGenerationDataTable)ClsPubSerialize.
                        DeSerialize(bytesObjS3G_Colection_ChallanGeneration_DataTable,
                        SerMode, typeof(S3GBusEntity.Collection.
                        ClnReceiptMgtServices.S3G_CLN_ChallanGenerationDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanGenerationRow objChallanGenerationRow in objChallanGeneration_DAl)
                    {
                        DbCommand command = db.GetStoredProcCommand(SPNames.S3G_CLN_InsertChallanGeneration);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, objChallanGenerationRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, objChallanGenerationRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, objChallanGenerationRow.Branch_ID);
                        db.AddInParameter(command, "@Challan_No", DbType.String, objChallanGenerationRow.Challan_No);
                        db.AddInParameter(command, "@Deposit_Bank_Code", DbType.String, objChallanGenerationRow.Deposit_Bank_Code);
                        db.AddInParameter(command, "@Challan_RuleDetails_ID", DbType.Int64, objChallanGenerationRow.Challan_RuleDetails_ID);
                        db.AddInParameter(command, "@Challan_Date", DbType.DateTime, objChallanGenerationRow.Challan_Date);
                        db.AddInParameter(command, "@Challan_Amount", DbType.Decimal, objChallanGenerationRow.Challan_Amount);
                        db.AddInParameter(command, "@Account_Link_Key", DbType.Int32, objChallanGenerationRow.Account_Link_Key);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, objChallanGenerationRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, objChallanGenerationRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, objChallanGenerationRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, objChallanGenerationRow.Modified_On);
                        db.AddInParameter(command, "@Txn_ID", DbType.Int32, objChallanGenerationRow.Txn_ID);

                        //Added By Kuppusamy.B on 30/Mar/2012 to change the XML to CLOB
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLDetail",
                                   OracleType.Clob, objChallanGenerationRow.XMLDetails.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, objChallanGenerationRow.XMLDetails);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLDetail", DbType.String, objChallanGenerationRow.XMLDetails);
                        }



                        db.AddInParameter(command, "@Challan_Type_Code", DbType.Int32, objChallanGenerationRow.Challan_Type_Code);
                        db.AddInParameter(command, "@Challan_Type", DbType.Int32, objChallanGenerationRow.Challan_Type);
                        db.AddInParameter(command, "@Instrument_Type", DbType.Int32, objChallanGenerationRow.Instrument_Type);
                        db.AddInParameter(command, "@Drawee_Bank_Name", DbType.String, objChallanGenerationRow.Drawee_Bank_Name);
                        db.AddInParameter(command, "@Past_Receipt", DbType.String, objChallanGenerationRow.Past_Receipt);
                        //db.AddInParameter(command, "@Receipt_Date", DbType.DateTime, objChallanGenerationRow.Receipt_Date);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@strChallanNo", DbType.String, 100);

                        //db.FunPubExecuteNonQuery(command);

                        //intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                        //if (command.Parameters["@strChallanNo"].Value != null)
                        //{
                        //    strChallanNumber = command.Parameters["@strChallanNo"].Value.ToString();
                        //}

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    //throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
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
                                    trans.Commit();
                                    strChallanNumber = Convert.ToString(command.Parameters["@Challan_No"].Value);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (intRowsAffected == 0)
                                    intRowsAffected = 50;
                                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                                trans.Rollback();
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    //if ((int)command.Parameters["@ErrorCode"].Value == 0)
                    //{
                    //    strChallanNo = Convert.ToString(command.Parameters["@strChallanNo"].Value);
                    //}
                    //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                    //----------------------------------------------------------
                    //using (DbConnection conn = db.CreateConnection())
                    //{
                    //    conn.Open();
                    //    DbTransaction trans = conn.BeginTransaction();
                    //    try
                    //    {
                    //        db.ExecuteNonQuery(command, trans);
                    //        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                    //        {
                    //            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    //            throw new Exception("Error thrown Error No" + intRowsAffected.ToString());
                    //        }
                    //        else if ((int)command.Parameters["@ErrorCode"].Value < 0)
                    //        {
                    //            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    //            if (intRowsAffected == -1)
                    //                throw new Exception("Document Sequence no not-defined");
                    //            if (intRowsAffected == -2)
                    //                throw new Exception("Document Sequence no exceeds defined limit");
                    //        }
                    //        else
                    //        {
                    //            trans.Commit();
                    //            strChallanNumber = Convert.ToString(command.Parameters["@Challan_No"].Value);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        if (intRowsAffected == 0)
                    //            intRowsAffected = 50;
                    //         ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    //        trans.Rollback();
                    //    }
                    //    finally
                    //    { 
                    //        conn.Close();
                    //    }
                    //} 
                    //--------------------------------------------------------------------

                    //    }
                }
                catch (Exception ex)
                {
                    if (intRowsAffected == 0) //Bug Fixing by Palani Kumar.A on 21/02/2014 for doc number issue
                        intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }


                return intRowsAffected;

            }

        }
    }
}
