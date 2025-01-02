using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity.Reports;

namespace S3GServiceLayer.Reports
{
    // NOTE: If you change the interface name "IReportAccountsMgtServices" here, you must also update the reference to "IReportAccountsMgtServices" in Web.config.
    [ServiceContract]
    public interface IReportAccountsMgtServices
    {
        #region Repayment Schedule


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLOB(int CompanyId, int UserId, int ProgramId, string CustomerId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetBranch(int CompanyId, int UserId, int ProgramId, string CustomerId,int LobId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProduct(int CompanyId, string CustomerId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMLA(byte[] ObjPrimeAccountBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSLA(string Type, int CompanyId, string PNum, int IsActive, string CustomerId, int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAssestDetails(int CompanyId, string PANum, string SANum);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRepayDetails(int CompanyId, string PANum, string SANum,string Type);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPunGetHeaderLobBranchProductDetails(string PANum);

        #endregion

        #region SOA

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubSOAGetLOB(int CompanyId, int UserId, int ProgramId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubBranch(int CompanyId, int UserId, int ProgramId, int LobId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPASA(ClsPubSOASelectionCriteria selection);

        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetTransactionDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs, out decimal OpeningBalance);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSummaryDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMemorandumDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSOAAssetDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs, out decimal OpeningBalance);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSummaryAccountDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetMemorandumAccountDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs);


        #endregion

        #region Enquiry

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubEnQGetLOB(int CompanyId, int UserId, bool Is_Active,int ProgramId);
        #endregion

        #region Customer At A Glance

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRegionDetails(int CompanyId, int UserId);
        
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustomerBasedLOB(int CompanyId, int UserId, bool Is_Active, string CustomerId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetProductDetails(int CompanyId,int LobId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetBranchDetails(int CompanyId, int UserId, int RegionId, Boolean IsActive);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustomerAtAGlanceDetails(byte[] byteHeaderDetails,byte[] PASAs);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCustomerGlanceAssetDetails(int CompanyId, string StartDate, string EndDate, byte[] PASAs);

        #endregion

        #region Disbursementreport
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLOB(int CompanyId, int UserId,int ProgramId);
        

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRegion(int CompanyId, bool Is_Active,int UserId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRegBranch(int CompanyId, int UserId, string Region_Id, bool Is_Active);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetLocation2(int ProgramId, int UserId, int CompanyId, int LobId, int LocationId);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetDisburseDetails(ClsPubDisburseSelectionCriteria disburseSelectionCriteria);

        #endregion

        #region Sanction Details

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] GetDenominations();

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSanctionDetails(byte[] ObjSanctionBytes);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetSanctionReport(byte[] ObjSanction);

        #endregion

        #region BranchPerformance
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetStockDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetPayment(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetNPADetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTotalNPAAddtionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTotalNPAOpeningaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTotalNPADeletionaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetTotalNPAClosingaccounts(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCumulativeCollectionDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetAccountDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRegionBranchDetails(ClsPubBranchInputSelectionCriteria branchSelectionCriteria);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetVendorDetails(int CompanyId, string EntityCode, string StartDate, string EndDate,decimal Denomination, out decimal OpeningBalance);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetJournalDetails(byte[] ObjJournal);
        #endregion

        #region CollateralDetails
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollateralDetails(byte[] byteHeader);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetCollateralType(int CompanyID);

        #endregion


        #region Disbursementreport
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetFactoringMaturityDetails(ClsPubFactoringMaturitySelectionCriteria factoringSelectionCriteria);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubLOB_FT(int CompanyId, int UserId, int ProgramId);


        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetFactoringBusinessDetails(ClsPubFactoringBusinessSelectionCriteria factoringBusinessSelectionCriteria);


        //[OperationContract]
        //[FaultContract(typeof(ClsPubFaultException))]
        //byte[] FunPubGetFactoringInterestDetails(ClsPubFactoringInterestSelectionCriteria factoringInterestSelectionCriteria);


        #endregion

        #region Repossession Report
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        byte[] FunPubGetRepossessionDetails(int LOBID, int Location, string FromDate, string ToDate, int CompanyID, int GaragecNo, int ProgramID, int UserID);
        #endregion
    }
}
