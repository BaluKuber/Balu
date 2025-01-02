using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCollectionDetails
    {
        [DataMember]
        public decimal TotalCollectionAmount { get; set; }
        [DataMember]
        public decimal CurrentCollection { get; set; }
        [DataMember]
        public decimal ArrearsCollection { get; set; }
        [DataMember]
        public decimal Insurance { get; set; }
        [DataMember]
        public decimal Interest { get; set; }
        [DataMember]
        public decimal OverDueInterest { get; set; }
        [DataMember]
        public decimal MemoCharges { get; set; }
        [DataMember]
        public decimal Others { get; set; }
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string PrimeAccountNumber { get; set; }
        [DataMember]
        public string SubAccountNumber { get; set; }
        [DataMember]
        public string ReceiptNumber { get; set; }
        [DataMember]
        public string ReceiptDate { get; set; }
        [DataMember]
        public decimal ReceiptAmount { get; set; }
        [DataMember]
        public string GPSSuffix { get; set; }

    }
}
