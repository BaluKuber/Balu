using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.LegalAndRepossession
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "LegalAndRepossessionMgtServices" here, you must also update the reference to "LegalAndRepossessionMgtServices" in Web.config.
    public class LegalAndRepossessionMgtServices : ILegalAndRepossessionMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;

        byte[] bytesDataTable;
        #region ILegalAndRepossessionMgtServices Members

        public int FunPubCreateLegalRepossessionApprovals(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg)
        {
            try
            {

                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalRepossessionApproval objLRApproval = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalRepossessionApproval();
                return objLRApproval.FunPubCreateLegalRepossessionApprovals(SerMode, bytesObjApprovalDatatable_SERLAY, out strErrMsg);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }


        #region LegalHearingTrack

        public int FunPubCreateLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable, out string strLHRNo)
        {
            try
            {

                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalHearingTrack objLegalHearingTrack = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalHearingTrack();
                return objLegalHearingTrack.FunPubCreateLegalHearingTrack(SerMode, byteobjS3G_LR_LegalHearingTrackDataTable, out strLHRNo);


            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public int FunPubModifyLegalHearingTrack(SerializationMode SerMode, byte[] byteobjS3G_LR_LegalHearingTrackDataTable)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalHearingTrack objLegalHearingTrack = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubLegalHearingTrack();
                return objLegalHearingTrack.FunPubModifyLegalHearingTrack(SerMode, byteobjS3G_LR_LegalHearingTrackDataTable);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        #endregion

        #endregion

        #region Repossession Release

        public int FunPubCreateRepoRelease(SerializationMode SerMode, byte[] byteobjS3G_LR_RepoReleaseTable, out string strRepoReleaseNo, string[] strCompanyLOBBranch)
        {
            try
            {

                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionRelease objRepoRelease = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionRelease();
                return objRepoRelease.FunPubCreateRepoRelease(SerMode, byteobjS3G_LR_RepoReleaseTable, out strRepoReleaseNo, strCompanyLOBBranch);


            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Repossession Track
        public int FunPubCreateLegalRepossessionTrack(SerializationMode SerMode, byte[] bytesTrack_Datatable, out string strErrMsg)
        {
            try
            {

                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionTrack objReposTrack = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionTrack();
                return objReposTrack.FunPubCreateLegalRepossessionTrack(SerMode, bytesTrack_Datatable, out strErrMsg);


            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion


        #region Repossession Notice
        public int FunPubCreateRepossessionNotice(SerializationMode SerMode, byte[] bytesRepossessionNoticeDataTable, out string strRN_No, out int strRN_ID)
        {
            try
            {
                //S3GDALayer .LegalRepossession .LegalAndRepossessionMgtServices.
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNotice objReposNotice = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNotice();
                return objReposNotice.FunPubCreateRepossessionNotice(SerMode, bytesRepossessionNoticeDataTable, out strRN_No, out strRN_ID);


            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Garage Master
        public int FunPubCreateGarageMaster(SerializationMode SerMode, byte[] bytesObjGarageMasterDAL_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster objLegalMgtServices = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster();
                return objLegalMgtServices.FunPubGarageMasterIns(SerializationMode.Binary, bytesObjGarageMasterDAL_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunGetGarageDetailsMod(int CompanyID, int GarageID)
        { 
            try
            {
            DataSet dsGarage = new DataSet();
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster objLegalMgtServices = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster();
            dsGarage = objLegalMgtServices.FunGarageMasterforModify(GarageID, CompanyID);
            bytesDataTable = ClsPubSerialize.Serialize(dsGarage, SerializationMode.Binary);
            return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunGetAccount(int CompanyID, int GarageID, int Mode)
        {
            try
            {
            DataSet dsGarage = new DataSet();
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster objLegalMgtServices = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster();
            dsGarage = objLegalMgtServices.FunGarageAccountDetails(GarageID, CompanyID, Mode);
            bytesDataTable = ClsPubSerialize.Serialize(dsGarage, SerializationMode.Binary);
            return bytesDataTable;
             }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunGetgarageMaster(int CompanyID)
        {
            try
            {
                DataSet dsGarage = new DataSet();
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster objLegalMgtServices = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubGarageMaster();
                dsGarage = objLegalMgtServices.FunGetGarageMaster(CompanyID);
                bytesDataTable = ClsPubSerialize.Serialize(dsGarage, SerializationMode.Binary);
                return bytesDataTable;
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region "LEGAL REPOSSESSION NOTE"
        public int FunPubCreateRepossessionNote(SerializationMode SerMode, byte[] bytesObjRepossessionNoteDataTable_SERLAY, out string LRNum)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote  objRepossessionDetails = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
                return objRepossessionDetails.FunPubCreateReposissionNote(SerMode, bytesObjRepossessionNoteDataTable_SERLAY, out LRNum);
            }
            catch (Exception objEx)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in RepossessionNote :" + objEx.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        
        public byte[] FunPubQueryLRNDetails(int? intCompany_Id, int? intEntity_id)
        {
            DataSet dsLRNDetails = null;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote objLRNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
                dsLRNDetails = objLRNote.FunPubQueryLRNDetails(intCompany_Id, intEntity_id);
                bytesDataTable = ClsPubSerialize.Serialize(dsLRNDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Querying Entity Details :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubGetMappedLRNDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
        {
            DataTable dtPanCust = new DataTable();
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote objClsPubRepossessionNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
            dtPanCust = objClsPubRepossessionNote.FunPubGetMappedLRNDetails(intCompanyId, intLobId, intBranchId, strPANum, strSANum);
            bytesDataTable = ClsPubSerialize.Serialize(dtPanCust, SerializationMode.Binary);
            return bytesDataTable;
        }
        public byte[] FunPubGetMappedPASADetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
        {
            DataTable dtPanCust = new DataTable();
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote objClsPubRepossessionNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
            dtPanCust = objClsPubRepossessionNote.FunPubGetMappedPASADetails(intCompanyId, intLobId, intBranchId, strPANum, strSANum);
            bytesDataTable = ClsPubSerialize.Serialize(dtPanCust, SerializationMode.Binary);
            return bytesDataTable;
        }
        public int FunPubGetMappedAccountAssetDetails(int intCompanyId, int intLobId, int intBranchId, string strPANum, string strSANum)
        {
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote objClsPubRepossessionNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
            return objClsPubRepossessionNote.FunPubGetMappedAccountAssetDetails(intCompanyId, intLobId, intBranchId, strPANum, strSANum);
        }
        public byte[] FunGetPANumCustomer(string strPANum)
        {
            DataTable dtPANumCustomer = new DataTable();
            S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote objClsPubRepossessionNote = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossessionNote();
            dtPANumCustomer = objClsPubRepossessionNote.FunGetPANumCustomer(strPANum);
            bytesDataTable = ClsPubSerialize.Serialize(dtPANumCustomer, SerializationMode.Binary);
            return bytesDataTable;
        }
        #endregion

        #region Sale Notification
        //--------------Sale Notification( Created By -Irsathameen K  Created On - 28-APR-2011)---------------

        //Begin
        public int FunPubCreateSaleNotificationDetails(SerializationMode SerMode, byte[] bytesObjS3G_LR_SaleNotificationDataTable_SERLAY, out string strSNNo)
        {
            try
            {

                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSaleNotification ObjSaleNotification = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSaleNotification();
                return ObjSaleNotification.FunPubCreateSaleNotificationDetails(SerMode, bytesObjS3G_LR_SaleNotificationDataTable_SERLAY, out strSNNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        //End
        #endregion

        #region Sales Bid Entry
        public int FunPubInsertSalesBidEntry(SerializationMode SerMode, byte[] byteObjSalesBidDAL_SERLAY, out string DSN)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry objSalesBidEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry();
                return objSalesBidEntry.FunPubInsertSalesBidEntry(SerializationMode.Binary, byteObjSalesBidDAL_SERLAY, out DSN);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
        public int FunPubUpdateSalesBidEntry(SerializationMode SerMode, byte[] byteObjSalesBidDAL_SERLAY)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry objSalesBidEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry();
                return objSalesBidEntry.FunPubUpdateSalesBidEntry(SerializationMode.Binary, byteObjSalesBidDAL_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubGetSalesBidDetails(int CompanyID, int SalesBidID)
        {
            DataSet dsSaleBidDetails;
            byte[] bytedsSaleBidDetails;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry objSalesBidEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesBidEntry();
                dsSaleBidDetails = objSalesBidEntry.FunGetSalesBidDetails(CompanyID, SalesBidID);
                bytedsSaleBidDetails = ClsPubSerialize.Serialize(dsSaleBidDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));

            }
            return bytedsSaleBidDetails;
        }
        #endregion

        #region Repossession
        public int FunCreateRepossesion(SerializationMode SerMode, byte[] bytesRepo_Datatable, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossession ObjRep = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubRepossession();
                return ObjRep.FunCreateRepossesion(SerMode, bytesRepo_Datatable, out strErrMsg);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion repossion

        #region Garage Maintenance
        public int FunCreateMaintenance(SerializationMode SerMode, byte[] bytesMaint_Datatable, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubMaintenance ObjMaint = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubMaintenance();
                return ObjMaint.FunCreateMaintenance(SerMode, bytesMaint_Datatable, out strErrMsg);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion repossion

        #region Sales Invoice Entry
        public int FunPubInsertSalesInvoiceEntry(SerializationMode SerMode, byte[] byteObjSalesInvDAL_SERLAY, out string strSalesInvNo)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                return objSalesInvEntry.FunPubInsertSalesInvoiceEntry(SerializationMode.Binary, byteObjSalesInvDAL_SERLAY, out strSalesInvNo);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }
        public int FunPubUpdateSalesInvoiceEntry(SerializationMode SerMode, byte[] byteObjSalesInvDAL_SERLAY)
        {
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                return objSalesInvEntry.FunPubUpdateSalesInvoiceEntry(SerializationMode.Binary, byteObjSalesInvDAL_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubGetSalesInvoiceDetails(int CompanyID, int SalesInvID)
        {
            DataSet dsSaleInvDetails;
            byte[] bytedsSaleInvDetails;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                dsSaleInvDetails = objSalesInvEntry.FunGetSalesInvoiceDetails(CompanyID, SalesInvID);
                bytedsSaleInvDetails = ClsPubSerialize.Serialize(dsSaleInvDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));

            }
            return bytedsSaleInvDetails;
        }

        public byte[] FunPubGetSaleBidNotificationDetails(int CompanyID, int BranchID, int LOBID, int SalesNotificationID, out int SaleBidID)
        {
            DataSet dsSaleBidNotificationDetails;
            byte[] bytedsSaleBidNotificationDetails;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                dsSaleBidNotificationDetails = objSalesInvEntry.FunGetSaleBidNotificationDetails(CompanyID, BranchID, LOBID, SalesNotificationID, out SaleBidID);
                bytedsSaleBidNotificationDetails = ClsPubSerialize.Serialize(dsSaleBidNotificationDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));

            }
            return bytedsSaleBidNotificationDetails;
        }

        public byte[] FunPubGetSaleDebCredGLCode(int CompanyID, string AccCode, int Mode)
        {
            DataSet dsSaleDebCredGLCode;
            byte[] bytedsSaleDebCredGLCode;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                dsSaleDebCredGLCode = objSalesInvEntry.FunGetSaleDebCredGLCode(CompanyID, AccCode, Mode);
                bytedsSaleDebCredGLCode = ClsPubSerialize.Serialize(dsSaleDebCredGLCode, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));

            }
            return bytedsSaleDebCredGLCode;
        }
        public byte[] FunPubGetSaleDebCredSLCode(int CompanyID, string GLCode, int LOBID, int BranchID)
        {
            DataSet dsSaleDebCredSLCode;
            byte[] bytedsSaleDebCredSLCode;
            try
            {
                S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry objSalesInvEntry = new S3GDALayer.LegalRepossession.LegalAndRepossessionMgtServices.ClsPubSalesInvoiceEntry();
                dsSaleDebCredSLCode = objSalesInvEntry.FunGetSaleDebCredSLCode(CompanyID, GLCode, LOBID, BranchID);
                bytedsSaleDebCredSLCode = ClsPubSerialize.Serialize(dsSaleDebCredSLCode, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));

            }
            return bytedsSaleDebCredSLCode;

        }
        #endregion
    }
}