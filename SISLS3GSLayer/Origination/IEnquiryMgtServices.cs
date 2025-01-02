using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;

namespace S3GServiceLayer.Origination
{
    // NOTE: If you change the interface name "IEnquiry" here,  you must also update the reference to "IEnquiry" in Web.config.
    [ServiceContract]
    public interface IEnquiryMgtServices
    {
        #region Enquiry Updation

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPriGetDocumentNumberControlEnquiryNoDetails(int CompanyID, string docTypeDesc, int LOB_ID, int Branch_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryPinCodeGlobalParamListEnquiryUpdation(int intCompanyId, SerializationMode SerMode, byte[] bytesObjAssetMasterDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubInsertEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService, out string strEntityCode);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryExistCustomerListEnquiryUpdation(int intCustomerID, int intCompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryUpdation(int intEnquiryUpdationID, int intCompanyID);
        #endregion

        #region Enquiry Assignment

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        void FunPubInsertRouter(SerializationMode SMode, byte[] byteEnquiryService);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        void FunPubUpdateRouter(SerializationMode SMode, byte[] byteEnquiryService);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryUpdateDetails(string EnquiryNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetEnquiryTOAssign();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRouterDetails(string EnquiryNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetEnquiryUpdateAssign(string StartDate, string EndDate);


        #endregion

        #region Company Hierarchy

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetUserBranchList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetUserLOBList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetLOBProductList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetWorkFlowList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity);

        #endregion

        #region Enquiry or Customer Appraisal Details


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryDetails(int intCompanyID, int intUserID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryPendingDoc(int intCompany_ID, int Type,int ID);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryDetailsByEnquiryNo(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataSet FunPubPendingCustomerDetails(int intCustomerID, int intCompany_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubPendingCustomer(int intCompany_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        DataTable FunPubGetEnquiryDetailsModify();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubAppraisedCustomerDetail();


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY, bool EnquiryORCustomer);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubModifyEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubCustomerForUpdate(int intEnqAppID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubEnquiryForUpdate(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubQueryEnquiryResponseID(int intEnquiryID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubEnquiryDocumentForUpdate(int intEnqAppID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubCustomerDocumentForUpdate(int intEnquiryAppID);


        #endregion

        //#region  Enquiry Response

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //Int32 FunPub_Insert_EnquiryResponse(S3GBusEntity.EnquiryResponse.S3G_ORG_EnquiryResponse ObjEResponse);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_ROI(S3GBusEntity.EnquiryResponse.Offer_ROI_Details ObjEResponse);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_PaymentRuleCard(S3GBusEntity.EnquiryResponse.Offer_PaymentRuleCard ObjEResponse);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_OfferDetails_CashFlow(S3GBusEntity.EnquiryResponse.S3G_ORG_CashFlow ObjEResponse);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_RepaymentDetails(S3GBusEntity.EnquiryResponse.S3G_ORG_Repayment ObjEResponse);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_Alerts(S3GBusEntity.ApplicationProcess.AlertDetails ObjApp);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //Int32 FunPub_Insert_Follow_Header(S3GBusEntity.ApplicationProcess.FollowUp_Header ObjApp);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //void FunPub_Insert_Follow_Details(S3GBusEntity.ApplicationProcess.FollowUp_Detail ObjApp, bool DoCommit);


        //#endregion


        #region  Enquiry Response

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        Int32 FunPubInsertEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity, out string strEnquiryResponse);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        Int32 FunPubModifyEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryResponse(int intEnquiryResponseId);
        #endregion
    }
}

