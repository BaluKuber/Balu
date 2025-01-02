using System.ServiceModel;
using S3GBusEntity;
using System.Data;
using System;

namespace S3GServiceLayer.LoanAdmin
{
    // NOTE: If you change the interface name "IContractMgtServices" here, you must also update the reference to "IContractMgtServices" in Web.config.
    [ServiceContract]
    public interface IContractMgtServices
    {

        #region Specification Revision

        #region Create
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRevisionSpecification(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_RevisionSpecificationDataTable, ClsSystemJournal ObjSysJournal, out string strRevisionNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifySpecificationArchive(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable);
        #endregion


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateSpecificationRevisionMasterDetails(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable, out string RevisionDocumentNumber);
        

        #endregion



        #region Bulk Revision

        #region Create
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBulkRevisionDetails(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_BulkRevisionDataTable,out string strBulkRevNo);
        #endregion

        #endregion

        #region Account Activation
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAccountActivation(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBulkActivation(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunActiavtedAccountForModification(string strAccountNo);

        #endregion

        #region "Account Closure"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAccountDetailsForClosure(string strPANum, string strSANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAccountClosure(SerializationMode SerMode, byte[] bytesObjAccountClosureDataTable_SERLAY, int intClosureType, out string strAccClosureNo, out int intClosureDetailId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAccountClosureDetails(SerializationMode SerMode, byte[] bytesObjAccountClosureDataTable_SERLAY, int intClosureType);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAccountsClosedForModify(string strAccClosureNo, string strPANum, string strSANum, int intClosureDetailId, int intCompanyID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCancelAccountClosure(SerializationMode SerMode, byte[] bytesObjCancelClosureDataTable_SERLAY, int intClosureType);


        #endregion

        #region Account Creation
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetApplicationProcessDetails(int intApplicationProcessId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertAccountCreationInt(AccountCreationEntity objAccountCreationEntity, out string strPASANo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyAccountCreationInt(AccountCreationEntity objAccountCreationEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateAccountStatus(AccountCreationEntity objAccountCreationEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMLASLAApplicable(int intLobId, int intCompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubMoveTempInvoices(AccountCreationEntity objAccountCreationEntity);
        #endregion

        #region Account Consolaidation
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAccountConsolidation(SerializationMode SerMode, byte[] bytesObjAccountConsolidationDataTable, out string strAcConNo);
        #endregion

        #region Account Split
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAccountSplit(SerializationMode SerMode, byte[] bytesObjAccountSplitDataTable, out string strSplitNum);
        #endregion

        #region Cash Flow Print
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubGenerateCashFlowPrint(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjTranche_DataTable, out Int64 intCashFlw_No, out string strErrorMsg);
        #endregion
    }
}
