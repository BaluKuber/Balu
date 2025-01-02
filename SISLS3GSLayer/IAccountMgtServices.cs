#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Account Management Services Interface ( Currency Creation  )
/// Created By			: Kaliraj K
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// <Program Summary>
#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

#endregion

namespace S3GServiceLayer
{
    // NOTE: If you change the interface name "IAccountMgtServices" here, you must also update the reference to "IAccountMgtServices" in Web.config.
    [ServiceContract]
    public interface IAccountMgtServices
    {

        #region Currency Master

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCurrencyPaging(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryExchangeRateCurrency(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);
        #endregion

        #region AccountSetup

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubAcctDesc(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAcctDesc(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAccountDetails(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateAcctSetup(SerializationMode SerMode, byte[] bytesObjAcctSetupDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyAcctSetup(SerializationMode SerMode, byte[] bytesObjAcctSetupModifyDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAccountDetailsPaging(SerializationMode SerMode, byte[] bytesObjAccountDetailsDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        #endregion

        #region ExchangeRateMaster
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateExchangeRate(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyExchangeRate(SerializationMode SerMode, byte[] bytesObjAcctDescDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryExchangeRate(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryExchangeRatePaging(SerializationMode SerMode, byte[] bytesObjCurrencyMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        

        #endregion


        #region TaxGuide

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTaxGuide(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDataTable_SERLAY, out string Tax_Code);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTaxGuide(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryTaxGuidePaging(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubCreateTaxGuideAssetHistoryPaging(SerializationMode SerMode, byte[] bytesObjTaxGuideMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        #endregion

        //#region CSPMASTER

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubCreateCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //int FunPubModifyCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPMasterDataTable_SERLAY);


        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubQueryCSPMaster(SerializationMode SerMode, byte[] bytesObjCSPDataTable_SERLAY);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubQueryCSPMasterPaging(SerializationMode SerMode, byte[] bytesObjCSPDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        
        

        //#endregion

    }
}
