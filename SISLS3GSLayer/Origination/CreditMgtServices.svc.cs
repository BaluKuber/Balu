#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDC Master
/// Created By			: Kaliraj K
/// Created Date		: 01-Jun-2010
/// Purpose	            : 
/// <Program Summary>
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Origination
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "CreditMgtServices" here, you must also update the reference to "CreditMgtServices" in Web.config.
    public class CreditMgtServices : ICreditMgtServices
    {
        int intResult;
        byte[] bytesDataTable;
        S3GDALayer.Origination.CreditMgtServices.ClsPubCreditScore ObjCreditScore = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditScore();
        S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditScore ObjTACreditScore = new S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditScore();
        S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCreditParameterApproval = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();


        #region ICreditMgtServices Members
        public byte[] FunPubQueryCreditScoreGuideParameter(SerializationMode SerMode, byte[] bytesObjCreditScoreGuideParameterDataTable_SERLAY)
        {
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreGuideParameterDataTable DtCreditScoreGuideParameter;
            S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();

            S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();

            DtCreditScoreGuideParameter = ObjGlobalCreditParameter.FunPubQueryCreditScoreGuideParameter(SerMode, bytesObjCreditScoreGuideParameterDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(DtCreditScoreGuideParameter, SerializationMode.Binary);
            return bytesDataTable;
        }
        public byte[] FunPubQueryGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDataTable_SERLAY)
        {
            DataSet DsGlobalCreditParameter = new DataSet();
            try
            {
                //S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_Global_Credit_ParameterDataTable DtGlobalCreditParameter;
                S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();
                DsGlobalCreditParameter = ObjGlobalCreditParameter.FunPubQueryGlobalCreditParameter(SerMode, bytesObjGlobalCreditParameterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DsGlobalCreditParameter, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting GlobalCreditParameter :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                DsGlobalCreditParameter.Dispose();
                DsGlobalCreditParameter = null;
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryGlobalCreditParameterScore(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterScoreDataTable_SERLAY)
        {
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterCreditScoreDataTable DtGlobalCreditParameterScore;
            S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();
            DtGlobalCreditParameterScore = ObjGlobalCreditParameter.FunPubQueryGlobalCreditScoreParameter(SerMode, bytesObjGlobalCreditParameterScoreDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(DtGlobalCreditParameterScore, SerializationMode.Binary);
            return bytesDataTable;
        }

        public byte[] FunPubQueryGlobalCreditParameterApproval(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterApprovalDatatable_SERLAY)
        {
            S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GlobalCreditParameterApprovalDataTable DtGlobalCreditParameterApproval;
            S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();
            DtGlobalCreditParameterApproval = ObjGlobalCreditParameter.FunPubQueryGlobalCreditParameterApproval(SerMode, bytesObjGlobalCreditParameterApprovalDatatable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(DtGlobalCreditParameterApproval, SerializationMode.Binary);
            return bytesDataTable;
        }
        public int FunPubCreateGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();
                return ObjGlobalCreditParameter.FunPubCreateGlobalCreditParameter(SerMode, bytesObjGlobalCreditParameterDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
        public int FunPubModifyGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter ObjGlobalCreditParameter = new S3GDALayer.Origination.CreditMgtServices.ClsPubGlobalCreditParameter();
                return ObjGlobalCreditParameter.FunPubModifyGlobalCreditParameter(SerMode, bytesObjGlobalCreditParameterDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryCustomerMasterByCode(SerializationMode SerMode, byte[] bytesCustomerMasterDataTableDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable DtCustomerMasterDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFieldInformationReport = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                DtCustomerMasterDataTable = ObjFieldInformationReport.FunPubQueryCustomerMasterByCode(SerMode, bytesCustomerMasterDataTableDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DtCustomerMasterDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubQueryEnquiryResponse(SerializationMode Sermode, byte[] bytesEnquiryResponseDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable DtEnquiryResponseDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFieldInformationReport = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                DtEnquiryResponseDataTable = ObjFieldInformationReport.FunPubQueryEnquiryResponse(Sermode, bytesEnquiryResponseDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DtEnquiryResponseDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubGetFIRTransDoc(SerializationMode Sermode, byte[] bytesFIRTransactionDocDetails)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_FIRTransactionDocDetailsDataTable DtFIRDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFieldInformationReport = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                DtFIRDataTable = ObjFieldInformationReport.FunPubGetFIRTransDoc(Sermode, bytesFIRTransactionDocDetails);
                bytesDataTable = ClsPubSerialize.Serialize(DtFIRDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateFIR(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY, out string FIRNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                return ObjFIR.FunPubCreateFIR(SerMode, bytesObjS3G_ORG_FIRDataTable_SERLAY, out FIRNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateLegalClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LEGDataTable, out string FIRNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                return ObjFIR.FunPubCreateLegalClearance(SerMode, bytesObjS3G_ORG_LEGDataTable, out FIRNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubCreateTechClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TechDataTable, out string FIRNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                return ObjFIR.FunPubCreateTechClearance(SerMode, bytesObjS3G_ORG_TechDataTable, out FIRNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryEnquiryResponseAllRows(SerializationMode Sermode, byte[] bytesEnquiryResponseDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryResponseDataTable DtEnquiryResponseDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFieldInformationReport = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                DtEnquiryResponseDataTable = ObjFieldInformationReport.FunPubQueryEnquiryResponseAllRows(Sermode, bytesEnquiryResponseDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DtEnquiryResponseDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }



        public byte[] FunPubQueryEntityMaster(SerializationMode Sermode, byte[] bytesEntityMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EntityMasterDataTable DtEntityMasterDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFieldInformationReport = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                DtEntityMasterDataTable = ObjFieldInformationReport.FunPubQueryEntityMaster(Sermode, bytesEntityMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DtEntityMasterDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubUpdateFIRResponse(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                return ObjFIR.FunPubUpdateFIRResponse(SerMode, bytesObjS3G_ORG_FIRDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubUpdateFIRCancel(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport ObjFIR = new S3GDALayer.Origination.CreditMgtServices.ClsPubFieldInformationReport();
                return ObjFIR.FunPubUpdateFIRCancel(SerMode, bytesObjS3G_ORG_FIRDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }



        public byte[] FunPubQueryCreditPArameterApproval(SerializationMode Sermode, byte[] bytesCreditParameterApprovalDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDataTable DtCreditParameterApprovalDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCreditParameterApproval = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                DtCreditParameterApprovalDataTable = ObjCreditParameterApproval.FunPubQueryCreditPArameterApproval(Sermode, bytesCreditParameterApprovalDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(DtCreditParameterApprovalDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryCreditParameterTransaction(SerializationMode Sermode, byte[] bytesCreditParameterTransactionDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable DtCreditParameterTransactionDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCreditParameterApproval = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                DtCreditParameterTransactionDataTable = ObjCreditParameterApproval.FunPubQueryCreditParameterTransaction(Sermode, bytesCreditParameterTransactionDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(DtCreditParameterTransactionDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryCreditParameterTransactionID(SerializationMode Sermode, int CPTID)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParamTransactionDataTable DtCreditParameterTransactionDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCreditParameterApproval = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                DtCreditParameterTransactionDataTable = ObjCreditParameterApproval.FunPubQueryCreditParameterTransactionID(Sermode, CPTID);
                bytesDataTable = ClsPubSerialize.Serialize(DtCreditParameterTransactionDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubQueryCreditParameterApproval_Enquiry(SerializationMode Sermode, byte[] bytesCreditParameterTransactionDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_EnquiryDataTable DtCreditParameterApprovalEnquiryDataTable;
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCreditParameterApproval = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                DtCreditParameterApprovalEnquiryDataTable = ObjCreditParameterApproval.FunPubQueryCreditParameterApproval_Enquiry(Sermode, bytesCreditParameterTransactionDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DtCreditParameterApprovalEnquiryDataTable, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubQueryCreditParameterApproval_ScoreBoard(SerializationMode SerMode, byte[] bytesObjCreditScoreMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_ScoreBoardDataTable dtCreditScoreMaster;
                dtCreditScoreMaster = ObjCreditParameterApproval.FunPubQueryCreditParameterApproval_ScoreBoard(SerMode, bytesObjCreditScoreMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCreditScoreMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }



        public byte[] FunPubQueryCreditParameterApprovalDetails_Enquiry(SerializationMode SerMode, byte[] bytesCreditParameterApprovalDetails, int CreditParamTransId)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditParameterApprovalDetailsDataTable dtCreditApprovalDetails;
                dtCreditApprovalDetails = ObjCreditParameterApproval.FunPubQueryCreditParameterApprovalDetails_Enquiry(SerMode, bytesCreditParameterApprovalDetails, CreditParamTransId);
                bytesDataTable = ClsPubSerialize.Serialize(dtCreditApprovalDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryCustomerDetailsById(SerializationMode SerMode, byte[] bytesObjCustomerMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CustomerMasterDataTable dtCustomerMaster;
                dtCustomerMaster = ObjCreditParameterApproval.FunPubQueryCustomerDetailsById(SerMode, bytesObjCustomerMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCustomerMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        // to validate user
        public byte[] FunPubQueryUserIsValid(SerializationMode SerMode, byte[] bytesObjUserDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_UserIsValidDataTable dtUser;
                dtUser = ObjCreditParameterApproval.FunPubQueryUserIsValid(SerMode, bytesObjUserDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtUser, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }


        // customer mode
        public byte[] FunPubQueryEnquiryCustomer(SerializationMode SerMode, byte[] bytesEnquiryCustomer_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_GetCreditParameterApproval_CustomerEnquiryDataTable dtCustomer;
                dtCustomer = ObjCreditParameterApproval.FunPubQueryEnquiryCustomer(SerMode, bytesEnquiryCustomer_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCustomer, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }



        public int FunPubCreateCreditParameterApprovalCustomerDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CustomerDetailsDataTable_SERLAY, int isFinalApproval)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCPA = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                return ObjCPA.FunPubCreateCreditParameterApprovalCustomerDetails(SerMode, bytesObjS3G_ORG_CustomerDetailsDataTable_SERLAY, isFinalApproval);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region CreditScoreGuide

        public int FunPubCreateCreditScoreDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable, out int outNoofYear, out int outCreditScoreID)
        {
            try
            {
                intResult = ObjCreditScore.FunPubCreateCreditScoreDetails(SerMode, bytesObjS3G_ORG_CreditScoreMasterDataTable, out outNoofYear, out outCreditScoreID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in CreditScore Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubDeleteCreditScoreParamDetails(int intCreditScoreParamId)
        {
            try
            {
                intResult = ObjCreditScore.FunPubDeleteCreditScoreParamDetails(intCreditScoreParamId);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in CreditScore Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubQueryCreditScoreParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable)
        {
            DataSet dsCreditScore = null;
            try
            {
                dsCreditScore = ObjCreditScore.FunPubQueryCreditScoreParameterDetails(SerMode, bytesObjS3G_ORG_CreditScoreMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsCreditScore, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting CreditScore :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsCreditScore.Dispose();
                dsCreditScore = null;
            }
            return bytesDataTable;

        }

        public byte[] FunPubQueryCreditScoreDetails(SerializationMode SerMode, byte[] bytesObjCreditScoreMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable dtCreditScoreMaster;
                dtCreditScoreMaster = ObjCreditScore.FunPubQueryCreditScoreDetails(SerMode, bytesObjCreditScoreMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCreditScoreMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }




        public byte[] FunPubQueryEnquiryCustomerAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerApprisalDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_EnquiryCustomerAppraisalDataTable dtEnquiryCustomerAppraisal;
                dtEnquiryCustomerAppraisal = ObjCreditParameterApproval.FunPubQueryEnquiryCustomerAppraisal(SerMode, bytesObjEnquiryCustomerApprisalDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtEnquiryCustomerAppraisal, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }


        public int FunPubCreateCreditParameterApproval(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADataTable_SERLAY, out int Credit_Parameter_Approval_ID)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCPA = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                return ObjCPA.FunPubCreateCreditParameterApproval(SerMode, bytesObjS3G_ORG_CPADataTable_SERLAY, out Credit_Parameter_Approval_ID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateCreditParameterApprovalDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADataTable_SERLAY, int FinalApproval, decimal sanctionAmount, out string sanctionnumber)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval ObjCPA = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditParameterApproval();
                return ObjCPA.FunPubCreateCreditParameterApprovalDetails(SerMode, bytesObjS3G_ORG_CPADataTable_SERLAY, FinalApproval, sanctionAmount, out sanctionnumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region Credit Guide Transaction

        public int FunPubCreateCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable)
        {
            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditGuideTransaction ObjCreditGuideTrans = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditGuideTransaction();
                return ObjCreditGuideTrans.FunPubCreateCreditGuideTransaction(SerMode, bytesObjS3G_ORG_CreditGuideTrans_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubModifyCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable)
        {

            try
            {
                S3GDALayer.Origination.CreditMgtServices.ClsPubCreditGuideTransaction ObjCreditGuideTrans = new S3GDALayer.Origination.CreditMgtServices.ClsPubCreditGuideTransaction();
                return ObjCreditGuideTrans.FunPubModifyCreditGuideTransaction(SerMode, bytesObjS3G_ORG_CreditGuideTrans_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion


        #region TA Credit Score Guide
        /// <summary>
        /// Added by Thangam M for Trade Advance
        /// </summary>

        public int FunPubCreateTACreditScoreDetails(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable, out int outNoofYear, out int outCreditScoreID)
        {
            try
            {
                intResult = ObjTACreditScore.FunPubCreateCreditScoreDetails(SerMode, bytesObjS3G_ORG_CreditScoreMasterDataTable, out outNoofYear, out outCreditScoreID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in CreditScore Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubQueryTACreditScoreParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable)
        {
            DataSet dsCreditScore = null;
            try
            {
                dsCreditScore = ObjTACreditScore.FunPubQueryCreditScoreParameterDetails(SerMode, bytesObjS3G_ORG_CreditScoreMasterDataTable);
                bytesDataTable = ClsPubSerialize.Serialize(dsCreditScore, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting CreditScore :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsCreditScore.Dispose();
                dsCreditScore = null;
            }
            return bytesDataTable;

        }

        public byte[] FunPubQueryTACreditScoreDetails(SerializationMode SerMode, byte[] bytesObjCreditScoreMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.Origination.CreditMgtServices.S3G_ORG_CreditScoreDataTable dtCreditScoreMaster;
                dtCreditScoreMaster = ObjTACreditScore.FunPubQueryCreditScoreDetails(SerMode, bytesObjCreditScoreMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtCreditScoreMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }


        #endregion

        #region TA Credit Guide Transaction

        public int FunPubTACreateCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable)
        {
            try
            {
                S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditGuideTransaction ObjCreditGuideTrans = new S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditGuideTransaction();
                return ObjCreditGuideTrans.FunPubCreateCreditGuideTransaction(SerMode, bytesObjS3G_ORG_CreditGuideTrans_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubTAModifyCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable)
        {

            try
            {
                S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditGuideTransaction ObjCreditGuideTrans = new S3GDALayer.TradeAdvance.CreditMgtServices.ClsPubCreditGuideTransaction();
                return ObjCreditGuideTrans.FunPubModifyCreditGuideTransaction(SerMode, bytesObjS3G_ORG_CreditGuideTrans_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion
    }
}
