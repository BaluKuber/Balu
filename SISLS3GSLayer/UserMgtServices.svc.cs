#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: User Management Services ( User,RoleCode,Escalation and module code creation service Class)
/// Created By			: Kaliraj K
/// Created Date		: 11-May-2010
/// Purpose	            : 
/// Last Updated By		: Gunasekar.K
/// Last Updated Date   : 12-Oct-2010
/// Reason              : To add Modulecode in FunPubQueryProgramMasterList - Function
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
using S3GDALayer;
using System.Data;
using System.ServiceModel.Activation;
#endregion

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "UserMgtServices" here, you must also update the reference to "UserMgtServices" in Web.config.
    public class UserMgtServices : IUserMgtServices
    {
        int intResult;
        #region IUserMgtServices Members - User Management ( User Creation )

        /// <summary>
        /// Calling DAL Layer's Create User Method (FunPubCreateUser) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubCreateUser(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                return ObjUserManagement.FunPubCreateUserInt(SerMode, bytesObjUserManagementDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public void FunPubUpdateLOBMapping(int intUserID, int intLOBID)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                ObjUserManagement.FunPubUpdateLOBMapping(intUserID, intLOBID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// This is used to fetch user details
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserManagementDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryUser(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_UserManagementDataTable dtUserManagement;
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                dtUserManagement = ObjUserManagement.FunPubQueryUser(SerMode, bytesObjUserManagementDataTable_SERLAY);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dtUserManagement, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryUserPaging(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_UserManagementDataTable dtUserManagement;
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                dtUserManagement = ObjUserManagement.FunPubQueryUserPaging(SerMode, bytesObjUserManagementDataTable_SERLAY, out intTotalRecords, ObjPaging);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dtUserManagement, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// This is used to fetch branch and role details based on user and compnay id
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserManagementDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryBranchRoleDetails(SerializationMode SerMode, byte[] bytesObjUserManagementDataTable_SERLAY, string strMode)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                DataSet dsBranchRole = ObjUserManagement.FunPubQueryBranchRoleDetails(SerMode, bytesObjUserManagementDataTable_SERLAY, strMode);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dsBranchRole, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// This is used to fetch user role details based on lob
        /// </summary>
        /// <param name="intCompanyID"></param>
        /// <param name="intUserId"></param>
        /// <param name="intLOBID"></param>
        /// <returns></returns>
        public byte[] FunPubQueryUserRoleDetails(int intCompanyID, int intUserId, int intLOBID, string strMode)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                DataTable dtBranchRole = ObjUserManagement.FunPubQueryUserRoleDetails(intCompanyID, intUserId, intLOBID, strMode);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dtBranchRole, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intCompanyID"></param>
        /// <param name="intLOBID"></param>
        /// <returns></returns>
        public byte[] FunPubQueryRoleDetails(int intCompanyID, int intLOBID)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                DataTable dtBranchRole = ObjUserManagement.FunPubQueryRoleDetails(intCompanyID, intLOBID);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dtBranchRole, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// This is used to fetch branches based on region .Here region ids are sent through XML string
        /// </summary>
        /// <param name="intCompanyID"></param>
        /// <param name="intUserId"></param>
        /// <param name="strXMLRegionDet"></param>
        /// <returns></returns>
        public byte[] FunPubQueryUserLocationDetails(int intCompanyID, int intUserId, int intLOB_ID, string strMode)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                DataTable dtBranchRole = ObjUserManagement.FunPubQueryUserLocationDetails(intCompanyID, intUserId, intLOB_ID, strMode);
                byte[] bytesUserManagement = ClsPubSerialize.Serialize(dtBranchRole, SerializationMode.Binary);
                return bytesUserManagement;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        /// <summary>
        /// This is used to fetch user code 
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUserManagementDataTable_SERLAY"></param>
        /// <returns></returns>
        public byte[] FunPubQueryUserMaster(SerializationMode SerMode, byte[] bytesObjUserMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_UserMaster_ListDataTable dtUserMaster;
                S3GDALayer.UserMgtServices.ClsPubUserManagement ObjUserManagement = new S3GDALayer.UserMgtServices.ClsPubUserManagement();
                dtUserMaster = ObjUserManagement.FunPubQueryUserMaster(SerMode, bytesObjUserMasterDataTable_SERLAY);
                byte[] bytesUserMaster = ClsPubSerialize.Serialize(dtUserMaster, SerializationMode.Binary);
                return bytesUserMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region RoleCenterMaster

        public int FunPubCreateRoleCodeMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                return ObjRoleCenterMaster.FunRoleCodeMasterInsertInt(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        public int FunPubModifyRoleCodeMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                return ObjRoleCenterMaster.FunPubModifyRoleCodeMaster(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryRoleCodeList(SerializationMode SerMode, byte[] bytesObjRoleCodeListDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable dtRoleCodeList;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCodeMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                dtRoleCodeList = ObjRoleCodeMaster.FunPubQueryRoleCodeList(SerMode, bytesObjRoleCodeListDataTable_SERLAY);
                byte[] bytesRoleCodeMaster = ClsPubSerialize.Serialize(dtRoleCodeList, SerializationMode.Binary);
                return bytesRoleCodeMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public int FunPubDeleteRoleCenterMaster(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                return ObjRoleCenterMaster.FunPubDeleteRoleCenterMaster(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable dtRoleCodeMasterDetails;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                dtRoleCodeMasterDetails = ObjRoleCenterMaster.FunPubQueryRoleCodeMasterDetails(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY);
                byte[] bytesObjRoleCentrMaster = ClsPubSerialize.Serialize(dtRoleCodeMasterDetails, SerializationMode.Binary);
                return bytesObjRoleCentrMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubQueryRoleCodeMasterPaging(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable dtRoleCodeMasterDetails;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                dtRoleCodeMasterDetails = ObjRoleCenterMaster.FunPubQueryRoleCodeMasterPaging(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                byte[] bytesObjRoleCentrMaster = ClsPubSerialize.Serialize(dtRoleCodeMasterDetails, SerializationMode.Binary);
                return bytesObjRoleCentrMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubCheckRoleCodeMasterDetails(SerializationMode SerMode, byte[] bytesObjRoleCenterMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCodeMasterDataTable dtRoleCodeMasterDetails;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                dtRoleCodeMasterDetails = ObjRoleCenterMaster.FunPubCheckRoleCodeMasterDetails(SerMode, bytesObjRoleCenterMasterDataTable_SERLAY);
                byte[] bytesObjRoleCentrMaster = ClsPubSerialize.Serialize(dtRoleCodeMasterDetails, SerializationMode.Binary);
                return bytesObjRoleCentrMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        //-- Added by Guna on 12-Oct-2010 To add the Module code 
        ////public byte[] FunPubQueryProgramMasterList(SerializationMode SerMode, byte[] bytesObjProgramMasterDataTable_SERLAY)
        public byte[] FunPubQueryProgramMasterList(SerializationMode SerMode, byte[] bytesObjProgramMasterDataTable_SERLAY, string sModuleCode)
        //-- Ends Here
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_ProgramMasterDataTable dtProgramMasterDetails;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjProgramMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                //-- Added by Guna on 12-Oct-2010 To add the Module code 
                ////dtProgramMasterDetails = ObjProgramMaster.FunPubQueryProgramMasterList(SerMode, bytesObjProgramMasterDataTable_SERLAY);
                dtProgramMasterDetails = ObjProgramMaster.FunPubQueryProgramMasterList(SerMode, bytesObjProgramMasterDataTable_SERLAY, sModuleCode);
                //Ends Here
                byte[] bytesObjProgramMaster = ClsPubSerialize.Serialize(dtProgramMasterDetails, SerializationMode.Binary);
                return bytesObjProgramMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubQueryModuleMasterList(SerializationMode SerMode, byte[] bytesObjModuleMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_ModuleMasterDataTable dtModuleMasterDetails;
                S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjModuleMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
                dtModuleMasterDetails = ObjModuleMaster.FunPubQueryModuleMasterList(SerMode, bytesObjModuleMasterDataTable_SERLAY);
                byte[] bytesObjModuleMaster = ClsPubSerialize.Serialize(dtModuleMasterDetails, SerializationMode.Binary);
                return bytesObjModuleMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region Escalation
        public byte[] FunPubRoleCodeList(SerializationMode SerMode, byte[] bytesObjRoleCodeListDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleCode_ListDataTable dtRoleCodeList;
                S3GDALayer.UserMgtServices.ClsPubEscalation ObjRoleCodeMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
                dtRoleCodeList = ObjRoleCodeMaster.FunPubRoleCodeList(SerMode, bytesObjRoleCodeListDataTable_SERLAY);
                byte[] bytesRoleCodeMaster = ClsPubSerialize.Serialize(dtRoleCodeList, SerializationMode.Binary);
                return bytesRoleCodeMaster;
            }
            catch (Exception ExObj)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + ExObj.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubRoleUserList(SerializationMode SerMode, byte[] bytesObjRoleUserListDataTable_SERLAY)
        {

            S3GBusEntity.UserMgtServices.S3G_SYSAD_RoleUser_ListDataTable dtRoleUserList;
            S3GDALayer.UserMgtServices.ClsPubEscalation ObjRoleUserMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
            dtRoleUserList = ObjRoleUserMaster.FunPubRoleUserList(SerMode, bytesObjRoleUserListDataTable_SERLAY);
            byte[] bytesRoleUserMaster = ClsPubSerialize.Serialize(dtRoleUserList, SerializationMode.Binary);
            return bytesRoleUserMaster;
        }

        public int FunPubCreateEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY)
        {
            S3GDALayer.UserMgtServices.ClsPubEscalation ObjEscalationMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
            return ObjEscalationMaster.FunPubCreateEscalation(SerMode, bytesObjEscalationMasterDataTable_SERLAY);

        }

        public byte[] FunPubQueryEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY)
        {
            S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable dtEscalationMasterDetails;
            S3GDALayer.UserMgtServices.ClsPubEscalation ObjEscalationMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
            dtEscalationMasterDetails = ObjEscalationMaster.FunPubQueryEscalation(SerMode, bytesObjEscalationMasterDataTable_SERLAY);
            byte[] bytesObjEscalationMaster = ClsPubSerialize.Serialize(dtEscalationMasterDetails, SerializationMode.Binary);
            return bytesObjEscalationMaster;
        }

        public byte[] FunPubQueryEscalationMasterPaging(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            S3GBusEntity.UserMgtServices.S3G_SYSAD_Get_EscalationDetailsDataTable dtEscalationMasterDetails;
            S3GDALayer.UserMgtServices.ClsPubEscalation ObjEscalationMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
            dtEscalationMasterDetails = ObjEscalationMaster.FunPubQueryEscalationPaging(SerMode, bytesObjEscalationMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
            byte[] bytesObjEscalationMaster = ClsPubSerialize.Serialize(dtEscalationMasterDetails, SerializationMode.Binary);
            return bytesObjEscalationMaster;
        }



        public int FunPubModifyEscalationMaster(SerializationMode SerMode, byte[] bytesObjEscalationMasterDataTable_SERLAY)
        {
            S3GDALayer.UserMgtServices.ClsPubEscalation ObjEscalationMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
            return ObjEscalationMaster.FunPubModifyEscalation(SerMode, bytesObjEscalationMasterDataTable_SERLAY);

        }
        public int FunPubDeleteEscalationDetails(int inEscalation_ID)
        {
            try
            {
                S3GDALayer.UserMgtServices.ClsPubEscalation ObjEscalationMaster = new S3GDALayer.UserMgtServices.ClsPubEscalation();
                intResult = ObjEscalationMaster.FunPubDeleteEscalation(inEscalation_ID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in DNC Program Access Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        #endregion

    }
}
