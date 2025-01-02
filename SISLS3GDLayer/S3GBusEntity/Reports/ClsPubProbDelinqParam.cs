using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubProbDelinqParam
    {
        [DataMember]
        public int Company_Id { get; set; }

        [DataMember]
        public int Lob_Id { get; set; }

        [DataMember]
        public int Location_Id { get; set; }

        [DataMember]
        public string DemandMonth { get; set; }
    }
}
