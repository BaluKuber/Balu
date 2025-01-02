#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Document Noumber Control Services ( User Creation Service Class)
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
using System.Data;
using System.ServiceModel.Activation;
#endregion

namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "DocMgtServices" here, you must also update the reference to "DocMgtServices" in Web.config.
    public class DocMgtServices : IDocMgtServices
    {
        int intResult;

        #region IDocMgtServices Members - Document Number Control Master

        public byte[] FunPubDocTypeList(SerializationMode SerMode, byte[] bytesObjDocTypeListDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationType_ListDataTable dtDocTypeList;
                S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjDocTypeMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
                dtDocTypeList = ObjDocTypeMaster.FunPubDocTypeList(SerMode, bytesObjDocTypeListDataTable_SERLAY);
                byte[] bytesDocTypeListMaster = ClsPubSerialize.Serialize(dtDocTypeList, SerializationMode.Binary);
                return bytesDocTypeListMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public byte[] FunPubGetDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable dtGetDNCDetails;
                S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjGetDNCMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
                dtGetDNCDetails = ObjGetDNCMaster.FunPubQueryDNS(SerMode, bytesObjDNCDataTable_SERLAY);
                byte[] bytesGetDNCMaster = ClsPubSerialize.Serialize(dtGetDNCDetails, SerializationMode.Binary);
                return bytesGetDNCMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetDNCDetails_View(SerializationMode SerMode, byte[] bytesObjDNCDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging)
        {
            byte[] bytesGetDNCMaster;
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_Get_DNCDetailsDataTable dtGetDNCDetails;
                S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjGetDNCMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
                dtGetDNCDetails = ObjGetDNCMaster.FunPubQueryDNS_View(SerMode, bytesObjDNCDataTable_SERLAY, out intTotalRecords, ObjPaging);
                bytesGetDNCMaster = ClsPubSerialize.Serialize(dtGetDNCDetails, SerializationMode.Binary);
                return bytesGetDNCMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Currency Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetBatchList(SerializationMode SerMode, byte[] bytesObjBatchDataTable_SERLAY)
        {
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_GetBatch_ListDataTable dtGetBatchDetails;
                S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjGetBatchMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
                dtGetBatchDetails = ObjGetBatchMaster.FunPubGetBatch(SerMode, bytesObjBatchDataTable_SERLAY);
                byte[] bytesGetBatchMaster = ClsPubSerialize.Serialize(dtGetBatchDetails, SerializationMode.Binary);
                return bytesGetBatchMaster;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Get Batch Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        public int FunPubCreateDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCMasterDataTable_SERLAY)
        {
            S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjDNCMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
            return ObjDNCMaster.FunPubCreateDNS(SerMode, bytesObjDNCMasterDataTable_SERLAY);


        }


        public int FunPubModifyDNCDetails(SerializationMode SerMode, byte[] bytesObjDNCMasterDataTable_SERLAY)
        {
            S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjDNCDetails = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
            return ObjDNCDetails.FunPubModifyDNS(SerMode, bytesObjDNCMasterDataTable_SERLAY);
        }

        public int FunPubDeleteDNCDetails(int intDoc_Number_Seq_ID)
        {
            try
            {
                S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl ObjDNCMaster = new S3GDALayer.DocMgtServices.ClsPubDoucmentNoControl();
                intResult = ObjDNCMaster.FunPubDeleteDNS(intDoc_Number_Seq_ID);
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

        #region IDocMgtServices Members - Document Path


        public byte[] FunPubGetDocFlagList(int intDocument_Flag_ID)
        {
            byte[] bytesGetDocFlagList;
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationFlagDataTable ObjDocFlagTable_SERLAY;
                S3GDALayer.DocMgtServices.ClsPubDocumentPath ObjDocumentFlag = new S3GDALayer.DocMgtServices.ClsPubDocumentPath();
                ObjDocFlagTable_SERLAY = ObjDocumentFlag.FunPubGetDocFlagList(intDocument_Flag_ID);
                bytesGetDocFlagList = ClsPubSerialize.Serialize(ObjDocFlagTable_SERLAY, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Document Flag Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesGetDocFlagList;

        }

        public int FunPubCreateDocPathDetails(SerializationMode SerMode, byte[] bytesObjDocPathMasterDataTable_SERLAY)
        {
            try
            {
                S3GDALayer.DocMgtServices.ClsPubDocumentPath ObjDocumentPath = new S3GDALayer.DocMgtServices.ClsPubDocumentPath();
                intResult= ObjDocumentPath.FunPubCreateDocumentPathInt(SerMode, bytesObjDocPathMasterDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Document Path  Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

        public int FunPubModifyDocPathDetails(SerializationMode SerMode, byte[] bytesObjDocPathMasterDataTable_SERLAY)
        {
            try
            {
            S3GDALayer.DocMgtServices.ClsPubDocumentPath ObjDocumentPath = new S3GDALayer.DocMgtServices.ClsPubDocumentPath();
            intResult = ObjDocumentPath.FunPubModifyDocumentPathInt(SerMode, bytesObjDocPathMasterDataTable_SERLAY);
            }
                catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Document Path  Modification :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;

        }

        public byte[] FunPubGetDocPathDetails(int intComapny_id, int intDocument_Path_ID)
        {
            byte[] bytesGetDocPathDetails;
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable ObjDocPathTable_SERLAY;
                S3GDALayer.DocMgtServices.ClsPubDocumentPath ObjDocumentFlag = new S3GDALayer.DocMgtServices.ClsPubDocumentPath();
                ObjDocPathTable_SERLAY = ObjDocumentFlag.FunPubGetDocPathDetails(intComapny_id,intDocument_Path_ID);
                bytesGetDocPathDetails = ClsPubSerialize.Serialize(ObjDocPathTable_SERLAY, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Document Path Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesGetDocPathDetails;

        }

        public byte[] FunPubQueryDocPathPaging(out int intTotalRecords, PagingValues ObjPaging)
        {
            byte[] bytesGetDocPathDetails;
            try
            {
                S3GBusEntity.DocMgtServices.S3G_SYSAD_DocumentationPathDataTable ObjDocPathTable_SERLAY;
                S3GDALayer.DocMgtServices.ClsPubDocumentPath ObjDocumentFlag = new S3GDALayer.DocMgtServices.ClsPubDocumentPath();
                ObjDocPathTable_SERLAY = ObjDocumentFlag.FunPubGetDocPathPaging(out intTotalRecords, ObjPaging);
                bytesGetDocPathDetails = ClsPubSerialize.Serialize(ObjDocPathTable_SERLAY, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Document Path Querying :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesGetDocPathDetails;
        }
        #endregion

        #region "Template Master"
        public int FunPubCreateTemplate(SerializationMode SerMode, byte[] bytesS3G_SYSAD_TemplateDtls, out string strTemplateNo)
        {
            try
            {
                S3GDALayer.DocMgtServices.ClsPubTemplateMaster ObjTemplateMaster = new S3GDALayer.DocMgtServices.ClsPubTemplateMaster();
                intResult = ObjTemplateMaster.FunPubCreateTemplate(SerMode, bytesS3G_SYSAD_TemplateDtls,out strTemplateNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Template Creation :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }
        #endregion

    }
}
