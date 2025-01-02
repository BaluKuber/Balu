using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSOAOpeningBalance
    {
        [DataMember]
        public string PrimeAccountNo { get; set; }

        [DataMember]
        public string SubAccountNo { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }
    }
}
