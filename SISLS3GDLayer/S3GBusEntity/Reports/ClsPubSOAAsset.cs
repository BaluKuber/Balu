using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{

    [Serializable]
    [DataContract]
    public sealed class ClsPubSOAAsset
    {
        //[DataMember]
        //public List<ClsPubSOABalance> SOABalance { get; set; }

        [DataMember]
        public List<ClsPubAsset> AssetDetails { get; set; }

        [DataMember]
        public List<ClsPubAsset> AccountDetails { get; set; }

        [DataMember]
        public List<ClsPubTransaction> Transaction { get; set; }

        [DataMember]
        public List<ClsPubSOAOpeningBalance > Openingbalance { get; set; }
    }
}
