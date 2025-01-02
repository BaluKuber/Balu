using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPrimeAccountDetails
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int IsActivated { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string CheckAccess { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public int LobId { get; set; }

       
        [DataMember]
        public int locationId { get; set; }


        [DataMember]
        public string PaNum { get; set; }

        [DataMember]
        public string SaNum { get; set; }

        [DataMember]
        public int ProgramId { get; set; }
    }
}
