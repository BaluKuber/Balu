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
namespace S3GDALayer.LoanAdmin
{
    namespace ApprovalMgtServices
    {
        public class ClsPubApprovals
        {
           
            int intRowsAffected = 0;

            S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable ObjS3G_ORG_Approval_DataTable;

            S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrDataTable  ObjS3G_ORG_RVMHdr_DataTable;


            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubApprovals()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            public int FunPubCreateRVMatrix(SerializationMode SerMode, byte[] bytesApproval_Datatable, out string strErrMsg)
            {
                strErrMsg = string.Empty;
                try
                {

                    ObjS3G_ORG_RVMHdr_DataTable = (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrDataTable)ClsPubSerialize.DeSerialize(bytesApproval_Datatable, SerMode, typeof(S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_RVMAT_ADD");
                    foreach (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_ORG_RV_Matrix_HdrRow ObjApprovalRow in ObjS3G_ORG_RVMHdr_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApprovalRow.LOB_ID);
                        db.AddInParameter(command, "@XMLRVMatrix", DbType.String, ObjApprovalRow.XML_GetRVMHeader);
                        db.AddInParameter(command, "@XmlMatRange", DbType.String, ObjApprovalRow.XmlMatRange);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@RV_HCODE", DbType.Int32, sizeof(Int64));
                        db.AddInParameter(command, "@USRID", DbType.Int32, ObjApprovalRow.User_ID);
                        db.AddInParameter(command, "@COMPID", DbType.Int32, ObjApprovalRow.COMPID);
                    }

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if (command.Parameters["@ErrorCode"].Value != null)
                            {
                                intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
                catch (Exception ex)
                {
                    intRowsAffected = 20;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                }
                return intRowsAffected;
            }





            public int FunPubCreateApprovals(SerializationMode SerMode, byte[] bytesApproval_Datatable, out string strErrMsg)
            {
                strErrMsg = string.Empty;
                try
                {
                    
                    ObjS3G_ORG_Approval_DataTable = (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable)ClsPubSerialize.DeSerialize(bytesApproval_Datatable, SerMode, typeof(S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_InsertApproval");
                    foreach (S3GBusEntity.LoanAdmin.ApprovalMgtServices.S3G_LOANAD_ApprovalRow ObjApprovalRow in ObjS3G_ORG_Approval_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjApprovalRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjApprovalRow.Branch_ID);
                        db.AddInParameter(command, "@Approval_ID", DbType.Int32, ObjApprovalRow.Approval_ID);
                        db.AddInParameter(command, "@Task_Number", DbType.String, ObjApprovalRow.Task_Number);
                        db.AddInParameter(command, "@Task_Type", DbType.String, ObjApprovalRow.Task_Type);
                        db.AddInParameter(command, "@Task_Status_Type_Code", DbType.Int32, ObjApprovalRow.Task_Status_Type_Code);
                        db.AddInParameter(command, "@Task_Status_Code", DbType.String, ObjApprovalRow.Task_Status_Code);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.String, ObjApprovalRow.Task_Approval_Serialvalue);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjApprovalRow.Remarks);
                        db.AddInParameter(command, "@Password", DbType.String, ObjApprovalRow.Password);
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApprovalRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ErrorMsg", DbType.String, 500);//Journal
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                command.CommandTimeout = 900;
                                db.FunPubExecuteNonQuery(command, ref trans);
                                if (command.Parameters["@ErrorCode"].Value !=null)
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

            public int FunPubRevokeOrUpdateApprovedDetails(Dictionary<string, string> dicParam)
            {
                int intRowsAffected = 0;
                //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                DbCommand command = db.GetStoredProcCommand("S3G_LOANAD_RevokeApproval");
                try
                {
                    foreach (KeyValuePair<string, string> Param in dicParam)
                    {
                        db.AddInParameter(command, Param.Key, DbType.String, Param.Value);
                    }

                    db.AddOutParameter(command, "@Result", DbType.Int32, sizeof(Int32));

                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            command.CommandTimeout = 180;
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@Result"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@Result"].Value;
                            if (intRowsAffected == 0)
                                trans.Commit();
                            else
                                trans.Rollback();

                        }
                        catch (Exception ex)
                        {
                            // Roll back the transaction. 
                            intRowsAffected = 50;
                            trans.Rollback();
                             ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                        }
                        conn.Close();
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
