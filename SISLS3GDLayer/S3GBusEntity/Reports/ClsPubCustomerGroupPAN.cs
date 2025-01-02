using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCustomerGroupPAN
    {
        public List<ClsPubDropDownList> Customerdetails { get; set; }
        public List<ClsPubDropDownList> LOBdetails { get; set; }
        public List<ClsPubDropDownList> CustomerGroupdetails { get; set; }
        public List<ClsPubDropDownList> Panumdetails { get; set; }
        //public List<ClsPubDCHeaderParams> PANUM { get; set; }
        [DataMember]
        public string Lob_ID { get; set; }
        [DataMember]
        public string Group_ID { get; set; }
        [DataMember]
        public string Customer_ID { get; set; }
    }

        //[DataMember]
        //public string CustomerId { get; set; }

        //[DataMember]
        //public string CustomerName { get; set; }

        //[DataMember]
        //public string GroupId { get; set; }

        //[DataMember]
        //public string GroupName { get; set; }

        //[DataMember]
        //public string PANum { get; set; }
}
