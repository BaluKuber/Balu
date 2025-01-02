using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
   public class ClsPubAssetPerformance
    {
        [DataMember]
        public string Asset_Code { get; set; }

        [DataMember]
        public string AssetClasification { get; set; }

        [DataMember]
        public string NewOrOld { get; set; }

        [DataMember]
        public string AsssetType { get; set; }

        [DataMember]
        public string AssetMake { get; set; }

        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public decimal BCD { get; set; }
        [DataMember]
        public decimal BCDAmt { get; set; }
        [DataMember]
        public decimal BDD { get; set; }
        [DataMember]
        public decimal BDDAmt { get; set; }

        [DataMember]
        public decimal BCM { get; set; }
        [DataMember]
        public decimal BCMAmt { get; set; }
        [DataMember]
        public decimal BDM { get; set; }
        [DataMember]
        public decimal BDMAmt { get; set; }

        [DataMember]
        public decimal BCY { get; set; }
        [DataMember]
        public decimal BCYAmt { get; set; }
        [DataMember]
        public decimal BDY { get; set; }
        [DataMember]
        public decimal BDYAmt { get; set; }


        [DataMember]
        public string ReportType { get; set; }

        [DataMember]
        public string IRRType { get; set; }

        [DataMember]
        public string LOB { get; set; }

        [DataMember]
        public string AmountFinanced { get; set; }

        [DataMember]
        public string DataRow1 { get; set; }

        [DataMember]
        public string DataRow2 { get; set; }

    }
}
