using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Origination
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ApplicationMgtServices" here, you must also update the reference to "ApplicationMgtServices" in Web.config.
    public class ApplicationMgtServices : IApplicationMgtServices
    {
        int intResult;
        #region IApplicationMgtServices Members

        public int FunPubCreateApplicationApproval(out int ApprovalStatus, SerializationMode SerMode, byte[] bytesObjApplicationApprovalDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval ObjApplicationApproval = new S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval();
                return ObjApplicationApproval.FunPubCreateApplicationApproval(out ApprovalStatus, SerMode, bytesObjApplicationApprovalDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        #endregion

        #region Proforma

        public int FunPubCreateProformaDetails(SerializationMode SerMode, byte[] bytesObjProformaDatatable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.ApplicationMgtServices.ClsPubProforma ObjProforma = new S3GDALayer.Origination.ApplicationMgtServices.ClsPubProforma();
                return ObjProforma.FunPubCreateProformaDetails(SerMode, bytesObjProformaDatatable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
            
    #endregion

        #region Application Processing

        S3GDALayer.Origination.ApplicationProcessDAL ObjApprocess = new S3GDALayer.Origination.ApplicationProcessDAL();

        public int FunPubCreateApplicationProcessInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out)
        {
            try
            {
                return ObjApprocess.FunPubCreateApplicationProcessInt(ObjApp, out strAppNumber_Out);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Creating Application:" + ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateApplicationProcessGoldLoanInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out)
        {
            try
            {
                return ObjApprocess.FunPubCreateApplicationProcessGoldLoanInt(ObjApp, out strAppNumber_Out);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Creating Application:" + ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubRevokeOrUpdateApprovedDetails(int intOptions, int intTask_ID, int intUser_ID,string strPassword)
        {
            try
            {
                S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval ObjApplicationApproval = new S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval();
                return ObjApplicationApproval.FunPubRevokeOrUpdateApprovedDetails(intOptions, intTask_ID, intUser_ID,strPassword);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Creating Application:" + ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubUpdateApplicationStatus(S3GBusEntity.ApplicationProcess.ApplicationProcess objApplicationProcessEntity)
        {
            return ObjApprocess.FunPubUpdateApplicationStatus(objApplicationProcessEntity);
        }

        public int FunPubRevokeOrUpdateApprovedDetailsPricing(int intOptions, string strIDs, int intUser_ID, string strPassword, out string outProposalNo)
        {
            try
            {
                S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval ObjApplicationApproval = new S3GDALayer.Origination.ApplicationMgtServices.ClsPubApplicationApproval();
                return ObjApplicationApproval.FunPubRevokeOrUpdateApprovedDetailsPricing(intOptions, strIDs, intUser_ID, strPassword, out outProposalNo);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Revoke Or Update Pricing:" + ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        public int FunPubCreateTallyIntegDetails(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.ApplicationMgtServices.ClsPubTallyIntegration ObjTallyProcessSave = new S3GDALayer.Origination.ApplicationMgtServices.ClsPubTallyIntegration();
                intResult = ObjTallyProcessSave.FunPubCreateTallyIntegDetails(SerMode, bytesObjROIRulesDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in TallyInteg Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        #region IApplicationMgtServices Members




        #endregion
    }
}
