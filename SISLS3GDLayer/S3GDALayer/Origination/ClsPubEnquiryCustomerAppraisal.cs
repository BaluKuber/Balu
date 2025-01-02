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

namespace S3GDALayer.Origination
{
    namespace EnquiryService
    {
        public class ClsPubEnquiryCustomerAppraisal
        {
            int intRowsAffected;

            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable ObjEnquiryCustomerDetails_DAL = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable ObjEnquiryPendingDoc_DAL = new S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable();
            //string S3G_Customer_Appraisal = "SELECT DISTINCT  * FROM S3G_View_Customer_Constitution_Document WHERE Doc_Status_Pending = 'Pending' AND Customer_ID=";
            //string S3G_Customer = "SELECT DISTINCT Customer_ID , (Customer_Code +  '-' + Customer_Name) AS Customer_Code FROM S3G_View_Customer_Constitution_Document WHERE Doc_Status_Pending = 'Pending'";
            //string S3G_Enquiry_Modify = "SELECT * FROM S3G_ORG_GetEnquiryDetailsModify";

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubEnquiryCustomerAppraisal()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            //--------------------Get Enquiry Details-----------------------------------

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubQueryEnquiryDetails(int intCompanyID, int intUserID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                // ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEnquiryDetails");
                    db.AddInParameter(command, "@Company_ID", DbType.Int32, intCompanyID);
                    db.AddInParameter(command, "@User_ID", DbType.Int32, intUserID);
                    //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }


            //Get Enqiry Pending Doc Details


            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable FunPubQueryEnquiryPendingDoc(int Company_ID, int Type, int ID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryPendingDoc = new S3GBusEntity.Origination.EnquiryService();
                //ObjEnquiryPendingDoc_DAL = (S3GBusEntity.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryPendingDocDataTable, SerMode, typeof(S3GBusEntity.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEnquiryPendingDoc");
                    db.AddInParameter(command, "@intCompany_ID", DbType.Int32, Company_ID);
                    db.AddInParameter(command, "@Type", DbType.Int32, Type);
                    db.AddInParameter(command, "@ID", DbType.Int32, ID);
                    //db.LoadDataSet(command, dsEnquiryPendingDoc, dsEnquiryPendingDoc.S3G_ORG_EnquiryPendingDoc.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryPendingDoc, dsEnquiryPendingDoc.S3G_ORG_EnquiryPendingDoc.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryPendingDoc.S3G_ORG_EnquiryPendingDoc;
            }


            public DataTable FunPubGetEnquiryDetailsModify()
            {

                try
                {
                    //DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //DbCommand command = db.GetSqlStringCommand(S3G_Enquiry_Modify);

                    //db.LoadDataSet(command, ObjDS, S3G_Enquiry_Modify);
                    //return (DataTable)ObjDS.Tables[S3G_Enquiry_Modify];

                    return new DataTable();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }



            //--------------------Get Enquiry Details By Enquiry No-----------------------------------
            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubQueryEnquiryDetailsByEnquiryNo(SerializationMode SerMode, byte[] bytesObj_EnquiryCustomerDataTable)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow ObjEnquiryCustomerDetailsRow in ObjEnquiryCustomerDetails_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEnquiryDetailsByEnquiryNo");
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEnquiryCustomerDetailsRow.Company_ID);
                        db.AddInParameter(command, "@Type", DbType.Int32, ObjEnquiryCustomerDetailsRow.Type);
                        db.AddInParameter(command, "@ID", DbType.Int32, ObjEnquiryCustomerDetailsRow.ID);
                        //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                        db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubEnquiryForUpdate(SerializationMode SerMode, byte[] bytesObj_EnquiryCustomerDataTable)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    foreach (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow ObjEnquiryCustomerDetailsRow in ObjEnquiryCustomerDetails_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_Enquiry_For_Update");
                        db.AddInParameter(command, "@Enq_Cus_App_ID", DbType.Int32, ObjEnquiryCustomerDetailsRow.Enq_Cus_App_ID);
                        //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                        db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    }
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubEnquiryDocumentForUpdate(int intEnqAppID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                //ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_EnquiryDocs_For_Update");
                    db.AddInParameter(command, "@EnquiryUpdation_ID", DbType.Int32, intEnqAppID);
                    //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubCustomerDocumentForUpdate(int intEnquiryAppID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                //ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_Enquiry_CustomerDoc_For_Update");
                    db.AddInParameter(command, "@Enq_Cus_App_ID", DbType.Int32, intEnquiryAppID);
                    //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }


            public DataSet FunPubPendingCustomerDetails(int intCustomerID, int intCompany_ID)
            {

                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand(SPNames.S3G_ORG_GetContitutionCustomer);
                    db.AddInParameter(command, "@CustomerId", DbType.Int32, intCustomerID);
                    //db.LoadDataSet(command, ObjDS, SPNames.S3G_ORG_GetContitutionCustomer);
                    db.FunPubLoadDataSet(command, ObjDS, SPNames.S3G_ORG_GetContitutionCustomer);
                    return ObjDS;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public DataTable FunPubPendingCustomer(int intCompany_ID)
            {

                try
                {
                    DataSet ObjDS = new DataSet();
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    //S3G_Customer = "AND Company_ID=" + "'" + intCompany_ID + "'";
                    //DbCommand command = db.GetSqlStringCommand(S3G_Customer);

                    //db.LoadDataSet(command, ObjDS, S3G_Customer);
                    //return (DataTable)ObjDS.Tables[S3G_Customer];

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEnquiryPendingCustomers");
                    db.AddInParameter(command, "@intCompany_ID", DbType.Int32, intCompany_ID);
                    //db.LoadDataSet(command, ObjDS, "PendingCustomers");
                    db.FunPubLoadDataSet(command, ObjDS, "PendingCustomers");

                    return ObjDS.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// <summary>
            /// Querying the Customer Details once the customer has been Appraised
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            public DataTable FunPubAppraisedCustomerDetail()
            {
                DataTable dtCustomerAppraisedtable = new DataTable();
                try
                {
                    S3GBusEntity.Origination.EnquiryServiceTableAdapters.S3G_ORG_Customer_Appraisal_DetailsTableAdapter CustomerAppraisedTableAdapter = new S3GBusEntity.Origination.EnquiryServiceTableAdapters.S3G_ORG_Customer_Appraisal_DetailsTableAdapter();
                    dtCustomerAppraisedtable = CustomerAppraisedTableAdapter.GetAppraisedCustomerdetails();
                    return dtCustomerAppraisedtable;

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                //finally
                //{
                //    dtCustomerAppraisedtable.Dispose();
                //    dtCustomerAppraisedtable = null;
                //}
            }

            /// <summary>
            /// Details from EnquiryCustomerAppraisal Table for Updating
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubCustomerForUpdate(int intEnquiryAppID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                //ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryCustomerDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_Enquiry_Customer_For_Update");
                    db.AddInParameter(command, "@Enq_Cus_App_ID", DbType.Int32, intEnquiryAppID);
                    //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);

                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }




            #region Insert

            //Insert Enquiry Appraisal Details

            public int FunPubCreateEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObj_EnquiryDataTable, bool EnquiryORCustomer)
            {
                try
                {
                    ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command;
                    foreach (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow ObjEnquiryDetailsRow in ObjEnquiryCustomerDetails_DAL.Rows)
                    {
                        if (EnquiryORCustomer == false)
                        {
                            command = db.GetStoredProcCommand("S3G_ORG_InsertEnquiryAppraisalDetails");
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjEnquiryDetailsRow.Branch_ID);
                            //db.AddInParameter(command, "@Enquiry_Response", DbType.String, ObjEnquiryDetailsRow.Enquiry_Response_ID);

                            db.AddInParameter(command, "@ID", DbType.Int32, ObjEnquiryDetailsRow.ID);
                            db.AddInParameter(command, "@PreDisbursement_Doc_Tran_ID", DbType.Int32, ObjEnquiryDetailsRow.PreDisbursement_Doc_Tran_ID);
                        }
                        else
                        {
                            command = db.GetStoredProcCommand("S3G_ORG_InsertCustomer_Appraisal_Details");
                        }
                        db.AddInParameter(command, "@Type", DbType.Int32, ObjEnquiryDetailsRow.Type);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjEnquiryDetailsRow.Company_ID);
                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjEnquiryDetailsRow.LOB_ID);

                        if (!ObjEnquiryDetailsRow.IsCustomer_IDNull())
                        {
                            db.AddInParameter(command, "@Customer_ID", DbType.Int64, ObjEnquiryDetailsRow.Customer_ID);
                        }
                        db.AddInParameter(command, "@Appraisal_Transaction_Type", DbType.String, ObjEnquiryDetailsRow.Appraisal_Transaction_Type);
                        db.AddInParameter(command, "@Date", DbType.DateTime, ObjEnquiryDetailsRow.Date);
                        db.AddInParameter(command, "@Is_Further_Process", DbType.Boolean, ObjEnquiryDetailsRow.Is_Further_Process);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjEnquiryDetailsRow.Created_By);
                        db.AddInParameter(command, "@XMLEnquiryDetails", DbType.String, ObjEnquiryDetailsRow.XmlEnquiryDetails);
                        if (!ObjEnquiryDetailsRow.IsXMLFacilityNull())
                        {
                            db.AddInParameter(command, "@XMLFacility", DbType.String, ObjEnquiryDetailsRow.XMLFacility);
                        }
                        db.AddInParameter(command, "@Facility_Amount", DbType.Decimal, ObjEnquiryDetailsRow.Facility_Amount);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int64, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        //if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        //    intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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


            public int FunPubModifyEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObj_EnquiryDataTable)
            {
                try
                {
                    ObjEnquiryCustomerDetails_DAL = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesObj_EnquiryDataTable, SerMode, typeof(S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow ObjEnquiryDetailsRow in ObjEnquiryCustomerDetails_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_Update_Customer_Appraisal");
                        db.AddInParameter(command, "@Is_Further_Process", DbType.Boolean, ObjEnquiryDetailsRow.Is_Further_Process);
                        db.AddInParameter(command, "@XMLEnquiryDetails", DbType.String, ObjEnquiryDetailsRow.XmlEnquiryDetails);
                        db.AddInParameter(command, "@Enq_Cus_App_ID", DbType.Int32, ObjEnquiryDetailsRow.Enq_Cus_App_ID);
                        if (!ObjEnquiryDetailsRow.IsXMLFacilityNull())
                        {
                            db.AddInParameter(command, "@XMLFacility", DbType.String, ObjEnquiryDetailsRow.XMLFacility);
                            db.AddInParameter(command, "@Facility_Amount", DbType.Decimal, ObjEnquiryDetailsRow.Facility_Amount);
                        }
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjEnquiryDetailsRow.Modified_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjEnquiryDetailsRow.Modified_By);
                        db.AddInParameter(command, "@Enq_Cus_App_Doc_ID", DbType.Int32, ObjEnquiryDetailsRow.Enq_Cus_App_Doc_ID);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int64, sizeof(Int64));
                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
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
            /// To get the Enquiry Response ID from EnquiryCustomerAppraisal table
            /// </summary>
            /// <param name="SerMode"></param>
            /// <param name="bytesObjS3G_ORG_PRDDCMasterDataTable"></param>
            /// <returns></returns>

            public S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable FunPubQueryEnquiryResponseID(int intEnquiryID)
            {
                S3GBusEntity.Origination.EnquiryService dsEnquiryCustomerDetails = new S3GBusEntity.Origination.EnquiryService();
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_EnquiryResponseID");
                    db.AddInParameter(command, "@Enq_Cus_App_ID", DbType.Int32, intEnquiryID);
                    //db.LoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                    db.FunPubLoadDataSet(command, dsEnquiryCustomerDetails, dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails.TableName);
                }
                catch (Exception ex)
                {
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return dsEnquiryCustomerDetails.S3G_ORG_EnquiryCustomerDetails;
            }


        }
    }
}