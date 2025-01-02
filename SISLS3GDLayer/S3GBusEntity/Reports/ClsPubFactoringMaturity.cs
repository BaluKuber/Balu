using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubFactoringMaturity
    {
        [DataMember]
        public List<ClsPubFactoringMaturityDetails> FactoringMaturity { get; set; }

        //[DataMember]
        //public List<ClsPubFactoringMaturityReport> FactoringMaturityReport { get; set; }

    }
}
