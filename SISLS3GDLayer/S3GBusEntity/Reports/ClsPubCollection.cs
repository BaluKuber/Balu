using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{

    [Serializable]
    [DataContract]
    public class ClsPubCollection
    {
        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Year { get; set; }

        [DataMember]
        public decimal CurrentCollection { get; set; }

        [DataMember]
        public decimal ArrearCollection { get; set; }

        [DataMember]
        public decimal TotalCollection { get; set; }
    }
}
