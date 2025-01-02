using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubFactoringBusiness
    {
        [DataMember]
        public List<ClsPubFactoringBusinessDetails> FactoringBusiness { get; set; }

        [DataMember]
        public List<ClsPubFactoringBusinessReport> FactoringBusinessReport { get; set; }


    }
}
