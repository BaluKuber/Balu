using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubFactoringMaturityDetails
    {
        //[DataMember]
        //public int ApplicationProcessId { get; set; }

  
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
        public bool IsDetails { get; set; }

        [DataMember]
        public string INPUTDATE_1 { get; set; }
                        


        
        //[DataMember]
        //public List<ClsPubDisbursed> Disbursed { get; set; }
    }
}
