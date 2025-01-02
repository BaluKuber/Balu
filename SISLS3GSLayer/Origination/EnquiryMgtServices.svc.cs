using System;
using System.ServiceModel;
using S3GBusEntity;
using System.Data;
using S3GDALayer;
using S3GDALayer.Origination;
using S3GServiceLayer.Origination;
using System.ServiceModel.Activation;


namespace S3GServiceLayer
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "EnquiryMgtServices" here, you must also update the reference to "EnquiryMgtServices" in Web.config.
    public class EnquiryMgtServices : IEnquiryMgtServices
    {
        byte[] bytesDataTable;




        #region Enquiry Updation Methods

        public byte[] FunPriGetDocumentNumberControlEnquiryNoDetails(int CompanyID, string docTypeDesc, int LOB_ID, int Branch_ID)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_DocumentNumberControlDataTable dtDocControl;

                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation objEnquiryUpdation = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
                dtDocControl = objEnquiryUpdation.FunPriGetDocumentNumberControlEnquiryNoDetails(CompanyID, docTypeDesc, LOB_ID, Branch_ID);
                bytesDataTable = ClsPubSerialize.Serialize(dtDocControl, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Enuiry Doc Control:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;

        }

        public byte[] FunPubQueryExistCustomerListEnquiryUpdation(int intCustomerID, int intCompanyID)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_ExistCustomerMasterDataTable dtExist;

                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation objEnquiryUpdation = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
                dtExist = objEnquiryUpdation.FunPubQueryExistCustomerListEnquiryUpdation(intCustomerID, intCompanyID);
                bytesDataTable = ClsPubSerialize.Serialize(dtExist, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Enquiry Existing Customer List:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;

        }
        public byte[] FunPubQueryEnquiryUpdation(int intEnquiryUpdationID, int intCompanyID)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryUpdationDataTable dtEnquiryUpdation;

                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation objEnquiryUpdation = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
                dtEnquiryUpdation = objEnquiryUpdation.FunPubQueryEnquiryUpdation(intEnquiryUpdationID, intCompanyID);
                bytesDataTable = ClsPubSerialize.Serialize(dtEnquiryUpdation, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Query the Enquiry Updation:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;

        }
        public byte[] FunPubQueryPinCodeGlobalParamListEnquiryUpdation(int intCompanyId, SerializationMode SerMode, byte[] bytesObjGlobalParamPincodeMasterDataTable_SERLAY)
        {

            S3GBusEntity.Origination.EnquiryService.S3G_SYSAD_GlobalParameterSetupDataTable dtGlobalParamPincodeMaster;
            S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjGloabalParamPincodeMaster = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
            dtGlobalParamPincodeMaster = ObjGloabalParamPincodeMaster.FunPubQueryPinCodeGlobalParamListEnquiryUpdation(intCompanyId, SerMode, bytesObjGlobalParamPincodeMasterDataTable_SERLAY);
            byte[] bytesGlobalParamPincodeMaster = ClsPubSerialize.Serialize(dtGlobalParamPincodeMaster, SerializationMode.Binary);
            return bytesGlobalParamPincodeMaster;
        }

        public int FunPubInsertEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService, out string strEntityCode)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjClsPubEnquiryUpdation = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
                return ObjClsPubEnquiryUpdation.FunPubInsertEnquiryUpdation(SMode, byteEnquiryService, out strEntityCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FunPubModifyEnquiryUpdation(SerializationMode SMode, byte[] byteEnquiryService)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjClsPubEnquiryUpdation = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
                return ObjClsPubEnquiryUpdation.FunPubModifyEnquiryUpdation(SMode, byteEnquiryService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Enquiry Assignment Methods

        /// <summary>
        /// To use Insert record into Router
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass bype object</param>
        public void FunPubInsertRouter(SerializationMode SMode, byte[] byteEnquiryService)
        {
            try
            {

                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjClsPubEnquiryAssignment = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();
                ObjClsPubEnquiryAssignment.FunPubInsertRouter(SMode, byteEnquiryService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To use Update record into Router
        /// </summary>
        /// <param name="SMode">Pass SerializationMode</param>
        /// <param name="byteEnquiryService">Pass byte Object</param>
        public void FunPubUpdateRouter(SerializationMode SMode, byte[] byteEnquiryService)
        {
            try
            {

                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjClsPubEnquiryAssignment = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();
                ObjClsPubEnquiryAssignment.FunPubUpdateRouter(SMode, byteEnquiryService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get Enquiry Update details
        /// </summary>
        /// <param name="EnquiryNo">Pass Enquiry No</param>
        /// <returns></returns>
        public byte[] FunPubGetEnquiryUpdateDetails(string EnquiryNo)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_DV_GetEnquiryUpdateDetailsDataTable ObjEnquiryDetails;
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjClsPubEnquiryAssignment = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();
                ObjEnquiryDetails = ObjClsPubEnquiryAssignment.FunPubGetEnquiryUpdateDetails(EnquiryNo);
                byte[] byteEnquiryDetails = ClsPubSerialize.Serialize(ObjEnquiryDetails, SerializationMode.Binary);
                return byteEnquiryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get User Branch List
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass CompanyHierarchyEntity Object</param>
        /// <returns>Return Object is Datatable</returns>
        public DataTable FunPubGetUserBranchList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy ObjClsPubCompanyHierarchy
                    = new S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy();

                return ObjClsPubCompanyHierarchy.FunPubGetUserBranchList(ObjCompanyHierarchyEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To Get User LOB List 
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass CompanyHierarchyEntity Object</param>
        /// <returns>Return Object is Datatable</returns>
        public DataTable FunPubGetUserLOBList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy ObjClsPubCompanyHierarchy
                    = new S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy();

                return ObjClsPubCompanyHierarchy.FunPubGetUserLOBList(ObjCompanyHierarchyEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To Get Product List for Particular LOB
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass CompanyHierarchyEntity Object</param>
        /// <returns>Return Datatable</returns>
        public DataTable FunPubGetLOBProductList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy ObjClsPubCompanyHierarchy
                    = new S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy();

                return ObjClsPubCompanyHierarchy.FunPubGetLOBProductList(ObjCompanyHierarchyEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To Get workflow List for particular Product
        /// </summary>
        /// <param name="ObjCompanyHierarchyEntity">Pass CompanyHierarchyEntity Object</param>
        /// <returns>Return Datatable</returns>
        public DataTable FunPubGetWorkFlowList(S3GBusEntity.CompanyHierarchyEntity ObjCompanyHierarchyEntity)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy ObjClsPubCompanyHierarchy
                    = new S3GDALayer.EnquiryMgtServices.ClsPubCompanyHierarchy();

                return ObjClsPubCompanyHierarchy.FunPubGetWorkFlowList(ObjCompanyHierarchyEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get Enquiry Update details to Assign (No need Arguments)
        /// </summary>
        /// <returns>Return Datatable</returns>
        public DataTable FunPubGetEnquiryTOAssign()
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjEnquiryTOAssign
                    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();

                return ObjEnquiryTOAssign.FunPubGetEnquiryTOAssign();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get Router Details
        /// </summary>
        /// <param name="EnquiryNo">Pass Enquiry No.</param>
        /// <returns>Return Object is byte[]</returns>
        public byte[] FunPubGetRouterDetails(string EnquiryNo)
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_RouterDetailsDataTable ObjEnquiryDetails;
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjClsPubEnquiryAssignment = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();
                ObjEnquiryDetails = ObjClsPubEnquiryAssignment.FunPubGetRouterDetails(EnquiryNo);
                byte[] byteEnquiryDetails = ClsPubSerialize.Serialize(ObjEnquiryDetails, SerializationMode.Binary);
                return byteEnquiryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Get Enquiry Update details and Assigned
        /// </summary>
        /// <param name="StartDate">Pass Start date</param>
        /// <param name="EndDate">Pass End date</param>
        /// <returns>Return Object is datatable</returns>
        public DataTable FunPubGetEnquiryUpdateAssign(string StartDate, string EndDate)
        {
            try
            {
                S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment ObjEnquiryTOAssign
                    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryAssignment();

                return ObjEnquiryTOAssign.FunPubGetEnquiryUpdateAssign(StartDate, EndDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Enquiry Response

        //public void FunPubInsert_S3G_ORG_EnquiryResponseOfferDetails(S3GBusEntity.S3G_ORG_EnquiryResponseOfferDetails ObjS3G_ORG_EnquiryResponseOfferDetails)
        //{
        //    try
        //    {
        //        //S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //        //    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();

        //        //ObjEnquiryTOResponse.FunPubInsert_S3G_ORG_EnquiryResponseOfferDetails(ObjS3G_ORG_EnquiryResponseOfferDetails);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        //public void FunPubInsert_S3G_ORG_EnquiryResponseAlertDetails(S3GBusEntity.S3G_ORG_EnquiryResponseAlertDetails ObjS3G_ORG_EnquiryResponseAlertDetails)
        //{
        //    try
        //    {
        //        //    S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //        //        = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();

        //        //    ObjEnquiryTOResponse.FunPubInsert_S3G_ORG_EnquiryResponseAlertDetails(ObjS3G_ORG_EnquiryResponseAlertDetails);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}








        //public void FunPubInsert_S3G_ORG_EnquiryResponseRepaymentDetails(S3GBusEntity.S3G_ORG_EnquiryResponseRepayTermsDetails ObjS3G_ORG_EnquiryResponseRepayTermsDetails)
        //{
        //    try
        //    {
        //        //S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //        //    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();

        //        //ObjEnquiryTOResponse.FunPubInsert_S3G_ORG_EnquiryResponseRepaymentDetails(ObjS3G_ORG_EnquiryResponseRepayTermsDetails);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}




        //public void FunPubInsert_S3G_ORG_EnquiryResponseFollowUpDetails(S3GBusEntity.S3G_ORG_EnquiryResponseFollowUpDetails ObjS3G_ORG_EnquiryResponseFollowUpDetails)
        //{
        //    try
        //    {
        //        //S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //        //    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();

        //        //ObjEnquiryTOResponse.FunPubInsert_S3G_ORG_EnquiryResponseFollowUpDetails(ObjS3G_ORG_EnquiryResponseFollowUpDetails);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}




        //public void FunPubInsert_S3G_ORG_EnquiryResponseDetails(S3GBusEntity.S3G_ORG_EnquiryResponse ObjS3G_ORG_EnquiryResponse)
        //{
        //    try
        //    {
        //        //S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //        //    = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();

        //        //ObjEnquiryTOResponse.FunPubInsert_S3G_ORG_EnquiryResponseDetails(ObjS3G_ORG_EnquiryResponse);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}




        //public void FunPub_Insert_ROI(S3GBusEntity.Offer_ROI_Details ObjApp)
        //{
        //    try
        //    {

        //        S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //            = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
        //        ObjEnquiryTOResponse.FunPub_Insert_ROI(ObjApp);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        //public Int32 FunPub_Insert_Follow_Header(S3GBusEntity.FollowUp_Header ObjApp)
        //{
        //    try
        //    {

        //        S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //         = new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
        //        return ObjEnquiryTOResponse.FunPub_Insert_Follow_Header(ObjApp);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_Follow_Details(S3GBusEntity.FollowUp_Detail ObjApp)
        //{
        //    try
        //    {

        //        S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation ObjEnquiryTOResponse
        //= new S3GDALayer.EnquiryMgtServices.ClsPubEnquiryUpdation();
        //        ObjEnquiryTOResponse.FunPub_Insert_Follow_Details(ObjApp);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region Enquiry and Customer Appraisal  Members

        // ----------Get Enquiry Details----------------------------

        #region Query

        public byte[] FunPubGetEnquiryDetails(int intCompanyID, int intUserID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubQueryEnquiryDetails(intCompanyID, intUserID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }

        public DataTable FunPubGetEnquiryDetailsModify()
        {
            try
            {
                return new DataTable();
                //-----Commented by Prabhu with help of Prakash----///
                //S3GDALayer.Origination.EnquiryService.ClsPubEnquiryup ObjCustomerAppraisal
                //    = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();

                //return ObjCustomerAppraisal.FunPubGetEnquiryDetailsModify();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //--------Get Enquiry Doc Details

        public byte[] FunPubGetEnquiryPendingDoc(int intCompany_ID, int Type,int ID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryPendingDocDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubQueryEnquiryPendingDoc(intCompany_ID, Type, ID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }


        //-----------Get Enquiry Details By Enquiry No--------------------------------

        //public byte[] FunPubGetEnquiryDetailsByEnquiryNo(int intCompanyID, int intEnquiryNo)
        public byte[] FunPubGetEnquiryDetailsByEnquiryNo(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubQueryEnquiryDetailsByEnquiryNo(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }


        public DataSet FunPubPendingCustomerDetails(int intCustomerID,int Company_ID)
        {
            try
            {
                S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjCustomerAppraisal
                    = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();

                return ObjCustomerAppraisal.FunPubPendingCustomerDetails(intCustomerID, Company_ID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FunPubPendingCustomer(int Compnay_ID)
        {
            try
            {
                S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjCustomerAppraisal
                    = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
                return ObjCustomerAppraisal.FunPubPendingCustomer(Compnay_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] FunPubAppraisedCustomerDetail()
        {
            try
            {
                S3GBusEntity.Origination.EnquiryService.S3G_ORG_Customer_Appraisal_DetailsDataTable dtCustomerAppraised;
                S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
                dtCustomerAppraised = (S3GBusEntity.Origination.EnquiryService.S3G_ORG_Customer_Appraisal_DetailsDataTable)ObjEnquiryAppraisal.FunPubAppraisedCustomerDetail();
                bytesDataTable = ClsPubSerialize.Serialize(dtCustomerAppraised, SerializationMode.Binary);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public byte[] FunPubCustomerForUpdate(int intEnqAppID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubCustomerForUpdate(intEnqAppID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }

        public byte[] FunPubEnquiryForUpdate(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubEnquiryForUpdate(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }

        public byte[] FunPubEnquiryDocumentForUpdate(int intEnqAppID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubEnquiryDocumentForUpdate(intEnqAppID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }


        public byte[] FunPubCustomerDocumentForUpdate(int intEnquiryAppID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubCustomerDocumentForUpdate(intEnquiryAppID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }

        //-----------Get Customer Details By Customer No--------------------------------

        //public byte[] FunPubGetCustomerDetailsByCustomerNo(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        //{
        //    S3GBusEntity.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
        //    S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();

        //    dtEnquiry = ObjEnquiryAppraisal.FunPubQueryCustomerDetailsByCustomerNo(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
        //    byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
        //    return bytesCurrencyMaster;

        //}
        #endregion

        //#region Insert

        ////-----------Insert Enquiry  Appraisal Details

        public int FunPubCreateEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY, bool EnquiryORCustomer)
        {
            try
            {

                S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
                return ObjEnquiryAppraisal.FunPubCreateEnquiryAppraisal(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY, EnquiryORCustomer);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }

        }

        ////-----------To get the Enquiry response ID


        public byte[] FunPubQueryEnquiryResponseID(int intEnquiryID)
        {
            S3GBusEntity.Origination.EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable dtEnquiry;
            S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
            dtEnquiry = ObjEnquiryAppraisal.FunPubQueryEnquiryResponseID(intEnquiryID);
            byte[] bytesCurrencyMaster = ClsPubSerialize.Serialize(dtEnquiry, SerializationMode.Binary);
            return bytesCurrencyMaster;
        }



        ////-----------Insert Customer Appraisal Details

        //public int FunPubCreateCustomerAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        //{
        //    try
        //    {

        //        S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
        //        return ObjEnquiryAppraisal.FunPubCreateEnquiryAppraisal(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }

        //}
        //#endregion

        //#region Modify


        ////---------------Modify Enquiry Appraisal Details

        public int FunPubModifyEnquiryAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        {
            try
            {

                S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal ObjEnquiryAppraisal = new S3GDALayer.Origination.EnquiryService.ClsPubEnquiryCustomerAppraisal();
                return ObjEnquiryAppraisal.FunPubModifyEnquiryAppraisal(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        ////-------------Modify Customer Appraisal Details

        //public int FunPubModifyCustomerAppraisal(SerializationMode SerMode, byte[] bytesObjEnquiryCustomerDataTable_SERLAY)
        //{
        //    try
        //    {
        //        S3GDALayer.UserMgtServices.ClsRoleCenterMaster ObjRoleCenterMaster = new S3GDALayer.UserMgtServices.ClsRoleCenterMaster();
        //        return ObjRoleCenterMaster.FunPubModifyRoleCodeMaster(SerMode, bytesObjEnquiryCustomerDataTable_SERLAY);
        //    }
        //    catch (Exception objExp)
        //    {
        //        ClsPubFaultException objFault = new ClsPubFaultException();
        //        objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
        //    }
        //}

        //#endregion
        #endregion

        //#region  Enquiry Response

        //public Int32 FunPub_Insert_EnquiryResponse(S3GBusEntity.EnquiryResponse.S3G_ORG_EnquiryResponse ObjEResponse)
        //{
        //    try
        //    {
        //        return ObjEnqResponseSave.FunPub_Insert_EnquiryResponse(ObjEResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_ROI(S3GBusEntity.EnquiryResponse.Offer_ROI_Details ObjEResponse)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_ROI(ObjEResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_PaymentRuleCard(S3GBusEntity.EnquiryResponse.Offer_PaymentRuleCard ObjEResponse)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_PaymentRuleCard(ObjEResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_OfferDetails_CashFlow(S3GBusEntity.EnquiryResponse.S3G_ORG_CashFlow ObjEResponse)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_OfferDetails_CashFlow(ObjEResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_RepaymentDetails(S3GBusEntity.EnquiryResponse.S3G_ORG_Repayment ObjEResponse)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_RepaymentDetails(ObjEResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_Alerts(S3GBusEntity.ApplicationProcess.AlertDetails ObjApp)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_Alerts(ObjApp);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public Int32 FunPub_Insert_Follow_Header(S3GBusEntity.ApplicationProcess.FollowUp_Header ObjApp)
        //{
        //    try
        //    {
        //        return ObjEnqResponseSave.FunPub_Insert_Follow_Header(ObjApp);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void FunPub_Insert_Follow_Details(S3GBusEntity.ApplicationProcess.FollowUp_Detail ObjApp, bool DoCommit)
        //{
        //    try
        //    {
        //        ObjEnqResponseSave.FunPub_Insert_Follow_Details(ObjApp, DoCommit);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //#endregion


        #region  Enquiry Response

        public Int32 FunPubInsertEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity, out string strEnquiryResponse)
        {
            try
            {
                S3GDALayer.Origination.ClsPubEnquiryResponse ObjEnquiryResponse = new ClsPubEnquiryResponse();
                return ObjEnquiryResponse.FunPubInsertEnquiryResponse(objEnquiryResponseEntity, out strEnquiryResponse);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        public Int32 FunPubModifyEnquiryResponse(S3GBusEntity.EnquiryResponse.EnquiryResponseEntity objEnquiryResponseEntity)
        {
            try
            {
                S3GDALayer.Origination.ClsPubEnquiryResponse ObjEnquiryResponse = new ClsPubEnquiryResponse();
                return ObjEnquiryResponse.FunPubModifyEnquiryResponse(objEnquiryResponseEntity);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
        }

        public byte[] FunPubGetEnquiryResponse(int intEnquiryResponseId)
        {
            DataSet ds;
            try
            {
                ClsPubEnquiryResponse objEnquiryResponse = new ClsPubEnquiryResponse();
                ds = (DataSet)objEnquiryResponse.FunPubGetEnquiryResponse(intEnquiryResponseId);
                bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLog.CustomErrorRoutine(ex);
                throw ex;
            }
            return bytesDataTable;
        }


        #endregion
    }
}
