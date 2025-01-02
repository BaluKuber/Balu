using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubSanctionDisbursableDetails
    {
        [DataMember]
        public int APPLICATION_PROCESS_ID { get; set; }

        [DataMember]
        public string PANUM { get; set; }

        [DataMember]
        public string SANUM { get; set; }

        [DataMember]
        public string DISBURSABLE_DATE { get; set; }

        [DataMember]
        public decimal DISBURSABLE_AMOUNT { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }

    }
}
