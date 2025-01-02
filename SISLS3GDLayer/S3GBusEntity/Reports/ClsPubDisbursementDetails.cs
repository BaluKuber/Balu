using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubDisbursementDetails
    {
        [DataMember]
        public int ApplicationProcessId { get; set; }

        [DataMember]
        public string LobName { get; set; }

        [DataMember]
        public string Region { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string ApplicationNumber { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string PrimeAccount { get; set; }

        [DataMember]
        public string SubAccount { get; set; }

        [DataMember]
        public decimal ApprovedAmount { get; set; }

        [DataMember]
        public decimal RemainingAmount { get; set; }

        [DataMember]
        public decimal PaidAmount { get; set; }

        [DataMember]
        public decimal AccountYetToBeCreated { get; set; }

        [DataMember]
        public decimal ageing0days { get; set; }

        [DataMember]
        public decimal ageing30days { get; set; }

        [DataMember]
        public decimal ageing60days { get; set; }

        [DataMember]
        public string GPSSuffix { get; set; }

        [DataMember]
        public string Denomination { get; set; }

        [DataMember]
        public List<ClsPubDisbursed> Disbursed { get; set; }

        public ClsPubDisbursementDetails()
        {
            Disbursed = new List<ClsPubDisbursed>();
        }

        
    }
}
