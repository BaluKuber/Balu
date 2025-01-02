using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubCollateralReport
    {
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string CustomerName { get; set; }        
        [DataMember]
        public string ReferenceNo { get; set; }
        [DataMember]
        public string CollateralType { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string CompanyAddress { get; set; }
        [DataMember]
        public string AssetDesc { get; set; }
        [DataMember]
        public string Units { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Storage1 { get; set; }
        [DataMember]
        public string Storage2 { get; set; }
        [DataMember]
        public string Storage3 { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDate { get; set; }        
        [DataMember]
        public Decimal GPSSUFFIX { get; set; }
    }
}
