using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDPDDetails
    {
        [DataMember]
        public int Bucket { get; set; }

        [DataMember]
        public int DaysFrom { get; set; }

        [DataMember]
        public int DaysTo { get; set; }

    }
}
