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

namespace S3GServiceLayer.Origination
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FileImport" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FileImport.svc or FileImport.svc.cs at the Solution Explorer and start debugging.
    public class FileImport : IFileImport
    {
        public int FunPubCreateFileUpload(SerializationMode SerMode, byte[] bytesObjFileUploadDatatable_SERLAY, out int Upload_ID)
        {
            try
            {
                S3GDALayer.Origination.ClsPubFileImport ObjFileUpload = new S3GDALayer.Origination.ClsPubFileImport();
                return ObjFileUpload.FunPubCreateFileUploadInt(SerMode, bytesObjFileUploadDatatable_SERLAY,out Upload_ID);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

        public int FunPubSaveFileUpload(SerializationMode SerMode, byte[] bytesObjFileSaveDatatable_SERLAY, out string LSQ_Number, out string Error_Msg)
        {
            try
            {
                S3GDALayer.Origination.ClsPubFileImport ObjFileUploadSave = new S3GDALayer.Origination.ClsPubFileImport();
                return ObjFileUploadSave.FunPubSaveFileUploadInt(SerMode, bytesObjFileSaveDatatable_SERLAY,out LSQ_Number,out Error_Msg);
            }
            catch (Exception objExp)
            {
                ClsPubFaultException objFault = new ClsPubFaultException();
                objFault.ProReasonRW = "Error in :" + objExp.Message.ToString();
                throw new FaultException<ClsPubFaultException>(objFault, new FaultReason(objFault.ProReasonRW));
            }
        }

    }
}
