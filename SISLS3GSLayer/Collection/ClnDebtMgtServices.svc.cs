#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Debt Collector Master Service Layer 
/// Created By			: Suresh P
/// Created Date		: 09-Oct-2010
/// Purpose	            : 
/// Last Updated By		: Rajendran
/// Last Updated Date   : 10/26/2010
/// Reason              : Debt Commission Methods Implemented
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Data;
using System.ServiceModel;
using S3GBusEntity;
using S3GDALayer;
using System.Collections.Generic;
using System.ServiceModel.Activation;
#endregion
namespace S3GServiceLayer.Collection
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ClnDebtMgtServices" here, you must also update the reference to "ClnDebtMgtServices" in Web.config.
    public class ClnDebtMgtServices : IClnDebtMgtServices
    {
        byte[] bytesDataTable;

        #region ClnDebtMgtServices DebtCollector Master
        /// <summary>
        /// Insert DebtCollector Master
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDebtCollectorMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateDebtCollectorMaster(SerializationMode SerMode, byte[] bytesObjDebtCollectorMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster ObjMonthClosure = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster();
                return ObjMonthClosure.FunPubCreateDebtCollectorInt(SerMode, bytesObjDebtCollectorMasterDataTable_SERLAY);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector Master:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Get  DebtCollector Master
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDebtCollectorDetailsDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryDebtCollector(SerializationMode SerMode, byte[] bytesObjDebtCollectorDetailsDataTable_SERLAY)
        {
            S3GBusEntity.Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorDetailsDataTable dtDebtCollectorDetails = null;
            S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster ObjDebtCollector = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster();
            dtDebtCollectorDetails = ObjDebtCollector.FunPubQueryDebtCollector(SerMode, bytesObjDebtCollectorDetailsDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(dtDebtCollectorDetails, SerMode);
            return bytesDataTable;
        }

        /// <summary>
        /// Modify DebtCollector Master
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDebtCollectorMasterDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubModifyDebtCollectorMaster(SerializationMode SerMode, byte[] bytesObjDebtCollectorMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster ObjMonthClosure = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorMaster();
                return ObjMonthClosure.FunPubModifyDebtCollectorInt(SerMode, bytesObjDebtCollectorMasterDataTable_SERLAY);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector Master:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region ClnDebtMgtServices Debt Collector RuleCard
        /// <summary>
        /// Insert DebtCollector RuleCard
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDebtCollectorRuleCardDataTable_SERLAY"></param>
        /// <returns></returns>
        public int FunPubCreateDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard ObjMonthClosure = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard();
                return ObjMonthClosure.FunPubCreateDebtCollectorRuleCardInt(SerMode, bytesObjDebtCollectorRuleCardDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector RuleCard:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// Get DebtCollector RuleCard
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjDebtCollectorRuleCardDetailsDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDetailsDataTable_SERLAY)
        {
            S3GBusEntity.Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorRuleCardDataTable dtDebtCollectorRuleCardDetails = null;
            S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard ObjDebtCollectorRuleCard = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard();
            dtDebtCollectorRuleCardDetails = ObjDebtCollectorRuleCard.FunPubQueryDebtCollectorRuleCard(SerMode, bytesObjDebtCollectorRuleCardDetailsDataTable_SERLAY);
            bytesDataTable = ClsPubSerialize.Serialize(dtDebtCollectorRuleCardDetails, SerMode);
            return bytesDataTable;
        }

        public int FunPubModifyDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard ObjMonthClosure = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDebtCollectorRuleCard();
                return ObjMonthClosure.FunPubModifyDebtCollectorRuleCardInt(SerMode, bytesObjDebtCollectorRuleCardDataTable_SERLAY);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector RuleCard:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion


        /// <summary>
        /// Function Mainly for Gridview Binding with paging it should have predefined Paging object
        /// passed to it.
        /// </summary>
        /// <param name="strProcName">Procedure name used for grid binding </param>
        /// <param name="dctProcParams">Parameter for the Procedure</param>
        /// <param name="intTotalRecords">Total records that the query return</param>
        /// <param name="ObjPaging"> Paging object that has page related details</param>
        /// <returns>DataTable</returns>
        public byte[] FunPubGetDatasetGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging)
        {
            byte[] bytesDataset;
            try
            {

                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing ObjClnDebtMgtServices = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing();
                DataSet dsGridPaging = ObjClnDebtMgtServices.FunPubGetDatasetGridPaging(strProcName, dctProcParams, out intTotalRecords, ObjPaging);
                bytesDataset = ClsPubSerialize.Serialize(dsGridPaging, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Filling Grid :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataset;
        }


        #region IClnDebtMgtServices Members


        public int FunPubCreateDebtCollectorCommission(SerializationMode SerMode, byte[] bytesObjDebtCollectorCommissionDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing ObjDCIncentive = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing();
                return ObjDCIncentive.FunCreateDCCommisionProcessing(SerMode, bytesObjDebtCollectorCommissionDataTable_SERLAY);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector Master:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryDebtCollectorCommission(int DC_CommisssionId)
        {
            try
            {                
                S3GBusEntity.Collection.ClnDebtMgtServices.S3G_CLN_DebtCollectorCommissionDataTable dtDCCommissionDetails = null;
                S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing ObjDCCommission = new S3GDALayer.Collection.ClnDebtMgtServices.ClsPubDCCommissionProcessing();
                dtDCCommissionDetails = ObjDCCommission.FuncQueryDCCommissionDetails(DC_CommisssionId);
                bytesDataTable = ClsPubSerialize.Serialize(dtDCCommissionDetails, SerializationMode.Binary);
                return bytesDataTable;

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Debt Collector Master:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubModifyDebtCollectorCommission(SerializationMode SerMode, byte[] bytesObjDebtCollectorCommissionDataTable_SERLAY)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
