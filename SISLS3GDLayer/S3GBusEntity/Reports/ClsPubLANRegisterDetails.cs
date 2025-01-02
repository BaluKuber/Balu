using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLANRegisterDetails
    {
         [DataMember]
        public string DocumentDate { get; set; }

        [DataMember]
         public string DocumentNo { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Lan { get; set; }

        [DataMember]
        public string Panum { get; set; }

        [DataMember]
        public string Sanum { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ValueDate { get; set; }

        [DataMember]
        public string Value_Date { get; set; }

        [DataMember]
        public decimal Debit { get; set; }

        [DataMember]
        public decimal Credit { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public string Denomination { get; set; }
 
        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Location { get; set; }

    }
}
