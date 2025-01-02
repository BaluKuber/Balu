using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLobLocation
    {
        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Lan { get; set; }
    }
}
