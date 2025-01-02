using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IPricingMgtServices" here, you must also update the reference to "IPricingMgtServices" in Web.config.
    [ServiceContract]
    public interface IPricingMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePricingApproval(SerializationMode SerMode, byte[] bytesObjPricingApprovalDatatable_SERLAY, out int intNoOfApproval, out int intApprovedCount);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateProposalApproval(SerializationMode SerMode, byte[] bytesObjPricingApprovalDatatable_SERLAY, out int intNoOfApproval, out int intApprovedCount);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePricingInt(SerializationMode SerMode, byte[] bytesObjPricingDatatable_SERLAY, out string strOfferNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyPricingInt(SerializationMode SerMode, byte[] bytesObjPricingDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPricingDetails(int intPricingId, int intCompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubWithDrawPricingInt(int intPricingId, int intCompanyId, int intCreatedBy);
    }
}
