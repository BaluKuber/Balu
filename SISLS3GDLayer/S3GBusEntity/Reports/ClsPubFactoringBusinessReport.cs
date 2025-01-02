using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubFactoringBusinessReport
    {
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string PrimeAccount { get; set; }

        [DataMember]
        public string SubAccount { get; set; }

        [DataMember]
        public string FILNo { get; set; }

        [DataMember]
        public string FILDate { get; set; }

        [DataMember]
        public int CreditDays { get; set; }

        [DataMember]
        public string MaturityDate { get; set; }

        [DataMember]
        public decimal PrinAmount { get; set; }

        [DataMember]
        public string VendorName { get; set; }

        [DataMember]
        public string InvoiceNo { get; set; }

        [DataMember]
        public decimal InvoiceAmount { get; set; }

        [DataMember]
        public decimal MarginAmount { get; set; }

        [DataMember]
        public string DiscountDate { get; set; }

        [DataMember]
        public decimal Credit_Amount { get; set; }

        [DataMember]
        public decimal CreditAvailable { get; set; }

        [DataMember]
        public decimal DISCOUNT_AMOUNT { get; set; }

        [DataMember]
        public decimal INTEREST_RATE { get; set; }    

        [DataMember]
        public bool IsDetails { get; set; }
    }
}
