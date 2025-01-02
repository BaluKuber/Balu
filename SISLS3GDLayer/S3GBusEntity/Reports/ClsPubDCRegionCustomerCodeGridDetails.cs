using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public sealed class ClsPubDCRegionCustomerCodeGridDetails
    {
        [DataMember]
        public string Frequency { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public string DemandMonth { get; set; }
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string LobId { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string RegionId { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerGroupCode { get; set; }
        [DataMember]
        public string PrimeAccountNo { get; set; }
        [DataMember]
        public string SubAccountNo { get; set; }
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
    }
}
