using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubAssetCategories
    {
        [DataMember]
        public int ClassId { get; set; }

        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public int MakeId { get; set; }

        [DataMember]
        public string AssetMake { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public string AssetType { get; set; }

    }
}
