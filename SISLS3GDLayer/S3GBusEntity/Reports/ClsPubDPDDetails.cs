using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDPDDetails
    {
        [DataMember]
        public string Bucketdetails { get; set; }
        [DataMember]
        public int Bucket { get; set; }
        [DataMember]
        public int DaysFrom { get; set; }
        [DataMember]
        public int DaysTo { get; set; }
        [DataMember]
        public string LineOfBusiness { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public DateTime CutOffDate { get; set; }
        [DataMember]
        public string LOBID { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public string Denominations { get; set; }
        [DataMember]
        public string ReceiptUpto { get; set; }

        

    }
}
