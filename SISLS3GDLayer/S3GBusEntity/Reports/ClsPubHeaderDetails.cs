using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubHeaderDetails
    {

        [DataMember]
        public string Customer { get; set; }

        [DataMember]
        public string Lob { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string SANum { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public bool IsAbstract { get; set; }

        [DataMember]
        public bool IsDetails { get; set; }

        [DataMember]
        public string PDCNo { get; set; }

        [DataMember]
        public string PDCDate { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CompanyAddress { get; set; }

        [DataMember]
        public string CCPU { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public string NoOf { get; set; }

        [DataMember]
        public string FromYearMonth { get; set; }

        [DataMember]
        public string ToYearMonth { get; set; }

        [DataMember]
        public string FromYearMonthCompare { get; set; }

        [DataMember]
        public string ToYearMonthCompare { get; set; }

        [DataMember]
        public string CustomerGroup { get; set; }

        [DataMember]
        public string Cutoffmonth { get; set; }

        [DataMember]
        public string category { get; set; }

        [DataMember]
        public string Ageing { get; set; }

        [DataMember]
        public string DebtCollectorCode { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public string Lan { get; set; }

        [DataMember]
        public string GlAccount { get; set; }

    }
}