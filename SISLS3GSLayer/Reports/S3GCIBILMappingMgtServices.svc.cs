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

namespace S3GServiceLayer.Reports
{
    // To Implement Service Compatablity
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: If you change the class name "S3GCIBILMappingMgtServices" here, you must also update the reference to "S3GCIBILMappingMgtServices" in Web.config.
    public class S3GCIBILMappingMgtServices : IS3GCIBILMappingMgtServices
    {
        int intResult;
        SerializationMode SerMode = SerializationMode.Binary;

        byte[] bytesDataTable;
        public int FunCreateCIBILMapping(SerializationMode SerMode, byte[] byteCIBILMapping_DataTable, out string strErrMsg)
        {
            try
            {
                S3GDALayer.Reports.CIBILMappingMgtServices.ClsPubCIBILMapping objCIBILMap = new S3GDALayer.Reports.CIBILMappingMgtServices.ClsPubCIBILMapping();
                return objCIBILMap.FunCreateCIBILMapping(SerMode, byteCIBILMapping_DataTable, out strErrMsg);
       
            }
            catch (Exception ex)
            {
                ClsPubFaultException objfaultex = new ClsPubFaultException();
                objfaultex.ProReasonRW = ex.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objfaultex, new FaultReason(objfaultex.ProReasonRW));
            }

        }

    }
}
