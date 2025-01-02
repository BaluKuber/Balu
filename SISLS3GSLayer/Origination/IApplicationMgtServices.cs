using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IApplicationMgtServices" here, you must also update the reference to "IApplicationMgtServices" in Web.config.
    [ServiceContract]
    public interface IApplicationMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateApplicationApproval(out int ApprovalStatus, SerializationMode SerMode, byte[] bytesObjApplicationApprovalDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateProformaDetails(SerializationMode SerMode, byte[] bytesObjProformaDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeOrUpdateApprovedDetails(int intOptions, int intTask_ID, int intUser_ID, string strPassword);
               
        #region Application Process

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateApplicationProcessInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateApplicationProcessGoldLoanInt(S3GBusEntity.ApplicationProcess.ApplicationProcess ObjApp, out string strAppNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateApplicationStatus(S3GBusEntity.ApplicationProcess.ApplicationProcess objApplicationProcessEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeOrUpdateApprovedDetailsPricing(int intOptions, string strIDs, int intUser_ID, string strPassword, out string outProposalNo);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTallyIntegDetails(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY);
    }
}
