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
    // NOTE: If you change the class name "RuleCardMgtServices" here, you must also update the reference to "RuleCardMgtServices" in Web.config.
    public class RuleCardMgtServices : IRuleCardMgtServices
    {

        int intResult;
        byte[] bytesDataTable;

        #region IRuleCardMgtServices Authorization Rule Card

        public int FunPubCreateAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY)
        {
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard ObjAuthorizationRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard();
            intResult = ObjAuthorizationRuleCard.FunPubCreateAuthorizationRuleCard(SerMode, bytesObjAuthorizationRuleCardDataTable_SERLAY);
            return intResult;
        }

        public int FunPubModifyAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY)
        {
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard ObjAuthorizationRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard();
            intResult = ObjAuthorizationRuleCard.FunPubModifyAuthorizationRuleCard(SerMode, bytesObjAuthorizationRuleCardDataTable_SERLAY);
            return intResult;
        }

        public byte[] FunPubQueryTransactionTypeMaster(SerializationMode SerMode, byte[] bytesObjTransactionTypeMasterDataTable_SERLAY)
        {
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_Status_LookUPDataTable DtTransactionType;
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard ObjAuthorizationRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard();
            DtTransactionType = ObjAuthorizationRuleCard.FunQueryTransactionTypeMaster(SerMode, bytesObjTransactionTypeMasterDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(DtTransactionType, SerializationMode.Binary);
            return bytesDataTable;
        }

        public byte[] FunPubQueryConstitutionMaster(SerializationMode SerMode, byte[] bytesObjConstitutionMasterDataTable_SERLAY)
        {
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ConstitutionMasterDataTable DtConstitutionMaster;
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard ObjAuthorizationRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard();
            DtConstitutionMaster = ObjAuthorizationRuleCard.FunQueryConstitutionMaster(SerMode, bytesObjConstitutionMasterDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(DtConstitutionMaster, SerializationMode.Binary);
            return bytesDataTable;
        }

        public byte[] FunPubQueryAuthorizationRuleCard(SerializationMode SerMode, byte[] bytesObjAuthorizationRuleCardDataTable_SERLAY)
        {
            DataSet DsAuthorizationRuleCard = new DataSet();
            try
            {
                S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_AuthorizationRuleCardDataTable DtAuthorizationRuleCard;
                S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard ObjAuthorizationRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubAuthorizationRuleCard();
                DsAuthorizationRuleCard = ObjAuthorizationRuleCard.FunQueryAuthorizationRuleCard(SerMode, bytesObjAuthorizationRuleCardDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(DsAuthorizationRuleCard, SerializationMode.Binary);
                return bytesDataTable;

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Getting AuthorizationRuleCard:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            finally
            {
                DsAuthorizationRuleCard.Dispose();
                DsAuthorizationRuleCard = null;
            }
        }

        #endregion

        #region IRuleCardMgtServices ROIRules

        public int FunPubCreateROIRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules();
                intResult = ObjRuleCard.FunPubCreateROIRulesInt(SerMode, bytesObjROIRulesDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in ROIRules Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public int FunPubModifyROIRulesInt(int intROIRulesID, bool blnIsActive, int intUserID)
        {
            try
            {
                S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules();
                intResult = ObjRuleCard.FunPubModifyROIRulesInt(intROIRulesID, blnIsActive, intUserID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in ROIRules Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public byte[] FunPubQueryROIRules(int intROIRulesID)
        {
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_ROI_RulesDataTable dtROIRules;
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubROIRules();
            dtROIRules = ObjRuleCard.FunPubQueryROIRulesDetails(intROIRulesID);

            bytesDataTable = ClsPubSerialize.Serialize(dtROIRules, SerializationMode.Binary);
            return bytesDataTable;
        }

        #endregion

        #region IRuleCardMgtServices PaymentRuleCard
        public int FunPubCreatePaymentRuleCardInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard();
                intResult = ObjRuleCard.FunPubCreatePaymentRuleCardInt(SerMode, bytesObjROIRulesDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in ROIRules Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public int FunPubModifyPaymentRuleCardInt(int intPayment_RuleCard_ID, bool blnIs_Active, int intUserID)
        {
            try
            {
                S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard();
                intResult = ObjRuleCard.FunPubModifyPaymentRuleCardInt(intPayment_RuleCard_ID, blnIs_Active, intUserID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in ROIRules Creation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        public byte[] FunPubQueryPaymentRuleCard(int intPayment_RuleCard_ID)
        {
            S3GBusEntity.Origination.RuleCardMgtServices.S3G_ORG_PaymentRuleCardDataTable dtPaymentRuleCard = null;
            S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard ObjRuleCard = new S3GDALayer.Origination.RuleCardMgtServices.ClsPubPaymentRuleCard();
            dtPaymentRuleCard = ObjRuleCard.FunPubQueryPaymentRuleCardsDetails(intPayment_RuleCard_ID);

            bytesDataTable = ClsPubSerialize.Serialize(dtPaymentRuleCard, SerializationMode.Binary);
            return bytesDataTable;
        }
        #endregion
    }
}
