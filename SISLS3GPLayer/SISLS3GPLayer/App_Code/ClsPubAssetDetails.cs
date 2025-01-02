using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3GBusEntity.Reports;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [DataContract]
    public class ClsPubAssetDetails
    {
        [DataMember]
        public string AssetDetails { get; set; }

        [DataMember]
        public string SLRegNo { get; set; }

        [DataMember]
        public decimal AmountFinanced { get; set; }

        [DataMember]
        public string Terms { get; set; }

        [DataMember]
        public string PolicyNo { get; set; }

        [DataMember]
        public DateTime ValidUpto { get; set; }

        [DataMember]
        public string Insurer { get; set; }

        [DataMember]
        public decimal PolicyAmount { get; set; }
    }
}
