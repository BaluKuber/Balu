using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Data;
using S3GDALayer;
using S3GDALayer.LoanAdmin;
using S3GServiceLayer.LoanAdmin;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.LoanAdmin
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "AssetMgtServices" here, you must also update the reference to "AssetMgtServices" in Web.config.
    public class AssetMgtServices : IAssetMgtServices
    {
        byte[] bytesDataTable;

        #region "Asset verification"
        /// <summary>
        /// Inserting records in Asset verification details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateAssetVerification(SerializationMode SMode, byte[] byteAssetService,out string strErrMsg)
        {
            try
            {

                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification objClsPubAssetVerification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification();
                return objClsPubAssetVerification.FunPubCreateAssetVerification(SMode, byteAssetService,out strErrMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updating records in Asset verification details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubModifyAssetVerification(SerializationMode SMode, byte[] byteAssetService, out string strErrMsg)
        {
            try
            {

                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification objClsPubAssetVerification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification();
                return objClsPubAssetVerification.FunPubModifyAssetVerification(SMode, byteAssetService,out strErrMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting Verified asset details 
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunVerifedAssetForModification(string strAssetNo)
        {
            DataTable dtAssetTable = new DataTable();
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification objClsPubAssetVerification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetVerification();
            dtAssetTable = objClsPubAssetVerification.FunVerifedAssetForModification(strAssetNo);
            bytesDataTable = ClsPubSerialize.Serialize(dtAssetTable, SerializationMode.Binary);
            return bytesDataTable;
        }
        
        #endregion

        #region "Asset Identification"

        /// <summary>
        /// To get PANum realted customer details
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunGetPANumCustomer(string strPANum)
        {
            DataTable dtPanCust = new DataTable();
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetIdentification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
            dtPanCust = objClsPubAssetIdentification.FunGetPANumCustomer(strPANum);
            bytesDataTable = ClsPubSerialize.Serialize(dtPanCust, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// To get Invoice no realted vendor details
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunGetAssetVendorDetails(int intVendorID)
        {
            DataTable dtVendor = new DataTable();
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetIdentification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
            dtVendor = objClsPubAssetIdentification.FunGetAssetVendorDetails(intVendorID);
            bytesDataTable = ClsPubSerialize.Serialize(dtVendor, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// To get Invoice no realted vendor details
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunGetAssetDetailsForVendor(int intInvoiceID, int intPASAID)
        {
            DataTable dtVendor = new DataTable();
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetIdentification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
            dtVendor = objClsPubAssetIdentification.FunGetAssetDetailsForVendor(intInvoiceID, intPASAID);
            bytesDataTable = ClsPubSerialize.Serialize(dtVendor, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// To get PASARefId for the PAN and SAN number
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunGetPASArefID(string intPAN, string intSAN)
        {
            DataTable dtVendor = new DataTable();
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetIdentification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
            dtVendor = objClsPubAssetIdentification.FunGetPASArefID(intPAN, intSAN);
            bytesDataTable = ClsPubSerialize.Serialize(dtVendor, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// Inserting records in Asset verification details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateAssetIdentification(SerializationMode SMode, byte[] byteAssetService, out string strDuplication, out string strDSNo)
        {
            try
            {

                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetVerification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
                return objClsPubAssetVerification.FunPubCreateAssetIdentification(SMode, byteAssetService, out strDuplication, out strDSNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get Invoice no realted vendor details
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunGetAssetIdentificationforModify(string strAssetID, int intCompanyID, int intBranchID)
        {
            try
            {
                DataTable dtVendor = new DataTable();
                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification objClsPubAssetIdentification = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetIdentification();
                dtVendor = objClsPubAssetIdentification.FunGetAssetIdentificationforModify(strAssetID, intCompanyID, intBranchID);
                bytesDataTable = ClsPubSerialize.Serialize(dtVendor, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Asset Acquisition Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region LeaseAssetSale
        // ----Lease Asset Sale (Created By- Irsathameen K)-------
        //Begin

        public int FunPubCreateLeaseAssetSaleDetails(SerializationMode SerMode, byte[] bytesObjLeaseAssetSaleDatatable_SERLAY, out string strLASNo)
        {
            try
            {

                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubLeaseAssetSale ObjLeaseAssetSale = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubLeaseAssetSale();
                return ObjLeaseAssetSale.FunPubCreateLeaseAssetSaleDetails(SerMode, bytesObjLeaseAssetSaleDatatable_SERLAY, out strLASNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        //End

        public int FunPubCreateLASDetails(SerializationMode SerMode, byte[] bytesObjLeaseAssetSaleDatatable_SERLAY, out string strLASNo)
        {
            try
            {
                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubLeaseAssetSale ObjLeaseAssetSale = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubLeaseAssetSale();
                return ObjLeaseAssetSale.FunPubCreateLASDetails(SerMode, bytesObjLeaseAssetSaleDatatable_SERLAY, out strLASNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion


        #region AssetMgtServices Asset Acquisition
        /// <summary>
        /// Insert Asset Acquisition Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjAssetAcquisitionDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateAssetAcquisition(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable_SERLAY, out string DSN)
        {
            try
            {
                S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetAcquisition ObjMonthClosure = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetAcquisition();
                return ObjMonthClosure.FunPubCreateAssetAcquisitionInt(SerMode, bytesObjAssetAcquisitionDataTable_SERLAY, out DSN);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Asset Acquisition Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubQueryAssetAcquisition(SerializationMode SerMode, byte[] bytesObjAssetAcquisitionDataTable_SERLAY)
        {
            S3GBusEntity.LoanAdmin.AssetMgtServices.S3G_LOANAD_LeaseAssetRegisterDataTable dtLeaseAssetRegister = null;
            S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetAcquisition ObjAssetAcquisition = new S3GDALayer.LoanAdmin.AssetMgtServices.ClsPubAssetAcquisition();
            dtLeaseAssetRegister = ObjAssetAcquisition.FunPubQueryAssetAcquisition(SerMode, bytesObjAssetAcquisitionDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(dtLeaseAssetRegister, SerMode);

            return bytesDataTable;


        }
        #endregion
    }
}
