using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using S3GBusEntity;

namespace S3GServiceLayer
{
    [ServiceContract]
    public interface ICommonMail
    {
        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
         void FunSendMail(ClsPubCOM_Mail ObjCOM_Mail_SERLAY);

        [OperationContract]
        [FaultContract(typeof(ClsPubFaultException))]
         string[] GetSuggestion(string prefixText, int count);

    }
}