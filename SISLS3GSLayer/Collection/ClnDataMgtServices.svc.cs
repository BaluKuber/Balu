using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;
using S3GDALayer.Collection;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Collection
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ClnDataMgtServices" here, you must also update the reference to "ClnDataMgtServices" in Web.config.
    public class ClnDataMgtServices : IClnDataMgtServices
    {
        byte[] bytesDataTable;
        int intResult;

        #region "Follow Up Process"
        /// <summary>
        /// Inserting records in Follow Up details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateFollowUpProcess(SerializationMode SMode, byte[] byteFollowUpService, out int intFollowUpID, out string strDocNo)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateFollowUp(SMode, byteFollowUpService, out intFollowUpID, out strDocNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Follow Up Process Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region "CRM"
        /// <summary>
        /// Inserting records in Follow Up details table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateCRM(SerializationMode SMode, byte[] byteCRMService, out int intCRMID, out string strDocNo, out int intCustomerID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateCRM(SMode, byteCRMService, out intCRMID, out strDocNo, out intCustomerID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Follow Up Process Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region "Customer"
        /// <summary>
        /// Inserting records in Customer table
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public int FunPubCreateProspectCust(SerializationMode SMode, byte[] byteCRMService, out string strDocNo, out int intCustomerID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateProspectCust(SMode, byteCRMService, out strDocNo, out intCustomerID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Follow Up Process Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion


        #region "OPC CRM"

        #region"Create/Update Prospect"

        public int FunPubCreateProspect(SerializationMode SerMode, byte[] bytesPspct_Data, out Int64 intCRMID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateProspect(SerMode, bytesPspct_Data, out intCRMID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Prospect Creation/Updation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region"Create/Update Lead"

        public int FunPubCreateLead(SerializationMode SerMode, byte[] bytesObjS3G_Colection_Customer_DataTable, out string strDocNo, out Int64 intLeadID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateLead(SerMode, bytesObjS3G_Colection_Customer_DataTable, out strDocNo, out intLeadID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Prospect Creation/Updation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region"Create Track"

        public int FunPubCreateTrackDetails(SerializationMode SerMode, byte[] bytesTrack_Data, out Int64 intTrackID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateTrackDetails(SerMode, bytesTrack_Data, out intTrackID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Prospect Creation/Updation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region"Create Document"

        public int FunPubCreateDocumentDetails(SerializationMode SerMode, byte[] bytesDoc_Data, out Int64 intDocID)
        {
            try
            {
                S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess objClsPubFollowUpProcess = new S3GDALayer.Collection.ClnDataMgtServices.ClsPubFollowUpProcess();
                return objClsPubFollowUpProcess.FunPubCreateDocumentDetails(SerMode, bytesDoc_Data, out intDocID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Prospect Creation/Updation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #endregion
    }
}
