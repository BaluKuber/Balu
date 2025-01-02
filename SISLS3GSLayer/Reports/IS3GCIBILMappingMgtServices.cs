using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer.Reports
{
    // NOTE: If you change the interface name "IS3GCIBILMappingMgtServices" here, you must also update the reference to "IS3GCIBILMappingMgtServices" in Web.config.
    [ServiceContract]
    public interface IS3GCIBILMappingMgtServices
    {
        #region CIBILMapping
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
        int FunCreateCIBILMapping(SerializationMode SerMode, byte[] byteCIBILMapping_DataTable, out string strErrMsg);
        #endregion

    }
}
