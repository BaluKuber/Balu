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
        public class ClsPubNoteCreation
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationDataTable ObjNote_DataTable = new S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationDataTable();
            S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable ObjNote_DataTableAPP = new S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable();

            Database db;
            public ClsPubNoteCreation()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region Create

            public int FunPubCreateOrModifyNote(SerializationMode SerMode, byte[] bytesObjNote_DataTable, out Int64 intNote_No, out string strErrorMsg, out string StrNoteNumber)
            {
                strErrorMsg = "";
                intNote_No = 0;
                StrNoteNumber = "";
                try
                {
                    S3GDALDBType enumDBType = Common.ClsIniFileAccess.S3G_DBType;
                    ObjNote_DataTable = (S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationDataTable)ClsPubSerialize.DeSerialize(bytesObjNote_DataTable, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_Note_CreationRow ObjNoteRow in ObjNote_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_FUNDMGT_INS_Note");
                        db.AddInParameter(command, "@Note_Header_id", DbType.Int64, ObjNoteRow.Note_Header_id);
                        db.AddInParameter(command, "@User_id", DbType.Int32, ObjNoteRow.User_id);
                        db.AddInParameter(command, "@Funder_id", DbType.Int32, ObjNoteRow.Funder_id);
                        db.AddInParameter(command, "@Grace_Days", DbType.Int32, ObjNoteRow.Grace_days);
                        db.AddInParameter(command, "@Funder_Bank_id", DbType.Int32, ObjNoteRow.Funder_bank_id);
                        db.AddInParameter(command, "@Customer_id", DbType.Int32, ObjNoteRow.Customer_id);
                        db.AddInParameter(command, "@note_date", DbType.DateTime, ObjNoteRow.Note_date);
                        db.AddInParameter(command, "@NOD_Date", DbType.DateTime, ObjNoteRow.NOD_Date);
                        db.AddInParameter(command, "@Disb_Date", DbType.DateTime, ObjNoteRow.Disb_Date);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjNoteRow.Company_id);
                        db.AddInParameter(command, "@IS_DSRA", DbType.Int32, ObjNoteRow.IS_DSRA);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjNoteRow.Location_ID);
                        db.AddOutParameter(command, "@Note_Number", DbType.String, 1000);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);
                        db.AddOutParameter(command, "@Note_ID_OUT", DbType.Int64, sizeof(Int64));
                     

                        if (!ObjNoteRow._Is_XML_TrancheNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_Tranche", OracleType.Clob,
                                    ObjNoteRow.XML_Tranche.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjNoteRow.XML_Tranche);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_Tranche", DbType.String,
                                   ObjNoteRow.XML_Tranche);
                            }
                        }
                        if (!ObjNoteRow._Is_XML_CashflowNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_Cashflow", OracleType.Clob,
                                    ObjNoteRow.XML_Cashflow.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjNoteRow.XML_Cashflow);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_Cashflow", DbType.String,
                                   ObjNoteRow.XML_Cashflow);
                            }
                        }
                        if (!ObjNoteRow._Is_XML_SanctionNull())
                        {
                            if (enumDBType == S3GDALDBType.ORACLE)
                            {
                                OracleParameter param = new OracleParameter("@XML_Sanction", OracleType.Clob,
                                    ObjNoteRow.XML_Sanction.Length, ParameterDirection.Input, true,
                                    0, 0, String.Empty, DataRowVersion.Default, ObjNoteRow.XML_Sanction);
                                command.Parameters.Add(param);
                            }
                            else
                            {
                                db.AddInParameter(command, "@XML_Sanction", DbType.String,
                                   ObjNoteRow.XML_Sanction);
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

                                intNote_No = (Int64)command.Parameters["@Note_ID_OUT"].Value;
                                StrNoteNumber = (string)command.Parameters["@Note_Number"].Value;
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

            public int FunPubInsertNoteApproval(SerializationMode SerMode, byte[] bytesObjNote_DataTableAPP, out string strNoteApprovalNumber, out Int64 intNoteApproval_No)
            {
                strNoteApprovalNumber = "";
                intNoteApproval_No = 0;
                try
                {
                    ObjNote_DataTableAPP = (S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable)ClsPubSerialize.DeSerialize(bytesObjNote_DataTableAPP, SerMode, typeof(S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALDataTable));
                    foreach (S3GBusEntity.FundManagement.FundMgtServices.S3G_ORG_NOTEAPPROVALRow ObjNoteApprovalRow in ObjNote_DataTableAPP.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_INSERT_NOTEAPPROVAL");

                        db.AddInParameter(command, "@NOTEApproval_ID", DbType.Int32, ObjNoteApprovalRow.NOTEApproval_ID);
                        db.AddInParameter(command, "@NOTE_ID", DbType.Int32, ObjNoteApprovalRow.NOTE_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjNoteApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Approver_ID", DbType.Int32, ObjNoteApprovalRow.Approver_ID);
                        db.AddInParameter(command, "@Approval_Date", DbType.DateTime, ObjNoteApprovalRow.Approval_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjNoteApprovalRow.Remarks);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.Int32, ObjNoteApprovalRow.Approval_Serial_Number);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjNoteApprovalRow.Action_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjNoteApprovalRow.Created_By);


                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@NOTEApprovalNumber", DbType.String, 50);
                        db.AddOutParameter(command, "@NOTEApproval_ID_OUT", DbType.Int64, sizeof(Int64));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
           
                                db.FunPubExecuteNonQuery(command, ref trans);
                  
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strNoteApprovalNumber = Convert.ToString(command.Parameters["@NOTEApprovalNumber"].Value);
                                intNoteApproval_No = (Int64)command.Parameters["@NOTEApproval_ID_OUT"].Value;

                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    trans.Rollback();
                                }
                                else
                                {
                                    trans.Commit();
                                }
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
