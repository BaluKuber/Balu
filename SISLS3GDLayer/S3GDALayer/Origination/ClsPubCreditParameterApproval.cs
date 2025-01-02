using System;
using S3GDALayer.S3GAdminServices;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using S3GBusEntity;

namespace S3GDALayer.Origination
{
    namespace CreditMgtServices
    {
        public class ClsPubCreditParameterApproval
        {
            int intRowsAffected;


            #region Query

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable
            ObjCreditParameterApproval_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable();

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubCreditParameterApproval()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable FunPubQueryCreditPArameterApproval(SerializationMode Sermode, byte[] bytesCreditParameterApproval, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterApprovalDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditParameterApproval_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterApproval, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterApproval");

                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                    db.FunPubLoadDataSet(command, ObjCreditParameterApprovalDataTable, ObjCreditParameterApprovalDataTable.S3G_ORG_CreditParameterApproval.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterApprovalDataTable.S3G_ORG_CreditParameterApproval;
            }


            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable
                 ObjCreditParameterTransaction_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable FunPubQueryCreditParameterTransaction(SerializationMode Sermode, byte[] bytesCreditParameterTransaction, out int intTotalRecords, PagingValues ObjPaging)
            {
                intTotalRecords = 0;
                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterTransactionDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditParameterTransaction_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterTranaction_Approval");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_CreditParameterTranaction_Approval");
                    db.AddInParameter(command, "@FilterType", DbType.String, ObjCreditParameterTransaction_DataTable.Rows[0]["FilterType"].ToString());
                    db.AddInParameter(command, "@StatusId", DbType.String, ObjCreditParameterTransaction_DataTable.Rows[0]["CreditParamStatus_ID"].ToString());
                    db.AddInParameter(command, "@Document_Type", DbType.Int32, ObjCreditParameterTransaction_DataTable.Rows[0]["Document_Type"].ToString());
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjPaging.ProCompany_ID);
                    db.AddInParameter(command, "@CurrentPage", DbType.Int32, ObjPaging.ProCurrentPage);
                    db.AddInParameter(command, "@PageSize", DbType.Int32, ObjPaging.ProPageSize);
                    db.AddInParameter(command, "@SearchValue", DbType.String, ObjPaging.ProSearchValue);
                    db.AddInParameter(command, "@OrderBy", DbType.String, ObjPaging.ProOrderBy);
                    db.AddInParameter(command, "@User_Id", DbType.String, ObjPaging.ProUser_ID);
                    db.AddOutParameter(command, "@TotalRecords", DbType.Int32, sizeof(Int32));

