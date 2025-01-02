using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCreditScoreDetails
    {
        [DataMember]
        public string BRANCH_ID { get; set; }

        [DataMember]
        public int PRODUCT_ID { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string ECAType { get; set; }

        [DataMember]
        public int NoofAccounts { get; set; }

        [DataMember]
        public decimal RequiredScore { get; set; }

        [DataMember]
        public decimal ActualScore { get; set; }

        [DataMember]
        public decimal Hygiene { get; set; }

        [DataMember]
        public decimal Accepted { get; set; }

        [DataMember]
        public decimal Rejected { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }
    }
}
