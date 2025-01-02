using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
/// <summary>
/// Summary description for ClsPubEnquiryLocation
/// </summary>
/// 
namespace S3GBusEntity.Reports
{
    [Serializable]
    [DataContract]
    public class ClsPubLANLocation
    {
        [DataMember]
        public List<ClsPubLANRegisterDetails> Disbursement { get; set; }

        [DataMember]
        public List<ClsPubLobLocation> loblocation { get; set; }

    }
}
