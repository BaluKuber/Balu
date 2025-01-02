using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using S3GDALayer.Origination;
using System.Data;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.Origination
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "CreditParameterMgtServices" here, you must also update the reference to "CreditParameterMgtServices" in Web.config.
    public class CreditParameterMgtServices : ICreditParameterMgtServices
    {
        byte[] bytesDataTable;
        public int FunPubInsertCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransactionEntity, out string CreditParamNumber)
        {
            try
            {
                ClsPubCreditParamTransaction ObjCreditParamTransactionDAL = new ClsPubCreditParamTransaction();
                return ObjCreditParamTransactionDAL.FunPubInsertCreditParamTransaction(ObjCreditParameterTransactionEntity, out CreditParamNumber);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in InsertCreditParamTransaction:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public void FunPubInsertCreditParamTransactionScoreDetails(CreditParamterTransScoreDTO ObjCreditParamterTransScoreDTO)
        {
            try
            {
                ClsPubCreditParamTransaction ObjCreditParamTransactionDAL = new ClsPubCreditParamTransaction();
                ObjCreditParamTransactionDAL.FunPubInsertCreditParamTransactionScoreDetails(ObjCreditParamterTransScoreDTO);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in InsertCreditParamTransaction:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

      

        public byte[] FunPubGetEnquiryCustomerAppraisalCPT(int intAppraisalType, int intCompanyId, int intAppraisalId, int intPageSize, int intCurrentPage, string strSearchValue, string strOrderBy, int intTotalRecords)
        {
            DataSet ds;
            try
            {
                ClsPubCreditParamTransaction objCreditParameterTransaction = new ClsPubCreditParamTransaction();
                ds = (DataSet)objCreditParameterTransaction.FunPubGetEnquiryCustomerAppraisalCPT(intAppraisalType, intCompanyId, intAppraisalId, intPageSize, intCurrentPage, strSearchValue, strOrderBy, intTotalRecords);
                if (ds != null)
                    bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in InsertCreditParamTransaction:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }



        public byte[] FunPubGetCreditParameterTransaction(int intCreditParamTransId)
        {
            DataSet ds;
            try
            {
                ClsPubCreditParamTransaction objCreditParameterTransaction = new ClsPubCreditParamTransaction();
                ds = (DataSet)objCreditParameterTransaction.FunPubGetCreditParameterTransaction(intCreditParamTransId);
                if (ds != null)
                    bytesDataTable = ClsPubSerialize.Serialize(ds, SerializationMode.Binary);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in InsertCreditParamTransaction:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
            return bytesDataTable;
        }

        public int FunPubModifyCreditParamTransaction(CreditParameterTransactionEntity ObjCreditParameterTransactionEntity)
        {
            try
            {
                ClsPubCreditParamTransaction ObjCreditParamTransactionDAL = new ClsPubCreditParamTransaction();
                return ObjCreditParamTransactionDAL.FunPubModifyCreditParamTransaction(ObjCreditParameterTransactionEntity);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in InsertCreditParamTransaction:" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

    }
}
