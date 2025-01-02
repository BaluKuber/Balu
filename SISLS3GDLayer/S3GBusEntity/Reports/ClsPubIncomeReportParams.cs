using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubIncomeReportParams
    {
        [DataMember]
        public int CompanyId { get; set; }
        
        [DataMember]
        public int Program_ID { get; set; }
        
        [DataMember]
        public int User_ID { get; set; }

        [DataMember]
        public int Report_Type { get; set; }

        [DataMember]
        public int LOB_ID { get; set; }

        [DataMember]
        public int Location_ID1 { get; set; }
        
        [DataMember]
        public int Location_ID2 { get; set; }

        [DataMember]
        public string CutoffMonth{ get; set; }

        [DataMember]
        public string CutoffMonth_Date{ get; set; }

        [DataMember]
        public string FinYearstart { get; set; }

        [DataMember]
        public string CurrentFinYear { get; set; }

        [DataMember]
        public string PreviousFinYear { get; set; }

        [DataMember]
        public decimal Denomintion { get; set; }
        
        [DataMember]
        public string Method { get; set; }

        [DataMember]
        public string Period { get; set; }

        [DataMember]
        public string XMLLOB_ID { get; set; }

        [DataMember]
        public int LOB_ID1 { get; set; }

        [DataMember]
        public int LOB_ID2 { get; set; }

        [DataMember]
        public int LOB_ID3 { get; set; }

        [DataMember]
        public int LOB_ID4 { get; set; }

       
    }
}
