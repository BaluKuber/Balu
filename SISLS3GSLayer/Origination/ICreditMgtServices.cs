using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;


namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "ICreditMgtServices" here, you must also update the reference to "ICreditMgtServices" in Web.config.
    [ServiceContract]
    public interface ICreditMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDatatable_SERLAY);
        
         [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditScoreGuideParameter(SerializationMode SerMode, byte[] bytesObjCreditScoreGuideParameterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGlobalCreditParameter(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGlobalCreditParameterScore(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterScoreDataTable_SERLAY);

        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGlobalCreditParameterApproval(SerializationMode SerMode, byte[] bytesObjGlobalCreditParameterApprovalDatatable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCustomerMasterByCode(SerializationMode SerMode, byte[] bytesObjCreditScoreGuideParameterDataTable_SERLAY);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryResponse(SerializationMode Sermode, byte[] bytesEnquiryResponseDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetFIRTransDoc(SerializationMode Sermode, byte[] bytesFIRTransactionDocDetails);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryResponseAllRows(SerializationMode Sermode, byte[] bytesEnquiryResponseDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEntityMaster(SerializationMode Sermode, byte[] bytesEntityMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFIR(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY, out string FIRNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLegalClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LEGDataTable, out string FIRNumber_Out);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTechClearance(SerializationMode SerMode, byte[] bytesObjS3G_ORG_TechDataTable, out string FIRNumber_Out);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateFIRResponse(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateFIRCancel(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRDataTable_SERLAY);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditPArameterApproval(SerializationMode Sermode, byte[] bytesCreditParameterApproval, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditParameterTransaction(SerializationMode Sermode, byte[] bytesCreditParameterApproval, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditParameterTransactionID(SerializationMode SerMode, int CreditParamTransID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditParameterApproval_Enquiry(SerializationMode Sermode, byte[] bytesCreditParameterApproval);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditParameterApproval_ScoreBoard(SerializationMode Sermode, byte[] bytesCreditParameterApproval);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUserIsValid(SerializationMode Sermode, byte[] bytesCreditParameterApproval);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryCustomer(SerializationMode Sermode, byte[] bytesEnquiryCustomer);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditParameterApprovalDetails_Enquiry(SerializationMode Sermode, byte[] bytesCreditParameterApprovalDetails, int CreditParamTransId);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCreditParameterApprovalCustomerDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CustomerDetailsDataTable_SERLAY,int isFinalApproval);

        #region Credit Score Guide

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCreditScoreDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable, out int outNoofYear, out int outCreditScoreID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditScoreParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteCreditScoreParamDetails(int intCreditScoreParamId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCreditScoreDetails(SerializationMode SerMode, byte[] bytesObjCreditScoreMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryCustomerAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerApprisalDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCustomerDetailsById(SerializationMode SerMode, byte[] bytesObjCustomerMasterDataTable_SERLAY);
       

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCreditParameterApproval(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADataTable_SERLAY, out int Credit_Parameter_Approval_ID);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCreditParameterApprovalDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CPADataTable_SERLAY,int FinalApproval,decimal SanctionAmount,out string SanctionNumber);


        #endregion

        #region Credit Guide Transaction
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable);
        #endregion


        #region TA Credit Score Guide

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTACreditScoreDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable, out int outNoofYear, out int outCreditScoreID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTACreditScoreParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditScoreMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTACreditScoreDetails(SerializationMode SerMode, byte[] bytesObjCreditScoreMasterDataTable_SERLAY);

        #endregion

        #region TA Credit Guide Transaction
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTAModifyCreditGuideTransaction(SerializationMode SerMode, byte[] bytesObjS3G_ORG_CreditGuideTrans_DataTable);
        #endregion
    }
}
