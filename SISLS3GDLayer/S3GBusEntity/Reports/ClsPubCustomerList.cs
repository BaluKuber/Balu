using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubCustomerList
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerMail { get; set; }
    }
}
