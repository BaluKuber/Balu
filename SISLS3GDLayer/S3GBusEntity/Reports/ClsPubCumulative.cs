using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCumulative
    {
        [DataMember]
        public decimal Cumm { get; set; }

        [DataMember]
        public int CA_Exists { get; set; }

        public List<ClsPubPaymentDCNPADetails> PaymentDCNPADetails { get; set; }
    }
}
