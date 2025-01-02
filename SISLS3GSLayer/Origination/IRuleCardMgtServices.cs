using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IRuleCardMgtServices" here, you must also update the reference to "IRuleCardMgtServices" in Web.config.
    [ServiceContract]
    public interface IRuleCardMgtServices
    {
        #region Authorization Rule Card
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTransactionTypeMaster(SerializationMode SerMode, byte[] bytesObjTransactionTypeMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryConstitutionMaster(SerializationMode SerMode, byte[] bytesObjConstitutionMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY);


        #endregion

        #region Authorization ROIRules

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateROIRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyROIRulesInt(int intROIRulesID, bool blnIsActive, int intUserID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryROIRules(int intROIRulesID);

        #endregion

        #region Authorization PaymentRuleCard

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePaymentRuleCardInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyPaymentRuleCardInt(int intPayment_RuleCard_ID, bool blnIs_Active, int intUserID);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPaymentRuleCard(int intPayment_RuleCard_ID);
        #endregion
    }
}
