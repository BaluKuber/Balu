using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubProbableDelinq
    {
        [DataMember]
        public string Location_Name { get; set; }

        [DataMember]
        public string Customer_Name { get; set; }

        [DataMember]
        public string PANum { get; set; }

        [DataMember]
        public string SANum { get; set; }

        [DataMember]
        public decimal Arrear_Due { get; set; }

        [DataMember]
        public decimal Current_Due { get; set; }
    }
}
