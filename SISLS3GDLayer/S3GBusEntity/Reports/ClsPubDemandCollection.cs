using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDemandCollection
    {
        [DataMember]
        public string Frequency { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public string DemandMonth { get; set; }

        [DataMember]
        public int RegionId { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public int BranchId { get; set; }


        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

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

        [DataMember]
        public string Panum { get; set; }

        [DataMember]
        public string Sanum { get; set; }

        [DataMember]
        public Decimal ArrearsAmount { get; set; }

        [DataMember]
        public Decimal OpeningDemand { get; set; }

        [DataMember]
        public Decimal OpeningCollection { get; set; }

        [DataMember]
        public Decimal OpeningPercentage { get; set; }

        [DataMember]
        public Decimal MonthlyDemand { get; set; }

        [DataMember]
        public Decimal MonthlyCollection { get; set; }

        [DataMember]
        public Decimal MonthlyPercentage { get; set; }

        [DataMember]
        public Decimal ClosingDemand { get; set; }

        [DataMember]
        public Decimal ClosingCollection { get; set; }

        [DataMember]
        public Decimal ClosingPercentage { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }

    }
}
