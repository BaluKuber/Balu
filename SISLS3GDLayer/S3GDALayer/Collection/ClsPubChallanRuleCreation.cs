#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Challan Rule creation DAL class
/// Created By			: Irsathameen K
/// Created Date		: 13-Oct-2010
/// Purpose	            : To access challan Rule  db methods

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

namespace S3GDALayer.Collection
{
    namespace ClnReceiptMgtServices
    {

       public class ClsPubChallanRuleCreation
        {
            int intRowsAffected;

            S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanRuleDetailsDataTable objChallanRule_DAL;

             //Code added for getting common connection string  from config file
            Database db;
            public ClsPubChallanRuleCreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            
            public int FunPubCreateChallanRuleDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_ChallanRuleDataTable, out string strChallanNo)
            {
                try
                {
                    strChallanNo = "";
                    objChallanRule_DAL = (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanRuleDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_CLN_ChallanRuleDataTable, SerMode, typeof(S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanRuleDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Collection.ClnReceiptMgtServices.S3G_CLN_ChallanRuleDetailsRow ObjChallanRuleRow in objChallanRule_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_CLN_InsertChallanRule");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjChallanRuleRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjChallanRuleRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjChallanRuleRow.Branch_ID);
                        db.AddInParameter(command, "@Is_Active", DbType.Boolean, ObjChallanRuleRow.Is_Active);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjChallanRuleRow.Created_By);
                        db.AddInParameter(command, "@Deposit_Bank_Code", DbType.Int32, ObjChallanRuleRow.Deposit_Bank_Code);

                        if (!ObjChallanRuleRow.IsXMLDraweeBankNull())
                        {
                            //Added By Kuppusamy.B on 12/April/2012 to change the XML to CLOB
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XMLDraweeBank",
                                       OracleType.Clob, ObjChallanRuleRow.XMLDraweeBank.Length,
                                       ParameterDirection.Input, true, 0, 0, String.Empty,
                                       DataRowVersion.Default, ObjChallanRuleRow.XMLDraweeBank);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XMLDraweeBank", DbType.String, ObjChallanRuleRow.XMLDraweeBank);
                            }
                        }

                        //if (!ObjChallanRuleRow.IsXMLDraweeBankNull())
                        //{ db.AddInParameter(command, "@XMLDraweeBank", DbType.String, ObjChallanRuleRow.XMLDraweeBank); }

                        if (!ObjChallanRuleRow.IsXMLDeletedDraweeBankNull())
                        { 
                            //Added By Kuppusamy.B on 12/April/2012 to change the XML to CLOB
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XMLDeletedDraweeBank",
                                       OracleType.Clob, ObjChallanRuleRow.XMLDeletedDraweeBank.Length,
                                       ParameterDirection.Input, true, 0, 0, String.Empty,
                                       DataRowVersion.Default, ObjChallanRuleRow.XMLDeletedDraweeBank);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XMLDeletedDraweeBank", DbType.String, ObjChallanRuleRow.XMLDeletedDraweeBank);
                            }
                        }

                        db.AddInParameter(command, "@ChallabRule_ID", DbType.Int32, ObjChallanRuleRow.ChallabRule_ID);
                        db.AddOutParameter(command, "@ChallanRule_No", DbType.String, 100);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command,ref trans);
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
                                    trans.Commit();
                                    strChallanNo = Convert.ToString(command.Parameters["@ChallanRule_No"].Value);
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
                            { conn.Close(); }
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

        }
    }
}
