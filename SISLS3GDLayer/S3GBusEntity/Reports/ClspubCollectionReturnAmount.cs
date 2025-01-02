using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClspubCollectionReturnAmount
    {
            
        public List<ClsPubCollectionPerformance> GetCollectionAmount { get; set; }
        public List<ClsPubChequeReturnAmount> GetChequeReturnAmount { get; set; }
        public List<ClsPubCollectionAnalysis> GetCollectionAnalysis { get; set; }

    }
}
