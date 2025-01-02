using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDemandParameterDetails
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int ProgramId { get; set; }

        [DataMember]
        public int LocationId1 { get; set; }

        [DataMember]
        public int LocationId2 { get; set; }


        [DataMember]
        public string LobId { get; set; }

        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public string GroupId { get; set; }

        [DataMember]
        public string RegionId { get; set; }

        [DataMember]
        public string RegionName { get; set; }

        [DataMember]
        public string ClassId { get; set; }

        [DataMember]
        public string BranchId { get; set; }

        [DataMember]
        public string BranchName { get; set; }

        [DataMember]
        public string FrequencyId { get; set; }

        [DataMember]
        public string GroupingCriteriaId { get; set; }

        [DataMember]
        public string GroupingCriteriaName { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string SANum { get; set; }

        [DataMember]
        public int AssetTypeId { get; set; }

        [DataMember]
        public string FromMonth { get; set; }

        [DataMember]
        public string ToMonth { get; set; }

        [DataMember]
        public string PreFromMonth { get; set; }

        [DataMember]
        public string PreToMonth { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        [DataMember]
        public DateTime FinYearStartMonthStartDate { get; set; }

        [DataMember]
        public DateTime FromMonthStartDate { get; set; }

        [DataMember]
        public DateTime FromMonthPreMonthEndDate { get; set; }

        [DataMember]
        public DateTime ToMonthEndDate { get; set; }

        [DataMember]
        public DateTime? CompareFinYearStartMonthStartDate { get; set; }

        [DataMember]
        public DateTime? CompareFromMonthStartDate { get; set; }

        [DataMember]
        public DateTime? CompareFromMonthPreMonthEndDate { get; set; }

        [DataMember]
        public DateTime? CompareToMonthEndDate { get; set; }

        public ClsPubDemandParameterDetails()
        {
            Denomination = 1;
        }
    }
}
