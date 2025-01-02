#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: UTPA Creation Service Layer 
/// Created By			: Suresh P
/// Created Date		: 19-May-2010
/// Purpose	            : 
/// Last Updated By		: NULL
/// Last Updated Date   : NULL
/// Reason              : NULL
/// <Program Summary>

// TESTING
#endregion

#region Namespaces
using System;
using System.ServiceModel;
using S3GBusEntity;
using System.ServiceModel.Activation;
#endregion

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "TPAMgtServices" here, you must also update the reference to "TPAMgtServices" in Web.config.
    public class TPAMgtServices : ITPAMgtServices
    {

        int intResult;
        byte[] bytesDataTable;

        /// <summary>
        /// Calling DAL Layer's Create UTPA Method (FunPubCreateUTPAInt) with Ser Mode and Bytes as Input
        /// </summary>
        /// <param name="SerMode"></param>
        /// <param name="bytesObjUTPAMasterDataTable_SERLAY"></param>
        /// <returns>Error Code (it is 0 if no error is found)</returns>
        public int FunPubCreateUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY, out int intUTPAID_Out)
        {
            try
            {
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                intResult = ObjUTPAMaster.FunPubCreateUTPAInt(SerMode, bytesObjUTPAMasterDataTable_SERLAY, out intUTPAID_Out);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                intResult = ObjUTPAMaster.FunPubModifyUTPAInt(SerMode, bytesObjUTPAMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }


        public int FunPubCreateUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                intResult = ObjUTPAMaster.FunPubCreateUTPAProgramAccessInt(SerMode, bytesObjUTPAProgramAccessDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Program Access Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                intResult = ObjUTPAMaster.FunPubModifyUTPAProgramAccessInt(SerMode, bytesObjUTPAProgramAccessDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubDeleteUTPAProgramAccess(int intUTPA_Prog_Access_ID)
        {
            try
            {
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                intResult = ObjUTPAMaster.FunPubDeleteUTPAProgramAccessInt(intUTPA_Prog_Access_ID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Program Access Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public byte[] FunPubQueryUTPA(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable dtUTPAMaster;
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                dtUTPAMaster = ObjUTPAMaster.FunPubQueryUTPADetails(SerMode, bytesObjUTPAMasterDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtUTPAMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryUTPAProgramAccess(SerializationMode SerMode, byte[] bytesObjUTPAProgramAccessDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable dtUTPAProgramAccess;
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAProgramAccessMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                dtUTPAProgramAccess = ObjUTPAProgramAccessMaster.FunPubQueryUTPAProgramAccessDetails(SerMode, bytesObjUTPAProgramAccessDataTable_SERLAY);
                bytesDataTable = ClsPubSerialize.Serialize(dtUTPAProgramAccess, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA ProgramAccess Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubQueryUTPAPaging(SerializationMode SerMode, byte[] bytesObjUTPAMasterDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            try
            {
                S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAMasterDataTable dtUTPAMaster;
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjUTPAMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                dtUTPAMaster = ObjUTPAMaster.FunPubQueryUTPADetailsPaging(SerMode, bytesObjUTPAMasterDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesDataTable = ClsPubSerialize.Serialize(dtUTPAMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in UTPA Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        /*public byte[] FunPubQueryUTPAProgramAccess(int intUTPAID, int intUTPA_Prog_Access_ID)
        {
            try
            {
                S3GBusEntity.TPAMgtServices.S3G_SYSAD_UTPAProgramAccessDataTable dtLOBMaster;
                S3GDALayer.TPAMgtServices.ClsPubUTPAMaster ObjLOBMaster = new S3GDALayer.TPAMgtServices.ClsPubUTPAMaster();
                dtLOBMaster = ObjLOBMaster.FunPubQueryUTPAProgramAccessDetails(intUTPAID, intUTPA_Prog_Access_ID);
                bytesDataTable = ClsPubSerialize.Serialize(dtLOBMaster, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in LOB Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }*/

    }
}
