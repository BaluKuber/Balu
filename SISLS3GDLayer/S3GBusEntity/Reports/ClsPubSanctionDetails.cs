using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubSanctionDetails
    {
        [DataMember]
        public int APPLICATION_PROCESS_ID { get; set; }

        [DataMember]
        public string REGION { get; set; }

        [DataMember]
        public string BRANCH { get; set; }

        [DataMember]
        public string PRODUCT_NAME { get; set; }

        [DataMember]
        public string APPLICATIONNO { get; set; }

        [DataMember]
        public string CUSTOMERNAME { get; set; }

        [DataMember]
        public string PANUM { get; set; }

        [DataMember]
        public string SANUM { get; set; }

        [DataMember]
        public Decimal FINANCEAMOUNTSOUGHT { get; set; }

        [DataMember]
        public Decimal FINANCEAMOUNTOFFERED { get; set; }

        [DataMember]
        public Decimal DISBURSABLE_AMOUNT { get; set; }

        [DataMember]
        public decimal DISBURSED_AMOUNT { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        public List<ClsPubSanctionDisbursableDetails> Disbursable { get; set; }
        public List<ClsPubSanctionDisbursedDetails> Disbursed { get; set; }

        public ClsPubSanctionDetails()
        {
            Disbursable = new List<ClsPubSanctionDisbursableDetails>();
            Disbursed = new List<ClsPubSanctionDisbursedDetails>();
        }
    }
}
