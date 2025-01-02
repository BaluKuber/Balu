using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubAssetPerfParam
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int IRR_Type { get; set; }

        [DataMember]
        public int Report_Type { get; set; }

        [DataMember]
        public int LOB_ID { get; set; }

        [DataMember]
        public int User_ID { get; set; }


        [DataMember]
        public string FinMonthYear { get; set; }

        [DataMember]
        public string CurrentDate { get; set; }

        [DataMember]
        public string CurrentMonthStartDate { get; set; }

        [DataMember]
        public string CurrentMonthEndDate { get; set; }

        [DataMember]
        public string FinYearStartDate { get; set; }

        [DataMember]
        public string FinYearStartEnd { get; set; }

        [DataMember]
        public string RptGenTime { get; set; }
        [DataMember]
        public decimal Denomintion { get; set; }

    }
}
