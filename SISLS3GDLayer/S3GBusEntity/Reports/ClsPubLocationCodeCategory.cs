using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLocationCodeCategory
    {
        [DataMember]
        public string LocationCode { get; set; }

        [DataMember]
        public string LocationName { get; set; } 
    }
}
