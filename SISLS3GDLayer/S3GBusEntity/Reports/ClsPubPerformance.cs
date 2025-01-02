using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubPerformance
    {
       [DataMember]
         public int LOB_ID { get; set; }

        [DataMember]
        public int Location_ID1 { get; set; }

        [DataMember]
        public int Location_ID2 { get; set; }

        [DataMember]
        public int Program_ID { get; set; }

        [DataMember]
        public int Company_ID { get; set; }

        [DataMember]
        public int User_ID { get; set; }

        [DataMember]
        public int Asset_Type { get; set; }

        [DataMember]
        public int Product_ID { get; set; }

        [DataMember]
        public string From_Date { get; set; }

        [DataMember]
        public string To_Date { get; set; }

        [DataMember]
        public string From_ComDate { get; set; }

        [DataMember]
        public string To_ComDate { get; set; }

        [DataMember]
        public int Mode { get; set; }

      
    }
}
