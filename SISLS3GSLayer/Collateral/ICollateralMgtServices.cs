using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Collateral
{
    // NOTE: If you change the interface name "ICollateralMgtServices" here, you must also update the reference to "ICollateralMgtServices" in Web.config.
    [ServiceContract]
    public interface ICollateralMgtServices
    {
        #region Collateral Valuation

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]

        int FunPubCreateCollateralValuation(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollValuationDataTable, out string strCVal_No);

        #endregion
        #region Collateral Master

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]

        int FunPubCreateCollateralMaster(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollateralMasterDataTable, out string strCollateralRefNo);
    
        #endregion
        #region Collateral Capture
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCollateralCapture(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollCaptureDataTable, out string strCollCapNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetCollateralDetailsID(int intCompany, int intLiquidityType, int intCollLevelCode);

        #endregion
       

        #region Collateral Storage
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCollateralStorage(SerializationMode SerMode, byte[] byteobjCollateralStorage, out string StrCollateralStorage);
        #endregion

        #region Collateral Sale
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode, out string CollateralSaleNo);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateCollateralSale(byte[] byteobjCollateralSale, SerializationMode SerMode,int ColSaleID);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetColSaleDetails(int ColSaleId, int CompanyID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCollateralSaleApproval(SerializationMode SerMode, byte[] byteobjS3G_CLT_ApprovalDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateCollateralSaleInvoicedetails(byte[] byteobjCollateralSale, SerializationMode SerMode, int ColSaleID);
        #endregion

        //Added by saranya 
        #region Collateral Release
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, out string CollateralReleaseNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateCollateralRelease(byte[] byteobjCollateralRelease, SerializationMode SerMode, int ColReleaseID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetColReleaseDetails(int ColReleaseId, int CompanyID);
        #endregion
        //End

        #region Trade Advance Collatral Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTradeAdvanceCollatralMaster(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollateralMasterDataTable, out string strCollateralRefNo);
        
        #endregion

        #region TA Collateral Capture

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateCollateralCapture(SerializationMode SerMode, byte[] byteobjS3G_CLT_CollCaptureDataTable, out string strCollCapNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunTAGetCollateralDetailsID(int intCompany, int intLiquidityType, int intCollLevelCode);

        #endregion
    }
}
