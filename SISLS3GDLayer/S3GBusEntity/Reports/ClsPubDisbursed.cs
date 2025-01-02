using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public  class ClsPubDisbursed
    {
        [DataMember]
        public int ApplicationProcessId { get; set; }

        [DataMember]
        public string PrimeAccount { get; set; }

        [DataMember]
        public string SubAccount { get; set; }

        [DataMember]
        public decimal PaidAmount { get; set; }


    }
}
