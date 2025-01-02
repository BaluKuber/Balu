using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubCustomerGlanceDetails
    {
        [DataMember]
        public string LOB { get; set; }
        //[DataMember]
        //public string Region { get; set; } 
        //[DataMember]
        //public string Branch { get; set; } 
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Product_ID { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Primeac { get; set; }
        [DataMember]
        public string Subac { get; set; }
        [DataMember]
        public Decimal AppliedAmt { get; set; }
        [DataMember]
        public Decimal CollateralValue { get; set; }
        [DataMember]
        public Decimal SancAmt { get; set; }
        [DataMember]
        public Decimal DisbursedAmount { get; set; }
        [DataMember]
        public Decimal GrossExposure { get; set; }
        [DataMember]
        public Decimal NetExposure { get; set; }
        [DataMember]
        public Decimal Dues { get; set; }
        [DataMember]
        public Decimal Collected { get; set; }
        [DataMember]
        public Decimal AverageDueDates { get; set; }
        [DataMember]
        public Decimal ODIDue { get; set; }
        [DataMember]
        public Decimal MemoDue { get; set; }
        [DataMember]
        public Decimal Others { get; set; }
        [DataMember]
        public Decimal Pending { get; set; }
        [DataMember]
        public Decimal GPSSUFFIX { get; set; }
    }
}
