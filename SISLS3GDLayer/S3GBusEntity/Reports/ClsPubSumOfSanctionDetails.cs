using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
  public class ClsPubSumOfSanctionDetails
    {
        [DataMember]
        public decimal TOTALFINANCEAMOUNTSOUGHT { get; set; }

        [DataMember]
        public decimal TOTALFINANCEAMOUNTOFFERED { get; set; }

        [DataMember]
        public decimal TOTALDISBURSABLE_AMOUNT { get; set; }

        [DataMember]
        public decimal TOTALDISBURSED_AMOUNT { get; set; }

        public List<ClsPubSanctionDetails> SanctionDetails { get; set; }

    }
}
