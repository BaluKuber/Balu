using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubRepayDetails
    {
        [DataMember]
        public int InstallmentNo { get; set; }

        [DataMember]
        public string InstallmentDate { get; set; }

        [DataMember]
        public decimal InstallmentAmount { get; set; }

        [DataMember]
        public decimal PrincipalAmount { get; set; }

        [DataMember]
        public decimal FinanceCharges { get; set; }

        [DataMember]
        public decimal Umfc { get; set; }

        [DataMember]
        public decimal InsuranceAmount { get; set; }

        [DataMember]
        public decimal Others { get; set; }

        [DataMember]
        public decimal VatRecovery { get; set; }

        [DataMember]
        public decimal TaxSetOff { get; set; }

        [DataMember]
        public decimal Tax { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }
    
    }
}
