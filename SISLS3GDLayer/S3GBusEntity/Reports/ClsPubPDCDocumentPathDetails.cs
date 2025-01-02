using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubPDCDocumentPathDetails
    {
        [DataMember]
        public string DocumentPathID { get; set; }
        [DataMember]
        public string DocumentPath { get; set; }
    }
}
