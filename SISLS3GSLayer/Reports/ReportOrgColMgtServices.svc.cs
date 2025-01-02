using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;
using S3GDALayer.Reports;
using S3GBusEntity;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Reports
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ReportOrgColMgtServices" here, you must also update the reference to "ReportOrgColMgtServices" in Web.config.
    public class ReportOrgColMgtServices : IReportOrgColMgtServices
    {
        #region Credit Score Details
        public byte[] FunPubGetLOB(int CompanyId, int UserId, bool Is_Active)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
            dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, Is_Active);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubBranch(int CompanyId, int UserId, bool Is_Active)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
            dropDownLists = obj.FunPubBranch(CompanyId, UserId, Is_Active);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

        public byte[] FunPubGetProduct(int CompanyId, int LOBId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
            dropDownLists = obj.FunPubGetProduct(CompanyId, LOBId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

        public byte[] FunPubGetCreditScoreDetails(ClsPubCredit ObjCredit)
        {
            try
            {
                ClsPubCreditScoreTransaction CreditScoreDetails;
                ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
                CreditScoreDetails = obj.FunPubGetCreditScoreDetails(ObjCredit);
                return ClsPubSerialize.Serialize(CreditScoreDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

        public byte[] FunPubGetCustomersDetails(ClsPubCredit ObjCustomers)
        {
            try
            {
            List<ClsPubCustomersDetails> CustomersDetails;
            ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
            CustomersDetails = obj.FunPubGetCustomersDetails(ObjCustomers);
            return ClsPubSerialize.Serialize(CustomersDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetRejCustomersDetails(ClsPubCredit ObjRejCustomers)
        {
            try
            {
                List<ClsPubCustomersDetails> CustomersRejDetails;
                ClsPubRptCreditScoreTransaction obj = new ClsPubRptCreditScoreTransaction();
                CustomersRejDetails = obj.FunPubGetRejCustomersDetails(ObjRejCustomers);
                return ClsPubSerialize.Serialize(CustomersRejDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region PDC Line of Business
        public byte[] FunPubGetPDCLOB(int CompanyId, int UserId, int ProgramId, string CustomerId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
                dropDownLists = obj.FunPubGetPDCLOB(CompanyId, UserId, ProgramId, CustomerId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region PDC Prime Account Number
        public byte[] FunPubPDCAckPAN(ClsPubStockReceivableparameters PDCPAN)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
                dropDownLists = obj.FunPubPDCAckPAN(PDCPAN);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region PDC Sub Account Number
        public byte[] FunPubPDCAckSAN(int CompanyId, string PNum)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
                dropDownLists = obj.FunPubPDCAckSAN(CompanyId, PNum);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region PDC Number
        public byte[] FunPubPDCNumber(string PNum, string SNum)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
                dropDownLists = obj.FunPubPDCNumber(PNum, SNum);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        //#region PDC Date
        //public byte[] FunPubPDCDate(string PdcNo)
        //{
        //    try
        //    {
        //        List<ClsPubDropDownList> dropDownLists;
        //        ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
        //        dropDownLists = obj.FunPubPDCDate(PdcNo);
        //        return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}
        //#endregion

        #region Get LOB,Branch Details for PDC
        public byte[] FunPubGetHeaderLobBranchDetails(string PANum)
        {
            try
            {
                ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
                return ClsPubSerialize.Serialize(obj.FunPubGetHeaderLobBranchDetails(PANum), SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        //#region Get PDC Number
        //public byte[] FunPubGetPDCNO(byte[] ObjPDCBytes)
        //{
        //    try
        //    {
        //    ClsPubPrimeAccountDetails ObjPDC = (ClsPubPrimeAccountDetails)ClsPubSerialize.DeSerialize(ObjPDCBytes, SerializationMode.Binary, typeof(ClsPubPrimeAccountDetails));
        //    List<ClsPubDropDownList> dropDownLists;
        //    ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
        //    dropDownLists = obj.FunPubGetPDCNO(ObjPDC);
        //    return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}
        //#endregion

        #region Get PDC Details Grid
        public byte[] FunPubGetPDCDetails(string PDC_NO, int CompanyId)
        {
            try
            {
            List<ClsPubPDCDetails> PDCDetails;
            ClsPubRptAcknowledgement obj = new ClsPubRptAcknowledgement();
            PDCDetails = obj.FunPubGetPDCDetails(PDC_NO, CompanyId);
            return ClsPubSerialize.Serialize(PDCDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region PDC Reminder Report
        public byte[] FunPubGetPDCReminderLOBDetails(int intCompanyId, int intUserId,int ProgramId)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            ClsPubRptPDCReminder objserpdcreminder = new ClsPubRptPDCReminder();
            dropdownlists = objserpdcreminder.FunPubGetPDCLOBDetails(intCompanyId, intUserId,ProgramId);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetPDCReminderBranchDetails(int intCompanyId, int intUserId,bool Is_Active,string LobId,int ProgramId)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            ClsPubRptPDCReminder objserpdcreminder = new ClsPubRptPDCReminder();
            dropdownlists = objserpdcreminder.FunPubGetPDCBranchDetails(intCompanyId,intUserId,ProgramId,LobId,Is_Active);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetLocationDetails(int CompanyId, int UserId, int ProgramId, int LobId, int LocationId)
        {
            try
            {
                List<ClsPubDropDownList> dropdownlists;
                ClsPubRptPDCReminder objserpdcreminder = new ClsPubRptPDCReminder();
                dropdownlists = objserpdcreminder.FunPubGetLocationDetails(CompanyId, UserId, ProgramId, LobId, LocationId);
                return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetPDCReminderGridDetails(byte[] objbytePDCHeaderDetails)
        {
            try
            {
            ClsPubRptPDCReminderHeaderDetails objheaderDetail = (ClsPubRptPDCReminderHeaderDetails)ClsPubSerialize.DeSerialize(objbytePDCHeaderDetails, SerializationMode.Binary, typeof(ClsPubRptPDCReminderHeaderDetails));
            ClsPubPDCReminderDetails PDCReminderDetails;
            ClsPubRptPDCReminder objpdcreminder = new ClsPubRptPDCReminder();
            PDCReminderDetails = objpdcreminder.FunPubGetPDCReminderGridDetails(objheaderDetail);
            return ClsPubSerialize.Serialize(PDCReminderDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetPDCDocPathDetails(int intCompanyId, int LobId,int ProgramId)
        {
            try
            {
                List<ClsPubPDCDocumentPathDetails> objdocumentpath = new List<ClsPubPDCDocumentPathDetails>();
                ClsPubRptPDCReminder objPdcReminder = new ClsPubRptPDCReminder();
                objdocumentpath = objPdcReminder.FunPubGetPDCDocPathDetails(intCompanyId, LobId, ProgramId);
                return ClsPubSerialize.Serialize(objdocumentpath, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        
        #region Get Frequency Details
        public byte[] FunPubGetFrequencyDetails()
        {
            try
            {
            List<ClsPubFrequencyDetails> FrequencyDetails;
            ClsPubRptDemandCollection objDemandCollection = new ClsPubRptDemandCollection();
            FrequencyDetails = objDemandCollection.FunPubGetFrequencyDetails();
            return ClsPubSerialize.Serialize(FrequencyDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        #endregion

        #region Get Asset Categories Type Details
        public byte[] FunPubGetAssetCategoriesType()
        {
            try
            {
            List<ClsPubDropDownList> AssetCategoriesTypes;
            ClsPubRptDemandCollection objDemandCollection = new ClsPubRptDemandCollection();
            AssetCategoriesTypes = objDemandCollection.FunPubGetAssetCategoriesType();
            return ClsPubSerialize.Serialize(AssetCategoriesTypes, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        #endregion

        #region Get Asset Details
        public byte[] FunPubGetAssetCategories(int CompanyId, int AssetTypeId)
        {
            try
            {
            List<ClsPubAssetCategories> AssetCategories;
            ClsPubRptDemandCollection objDemandCollection = new ClsPubRptDemandCollection();
            AssetCategories = objDemandCollection.FunPubGetAssetCategories(CompanyId, AssetTypeId);
            return ClsPubSerialize.Serialize(AssetCategories, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        #endregion

        #region Demand Collection 
        public byte[] FunPubGetDemandCollection(byte[] ObjDemandBytes)
        {
            try
            {
            ClsPubDemandParameterDetails Demand = (ClsPubDemandParameterDetails)ClsPubSerialize.DeSerialize(ObjDemandBytes, SerializationMode.Binary, typeof(ClsPubDemandParameterDetails));
            List<ClsPubDemandCollection> DemandCollection;
            ClsPubRptDemandCollection obj = new ClsPubRptDemandCollection();
            DemandCollection = obj.FunPubGetDemandCollection(Demand);
            return ClsPubSerialize.Serialize(DemandCollection, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Demand Collection Asset Wise
        public byte[] FunPubGetDemandCollectionDetails(byte[] ObjDemandBytes)
        {
            try
            {
            ClsPubDemandParameterDetails Demand = (ClsPubDemandParameterDetails)ClsPubSerialize.DeSerialize(ObjDemandBytes, SerializationMode.Binary, typeof(ClsPubDemandParameterDetails));
            List<ClsPubDemandCollection> DemandCollection;
            ClsPubRptDemandCollection obj = new ClsPubRptDemandCollection();
            DemandCollection = obj.FunPubGetDemandCollectionDetails(Demand);
            return ClsPubSerialize.Serialize(DemandCollection, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
       

        #region EnquiryPerformance
        //public byte[] FunPubGetEnquiryDetails(byte[] ObjEnq)
        public byte[] FunPubGetEnquiryDetails(ClsPubEnquiryParameters enq)
        {
            try
            {
            //ClsPubEnquiryParameters Enq = (ClsPubEnquiryParameters)ClsPubSerialize.DeSerialize(ObjEnq, SerializationMode.Binary, typeof(ClsPubEnquiryParameters));
            //List<ClsPubEnquiryPerformanceDetails> EnquiryDetails;
            //ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
            //EnquiryDetails = obj.FunPubGetEnquiryDetails(Enq);
            //return ClsPubSerialize.Serialize(EnquiryDetails, SerializationMode.Binary);
                ClsPubEnquiryLocation details = new ClsPubEnquiryLocation();
                ClsPubRptEnquiryPerformance report = new ClsPubRptEnquiryPerformance();
                details = report.FunPubGetEnquiryDetails(enq);
                return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetEnquiryRecCount(byte[] ObjRec)
        {
            try
            {
            ClsPubEnquiryParameters Enqparam = (ClsPubEnquiryParameters)ClsPubSerialize.DeSerialize(ObjRec, SerializationMode.Binary, typeof(ClsPubEnquiryParameters));
            List<ClsPubEnquiryCount> Enqcounts;
            ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
            Enqcounts = obj.FunPubGetEnquiryRecCount(Enqparam);
            return ClsPubSerialize.Serialize(Enqcounts, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetEnquirySucCount(byte[] ObjSuc)
        {
            try
            {
            ClsPubEnquiryParameters EnqSuc = (ClsPubEnquiryParameters)ClsPubSerialize.DeSerialize(ObjSuc, SerializationMode.Binary, typeof(ClsPubEnquiryParameters));
            List<ClsPubEnquiryCount> EnqSuccess;
            ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
            EnqSuccess = obj.FunPubGetEnquirySucCount(EnqSuc);
            return ClsPubSerialize.Serialize(EnqSuccess, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetEnquiryUnderFollowupCount(byte[] ObjFollow)
        {
            try
            {
            ClsPubEnquiryParameters EnqFollow = (ClsPubEnquiryParameters)ClsPubSerialize.DeSerialize(ObjFollow, SerializationMode.Binary, typeof(ClsPubEnquiryParameters));
            List<ClsPubEnquiryCount> EnqFollowups;
            ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
            EnqFollowups = obj.FunPubGetEnquiryUnderFollowupCount(EnqFollow);
            return ClsPubSerialize.Serialize(EnqFollowups, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetEnquiryRejectedCount(byte[] ObjReject)
        {
            try
            {
            ClsPubEnquiryParameters EnqRej = (ClsPubEnquiryParameters)ClsPubSerialize.DeSerialize(ObjReject, SerializationMode.Binary, typeof(ClsPubEnquiryParameters));
            List<ClsPubEnquiryCount> EnqRejects;
            ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
            EnqRejects = obj.FunPubGetEnquiryRejectedCount(EnqRej);
            return ClsPubSerialize.Serialize(EnqRejects, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        #endregion


        #region Get Customer Group Name
        public byte[] FunPubGetGroup(int CompanyId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDCCL obj = new ClsPubRptDCCL();
            dropDownLists = obj.FunPubGetGroup(CompanyId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get the Prime Account Number based on Company ID
        public byte[] FunPubGetDemandPAN(int CompanyId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDCCL obj = new ClsPubRptDCCL();
            dropDownLists = obj.FunPubGetDemandPAN(CompanyId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get Demand Sub Account Number
        public byte[] FunPubGetDemandSAN(string PANum)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDCCL obj = new ClsPubRptDCCL();
            dropDownLists = obj.FunPubGetDemandSAN(PANum);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get Customer,Customer Group Code and Prime Account  Number
        public byte[] FunPubGetCustomerGroupPAN(string Option, string Value, int CompanyId)
        {
            try
            {
            ClsPubCustomerGroupPAN CustomerGroupPANs;
            ClsPubRptDCCL obj = new ClsPubRptDCCL();
            CustomerGroupPANs = obj.FunPubGetCustomerGroupPAN(Option, Value, CompanyId);
            return ClsPubSerialize.Serialize(CustomerGroupPANs, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Denomination
        public byte[] GetDenominations()
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptSanctionDetails obj = new ClsPubRptSanctionDetails();
            dropDownLists = obj.GetDenominations();
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get Demand Collection Customer Level 
        public byte[] FunPubGetDCCLDetails(byte[] ObjDemandBytes)
        {
            try
            {
            ClsPubDemandParameterDetails Demand = (ClsPubDemandParameterDetails)ClsPubSerialize.DeSerialize(ObjDemandBytes, SerializationMode.Binary, typeof(ClsPubDemandParameterDetails));
            List<ClsPubDemandCollection> DemandCollection;
            ClsPubRptDCCL obj = new ClsPubRptDCCL();
            DemandCollection = obj.FunPubGetDCCLDetails(Demand);
            return ClsPubSerialize.Serialize(DemandCollection, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Demand Collection Region Customer Code Level
        public byte[] FunPubGetDCRegionCLDetails(byte[] ObjDemandBytes)
        {
            try
            {
            ClsPubDemandParameterDetails Demand = (ClsPubDemandParameterDetails)ClsPubSerialize.DeSerialize(ObjDemandBytes, SerializationMode.Binary, typeof(ClsPubDemandParameterDetails));
            List<ClsPubDCRegionCustomerCodeGridDetails> DemandCollection;
            ClsPubRptDCRegionCustomerCodeLevel obj = new ClsPubRptDCRegionCustomerCodeLevel();
            DemandCollection = obj.FunPubGetDemandCollectionReginCustomerCodeLevel(Demand);
            return ClsPubSerialize.Serialize(DemandCollection, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Payment Demand Collection NPA Report
        public byte[] FunPubGetPaymentDemandCollectionNPA(byte[] ObjPaymentBytes)
        {
            try
            {
            ClsPubPaymentDCNPAParameters Payment = (ClsPubPaymentDCNPAParameters)ClsPubSerialize.DeSerialize(ObjPaymentBytes, SerializationMode.Binary, typeof(ClsPubPaymentDCNPAParameters));
            //List<ClsPubPaymentDCNPADetails> PaymentDemandCollection;
            ClsPubCumulative Cumulative;
            ClsPubRptPaymentDemandCollectionNPA obj = new ClsPubRptPaymentDemandCollectionNPA();
            //PaymentDemandCollection = obj.FunPubGetPaymentDemandCollectionNPA(Payment);
            Cumulative = obj.FunPubGetPaymentDemandCollectionNPA(Payment);
            return ClsPubSerialize.Serialize(Cumulative, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Demand Statement
        public byte[] FunPubGetCategory(int CompanyId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDemandStatement obj = new ClsPubRptDemandStatement();
            dropDownLists = obj.FunPubGetCategory(CompanyId);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetDebtCollectorCode(ClsPubDemandSelectionCriteria demandselectioncriteria)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDemandStatement obj = new ClsPubRptDemandStatement();
            dropDownLists = obj.FunPubGetDebtCollectorCode(demandselectioncriteria);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetDemandStatement(ClsPubDemandSelectionCriteria demandselectioncriteria)
        {
            try
            {
            List<ClsPubDemandStatement> demand;
            ClsPubRptDemandStatement statement = new ClsPubRptDemandStatement();
            demand = statement.FunPubGetDemandStatement(demandselectioncriteria);
            return ClsPubSerialize.Serialize(demand, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
            
        }
        #endregion

        #region DPD Report
        public byte[] FunPubGetDPDBucketDetails(int LOB_ID, int BUCKET_COUNT)
        {
            try
            {
                DataSet DSBktDetails = new DataSet();
                S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport objDPDReport = new S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport();
                DSBktDetails = objDPDReport.FunPubGetDPDBucketDetails(LOB_ID, BUCKET_COUNT);
                return ClsPubSerialize.Serialize(DSBktDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public byte[] FunPubGetDPDReportDetails(int LOBID, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Location1, int Location2, int ProgramID, int UserID, int AccountStatus)
        {
            try
            {
                DataSet DSDPDDetails = new DataSet();
                S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport objDPDReport = new S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport();
                DSDPDDetails = objDPDReport.FunPubGetDPDReportDetails(LOBID, CutOff, Denomination, Bucketdetails, RcptUpto, CompanyID, Location1, Location2, ProgramID, UserID, AccountStatus);
                return ClsPubSerialize.Serialize(DSDPDDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            } 
        }
        public byte[] FunPubGetDPDReportDetailsforRPT(int LOBID, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Location1, int Location2, int ProgramID, int UserID, int AccountStatus)
        {
            try
            {
                DataSet DSDPDDetails = new DataSet();
                S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport objDPDReport = new S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport();
                DSDPDDetails = objDPDReport.FunPubGetDPDReportDetailsForRPT(LOBID, CutOff, Denomination, Bucketdetails, RcptUpto, CompanyID, Location1, Location2, ProgramID, UserID, AccountStatus);
                return ClsPubSerialize.Serialize(DSDPDDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        public byte[] FunPubGetAssetDPDReportDetails(int LOBID, int Class,string LOB, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Branch, int Region)
        {
            try
            {
                DataSet DSDPDDetails = new DataSet();
                S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport objDPDReport = new S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport();
                DSDPDDetails = objDPDReport.FunPubGetAssetDPDReportDetails(LOBID, LOB, Class, CutOff, Denomination, Bucketdetails, RcptUpto, CompanyID, Branch, Region);
                return ClsPubSerialize.Serialize(DSDPDDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            } 

        }
        public byte[] FunPubCheckDemandMonthExists(int Company_ID, int LOB_ID, string Demand_Month)
        {
            try
            {
                DataSet DSDPDDemand = new DataSet();
                S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport objDPDReport = new S3GDALayer.Reports.ReportMgtServices.ClsPubRptDPDReport();
                DSDPDDemand = objDPDReport.FunPubCheckDemandMonthExists(Company_ID, LOB_ID, Demand_Month);
                return ClsPubSerialize.Serialize(DSDPDDemand, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            } 

         }
       #endregion

        #region Collection Report
        public byte[] FunPubGetCollectionPreciseDetails(byte[] objbyteCollectionHeader)
        {
            try
            {
            ClsPubCollectionHeader objheaderDetail = (ClsPubCollectionHeader)ClsPubSerialize.DeSerialize(objbyteCollectionHeader, SerializationMode.Binary, typeof(ClsPubCollectionHeader));
            ClsPubRptCollection objcollection = new ClsPubRptCollection();
            List<ClsPubCollectionDetails> objcollectiondetails = objcollection.FunPubGetCollectionPreciseDetails(objheaderDetail);
            return ClsPubSerialize.Serialize(objcollectiondetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetCollectionDetails(byte[] objbyteCollectionHeaderDetails)
        {
            try
            {
            ClsPubCollectionHeader objheaderDetails = (ClsPubCollectionHeader)ClsPubSerialize.DeSerialize(objbyteCollectionHeaderDetails, SerializationMode.Binary, typeof(ClsPubCollectionHeader));
            ClsPubRptCollection objcollectionDetails = new ClsPubRptCollection();
            List<ClsPubCollectionDetails> objcollectiondetails = objcollectionDetails.FunPubGetCollectionDetails(objheaderDetails);
            return ClsPubSerialize.Serialize(objcollectiondetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Stock Receivables Report
        public byte[] FunPubGetStockReceivablesDetails(ClsPubStockReceivableparameters ObjStock)
        {
            try
            {
            List<ClsPubStockReceivableDetails> StockReceivablesDet;
            ClsPubRptStockReceivables Obj = new ClsPubRptStockReceivables();
            StockReceivablesDet = Obj.FunPubGetStockReceivablesDetails(ObjStock);
            return ClsPubSerialize.Serialize(StockReceivablesDet, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get LEVEL DETAILS
        public byte[] FunPubGetLevelDetails(int LOB_ID, int USER_ID, int COMPANY_ID)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            clsPubRPTCollectionPerformance objCollectionPerformanc = new clsPubRPTCollectionPerformance();
            dropdownlists = objCollectionPerformanc.FunPubGetLevelDetails(COMPANY_ID, USER_ID, LOB_ID);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        
        #region Get LEVELVALUE DETAILS
        public byte[] FunPubGetLevelValueDetails(int Hierarchy_Type, int Company_ID)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            clsPubRPTCollectionPerformance objCollectionPerformanc = new clsPubRPTCollectionPerformance();
            dropdownlists = objCollectionPerformanc.FunPubGetLevelValueDetails(Hierarchy_Type, Company_ID);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get ASSETTYPE DETAILS
        public byte[] FunPubGetAssetTypeDetails(int User_ID, int Company_ID)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            clsPubRPTCollectionPerformance objCollectionPerformanc = new clsPubRPTCollectionPerformance();
            dropdownlists = objCollectionPerformanc.FunPubGetAssetTypeDetails(User_ID, Company_ID);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
               
        #region Get PRODUCT DETAILS
        public byte[] FunPubGetProductsDetails(int Company_ID, int LOB_ID)
        {
            try
            {
            List<ClsPubDropDownList> dropdownlists;
            clsPubRPTCollectionPerformance objCollectionPerformanc = new clsPubRPTCollectionPerformance();
            dropdownlists = objCollectionPerformanc.FunPubGetProductsDetails(Company_ID, LOB_ID);
            return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion


      
       #region COLLECTION AMOUNT
        public byte[] FunPubGetCollectionAmount(ClsPubPerformance CollectionPerformance)
        {
            try
            {
                ClspubCollectionReturnAmount objCollectionPerformanceDetails;
                clsPubRPTCollectionPerformance objCollectionPerformance = new clsPubRPTCollectionPerformance();
                //ClsPubChequeReturnAmount objCollectionPerformance = new ClsPubChequeReturnAmount();
                objCollectionPerformanceDetails = objCollectionPerformance.FunPubGetCollectionAmount(CollectionPerformance);
                return ClsPubSerialize.Serialize(objCollectionPerformanceDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            } 
        }
        public byte[] FunPubgetCollectionAnalysisDtls(ClsPubPerformance CollectionPerformance)
        {
            try
            {
                List<ClsPubCollectionAnalysis> objCollectionAnalysisDetails;
                clsPubRPTCollectionPerformance objCollectionPerformance = new clsPubRPTCollectionPerformance();
                objCollectionAnalysisDetails = objCollectionPerformance.FunPubgetCollectionAnalysisDtls(CollectionPerformance);
                return ClsPubSerialize.Serialize(objCollectionAnalysisDetails, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            } 
        }
        #endregion




        #region Asset Performance Details
        //To Load LOB
        public byte[] FunPubLOBAssetPerf(int Company_ID, int UserId, int Option, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropdownlists;

                ClsPubRptAssetPerformance objAssetPerformance = new ClsPubRptAssetPerformance();
                dropdownlists = objAssetPerformance.FunPubLOBAssetPerf(Company_ID, UserId, Option,ProgramId );
                return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
               
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        //To Load Asset Performance Grid
        public byte[] FunPubGetAssestPerformance(ClsPubAssetPerfParam objAssetParams)
        {
            try
            {

                List<ClsPubAssetPerformance> objAssetPerformanceGrid;
                ClsPubRptAssetPerformance objAssetPerformance = new ClsPubRptAssetPerformance();
                objAssetPerformanceGrid = objAssetPerformance.FunPubGetAssestPerformance(objAssetParams);
                return ClsPubSerialize.Serialize(objAssetPerformanceGrid, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        //To Load LOB, PANUM, Group details in Demand Collection customer Level
        public byte[] FunPubGetGroupPAN(ClsPubDCHeaderParams objHeaderParams)
        {
            try
            {
                ClsPubCustomerGroupPAN groupPANService = new ClsPubCustomerGroupPAN();
                ClsPubRptDCCL objDCCL = new ClsPubRptDCCL();
                groupPANService = objDCCL.FunPubGetGroupPAN(objHeaderParams);
                return ClsPubSerialize.Serialize(groupPANService, SerializationMode.Binary);

                
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        //To Load LOB in Demand Collection customer Level
        public byte[] FunPubGetDemandCustLOB(int CompanyId, int UserId, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptDCCL obj = new ClsPubRptDCCL();
                dropDownLists = obj.FunPubGetDemandCustLOB(CompanyId, UserId, ProgramId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }



        #region Income Report 
        //To Load LOB
        public byte[] FunPubLOBIncome(int Company_ID, int UserId, int Option, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropdownlists;

                ClsPubRptIncomeReport objIncomeReport = new ClsPubRptIncomeReport();
                dropdownlists = objIncomeReport.FunPubLOBIncome(Company_ID, UserId, Option, ProgramId);
                return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        //To Load Location1 on Multi LOB
        public byte[] FunPubLoc1_MultiLOB(int CompanyId, int UserId, int ProgramId, string LobId)
        {
            try
            {
                List<ClsPubDropDownList> dropdownlists;

                ClsPubRptIncomeReport objIncomeReport = new ClsPubRptIncomeReport();
                dropdownlists = objIncomeReport.FunPubLoc1_MultiLOB(CompanyId, UserId, ProgramId, LobId);
                return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);

            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        //To Load Location2 on Multi LOB and Location2
        public byte[] FunPubLoc2_MultiLOB(int ProgramId, int UserId, int CompanyId, string LobId, int LocationId)
        {
            try
            {
                List<ClsPubDropDownList> dropdownlists;
                ClsPubRptIncomeReport objIncomeReport = new ClsPubRptIncomeReport();
                dropdownlists = objIncomeReport.FunPubLoc2_MultiLOB(ProgramId, UserId, CompanyId, LobId, LocationId);
                return ClsPubSerialize.Serialize(dropdownlists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        //To Load Income Report Grid
        public byte[] FunPubGetIncomeReport(ClsPubIncomeReportParams objIncomeParams)
        {
            try
            {

                //List<ClsPubIncomeReport> objIncomeReportGrid;
                ClsPubIncomeReport objIncomeReportGrid;
                ClsPubRptIncomeReport objIncomeReport = new ClsPubRptIncomeReport();
                objIncomeReportGrid = objIncomeReport.FunPubGetIncomeReport(objIncomeParams);
                return ClsPubSerialize.Serialize(objIncomeReportGrid, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region Probable Delinquency
        public byte[] FunPubGetProbableDelinq(ClsPubProbDelinqParam ObjDelinqPar)
        {
            try
            {
                List<ClsPubProbableDelinq> objProbDelinqGrid;
                ClsPubProbableDelinqency objprbdlnq = new ClsPubProbableDelinqency();
                objProbDelinqGrid = objprbdlnq.FunPubGetProbableDelinq(ObjDelinqPar);
                return ClsPubSerialize.Serialize(objProbDelinqGrid, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

    }
}