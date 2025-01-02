#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Number Control Services ( Currency Creation Service Class)
/// Created By			: Kannan RC
/// Created Date		: 22-May-2010
/// Purpose	            : 
/// Last Updated By		: 
/// Last Updated Date   : 
/// Reason              : 
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
#endregion

namespace S3GServiceLayer
{
    // NOTE: If you change the interface name "IDocMgtServices" here, you must also update the reference to "IDocMgtServices" in Web.config.
    [ServiceContract]
    public interface IDocMgtServices
    {

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubDocTypeList(SerializationMode SerMode, byte[] bytesObjDocTypeListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDNCDetails_View(SerializationMode SerMode, byte[] bytesObjDNCDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetBatchList(SerializationMode SerMode, byte[] bytesObjBatchDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteDNCDetails(int intDoc_Number_Seq_ID);

        #region Document Path
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDocFlagList(int intDocument_Flag_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDocPathDetails(int intComapny_id, int intDocument_Path_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryDocPathPaging(out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateDocPathDetails(SerializationMode SerMode, byte[] bytesObjDocPathMasterDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyDocPathDetails(SerializationMode SerMode, byte[] bytesObjDocPathMasterDataTable_SERLAY);

        #endregion


        #region "Template Master"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateTemplate(SerializationMode SerMode, byte[] bytesS3G_SYSAD_TemplateDtls, out string strTemplateNo);

        #endregion

    }
}