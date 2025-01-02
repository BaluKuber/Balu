using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubRptPDCReminderHeaderDetails
    {
        [DataMember]
        public string LineOfBusiness {get;set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public string LOBId { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string LocationId1 { get; set; }
        [DataMember]
        public string LocationId2 { get; set; }
        [DataMember]
        public string ProgramId { get; set; }
        [DataMember]
        public string FilePath { get; set; }
    }
}
