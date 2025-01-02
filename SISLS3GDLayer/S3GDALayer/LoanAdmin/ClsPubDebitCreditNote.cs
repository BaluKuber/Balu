using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;
using S3GDALayer.S3GAdminServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace S3GDALayer.LoanAdmin
{
    namespace LoanAdminAccMgtServices
    {
        public class ClsPubDebitCreditNote
        {
            int intRowsAffected;
            //S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_DebitCreditNoteDataTable ObjDCNDT;
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTDataTable ObjCRDRDT;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubDebitCreditNote()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }           

            public int FunPubInsertDebitCredit(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_CRDRDataTable, out string strMJVNumber)//Crdeit Debit Note
            {
                strMJVNumber = string.Empty;
                try
                {
                    ObjCRDRDT = (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_LOANAD_CRDRDataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTDataTable));
                    foreach (S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_CRDRDTRow ObjMJVRow in ObjCRDRDT.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LAD_DCN_Insertion");
                        db.AddInParameter(command, "@User_Id", DbType.Int32, ObjMJVRow.USRID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjMJVRow.CompanyId);
                        db.AddInParameter(command, "@Location_Id", DbType.Int32, ObjMJVRow.Location_Id);
                        if (!ObjMJVRow.IsTran_TypeNull())
                            db.AddInParameter(command, "@TxnType", DbType.Int32, ObjMJVRow.Tran_Type);
                        db.AddInParameter(command, "@TranSubType", DbType.Int32, ObjMJVRow.TranSubType);
                        db.AddInParameter(command, "@Doc_Id", DbType.Int32, ObjMJVRow.DocumentNo);
                        db.AddInParameter(command, "@DocumentDate", DbType.DateTime, ObjMJVRow.Document_Date);
                        db.AddInParameter(command, "@EntityType", DbType.Int32, ObjMJVRow.Entity_Type);
                        db.AddInParameter(command, "@EntityId", DbType.Int32, ObjMJVRow.Entity_Id);
                        db.AddInParameter(command, "@EntityCode", DbType.String, ObjMJVRow.Entity_Code);
                        db.AddInParameter(command, "@TxtnDate", DbType.DateTime, ObjMJVRow.Txn_Date);
                        db.AddInParameter(command, "@TxnAmount", DbType.Decimal, ObjMJVRow.TxnAmoun);
                        db.AddInParameter(command, "@AccountCode", DbType.String, ObjMJVRow.Account_Code);
                        db.AddInParameter(command, "@Billing_Address_ID", DbType.Int32, ObjMJVRow.Billing_Address_ID); // Added by kalaivanan

                        if (!ObjMJVRow.IsIS_DELETENull())
                        {
                            db.AddInParameter(command, "@IS_DELETE", DbType.String, ObjMJVRow.IS_DELETE);
                        }
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjMJVRow.Remarks);
                        db.AddInParameter(command, "@GLAccount", DbType.String, ObjMJVRow.GL_Account);
                        db.AddInParameter(command, "@SLAccount", DbType.String, ObjMJVRow.SL_Account);
                        db.AddInParameter(command, "@Lob_Id", DbType.String, ObjMJVRow.Lob_Id);                        
                        if (!ObjMJVRow.IsInvoice_NoNull())
                            db.AddInParameter(command, "@Invoice_No", DbType.String, ObjMJVRow.Invoice_No);
                        db.AddInParameter(command, "@XMLAccountDetail", DbType.String, ObjMJVRow.XMlAccountDetails);

                        db.AddInParameter(command, "@Due_Date", DbType.DateTime, ObjMJVRow.Due_Date);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@CRDRNO_OUT", DbType.String, 500);

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if ((int)command.Parameters["@ErrorCode"].Value != 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    if (intRowsAffected == 3)
                                        trans.Commit();
                                    else
                                        trans.Rollback();
                                }
                                else
                                {
                                    strMJVNumber = command.Parameters["@CRDRNO_OUT"].Value.ToString();
                                    trans.Commit();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
        
        }
    }
}
