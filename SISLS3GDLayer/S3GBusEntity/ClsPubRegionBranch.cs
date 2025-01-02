using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubRegionBranch
    {
        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }
    }
}
