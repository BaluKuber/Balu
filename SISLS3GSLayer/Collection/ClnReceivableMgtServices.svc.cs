using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;
using S3GDALayer.Collection.ClnReceivableMgtServices;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Collection
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ClnReceivableMgtServices" here, you must also update the reference to "ClnReceivableMgtServices" in Web.config.
    public class ClnReceivableMgtServices : IClnReceivableMgtServices
    {
        #region "Delinquent Parameters"

        /// <summary>
        /// Inserting records in Delinquency tables
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateDelinquentParameters(SerializationMode SMode, byte[] byteDelinquency, out string DSNO)
        {
            try
            {

                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter objClsPubDelinquency = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter();
                return objClsPubDelinquency.FunPubCreateDelinquentParameters(SMode, byteDelinquency,out DSNO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int FunPubModifyDelinquentParameters(SerializationMode SMode, byte[] byteDelinquency)
        {
            try
            {

                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter objClsPubDelinquency = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter();
                return objClsPubDelinquency.FunPubModifyDelinquentParameters(SMode, byteDelinquency);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Deactivating  records in Delinquency tables
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubDeactivateDelinquencyType(SerializationMode SMode, byte[] byteDelinquency)
        {
            try
            {
                
                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter objClsPubDelinquency = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquentParameter();
                return objClsPubDelinquency.FunPubDeactivateDelinquencyType(SMode, byteDelinquency);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 
   
        #region "Manual Bucket Classification"

        public int FunPubCreateManualBucketClassifcation(SerializationMode SMode, byte[] byteBucketClassifcation)
        {
            try
            {

                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubManualBucketClassification objClsBucketClassifcation = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubManualBucketClassification();
                return objClsBucketClassifcation.FunPubCreateManualBucketClassifcation(SMode, byteBucketClassifcation);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunPubModifyManualBucketClassifcationcategory(SerializationMode SMode, byte[] byteBucketClassifcation)
        {
            try
            {

                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubManualBucketClassification objClsBucketClassifcation = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubManualBucketClassification();
                return objClsBucketClassifcation.FunPubModifyManualBucketClassifcationcategory(SMode, byteBucketClassifcation);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Bucket_Parameter
        // ----Bucket Parameter(Created By- Irsathameen K)-------
       // ----Created On 11-Oct-2010------
        //Begin
     
        public int FunPubCreateBucketParameterDetails(SerializationMode SerMode, byte[] bytesObjS3G_CLN_BucketParameterDataTable, out string strBuckNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubBucketParameter objClsPubBucketParameter = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubBucketParameter();
                return objClsPubBucketParameter.FunPubCreateBucketParameterDetails(SerMode, bytesObjS3G_CLN_BucketParameterDataTable, out strBuckNo);
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

        #region IClnReceivableMgtServices Members


        public int FunPubCreateDelinquencySpooling(SerializationMode SerMode, byte[] bytesObjDelinquencySpoolingDataTable_SERLAY)
        {
            S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquencySpooling objClsPubDelinquency = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubDelinquencySpooling();
            return objClsPubDelinquency.FunPubCreateDelinquencySpooling(SerMode, bytesObjDelinquencySpoolingDataTable_SERLAY);  
        }

        #endregion
        
        #region Loan To Value
        public int FunPubCreateLoanToValue(SerializationMode SerMode, byte[] bytesLoanToValueDatatable_SERLAY)
        {
            S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubLoanToValueSpooling objClsPubLoanToValue = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubLoanToValueSpooling();
            return objClsPubLoanToValue.FunPubCreateLoanToValue(SerMode, bytesLoanToValueDatatable_SERLAY);
        }
        #endregion

        //#region IncomeRecognition

        //public int FunPubCreateIncomeRecognition(SerializationMode SerMode, byte[] bytesObjS3G_CLN_IncomeRecog_DataTable)
        //{
        //    try
        //    {
        //        S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubIncomeRecognition objClsIncomeRecog = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubIncomeRecognition();
        //        return objClsIncomeRecog.FunPubCreateIncomeRecognition(SerMode, bytesObjS3G_CLN_IncomeRecog_DataTable);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }

        //}

        //public byte[] FunPubGetIncomeRecognition(string strProcName, Dictionary<string, string> dctProcParams,out int intErrCode,out string strErrMsg)
        //{
        //    byte[] bytesDataSet;
        //    try
        //    {
        //        S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubIncomeRecognition objClsIncomeRecog = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubIncomeRecognition();
        //        DataSet ObjDataset = objClsIncomeRecog.FunPubGetIncomeRecognition(strProcName, dctProcParams, out intErrCode, out strErrMsg);
        //        bytesDataSet = ClsPubSerialize.Serialize(ObjDataset, SerializationMode.Binary);
        //        ObjDataset.Dispose();
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //    return bytesDataSet;
        //}


        //#endregion

        #region "Demand Processing"

        public int FunPubCreateDemandProcessing(SerializationMode SerMode, byte[] bytesObjDemandProcessingDataTable_SERLAY)
        {
            try
            {
                ClsPubDemandProcessing objClsDemandProcessing = new ClsPubDemandProcessing();
                return objClsDemandProcessing.FunPubCreateDemandProcessing(SerMode, bytesObjDemandProcessingDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        public int FunPubModifyDemandProcessing(SerializationMode SerMode, byte[] bytesObjDemandProcessingDataTable_SERLAY)
        {
            try
            {
                ClsPubDemandProcessing objClsDemandProcessing = new ClsPubDemandProcessing();
                return objClsDemandProcessing.FunPubModifyDemandProcessing(SerMode, bytesObjDemandProcessingDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        #endregion

       

        #region "Market Value Entry"

        public int FunPubCreateMarketValueEntry(SerializationMode SerMode, byte[] bytesObjS3G_Colection_MarketEntry_DataTable, out string strMVENumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue objMarketValue = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue();
                return objMarketValue.FunPubCreateMarketValueEntry(SerMode, bytesObjS3G_Colection_MarketEntry_DataTable, out strMVENumber);

            }
            catch (Exception objMarketValue)
            {
              ClsPubFaultException objFault = new ClsPubFaultException();
              objFault.ProReasonRW = objMarketValue.Message.ToString();
              throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));  
            }
        }

        public DataTable FunMarketEntryForModification(string strMarketValueNo)
        {
            try
            {
                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue objMarketValueModification = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue();
                return objMarketValueModification.FunMarketEntryForModification(strMarketValueNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));  

            }
        }
    
        #endregion

        #region "Market Value Entry GL"

        public int FunPubCreateMarketValueEntryGL(SerializationMode SerMode, byte[] bytesObjS3G_Colection_MarketEntry_DataTable, out string strMVENumber)
        {
            try
            {
                S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue objMarketValue = new S3GDALayer.Collection.ClnReceivableMgtServices.ClsPubMarketEntryValue();
                return objMarketValue.FunPubCreateMarketValueEntryGL(SerMode, bytesObjS3G_Colection_MarketEntry_DataTable, out strMVENumber);

            }
            catch (Exception objMarketValue)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objMarketValue.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion
    }
}