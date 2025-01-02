using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubLANdetails
    {
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Lan { get; set; }

        [DataMember]
        public string AssetDescription { get; set; }

        [DataMember]
        public string RegnNumber { get; set; }

        [DataMember]
        public string ChasisNumber { get; set; }

        [DataMember]
        public string EngineNumber   { get; set; }

        [DataMember]
        public string SerialNumber { get; set; }

        [DataMember]
        public string PerformingStatus { get; set; }

        [DataMember]
        public string AvailabilityStatus { get; set; }

        [DataMember]
        public string LanBookingUpto { get; set; }

       

    }
}
