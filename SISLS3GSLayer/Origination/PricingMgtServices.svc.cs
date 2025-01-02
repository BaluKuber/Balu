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
    // NOTE: If you change the class name "PricingMgtServices" here, you must also update the reference to "PricingMgtServices" in Web.config.
    public class PricingMgtServices : IPricingMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] bytesDataTable;

        #region IPricingMgtServices Members
        public int FunPubCreatePricingApproval(SerializationMode SerMode, byte[] bytesObjPricingApprovalDatatable_SERLAY, out int intNoOfApproval, out int intApprovedCount)
        {
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricingApproval ObjPricingApproval = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricingApproval();
                return ObjPricingApproval.FunPubCreatePricingApproval(SerMode, bytesObjPricingApprovalDatatable_SERLAY, out intNoOfApproval, out intApprovedCount);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubCreateProposalApproval(SerializationMode SerMode, byte[] bytesObjPricingApprovalDatatable_SERLAY, out int intNoOfApproval, out int intApprovedCount)
        {
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricingApproval ObjPricingApproval = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricingApproval();
                return ObjPricingApproval.FunPubCreateProposalApproval(SerMode, bytesObjPricingApprovalDatatable_SERLAY, out intNoOfApproval, out intApprovedCount);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }


        public int FunPubCreatePricingInt(SerializationMode SerMode, byte[] bytesObjPricingDatatable_SERLAY,out string strOfferNumber_Out)
        {
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricing ObjPricing = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricing();
                intResult = ObjPricing.FunPubCreatePricingInt(SerMode, bytesObjPricingDatatable_SERLAY, out strOfferNumber_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Pricing Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyPricingInt(SerializationMode SerMode, byte[] bytesObjPricingDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricing ObjPricing = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricing();
                intResult = ObjPricing.FunPubModifyPricingInt(SerMode, bytesObjPricingDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Pricing Updation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubGetPricingDetails(int intPricingId, int intCompanyId)
        {
            DataSet dsPricingDetails = new DataSet();
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricing ObjPricing = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricing();
                dsPricingDetails = ObjPricing.FunPubGetPricingDetails(intPricingId, intCompanyId);
                bytesDataTable = ClsPubSerialize.Serialize(dsPricingDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting Pricing Details:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public int FunPubWithDrawPricingInt(int intPricingId, int intCompanyId, int intCreatedBy)
        {
            try
            {
                S3GDALayer.Origination.PricingMgtServices.ClsPubPricing ObjPricing = new S3GDALayer.Origination.PricingMgtServices.ClsPubPricing();
                intResult = ObjPricing.FunPubWithDrawPricingInt(intPricingId, intCompanyId, intCreatedBy);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in witdrwing pricing:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        #endregion

    }
}
