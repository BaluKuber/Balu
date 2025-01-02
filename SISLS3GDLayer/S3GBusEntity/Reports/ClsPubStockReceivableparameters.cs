using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubStockReceivableparameters
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string LobId { get; set; }

        //[DataMember]
        //public string BranchId { get; set; }

        [DataMember]
        public string LocationId1 { get; set; }

        [DataMember]
        public string LocationId2 { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public string FromMonth { get; set; }

        [DataMember]
        public string ToMonth { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        [DataMember]
        public int GROUP_ID { get; set; }

        [DataMember]
        public string GROUP_NAME { get; set; }

        [DataMember]
        public int INDUSTRY_ID { get; set; }

        [DataMember]
        public string INDUSTRY_NAME { get; set; }

        [DataMember]
        public int REFERENCE_ID { get; set; }

        [DataMember]
        public int CUST_ID { get; set; }


        public ClsPubStockReceivableparameters()
        {
            Denomination = 1;
        }
    }
}
