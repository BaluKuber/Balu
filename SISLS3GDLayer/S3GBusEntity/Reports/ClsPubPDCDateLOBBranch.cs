using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPDCDateLOBBranch
    {
        //[DataMember]
        //public string PDC_Date { get; set; }

        [DataMember]
        public string LOBId { get; set; }

        [DataMember]
        public string BranchId { get; set; }
    }
}
