using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsNPAaccount
    {

        [DataMember]
        public int RegionId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public int ClassId { get; set; }

        [DataMember]
        public string AssetClass { get; set; }
        [DataMember]
        public string PrimeAccountNumber { get; set; }

        [DataMember]
        public string SubAccountNumber { get; set; }

        [DataMember]
        public decimal Stock { get; set; }

        [DataMember]
        public decimal Arrear { get; set; }

        [DataMember]
        public string Col1 { get; set; }

        [DataMember]
        public string Col2 { get; set; }

        [DataMember]
        public string Col3 { get; set; }


    }
}
