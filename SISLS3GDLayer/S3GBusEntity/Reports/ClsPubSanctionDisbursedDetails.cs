using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSanctionDisbursedDetails
    {
   
        [DataMember]
        public int APPLICATION_PROCESS_ID { get; set; }

        [DataMember]
        public string PANUM { get; set; }

        [DataMember]
        public string SANUM { get; set; }

        [DataMember]
        public string DISBURSED_NO { get; set; }

        [DataMember]
        public string DISBURSED_DATE { get; set; }

        [DataMember]
        public decimal DISBURSED_AMOUNT { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }

    }
}
