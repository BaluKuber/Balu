using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubCreditScoreTransaction
    {
        [DataMember]
        public List<ClsPubCreditScoreDetails> CreditScoreTrans { get; set; }

        [DataMember]
        public List<ClsPubLocationCodeCategory> Creditlocation { get; set; }
    }
}
