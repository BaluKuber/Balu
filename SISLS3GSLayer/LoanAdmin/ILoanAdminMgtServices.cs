#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: LoanAdmin
/// Screen Name			: InvoiceVendor Interface Class
/// Created By			: Kaliraj K
/// Created Date		: 17-Jul-2010
/// Purpose	            : WCF Interface class for defining Invoice vendor details methods

/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.LoanAdmin
{
    // NOTE: If you change the interface name "ILoanAdminMgtServices" here, you must also update the reference to "ILoanAdminMgtServices" in Web.config.
    [ServiceContract]
    public interface ILoanAdminMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateInvoiceDetails(SerializationMode SerMode, byte[] bytesObjInvoiceDatatable_SERLAY, ClsSystemJournal ObjSysJournal, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDeliveryIns(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY, out string strDeliveryNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunCancelDeliveryIns(int DeliveryInstruction_ID, string Flag, out int ErrorCode);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssetDetails(SerializationMode SerMode, byte[] bytesObjAssetDetailsDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustDetails(SerializationMode SerMode, byte[] bytesObjCustDetailsDataTable_SERLAY);


        //--------------Factoring Invoice Loading( Created By -Irsathameen K)---------------
        //Begin

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFactoringInvoiceDetails(SerializationMode SerMode, byte[] bytesObjFactoringInvoiceDatatable_SERLAY, out string strFILNo, out string strInvoiceNo, out string strPartyName);


        //--------------NOC Termination( Created By -Irsathameen K)---------------
        //Begin
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateNOCTerminationDetails(SerializationMode SerMode, byte[] bytesObjNOCTerminationDatatable_SERLAY, out string strNOCNo);

        //End
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGlobalParameterDetails(SerializationMode SerMode, byte[] bytesObjGlobalParameterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLOBDetails(SerializationMode SerMode, byte[] bytesObjLOBDetailsDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGPSDetails(SerializationMode SerMode, byte[] bytesObjGPSDetailsDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyGPSDetails(SerializationMode SerMode, byte[] bytesObjGPSDetailsDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTLEWCIns(SerializationMode SerMode, byte[] bytesObjTLEWCDatatable_SERLAY, out string strTLEWCNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyTLEWC(SerializationMode SerMode, byte[] bytesObjTLEWCDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateWCIns(SerializationMode SerMode, byte[] bytesObjTLEWCDatatable_SERLAY, out string strTLEWCNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyWC(SerializationMode SerMode, byte[] bytesObjTLEWCDatatable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelTopUpTLEWC(int intTLEWCID, int intCompanyId, int intUserId);  //Canceling Topup

        #region Billing
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBillingInt(BillingEntity objBillingEntity, out string strJournalMessage);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBillEmailStatus(BillingEntity objBillingEntity, out string strJournalMessage);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubGetPDF(BillingEntity objBillingEntity);

        //Added by Suresh
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelInvoice(BillingEntity objBillingEntity, out string strInvoiceNo);

        #endregion

        #region Billing
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCCInt(BillingEntity objBillingEntity, out string strJournalMessage);
        #endregion

        #region TA Delivery Instruction

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateDeliveryIns(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY, out string strDeliveryNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunTACancelDeliveryIns(int DeliveryInstruction_ID, string Flag, out int ErrorCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAGetAssetDetails(SerializationMode SerMode, byte[] bytesObjAssetDetailsDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAGetCustDetails(SerializationMode SerMode, byte[] bytesObjCustDetailsDataTable_SERLAY);


        #endregion

        #region TA Income Process
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateIncomeCalculation(BillingEntity objBillingEntity, out string strJournalMessage);
        #endregion

        #region Factoring Retirement

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFactoringRetirement(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_FT_RetirementDataTable, out string strFILNo);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdatePO(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSavePO(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY, out string strPO_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelPO(int intPO_Hdr_ID, int intVendorGroup, string strReason_For_Cancellation, out int ErrorCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancel_MPO(int intPO_Hdr_ID, int intVendorGroup, string strReason_For_Cancellation, out int ErrorCode);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubSavetPO_EMails(string strPODetails, out int ErrorCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelMultiplePO(string strPONumber, int intOption, string strReason_For_Cancellation, out int ErrorCode, out string strPONumberOut);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdatePI(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSavePI(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY, out string strLSQ_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelPI(int intPI_Hdr_ID, string PI_No, string PO_No, out int ErrorCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateVI(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSaveVI(SerializationMode SerMode, byte[] bytesObjDeliveryDatatable_SERLAY, out string strLSQ_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelVI(int intVI_Hdr_ID, string VI_No, string PO_No, out int ErrorCode);

        #region RS CHARGE MAINTENANCE

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRSChargeMaint(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable, out string RSCM_Code);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRSChargeMaintenance(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRSChargeMaintenancePaging(SerializationMode SerMode, byte[] bytesObjRS_Charge_MgmtDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        #endregion

        #region Way Bill Tracking

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateWayBillTracking(SerializationMode SerMode, byte[] bytesObjWayBillTrackingDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSaveWayBillTracking(SerializationMode SerMode, byte[] bytesObjWayBillTrackingDataTable_SERLAY);

        #endregion

        #region Asset Serial No Entry
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateAssetSerialNo(SerializationMode SerMode, byte[] bytesObjAssetSerial_SERLAY);

        #endregion

         #region VendorC
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateVendorC(SerializationMode SerMode, byte[] bytesObjVendorC_SERLAY);

        #endregion
        
    }
}
