using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDisbursement
    {
        [DataMember]
        public List<ClsPubDisbursementDetails> Disbursement { get; set; }

        [DataMember]
        public List<ClsPubLobLocation> loblocation { get; set; }
    }
}
