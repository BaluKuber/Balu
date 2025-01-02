using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.ServiceModel.Activation;


namespace S3GServiceLayer.Insurance
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "InsuranceMgtService" here, you must also update the reference to "InsuranceMgtService" in Web.config.
    public class InsuranceMgtServices : IInsuranceMgtServices
    {
        byte[] bytesDataTable;
        int intResult;

        #region Asset Insurance Entry

        public int FunPubCreateAssetEntryDetails(S3GBusEntity.SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry, out string strAINSENO)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry objClsPubAssetEntry = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry();
                return objClsPubAssetEntry.FunPubCreateAssetEntryDetails(SerMode, byteObjS3G_Insurance_AssetEntry, out strAINSENO);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubUpdateAssetEntryDetails(S3GBusEntity.SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry objClsPubAssetEntry = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry();
                return objClsPubAssetEntry.FunPubUpdateAssetEntryDetails(SerMode, byteObjS3G_Insurance_AssetEntry);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubUpdateInvoiceEntryDetails(S3GBusEntity.SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry objClsPubAssetEntry = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceEntry();
                return objClsPubAssetEntry.FunPubUpdateInvoiceEntryDetails(SerMode, byteObjS3G_Insurance_AssetEntry);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        

        #endregion

        #region InsuranceRemainder
        public int FunPubModifyInsDetails(S3GBusEntity.SerializationMode SerMode, byte[] byteObjS3G_Insurance_Remainder)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubInsuranceRemainder objClsPubInsuranceRemainder = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubInsuranceRemainder();
                return objClsPubInsuranceRemainder.FunPubModifyInsDetails(SerMode, byteObjS3G_Insurance_Remainder);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Insurance Claim

        public int FunPubCreateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, out string strICNNO)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceClaim objClsPubAssetClaim = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceClaim();
                return objClsPubAssetClaim.FunPubCreateAssetInsuranceClaim(SerMode, byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, out strICNNO);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubUpdateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable)
        {
            try
            {

                S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceClaim objClsPubAssetClaim = new S3GDALayer.Insurance.InsuranceMgtServices.ClsPubAssetInsuranceClaim();
                return objClsPubAssetClaim.FunPubUpdateAssetInsuranceClaim(SerMode, byteObjS3G_Insurance_AssetInsuranceClaim_DataTable);

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
