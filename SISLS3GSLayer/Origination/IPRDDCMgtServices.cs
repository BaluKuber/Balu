#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Origination
/// Screen Name			: PRDDC Master
/// Created By			: Kaliraj K
/// Created Date		: 07-Jun-2010
/// Purpose	            : 
/// <Program Summary>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IPRDDCMgtServices" here, you must also update the reference to "IPRDDCMgtServices" in Web.config.
    [ServiceContract]
    public interface IPRDDCMgtServices
    {

        #region PRDDC Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        //FIR
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFIRDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatelegalDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        //FIR

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateFIROtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable);



        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQuerylegalDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_LegalMasterDataTable);

        //FIR
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryFIRDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_FIRMasterDataTable);

        //END

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPRDDCMaster(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPRDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY);
        #endregion

        #region PDDC Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOtherPostDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PDDCMasterDataTable);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubQueryPDDCMaster(SerializationMode SerMode, byte[] bytesObjPDDCMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPDDCMasterDataTable_SERLAY);
        #endregion

        #region PRDDT
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPRDDTLookUp(SerializationMode SerMode, byte[] bytesObjPRDDTLookDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTypeTrans(SerializationMode SerMode, byte[] bytesObjTypeTransDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTransDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPRDDTransDoc(SerializationMode SerMode, byte[] bytesObjPRDDTransDocDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY, out string strPRDDTNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubExitsPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTExitsDataTable_SERLAY);
        #endregion PRDDT

        #region PDDT

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreatePDDTransMaster(SerializationMode SerMode, byte[] bytesObjPDDDTrans_SERLAY, out string strPRDDTNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyPDDTransMaster(SerializationMode SerMode, byte[] bytesObjPDDTrans_SERLAY);

        #endregion

        #region TA PRDDC Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreatePRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTACreateOtherDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAQueryPRDDCDocDetails(SerializationMode SerMode, byte[] bytesObjS3G_ORG_PRDDCMasterDataTable);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAQueryPRDDCMaster(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubTAQueryPRDDCMasterCombi(SerializationMode SerMode, byte[] bytesObjPRDDCMasterDataTable_SERLAY);
        #endregion

        #region TA PRDD Transaction
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTAPRDDTLookUp(SerializationMode SerMode, byte[] bytesObjPRDDTLookDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTATypeTrans(SerializationMode SerMode, byte[] bytesObjTypeTransDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTAPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTransDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTAPRDDTransDoc(SerializationMode SerMode, byte[] bytesObjPRDDTransDocDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTAPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY, out string strPRDDTNumber_Out);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyTAPRDDTransMaster(SerializationMode SerMode, byte[] bytesObjPRDDTrans_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubExitsTAPRDDTrans(SerializationMode SerMode, byte[] bytesObjPRDDTExitsDataTable_SERLAY);
        #endregion PRDDT

    }
}
