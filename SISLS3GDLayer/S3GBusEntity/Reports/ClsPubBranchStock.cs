using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubBranchStock
    {
        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public decimal StockOnHire { get; set; }

        [DataMember]
        public decimal Arrears { get; set; }

        [DataMember]
        public decimal ArrearsStock { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }


    }
}
