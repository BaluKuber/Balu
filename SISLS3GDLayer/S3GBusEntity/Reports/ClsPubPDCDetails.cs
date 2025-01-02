using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPDCDetails
    {
        [DataMember]
        public int PDCSNo { get; set; }

        [DataMember]
        public long ChequeNumber { get; set; }

        [DataMember]
        public string ChequeDate { get; set; }

        [DataMember]
        public string DrawnonBank { get; set; }

        [DataMember]
        public string BankingDate { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }
    }
}
