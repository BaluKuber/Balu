using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;
using S3GDALayer.Reports;
using S3GBusEntity;
using System.ServiceModel.Activation;
using System.Data;

namespace S3GServiceLayer.Reports
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ReportAccountsMgtServices" here, you must also update the reference to "ReportAccountsMgtServices" in Web.config.
    public class ReportAccountsMgtServices : IReportAccountsMgtServices
    {
        #region Repayment Schedule

        #region Get LOB
        public byte[] FunPubGetLOB(int CompanyId, int UserId,int ProgramId, string CustomerId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, ProgramId, CustomerId);
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

        #region Get Branch
        public byte[] FunPubGetBranch(int CompanyId, int UserId, int ProgramId, string CustomerId, int LobId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            dropDownLists = obj.FunPubGetBranch(CompanyId, UserId, ProgramId, CustomerId, LobId);
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

        #region Get Product
        public byte[] FunPubGetProduct(int CompanyId, string CustomerId)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            dropDownLists = obj.FunPubGetProduct(CompanyId, CustomerId);
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

        #region Get prime Account Number
        public byte[] FunPubGetMLA(byte[] ObjPrimeAccountBytes)
        {
            try
            {
            ClsPubPrimeAccountDetails ObjPrimeAccount = (ClsPubPrimeAccountDetails)ClsPubSerialize.DeSerialize(ObjPrimeAccountBytes, SerializationMode.Binary, typeof(ClsPubPrimeAccountDetails));
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            dropDownLists = obj.FunPubGetMLA(ObjPrimeAccount);
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

        #region Get Sub Account Number
        public byte[] FunPubGetSLA(string Type, int CompanyId, string PNum, int IsActive, string CustomerId,int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
                dropDownLists = obj.FunPubGetSLA(Type, CompanyId, PNum, IsActive, CustomerId, ProgramId);
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

        #region Get Asset Details
        public byte[] FunPubGetAssestDetails(int CompanyId, string PANum, string SANum)
        {
            try
            {
            List<ClsPubAssestDetails> assestDetails;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            assestDetails = obj.FunPubGetAssestDetails(CompanyId,PANum, SANum);
            return ClsPubSerialize.Serialize(assestDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        #endregion

        #region Get Repayment Details
        public byte[] FunPubGetRepayDetails(int CompanyId,string PANum, string SANum,string Type)
        {
            try
            {
            List<ClsPubRepayDetails> RepayDetails;
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            RepayDetails = obj.FunPubGetRepayDetails(CompanyId,PANum, SANum,Type);
            return ClsPubSerialize.Serialize(RepayDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        #endregion

        #region Get LOB,Branch,Product Details
        public byte[] FunPunGetHeaderLobBranchProductDetails(string PANum)
        {
            try
            {
            ClsPubRptRepaymentSchedule obj = new ClsPubRptRepaymentSchedule();
            return ClsPubSerialize.Serialize(obj.FunPunGetHeaderLobBranchProductDetails(PANum), SerializationMode.Binary);
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

        #region SOA
        public byte[] FunPubSOAGetLOB(int CompanyId, int UserId, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubSOA obj = new ClsPubSOA();
                dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, ProgramId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubBranch(int CompanyId, int UserId, int ProgramId,int LobId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubSOA obj = new ClsPubSOA();
                dropDownLists = obj.FunPubBranch(CompanyId, UserId, ProgramId, LobId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetPASA(ClsPubSOASelectionCriteria selection)
        {
            try
            {
            List<ClsPubPASA> PASAs;
            ClsPubSOA obj = new ClsPubSOA();
            PASAs = obj.FunPubGetPASA(selection);
            return ClsPubSerialize.Serialize(PASAs, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        //public byte[] FunPubGetTransactionDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs, out decimal OpeningBalance)
        //{
        //    ClsPubSOA obj = new ClsPubSOA();
        //    List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
        //    List<ClsPubTransaction> transactions = obj.FunPubGetTransactionDetails(CompanyId, StartDate, EndDate, PASAsList, out OpeningBalance);

        //    return ClsPubSerialize.Serialize(transactions, SerializationMode.Binary);

        //}
        public byte[] FunPubGetSummaryDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs)
        {
            try
            {

            ClsPubSOA obj = new ClsPubSOA();
            List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
            ClsPubSummary summary = obj.FunPubGetSummaryDetails(CompanyId, StartDate, EndDate, PASAsList);

            return ClsPubSerialize.Serialize(summary, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }


        public byte[] FunPubGetMemorandumDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs)
        {
            try
            {
            ClsPubSOA obj = new ClsPubSOA();
            List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
            ClsPubMemorandum memo = obj.FunPubGetMemorandumDetails(CompanyId, StartDate, EndDate, PASAsList);

            return ClsPubSerialize.Serialize(memo, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        public byte[] FunPubGetSOAAssetDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs, out decimal OpeningBalance)
        {
            try
            {
            ClsPubSOA obj = new ClsPubSOA();
            List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
            ClsPubSOAAsset assets = obj.FunPubGetAssetDetails(CompanyId, StartDate, EndDate, PASAsList,out  OpeningBalance);
            return ClsPubSerialize.Serialize(assets, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetSummaryAccountDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs)
        {
            try
            {

                ClsPubSOA obj = new ClsPubSOA();
                List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
                List<ClsPubSummaryAccount> summary = obj.FunPubGetSummaryAccountDetails(CompanyId, StartDate, EndDate, PASAsList);
                return ClsPubSerialize.Serialize(summary, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

        public byte[] FunPubGetMemorandumAccountDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs)
        {
            try
            {

                ClsPubSOA obj = new ClsPubSOA();
                List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
                List<ClsPubMemorandumAccount> summary = obj.FunPubGetMemorandumAccountDetails(CompanyId, StartDate, EndDate, PASAsList);
                return ClsPubSerialize.Serialize(summary, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        
        #endregion

        #region Enquiryperformance
        #region Get LOB
        public byte[] FunPubEnQGetLOB(int CompanyId, int UserId, bool Is_Active, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptEnquiryPerformance obj = new ClsPubRptEnquiryPerformance();
                dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, Is_Active, ProgramId);
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
        #region Customer At A Glance

        public byte[] FunPubGetRegionDetails(int CompanyId, int UserId)
        {
            try
            {
                ClsPubCustomerGlance customerGlance = new ClsPubCustomerGlance();
                List<ClsPubDropDownList> region = customerGlance.FunPubGetRegionDetails(CompanyId, UserId);
                return ClsPubSerialize.Serialize(region, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetCustomerBasedLOB(int CompanyId, int UserId, bool Is_Active, string CustomerId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubCustomerGlance obj = new ClsPubCustomerGlance();
                dropDownLists = obj.FunPubGetLOB(CompanyId, UserId, Is_Active, CustomerId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetProductDetails(int CompanyId,int LobId)
        {
            try
            {
            ClsPubCustomerGlance customerGlance = new ClsPubCustomerGlance();
            List<ClsPubDropDownList> product = customerGlance.FunPubGetProductDetails(CompanyId,LobId);
            return ClsPubSerialize.Serialize(product, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetBranchDetails(int CompanyId, int UserId, int RegionId, Boolean IsActive)
        {
            try
            {
            ClsPubCustomerGlance customerGlance = new ClsPubCustomerGlance();
            List<ClsPubDropDownList> branch = customerGlance.FunPubGetBranchDetails(CompanyId, UserId, RegionId, IsActive);
            return ClsPubSerialize.Serialize(branch, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        public byte[] FunPubGetCustomerAtAGlanceDetails(byte[] byteHeaderDetails, byte[] PASAs)
        {
            try
            {
            ClsPubCustomerGlanceHeaderDetails headerDetail = (ClsPubCustomerGlanceHeaderDetails)ClsPubSerialize.DeSerialize(byteHeaderDetails, SerializationMode.Binary, typeof(ClsPubCustomerGlanceHeaderDetails));
            List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
            ClsPubCustomerGlance customerGlance = new ClsPubCustomerGlance();
            List<ClsPubCustomerGlanceDetails> customerGlanceDetails = customerGlance.FunPubGetCustomerAtAGlanceDetails(headerDetail, PASAsList);
            return ClsPubSerialize.Serialize(customerGlanceDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetCustomerGlanceAssetDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs)
        {
            try
            {
            ClsPubCustomerGlance obj = new ClsPubCustomerGlance();
            List<ClsPubPASA> PASAsList = (List<ClsPubPASA>)ClsPubSerialize.DeSerialize(PASAs, SerializationMode.Binary, typeof(List<ClsPubPASA>));
            List<ClsPubAsset> assets = obj.FunPubGetAssetDetails(CompanyId, StartDate, EndDate, PASAsList);
            return ClsPubSerialize.Serialize(assets, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        #endregion

        #region Disbursementreport
        public byte[] FunPubLOB(int CompanyId, int UserId,int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptDisbursement obj = new ClsPubRptDisbursement();
                dropDownLists = obj.FunPubLOB(CompanyId, UserId, ProgramId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        
        #region Region
        public byte[] FunPubGetRegion(int CompanyId, bool Is_Active,int UserId)
        {
            try
            {
            ClsPubRptDisbursement obj = new ClsPubRptDisbursement();
            List<ClsPubDropDownList> region = obj.FunPubGetRegion(CompanyId, Is_Active, UserId);
            return ClsPubSerialize.Serialize(region, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Get Branch
        public byte[] FunPubGetRegBranch(int CompanyId, int UserId, string Region_Id, bool Is_Active)
        {
            try
            {
            List<ClsPubDropDownList> dropDownLists;
            ClsPubRptDisbursement obj = new ClsPubRptDisbursement();
            dropDownLists = obj.FunPubGetRegBranch(CompanyId, UserId, Region_Id, Is_Active);
            return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }
        public byte[] FunPubGetLocation2(int ProgramId, int UserId, int CompanyId, int LobId, int LocationId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptDisbursement obj = new ClsPubRptDisbursement();
                dropDownLists = obj.FunPubGetLocation2(ProgramId, UserId, CompanyId, LobId, LocationId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

        public byte[] FunPubGetDisburseDetails(ClsPubDisburseSelectionCriteria disburseSelectionCriteria)
        {
            try
            {
               ClsPubDisbursement details=new ClsPubDisbursement();
                ClsPubRptDisbursement report = new ClsPubRptDisbursement();
                details = report.FunPubGetDisburseDetails(disburseSelectionCriteria);
                return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
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

        #region Sanction Details

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
        public byte[] FunPubGetSanctionDetails(byte[] ObjSanctionBytes)
        {
            try
            {
            ClsPubSanctionParameterDetails ObjSanctionParameter = (ClsPubSanctionParameterDetails)ClsPubSerialize.DeSerialize(ObjSanctionBytes, SerializationMode.Binary, typeof(ClsPubSanctionParameterDetails));
            ClsPubSumOfSanctionDetails SanctionDetails;
            ClsPubRptSanctionDetails obj = new ClsPubRptSanctionDetails();
            SanctionDetails = obj.FunPubGetSanctionDetails(ObjSanctionParameter);
            return ClsPubSerialize.Serialize(SanctionDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetSanctionReport(byte[] ObjSanction)
        {
            try
            {
                ClsPubSanctionParameterDetails ObjSanctionParameter = (ClsPubSanctionParameterDetails)ClsPubSerialize.DeSerialize(ObjSanction, SerializationMode.Binary, typeof(ClsPubSanctionParameterDetails));
                ClsPubSanctionDetailsReport SanctionDetailsReport;
                ClsPubRptSanctionDetails obj = new ClsPubRptSanctionDetails();
                SanctionDetailsReport = obj.FunPubGetSanctionReport(ObjSanctionParameter);
                return ClsPubSerialize.Serialize(SanctionDetailsReport, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region BranchPerformance
        public byte[] FunPubGetCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsPubCollection> collections;
            ClsPubRptBranchPerformance branch = new ClsPubRptBranchPerformance();
            collections = branch.FunPubGetCollectionDetails(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(collections, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetStockDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {

            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            List<ClsPubBranchStock> branchstock = obj.FunPubGetStockDetails(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(branchstock, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetPayment(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            
            ClsPubPayment payment = new ClsPubPayment();
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            payment = obj.FunPubGetPayment(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(payment, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetNPADetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsPubNPA> npa;
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            npa = obj.FunPubGetNPADetails(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(npa, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsNPAaccount> npaaccount;
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            npaaccount = obj.FunPubGetNPAOpeningaccounts(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
          }
        public byte[] FunPubGetNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsNPAaccount> npaaccount;
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            npaaccount = obj.FunPubGetNPAAddtionaccounts(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsNPAaccount> npaaccount;
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            npaaccount = obj.FunPubGetNPADeletionaccounts(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        public byte[] FunPubGetNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            List<ClsNPAaccount> npaaccount;
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            npaaccount = obj.FunPubGetNPAClosingaccounts(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetTotalNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
                List<ClsNPAaccount> npaaccount;
                ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
                npaaccount = obj.FunPubGetTotalNPAAddtionaccounts(branchSelectionCriteria);
                return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetTotalNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
                List<ClsNPAaccount> npaaccount;
                ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
                npaaccount = obj.FunPubGetTotalNPAOpeningaccounts(branchSelectionCriteria);
                return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetTotalNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
                List<ClsNPAaccount> npaaccount;
                ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
                npaaccount = obj.FunPubGetTotalNPADeletionaccounts(branchSelectionCriteria);
                return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetTotalNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
                List<ClsNPAaccount> npaaccount;
                ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
                npaaccount = obj.FunPubGetTotalNPAClosingaccounts(branchSelectionCriteria);
                return ClsPubSerialize.Serialize(npaaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetCumulativeCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            List<ClsPubCumulativeCollection> cumulative = obj.FunPubGetCumulativeCollectionDetails(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(cumulative, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetAccountDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {
            ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
            List<ClsPubBranchAccount> branchaccount = obj.FunPubGetAccountDetails(branchSelectionCriteria);
            return ClsPubSerialize.Serialize(branchaccount, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetRegionBranchDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria)
        {
            try
            {

                ClsPubRptBranchPerformance obj = new ClsPubRptBranchPerformance();
                List<ClsPubRegionBranch> branchstock = obj.FunPubGetRegionBranchDetails(branchSelectionCriteria);
                return ClsPubSerialize.Serialize(branchstock, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion
        #region Vendor Details
        public byte[] FunPubGetVendorDetails(int CompanyId, string EntityCode, string StartDate, string EndDate,decimal Denomination, out decimal OpeningBalance)
        {
            try
            {
                ClsPubVendorDetails obj = new ClsPubVendorDetails();
                List<ClsPubTransaction> vendorDetails = obj.FunPubGetVendorDetails(CompanyId, EntityCode, StartDate, EndDate,Denomination, out OpeningBalance);
                return ClsPubSerialize.Serialize(vendorDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
        #endregion

        #region Journal Query
        public byte[] FunPubGetJournalDetails(byte[] ObjJournal)
        {
            try
            {
                ClsPubJournalParameters objJournalParameters = (ClsPubJournalParameters)ClsPubSerialize.DeSerialize(ObjJournal, SerializationMode.Binary, typeof(ClsPubJournalParameters));
                ClsPubRptJournalQuery objJournalQuery = new ClsPubRptJournalQuery();
                List<ClsPubTransaction> JournalDetails = objJournalQuery.FunPubGetJournalDetails(objJournalParameters);
                return ClsPubSerialize.Serialize(JournalDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }
       
        #endregion

        #region Collateral Report

        public byte[] FunPubGetCollateralDetails(byte[] byteHeader)
        {
            try
            {
                ClsPubCollateralHeader objCollateralParameters = (ClsPubCollateralHeader)ClsPubSerialize.DeSerialize(byteHeader, SerializationMode.Binary, typeof(ClsPubCollateralHeader));
                ClsPubRptCollateralReport objCollateralReport = new ClsPubRptCollateralReport();
                List<ClsPubCollateralReport> CollateralDetails = objCollateralReport.FunPubGetCollateralReport(objCollateralParameters);
                return ClsPubSerialize.Serialize(CollateralDetails, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }

        public byte[] FunPubGetCollateralType(int CompanyID)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptCollateralReport obj = new ClsPubRptCollateralReport();
                dropDownLists = obj.FunPubGetCollateralType(CompanyID);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }


        public byte[] FunPubLOB_FT(int CompanyId, int UserId, int ProgramId)
        {
            try
            {
                List<ClsPubDropDownList> dropDownLists;
                ClsPubRptFactoringMaturity obj = new ClsPubRptFactoringMaturity();
                dropDownLists = obj.FunPubLOB_FT(CompanyId, UserId, ProgramId);
                return ClsPubSerialize.Serialize(dropDownLists, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }


        public byte[] FunPubGetFactoringMaturityDetails(ClsPubFactoringMaturitySelectionCriteria factoringSelectionCriteria)
        {
            try
            {
                ClsPubFactoringMaturity details = new ClsPubFactoringMaturity();
                ClsPubRptFactoringMaturity report = new ClsPubRptFactoringMaturity();                
                details = report.FunPubGetFactoringMaturityDetails(factoringSelectionCriteria);
                return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }


        public byte[] FunPubGetFactoringBusinessDetails(ClsPubFactoringBusinessSelectionCriteria factoringBusinessSelectionCriteria)    
        {
            try
            {
                ClsPubFactoringBusiness details = new ClsPubFactoringBusiness();
                ClsPubRptFactoringBusiness report = new ClsPubRptFactoringBusiness();
                details = report.FunPubGetFactoringBusinessDetails(factoringBusinessSelectionCriteria);
                return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }
        }


        //public byte[] FunPubGetFactoringInterestDetails(ClsPubFactoringInterestSelectionCriteria factoringInterestSelectionCriteria)
        //{
        //    try
        //    {
        //        ClsPubFactoringInterest details = new ClsPubFactoringInterest();
        //        ClsPubRptFactoringInterest report = new ClsPubRptFactoringInterest();
        //        details = report.FunPubGetFactoringInterestDetails(factoringInterestSelectionCriteria);
        //        return ClsPubSerialize.Serialize(details, SerializationMode.Binary);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClsPubFaultException objfaultex = new ClsPubFaultException();
        //        objfaultex.ProReasonRW = ex.Message.ToString();
        //        throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
        //    }
        //}

       
        

        #endregion

        #region Repossession Report        
        public byte[] FunPubGetRepossessionDetails(int LOBID, int Location, string FromDate, string ToDate, int CompanyID, int GaragecNo, int ProgramID, int UserID)
        {
            try
            {
                DataSet DSRepoDetails = new DataSet();
                S3GDALayer.Reports.ReportAccountsMgtServices.ClsPubRptRepossession objRepoReport = new S3GDALayer.Reports.ReportAccountsMgtServices.ClsPubRptRepossession();              
                DSRepoDetails = objRepoReport.FunPubGetRepossessionDetails(LOBID, Location, FromDate, ToDate, CompanyID, GaragecNo, ProgramID, UserID);
                return ClsPubSerialize.Serialize(DSRepoDetails, SerializationMode.Binary);
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
        #endregion