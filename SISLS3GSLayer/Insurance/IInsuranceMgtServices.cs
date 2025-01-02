using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;


namespace S3GServiceLayer.Insurance
{
    // NOTE: If you change the interface name "IInsuranceMgtService" here, you must also update the reference to "IInsuranceMgtService" in Web.config.
    [ServiceContract]
    public interface IInsuranceMgtServices
    {
        #region Asset Insurance Entry

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry, out string strAINSENO);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateAssetEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateInvoiceEntryDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetEntry);
        
        #endregion

        #region InsuranceRemainder
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyInsDetails(SerializationMode SerMode, byte[] byteObjS3G_Insurance_Remainder);
        #endregion

        #region Asset Insurance Claim

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable, out string strICNNO);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateAssetInsuranceClaim(SerializationMode SerMode, byte[] byteObjS3G_Insurance_AssetInsuranceClaim_DataTable);

        #endregion
    }
}
