using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDropDownList
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Description { get; set; }

    }
}
