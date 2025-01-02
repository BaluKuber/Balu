using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.TradeAdvance
{
    // NOTE: If you change the interface name "ITradeAdvanceMgtServices" here, you must also update the reference to "ITradeAdvanceMgtServices" in Web.config.
    [ServiceContract]
    public interface ITradeAdvanceMgtServices
    {
        #region Scheme Master
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubTradeAdvanceAdd(SerializationMode SerMode, byte[] bytesObjTradeAdvanceDataTable_Add);
        #endregion

        #region Application Process
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubCreateApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable, out string strApplilcationNumber);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunPubApproveApplicationProcess(SerializationMode SerMode, byte[] bytesObjS3G_TA_ApplicationProcDataTable);

        #endregion

    }
}
