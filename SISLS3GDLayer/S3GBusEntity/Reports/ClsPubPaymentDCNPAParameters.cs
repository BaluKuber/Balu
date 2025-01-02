using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPaymentDCNPAParameters
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
        public string ToMonthStartDate { get; set; }

        [DataMember]
        public string ToMonthEndDate { get; set; }

        [DataMember]
        public string PreToMonthStartDate { get; set; }

        [DataMember]
        public string PreToMonthEndDate { get; set; }

        [DataMember]
        public string FromMonthStartDate { get; set; }

        [DataMember]
        public string PreYearFromMonthStartDate { get; set; }

        [DataMember]
        public string PreYearToMonthEndDate { get; set; }

        [DataMember]
        public string FinYearStartMonthStartDate { get; set; }

        [DataMember]
        public string PreFromMonth { get; set; }

        [DataMember]
        public string FromMonth { get; set; }

        [DataMember]
        public string ToMonth { get; set; }

        [DataMember]
        public string OpeningMonth { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        public ClsPubPaymentDCNPAParameters()
        {
            Denomination = 1;
        }
    }
}
