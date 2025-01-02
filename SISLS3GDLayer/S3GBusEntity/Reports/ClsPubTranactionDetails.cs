using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ServiceModel;
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubTranactionDetails
    {
        [DataMember]
        public DateTime DocDate { get; set; }
        [DataMember]
        public DateTime ValueDate { get; set; }
        [DataMember]
        public string DocRef { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Decimal Due { get; set; }
        [DataMember]
        public Decimal Receipts { get; set; }
        [DataMember]
        public Decimal Balance { get; set; }      

    }
}
