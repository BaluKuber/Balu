using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDPDInputParam
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string LOBID { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public decimal denomination { get; set; }

        [DataMember]
        public int BucketCount { get; set; }

        


    }
}
