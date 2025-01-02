#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: UTPA Creation Service Layer InterFace
/// Created By			: Suresh P
/// Created Date		: 19-May-2010
/// Purpose	            : 
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
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
    // NOTE: If you change the interface name "ITPAMgtServices" here, you must also update the reference to "ITPAMgtServices" in Web.config.
    [ServiceContract]
    public interface ITPAMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY, out int intUTPAID_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteUTPAProgramAccess(int intUTPA_Prog_Access_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUTPAPaging(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

    }
}
