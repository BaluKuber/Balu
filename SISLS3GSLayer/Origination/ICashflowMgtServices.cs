#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: Cashflow Services Interface ( Currency Creation  )
/// Created By			: Kannan RC
/// Created Date		: 01-Jun-2010
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

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "ICashflowMgtServices" here, you must also update the reference to "ICashflowMgtServices" in Web.config.
    [ServiceContract]
    public interface ICashflowMgtServices
    {
        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetCashflowflagList(SerializationMode SerMode, byte[] bytesObjCashflowflagListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGLSLCodeList(SerializationMode SerMode, byte[] bytesObjGLSLCodeListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProgramNumber(SerializationMode SerMode, byte[] bytesObjProgramNumnerDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCashflowLook(SerializationMode SerMode, byte[] bytesObjCashflowLookDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCashflowMaster(SerializationMode SerMode, byte[] bytesObjCashflowDataTable_SERLAY, out string strCashflowNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCashflowMaster(SerializationMode SerMode, byte[] bytesObjCashflowDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteCashflowMaster(SerializationMode SerMode, byte[] bytesObjDeleteCashflowDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCashflowDetails(SerializationMode SerMode, byte[] bytesObjCashflowDetailsDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCashflowMapping(SerializationMode SerMode, byte[] bytesObjCashflowMappingDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCashFlowRulesInt(SerializationMode SerMode, byte[] bytesObjROIRulesDataTable_SERLAY);
       

    }
}
