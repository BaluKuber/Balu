using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{

    [Serializable]
    [DataContract]
    public class ClsPubNPA
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
        public decimal OpeningNoOfAccounts { get; set; }

        [DataMember]
        public decimal OpeningStock { get; set; }

        [DataMember]
        public decimal OpeningArrear { get; set; }

        [DataMember]
        public decimal AdditionNoOfAccounts { get; set; }

        [DataMember]
        public decimal AdditionStock { get; set; }

        [DataMember]
        public decimal AdditionArrear { get; set; }

        [DataMember]
        public decimal DeletionNoOfAccounts { get; set; }

        [DataMember]
        public decimal DeletionStock { get; set; }

        [DataMember]
        public decimal DeletionArrear { get; set; }

        [DataMember]
        public decimal ClosingNoOfAccounts { get; set; }

        [DataMember]
        public decimal ClosingStock { get; set; }

        [DataMember]
        public decimal ClosingArrear { get; set; }

        [DataMember]
        public decimal stock { get; set; }

        [DataMember]
        public decimal Arrears { get; set; }

        [DataMember]
        public string Col1 { get; set; }

        [DataMember]
        public string Col2 { get; set; }

        [DataMember]
        public string Col3 { get; set; }
    }
}
