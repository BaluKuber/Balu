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

namespace S3GServiceLayer.LoanAdmin
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "ApprovalMgtServices" here, you must also update the reference to "ApprovalMgtServices" in Web.config.
    public class ApprovalMgtServices : IApprovalMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;
        byte[] bytesDataTable;
        #region IApprovalMgtServices Members

        public int FunPubCreateApprovals(S3GBusEntity.SerializationMode SerMode, byte[] bytesObjApprovalDatatable_SERLAY, out string strErrMsg)
        {
            try
            {
                S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals ObjApproval = new S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals();
                return ObjApproval.FunPubCreateApprovals(SerMode, bytesObjApprovalDatatable_SERLAY, out strErrMsg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubRevokeOrUpdateApprovedDetails(Dictionary<string, string> dicParam)
        {
            try
            {
                S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals ObjApproval = new S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals();
                return ObjApproval.FunPubRevokeOrUpdateApprovedDetails(dicParam);

            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        #endregion

        #region "Loan End Use Approval"

        #region Create
        public int FunPubCreateOrModifyLoanEndUseApproval(SerializationMode SerMode, byte[] bytesObjLoanEndUseApproval_DataTable, out string strEndUseNumber, out int intApproval_No)
        {

            try
            {
                strEndUseNumber = "";
                intApproval_No = 0;
                S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubLoanEndUseApproval ObjLoanEndUseApproval = new S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubLoanEndUseApproval();
                return ObjLoanEndUseApproval.FunPubCreateOrModifyLoanEndUseApproval(SerMode, bytesObjLoanEndUseApproval_DataTable, out strEndUseNumber, out intApproval_No);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubCreateRVMatrix(SerializationMode SerMode, byte[] bytesObjS3G_ORG_Enityt_MasterDataTable, out string strRVMCode)
        {
            try
            {
                S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals objRVMatrix = new S3GDALayer.LoanAdmin.ApprovalMgtServices.ClsPubApprovals();
                intResult = objRVMatrix.FunPubCreateRVMatrix(SerMode, bytesObjS3G_ORG_Enityt_MasterDataTable, out strRVMCode);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Creating RV Matrix :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return intResult;
        }

       
        #endregion

        #endregion
    
    }
}
