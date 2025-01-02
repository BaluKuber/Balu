using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Activation;
using S3GBusEntity.Collateral;



namespace S3GServiceLayer.Collateral
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "CollateralMgtServices" here, you must also update the reference to "CollateralMgtServices" in Web.config.
    public class CollateralMgtServices : ICollateralMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;

        byte[] bytesDataTable;


        #region Collateral Valuation
        public int FunPubCreateCollateralValuation(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollValuationDataTable, out string strCVal_No)
        {
            try
            {

                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralValuation ObjCollateralValuation = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralValuation();
                return ObjCollateralValuation.FunPubCreateCollateralValuation(SerMode, byteobjS3G_CLT_CollValuationDataTable, out strCVal_No);
              

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
      
        #endregion
        #region Collateral Master
        public int FunPubCreateCollateralMaster(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollateralMasterDataTable, out string strCollateralRefNo)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralMaster objCollateralMaster = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralMaster();
                return objCollateralMaster.FunPubCreateCollateralMaster(SerMode, byteobjS3G_CLT_CollateralMasterDataTable, out strCollateralRefNo);

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));

            }
        }
        #endregion


        #region TradeAdvance Collatral Master detail



        public int FunPubTradeAdvanceCollatralMaster(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollateralMasterDataTable, out string strCollateralRefNo)
        {
            try
            {
                
                S3GDALayer.TradeAdvance.ClsPubTACollatralMasterDetails.ClsPubTACollateralMaster objCollateralMaster = new S3GDALayer.TradeAdvance.ClsPubTACollatralMasterDetails.ClsPubTACollateralMaster();
                return objCollateralMaster.FunPubTACreateCollateralMaster(SerMode, byteobjS3G_CLT_CollateralMasterDataTable, out strCollateralRefNo);

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));

            }
        }












        #endregion


        #region COLLATERAL CAPTURE
        //public byte[] FunGetCustomerDetails(int intCustomerID, int intRefPoint, int intRefPointDetails)
        //{
        //    DataTable dtPANumCustomer = new DataTable();
        //    S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralMaster objClsPubRepossessionNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
        //    dtPANumCustomer = objClsPubRepossessionNote.FunPubCreateCollateralMaster(strPANum);
        //    bytesDataTable = ClsPubSerialize.Serialize(dtPANumCustomer, SerializationMode.Binary);
        //    return bytesDataTable;
        //}

        public int FunPubCreateCollateralCapture(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollCaptureDataTable, out string strCollCap_No)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralCapture ObjCollateralCapture = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralCapture();
                return ObjCollateralCapture.FunPubCreateCollateralCapture(SerMode, byteobjS3G_CLT_CollCaptureDataTable, out strCollCap_No);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunGetCollateralDetailsID(int intCompany, int intLiquidityType, int intCollLevelCode)
        {
            DataTable dtCollDetails = new DataTable();
            S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralCapture objClsCollCapture = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralCapture();
            dtCollDetails = objClsCollCapture.FunGetCollateralDetailsID(intCompany, intLiquidityType, intCollLevelCode);
            bytesDataTable = ClsPubSerialize.Serialize(dtCollDetails, SerializationMode.Binary);
            return bytesDataTable;
        }
        # endregion

        #region Collateral Storage
        public int FunPubCollateralStorage(SerializationMode SerMode, byte[] byteobjCollateralStorage, out string StrCollateralStorage)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralStorage ObjCollateralStorage = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralStorage();
                return ObjCollateralStorage.FunPubCollateralStorage(SerMode, byteobjCollateralStorage, out StrCollateralStorage);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Collateral Sale
        public int FunPubInsertCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode, out string CollateralSaleNo)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
                return objCollateralSale.FunPubInsertCollateralSale(byteobjCollateralSale, SerMode, out CollateralSaleNo);
             }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public int FunPubUpdateCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode, int ColSaleID)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
            return objCollateralSale.FunPubUpdateCollateralSale(byteobjCollateralSale, SerMode,ColSaleID);
             }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public int FunPubUpdateCollateralSaleInvoicedetails(byte[] byteobjCollateralSale, SerializationMode SerMode, int ColSaleID)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
                return objCollateralSale.FunPubUpdateCollateralSaleInvoicedetails(byteobjCollateralSale, SerMode, ColSaleID);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetColSaleDetails(int ColSaleId, int CompanyID)
        {
            try
            {
            byte[] byteDsColSaleDetails;
            S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
            byteDsColSaleDetails= ClsPubSerialize.Serialize(objCollateralSale.FunPubGetColSaleDetails(ColSaleId, CompanyID), SerializationMode.Binary);
            return byteDsColSaleDetails;
             }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        
        //Added by saranya
        #region Collateral Release
        public int FunPubInsertCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, out string CollateralReleaseNo)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
                return objCollateralSale.FunPubInsertCollateralRelease(byteobjCollateralRelease, SerMode, out CollateralReleaseNo);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public int FunPubUpdateCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, int ColReleaseID)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralSale = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
                return objCollateralSale.FunPubUpdateCollateralRelease(byteobjCollateralRelease, SerMode, ColReleaseID);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetColReleaseDetails(int ColReleaseId, int CompanyID)
        {
            try
            {
                byte[] byteDsColReleaseDetails;
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale objCollateralRelease = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSale();
                byteDsColReleaseDetails = ClsPubSerialize.Serialize(objCollateralRelease.FunPubGetColReleaseDetails(ColReleaseId, CompanyID), SerializationMode.Binary);
                return byteDsColReleaseDetails;
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        //End


        #region Collateral Sale Approval
        public int FunPubCreateCollateralSaleApproval(SerializationMode SerMode, byte[] byteobjS3G_CLT_ApprovalDataTable)
        {
            try
            {
                S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSaleApproval objCollateralSaleApproval = new S3GDALayer.Collateral.CollateralMgtServices.ClsPubCollateralSaleApproval();
                return objCollateralSaleApproval.FunPubCreateCollateralSaleApproval(SerMode, byteobjS3G_CLT_ApprovalDataTable);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region TA Collateral Capture

        public int FunPubTACreateCollateralCapture(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollCaptureDataTable, out string strCollCap_No)
        {
            try
            {
                S3GDALayer.TradeAdvance.CollateralMgtServices.ClsPubCollateralCapture ObjCollateralCapture = new S3GDALayer.TradeAdvance.CollateralMgtServices.ClsPubCollateralCapture();
                return ObjCollateralCapture.FunPubCreateCollateralCapture(SerMode, byteobjS3G_CLT_CollCaptureDataTable, out strCollCap_No);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunTAGetCollateralDetailsID(int intCompany, int intLiquidityType, int intCollLevelCode)
        {
            DataTable dtCollDetails = new DataTable();
            S3GDALayer.TradeAdvance.CollateralMgtServices.ClsPubCollateralCapture objClsCollCapture = new S3GDALayer.TradeAdvance.CollateralMgtServices.ClsPubCollateralCapture();
            dtCollDetails = objClsCollCapture.FunGetCollateralDetailsID(intCompany, intLiquidityType, intCollLevelCode);
            bytesDataTable = ClsPubSerialize.Serialize(dtCollDetails, SerializationMode.Binary);
            return bytesDataTable;
        }

        # endregion


    }
}
