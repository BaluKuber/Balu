#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: User Management Services Interface ( User Creation  )
/// Created By			: Kaliraj K
/// Created Date		: 13-May-2010
/// Purpose	            : 
/// Last Updated By     : Gunasekar.K
/// Last Updated Date   : 12-Oct-2010
/// Purpose             : To add Modulecode in FunPubQueryProgramMasterList - Function
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
    // NOTE: If you change the interface name "IUserMgtServices" here, you must also update the reference to "IUserMgtServices" in Web.config.
    [ServiceContract]
    public interface IUserMgtServices
    {
        //#region User Management

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateUser(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        void FunPubUpdateLOBMapping(int intUserID, int intLOBID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUser(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUserPaging(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUserMaster(SerializationMode SerMode, byte[] bytesObjUserMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryBranchRoleDetails(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY, string strMode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUserRoleDetails(int intCompanyID, int intUserId, int intLOBID, string strMode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRoleDetails(int intCompanyID, int intLOBID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryUserLocationDetails(int intCompanyID, int intUserId, int intLOB_ID, string strMode);


        //#endregion

        #region RoleCenterMaser

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRoleCodeMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyRoleCodeMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteRoleCenterMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRoleCodeList(SerializationMode SerMode, byte[] bytesObjRoleCodeListDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubCheckRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRoleCodeMasterPaging(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        //-- Added by Guna on 12-Oct-2010 To add the Module code 
        ////byte[] FunPubQueryProgramMasterList(SerializationMode SerMode, byte[] bytesObjProgramMasterDataTable_SERLAY);
        byte[] FunPubQueryProgramMasterList(SerializationMode SerMode, byte[] bytesObjProgramMasterDataTable_SERLAY, string sModuleCode);
        //-- Ends Here

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryModuleMasterList(SerializationMode SerMode, byte[] bytesObjModuleMasterDataTable_SERLAY);
        #endregion

        #region Escalation
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubRoleCodeList(SerializationMode SerMode, byte[] bytesObjRoleCodeListDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubRoleUserList(SerializationMode SerMode, byte[] bytesObjRoleUserListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEscalationMasterPaging(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteEscalationDetails(int inEscalation_ID);
        #endregion
    }
}
