using S3GBusEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GDALayer;

namespace S3GServiceLayer.Origination
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFileImport" in both code and config file together.
    [ServiceContract]
    public interface IFileImport
    {
       [OperationContract]
        int FunPubCreateFileUpload(SerializationMode SerMode, byte[] bytesObjFileUploadDatatable_SERLAY, out int Upload_ID);

       [OperationContract]
       int FunPubSaveFileUpload(SerializationMode SerMode, byte[] bytesObjFileSaveDatatable_SERLAY, out string LSQ_Number,out string Error_Msg);
    }
}
