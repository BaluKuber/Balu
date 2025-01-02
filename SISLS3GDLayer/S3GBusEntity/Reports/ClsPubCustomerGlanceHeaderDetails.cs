using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Common;
using System.Data.SqlClient;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubCustomerGlanceHeaderDetails
    {
        [DataMember]
        public string LOBId { get; set; }
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public string RegionId { get; set; }
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public int ProgramId { get; set; }
        [DataMember]
        public string ProductId { get; set; }
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
