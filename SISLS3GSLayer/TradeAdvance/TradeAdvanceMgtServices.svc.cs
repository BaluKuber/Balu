using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;
using System.Data;
using S3GDALayer.Collection;
using System.ServiceModel.Activation;

namespace S3GServiceLayer.TradeAdvance
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // NOTE: If you change the class name "TradeAdvanceMgtServices" here, you must also update the reference to "TradeAdvanceMgtServices" in Web.config.
    public class TradeAdvanceMgtServices : ITradeAdvanceMgtServices
    {
        #region Scheme Master
        public int FunPubTradeAdvanceAdd(SerializationMode SerMode, byte[] bytesObjTradeAdvanceDataTable_Add)
        {
            try
            {

                S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubSchemeMaster ObjTradeAdvanceServices = new S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubSchemeMaster();
                return ObjTradeAdvanceServices.FunPubTradeAdvance(SerMode, bytesObjTradeAdvanceDataTable_Add);
            }

            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Create TradeAdvance :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion

        #region Application Process
        public int FunPubCreateApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable, out string strApplilcationNumber)
        {
            try
            {
                S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubApplicationProcess ObjApplicationProcess = new S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubApplicationProcess();
                return ObjApplicationProcess.FunPubCreateApplicationProcess(SerMode, bytesObjS3G_TA_ApplicationProcDataTable, out strApplilcationNumber);
            }

            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Create TradeAdvance :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubApproveApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable)
        {
            try
            {
                S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubApplicationProcess ObjApplicationProcess = new S3GDALayer.TradeAdvance.TradeAdvanceMgtServices.ClsPubApplicationProcess();
                return ObjApplicationProcess.FunPubApproveApplicationProcess(SerMode, bytesObjS3G_TA_ApplicationProcDataTable);
            }

            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in Create TradeAdvance :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }
        #endregion
    }
}
