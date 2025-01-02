using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSummary
    {
        [DataMember]
        public decimal InstallmentDues { get; set; }

        [DataMember]
        public decimal InterestDues { get; set; }

        [DataMember]
        public decimal InsuranceDues { get; set; }

        [DataMember]
        public decimal  ODIDues { get; set; }

        [DataMember]
        public decimal OtherDues { get; set; }

        
    }
}
