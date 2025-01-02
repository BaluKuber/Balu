using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.LegalAndRepossession
{
    // NOTE: If you change the interface name "ILegalAndRepossessionMgtServices" here, you must also update the reference to "ILegalAndRepossessionMgtServices" in Web.config.
    [ServiceContract]
    public interface ILegalAndRepossessionMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        //int FunPubCreateApprovals(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg);
        int FunPubCreateLegalRepossessionApprovals(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg);
       
        #region LegalHearinTrack

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]

        int FunPubCreateLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable, out string strLHRNo);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable);
        
        #endregion

        #region Repossession Release
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRepoRelease(SerializationMode SerMode, byte[] byteobjS3G_LR_RepoReleaseTable, out string strRepoReleaseNo,string[] strCompanyLOBBranch);
        #endregion

        #region Repossession Track

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateLegalRepossessionTrack(SerializationMode SerMode, byte[] bytesTrack_Datatable, out string strErrMsg);
        
        #endregion

        #region Repossession
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunCreateRepossesion(SerializationMode SerMode, byte[] bytesRepo_Datatable, out string strErrMsg);
        #endregion

        #region Garage Maintenance
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunCreateMaintenance(SerializationMode SerMode, byte[] bytesMaint_Datatable, out string strErrMsg);
        #endregion

        #region Repossession Notice

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRepossessionNotice(SerializationMode SerMode, byte[] bytesRepossessionNoticeDataTable, out string strRN_No, out int strRN_ID);

        #endregion

        #region "Garage Master"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateGarageMaster(SerializationMode SerMode, byte[] bytesObjGarageMasterDAL_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetGarageDetailsMod(int CompanyID, int GarageID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetAccount(int CompanyID, int GarageID, int Mode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetgarageMaster(int CompanyID);

        #endregion

        #region "LEGAL REPOSSESSION NOTE"

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRepossessionNote(SerializationMode SerMode, byte[] bytesObjRepossessionNoteDataTable_SERLAY, out string LRNum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunGetPANumCustomer(string strPANum);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryLRNDetails(int? intCompany_Id, int? intEntity_id);

    
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMappedLRNDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMappedPASADetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubGetMappedAccountAssetDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum);
               
        #endregion 

        #region Sale Notification 
        //--------------Sale Notification( Created By -Irsathameen K  Created On - 28-APR-2011)---------------

        //Begin
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
         int FunPubCreateSaleNotificationDetails(SerializationMode SerMode, byte[] bytesObjS3G_LR_SaleNotificationDataTable_SERLAY, out string strSNNo);

        //End
        #endregion

        #region Sales Bid Entry

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertSalesBidEntry(SerializationMode SerMode, byte[] byteObjSalesBidDAL_SERLAY, out string DSN);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateSalesBidEntry(SerializationMode SerMode, byte[] byteObjSalesBidDAL_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSalesBidDetails(int CompanyID, int SalesBidID);

        #endregion

        #region Sales Invoice Entry

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertSalesInvoiceEntry(SerializationMode SerMode, byte[] byteObjSalesInvDAL_SERLAY, out string strSalesInvNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubUpdateSalesInvoiceEntry(SerializationMode SerMode, byte[] byteObjSalesInvDAL_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSalesInvoiceDetails(int CompanyID, int SalesInvID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSaleBidNotificationDetails(int CompanyID, int BranchID, int LOBID, int SalesNotificationID, out int SaleBidID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSaleDebCredGLCode(int CompanyID, string AccCode, int Mode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSaleDebCredSLCode(int CompanyID, string GLCode, int LOBID, int BranchID);

        #endregion
    }
}
