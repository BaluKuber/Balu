using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDemandStatement
    {
        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string CustomerCode { get; set; }

        [DataMember]
        public string PrimeAccountNo { get; set; }

        [DataMember]
        public string SubAccountNo { get; set; }

        [DataMember]
        public string DebtCollectorCode { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public decimal DueAmount { get; set; }

        [DataMember]
        public decimal ageing0days { get; set; }

        [DataMember]
        public decimal ageing30days { get; set; }

        [DataMember]
        public decimal ageing60days { get; set; }

        [DataMember]
        public decimal ageingabove90days { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }


              
    }
}
