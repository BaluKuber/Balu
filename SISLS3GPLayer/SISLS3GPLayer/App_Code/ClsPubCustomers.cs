using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCustomers
    {
        [DataMember]
        public string NameofCustomer { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public decimal LoanBorrowed { get; set; }

        [DataMember]
        public string EnquiryDate { get; set; }

        [DataMember]
        public string ApplicationDate { get; set; }

        [DataMember]
        public string Constitution { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string SANum { get; set; }
    }
}
