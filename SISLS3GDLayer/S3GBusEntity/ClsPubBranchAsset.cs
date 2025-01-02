using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubBranchAsset
    {
        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public decimal AllAssetsMonth { get; set; }

        [DataMember]
        public decimal AllAssetsYTM { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

     
    }
}
