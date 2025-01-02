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

namespace S3GDALayer.LoanAdmin
{
    namespace ApprovalMgtServices
    {
        public class ClsPubLoanEndUseApproval
        {
            #region Declaration
            int intRowsAffected;
            S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable ObjLoanEndUseApproval_DataTable = new S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable();
            

            Database db;
            public ClsPubLoanEndUseApproval()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #endregion

            #region "Create Mode"

            public int FunPubCreateOrModifyLoanEndUseApproval(SerializationMode SerMode, byte[] bytesObjLoanEndUseApproval_DataTable, out string strEndUseNumber, out int intApproval_No)
            {
                strEndUseNumber = "";
                intApproval_No = 0;
                try
                {
                    ObjLoanEndUseApproval_DataTable = (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable)ClsPubSerialize.DeSerialize(bytesObjLoanEndUseApproval_DataTable, SerMode, typeof(S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalDataTable));
                    foreach (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3g_LOANAD_LoanEndUseApprovalRow ObjLoanEndUseApprovalRow in ObjLoanEndUseApproval_DataTable.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_INS_LoanEndUseApproval");

                        db.AddInParameter(command, "@End_Use_Number", DbType.String, ObjLoanEndUseApprovalRow.End_Use_Number);
                        db.AddInParameter(command, "@End_Use_Date", DbType.DateTime, ObjLoanEndUseApprovalRow.End_Use_Date);
                        db.AddInParameter(command, "@Loan_Approval_ID", DbType.Int32, ObjLoanEndUseApprovalRow.Loan_Approval_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLoanEndUseApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.String, ObjLoanEndUseApprovalRow.Location_Code);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLoanEndUseApprovalRow.LOB_ID);
                        db.AddInParameter(command, "@Customer_Code", DbType.Int32, ObjLoanEndUseApprovalRow.Customer_Code);
                        db.AddInParameter(command, "@PANUM", DbType.String, ObjLoanEndUseApprovalRow.PANUM);
                        db.AddInParameter(command, "@Components", DbType.String, ObjLoanEndUseApprovalRow.Components);
                        if (!(string.IsNullOrEmpty(ObjLoanEndUseApprovalRow.XML_Payment_Dtls)))
                            db.AddInParameter(command, "@XML_Payment_Dtls", DbType.String, ObjLoanEndUseApprovalRow.XML_Payment_Dtls);
                        if (!(string.IsNullOrEmpty(ObjLoanEndUseApprovalRow.XML_Approval_Dtls)))
                            db.AddInParameter(command, "@XML_Approval_Dtls", DbType.String, ObjLoanEndUseApprovalRow.XML_Approval_Dtls);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLoanEndUseApprovalRow.Created_By);
                        db.AddInParameter(command, "@Amount_Utilized_Code", DbType.Int32, ObjLoanEndUseApprovalRow.Amount_Utilized_Code);
                        db.AddInParameter(command, "@Option", DbType.Int32, ObjLoanEndUseApprovalRow.Option);


                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int32));
                        db.AddOutParameter(command, "@EndUseNumber", DbType.String, 50);
                        db.AddOutParameter(command, "@Approval_No", DbType.Int32, sizeof(Int32));

                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                // db.ExecuteNonQuery(command, trans);

                                db.FunPubExecuteNonQuery(command, ref trans);

                                // if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                strEndUseNumber = Convert.ToString(command.Parameters["@EndUseNumber"].Value);
                                intApproval_No = (int)command.Parameters["@Approval_No"].Value;
                                //if (intRowsAffected == 0)
                                trans.Commit();
                                //else
                                //    trans.Rollback();

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
