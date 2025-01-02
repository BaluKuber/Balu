using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCustomersDetails
    {
        [DataMember]
        public string BRANCH_ID { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string CustomerNameAddress { get; set; }

        //[DataMember]
        //public string Address { get; set; }

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

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public decimal LoanRequested { get; set; }
    }
}
