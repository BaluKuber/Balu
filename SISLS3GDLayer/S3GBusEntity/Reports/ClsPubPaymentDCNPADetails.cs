using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPaymentDCNPADetails
    {
       
        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public int LobId { get; set; }

        [DataMember]
        public string Lob { get; set; }

        [DataMember]
        public int RegionId { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string Product { get; set; }


        [DataMember]
        public Decimal GrowthPercentageLastMonth { get; set; }

        [DataMember]
        public Decimal Cumulative { get; set; }

        [DataMember]
        public Decimal GrowthPercentageLastYear { get; set; }

        [DataMember]
        public Decimal ArrearDemand { get; set; }

        [DataMember]
        public Decimal ArrearCollection { get; set; }

        [DataMember]
        public Decimal ArrearCollectionPercentage { get; set; }

        [DataMember]
        public Decimal CurrentDemand { get; set; }

        [DataMember]
        public Decimal CurrentCollection { get; set; }

        [DataMember]
        public Decimal CurrentPercentage { get; set; }

        [DataMember]
        public Decimal TotalDemand { get; set; }

        [DataMember]
        public Decimal TotalCollection { get; set; }

        [DataMember]
        public Decimal TotalPercentage { get; set; }

        [DataMember]
        public Decimal BadDebts { get; set; }

        [DataMember]
        public Decimal Stock { get; set; }

        [DataMember]
        public Decimal Npa { get; set; }

        [DataMember]
        public Decimal NpaPercentage { get; set; }

        [DataMember]
        public Decimal ClosingArrear { get; set; }

        [DataMember]
        public Decimal ArrearPercentage { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }


    }
}
