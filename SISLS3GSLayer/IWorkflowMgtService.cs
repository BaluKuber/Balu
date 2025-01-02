using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;


namespace S3GServiceLayer
{
    [ServiceContract]
    public interface IWorkflowMgtService
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertWorkflowStatus(WorkFlowStatus objWorkflowStatus);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadPreDisDocTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadFIRTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadProforma(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadInvoice(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadPostDisb(string strEnquiryNo, int intCompanyId, int intDocument_Type, string PANum, string SANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadEnquiryCustomerAppraisal(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadCreditGuideTransaction(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadCreditParameterApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadPricing(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadPricingApproval(string strEnquiryNo, int intCompanyId, int intDocument_Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetUserAccess(int intUserId, int intCompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoadApplicationApproval(string strApplicationNo, int intCompanyId, int intUserId);


    }

}
