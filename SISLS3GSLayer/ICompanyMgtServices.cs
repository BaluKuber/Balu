#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: System Admin
/// Screen Name			: Company Creation Service Layer InterFace
/// Created By			: Nataraj Y
/// Created Date		: 10-May-2010
/// Purpose	            : 
/// Last Updated By		: Nataraj Y
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin Company Module Developement

/// Last Updated By		: Suresh P
/// Last Updated Date   : 10-May-2010
/// Reason              : System Admin LOB Module Developement

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
using System.Data;
#endregion

namespace S3GServiceLayer
{
    // NOTE: If you change the interface name "ICompanyMgtServices" here, you must also update the reference to "ICompanyMgtServices" in Web.config.
    [ServiceContract]
    public interface ICompanyMgtServices
    {
        #region CompanyMaster
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCompany(SerializationMode SerMode, byte[] bytesObjCompanyMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCompany_List(int CompanyId);
        #endregion

        #region LOB MASTER
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY);

             [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryLOB(SerializationMode SerMode, byte[] bytesObjLOBMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryLOB_LIST(SerializationMode SerMode, byte[] bytesObjLOBMasterListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryLOBPaging(SerializationMode SerMode, byte[] bytesObjLOBMasterListDataTable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        
        #endregion

        #region Global Parameter Setup
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGlobalParamSetUp(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyGlobalParamSetUp(SerializationMode SerMode, byte[] bytesobjS3G_SYSAD_GlobalParameterSetupDataTable_SERALY, byte[] bytesobjS3G_SYSAD_GlobalParamMonthEnd_DetailsDataTable_SERALY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGPSDetails(int intCompanyId,string Year,string Month);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryGPSDetails_New(int intCompanyId, string Year, string Month);
        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCustomerAlertPaging(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);
        #region Product Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryProduct(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryProductPaging(SerializationMode SerMode, byte[] bytesObjProductMasterDatatable_SERLAYout,out int intTotalRecords, PagingValues ObjPaging);
        #endregion

        #region Custromer Alert
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubDeleteCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEventTypeMaster(SerializationMode SerMode, byte[] bytesObjEventTypeDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEntityTypeMaster(SerializationMode SerMode, byte[] bytesObjEntityTypeDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryCustomerAlert(SerializationMode SerMode, byte[] bytesObjCustomerAlertDatatable_SERLAY);
        #endregion

        #region Region
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRegion(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryRegion_View(SerializationMode SerMode, byte[] bytesObjRegionMasterDatatable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubRegionCode_LIST(SerializationMode SerMode, byte[] bytesObjRegionCodeListDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubRegionName(SerializationMode SerMode, byte[] bytesObjRegionNameDataTable_SERLAY);
        #endregion

        #region Branch
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryBranch(SerializationMode SerMode, byte[] bytesObjBranchMasterDatatable_SERLAY, out int intTotalRecords, PagingValues ObjPaging);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryBranch_List(SerializationMode SerMode, byte[] bytesObjBranchMasterDatatable_SERLAY);
        

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubBranch_List(SerializationMode SerMode, byte[] bytesObjBranchListDataTable_SERLAY);
        #endregion

        #region Workflow

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateWorkflow(SerializationMode SerMode, byte[] bytesObjWorkflowMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryWorkflow(SerializationMode SerMode, byte[] bytesObjWorkflowMasterDatatable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        string FunPubGetCompanyCode(int intCompanyID,  string strCompanyCode);

        #endregion

        #region "Hierachy Master"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateHierachyMasterDetails(SerializationMode SerMode, byte[] bytesObjS3G_Sys_HierachyMAsterDataTable);     
        #endregion

        #region "Manual Lookup Master"
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateManualLookup(SerializationMode SerMode, byte[] bytesObjManualLookupDataTable_SERLAY);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyManualLookup(SerializationMode SerMode, byte[] bytesObjManualLookupDataTable_SERLAY);

        #endregion

   

    }

    #region For Fault Contract
    [DataContract]
    public class ClsPubFaultException
    {
        private string strReason;

        [DataMember]
        public string ProReasonRW
        {
            get { return strReason; }
            set { strReason = value; }
        }
    }
    #endregion
}
