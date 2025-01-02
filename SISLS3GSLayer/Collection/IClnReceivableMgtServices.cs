using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Collection
{
    // NOTE: If you change the interface name "IClnReceivableMgtServices" here, you must also update the reference to "IClnReceivableMgtServices" in Web.config.
    [ServiceContract]
    public interface IClnReceivableMgtServices
    {
        #region "Delinquent Parameters"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDelinquentParameters(SerializationMode SerMode, byte[] bytesObjDelinquentDataTable_SERLAY, out string DSNO);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyDelinquentParameters(SerializationMode SerMode, byte[] bytesObjS3G_CLN_DelinquencyTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeactivateDelinquencyType(SerializationMode SerMode, byte[] bytesObjDelinquentDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDelinquencySpooling(SerializationMode SerMode, byte[] bytesObjDelinquencySpoolingDataTable_SERLAY);

        

        #endregion

        #region "Demand Processing"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDemandProcessing(SerializationMode SerMode, byte[] bytesObjDemandProcessingDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyDemandProcessing(SerializationMode SerMode, byte[] bytesObjDemandProcessingDataTable_SERLAY);
        #endregion

        #region "Manual Bucket Classification"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateManualBucketClassifcation(SerializationMode SerMode, byte[] bytesManualBucketClassifcation_Datatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyManualBucketClassifcationcategory(SerializationMode SerMode, byte[] bytesManualBucketClassifcation_Datatable_SERLAY);
        
        #endregion

     

        #region "Bucket Parameter"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBucketParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_BucketParameterDataTable, out string strBuckNo);
        #endregion

        #region Loan To Value
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLoanToValue(SerializationMode SerMode, byte[] bytesLoanToValueDatatable_SERLAY);
        #endregion

        //#region IncomeRecognition

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubCreateIncomeRecognition(SerializationMode SerMode, byte[] bytesObjS3G_CLN_IncomeRecog_DataTable);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetIncomeRecognition(string strProcName, Dictionary<string, string> dctProcParams, out int intErrCode, out string strErrMsg);
        

        //#endregion


        #region Market Value Entry

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateMarketValueEntry(SerializationMode SerMode, byte[] bytesObjMarketEntryDAL_SERLAY, out string strMVENumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateMarketValueEntryGL(SerializationMode SerMode, byte[] bytesObjMarketEntryDAL_SERLAY, out string strMVENumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunMarketEntryForModification(string strMarketValueNo);

        #endregion
        

    }
}
