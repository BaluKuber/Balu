using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDemandSpooling
    {
        [DataMember]
        public string DemandMonth { get; set; }

        [DataMember]
        public int LobId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int RegionId { get; set; }
    }
}
