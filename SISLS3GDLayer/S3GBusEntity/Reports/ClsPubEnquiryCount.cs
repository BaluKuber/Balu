using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract] 
    public class ClsPubEnquiryCount
    {
        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string EnqNo { get; set; }

        [DataMember]
        public string EnquiryDate { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string AssetDetails { get; set; }

        [DataMember]
        public string FacilityAmount { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string PrimeAccNo { get; set; }

        [DataMember]
        public string SubAccNo { get; set; }

        [DataMember]
        public string Stage { get; set; }

        [DataMember]
        public string Remarks { get; set; }

    }
}
