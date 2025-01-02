using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using S3GBusEntity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Data;

namespace S3GDALayer.Origination
{
    namespace ApplicationMgtServices
    {
        public class ClsPubApplicationApproval
        {
            int intRowsAffected = 0;
            S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalDataTable ObjApplicationApproval;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubApplicationApproval()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }


            public int FunPubCreateApplicationApproval(out int ApprovalStatus, SerializationMode SerMode, byte[] bytesApplicationApproval_Datatable)
            {
                ApprovalStatus = 0;
                try
                {
                    ObjApplicationApproval = (S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalDataTable)ClsPubSerialize.DeSerialize(bytesApplicationApproval_Datatable, SerMode, typeof(S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertApplicationApproval");
                    foreach (S3GBusEntity.Origination.ApplicationMgtServices.S3G_ORG_ApplicationApprovalRow ObjApplicationApprovalRow in ObjApplicationApproval.Rows)
                    {
                        db.AddInParameter(command, "@Application_Process_ID", DbType.Int32, ObjApplicationApprovalRow.Application_Process_ID);
                        db.AddInParameter(command, "@Application_Approval_ID", DbType.Int32, ObjApplicationApprovalRow.Application_Approval_ID);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.Int32, ObjApplicationApprovalRow.Approval_Serial_Number);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjApplicationApprovalRow.Action_ID);
                        if(!ObjApplicationApprovalRow.IsRemarksNull())
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjApplicationApprovalRow.Remarks);
                        db.AddInParameter(command, "@Password", DbType.String, ObjApplicationApprovalRow.Password);
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjApplicationApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjApplicationApprovalRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@ApprovalStatus", DbType.Int16, sizeof(Int16));
                        using (DbConnection conn = db.CreateConnection())
                        {
                            conn.Open();
                            DbTransaction trans = conn.BeginTransaction();
                            try
                            {
                                db.FunPubExecuteNonQuery(command, ref trans);

                                ApprovalStatus = Convert.ToInt16(command.Parameters["@ApprovalStatus"].Value.ToString());

                                if ((int)command.Parameters["@ErrorCode"].Value > 0)
                                {
                                    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
                                }
                                if (intRowsAffected ==0)
                                {
                                    trans.Commit();
                                }
                                else
                                {
                                    trans.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Roll back the transaction. 
                                intRowsAffected = 20;
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
            public int FunPubRevokeOrUpdateApprovedDetails(int intOptions,int intTask_ID,int intUser_ID,string strPassword)
            {
                int intRowsAffected = 0;
                 //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                 DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateApprovedstatus");
                try
                {
                    db.AddInParameter(command, "@Options", DbType.Int32,intOptions);
                    db.AddInParameter(command, "@Task_ID", DbType.Int32, intTask_ID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUser_ID);
                    db.AddInParameter(command, "@Password", DbType.String, strPassword.Trim());
                    db.AddOutParameter(command, "@Result", DbType.Int32, sizeof(Int32));
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
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

            public int FunPubRevokeOrUpdateApprovedDetailsPricing(int intOptions, string str_IDs, int intUser_ID, string strPassword, out string outProposalNo)
            {
                int intRowsAffected = 0;
                outProposalNo = string.Empty;
                DbCommand command = db.GetStoredProcCommand("S3G_ORG_UpdateApprovedstatus");

                try
                {
                    db.AddInParameter(command, "@Options", DbType.Int32,intOptions);
                    db.AddInParameter(command, "@Pricing_ID", DbType.String, str_IDs);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUser_ID);
                    db.AddInParameter(command, "@Password", DbType.String, strPassword.Trim());
                    db.AddOutParameter(command, "@Result", DbType.Int32, sizeof(Int32));
                    db.AddOutParameter(command, "@OUTPUT", DbType.String, -1);
                    using (DbConnection conn = db.CreateConnection())
                    {
                        conn.Open();
                        DbTransaction trans = conn.BeginTransaction();
                        try
                        {
                            db.FunPubExecuteNonQuery(command, ref trans);
                            if ((int)command.Parameters["@Result"].Value > 0)
                                intRowsAffected = (int)command.Parameters["@Result"].Value;
                            if (intRowsAffected == 0)
                            {
                                trans.Commit();
                            }
                            else
                            {
                                outProposalNo = (string)command.Parameters["@OUTPUT"].Value;
                                trans.Rollback();
                            }
                        }
                        catch (Exception ex)
                        {
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
