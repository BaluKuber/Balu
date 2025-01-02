using System.ServiceModel;
using S3GBusEntity;

namespace S3GServiceLayer.LoanAdmin
{
    // NOTE: If you change the interface name "IAssetMgtServices" here, you must also update the reference to "IAssetMgtServices" in Web.config.
    [ServiceContract]
    public interface IAssetMgtServices
    {
        #region "Asset Verification"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetVerification(SerializationMode SerMode, byte[] bytesObjAssetVerificationDAL_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyAssetVerification(SerializationMode SerMode, byte[] bytesObjAssetVerificationDAL_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunVerifedAssetForModification(string strAssetNo);

        #endregion

        #region "Asset Identification"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetPANumCustomer(string strPANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAssetVendorDetails(int intVendorID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAssetDetailsForVendor(int intInvoiceID, int intPASAID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetPASArefID(string intPAN, string intSAN);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetIdentification(SerializationMode SerMode, byte[] bytesObjAssetVerificationDAL_SERLAY, out string strDuplication, out string strDSNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAssetIdentificationforModify(string strAssetID, int intCompanyID, int intBranchID);

        #endregion


        #region LeaseAssetSale

        //--------------Lease Asset Sale( Created By -Irsathameen K)---------------
        //Begin

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLeaseAssetSaleDetails(SerializationMode SerMode, byte[] bytesObjLeaseAssetSaleDatatable_SERLAY, out string strLASNo);

        //End

        //Added for OPC
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLASDetails(SerializationMode SerMode, byte[] bytesObjLeaseAssetSaleDatatable_SERLAY, out string strLASNo);

        #endregion

        #region AssetMgtServices Asset Acquisition

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetAcquisition(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable_SERLAY, out string DSN);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryAssetAcquisition(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable_SERLAY);

        #endregion

    }
}