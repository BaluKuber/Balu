using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{

    [Serializable]
    [DataContract]
    public class ClsPubSOASelectionCriteria
    {
        [DataMember]
        public  int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string LobId { get; set; }

        [DataMember]
        public string LocationID1 { get; set; }

        [DataMember]
        public string LocationID2 { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int ProgramId { get; set; }

        [DataMember]
        public string ProductId { get; set; }
    }
}
