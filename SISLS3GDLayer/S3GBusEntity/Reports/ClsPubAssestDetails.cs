using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubAssestDetails
    {
        [DataMember]
        public string AssetDetails { get; set; }

        [DataMember]
        public string SLRegNo { get; set; }

        [DataMember]
        public decimal AmountFinanced { get; set; }

        [DataMember]
        public decimal Irr { get; set; }


        [DataMember]
        public string Terms { get; set; }

        [DataMember]
        public string PolicyNo { get; set; }

        [DataMember]
        public string ValidUpto { get; set; }

        [DataMember]
        public string Insurer { get; set; }

        [DataMember]
        public decimal PolicyAmount { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }
    }
}
