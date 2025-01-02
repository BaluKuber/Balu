using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSanctionParameterDetails
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int ProgramId { get; set; }

        [DataMember]
        public int LobId { get; set; }

        [DataMember]
        public int LocationId1 { get; set; }

        [DataMember]
        public int LocationId2 { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public bool IsDetail { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        public ClsPubSanctionParameterDetails()
        {
            Denomination = 1;
        }

    }
}
