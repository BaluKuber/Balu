using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCustomer
    {
        [DataMember]
        public string Customer { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string WebSite { get; set; }
    }
}
