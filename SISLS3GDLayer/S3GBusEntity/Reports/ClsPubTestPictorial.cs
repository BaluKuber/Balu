using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    class ClsPubTestPictorial
    {

        [DataMember]
        public decimal Col1 { get; set; }

        [DataMember]
        public decimal Col2 { get; set; }

        [DataMember]
        public decimal Col3 { get; set; }

        [DataMember]
        public decimal Col4 { get; set; }
             
    }
}
