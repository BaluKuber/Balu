#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Collection
/// Screen Name			: Debt Collector Master Service Layer InterFace
/// Created By			: Suresh P
/// Created Date		: 09-Oct-2010
/// Purpose	            : 
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>
#endregion

#region Namespaces
using System.ServiceModel;
using S3GBusEntity;
using System.Collections;
using System.Collections.Generic;
#endregion


namespace S3GServiceLayer.Collection
{
    // NOTE: If you change the interface name "IClnDebtMgtServices" here, you must also update the reference to "IClnDebtMgtServices" in Web.config.
    [ServiceContract]
    public interface IClnDebtMgtServices
    {
        #region ClnDebtMgtServices Debt Collector Master
        [OperationContract]
        int FunPubCreateDebtCollectorMaster(SerializationMode SerMode, byte[] bytesObjDebtCollectorMasterDataTable_SERLAY);

        [OperationContract]
        byte[] FunPubQueryDebtCollector(SerializationMode SerMode, byte[] bytesObjDebtCollectorDetailsDataTable_SERLAY);

        [OperationContract]
        int FunPubModifyDebtCollectorMaster(SerializationMode SerMode, byte[] bytesObjDebtCollectorMasterDataTable_SERLAY);
        #endregion

        #region ClnDebtMgtServices Debt Collector RuleCard
        [OperationContract]
        int FunPubCreateDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDataTable_SERLAY);

        [OperationContract]
        byte[] FunPubQueryDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDetailsDataTable_SERLAY);

        [OperationContract]
        int FunPubModifyDebtCollectorRuleCard(SerializationMode SerMode, byte[] bytesObjDebtCollectorRuleCardDataTable_SERLAY);

        #endregion

        #region  Debt Collection Incentive / Commission Processing Interface
        [OperationContract]
        int FunPubCreateDebtCollectorCommission(SerializationMode SerMode, byte[] bytesObjDebtCollectorCommissionDataTable_SERLAY);

        [OperationContract]
        byte[] FunPubQueryDebtCollectorCommission(int DC_CommisssionId);

        [OperationContract]
        int FunPubModifyDebtCollectorCommission(SerializationMode SerMode, byte[] bytesObjDebtCollectorCommissionDataTable_SERLAY);
        #endregion


        [OperationContract]
        byte[] FunPubGetDatasetGridPaging(string strProcName, Dictionary<string, string> dctProcParams, out int intTotalRecords, PagingValues ObjPaging);

    }
}
