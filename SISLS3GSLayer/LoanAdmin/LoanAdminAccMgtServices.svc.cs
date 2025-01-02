using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Data;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.LoanAdmin
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class LoanAdminAccMgtServices : ILoanAdminAccMgtServices
    {
        int intResult;
        byte[] bytesDataTable;

        #region Payment Request

        #region Create
        public int FunPubCreateOrModifyPaymentRequest(SerializationMode SerMode, byte[] bytesObjPaymentRequestDatatable_SERLAY, out string paynum, out int request_No)
        {

            try
            {
                paynum = "";
                request_No = 0;
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubCreateOrModifyPaymentRequest(SerMode, bytesObjPaymentRequestDatatable_SERLAY, out paynum, out request_No);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubCreateOrModifyPaymentRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetailsDatatable_SERLAY)
        {

            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubCreateOrModifyPaymentRequestDetails(SerMode, bytesObjPaymentRequestDetailsDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Query
        public byte[] FunPubQueryGetCreditParameterRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails)
        {

            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable dsPaymentRequestDetails = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable();
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequestDetails = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                dsPaymentRequestDetails = ObjPaymentRequestDetails.FunPubQueryGetCreditParameterRequestDetails(SerMode, bytesObjPaymentRequestDetails);
                bytesDataTable = ClsPubSerialize.Serialize(dsPaymentRequestDetails, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting Payment Request Details:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPaymentRequestDetails.Dispose();
                dsPaymentRequestDetails = null;
            }
            return bytesDataTable;
        }
        #endregion
        #region "Instrument"
        public int FunPubInsPaymentRequestInstrument(SerializationMode SerMode, byte[] bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY)
        {

            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubInsPaymentRequestInstrument(SerMode, bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        # endregion


        #endregion

        #region LoanAdminAccMgtServices Month Closure
        /// <summary>
        /// Insert Month Closure Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjMonthClosureDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateMonthClosure(SerializationMode SerMode, byte[] bytesObjMonthClosureDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubMonthClosure ObjMonthClosure = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubMonthClosure();
                return ObjMonthClosure.FunPubMonthClosureInt(SerMode, bytesObjMonthClosureDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Month Closure Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region "Operating Lease Depreciation"

        /// <summary>
        /// To get Asset derpreciation details for branch, company
        /// </summary>
        /// <param name="SMode"></param>
        /// <param name="byteEnquiryService"></param>

        public byte[] FunGetAssetDepreciationDetail(int BranchID, int CompanyID)
        {
            DataTable dtTable = new DataTable();
            S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation objClsPubOperatingLeaseDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
            dtTable = objClsPubOperatingLeaseDepreciation.FunGetAssetDepreciationDetail(BranchID, CompanyID);
            bytesDataTable = ClsPubSerialize.Serialize(dtTable, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// To get Operational Lease LOB and related Branch
        /// </summary>
        /// <param name="SMode"></param>
        /// <param name="byteEnquiryService"></param>

        public byte[] FunGetOperatingLeaseLOBBranch(int intCompanyID, int intUserID)
        {
            DataTable dtTable = new DataTable();
            S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation objClsPubOperatingLeaseDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
            dtTable = objClsPubOperatingLeaseDepreciation.FunGetOperatingLeaseLOBBranch(intCompanyID, intUserID);
            bytesDataTable = ClsPubSerialize.Serialize(dtTable, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// Insert Depreciation Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDepreciationDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateAssetDepreciation(SerializationMode SerMode, byte[] bytesObjDepreciationDataTable_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation ObjDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
                return ObjDepreciation.FunPubCreateAssetDepreciation(SerMode, bytesObjDepreciationDataTable_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in inserting Depreciation Details:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// Insert Depreciation Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDepreciationDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubDeleteAssetDepreciation(SerializationMode SerMode, byte[] bytesObjDepreciationDataTable_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation ObjDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
                return ObjDepreciation.FunPubDeleteAssetDepreciation(SerMode, bytesObjDepreciationDataTable_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Revoking Depreciation Details:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// To get SJV Number related Depreciation details
        /// </summary>
        /// <param name="SMode"></param>
        /// <param name="byteEnquiryService"></param>

        public byte[] FunGetOperatingDepreciationDetails(string intSJVNumber, int CompanyId)
        {
            DataSet dtTable = new DataSet();
            S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation objClsPubOperatingLeaseDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
            dtTable = objClsPubOperatingLeaseDepreciation.FunGetOperatingDepreciationDetails(intSJVNumber, CompanyId);
            bytesDataTable = ClsPubSerialize.Serialize(dtTable, SerializationMode.Binary);
            return bytesDataTable;
        }

        /// <summary>
        /// Delete Depreciation Details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDepreciationDataTable_SERLAY"></param>
        /// <returns></returns>
        //public int FunPubDeleteAssetDepreciation(int intSJVNumber, int intCompany_ID)
        //{
        //    try
        //    {
        //        S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation ObjDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
        //        return ObjDepreciation.FunPubDeleteAssetDepreciation(intSJVNumber, intCompany_ID);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Error in inserting Depreciation Details:" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //}

        public int FunPubGetGlobalParameterVale(int companyID)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation ObjDepreciation = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseDepreciation();
                return ObjDepreciation.FunGetDenominationDays(companyID);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = "Error in creating CFMBooking : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
        }


        #endregion

        #region OperatingLeaseExpenses
        // ---- Operating Lease Expenses (Created By- Irsathameen K)-------
        //Begin
        public int FunPubCreateOperatingLeaseExpensesDetails(SerializationMode SerMode, byte[] bytesObjOperatingLeaseExpensesDatatable_SERLAY, out string strOLENo, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseExpenses ObjOperatingLeaseExpenses = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubOperatingLeaseExpenses();
                return ObjOperatingLeaseExpenses.FunPubCreateOperatingLeaseExpensesDetails(SerMode, bytesObjOperatingLeaseExpensesDatatable_SERLAY, out strOLENo, out strErrMsg);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        //End
        #endregion

        #region Manual Journal

        public int FunPubCreateManualJournal(SerializationMode SerMode, byte[] bytesObjManualJournalDatatable_SERLAY, out string strMJVNumber)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubManualJournal ObjManualJournal = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubManualJournal();
                return ObjManualJournal.FunPubCreateManualJournal(SerMode, bytesObjManualJournalDatatable_SERLAY, out strMJVNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region ODI Calculations
        public int FunPubCreateODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations ObjODICalculations = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations();
                return ObjODICalculations.FunPubCreateODICalculations(SerMode, bytesObjODICalculationsDataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable)
        {
            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_ODICalculationsDataTable dtODICalculationsDataTable = null;
            S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations ObjODICalculations = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations();
            dtODICalculationsDataTable = ObjODICalculations.FunPubQueryODICalculations(SerMode, bytesObjODICalculationsDataTable);
            bytesDataTable = ClsPubSerialize.Serialize(dtODICalculationsDataTable, SerMode);
            return bytesDataTable;
        }

        //public int FunPubSaveODICalculations(Dictionary<string,string> objDictionary,out string strErrorLog)
        //source modified by Tamiselvan.S on 27/01/2011
        public int FunPubSaveODICalculations(SerializationMode SerMode, byte[] bytesObjODICalculationsDataTable, out string strErrorLog)
        {
            int intResult = 0;
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations ObjODICalculations = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations();
                intResult = ObjODICalculations.FunPubSaveODI(SerMode, bytesObjODICalculationsDataTable, out strErrorLog);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubSaveODISchedule(SerializationMode SerMode, byte[] bytesDictionary)
        {
            int intResult = 0;
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations ObjODICalculations = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations();
                intResult = ObjODICalculations.FunPubSaveODISchedule(SerMode, bytesDictionary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
            return intResult;
        }

        public string FunPubRevokeODI(int intCompanyId, string ODIId, int intUserId)
        {
            string intResult = "0";
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations ObjODICalculations = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubODICalculations();
                intResult = ObjODICalculations.FunPubRevokeODI(intCompanyId, ODIId, intUserId);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion

        #region Cash flow Monthly Booking


        public int FunPubCreateCFMBkInt(SerializationMode SerMode, byte[] bytesObjCashflowMntlyBkDataTable, out string strCFMNumber)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubCashflowMntlyBk ObjCashflowMntlyBk = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubCashflowMntlyBk();
                return ObjCashflowMntlyBk.FunPubCreateCFMBkInt(SerMode, bytesObjCashflowMntlyBkDataTable, out strCFMNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = "Error in creating CFMBooking : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
        }

        public int FunPubRevokeCFMBkInt(string strCFMNumber, int intCompany_ID)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubCashflowMntlyBk ObjCashflowMntlyBk = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubCashflowMntlyBk();
                return ObjCashflowMntlyBk.FunPubRevokeCFMBkInt(strCFMNumber, intCompany_ID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException ObjFault = new ClsPubFaultException();
                ObjFault.ProReasonRW = "Error in creating CFMBooking : " + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(ObjFault, new FaultReason(ObjFault.ProReasonRW));
            }
        }

        #endregion

        #region IncomeRecognition

        public int FunPubCreateIncomeRecognition(SerializationMode SerMode, byte[] bytesObjS3G_CLN_IncomeRecog_DataTable)
        {
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition objClsIncomeRecog = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition();
                return objClsIncomeRecog.FunPubCreateIncomeRecognition(SerMode, bytesObjS3G_CLN_IncomeRecog_DataTable);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        public byte[] FunPubGetIncomeRecognition(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg)
        {
            byte[] bytesDataSet;
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition objClsIncomeRecog = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition();
                DataSet ObjDataset = objClsIncomeRecog.FunPubGetIncomeRecognition(strProcName, dctProcParams, out intErrCode, out strErrMsg);
                bytesDataSet = ClsPubSerialize.Serialize(ObjDataset, SerializationMode.Binary);
                ObjDataset.Dispose();
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataSet;
        }

        public byte[] FunPubGetIncomeRecognitionToSchedule(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg)
        {
            byte[] bytesDataSet;
            try
            {
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition objClsIncomeRecog = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubIncomeRecognition();
                DataSet ObjDataset = objClsIncomeRecog.FunPubGetIncomeRecognitionToSchedule(strProcName, dctProcParams, out intErrCode, out strErrMsg);
                bytesDataSet = ClsPubSerialize.Serialize(ObjDataset, SerializationMode.Binary);
                ObjDataset.Dispose();
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataSet;
        }

        #endregion

        #region TA Payment Request

        #region Create
        public int FunPubTACreateOrModifyPaymentRequest(SerializationMode SerMode, byte[] bytesObjPaymentRequestDatatable_SERLAY, out string paynum, out int request_No)
        {

            try
            {
                paynum = "";
                request_No = 0;
                S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubCreateOrModifyPaymentRequest(SerMode, bytesObjPaymentRequestDatatable_SERLAY, out paynum, out request_No);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubTACreateOrModifyPaymentRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetailsDatatable_SERLAY)
        {

            try
            {
                S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubCreateOrModifyPaymentRequestDetails(SerMode, bytesObjPaymentRequestDetailsDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Query
        public byte[] FunPubTAQueryGetCreditParameterRequestDetails(SerializationMode SerMode, byte[] bytesObjPaymentRequestDetails)
        {

            S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable dsPaymentRequestDetails = new S3GBusEntity.LoanAdmin.LoanAdminAccMgtServices.S3G_LOANAD_GetPaymentDetailsDataTable();
            try
            {
                S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequestDetails = new S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                dsPaymentRequestDetails = ObjPaymentRequestDetails.FunPubQueryGetCreditParameterRequestDetails(SerMode, bytesObjPaymentRequestDetails);
                bytesDataTable = ClsPubSerialize.Serialize(dsPaymentRequestDetails, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting Payment Request Details:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                dsPaymentRequestDetails.Dispose();
                dsPaymentRequestDetails = null;
            }
            return bytesDataTable;
        }
        #endregion
        #region "Instrument"
        public int FunPubTAInsPaymentRequestInstrument(SerializationMode SerMode, byte[] bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY)
        {

            try
            {
                S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest ObjPaymentRequest = new S3GDALayer.TradeAdvance.LoanAdminAccMgtServices.ClsPubPaymentRequest();
                return ObjPaymentRequest.FunPubInsPaymentRequestInstrument(SerMode, bytesObjPaymentRequestInstrumentDetails_DataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        # endregion


        #endregion

        #region DEBIT CREDIT NOTE

        public int FunPubInsertDebitCredit(SerializationMode SerMode, byte[] bytesObjS3G_LOANAD_CRDRDataTable_SERLAY, out string strMJVNumber)
        {
            try
            {
                strMJVNumber = "";
                S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubDebitCreditNote ObjDebitCreditNote = new S3GDALayer.LoanAdmin.LoanAdminAccMgtServices.ClsPubDebitCreditNote();
                intResult = ObjDebitCreditNote.FunPubInsertDebitCredit(SerMode, bytesObjS3G_LOANAD_CRDRDataTable_SERLAY, out strMJVNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion

        #region Create
        public int FunPubCreateOrModifyInterim(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjInterim_DataTable, out Int32 intInterim, out string strErrorMsg, out string StrInterimNumber)
        {
            try
            {

                intInterim = 0;
                S3GDALayer.LoanAdmin.ClsPubInterim ObjNote = new S3GDALayer.LoanAdmin.ClsPubInterim();
                return ObjNote.FunPubCreateOrModifyInterim(SerMode, bytesObjInterim_DataTable, out intInterim, out strErrorMsg, out StrInterimNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Stock_Transfer
        public int FunPubCreateStockMaster(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjStockMasterDetails_DataTable, out Int32 intStock, out string strErrorMsg, out string StrDocumentNumber)
        {
            try
            {

                intStock = 0;
                S3GDALayer.LoanAdmin.ClsPubStockTransfer ObjStock = new S3GDALayer.LoanAdmin.ClsPubStockTransfer();
                return ObjStock.FunPubCreateStockMaster(SerMode, bytesObjStockMasterDetails_DataTable, out intStock, out strErrorMsg, out StrDocumentNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion
    }
}
