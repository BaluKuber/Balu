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

namespace S3GDALayer.Origination
{
    namespace OrgMasterMgtServices
    {

        public class ClsPubBankStatement
        {
            int intRowsAffected;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsDataTable objBankStatementLOV_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLDataTable objBankListXML_DAL;
            S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable objBankStatemntDataCapture_DAL;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubBankStatement()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #region "Bank Statemnt Capture"

            /// <summary>
            /// Inserting the Bank statement List values
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            public int FunPubCreateBankStatemntListValues(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ListValues_XMLDataTable)
            {
                try
                {
                    objBankListXML_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ListValues_XMLDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_ListValues_XMLRow ObjBankXMLRow in objBankListXML_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Insert_BankStatement_LOVDetails");
                        db.AddInParameter(command, "@Bank_Stmt_Data_Capture_ID", DbType.Int32, ObjBankXMLRow.Bank_Stmt_Data_Capture_ID);
                        db.AddInParameter(command, "@Bank_Stmt_LOV_ID", DbType.Int32, ObjBankXMLRow.Bank_Stmt_LOV_ID);


                        //Added By Kuppusamy.B on 07/Apr/2012 to change the XML to CLOB
                        S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                        if (enumDBType == S3GDALDBType.ORACLE)
                        {
                            OracleParameter param = new OracleParameter("@XMLListValues",
                                   OracleType.Clob, ObjBankXMLRow.XMLListValues.Length,
                                   ParameterDirection.Input, true, 0, 0, String.Empty,
                                   DataRowVersion.Default, ObjBankXMLRow.XMLListValues);
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            db.AddInParameter(command, "@XMLListValues", DbType.String, ObjBankXMLRow.XMLListValues);
                        }

                        //db.AddInParameter(command, "@XMLListValues", DbType.String, ObjBankXMLRow.XMLListValues);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected == 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();

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
            /// Inserting the Bank statement details such as LOB, Product , the bank statement ..
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>


            public int FunPubCreateBankStatemntData(SerializationMode SerMode, byte[] bytesObjS3G_ORG_BankStatemnt_CaptureDataTable, int intUpdateBank_ID)
            {
                try
                {
                    DbCommand command;
                    objBankStatemntDataCapture_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_BankStatemnt_CaptureDataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_Data_CaptureRow ObjBankDataRow in objBankStatemntDataCapture_DAL.Rows)
                    {
                        command = db.GetStoredProcCommand("S3G_Insert_BankStatement_DataCapture");
                        db.AddInParameter(command, "@Bank_Stmt_Capture_ID", DbType.Int32, ObjBankDataRow.Bank_Stmt_Data_Capture_ID);
                        if (intUpdateBank_ID == 0)
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjBankDataRow.LOB_ID);
                            db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjBankDataRow.Product_ID);
                            db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjBankDataRow.Constitution_ID);
                            db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjBankDataRow.Company_ID);
                            db.AddInParameter(command, "@FlatFile_Path", DbType.String, ObjBankDataRow.FlatFile_Path);
                            db.AddInParameter(command, "@Scan_Download", DbType.Boolean, ObjBankDataRow.Scan_Download);

                            //Added By Kuppusamy.B on 07/Apr/2012 to change the XML to CLOB
                            S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XMLListValues",
                                       OracleType.Clob, ObjBankDataRow.XMLListValues.Length,
                                       ParameterDirection.Input, true, 0, 0, String.Empty,
                                       DataRowVersion.Default, ObjBankDataRow.XMLListValues);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XMLListValues", DbType.String, ObjBankDataRow.XMLListValues);
                            }

                            //db.AddInParameter(command, "@XMLListValues", DbType.String, ObjBankDataRow.XMLListValues);
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjBankDataRow.Created_By);
                            db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjBankDataRow.Created_On);
                        }
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjBankDataRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjBankDataRow.Modified_On);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        //db.FunPubExecuteNonQuery(command);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                                if (intRowsAffected == 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();
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
            /// Gets the Bank List Values using Serialized data table object and serialized mode
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns>Datatable containing Bank statement list values</returns>

            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Get_Bank_Statement_LOVDetailsDataTable FunPubQueryBankStatemntCopyListValues(int Bank_ID)
            {
                S3GBusEntity.Origination.OrgMasterMgtServices dtListValueTable = null;
                try
                {
                    dtListValueTable = new S3GBusEntity.Origination.OrgMasterMgtServices();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Bank_Statement_LOVDetails");
                    db.AddInParameter(command, "@Bank_Stmt_Data_Capture_ID", DbType.Int32, Bank_ID);
                    db.FunPubLoadDataSet(command, dtListValueTable, dtListValueTable.S3G_Get_Bank_Statement_LOVDetails.TableName);
                    return dtListValueTable.S3G_Get_Bank_Statement_LOVDetails;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                finally
                {
                    dtListValueTable.Dispose();
                    dtListValueTable = null;
                }

            }

            /// <summary>
            /// Gets the Bank statement data capture ID
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns>Datatable containing Bank Capture ID from the Bank statement table from the DB</returns>

            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Get_Bank_Statement_IDDataTable FunPubGetBankStatemnt_ID()
            {

                S3GBusEntity.Origination.OrgMasterMgtServices dtBankIDTable = null;
                try
                {
                    dtBankIDTable = new S3GBusEntity.Origination.OrgMasterMgtServices();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Bank_Statement_ID");
                    db.FunPubLoadDataSet(command, dtBankIDTable, dtBankIDTable.S3G_Get_Bank_Statement_ID.TableName);
                    return dtBankIDTable.S3G_Get_Bank_Statement_ID;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;

                }
                finally
                {
                    dtBankIDTable.Dispose();
                    dtBankIDTable = null;
                }
            }

            public S3GBusEntity.Origination.OrgMasterMgtServices.S3G_Get_Bank_Stmt_Data_CaptureDetailsDataTable FunPubQueryBankStatemntData(int CompanyID, int CreatedBy)
            {

                S3GBusEntity.Origination.OrgMasterMgtServices dtBankDataTable = null;
                try
                {
                    dtBankDataTable = new S3GBusEntity.Origination.OrgMasterMgtServices();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Get_Bank_Stmt_Data_CaptureDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, CompanyID);
                    db.AddInParameter(command, "@Created_By", DbType.Int32, CreatedBy);
                    db.FunPubLoadDataSet(command, dtBankDataTable, dtBankDataTable.S3G_Get_Bank_Stmt_Data_CaptureDetails.TableName);
                    return dtBankDataTable.S3G_Get_Bank_Stmt_Data_CaptureDetails;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }


            }

            public int FunPubDeleteBankStatemntListvalues(SerializationMode SerMode, byte[] bytesObjS3G_ORG_ListValues_DataTable)
            {
                try
                {
                    objBankStatementLOV_DAL = (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_ListValues_DataTable, SerMode, typeof(S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_Delete_Bank_Listvalues");
                    foreach (S3GBusEntity.Origination.OrgMasterMgtServices.S3G_ORG_Bank_Statement_LOVDetailsRow ObjLOVDetailsRow in objBankStatementLOV_DAL.Rows)
                    {
                        db.AddInParameter(command, "@Bank_Stmt_Capture_ID", DbType.Int32, ObjLOVDetailsRow.Bank_Stmt_Data_Capture_ID);
                        db.AddInParameter(command, "@Bank_Stmt_LOV_ID", DbType.Int32, ObjLOVDetailsRow.Bank_Stmt_LOV_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.FunPubExecuteNonQuery(command);
                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;

            }


            #endregion
        }
    }
}