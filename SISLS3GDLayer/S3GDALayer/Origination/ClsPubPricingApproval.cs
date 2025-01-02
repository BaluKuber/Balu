using System;using S3GDALayer.S3GAdminServices;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using S3GBusEntity;

namespace S3GDALayer.Origination
{
    namespace PricingMgtServices
    {
        public class ClsPubPricingApproval
        {
            int intRowsAffected = 0;
            S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable ObjS3G_ORG_PricingApproval_DataTable;

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubPricingApproval()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public int FunPubCreatePricingApproval(SerializationMode SerMode, byte[] bytesPricingApproval_Datatable, out int intNoOfApproval, out int intApprovedCount)
            {
                intNoOfApproval = 0;
                intApprovedCount = 0;
                try
                {
                    ObjS3G_ORG_PricingApproval_DataTable = (S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable)ClsPubSerialize.DeSerialize(bytesPricingApproval_Datatable, SerMode, typeof(S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable));

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertPricingApproval");
                    foreach (S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalRow ObjPricingApprovalRow in ObjS3G_ORG_PricingApproval_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Pricing_ID", DbType.Int32, ObjPricingApprovalRow.Pricing_ID);
                        db.AddInParameter(command, "@PricingApproval_ID", DbType.Int32, ObjPricingApprovalRow.PricingApproval_ID);
                        db.AddInParameter(command, "@Approval_Serial_Number", DbType.Int32, ObjPricingApprovalRow.Approval_Serial_Number);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjPricingApprovalRow.Action_ID);
                        db.AddInParameter(command, "@Password", DbType.String, ObjPricingApprovalRow.Password);
                        if (!ObjPricingApprovalRow.IsRemarksNull())
                            db.AddInParameter(command, "@Remarks", DbType.String, ObjPricingApprovalRow.Remarks);
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjPricingApprovalRow.Company_ID);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPricingApprovalRow.Created_By);
                        db.AddInParameter(command, "@XMLDet", DbType.Int32, ObjPricingApprovalRow.XmlBulkApproval);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Total_Approval", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Approval_No", DbType.Int32, sizeof(Int64));
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
                                {
                                    intNoOfApproval = (int)command.Parameters["@Total_Approval"].Value;
                                    intApprovedCount = (int)command.Parameters["@Approval_No"].Value;
                                    trans.Commit();
                                }
                                else
                                    trans.Rollback();
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



            public int FunPubCreateProposalApproval(SerializationMode SerMode, byte[] bytesPricingApproval_Datatable, out int intNoOfApproval, out int intApprovedCount)
            {
                intNoOfApproval = 0;
                intApprovedCount = 0;
                try
                {
                    ObjS3G_ORG_PricingApproval_DataTable = (S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable)ClsPubSerialize.DeSerialize(bytesPricingApproval_Datatable, SerMode, typeof(S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalDataTable));
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertPricingProposalApproval");
                    foreach (S3GBusEntity.Origination.PricingMgtServices.S3G_ORG_PricingApprovalRow ObjPricingApprovalRow in ObjS3G_ORG_PricingApproval_DataTable.Rows)
                    {
                        //db.AddInParameter(command, "@Pricing_ID", DbType.String, ObjPricingApprovalRow.PricingOfferNo);
                        //db.AddInParameter(command, "@PricingApproval_ID", DbType.String, ObjPricingApprovalRow.PricingOfferAppNo);
                        //db.AddInParameter(command, "@Approval_Serial_Number", DbType.String, ObjPricingApprovalRow.PricingOfferSerNo);
                        db.AddInParameter(command, "@Action_ID", DbType.Int32, ObjPricingApprovalRow.Action_ID);
                        db.AddInParameter(command, "@Password", DbType.String, ObjPricingApprovalRow.Password);
                        if (!ObjPricingApprovalRow.IsRemarksNull())
                            db.AddInParameter(command, "@Remarks", DbType.String, ObjPricingApprovalRow.Remarks);
                        db.AddInParameter(command, "@Company_ID", DbType.String, ObjPricingApprovalRow.Company_ID);
                        //db.AddInParameter(command, "@Approval_Date", DbType.String, ObjPricingApprovalRow.Approval_Date);
                        db.AddInParameter(command, "@UserID", DbType.String, ObjPricingApprovalRow.Approver_ID);
                        db.AddInParameter(command, "@XMLDet", DbType.String, ObjPricingApprovalRow.XmlBulkApproval);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjPricingApprovalRow.Created_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Total_Approval", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@Approval_No", DbType.Int32, sizeof(Int64));
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
                                {
                                    intNoOfApproval = (int)command.Parameters["@Total_Approval"].Value;
                                    intApprovedCount = (int)command.Parameters["@Approval_No"].Value;
                                    trans.Commit();
                                }
                                else
                                    trans.Rollback();
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
        }
    }
}
