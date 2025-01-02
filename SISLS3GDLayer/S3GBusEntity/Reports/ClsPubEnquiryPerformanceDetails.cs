using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract] 

    public class ClsPubEnquiryPerformanceDetails
    {
        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string BranchId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Location { get; set; }
        
        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public int ReceivedCount { get; set; }

        [DataMember]
        public decimal ReceivedValue { get; set; }

        [DataMember]
        public int SuccessfulCount { get; set; }

        [DataMember]
        public decimal SuccessfulValue { get; set; }

        [DataMember]
        public int UnderFollowupCount { get; set; }

        [DataMember]
        public decimal UnderFollowupValue { get; set; }

        [DataMember]
        public int RejectedCount { get; set; }

        [DataMember]
        public decimal RejectedValue { get; set; }

        [DataMember]
        public string Denomination { get; set; }



        [DataMember]
        public string GPSSuffix { get; set; }


        }
    }
