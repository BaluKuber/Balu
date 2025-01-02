using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubBranchInputSelectionCriteria
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public string LobId { get; set; }

        [DataMember]
        public string LocationID1 { get; set; }

        [DataMember]
        public string LocationID2 { get; set; }

        [DataMember]
        public string LocationCode { get; set; }

        [DataMember]
        public int ProgramId { get; set; }

        [DataMember]
        public int ClassId { get; set; }

        
        [DataMember]
        public string CutOffYear { get; set; }

        [DataMember]
        public string CutOffMonth { get; set; }

        [DataMember]
        public string CUTOFFPREVIOUSMONTH { get; set; }

        [DataMember]
        public string Financial_Year_From { get; set; }

        [DataMember]
        public DateTime FYSTARTDATE { get; set; }

        [DataMember]
        public DateTime CMSTARTDATE { get; set; }

        [DataMember]
        public DateTime CMENDDATE { get; set; }

        [DataMember]
        public string FinancialMonth { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public decimal Denomination { get; set; }

        public ClsPubBranchInputSelectionCriteria()
        {
            Denomination = 1;
        }
    }
}