                    db.FunPubLoadDataSet(command, ObjCreditParameterTransactionDataTable, ObjCreditParameterTransactionDataTable.S3G_ORG_CreditParamTransaction.TableName);
                    if ((int)command.Parameters["@TotalRecords"].Value > 0)
                        intTotalRecords = (int)command.Parameters["@TotalRecords"].Value;
                }
                //}
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterTransactionDataTable.S3G_ORG_CreditParamTransaction;
            }

            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable FunPubQueryCreditParameterTransactionID(SerializationMode Sermode, int CPTID)
            {
                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterTransactionDataTable = new S3GBusEntity.Origination.CreditMgtServices();


                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterTranactionID");

                    db.AddInParameter(command, "@Credit_ParamID", DbType.Int32, CPTID);

                    db.FunPubLoadDataSet(command, ObjCreditParameterTransactionDataTable, ObjCreditParameterTransactionDataTable.S3G_ORG_CreditParamTransaction.TableName);

                }
                //}
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterTransactionDataTable.S3G_ORG_CreditParamTransaction;
            }

            // To display the credit parameter approval - in enquiry mode.
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable
                ObjCreditParameterApprovalEnquiry_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable FunPubQueryCreditParameterApproval_Enquiry(SerializationMode Sermode, byte[] bytesCreditParameterTransaction)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterApprovalEnquiryDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditParameterApprovalEnquiry_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterApproval_Enquiry");
                    db.AddInParameter(command, "@EnquiryNo", DbType.String, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["Enquiry_No"].ToString());


                    db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["Company_ID"].ToString());
                    db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["LOB_ID"].ToString());
                    db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["Product_ID"].ToString());
                    db.AddInParameter(command, "@Location_Id", DbType.Int32, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["Branch_Id"].ToString());
                    db.AddInParameter(command, "@Constitution_ID", DbType.Int32, ObjCreditParameterApprovalEnquiry_DataTable.Rows[0]["Constitution_ID"].ToString());

                    db.FunPubLoadDataSet(command, ObjCreditParameterApprovalEnquiryDataTable,
                        ObjCreditParameterApprovalEnquiryDataTable.S3G_ORG_GetCreditParameterApproval_Enquiry.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterApprovalEnquiryDataTable.S3G_ORG_GetCreditParameterApproval_Enquiry;
            }


            // to display the score board           
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable
                ObjCreditParameterApprovalScoreBoard_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable FunPubQueryCreditParameterApproval_ScoreBoard(SerializationMode Sermode, byte[] bytesCreditParameterTransaction)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterApprovalScoreBoardDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditParameterApprovalScoreBoard_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterTransaction, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterApproval_ScoreBoard");
                    db.AddInParameter(command, "@CreditParamTrans_ID", DbType.Int32, ObjCreditParameterApprovalScoreBoard_DataTable.Rows[0]["CreditParamTrans_ID"].ToString());



                    db.FunPubLoadDataSet(command, ObjCreditParameterApprovalScoreBoardDataTable,
                        ObjCreditParameterApprovalScoreBoardDataTable.S3G_ORG_GetCreditParameterApproval_ScoreBoard.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterApprovalScoreBoardDataTable.S3G_ORG_GetCreditParameterApproval_ScoreBoard;
            }

            // to display Credit PArameter Approval - Details - in Enquiry Mode          
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable
                ObjCreditParameterApprovalDetails_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable FunPubQueryCreditParameterApprovalDetails_Enquiry(SerializationMode Sermode, byte[] bytesCreditParameterApprovalDetails, int CreditParamTransId)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjCreditParameterApprovalDetailsDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCreditParameterApprovalDetails_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable)ClsPubSerialize.DeSerialize(bytesCreditParameterApprovalDetails, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterDetails_Enquiry");
                    db.AddInParameter(command, "@CreditParamTrans_ID", DbType.Int32, CreditParamTransId.ToString());



                    db.FunPubLoadDataSet(command, ObjCreditParameterApprovalDetailsDataTable,
                        ObjCreditParameterApprovalDetailsDataTable.S3G_ORG_CreditParameterApprovalDetails.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCreditParameterApprovalDetailsDataTable.S3G_ORG_CreditParameterApprovalDetails;
            }


            // to check if the user exists.          
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable
                ObjUserIsValid_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable FunPubQueryUserIsValid(SerializationMode Sermode, byte[] bytesUserIsValid)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjUserIsValid = new S3GBusEntity.Origination.CreditMgtServices();
                ObjUserIsValid_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable)ClsPubSerialize.DeSerialize(bytesUserIsValid, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_UserIsValid");
                    db.AddInParameter(command, "@User_Name", DbType.String, ObjUserIsValid_DataTable.Rows[0]["User_Name"].ToString());
                    db.AddInParameter(command, "@password", DbType.String, ObjUserIsValid_DataTable.Rows[0]["password"].ToString());



                    db.FunPubLoadDataSet(command, ObjUserIsValid,
                        ObjUserIsValid.S3G_ORG_UserIsValid.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjUserIsValid.S3G_ORG_UserIsValid;
            }

            // Credit Parameter Approval - Customer Mode          
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable
                ObjEnquiryCustomerAppraisal_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable FunPubQueryEnquiryCustomerAppraisal(SerializationMode Sermode, byte[] bytesEnquiryCustomerAppraisal)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEnquiryCustomerAppraisal = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEnquiryCustomerAppraisal_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryCustomerAppraisal, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterTranaction_CusMode");
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjEnquiryCustomerAppraisal_DataTable.Rows[0]["Customer_ID"].ToString());




                    db.FunPubLoadDataSet(command, ObjEnquiryCustomerAppraisal,
                        ObjEnquiryCustomerAppraisal.S3G_ORG_EnquiryCustomerAppraisal.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEnquiryCustomerAppraisal.S3G_ORG_EnquiryCustomerAppraisal;
            }


            // Get customer details by customer id
            // Credit Parameter Approval - Customer Mode          
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable
                ObjEnquiryCustomer_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable FunPubQueryCustomerDetailsById(SerializationMode Sermode, byte[] bytesEnquiryCustomer)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEnquiryCustomer = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEnquiryCustomer_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryCustomer, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCustomerDetailsByCustomerId");
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjEnquiryCustomer_DataTable.Rows[0]["Customer_ID"].ToString());




                    db.FunPubLoadDataSet(command, ObjEnquiryCustomer,
                        ObjEnquiryCustomer.S3G_ORG_CustomerMaster.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEnquiryCustomer.S3G_ORG_CustomerMaster;
            }



            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable
                ObjEnquiryCustomer_ModeDataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable FunPubQueryEnquiryCustomer(SerializationMode Sermode, byte[] bytesEnquiryCustomer)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEnquiryCustomer = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEnquiryCustomer_ModeDataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryCustomer, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCreditParameterApproval_CustomerEnquiry");
                    db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjEnquiryCustomer_ModeDataTable.Rows[0]["Customer_ID"].ToString());




                    db.FunPubLoadDataSet(command, ObjEnquiryCustomer,
                        ObjEnquiryCustomer.S3G_ORG_GetCreditParameterApproval_CustomerEnquiry.TableName);


                }

                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEnquiryCustomer.S3G_ORG_GetCreditParameterApproval_CustomerEnquiry;
            }
            #endregion


            #region Create

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable ObjCPA;
            public int FunPubCreateCreditParameterApproval(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADataTable, out int Credit_Parameter_Approval_ID)
            {
                Credit_Parameter_Approval_ID = -1;
                try
                {
                    ObjCPA = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CPADataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");                   

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalRow ObjCPARow in ObjCPA.Rows)
                    {
                        using (DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertCreditParameterApproval"))
                        {

                            db.AddInParameter(command, "@CreditParamTrans_ID", DbType.Int32, ObjCPARow.CreditParamTrans_ID);
                            if (!ObjCPARow.IsCustomer_IDNull())
                            {
                                db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjCPARow.Customer_ID);
                            }
                            // Rajendran 20101028
                            db.AddInParameter(command, "@isFinalApproval", DbType.Int32, ObjCPARow.isFinalApproval);

                            db.AddOutParameter(command, "@Santion_Number", DbType.String, 20);
                            db.AddInParameter(command, "@Sanction_Date", DbType.DateTime, DateTime.Today);
                            db.AddInParameter(command, "@Approval_Details_ID", DbType.Int32, ObjCPARow.Approval_Details_ID);
                            db.AddInParameter(command, "@Sanction_Amount", DbType.Double, ObjCPARow.Sanction_Amount);
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCPARow.LOB_ID);
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjCPARow.Branch_ID);
                            if (string.IsNullOrEmpty(ObjCPARow.Product_ID.ToString()))
                            {
                                db.AddInParameter(command, "@Product_ID", DbType.Int32, DBNull.Value);
                            }
                            else
                            {
                                db.AddInParameter(command, "@Product_ID", DbType.Int32, ObjCPARow.Product_ID);
                            }
                            db.AddInParameter(command, "@Created_By", DbType.Int32, ObjCPARow.Created_By);
                            db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjCPARow.Created_On);
                            db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjCPARow.Modified_By);
                            db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjCPARow.Modified_On);
                            db.AddOutParameter(command, "@Credit_Parameter_Approval_ID", DbType.Int32, sizeof(Int64));
                            db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));


                            //db.FunPubExecuteNonQuery(command);


                            //intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                            //Credit_Parameter_Approval_ID = (int)command.Parameters["@Credit_Parameter_Approval_ID"].Value;  // to get the newly generated primary key 
                            //if (command.Parameters["@Santion_Number"].Value != null)
                            //{
                            //    string sanction_number = command.Parameters["@Santion_Number"].Value.ToString();
                            //}


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

                                    if (command.Parameters["@Credit_Parameter_Approval_ID"].Value != null)
                                    {
                                        Credit_Parameter_Approval_ID = (int)command.Parameters["@Credit_Parameter_Approval_ID"].Value;
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
                                        intRowsAffected = 50;
                                    }
                                    trans.Rollback();
                                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);

                                }
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



            // to insert into the CreditParameterApprovalDetials
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable ObjCPADetails;
            public int FunPubCreateCreditParameterApprovalDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADetailsDataTable, int FinalApproval, decimal SanctionAmount, out string SanctionNumber)
            {
                try
                {

                    SanctionNumber = string.Empty;

                    ObjCPADetails = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CPADetailsDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsRow ObjCPADetailsRow in ObjCPADetails.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertCreditParameterApprovalDetails");

                        db.AddInParameter(command, "@Credit_Parameter_Approval_ID", DbType.Int32, ObjCPADetailsRow.Credit_Parameter_Approval_ID);
                        db.AddInParameter(command, "@Approval_Serial_No", DbType.Int32, ObjCPADetailsRow.Approval_Serial_No);

                        db.AddInParameter(command, "@Approver_ID", DbType.Int32, ObjCPADetailsRow.Approver_ID);
                        db.AddInParameter(command, "@Approval_Status_ID", DbType.Int32, ObjCPADetailsRow.Approval_Status_ID);
                        db.AddInParameter(command, "@Approval_Details_ID", DbType.Int32, ObjCPADetailsRow.Approval_Details_ID);
                        db.AddInParameter(command, "@Approval_Date", DbType.DateTime, ObjCPADetailsRow.Approval_Date);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjCPADetailsRow.Remarks);

                        db.AddInParameter(command, "@isFinalApproval", DbType.Int32, FinalApproval);
                        db.AddInParameter(command, "@Sanction_Amount", DbType.Decimal, SanctionAmount);
                        db.AddOutParameter(command, "@Santion_Number", DbType.String, 50);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, 3);


                        //db.FunPubExecuteNonQuery(command);

                        //intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

                        //if (command.Parameters["@Santion_Number"].Value != null)
                        //{
                        //    SanctionNumber = command.Parameters["@Santion_Number"].Value.ToString();
                        //}


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
                                if (command.Parameters["@Santion_Number"].Value != null)
                                {
                                    SanctionNumber = command.Parameters["@Santion_Number"].Value.ToString();
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
                                if (command.Parameters["@Santion_Number"].Value != null)
                                {
                                    SanctionNumber = command.Parameters["@Santion_Number"].Value.ToString();
                                }

                                else
                                {
                                    intRowsAffected = 50;
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
                    intRowsAffected = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected;
            }




            // to insert into the CreditParameterApprovalCustomerDetials
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable ObjCustomerDetails;
            public int FunPubCreateCreditParameterApprovalCustomerDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CustomerDetailsDataTable, int isfinalApproval)
            {

                try
                {

                    ObjCustomerDetails = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_CustomerDetailsDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalCusDetailsRow ObjCustomerDetailsRow in ObjCustomerDetails.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertCreditParameterApprovalCusDetails");

                        db.AddInParameter(command, "@Credit_Parameter_Approval_ID", DbType.Int32, ObjCustomerDetailsRow.Credit_Parameter_Approval_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjCustomerDetailsRow.LOB_ID);

                        db.AddInParameter(command, "@Required_facility", DbType.Int64, ObjCustomerDetailsRow.Required_facility);
                        db.AddInParameter(command, "@Sanctioned_limit", DbType.Decimal, ObjCustomerDetailsRow.Sanctioned_limit);
                        db.AddInParameter(command, "@Override", DbType.String, ObjCustomerDetailsRow.Override);
                        db.AddInParameter(command, "@Offer_Card", DbType.String, ObjCustomerDetailsRow.Offer_Card);
                        db.AddInParameter(command, "@Employee_ID", DbType.Int32, ObjCustomerDetailsRow.Employee_ID);
                        db.AddInParameter(command, "@Final_Sanctioned_Limit", DbType.Decimal, ObjCustomerDetailsRow.Final_Sanctioned_Limit);
                        db.AddInParameter(command, "@Approved_Date", DbType.DateTime, ObjCustomerDetailsRow.Approved_Date);

                        db.AddInParameter(command, "@IsFinalApproval", DbType.Int16, isfinalApproval);
                        db.AddInParameter(command, "@XMLCredit", DbType.String, ObjCustomerDetailsRow.XML_Credit);

                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));



                        db.FunPubExecuteNonQuery(command);

                        intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;

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
            #endregion
        }
    }
}
