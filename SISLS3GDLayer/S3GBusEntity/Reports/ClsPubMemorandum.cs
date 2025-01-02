using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubMemorandum
    {
   

        [DataMember]
        public decimal ChequeReturnDues { get; set; }

        [DataMember]
        public decimal DocumentChargesDues { get; set; }

        [DataMember]
        public decimal ODIDues { get; set; }


        [DataMember]
        public decimal VerificationChargesDues { get; set; }

        [DataMember]
        public decimal OtherDues { get; set; }


    }
}
