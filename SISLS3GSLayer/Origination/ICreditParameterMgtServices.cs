
using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "ICreditParameterMgtServices" here, you must also update the reference to "ICreditParameterMgtServices" in Web.config.
    [ServiceContract]
    public interface ICreditParameterMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransDTO, out string CreditParamNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        void FunPubInsertCreditParamTransactionScoreDetails(CreditParamterTransScoreDTO ObjCreditParamterTransScoreDTO);



        #region Credit Parameter Transaction
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryCustomerAppraisalCPT(int intAppraisalType, int intCompanyId, int intAppraisalId, int intPageSize, int intCurrentPage, string strSearchValue, string strOrderBy, int intTotalRecords);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCreditParameterTransaction(int intCreditParamTransId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransactionEntity);
        #endregion
    }
}
