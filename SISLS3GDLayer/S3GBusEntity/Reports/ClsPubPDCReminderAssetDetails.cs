using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public sealed class ClsPubPDCReminderAssetDetails
    {
        [DataMember]
        public string PRIMEACCOUNTNO { get; set; }
        [DataMember]
        public string SUBACCOUNTNO { get; set; }
        [DataMember]
        public string AssetMake { get; set; }
        [DataMember]
        public string AssetType { get; set; }
        [DataMember]
        public string RegistrationNumber { get; set; }
    }
}
