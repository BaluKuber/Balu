using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubCollateralHeader
    {
        [DataMember]
        public string LOBId { get; set; }
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string LocationId1 { get; set; }
        [DataMember]
        public string LocationId2 { get; set; }        
        [DataMember]
        public int ProgramId { get; set; }
        [DataMember]
        public string CollateralTypeId { get; set; }
        [DataMember]
        public string StatusId { get; set; }
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string CompanyId { get; set; }  
    }
}
