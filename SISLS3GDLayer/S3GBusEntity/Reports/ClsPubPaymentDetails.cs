using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPaymentDetails
    {
        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public decimal AssetClassMonth { get; set; }

        [DataMember]
        public decimal AssetClassYTM { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }


    }
}
