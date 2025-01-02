using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;

namespace S3GServiceLayer.Reports
{
    // NOTE: If you change the interface name "IReportOrgColMgtServices" here, you must also update the reference to "IReportOrgColMgtServices" in Web.config.
    [ServiceContract]
    public interface IReportOrgColMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLOB(int CompanyId, int UserId, bool Is_Active);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubBranch(int CompanyId, int UserId, bool Is_Active);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProduct(int CompanyId, int LOBId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCreditScoreDetails(ClsPubCredit ObjCredit);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustomersDetails(ClsPubCredit ObjCustomers);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRejCustomersDetails(ClsPubCredit ObjRejCustomers);

        #region PDC Reminder Report
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCReminderLOBDetails(int intCompanyId, int intUserId,int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLocationDetails(int CompanyId, int UserId, int ProgramId, int LobId, int LocationId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCReminderBranchDetails(int intCompanyId, int intUserId, bool Is_Active, string LobId, int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCReminderGridDetails(byte[] objbytePDCHeaderDetails);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCDocPathDetails(int intCompanyId, int LobId, int ProgramId);
        #endregion


        #region ENQUIRY PERFORMANCE REPORT
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryDetails(ClsPubEnquiryParameters enq);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryRecCount(byte[] ObjRec);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquirySucCount(byte[] ObjSuc);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryUnderFollowupCount(byte[] ObjFollow);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetEnquiryRejectedCount(byte[] ObjReject);

        #endregion

        #region PDC Acknowledgement

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetMLA(byte[] ObjPrimeAccountBytes);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetSLA(string Type, int CompanyId, string PNum, int IsActive, string CustomerId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCLOB(int CompanyId, int UserId, int ProgramId, string CustomerId);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubPDCAckPAN(ClsPubStockReceivableparameters PDCPAN);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubPDCAckSAN(int CompanyId, string PNum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubPDCNumber(string PNum, string SNum);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubPDCDate(string PdcNo);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetHeaderLobBranchDetails(string PANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPDCDetails(string PDC_NO,int CompanyId);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetFrequencyDetails();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssetCategoriesType();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssetCategories(int CompanyId, int AssetTypeId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandCollection(byte[] ObjDemandBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandCollectionDetails(byte[] ObjDemandBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDCCLDetails(byte[] ObjDemandBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDCRegionCLDetails(byte[] ObjDemandBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGroup(int CompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandPAN(int CompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandSAN(string PANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustomerGroupPAN(string Option, string Value, int CompanyId);

        #region Denominations
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] GetDenominations();
        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPaymentDemandCollectionNPA(byte[] ObjPaymentBytes);

        #region Demand Statement
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCategory(int CompanyId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDebtCollectorCode(ClsPubDemandSelectionCriteria demandselectioncriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandStatement(ClsPubDemandSelectionCriteria demandselectioncriteria);
        #endregion

        #region DPD report
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDPDBucketDetails(int LOBID, int BucketCount);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDPDReportDetails(int LOBID, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Location1, int Location2, int ProgramID, int UserID, int AccountStatus);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDPDReportDetailsforRPT(int LOBID, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Location1, int Location2, int ProgramID, int UserID, int AccountStatus);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssetDPDReportDetails(int LOBID, int Class, string LOB, string CutOff, int Denomination, string Bucketdetails, string RcptUpto, int CompanyID, int Branch, int Region);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubCheckDemandMonthExists(int Company_ID, int LOB_ID, string Demand_Month);
        #endregion

        #region Collection Report
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollectionPreciseDetails(byte[] objbyteCollectionHeader);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollectionDetails(byte[] objbyteCollectionHeaderDetails);
        #endregion

        #region Stock Receivables Report
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetStockReceivablesDetails(ClsPubStockReceivableparameters ObjStock);
        #endregion

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLevelDetails(int LOB_ID, int USER_ID,int COMPANY_ID);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLevelValueDetails(int Hierarchy_Type, int Company_ID);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssetTypeDetails(int User_ID, int Company_ID);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProductsDetails(int Company_ID, int LOB_ID);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollectionAmount(ClsPubPerformance CollectionPerformance);
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubgetCollectionAnalysisDtls(ClsPubPerformance CollectionPerformance);



        #region Asset Performance Details
        //To Load LOB
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLOBAssetPerf(int Company_ID, int UserId, int Option, int ProgramId);
       
        //To Load Asset Performance Grid
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssestPerformance(ClsPubAssetPerfParam objAssetParams);
        
        #endregion


        //To Load LOB
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetGroupPAN(ClsPubDCHeaderParams objHeaderParams);

        //To Get the Demand Collection Customer Level LOB
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDemandCustLOB(int CompanyId, int UserId, int ProgramId);


        #region Income Report
        //To Load LOB
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLOBIncome(int Company_ID, int UserId, int Option, int ProgramId);

        //To Load Location1 on Multi LOB
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoc1_MultiLOB(int CompanyId, int UserId, int ProgramId, string LobId);

        //To Load Location2 on Multi LOB and Location2
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLoc2_MultiLOB(int ProgramId, int UserId, int CompanyId, string LobId, int LocationId);
     
        //To Load Income Details Grid
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetIncomeReport(ClsPubIncomeReportParams objIncomeParams);

        #endregion

        #region Probable Delinquency
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProbableDelinq(ClsPubProbDelinqParam ObjDelinqPar);
        #endregion
    }
}
