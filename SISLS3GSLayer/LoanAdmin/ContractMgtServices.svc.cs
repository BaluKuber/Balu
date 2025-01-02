using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Data;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.LoanAdmin
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ContractMgtServices" here, you must also update the reference to "ContractMgtServices" in Web.config.
    public class ContractMgtServices : IContractMgtServices
    {

        byte[] bytesDataTable;


        #region Specification Revision

        #region Create
        public int FunPubCreateRevisionSpecification(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_RevisionSpecificationDataTable, ClsSystemJournal ObjSysJournal, out string strRevisionNumber)
        {
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision ObjRevisionSpecification = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision();
                return ObjRevisionSpecification.FunPubCreateOrModifySpecificationRevision(SerMode, bytesObjS3G_LOANAD_RevisionSpecificationDataTable, ObjSysJournal, out strRevisionNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Specification Revision Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateOrModifySpecificationArchive(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable)
        {
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision ObjRevisionSpecification = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision();
                return ObjRevisionSpecification.FunPubCreateOrModifySpecificationArchive(SerMode, bytesObjRepayDetailsIRRArchiveHeader_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Specification Revision Archive:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubCreateSpecificationRevisionMasterDetails(SerializationMode SerMode, byte[] bytesObjRepayDetailsIRRArchiveHeader_DataTable, out string RevisionDocumentNumber)
        {
            RevisionDocumentNumber = string.Empty;
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision ObjRevisionSpecification = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubSpecificationRevision();
                return ObjRevisionSpecification.FunPubCreateSpecificationRevisionMasterDetails(SerMode, bytesObjRepayDetailsIRRArchiveHeader_DataTable, out RevisionDocumentNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Specification Revision Archive:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #endregion




        #region Bulk Revision

        #region Create
        public int FunPubCreateBulkRevisionDetails(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_BulkRevisionDataTable, out string strBulkRevNo)
        {
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubBulkRevision ObjBulkRevision = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubBulkRevision();
                return ObjBulkRevision.FunPubCreateBulkRevisionDetails(SerMode, bytesObjS3G_LOANAD_BulkRevisionDataTable, out strBulkRevNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Bulk Revision Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #endregion


        #region IContractMgtServices AccountActivation
        /// <summary>
        /// Insert Account Activation Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjAccountActivationDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateAccountActivation(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation ObjAccountActivation = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation();
                return ObjAccountActivation.FunPubAccountActivationInt(SerMode, bytesObjAccountActivationDataTable_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Account Activation Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateBulkActivation(SerializationMode SerMode, byte[] bytesObjAccountActivationDataTable_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation ObjAccountActivation = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation();
                return ObjAccountActivation.FunPubBulkActivationInt(SerMode, bytesObjAccountActivationDataTable_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Account Activation Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Getting Verified asset details 
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>

        public byte[] FunActiavtedAccountForModification(string strAccountNo)
        {
            DataTable dtAccount = new DataTable();
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation objClsPubAssetVerification = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountActivation();
            dtAccount = objClsPubAssetVerification.FunActiavtedAccountForModification(strAccountNo);
            bytesDataTable = ClsPubSerialize.Serialize(dtAccount, SerializationMode.Binary);
            return bytesDataTable;
        }

        #endregion


        #region "Account Closure/PreMature Closure"
        /// <summary>
        /// To get Account details for PANum, SANum
        /// </summary>
        /// <param name="SMode"></param>
        /// <param name="byteEnquiryService"></param>

        public byte[] FunGetAccountDetailsForClosure(string strPANum, string strSANum)
        {
            DataSet dtSet = new DataSet();
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure objAccount = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure();
            dtSet = objAccount.FunGetAccountDetailsForClosure(strPANum, strSANum);
            bytesDataTable = ClsPubSerialize.Serialize(dtSet, SerializationMode.Binary);
            return bytesDataTable;
        }

        public int FunPubCreateAccountClosure(SerializationMode SerMode, byte[] bytesObjAccountClosureDataTable_SERLAY, int intClosureType, out string strAccClosureNo, out int intClosureDetailId)
        {
            try
            {
                if (intClosureType == 0)
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure ObjAccountClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure();
                    return ObjAccountClosure.FunPubCreateAccountClosure(SerMode, bytesObjAccountClosureDataTable_SERLAY, out strAccClosureNo, out intClosureDetailId);
                }
                else
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure ObjPreMatureClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure();
                    return ObjPreMatureClosure.FunPubCreateAccountClosure(SerMode, bytesObjAccountClosureDataTable_SERLAY, out strAccClosureNo,out intClosureDetailId);
                }
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Account Closure Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateAccountClosureDetails(SerializationMode SerMode, byte[] bytesObjAccountClosureDataTable_SERLAY, int intClosureType)
        {
            try
            {
                if (intClosureType == 0)
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure ObjAccountClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure();
                    return ObjAccountClosure.FunPubCreateAccountClosureDetails(SerMode, bytesObjAccountClosureDataTable_SERLAY);
                }
                else
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure ObjPreMatureClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure();
                    return ObjPreMatureClosure.FunPubCreateAccountClosureDetails(SerMode, bytesObjAccountClosureDataTable_SERLAY);
                }
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Account Closure Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunGetAccountsClosedForModify(string strAccClosureNo, string strPANum, string strSANum, int intClosureDetailId, int intCompanyID)
        {
            DataTable dtSet = new DataTable();
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure objAccount = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure();
            dtSet = objAccount.FunGetAccountsClosedForModify(strAccClosureNo, strPANum, strSANum, intClosureDetailId, intCompanyID);
            bytesDataTable = ClsPubSerialize.Serialize(dtSet, SerializationMode.Binary);
            return bytesDataTable;
        }

        public int FunPubCancelAccountClosure(SerializationMode SerMode, byte[] bytesObjCancelClosureDataTable_SERLAY, int intClosureType)
        {
            try
            {
                if (intClosureType == 0)
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure ObjAccountClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountClosure();
                    return ObjAccountClosure.FunPubCancelAccountClosure(SerMode, bytesObjCancelClosureDataTable_SERLAY);
                }
                else
                {
                    S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure ObjAccountClosure = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubPreMatureClosure();
                    return ObjAccountClosure.FunPubCancelAccountClosure(SerMode, bytesObjCancelClosureDataTable_SERLAY);
                }

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Account Closure Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region Account Creation
        public byte[] FunPubGetApplicationProcessDetails(int intApplicationProcessId)
        {
            DataSet ds;
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            ds = objAccountCreationClient.FunPubGetApplicationProcessDetails(intApplicationProcessId);
            bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }

        public int FunPubInsertAccountCreationInt(AccountCreationEntity objAccountCreationEntity, out string strPASANo)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            return objAccountCreationClient.FunPubInsertAccountCreationInt(objAccountCreationEntity, out strPASANo);
        }

        public int FunPubUpdateAccountStatus(AccountCreationEntity objAccountCreationEntity)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            return objAccountCreationClient.FunPubUpdateAccountStatus(objAccountCreationEntity);
        }

        public byte[] FunPubGetMLASLAApplicable(int intLobId, int intCompanyId)
        {
            DataSet ds;
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            ds = objAccountCreationClient.FunPubGetMLASLAApplicable(intLobId, intCompanyId);
            bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            return bytesDataTable;
        }

        public int FunPubModifyAccountCreationInt(AccountCreationEntity objAccountCreationEntity)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            return objAccountCreationClient.FunPubModifyAccountCreationInt(objAccountCreationEntity);
        }

        public int FunPubMoveTempInvoices(AccountCreationEntity objAccountCreationEntity)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation objAccountCreationClient = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountCreation();
            return objAccountCreationClient.FunPubMoveTempInvoices(objAccountCreationEntity);
        }

        #endregion

        #region Account Consolidation


        public int FunPubCreateAccountConsolidation(SerializationMode SerMode, byte[] bytesObjAccountConsolidationDataTable, out string strAcConNo)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountConsolidation objAccountConsolidation = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountConsolidation();
            return objAccountConsolidation.FunPubAccountConsolidationInt(SerMode, bytesObjAccountConsolidationDataTable, out strAcConNo);
        }

        #endregion

        #region Account Split


        public int FunPubCreateAccountSplit(SerializationMode SerMode, byte[] bytesObjAccountSplitDataTable, out string strSplitNum)
        {
            S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountSplit objAccountSplit = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubAccountSplit();
            return objAccountSplit.FunPubAccountSplitInt(SerMode, bytesObjAccountSplitDataTable, out strSplitNum);
        }

        #endregion

        #region "Cash Flow Print"

        #region Create
        public int FunPubGenerateCashFlowPrint(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjCashFlow_DataTable, out Int64 intCashFlw_No, out string strErrorMsg)
        {
            try
            {

                intCashFlw_No = 0;
                S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubCashFlowPrint ObjCshFlw = new S3GDALayer.LoanAdmin.ContractMgtServices.ClsPubCashFlowPrint();
                return ObjCshFlw.FunPubGenerateCashFlowPrint(SerMode, bytesObjCashFlow_DataTable, out intCashFlw_No, out strErrorMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion



        #endregion
    }
}
