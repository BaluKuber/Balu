using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubIncomeReport
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int User_ID { get; set; }

        [DataMember]
        public int Report_Type { get; set; }

        [DataMember]
        public int LOB_ID { get; set; }

        [DataMember]
        public string LOBName { get; set; }

        [DataMember]
        public int Location_ID1 { get; set; }

        [DataMember]
        public int Location_ID2 { get; set; }

        [DataMember]
        public string LocationName1 { get; set; }

        [DataMember]
        public string LocationName2 { get; set; }

        [DataMember]
        public decimal Month { get; set; }

        [DataMember]
        public decimal Year { get; set; }

        [DataMember]
        public string Method { get; set; }

        [DataMember]
        public string Period { get; set; }

        [DataMember]
        public string FinYearStartDate { get; set; }

        [DataMember]
        public string FinYearEndDate { get; set; }

        [DataMember]
        public string RptGenTime { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string XMLLOB_ID { get; set; }
       
        [DataMember]
        public decimal Denomintion { get; set; }

        [DataMember]
        public string DataRow1 { get; set; }

        [DataMember]
        public decimal DataRow2{ get; set; }


        [DataMember]
        public List<ClsPubIncomeReport> IncomeDetails { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> GPSDetails { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> LOB1 { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> LOB2 { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> LOB3 { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> LOB4 { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> SampleLOB { get; set; }

        [DataMember]
        public List<ClsPubIncomeReport> MainReportLoc { get; set; }
    }
}
