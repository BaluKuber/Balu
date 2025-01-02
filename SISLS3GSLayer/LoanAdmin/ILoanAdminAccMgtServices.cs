using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S3GServiceLayer.LoanAdmin
{
    // NOTE: If you change the interface name "ILoanAdminAccMgtServices" here, you must also update the reference to "ILoanAdminAccMgtServices" in Web.config.
    
    [ServiceContract]
    
    public interface ILoanAdminAccMgtServices
    {
        #region Payment Request Table

        #region Create
        [OperationContract]
        
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyPaymentRequest(SerializationMode SerMode, byte[] bytesObjPaymentRequestDatatable_SERLAY, out string paynum, out int request_No);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyPaymentRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetailsDatatable_SERLAY);

        #endregion

        #region Query
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGetCreditParameterRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails);
        #endregion


        #region Instrument
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsPaymentRequestInstrument(SerializationMode SerMode, byte[] bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY);
        #endregion

        #endregion

        #region LoanAdminAccMgtServices Month Closure
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateMonthClosure(SerializationMode SerMode, byte[] bytesObjMonthClosureDataTable_SERLAY);
        #endregion

        #region "Operating Lease Depreciation"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAssetDepreciationDetail(int BranchID, int CompanyID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetOperatingLeaseLOBBranch(int intCompanyID, int intUserID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAssetDepreciation(SerializationMode SerMode, byte[] bytesObjDepreciationDataTable_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteAssetDepreciation(SerializationMode SerMode, byte[] bytesObjDepreciationDataTable_SERLAY, out string strErrMsg);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetOperatingDepreciationDetails(string intSJVNumber,int CompanyId);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubDeleteAssetDepreciation(int intSJVNumber, int intCompany_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubGetGlobalParameterVale(int intCompany_ID);

        #endregion

        #region OperatingLeaseExpenses

        //--------------Operating Lease Expenses( Created By -Irsathameen K)---------------
        //Begin

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOperatingLeaseExpensesDetails(SerializationMode SerMode, byte[] bytesObjOperatingLeaseExpensesDatatable_SERLAY, out string strOLENo, out string strErrMsg);

        //End
        #endregion

        #region Manual Journal

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateManualJournal(SerializationMode SerMode, byte[] bytesObjManualJournalDatatable_SERLAY,out string MJVNumber);
        #endregion

        #region ODI Calculations
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubSaveODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable, out string strErrorLog);

        //int FunPubSaveODICalculations(Dictionary<string, string> objDictionary, out string strErrorLog);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        //int FunPubSaveODISchedule(Dictionary<string, string> objDictionary);
        int FunPubSaveODISchedule(SerializationMode SerMode, byte[] bytesDictionary);

        

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunPubRevokeODI(int intCompanyId, string ODIId, int intUserId);
        

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable);

        #endregion

        #region Cash flow Monthly Booking
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCFMBkInt(SerializationMode SerMode, byte[] bytesObjCashflowMntlyBkDataTable, out string strCFMNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeCFMBkInt(string strCFMNumber, int intCompany_ID);

        #endregion

        #region IncomeRecognition

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateIncomeRecognition(SerializationMode SerMode, byte[] bytesObjS3G_CLN_IncomeRecog_DataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetIncomeRecognition(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetIncomeRecognitionToSchedule(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg);

        #endregion

        #region TA Payment Request Table

        #region Create
        [OperationContract]

        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateOrModifyPaymentRequest(SerializationMode SerMode, byte[] bytesObjPaymentRequestDatatable_SERLAY, out string paynum, out int request_No);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateOrModifyPaymentRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetailsDatatable_SERLAY);

        #endregion

        #region Query
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAQueryGetCreditParameterRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails);
        #endregion


        #region Instrument
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTAInsPaymentRequestInstrument(SerializationMode SerMode, byte[] bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY);
        #endregion

        #endregion

        #region DEBIT CREDIT NOTE

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertDebitCredit(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_DebitCreditNoteDataTable_SERLAY, out string strDCNCode);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyInterim(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjInterim_DataTable, out int intInterim, out string strErrorMsg, out string StrInterimNumber);

        #region Stock Transfer
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateStockMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjStockMasterDetails_DataTable, out int intStock, out string strErrorMsg, out string StrDocumentNumber);
        #endregion

    }
}
