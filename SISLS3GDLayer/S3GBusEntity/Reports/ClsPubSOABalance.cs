using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSOABalance
    {
        [DataMember]
        public string SOABalance { get; set; }
    }
}
