using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCredit
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int LOBId { get; set; }

        [DataMember]
        public int LocationId1 { get; set; }
        //public int BranchId { get; set; }

        [DataMember]
        public int LocationId2 { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public string Start_Date { get; set; }

        [DataMember]
        public string End_Date { get; set; }

        [DataMember]
        public int Status { get; set; }
    }
}
