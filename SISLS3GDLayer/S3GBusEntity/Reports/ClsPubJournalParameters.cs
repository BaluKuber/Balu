using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubJournalParameters
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
        public string Panum { get; set; }

        [DataMember]
        public string Sanum { get; set; }

        [DataMember]
        public string LeaseAssetNo { get; set; }

        [DataMember]
        public string GlCode { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public int Denomination { get; set; }

        public ClsPubJournalParameters()
        {
            Denomination = 1;
        }

    }
}
