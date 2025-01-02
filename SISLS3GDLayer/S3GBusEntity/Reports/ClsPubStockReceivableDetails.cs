using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubStockReceivableDetails
    {
        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public int LOB_ID { get; set; }

        [DataMember]
        public string LOB { get; set; }

        [DataMember]
        public int TeamId { get; set; }

        [DataMember]
        public string Team { get; set; }


        [DataMember]
        public int LOCATION_ID { get; set; }

          [DataMember]
        public string LOCATION_NAME { get; set; }

        
        
        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public int CustomerServiceId { get; set; }

        [DataMember]
        public string CustomerServicePoint { get; set; }

        [DataMember]
        public int CUSTOMER_ID { get; set; }

        [DataMember]
        public string CustomerCodeName { get; set; }

        [DataMember]
        public int GROUP_ID { get; set; }

        [DataMember]
        public string GROUP_NAME { get; set; }

        [DataMember]
        public int INDUSTRY_ID { get; set; }

        [DataMember]
        public string INDUSTRY_NAME { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string SANum { get; set; }

        [DataMember]
        public Decimal GrossStock { get; set; }

        [DataMember]
        public Decimal UMFC { get; set; }

        [DataMember]
        public Decimal BilledUncollectedPrincipal { get; set; }

        [DataMember]
        public Decimal BilledUncollectedFC { get; set; }

        [DataMember]
        public int NoofInstallment { get; set; }

        [DataMember]
        public Decimal NetStock { get; set; }
    }
}