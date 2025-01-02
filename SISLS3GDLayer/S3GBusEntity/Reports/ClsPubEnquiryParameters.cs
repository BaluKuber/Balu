using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]   

    public class ClsPubEnquiryParameters
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int USER_ID { get; set; }

        [DataMember]
        public int PROGRAM_ID { get; set; }

        [DataMember]
        public string LobId { get; set; }

        [DataMember]
        public string RegionId { get; set; }

        [DataMember]
        public string BranchId { get; set; }

        [DataMember]
        public string ProductId { get; set; }

       

        [DataMember]
        public string LOCATION_ID1 { get; set; }

        [DataMember]
        public string LOCATION_ID2 { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }
        [DataMember]
        public int Option { get; set; }

    }
}
