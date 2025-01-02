using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubLANRegisterinput
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int LobId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int ProgramId { get; set; }

        [DataMember]
        public int LocationId1 { get; set; }

        [DataMember]
        public int LocationId2 { get; set; }

        [DataMember]
        public string Customer_ID { get; set; }

        [DataMember]
        public string Panum { get; set; }

        [DataMember]
        public string Sanum { get; set; }

        [DataMember]
        public string LanFrom { get; set; }

        [DataMember]
        public string LanTo { get; set; }

        [DataMember]
        public DateTime FromDate { get; set; }

        [DataMember]
        public DateTime ToDate { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        [DataMember]
        public int Option { get; set; }
 
        }
}
