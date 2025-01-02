using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubHeaderLobBranchProductDetails
    {
        [DataMember]
        public int LobId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

    }
}
