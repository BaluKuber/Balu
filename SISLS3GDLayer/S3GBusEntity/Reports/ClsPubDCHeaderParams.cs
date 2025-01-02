using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDCHeaderParams
    {
        [DataMember]
        public string Customer_ID { get; set; }

        [DataMember]
        public string Lob_ID { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string Group_ID { get; set; }

        [DataMember]
        public string Option { get; set; }

        [DataMember]
        public string Company_ID { get; set; }

        
    }
}
