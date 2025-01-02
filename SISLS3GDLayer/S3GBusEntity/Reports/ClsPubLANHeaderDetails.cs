using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLANHeaderDetails
    {
        [DataMember]
        public string LOB_ID { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public string PrimeAccountNo { get; set; }

        [DataMember]
        public string SubAccountNo { get; set; }

        [DataMember]
        public string LanNoFrom { get; set; }

        [DataMember]
        public string LanNoTo { get; set; }

        [DataMember]
        public string DateFrom { get; set; }

        [DataMember]
        public string DateTo { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        [DataMember]
        public string LOCATION_ID1 { get; set; }

        [DataMember]
        public string LOCATION_ID2 { get; set; }

    }
}
