using System;using S3GDALayer.S3GAdminServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORG = S3GBusEntity.Origination;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using S3GBusEntity;
using System.Data;

namespace S3GDALayer.LegalRepossession
{
    namespace LegalAndRepossessionMgtServices
    {
        public class ClsPubLegalRepossessionApproval : ClsPubDalLegalRepossessionBase
        {

            int intRowsAffected = 0;

            S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable objS3G_LR_Approval_DataTable;

            public int FunPubCreateLegalRepossessionApprovals(SerializationMode SerMode, byte[] bytesApproval_Datatable, out string strErrMsg)
            {
                strErrMsg = string.Empty;
                try
                {
                    objS3G_LR_Approval_DataTable = (S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable)ClsPubSerialize.DeSerialize(bytesApproval_Datatable, SerMode, typeof(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalDataTable));

                    //ObjS3G_ORG_Approval_DataTable = (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable)ClsPubSerialize.DeSerialize(bytesApproval_Datatable, SerMode, typeof(S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable));
                     
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertLRApproval");
                    
                    foreach(S3GBusEntity.LegalRepossession.LegalRepossessionMgtServices.S3G_LR_ApprovalRow ObjLRApprovalRow in objS3G_LR_Approval_DataTable.Rows)
                    {
                        //db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLRApprovalRow.LOB_ID);
                        //db.AddInParameter(command, "@Branch_ID", DbType.Int32, ObjLRApprovalRow.Branch_ID);
                        //db.AddInParameter(command, "@Approval_ID", DbType.Int32, ObjLRApprovalRow.Approval_ID);
                        //db.AddInParameter(command, "@Task_Number", DbType.String, ObjLRApprovalRow.Task_Number);
                        //db.AddInParameter(command, "@Task_Type", DbType.String, ObjLRApprovalRow.Task_Type);
                        //db.AddInParameter(command, "@Task_Status_Type_Code", DbType.Int32, ObjLRApprovalRow.Task_Status_Type_Code);
                        //db.AddInParameter(command, "@Task_Status_Code", DbType.String, ObjLRApprovalRow.Task_Status_Code);
                        //db.AddInParameter(command, "@Approval_Serial_Number", DbType.String, ObjLRApprovalRow.Task_Approval_Serialvalue);
                        //db.AddInParameter(command, "@Remarks", DbType.String, ObjLRApprovalRow.Remarks);
                        //db.AddInParameter(command, "@Password", DbType.String, ObjLRApprovalRow.Password);
                        //db.AddInParameter(command, "@Company_ID", DbType.String, ObjLRApprovalRow.Company_ID);
                        //db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLRApprovalRow.Created_By);
                        //db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                    
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.ExecuteNonQuery(command, trans);
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                }
                                if (intRowsAffected == 0)
                                    trans.Commit();
                                else
                                    trans.Rollback();
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                // To identify if journal entry is failed
                                if (command.Parameters["@ErrorCode"].Value != null)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                    strErrMsg = (string)command.Parameters["@ErrorMsg"].Value;
                                }
                                else
                                {
                                    intRowsAffected = 20;
                                }
                                trans.Rollback();
                                 ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                            }
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }

         
        }
    }
}
