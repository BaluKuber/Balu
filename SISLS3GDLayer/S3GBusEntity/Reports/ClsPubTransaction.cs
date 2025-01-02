using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubTransaction
    {
       [DataMember]
        public string PrimeAccountNo { get; set; }

       [DataMember]
       public string Due_Flag { get; set; }

        [DataMember]
       public string SubAccountNo { get; set; }

        [DataMember]
        public string DocumentDate{ get; set; }

        [DataMember]
        public string ValueDate{ get; set; }

        [DataMember]
        public string DocumentReference{ get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Dues { get; set; }

        [DataMember]
        public decimal Receipts { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        //For Journal Query
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string GlAccount { get; set; }

        [DataMember]
        public string Lan{ get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Narration { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }
        }
}
