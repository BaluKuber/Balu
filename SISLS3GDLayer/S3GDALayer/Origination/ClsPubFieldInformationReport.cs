using System;using S3GDALayer.S3GAdminServices;
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

        public class ClsPubFieldInformationReport
        {

            //Code added for getting common connection string  from config file
            Database db;
            public ClsPubFieldInformationReport()
            {
                db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            }

            #region Query


            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable
                        ObjEntityMaster_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable FunPubQueryEntityMaster(SerializationMode Sermode, byte[] bytesEntityMaster)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEntityMasterDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEntityMaster_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable)ClsPubSerialize.DeSerialize(bytesEntityMaster, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEntity_Master");

                    //db.LoadDataSet(command, ObjEntityMasterDataTable, ObjEntityMasterDataTable.S3G_ORG_EntityMaster.TableName);
                    db.FunPubLoadDataSet(command, ObjEntityMasterDataTable, ObjEntityMasterDataTable.S3G_ORG_EntityMaster.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEntityMasterDataTable.S3G_ORG_EntityMaster;
            }







            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable
                          ObjCustomerMaster_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable FunPubQueryCustomerMasterByCode(SerializationMode Sermode, byte[] bytesCreditScoreGuideParameter)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjCustomerMasterDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjCustomerMaster_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable)ClsPubSerialize.DeSerialize(bytesCreditScoreGuideParameter, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetCustomerMasterByCode");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterRow ObjRow in ObjCustomerMaster_DataTable.Rows)
                    {
                        db.AddInParameter(command, "@Customer_Code", DbType.String, ObjRow.Customer_Code);
                    }
                    //db.LoadDataSet(command, ObjCustomerMasterDataTable, ObjCustomerMasterDataTable.S3G_ORG_CustomerMaster.TableName);
                    db.FunPubLoadDataSet(command, ObjCustomerMasterDataTable, ObjCustomerMasterDataTable.S3G_ORG_CustomerMaster.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjCustomerMasterDataTable.S3G_ORG_CustomerMaster;
            }

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable
                 ObjEnquiryResponse_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable FunPubQueryEnquiryResponse(SerializationMode Sermode, byte[] bytesCreditScoreGuideParameter)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEnquiryResponseDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEnquiryResponse_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable)ClsPubSerialize.DeSerialize(bytesCreditScoreGuideParameter, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetFIRByEnquiryID");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseRow ObjRow in ObjEnquiryResponse_DataTable.Rows)
                    {

                        db.AddInParameter(command, "@Enquiry_ID", DbType.Int32, ObjRow.Enquiry_No);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRow.Company_ID);

                    }
                    //db.LoadDataSet(command, ObjEnquiryResponseDataTable, ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse.TableName);
                    db.FunPubLoadDataSet(command, ObjEnquiryResponseDataTable, ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse;
            }



            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable
                 ObjEnquiryResponseAll_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable FunPubQueryEnquiryResponseAllRows(SerializationMode Sermode, byte[] bytesCreditScoreGuideParameter)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjEnquiryResponseDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjEnquiryResponseAll_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable)ClsPubSerialize.DeSerialize(bytesCreditScoreGuideParameter, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetEnquiry_Response");
                    //foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Enquiry_ResponseRow ObjRow in ObjEnquiryResponse_DataTable.Rows)
                    //{
                    //    db.AddInParameter(command, "@Enquiry_ID", DbType.Int32, ObjRow.EnquiryReferenceNumber);
                    //}
                    //db.LoadDataSet(command, ObjEnquiryResponseDataTable, ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse.TableName);
                    db.FunPubLoadDataSet(command, ObjEnquiryResponseDataTable, ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjEnquiryResponseDataTable.S3G_ORG_EnquiryResponse;
            }
            #endregion

            #region Create
            int intRowsAffected;
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable ObjFIR_DAL;
            public int FunPubCreateFIR(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable, out string strFIRNumber_Out)
            {
                try
                {
                    strFIRNumber_Out = string.Empty;

                    ObjFIR_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjFIRRow in ObjFIR_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertFieldInformationReport");

                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjFIRRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjFIRRow.Branch_ID);
                        db.AddInParameter(command, "@Doc_Type", DbType.Int32, ObjFIRRow.Doc_Type);
                        db.AddInParameter(command, "@Doc_Ref_ID", DbType.Int32, ObjFIRRow.Doc_Ref_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjFIRRow.Customer_ID);
                        //db.AddInParameter(command, "@Enquiry_Response_ID", DbType.Int32, ObjFIRRow.Enquiry_Response_ID);
                        db.AddInParameter(command, "@Terms_Conditions", DbType.String, ObjFIRRow.Terms_Conditions);
                        db.AddInParameter(command, "@FIR_ID", DbType.Int32, ObjFIRRow.FIR_ID);
                        db.AddInParameter(command, "@FIR_No", DbType.String, ObjFIRRow.FIR_No);
                        db.AddInParameter(command, "@FIR_Date", DbType.DateTime, ObjFIRRow.FIR_Date);
                        db.AddInParameter(command, "@Round", DbType.String, ObjFIRRow.Round);
                        db.AddInParameter(command, "@Requested_By", DbType.String, ObjFIRRow.Requested_By);
                        db.AddInParameter(command, "@Requested_Date", DbType.DateTime, ObjFIRRow.Requested_Date );
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjFIRRow.Entity_ID);
                        db.AddInParameter(command, "@Field_Request", DbType.String, ObjFIRRow.Field_Request);
                        db.AddInParameter(command, "@Notification_Sent_by", DbType.String, ObjFIRRow.Notification_Sent_by);
                        db.AddInParameter(command, "@Responded_By", DbType.String, ObjFIRRow.Responded_By);
                        db.AddInParameter(command, "@Responded_Date", DbType.DateTime, ObjFIRRow.Responded_Date );
                        db.AddInParameter(command, "@Response_Designation", DbType.String, ObjFIRRow.Response_Designation);
                        db.AddInParameter(command, "@Response", DbType.String, ObjFIRRow.Response);
                        db.AddInParameter(command, "@Value", DbType.String, ObjFIRRow.Value);
                        db.AddInParameter(command, "@Currency", DbType.String, ObjFIRRow.Currency);
                        db.AddInParameter(command, "@Status", DbType.String, ObjFIRRow.Status);
                        db.AddInParameter(command, "@Client_Credibility", DbType.String, ObjFIRRow.Client_Credibility);
                        db.AddInParameter(command, "@Client_Net_Worth", DbType.String, ObjFIRRow.Client_Net_Worth);
                        db.AddInParameter(command, "@Field_Officer_EmailID", DbType.String, ObjFIRRow.Field_Officer_EMailID);
                        db.AddInParameter(command, "@Field_Officer_MobileNo", DbType.String, ObjFIRRow.Field_Officer_MobileNo);
                        db.AddInParameter(command, "@Cancelled", DbType.Boolean, ObjFIRRow.Cancelled);
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjFIRRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjFIRRow.Created_On );
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjFIRRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjFIRRow.Modified_On );
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjFIRRow.Company_ID);
                        db.AddInParameter(command, "@XML_FIRDeltails", DbType.String, ObjFIRRow.XML_FIRDeltails);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                        db.AddOutParameter(command, "@RefNo", DbType.String, 100);
                        db.AddInParameter(command, "@Agency_Type", DbType.Int32, ObjFIRRow.AgencyType);

                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        strFIRNumber_Out = (string)command.Parameters["@RefNo"].Value;

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected = (int)command.Parameters["@ErrorCode"].Value;
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
            #endregion


            #region Create
            int intRowsAffected1;
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_LegalClearanceReportDataTable ObjLEG_DAL;
            public int FunPubCreateLegalClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LEGDataTable, out string strFIRNumber_Out)
            {
                try
                {
                    strFIRNumber_Out = string.Empty;

                    ObjLEG_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_LegalClearanceReportDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_LEGDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_LegalClearanceReportDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_LegalClearanceReportRow ObjLEGRow in ObjLEG_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertLegal_trans_Details");

                        db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjLEGRow.LOB_ID);
                        db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjLEGRow.Branch_ID);
                        db.AddInParameter(command, "@Doc_Type", DbType.Int32, ObjLEGRow.Doc_Type);
                        db.AddInParameter(command, "@Doc_Ref_ID", DbType.Int32, ObjLEGRow.Doc_Ref_ID);
                        db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjLEGRow.Customer_ID);
                        //db.AddInParameter(command, "@Enquiry_Response_ID", DbType.Int32, ObjFIRRow.Enquiry_Response_ID);
                        db.AddInParameter(command, "@Terms_Conditions", DbType.String, ObjLEGRow.Terms_Conditions);
                        db.AddInParameter(command, "@Legal_ID", DbType.Int32, ObjLEGRow.Legal_ID);
                        db.AddInParameter(command, "@Legal_No", DbType.String, ObjLEGRow.Legal_No);
                        db.AddInParameter(command, "@Legal_Date", DbType.DateTime, ObjLEGRow.Legal_Date);
                        db.AddInParameter(command, "@Round", DbType.String, ObjLEGRow.Round);
                        db.AddInParameter(command, "@Requested_By", DbType.String, ObjLEGRow.Requested_By);
                        db.AddInParameter(command, "@Requested_Date", DbType.DateTime, ObjLEGRow.Requested_Date);
                        db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjLEGRow.Entity_ID);
                        db.AddInParameter(command, "@Field_Request", DbType.String, ObjLEGRow.Field_Request);
                        db.AddInParameter(command, "@Notification_Sent_by", DbType.String, ObjLEGRow.Notification_Sent_by);
                        if (!ObjLEGRow.IsResponded_ByNull())
                        {
                            db.AddInParameter(command, "@Responded_By", DbType.Int32, ObjLEGRow.Responded_By);
                        }
                        if (!ObjLEGRow.IsResponded_DateNull())
                        {
                            db.AddInParameter(command, "@Responded_Date", DbType.DateTime, ObjLEGRow.Responded_Date);
                        }
                        if (!ObjLEGRow.IsResponse_DesignationNull())
                        {
                            db.AddInParameter(command, "@Response_Designation", DbType.String, ObjLEGRow.Response_Designation);
                        }
                        if (!ObjLEGRow.IsResponseNull())
                        {
                            db.AddInParameter(command, "@Response", DbType.String, ObjLEGRow.Response);
                        }
                        //db.AddInParameter(command, "@Value", DbType.String, ObjLEGRow.Value);
                        if (!ObjLEGRow.IsCurrencyNull())
                        {
                            db.AddInParameter(command, "@Currency", DbType.String, ObjLEGRow.Currency);
                        }
                        db.AddInParameter(command, "@Status", DbType.String, ObjLEGRow.Status);
                        //db.AddInParameter(command, "@Client_Credibility", DbType.String, ObjLEGRow.Client_Credibility);
                        //db.AddInParameter(command, "@Client_Net_Worth", DbType.String, ObjLEGRow.Client_Net_Worth);
                        if (!ObjLEGRow.IsField_Officer_EMailIDNull())
                        {
                            db.AddInParameter(command, "@Field_Officer_EmailID", DbType.String, ObjLEGRow.Field_Officer_EMailID);
                        }

                        if (!ObjLEGRow.IsField_Officer_MobileNoNull())
                        {
                            db.AddInParameter(command, "@Field_Officer_MobileNo", DbType.String, ObjLEGRow.Field_Officer_MobileNo);
                        }
                        if (!ObjLEGRow.IsCancelledNull())
                        {
                            db.AddInParameter(command, "@Cancelled", DbType.Boolean, ObjLEGRow.Cancelled);
                        }
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjLEGRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjLEGRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjLEGRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjLEGRow.Modified_On);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjLEGRow.Company_ID);
                        db.AddInParameter(command, "@XML_legalDeltails", DbType.String, ObjLEGRow.XML_LegalDeltails);
                        if (!ObjLEGRow.IsXML_LegalAssetDetailsNull())
                        {
                            db.AddInParameter(command, "@XML_LegalAssetDetails", DbType.String, ObjLEGRow.XML_LegalAssetDetails);
                        }
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));
                      
                        db.AddOutParameter(command, "@RefNo", DbType.String, 100);
                        if (!ObjLEGRow.IsAgencyTypeNull())
                        {
                            db.AddInParameter(command, "@Agency_Type", DbType.Int32, ObjLEGRow.AgencyType);
                        }

                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        strFIRNumber_Out = (string)command.Parameters["@RefNo"].Value;

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected1 = (int)command.Parameters["@ErrorCode"].Value;
                        }
                    }

                }
                catch (Exception ex)
                {
                    intRowsAffected1 = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected1;
            }
            #endregion

            //technical clearance//

            #region Create
            int intRowsAffected2;
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_TechincalClearanceDataTable ObjTEC_DAL;
            public int FunPubCreateTechClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TechDataTable, out string strFIRNumber_Out)
            {
                try
                {
                    strFIRNumber_Out = string.Empty;

                    ObjTEC_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_TechincalClearanceDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_TechDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_TechincalClearanceDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_TechincalClearanceRow ObjTECRow in ObjTEC_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_InsertTech_trans_Details");
                        if (!ObjTECRow.IsLOB_IDNull())
                        {
                            db.AddInParameter(command, "@LOB_ID", DbType.Int32, ObjTECRow.LOB_ID);
                        }
                        if (!ObjTECRow.IsBranch_IDNull())
                        {
                            db.AddInParameter(command, "@Location_ID", DbType.Int32, ObjTECRow.Branch_ID);
                        }
                        if (!ObjTECRow.IsDoc_TypeNull())
                        {
                            db.AddInParameter(command, "@Doc_Type", DbType.Int32, ObjTECRow.Doc_Type);
                        }
                        if (!ObjTECRow.IsDoc_Ref_IDNull())
                        {
                            db.AddInParameter(command, "@Doc_Ref_ID", DbType.Int32, ObjTECRow.Doc_Ref_ID);
                        }
                        if (!ObjTECRow.IsCustomer_IDNull())
                        {
                            db.AddInParameter(command, "@Customer_ID", DbType.Int32, ObjTECRow.Customer_ID);
                        }
                        //db.AddInParameter(command, "@Enquiry_Response_ID", DbType.Int32, ObjFIRRow.Enquiry_Response_ID);
                        if (!ObjTECRow.IsTerms_ConditionsNull())
                        {
                            db.AddInParameter(command, "@Terms_Conditions", DbType.String, ObjTECRow.Terms_Conditions);
                        }
                        if (!ObjTECRow.IsTech_IDNull())
                        {
                            db.AddInParameter(command, "@Tech_ID", DbType.Int32, ObjTECRow.Tech_ID);
                        }
                        if (!ObjTECRow.IsTech_NoNull())
                        {
                            db.AddInParameter(command, "@Tech_No", DbType.String, ObjTECRow.Tech_No);
                        }
                        if (!ObjTECRow.IsTech_DateNull())
                        {
                            db.AddInParameter(command, "@Tech_Date", DbType.DateTime, ObjTECRow.Tech_Date);
                        }
                        if (!ObjTECRow.IsRoundNull())
                        {
                            db.AddInParameter(command, "@Round", DbType.String, ObjTECRow.Round);
                        }
                        if (!ObjTECRow.IsRequested_ByNull())
                        {
                            db.AddInParameter(command, "@Requested_By", DbType.String, ObjTECRow.Requested_By);
                        }
                        if (!ObjTECRow.IsRequested_DateNull())
                        {
                            db.AddInParameter(command, "@Requested_Date", DbType.DateTime, ObjTECRow.Requested_Date);
                        }
                        if (!ObjTECRow.IsEntity_IDNull())
                        {
                            db.AddInParameter(command, "@Entity_ID", DbType.Int32, ObjTECRow.Entity_ID);
                        }
                        if (!ObjTECRow.IsField_RequestNull())
                        {

                            db.AddInParameter(command, "@Field_Request", DbType.String, ObjTECRow.Field_Request);
                        }
                        if (!ObjTECRow.IsNotification_Sent_byNull())
                        {
                            db.AddInParameter(command, "@Notification_Sent_by", DbType.String, ObjTECRow.Notification_Sent_by);
                        }
                        if (!ObjTECRow.IsResponded_ByNull())
                        {
                            db.AddInParameter(command, "@Responded_By", DbType.Int32, ObjTECRow.Responded_By);
                        }
                        if (!ObjTECRow.IsResponded_DateNull())
                        {
                            db.AddInParameter(command, "@Responded_Date", DbType.DateTime, ObjTECRow.Responded_Date);
                        }
                        if (!ObjTECRow.IsResponse_DesignationNull())
                        {
                            db.AddInParameter(command, "@Response_Designation", DbType.String, ObjTECRow.Response_Designation);
                        }
                        if (!ObjTECRow.IsResponseNull())
                        {
                            db.AddInParameter(command, "@Response", DbType.String, ObjTECRow.Response);
                        }
                        //db.AddInParameter(command, "@Value", DbType.String, ObjLEGRow.Value);
                        if (!ObjTECRow.IsCurrencyNull())
                        {
                            db.AddInParameter(command, "@Currency", DbType.String, ObjTECRow.Currency);
                        }
                        db.AddInParameter(command, "@Status", DbType.String, ObjTECRow.Status);
                        //db.AddInParameter(command, "@Client_Credibility", DbType.String, ObjLEGRow.Client_Credibility);
                        //db.AddInParameter(command, "@Client_Net_Worth", DbType.String, ObjLEGRow.Client_Net_Worth);
                        if (!ObjTECRow.IsField_Officer_EMailIDNull())
                        {
                            db.AddInParameter(command, "@Field_Officer_EmailID", DbType.String, ObjTECRow.Field_Officer_EMailID);
                        }

                        if (!ObjTECRow.IsField_Officer_MobileNoNull())
                        {
                            db.AddInParameter(command, "@Field_Officer_MobileNo", DbType.String, ObjTECRow.Field_Officer_MobileNo);
                        }
                        if (!ObjTECRow.IsCancelledNull())
                        {
                            db.AddInParameter(command, "@Cancelled", DbType.Boolean, ObjTECRow.Cancelled);
                        }
                        db.AddInParameter(command, "@Created_By", DbType.Int32, ObjTECRow.Created_By);
                        db.AddInParameter(command, "@Created_On", DbType.DateTime, ObjTECRow.Created_On);
                        db.AddInParameter(command, "@Modified_By", DbType.Int32, ObjTECRow.Modified_By);
                        db.AddInParameter(command, "@Modified_On", DbType.DateTime, ObjTECRow.Modified_On);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjTECRow.Company_ID);
                        db.AddInParameter(command, "@XML_TechDeltails", DbType.String, ObjTECRow.XML_TechDeltails);
                        db.AddInParameter(command, "@@xmloutflow", DbType.String, ObjTECRow.xmloutflow);
                        //if (!ObjTECRow.IsXML_LegalAssetDetailsNull())
                        //{
                        //    db.AddInParameter(command, "@XML_LegalAssetDetails", DbType.String, ObjTECRow.XML_LegalAssetDetails);
                        //}
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));

                        db.AddOutParameter(command, "@RefNo", DbType.String, 100);
                        if (!ObjTECRow.IsAgencyTypeNull())
                        {
                            db.AddInParameter(command, "@Agency_Type", DbType.Int32, ObjTECRow.AgencyType);
                        }

                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);
                        strFIRNumber_Out = (string)command.Parameters["@RefNo"].Value;

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
                        {
                            intRowsAffected2 = (int)command.Parameters["@ErrorCode"].Value;
                        }
                    }

                }
                catch (Exception ex)
                {
                    intRowsAffected2 = 50;
                    ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return intRowsAffected2;
            }
            #endregion

            # region Update


            public int FunPubUpdateFIRResponse(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable)
            {
                try
                {

                    ObjFIR_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjFIRRow in ObjFIR_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_ORG_Update_FieldInformationReportRespond");

                        db.AddInParameter(command, "@FIR_No", DbType.String, ObjFIRRow.FIR_No);
                        //db.AddInParameter(command, "@Responded_By", DbType.String, ObjFIRRow.Requested_By);
                        db.AddInParameter(command, "@Responded_By", DbType.String, ObjFIRRow.Responded_By );
                        //db.AddInParameter(command, "@Responded_Date", DbType.DateTime, ObjFIRRow.Requested_Date);
                        db.AddInParameter(command, "@Responded_Date", DbType.DateTime, ObjFIRRow.Responded_Date);
                        db.AddInParameter(command, "@Response_Designation", DbType.String, ObjFIRRow.Response_Designation);
                        db.AddInParameter(command, "@Field_Officer_EmailID", DbType.String, ObjFIRRow.Field_Officer_EMailID);
                        db.AddInParameter(command, "@Field_Officer_MobileNo", DbType.String, ObjFIRRow.Field_Officer_MobileNo);
                        db.AddInParameter(command, "@Client_Credibility", DbType.String, ObjFIRRow.Client_Credibility);
                        db.AddInParameter(command, "@Value", DbType.String, ObjFIRRow.Value);
                        db.AddInParameter(command, "@Currency", DbType.String, ObjFIRRow.Currency);
                        db.AddInParameter(command, "@Client_Net_Worth", DbType.String, ObjFIRRow.Client_Net_Worth);
                        db.AddInParameter(command, "@Response", DbType.String, ObjFIRRow.Response);
                        db.AddInParameter(command, "@Status", DbType.String, ObjFIRRow.Status);
                        db.AddInParameter(command, "@Round", DbType.String, ObjFIRRow.Round);

                        //db.AddInParameter(command, "@Cancelled", DbType.Boolean, ObjFIRRow.Cancelled);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));


                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
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


            public int FunPubUpdateFIRCancel(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable)
            {
                try
                {

                    ObjFIR_DAL = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable)ClsPubSerialize.DeSerialize(bytesObjS3G_ORG_FIRDataTable, SerMode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportDataTable));

                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");

                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FieldInformationReportRow ObjFIRRow in ObjFIR_DAL.Rows)
                    {
                        DbCommand command = db.GetStoredProcCommand("S3G_Update_FieldInformationReportCancel");

                        db.AddInParameter(command, "@FIR_No", DbType.String, ObjFIRRow.FIR_No);
                        db.AddInParameter(command, "@Remarks", DbType.String, ObjFIRRow.Field_Request);
                        db.AddInParameter(command, "@UserID", DbType.Int32, ObjFIRRow.Modified_By);
                        db.AddOutParameter(command, "@ErrorCode", DbType.Int32, sizeof(Int64));


                        //db.ExecuteNonQuery(command);
                        db.FunPubExecuteNonQuery(command);

                        if ((int)command.Parameters["@ErrorCode"].Value > 0)
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

            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable
     ObjFIR_DataTable = new S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable();
            public S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable FunPubGetFIRTransDoc(SerializationMode Sermode, byte[] bytesFIRTransactionDocDetails)
            {

                S3GBusEntity.Origination.CreditMgtServices ObjFIRDataTable = new S3GBusEntity.Origination.CreditMgtServices();
                ObjFIR_DataTable = (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable)ClsPubSerialize.DeSerialize(bytesFIRTransactionDocDetails, Sermode, typeof(S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable));
                try
                {
                    //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
                    DbCommand command = db.GetStoredProcCommand("S3G_ORG_GetFIRTransDocDetails");
                    foreach (S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsRow ObjRow in ObjFIR_DataTable.Rows)
                    {

                        db.AddInParameter(command, "@Field_Information_Report_ID", DbType.Int32, ObjRow.Field_Information_Report_ID);
                        db.AddInParameter(command, "@Company_ID", DbType.Int32, ObjRow.Company_ID);

                    }
                    //db.LoadDataSet(command, ObjFIRDataTable, ObjFIRDataTable.S3G_ORG_FIRTransactionDocDetails.TableName);
                    db.FunPubLoadDataSet(command, ObjFIRDataTable, ObjFIRDataTable.S3G_ORG_FIRTransactionDocDetails.TableName);
                }
                catch (Exception ex)
                {
                     ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                    throw ex;
                }
                return ObjFIRDataTable.S3G_ORG_FIRTransactionDocDetails;
            }

            #endregion

        }
    }
}

