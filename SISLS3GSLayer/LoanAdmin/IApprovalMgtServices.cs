using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace S3GServiceLayer.LoanAdmin
{
    // NOTE: If you change the interface name "IApprovalMgtServices" here, you must also update the reference to "IApprovalMgtServices" in Web.config.
    [ServiceContract]
    public interface IApprovalMgtServices
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateApprovals(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubRevokeOrUpdateApprovedDetails(Dictionary<string, string> dicParam);

        #region "Loan End Use Approval"

        #region Create

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateOrModifyLoanEndUseApproval(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjLoanEndUseApproval_DataTable, out string strEndUseNumber, out int intApproval_No);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateRVMatrix(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjLoanEndUseApproval_DataTable, out string strRVMCode);

        #endregion

        #endregion
    }
}
